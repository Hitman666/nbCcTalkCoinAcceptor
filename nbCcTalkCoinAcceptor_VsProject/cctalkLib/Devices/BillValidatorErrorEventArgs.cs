using System;

namespace dk.CctalkLib.Devices
{
	public class BillValidatorErrorEventArgs : EventArgs
	{

		public BillValidatorErrors Error { get; }
		public string ErrorMessage { get; }
		//public byte ErrorCode { get; private set; }

		public BillValidatorErrorEventArgs(BillValidatorErrors error, string errorMessage)
		{
			Error = error;
			ErrorMessage = errorMessage;
			//ErrorCode = errorCode;
		}
	}
}
