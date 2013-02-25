using System;

namespace dk.CctalkLib.Devices
{
	public class CoinAcceptorErrorEventArgs : EventArgs
	{
		public CoinAcceptorErrors Error { get; private set; }
		public string ErrorMessage { get; private set; }
		//public byte ErrorCode { get; private set; }

		public CoinAcceptorErrorEventArgs(CoinAcceptorErrors error, String errorMessage)
		{
			Error = error;
			ErrorMessage = errorMessage;
			//ErrorCode = errorCode;
		}
	}
}