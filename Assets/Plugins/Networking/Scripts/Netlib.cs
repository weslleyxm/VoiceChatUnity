using LiteNetLib;
using Networking.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Networking
{
    public static class Netlib 
    {
        /// <summary>
        /// Check the current instance is server
        /// </summary>
        public static bool IsServer => NetworkCore.IsServer;

        /// <summary>
        /// Check the current instance is client
        /// </summary>
        public static bool IsClient => NetworkCore.IsClient;

        /// <summary>
        /// Called when a client was disconnected from server
        /// </summary>
        public static Action<NetPeer> OnClientDisconnected; 

        /// <summary>
        /// Called when a client was connected from server
        /// </summary>
        public static Action<NetPeer> OnClientConnected;

        /// <summary>
        /// List of entities that are inserted in the server
        /// </summary>
        public static IEnumerable<EntityView> Entities => NetworkEntities.Entities.Values;

        /// <summary>
        /// Start server with port
        /// </summary>
        /// <param name="serverPort"></param> 
        public static void StartServer(ushort serverPort = 7777)
        {
            NetworkCore.Start(serverPort);
        }

        /// <summary>
        /// Connect to server using server address and port
        /// </summary>
        /// <param name="serverIp"></param>
        /// <param name="port"></param>
        public static void Connect(string serverIp = "localhost", int port = 7777)
        {
            NetworkCore.Connect(serverIp, port);
        } 

        /// <summary>
        /// Instantiate an object 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static EntityView Instantiate(EntityView entity, Vector3 position, Quaternion rotation)
        {
            return NetworkEntities.Instantiate(entity, position, rotation);
        }

        public static void Destroy(EntityView view)
        {
            NetworkEntities.Destroy(view);
        }
    }
}