using Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTest : MonoBehaviour
{

    public EntityView playerPrefab;
    public GameObject canvas;

    private void Start()
    {
        Netlib.OnClientConnected += (peer) =>
        {
            if (Netlib.IsServer)
            {
                //instantiate a new eentity and give the controller for him
                var entity = Netlib.Instantiate(playerPrefab, new Vector3(Random.Range(-10, 10), 0, 0), Quaternion.identity);
                entity.AssignControl(peer);
            }

            canvas.SetActive(false); 
        };

        Netlib.OnClientDisconnected += (peer) =>
        {
            if (Netlib.IsServer)
            {
                var objcts = FindObjectsOfType<EntityView>();
                foreach (var item in objcts)
                {
                    if (item.Controller == peer)
                    {
                        Netlib.Destroy(item);
                    }
                }
            }
            else
            {
                canvas.SetActive(true);
            }
        };
    }

    public void StartServer()
    {
        canvas.SetActive(false);
        Netlib.StartServer();
    }

    public void StartClient()
    {
        Netlib.Connect();
    }
}
