using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dk.CctalkLib.Devices
{
	public class BillValidatorErrorEventArgs : EventArgs
	{

		public BillValidatorErrors Error { get; private set; }
		public string ErrorMessage { get; private set; }
		//public byte ErrorCode { get; private set; }

		public BillValidatorErrorEventArgs(BillValidatorErrors error, String errorMessage)
		{
			Error = error;
			ErrorMessage = errorMessage;
			//ErrorCode = errorCode;
		}
	}
}
