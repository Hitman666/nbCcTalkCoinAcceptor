using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using dk.CctalkLib.Connections;
using dk.CctalkLib.Devices;

namespace nbCcTalkCoinAcceptor
{
    public partial class nbCoinAcceptor : Form
    {
        private CoinAcceptor _coinAcceptor;
        public event Action<decimal> CoinAccepted;

        private decimal _coinValue;
        private string newline = Environment.NewLine + Environment.NewLine;
        public nbCoinAcceptor()
        {
            InitializeComponent();
        }

        public void btnConnectToCA_Click(object sender, EventArgs e)
        {
            if (ConnectToCoinAcceptor())
            {
                txtLog.Text += "Successfully connected to coin acceptor!" + newline;

                if (ModifyCoinAcceptorInhibitStatus())
                {
                    txtLog.Text += "Successfully modified the inhibit status!" + newline;
                }
                else
                {
                    txtLog.Text += "Error while modifying the inhibit status!" + newline;
                }
            }
            else
            {
                txtLog.Text += "Error while connecting to coin acceptor!" + newline;
            }

            _coinAcceptor.AllowedCoins = CoinIndex.One | CoinIndex.Two | CoinIndex.Three;
        }

        private bool ConnectToCoinAcceptor()
        {
            Dictionary<byte, CoinTypeInfo> coins;
            coins = CoinAcceptor.DefaultConfig;

            byte deviceNumber = 2; //device number defaults to 2 - coin acceptor

            //can be changed to ones liking by using SetCoins(coinsDefaultText, out coins)
            //default from .dll is:
            //1=0,05=5 cent; 2=0,1=10 cent; 3=0,2=20 cent; 4=0,5=50 cent; 5=1=1 euro; 6=2=2 euro;
            string coinsDefaultText = CoinAcceptor.ConfigWord(CoinAcceptor.DefaultConfig);

            txtLog.Text += "Using the following connection string for coins:" + Environment.NewLine + coinsDefaultText + newline;

            try
            {
                string port = "COM" + txtPortNumber.Text;
                var connection = new ConnectionRs232
                                     {
                                         PortName = port,
                                         RemoveEcho = true
                                     };

                txtLog.Text += "Trying to connect to port:" + port + " - ";

                _coinAcceptor = new CoinAcceptor(deviceNumber, connection, coins, null);

                _coinAcceptor.CoinAccepted += _coinAcceptor_CoinAccepted;
                _coinAcceptor.ErrorMessageAccepted += _coinAcceptor_ErrorMessageAccepted;

                _coinAcceptor.Init(true);

              

                if (_coinAcceptor.IsInitialized)
                {
                    txtLog.Text += "Successfully initialized the CoinAcceptor!" + newline;
                    return true;
                }

                txtLog.Text += "Failed initializing the CoinAcceptor!" + newline;
                DisposeCoinAcceptor();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return false;
        }

        private bool ModifyCoinAcceptorInhibitStatus()
        {
            try
            {
                //if called without parameters they default to 255,0, meaning first 8 coins are set to accept receiving 
                _coinAcceptor.ModifyInhibitStatus();
                return true;
            }
            catch (Exception ex)
            {
                //txtLog.Text += ex.ToString();
                return false;
            }
        }

        private void DisposeCoinAcceptor()
        {
            if (_coinAcceptor == null)
                return;

            if (_coinAcceptor.IsInitialized)
            {
                _coinAcceptor.IsInhibiting = true;
                _coinAcceptor.UnInit();
            }

            _coinAcceptor.Dispose();

            _coinAcceptor = null;
        }

        void _coinAcceptor_ErrorMessageAccepted(object sender, CoinAcceptorErrorEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((EventHandler<CoinAcceptorErrorEventArgs>)_coinAcceptor_ErrorMessageAccepted, sender, e);
                return;
            }

            txtLog.Text += String.Format("Coin acceptor error: {0} ({1}, {2:X2})", e.ErrorMessage, e.Error, (Byte)e.Error) + Environment.NewLine;
        }

        void _coinAcceptor_CoinAccepted(object sender, CoinAcceptorCoinEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((EventHandler<CoinAcceptorCoinEventArgs>)_coinAcceptor_CoinAccepted, sender, e);
                return;
            }
            _coinValue += e.CoinValue;
            txtLog.Text += String.Format("Coin accepted: {0} ({1:X2}), path {3}. Now accepted: {2}", e.CoinName, e.CoinCode, _coinValue, e.RoutePath) + Environment.NewLine;
            labTotalMoneyIn.Text = _coinValue.ToString();

            if (CoinAccepted != null)
                CoinAccepted(e.CoinValue);

            // There is simulator of long-working event handler
            Thread.Sleep(1000);
        }

        private void SetCoins(string coinsDefaultText, out Dictionary<byte, CoinTypeInfo> coins)
        {
            if (!CoinAcceptor.TryParseConfigWord(coinsDefaultText, out coins))
            {
                coins = CoinAcceptor.DefaultConfig;
                txtLog.Text += "Wrong config word, using defaults: " + CoinAcceptor.ConfigWord(CoinAcceptor.DefaultConfig) + newline;
            }
        }

        #region polling
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (_coinAcceptor == null) return;

            if (!_coinAcceptor.IsInitialized)
                _coinAcceptor.Init(true);

            if (cbPolling.Checked)
                _coinAcceptor.StartPoll();
            else
                _coinAcceptor.EndPoll();
        }
        public void StartPoll()
        {
            _coinAcceptor.StartPoll();
        }
        public void EndPoll()
        {
            _coinAcceptor.EndPoll();
        }
        #endregion

        #region inhibition
        private void cbInhibit_CheckedChanged(object sender, EventArgs e)
        {
            if (_coinAcceptor != null)
                _coinAcceptor.IsInhibiting = cbInhibit.Checked;
        }
        public void startInhibit()
        {
            _coinAcceptor.IsInhibiting = true;
        }
        public void stopInhibit()
        {
            _coinAcceptor.IsInhibiting = false;
        }
        #endregion
    }
}
