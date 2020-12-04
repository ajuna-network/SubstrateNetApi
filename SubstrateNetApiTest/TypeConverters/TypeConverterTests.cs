using NUnit.Framework;
using SubstrateNetApi.MetaDataModel.Values;
using SubstrateNetApi.TypeConverters;

namespace SubstrateNetApiTests
{
    public class TypeConverterTests
    {
        [Test]
        public void U16TypeConverterTest()
        {
            var tc = new U16TypeConverter();
            var actual = tc.Create("0xf142");
            Assert.AreEqual((short) 0x42f1, actual);
        }

        [Test]
        public void U32TypeConverterTest()
        {
            var tc = new U32TypeConverter();
            var actual = tc.Create("0xf142bca0");
            Assert.AreEqual(0xa0bc42f1, actual);
        }

        [Test]
        public void U64TypeConverterTest()
        {
            var tc = new U64TypeConverter();
            var actual = tc.Create("0x01de99faf142bca0");
            Assert.AreEqual(0xa0bc42f1fa99de01, actual);
        }

        [Test]
        public void MogwaiStructTypeConverterTest()
        {
            var tc = new MogwaiStructTypeConverter();
            var actual =
                tc.Create(
                    "0x17E26CA749780270EEC18507AB3C03854E75E264DB13EC1F90314C3AF02CCDF817E26CA749780270EEC18507AB3C03854E75E264DB13EC1F90314C3AF02CCDF87D320000000000000000000000000000000000000000000000000000");
            Assert.IsTrue(actual is MogwaiStruct);
            Assert.AreEqual(new Hash("0x17E26CA749780270EEC18507AB3C03854E75E264DB13EC1F90314C3AF02CCDF8").HexString, (actual as MogwaiStruct).Id.HexString);
        }

    }
}
