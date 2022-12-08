using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class RoomConnector : MonoBehaviour
{
    private bool realtimeSet = false;
    private Realtime realtime;
    public GameObject geoManager;

    private void Awake()
    {
        realtime = FindObjectOfType<Realtime>();
    }

    void Update()
    {
        if(realtimeSet == false && realtime.connected == true)
        {
            geoManager.SetActive(true);
        }
    }
}
