using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Enum;

namespace SubstrateNetApi.Model.Types.Struct
{
    public partial class BaseEvent : StructType
    {
        public override string Name() => "Event";

        private int _size;
        public override int Size() => _size;

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

            ModuleIndex = new U8();
            ModuleIndex.Decode(byteArray, ref p);

            var module = _metaData.Modules[ModuleIndex.Value];
            ModuleName = module.Name;

            EventIndex = new U8();
            EventIndex.Decode(byteArray, ref p);

            var moduleEvent = module.Events[EventIndex.Value];
            EventName = moduleEvent.Name;

            EventArgs = new IType[moduleEvent.EventArgs.Length];
            for (var i = 0; i < moduleEvent.EventArgs.Length; i++)
            {
                var eventArgStr = moduleEvent.EventArgs[i];
                EventArgs[i] = BaseEvent.TypeMapper(eventArgStr, byteArray, ref p);
            }
        }

        [JsonIgnore]
        public U8 ModuleIndex;
        public string ModuleName;
        [JsonIgnore]
        public U8 EventIndex;
        public string EventName;
        public IType[] EventArgs;
    }

    public partial class BaseEvent
    {
        private static bool TryGetType(string typeStr, out Type type)
        {
            type = null;

            var typeNamespaces = new List<string>()
            {
                new U8().GetType().Namespace,
                new DispatchClass().GetType().Namespace,
                new DispatchInfo().GetType().Namespace
            }.ToArray();

            foreach (var typeNameSpace in typeNamespaces)
            {
                type = Type.GetType($"{typeNameSpace}.{typeStr}, SubstrateNetApi");
                if (type != null)
                {
                    return true;
                }
            }

            return false;
        }

        private static IType TypeMapper(string typeStr, byte[] byteArray, ref int p)
        {
            // TODO: check if we can use reflection for this also in Unity3D

            if (TryGetType(typeStr, out Type type))
            {
                var iType = (IType)Activator.CreateInstance(type);
                iType.Decode(byteArray, ref p);
                return iType;
            }

            throw new NotImplementedException(
                $"Currently event args isn't implemented, please report {typeStr}!");
            
            //IType result;
            //switch (typeStr)
            //{
            //    case "DispatchInfo":
            //        result = new DispatchInfo();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "AccountIndex":
            //        result = new AccountIndex();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "AccountId":
            //        result = new AccountId();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "Balance":
            //        result = new Balance();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "EraIndex":
            //        result = new EraIndex();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "SessionIndex":
            //        result = new SessionIndex();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "PropIndex":
            //        result = new PropIndex();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "ReferendumIndex":
            //        result = new ReferendumIndex();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "Hash":
            //        result = new Hash();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "BlockNumber":
            //        result = new BlockNumber();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "ProposalIndex":
            //        result = new ProposalIndex();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "MemberCount":
            //        result = new MemberCount();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "BountyIndex":
            //        result = new BountyIndex();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "RegistrarIndex":
            //        result = new RegistrarIndex();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "CallHash":
            //        result = new CallHash();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "Vec<AccountId>":
            //        result = new Vec<AccountId>();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "Vec<u8>":
            //        result = new Vec<U8>();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "AuthorityList":
            //        result = new AuthorityList();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "AuthorityId":
            //        result = new AuthorityId();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "ElectionCompute":
            //        result = new EnumType<ElectionCompute>();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "VoteThreshold":
            //        result = new EnumType<VoteThreshold>();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "OpaqueTimeSlot":
            //        result = new OpaqueTimeSlot();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "Vec<(AccountId, Balance)>":
            //        result = new Vec<RustTuple<AccountId, Balance>>();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "ProxyType":
            //        result = new EnumType<ProxyType>();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "Status":
            //        result = new EnumType<BalanceStatus>();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "TaskAddress<BlockNumber>":
            //        result = new TaskAddress();
            //        result.Decode(byteArray, ref p);
            //        return result;
            //    case "Timepoint<BlockNumber>":
            //        result = new Timepoint();
            //        result.Decode(byteArray, ref p);
            //        return result;

            //    case "DispatchResult":
            //    case "Kind":
            //    case "DispatchError":
            //        throw new NotImplementedException(
            //            $"Currently event args isn't implemented, please report {typeStr}!");

            //    case "bool":
            //    case "u16":
            //    case "u32":
            //        throw new NotImplementedException(
            //            $"Currently event args isn't implemented, please report {typeStr}!");


            //    case "sp_std::marker::PhantomData<(AccountId, Event)>":
            //    case "Vec<IdentificationTuple>":
            //    case "Option<Vec<u8>>":
            //    default:
            //        throw new NotImplementedException(
            //            $"Currently event args isn't implemented, please report {typeStr}!");
            //}
        }
    }
}