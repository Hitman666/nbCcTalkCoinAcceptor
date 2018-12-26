
namespace dk.CctalkLib.Devices
{
	public struct DeviceEvent
	{
		public DeviceEvent(byte coinCode, byte errorOrRouteCode)
		{
			CoinCode = coinCode;
			ErrorOrRouteCode = errorOrRouteCode;
		}

		public byte CoinCode;
		public byte ErrorOrRouteCode;

	    public bool IsError => CoinCode == 0;
	}
}