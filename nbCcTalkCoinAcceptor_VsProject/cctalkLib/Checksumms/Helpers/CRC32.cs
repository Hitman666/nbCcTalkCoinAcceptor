using System;
using System.Collections.Generic;

namespace dk.CctalkLib.Checksumms.Helpers
{
    class CRC32 : CRC
    {
        private uint[] table;

        public CRC32(InitialCrcValue val)
            : base(val)
        {
            var poly = 0xedb88320;
            table = new uint[256];
            uint temp = 0;
            for (uint i = 0; i < table.Length; ++i)
            {
                temp = i;
                for (var j = 8; j > 0; --j)
                {
                    if ((temp & 1) == 1)
                    {
                        temp = (temp >> 1) ^ poly;
                    }
                    else
                    {
                        temp >>= 1;
                    }
                }
                table[i] = temp;
            }
        }

        public override IEnumerable<byte> ComputeChecksumBytes(IEnumerable<byte> arr)
        {
            return BitConverter.GetBytes(ComputeChecksum(arr));
        }

        protected override uint ComputeChecksum(IEnumerable<byte> bytes)
        {
            var crc = 0xffffffff;
            foreach (var item in bytes)
            {
                var index = (byte)(((crc) & 0xff) ^ item);
                crc = (crc >> 8) ^ table[index];
            }
            return ~crc;
        }
    }
}
