using LiteNetLib.Utils;

namespace Networking
{
    internal class GiveEntityControl : INetSerializable
    {
        internal string networkId;

        public void Deserialize(NetDataReader reader)
        {
            networkId = reader.GetString();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(networkId); 
        }
    }
}