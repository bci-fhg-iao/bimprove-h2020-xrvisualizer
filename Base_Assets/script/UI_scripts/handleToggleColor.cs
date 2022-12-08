using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class handleToggleColor : MonoBehaviour
{
  // Start is called before the first frame update
  public Color checkedColor;
  public Color uncheckedColor;
  public Color pressedColor;

  private Toggle myToggleScript;
  private ColorBlock myCb;
  void Start()
  {
    myToggleScript = GetComponent<Toggle>();
    myCb = myToggleScript.colors;
    myCb.pressedColor = pressedColor;
    if (myToggleScript.isOn)
    {
      myCb.normalColor = checkedColor;
      myCb.highlightedColor = checkedColor;
    } else
    {
      myCb.normalColor = uncheckedColor;
      myCb.highlightedColor = uncheckedColor;
    }
    myToggleScript.colors = myCb;

    myToggleScript.onValueChanged.AddListener(OnToggleValueChanged);
  }

  // Update is called once per frame
  private void OnToggleValueChanged(bool isOn)
  {
    if (isOn)
    {
      myCb.normalColor = checkedColor;
      myCb.highlightedColor = checkedColor;
      myToggleScript.colors = myCb;
    }
    else
    {
      myCb.normalColor = uncheckedColor;
      myCb.highlightedColor = uncheckedColor;
      myToggleScript.colors = myCb;
    }
  }
}
