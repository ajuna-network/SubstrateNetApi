using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NUnit.Framework;
using SubstrateNetApi.Model.Meta;
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
            
            var jsonFile = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "DOTMogNET.json"));
            _metaData = JsonConvert.DeserializeObject<MetaData>(jsonFile);
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
    }
}
