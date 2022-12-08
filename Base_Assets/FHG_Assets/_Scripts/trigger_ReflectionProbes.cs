using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger_ReflectionProbes : MonoBehaviour {
     GameObject m_outside; //ReflectionProbes for landscape
     GameObject m_inside; //ReflectionProbes for rooms

    bool m_init = false;

    // Use this for initialization
    void Start () {

    }


	
	// Update is called once per frame
	void Update () {
        if (!m_init)
        {
            m_outside = transform.Find("Outside").gameObject;
            m_inside = transform.Find("Inside").gameObject;
            if (m_outside != null && m_inside !=null)
            {
                m_init = true;
                setOutside(true);
            }
        }
    }

    void setOutside(bool isOutside)
    {
        if (m_init)
        {
                m_inside.SetActive(!isOutside);
                m_outside.SetActive(isOutside);
        }
        else
        {
            Debug.Log("Error: Trigger Reflection probes Gameobjects not found!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Reflection Trigger ENTER: Pos(" + Camera.main.transform.position);
        setOutside(false);
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Reflection Trigger EXIT: Pos(" + Camera.main.transform.position);
        setOutside(true);
    }
}
