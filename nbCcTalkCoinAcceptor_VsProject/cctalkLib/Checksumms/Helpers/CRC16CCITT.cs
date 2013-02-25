namespace dk.CctalkLib.Checksumms.Helpers
{
    class CRC16CCITT : CRC
    {
        private const ushort poly = 4129;
        private ushort[] table = new ushort[256];
        private ushort initialValue = 0;

        public CRC16CCITT(InitialCrcValue val)
            : base(val)
        {
            this.initialValue = (ushort)val;
            ushort temp, a;
            for (int i = 0; i < table.Length; ++i)
            {
                temp = 0;
                a = (ushort)(i << 8);
                for (int j = 0; j < 8; ++j)
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

        public override byte[] ComputeChecksumBytes(byte[] arr)
        {
            uint crc = ComputeChecksum(arr);
            return new byte[] { (byte)(crc >> 8), (byte)(crc & 0x00ff) };
        }

        protected override uint ComputeChecksum(byte[] bytes)
        {
            ushort crc = this.initialValue;
            for (int i = 0; i < bytes.Length; ++i)
            {
                crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes[i]))]);
            }
            return crc;
        }
    }
}
