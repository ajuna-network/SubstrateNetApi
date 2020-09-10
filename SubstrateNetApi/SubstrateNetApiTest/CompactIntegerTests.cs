using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NUnit.Framework;
using SubstrateNetApi;

namespace SubstrateNetApiTests
{
    public class CompactIntegerTests
    {
        [Test]
        public void AdditionTest()
        {
            CompactInteger ci = BigInteger.One;
            ci += (byte)1;
            Assert.AreEqual((BigInteger)2, (BigInteger)ci);
            ci += (uint)1;
            Assert.AreEqual((BigInteger)3, (BigInteger)ci);
            ci += 1;
            Assert.AreEqual((BigInteger)4, (BigInteger)ci);
            ci += (CompactInteger)1;
            Assert.AreEqual((BigInteger)5, (BigInteger)ci);
        }

        [Test]
        public void MultiplyTest()
        {
            CompactInteger ci = BigInteger.One;
            ci *= 2;
            Assert.AreEqual((BigInteger)2, (BigInteger)ci);
            ci *= (byte)2;
            Assert.AreEqual((BigInteger)4, (BigInteger)ci);
            ci *= (uint)2;
            Assert.AreEqual((BigInteger)8, (BigInteger)ci);
            ci *= (CompactInteger)2;
            Assert.AreEqual((BigInteger)16, (BigInteger)ci);
        }
    }
}
