using System;

namespace dk.CctalkLib.Devices
{
	[Serializable]
	internal class InvalidRespondFormatException : Exception
	{
		public InvalidRespondFormatException(Byte[] respondRawData) : this(respondRawData, "Invalid respond")
		{
		}

		public InvalidRespondFormatException(byte[] respondRawData, string message) : base(message)
		{
			InvalidRespondData = respondRawData;
		}

		public Byte[] InvalidRespondData { get; private set; }

	}
}