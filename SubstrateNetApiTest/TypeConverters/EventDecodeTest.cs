using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NUnit.Framework;
using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.Rpc;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Enum;
using SubstrateNetApi.Model.Types.Struct;
using SubstrateNetApi.TypeConverters;
using SubstrateNetWallet;

namespace SubstrateNetApiTests.TypeConverters
{
    public class  EventDecodeTest
    {
        private MetaData _metaData;

        [SetUp]
        public void Setup()
        {
            SystemInteraction.ReadData = f => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, f));
            SystemInteraction.DataExists = f => File.Exists(Path.Combine(Environment.CurrentDirectory, f));
            SystemInteraction.ReadPersistent = f => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, f));
            SystemInteraction.PersistentExists = f => File.Exists(Path.Combine(Environment.CurrentDirectory, f));
            SystemInteraction.Persist = (f, c) => File.WriteAllText(Path.Combine(Environment.CurrentDirectory, f), c);
            
            var metaDataJson = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "DOTMogNET.json"));
            _metaData = JsonConvert.DeserializeObject<MetaData>(metaDataJson);
        }

        [Test]
        public void BasicEvent1()
        {
            Assert.NotNull(_metaData);

            var eventStr = "0x04000000000000005095a20900000000020000";
            var eventRecords = new EventRecords(_metaData);
            eventRecords.Create(eventStr);

            Assert.AreEqual(1, eventRecords.Value.Count);
            Assert.AreEqual(PhaseState.None, eventRecords.Value[0].Phase.PhaseState.Value);
            Assert.AreEqual(0, eventRecords.Value[0].Phase.ApplyExtrinsic.Value);

            Assert.AreEqual("System", eventRecords.Value[0].BaseEvent.ModuleName);
            Assert.AreEqual("ExtrinsicSuccess", eventRecords.Value[0].BaseEvent.EventName);
            Assert.AreEqual(1, eventRecords.Value[0].BaseEvent.EventArgs.Length);
            Assert.AreEqual("DispatchInfo", eventRecords.Value[0].BaseEvent.EventArgs[0].GetType().Name);
            var dispetchInfo = (DispatchInfo) eventRecords.Value[0].BaseEvent.EventArgs[0];
            Assert.AreEqual(161650000, dispetchInfo.Weight.Value);
        }

        [Test]
        public void BasicEvent2()
        {
            Assert.NotNull(_metaData);

            var eventStr = "0x08000000000000005095a20900000000020000000100000000000000000000000000020000";
            var eventRecords = new EventRecords(_metaData);
            eventRecords.Create(eventStr);

            // TODO add asserts
            Assert.True(true);
        }

        [Test]
        public void BasicEvent3()
        {
            Assert.NotNull(_metaData);

            var eventStr = "0x1802130100020800bc0000007f9267dfabb62a000000000000000000ac9baa9c3eff7f000000" +
                           "00000000000000021006ac9baa9c3eff7f000000000000000000000209006e0400000000000000000000" +
                           "5095a20900000000020000010f00087c932416d1f140d6351d3b6b09ff6fee66ff240bdb92976d36c2ef" +
                           "5b13d83c7f0100000000000000490b83057d01d315d27e2b607c31754419bce23df85e39db096abce127" +
                           "16470b010000000000000000";
            var eventRecords = new EventRecords(_metaData);
            eventRecords.Create(eventStr);

            // TODO add asserts
            Assert.True(true);
        }

        [Test]
        public void AdvancedJsonConverter1()
        {
            string state_storage_response =
                "{\r\n\"block\":\"0x30712fe96155dcce61b9e3e41844f13e7a6f642f27a745e44a483b0009191c77\",\r\n\"changes\":[\r\n[\r\n\"0x26aa394eea5630e07c48ae0c9558cef7b99d880ec681799c0cf30e8886371da9438d5c56935253cd6d9ea70f2bf95d60137f0ffc8904f42aaabf63d4ec80862f5087a8b059260c191401e03c03836767\",\r\nnull\r\n]\r\n]\r\n}";
            StorageChangeSet storageChangeSet = JsonConvert.DeserializeObject<StorageChangeSet>(state_storage_response);
            
            Assert.AreEqual("0x30712FE96155DCCE61B9E3E41844F13E7A6F642F27A745E44A483B0009191C77", storageChangeSet.Block.Value);

            //NewStorageChangeSet newStorageChangeSet = JsonConvert.DeserializeObject<NewStorageChangeSet>(state_storage_response);
            //Assert.AreEqual("0x30712FE96155DCCE61B9E3E41844F13E7A6F642F27A745E44A483B0009191C77", newStorageChangeSet.Block.Value);
        }
    }
}
