using System;
using NUnit.Framework;
using SubstrateNetApi;
using System.Numerics;

namespace SubstrateNetApiTests
{
    public class CompactIntegerTests
    {
        [Test]
        public void AdditionTest()
        {
            CompactInteger ci = 1;
            ci += (byte)1;
            Assert.AreEqual((CompactInteger)2, ci);
            ci += (uint)1;
            Assert.AreEqual((CompactInteger)3, ci);
            ci += (int)1;
            Assert.AreEqual((CompactInteger)4, ci);
            ci += (CompactInteger)1;
            Assert.AreEqual((CompactInteger)5, ci);
            ci += (sbyte)1;
            Assert.AreEqual((CompactInteger)6, ci);
            ci += (long)1;
            Assert.AreEqual((CompactInteger)7, ci);
            ci += (short)1;
            Assert.AreEqual((CompactInteger)8, ci);
            ci += (ulong)1;
            Assert.AreEqual((CompactInteger)9, ci);
            ci += (ushort)1;
            Assert.AreEqual((CompactInteger)10, ci);
            ci += BigInteger.One;
            Assert.AreEqual((CompactInteger)11, ci);
        }

        [Test]
        public void MultiplyTest()
        {
            CompactInteger ci = 1;
            ci *= 2;
            Assert.AreEqual((CompactInteger)2, ci);
            ci *= (byte)2;
            Assert.AreEqual((CompactInteger)4, ci);
            ci *= (uint)2;
            Assert.AreEqual((CompactInteger)8, ci);
            ci *= (CompactInteger)2;
            Assert.AreEqual((CompactInteger)16, ci);
            ci *= (sbyte)2;
            Assert.AreEqual((CompactInteger)32, ci);
            ci *= (long)2;
            Assert.AreEqual((CompactInteger)64, ci);
            ci *= (short)2;
            Assert.AreEqual((CompactInteger)128, ci);
            ci *= (ulong)2;
            Assert.AreEqual((CompactInteger)256, ci);
            ci *= (ushort)2;
            Assert.AreEqual((CompactInteger)512, ci);
            ci *= (BigInteger)2;
            Assert.AreEqual((CompactInteger)1024, ci);
        }

        [Test]
        public void SubtractionTest()
        {
            CompactInteger ci = 11;
            ci -= (byte)1;
            Assert.AreEqual((CompactInteger)10, ci);
            ci -= (uint)1;
            Assert.AreEqual((CompactInteger)9, ci);
            ci -= (int)1;
            Assert.AreEqual((CompactInteger)8, ci);
            ci -= (CompactInteger)1;
            Assert.AreEqual((CompactInteger)7, ci);
            ci -= (sbyte)1;
            Assert.AreEqual((CompactInteger)6, ci);
            ci -= (long)1;
            Assert.AreEqual((CompactInteger)5, ci);
            ci -= (short)1;
            Assert.AreEqual((CompactInteger)4, ci);
            ci -= (ulong)1;
            Assert.AreEqual((CompactInteger)3, ci);
            ci -= (ushort)1;
            Assert.AreEqual((CompactInteger)2, ci);
            ci -= BigInteger.One;
            Assert.AreEqual((CompactInteger)1, ci);
        }

        [Test]
        public void DivideTest()
        {
            CompactInteger ci = 1024;
            ci /= 2;
            Assert.AreEqual((CompactInteger)512, ci);
            ci /= (byte)2;
            Assert.AreEqual((CompactInteger)256, ci);
            ci /= (uint)2;
            Assert.AreEqual((CompactInteger)128, ci);
            ci /= (CompactInteger)2;
            Assert.AreEqual((CompactInteger)64, ci);
            ci /= (sbyte)2;
            Assert.AreEqual((CompactInteger)32, ci);
            ci /= (long)2;
            Assert.AreEqual((CompactInteger)16, ci);
            ci /= (short)2;
            Assert.AreEqual((CompactInteger)8, ci);
            ci /= (ulong)2;
            Assert.AreEqual((CompactInteger)4, ci);
            ci /= (ushort)2;
            Assert.AreEqual((CompactInteger)2, ci);
            ci /= (BigInteger)2;
            Assert.AreEqual((CompactInteger)1, ci);
        }

        [Test]
        public void EqualityTest()
        {
            CompactInteger ci = 1;
            Assert.IsTrue(ci == 1);
            Assert.IsFalse(ci == 2);
            Assert.IsTrue(ci != 2);
            Assert.IsFalse(ci != 1);
            Assert.IsTrue(ci >= 0);
            Assert.IsTrue(ci >= 1);
            Assert.IsFalse(ci >= 2);
            Assert.IsFalse(ci <= 0);
            Assert.IsTrue(ci <= 1);
            Assert.IsTrue(ci <= 2);
            Assert.IsTrue(ci.Equals((CompactInteger)1));
            Assert.IsFalse(ci.Equals(1));
        }

        [Test]
        public void ConversionTest()
        {
            CompactInteger ci = 1;
            Assert.IsTrue(1 == (int)ci);
            Assert.IsTrue(1 == (uint)ci);
            Assert.IsTrue(1 == (sbyte)ci);
            Assert.IsTrue(1 == (byte)ci);
            Assert.IsTrue(1 == (long)ci);
            Assert.IsTrue(1 == (ulong)ci);
            Assert.IsTrue(1 == (short)ci);
            Assert.IsTrue(1 == (ushort)ci);
            Assert.IsTrue(1 == (BigInteger)ci);
        }

        [Test]
        public void ToStringTest()
        {
            CompactInteger ci = 1;
            Assert.AreEqual("1", ci.ToString());
        }

        [Test]
        public void GetHashCodeTest()
        {
            CompactInteger ci = 1;
            Assert.AreEqual(BigInteger.One.GetHashCode(), ci.GetHashCode());
        }

        [Test]
        public void EncodeDecodeTest()
        {
            ulong[] array = new UInt64[] { 0, 1, 255, 256, 65535, 4294967295, 4294967296, 8000000000000000000, 18446744073709551615 };
            foreach (var t in array)
            {
                CompactInteger v = new CompactInteger(t);
                Assert.AreEqual(v, CompactInteger.Decode(v.Encode()));
            }
        }

        [Test]
        public void EncodeDecodeTest2()
        {
            for (int i = 0; i < 1000000; i++)
            {
                CompactInteger c = i;
                Assert.AreEqual(c, CompactInteger.Decode(c.Encode()));
            }
        }
    }
}
