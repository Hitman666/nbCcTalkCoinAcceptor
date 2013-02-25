using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using dk.CctalkLib.Checksumms;
using dk.CctalkLib.Connections;
using dk.CctalkLib.Messages;

namespace dk.CctalkLib.Devices
{
	/// <summary>
	///  Executes ccTalk commands and parses respond
	/// </summary>
	public class GenericCctalkDevice
	{
		public static readonly Byte SourceAddress = 1;

		public Byte Address { get; set; }

		public ICctalkConnection Connection { get; set; }

		protected readonly Checksum _checksumHandler = new Checksum();

		static readonly Dictionary<Byte, TimeSpan> PollingIntervalUnits
			= new Dictionary<byte, TimeSpan>
			  	{
			  		// 0 - special case
			  		{1, new TimeSpan(0, 0, 0, 0, 1)},//ms 
			  		{2, new TimeSpan(0, 0, 0, 0, 10)},//x10 ms 
			  		{3, new TimeSpan(0, 0, 0, 1, 0)},//seconds 
			  		{4, new TimeSpan(0, 0, 1, 0, 0)},//minutes
			  		{5, new TimeSpan(0, 1, 0, 0, 0)},//hours
			  		{6, new TimeSpan(1, 0, 0, 0, 0)},//days
			  		{7, new TimeSpan(7, 0, 0, 0, 0)},//weeks
			  		{8, new TimeSpan(30, 0, 0, 0, 0)},//months
			  		{9, new TimeSpan(365, 0, 0, 0, 0)},//years
			  	};

		static readonly Dictionary<String, CctalkDeviceTypes> DeviceTypes
			= new Dictionary<String, CctalkDeviceTypes>
			  	{
			  		{"Coin Acceptor", CctalkDeviceTypes.CoinAcceptor},
			  		{"Payout", CctalkDeviceTypes.Payout},
			  		{"Reel", CctalkDeviceTypes.Reel},
			  		{"Bill Validator", CctalkDeviceTypes.BillValidator},
			  		{"Card Reader", CctalkDeviceTypes.CardReader},
			  		{"Changer", CctalkDeviceTypes.Changer},
			  		{"Display", CctalkDeviceTypes.Display},
			  		{"Keypad", CctalkDeviceTypes.Keypad},
			  		{"Dongle", CctalkDeviceTypes.Dongle},
			  		{"Meter", CctalkDeviceTypes.Meter},
			  		{"Bootloader", CctalkDeviceTypes.Bootloader},
			  		{"Power", CctalkDeviceTypes.Power},
			  		{"Printer", CctalkDeviceTypes.Printer},
			  		{"RNG", CctalkDeviceTypes.RNG},
			  		{"Hopper Scale", CctalkDeviceTypes.HopperScale},
			  		{"Coin Feeder", CctalkDeviceTypes.CoinFeeder},
			  		{"Debug", CctalkDeviceTypes.Debug},
			  	};


		protected CctalkMessage CreateMessage(Byte header)
		{
			return new CctalkMessage
					   {
						   DestAddr = Address,
						   SourceAddr = SourceAddress,
						   Header = header,
					   };
		}

		#region Commands

		public void CmdReset()
		{
			var msg = CreateMessage(1);

			Connection.Send(msg, _checksumHandler);
			//TODO: after reset device could not respond for random time. workaround needed. Maybe sleep?
			Thread.Sleep(50); // not wery useful in multithread program
		}

		public void CmdSimplePoll()
		{
			var msg = CreateMessage(254);

			Connection.Send(msg, _checksumHandler);
		}

		public TimeSpan CmdRequestPollingPriority()
		{
			var msg = CreateMessage(249);

			var respond = Connection.Send(msg, _checksumHandler);
			if (respond.DataLength < 2)
				throw new InvalidRespondException(respond);

			var units = respond.Data[0];
			var value = respond.Data[1];

			if (units == 0)
				return TimeSpan.Zero;


			return TimeSpan.FromMilliseconds(PollingIntervalUnits[units].TotalMilliseconds * value);

		}

		public CctalkDeviceStatus CmdRequestStatus()
		{
			var msg = CreateMessage(248);

			var respond = Connection.Send(msg, _checksumHandler);
			if (respond.DataLength < 1)
				throw new InvalidRespondException(respond);

			return (CctalkDeviceStatus)respond.Data[0];
		}
		public String CmdRequestManufacturerId()
		{
			return RequestForStringHelper(246);

		}
		public CctalkDeviceTypes CmdRequestEquipmentCategory()
		{
			var catName = RequestForStringHelper(245);

			CctalkDeviceTypes ret;
			DeviceTypes.TryGetValue(catName, out ret);

			return ret;
		}

		public String CmdRequestProductCode()
		{
			return RequestForStringHelper(244);
		}

		public String CmdRequestSoftwareRevision()
		{
			return RequestForStringHelper(241);
		}

		public DeviceEventBuffer CmdReadEventBuffer()
		{
			var msg = CreateMessage(229);
			var respond = Connection.Send(msg, _checksumHandler);

			if (respond.DataLength < 11)
				throw new InvalidRespondException(respond);

			var data = respond.Data;
			var events = new[]
                             {
                                 new DeviceEvent(data[1], data[2]),
                                 new DeviceEvent(data[3], data[4]),
                                 new DeviceEvent(data[5], data[6]),
                                 new DeviceEvent(data[7], data[8]),
                                 new DeviceEvent(data[9], data[10]),
                             }
				;


			var ret = new DeviceEventBuffer
						  {
							  Counter = respond.Data[0],
							  Events = events,
						  };

			return ret;
		}

		public Int32 CmdGetSerial()
		{
			var msg = CreateMessage(242);
			var respond = Connection.Send(msg, _checksumHandler);

			if (respond.DataLength < 3)
				throw new InvalidRespondException(respond);

			Int32 sn = 0;
			sn += respond.Data[2];
			sn = sn << 8;
			sn += respond.Data[1];
			sn = sn << 8;
			sn += respond.Data[0];

			return sn;
		}


		public void CmdSetMasterInhibitStatus(Boolean isInhibiting)
		{
			var msg = CreateMessage(228);
			msg.Data = new[] {(Byte) (isInhibiting ? 0 : 1)};
			Connection.Send(msg, _checksumHandler);
		}

		public Boolean CmdGetMasterInhibitStatus()
		{
			var msg = CreateMessage(227);
			var respond = Connection.Send(msg, _checksumHandler);
			if (respond.DataLength < 1)
				throw new InvalidRespondException(respond);

			bool isInhibiting = (respond.Data[0] & 0x01) == 0; // only last bit significant
			return isInhibiting;
		}

		/*
		 *  Send: [Dir] [2] [2] [231] [Data 1] [Data 2] [Chk]
		 *	Reply: [1] [0] [Dir] [0] [Chk] -> ACK without data
		 *	
		 * [Data 1] = Inhibit byte 1 (LSB), coins 1 to 8
		 * [Data 2] = Inhibit byte 2 (MSB), coins 9 to 16
		 * 
		 * bit 0 (in Data 1): coin 1
		 * bit 15 (in Data 2, equivalent to bit 7 of Data 2): coin 16
		*/
		public void CmdModifyInhibitStatus(int Data1, int Data2)
		{
			var msg = CreateMessage(231);
			msg.Data = new[] { (Byte)Data1, (Byte)Data2 };
			Connection.Send(msg, _checksumHandler);			
		}
		#endregion

		String RequestForStringHelper(Byte header)
		{
			var msg = CreateMessage(header);

			var respond = Connection.Send(msg, _checksumHandler);
			if (respond.DataLength < 1)
				throw new InvalidRespondException(respond);

			var ret = ParseAsciiHelper(respond.Data);

			return ret;

		}


		static String ParseAsciiHelper(Byte[] data)
		{
			return Encoding.ASCII.GetString(data);
		}

		public static CctalkMessage ParseRespond(Byte[] source, Int32 offset, Int32 length)
		{
			var ret = ParseMessage(source, offset, length);
			if (ret.Header != 0)
				throw new InvalidRespondFormatException(source, "Invalid respond header. Possible reason: echo is enabled");
			return ret;
		}

		public static CctalkMessage ParseMessage(Byte[] source, Int32 offset, Int32 length)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (source.Length < offset + length)
				throw new ArgumentException("offset or length are invalid");

			if (length < CctalkMessage.MinMessageLength)
				throw new ArgumentException("too small for message", "length");

			//if (header != this.Header)
			//    throw new ArgumentException("invalid message type for this class", "source");

			var dataLen = source[CctalkMessage.PosDataLen + offset];

			if (dataLen + 5 != length)
				throw new ArgumentException("invalid message data lengh", "source");

			var ret = new CctalkMessage
						  {
							  Header = source[CctalkMessage.PosHeader + offset],
							  DestAddr = source[CctalkMessage.PosDestAddr + offset],
							  SourceAddr = source[CctalkMessage.PosSourceAddr + offset]
						  };

			if (dataLen > 0)
			{
				ret.Data = new byte[dataLen];
				Array.Copy(source, CctalkMessage.PosDataStart + offset, ret.Data, 0, dataLen);
			}

			return ret;
		}

		public static Boolean IsRespondComplete(Byte[] respondRawData, Int32 lengthOverride)
		{
			if (lengthOverride <= 4) return false;
			if (lengthOverride > 255) return true;

			var expectedLen = GetExpectedLength(respondRawData);
			return expectedLen == lengthOverride;
		}

		public static Boolean IsRespondComplete(Byte[] respondRawData)
		{
			return IsRespondComplete(respondRawData, respondRawData.Length);
		}

		private static Int32 GetExpectedLength(Byte[] respondRawData)
		{
			if (respondRawData.Length <= CctalkMessage.MinMessageLength)
				throw new InvalidRespondFormatException(respondRawData);

			var dataLen = respondRawData[CctalkMessage.PosDataLen];
			return CctalkMessage.MinMessageLength + dataLen; // 1Src+1Len+1Dest+1Header+Data+1Checksum
		}


	}
}