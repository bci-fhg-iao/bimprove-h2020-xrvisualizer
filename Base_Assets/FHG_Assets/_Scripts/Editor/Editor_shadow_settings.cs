using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Editor_shadow_settings : EditorWindow
{
    public GameObject m_3D_model;
    public bool m_cast_shadows = true;
    public bool m_receive_shadows = true;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("VR-Workflow/Shadow-settings")]

    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(Editor_shadow_settings));
    }

    // Use this for initialization
    void Start()
    {

    }

    void OnGUI()
    {
        GUILayout.Label("Material-Zuweisung", EditorStyles.boldLabel);

        m_3D_model = EditorGUILayout.ObjectField("3D-Modell", m_3D_model, typeof(Object), true) as GameObject;
        m_cast_shadows = EditorGUILayout.Toggle("Cast shadows", m_cast_shadows);
        m_receive_shadows = EditorGUILayout.Toggle("Receive shadows", m_receive_shadows);

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
                    myRenderer.receiveShadows = m_receive_shadows;
                    if (m_cast_shadows)
                        myRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    else
                        myRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
            }
        }
        else
        {
            Debug.Log("ERROR [Editor_Material_Manager->applySimpleMaterial]: kein Material oder Zielobjekt");
        }

    }
}
