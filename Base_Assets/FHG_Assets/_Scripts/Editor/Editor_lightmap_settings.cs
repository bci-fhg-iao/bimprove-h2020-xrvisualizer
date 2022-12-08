using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public class Editor_lightmap_settings : EditorWindow
{
    public GameObject m_3D_model;
    public bool m_lightmap_Static = true;
    public bool m_lightprobes_proxy_volumes = true;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("VR-Workflow/Lightmap-settings")]

    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(Editor_lightmap_settings));
    }

    // Use this for initialization
    void Start()
    {

    }

    void OnGUI()
    {
        GUILayout.Label("Lightmap-Settings", EditorStyles.boldLabel);

        m_3D_model = EditorGUILayout.ObjectField("3D-Modell", m_3D_model, typeof(Object), true) as GameObject;
        m_lightmap_Static = EditorGUILayout.Toggle("Lightmap static", m_lightmap_Static);
        m_lightprobes_proxy_volumes = EditorGUILayout.Toggle("Use lightprobe proxy volume", m_lightprobes_proxy_volumes);

        if (GUILayout.Button("anwenden"))
        {
            if (m_3D_model == null)
                ShowNotification(new GUIContent("Bitte Zielobjekt angegeben"));
            else
                applyShadowSetings();
        }

        //if (GUILayout.Button("Löschen"))
        //{
        //    if (m_3D_model == null || m_setMaterial == null)
        //        ShowNotification(new GUIContent("Bitte Material und Zielobjekt angegeben"));
        //    else
        //        clearMaterials();
        //}
    }

    void applyShadowSetings()
    {
        if (m_3D_model != null)
        {
            string obj_name;
            Renderer myRenderer = null;


            foreach (Transform childTrans in m_3D_model.GetComponentsInChildren<Transform>(true)) //include inactive
            {
                obj_name = childTrans.gameObject.name;
                myRenderer = childTrans.GetComponent<Renderer>();

                if (myRenderer != null) //Wenn Geometrie-Knoten
                {
                    if (m_lightprobes_proxy_volumes)
                        myRenderer.lightProbeUsage = LightProbeUsage.UseProxyVolume;

                    if (m_lightmap_Static == false)
                    {
                     //myRenderer.
                    }

                }
            }
        }
        else
        {
            Debug.Log("ERROR [Editor_Material_Manager->applySimpleMaterial]: kein Material oder Zielobjekt");
        }

    }
}
