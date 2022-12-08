using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trilib_model_manager : MonoBehaviour
{
    private List<model_component> m_components;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_components != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                List<GameObject> myObjects = getGameObjectList();
                List<string> myNames = getNameList();
                List<bool> myTransp = getTransparencyList();
                Debug.Log("Got lists");
            }

            // TEST: set active and transparent
            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    setComponentActive(0, true);
            //}

            //if (Input.GetKeyDown(KeyCode.Q))
            //{
            //    setComponentActive(0, false);
            //}

            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    setComponentTransparent(0, true);
            //}

            //if (Input.GetKeyDown(KeyCode.Y))
            //{
            //    setComponentTransparent(0, false);
            //}

            // TEST: toggle active and transparent
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                toggleComponentActive(0);
            }
        
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                toggleComponentActive(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                toggleComponentActive(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                toggleComponentActive(3);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                toggleComponentActive(4);
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                toggleComponentActive(5);
            }

            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                toggleComponentActive(6);
            }

            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                toggleComponentActive(7);
            }

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                toggleComponentActive(8);
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                toggleComponentActive(9);
            }

            // Toggle Transparent
            if (Input.GetKeyDown(KeyCode.Q))
            {
                toggleComponentTransparent(0);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                toggleComponentTransparent(1);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                toggleComponentTransparent(2);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                toggleComponentTransparent(3);
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                toggleComponentTransparent(4);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                toggleComponentTransparent(5);
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                toggleComponentTransparent(6);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                toggleComponentTransparent(7);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                toggleComponentTransparent(8);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                toggleComponentTransparent(9);
            }
        }

    }

    public void setComponents(List<model_component> components)
    {
        if (components != null)
        {
            m_components = components;
        }
        else
        {
            Debug.Log("ERROR: trilib_model_manager->setLoader: components are NULL");
        }
    }

    public void setComponentActive(int index, bool isActive)
    {
        if(m_components!=null && index>-1 && index < m_components.Count)
        {
            m_components[index].setActive(isActive);
        }
    }

    public void setComponentTransparent(int index, bool isTransparent)
    {
        if (m_components != null && index > -1 && index < m_components.Count)
        {
            m_components[index].setTransparent(isTransparent);
        }
    }

    public void toggleComponentActive(int index)
    {
        if (m_components != null && index > -1 && index < m_components.Count)
        {
            m_components[index].toggleActive();
        }
    }

    public void toggleComponentTransparent(int index)
    {
        if (m_components != null && index > -1 && index < m_components.Count)
        {
            m_components[index].toggleTransparent();
        }
    }

    public List<GameObject> getGameObjectList()
    {
        List<GameObject> myObjects = new List<GameObject>(m_components.Count);
        for(int i=0; i<m_components.Count; i++)
        {
            myObjects.Add(m_components[i].getNodeObject());
        }
        return myObjects;
    }

    public List<string> getNameList()
    {
        List<string> myNames = new List<string>(m_components.Count);
        for (int i = 0; i < m_components.Count; i++)
        {
            myNames.Add(m_components[i].getName());
        }
        return myNames;
    }

    public List<bool> getTransparencyList()
    {
        List<bool> myTransparency = new List<bool>(m_components.Count);
        for (int i = 0; i < m_components.Count; i++)
        {
            myTransparency.Add(m_components[i].getTransparentMode());
        }
        return myTransparency;
    }
}
