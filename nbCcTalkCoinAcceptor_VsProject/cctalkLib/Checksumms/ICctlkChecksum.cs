using System.Collections.Generic;

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
        void CalcAndApply(IEnumerable<byte> messageInBytes);
		bool Check(IEnumerable<byte> messageInBytes, int offset, int length);

    }
}
