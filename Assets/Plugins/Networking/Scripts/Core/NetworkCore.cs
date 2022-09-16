using LiteNetLib;
using LiteNetLib.Utils;
using System;
using UnityEngine;

namespace Networking.Core
{
    internal static class NetworkCore
    {
        internal static NetManager NetManager;
        internal static EventBasedNetListener EventBasedNetListener;

        internal static bool IsServer = false;
        internal static bool IsClient = false;

        /// <summary>
        /// Start server with port
        /// </summary>
        /// <param name="serverPort"></param>
        internal static void Start(ushort serverPort)
        {
            if (IsServer)
            {
                Debug.Log("You cannot start the server twice");
                return;
            }

            if (IsClient)
            {
                Debug.Log("You can't start a server as a client");
                return;
            }

            SetNetworkSettings();

            NetManager = new NetManager(EventBasedNetListener)
            {
                AutoRecycle = true
            };

            NetManager.UpdateTime = 15;
            NetManager.Start(serverPort);

            Debug.Log(string.Format("Server was started!"));

            IsServer = true;
            IsClient = false;
        }

        /// <summary>
        /// Set network settings and callbacks
        /// </summary>
        private static void SetNetworkSettings()
        {
            GameObject networkObject = new GameObject("[NetworkUpdate]");
            MonoBehaviour.DontDestroyOnLoad(networkObject);

            networkObject.AddComponent<NetworkUpdate>();

            //set the method to perfomace update
            NetworkUpdate.Callback += OnUpdate;

            EventBasedNetListener = new EventBasedNetListener();

            EventBasedNetListener.PeerConnectedEvent += OnPeerConnected;
            EventBasedNetListener.PeerDisconnectedEvent += OnPeerDisconnectedEvent;
            EventBasedNetListener.ConnectionRequestEvent += OnConnectionRequestEvent;
            EventBasedNetListener.NetworkReceiveEvent += OnNetworkReceiveEvent;

            Debug.Log(string.Format("Network settings updated..."));
        }

        /// <summary>
        /// Stop the net manager
        /// </summary>
        public static void Stop()
        {
            NetManager.Stop();
        }

        /// <summary>
        /// Connect to server using server address and port
        /// </summary>
        /// <param name="serverIp"></param>
        /// <param name="port"></param>
        internal static void Connect(string serverIp, int port)
        {
            if (IsServer)
            {
                Debug.Log("Can't connect to server when you are a server!");
                return;
            }

            if (IsClient)
            {
                Debug.Log("Cannot connect to server twice!");
                return;
            }

            SetNetworkSettings();

            NetManager = new NetManager(EventBasedNetListener)
            {
                AutoRecycle = true
            };

            NetManager.UpdateTime = 15;

            NetManager.Start();
            NetManager.Connect(serverIp, port, "");

            IsServer = false;
            IsClient = true;
        }

        /// <summary>
        /// Update poll event of net manager
        /// </summary>
        internal static void OnUpdate()
        {
            if (NetManager != null)
            {
                NetManager.PollEvents();
            }
        }

        internal static void SendPacketSerializable<T>(PacketType type, T packet) where T : INetSerializable
        {
            if (NetManager.FirstPeer != null)
            {
                NetDataWriter writer = new NetDataWriter();
                writer.Put((byte)type);
                packet.Serialize(writer);

                NetManager.FirstPeer.Send(writer, DeliveryMethod.ReliableOrdered);
            }
        }


        #region NewtworkCallbacks
        private static void OnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Netlib.OnClientDisconnected?.Invoke(peer);
        }

        private static void OnPeerConnected(NetPeer peer)
        {
            Netlib.OnClientConnected?.Invoke(peer);


            if (IsServer)
            {
                //froce the client to instantiate all entities
                NetworkEntities.InstantiateAll(peer);
            }


            Debug.Log(string.Format("[Server] We got a new connection: {0}", peer.EndPoint));
        }

        private static void OnConnectionRequestEvent(ConnectionRequest request)
        {
            if (IsServer)
                request.Accept();

        }

        private static void OnNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            PacketType packetType = (PacketType)reader.GetByte();

            switch (packetType)
            {
                case PacketType.Instantiate:
                    NetworkEntities.OnReceiveInstantiate(peer, reader);
                    break;
                case PacketType.Destroy:
                    NetworkEntities.OnReceiveDestroy(peer, reader);
                    break;
                case PacketType.Voice:
                    NetworkVoiceController.OnReceiveVoice(peer, reader);
                    break;
                case PacketType.GiveEntityControl:
                    NetworkEntities.OnReceiveEntityControll(peer, reader);
                    break;
            }
        }
        #endregion

        #region SendPackets
        internal static void SendPacketSerializable(NetDataWriter packet)
        {
            if (NetManager.FirstPeer != null)
            {
                NetManager.FirstPeer.Send(packet, DeliveryMethod.ReliableOrdered);
            }
        }

        internal static void SendPacketSerializable<T>(PacketType type, NetPeer peer, T packet) where T : INetSerializable
        {
            NetDataWriter writer = new NetDataWriter();
            writer.Put((byte)type);
            packet.Serialize(writer);

            peer.Send(writer, DeliveryMethod.ReliableOrdered);
        }

        internal static void SendPacketToAllSerializable<T>(PacketType type, T packet) where T : INetSerializable
        {
            NetDataWriter writer = new NetDataWriter();
            writer.Put((byte)type);
            packet.Serialize(writer);

            NetManager.SendToAll(writer, DeliveryMethod.ReliableOrdered);
        }

        internal static void SendPacketToAllExceptSerializable<T>(PacketType type, T packet, NetPeer except) where T : INetSerializable
        {
            foreach (var item in NetManager.ConnectedPeerList)
            {
                if (item != except)
                {
                    NetDataWriter writer = new NetDataWriter();
                    writer.Put((byte)type);
                    packet.Serialize(writer);

                    item.Send(writer, DeliveryMethod.ReliableOrdered);
                }
            }
        }

        internal static void SendPacketSerializable(object assignControl, NetPeer peer, GiveEntityControl assignEntity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

