using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.Model.Calls;
using SubstrateNetApi.Model.Rpc;
using SubstrateNetApi.Model.Types;

namespace SubstrateNetApiTests
{
    public class ValueTests
    {
        [Test]
        public void EncodingTest()
        {
            var accountId = new AccountId();
            accountId.Create("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d");

            var balance = new Balance();
            balance.Create(100);

            var callArguments = new GenericExtrinsicCall("", "", accountId, balance);
            switch (Constants.AddressVersion)
            {
                case 0:
                    Assert.AreEqual("D43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D9101", Utils.Bytes2HexString(callArguments.Encode(), Utils.HexStringFormat.PURE));
                    break;
                case 1:
                    Assert.AreEqual("FFD43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D9101", Utils.Bytes2HexString(callArguments.Encode(), Utils.HexStringFormat.PURE));
                    break;
                case 2:
                    Assert.AreEqual("00D43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D9101", Utils.Bytes2HexString(callArguments.Encode(), Utils.HexStringFormat.PURE));
                    break;
            }
            
        }

        [Test]
        public void BalanceTest()
        {
            var balance1 = new Balance();
            balance1.Create("0x518fd3f9a8503a4f7e00000000000000");
            Assert.AreEqual("2329998717451725147985", balance1.Value.ToString());

            var balance2 = new Balance();
            balance2.Create(Utils.HexToByteArray("518fd3f9a8503a4f7e00000000000000"));
            Assert.AreEqual("2329998717451725147985", balance2.Value.ToString());
        }

        [Test]
        public void AccountDataTest()
        {
            var account = new AccountData(Utils.HexToByteArray("518fd3f9a8503a4f7e0000000000000000c040b571e8030000000000000000000000c16ff2862300000000000000000000000000000000000000000000000000"));
            Assert.AreEqual("2329998717451725147985", account.Free.Value.ToString());
            Assert.AreEqual("1100000000000000", account.Reserved.Value.ToString());
            Assert.AreEqual("0", account.FeeFrozen.Value.ToString());
            Assert.AreEqual("10000000000000000", account.MiscFrozen.Value.ToString());
        }

        [Test]
        public void AccountInfoTest()
        {
            var accountInfo = new AccountInfo(Utils.HexToByteArray("0500000000000000010000001d58857016a4755a6c00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"));
            Assert.AreEqual(5, accountInfo.Nonce.Value);
            Assert.AreEqual(0, accountInfo.Consumers.Value);
            Assert.AreEqual(1, accountInfo.Providers.Value);
            Assert.AreEqual("1998766656412604258333", accountInfo.AccountData.Free.Value.ToString());
            Assert.AreEqual("0", accountInfo.AccountData.Reserved.Value.ToString());
            Assert.AreEqual("0", accountInfo.AccountData.FeeFrozen.Value.ToString());
            Assert.AreEqual("0", accountInfo.AccountData.MiscFrozen.Value.ToString());
        }
    }
}
