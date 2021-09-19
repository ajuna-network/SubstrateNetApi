using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Enum;
using SubstrateNetApi.Model.Types.Primitive;

namespace SubstrateNetApi.Model.Types.Struct
{
    public partial class BaseEvent : BaseType
    {
        public override string TypeName() => "Event";

        private readonly MetaData _metaData;
        public BaseEvent() { }

        public BaseEvent(MetaData metaData)
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

            ModuleIndex = new PrimU8();
            ModuleIndex.Decode(byteArray, ref p);

            var module = _metaData.Modules[ModuleIndex.Value];
            ModuleName = module.Name;

            EventIndex = new PrimU8();
            EventIndex.Decode(byteArray, ref p);

            var moduleEvent = module.Events[EventIndex.Value];
            EventName = moduleEvent.Name;

            EventArgs = new IType[moduleEvent.EventArgs.Length];
            for (var i = 0; i < moduleEvent.EventArgs.Length; i++)
            {
                var eventArgStr = moduleEvent.EventArgs[i];
                EventArgs[i] = TypeUtil.Mapper(eventArgStr, byteArray, ref p);
            }
        }

        [JsonIgnore]
        public PrimU8 ModuleIndex;
        public string ModuleName;
        [JsonIgnore]
        public PrimU8 EventIndex;
        public string EventName;
        public IType[] EventArgs;
    }
}