using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simple_Toggler : MonoBehaviour
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

            return false;
        else
        {
            return true;
        }
    }

    // Updam_Elektro;te is called once per frame
    void Update()
    {
        //if (m_init_OK == false)
        //{
        //    m_init_OK = checkModels();
        //}
        //else
        //{
        if (Input.GetKeyDown(KeyCode.Q))
        {
            toggleObject(m_Umgebungsmodell);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            toggleObject(m_Arch_Gelaende);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            toggleObject(m_Architektur);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            toggleTransparent(m_Architektur);
        }
        //else if (Input.GetKeyDown(KeyCode.T))
        //{
        //    toggleObject(m_ArchMoebel);
        //}
        else if (Input.GetKeyDown(KeyCode.A))
        {
            toggleObject(m_LaborPlanung);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            toggleObject(m_TGA_Elektro);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            toggleObject(m_TGA_Heizung);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            toggleObject(m_TGA_Lueftung);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            toggleObject(m_TGA_Sanitaer);
        }



        //   }

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

}

