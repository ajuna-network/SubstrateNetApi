using Newtonsoft.Json;
using NLog;
using System;
using System.Reflection;

namespace SubstrateNetWallet
{
    public class Caching
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static bool TryReadFile<T>(string path, out T obj)
        {
            obj = default(T);

            try
            {
                var objDecrypted = Decrypt(SystemInteraction.ReadAllText(path));
                obj = JsonConvert.DeserializeObject<T>(objDecrypted);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"TryReadFile<{obj?.GetType()}>: {e}");
                return false;
            }
        }

        public static void Persist<T>(string path, T obj)
        {
            var objEncrypted = Encrypt(JsonConvert.SerializeObject(obj));
            SystemInteraction.Persist(path, objEncrypted);
        }

        //unused, already encrypted maybe for later
        private static string Encrypt(string str)
        {
            return str;
        }

        //unused, already encrypted maybe for later
        private static string Decrypt(string str)
        {
            return str;
        }
    }
}
