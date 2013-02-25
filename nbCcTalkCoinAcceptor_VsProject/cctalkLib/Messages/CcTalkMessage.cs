using System;

namespace dk.CctalkLib.Messages
{
	/// <summary>
	///  Incapsulates fields of generic cctalk message (command or respond). Implemets serializatoin.
	/// </summary>
	public class CctalkMessage
    {
		/// <summary>
		/// Minimal possible message length
		/// </summary>
        public static readonly Byte MinMessageLength = 5;

		/// <summary>
		///  Maximal posible data length
		/// </summary>
        public static readonly Byte MaxDataLength = 252;

        public static readonly Byte PosDestAddr = 0;
        public static readonly Byte PosDataLen = 1;
        public static readonly Byte PosSourceAddr = 2;
        public static readonly Byte PosHeader = 3;
        public static readonly Byte PosDataStart = 4;

		/// <summary>
		///  Destination device cctalk address. 0 - broadcast (single device on bus only).
		/// </summary>
        public Byte DestAddr { set; get; }
		/// <summary>
		/// Source device cctalk address. 1 for host devices. When CRC checksum used - there is second byte for checksum.
		/// </summary>
        public Byte SourceAddr { set; get; }

		/// <summary>
		///  Message header. Command or respond code.
		/// </summary>
        public Byte Header { get; set; }

		/// <summary>
		///  Dete for message. Format depends on header.
		/// </summary>
        public Byte[] Data;

		/// <summary>
		///  Length of data in bytes
		/// </summary>
        public Byte DataLength { get { return (Byte)(Data == null ? 0 : Data.Length); }
        }

		/// <summary>
		///  Serializes message for transfer, but does not apply checksum
		/// </summary>
        public Byte[] GetTransferDataNoChecksumm()
        {
            Byte[] msgData = Data;
            var msgDataLen = (Byte)(msgData == null ? 0 : msgData.Length);

            if (msgDataLen > MaxDataLength) 
                throw new InvalidOperationException("Data too long. " + GetType().Name);

            var msg = new byte[MinMessageLength + msgDataLen];
            msg[PosDestAddr] = DestAddr;
            msg[PosDataLen] = msgDataLen;
            msg[PosSourceAddr] = SourceAddr;
            msg[PosHeader] = Header;

            if (msgData!= null && msgDataLen > 0)
                Array.Copy(msgData, 0, msg, PosDataStart, msgData.Length);

            return msg;
        }


    }
}
