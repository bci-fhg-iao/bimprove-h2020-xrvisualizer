using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class NetIssueBehaviour : MonoBehaviour
{
    public GameObject connectedIssue;
    public List<RealtimeTransform> transforms = new List<RealtimeTransform>();
    public List<RealtimeView> views = new List<RealtimeView>();

    public void DecoupleObject()
    {
        if (transform.parent.transform.localScale.x == 0.02f)
        {
            connectedIssue.transform.position = transform.position;
        }
        else if (transform.parent.localScale.y == 0.02f)
        {
            connectedIssue.transform.GetChild(0).transform.localPosition = new Vector3(0, 0, 0);
            connectedIssue.transform.position = transform.position + new Vector3 (0, 0.3f, 0);
        }

        foreach (RealtimeView view in views)
        {
            view.RequestOwnership();
        }

        foreach (RealtimeTransform transform in transforms)
        {
            transform.RequestOwnership();
        }
    }
}
