using Networking;
using Networking.Core;
using System.Collections;
using UnityEngine;

/// <summary>
/// This class is responsible for performing static class updates
/// </summary>
public class NetworkUpdate : MonoBehaviour
{ 
    public delegate void NetworkObjectUpdateCallback(); 
    public static NetworkObjectUpdateCallback Callback;
    
    private static NetworkUpdate _NetworkObjectUpdate;
    public static void InitCoroutine(IEnumerator routine)  
    {
        _NetworkObjectUpdate.StartCoroutine(routine); 
    }

    private void Awake()
    {
        _NetworkObjectUpdate = this;
    }

    void Update()
    {
        Callback?.Invoke();
    }

    private void OnDestroy()
    {
        NetworkCore.Stop();
    } 
}
