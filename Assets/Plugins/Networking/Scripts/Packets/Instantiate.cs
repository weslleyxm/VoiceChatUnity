using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using static Networking.NetworkEntities;

namespace Networking.Packets
{
    /// <summary>
    /// Instantiate to send for client by server 
    /// </summary>
    public class Instantiate : INetSerializable
    {
        /// <summary>
        /// type of prefab to instantiate
        /// </summary>
        public PrefabId prefabId;
        /// <summary>
        /// network id returnd by server
        /// </summary>
        public string networkId;
        /// <summary>
        /// instantiate position
        /// </summary>
        public Vector3 position;
        /// <summary>
        /// instantiate rotation
        /// </summary>
        public Quaternion rotation;

        public virtual void Deserialize(NetDataReader reader)
        {
            prefabId = (PrefabId)reader.GetShort(); 
            networkId = reader.GetString(); 
            position = Extensions.GetVector3(reader);
            rotation = Extensions.GetQuaternion(reader);
        } 

        public virtual void Serialize(NetDataWriter writer)
        {
            writer.Put((short)prefabId); 
            writer.Put(networkId);
            Extensions.Put(writer, position);
            Extensions.Put(writer, rotation);  
        }
    }
}
