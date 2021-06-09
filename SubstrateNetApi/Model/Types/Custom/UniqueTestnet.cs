using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Enum;
using SubstrateNetApi.Model.Types.Struct;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SubstrateNetApi.Model.Types.Custom
{
    #region BASE_TYPES

    #endregion

    #region ENUM_TYPES

    public enum AccessMode
    {
        Normal,
        WhiteList
    }

    public enum SchemaVersion
    {
        ImageURL,
        Unique
    }

    public enum CollectionMode
    {
        //TODO
    }

    public enum SponsorshipState
    {
        //TODO
    }

    public enum CreateItemData
    {
        //TODO
    }

    #endregion

    #region STRUCT_TYPES

    public class Ownership : StructType
    {
        public override string Name() => "Ownership<T::AccountId>";
        
        private int _size;
        
        public override int Size() => _size;
        
        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Owner = new AccountId();
            Owner.Decode(byteArray, ref p);

            Fraction = new U128();
            Fraction.Decode(byteArray, ref p);

            _size = p - start;
        }
        
        public AccountId Owner { get; private set; }
        public U128 Fraction { get; private set; }
    }

    public class FungibleItemType : StructType
    {
        public override string Name() => "FungibleItemType";
        
        private int _size;
        
        public override int Size() => _size;
        
        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Value = new U128();
            Value.Decode(byteArray, ref p);

            _size = p - start;
        }
        
        public U128 Value { get; private set; }
    }

    public class NftItemType : StructType
    {
        public override string Name() => "NftItemType<T::AccountId>";

        private int _size;

        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Owner = new AccountId();
            Owner.Decode(byteArray, ref p);

            ConstData = new Vec<U8>();
            ConstData.Decode(byteArray, ref p);

            VariableData = new Vec<U8>();
            VariableData.Decode(byteArray, ref p);

            _size = p - start;
        }

        public AccountId Owner { get; private set; }
        public Vec<U8> ConstData { get; private set; }
        public Vec<U8> VariableData { get; private set; }
    }
    
    public class ReFungibleItemType : StructType
    {
        public override string Name() => "ReFungibleItemType<T::AccountId>";

        private int _size;

        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Owner = new Vec<Ownership>();
            Owner.Decode(byteArray, ref p);

            ConstData = new Vec<U8>();
            ConstData.Decode(byteArray, ref p);

            VariableData = new Vec<U8>();
            VariableData.Decode(byteArray, ref p);

            _size = p - start;
        }

        public Vec<Ownership> Owner { get; private set; }
        public Vec<U8> ConstData { get; private set; }
        public Vec<U8> VariableData { get; private set; }
    }

    public class Collection : StructType
    {
        public override string Name() => "Collection<T>";

        private int _size;

        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Owner = new AccountId();
            Owner.Decode(byteArray, ref p);

            Mode = new EnumType<CollectionMode>();
            Mode.Decode(byteArray, ref p);

            Access = new EnumType<AccessMode>();
            Access.Decode(byteArray, ref p);

            DecimalPoints = new DecimalPoints();
            DecimalPoints.Decode(byteArray, ref p);

            CollectionName = new Vec<U16>();
            CollectionName.Decode(byteArray, ref p);

            Description = new Vec<U16>();
            Description.Decode(byteArray, ref p);

            TokenPrefix = new Vec<U8>();
            TokenPrefix.Decode(byteArray, ref p);

            MintMode = new Bool();
            MintMode.Decode(byteArray, ref p);

            OffchainSchema = new Vec<U8>();
            OffchainSchema.Decode(byteArray, ref p);

            SchemaVersion = new EnumType<SchemaVersion>();
            SchemaVersion.Decode(byteArray, ref p);

            Sponsorship = new EnumType<SponsorshipState>();
            Sponsorship.Decode(byteArray, ref p);

            Limits = new CollectionLimits();
            Limits.Decode(byteArray, ref p);

            VariableOnChainSchema = new Vec<U8>();
            VariableOnChainSchema.Decode(byteArray, ref p);

            ConstOnChainSchema = new Vec<U8>();
            ConstOnChainSchema.Decode(byteArray, ref p);

            _size = p - start;
        }

        public AccountId Owner { get; private set; }
        public EnumType<CollectionMode> Mode { get; private set; }
        public EnumType<AccessMode> Access { get; private set; }
        public DecimalPoints DecimalPoints { get; private set; }
        public Vec<U16> CollectionName { get; private set; }
        public Vec<U16> Description { get; private set; }
        public Vec<U8> TokenPrefix { get; private set; }
        public Bool MintMode { get; private set; }
        public Vec<U8> OffchainSchema { get; private set; }
        public EnumType<SchemaVersion> SchemaVersion { get; private set; }
        public EnumType<SponsorshipState> Sponsorship { get; private set; }
        public CollectionLimits Limits { get; private set; }
        public Vec<U8> VariableOnChainSchema { get; private set; }
        public Vec<U8> ConstOnChainSchema { get; private set; }
    }

    public class CollectionLimits : StructType
    {
        public override string Name() => "CollectionLimits<T::BlockNumber>";

        private int _size;

        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            AccountTokenOwnershipLimit = new U32();
            AccountTokenOwnershipLimit.Decode(byteArray, ref p);

            SponsoredDataSize = new U32();
            SponsoredDataSize.Decode(byteArray, ref p);

            SponsoredDataRateLimit = new Option<BlockNumber>();
            SponsoredDataRateLimit.Decode(byteArray, ref p);

            TokenLimit = new U32();
            TokenLimit.Decode(byteArray, ref p);

            SponsorTimeout = new U32();
            SponsorTimeout.Decode(byteArray, ref p);

            OwnerCanTransfer = new Bool();
            OwnerCanTransfer.Decode(byteArray, ref p);

            OwnerCanDestroy = new Bool();
            OwnerCanDestroy.Decode(byteArray, ref p);

            _size = p - start;
        }

        public U32 AccountTokenOwnershipLimit { get; private set; }
        public U32 SponsoredDataSize { get; private set; }
        public Option<BlockNumber> SponsoredDataRateLimit { get; private set; }
        public U32 TokenLimit { get; private set; }
        public U32 SponsorTimeout { get; private set; }
        public Bool OwnerCanTransfer { get; private set; }
        public Bool OwnerCanDestroy { get; private set; }
    }

    public class CreateNftData : StructType
    {
        public override string Name() => "CreateNftData";

        private int _size;

        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            ConstData = new Vec<U8>();
            ConstData.Decode(byteArray, ref p);

            VariableData = new Vec<U8>();
            VariableData.Decode(byteArray, ref p);

            _size = p - start;
        }

        public Vec<U8> ConstData { get; private set; }
        public Vec<U8> VariableData { get; private set; }
    }

    public class CreateFungibleData : StructType
    {
        public override string Name() => "CreateFungibleData";

        private int _size;

        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Value = new U128();
            Value.Decode(byteArray, ref p);

            _size = p - start;
        }

        public U128 Value { get; private set; }
    }

    public class CreateReFungibleData : StructType
    {
        public override string Name() => "CreateReFungibleData";

        private int _size;

        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            ConstData = new Vec<U8>();
            ConstData.Decode(byteArray, ref p);

            VariableData = new Vec<U8>();
            VariableData.Decode(byteArray, ref p);

            Pieces = new U128();
            Pieces.Decode(byteArray, ref p);

            _size = p - start;
        }

        public Vec<U8> ConstData { get; private set; }
        public Vec<U8> VariableData { get; private set; }
        public U128 Pieces { get; private set; }
    }

    public class ChainLimits : StructType
    {
        public override string Name() => "CreateReFungibleData";

        private int _size;

        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            CollectionNumbersLimit = new U32();
            CollectionNumbersLimit.Decode(byteArray, ref p);

            AccountTokenOwnershipLimit = new U32();
            AccountTokenOwnershipLimit.Decode(byteArray, ref p);

            CollectionAdminsLimit = new U64();
            CollectionAdminsLimit.Decode(byteArray, ref p);

            CustomDataLimit = new U32();
            CustomDataLimit.Decode(byteArray, ref p);

            NftSponsorTimeout = new U32();
            NftSponsorTimeout.Decode(byteArray, ref p);

            FungibleSponsorTimeout = new U32();
            FungibleSponsorTimeout.Decode(byteArray, ref p);

            RefungibleSponsorTimeout = new U32();
            RefungibleSponsorTimeout.Decode(byteArray, ref p);

            OffchainSchemaLimit = new U32();
            OffchainSchemaLimit.Decode(byteArray, ref p);

            VariableOnChainSchemaLimit = new U32();
            VariableOnChainSchemaLimit.Decode(byteArray, ref p);

            ConstOnChainSchemaLimit = new U32();
            ConstOnChainSchemaLimit.Decode(byteArray, ref p);

            _size = p - start;
        }

        public U32 CollectionNumbersLimit { get; private set; }
        public U32 AccountTokenOwnershipLimit { get; private set; }
        public U64 CollectionAdminsLimit { get; private set; }
        public U32 CustomDataLimit { get; private set; }
        public U32 NftSponsorTimeout { get; private set; }
        public U32 FungibleSponsorTimeout { get; private set; }
        public U32 RefungibleSponsorTimeout { get; private set; }
        public U32 OffchainSchemaLimit { get; private set; }
        public U32 VariableOnChainSchemaLimit { get; private set; }
        public U32 ConstOnChainSchemaLimit { get; private set; }
    }

    #endregion

    #region INHERITED_TYPES

    public class DecimalPoints : U8
    {
        public override string Name() => "DecimalPoints";
    }

    public class RawData : Vec<U8>
    {
        public override string Name() => "RawData";
    }

    public class Address : AccountId
    {
        public override string Name() => "Address";
    }

    public class LookupSource : AccountId
    {
        public override string Name() => "LookupSource";
    }

    public class Weight : U64
    {
        public override string Name() => "Weight";
    }

    public class CollectionId : U32
    {
        public override string Name() => "CollectionId";
    }

    public class TokenId : U32
    {
        public override string Name() => "TokenId";
    }

    #endregion


}
