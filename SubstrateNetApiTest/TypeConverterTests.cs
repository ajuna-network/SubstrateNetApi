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
                    "0xad35415cb5b574819c8521b9192fffda772c0770fed9a55494293b2d728f104cad35415cb5b574819c8521b9192fffda772c0770fed9a55494293b2d728f104c000000000000000000000000000000000000000000000000");
            Assert.IsTrue(actual is MogwaiStruct);
            Assert.AreEqual(new Hash("0xAD35415CB5B574819C8521B9192FFFDA772C0770FED9A55494293B2D728F104C").HexString, (actual as MogwaiStruct).Id.HexString);
        }
    }
}
