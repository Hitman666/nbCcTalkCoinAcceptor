using System.Collections.Generic;
using System.Linq;

namespace dk.CctalkLib.Checksumms.Helpers
{
    class CRC16CCITT : CRC
    {
        private const ushort poly = 4129;
        private ushort[] table = new ushort[256];
        private ushort initialValue;

        public CRC16CCITT(InitialCrcValue val)
            : base(val)
        {
            initialValue = (ushort)val;
            ushort temp, a;
            for (var i = 0; i < table.Length; ++i)
            {
                temp = 0;
                a = (ushort)(i << 8);
                for (var j = 0; j < 8; ++j)
                {
                    if (((temp ^ a) & 0x8000) != 0)
                    {
                        temp = (ushort)((temp << 1) ^ poly);
                    }
                    else
                    {
                        temp <<= 1;
                    }
                    a <<= 1;
                }
                table[i] = temp;
            }
        }

        public override IEnumerable<byte> ComputeChecksumBytes(IEnumerable<byte> bytes)
        {
            var crc = ComputeChecksum(bytes);
            return new[] { (byte)(crc >> 8), (byte)(crc & 0x00ff) };
        }

        protected override uint ComputeChecksum(IEnumerable<byte> bytes)
        {
            return bytes.Aggregate(initialValue,
                (current, item) => 
                    (ushort) ((current << 8) ^ table[(current >> 8) ^ (0xff & item)]));
        }
    }
}
