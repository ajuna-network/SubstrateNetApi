using NUnit.Framework;
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
        public void MogwaiStructTypeConverterTest()
        {
            var tc = new GenericTypeConverter<MogwaiStruct>();
            var actual =
                tc.Create(
                    "0x17E26CA749780270EEC18507AB3C03854E75E264DB13EC1F90314C3AF02CCDF817E26CA749780270EEC18507AB3C03854E75E264DB13EC1F90314C3AF02CCDF87D320000000000000000000000000000000000000000000000000000");
            Assert.IsTrue(actual is MogwaiStruct);
            var hash = new Hash();
            hash.Create("0x17E26CA749780270EEC18507AB3C03854E75E264DB13EC1F90314C3AF02CCDF8");
            Assert.AreEqual(hash.Value, (actual as MogwaiStruct).Id.Value);
        }
    }
}