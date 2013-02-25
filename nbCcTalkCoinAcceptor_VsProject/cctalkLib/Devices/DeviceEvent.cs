using System;

namespace dk.CctalkLib.Devices
{
	public struct DeviceEvent
	{
		public DeviceEvent(Byte coinCode, Byte errorOrRouteCode)
		{
			CoinCode = coinCode;
			ErrorOrRouteCode = errorOrRouteCode;
		}

		public Byte CoinCode;
		public Byte ErrorOrRouteCode;

		public Boolean IsError { get { return CoinCode == 0; } }
	}
}