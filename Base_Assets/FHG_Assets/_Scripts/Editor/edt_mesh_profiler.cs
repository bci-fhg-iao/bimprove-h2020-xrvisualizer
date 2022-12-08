using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Linq;

public class edt_mesh_profiler : EditorWindow
{
    public GameObject m_3D_model;
    public float m_min_volume = 0.0f;
    public float m_min_area = 0.0f;
    public bool m_print_debug = false;

    private int m_triangles_count;
    private int m_LOD_triangles_count;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("3D-Workflow/Mesh Profiler")]

    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(edt_mesh_profiler));
    }

    // Use this for initialization
    void Start()
    {

    }

    void OnGUI()
    {
        GUILayout.Label("Material-Zuweisung", EditorStyles.boldLabel);

        m_3D_model = EditorGUILayout.ObjectField("3D-Modell", m_3D_model, typeof(Object), true) as GameObject;
        m_min_volume = EditorGUILayout.FloatField("Min Volumen: [Liter]", m_min_volume);
        m_min_area = EditorGUILayout.FloatField("Min Fläche [cm²]:", m_min_area);
        m_print_debug = EditorGUILayout.Toggle("Alle Debug-Meldungen", m_print_debug);


        if (GUILayout.Button("anwenden"))
        {
            if (m_3D_model == null)
                ShowNotification(new GUIContent("Bitte Zielobjekt angegeben"));
            else
                profile_Mesh();
        }

    }

    void profile_Mesh()
    {
        Debug.Log("Start profiler");
        if (m_3D_model != null)
        {
            string obj_name;
            Renderer myRenderer = null;

            Vector3 size;
            float volume = 0.0f;
            float triangles = 0.0f;
            float area = 0.0f;

            GameObject details = check_LOD_obj();

            foreach (Transform childTrans in m_3D_model.GetComponentsInChildren<Transform>(true)) //include inactive
            {
                obj_name = childTrans.gameObject.name;
                myRenderer = childTrans.GetComponent<Renderer>();

                if (myRenderer != null) //Wenn Geometrie-Knoten
                {
                    volume = 0.0f;
                    triangles = 0.0f;

                    size = myRenderer.bounds.max - myRenderer.bounds.min;
                    volume = size[0] * size[1] * size[2];

                    triangles = myRenderer.gameObject.GetComponent<MeshFilter>().sharedMesh.triangles.Length / 3;

                    if (volume == 0)
                    {
                        area = getArea(size);

                        if(m_print_debug)
                            Debug.Log(myRenderer.gameObject.name + ": Area=" + area + "  -> Triangles: " + triangles);

                        if (m_min_area > 0 && area < m_min_area / 10000.0f) //cm² -> m²
                        {
                            myRenderer.gameObject.transform.parent = details.transform;
                            m_LOD_triangles_count = m_LOD_triangles_count + (int)triangles;
                        }
                        else
                        {
                            m_triangles_count = m_triangles_count + (int)triangles;
                        }
                    }
                    else
                    {
                        if (m_print_debug)
                            Debug.Log(myRenderer.gameObject.name + ": Volume Boundingbox=" + volume + "  -> Triangles: " + triangles);

                        if (m_min_volume > 0 && volume < m_min_volume / 1000.0f) //Liter -> m³
                        {
                            myRenderer.gameObject.transform.parent = details.transform;
                            m_LOD_triangles_count = m_LOD_triangles_count + (int)triangles;
                        }
                        else
                        {
                            m_triangles_count = m_triangles_count + (int)triangles;
                        }
                    }
                }
            }// end foreach

            Debug.Log("Triangles normal: " + System.Math.Round(m_triangles_count/1000000.0f,2) + " Mio.");
            Debug.Log("Triangles LOD   : " + System.Math.Round(m_LOD_triangles_count / 1000000.0f, 2) + " Mio.");
            Debug.Log("Triangles total   : " + System.Math.Round((m_LOD_triangles_count+ m_triangles_count) / 1000000.0f, 2) + " Mio.");


        }
        else
        {
            Debug.Log("ERROR [Editor_Material_Manager->applySimpleMaterial]: kein Material oder Zielobjekt");
        }
    }

    //für Planes in Koordinaten-Ebenen, ansonsten BoundingBox
    float getArea(Vector3 size)
    {
        float area = 0.0f;

        if (size[0] == 0)
            size[0] = 1;
        if (size[1] == 0)
            size[1] = 1;
        if (size[2] == 0)
            size[2] = 1;

        area = size[0] * size[1] * size[2];

        return area;
    }

    GameObject check_LOD_obj()
    {
        GameObject LOD;

        if (m_3D_model != null)
        {
            LOD = m_3D_model.Child("LOD_Details");

            if (LOD == null)
            {
                LOD = new GameObject();
            }

            LOD.name = "LOD_Details";
            LOD.transform.parent = m_3D_model.transform;

        }
        else
        {
            LOD = new GameObject();
            LOD.name = "LOD_Details";
            Debug.Log("Error: MeshProfiler->check_LOD_obj: created standard LOD holder");
        }
        return LOD;
    }
}
