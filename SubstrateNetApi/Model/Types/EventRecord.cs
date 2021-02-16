using Newtonsoft.Json;
using SubstrateNetApi.Model.Meta;
using System;
using System.Collections.Generic;

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
            var byteArray = Utils.HexToByteArray(eventStr).AsMemory();

            int p = 0;
            var eventCount = CompactInteger.Decode(byteArray.ToArray(), ref p).Value;

            // skipping one byte, unclear why!
            EventRecord[] events = new EventRecord[(int)eventCount];
            for (int i = 0; i < events.Length; i++)
            {
                var startByte = byteArray.Span.Slice(p, 1).ToArray()[0];
                p += 1;

                var phaseArray = byteArray.Span.Slice(p, 4).ToArray();
                var phase = BitConverter.ToUInt32(phaseArray, 0);
                p += 4;

                var module = metaData.Modules[byteArray.Span.Slice(p, 1)[0]];
                p += 1;

                var moduleEvent = module.Events[byteArray.Span.Slice(p, 1)[0]];
                p += 1;

                var eventData = GetEventDataSize(moduleEvent.EventArgs, byteArray, ref p);

                var endByte = byteArray.Span.Slice(p, 1).ToArray()[0];
                p += 1;

                events[i] = new EventRecord(
                    new Phase(phase),
                    new BaseEvent(module.Name, moduleEvent.Name, eventData));
            }

            return new EventRecords(events);
        }

        private static object[] GetEventDataSize(string[] eventArgs, Memory<byte> byteArray, ref int p)
        {
            List<object> data = new List<object>();

            foreach (var eventArg in eventArgs)
            {
                // probablly a bad idear for unity
                // -------------------------------
                //object[] arguments = new object[] { byteArray, p };
                //MethodInfo method = Type.GetType($"SubstrateNetApi.MetaDataModel.Values.{eventArg}, SubstrateNetApi")
                //    .GetMethod("Decode", BindingFlags.Static | BindingFlags.Public);

                switch (eventArg)
                {
                    case "DispatchInfo":
                        data.Add(DispatchInfo.Decode(byteArray, ref p));
                        break;
                    case "DispatchError":
                        data.Add(DispatchError.Decode(byteArray, ref p));
                        break;
                    case "AccountId":
                        var accountId = new AccountId();
                        accountId.Decode(byteArray.ToArray(), ref p);
                        data.Add(accountId);
                        break;
                    case "AccountIndex":
                        data.Add(AccountIndex.Decode(byteArray, ref p));
                        break;
                    case "Balance":
                        var balance = new Balance();
                        balance.Decode(byteArray.ToArray(), ref p);
                        data.Add(balance);
                        break;
                    case "Status":
                        data.Add(Status.Decode(byteArray, ref p));
                        break;
                    case "EraIndex":
                        data.Add(EraIndex.Decode(byteArray, ref p));
                        break;
                    case "SessionIndex":
                        var sessionIndex = new SessionIndex();
                        sessionIndex.Decode(byteArray.ToArray(), ref p);
                        data.Add(sessionIndex);
                        break;
                    case "ElectionCompute":
                        data.Add(ElectionCompute.Decode(byteArray, ref p));
                        break;
                    case "PropIndex":
                        data.Add(PropIndex.Decode(byteArray, ref p));
                        break;
                    case "ReferendumIndex":
                        data.Add(ReferendumIndex.Decode(byteArray, ref p));
                        break;
                    case "VoteThreshold":
                        data.Add(VoteThreshold.Decode(byteArray, ref p));
                        break;
                    case "Hash":
                        var hash = new Hash();
                        hash.Decode(byteArray.ToArray(), ref p);
                        data.Add(hash);
                        break;
                    case "BlockNumber":
                        var blockNumber = new BlockNumber();
                        blockNumber.Decode(byteArray.ToArray(), ref p);
                        data.Add(blockNumber);
                        break;
                    case "ProposalIndex":
                        data.Add(ProposalIndex.Decode(byteArray, ref p));
                        break;
                    case "MemberCount":
                        data.Add(MemberCount.Decode(byteArray, ref p));
                        break;
                    case "DispatchResult":
                        data.Add(DispatchResult.Decode(byteArray, ref p));
                        break;
                    case "AuthorityList":
                        data.Add(AuthorityList.Decode(byteArray, ref p));
                        break;
                    case "BountyIndex":
                        data.Add(BountyIndex.Decode(byteArray, ref p));
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
                        data.Add(RegistrarIndex.Decode(byteArray, ref p));
                        break;
                    case "ProxyType":
                        data.Add(ProxyType.Decode(byteArray, ref p));
                        break;
                    case "CallHash":
                        data.Add(CallHash.Decode(byteArray, ref p));
                        break;

                    case "bool":
                    case "u16":
                    case "u32":
                        throw new NotImplementedException($"Currently event args isn't implemented, please report {eventArg}!");

                    case "Vec<AccountId>":
                    case "Vec<(AccountId, Balance)>":
                    case "sp_std::marker::PhantomData<(AccountId, Event)>":
                    case "Vec<u8>":
                    case "Vec<IdentificationTuple>":
                    case "TaskAddress<BlockNumber>":
                    case "Option<Vec<u8>>":
                    case "Timepoint<BlockNumber>":
                    default:
                        throw new NotImplementedException($"Currently event args isn't implemented, please report {eventArg}!");
                }
            }
            return data.ToArray();
        }
    }

    public class EventRecord
    {
        public Phase Phase;

        public BaseEvent Event;

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
        public uint ApplyExtrinsic { get; set; }

        public PhaseState PhaseState { get; set; }

        public Phase(uint applyExtrinisc)
        {
            ApplyExtrinsic = applyExtrinisc;
            PhaseState = PhaseState.None;
        }
    }

    public class BaseEvent
    {
        public string ModuleName;

        public string EventName;

        public object[] EventData;
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
