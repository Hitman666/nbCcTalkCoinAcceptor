using System;
using System.Collections.Generic;

namespace dk.CctalkLib.Devices
{
	[Serializable]
	internal class InvalidRespondFormatException : Exception
	{
		public InvalidRespondFormatException(IEnumerable<byte> respondRawData) : this(respondRawData, "Invalid respond")
		{
		}

		public InvalidRespondFormatException(IEnumerable<byte> respondRawData, string message) : base(message)
		{
			InvalidRespondData = respondRawData;
		}

	    public IEnumerable<byte> InvalidRespondData { get; }

	}
}