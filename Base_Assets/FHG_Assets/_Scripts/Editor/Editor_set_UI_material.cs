 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Autodesk-Materialien werden beim Import von FBX-Dateien aus Revit von Unity NICHT erkannt -> ungültige Materialdefinitionen 
// Dieses Editor-Skript fügt Unity-Materialien hinzu: 
// - Modus "einfach": angegebenes Material wird allen Knoten hinzugefügt, die nicht "Glas" oder "glas" oder "Fensterelement" im Namen enthalten. Glas-Material für die durchsichtigen "Glas-Knoten"
// - TODO: Materialdatenbank und Zuweisung nach Knoten-Namen

public class Editor_set_UI_material : EditorWindow
{
    Dictionary<string, Material> m_materials; //materials

    public Material m_setMaterial;
    public GameObject m_3D_model;
    public bool m_is_raycast_target = false;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("VR-Workflow/UI-Material")]

    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(Editor_set_UI_material));
    }

    // Use this for initialization
    void Start()
    {

    }

    void OnGUI()
    {
        GUILayout.Label("Material-Zuweisung", EditorStyles.boldLabel);

        m_3D_model = EditorGUILayout.ObjectField("3D-Modell", m_3D_model, typeof(Object), true) as GameObject;
        m_setMaterial = EditorGUILayout.ObjectField("Material", m_setMaterial, typeof(Object), true) as Material;
        m_is_raycast_target = EditorGUILayout.Toggle("Ist Raycast-Target", m_is_raycast_target);

        if (GUILayout.Button("zuweisen"))
        {
            if (m_3D_model == null || m_setMaterial == null)
                ShowNotification(new GUIContent("Bitte Material und Zielobjekt angegeben"));
            else
                set_UI_material();
        }

        //if (GUILayout.Button("Löschen"))
        //{
        //    if (m_3D_model == null || m_setMaterial == null)
        //        ShowNotification(new GUIContent("Bitte Material und Zielobjekt angegeben"));
        //    else
        //        clearMaterials();
        //}
    }



    // Update is called once per frame
    void Update()
    {

    }

    void set_UI_material()
    {
        if (m_3D_model != null && m_setMaterial != null)
        {
            UnityEngine.UI.Image my_image;
            UnityEngine.UI.Text my_text;

            foreach (Transform childTrans in m_3D_model.GetComponentsInChildren<Transform>(true)) //include inactive
            {
                my_image = childTrans.GetComponent<UnityEngine.UI.Image>();
                if (my_image!=null)
                {
                    my_image.material = m_setMaterial;
                    my_image.raycastTarget = m_is_raycast_target;
                }

                my_text = childTrans.GetComponent<UnityEngine.UI.Text>();
                if (my_text != null)
                {
                    my_text.material = m_setMaterial;
                    my_text.raycastTarget = m_is_raycast_target;
                }
            }
        }
        else
        {
            Debug.Log("ERROR [Editor_Material_Manager->applySimpleMaterial]: kein Material oder Zielobjekt");
        }
    }

   }

