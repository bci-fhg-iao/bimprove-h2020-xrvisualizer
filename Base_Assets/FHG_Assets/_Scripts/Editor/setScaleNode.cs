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
using System.Collections;

public class SetScaleNode : EditorWindow
{

    float m_scale = 1; //scale
    float m_scaleInput = 1;

    GameObject m_obj; //Selected object in the Hierarchy
    Transform m_cachedTransform;
    Bounds m_bbox;
    //MeshFilter m_meshFilter; //Mesh Filter of the selected object
    //Mesh m_mesh; //Mesh of the selected object
    //Collider col; //Collider of the selected object

    [MenuItem("IAO-Workflow/Set Scale Node")] //Place the SetScale menu item in the GameObject menu

    static void Init()
    {
        SetScaleNode window = (SetScaleNode)EditorWindow.GetWindow(typeof(SetScaleNode));
        window.RecognizeSelectedObject(); //Initialize the variables by calling RecognizeSelectedObject on the class instance
        window.Show();
    }

    void OnGUI()
    {
        if (m_obj && m_cachedTransform)
        {

            if (m_bbox.size.x>0 && m_bbox.size.y>0 && m_bbox.size.z>0)
            {
                m_scaleInput = EditorGUILayout.FloatField("Scale Factor:", m_scaleInput);
                EditorGUILayout.LabelField("X Width \t[m]:",  Math.Round(m_scale * m_bbox.size.x, 3).ToString());
                EditorGUILayout.LabelField("Y Height \t[m]:", Math.Round(m_scale * m_bbox.size.y, 3).ToString());
                EditorGUILayout.LabelField("Z Length \t[m]:", Math.Round(m_scale * m_bbox.size.z, 3).ToString());

                if (m_scaleInput > 0)
                {
                    m_scale = m_scaleInput;
                    m_cachedTransform.localScale = new Vector3(m_scale, m_scale, m_scale);

                    //m_mesh.RecalculateBounds(); // no effect -> width = boundingX * scale
                }
            }
            else
            {
                GUILayout.Label("No boundingbox!");
            }
        }
        else { GUILayout.Label("No object selected!"); }

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
                m_cachedTransform = t;
                m_scale = m_cachedTransform.localScale.x;
                m_scaleInput = m_scale;
              

               // MeshFilter this_mf = m_obj.GetComponent(typeof(MeshFilter)) as MeshFilter;
                MeshFilter this_mf = m_obj.GetComponent<MeshFilter>();
                    if (this_mf == null)
                    {
                        m_bbox = new Bounds(Vector3.zero, Vector3.zero);
                    }
                    else
                    {
                        m_bbox = this_mf.sharedMesh.bounds;
                    }
                    MeshFilter[] mfs = m_obj.GetComponentsInChildren<MeshFilter>();
                    foreach (MeshFilter mf in mfs)
                    {
                        Vector3 pos = mf.transform.localPosition;
                        Bounds child_bounds = mf.sharedMesh.bounds;
                        child_bounds.center += pos;
                        m_bbox.Encapsulate(child_bounds);
                    }
                
        }
        else
        {
         //  GUILayout.Label("Selected object has no transform!");
        }
    }

}