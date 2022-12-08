using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoIDBehaviour : MonoBehaviour
{
    public GameObject connectedToggle;
    public GameObject connectedObject;

    void Start()
    {
        StartCoroutine(SetStates());
    }

    IEnumerator SetStates()
    {
        yield return new WaitForSeconds(0.3f);
        connectedToggle.GetComponent<toggleObjectList>().assignedModule = gameObject;
    }
}
