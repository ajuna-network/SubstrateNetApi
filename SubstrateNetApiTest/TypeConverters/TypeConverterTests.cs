using System.Numerics;
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

        [Test]
        public void MogwaiBiosTypeConverterTest()
        {
            var tc = new GenericTypeConverter<MogwaiBios>();
            var actual =
                tc.Create("0xe2d3965c287d92c7cf45dc3ff832e8060607cc8eb7f85ae598b4030338f59587" +
                          "00000000" +
                          "08" +
                          "f27a7c4788ef094b6e4d8d6eaa0732c4" +
                          "844cbb8fb0ab077a7aba7f7a7baba77a" +
                          "000000000000000000000000" +
                          "0000000001" +
                          "040c1f020000");
            Assert.IsTrue(actual is MogwaiBios);
            var mogwaiBios = actual as MogwaiBios;
            Assert.AreEqual("0xe2d3965c287d92c7cf45dc3ff832e8060607cc8eb7f85ae598b4030338f59587",
                mogwaiBios.Id.Value.ToLower());
            Assert.AreEqual(0, mogwaiBios.State.Value);
            Assert.AreEqual("0xf27a7c4788ef094b6e4d8d6eaa0732c4", 
                (mogwaiBios.MetaXy.Value[0].Value as string).ToLower());
            Assert.AreEqual("0x844cbb8fb0ab077a7aba7f7a7baba77a", 
                (mogwaiBios.MetaXy.Value[1].Value as string).ToLower());
            Assert.AreEqual(new BigInteger(0), mogwaiBios.Intrinsic.Value);
            Assert.AreEqual(1, mogwaiBios.Level.Value);
            Assert.AreEqual(139020, mogwaiBios.Phases.Value[0].Value);
            Assert.AreEqual(0, mogwaiBios.Adaptations.Value.Count);
        }
    }
}