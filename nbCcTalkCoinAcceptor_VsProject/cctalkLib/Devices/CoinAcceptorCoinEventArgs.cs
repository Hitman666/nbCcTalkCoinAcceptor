using System;

namespace dk.CctalkLib.Devices
{

	/// <summary>
	/// Data for CoinAcceptor`s event. Contains various info about accepted coin
	/// </summary>
	public class CoinAcceptorCoinEventArgs : EventArgs
	{
		/// <summary>
		///  Name of coin as was set in CouinAccpetor constructor.
		/// </summary>
		public String CoinName { get; private set; }
		public Decimal CoinValue { get; private set; }

		/// <summary>
		///  Device`s code for coin
		/// </summary>
		public Byte CoinCode { get; private set; }

		/// <summary>
		///  Device`s route id for coin.
		/// </summary>
		public Byte RoutePath { get; private set; }

		/// <summary>
		///  Creates instance of CoinAcceptorCoinEventArgs
		/// </summary>
		public CoinAcceptorCoinEventArgs(String coinName, decimal coinValue, Byte coinCode, Byte routePath)
		{
			CoinName = coinName;
			CoinValue = coinValue;
			CoinCode = coinCode;
			RoutePath = routePath;
		}
	}
}