using System;

namespace SubstrateNetApi.Model.Types
{
    public partial class AccountId
    {
        public static AccountId Decode(Memory<byte> byteArray, ref int p)
        {
            var accountId = new AccountId(byteArray.Span.Slice(p, 32).ToArray());
            p += 32;
            return accountId;
        }
    }

    public partial class AccountIndex
    {
        public static AccountIndex Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class AuthorityId
    {
        public static AuthorityId Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class AuthorityList
    {
        public static AuthorityList Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class Balance
    {
        public static Balance Decode(Memory<byte> byteArray, ref int p)
        {
            var balance = new Balance(byteArray.Span.Slice(p, 16).ToArray());
            p += 16;
            return balance;
        }
    }

    public partial class BountyIndex
    {
        public static BountyIndex Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class BlockNumber
    {
        public static BlockNumber Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class CallHash
    {
        public static CallHash Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class DispatchError
    {
        public static DispatchError Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class DispatchInfo
    {
        public static DispatchInfo Decode(Memory<byte> byteArray, ref int p)
        {
            var weight = BitConverter
                .ToUInt64(byteArray.Span.Slice(p, 8).ToArray(), 0);
            p += 8; // weight

            var dispatchClass = (Enums)byteArray.Span.Slice(p, 1)[0];
            p += 1; // class

            var pays = (Pays)byteArray.Span.Slice(p, 1)[0];
            p += 1; // paysFee

            return new DispatchInfo(weight, dispatchClass, pays);
        }
    }

    public partial class DispatchResult
    {
        public static DispatchResult Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class ElectionCompute
    {
        public static ElectionCompute Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class EraIndex
    {
        public static EraIndex Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class Hash
    {
        public static Hash Decode(Memory<byte> byteArray, ref int p)
        {
            var hash = new Hash(byteArray.Span.Slice(p, 32).ToArray());
            p += 32;
            return hash;
        }
    }

    public partial class Kind
    {
        public static Kind Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class MemberCount
    {
        public static MemberCount Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class Moment
    {
        public byte[] Encode()
        {
            throw new NotImplementedException();
        }
    }

    public partial class OpaqueTimeSlot
    {
        public static OpaqueTimeSlot Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class PropIndex
    {
        public static PropIndex Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class ProposalIndex
    {
        public static ProposalIndex Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class ProxyType
    {
        public static ProxyType Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class Status
    {
        public static Status Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class ReferendumIndex
    {
        public static ReferendumIndex Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class RegistrarIndex
    {
        public static RegistrarIndex Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class VoteThreshold
    {
        public static VoteThreshold Decode(Memory<byte> byteArray, ref int p)
        {
            throw new NotImplementedException();
        }
    }

    public partial class U8
    {
        public static U8 Decode(Memory<byte> byteArray, ref int p)
        {
            var size = 1;
            var result = byteArray.Span.Slice(p, size)[0];
            p += size;
            return new U8(result);
        }
    }

    public partial class U16
    {
        public static U16 Decode(Memory<byte> byteArray, ref int p)
        {
            var size = 2;
            var result = BitConverter
                .ToUInt16(byteArray.Span.Slice(p, size).ToArray(), 0);
            p += size;
            return new U16(result);
        }
    }

    public partial class U32
    {
        public static U32 Decode(Memory<byte> byteArray, ref int p)
        {
            var size = 4;
            var result = BitConverter
                .ToUInt32(byteArray.Span.Slice(p, size).ToArray(), 0);
            p += size;
            return new U32(result);
        }
    }

    public partial class U64
    {
        public static U64 Decode(Memory<byte> byteArray, ref int p)
        {
            var size = 8;
            var result = BitConverter
                .ToUInt64(byteArray.Span.Slice(p, size).ToArray(), 0);
            p += size;
            return new U64(result);
        }
    }
}
