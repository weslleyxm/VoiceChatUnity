using Networking;
using Networking.Core;
using Networking.Packets;
using UnityEngine;
using VoiceChat;
using static UnityEngine.EventSystems.EventTrigger;

public class VoiceManager : MonoBehaviour
{
    private string networkId;
    private VoiceChatPlayer player = null;
    private EntityView entity;

    private AudioSource _Source;
    public GameObject voiceIndication;

    private void Awake()
    { 
        player = gameObject.AddComponent<VoiceChatPlayer>();
        entity = GetComponent<EntityView>();

        entity.OnReceiveController += () =>
        {
            networkId = entity.NetworkID;

            VoiceChatRecorder.Instance.NewSample += OnNewSample;
            VoiceChatRecorder.Instance.Device = VoiceChatRecorder.Instance.AvailableDevices[0];

            Debug.Log("You have the controller");
        };
         
    }

    private void Update()
    {
        if (entity.isController)
        {
            VoiceChatRecorder.Instance.StartRecording();
        }

        voiceIndication.SetActive(GetComponent<AudioSource>().isPlaying); 
    }

    public void OnReceivePacket(VoicePacketMessage data)
    {
        player.OnNewSample(data.packet);  
    }

    public void OnNewSample(VoiceChatPacket packet)
    {
        var packetMessage = new VoicePacketMessage
        {
            packet = packet,
            networkId = networkId
        };

        NetworkCore.SendPacketToAllSerializable(PacketType.Voice, packetMessage);
    }
}
