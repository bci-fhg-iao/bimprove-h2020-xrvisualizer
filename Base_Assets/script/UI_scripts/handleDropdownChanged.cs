using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class handleDropdownChanged : MonoBehaviour
{
  public List<GameObject> myObjects;
  private Dropdown myDropdown;
  public bool isMultiuser;
  public IntSync intSync;
  public StateRefresherDropdown stateRefresherDropdown;

  // Start is called before the first frame update
  void Start()
  {
        if (isMultiuser == true)
        {
            stateRefresherDropdown = FindObjectOfType<StateRefresherDropdown>();
            //intSync = FindObjectOfType<IntSync>();
        }

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
        else
        {
            for (int i = 0; i < myObjects.Count; i++)
            {
                if (i == change.value)
                {
                    myObjects[i].SetActive(true);
                }
                else
                {
                    myObjects[i].SetActive(false);
                }
            }
        }
  }
}
