using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System;
using System.IO;
using System.Linq;
using TriLib;
//  Verwaltet Komponenten/Teilmodelle von 3D-Modellen:
//  - Verweise und Verwaltung auf Eltern-Knoten sowie opake und transparente Darstellung

public class model_component
{
    private GameObject m_node;
    private GameObject m_node_opaque;
    private GameObject m_node_transparent;

    private bool m_is_transparent=false;
    private bool m_has_transparency_node=false;
    private bool m_transparency_node_created = false;
    private float m_transparency = 0.0f;

    private bool m_use_sync_loader = false; //sync loading true -> create transparent copies on first toggleTransparency | false ->  create transparent copies in constructor

    public model_component (string component_name, GameObject[] model_objects, GameObject parent, float transparency=0.0f, bool use_transparency=false)
    {
        if (model_objects != null || parent != null)
        {
            m_node = new GameObject(component_name);
            m_node.transform.parent = parent.transform;

            m_node_opaque = new GameObject("opaque_" + component_name);
            m_node_opaque.transform.parent = m_node.transform;

            for (int i=0; i<model_objects.Length;i++)
            {
                optimizeMaterials(model_objects[i]); //Glanzeffekt entfernen, Transparenz verdoppeln

                if (!component_name.Contains("rchitektur")) //Schatten nur bei Architektur bzw. architektur
                {                    
                    model_objects[i].setShadows(false, false);
                }
                model_objects[i].transform.parent = m_node_opaque.transform;
            }

            if (use_transparency)
            {
                m_has_transparency_node = true;
                m_transparency = transparency;
                m_is_transparent = false;

                if (m_use_sync_loader == false)
                {
                    m_node_transparent = new GameObject("transparent_" + component_name);
                    m_node_transparent.transform.parent = m_node.transform;

                    GameObject myGameObject;

                    for (int i = 0; i < model_objects.Length; i++)
                    {
                        myGameObject = GameObject.Instantiate(model_objects[i]); //copy of opaque obj
                        myGameObject.transform.parent = m_node_transparent.transform;                        
                        myGameObject.makeTransparent(transparency);
                    }

                    //initial: show opaque only
                    m_node_transparent.SetActive(false);
                }
            }
        }
        else
        {
            Debug.Log("ERROR: Construction of model_component failed!");
        }
    }

    public GameObject getNodeObject()
    {
        return m_node;
    }

    public void createTranspNode()
    {
        if (m_has_transparency_node && m_transparency_node_created==false)
        {
            m_node_transparent = GameObject.Instantiate(m_node_opaque);
            m_node_transparent.name = ("transparent_" + m_node.name);
            m_node_transparent.transform.parent = m_node.transform;

            foreach (Transform childTrans in m_node_opaque.GetComponentsInChildren<Transform>(true))
            {
                //makeTransp(childTrans.gameObject, m_transparency);
                childTrans.gameObject.makeTransparent(m_transparency);
            }

            //initial: show opaque only
            m_node_transparent.SetActive(false);
            m_transparency_node_created=true;
        }
    }

    public string getName()
    {
        return m_node.name;
    }

    public bool getTransparentMode()
    {
        return m_has_transparency_node;
    }

    public void setTransparent(bool transp)
    {
        if (m_use_sync_loader && m_transparency_node_created == false)
        {
            createTranspNode();
        }

        if (m_has_transparency_node)
        {
            m_is_transparent = transp;
            m_node_transparent.SetActive(m_is_transparent);
            m_node_opaque.SetActive(!m_is_transparent);
        }
    }

    public void toggleTransparent()
    {
        if (m_use_sync_loader && m_transparency_node_created == false)
        {
            createTranspNode();

            m_is_transparent = !m_is_transparent;
            m_node_transparent.SetActive(m_is_transparent);
            m_node_opaque.SetActive(!m_is_transparent);
        }

        if (m_has_transparency_node)
        {
            m_is_transparent = !m_is_transparent;
            m_node_transparent.SetActive(m_is_transparent);
            m_node_opaque.SetActive(!m_is_transparent);
        }
    }

    public void setActive(bool isActive)
    {
        m_node.SetActive(isActive);
    }

    public void toggleActive()
    {
        m_node.SetActive(!m_node.activeSelf);
    }

    void optimizeMaterials(GameObject obj)
    {
        Renderer myRenderer = null;
        Material myMaterial = null;
        Color myColor = new Color();

        float smoothness_new = 0.0f;

        foreach (Transform childTrans in obj.GetComponentsInChildren<Transform>(true)) //include inactive
        {
            myRenderer = childTrans.GetComponent<Renderer>();

            if (myRenderer != null) //Wenn Geometrie-Knoten
            {
                int matSize = myRenderer.materials.Length;
                if (matSize > 0)
                {
                    for (int i = 0; i < matSize; i++)
                    {
                        myMaterial = myRenderer.materials[i];
                        myMaterial.shader = Shader.Find("Standard");

                        // Materilaien mit GLas in Transparenz-Modus und Transparenz verdoppeln
                        if (myMaterial.name.Contains("Glas") || myMaterial.name.Contains("glas"))
                        {
                            myMaterial.ToFadeMode();
                            myColor = myMaterial.color;
                            myColor.a = myColor.a / 2.0f;
                            myMaterial.color = myColor;
                        }
                        else
                        {
                            // Glanzeffekte auf Null,da Trilib diese falsch interpretiert
                            if (myMaterial.shader != null)
                            {
                                myMaterial.SetFloat("_Glossiness", smoothness_new);
                            }
                        }
                    }
                }
            }
        }
    }
}
