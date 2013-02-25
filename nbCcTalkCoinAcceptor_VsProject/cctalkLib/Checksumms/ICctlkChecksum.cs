using System;

namespace dk.CctalkLib.Checksumms
{
	/// <summary>
	///  Type of checksumm for cctalk messages.
	///  ** CRC is currenly not supported (but there is some raw code for it) **
	/// </summary>
    public enum CheckSumType
    {
        None = 0,
        Normal = 1,
        CRC = 2
    }

    public interface ICctalkChecksum
    {
        void CalcAndApply(Byte[] messageInBytes);
		Boolean Check(Byte[] messageInBytes, Int32 offset, Int32 length);

    }
}
