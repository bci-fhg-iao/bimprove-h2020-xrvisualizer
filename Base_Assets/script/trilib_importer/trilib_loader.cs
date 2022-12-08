using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System;
using System.IO;
using System.Linq;
using TriLib;

public class trilib_loader : MonoBehaviour
{
    public GameObject m_target_01;
    public string m_modelpath;
    public float m_transparency = 0.25f;
    private GameObject m_model_copy;
    private bool m_arch_vis = false;


    // Start is called before the first frame update

    private string[] m_files;
    void Start()
    {
        loadModels(m_modelpath, m_target_01);
        createCopy();
        optimizeMaterials();
        makeTransp(m_model_copy);
        toggleTransp();


        //m_model_copy.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            toggleTransp();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            optimizeMaterials();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            createCopy();
            makeTransp(m_model_copy);
            toggleTransp();
            Debug.Log("Loaded: " + m_target_01.name);
            Debug.Log("Transp: " + m_model_copy.name);

        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            loadModels(m_modelpath, m_target_01);
        }
    }

    void toggleTransp()
    {
        m_arch_vis = !m_arch_vis;

        if (m_model_copy != null && m_target_01 != null)
        {
            m_model_copy.SetActive(!m_arch_vis);
            m_target_01.SetActive(m_arch_vis);
        }
    }

    void loadModels(string model_path, GameObject target_obj)
    {

        GameObject myGameObject;
        string filter = AssetLoaderBase.GetSupportedFileExtensions();
        m_files = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, m_modelpath)).Where(x => filter.Contains("*" + FileUtils.GetFileExtension(x) + ";")).ToArray();

        for (int i = 0; i < m_files.Length; i++)
        {
            var file = m_files[i];

            AssetLoaderOptions assetLoaderOptions = AssetLoaderOptions.CreateInstance();
            //assetLoaderOptions.RotationAngles = new Vector3(90f, 180f, 0f);
            assetLoaderOptions.AutoPlayAnimations = false;
            assetLoaderOptions.UseOriginalPositionRotationAndScale = true;

            AssetLoader assetLoader = new AssetLoader();

            myGameObject = assetLoader.LoadFromFile(file, assetLoaderOptions, target_obj);
            myGameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
            // myGameObject.transform.localScale = new Vector3(1f, 1f, 1f);

            //geladenes Modell an angegebenes GameObject oder an "diesen" Knoten anfügen
            if (target_obj != null)
                myGameObject.transform.parent = target_obj.transform;
            else
            {
                myGameObject.transform.parent = transform;
                m_target_01 = transform.gameObject;
            }
        }
    }


    void optimizeMaterials()
    {
        if (m_target_01 == null)
        {
            //Debug.Log("ERROR replaceMaterial->changeMaterial] Materials or target NULL");
            m_target_01 = transform.gameObject;
        }

        Renderer myRenderer = null;
        Material myMaterial = null;
        Color myColor = new Color();

        float smoothness_new = 0.0f;

        foreach (Transform childTrans in m_target_01.GetComponentsInChildren<Transform>(true)) //include inactive
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


    void createCopy()
    {
        if (m_target_01 == null)
        {
            m_target_01 = transform.gameObject;
            Debug.Log("ERROR createTranspCopy->createCopy] target is NULL");
        }
        m_model_copy = Instantiate(m_target_01);
        m_model_copy.name = "transp_" + m_target_01.name;
        m_model_copy.transform.parent = m_target_01.transform.parent;

        m_model_copy.transform.localPosition = m_target_01.transform.localPosition;
        m_model_copy.transform.localRotation = m_target_01.transform.localRotation;
        m_model_copy.transform.localScale = m_target_01.transform.localScale;
    }


    void makeTransp(GameObject obj)
    {
        if (obj != null)
        {
            Renderer myRenderer = null;
            Material myMaterial = null;
            Color myColor = new Color();

            foreach (Transform childTrans in obj.GetComponentsInChildren<Transform>(true)) //include inactive
            {
                myRenderer = childTrans.GetComponent<Renderer>();

                if (myRenderer != null) //Wenn Geometrie-Knoten
                {
                    //int matSize = myRenderer.materials.Length;
                    int matSize = myRenderer.sharedMaterials.Length;
                    if (matSize > 0)
                    {
                        Material[] newMaterials = new Material[matSize];
                        for (int i = 0; i < matSize; i++)
                        {
                            //myMaterial = Instantiate(myRenderer.materials[i]);
                            myMaterial = Instantiate(myRenderer.sharedMaterials[i]);
                            myMaterial.name = "transp_" + myMaterial.name;
                            myMaterial.ToFadeMode();

                            myColor = myMaterial.color;
                            myColor.a = m_transparency;
                            myMaterial.color = myColor;

                            newMaterials[i] = myMaterial;
                        }

                        //myRenderer.materials = newMaterials;
                        myRenderer.sharedMaterials = newMaterials;
                    }
                }
            }
        }
    }
}