using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceChat;

[RequireComponent(typeof(VoiceChatRecorder))]
public class MicrofoneController : MonoBehaviour
{
    public KeyCode keyCode = KeyCode.F;
    void Update()
    {
        if (Input.GetKey(keyCode))   
            VoiceChatRecorder.Instance.transmitToggled = true; 
        else
            VoiceChatRecorder.Instance.transmitToggled = false;
    }
}
