using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Linq; // using LINQ to GameObject

public enum comp_category { Architektur = 0, Tragwerk = 1, TGA = 2, Baustellen_Einrichtung = 3, Details = 4 };

public class data_building
{
    //Dictionary<comp_category, GameObject> m_enum_categories; //arch, constr, TGA...
    GameObject m_building;

    Dictionary<string, GameObject> m_categories; //arch, constr, TGA...
    Dictionary<string, GameObject> m_architecture;
    Dictionary<string, GameObject> m_TGA; //technische Gebäudeausrüstung/HLSK
    Dictionary<string, GameObject> m_construction;
    Dictionary<string, GameObject> m_BSE; //Baustelleneinrichtung BSE
    Dictionary<string, GameObject> m_detail; //alle zusätzlichen Details: z.B. Schrauben für Stahlbau

    public data_building()
    {
        m_categories = new Dictionary<string, GameObject>();
        m_architecture = new Dictionary<string, GameObject>();
        m_TGA = new Dictionary<string, GameObject>();
        m_construction = new Dictionary<string, GameObject>();
        m_BSE = new Dictionary<string, GameObject>();
        m_detail = new Dictionary<string, GameObject>();
    }

    public bool showBuilding(bool showIt, bool mode_complete = true)
    {
        if (m_building != null)
        {
            m_building.SetActive(showIt);
            if (mode_complete)
            {
                showCategory(showIt, "Architektur", true);
                showCategory(showIt, "Tragwerk", true);
                showCategory(showIt, "TGA", true);
                showCategory(showIt, "Baustellen_Einrichtung", true);
            }
            return true;
        }
        else
        {
            Debug.Log("ERROR: databuilding->showBuilding no gameobject");
            return false;
        }
    }

    public GameObject Building
    {
        get
        {
            return m_building;
        }
        set
        {
            if (value != null)
            {
                m_building = value;
            }
        }
    }

    public GameObject getCategoryObj(string category_name)
    {
        GameObject obj;
        if (m_categories.TryGetValue(category_name, out obj))
        {
            return obj;
        }
        else
        {
            Debug.Log("ERROR [data_building->getCategoryObj] not found: " + m_building.name + " -> " + category_name);
            return null;
        }
    }



    public GameObject getComponentObj(comp_category category, string component_name)
    {
        Dictionary<string, GameObject> category_obj;

        switch (category)
        {
            case comp_category.Architektur:
                category_obj = m_architecture;
                break;
            case comp_category.Tragwerk:
                category_obj = m_construction;
                break;
            case comp_category.TGA:
                category_obj = m_TGA;
                break;
            case comp_category.Baustellen_Einrichtung:
                category_obj = m_BSE;
                break;
            case comp_category.Details:
                category_obj = m_detail;
                break;
            default:
                category_obj = null;
                break;
        }

        if (category_obj != null)
        {
            GameObject obj;
            if (category_obj.TryGetValue(component_name, out obj))
            {
                if (obj != null)
                {
                    return obj;
                }
                else
                {
                    Debug.Log("ERROR: [data_building->getComponentObj] component not found:" + m_building.name + "->" + category.ToString("g") + "->" + component_name);
                    return null;
                }
            }
            else
            {
                Debug.Log("ERROR: [data_building->getComponentObj] category not found:" + m_building.name + "->" + category.ToString("g") + "->" + component_name);
                return null;
            }
        }
        else
        {
            Debug.Log("ERROR: [data_building->getComponentObj] category not found:" + m_building.name + "->" + category.ToString("g") + "->" + component_name);
            return null;
        }
    }

    public bool addCategory_obj(string name, GameObject obj)
    {
        if (name.Length > 0 && obj != null)
        {
            m_categories.Add(name, obj);
            return true;
        }
        else
        {
            Debug.Log("ERROR: data_building->addCategory_obj: " + name);
            return false;
        }
    }

    public bool addArchitecture_obj(string name, GameObject obj)
    {
        if (name.Length > 0 && obj != null)
        {
            m_architecture.Add(name, obj);
            return true;
        }
        else
        {
            Debug.Log("ERROR: data_building->addArchitecture_obj: " + name);
            return false;
        }
    }

    public bool addTGA_obj(string name, GameObject obj)
    {
        if (name.Length > 0 && obj != null)
        {
            m_TGA.Add(name, obj);
            return true;
        }
        else
        {
            Debug.Log("ERROR: data_building->addTGA_obj: " + name);
            return false;
        }
    }

    public bool addConstruction_obj(string name, GameObject obj)
    {
        if (name.Length > 0 && obj != null)
        {
            m_construction.Add(name, obj);
            return true;
        }
        else
        {
            Debug.Log("ERROR: data_building->addConstruction_obj: " + name);
            return false;
        }
    }

    public bool addBSE_obj(string name, GameObject obj)
    {
        if (name.Length > 0 && obj != null)
        {
            m_BSE.Add(name, obj);
            return true;
        }
        else
        {
            Debug.Log("ERROR: data_building->addBSE_obj: " + name);
            return false;
        }
    }

    public bool addDetail_obj(string name, GameObject obj)
    {
        if (name.Length > 0 && obj != null)
        {
            m_detail.Add(name, obj);
            return true;
        }
        else
        {
            Debug.Log("ERROR: data_building->addDetail_obj: " + name);
            return false;
        }
    }

    public bool showCategory(bool showIt, string category_name, bool mode_complete = true)
    {
        GameObject obj;
        if (m_categories.TryGetValue(category_name, out obj))
        {
            obj.SetActive(showIt);

            if (mode_complete) //true: all children, false: root-object only
            {
                foreach (GameObject comp_obj in obj.Children())
                {
                    comp_obj.SetActive(showIt);

                    //detail level:
                    GameObject details_obj = comp_obj.Child("LOD_Details");
                    if (details_obj != null)
                        details_obj.SetActive(false);
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }


    public bool showComponent(bool showIt, comp_category category, string compo_name, bool mode_complete = true)
    {
        Dictionary<string, GameObject> category_obj;

        switch (category)
        {
            case comp_category.Architektur:
                category_obj = m_architecture;
                break;
            case comp_category.Tragwerk:
                category_obj = m_construction;
                break;
            case comp_category.TGA:
                category_obj = m_TGA;
                break;
            case comp_category.Baustellen_Einrichtung:
                category_obj = m_BSE;
                break;
            case comp_category.Details:
                category_obj = m_detail;
                break;
            default:
                category_obj = null;
                break;
        }

        if (category_obj != null)
        {
            GameObject obj;
            if (category_obj.TryGetValue(compo_name, out obj))
            {
                obj.SetActive(showIt);

                // Kategorie und Gebäude auf Active setzen
                if (showIt)
                {
                    GameObject cat_obj = obj.Parent();

                    if (cat_obj != null)
                    {
                        cat_obj.SetActive(true);
                        m_building.SetActive(true);
                    }
                }

                if (mode_complete)
                {
                    //detail level:
                    GameObject details_obj = obj.Child("LOD_Details");
                    if (details_obj != null)
                        details_obj.SetActive(showIt);
                }
                return true;
            }
            else
            {
                Debug.Log("ERROR: data_building->showComponent Component not found: " + compo_name + "  Building: "+m_building.name);
                return false;
            }
        }
        else
        {
            Debug.Log("ERROR: data_building->showComponent Category not found");
            return false;
        }
    }
}
