//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.Custom.Runtime;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Composite;
using SubstrateNetApi.Model.Types.Enum;
using SubstrateNetApi.Model.Types.Primitive;
using SubstrateNetApi.Model.Types.Sequence;
using System;
using System.Collections.Generic;


namespace SubstrateNetApi.Model.Types.Composite
{
    
    
    /// <summary>
    /// >> Path: frame_system.EventRecord
    /// </summary>
    public sealed class EventRecord : BaseType
    {
        
        private EnumPhase _phase;
        
        private EnumNodeEvent _event;
        
        private BaseVec<H256> _topics;
        
        public EnumPhase Phase
        {
            get
            {
                return this._phase;
            }
            set
            {
                this._phase = value;
            }
        }
        
        public EnumNodeEvent Event
        {
            get
            {
                return this._event;
            }
            set
            {
                this._event = value;
            }
        }
        
        public BaseVec<H256> Topics
        {
            get
            {
                return this._topics;
            }
            set
            {
                this._topics = value;
            }
        }
        
        public override string TypeName()
        {
            return "EventRecord";
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Phase.Encode());
            result.AddRange(Event.Encode());
            result.AddRange(Topics.Encode());
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Phase = new EnumPhase();
            Phase.Decode(byteArray, ref p);
            Event = new EnumNodeEvent();
            Event.Decode(byteArray, ref p);
            Topics = new BaseVec<H256>();
            Topics.Decode(byteArray, ref p);
            TypeSize = p - start;
        }
    }
}
