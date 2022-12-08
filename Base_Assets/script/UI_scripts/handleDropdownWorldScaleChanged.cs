using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class handleDropdownWorldScaleChanged : MonoBehaviour
{
  public Transform worldScaleTransform;
  private Dropdown myDropdown;
    public bool isMultiuser;
    public IntSync intSync;

    // Start is called before the first frame update
    void Start()
  {
    myDropdown = GetComponent<Dropdown>();
    myDropdown.onValueChanged.AddListener(delegate {
      OnDropdownValueChanged(myDropdown);
    });
  }

  // Update is called on value changed
  private void OnDropdownValueChanged(Dropdown change)
  {
        if (isMultiuser == true)
        {
            intSync.SetInt(change.value);
        }

        Debug.Log("handleDropdownWorldScaleChanged :: New value is = " + change.value);
    if (change.value == 0)
    {
      worldScaleTransform.localScale = new Vector3(1f, 1f, 1f);
    }
    else if (change.value == 1)
    {
      worldScaleTransform.localScale = new Vector3(.1f, .1f, .1f);
    }
    else if (change.value == 2)
    {
      worldScaleTransform.localScale = new Vector3(.01f, .01f, .01f);
    }
    else if (change.value == 3)
    {
      worldScaleTransform.localScale = new Vector3(.001f, .001f, .001f);
    }
    else if (change.value == 4)
    {
      worldScaleTransform.localScale = new Vector3(1000f, 1000f, 1000f);
    }
    else if (change.value == 5)
    {
      worldScaleTransform.localScale = new Vector3(0.0254f, 0.0254f, 0.0254f);
    }
    else if (change.value == 6)
    {
      worldScaleTransform.localScale = new Vector3(0.3048f, 0.3048f, 0.3048f);
    }

  }
}
