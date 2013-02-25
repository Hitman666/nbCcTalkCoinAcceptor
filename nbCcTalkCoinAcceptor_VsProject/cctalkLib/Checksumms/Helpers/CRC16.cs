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

        public override byte[] ComputeChecksumBytes(byte[] arr)
        {
            uint crc = this.ComputeChecksum(arr);
            return new byte[] { (byte)(crc >> 8), (byte)(crc & 0x00ff) };
        }

        protected override uint ComputeChecksum(byte[] bytes)
        {
            ushort crc = 0;
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte index = (byte)(crc ^ bytes[i]);
                crc = (ushort)((crc >> 8) ^ table[index]);
            }
            return crc;
        }
    }
}
