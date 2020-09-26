using System;

namespace SubstrateNetApi.MetaDataModel
{
    public class Module
    {
        public string Name { get; set; }

        public byte Index { get; set; }
        public Storage Storage { get; set; }
        public Call[] Calls { get; set; }
        public Event[] Events { get; set; }
        public Const[] Consts { get; set; }
        public Error[] Errors { get; set; }

        public bool TryGetStorageItemByName(string name, out Item result)
        {
            result = null;
            if (Storage is null)
            {
                return false;
            }

            foreach (Item item in Storage.Items)
            {
                if (item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = item;
                    return true;
                }
            }

            return false;
        }

        internal bool TryGetCallByName(string name, out Call result)
        {
            result = null;
            foreach (Call call in Calls)
            {
                if (call.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = call;
                    return true;
                }
            }

            return false;
        }

        public byte IndexOf(Call call)
        {
            return (byte) Array.IndexOf(Calls, call);
        }
    }
}