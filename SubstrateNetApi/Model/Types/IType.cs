using System;

namespace SubstrateNetApi.Model.Types
{
    /// <summary>
    /// Reference to the polkadot js types implementation
    /// https://github.com/polkadot-js/api/tree/master/packages/types/src
    /// </summary>
    public interface IType
    {
        string Name();

        int Size();

        byte[] Encode();

        void Decode(byte[] byteArray, ref int p);

        void Create(string str);

        void CreateFromJson(string str);

        void Create(byte[] byteArray);
    }
}