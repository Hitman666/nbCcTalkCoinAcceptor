using System;
using System.Collections.Generic;
using System.Linq;
using dk.CctalkLib.Checksumms.Helpers;
using dk.CctalkLib.Messages;

namespace dk.CctalkLib.Checksumms
{
    class CRCChecksum : ICctalkChecksum
    {
        #region ICcTalkChecksum Membri di
        private Dictionary<CRCType, ICRC> cr;
        //private byte[] arr;
        //private int length;

        public CRCChecksum()
        {
            cr = new Dictionary<CRCType, ICRC>
                     {
                         {CRCType.CRC16, new CRC16(InitialCrcValue.Zeros)},
                         {CRCType.CRC16CCITT, new CRC16CCITT(InitialCrcValue.Zeros)},
                         {CRCType.CRC32, new CRC32(InitialCrcValue.Zeros)}
                     };
        }

        public IEnumerable<byte> Execute(IEnumerable<byte> source)
        {
            var check = cr[CRCType.CRC16];
            var d = check.ComputeChecksumBytes(source);
            //this.arr[2] = d[1];
            //this.arr[this.length - 1] = d[0];
            return d;
        }

        public void CalcAndApply(IEnumerable<byte> messageInBytes)
        {
            var msgArry = messageInBytes.ToArray();

            var tmp = new byte[msgArry.Length - 2];
            tmp[0] = msgArry[0];
            tmp[1] = msgArry[1];

            Array.Copy(msgArry, 3, tmp, 2, msgArry.Length - 4); //TODO: bug possible. not checked

            var cs = Execute(tmp).ToArray();
            msgArry[msgArry.Length-1] = cs[0];
            msgArry[CctalkMessage.PosSourceAddr] = cs[1];
        }

    	public bool Check(IEnumerable<byte> messageInBytes, int offset, int length)
    	{
    		throw new NotImplementedException();
    	}

    	#endregion
    }
}
