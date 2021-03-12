using System;
using System.Text;
using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.Model.Types.Struct;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApiTests
{
    public class CustomTypes
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void MogwaiStructTest()
        {
            var mogwaiStructStr = "0xbb45ac2c375db3d5239ea8cc0c08bd75bea17abe903493e88a2c8f9fafe0daa1bb45ac2c375db3d5239ea8cc0c08bd75bea17abe903493e88a2c8f9fafe0daa161020100000000000000000000000000000000000000000000000000";
            var mogwaiStructA = new MogwaiStruct();
            mogwaiStructA.Create(mogwaiStructStr);


            var mogwaiStructB = new MogwaiStruct();

            var id = new Hash();
            id.Create(mogwaiStructA.Id.Value);

            var dna = new Hash();
            dna.Create(mogwaiStructA.Dna.Value);

            var genesis = new BlockNumber();
            genesis.Create(mogwaiStructA.Genesis.Value);

            var price = new Balance();
            price.Create(mogwaiStructA.Price.Value);

            var gen = new U64();
            gen.Create(mogwaiStructA.Gen.Value);

            mogwaiStructB.Create(id, dna, genesis, price, gen);

            Assert.AreEqual(mogwaiStructB.Id.Value, mogwaiStructA.Id.Value);
            Assert.AreEqual(mogwaiStructB.Dna.Value, mogwaiStructA.Dna.Value);
            Assert.AreEqual(mogwaiStructB.Genesis.Value, mogwaiStructA.Genesis.Value);
            Assert.AreEqual(mogwaiStructB.Price.Value, mogwaiStructA.Price.Value);
            Assert.AreEqual(mogwaiStructB.Gen.Value, mogwaiStructA.Gen.Value);
        }

        [Test]
        public void MogwaiBiosTest()
        {
            var mogwaiBiosStr = "0x0b1b9f0f79a9e3971baf6188ed98623284f1c3bb275883602164b7097789523f000000000881f7a106cc0f747e85deedaf2946297ebacbe008a7a4887c334448fcc4c4888c00000000000000000000000000000000010426df010000";
            var mogwaiBiosA = new MogwaiBios();
            mogwaiBiosA.Create(mogwaiBiosStr);

            var mogwaiBiosB = new MogwaiBios();

            var id = new Hash();
            id.Create(mogwaiBiosA.Id.Value);
            Assert.AreEqual("0x0B1B9F0F79A9E3971BAF6188ED98623284F1C3BB275883602164B7097789523F", mogwaiBiosA.Id.Value);

            var state = new U32();
            state.Create(mogwaiBiosA.State.Value);
            Assert.AreEqual(0, mogwaiBiosA.State.Value);

            var metaXy = new Vec<U8Arr16>();
            metaXy.Create(mogwaiBiosA.MetaXy.Bytes);

            Assert.AreEqual(2, mogwaiBiosA.MetaXy.Value.Count);
            Assert.AreEqual("0x0881F7A106CC0F747E85DEEDAF2946297EBACBE008A7A4887C334448FCC4C4888C", Utils.Bytes2HexString(mogwaiBiosA.MetaXy.Bytes));
            Assert.AreEqual("0x81F7A106CC0F747E85DEEDAF2946297E", Utils.Bytes2HexString(metaXy.Value[0].Bytes));

            var intrinsic = new Balance();
            intrinsic.Create(mogwaiBiosA.Intrinsic.Value);

            var level = new U8();
            level.Create(mogwaiBiosA.Level.Value);

            var phases = new Vec<BlockNumber>();
            phases.Create(mogwaiBiosA.Phases.Bytes);

            var adaptations = new Vec<Hash>();
            adaptations.Create(mogwaiBiosA.Adaptations.Bytes);

            mogwaiBiosB.Create(id, state, metaXy, intrinsic, level, phases, adaptations);

            Assert.AreEqual(mogwaiBiosB.Id.Value, mogwaiBiosA.Id.Value);
            Assert.AreEqual(mogwaiBiosB.State.Value, mogwaiBiosA.State.Value);
            Assert.AreEqual(mogwaiBiosB.MetaXy.Value[0].Bytes, mogwaiBiosA.MetaXy.Value[0].Bytes);
            Assert.AreEqual(mogwaiBiosB.Intrinsic.Value, mogwaiBiosA.Intrinsic.Value);
            Assert.AreEqual(mogwaiBiosB.Level.Value, mogwaiBiosA.Level.Value);
            Assert.AreEqual(mogwaiBiosB.Phases.Value[0].Bytes, mogwaiBiosA.Phases.Value[0].Bytes);
            Assert.AreEqual(mogwaiBiosB.Adaptations.Value, mogwaiBiosA.Adaptations.Value);
        }
    }
}