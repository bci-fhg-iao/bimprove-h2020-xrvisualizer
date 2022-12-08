using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    //transform.Find(); will only search the immediate children
    public static Transform Search(this Transform target, string name)
    {
        if (target.name == name) return target;

        for (int i = 0; i < target.childCount; ++i)
        {
            var result = Search(target.GetChild(i), name);
            if (result != null) return result;
        }

        return null;
    }
}


public class HV_display_manager : MonoBehaviour {
    public Transform m_neu_parkhaus;
    public Transform m_neu_parkhaus_stahl;
    public Transform m_neu_hotel;
    public Transform m_neu_wum_welt;
    public Transform m_bestand;
    public Transform m_punktwolke;
    public Transform m_gelaende;
    public Canvas m_canvas;

    GameObject m_bestand_bau;

    nodeManager_2 m_node_manager;

    List<Transform> m_TGA_list = null; //TGA
    List<Transform> m_neubau_bau_list = null; //nur Bau von Neubau
    List<Transform> m_neubau_list = null; //Neubau komplett

    bool m_init = false;
    bool m_init_GUI = false;

    GameObject m_BTN_showModes;
GameObject m_GUI_visCompos;

    // Use this for initialization
    void Start () {
        if (objectsValid())
        {
            m_BTN_showModes = GameObject.Find("BTN_Filter");
            m_GUI_visCompos = GameObject.Find("GUI-Modes").transform.Find("Vis_Komponenten").gameObject;

            showCompoGUI(false);

            m_init = true;

            m_TGA_list = new List<Transform>();

            Transform obj = m_neu_parkhaus.Search("TGA");
            if (obj != null)
            {
                m_TGA_list.Add(obj);
            }
            else{
                m_init = false;
            }

            obj = m_neu_parkhaus.Search("Stahlbau");
            if (obj != null)
            {
                m_neu_parkhaus_stahl = obj;
            }
            else
            {
                m_init = false;
            }

            obj = m_neu_hotel.Search("TGA");
            if (obj != null)
            {
                m_TGA_list.Add(obj);
            }
            else
            {
                m_init = false;
            }

            obj = m_neu_wum_welt.Search("TGA");
            if (obj != null)
            {
                m_TGA_list.Add(obj);
            }
            else
            {
                m_init = false;
            }

            obj = m_bestand.Search("TGA");
            if (obj != null)
            {
                m_TGA_list.Add(obj);
            }
            else
            {
                m_init = false;
            }

            m_neubau_bau_list = new List<Transform>();
            obj = m_neu_parkhaus.Search("Bau");
            if (obj != null)
            {
                m_neubau_bau_list.Add(obj);
            }
            else
            {
                m_init = false;
            }

            obj = m_neu_hotel.Search("Bau");
            if (obj != null)
            {
                m_neubau_bau_list.Add(obj);
            }
            else
            {
                m_init = false;
            }

            obj = m_neu_wum_welt.Search("Bau");
            if (obj != null)
            {
                m_neubau_bau_list.Add(obj);
            }
            else
            {
                m_init = false;
            }

            obj = m_bestand.Search("Bau");
            if (obj != null)
            {
                m_bestand_bau = obj.gameObject;
            }
            else
            {
                m_init = false;
            }

            m_neubau_list = new List<Transform>();
            m_neubau_list.Add(m_neu_hotel);
            m_neubau_list.Add(m_neu_parkhaus);
            m_neubau_list.Add(m_neu_wum_welt);
        }

        if (m_init)
        {
            initNodes();
			show_PointCloud (false);
        }
        else
        {
            Debug.Log("Error: [ HV_display_manager->Start() ] Objekte nicht initialisiert");
        }

    }
	
    void initNodes()
    {
        //Bau: Bestand
        add_multimaterial_node(m_bestand_bau.transform);

        //Bau: Neubau
        foreach (Transform t in m_neubau_bau_list) //include inactive
        {
            add_multimaterial_node(t);
        }
    }

    public bool initEventCam()
    {
        //nach GUI-init: Event-Camera für GUI setzen
        if (m_init_GUI==false)
        {
            GameObject obj_cam_left = GameObject.Find("left");
            GameObject obj_canvas = GameObject.Find("Canvas");
            if (obj_cam_left != null && obj_canvas != null)
            {
                Canvas myCanvas = obj_canvas.GetComponent<Canvas>();
                myCanvas.worldCamera = obj_cam_left.GetComponent<Camera>(); // -> event camera im Unity-Inspector:

                if (myCanvas.worldCamera == null)
                {
                    Debug.Log("ERROR [GUImanager->initEventCam] objects not found");
                    return false;
                }
                else
                {
                   
                    Debug.Log("[GUImanager->initEventCam] OK");
                    return true;
                }
            }
            else
                return false;
        } else
        return true;
    }

    public void showCompoGUI(bool showGUI)
    {
        m_BTN_showModes.SetActive(!showGUI);
        m_GUI_visCompos.SetActive(showGUI);
    }

    void add_multimaterial_node(Transform root_node)
    {
        if (root_node != null)
        {
            foreach (Transform t in root_node.GetComponentsInChildren<Transform>(true)) //include inactive
            {
                Renderer childRenderer = t.GetComponent<Renderer>();
                if (childRenderer != null)
                {
                    //t.gameObject.AddComponent<MeshCollider>();
                    t.gameObject.AddComponent<multiMaterialNode>();
                    multiMaterialNode myNode = t.gameObject.GetComponent<multiMaterialNode>();

                    if (myNode!= null)
                    {
                        //myNode.initOriginalMaterial();
                        //myNode.setOriginalMaterial();
                    }
                }
            }
        }
    }

   void setTransparent(Transform root_node, bool isTransparent)
    {
        if (m_init &&root_node!=null)
        {
            foreach (Transform t in root_node.GetComponentsInChildren<Transform>(true)) //include inactive
            {
                multiMaterialNode childWire = t.GetComponent<multiMaterialNode>();
                if (childWire != null)
                {
                    childWire.setWireframeMode(isTransparent);
                }
            }
        }
        else
            Debug.Log("ERROR: [HV_display_manager->setTransparent]");
    }

    bool objectsValid()
    {
        bool valid = false;
       if( m_neu_parkhaus!=null && m_neu_hotel!=null && m_neu_wum_welt!=null && m_bestand != null && m_punktwolke != null && m_gelaende != null && m_canvas!=null)
        {
            valid = true;
        }
        else
        {
            Debug.Log("ERROR [HV_display_manager]->objectsValid() : Mindenstens ein Objekt fehlt!");
        }
        return valid;
    }
	// Update is called once per frame
	void Update () {
        m_init_GUI = initEventCam();

    }


    public void show_TGA(bool show)
    {
        if (m_init){
            foreach (Transform trans in m_TGA_list)
            {
                trans.gameObject.SetActive(show);
            }
        }
    }


    public void show_PointCloud(bool show)
    {
        if (m_init)
        {
            m_punktwolke.gameObject.SetActive(show);
        }
    }

    public void show_Gelaende(bool show)
    {
        if (m_init)
        {
            m_gelaende.gameObject.SetActive(show);
        }
    }

    public void show_Neubau(bool show)
    {
        if (m_init)
        {
            foreach (Transform trans in m_neubau_list)
            {
                trans.gameObject.SetActive(show);
            }
        }
    }

    public void show_Bestand(bool show)
    {
        if (m_init)
        {
            m_bestand.gameObject.SetActive(show);
        }
    }

    public void make_Bestand_transparent(bool isTransparent)
    {
        if (m_init)
        {
            setTransparent(m_bestand_bau.transform, isTransparent);
        }
    }

    public void make_Neubau_transparent(bool isTransparent)
    {
        if (m_init)
        {
            foreach (Transform trans in m_neubau_bau_list)
            {
                setTransparent(trans, isTransparent);
            }

            //setTransparent(m_neu_parkhaus_stahl, false); //Ausnahme Stahlbau
        }
    }
}
