using Networking.Packets;
using LiteNetLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using Networking.Core;
using Instantiate = Networking.Packets.Instantiate;

namespace Networking
{
    public static class NetworkEntities
    {
        internal static Dictionary<string, EntityView> Entities = new Dictionary<string, EntityView>();

        internal static void OnReceiveInstantiate(NetPeer netPeer, NetPacketReader reader)
        {
            Instantiate packet = new Instantiate();
            packet.Deserialize(reader);

            Instantiate(packet);
        }

        internal static void OnReceiveEntityControll(NetPeer netPeer, NetPacketReader reader)
        {
            GiveEntityControl packet = new GiveEntityControl();
            packet.Deserialize(reader);

            ///contains the network id object
            if (Entities.ContainsKey(packet.networkId))
            {
                //call the event if have one
                Entities[packet.networkId].OnReceiveController?.Invoke();
                Entities[packet.networkId].isController = true; 
            }
        }

        internal static void OnReceiveDestroy(NetPeer netPeer, NetPacketReader reader)
        {
            Destroy destroy = new Destroy();
            destroy.Deserialize(reader);

            Destroy(destroy);
        }

        internal static EntityView Instantiate(EntityView entity, Vector3 position, Quaternion rotation)
        {
            Instantiate instantiate = new Instantiate
            {
                position = position,
                rotation = rotation,
                networkId = Guid.NewGuid().ToString(),
                prefabId = entity.prefabId
            };

            NetworkCore.SendPacketToAllSerializable(PacketType.Instantiate, instantiate);
            var view = Instantiate(instantiate);
            view.isMine = true;

            MonoBehaviour.DontDestroyOnLoad(view);
            return view;
        }

        internal static EntityView Instantiate(Instantiate packet)
        {
            if (!Entities.ContainsKey(packet.networkId))
            { 
                GameObject @object = NetworkObjects.Instance.GetObject(packet.prefabId).gameObject;
                EntityView entity = MonoBehaviour.Instantiate(@object, packet.position, packet.rotation).GetComponent<EntityView>();
                entity.NetworkID = packet.networkId;

                entity.isMine = false;
                 
                Entities.Add(packet.networkId, entity);

                entity.name = "[Network] " + entity.name.Replace("(Clone)", "");
                MonoBehaviour.DontDestroyOnLoad(entity);
                return entity;
            }

            return null;
        }

        private static void Destroy(Destroy destroy)
        {
            if (Entities.ContainsKey(destroy.networkId))
            {
                MonoBehaviour.Destroy(Entities[destroy.networkId].gameObject);
                Entities.Remove(destroy.networkId);
            }
        }

        internal static EntityView GetEntity(string guid)
        {
            if (Entities.ContainsKey(guid))
                return Entities[guid];
            else return null;
        }
         
        internal static void Destroy(EntityView view)
        {
            Destroy destroy = new Destroy
            {
                networkId = view.NetworkID
            };

            NetworkCore.SendPacketToAllSerializable(PacketType.Destroy, destroy);

            Destroy(destroy);
        }
         

        internal static void InstantiateAll(NetPeer peer)
        {
            foreach (var item in Entities.Values)
            {
                Instantiate instantiate = new Instantiate
                {
                    position = item.transform.position,
                    rotation = item.transform.rotation,
                    networkId = item.NetworkID,
                    prefabId = item.prefabId
                };

                NetworkCore.SendPacketSerializable(PacketType.Instantiate, peer, instantiate);
            }
        }
    }
}