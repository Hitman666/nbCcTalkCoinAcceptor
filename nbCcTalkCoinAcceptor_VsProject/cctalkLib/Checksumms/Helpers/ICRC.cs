using System.Collections.Generic;

namespace dk.CctalkLib.Checksumms.Helpers
{
    enum CRCType
    {
        CRC16 = 100,
        CRC32 = 101,
        CRC16CCITT = 102
    }
    
    enum InitialCrcValue 
    { 
        Zeros = 0x0000, 
        NonZero1 = 0xffff, 
        NonZero2 = 0x1D0F 
    }
    
    interface ICRC
    {

        IEnumerable<byte> ComputeChecksumBytes(IEnumerable<byte> arr);
        //ushort ComputeChecksum(byte[] bytes);
    }
    
}
