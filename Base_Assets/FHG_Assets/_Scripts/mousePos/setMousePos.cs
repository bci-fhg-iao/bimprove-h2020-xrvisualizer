using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Text;


public class setMousePos : MonoBehaviour {
    public int m_X_pos=0;
    public int m_Y_pos = 0;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {
            helper_Win_API.SetCursorPos(m_X_pos, m_Y_pos);            
        }
	}
}
