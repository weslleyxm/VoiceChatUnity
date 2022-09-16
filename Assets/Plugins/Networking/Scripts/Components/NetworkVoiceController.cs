using LiteNetLib;
using Networking.Core;
using Networking.Packets;

namespace Networking
{
    public static class NetworkVoiceController
    {
        /// <summary>
        /// Called when a packet of voice with receveid from cleint
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="reader"></param>
        internal static void OnReceiveVoice(NetPeer peer, NetPacketReader reader)
        {
            VoicePacketMessage voicePacket = new VoicePacketMessage();
            voicePacket.Deserialize(reader);

            //if is  server resend for all client execept sender
            if (Netlib.IsServer)
                NetworkCore.SendPacketToAllExceptSerializable(PacketType.Voice, voicePacket, peer);

            ///get sender entity to deserialize data voice
            EntityView view = NetworkEntities.GetEntity(voicePacket.networkId);

            //add the packte to entity to send
            //to play audio
            if (view) view.GetComponent<VoiceManager>().OnReceivePacket(voicePacket);
        }
    }
}