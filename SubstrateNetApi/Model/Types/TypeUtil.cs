using System;
using System.Collections.Generic;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Enum;
using SubstrateNetApi.Model.Types.Struct;

namespace SubstrateNetApi.Model.Types
{
    public class TypeUtil
    {
        /// <summary>
        /// Try get the type from string, parsing through the three namespaces which currently
        /// implement types, base, enum and struct. 
        /// </summary>
        /// <param name="typeStr">String naming the type.</param>
        /// <param name="type">If true, Type of the found class.</param>
        /// <returns></returns>
        public static bool TryGetType(string typeStr, out Type type)
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

        /// <summary>
        /// Mapper to extract a know type out of a bytearray position, including pointer position.
        /// </summary>
        /// <param name="typeStr">String naming the type.</param>
        /// <param name="byteArray">Byte array to read the type from.</param>
        /// <param name="p">Pointer of the reading position.</param>
        /// <returns></returns>
        public static IType Mapper(string typeStr, byte[] byteArray, ref int p)
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