using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleTwoObjectReverse : MonoBehaviour
{
  Toggle m_Toggle;
  public GameObject ObjectToToggle;
  public GameObject ObjectToToggleReverse;

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
    bool isbool = m_Toggle.isOn;
    ObjectToToggle.SetActive(isbool);
    ObjectToToggleReverse.SetActive(!isbool);
  }
}
