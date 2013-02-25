namespace dk.CctalkLib.Checksumms.Helpers
{
    abstract class CRC : ICRC
    {
        #region ICRC Membri di
        public CRC(InitialCrcValue val)
        {


        }

        public abstract byte[] ComputeChecksumBytes(byte[] arr);

        protected abstract uint ComputeChecksum(byte[] bytes);

        #endregion
    }
}
