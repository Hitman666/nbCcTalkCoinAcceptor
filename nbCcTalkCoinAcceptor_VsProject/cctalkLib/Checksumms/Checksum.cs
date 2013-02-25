using System;

namespace dk.CctalkLib.Checksumms
{
    public class Checksum : ICctalkChecksum
    {
        private static byte ChecksumHelper(byte[] source)
        {
            long sum = 0L;
            byte div = 0;

            foreach (byte t in source)
            {
            	sum += t;
            }

        	while (sum > 255)
            {
                sum -= 256;
            }
            if (sum == 0) sum = 256;

            div = (byte) (256 - sum);
            return div;
        }

        #region ICcTalkChecksum Membri di

        public void CalcAndApply(Byte[] messageInBytes)
        {
            if (messageInBytes == null) throw new ArgumentNullException("messageInBytes");

            var checksumPlace = messageInBytes.Length - 1;

            if (messageInBytes[checksumPlace] != 0)
                throw  new ArgumentException("Checksumm alredy set");

            byte retByte = ChecksumHelper(messageInBytes);
            messageInBytes[checksumPlace] = retByte;
        }

		public bool Check(byte[] messageInBytes, int offset, int length)
		{
			var cpy = new Byte[length];
			Array.Copy(messageInBytes, offset, cpy, 0, length);
			var res = ChecksumHelper(cpy);
			return res == 0;
		}

    	#endregion
    }
}