using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToPlayer : MonoBehaviour
{


    private Transform target;


    // Use this for initialization
    void Start()
    {
        target = FindObjectOfType<Camera>().transform;
        transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(target);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
