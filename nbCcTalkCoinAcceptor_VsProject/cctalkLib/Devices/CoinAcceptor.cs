using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Timers;
using dk.CctalkLib.Connections;

namespace dk.CctalkLib.Devices
{

    /// <summary>
    /// Encapsulates various routines linked with coin acceptors. 
    /// Also invokes events to Wpf or Windows.Forms application/
    /// </summary>
    public class CoinAcceptor : IDisposable
    {
        const int PollPeriod = 500;

        readonly GenericCctalkDevice _rawDev = new GenericCctalkDevice();

        Timer _t;
        byte _lastEvent;

        /// <summary>
        /// Fires when coin was accepted. Only when polling is on.
        /// </summary>
        public event EventHandler<CoinAcceptorCoinEventArgs> CoinAccepted;

        /// <summary>
        /// Fires when any error was detected during poll.
        /// </summary>
        public event EventHandler<CoinAcceptorErrorEventArgs> ErrorMessageAccepted;

        TimeSpan _pollInterval;

        readonly object _timersSyncRoot = new object(); // for sync with timer threads only
        readonly Dictionary<byte, string> _errors = new Dictionary<byte, string>();
        readonly Dictionary<byte, CoinTypeInfo> _coins = new Dictionary<byte, CoinTypeInfo>();

        #region Constructors

        /// <summary>
        /// Creates instance of CoinAcceptor without invoking
        /// </summary>
        /// <param name="addr">Device ccTalk address. 0 - broadcast</param>
        /// <param name="configuredConnection">Connection to work. Sharing connection between seweral device object not recommended and not supported. But can work :)</param>
        /// <param name="coins">data for resolving coin codes to coin values and names. Depends on device firmware. Can be found on device case.</param>
        /// <param name="errorNames">overriding for error nemes</param>
        public CoinAcceptor(
            byte addr,
            ICctalkConnection configuredConnection,
            Dictionary<byte, CoinTypeInfo> coins,
            Dictionary<byte, string> errorNames
            )
        {
            _rawDev.Connection = configuredConnection ?? throw new ArgumentNullException("configuredConnection");
            _rawDev.Address = addr;

            if (errorNames != null)
                foreach (var error in errorNames)
                    _errors[error.Key] = error.Value;

            if (coins != null)
                foreach (var coin in coins)
                    _coins[coin.Key] = coin.Value;

            _allowedCoins = CoinIndex.All;
        }

        /// <summary>
        /// Creates instance of CoinAcceptor with minimal custumization
        /// </summary>
        /// <param name="addr">Device ccTalk address. 0 - broadcast</param>
        /// <param name="configuredConnection">Connection to work. Sharing connection between seweral device object not recommended and not supported. But can work :)</param>
        public CoinAcceptor(
            byte addr,
            ICctalkConnection configuredConnection
            )
            : this(addr, configuredConnection, null, null)
        {
        }

        #endregion


        public void ModifyAcceptedCoins(Dictionary<byte, CoinTypeInfo> coins)
        {
            if (coins != null)
                foreach (var coin in coins)
                    _coins[coin.Key] = coin.Value;
        }

        /// <summary>
        /// Connection used for communication with cctalk device
        /// </summary>
        public ICctalkConnection Connection => _rawDev.Connection;

        /// <summary>
        ///  Opens connection and request main data from device (e.g. Serial number)
        ///  If there is some data in event buffer - events will be raised
        /// </summary>
        public void Init(bool ignoreLastEvents = true)
        {
            _rawDev.Connection.Open();

            DeviceCategory = _rawDev.CmdRequestEquipmentCategory();
            if (DeviceCategory != CctalkDeviceTypes.CoinAcceptor)
                throw new InvalidOperationException("Connected device is not a coin acceptor. " + DeviceCategory);

            //_rawDev.CmdReset();			
            _rawDev.CmdSetMasterInhibitStatus(IsInhibiting);

            //throw new InvalidOperationException("Msg: " + _rawDev.CmdGetMasterInhibitStatus().ToString()); - vraca false znaci da neInhibira.

            SerialNumber = _rawDev.CmdGetSerial();
            PollInterval = _rawDev.CmdRequestPollingPriority();
            Manufacturer = _rawDev.CmdRequestManufacturerId();
            ProductCode = _rawDev.CmdRequestProductCode();

            var evBuf = _rawDev.CmdReadEventBuffer();

            if (!ignoreLastEvents)
            {
                RaiseLastEvents(evBuf);
            }
            _lastEvent = evBuf.Counter;

            IsInitialized = true;
        }

        public void ModifyInhibitStatus(int Data1 = 255, int Data2 = 0)
        {
            _rawDev.CmdModifyInhibitStatus(Data1, Data2);
        }

        /// <summary>
        ///  Closes port
        /// </summary>
        public void UnInit()
        {
            lock (_timersSyncRoot)
            {
                EndPoll();
                _rawDev.Connection.Close();
                IsInitialized = false;
            }

        }

        /// <summary>
        ///  true - port is open, ready for sending commands
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        ///  ccTalk address of device. 0 - broadcast.
        /// </summary>
        public byte Address => _rawDev.Address;

        /// <summary>
        ///  Is polling is running now. Commands (as GetStatus) CAN be sent while polling.
        /// </summary>
        public bool IsPolling => _t != null;

        protected string ProductCode { get; private set; }

        /// <summary>
        /// Serial number of device. Value accepted from device while Init.
        /// </summary>
        public int SerialNumber { get; private set; }

        /// <summary>
        ///  Manufacturer name of device. Value accepted from device while Init.
        /// </summary>
        public string Manufacturer { get; private set; }

        /// <summary>
        ///  Type of device. Value accepted from device while Init.
        /// </summary>
        public CctalkDeviceTypes DeviceCategory { get; private set; }

        bool _isInhibiting;

        /// <summary>
        ///  Indicates the state, when device is rejecting all coins.
        /// </summary>
        public bool IsInhibiting
        {
            get => _isInhibiting;
            set
            {
                _rawDev.CmdSetMasterInhibitStatus(value);
                _isInhibiting = value;
            }

        }

        CoinIndex _allowedCoins;
        /// <summary>
        /// Gets or sets the allowed indexes that this CoinAcceptor is allowed to accept. Initially set to CoinIndex.All
        /// </summary>
        public CoinIndex AllowedCoins
        {
            get => _allowedCoins;
            set
            {
                if (_allowedCoins == value) return;
                _allowedCoins = value;
                _rawDev.CmdModifyInhibitStatus((byte)_allowedCoins, (byte)((int)_allowedCoins >> 8));
            }
        }

        /// <summary>
        /// *CURRENTLY UNUSED: interval hardcoded*
        /// Poll interval. By default value accepted from device while Init procedure. Can be changed, but next Init will reset value.
        /// If value = TimeSpan.Zero - device not recommends any intervals or use different hardware lines for polling.
        /// </summary>
        public TimeSpan PollInterval
        {
            get => _pollInterval;
            set
            {
                if (IsPolling)
                    throw new InvalidOperationException("Stop polling first");
                _pollInterval = value;
            }
        }

        /// <summary>
        ///  Starts poll events from device
        /// </summary>
        public void StartPoll()
        {
            if (_t != null)
                throw new InvalidOperationException("Stop polling first");

            lock (_timersSyncRoot)
            {
                if (!_rawDev.Connection.IsOpen())
                    throw new InvalidOperationException("Init first");
                _t = new Timer(PollPeriod)
                {
                    AutoReset = false
                };
                _t.Elapsed += TimerTick;
                _t.Start();
            }
        }

        /// <summary>
        ///  Stops poll events from device
        /// </summary>
        public void EndPoll()
        {
            if (_t == null) return;
            lock (_timersSyncRoot)
            {
                _t.Elapsed -= TimerTick;
                _t.Stop();
                _t.Dispose();
                _t = null;
            }
        }

        /// <summary>
        ///  Polls the device immediately. Returns true if device is ready. 
        ///  There is no need in calling this method when devise is polled.
        /// </summary>
        public CctalkDeviceStatus GetStatus()
        {
            if (!IsInitialized) return CctalkDeviceStatus.OtherError;
            var status = _rawDev.CmdRequestStatus();
            return status;
        }

        /// <summary>
        /// Remembers current state of device`s event buffer as empty.
        /// All unread events in buffer will be discarded.
        /// </summary>
        public void ClearEventBuffer()
        {
            lock (_timersSyncRoot)
            {
                _isClearEventBufferRequested = true;
            }
        }

        /// <summary>
        /// Immediately executes poll process in blocking mode.
        /// All usual polling events will fire at calling thread.
        /// </summary>
        public void PollNow()
        {
            TimerTick(this, EventArgs.Empty);
        }

        bool _isResetExpected;
        bool _isClearEventBufferRequested;

        void TimerTick(object sender, EventArgs e)
        {
            lock (_timersSyncRoot)
            {
                //if(_t == null) return;
                try
                {
                    var buf = _rawDev.CmdReadEventBuffer();

                    var wasReset = buf.Counter == 0;
                    if (wasReset)
                    {
                        if (!_isResetExpected && _lastEvent != 0)
                        {
                            RaiseInvokeErrorEvent(
                                new CoinAcceptorErrorEventArgs(
                                    CoinAcceptorErrors.UnspecifiedAlarmCode,
                                    "Unexpected reset"
                                    )
                                );
                        }
                    }

                    if (_isClearEventBufferRequested)
                    {
                        _lastEvent = buf.Counter;
                    }

                    _isResetExpected = false;

                    RaiseLastEvents(buf);
                }
                finally
                {
                    if (_t != null && ReferenceEquals(sender, _t))
                        _t.Start();
                }
            }
        }

        private void RaiseLastEvents(DeviceEventBuffer buf)
        {
            var newEventsCount = GetNewEventsCountHelper(_lastEvent, buf.Counter);
            _lastEvent = buf.Counter;
            RaiseEventsByBufferHelper(buf, newEventsCount);
        }

        static byte GetNewEventsCountHelper(byte lastCounerVal, byte newCounterVal)
        {
            if (newCounterVal == 0) return 0;

            var newEventsCount = lastCounerVal <= newCounterVal
                        ? newCounterVal - lastCounerVal
                        : (255 - lastCounerVal) + newCounterVal;

            return Convert.ToByte(newEventsCount);
        }

        void RaiseEventsByBufferHelper(DeviceEventBuffer buf, byte countToShow)
        {
            if (countToShow == 0) return;

            for (var i = 0; i < Math.Min(countToShow, buf.Events.Length); i++)
            {
                var ev = buf.Events[i];
                if (ev.IsError)
                {
                    string errMsg;
                    var errCode = (CoinAcceptorErrors)ev.ErrorOrRouteCode;
                    _errors.TryGetValue(ev.ErrorOrRouteCode, out errMsg);
                    RaiseInvokeErrorEvent(new CoinAcceptorErrorEventArgs(errCode, errMsg));

                }
                else
                {
                    CoinTypeInfo coinInfo;
                    _coins.TryGetValue(ev.CoinCode, out coinInfo);
                    var evVal = coinInfo?.Value ?? 0;
                    var evName = coinInfo?.Name;
                    RaiseInvokeCoinEvent(new CoinAcceptorCoinEventArgs(evName, evVal, ev.CoinCode, ev.ErrorOrRouteCode));
                }
            }

            var eventsLost = countToShow - buf.Events.Length;

            if (eventsLost > 0)
            {
                RaiseInvokeErrorEvent(new CoinAcceptorErrorEventArgs(CoinAcceptorErrors.UnspecifiedAlarmCode,
                                                                     "Events lost:" + eventsLost));
            }


        }

        void RaiseInvokeErrorEvent(CoinAcceptorErrorEventArgs ea)
        {
            if (CoinAccepted != null)
                ErrorMessageAccepted(this, ea);
        }

        void RaiseInvokeCoinEvent(CoinAcceptorCoinEventArgs ea)
        {
            if (CoinAccepted != null)
                CoinAccepted(this, ea);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        void Dispose(bool disposing)
        {
            UnInit();
        }

        /// <summary>
        /// builds correct config word from coins
        /// config word structure:
        /// {coin byre code}={coin value}={coin name};
        ///                 ^ splitter   ^ splitter  ^entry splitter
        /// </summary>
        /// <param name="coins">configuration to build the config word</param>
        /// <returns>config word itself</returns>
        public static string ConfigWord(Dictionary<byte, CoinTypeInfo> coins)
        {
            var sb = new StringBuilder();
            foreach (var coin in coins)
            {
                sb.AppendFormat("{0}={1}={2};", coin.Key, coin.Value.Value, coin.Value.Name);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Tries to parse config word and return coins info for the constructor
        /// {coin byre code}={coin value}={coin name};
        ///                 ^ splitter   ^ splitter  ^entry splitter
        /// </summary>
        /// <param name="word">config word</param>
        /// <param name="coins">out dictionary for parsed word, null if parsing fails</param>
        /// <returns>true for success, otherwise - false</returns>
        public static bool TryParseConfigWord(string word, out Dictionary<byte, CoinTypeInfo> coins)
        {
            try
            {
                var coinWords = word.Split(';');
                coins = new Dictionary<byte, CoinTypeInfo>(coinWords.Length);
                foreach (var coinWord in coinWords)
                {
                    if (string.IsNullOrEmpty(coinWord))
                        continue;
                    var values = coinWord.Split('=');
                    var code = byte.Parse(values[0]);
                    var value = decimal.Parse(values[1], NumberStyles.Currency);
                    var name = values.Length >= 3 ? values[2] : values[1];
                    coins[code] = new CoinTypeInfo(name, value);
                }
            }
            catch (Exception)
            {
                coins = null;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 1=2=2EUR; 2=1=1EUR; 3=0,5=50 cent; 4=0,2=20 cent; 5=0.1=10 cent; 6=0.05=5cent;		
        /// </summary>
        public static Dictionary<byte, CoinTypeInfo> DefaultConfig = new Dictionary<byte, CoinTypeInfo>
		{		
			{1, new CoinTypeInfo("2 euro",	2M)},						
			{2, new CoinTypeInfo("1 euro",	1M)},
			{3, new CoinTypeInfo("50 cent", 0.5M)},
			{4, new CoinTypeInfo("20 cent", 0.20M)},
			{5, new CoinTypeInfo("10 cent", 0.10M)},
			{6, new CoinTypeInfo("5 cent", 0.05M)}
		};
    }
}