using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Calls
{
    public class DotMogCall
    {
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

        public static GenericExtrinsicCall MorphMogwai(Hash mogwaiId)
        {
            return new GenericExtrinsicCall("DotMogModule", "morph_mogwai", mogwaiId);
        }

        public static GenericExtrinsicCall SetPrice(Hash mogwaiId, Balance newPrice)
        {
            return new GenericExtrinsicCall("DotMogModule", "set_price", mogwaiId, newPrice);
        }

        public static GenericExtrinsicCall Transfer(AccountId to, Hash mogwaiId)
        {
            return new GenericExtrinsicCall("DotMogModule", "transfer", to, mogwaiId);
        }

        public static GenericExtrinsicCall UpdateConfig(U8 index, U8 valueOpt)
        {
            return new GenericExtrinsicCall("DotMogModule", "update_config", index, valueOpt);
        }

        public static GenericExtrinsicCall UpdateConfig(U8 index)
        {
            return new GenericExtrinsicCall("DotMogModule", "update_config", index);
        }
    }
}
