using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Normal.Realtime;


public class toggleObjectList : MonoBehaviour
{
    Toggle m_Toggle;
    public List<GameObject> ObjectsToToggle;
    private UISync uiSync;
    public bool isMultiuser;
    public GameObject assignedModule;

    void Awake()
    {
        if (isMultiuser)
        {
            uiSync = GameObject.Find("UIManager").GetComponent<UISync>();
        }

        m_Toggle = GetComponent<Toggle>();

        m_Toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(m_Toggle);
        });
    }

    void ToggleValueChanged(Toggle change)
    {
        if (isMultiuser)
        {
            uiSync.SetInt(int.Parse(transform.name));
            uiSync.SetBool(m_Toggle.isOn);

            var uiSync2 = assignedModule.GetComponent<UISync>();
            uiSync2.SetInt(int.Parse(transform.name));
            uiSync2.SetBool(m_Toggle.isOn);
        }
        SetLocalStates();
    }

    public void SetLocalStates()
    {
        foreach (GameObject obj in ObjectsToToggle)
        {
            obj.SetActive(m_Toggle.isOn);
        }
    }
}



