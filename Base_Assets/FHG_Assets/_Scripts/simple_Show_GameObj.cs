using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simple_Show_GameObj : MonoBehaviour
{
    public GameObject m_Architektur;
    //public GameObject m_ArchMoebel;
    //public GameObject m_TGA;
    public GameObject m_TGA_Elektro;
    public GameObject m_TGA_Heizung;
    public GameObject m_TGA_Lueftung;
    public GameObject m_TGA_Sanitaer;
    public GameObject m_LaborPlanung;
    public GameObject m_Arch_Gelaende;
    public GameObject m_Umgebungsmodell;

    nodeManager m_node_manager;

    bool m_init_OK = false;
    bool m_is_wireframe = false;

    // Use this for initialization
    void Start()
    {
        m_node_manager = transform.GetComponent<nodeManager>() as nodeManager;
        m_init_OK = checkModels();
    }

    void Update()
    {
        if (!m_init_OK)
        {
            m_init_OK = checkModels();
        }
    }

    bool checkModels()
    {
        if (
            m_Architektur == null ||
            //m_ArchMoebel == null ||
            // m_TGA == null ||
            m_TGA_Elektro == null ||
            m_TGA_Heizung == null ||
            m_TGA_Lueftung == null ||
            m_TGA_Sanitaer == null ||
            m_LaborPlanung == null ||
            m_Arch_Gelaende == null ||
            m_Umgebungsmodell == null)
        {

            Debug.Log("Error simple_Show_GameObj: Object(s) missing");
            return false;
        }
        else
        {
            Debug.Log("simple_Show_GameObj: All object(s) found");
            return true;
        }
    }
    // Use this for initialization

    public void showArchitektur(bool showIt)
    {
        if (m_init_OK)
        {
            showObject(m_Architektur, showIt);
        }
    }

    public void show_TGA_Elektro(bool showIt)
    {
        if (m_init_OK)
        {
            showObject(m_TGA_Elektro, showIt);
        }
    }

    public void show_TGA_Heizung(bool showIt)
    {
        if (m_init_OK)
        {
            showObject(m_TGA_Heizung, showIt);
        }
    }

    public void show_TGA_Lueftung(bool showIt)
    {
        if (m_init_OK)
        {
            showObject(m_TGA_Lueftung, showIt);
        }
    }

    public void show_TGA_Sanitaer(bool showIt)
    {
        if (m_init_OK)
        {
            showObject(m_TGA_Sanitaer, showIt);
        }
    }

    public void show_Labor(bool showIt)
    {
        if (m_init_OK)
        {
            showObject(m_LaborPlanung, showIt);
        }
    }

    public void show_Grundstueck(bool showIt)
    {
        if (m_init_OK)
        {
            showObject(m_Arch_Gelaende, showIt);
        }
    }

    public void show_Umgebung(bool showIt)
    {
        if (m_init_OK)
        {
            showObject(m_Umgebungsmodell, showIt);
        }
    }

    public void setArchTransp(bool isTransparent)
    {
        if (m_init_OK)
        {
            setTansparent(m_Architektur, isTransparent);
        }
    }

    void toggleTransparent(GameObject obj)
    {
        if (m_node_manager != null)
        {
            m_is_wireframe = !m_is_wireframe;
            m_node_manager.displayObjectAsWireframe(m_is_wireframe, obj);
        }
    }
    void toggleObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }

    void setTansparent(GameObject obj, bool isTransparent)
    {
        if (m_node_manager != null && obj != null)
        {
            m_node_manager.displayObjectAsWireframe(isTransparent, obj);
            Debug.Log("simple_Show_GameObj->setTansparent:" + isTransparent);
        }
        else
        {
            Debug.Log("Error: simple_Show_GameObj->setTansparent:" + isTransparent);
        }
    }
    void showObject(GameObject obj, bool showIt)
    {

        if (obj != null)
        {
            obj.SetActive(showIt);
            Debug.Log("simple_Show_GameObj->showObject");
        }
        else
        {
            Debug.Log("Error: simple_Show_GameObj->showObject");
        }
    }
}
