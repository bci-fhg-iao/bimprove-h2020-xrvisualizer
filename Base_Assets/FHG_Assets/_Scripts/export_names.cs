using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class export_names : MonoBehaviour
{
    public Transform m_rootObject;

    // Use this for initialization
    void Start()
    {
        if (m_rootObject != null)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\temp\WriteLines2.txt"))
            {
                string obj_name = "";
                foreach (Transform t in m_rootObject.GetComponentsInChildren<Transform>(true)) //include inactive
                {
                    obj_name = t.gameObject.name;
                    file.WriteLine(obj_name);
                }
            }
        }
        //    foreach (Transform t in m_rootObject.GetComponentsInChildren<Transform>(true)) //include inactive
        //    {
        //        Renderer childRenderer = t.GetComponent<Renderer>();
        //        if (childRenderer != null)
        //        {
        //            t.gameObject.AddComponent<MeshCollider>();
        //            t.gameObject.AddComponent<multiMaterialNode>();
        //        }
        //    }
        //}

    }

    // Update is called once per frame
    void Update()
    {

    }
}

