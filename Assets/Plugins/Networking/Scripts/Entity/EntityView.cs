using LiteNetLib;
using LiteNetLib.Utils;
using Networking.Core;
using Networking.Packets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Networking
{
    [DisallowMultipleComponent]
    public sealed class EntityView : MonoBehaviour
    {
        /// <summary>
        /// Network id from server
        /// </summary>
        public string NetworkID { get; internal set; }

        /// <summary>
        /// Current prefab id
        /// </summary>
        public PrefabId prefabId;

        /// <summary>
        /// Current connection who controll this entity only server
        /// </summary>
        public NetPeer Controller { get; internal set; }

        /// <summary>
        /// My is owner of this entity
        /// </summary>
        public bool isMine { get; internal set; }

        /// <summary>
        /// My connection is controller of this enity
        /// </summary>
        public bool isController { get; internal set; }

        /// <summary>
        /// Called whe the connection receive controller for this entity
        /// </summary> 
        public UnityAction OnReceiveController;
         
        /// <summary>
        /// Give entity control the connection
        /// </summary>
        /// <param name="peer"></param>
        public void AssignControl(NetPeer peer)
        {
            GiveEntityControl assignEntity = new GiveEntityControl
            {
                networkId = NetworkID
            };

            Controller = peer;
            NetworkCore.SendPacketSerializable(PacketType.GiveEntityControl, peer, assignEntity);
        }
    }
}