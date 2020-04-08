using System;
using System.Collections.Generic;
using System.Linq;

namespace dk.CctalkLib.Checksumms
{
    public class Checksum : ICctalkChecksum
    {
        private static byte ChecksumHelper(byte[] source)
        {
            var sum = 0L;
            byte div = 0;

            foreach (var t in source)
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

        public void CalcAndApply(IEnumerable<byte> messageInBytes)
        {
            if (messageInBytes == null) throw new ArgumentNullException("messageInBytes");

            var byteArray = messageInBytes.ToArray();

            var checksumPlace = byteArray.Length - 1;

            if (byteArray[checksumPlace] != 0)
                throw  new ArgumentException("Checksumm alredy set");

            var retByte = ChecksumHelper(byteArray);
            byteArray[checksumPlace] = retByte;
        }

		public bool Check(IEnumerable<byte> messageInBytes, int offset, int length)
		{
			var cpy = new byte[length];
			Array.Copy(messageInBytes.ToArray(), offset, cpy, 0, length);
			var res = ChecksumHelper(cpy);
			return res == 0;
		}

    	#endregion
    }
}