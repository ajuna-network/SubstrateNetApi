using System;
using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using SubstrateNetApi.Model.Types.Struct;
using SubstrateNetApi.TypeConverters;

namespace SubstrateNetApiTests
{
    public class PrimitiveTypesTest
    {
        [Test]
        public void PrimBoolTest()
        {
            var primFalse = new SubstrateNetApi.Model.Types.Primitive.Bool();
            primFalse.Create("0x00");
            Assert.AreEqual(false, primFalse.Value);

            var primTrue = new SubstrateNetApi.Model.Types.Primitive.Bool();
            primTrue.Create("0x01");
            Assert.AreEqual(true, primTrue.Value);
        }

        [Test]
        public void PrimCharTest()
        {
            Assert.Fail("Not implemented");
        }

        [Test]
        public void PrimU8Test()
        {
            var prim = new SubstrateNetApi.Model.Types.Primitive.U8();
            prim.Create("0x45");
            Assert.AreEqual(69, prim.Value);
        }

        [Test]
        public void PrimU16Test()
        {
            var prim = new SubstrateNetApi.Model.Types.Primitive.U16();
            prim.Create("0x2a00");
            Assert.AreEqual(42, prim.Value);
        }

        [Test]
        public void PrimU32Test()
        {
            var prim = new SubstrateNetApi.Model.Types.Primitive.U32();
            prim.Create("0xffffff00");
            Assert.AreEqual(16777215, prim.Value);
        }

        [Test]
        public void PrimU64Test()
        {
            Assert.Fail("Not implemented");
        }

        [Test]
        public void PrimU128Test()
        {
            Assert.Fail("Not implemented");
        }

        [Test]
        public void PrimU256Test()
        {
            Assert.Fail("Not implemented");
        }

        [Test]
        public void PrimI8Test()
        {
            Assert.Fail("Not implemented");
        }

        [Test]
        public void PrimI16Test()
        {
            Assert.Fail("Not implemented");
        }

        [Test]
        public void PrimI32Test()
        {
            Assert.Fail("Not implemented");
        }

        [Test]
        public void PrimI64Test()
        {
            Assert.Fail("Not implemented");
        }

        [Test]
        public void PrimI128Test()
        {
            Assert.Fail("Not implemented");
        }

        [Test]
        public void PrimI256Test()
        {
            Assert.Fail("Not implemented");
        }

        [Test]
        public void PrimVecTest()
        {
            // vec u16 test
            var vecUInt16 = new uint[] { 4, 8, 15, 16, 23, 42 };
            var primVec = new BaseVec<SubstrateNetApi.Model.Types.Primitive.U16>();
            primVec.Create("0x18040008000f00100017002a00");
            for (int i = 0; i < vecUInt16.Length; i++)
            {
                Assert.AreEqual(vecUInt16[i], primVec.Value[i].Value);
            }
        }

        [Test]
        public void PrimStrTest()
        {
            // str test
            var vecChar = new char[] { 'b', 'a', 'n', 'a', 'n', 'e' };           
            var primVec = new SubstrateNetApi.Model.Types.Primitive.Str();
            primVec.Create("0x1862616e616e65");
            for (int i = 0; i < vecChar.Length; i++)
            {
                Assert.AreEqual(String.Join("", vecChar[i]), primVec.Value);
            }
        }
    }
}