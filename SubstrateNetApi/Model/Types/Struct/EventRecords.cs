using System;
using System.Collections.Generic;
using SubstrateNetApi.Model.Meta;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class EventRecords : BaseType
    {
        public override string TypeName() => $"Vec<EventRecord<T::Event, T::Hash>>";

        private MetaData _metaData;

        public EventRecords()
        {
        }

        public EventRecords(MetaData metaData)
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

            var list = new List<EventRecord>();

            var length = CompactInteger.Decode(byteArray, ref p);
            for (var i = 0; i < length; i++)
            {
                var t = new EventRecord(_metaData);
                t.Decode(byteArray, ref p);
                list.Add(t);
            }

            Bytes = byteArray;
            Value = list;

            TypeSize = p - start;
        }

        public List<EventRecord> Value { get; internal set; }
    }
}