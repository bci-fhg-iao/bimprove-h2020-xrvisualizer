using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Linq; // using LINQ to GameObject

[RequireComponent(typeof(nodeManager))]

public class modelManager : MonoBehaviour
{

    public GameObject m_3D_model; //root-obect für alle 3D-Modelle in der Szene

    nodeManager m_node_manager; //verwaltet die einzelnen Knoten/Bauteile: Auswahl, Ein- / Ausblenden, Transparenz
    windowManager m_win_manager; //verwaltet Window-Handles/Fenster von Unity-App und Browser-Client Chrome
    set_viewpoints m_viewpoints;

    //Gebäude mit Kategorien und Komponenten
    Dictionary<string, data_building> m_buildings_new; //Neubau
    Dictionary<string, data_building> m_buildings_old; //Bestand

    GameObject m_terrain; //Gelände aus dem 3D-Mdell
    GameObject m_terrain_scene; //Modell für das Gelände der Szene -> um das Gebäude/Gelände herum
    GameObject m_ground_scene; //Modell für das Erdreich der Szene -> um das Gebäude/Gelände herum
    GameObject m_pointcloud; //Punktwolke

    // Status-Variablen für schrittweise Initialisierung
    bool m_init_OK = false;
    bool m_new_buildings_OK = false;
    bool m_old_buildings_OK = false;


    // Use this for initialization
    void Start()
    {
        m_buildings_new = new Dictionary<string, data_building>();
        m_buildings_old = new Dictionary<string, data_building>();

    }

    public bool isInit()
    {
        return m_init_OK;
    }

    public bool showScene(string scene_id)
    {
        switch (scene_id)
        {
            case "scene_overview":
                showScene_Overview();
                break;
            case "scene_hotel":
                showScene_Hotel(false);
                break;
            case "scene_hotel_transp":
                showScene_Hotel(true);
                break;
            default:
                displayUnityApp();
                return false;
        }
        displayUnityApp();
        return true;
    }

    void displayUnityApp()
    {
        if (m_init_OK)
        {
            m_win_manager.showApp();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_init_OK == false)
        {
            init();
        }
        else
        {
            //Keyboard commands

            //reset
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    resetScene();
            //}


        }
    }

    void init()
    {
        if (m_3D_model != null)
        {
            if (m_new_buildings_OK == false)
            {
                GameObject new_buildings = m_3D_model.Child("Neubau");
                if (new_buildings != null)
                {
                    m_new_buildings_OK = generateBuildingList(new_buildings, m_buildings_new);
                }
            }

            if (m_old_buildings_OK == false)
            {
                GameObject old_buildings = m_3D_model.Child("Bestand");
                if (old_buildings != null)
                {
                    m_old_buildings_OK = generateBuildingList(old_buildings, m_buildings_old);
                }
            }

            if (m_win_manager == null)
            {
                m_win_manager = gameObject.GetComponent<windowManager>();
            }

            m_node_manager = transform.GetComponent<nodeManager>() as nodeManager;
            m_node_manager.m_rootObject = m_3D_model.transform;

            m_pointcloud = m_3D_model.Child("Punktwolke");
            m_terrain = m_3D_model.Child("Gelände");
            m_terrain_scene = m_3D_model.Child("Szene-Gelände");
            m_ground_scene = m_3D_model.Child("Szene-Erdreich");

            if (m_viewpoints == null)
            {
                GameObject cam = GameObject.Find("Main Camera");

                if (cam != null)
                    m_viewpoints = cam.GetComponent<set_viewpoints>();
            }


            if (m_old_buildings_OK && m_new_buildings_OK && m_node_manager != null && m_terrain != null && m_terrain_scene != null && m_pointcloud != null && m_viewpoints != null && m_win_manager != null)
            {
                m_init_OK = true;
                showScene_Overview();
            }
            else
            {
                Debug.Log("ERROR: ModelManager -> init: no buildings");
            }
        }
        else
        {
            Debug.Log("ERROR: ModelManager -> init: no root object for 3D-model");
        }
    }

    void addDetail(ref data_building building_data, GameObject component)
    {
        GameObject detail_obj = component.Child("Details");
        if (detail_obj != null)
        {
            building_data.addDetail_obj(detail_obj.name, detail_obj);
        }
    }

    bool generateBuildingList(GameObject root_node, Dictionary<string, data_building> building_dict)
    {
        if (root_node != null)
        {
            data_building building_data;

            foreach (GameObject building in root_node.Children())
            {
                Debug.Log("Building:" + building.name);
                building_data = new data_building();

                building_data.Building = building;

                //Kategorien des Gebäudes
                foreach (GameObject obj in building.Children())
                {
                    building_data.addCategory_obj(obj.name, obj);
                }

                //Kategorie: Architektur allgemein
                GameObject arch_root = building.Child("Architektur");
                if (arch_root != null)
                {
                    foreach (GameObject obj in arch_root.Children())
                    {
                        building_data.addArchitecture_obj(obj.name, obj);
                        addDetail(ref building_data, obj);

                        Debug.Log("Building:" + building.name + " | Arch-Compo: " + obj.name);
                    }
                }

                //Kategorie: Tragwerk allgemein
                GameObject const_root = building.Child("Tragwerk");
                if (const_root != null)
                {
                    foreach (GameObject obj in const_root.Children())
                    {
                        building_data.addConstruction_obj(obj.name, obj);
                        addDetail(ref building_data, obj);

                        Debug.Log("Building:" + building.name + " | Tragwerk-Compo: " + obj.name);
                    }
                }

                //Kategorie: TGA (Technische Gebäudeausrüstung) allgemein
                GameObject TGA_root = building.Child("TGA");
                if (const_root != null)
                {
                    foreach (GameObject obj in TGA_root.Children())
                    {
                        building_data.addTGA_obj(obj.name, obj);
                        addDetail(ref building_data, obj);

                        Debug.Log("Building:" + building.name + " | TGA-Compo: " + obj.name);
                    }
                }

                //Kategorie: BSE (Baustelleneinrichtung) allgemein
                GameObject BSE_root = building.Child("Baustellen_Einrichtung");
                if (const_root != null)
                {
                    foreach (GameObject obj in BSE_root.Children())
                    {
                        building_data.addBSE_obj(obj.name, obj);
                        addDetail(ref building_data, obj);
                        Debug.Log("Building:" + building.name + " | BSE-Compo: " + obj.name);
                    }
                }

                building_dict.Add(building.name, building_data);
            }
            return true;
        }
        else
        {
            Debug.Log("generateBuildingDictionary no 3D-model found");
            return false;
        }
    }

    public void showBuilding(bool showIt, string building_name, bool isNewBuilding = true, bool mode_complete = true)
    {
        Dictionary<string, data_building> buildings;
        if (isNewBuilding)
        {
            buildings = m_buildings_new;
        }
        else
        {
            buildings = m_buildings_old;
        }

        data_building obj;
        if (buildings.TryGetValue(building_name, out obj))
        {
            obj.showBuilding(showIt, mode_complete);
        }
        else
        {
            Debug.Log("Error modelManager->showArch Building not found: " + building_name);
        }
    }

    public void displayBuildingAsWireframe(bool isWireframe, string building_name, bool isNewBuilding = true, bool mode_complete = true)
    {
        Dictionary<string, data_building> buildings;
        if (isNewBuilding)
        {
            buildings = m_buildings_new;
        }
        else
        {
            buildings = m_buildings_old;
        }

        data_building obj;
        if (buildings.TryGetValue(building_name, out obj))
        {
            m_node_manager.displayObjectAsWireframe(isWireframe, obj.Building);

        }
        else
        {
            Debug.Log("Error modelManager->showArch Building not found: " + building_name);
        }
    }

    public void showCategory(bool showIt, string building_name, string category_name, bool isNewBuilding = true, bool mode_complete = true)
    {
        Dictionary<string, data_building> buildings;
        if (isNewBuilding)
        {
            buildings = m_buildings_new;
        }
        else
        {
            buildings = m_buildings_old;
        }

        data_building obj;
        if (buildings.TryGetValue(building_name, out obj))
        {
            obj.showCategory(showIt, category_name, mode_complete);

        }
        else
        {
            Debug.Log("Error modelManager->showArch Building not found: " + building_name);
        }
    }

    public void showComponent(bool showIt, string building_name, comp_category category, string component_name, bool isNewBuilding = true, bool mode_complete = true)
    {
        Dictionary<string, data_building> buildings;
        if (isNewBuilding)
        {
            buildings = m_buildings_new;
        }
        else
        {
            buildings = m_buildings_old;
        }

        data_building building;
        if (buildings.TryGetValue(building_name, out building))
        {
            building.showComponent(showIt, category, component_name, mode_complete);
        }
        else
        {
            Debug.Log("Error modelManager->showArch Building not found: " + building_name);
        }
    }

    public void toggleComponent(bool showIt, string building_name, comp_category category, string component_name, bool isNewBuilding = true, bool mode_complete = true)
    {
        Dictionary<string, data_building> buildings;
        if (isNewBuilding)
        {
            buildings = m_buildings_new;
        }
        else
        {
            buildings = m_buildings_old;
        }

        data_building building;
        if (buildings.TryGetValue(building_name, out building))
        {
            building.showComponent(showIt, category, component_name, mode_complete);
        }
        else
        {
            Debug.Log("Error modelManager->showArch Building not found: " + building_name);
        }
    }

    public void displayComponentAsWireframe(bool isWireframe, string building_name, comp_category category, string component_name, bool isNewBuilding = true)
    {
        Dictionary<string, data_building> buildings;
        if (isNewBuilding)
        {
            buildings = m_buildings_new;
        }
        else
        {
            buildings = m_buildings_old;
        }

        data_building building;
        if (buildings.TryGetValue(building_name, out building))
        {
            GameObject obj = building.getComponentObj(category, component_name);
            if (obj != null)
            {
                m_node_manager.displayObjectAsWireframe(isWireframe, obj); ;
            }
        }
        else
        {
            Debug.Log("Error modelManager->showArch Building not found: " + building_name);
        }
    }
    public void displayCategoryAsWireframe(bool isWireframe, string building_name, comp_category category, bool isNewBuilding = true)
    {
        Dictionary<string, data_building> buildings;
        if (isNewBuilding)
        {
            buildings = m_buildings_new;
        }
        else
        {
            buildings = m_buildings_old;
        }

        data_building building;
        if (buildings.TryGetValue(building_name, out building))
        {
            GameObject obj = building.getCategoryObj(category.ToString("g"));
            if (obj != null)
            {
                m_node_manager.displayObjectAsWireframe(isWireframe, obj); ;
            }

        }
        else
        {
            Debug.Log("Error modelManager->showArch Building not found: " + building_name);
        }
    }

    void showAllBuildings(bool showIt, bool isNewBuilding = true)
    {
        Dictionary<string, data_building> buildings;
        if (isNewBuilding)
        {
            buildings = m_buildings_new;
        }
        else
        {
            buildings = m_buildings_old;
        }

        foreach (KeyValuePair<string, data_building> building in buildings)
        {
            if (building.Value != null)
                building.Value.showBuilding(showIt, true);
        }
    }

    void resetScene()
    {
        showAllBuildings(false, false); //hide Bestand
        showAllBuildings(false, true); //hide Neubau

        m_pointcloud.SetActive(false);
        m_terrain.SetActive(false);
        m_terrain_scene.SetActive(false);
        m_ground_scene.SetActive(false);

        m_node_manager.displayObjectAsWireframe(false, m_3D_model); //Wireframe-Modus global aus
    }


    //scenarios

    // Übersicht Bestand und Neubau, ohne TGA und Möbel
    public void showScene_Overview()
    {
        if (m_init_OK)
        {
            resetScene();
            m_terrain.SetActive(true);
            m_terrain_scene.SetActive(true);
            m_ground_scene.SetActive(true);

            showComponent(true, "HV-Verwaltung", comp_category.Architektur, "Fassade", false, true);
            showComponent(true, "HV-Verwaltung", comp_category.Architektur, "Rohbau", false, true);

            showComponent(true, "Hotel", comp_category.Architektur, "Fassade", true, true);
            showComponent(true, "Hotel", comp_category.Architektur, "Rohbau", true, true);

            showComponent(true, "WuM_Welt", comp_category.Architektur, "Fassade", true, true);
            showComponent(true, "WuM_Welt", comp_category.Architektur, "Rohbau", true, true);

            showCategory(true, "Parkhaus", "Architektur", true, true);
            showComponent(true, "Parkhaus", comp_category.Tragwerk, "Stahlbau", true, true);

            m_viewpoints.apply_camPos(0);
        }
    }

    //Parkhaus mit TGA und Stahlbau, Betonkonstruktion transparent/opak
    public void showScene_Parkhaus(bool isTransparent)
    {
        if (m_init_OK)
        {
            //showScene_Overview();

            resetScene();
            m_terrain.SetActive(true);
            m_terrain_scene.SetActive(true);
            m_ground_scene.SetActive(true);

            showBuilding(true, "Parkhaus", true, true);

            if (isTransparent)
            {
                displayComponentAsWireframe(true, "Parkhaus", comp_category.Architektur, "Rohbau", true);
            }

            m_viewpoints.apply_camPos(4);
        }
    }


    //Bestand und Punktwolke, ohne Gelände
    public void showScene_Old_and_Scan()
    {
        if (m_init_OK)
        {
            resetScene();

            m_terrain.SetActive(false);
            m_terrain_scene.SetActive(false);
            m_ground_scene.SetActive(false);

            m_pointcloud.SetActive(true);

            showComponent(true, "HV-Verwaltung", comp_category.Architektur, "Fassade", false, true);
            showComponent(true, "HV-Verwaltung", comp_category.Architektur, "Rohbau", false, true);

            displayComponentAsWireframe(true, "HV-Verwaltung", comp_category.Architektur, "Rohbau", false);
            displayComponentAsWireframe(true, "HV-Verwaltung", comp_category.Architektur, "Fassade", false);

            m_viewpoints.apply_camPos(5);
        }
    }


    // Hotel mit Details, Rest nur Fassade
    public void showScene_Hotel(bool isTransparent)
    {
        if (m_init_OK)
        {
            resetScene();
            m_terrain.SetActive(true);
            m_terrain_scene.SetActive(true);
            m_ground_scene.SetActive(true);

            showBuilding(true, "Hotel", true, true);

            if (isTransparent)
            {
                displayComponentAsWireframe(true, "Hotel", comp_category.Architektur, "Ausbau", true);
                displayComponentAsWireframe(true, "Hotel", comp_category.Architektur, "Fassade", true);
                displayComponentAsWireframe(true, "Hotel", comp_category.Architektur, "Rohbau", true);
            }

            m_viewpoints.apply_camPos(2);
        }
    }

    // WuM-Welt mit Details, Rest nur Fassade
    public void showScene_WuM_Welt(bool isTransparent)
    {
        if (m_init_OK)
        {
            resetScene();
            m_terrain.SetActive(true);
            m_terrain_scene.SetActive(true);
            m_ground_scene.SetActive(true);

            showBuilding(true, "WuM_Welt", true, true);

            if (isTransparent)
            {
                displayComponentAsWireframe(true, "WuM_Welt", comp_category.Architektur, "Ausbau", true);
                displayComponentAsWireframe(true, "WuM_Welt", comp_category.Architektur, "Fassade", true);
                displayComponentAsWireframe(true, "WuM_Welt", comp_category.Architektur, "Rohbau", true);
            }

            m_viewpoints.apply_camPos(5);
        }
    }
}
