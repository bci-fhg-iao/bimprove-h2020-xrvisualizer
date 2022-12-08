using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materialNode : MonoBehaviour
{

    Renderer m_Renderer;

    Material[] m_org_material_list; // material of geometry
    Material[] m_wireframe_mat_list; //material of wireframeShader
    Material[] m_wireframe_selected_mat_list; //material of wireframeShader
    Material[] m_selection_mat_list; //material of wireframeShader

    bool m_init = false;
    bool m_isWireframe = false;
    bool m_isSelected = false;

    string m_materialName;

    // Use this for initialization
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();

        m_materialName = "transp_Mat";

        if (m_Renderer != null)
        {
            m_org_material_list = m_Renderer.materials;
            m_wireframe_mat_list = new Material[m_org_material_list.Length];
            m_wireframe_selected_mat_list = new Material[m_org_material_list.Length];
            m_selection_mat_list = new Material[m_org_material_list.Length];

            for (int i = 0; i < m_wireframe_mat_list.Length; i++)
            {
                m_wireframe_mat_list[i] = (Material)Resources.Load(m_materialName, typeof(Material));
            }

            for (int i = 0; i < m_wireframe_selected_mat_list.Length; i++)
            {
                //m_wireframe_selected_mat_list[i] = (Material)Resources.Load("wireFrame_Selected", typeof(Material));
                //m_wireframe_selected_mat_list[i] = (Material)Resources.Load("outlineShaderMatSelected", typeof(Material));
                m_wireframe_selected_mat_list[i] = (Material)Resources.Load(m_materialName + "_Selected", typeof(Material));
            }

            for (int i = 0; i < m_selection_mat_list.Length; i++)
            {
                m_selection_mat_list[i] = (Material)Resources.Load("selectionMat_01", typeof(Material));
            }



            if (m_org_material_list != null && m_wireframe_mat_list != null && m_wireframe_selected_mat_list != null && m_selection_mat_list != null)
                m_init = true;
            else
                Debug.Log("ERROR: materialNode -> init false!");
        }
        else
        {
            Debug.Log("ERROR: materialNode no renderer found");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool setWireframeMode(bool isWireframe)
    {
        if (m_init)
        {
            m_isWireframe = isWireframe;

            if (isWireframe)
            {
                if (m_isSelected)
                    m_Renderer.materials = m_wireframe_selected_mat_list;
                else
                    m_Renderer.materials = m_wireframe_mat_list;
            }
            else
            {
                if (m_isSelected)
                    m_Renderer.materials = m_selection_mat_list;
                else               
                m_Renderer.materials = m_org_material_list;
            }
            return true;
        }
        else
            return false;
    }


    public bool setSelected(bool isSelected)
    {
        if (m_init)
        {
            m_isSelected = isSelected;

            if (m_isSelected)
            {
                if (m_isWireframe)
                    m_Renderer.materials = m_wireframe_selected_mat_list;
                else
                    m_Renderer.materials = m_selection_mat_list;
            }
            else
            {
                if (m_isWireframe)
                    m_Renderer.materials = m_wireframe_mat_list;
                else
                    m_Renderer.materials = m_org_material_list;
            }
            return true;
        }
        else
            return false;
    }
}
