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

        [Test]
        public void BalanceTest()
        {
            var balance1 = new Balance("518fd3f9a8503a4f7e00000000000000");
            Assert.AreEqual("2329998717451725147985", balance1.Value.ToString());

            var balance2 = new Balance(Utils.HexToByteArray("518fd3f9a8503a4f7e00000000000000"));
            Assert.AreEqual("2329998717451725147985", balance2.Value.ToString());
        }

        [Test]
        public void AccountDataTest()
        {
            var account = new AccountData(Utils.HexToByteArray("518fd3f9a8503a4f7e0000000000000000c040b571e8030000000000000000000000c16ff2862300000000000000000000000000000000000000000000000000"));
            Assert.AreEqual("2329998717451725147985", account.Free.Value.ToString());
            Assert.AreEqual("1100000000000000", account.Reserved.ToString());
            Assert.AreEqual("0", account.FeeFrozen.ToString());
            Assert.AreEqual("10000000000000000", account.MiscFrozen.ToString());
        }

        [Test]
        public void AccountInfoTest()
        {
            var accountInfo = new AccountInfo(Utils.HexToByteArray("2200000001000000518fd3f9a8503a4f7e0000000000000000c040b571e8030000000000000000000000c16ff2862300000000000000000000000000000000000000000000000000"));
            Assert.AreEqual(34, accountInfo.Nonce);
            Assert.AreEqual(1, accountInfo.RefCount);
            Assert.AreEqual("2329998717451725147985", accountInfo.AccountData.Free.Value.ToString());
            Assert.AreEqual("1100000000000000", accountInfo.AccountData.Reserved.ToString());
            Assert.AreEqual("0", accountInfo.AccountData.FeeFrozen.ToString());
            Assert.AreEqual("10000000000000000", accountInfo.AccountData.MiscFrozen.ToString());
        }
    }
}
