using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOD_onTrigger : MonoBehaviour
{
    public float m_margin = 0.0f;

    MeshFilter[] m_meshes = null;
    GameObject m_details = null;
    bool m_init = false;

    Vector3 m_bb_min = Vector3.zero;
    Vector3 m_bb_max = Vector3.zero;



    void Awake()
    {
        m_meshes = GetComponentsInChildren<MeshFilter>();

       //calcMinMax(); //Fehler in y
       createBBox();

    }

    void Update()
    {
        if (!m_init)
        {
            m_details = transform.Find("LOD_Details").gameObject;
            if (m_details != null)
            {
                m_init = true;
                m_details.SetActive(false);
            }
        }
    }

    void createBBox()
    {
        BoxCollider colBox = GetComponent<BoxCollider>();

        if (colBox == null)
        {
            colBox = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
            colBox.isTrigger = true;
        }

        Quaternion currentRotation = this.transform.rotation;
        this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        Bounds bounds = new Bounds(this.transform.position, Vector3.zero);
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }
        //Vector3 localCenter = bounds.center - this.transform.position;
        //bounds.center = localCenter;

        Debug.Log("The local bounds of this model is " + bounds);
        this.transform.rotation = currentRotation;

        colBox.center = bounds.center - this.transform.position;
        colBox.extents = new Vector3(bounds.extents.x + m_margin, bounds.extents.y + m_margin, bounds.extents.z + m_margin);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter: Pos("+ Camera.main.transform.position);
        if (m_init && m_details.transform.parent.gameObject.activeInHierarchy)
        {
            m_details.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exit: Pos(" + Camera.main.transform.position);

        if (m_init)
        {
            m_details.SetActive(false);
        }
    }



    void calcMinMax()
    {
        Vector3 min = Vector3.zero;
        Vector3 max = Vector3.zero;

        Quaternion currentRotation = this.transform.rotation;
        this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            max = renderer.bounds.max;
            if (max.x > m_bb_max.x)
                m_bb_max.x = max.x;
            if (max.y > m_bb_max.y)
                m_bb_max.y = max.y;
            if (max.z > m_bb_max.z)
                m_bb_max.z = max.z;

            min = renderer.bounds.min;
            if (min.x < m_bb_min.x)
                m_bb_min.x = min.x;
            if (min.y < m_bb_min.y)
                m_bb_min.y = min.y;
            if (min.z < m_bb_min.z)
                m_bb_min.z = min.z;
        }

        Vector3 center = Vector3.Lerp(m_bb_min, m_bb_max, 0.5f)- this.transform.position;
        Vector3 test = m_bb_min + (m_bb_max- m_bb_min) * 0.5f;

        Vector3 size = new Vector3(m_bb_max.x - m_bb_min.x, m_bb_max.y - m_bb_min.y, m_bb_max.z - m_bb_min.z);

        BoxCollider colBox = GetComponent<BoxCollider>();

        if (colBox == null)
        {
            colBox = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
            colBox.isTrigger = true;
        }

        this.transform.rotation = currentRotation;

        colBox.center = center;
        colBox.size = size;

        Debug.Log("AHA");
    }
}
