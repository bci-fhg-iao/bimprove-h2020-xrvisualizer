using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotation : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            transform.rotation = Quaternion.identity;
        }
    }

    public void RotationReset()
    {
        Debug.Log("Rotation has been reset");
        transform.rotation = Quaternion.identity;
    }
}
