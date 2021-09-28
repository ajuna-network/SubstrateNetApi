//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SubstrateNetApi.Model.Extrinsics;
using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.PrimitiveTypes;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace SubstrateNetApi.Model.PalletMmr
{
    
    
    public sealed class MmrStorage
    {
        
        // Substrate client for the storage calls.
        private SubstrateNetApi.SubstrateClient _client;
        
        public MmrStorage(SubstrateNetApi.SubstrateClient client)
        {
            this._client = client;
        }
        
        public static string RootHashParams()
        {
            return RequestGenerator.GetStorage("Mmr", "RootHash", SubstrateNetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> RootHash
        ///  Latest MMR Root hash.
        /// </summary>
        public async Task<SubstrateNetApi.Model.PrimitiveTypes.H256> RootHash(CancellationToken token)
        {
            string parameters = MmrStorage.RootHashParams();
            return await _client.GetStorageAsync<SubstrateNetApi.Model.PrimitiveTypes.H256>(parameters, token);
        }
        
        public static string NumberOfLeavesParams()
        {
            return RequestGenerator.GetStorage("Mmr", "NumberOfLeaves", SubstrateNetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> NumberOfLeaves
        ///  Current size of the MMR (number of leaves).
        /// </summary>
        public async Task<SubstrateNetApi.Model.Types.Primitive.U64> NumberOfLeaves(CancellationToken token)
        {
            string parameters = MmrStorage.NumberOfLeavesParams();
            return await _client.GetStorageAsync<SubstrateNetApi.Model.Types.Primitive.U64>(parameters, token);
        }
        
        public static string NodesParams(SubstrateNetApi.Model.Types.Primitive.U64 key)
        {
            return RequestGenerator.GetStorage("Mmr", "Nodes", SubstrateNetApi.Model.Meta.Storage.Type.Map, new SubstrateNetApi.Model.Meta.Storage.Hasher[] {
                        SubstrateNetApi.Model.Meta.Storage.Hasher.Identity}, new SubstrateNetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> Nodes
        ///  Hashes of the nodes in the MMR.
        /// 
        ///  Note this collection only contains MMR peaks, the inner nodes (and leaves)
        ///  are pruned and only stored in the Offchain DB.
        /// </summary>
        public async Task<SubstrateNetApi.Model.PrimitiveTypes.H256> Nodes(SubstrateNetApi.Model.Types.Primitive.U64 key, CancellationToken token)
        {
            string parameters = MmrStorage.NodesParams(key);
            return await _client.GetStorageAsync<SubstrateNetApi.Model.PrimitiveTypes.H256>(parameters, token);
        }
    }
    
    public sealed class MmrCalls
    {
    }
}
