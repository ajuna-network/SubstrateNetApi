using System;
using System.Text;
using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.Model.Types.Struct;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.Model.Types.Enum;
using SubstrateNetApi.TypeConverters;
using System.Numerics;

namespace SubstrateNetApiTests
{
    public class CustomTypeConverters
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void MogwaiStructTypeConverterTest()
        {
            var tc = new GenericTypeConverter<MogwaiStruct>();
            var actual =
                tc.Create(
                    "0x89ab510f57802886c16922685a376edb536f762584dda569cda67381c4e4dec889ab510f57802886c16922685a376edb536f762584dda569cda67381c4e4dec871000000000000000000000000000000000000000000000000");
            Assert.IsTrue(actual is MogwaiStruct);
            var hash = new Hash();
            hash.Create("0x89ab510f57802886c16922685a376edb536f762584dda569cda67381c4e4dec8");
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