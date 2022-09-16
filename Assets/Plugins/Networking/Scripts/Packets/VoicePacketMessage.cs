using LiteNetLib.Utils;
using UnityEngine;
using UnityEngine.Networking.Types;
using VoiceChat;

public struct VoiceChatPacket
{
    public ulong PacketId;
    public VoiceChatCompression Compression;
    public int Length;
    public byte[] Data;
}

namespace Networking.Packets
{
    public class VoicePacketMessage : INetSerializable
    {
        public string networkId;  
        public VoiceChatPacket packet;
        public void Deserialize(NetDataReader reader)
        {  
            packet = new VoiceChatPacket();
            networkId = reader.GetString();
            packet.PacketId = reader.GetULong();
            packet.Compression = (VoiceChatCompression)reader.GetByte();
            packet.Length = reader.GetInt();
            packet.Data = reader.GetBytesWithLength(); 
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(networkId); 
            writer.Put(packet.PacketId);
            writer.Put((byte)packet.Compression);
            writer.Put(packet.Length);
            writer.PutBytesWithLength(packet.Data);
        }
    }
}
