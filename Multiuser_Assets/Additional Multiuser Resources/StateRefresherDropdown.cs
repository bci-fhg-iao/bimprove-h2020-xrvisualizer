using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateRefresherDropdown : MonoBehaviour
{
    public int _currentInt;
    public IntSync _intSync;
    public handleDropdownChanged handleDropdownChanged;
    public Dropdown objectDropdown;

    private void Start()
    {
        handleDropdownChanged = FindObjectOfType<handleDropdownChanged>();
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
        for (int i = 0; i < handleDropdownChanged.myObjects.Count; i++)
        {
            if (i == _currentInt)
            {
                handleDropdownChanged.myObjects[i].SetActive(true);
            }
            else
            {
                handleDropdownChanged.myObjects[i].SetActive(false);
            }
        }
        Debug.Log("successfully set states");
    }
}
