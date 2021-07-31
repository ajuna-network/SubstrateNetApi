using System;
using System.IO;

namespace SubstrateNetWallet
{
    public static class SystemInteraction
    {
        /// <summary>
        /// Gets or sets the read data.
        /// </summary>
        /// <value>
        /// The read data.
        /// </value>
        public static Func<string, string> ReadData { get; set; }
        
        /// <summary>
        /// Gets or sets the data exists.
        /// </summary>
        /// <value>
        /// The data exists.
        /// </value>
        public static Func<string, bool> DataExists { get; set; }
        
        /// <summary>
        /// Gets or sets the read persistent.
        /// </summary>
        /// <value>
        /// The read persistent.
        /// </value>
        public static Func<string, string> ReadPersistent { get; set; }
        
        /// <summary>
        /// Gets or sets the persistent exists.
        /// </summary>
        /// <value>
        /// The persistent exists.
        /// </value>
        public static Func<string, bool> PersistentExists { get; set; }
        
        /// <summary>
        /// Gets or sets the persist.
        /// </summary>
        /// <value>
        /// The persist.
        /// </value>
        public static Action<string, string> Persist { get; set; }
        
        /// <summary>
        /// Reads all text.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string ReadAllText(string path)
        {
            // First check if file exists in persistent store.
            if (PersistentExists(path)) return ReadPersistent(path);

            // Then check if file exists in data store.
            if (DataExists(path)) return ReadData(path);

            throw new FileNotFoundException(path);
        }
    }
}