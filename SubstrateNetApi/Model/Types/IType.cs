namespace SubstrateNetApi.Model.Types
{
    /// <summary>
    ///     Reference to the polkadot js types implementation
    ///     https://github.com/polkadot-js/api/tree/master/packages/types/src
    /// </summary>
    public interface IType
    {
        /// <summary>
        /// Names this instance.
        /// </summary>
        /// <returns></returns>
        string TypeName();

        /// <summary>
        /// Sizes this instance.
        /// </summary>
        /// <returns></returns>
        int TypeSize { get; set; }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        /// <returns></returns>
        byte[] Encode();

        /// <summary>
        /// Decodes the specified byte array.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <param name="p">The p.</param>
        void Decode(byte[] byteArray, ref int p);

        /// <summary>
        /// Creates the specified string.
        /// </summary>
        /// <param name="str">The string.</param>
        void Create(string str);

        /// <summary>
        /// Creates from json.
        /// </summary>
        /// <param name="str">The string.</param>
        void CreateFromJson(string str);

        /// <summary>
        /// Creates the specified byte array.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        void Create(byte[] byteArray);

        /// <summary>
        /// News this instance.
        /// </summary>
        /// <returns></returns>
        IType New();
    }
}