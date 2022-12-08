using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class set_viewpoints_vive : MonoBehaviour
{
    List<Vector3> m_pos;
    List<Vector3> m_ori;


    // Use this for initialization
    void Start()
    {
        m_pos = new List<Vector3>();
        m_ori = new List<Vector3>();

        m_pos.Add(new Vector3(44.5f, 37.30f, -94.0f)); //Vogelperspektive n
        m_ori.Add(new Vector3(0.0f, -31.0f, 0.0f));
		
		m_pos.Add(new Vector3(-117.0f, 35.70f, 127.0f)); //Vogelperspektive s
        m_ori.Add(new Vector3(0.0f, 143.40f, 0.0f));
		
		m_pos.Add(new Vector3(-31.7f, 0.0f, -61.0f)); //Front
        m_ori.Add(new Vector3(0.0f, 4.5f, 0.0f));		
		
		 m_pos.Add(new Vector3(-51.0f, 0.0f, -5.5f)); //Haupteingang
        m_ori.Add(new Vector3(0.0f, 60.0f, 0.0f));
				
		m_pos.Add(new Vector3(-38.5f, 0.0f, 2.5f)); //Foyer
        m_ori.Add(new Vector3(0.0f, -15.5f, 0.0f));

		
		m_pos.Add(new Vector3(-3.4f, 0.0f, 19.2f)); //Halle
        m_ori.Add(new Vector3(0.0f, -90.0f, 0.0f));
		
		m_pos.Add(new Vector3(-5.2f, 5.0f, 12.7f)); //Halle treppe
        m_ori.Add(new Vector3(0.0f, -69.0f, 0.0f));




        apply_camPos(1);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
						Debug.Log("set_viewpoints::Update --> Input:1");
            apply_camPos(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
					Debug.Log("set_viewpoints::Update --> Input:2");
					apply_camPos(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            apply_camPos(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            apply_camPos(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            apply_camPos(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            apply_camPos(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            apply_camPos(6);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            apply_camPos(7);
        }


    }

    public void apply_camPos(int pos)
    {
        if (pos < m_pos.Count && pos < m_ori.Count)
        {
            transform.position = m_pos[pos];
            transform.rotation = Quaternion.Euler(m_ori[pos]);
        }
    }
}
