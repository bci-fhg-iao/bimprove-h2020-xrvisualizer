using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Autodesk-Materialien werden beim Import von FBX-Dateien aus Revit von Unity NICHT erkannt -> ungültige Materialdefinitionen 
// Dieses Editor-Skript fügt Unity-Materialien hinzu: 
// - Modus "einfach": angegebenes Material wird allen Knoten hinzugefügt, die nicht "Glas" oder "glas" oder "Fensterelement" im Namen enthalten. Glas-Material für die durchsichtigen "Glas-Knoten"
// - TODO: Materialdatenbank und Zuweisung nach Knoten-Namen

public class Editor_Material_Manager : EditorWindow
{
    Dictionary<string, Material> m_materials; //materials

    public Material m_setMaterial;
    public GameObject m_3D_model;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("VR-Workflow/Material-Zuweisung")]

    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(Editor_Material_Manager));
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

        if (GUILayout.Button("zuweisen"))
        {
            if (m_3D_model == null || m_setMaterial == null)
                ShowNotification(new GUIContent("Bitte Material und Zielobjekt angegeben"));
            else
                applySimpleMaterial();
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

    void applySimpleMaterial()
    {
        if (m_3D_model != null && m_setMaterial != null)
        {
            string obj_name;
            Renderer myRenderer = null;

            m_materials = new Dictionary<string, Material>();
            m_materials.Add("Glas", (Material)Resources.Load("Arch_Materials/Glas", typeof(Material)));

            foreach (Transform childTrans in m_3D_model.GetComponentsInChildren<Transform>(true)) //include inactive
            {
                obj_name = childTrans.gameObject.name;
                myRenderer = childTrans.GetComponent<Renderer>();

                if (myRenderer != null) //Wenn Geometrie-Knoten
                {
                    if (obj_name.Contains("Glas") || obj_name.Contains("glas") || obj_name.Contains("Fenster") || obj_name.Contains("Fensterelement") || obj_name.Contains("Fensterfüllung") || obj_name.Contains("Flurfenster") || obj_name.Contains("Drehtür") ) //Glas?
                    {
                        setMaterial(myRenderer, m_materials["Glas"]);
                    }
                    else
                    {
                        setMaterial(myRenderer, m_setMaterial);
                    }
                }
            }
        }
        else
        {
            Debug.Log("ERROR [Editor_Material_Manager->applySimpleMaterial]: kein Material oder Zielobjekt");
        }
    }

    void setMaterial(Renderer myRenderer, Material myMaterial)
    {        
        if(myRenderer!=null && myMaterial!=null)
        {
            // int matSize = myRenderer.sharedMaterials.Length; // Änderungen  temporär ->können nicht in der Szene gespeichert werden
            int matSize = myRenderer.materials.Length; //permamente Änderungen!
            Material[] newMaterials = new Material[matSize];

            for (int i=0; i< matSize; i++)
            {
                newMaterials[i] = myMaterial;
            }

            //myRenderer.sharedMaterials = newMaterials;// Änderungen temporär -> können nicht in der Szene gespeichert werden
            myRenderer.materials = newMaterials; //permamente Änderungen!
        }
        else
        {
            Debug.Log("ERROR [Editor_Material_Manager->setMaterial]: kein Material oder Renderer");
        }

    }


    //void clearMaterials()
    //{
    //    if (m_3D_model != null && m_setMaterial != null)
    //    {

    //    }
    //    else
    //    {
    //        Debug.Log("ERROR [Editor_Material_Manager->clearMaterials]: kein Material oder Zielobjekt");
    //    }
    //}
}

