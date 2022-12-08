using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class OpenVRInput : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger) )
        {
          Debug.Log("party?");
        }
    }


}


