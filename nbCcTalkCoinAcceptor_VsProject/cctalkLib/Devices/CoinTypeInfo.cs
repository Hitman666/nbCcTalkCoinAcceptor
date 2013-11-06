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

    /// <summary>
    /// Represents the available channels for coins in a coin acceptor
    /// </summary>
    public enum CoinIndex
    {
        One = 1,
        Two = 2,
        Three = 4,
        Four = 8,
        Five = 16,
        Six = 32,
        Seven = 64,
        Height = 128,
        Nine = 256,
        Ten = 512,
        Eleven = 1024,
        Twelve = 2048,
        Thirteen = 4096,
        Fourteen = 8192,
        Fifteen = 16384,
        Sixteen = 32768,
        None = 0,
        All = 0xffff,
    }

}