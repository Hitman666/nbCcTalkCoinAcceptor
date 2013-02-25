using System;
using System.Diagnostics;
using System.Threading;
using System.IO.Ports;
using dk.CctalkLib.Checksumms;
using dk.CctalkLib.Devices;
using dk.CctalkLib.Messages;

namespace dk.CctalkLib.Connections
{

	/// <summary>
	///  Incapsulates routines for Com-port exchange with device.
	///  Provides syncronization for send requests and waits for respond from device.
	/// </summary>
	public class ConnectionRs232 : ICctalkConnection
	{
		const Int32 RespondStartTimeout = 2000;
		const Int32 RespondDataTimeout = 50;   //time to wait next byte within message packet recive operation. Correspinds to 11.1 paragraph of ccTalk Generic Specification
		// const Int32 RespondDataTimeout = 1500;


		readonly Object _callSyncRoot = new Object();
		readonly Object _phaseSyncRoot = new Object();

		readonly SerialPort _port = new SerialPort();
		readonly Byte[] _respondBuf = new byte[255];
		readonly AutoResetEvent _readWait = new AutoResetEvent(false);
		private readonly Stopwatch _timer = new Stopwatch();
		private bool _removeEcho = false;



		/// <summary>
		/// Serial baut rate
		/// </summary>
		public int BaudRate
		{
			get { return _port.BaudRate; }
			set { _port.BaudRate = value; }
		}

		public int DataBits
		{
			get { return _port.DataBits; }
			set { _port.DataBits = value; }
		}


		public Parity Parity
		{
			get { return _port.Parity; }
			set { _port.Parity = value; }
		}

		public StopBits StopBits
		{
			get { return _port.StopBits; }
			set { _port.StopBits = value; }
		}

		public Handshake Handshake
		{
			get { return _port.Handshake; }
			set { _port.Handshake = value; }
		}

		public String PortName
		{
			get { return _port.PortName; }
			set { _port.PortName = value; }
		}

		public bool RemoveEcho
		{
			get { return _removeEcho; }
			set { _removeEcho = value; }
		}


		public ConnectionRs232()
		{
			SetDefaultPortConfig();

			_port.ReadBufferSize = 2048;
			_port.WriteBufferSize = 2048;
		}

		void SetDefaultPortConfig()
		{
			_port.Handshake = Handshake.None;
			_port.Parity = Parity.None;
			//_port.Parity = Parity.Odd;
			_port.PortName = "com1";
			//_port.BaudRate = 921600;
			_port.BaudRate = 9600;
			_port.StopBits = StopBits.One;
			_port.DataBits = 8;

		}

		#region Finalizing and disposing

		~ConnectionRs232()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			lock (_callSyncRoot)
			{
				Dispose(true);
			}
		}

		void Dispose(bool disposing)
		{
			_port.Dispose();
			//if (_serialPort.IsOpen) 
			//	_serialPort.Close();
		}

		#endregion

		#region IConnection members

		/// <summary>
		///  Is port open?
		/// </summary>
		public bool IsOpen()
		{
			return _port.IsOpen;
		}

		/// <summary>
		///  Opens port
		/// </summary>
		public void Open()
		{
			lock (_callSyncRoot)
			{
				//_port.DataReceived += SerialPortDataReceived;
				_port.Open();
				_port.DiscardInBuffer();
				_port.DiscardOutBuffer();
				IsOpen();
			}
		}

		/// <summary>
		///  Closes port.
		/// </summary>
		public void Close()
		{
			lock (_callSyncRoot)
			{
				//_port.DataReceived -= SerialPortDataReceived;
				_port.Close();
			}
		}


		/// <summary>
		///  Sends Cctalk message to device and waits for answer.
		/// </summary>
		public CctalkMessage Send(CctalkMessage com, ICctalkChecksum chHandler)
		{
			// TODO: handle BUSY message
			lock (_callSyncRoot)
			{

				var msgBytes = com.GetTransferDataNoChecksumm();
				chHandler.CalcAndApply(msgBytes);

				//_respondChecksumChecker = chHandler;

				_port.DiscardInBuffer();

				_port.Write(msgBytes, 0, msgBytes.Length);

				_port.ReadTimeout = RespondStartTimeout;
				Int32 respondBufPos = 0;
				CctalkMessage respond;


				var echoRemover = 0;
				while (true)
				{
					try
					{
						var b = (Byte)_port.ReadByte();
						_port.ReadTimeout = RespondDataTimeout;

						if (_removeEcho && (echoRemover < msgBytes.Length))
						{
							echoRemover++;
							continue;
						}
						_respondBuf[respondBufPos] = b;
						respondBufPos++;

						var isRespondComplete = GenericCctalkDevice.IsRespondComplete(_respondBuf, respondBufPos);
						if (isRespondComplete)
						{
							if (!chHandler.Check(_respondBuf, 0, respondBufPos))
							{
								var copy = new byte[respondBufPos];
								Array.Copy(_respondBuf, copy, respondBufPos);
								throw new InvalidRespondFormatException(copy, "Checksumm check fail");
							}
							respond = GenericCctalkDevice.ParseRespond(_respondBuf, 0, respondBufPos);
							Array.Clear(_respondBuf, 0, _respondBuf.Length);
							break;
						}

					}
					catch (TimeoutException ex)
					{
						if (_port.ReadTimeout == RespondStartTimeout)
							throw new TimeoutException("Device not respondng", ex);

						throw new TimeoutException("Pause in reply (should reset all communication vatiables and be ready to recive the next message)", ex);

					}
				}

				return respond;


				/*
				 * When receiving bytes within a message packet, the communication software should 
				 * wait up to 50ms for another byte if it is expected. If a timeout condition occurs, the 
				 * software should reset all communication variables and be ready to receive the next 
				 * message. No other action should be taken. (cctalk spec part1, 11.1) 
				 */

			}
		}

		#endregion

	}
}