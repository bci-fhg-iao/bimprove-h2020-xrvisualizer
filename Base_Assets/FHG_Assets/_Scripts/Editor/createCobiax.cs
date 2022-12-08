using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class createCobiax : EditorWindow
{

    public GameObject m_3D_model;
    public GameObject m_cobiax_obj;
    public CapsuleCollider m_capsule;
    public Vector3 m_delta_pos;
    //public Vector3 m_scale;

    private MeshFilter my_meshfilter;
    private MeshRenderer my_meshrenderer;
    private BoxCollider my_boxcollider;

    private ArrayList myNodes;
    private float m_scale = 30.48f;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("VR-Workflow/Cobiax-Generierung")]

    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(createCobiax));
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        GUILayout.Label("Create Cobiax", EditorStyles.boldLabel);

        m_3D_model = EditorGUILayout.ObjectField("Quelle", m_3D_model, typeof(Object), true) as GameObject;
        m_cobiax_obj = EditorGUILayout.ObjectField("Ziel", m_cobiax_obj, typeof(Object), true) as GameObject;
        m_scale = EditorGUILayout.FloatField("Skalierungsfaktor", m_scale);

        if (GUILayout.Button("create"))
        {
            createCapsules();
        }
    }

    void createCapsules()
    {
       // GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        //Vector3 deltaPos = new Vector3(0, -0.173f, 0);
        //Vector3 scale = new Vector3(0.32f, 0.112f, 0.32f);
        Vector3 deltaPos = new Vector3(0, -0.316f, 0);
        Vector3 scale = new Vector3(0.32f, 0.107f, 0.32f);

        //   Vector3 scale = new Vector3(0.32f, 0.0039f* 30.48f, 0.32f); 
        GameObject myCapsule;

        if (m_3D_model != null & m_cobiax_obj != null)
        {
            foreach (Transform t in m_3D_model.GetComponentsInChildren<Transform>(true)) //include inactive
            {
                Renderer childRenderer = t.GetComponent<Renderer>();
                if (childRenderer != null)
                {
                    myCapsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    myCapsule.transform.position = new Vector3(t.localPosition.x* m_scale, (t.localPosition.y) * m_scale+ deltaPos.y, t.localPosition.z * m_scale);
                    //myCapsule.transform.localScale = new Vector3(0.32f, 0.0039f * m_scale, 0.32f); 
                    myCapsule.transform.localScale = scale;

                    myCapsule.transform.parent = m_cobiax_obj.transform;

                }
            }
        }
    }

}
