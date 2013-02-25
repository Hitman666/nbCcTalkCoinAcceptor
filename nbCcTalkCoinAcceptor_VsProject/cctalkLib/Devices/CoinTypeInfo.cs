using System;

namespace dk.CctalkLib.Devices
{
	public class CoinTypeInfo
	{
		public CoinTypeInfo(String name, Decimal value)
		{
			Name = name;
			Value = value;
		}

		public String Name { get; private set; }
		public Decimal Value { get; private set; }

	}
}