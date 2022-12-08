using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clipping_plane_slider : MonoBehaviour
{

    public Slider cpslide;

    public void CPslider()

    {
        Camera ARCam = gameObject.GetComponent<Camera>();
        ARCam.nearClipPlane = cpslide.value;
    }
}
