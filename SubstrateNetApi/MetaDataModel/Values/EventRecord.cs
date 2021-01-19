using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SubstrateNetApi.MetaDataModel.Values
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

                var module = metaData.Modules[(int)byteArray.Span.Slice(p, 1)[0]];
                p += 1;

                var moduleEvent = module.Events[(int)byteArray.Span.Slice(p, 1)[0]];
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
            foreach(var eventArg in eventArgs)
            {
                switch(eventArg)
                {
                    case "DispatchInfo":
                        data.Add(DispatchInfo.Decode(byteArray, ref p));
                        break;
                    case "AccountId":
                        data.Add(AccountId.Decode(byteArray, ref p));
                        break;
                    case "Hash":
                        data.Add(Hash.Decode(byteArray, ref p));
                        break;
                    case "Balance":
                        data.Add(Balance.Decode(byteArray, ref p));
                        break;
                    default:
                        throw new NotImplementedException($"Unknown eventargs please calculate the size of it {eventArg}");
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
}
