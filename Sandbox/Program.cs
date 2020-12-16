using SubstrateNetApi;
using System;
using System.Numerics;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine(new BigInteger(Utils.HexToByteArray("518fd3f9a8503a4f7e00000000000000")));

            Console.WriteLine(new BigInteger(Utils.HexToByteArray("00c040b571e803")));

            Console.WriteLine(new BigInteger(Utils.HexToByteArray("0000c16ff28623")));
        }
    }
}
