// Author FNS/311
// Vorlage: http://wiki.unity3d.com/index.php?title=SetPivot


/* 
 * USAGE:
 * - select geometry object
 * - GUI: GameObject -> Set Scale
 * - apply scale factor
 */


using UnityEngine;
using UnityEditor;
using System;

public class SetScale : EditorWindow
{

    float m_scale = 1; //scale
    float m_scaleInput=1;

    GameObject m_obj; //Selected object in the Hierarchy
    Transform m_cachedTransform;
    MeshFilter m_meshFilter; //Mesh Filter of the selected object
    Mesh m_mesh; //Mesh of the selected object
    //Collider col; //Collider of the selected object

    [MenuItem("IAO-Workflow/Set Scale")] //Place the SetScale menu item in the GameObject menu

    static void Init()
    {
        SetScale window = (SetScale)EditorWindow.GetWindow(typeof(SetScale));
        window.RecognizeSelectedObject(); //Initialize the variables by calling RecognizeSelectedObject on the class instance
        window.Show();
    }

    void OnGUI()
    {
        if (m_obj)
        {
            if (m_mesh && m_cachedTransform) 
            {
                m_scaleInput = EditorGUILayout.FloatField("Scale Factor:", m_scaleInput);
                EditorGUILayout.LabelField( "X Width \t[m]:",  Math.Round( m_scale*m_mesh.bounds.size.x, 3).ToString() );
                EditorGUILayout.LabelField( "Y Height \t[m]:", Math.Round( m_scale*m_mesh.bounds.size.y, 3).ToString() );
                EditorGUILayout.LabelField( "Z Length \t[m]:", Math.Round (m_scale*m_mesh.bounds.size.z, 3).ToString() );

                if (m_scaleInput > 0)
                {
                    m_scale = m_scaleInput;
                    m_cachedTransform.localScale = new Vector3(m_scale, m_scale, m_scale);
                    
                    //m_mesh.RecalculateBounds(); // no effect -> width = boundingX * scale
                }

            }
            else
            {
                GUILayout.Label("Selected object does not have a Mesh specified.");
            }
        }
        else
        {
            GUILayout.Label("No object selected in Hierarchy.");
        }
    }

    //When a selection change notification is received
    //recalculate the variables and references for the new object
    void OnSelectionChange()
    {
        RecognizeSelectedObject();
    }

    //Gather references for the selected object and its components
    //and update the pivot vector if the object has a Mesh specified
    void RecognizeSelectedObject()
    {
        Transform t = Selection.activeTransform;
        m_obj = t ? t.gameObject : null;
        if (m_obj)
        {
            m_meshFilter = m_obj.GetComponent(typeof(MeshFilter)) as MeshFilter;
            m_mesh = m_meshFilter ? m_meshFilter.sharedMesh : null;
            if (m_mesh)
            {
                m_cachedTransform = t;
                m_scale = m_cachedTransform.localScale.x;
                m_scaleInput = m_scale;
            }
            //col = obj.GetComponent(typeof(Collider)) as Collider;
            
        }
        else
        {
            m_mesh = null;
        }
    }

}