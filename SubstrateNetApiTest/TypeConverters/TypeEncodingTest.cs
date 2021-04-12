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
    public class TypeEncodingTest
    {
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

        [Test]
        public void EnumEncodingTest()
        {
            var dispatchClass1 = new EnumType<DispatchClass>();
            var dispatchClass2 = new EnumType<DispatchClass>();


            dispatchClass1.Create("0x00");
            dispatchClass2.Create(DispatchClass.Normal);

            Assert.AreEqual(DispatchClass.Normal, dispatchClass1.Value);
            Assert.AreEqual(dispatchClass2.Value, dispatchClass1.Value);

            dispatchClass1.Create("0x01");
            dispatchClass2.Create(DispatchClass.Operational);

            Assert.AreEqual(DispatchClass.Operational, dispatchClass1.Value);
            Assert.AreEqual(dispatchClass2.Value, dispatchClass1.Value);

            dispatchClass1.Create("0x02");
            dispatchClass2.Create(DispatchClass.Mandatory);

            Assert.AreEqual(DispatchClass.Mandatory, dispatchClass1.Value);
            Assert.AreEqual(dispatchClass2.Value, dispatchClass1.Value);



        }

    }
}