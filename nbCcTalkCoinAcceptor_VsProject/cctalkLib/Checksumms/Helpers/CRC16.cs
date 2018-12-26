using System.Collections.Generic;
using System.Linq;

namespace dk.CctalkLib.Checksumms.Helpers
{
    class CRC16 : CRC
    {

        private const ushort polynomial = 0xA001;
        private ushort[] table = new ushort[256];

        public CRC16(InitialCrcValue val)
            : base(val)
        {
            ushort value;
            ushort temp;
            for (ushort i = 0; i < table.Length; ++i)
            {
                value = 0;
                temp = i;
                for (byte j = 0; j < 8; ++j)
                {
                    if (((value ^ temp) & 0x0001) != 0)
                    {
                        value = (ushort)((value >> 1) ^ polynomial);
                    }
                    else
                    {
                        value >>= 1;
                    }
                    temp >>= 1;
                }
                table[i] = value;
            }
        }

        public override IEnumerable<byte> ComputeChecksumBytes(IEnumerable<byte> arr)
        {
            var crc = ComputeChecksum(arr);
            return new[] { (byte)(crc >> 8), (byte)(crc & 0x00ff) };
        }

        protected override uint ComputeChecksum(IEnumerable<byte> bytes)
        {
            ushort crc = 0;

            foreach (var item in bytes)
            {
                var index = (byte)(crc ^ item);
                crc = (ushort)((crc >> 8) ^ table[index]);
            }
            return crc;
        }
    }
}
