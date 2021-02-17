namespace SubstrateNetApi
{
    /// <summary> Interface for type converter. </summary>
    /// <remarks> 19.09.2020. </remarks>
    public interface ITypeConverter
    {
        /// <summary> Gets the name of the type. </summary>
        /// <value> The name of the type. </value>
        string TypeName { get; }

        /// <summary> Creates a new object. </summary>
        /// <param name="value"> The value. </param>
        /// <returns> An object. </returns>
        object Create(string value);
    }
}