using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamJsonRpc.Protocol;

namespace SubstrateNetApi
{

    public class RequestGenerator
    {
        public static string GetStorage(Module module, Item item, byte[] parameter = null)
        {
            var mBytes = Encoding.ASCII.GetBytes(module.Name);
            var iBytes = Encoding.ASCII.GetBytes(item.Name);

            var keybytes = HashExtension.XXHash128(mBytes).Concat(HashExtension.XXHash128(iBytes)).ToArray();
            var request = BitConverter.ToString(keybytes).Replace("-", "");
            
            switch (item.Type)
            {
                case Storage.Type.Plain:
                    return request;
                case Storage.Type.Map:

                    //var keyType = item.Function.Key1;
                    
                    switch (item.Function.Hasher)
                    {
                        case Storage.Hasher.Identity:
                            //keybytes.Concat(Utils.HexToByteArray(parameter)).;
                            return "";
                        case Storage.Hasher.Blake2_128:
                        case Storage.Hasher.Blake2_256:
                        case Storage.Hasher.Blake2_128Concat:
                        case Storage.Hasher.Twox128:
                        case Storage.Hasher.Twox256:
                        case Storage.Hasher.Twox64Concat:
                        case Storage.Hasher.None:
                        default:
                            break;
                    }
                    return "";
                case Storage.Type.DoubleMap:
                    return "";
                default:
                    return "";
            }
        }
    }
}
