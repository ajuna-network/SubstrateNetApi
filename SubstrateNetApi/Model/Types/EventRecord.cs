using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Enum;
using SubstrateNetApi.Model.Types.Struct;

namespace SubstrateNetApi.Model.Types
{
    public class EventRecords
    {
        public EventRecord[] Events;

        public EventRecords(EventRecord[] events)
        {
            Events = events;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static EventRecords Decode(string eventStr, MetaData metaData)
        {
            var byteArray = Utils.HexToByteArray(eventStr);

            var p = 0;
            var eventCount = CompactInteger.Decode(byteArray, ref p).Value;


            var byteArrayMemory = byteArray.AsMemory();

            // skipping one byte, unclear why!
            var events = new EventRecord[(int) eventCount];
            for (var i = 0; i < events.Length; i++)
            {
                var startByte = byteArrayMemory.Span.Slice(p, 1).ToArray()[0];
                p += 1;

                var phaseArray = byteArrayMemory.Span.Slice(p, 4).ToArray();
                var phase = BitConverter.ToUInt32(phaseArray, 0);
                p += 4;

                var module = metaData.Modules[byteArrayMemory.Span.Slice(p, 1)[0]];
                p += 1;

                var moduleEvent = module.Events[byteArrayMemory.Span.Slice(p, 1)[0]];
                p += 1;

                var eventData = GetEventDataSize(moduleEvent.EventArgs, byteArray, ref p);

                var endByte = byteArrayMemory.Span.Slice(p, 1).ToArray()[0];
                p += 1;

                events[i] = new EventRecord(
                    new Phase(phase),
                    new BaseEvent(module.Name, moduleEvent.Name, eventData));
            }

            return new EventRecords(events);
        }

        private static object[] GetEventDataSize(string[] eventArgs, byte[] byteArray, ref int p)
        {
            var data = new List<object>();

            foreach (var eventArg in eventArgs)
                // TODO, reflection would solve this much better, but probably a bad idea for unity3d we need to test.
                // -------------------------------
                //object[] arguments = new object[] { byteArray, p };
                //MethodInfo method = Type.GetType($"SubstrateNetApi.MetaDataModel.Values.{eventArg}, SubstrateNetApi")
                //    .GetMethod("Decode", BindingFlags.Static | BindingFlags.Public);

                switch (eventArg)
                {
                    case "DispatchInfo":
                        var dispatchInfo = new DispatchInfo();
                        dispatchInfo.Decode(byteArray, ref p);
                        data.Add(dispatchInfo);
                        break;
                    case "DispatchError":
                        data.Add(DispatchError.Decode(byteArray, ref p));
                        break;
                    case "AccountId":
                        var accountId = new AccountId();
                        accountId.Decode(byteArray, ref p);
                        data.Add(accountId);
                        break;
                    case "AccountIndex":
                        data.Add(AccountIndex.Decode(byteArray, ref p));
                        break;
                    case "Balance":
                        var balance = new Balance();
                        balance.Decode(byteArray, ref p);
                        data.Add(balance);
                        break;
                    case "Status":
                        data.Add(Status.Decode(byteArray, ref p));
                        break;
                    case "EraIndex":
                        var eraIndex = new EraIndex();
                        eraIndex.Decode(byteArray, ref p);
                        data.Add(eraIndex);
                        break;
                    case "SessionIndex":
                        var sessionIndex = new SessionIndex();
                        sessionIndex.Decode(byteArray, ref p);
                        data.Add(sessionIndex);
                        break;
                    case "ElectionCompute":
                        data.Add(ElectionCompute.Decode(byteArray, ref p));
                        break;
                    case "PropIndex":
                        var propIndex = new PropIndex();
                        propIndex.Decode(byteArray, ref p);
                        data.Add(propIndex);
                        break;
                    case "ReferendumIndex":
                        var referendumIndex = new ReferendumIndex();
                        referendumIndex.Decode(byteArray, ref p);
                        data.Add(referendumIndex);
                        break;
                    case "VoteThreshold":
                        data.Add(VoteThreshold.Decode(byteArray, ref p));
                        break;
                    case "Hash":
                        var hash = new Hash();
                        hash.Decode(byteArray, ref p);
                        data.Add(hash);
                        break;
                    case "BlockNumber":
                        var blockNumber = new BlockNumber();
                        blockNumber.Decode(byteArray, ref p);
                        data.Add(blockNumber);
                        break;
                    case "ProposalIndex":
                        var proposalIndex = new ProposalIndex();
                        proposalIndex.Decode(byteArray, ref p);
                        data.Add(proposalIndex);
                        break;
                    case "MemberCount":
                        var memberCount = new MemberCount();
                        memberCount.Decode(byteArray, ref p);
                        data.Add(memberCount);
                        break;
                    case "DispatchResult":
                        data.Add(DispatchResult.Decode(byteArray, ref p));
                        break;
                    case "AuthorityList":
                        data.Add(AuthorityList.Decode(byteArray, ref p));
                        break;
                    case "BountyIndex":
                        var bountyIndex = new BountyIndex();
                        bountyIndex.Decode(byteArray, ref p);
                        data.Add(bountyIndex);
                        break;
                    case "AuthorityId":
                        data.Add(AuthorityId.Decode(byteArray, ref p));
                        break;
                    case "Kind":
                        data.Add(Kind.Decode(byteArray, ref p));
                        break;
                    case "OpaqueTimeSlot":
                        data.Add(OpaqueTimeSlot.Decode(byteArray, ref p));
                        break;
                    case "RegistrarIndex":
                        var registrarIndex = new RegistrarIndex();
                        registrarIndex.Decode(byteArray, ref p);
                        data.Add(registrarIndex);
                        break;
                    case "ProxyType":
                        data.Add(ProxyType.Decode(byteArray, ref p));
                        break;
                    case "CallHash":
                        var callHash = new CallHash();
                        callHash.Decode(byteArray, ref p);
                        data.Add(callHash);
                        break;
                    case "Vec<AccountId>":
                        var vecAccountId = new Vec<AccountId>();
                        vecAccountId.Decode(byteArray, ref p);
                        data.Add(vecAccountId);
                        break;
                    case "Vec<u8>":
                        var vecU8 = new Vec<U8>();
                        vecU8.Decode(byteArray, ref p);
                        data.Add(vecU8);
                        break;

                    case "bool":
                    case "u16":
                    case "u32":
                        throw new NotImplementedException(
                            $"Currently event args isn't implemented, please report {eventArg}!");

                    case "Vec<(AccountId, Balance)>":
                    case "sp_std::marker::PhantomData<(AccountId, Event)>":
                    case "Vec<IdentificationTuple>":
                    case "TaskAddress<BlockNumber>":
                    case "Option<Vec<u8>>":
                    case "Timepoint<BlockNumber>":
                    default:
                        throw new NotImplementedException(
                            $"Currently event args isn't implemented, please report {eventArg}!");
                }

            return data.ToArray();
        }
    }

    public class EventRecord
    {
        public BaseEvent Event;
        public Phase Phase;

        public Topics[] Topics;

        public EventRecord(Phase phase, BaseEvent baseEvent)
        {
            Phase = phase;
            Event = baseEvent;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Phase
    {
        public Phase(uint applyExtrinisc)
        {
            ApplyExtrinsic = applyExtrinisc;
            PhaseState = PhaseState.None;
        }

        public uint ApplyExtrinsic { get; set; }

        public PhaseState PhaseState { get; set; }
    }

    public class BaseEvent
    {
        public object[] EventData;

        public string EventName;
        public string ModuleName;

        public BaseEvent(string moduleName, string eventName, object[] eventData)
        {
            ModuleName = moduleName;
            EventName = eventName;
            EventData = eventData;
        }
    }

    public class Topics
    {
    }
}