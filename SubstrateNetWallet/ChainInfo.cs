using System;
using Newtonsoft.Json;
using SubstrateNetApi.Model.Rpc;

namespace SubstrateNetWallet
{
    /// <summary>
    /// Chain Info
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ChainInfo : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChainInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="version">The version.</param>
        /// <param name="chain">The chain.</param>
        /// <param name="runtime">The runtime.</param>
        public ChainInfo(string name, string version, string chain, RuntimeVersion runtime)
        {
            Name = name;
            Version = version;
            Chain = chain;
            RuntimeVersion = runtime;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; }

        /// <summary>
        /// Gets the chain.
        /// </summary>
        /// <value>
        /// The chain.
        /// </value>
        public string Chain { get; }

        /// <summary>
        /// Gets the runtime version.
        /// </summary>
        /// <value>
        /// The runtime version.
        /// </value>
        public RuntimeVersion RuntimeVersion { get; }

        /// <summary>
        /// Gets the block number.
        /// </summary>
        /// <value>
        /// The block number.
        /// </value>
        public ulong BlockNumber { get; private set; }

        internal void UpdateFinalizedHeader(Header header)
        {
            BlockNumber = header.Number.Value;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}