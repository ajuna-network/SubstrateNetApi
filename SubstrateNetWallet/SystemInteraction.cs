using System;
using System.IO;

namespace SubstrateNetWallet
{
    public static class SystemInteraction
    {
        public static Func<string, string> ReadData { get; set; }
        public static Func<string, bool> DataExists { get; set; }
        public static Func<string, string> ReadPersistent { get; set; }
        public static Func<string, bool> PersistentExists { get; set; }
        public static Action<string, string> Persist { get; set; }

        public static string ReadAllText(string path)
        {
            // First check if file exists in persistent store.
            if (PersistentExists(path))
            {
                return ReadPersistent(path);
            }

            // Then check if file exists in data store.
            if (DataExists(path))
            {
                return ReadData(path);
            }

            throw new FileNotFoundException(path);
        }
    }
}
