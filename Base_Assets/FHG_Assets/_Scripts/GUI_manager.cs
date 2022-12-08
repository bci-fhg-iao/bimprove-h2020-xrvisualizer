using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;//.EventSystems.UIBehaviour;

public class GUI_manager : MonoBehaviour
{

    //Dictionary<string, bool> m_visComponents;
    //Dictionary<string, GameObject> m_Components;

    Canvas m_canvas;
    bool m_canvas_found = false;

    public GameObject m_3D_model;

    bool m_init = false;
    bool m_init_vis = false;
    bool m_init_GUI = false;
    bool m_init_VR_cams = false;

    public bool m_show_BCF = true;
    public bool m_show_Filter = true;

    // UnityEngine.UI.Button showModes;
    GameObject m_BTN_showModes;
    GameObject m_GUI_visCompos;

    UnityEngine.UI.Text m_TXT_BCF;
    UnityEngine.UI.Text m_Header_BCF;
    Material m_setMaterial;

    //BCF_folder_manager m_bcf_manager;


    nodeManager m_matNode_Manager;
    set_viewpoints m_viewpoints;

    //modelManager m_model_manager;

    // Use this for initialization
    void Start()
    {
        m_setMaterial = (Material)Resources.Load("GUIOverlay", typeof(Material));
      //  m_bcf_manager = GameObject.Find("_Scripts").GetComponent<BCF_folder_manager>();
        m_matNode_Manager = GameObject.Find("_Scripts").GetComponent<nodeManager>();
        m_viewpoints = GameObject.Find("Main Camera").GetComponent<set_viewpoints>();

        //m_model_manager = gameObject.AddComponent<modelManager>() as modelManager;
        //m_model_manager.m_3D_model = m_3D_model;

    }



    // Update is called once per frame
    void Update()
    {
        if (m_init == false)
        {
            //if (m_init_vis ==false & m_model_manager.isInit() == true)
            //{
            //    //show initial scenario
            //    m_model_manager.showScene_Parkhaus(true);

            //    // TODO m_matNode_Manager.setComponents(m_Components);
            //    m_init_vis = true;
            //}

            if (m_canvas_found == false)
                m_canvas_found = findCanvas();

            if (m_init_VR_cams == false)
                initEventCam();

            if (m_init_GUI == false)
            {
                m_init_GUI = initCanvas();
            }

            m_init = isInitOK();

        }
        else
        {

            //Keyboard commands
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    m_bcf_manager.selectStartView();
            //    if (m_viewpoints != null)
            //        m_viewpoints.apply_camPos(0);
            //}

            //if (Input.GetKeyDown(KeyCode.LeftArrow))
            //{
            //    m_bcf_manager.nextCameraPos(-1);
            //}

            //if (Input.GetKeyDown(KeyCode.RightArrow))
            //{
            //    m_bcf_manager.nextCameraPos(1);
            //}

            //if (Input.GetKeyDown(KeyCode.UpArrow))
            //{
            //    m_bcf_manager.nextReport(1);
            //}

            //if (Input.GetKeyDown(KeyCode.DownArrow))
            //{
            //    m_bcf_manager.nextReport(-1);
            //}

            if (Input.GetKeyDown(KeyCode.I))
            {
                initEventCam();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_EDITOR
                if (EditorApplication.isPlaying)
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }
#endif
                Application.Quit();
            }
        }
    }

    bool isInitOK()
    {
        //if (m_init_vis && m_canvas_found && m_init_VR_cams && m_init_GUI)
        if (m_canvas_found && m_init_VR_cams && m_init_GUI)
        {
            return true;
        }
        else return false;
    }

    bool findCanvas()
    {
        GameObject canvas_obj = GameObject.Find("Canvas");
        if (canvas_obj != null)
        {
            m_canvas = canvas_obj.GetComponent<Canvas>();

            //Skalieren des Canvas auf Bildschirmgröße

            GameObject left_cam_obj = GameObject.Find("left");
            if (left_cam_obj != null)
            {
                //float pixels_per_unit = m_canvas.GetComponent<CanvasScaler>().referencePixelsPerUnit;
                //Rect canvas_Rect = m_canvas.pixelRect;

                //float scale_factor = 143.0f / m_canvas.pixelRect.width/ pixels_per_unit;

                //Vector3 scale = new Vector3(scale_factor, scale_factor, scale_factor);
                //m_canvas.transform.localScale = scale;

                return true;
            }
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    void initEventCam()
    {
        //    bool success=false;

        //nach GUI-init: Event-Camera für GUI setzen
        if (m_init_GUI)
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
                }
                else
                {
                    m_init_VR_cams = true;
                    Debug.Log("[GUImanager->initEventCam] OK");
                }
            }

        }
    }




    bool initCanvas()
    {
        //showModes = GameObject.Find("BTN_Filter").GetComponent<UnityEngine.UI.Button>();
        m_BTN_showModes = GameObject.Find("BTN_Filter");
        m_GUI_visCompos = GameObject.Find("GUI-Modes").transform.Find("Vis_Komponenten").gameObject;

        m_TXT_BCF = GameObject.Find("TXT_BCF").GetComponent<UnityEngine.UI.Text>();
        m_Header_BCF = GameObject.Find("Header_BCF").GetComponent<UnityEngine.UI.Text>();

        GameObject BCF_GUI_obj = GameObject.Find("BCF_Topics");

        if (m_BTN_showModes != null && m_GUI_visCompos != null && m_TXT_BCF != null && m_Header_BCF != null)
        {
            Debug.Log("GUI-Manager: all GUI elements found");
            showCompoGUI(false);

            //if (m_bcf_manager != null)
            //    m_bcf_manager.selectCamPos(0);

            if (BCF_GUI_obj != null)
                BCF_GUI_obj.SetActive(m_show_BCF);

            m_TXT_BCF.enabled = m_show_BCF;
            m_Header_BCF.enabled = m_show_BCF;

            m_BTN_showModes.SetActive(m_show_Filter);

            set_UI_material();

            return true;
        }
        else
        {
            Debug.Log("GUI-Manager: ERROR GUI elements missing");
            return false;
        }

        //test canvas scale for VR

    }



    //update GUI
    public void setBCFheader(string BCF_header)
    {
        if (m_Header_BCF != null)
            m_Header_BCF.text = BCF_header;
    }

    public void setBCFcomment(string BCF_txt)
    {
        if (m_TXT_BCF != null)
            m_TXT_BCF.text = BCF_txt;
    }



    //Button-Commands
    public void setWireframeArch(bool isWireframe)
    {
        if (m_matNode_Manager != null)
        {
            m_matNode_Manager.setWireframe(isWireframe, "Architektur");
        }
    }

    public void setWireframeKonstr(bool isWireframe)
    {
        if (m_matNode_Manager != null)
        {
            m_matNode_Manager.setWireframe(isWireframe, "Konstruktion");
        }
    }

    public void showCompoGUI(bool showGUI)
    {
        m_BTN_showModes.SetActive(!showGUI);
        m_GUI_visCompos.SetActive(showGUI);
    }



    //public void showArch(bool showIt)
    //{
    //    if (m_init_vis)
    //    {
    //       m_Components["Architektur"].SetActive(showIt);
    //    }
    //}

    //public void showKonstr(bool showIt)
    //{
    //    if (m_init_vis)
    //    {
    //        m_Components["Konstruktion"].SetActive(showIt);
    //    }
    //}

    //public void showElektro(bool showIt)
    //{
    //    if (m_init_vis)
    //    {
    //        m_Components["Elektro"].SetActive(showIt);
    //    }
    //}

    //public void showHLS(bool showIt)
    //{
    //    if (m_init_vis)
    //    {
    //        m_Components["HLSK"].SetActive(showIt);
    //    }
    //}

    //public void showMoebel(bool showIt)
    //{
    //    if (m_init_vis)
    //    {
    //        m_Components["Moebel"].SetActive(showIt);
    //    }
    //}

    //public void showStahlbau(bool showIt)
    //{
    //    if (m_init_vis)
    //    {
    //        m_Components["Stahlbau"].SetActive(showIt);
    //    }
    //}

    //public void showKueche(bool showIt)
    //{
    //    if (m_init_vis)
    //    {
    //        m_Components["Kueche"].SetActive(showIt);
    //    }
    //}

    //public void next_BCF_topic(int step)
    //{
    //    //if (m_bcf_manager != null)
    //    //{
    //    //    m_bcf_manager.nextCameraPos(step);
    //    //}
    //}

    //public void next_BCF_report(int step)
    //{
    //    //if (m_bcf_manager != null)
    //    //{
    //    //    m_bcf_manager.nextReport(step);
    //    //}
    //}

    void set_UI_material()
    {
        if (m_3D_model != null && m_setMaterial != null)
        {
            UnityEngine.UI.Image my_image;
            UnityEngine.UI.Text my_text;

            int counter = 0;
            foreach (Transform childTrans in m_canvas.GetComponentsInChildren<Transform>(true)) //include inactive
            {
                Debug.Log("Add Overlay to" + childTrans.name + " " + counter++);
                my_image = childTrans.GetComponent<UnityEngine.UI.Image>();
                if (my_image != null)
                {
                    my_image.material = m_setMaterial;
                    my_image.raycastTarget = true;
                }

                my_text = childTrans.GetComponent<UnityEngine.UI.Text>();
                if (my_text != null)
                {
                    my_text.material = m_setMaterial;
                    my_text.raycastTarget = true;
                }
            }
        }
        else
        {
            Debug.Log("ERROR [Editor_Material_Manager->applySimpleMaterial]: kein Material oder Zielobjekt");
        }
    }
}
