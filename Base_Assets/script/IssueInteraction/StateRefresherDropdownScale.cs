using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateRefresherDropdownScale : MonoBehaviour
{
    public int _currentInt;
    public IntSync _intSync;
    public handleDropdownWorldScaleChanged handleDropdownChanged;
    public Dropdown objectDropdown;

    private void Start()
    {
        handleDropdownChanged = FindObjectOfType<handleDropdownWorldScaleChanged>();
        Getter();
    }

    private void FixedUpdate()
    {
        if (_currentInt != _intSync._dropdownInt)
        {
            Getter();
            Setter();
        }
    }

    public void Getter()
    {
        _currentInt = _intSync._dropdownInt;
    }

    public void Setter()
    {
        Debug.Log(_currentInt);
        objectDropdown.value = _currentInt;
        objectDropdown.RefreshShownValue();
        Debug.Log("successfully set states");
    }
}
