namespace dk.CctalkLib.Devices
{
	/// <summary>
	///  Error codes for Coin acceptor events
	/// </summary>
	public enum CoinAcceptorErrors
	{
		///<summary>Null event ( no error ) </summary>
		NoError = 0,
		///<summary>Reject coin </summary>
		RejectCoin = 1,
		///<summary>Inhibited coin </summary>
		InhibitedCoin = 2,
		///<summary>Multiple window </summary>
		MultipleWindow = 3,
		///<summary>Wake-up timeout </summary>
		WakeupTimeout = 4,
		///<summary>Validation timeout </summary>
		ValidationTimeout = 5,
		///<summary>Credit sensor timeout </summary>
		CreditSensorTimeout = 6,
		///<summary>Sorter opto timeout </summary>
		SorterOptoTimeout = 7,
		///<summary>2nd close coin error </summary>
		SecondCloseCoinError = 8,
		///<summary>Accept gate not ready </summary>
		AcceptGateNotReady = 9,
		///<summary>Credit sensor not ready </summary>
		CreditSensorNotReady = 10,
		///<summary>Sorter not ready </summary>
		SorterNotReady = 11,
		///<summary>Reject coin not cleared </summary>
		RejectCoinNotCleared = 12,
		///<summary>Validation sensor not ready </summary>
		ValidationSensorNotReady = 13,
		///<summary>Credit sensor blocked </summary>
		CreditSensorBlocked = 14,
		///<summary>Sorter opto blocked </summary>
		SorterOptoBlocked = 15,
		///<summary>Credit sequence error </summary>
		CreditSequenceError = 16,
		///<summary>Coin going backwards </summary>
		CoinGoingBackwards = 17,
		///<summary>Coin too fast ( over credit sensor ) </summary>
		CoinTooFast = 18,
		///<summary>Coin too slow ( over credit sensor ) </summary>
		CoinTooSlow = 19,
		///<summary>C.O.S. mechanism activated ( coin-on-string ) </summary>
		CosActivated = 20,
		///<summary>DCE opto timeout </summary>
		DceOptoTimeout = 21,
		///<summary>DCE opto not seen </summary>
		DceOptoNotSeen = 22,
		///<summary>Credit sensor reached too early </summary>
		CreditSensorReachedTooEarly = 23,
		///<summary>Reject coin ( repeated sequential trip ) </summary>
		RejectCoinRepeat = 24,
		///<summary>Reject slug </summary>
		RejectSlug = 25,
		///<summary>Reject sensor blocked </summary>
		RejectSensorBlocked = 26,
		///<summary>Games overload </summary>
		GamesOverload = 27,
		///<summary>Max. coin meter pulses exceeded </summary>
		MaxCoinMeterPulsesExceeded = 28,
		///<summary>Accept gate open not closed </summary>
		AcceptGateOpenNotClosed = 29,
		///<summary>Accept gate closed not open </summary>
		AcceptGateClosedNotOpen = 30,
		///<summary>Manifold opto timeout </summary>
		ManifoldOptoTimeout = 31,
		///<summary>Manifold opto blocked </summary>
		ManifoldOptoBlocked = 32,
		///<summary>Manifold not ready </summary>
		ManifoldNotReady = 33,
		///<summary>Security status changed </summary>
		SecurityStatusChanged = 34,
		///<summary>Motor exception </summary>
		MotorException = 35,
		///<summary>Inhibited coin ( Type 1 ) </summary>
		InhibitedCoin01 = 128,
		///<summary>Inhibited coin ( Type 2 ) </summary>
		InhibitedCoin02 = 129,
		///<summary>Inhibited coin ( Type 3 ) </summary>
		InhibitedCoin03 = 130,
		///<summary>Inhibited coin ( Type 4 ) </summary>
		InhibitedCoin04 = 131,
		///<summary>Inhibited coin ( Type 5 ) </summary>
		InhibitedCoin05 = 132,
		///<summary>Inhibited coin ( Type 6 ) </summary>
		InhibitedCoin06 = 133,
		///<summary>Inhibited coin ( Type 7 ) </summary>
		InhibitedCoin07 = 134,
		///<summary>Inhibited coin ( Type 8 ) </summary>
		InhibitedCoin08 = 135,
		///<summary>Inhibited coin ( Type 9 ) </summary>
		InhibitedCoin09 = 136,
		///<summary>Inhibited coin ( Type 10 ) </summary>
		InhibitedCoin10 = 137,
		///<summary>Inhibited coin ( Type 11 ) </summary>
		InhibitedCoin11 = 138,
		///<summary>Inhibited coin ( Type 12 ) </summary>
		InhibitedCoin12 = 139,
		///<summary>Inhibited coin ( Type 13 ) </summary>
		InhibitedCoin13 = 140,
		///<summary>Inhibited coin ( Type 14 ) </summary>
		InhibitedCoin14 = 141,
		///<summary>Inhibited coin ( Type 15 ) </summary>
		InhibitedCoin15 = 142,
		///<summary>Inhibited coin ( Type 16 ) </summary>
		InhibitedCoin16 = 143,
		///<summary>Inhibited coin ( Type 17 ) </summary>
		InhibitedCoin17 = 144,
		///<summary>Inhibited coin ( Type 18 ) </summary>
		InhibitedCoin18 = 145,
		///<summary>Inhibited coin ( Type 19 ) </summary>
		InhibitedCoin19 = 146,
		///<summary>Inhibited coin ( Type 20 ) </summary>
		InhibitedCoin20 = 147,
		///<summary>Inhibited coin ( Type 21 ) </summary>
		InhibitedCoin21 = 148,
		///<summary>Inhibited coin ( Type 22 ) </summary>
		InhibitedCoin22 = 149,
		///<summary>Inhibited coin ( Type 23 ) </summary>
		InhibitedCoin23 = 150,
		///<summary>Inhibited coin ( Type 24 ) </summary>
		InhibitedCoin24 = 151,
		///<summary>Inhibited coin ( Type 25 ) </summary>
		InhibitedCoin25 = 152,
		///<summary>Inhibited coin ( Type 26 ) </summary>
		InhibitedCoin26 = 153,
		///<summary>Inhibited coin ( Type 27 ) </summary>
		InhibitedCoin27 = 154,
		///<summary>Inhibited coin ( Type 28 ) </summary>
		InhibitedCoin28 = 155,
		///<summary>Inhibited coin ( Type 29 ) </summary>
		InhibitedCoin29 = 156,
		///<summary>Inhibited coin ( Type 30 ) </summary>
		InhibitedCoin30 = 157,
		///<summary>Inhibited coin ( Type 31 ) </summary>
		InhibitedCoin31 = 158,
		///<summary>Inhibited coin ( Type 32 ) </summary>
		InhibitedCoin32 = 159,
		///<summary> Data block request ( note_α ) </summary>
		DataBlockRequest = 253,
		///<summary>Coin return mechanism activated ( flight deck open ) </summary>
		CoinReturnMechanismActivated = 254,
		///<summary>Unspecified alarm code </summary>
		UnspecifiedAlarmCode = 255,
	}
}