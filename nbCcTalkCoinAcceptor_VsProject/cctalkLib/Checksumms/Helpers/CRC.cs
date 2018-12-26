using System.Collections.Generic;

namespace dk.CctalkLib.Checksumms.Helpers
{
    abstract class CRC : ICRC
    {
        #region ICRC Membri di
        public CRC(InitialCrcValue val)
        {


        }

        public abstract IEnumerable<byte> ComputeChecksumBytes(IEnumerable<byte> arr);

        protected abstract uint ComputeChecksum(IEnumerable<byte> bytes);

        #endregion
    }
}
