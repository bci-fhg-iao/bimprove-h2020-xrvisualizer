using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class EventOwnershipRequester : MonoBehaviour
{
    public Realtime _realtime;
    public void RequestForOwnership()
    {
        if (_realtime.connected)
        {
            transform.GetComponent<RealtimeTransform>().RequestOwnership();
        }
    }
}
