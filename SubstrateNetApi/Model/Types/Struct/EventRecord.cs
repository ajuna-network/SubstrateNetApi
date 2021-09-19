using System;
using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class EventRecord : BaseType
    {
        public override string TypeName() => "EventRecord<T::Event, T::Hash>";

        private MetaData _metaData;

        public EventRecord()
        {
        }

        public EventRecord(MetaData metaData)
        {
            _metaData = metaData;
        }

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            if (_metaData is null)
            {
                throw new NotImplementedException("Need MetaData in ctor to decode.");
            }

            var start = p;

            Phase = new Phase();
            Phase.Decode(byteArray, ref p);

            BaseEvent = new BaseEvent(_metaData);
            BaseEvent.Decode(byteArray, ref p);

            Topics = new Vec<Topic>();
            Topics.Decode(byteArray, ref p);

            TypeSize = p - start;
        }

        public Phase Phase;
        public BaseEvent BaseEvent;
        public Vec<Topic> Topics;
    }
}
