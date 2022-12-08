using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum transpMaterialEffect
{
    wireFrame = 1, transp_Mat = 2, outlineShaderMat = 3
}

public enum materialVariant
{
    textured = 0, simple = 1, real=2
}


public class multiMaterialNode : MonoBehaviour
{
    Renderer m_Renderer;

    Material[] m_geo_mat_1; //  material variant 1 of geometry: textured
    Material[] m_geo_mat_2; // material variant 2 of geometry: simple

    Material[] m_transparent_mat_list; //material effect for seethrough: normal
    Material[] m_transparent_mat_list_selected; //material effect for seethrough: selected
    Material[] m_transparent_mat_list_marked; //material effect for seethrough: marked

    Material[] m_mat_list_selected; //material selected 
    Material[] m_mat_list_marked; //material marked

    bool m_init = false;
    bool m_isTransparent = false;
    bool m_isSelected = false;
    bool m_isMarked = false;
    bool m_textureModeOn = true;

    string m_transpMaterialName;

    // Use this for initialization
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();

       // GameObject scriptHolder = GameObject.Find("_Scripts");
        //BIMmaterialManager myManager=null;

        //if (scriptHolder != null) {
        //    myManager = scriptHolder.GetComponent<BIMmaterialManager>();
        //}

        m_transpMaterialName = transpMaterialEffect.transp_Mat.ToString("g"); //material effect for seethrough

        if (m_Renderer != null)// && myManager != null)
        {
            int matListLenght = m_Renderer.materials.Length;

            m_geo_mat_1 = new Material[matListLenght];
            m_geo_mat_2 = new Material[matListLenght];
            m_transparent_mat_list = new Material[matListLenght];
            m_transparent_mat_list_selected = new Material[matListLenght];
            m_transparent_mat_list_marked = new Material[matListLenght]; 

            m_mat_list_selected = new Material[matListLenght];
            m_mat_list_marked = new Material[matListLenght];


            for (int i = 0; i < matListLenght; i++)
            {
                //m_category_materials.Add("Architektur", (Material)Resources.Load("Arch_Standardfarben/Kat_Arch", typeof(Material)));
                m_transparent_mat_list[i] = (Material)Resources.Load("highlight_materials/" + m_transpMaterialName, typeof(Material));
                m_transparent_mat_list_selected[i] = (Material)Resources.Load("highlight_materials/" + m_transpMaterialName + "_Selected", typeof(Material));
                m_transparent_mat_list_marked[i] = (Material)Resources.Load("highlight_materials/" + m_transpMaterialName + "_Marked", typeof(Material));

                m_mat_list_selected[i] = (Material)Resources.Load("highlight_materials/" + "selectionMat_01", typeof(Material));
                m_mat_list_marked[i] = (Material)Resources.Load("highlight_materials/" + "markerMat_01", typeof(Material));

                //durch nodeManager gesetzt:
                //m_geo_mat_1[i] = myManager.getMaterial(GetComponent<Transform>().parent.name, true);
                //m_geo_mat_2[i] = myManager.getMaterial(GetComponent<Transform>().parent.name, false);
            }
        }
        else
        {
            Debug.Log("ERROR: materialNode no renderer found");
        }
    }

    public void setMaterial(materialVariant textureMode, Material material)
    {
        if (m_geo_mat_1 != null && m_geo_mat_2 != null)
        {
            Material[] matHolder = null;
            if (textureMode == materialVariant.textured)
            {
                matHolder = m_geo_mat_1;
            }
            else if (textureMode == materialVariant.simple)
            {
                matHolder = m_geo_mat_2;
            }

            for (int i = 0; i < matHolder.Length; i++)
            {
                matHolder[i] = material;
            }

            m_init = true;
        }
    }

    public void setOriginalMaterial()
    {
        if (m_geo_mat_1 != null && m_geo_mat_2 != null)
        {
            m_geo_mat_1 = m_Renderer.materials;
            m_geo_mat_2 = m_Renderer.materials;
            m_init = true;
        }
    }

        // Update is called once per frame
        void Update()
    {
        //if (!m_init)
        //{
        //    if (m_geo_mat_1[0]!=null && m_geo_mat_2[0] && m_wireframe_mat_list[0] != null && m_wireframe_selected_mat_list[0] != null && m_selection_mat_list[0] != null)
        //    {
        //        m_init = true;
        //        applyMaterials();
        //    }
        //}
    }

    public bool setWireframeMode(bool isWireframe)
    {
        if (m_init)
        {
            m_isTransparent = isWireframe;
            applyMaterials();
            return true;
        }
        else
            return false;
    }

    public bool setTextureMode(bool isTextured)
    {
        if (m_init)
        {
            m_textureModeOn = isTextured;
            applyMaterials();
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
            applyMaterials();
            return true;
        }
        else
            return false;
    }

    public bool setMarked(bool isMarked)
    {
        if (m_init)
        {
            m_isMarked = isMarked;
            applyMaterials();
            return true;
        }
        else
            return false;
    }

    //Fallbehandlung: Materialmodus, Transparenz und BCF-Markierung
    void applyMaterials()
    {


        //if (m_isTransparent)
        //{
        //    if (m_isMarked)
        //        m_Renderer.materials = m_transparent_mat_list_marked;
        //    else if(m_isSelected)
        //        m_Renderer.materials = m_transparent_mat_list_selected;
        //    else
        //    m_Renderer.materials = m_transparent_mat_list;
        //}
        //else
        //{
        //    if (m_isMarked)
        //        m_Renderer.materials = m_mat_list_marked;
        //    else if (m_isSelected)
        //        m_Renderer.materials = m_mat_list_selected;
        //    else
        //    {
        //        if (m_textureModeOn)
        //            m_Renderer.materials = m_geo_mat_1;
        //        else
        //            m_Renderer.materials = m_geo_mat_2;
        //    }

        //}

        if (m_isSelected)
        {
            if (m_isTransparent)
                m_Renderer.materials = m_transparent_mat_list_selected;
            else
                m_Renderer.materials = m_mat_list_selected;
        }
        else
        {
            if (m_isTransparent)
            {
                if (m_isMarked)
                    m_Renderer.materials = m_transparent_mat_list_marked;
                else
                    m_Renderer.materials = m_transparent_mat_list;
            }
            else
            {
                if (m_isMarked)
                {
                    m_Renderer.materials = m_mat_list_marked;
                }
                else
                {
                    if (m_textureModeOn)
                        m_Renderer.materials = m_geo_mat_1;
                    else
                        m_Renderer.materials = m_geo_mat_2;
                }
            }
        }
    }
}
