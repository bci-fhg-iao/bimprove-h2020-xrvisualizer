using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//enum Plant { Baum_Esche = 0, Baum_Graubirke = 1, Baum_Rotahorn = 2, Baum_Rotesche = 3, Baum_Schwarzer_Hollunder = 4, Strauch_Berberitze = 100 };

public class createPlants : EditorWindow
{

    public GameObject m_input_obj;
    public GameObject m_output_obj;

    private float m_scale = 30.48f;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("VR-Workflow/Pflanzen-Generierung")]

    static void Init()
    {
        // Get existing open window or if none, make a new one:
        createPlants window = (createPlants)EditorWindow.GetWindow(typeof(createPlants));
        window.Show();
    }


    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        GUILayout.Label("Create Plants", EditorStyles.boldLabel);

        m_input_obj = EditorGUILayout.ObjectField("Quelle", m_input_obj, typeof(Object), true) as GameObject;
        m_output_obj = EditorGUILayout.ObjectField("Ziel", m_output_obj, typeof(Object), true) as GameObject;
        m_scale = EditorGUILayout.FloatField("Skalierungsfaktor", m_scale);

        if (GUILayout.Button("create"))
        {
            setPlants();
            clearPlants();
        }
    }

    void clearPlants()
    {

    }

    void setPlants()
    {
        // GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        //Vector3 deltaPos = new Vector3(0, -0.173f, 0);
        //Vector3 scale = new Vector3(0.32f, 0.112f, 0.32f);
        Vector3 deltaPos = new Vector3(0, 0, 0);
       // Vector3 scale = new Vector3(0.32f, 0.107f, 0.32f);

        //   Vector3 scale = new Vector3(0.32f, 0.0039f* 30.48f, 0.32f); 
        GameObject myPlant = null;

        if (m_input_obj != null & m_output_obj != null)
        {
            foreach (Transform t in m_input_obj.GetComponentsInChildren<Transform>(true)) //include inactive
            {
                myPlant = null;

                if (t.name.Contains("Laubbaum Esche"))
                {
                    myPlant = Instantiate(Resources.Load("Plants/Broadleaf_Desktop")) as GameObject;
                    myPlant.transform.localScale = new Vector3(1, 1, 1) * 5.6f / 18.6f; //18.6 Höhe des Baums "Speedtree Broadleaf" ab OK Boden
                    
                }
                else if (t.name.Contains("Laubbaum Graubirke"))
                {
                    myPlant = Instantiate(Resources.Load("Plants/Broadleaf_Desktop")) as GameObject;
                    myPlant.transform.localScale = new Vector3(1, 1, 1) * 3.1f / 18.6f; //18.6 Höhe des Baums "Speedtree Broadleaf" ab OK Boden
                }

                else if (t.name.Contains("Laubbaum Rotahorn"))
                {
                    myPlant = Instantiate(Resources.Load("Plants/Birch_1")) as GameObject;
                    myPlant.transform.localScale = new Vector3(1, 1, 1) * 9.0f / 8.1f; //8.1 Höhe des Baums "Birch_1" ab OK Boden
                }

                else if (t.name.Contains("Laubbaum Rotesche"))
                {
                    myPlant = Instantiate(Resources.Load("Plants/Broadleaf_Desktop")) as GameObject;
                    myPlant.transform.localScale = new Vector3(1, 1, 1) * 7.6f / 18.6f; //18.6 Höhe des Baums "Speedtree Broadleaf" ab OK Boden
                }

                else if (t.name.Contains("Laubbaum Schwarzer Holunder"))
                {
                    myPlant = Instantiate(Resources.Load("Plants/Broadleaf_Desktop")) as GameObject;
                    myPlant.transform.localScale = new Vector3(1, 1, 1) * 4.5f / 18.6f; //18.6 Höhe des Baums "Speedtree Broadleaf" ab OK Boden
                }

                if (t.name.Contains("Strauch Berberitze"))
                {
                    myPlant = Instantiate(Resources.Load("Plants/bush02")) as GameObject;
                    myPlant.transform.localScale = new Vector3(1, 1, 1) * 1.0f / 1.65f; //1.65 Höhe des Strauchs "bush02" ab OK Boden
                }

                if (myPlant == null)
                {
                    myPlant = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    Debug.Log("No plant found for: " + t.name);
                }

                myPlant.name = t.name;

                myPlant.transform.position = new Vector3(t.localPosition.x * m_scale, (t.localPosition.y) * m_scale + deltaPos.y, t.localPosition.z * m_scale);
                // myPlant.transform.localScale = scale;
                myPlant.transform.parent = m_output_obj.transform;
                randomHeading(myPlant);
            }
        }
    }

    void randomHeading(GameObject myObj)
    {
        if (myObj != null)
        {
            myObj.transform.localRotation = Quaternion.AngleAxis(Random.value*360, Vector3.up);
        }
    }

}
