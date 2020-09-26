using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.MetaDataModel.Calls;
using SubstrateNetApi.MetaDataModel.Values;
using SubstrateNetApi.TypeConverters;

namespace SubstrateNetApiTests
{
    public class ValueTests
    {
        [Test]
        public void EncodingTest()
        {
            var callArguments = new GenericExtrinsicCall("","", new AccountId("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d"), new Balance(100));
            Assert.AreEqual("D43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D9101", Utils.Bytes2HexString(callArguments.Encode(), Utils.HexStringFormat.PURE));
        }
    }
}
