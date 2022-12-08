using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleObjectActive : MonoBehaviour
{
  Toggle m_Toggle;
  public GameObject ObjectToToggle;

  void Start()
  {
    //Fetch the Toggle GameObject
    m_Toggle = GetComponent<Toggle>();
    //Add listener for when the state of the Toggle changes, to take action
    m_Toggle.onValueChanged.AddListener(delegate {
      ToggleValueChanged(m_Toggle);
    });

  }

  //Output the new state of the Toggle into Text
  void ToggleValueChanged(Toggle change)
  {
    ObjectToToggle.SetActive(m_Toggle.isOn);
  }
}
