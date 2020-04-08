using System;

namespace dk.CctalkLib.Devices
{

	/// <summary>
	/// Data for CoinAcceptor`s event. Contains various info about accepted coin
	/// </summary>
	public class CoinAcceptorCoinEventArgs : EventArgs
	{
		/// <summary>
		///  Name of coin as was set in CoinAccpetor constructor.
		/// </summary>
		public string CoinName { get; }
		public decimal CoinValue { get; }

		/// <summary>
		///  Device`s code for coin
		/// </summary>
		public byte CoinCode { get; }

		/// <summary>
		///  Device`s route id for coin.
		/// </summary>
		public byte RoutePath { get; }

		/// <summary>
		///  Creates instance of CoinAcceptorCoinEventArgs
		/// </summary>
		public CoinAcceptorCoinEventArgs(string coinName, decimal coinValue, byte coinCode, byte routePath)
		{
			CoinName = coinName;
			CoinValue = coinValue;
			CoinCode = coinCode;
			RoutePath = routePath;
		}
	}
}