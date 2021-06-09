using SubstrateNetApi.Model.Types;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Custom;
using SubstrateNetApi.Model.Types.Enum;
using SubstrateNetApi.Model.Types.Struct;
using System.Linq;
using System.Numerics;
using System.Text;

namespace SubstrateNetApi.Model.Calls
{
    public class DotMogCall
    {
        public static GenericExtrinsicCall Claim(string mogwaicoinAddress, string account, string signatureStr)
        {
            var address = new Vec<U8>();
            address.Create(
                Encoding.ASCII.GetBytes(mogwaicoinAddress).ToList().Select(p => {
                    var u = new U8(); u.Create(p); return u;
                }).ToList()
            );

            var accountId = new RawAccountId();
            accountId.Create(Utils.GetPublicKeyFrom(account));

            var signature = new Vec<U8>();
            signature.Create(
                Encoding.ASCII.GetBytes(signatureStr).ToList().Select(p => {
                    var u = new U8(); u.Create(p); return u;
                }).ToList()
            );

            return Claim(address, accountId, signature);
        }

        public static GenericExtrinsicCall Claim(Vec<U8> address, RawAccountId account, Vec<U8> signature)
        {
            return new GenericExtrinsicCall("DotMogBase", "claim", address, account, signature);
        }

        public static GenericExtrinsicCall UpdateClaim(string mogwaicoinAddress, string account, ClaimState claimState, RawBalance balanceAmount)
        {
            var address = new Vec<U8>();
            address.Create(
                Encoding.ASCII.GetBytes(mogwaicoinAddress).ToList().Select(p => {
                    var u = new U8(); u.Create(p); return u;
                }).ToList()
            );

            var accountId = new RawAccountId();
            accountId.Create(Utils.GetPublicKeyFrom(account));

            var state = new EnumType<ClaimState>();
            state.Create(claimState);

            return UpdateClaim(address, accountId, state, balanceAmount);
        }

        public static GenericExtrinsicCall UpdateClaim(Vec<U8> address, RawAccountId account, EnumType<ClaimState> state, RawBalance balance)
        {
            return new GenericExtrinsicCall("DotMogBase", "update_claim", address, account, state, balance);
        }

        public static GenericExtrinsicCall RemoveClaim(string mogwaicoinAddress, string account)
        {
            var address = new Vec<U8>();
            address.Create(
                Encoding.ASCII.GetBytes(mogwaicoinAddress).ToList().Select(p => {
                    var u = new U8(); u.Create(p); return u;
                }).ToList()
            );

            var accountId = new RawAccountId();
            accountId.Create(Utils.GetPublicKeyFrom(account));

            return RemoveClaim(address, accountId);
        }

        public static GenericExtrinsicCall RemoveClaim(Vec<U8> address, RawAccountId account)
        {
            return new GenericExtrinsicCall("DotMogBase", "remove_claim", address, account);
        }

        public static GenericExtrinsicCall BidAuction(Hash mogwaiId, Balance bid)
        {
            return new GenericExtrinsicCall("DotMogModule", "bid_auction", mogwaiId, bid);
        }

        public static GenericExtrinsicCall BreedMogwai(Hash mogwaiId1, Hash mogwaiId2)
        {
            return new GenericExtrinsicCall("DotMogModule", "breed_mogwai", mogwaiId1, mogwaiId2);
        }

        public static GenericExtrinsicCall BuyMogwai(Hash mogwaiId, Balance maxPrice)
        {
            return new GenericExtrinsicCall("DotMogModule", "buy_mogwai", mogwaiId, maxPrice);
        }

        public static GenericExtrinsicCall CreateAuction(Hash mogwaiId, Balance minBid, BlockNumber expiry)
        {
            return new GenericExtrinsicCall("DotMogModule", "create_auction", mogwaiId, minBid, expiry);
        }

        public static GenericExtrinsicCall CreateMogwai()
        {
            return new GenericExtrinsicCall("DotMogModule", "create_mogwai");
        }

        public static GenericExtrinsicCall RemoveMogwai(Hash mogwaiId)
        {
            return new GenericExtrinsicCall("DotMogModule", "remove_mogwai", mogwaiId);
        }

        public static GenericExtrinsicCall SacrificeMogwai(Hash mogwaiId)
        {
            return new GenericExtrinsicCall("DotMogModule", "sacrifice", mogwaiId);
        }

        public static GenericExtrinsicCall SacrificeMogwai(Hash mogwaiId1, Hash mogwaiId2)
        {
            return new GenericExtrinsicCall("DotMogModule", "sacrifice_into", mogwaiId1, mogwaiId2);
        }

        public static GenericExtrinsicCall MorphMogwai(Hash mogwaiId)
        {
            return new GenericExtrinsicCall("DotMogModule", "morph_mogwai", mogwaiId);
        }

        public static GenericExtrinsicCall SetPrice(Hash mogwaiId, Balance newPrice)
        {
            return new GenericExtrinsicCall("DotMogModule", "set_price", mogwaiId, newPrice);
        }

        public static GenericExtrinsicCall Transfer(RawAccountId to, Hash mogwaiId)
        {
            // TODO check if RawAccountId or AccountId ...
            return new GenericExtrinsicCall("DotMogModule", "transfer", to, mogwaiId);
        }

        public static GenericExtrinsicCall UpdateConfig(U8 index, U8 valueOpt)
        {
            var optionByte = new U8();
            optionByte.Create(1);
            return new GenericExtrinsicCall("DotMogModule", "update_config", index, optionByte, valueOpt);
        }

        public static GenericExtrinsicCall UpdateConfig(U8 index)
        {
            var optionByte = new U8();
            optionByte.Create(0);
            return new GenericExtrinsicCall("DotMogModule", "update_config", index, optionByte);
        }
    }
}
