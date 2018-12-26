
using System;

namespace dk.CctalkLib.Devices
{
    public class CoinTypeInfo
    {
        public CoinTypeInfo(string name, decimal value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public decimal Value { get; }

    }

    /// <summary>
    /// Represents the available channels for coins in a coin acceptor
    /// </summary>
    [Flags]
    public enum CoinIndex
    {
        None = 0,
        One = 1<<0, //1
        Two = 1<<1, //2
        Three = 1<<3, //4
        Four = 1<<4, //8
        Five = 1<<5, //16
        Six = 1<<6, //32
        Seven = 1<<7, //64
        Eight = 1<<8, //128
        Nine = 1<<9, //256
        Ten = 1<<10, //512
        Eleven = 1<<11, //1024
        Twelve = 1<<12, //2048
        Thirteen = 1<<13, //4096
        Fourteen = 1<<14, //8192
        Fifteen = 1<<15, //16384
        Sixteen = 1<<16, //32768
        All = ~0
    }

}