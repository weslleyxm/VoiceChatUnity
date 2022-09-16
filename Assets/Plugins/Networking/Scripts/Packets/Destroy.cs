using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Packets
{
    public class Destroy : INetSerializable
    {
        public string networkId;
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
