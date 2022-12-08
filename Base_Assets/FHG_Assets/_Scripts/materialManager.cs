using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// verwaltet Materialien, die den Architekturmodellen dynamisch hinzugefügt werden. 
// Autodesk-Materialien werden beim Import von FBX-Dateien aus Revit von Unity NICHT erkannt -> ungültige Materialdefinitionen 
public class materialManager : MonoBehaviour
{
    //public Transform rootObject;
    bool initOK = false;

    Dictionary<string, Material> m_texture_materials;     //realistische Materialien mit Texturen, die nach Bauteilnamen zugeordnet werden
    Dictionary<string, Material> m_simple_materials; // einfache Farb-Materialien, die nach Bauteilnamen zugeordnet werden
    Dictionary<string, Material> m_category_materials; // einfache Farb-Materialien, die nach Gewerken/Komponenten wie "Architektur" oder "Elektro" zugeordnet werden, Ausnahme: Glas wird transparent dargestellt

    // Use this for initialization
    void Start()
    {
        createTextureMaterials();
        createSimpleMaterials(); 
        createCategoryMaterials();
    }


    // Update is called once per frame
    void Update()
    {

    }

    // einfache Farb-Materialien, die nach Gewerken/Komponenten wie "Architektur" oder "Elektro" zugeordnet werden, Ausnahme: Glas wird transparent dargestellt
    void createCategoryMaterials()
    {
        m_category_materials = new Dictionary<string, Material>();
        m_category_materials.Add("Architektur", (Material)Resources.Load("Arch_Standardfarben/Kat_Arch", typeof(Material)));
        m_category_materials.Add("Elektro", (Material)Resources.Load("Arch_Standardfarben/Kat_Elektro", typeof(Material)));
        m_category_materials.Add("Gelaende", (Material)Resources.Load("Arch_Standardfarben/Kat_Gelaende", typeof(Material)));
        m_category_materials.Add("HLSK", (Material)Resources.Load("Arch_Standardfarben/Kat_HLSK", typeof(Material)));
        m_category_materials.Add("Konstruktion", (Material)Resources.Load("Arch_Standardfarben/Kat_Konstruktion", typeof(Material)));
        m_category_materials.Add("Kueche", (Material)Resources.Load("Arch_Standardfarben/Kat_Kueche", typeof(Material)));
        m_category_materials.Add("Moebel", (Material)Resources.Load("Arch_Standardfarben/Kat_Moebel", typeof(Material)));
        m_category_materials.Add("Stahlbau", (Material)Resources.Load("Arch_Standardfarben/Kat_Stahlbau", typeof(Material)));
    }

    //realistische Materialien mit Texturen, die nach Bauteilnamen zugeordnet werden
    void createTextureMaterials()
    {
        m_texture_materials = new Dictionary<string, Material>();

        m_texture_materials.Add("Glas", (Material)Resources.Load("Arch_Materials/Glas", typeof(Material)));
        m_texture_materials.Add("Betonstütze", (Material)Resources.Load("Arch_Materials/Beton_fugenlos_01", typeof(Material)));
        m_texture_materials.Add("STB-Stütze", (Material)Resources.Load("Arch_Materials/Beton_fugenlos_01", typeof(Material)));
        m_texture_materials.Add("Betonunterzug", (Material)Resources.Load("Arch_Materials/Fertigteil_Beton_01", typeof(Material)));
        m_texture_materials.Add("Betonkonsole", (Material)Resources.Load("Arch_Materials/Fertigteil_Beton_01", typeof(Material)));
        m_texture_materials.Add("Betonwand", (Material)Resources.Load("Arch_Materials/Schalbeton_01", typeof(Material)));
        m_texture_materials.Add("Betonbodenplatte", (Material)Resources.Load("Arch_Materials/Beton_fugenlos_01", typeof(Material)));
        m_texture_materials.Add("STB-Bodenplatte", (Material)Resources.Load("Arch_Materials/Beton_fugenlos_01", typeof(Material)));
        m_texture_materials.Add("Betondecke", (Material)Resources.Load("Arch_Materials/Beton_fugenlos_01", typeof(Material)));
        m_texture_materials.Add("STB-Decke", (Material)Resources.Load("Arch_Materials/Beton_fugenlos_01", typeof(Material)));
        m_texture_materials.Add("Treppenlauf", (Material)Resources.Load("Arch_Materials/Beton_fugenlos_01", typeof(Material)));

        m_texture_materials.Add("HE-A", (Material)Resources.Load("Arch_Materials/Metall_matt", typeof(Material)));
        m_texture_materials.Add("HE-B", (Material)Resources.Load("Arch_Materials/Metall_matt", typeof(Material)));
        m_texture_materials.Add("QRO-Stütze", (Material)Resources.Load("Arch_Materials/Metall_matt", typeof(Material))); //Hohlprofil DIN EN 10219-2
        m_texture_materials.Add("Flachstahl", (Material)Resources.Load("Arch_Materials/Metall_matt", typeof(Material)));
        m_texture_materials.Add("U-Profil", (Material)Resources.Load("Arch_Materials/Metall_matt", typeof(Material)));


        m_texture_materials.Add("Lettenkeuper", (Material)Resources.Load("Arch_Materials/Boden-braun", typeof(Material)));
    }

    // einfache Farb-Materialien, die nach Bauteilnamen zugeordnet werden
    void createSimpleMaterials()
    {
        m_simple_materials = new Dictionary<string, Material>();

        m_simple_materials.Add("Glas", (Material)Resources.Load("Arch_Materials/Glas", typeof(Material)));
        m_simple_materials.Add("Betonstütze", (Material)Resources.Load("Arch_Materials/RGB_Beton", typeof(Material)));
        m_simple_materials.Add("STB-Stütze", (Material)Resources.Load("Arch_Materials/RGB_Beton", typeof(Material)));
        m_simple_materials.Add("Betonunterzug", (Material)Resources.Load("Arch_Materials/RGB_Beton", typeof(Material)));
        m_simple_materials.Add("Betonkonsole", (Material)Resources.Load("Arch_Materials/RGB_Beton", typeof(Material)));
        m_simple_materials.Add("Betonwand", (Material)Resources.Load("Arch_Materials/RGB_Beton", typeof(Material)));
        m_simple_materials.Add("Betonbodenplatte", (Material)Resources.Load("Arch_Materials/RGB_Beton", typeof(Material)));
        m_simple_materials.Add("STB-Bodenplatte", (Material)Resources.Load("Arch_Materials/RGB_Beton", typeof(Material)));
        m_simple_materials.Add("Betondecke", (Material)Resources.Load("Arch_Materials/RGB_Beton", typeof(Material)));
        m_simple_materials.Add("STB-Decke", (Material)Resources.Load("Arch_Materials/RGB_Beton", typeof(Material)));
        m_simple_materials.Add("Treppenlauf", (Material)Resources.Load("Arch_Materials/RGB_Beton", typeof(Material)));

        m_simple_materials.Add("HE-A", (Material)Resources.Load("Arch_Materials/RGB_Metall", typeof(Material)));
        m_simple_materials.Add("HE-B", (Material)Resources.Load("Arch_Materials/RGB_Metall", typeof(Material)));
        m_simple_materials.Add("QRO-Stütze", (Material)Resources.Load("Arch_Materials/RGB_Metall", typeof(Material))); //Hohlprofil DIN EN 10219-2
        m_simple_materials.Add("Flachstahl", (Material)Resources.Load("Arch_Materials/RGB_Metall", typeof(Material)));
        m_simple_materials.Add("U-Profil", (Material)Resources.Load("Arch_Materials/RGB_Metall", typeof(Material)));


        m_simple_materials.Add("Lettenkeuper", (Material)Resources.Load("Arch_Materials/Boden-braun", typeof(Material)));
    }



    public Material getMaterial(string bauteilname, materialVariant matvar, string component = "")
    {
        Material myMat = (Material)Resources.Load("Arch_Materials/BIM_Standard_Material", typeof(Material));
        bool materialFound = false;

        if (matvar == materialVariant.textured)
        {
            foreach (var item in m_texture_materials)
            {
                if (bauteilname.Contains(item.Key))
                {
                    myMat = item.Value;
                    materialFound = true;
                    break;
                }
            }
        }
        else
        {
            return getCategoryMaterial(bauteilname, component);
            //if (component.Length == 0)
            //{
            //    foreach (var item in m_simple_materials)
            //    {
            //        if (bauteilname.Contains(item.Key))
            //        {
            //            myMat = item.Value;
            //            materialFound = true;
            //            break;
            //        }
            //    }
            //}
            //else
            //{
            //    return getCategoryMaterial(bauteilname, component);
            //}
        }

        if (materialFound == false)
        {
            //Debug.Log("BIM_Material: Kein Material gefunden für Bauteil: " + bauteilname);
        }

        return myMat;
    }

    Material getCategoryMaterial(string bauteilname, string component)
    {
        bool materialFound = false;
        Material myMat = (Material)Resources.Load("Arch_Materials/BIM_Standard_Material", typeof(Material));

        // Verglasung transparent, ansonsten Farbe der Kategorie 
        if (bauteilname.Contains("Glas") || bauteilname.Contains("glas") || bauteilname.Contains("Fensterelement")){
            myMat = (Material)Resources.Load("Arch_Materials/Glas", typeof(Material));
            materialFound = true;
            //Debug.Log("getCategoryMaterial: Glas: " + bauteilname + " in Komponente " + component);
        }
        else {
            foreach (var item in m_category_materials)
            {
                if (component.Contains(item.Key))
                {
                    myMat = item.Value;
                    materialFound = true;
                    break;
                }
            }
        }

        if (materialFound == false)
        {
            Debug.Log("getCategoryMaterial: Kein Material gefunden für Bauteil: " + bauteilname +" in Komponente "+component);
        }

        return myMat;
    }

}
