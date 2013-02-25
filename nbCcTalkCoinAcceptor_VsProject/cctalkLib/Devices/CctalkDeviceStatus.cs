namespace dk.CctalkLib.Devices
{
	/// <summary>
	///  Possible statuses of cctalk devices
	/// </summary>
	public enum CctalkDeviceStatus:byte
	{
		// cctalk ref part3 page 72

		/// <summary>
		/// OK 
		/// </summary>
		Ok = 0,
		/// <summary>
		/// Coin return mechanism activated ( flight deck open )
		/// </summary>
		CoinReturn = 1,
		/// <summary>
		/// C.O.S. mechanism activated ( coin-on-string ) 
		/// </summary>
		Cos = 2,

		/// <summary>
		/// Unknown error or invalid call
		/// </summary>
		OtherError = 255,
	}
}