namespace dk.CctalkLib.Devices
{
	public class BillTypeInfo
	{
		public BillTypeInfo(string name, decimal value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; }
		public decimal Value { get; }
	}
}
