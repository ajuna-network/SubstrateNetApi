using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Struct;
using SubstrateNetApi.TypeConverters;

namespace SubstrateNetApiTests
{
    public class TypeConverterTests
    {
        [Test]
        public void U16TypeConverterTest()
        {
            var tc = new GenericTypeConverter<U16>();
            var actual = (U16) tc.Create("0xf142");
            Assert.AreEqual((short) 0x42f1, actual.Value);
        }

        [Test]
        public void U32TypeConverterTest()
        {
            var tc = new GenericTypeConverter<U32>();
            var actual = (U32) tc.Create("0xf142bca0");
            Assert.AreEqual(0xa0bc42f1, actual.Value);
        }

        [Test]
        public void U64TypeConverterTest()
        {
            var tc = new GenericTypeConverter<U64>();
            var actual = (U64) tc.Create("0x01de99faf142bca0");
            Assert.AreEqual(0xa0bc42f1fa99de01, actual.Value);
        }

        [Test]
        public void VecU8TypeConverterTest()
        {
            var tc = new GenericTypeConverter<Vec<U8>>();
            var actual = (Vec<U8>) tc.Create("0x200101020304050607");
            Assert.AreEqual(1, actual.Value[0].Value);
            Assert.AreEqual(1, actual.Value[1].Value);
            Assert.AreEqual(2, actual.Value[2].Value);
            Assert.AreEqual(3, actual.Value[3].Value);
            Assert.AreEqual(4, actual.Value[4].Value);
            Assert.AreEqual(5, actual.Value[5].Value);
            Assert.AreEqual(6, actual.Value[6].Value);
            Assert.AreEqual(7, actual.Value[7].Value);
        }

        [Test]
        public void VecU8EncodingTest()
        {
            var tc = new GenericTypeConverter<Vec<U8>>();
            var actual = (Vec<U8>)tc.Create("0x200101020304050607");

            Assert.AreEqual(actual.Bytes, actual.Encode());

            var t1 = new U8(); t1.Create(actual.Value[0].Value);
            var t2 = new U8(); t2.Create(actual.Value[1].Value);
            var t3 = new U8(); t3.Create(actual.Value[2].Value);
            var t4 = new U8(); t4.Create(actual.Value[3].Value);
            var t5 = new U8(); t5.Create(actual.Value[4].Value);
            var t6 = new U8(); t6.Create(actual.Value[5].Value);
            var t7 = new U8(); t7.Create(actual.Value[6].Value);
            var t8 = new U8(); t8.Create(actual.Value[7].Value);

            List <U8> list = new List<U8>()
            {
                t1,t2,t3,t4,t5,t6,t7,t8
            };

            var vecU8 = new Vec<U8>();
            vecU8.Create(list);

            Assert.AreEqual("0x200101020304050607", Utils.Bytes2HexString(vecU8.Bytes));

        }

    }
}