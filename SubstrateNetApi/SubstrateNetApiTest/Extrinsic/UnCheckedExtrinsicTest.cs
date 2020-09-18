using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.MetaDataModel;
using SubstrateNetApi.MetaDataModel.Extrinsic;
using SubstrateNetApi.MetaDataModel.Values;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SubstrateNetApiTests.Extrinsic
{
    public class UnCheckedExtrinsicTest
    {
        private Random _random;

        [OneTimeSetUp]
        public void Setup()
        {
            _random = new Random();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }

        [Test]
        public void BalanceTransferMockedTest1()
        {
            byte publicKeyType = 0x00;
            // Utils.HexToByteArray("0x9EFFC1668CA381C242885516EC9FA2B19C67B6684C02A8A3237B6862E5C8CD7E");
            byte[] publicKey = Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM");
            CompactInteger nonce = 1;
            byte moduleIndex = 0x04;
            byte callIndex = 0x00;
            // Utils.HexToByteArray("0x9EFFC1668CA381C242885516EC9FA2B19C67B6684C02A8A3237B6862E5C8CD7E");
            byte[] destPublicKey = Utils.GetPublicKeyFrom("5FfBQ3kwXrbdyoqLPvcXRp7ikWydXawpNs2Ceu3WwFdhZ8W4"); 
            CompactInteger amount = 987456321;
            byte[] parameters = destPublicKey.Concat(amount.Encode()).ToArray();
            byte[] genesisHash = null;
            byte[] currentBlockHash = null;
            ulong currentBlockNumber = 47;
            CompactInteger tip = 1234;

            // mocked signature
            byte[] signature = Utils.HexToByteArray("0x14AE74DD7964365038EBA44F51C347B9C7070231D56E38EF1024457EBDC6DC03D20226243B1B2731DF6FD80F7170643221BD8BF8D06215D4BFEAC68A2C9D2305");

            var uncheckedExtrinsic = new UnCheckedExtrinsic(true, publicKeyType, publicKey, nonce, moduleIndex, callIndex, parameters, genesisHash, currentBlockHash, currentBlockNumber, tip);
            
            string balanceTransfer = "0x350284278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e0014ae74dd7964365038eba44f51c347b9c7070231d56e38ef1024457ebdc6dc03d20226243b1b2731df6fd80f7170643221bd8bf8d06215d4bfeac68a2c9d2305f50204491304009effc1668ca381c242885516ec9fa2b19c67b6684c02a8a3237b6862e5c8cd7e068d6deb";

            Assert.AreEqual(Utils.HexToByteArray(balanceTransfer), uncheckedExtrinsic.Serialize(signature));    
        }

        [Test]
        public void BalanceTransferMockedTest2()
        {
            //[
            //  {
            //    "signature": {
            //      "signer": "5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM",
            //      "signature": {
            //          "Ed25519": "0x97176876e9cb26b2449855a332efee478768b31e215ed025ce57dd8e6bdfa63df3adfcf29091eb53c326173a5aad23661e074d132ae804bbd91126cd19093302"
            //      },
            //      "era": {
            //          "MortalEra": "0x7500"
            //      },
            //      "nonce": 6,
            //      "tip": 1234
            //    },
            //    "method": {
            //    "callIndex": "0x0400",
            //      "args": {
            //        "dest": "5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY",
            //        "value": 7654321
            //      }
            //    }
            //  }
            //]
            byte publicKeyType = 0x00;
            byte[] publicKey = Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM");
            CompactInteger nonce = 6;
            byte moduleIndex = 0x04;
            byte callIndex = 0x00;
            byte[] destPublicKey = Utils.GetPublicKeyFrom("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY");
            CompactInteger amount = 7654321;
            byte[] parameters = destPublicKey.Concat(amount.Encode()).ToArray();
            byte[] genesisHash = Utils.HexToByteArray("0x9b443ea9cd42d9c3e0549757d029d28d03800631f9a9abf1d96d0c414b9aded9");
            byte[] currentBlockHash = Utils.HexToByteArray("0xcfa2f9c52f94bc50658735d0f18f72590c981fdc15657636a99c437553c53253"); ;
            ulong currentBlockNumber = 7;
            CompactInteger tip = 1234;

            // mocked signature
            byte[] signature = Utils.HexToByteArray("0x97176876e9cb26b2449855a332efee478768b31e215ed025ce57dd8e6bdfa63df3adfcf29091eb53c326173a5aad23661e074d132ae804bbd91126cd19093302");

            var uncheckedExtrinsic = new UnCheckedExtrinsic(true, publicKeyType, publicKey, nonce, moduleIndex, callIndex, parameters, genesisHash, currentBlockHash, currentBlockNumber, tip);

            string balanceTransfer = "0x350284278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e0097176876e9cb26b2449855a332efee478768b31e215ed025ce57dd8e6bdfa63df3adfcf29091eb53c326173a5aad23661e074d132ae804bbd91126cd1909330275001849130400d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27dc62ed301";

            Assert.AreEqual(Utils.HexToByteArray(balanceTransfer), uncheckedExtrinsic.Serialize(signature));

            var payload = uncheckedExtrinsic.GetPayload().Serialize();
        }

        [Test]
        public void BalanceTransferAliceTest()
        {
            //[
            //  {
            //    isSigned: true,
            //    method: {
            //      args: [
            //        5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM,
            //        4.321n Unit
            //      ],
            //      method: transfer,
            //      section: balances
            //    },
            //    era: {
            //      MortalEra: {
            //        period: 64,
            //        phase: 10
            //      }
            //    },
            //    nonce: 4,
            //    signature: 0x726ba1fab06d3e1bf6abfa0d5af85e25f2a970e11384162b7caf83935c58f769b6fef3b83a29ffd8d813a037d01cd6bcb21beaa88e9a18b3abe366b0458a8a82,
            //    signer: 5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY,
            //    tip: 1.234n Unit
            //  }
            //]
            // [{ "signature":{ "signer":"5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY","signature":{ "Sr25519":"0x726ba1fab06d3e1bf6abfa0d5af85e25f2a970e11384162b7caf83935c58f769b6fef3b83a29ffd8d813a037d01cd6bcb21beaa88e9a18b3abe366b0458a8a82"},"era":{ "MortalEra":"0xa500"},"nonce":4,"tip":1234},"method":{ "callIndex":"0x0400","args":{ "dest":"5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM","value":4321} } }]
            
            byte publicKeyType = 0x01;
            byte[] publicKey = Utils.GetPublicKeyFrom("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY");
            CompactInteger nonce = 4;
            byte moduleIndex = 0x04;
            byte callIndex = 0x00;
            byte[] destPublicKey = Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM");
            CompactInteger amount = 4321;
            byte[] parameters = destPublicKey.Concat(amount.Encode()).ToArray();
            string bb = Utils.Bytes2HexString(amount.Encode());
            byte[] genesisHash = Utils.HexToByteArray("0x9b443ea9cd42d9c3e0549757d029d28d03800631f9a9abf1d96d0c414b9aded9");
            byte[] currentBlockHash = Utils.HexToByteArray("0x27bf1e86b29c84ca5830c2bfeba545a7856dd0bc107d16325acc9ad440abac0c"); ;
            ulong currentBlockNumber = 10;
            CompactInteger tip = 1234;

            // mocked signature
            byte[] signature = Utils.HexToByteArray("0x726ba1fab06d3e1bf6abfa0d5af85e25f2a970e11384162b7caf83935c58f769b6fef3b83a29ffd8d813a037d01cd6bcb21beaa88e9a18b3abe366b0458a8a82");

            var uncheckedExtrinsic = new UnCheckedExtrinsic(true, publicKeyType, publicKey, nonce, moduleIndex, callIndex, parameters, genesisHash, currentBlockHash, currentBlockNumber, tip);

            string b = Utils.Bytes2HexString(uncheckedExtrinsic.Serialize(signature));

            string balanceTransfer = "0x2d0284d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d01726ba1fab06d3e1bf6abfa0d5af85e25f2a970e11384162b7caf83935c58f769b6fef3b83a29ffd8d813a037d01cd6bcb21beaa88e9a18b3abe366b0458a8a82a5001049130400278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e8543";

            Assert.AreEqual(Utils.HexToByteArray(balanceTransfer), uncheckedExtrinsic.Serialize(signature));

            var payload = uncheckedExtrinsic.GetPayload().Serialize();
        }
    }
}
