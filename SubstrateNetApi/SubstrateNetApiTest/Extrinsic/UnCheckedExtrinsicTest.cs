using NUnit.Framework;
using Schnorrkel;
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

            Assert.AreEqual(Utils.HexToByteArray(balanceTransfer), uncheckedExtrinsic.Encode(signature));
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

            Assert.AreEqual(Utils.HexToByteArray(balanceTransfer), uncheckedExtrinsic.Encode(signature));

            var payload = uncheckedExtrinsic.GetPayload().Encode();
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

            byte[] privatKey = Utils.HexToByteArray("0x33A6F3093F158A7109F679410BEF1A0C54168145E0CECB4DF006C1C2FFFB1F09925A225D97AA00682D6A59B95B18780C10D7032336E88F3442B42361F4A66011");

            byte publicKeyType = 0x01;
            byte[] publicKey = Utils.GetPublicKeyFrom("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY"); // Alice
            CompactInteger nonce = 4;
            byte moduleIndex = 0x04;
            byte callIndex = 0x00;

            byte[] destPublicKey = Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM");
            CompactInteger amount = 4321;
            byte[] parameters = destPublicKey.Concat(amount.Encode()).ToArray();

            byte[] genesisHash = Utils.HexToByteArray("0x9b443ea9cd42d9c3e0549757d029d28d03800631f9a9abf1d96d0c414b9aded9");
            byte[] currentBlockHash = Utils.HexToByteArray("0x27bf1e86b29c84ca5830c2bfeba545a7856dd0bc107d16325acc9ad440abac0c"); ;
            ulong currentBlockNumber = 10;
            CompactInteger tip = 1234;

            // mocked signature
            byte[] signature = Utils.HexToByteArray("0x726ba1fab06d3e1bf6abfa0d5af85e25f2a970e11384162b7caf83935c58f769b6fef3b83a29ffd8d813a037d01cd6bcb21beaa88e9a18b3abe366b0458a8a82");

            var uncheckedExtrinsic = new UnCheckedExtrinsic(true, publicKeyType, publicKey, nonce, moduleIndex, callIndex, parameters, genesisHash, currentBlockHash, currentBlockNumber, tip);

            string balanceTransfer = "0x2d0284d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d01726ba1fab06d3e1bf6abfa0d5af85e25f2a970e11384162b7caf83935c58f769b6fef3b83a29ffd8d813a037d01cd6bcb21beaa88e9a18b3abe366b0458a8a82a5001049130400278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e8543";

            Assert.AreEqual(Utils.HexToByteArray(balanceTransfer), uncheckedExtrinsic.Encode(signature));

            var payload = uncheckedExtrinsic.GetPayload().Encode();

            var simpleSign = Sr25519v091.SignSimple(publicKey, privatKey, payload);

            Assert.True(Sr25519v091.Verify(simpleSign, publicKey, payload));

            //Assert.AreEqual(signature, simpleSign);

        }

        [Test]
        public void DmogCreateImmortalAliceTest()
        {
            byte[] privatKey = Utils.HexToByteArray("0x33A6F3093F158A7109F679410BEF1A0C54168145E0CECB4DF006C1C2FFFB1F09925A225D97AA00682D6A59B95B18780C10D7032336E88F3442B42361F4A66011");

            byte publicKeyType = 0x01;
            byte[] publicKey = Utils.GetPublicKeyFrom("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY"); // Alice
            CompactInteger nonce = 5;
            byte moduleIndex = 0x06;
            byte callIndex = 0x02;

            byte[] parameters = new byte[0];

            byte[] genesisHash = Utils.HexToByteArray("0x9b443ea9cd42d9c3e0549757d029d28d03800631f9a9abf1d96d0c414b9aded9");
            byte[] currentBlockHash = Utils.HexToByteArray("0x9b443ea9cd42d9c3e0549757d029d28d03800631f9a9abf1d96d0c414b9aded9"); ;
            ulong currentBlockNumber = 0;
            CompactInteger tip = 0;

            // mocked signature
            byte[] signature = Utils.HexToByteArray("0xB8FB3FE1B723B69ED2011E5E3B168F202DFAE3853C81D5617DD35A60C29F1C4B49B95DCF5631CCA678837BC1B347DD1C20161E12512E16CED78A9592DEECDA8C");

            var uncheckedExtrinsic = new UnCheckedExtrinsic(true, publicKeyType, publicKey, nonce, moduleIndex, callIndex, parameters, genesisHash, currentBlockHash, currentBlockNumber, tip);

            string dmogCreateImmortal = "0x9d0184d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d01b8fb3fe1b723b69ed2011e5e3b168f202dfae3853c81d5617dd35a60c29f1c4b49b95dcf5631cca678837bc1b347dd1c20161e12512e16ced78a9592deecda8c0014000602";

            Assert.AreEqual(Utils.HexToByteArray(dmogCreateImmortal), uncheckedExtrinsic.Encode(signature));

            var payload = uncheckedExtrinsic.GetPayload().Encode();
            var payloadStr = Utils.Bytes2HexString(payload);

            if (payload.Length > 256)
            {
                payload = HashExtension.Blake2(payload, 256);
            }

            var simpleSign = Sr25519v091.SignSimple(publicKey, privatKey, payload);
            var simpleSignStr = Utils.Bytes2HexString(simpleSign);

            Assert.True(Sr25519v091.Verify(simpleSign, publicKey, payload));
            Assert.True(Sr25519v091.Verify(signature, publicKey, payload));

        }

        [Test]
        public void DmogCreateImmortalAliceTest2()
        {

            //  length: 103[2]
            //  signatureVersion: 0x84
            //  sendPublicKey: 5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY[0xD43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D]
            //  sendPublicKeyType: 0x01
            //  signature: 0x583313EF997E42929D889260EE8B75AE7FB5CE19B92E435CA0827A8C7B5BC44B7D1D3A8638D76C24EF47E61981B54BDDFDE64AA0C078F2B78EF915FF1B74468F
            //  era: 0x00
            //  nonce: 5[1]
            //  tip: 0[1]
            //  moduleIndex: 0x0602

            byte[] privatKey = Utils.HexToByteArray("0x33A6F3093F158A7109F679410BEF1A0C54168145E0CECB4DF006C1C2FFFB1F09925A225D97AA00682D6A59B95B18780C10D7032336E88F3442B42361F4A66011");

            byte publicKeyType = 0x01;
            byte[] publicKey = Utils.GetPublicKeyFrom("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY"); // Alice
            CompactInteger nonce = 5;
            byte moduleIndex = 0x06;
            byte callIndex = 0x02;

            byte[] parameters = new byte[0];

            byte[] genesisHash = Utils.HexToByteArray("0x9b443ea9cd42d9c3e0549757d029d28d03800631f9a9abf1d96d0c414b9aded9");
            byte[] currentBlockHash = Utils.HexToByteArray("0x9b443ea9cd42d9c3e0549757d029d28d03800631f9a9abf1d96d0c414b9aded9"); ;
            ulong currentBlockNumber = 0;
            CompactInteger tip = 0;

            // mocked signature
            byte[] signature = Utils.HexToByteArray("0x583313EF997E42929D889260EE8B75AE7FB5CE19B92E435CA0827A8C7B5BC44B7D1D3A8638D76C24EF47E61981B54BDDFDE64AA0C078F2B78EF915FF1B74468F");

            var uncheckedExtrinsic = new UnCheckedExtrinsic(true, publicKeyType, publicKey, nonce, moduleIndex, callIndex, parameters, genesisHash, currentBlockHash, currentBlockNumber, tip);

            string dmogCreateImmortal = "0x9d0184d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d01583313ef997e42929d889260ee8b75ae7fb5ce19b92e435ca0827a8c7b5bc44b7d1d3a8638d76c24ef47e61981b54bddfde64aa0c078f2b78ef915ff1b74468f0014000602";

            Assert.AreEqual(Utils.HexToByteArray(dmogCreateImmortal), uncheckedExtrinsic.Encode(signature));

            var payload = uncheckedExtrinsic.GetPayload().Encode();
            var payloadStr = Utils.Bytes2HexString(payload);

            if (payload.Length > 256)
            {
                payload = HashExtension.Blake2(payload, 256);
            }

            var simpleSign = Sr25519v091.SignSimple(publicKey, privatKey, payload);
            var simpleSignStr = Utils.Bytes2HexString(simpleSign);

            Assert.True(Sr25519v091.Verify(simpleSign, publicKey, payload));
            Assert.True(Sr25519v091.Verify(signature, publicKey, payload));

        }

        [Test]
        public void DmogCreateMortalAliceTest1()
        {

            //length: 104[2]
            //signatureVersion: 0x84
            //sendPublicKey: 5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY[0xD43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D]
            //sendPublicKeyType: 0x01
            //signature: 0x448082984004E4DC7CB964EBA2EB7201C5686D80E666944E2AA01C2BE95EAA5BE9D547DA63616A82631E87E4078A647FBD07920F97C8EA0993207C0FBDD2A98E
            //era: 0x1503
            //nonce: 5[1]
            //tip: 0[1]
            //moduleIndex: 0x0602

            //[
            //  {
            //    isSigned: true,
            //    method:
            //    {
            //      args:[],
            //      method: createMogwai,
            //      section: dmog
            //    },
            //    era:
            //    {
            //      MortalEra:
            //      {
            //          period: 64,
            //          phase: 49
            //      }
            //    },
            //    nonce: 5,
            //    signature: 0x448082984004e4dc7cb964eba2eb7201c5686d80e666944e2aa01c2be95eaa5be9d547da63616a82631e87e4078a647fbd07920f97c8ea0993207c0fbdd2a98e,
            //    signer: 5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY,
            //    tip: 0
            //  }
            //]

            //0xcd36f4e312289c56a3d9a464cc9b555e4f3634cd91409a7e05de58f37f9e0289
            // [{ "signature":{ "signer":"5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY","signature":{ "Sr25519":"0x448082984004e4dc7cb964eba2eb7201c5686d80e666944e2aa01c2be95eaa5be9d547da63616a82631e87e4078a647fbd07920f97c8ea0993207c0fbdd2a98e"},"era":{ "MortalEra":"0x1503"},"nonce":5,"tip":0},"method":{ "callIndex":"0x0602","args":{ } } }]

            // { block: { header: { parentHash: 0xaad3e1fd9da755971ccb9e07bd7cd4c335dffc236fd03a870178c211ce15ea06, number: 1587, stateRoot: 0x90469b87f5e818c5c8496a188997a057d14948fb421c62bb64eea52c442bde0f, extrinsicsRoot: 0x9882c28bc845ac3a17dcb244a5f1eaaf470ce43da24693cf9ef2fe1352da319f, digest: { logs:[{ "PreRuntime":[1634891105,"0x4457e60f00000000"]}, { "Seal":[1634891105,"0x42f94aef7c67e271aab99da484f83dda673cf5ecc0fc1a796aa2a06d0350d67afbf398fcf936d1574ccd89fef9397d09070921238e7c1f5742db795316084889"]}]} }, extrinsics:[{ "signature":{ "signer":"5C4hrfjw9DjXZTzV3MwzrrAr9P1MJhSrvWGWqi1eSuyUpnhM","signature":{ "Ed25519":"0x00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"},"era":{ "ImmortalEra":"0x00"},"nonce":0,"tip":0},"method":{ "callIndex":"0x0200","args":{ "now":1600523160000} } }]}, justification: 0x}
            byte[] privatKey = Utils.HexToByteArray("0x33A6F3093F158A7109F679410BEF1A0C54168145E0CECB4DF006C1C2FFFB1F09925A225D97AA00682D6A59B95B18780C10D7032336E88F3442B42361F4A66011");

            byte publicKeyType = 0x01;
            byte[] publicKey = Utils.GetPublicKeyFrom("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY"); // Alice
            CompactInteger nonce = 5;
            byte moduleIndex = 0x06;
            byte callIndex = 0x02;

            byte[] parameters = new byte[0];

            byte[] genesisHash = Utils.HexToByteArray("0x9b443ea9cd42d9c3e0549757d029d28d03800631f9a9abf1d96d0c414b9aded9");
            byte[] currentBlockHash = Utils.HexToByteArray("0xdce5a3ddc16196c00041d716e0cca8a8bf88b8aeebdb2714778fcdc0fe20079f"); ;

            Era era = new Era(64, 49, false);
            var blockHash = era.EraStart(1587);

            ulong currentBlockNumber = 49;
            CompactInteger tip = 0;

            // mocked signature
            byte[] signature = Utils.HexToByteArray("0x448082984004e4dc7cb964eba2eb7201c5686d80e666944e2aa01c2be95eaa5be9d547da63616a82631e87e4078a647fbd07920f97c8ea0993207c0fbdd2a98e");

            var uncheckedExtrinsic = new UnCheckedExtrinsic(true, publicKeyType, publicKey, nonce, moduleIndex, callIndex, parameters, genesisHash, currentBlockHash, currentBlockNumber, tip);

            string dmogCreateMortal = "0xa10184d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d01448082984004e4dc7cb964eba2eb7201c5686d80e666944e2aa01c2be95eaa5be9d547da63616a82631e87e4078a647fbd07920f97c8ea0993207c0fbdd2a98e150314000602";

            var uncheckedExtrinsicStr = Utils.Bytes2HexString(uncheckedExtrinsic.Encode(signature));

            Assert.AreEqual(Utils.HexToByteArray(dmogCreateMortal), uncheckedExtrinsic.Encode(signature));

            var payload = uncheckedExtrinsic.GetPayload().Encode();
            var payloadStr = Utils.Bytes2HexString(payload);

            if (payload.Length > 256)
            {
                payload = HashExtension.Blake2(payload, 256);
            }

            var simpleSign = Sr25519v091.SignSimple(publicKey, privatKey, payload);
            var simpleSignStr = Utils.Bytes2HexString(simpleSign);

            Assert.True(Sr25519v091.Verify(simpleSign, publicKey, payload));
            Assert.True(Sr25519v091.Verify(signature, publicKey, payload));

        }
    }
}
