using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapedButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}
