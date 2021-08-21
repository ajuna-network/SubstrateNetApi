using System;
using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Enum;
using SubstrateNetApi.Model.Types.Struct;
using SubstrateNetApi.TypeConverters;

namespace SubstrateNetApiTests
{
    public class TypesMapperTest
    {
        [Test]
        public void TypeUtilTest()
        {
            Type type = null;
            Assert.IsTrue(TypeUtil.TryGetType("U8", out type));
            Assert.AreEqual("SubstrateNetApi.Model.Types.Base.U8", type.FullName);

            Assert.IsTrue(TypeUtil.TryGetType("BalanceStatus", out type));
            Assert.AreEqual("SubstrateNetApi.Model.Types.Enum.BalanceStatus", type.FullName);

            Assert.IsTrue(TypeUtil.TryGetType("AccountData", out type));
            Assert.AreEqual("SubstrateNetApi.Model.Types.Struct.AccountData", type.FullName);

            Assert.IsTrue(TypeUtil.TryGetType("CustomU32", out type));
            Assert.AreEqual("SubstrateNetApi.Model.Types.Custom.CustomU32", type.FullName);
        }
    }
}