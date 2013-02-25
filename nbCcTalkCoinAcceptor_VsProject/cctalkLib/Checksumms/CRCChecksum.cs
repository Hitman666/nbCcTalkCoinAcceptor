using System;
using System.Collections.Generic;
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

        public Byte[] Execute(Byte[] source)
        {
            ICRC check = this.cr[CRCType.CRC16];
            byte[] d = check.ComputeChecksumBytes(source);
            //this.arr[2] = d[1];
            //this.arr[this.length - 1] = d[0];
            return d;
        }

        public void CalcAndApply(Byte[] messageInBytes)
        {
            //Byte[] sourceCut;
            //if(offset==0&&length==source.Length)
            //    sourceCut = source;
            //else
            //{
            //   sourceCut = new byte[length];
            //    Array.Copy(source,offset,sourceCut,0, length);
            //}

            var tmp = new byte[messageInBytes.Length - 2];
            tmp[0] = messageInBytes[0];
            tmp[1] = messageInBytes[1];

            Array.Copy(messageInBytes, 3, tmp, 2, messageInBytes.Length - 4); //TODO: bug possible. not checked

            //tmp[2] = this.arr[3];
            //for (int i = 0; i < this.arr[1]; i++)
            //{
            //    tmp[3 + i] = this.arr[4 + i];
            //}


            var cs = Execute(tmp);
            messageInBytes[messageInBytes.Length-1] = cs[0];
            messageInBytes[CctalkMessage.PosSourceAddr] = cs[1];
        }

    	public bool Check(byte[] messageInBytes, int offset, int length)
    	{
    		throw new NotImplementedException();
    	}

    	#endregion
    }
}
