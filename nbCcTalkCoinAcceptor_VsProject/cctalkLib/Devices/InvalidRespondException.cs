using System;
using dk.CctalkLib.Messages;

namespace dk.CctalkLib.Devices
{
	[Serializable]
	internal class InvalidRespondException : Exception
	{
		public InvalidRespondException(CctalkMessage respond)
			: this(respond, "Invalid respond")
		{
		}

		public InvalidRespondException(CctalkMessage respond, string message)
			: base(message)
		{
			InvalidRespond = respond;
		}

		public CctalkMessage InvalidRespond { get; private set; }

	}
}