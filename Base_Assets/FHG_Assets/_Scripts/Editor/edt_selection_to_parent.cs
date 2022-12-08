using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Linq;
public class edt_selection_to_parent : EditorWindow
{
    public GameObject m_3D_model;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("3D-Workflow/Nodes umordnen")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(edt_selection_to_parent));
    }

    void OnGUI()
    {
        GUILayout.Label("Material-Zuweisung", EditorStyles.boldLabel);

        m_3D_model = EditorGUILayout.ObjectField("Neuer Knoten", m_3D_model, typeof(Object), true) as GameObject;
        

        if (GUILayout.Button("anwenden"))
        {
            if (m_3D_model == null)
                ShowNotification(new GUIContent("Bitte Zielobjekt angegeben"));
            else
                attachSelection();
        }

    }

    void attachSelection()
    {
        if (m_3D_model != null && Selection.gameObjects.Length > 0)
        {
            foreach (GameObject obj in Selection.gameObjects) //include inactive
            {
                obj.transform.parent = m_3D_model.transform;
            }
        }
    }

}
