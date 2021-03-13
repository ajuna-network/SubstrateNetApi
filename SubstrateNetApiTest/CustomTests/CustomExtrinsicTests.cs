using System;
using System.Text;
using NUnit.Framework;
using SubstrateNetApi;
using SubstrateNetApi.Model.Types.Struct;
using SubstrateNetApi.Model.Types.Base;
using NLog.Config;
using NLog;
using System.Threading.Tasks;
using SubstrateNetApi.Exceptions;
using NLog.Targets;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.Model.Rpc;
using SubstrateNetApi.Model.Extrinsics;

namespace SubstrateNetApiTests
{
    public class CustomExtrinsicTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }

        [Test]
        public void CreateMogwaiTestZurich()
        {
            // 792,193 ---> 0x0cf64c1e0e45b2fba6fd524e180737f5e1bb46e0691783d6963b2e26253f8592

            var runtime = new RuntimeVersion
            {
                SpecVersion = 1,
                TransactionVersion = 1
            };
            Constants.AddressVersion = 1;

            var accountZurich = Account.Build(
                KeyType.Ed25519,
                Utils.HexToByteArray(
                    "0xf5e5767cf153319517630f226876b86c8160cc583bc013744c6bf255f5cc0ee5278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e"),
                Utils.GetPublicKeyFrom("5CxW5DWQDpXi4cpACd62wzbPjbYrx4y67TZEmRXBcvmDTNaM"));

            var privatKey =
                Utils.HexToByteArray(
                    "0xf5e5767cf153319517630f226876b86c8160cc583bc013744c6bf255f5cc0ee5278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e");
            var publicKey = Utils.HexToByteArray("0x278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e");

            //                                     278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e
            //                                                                                                       029d6a4d204108ecc3d27ccfb5163c85582f946282ba7625c0c9da2595ba89856df529efea6fdc36426a6be89bddefed52d23fc1ccf66dd9483b542ed96e0b08
            var referenceExtrinsic =
                "0xa50184ff278117fc144c72340f67d0f2316e8386ceffbf2b2428c9c51fef7c597f1d426e00029d6a4d204108ecc3d27ccfb5163c85582f946282ba7625c0c9da2595ba89856df529efea6fdc36426a6be89bddefed52d23fc1ccf66dd9483b542ed96e0b08750304002003";
            //                          "0xA50184FF278117FC144C72340F67D0F2316E8386CEFFBF2B2428C9C51FEF7C597F1D426E00029D6A4D204108ECC3D27CCFB5163C85582F946282BA7625C0C9DA2595BA89856DF529EFEA6FDC36426A6BE89BDDEFED52D23FC1CCF66DD9483B542ED96E0B08750304003203"
            var method = new Method(0x20, 0x03);

            var era = Era.Create(64, 792183);

            CompactInteger nonce = 7;

            CompactInteger tip = 0;

            var genesis = new Hash();
            genesis.Create(Utils.HexToByteArray("0x778c4bb53621114939206c9c9874c5fa1da38d2e14293d053a0b8dd6125b4042"));

            var startEra = new Hash();
            startEra.Create(Utils.HexToByteArray("0x1a62fe1013aab94901e7dd80051f8e2b6b3c44bd0f0c934ff665768d459b3aa5"));

            var uncheckedExtrinsic = new UnCheckedExtrinsic(true
                , Account.Build(KeyType.Ed25519, privatKey, publicKey)
                , method
                , era
                , 1
                , 0
                , genesis
                , startEra // currentblock
            );


            uncheckedExtrinsic.AddPayloadSignature(Utils.HexToByteArray(
                "0x029d6a4d204108ecc3d27ccfb5163c85582f946282ba7625c0c9da2595ba89856df529efea6fdc36426a6be89bddefed52d23fc1ccf66dd9483b542ed96e0b08"));

            var uncheckedExtrinsicStr = Utils.Bytes2HexString(uncheckedExtrinsic.Encode());


            var payload = uncheckedExtrinsic.GetPayload(runtime).Encode();

            /// Payloads longer than 256 bytes are going to be `blake2_256`-hashed.
            if (payload.Length > 256) payload = HashExtension.Blake2(payload, 256);

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

    }
}