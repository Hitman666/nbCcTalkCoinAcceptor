using System;

namespace dk.CctalkLib.Devices
{
	public class CoinAcceptorErrorEventArgs : EventArgs
	{
	    public CoinAcceptorErrors Error { get; }

	    public string ErrorMessage { get; }
	    //public byte ErrorCode { get; private set; }

		public CoinAcceptorErrorEventArgs(CoinAcceptorErrors error, string errorMessage)
		{
			Error = error;
			ErrorMessage = errorMessage;
			//ErrorCode = errorCode;
		}
	}
}