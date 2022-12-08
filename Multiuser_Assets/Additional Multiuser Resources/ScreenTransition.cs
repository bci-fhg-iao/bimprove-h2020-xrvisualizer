using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    public GameObject fadeScreen;

    public void TransitionScreen()
    {
        fadeScreen.GetComponent<Animation>().Play("FadeScreen");
    }
}
