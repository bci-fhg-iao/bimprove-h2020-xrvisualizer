using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class replaceMaterial : EditorWindow
{
    public Material m_new_Material;
    public Material m_old_Material;
    public GameObject m_3D_model;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("VR-Workflow/Material ersetzen")]

    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(replaceMaterial));
    }

    // Use this for initialization
    void Start()
    {

    }

    void OnGUI()
    {
        GUILayout.Label("Material-Ersetzung", EditorStyles.boldLabel);

        m_3D_model = EditorGUILayout.ObjectField("3D-Objekt", m_3D_model, typeof(Object), true) as GameObject;
        m_new_Material = EditorGUILayout.ObjectField("Neues Material", m_new_Material, typeof(Object), true) as Material;
        m_old_Material = EditorGUILayout.ObjectField("Altes Material", m_old_Material, typeof(Object), true) as Material;

        if (GUILayout.Button("zuweisen"))
        {
            if (m_3D_model == null || m_new_Material == null || m_old_Material == null)
                ShowNotification(new GUIContent("Bitte Materialien und Zielobjekt angegeben"));
            else
                changeMaterial();
        }
    }

    void changeMaterial()
    {
        if (m_3D_model == null || m_new_Material == null || m_old_Material == null)
        {
            Debug.Log("ERROR replaceMaterial->changeMaterial] Materials or target NULL");
        }
        else
        {
            Debug.Log("Ersetze Material:\n" + m_old_Material.name + "  ->  " + m_new_Material.name);

            Renderer myRenderer = null;
            Material existingMat = null;
            int material_changes = 0;

            foreach (Transform childTrans in m_3D_model.GetComponentsInChildren<Transform>(true)) //include inactive
            {
                myRenderer = childTrans.GetComponent<Renderer>();

                if (myRenderer != null) //Wenn Geometrie-Knoten
                {
                    int matSize = myRenderer.materials.Length;
                    if (matSize > 0)
                    {
                        material_changes = 0;
                        Material[] newMaterials = new Material[matSize];

                        for (int i = 0; i < matSize; i++)
                        {
                            existingMat = myRenderer.materials[i];

                            ////geht nicht:
                            //if (m_old_Material == myRenderer.materials[i])
                            // if (myMat.Equals(m_old_Material))

                            string name = m_old_Material.name + "(Instance)";
                            string existname = existingMat.name;

                            Debug.Log("Check: " + existingMat.name + " " + m_old_Material.name);
                            //if (existingMat.name== m_old_Material.name || existingMat.name == m_old_Material.name + " (Instance)")
                            if (existingMat.name == m_old_Material.name || existingMat.name.Contains(m_old_Material.name))// + " (Instance)")
                                {
                                Debug.Log("change");
                                newMaterials[i] = m_new_Material;
                                material_changes++;
                            }
                            else { 
                                newMaterials[i] = existingMat;                                                           //Debug.Log("Material-ID of existing Material: " + myRenderer.materials[i].GetInstanceID());
                            }
                        }

                        if (material_changes > 0)
                        {
                            //myRenderer.materials = newMaterials;
                            myRenderer.sharedMaterials = newMaterials;
                        }
                    }
                }
            }


        }
    }
}

