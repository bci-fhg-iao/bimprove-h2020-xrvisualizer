using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateRefresher : MonoBehaviour
{
    public int _currentInt;
    public bool _currentBool;
    public GameObject _currentToggle;
    public UISync _uiSync;

    private void Start()
    {
        Getter();
    }

    private void Update()
    {
        if (_currentInt != _uiSync._uiInt)
        {
            Debug.Log("Int changed");
            Getter();
            Setter();
        }

        if (_currentBool != _uiSync._uiBool)
        {
            Debug.Log("Bool changed");
            Getter();
            Setter();
        }
    }

    public void Getter()
    {
        _currentInt = _uiSync._uiInt;
        _currentBool = _uiSync._uiBool;
    }

    public void Setter()
    {
        _currentToggle = GameObject.Find(_currentInt.ToString());
        _currentToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(_currentBool);
        _currentToggle.GetComponent<toggleObjectList>().SetLocalStates();
    }
}
