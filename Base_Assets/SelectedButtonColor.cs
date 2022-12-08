using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedButtonColor : MonoBehaviour
{
    public Image targetImage;
    public Color onState;
    public Color offState;

    public void ChangeButtonColorOn()
    {
        targetImage.color = onState;
    }

    public void ChangeButtonColorOff()
    {
        targetImage.color = offState;
    }
}
