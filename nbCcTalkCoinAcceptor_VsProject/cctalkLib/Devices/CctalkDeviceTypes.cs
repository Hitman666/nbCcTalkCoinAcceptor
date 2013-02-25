namespace dk.CctalkLib.Devices
{
	/// <summary>
	/// Possible types of cctalk devices
	/// </summary>
	public enum CctalkDeviceTypes
	{
		/// <summary> Unknown device </summary>
		Unknown = 0,

		///<summary>a.k.a Coin Validator </summary>
		CoinAcceptor = 2,

		///<summary>a.k.a Hopper </summary>
		Payout = 3,

		/// <summary> Reel </summary>
		Reel = 30,

		///<summary>a.k.a Note_Acceptor </summary>
		BillValidator = 40,

		/// <summary> Card reader </summary>
		CardReader = 50,

		///<summary>Money-in, money-out recyclers. Also used for coin singulators and sorters. </summary>
		Changer = 55,

		///<summary> e.g. LCD panels, alpha-numeric displays </summary>
		Display = 60,

		///<summary>Remote keyboard</summary>
		Keypad = 70,

		///<summary> Security device, interface box or interface hub </summary>
		Dongle = 80,

		///<summary>Electro-mechanical counter replacement </summary>
		Meter = 90,

		///<summary>Bootloader firmware and diagnostics when no application code is loaded.</summary>
		Bootloader = 99,

		///<summary> Power switching hub or intelligent power supply </summary>
		Power = 100,

		///<summary>Ticket printer for coupons and barcodes </summary>
		Printer = 110,

		///<summary>Random Number Generator </summary>
		RNG = 120,

		///<summary> Hopper with weigh scale </summary>
		HopperScale = 130,

		///<summary>Motorised coin feeder or singulator</summary>
		CoinFeeder = 140,

		///<summary>This address range may be used when developing new peripherals </summary>
		Debug = 240,
	}
}