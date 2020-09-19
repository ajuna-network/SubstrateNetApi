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

            Assert.AreEqual(Utils.HexToByteArray(balanceTransfer), uncheckedExtrinsic.Serialize(signature));

            var payload = uncheckedExtrinsic.GetPayload().Serialize();

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

            Assert.AreEqual(Utils.HexToByteArray(dmogCreateImmortal), uncheckedExtrinsic.Serialize(signature));

            var payload = uncheckedExtrinsic.GetPayload().Serialize();
            var payloadStr = Utils.Bytes2HexString(payload);

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
                                         
            Assert.AreEqual(Utils.HexToByteArray(dmogCreateImmortal), uncheckedExtrinsic.Serialize(signature));

            var payload = uncheckedExtrinsic.GetPayload().Serialize();
            var payloadStr = Utils.Bytes2HexString(payload);

            var simpleSign = Sr25519v091.SignSimple(publicKey, privatKey, payload);
            var simpleSignStr = Utils.Bytes2HexString(simpleSign);

            Assert.True(Sr25519v091.Verify(simpleSign, publicKey, payload));
            Assert.True(Sr25519v091.Verify(signature, publicKey, payload));

        }
        

        [Test]
        public void ReducedTestByHandSignedPayloadCallAndGenesis()
        {
            //{
            //block:
            //    {
            //    header:
            //        {
            //        parentHash: 0x7c9966a367e8ee67049ff82957c5006d0cfbd4e497622d81e107e8e41216788d,
            //      number: 21,
            //      stateRoot: 0x56c02eaf00b08190c685ed01224468bd80e2a6142228c73d5d6af2f23742afd2,
            //      extrinsicsRoot: 0x50efe682682114f076dd19c6d2da265be67e0b7f50b170722eca86b5f79d1095,
            //      digest:
            //            {
            //            logs:
            //                [
            //              {
            //                PreRuntime:[
            //                 aura,
            //                 0x8351e60f00000000
            //                ]
            //          },
            //          {
            //                Seal:[
            //                 aura,
            //                 0x1675a04aed7c715149212fafc1fa079e784824d93d0dd72729dc57ec36b66906944255297e508c563ea0d050bdfc52a572aed9256dd12ae1bded7c5115978f89
            //            ]
            //          }
            //        ]
            //      }
            //        },
            //    extrinsics:
            //        [
            //      {
            //        isSigned: false,
            //        method:
            //            {
            //            args:[
            //             1,600,514,322,000
            //          ],
            //          method: set,
            //          section: timestamp
            //        }
            //        },
            //      {
            //        isSigned: true,
            //        method:
            //            {
            //            args:[
            //             5FHneW46xGXgs5mUiveU4sbTyGBzmstUspZC92UhjJM694ty,
            //            1.234n Unit
            //          ],
            //          method: transfer,
            //          section: balances
            //        },
            //        era:
            //            {
            //            MortalEra:
            //                {
            //                period: 64,
            //            phase: 18
            //            }
            //            },
            //        nonce: 0,
            //        signature: 0x4a47136012572194d55ad4dcf4672d697b5171ff908d8113ba78b5a546a0227969e9833f3e1b164273e19359a7f275aee4d02c2240ddd21f99fbe44d8b53468b,
            //        signer: 5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY,
            //        tip: 0
            //      }
            //    ]
            //  },
            //  justification:
            //}

            //0xdc4f62090b18b6893c1431369461069ee3e9c1da7f9f9a8c097c0cebbeac2bb9

            // alice private Key
            //byte[] seed = Utils.HexToByteArray("0xe5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a");
            byte[] privatKey = Utils.HexToByteArray("0x33A6F3093F158A7109F679410BEF1A0C54168145E0CECB4DF006C1C2FFFB1F09925A225D97AA00682D6A59B95B18780C10D7032336E88F3442B42361F4A66011");

            byte publicKeyType = 0xFF;
            
            // alice public Key
            byte[] publicKey = Utils.GetPublicKeyFrom("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY"); // Alice

            var payload = new List<byte>();
            // add module index
            payload.Add(0x04);
            // add call index
            payload.Add(0x00);
            // add public key type
            payload.Add(publicKeyType); // 0xFF, 0x00, 0x01
            // add dest public key
            payload.AddRange(Utils.GetPublicKeyFrom("5FHneW46xGXgs5mUiveU4sbTyGBzmstUspZC92UhjJM694ty"));
            // add amount 1234
            CompactInteger amount = 1234;
            payload.AddRange(amount.Encode());
            // add genesis_hash
            payload.AddRange(Utils.HexToByteArray("0xdc4f62090b18b6893c1431369461069ee3e9c1da7f9f9a8c097c0cebbeac2bb9"));

            var payloadBytes = payload.ToArray();

            if (payloadBytes.Length > 256)
            {
                payloadBytes = HashExtension.Blake2(payloadBytes, 256);
            }

            var payloadBytesStr = Utils.Bytes2HexString(payloadBytes);
            // PAYLOAD UNSIGNED 0x0400FF8EAF04151687736326C9FEA17E25FC5287613693C912909CB226AA4794F26A484913DC4F62090B18B6893C1431369461069EE3E9C1DA7F9F9A8C097C0CEBBEAC2BB9

            var signedPayload = Sr25519v091.SignSimple(publicKey, privatKey, payloadBytes);
            var signedPayloadStr = Utils.Bytes2HexString(signedPayload);
            // PAYLOAD SIGNATURES BY ALICE 
            // 0x0E76DDBB6EBEB2D293E21312861DC1A287158D2A254718D479F7A394E35FB0098D047A0FD4DEEC119D9235EDEBA3A49DF4E9392FF730270522DE5DB3D2917084
            // 0x16C60679E04F2EB6FED70B0A25F476E6F7A7C62C1C0DE839AC9785DC9E0B8E58A4D1D94B7E2BEC47CF135AD134F0F9590EEF07B309CBCF73CEE38E9CC4A80C80
            // 0x3891D4C54E3BC12700190AEAD46FDA5ACBB73B0379E758601E944996B1F23D2D047C6AC4B27421FC7AD61EE5B92F53621B4C32FD5AB0C187F9D0F8618C59E18A
            Assert.True(Sr25519v091.Verify(signedPayload, publicKey, payloadBytes));

            var signatureExpected = Utils.HexToByteArray("0x4a47136012572194d55ad4dcf4672d697b5171ff908d8113ba78b5a546a0227969e9833f3e1b164273e19359a7f275aee4d02c2240ddd21f99fbe44d8b53468b");

            Assert.True(Sr25519v091.Verify(signatureExpected, publicKey, payloadBytes));

            //Assert.AreEqual(signatureExpected, signedPayload);

        }
    }
}
