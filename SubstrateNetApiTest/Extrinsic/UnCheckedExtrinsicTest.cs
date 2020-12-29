using NUnit.Framework;
using Schnorrkel;
using SubstrateNetApi;
using SubstrateNetApi.MetaDataModel.Calls;
using SubstrateNetApi.MetaDataModel.Extrinsics;
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
            Constants.SPEC_VERSION = 1;
            Constants.ADDRESS_VERSION = 0;

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
            byte[] genesisHash = new byte[] { 0x00 };
            byte[] currentBlockHash = new byte[] { 0x00 };
            ulong currentBlockNumber = 47;
            CompactInteger tip = 1234;

            // mocked signature
            byte[] signature = Utils.HexToByteArray("0x14AE74DD7964365038EBA44F51C347B9C7070231D56E38EF1024457EBDC6DC03D20226243B1B2731DF6FD80F7170643221BD8BF8D06215D4BFEAC68A2C9D2305");

            Method method = new Method(moduleIndex, callIndex, parameters);

            Era era = new Era(Constants.EXTRINSIC_ERA_PERIOD_DEFAULT, currentBlockNumber, currentBlockNumber == 0 ? true : false);

            var uncheckedExtrinsic = new UnCheckedExtrinsic(true, new Account(KeyType.ED25519, new byte[0], publicKey), method, era, nonce, tip, new Hash(genesisHash), new Hash(currentBlockHash));

            string balanceTransfer = "0x350284278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e0014ae74dd7964365038eba44f51c347b9c7070231d56e38ef1024457ebdc6dc03d20226243b1b2731df6fd80f7170643221bd8bf8d06215d4bfeac68a2c9d2305f50204491304009effc1668ca381c242885516ec9fa2b19c67b6684c02a8a3237b6862e5c8cd7e068d6deb";

            uncheckedExtrinsic.AddPayloadSignature(signature);

            Assert.AreEqual(Utils.HexToByteArray(balanceTransfer), uncheckedExtrinsic.Encode());
        }

        [Test]
        public void BalanceTransferMockedTest2()
        {
            Constants.SPEC_VERSION = 1;
            Constants.ADDRESS_VERSION = 0;

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

            Method method = new Method(moduleIndex, callIndex, parameters);

            Era era = new Era(Constants.EXTRINSIC_ERA_PERIOD_DEFAULT, currentBlockNumber, currentBlockNumber == 0 ? true : false);

            var uncheckedExtrinsic = new UnCheckedExtrinsic(true, new Account(KeyType.ED25519, new byte[0], publicKey), method, era, nonce, tip, new Hash(genesisHash), new Hash(currentBlockHash));

            string balanceTransfer = "0x350284278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e0097176876e9cb26b2449855a332efee478768b31e215ed025ce57dd8e6bdfa63df3adfcf29091eb53c326173a5aad23661e074d132ae804bbd91126cd1909330275001849130400d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27dc62ed301";

            uncheckedExtrinsic.AddPayloadSignature(signature);

            Assert.AreEqual(Utils.HexToByteArray(balanceTransfer), uncheckedExtrinsic.Encode());

            var payload = uncheckedExtrinsic.GetPayload().Encode();
        }

        [Test]
        public void BalanceTransferAliceTest()
        {
            Constants.SPEC_VERSION = 1;
            Constants.ADDRESS_VERSION = 0;

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

            var bytes = new List<byte>();
            bytes.AddRange(Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM"));
            CompactInteger amount = 4321;
            bytes.AddRange(amount.Encode());
            byte[] parameters = bytes.ToArray();

            byte[] genesisHash = Utils.HexToByteArray("0x9b443ea9cd42d9c3e0549757d029d28d03800631f9a9abf1d96d0c414b9aded9");
            byte[] startEra = Utils.HexToByteArray("0xcfa2f9c52f94bc50658735d0f18f72590c981fdc15657636a99c437553c53253"); // CurrentBlock 780, startErar 778
            ulong currentBlockNumber = 10;
            CompactInteger tip = 1234;

            var Era = new Era(64, 10, false);

            // mocked signature
            byte[] signature = Utils.HexToByteArray("0x726ba1fab06d3e1bf6abfa0d5af85e25f2a970e11384162b7caf83935c58f769b6fef3b83a29ffd8d813a037d01cd6bcb21beaa88e9a18b3abe366b0458a8a82");

            Method method = new Method(moduleIndex, callIndex, parameters);

            Era era = new Era(Constants.EXTRINSIC_ERA_PERIOD_DEFAULT, currentBlockNumber, currentBlockNumber == 0 ? true : false);

            var uncheckedExtrinsic = new UnCheckedExtrinsic(true, new Account(KeyType.SR25519, new byte[0], publicKey), method, era, nonce, tip, new Hash(genesisHash), new Hash(startEra));

            string balanceTransfer = "0x2d0284d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d01726ba1fab06d3e1bf6abfa0d5af85e25f2a970e11384162b7caf83935c58f769b6fef3b83a29ffd8d813a037d01cd6bcb21beaa88e9a18b3abe366b0458a8a82a5001049130400278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e8543";

            uncheckedExtrinsic.AddPayloadSignature(signature);

            Assert.AreEqual(Utils.HexToByteArray(balanceTransfer), uncheckedExtrinsic.Encode());

            var payload = uncheckedExtrinsic.GetPayload().Encode();

            var simpleSign = Sr25519v091.SignSimple(publicKey, privatKey, payload);

            Assert.True(Sr25519v091.Verify(simpleSign, publicKey, payload));

            Assert.True(Sr25519v091.Verify(signature, publicKey, payload));

        }

        [Test]
        public void DmogCreateImmortalAliceTest()
        {
            Constants.SPEC_VERSION = 1;
            Constants.ADDRESS_VERSION = 0;

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

            Method method = new Method(moduleIndex, callIndex, parameters);

            Era era = new Era(Constants.EXTRINSIC_ERA_PERIOD_DEFAULT, currentBlockNumber, currentBlockNumber == 0 ? true : false);

            var uncheckedExtrinsic = new UnCheckedExtrinsic(true, new Account(KeyType.SR25519, new byte[0], publicKey), method, era, nonce, tip, new Hash(genesisHash), new Hash(currentBlockHash));

            string dmogCreateImmortal = "0x9d0184d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d01b8fb3fe1b723b69ed2011e5e3b168f202dfae3853c81d5617dd35a60c29f1c4b49b95dcf5631cca678837bc1b347dd1c20161e12512e16ced78a9592deecda8c0014000602";

            uncheckedExtrinsic.AddPayloadSignature(signature);

            Assert.AreEqual(Utils.HexToByteArray(dmogCreateImmortal), uncheckedExtrinsic.Encode());

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
            Constants.SPEC_VERSION = 1;
            Constants.ADDRESS_VERSION = 0;

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

            Method method = new Method(moduleIndex, callIndex, parameters);

            Era era = new Era(Constants.EXTRINSIC_ERA_PERIOD_DEFAULT, currentBlockNumber, currentBlockNumber == 0 ? true : false);

            var uncheckedExtrinsic = new UnCheckedExtrinsic(true, new Account(KeyType.SR25519, new byte[0], publicKey), method, era, nonce, tip, new Hash(genesisHash), new Hash(currentBlockHash));

            string dmogCreateImmortal = "0x9d0184d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d01583313ef997e42929d889260ee8b75ae7fb5ce19b92e435ca0827a8c7b5bc44b7d1d3a8638d76c24ef47e61981b54bddfde64aa0c078f2b78ef915ff1b74468f0014000602";

            uncheckedExtrinsic.AddPayloadSignature(signature);

            Assert.AreEqual(Utils.HexToByteArray(dmogCreateImmortal), uncheckedExtrinsic.Encode());

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
            Constants.SPEC_VERSION = 1;
            Constants.ADDRESS_VERSION = 0;

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

            CompactInteger tip = 0;

            // mocked signature
            byte[] signature = Utils.HexToByteArray("0x448082984004e4dc7cb964eba2eb7201c5686d80e666944e2aa01c2be95eaa5be9d547da63616a82631e87e4078a647fbd07920f97c8ea0993207c0fbdd2a98e");

            Method method = new Method(moduleIndex, callIndex, parameters);

            var uncheckedExtrinsic = new UnCheckedExtrinsic(true, new Account(KeyType.SR25519, new byte[0], publicKey), method, era, nonce, tip, new Hash(genesisHash), new Hash(currentBlockHash));

            string dmogCreateMortal = "0xa10184d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d01448082984004e4dc7cb964eba2eb7201c5686d80e666944e2aa01c2be95eaa5be9d547da63616a82631e87e4078a647fbd07920f97c8ea0993207c0fbdd2a98e150314000602";

            uncheckedExtrinsic.AddPayloadSignature(signature);

            var uncheckedExtrinsicStr = Utils.Bytes2HexString(uncheckedExtrinsic.Encode());

            Assert.AreEqual(Utils.HexToByteArray(dmogCreateMortal), uncheckedExtrinsic.Encode());

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
        public void BalanceTransferCreateMortalZurichToAliceTest1()
        {
            Constants.SPEC_VERSION = 1;
            Constants.ADDRESS_VERSION = 0;

            // 0x310284278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e007c9777cf14fe0e14e8aef019695043be2fd153a75ff3381f4cc4850755d537b1a9d7920e509ee2e4e1f244dad670dc44ec3fc24388181e6465fdda13d59ae70063001c000400d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d02890700
            // sender 5FfBQ3kwXrbdyoqLPvcXRp7ikWydXawpNs2Ceu3WwFdhZ8W4
            // recent blocks 15,689 0xf8438a058c66ab33628249459d1f0bc8114c6550d3ddea45351a96cbf2999813
            // nonce 7
            // receiver 5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY
            // Lifetime 12

            //length: 140[2]
            //signatureVersion: 0x84
            //sendPublicKey: 5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM[0x278117FC144C72340F67D0F2316E8386CEFFBF2B2428C9C51FEF7C597F1D426E]
            //sendPublicKeyType: 0x00
            //signature: 0x7C9777CF14FE0E14E8AEF019695043BE2FD153A75FF3381F4CC4850755D537B1A9D7920E509EE2E4E1F244DAD670DC44EC3FC24388181E6465FDDA13D59AE700
            //era: 0x6300
            //nonce: 7[1]
            //tip: 0[1]
            //moduleIndex: 0x0400
            //destPublicKey: 5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY[0xD43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D]
            //amount: 123456[4]


            byte[] privatKey = Utils.HexToByteArray("0xf5e5767cf153319517630f226876b86c8160cc583bc013744c6bf255f5cc0ee5278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e");
            byte[] publicKey = Utils.HexToByteArray("0x278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e");
            var account = new Account(KeyType.ED25519, privatKey, publicKey);

            var bytes = new List<byte>();
            bytes.AddRange(Utils.GetPublicKeyFrom("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY"));
            CompactInteger amount = 123456;
            bytes.AddRange(amount.Encode());
            var method = new Method(0x04, 0x00, bytes.ToArray());

            var era = Era.Create(12, 15686);

            CompactInteger nonce = 7;

            CompactInteger tip = 0;

            var genesis = new Hash(Utils.HexToByteArray("0x9b443ea9cd42d9c3e0549757d029d28d03800631f9a9abf1d96d0c414b9aded9"));
            var startEra = new Hash(Utils.HexToByteArray("0x4c4e0d1594e526c2392e7b6306f890fd0705085a5f83f9114caebb369bc1511f")); // FinalizedHead 15686


            // mocked signature
            byte[] signature = Utils.HexToByteArray("0x7C9777CF14FE0E14E8AEF019695043BE2FD153A75FF3381F4CC4850755D537B1A9D7920E509EE2E4E1F244DAD670DC44EC3FC24388181E6465FDDA13D59AE700");

            var uncheckedExtrinsic = new UnCheckedExtrinsic(true, account, method, era, nonce, tip, genesis, startEra);

            uncheckedExtrinsic.AddPayloadSignature(signature);

            var balanceTransferNode = "0x310284278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e007c9777cf14fe0e14e8aef019695043be2fd153a75ff3381f4cc4850755d537b1a9d7920e509ee2e4e1f244dad670dc44ec3fc24388181e6465fdda13d59ae70063001c000400d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d02890700";

            var uncheckedExtrinsicStr = Utils.Bytes2HexString(uncheckedExtrinsic.Encode());

            Assert.AreEqual(Utils.HexToByteArray(balanceTransferNode), uncheckedExtrinsic.Encode());


            var payload = uncheckedExtrinsic.GetPayload().Encode();
            var payloadStr = Utils.Bytes2HexString(payload);

            if (payload.Length > 256)
            {
                payload = HashExtension.Blake2(payload, 256);
            }

            var simpleSign = Chaos.NaCl.Ed25519.Sign(payload, account.PrivateKey);
            var simpleSignStr = Utils.Bytes2HexString(simpleSign);

            Assert.True(Chaos.NaCl.Ed25519.Verify(simpleSign, payload, publicKey));
            Assert.True(Chaos.NaCl.Ed25519.Verify(signature, payload, publicKey));

            var extrinsic = RequestGenerator.SubmitExtrinsic(true, account, method, era, nonce, tip, genesis, startEra);
            Assert.True(Chaos.NaCl.Ed25519.Verify(extrinsic.Signature, extrinsic.GetPayload().Encode(), publicKey));
            Assert.True(Chaos.NaCl.Ed25519.Verify(signature, extrinsic.GetPayload().Encode(), publicKey));
        }

        [Test]
        public void CreateMogwaiTestZurich()
        {
            // 792,193 ---> 0x0cf64c1e0e45b2fba6fd524e180737f5e1bb46e0691783d6963b2e26253f8592

            Constants.SPEC_VERSION = 259;
            Constants.ADDRESS_VERSION = 1;

            Account accountZurich = new Account(
                KeyType.ED25519,
                Utils.HexToByteArray("0xf5e5767cf153319517630f226876b86c8160cc583bc013744c6bf255f5cc0ee5278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e"),
                Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM"));

            byte[] privatKey = Utils.HexToByteArray("0xf5e5767cf153319517630f226876b86c8160cc583bc013744c6bf255f5cc0ee5278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e");
            byte[] publicKey = Utils.HexToByteArray("0x278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e");

            //                                     278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e
            //                                                                                                       029d6a4d204108ecc3d27ccfb5163c85582f946282ba7625c0c9da2595ba89856df529efea6fdc36426a6be89bddefed52d23fc1ccf66dd9483b542ed96e0b08
            string referenceExtrinsic = "0xa50184ff278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e00029d6a4d204108ecc3d27ccfb5163c85582f946282ba7625c0c9da2595ba89856df529efea6fdc36426a6be89bddefed52d23fc1ccf66dd9483b542ed96e0b08750304002003";
            //                          "0xA50184FF278117FC144C72340F67D0F2316E8386CEFFBF2B2428C9C51FEF7C597F1D426E00029D6A4D204108ECC3D27CCFB5163C85582F946282BA7625C0C9DA2595BA89856DF529EFEA6FDC36426A6BE89BDDEFED52D23FC1CCF66DD9483B542ED96E0B08750304003203"
            var method = new Method(0x20, 0x03);

            var era = Era.Create(64, 792183);

            CompactInteger nonce = 7;

            CompactInteger tip = 0;

            var uncheckedExtrinsic = new UnCheckedExtrinsic(true
                , new Account(KeyType.ED25519, privatKey, publicKey)
                , method
                , era
                , 1
                , 0
                , new Hash("0x778c4bb53621114939206c9c9874c5fa1da38d2e14293d053a0b8dd6125b4042")
                , new Hash("0x1a62fe1013aab94901e7dd80051f8e2b6b3c44bd0f0c934ff665768d459b3aa5") // currentblock
            );


            uncheckedExtrinsic.AddPayloadSignature(Utils.HexToByteArray("0x029d6a4d204108ecc3d27ccfb5163c85582f946282ba7625c0c9da2595ba89856df529efea6fdc36426a6be89bddefed52d23fc1ccf66dd9483b542ed96e0b08"));

            var uncheckedExtrinsicStr = Utils.Bytes2HexString(uncheckedExtrinsic.Encode());


            var payload = uncheckedExtrinsic.GetPayload().Encode();

            /// Payloads longer than 256 bytes are going to be `blake2_256`-hashed.
            if (payload.Length > 256)
            {
                payload = HashExtension.Blake2(payload, 256);
            }

            byte[] signature;
            signature = Chaos.NaCl.Ed25519.Sign(payload, privatKey);
            var signatureStr = Utils.Bytes2HexString(signature);

            uncheckedExtrinsic.AddPayloadSignature(signature);

            Assert.AreEqual(referenceExtrinsic, uncheckedExtrinsicStr.ToLower());

            /**
             *       
             * {
                isSigned: true,
                method: {
                  args: [],
                  method: createMogwai,
                  section: dotMogModule
                },
                era: {
                  MortalEra: {
                    period: 64,
                    phase: 55
                  }
                },
                nonce: 1,
                signature: 0x029d6a4d204108ecc3d27ccfb5163c85582f946282ba7625c0c9da2595ba89856df529efea6fdc36426a6be89bddefed52d23fc1ccf66dd9483b542ed96e0b08,
                signer: 5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM,
                tip: 0
              }
             * 
             */

            //{
            //    "Signed": true,
            //    "TransactionVersion": 4,
            //    "Account": {
            //                "KeyType": 0,
            //      "Address": "5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM",
            //      "PublicKey": "J4EX/BRMcjQPZ9DyMW6Dhs7/vyskKMnFH+98WX8dQm4="
            //    },
            //    "Era": {
            //      "IsImmortal": false,
            //      "Period": 64,
            //      "Phase": 55
            //    },
            //    "Nonce": {
            //                "Value": 1
            //    },
            //    "Tip": {
            //                "Value": 0
            //    },
            //    "Method": {
            //      "ModuleName": "DotMogModule",
            //      "ModuleIndex": 32,
            //      "CallName": "create_mogwai",
            //      "CallIndex": 3,
            //      "Arguments": [
            //      ],
            //      "Parameters": ""
            //    },
            //    "Signature": "Ap1qTSBBCOzD0nzPtRY8hVgvlGKCunYlwMnaJZW6iYVt9Snv6m/cNkJqa+ib3e/tUtI/wcz2bdlIO1Qu2W4LCA=="
            //  }

        }

        [Test]
        public void BalanceTransferZurichTest()
        {
            Constants.SPEC_VERSION = 259;
            Constants.ADDRESS_VERSION = 1;

            // 797447 --> 0xe7b99ee484e6369dd3c2a66d6306bffde5048ddf2090e990faae83e66f5275f4

            Account accountZurich = new Account(
                KeyType.ED25519,
                Utils.HexToByteArray("0xf5e5767cf153319517630f226876b86c8160cc583bc013744c6bf255f5cc0ee5278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e"),
                Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM"));

            byte[] privatKey = Utils.HexToByteArray("0xf5e5767cf153319517630f226876b86c8160cc583bc013744c6bf255f5cc0ee5278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e");
            byte[] publicKey = Utils.HexToByteArray("0x278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e");

            var receiverPublicKey = Utils.Bytes2HexString(Utils.GetPublicKeyFrom("5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH"));

            string referenceExtrinsic = "0x450284ff278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e00d6a14aac2c0da8330f67a04f9ff4154b3c31d02529eaf112a23d59f5a5e1d1766efbb7f4dd56e6ed84a543de94342bdec8c80bdac62373d22387ea980a42270f36000c000600ff4d2b23d27e1f6e3733d7ebf3dc04f3d5d0010cd18038055f9bbbab48f460b61e0b00b04e2bde6f";
            //                          "0x450284FF278117FC144C72340F67D0F2316E8386CEFFBF2B2428C9C51FEF7C597F1D426E00D6A14AAC2C0DA8330F67A04F9FF4154B3C31D02529EAF112A23D59F5A5E1D1766EFBB7F4DD56E6ED84A543DE94342BDEC8C80BDAC62373D22387EA980A42270F35000C000600FF4D2B23D27E1F6E3733D7EBF3DC04F3D5D0010CD18038055F9BBBAB48F460B61E0B00B04E2BDE6F"
            //                          "0x450284FF278117FC144C72340F67D0F2316E8386CEFFBF2B2428C9C51FEF7C597F1D426E00D6A14AAC2C0DA8330F67A04F9FF4154B3C31D02529EAF112A23D59F5A5E1D1766EFBB7F4DD56E6ED84A543DE94342BDEC8C80BDAC62373D22387EA980A42270F36000C000600FF4D2B23D27E1F6E3733D7EBF3DC04F3D5D0010CD18038055F9BBBAB48F460B61E0B00B04E2BDE6F"
            
            //var bytes = new List<byte>();
            //bytes.Add(0xFF);
            //bytes.AddRange(Utils.GetPublicKeyFrom("5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH"));
            //CompactInteger amount = 123000000000000;
            //bytes.AddRange(amount.Encode());
            //byte[] parameters = bytes.ToArray();
            ////var method = new Method(0x06, 0x00, parameters);

            var extrinsic = ExtrinsicCall.BalanceTransfer(new AccountId(Utils.GetPublicKeyFrom("5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH")), new Balance(123000000000000));

            var method = new Method(0x06, 0x00, extrinsic.Encode());

            //var era = Era.Create(64, 797443);

            var era = new Era(128, 3, false);
                
            CompactInteger nonce = 3;

            CompactInteger tip = 0;

            var uncheckedExtrinsic = new UnCheckedExtrinsic(true
                , new Account(KeyType.ED25519, privatKey, publicKey)
                , method
                , era
                , nonce
                , tip
                , new Hash("0x778c4bb53621114939206c9c9874c5fa1da38d2e14293d053a0b8dd6125b4042")
                , new Hash("0xd5a0f4467c6c8885b531f12028789e83c2e473b8d2d44edbc09811fd2f903f1f") // currentblock
            );

            //uncheckedExtrinsic.AddPayloadSignature(Utils.HexToByteArray("0xd6a14aac2c0da8330f67a04f9ff4154b3c31d02529eaf112a23d59f5a5e1d1766efbb7f4dd56e6ed84a543de94342bdec8c80bdac62373d22387ea980a42270f"));

            var payload = uncheckedExtrinsic.GetPayload().Encode();

            /// Payloads longer than 256 bytes are going to be `blake2_256`-hashed.
            if (payload.Length > 256)
            {
                payload = HashExtension.Blake2(payload, 256);
            }

            byte[] signature;
            signature = Chaos.NaCl.Ed25519.Sign(payload, privatKey);
            var signatureStr = Utils.Bytes2HexString(signature);

            uncheckedExtrinsic.AddPayloadSignature(signature);

            var uncheckedExtrinsicStr = Utils.Bytes2HexString(uncheckedExtrinsic.Encode());

            Assert.AreEqual(referenceExtrinsic, uncheckedExtrinsicStr.ToLower());

            //{
            //    isSigned: true,
            //    method:
            //            {
            //            args:[
            //             5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH,
            //             123.0000 mUnit
            //      ],
            //      method: transfer,
            //      section: balances
            //    },
            //    era:
            //            {
            //            MortalEra:
            //                {
            //                period: 128,
            //        phase: 3
            //            }
            //            },
            //    nonce: 3,
            //    signature: 0xd6a14aac2c0da8330f67a04f9ff4154b3c31d02529eaf112a23d59f5a5e1d1766efbb7f4dd56e6ed84a543de94342bdec8c80bdac62373d22387ea980a42270f,
            //    signer: 5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM,
            //    tip: 0
            //}


            //{
            //    "Signed": true,
            //    "TransactionVersion": 4,
            //    "Account": {
            //                "KeyType": 0,
            //      "Address": "5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM",
            //      "PublicKey": "J4EX/BRMcjQPZ9DyMW6Dhs7/vyskKMnFH+98WX8dQm4="
            //    },
            //    "Era": {
            //                "IsImmortal": false,
            //      "Period": 128,
            //      "Phase": 3
            //    },
            //    "Nonce": {
            //                "Value": 3
            //    },
            //    "Tip": {
            //                "Value": 0
            //    },
            //    "Method": {
            //                "ModuleName": "Balances",
            //      "ModuleIndex": 6,
            //      "CallName": "transfer",
            //      "CallIndex": 0,
            //      "Arguments": [
            //        {
            //                    "Name": "dest",
            //          "Type": "<T::Lookup as StaticLookup>::Source",
            //          "Value": "5DotMog6fcsVhMPqniyopz5sEJ5SMhHpz7ymgubr56gDxXwH"
            //        },
            //        {
            //                    "Name": "value",
            //          "Type": "Compact<T::Balance>",
            //          "Value": {
            //                        "Value": 123000000000000
            //          }
            //                }
            //      ],
            //      "Parameters": "/00rI9J+H243M9fr89wE89XQAQzRgDgFX5u7q0j0YLYeCwCwTivebw=="
            //    },
            //    "Signature": "1qFKrCwNqDMPZ6BPn/QVSzwx0CUp6vESoj1Z9aXh0XZu+7f03Vbm7YSlQ96UNCveyMgL2sYjc9Ijh+qYCkInDw=="
            //}

        }


    }
}
