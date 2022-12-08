using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System;
using System.IO;
using System.Linq;
using TriLib;

public class trilib_folder_loader : MonoBehaviour
{
    public GameObject m_3D_model;
    public string m_modelpath;
    public float m_transparency = 0.25f;
    public bool m_use_folder_code = true;

    private bool m_init = false;
    private List<model_component> m_loaded_components;

    // Start is called before the first frame update
    void Start()
    {
      //  StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(10);
    }

    //// Update is called once per frame
    void Update()
    {
        //if (!m_init && Input.GetKeyDown(KeyCode.Space))
        if (!m_init)
        {
            m_init = init();
        }
    }
    bool init()
    {
        m_loaded_components = new List<model_component>();
        if (m_use_folder_code)
        {
            loadFolderNaming(m_modelpath, m_3D_model); // use name format for folder names: 001_T_name
        }
        else
        {
            loadFolder(m_modelpath, m_3D_model);
        }

        // add simple manager for keyboard interaction
        trilib_model_manager manager = gameObject.AddComponent(typeof(trilib_model_manager)) as trilib_model_manager;
        manager.setComponents(m_loaded_components);

        return true;
    }


    // folder names don't matter, no code naming convention: foldername -> GameObject name
    void loadFolder(string model_path, GameObject target_obj)
    {
        string[] directories = Directory.GetDirectories(Path.Combine(Application.streamingAssetsPath, m_modelpath));

        for (int i = 0; i < directories.Length; i++)
        {
            //Debug.Log("Directory: " + directories[i]);
            string folder_name = directories[i].Substring(directories[i].LastIndexOf("\\") + 1); //name of subfolder -> component name
            m_loaded_components.Add(new model_component(folder_name, loadComponent(directories[i]), target_obj, m_transparency, false));
        }
        Debug.Log("Directory loaded ");
    }


    // folder name: 001_T_objName 
    void loadFolderNaming(string model_path, GameObject target_obj)
    {
        string[] directories = Directory.GetDirectories(Path.Combine(Application.streamingAssetsPath, m_modelpath));

        for (int i = 0; i < directories.Length; i++)
        {
            //Debug.Log("Directory: " + directories[i]);
            string folder_name = directories[i].Substring(directories[i].LastIndexOf("\\") + 1); //name of subfolder -> component name
            bool use_transparency = false;

            //check transparency
            use_transparency = folder_name.Contains("_T_");

            if (use_transparency)
            {
                folder_name = folder_name.Substring(folder_name.LastIndexOf("_T_") + 3);
            }
            else
            {
                folder_name = folder_name.Substring(folder_name.IndexOf("_") + 1);
            }

            //sync load
           m_loaded_components.Add(new model_component(folder_name, loadComponent(directories[i]), target_obj, m_transparency, use_transparency));

            //async load
           //  m_loaded_components.Add(new model_component(folder_name, loadComponentAsync(directories[i]), target_obj, m_transparency, use_transparency));
        }
        Debug.Log("Directory loaded ");
    }




    GameObject[] loadComponent(string model_path)
    {
        string[] myFiles;

        GameObject myGameObject;
        string filter = AssetLoaderBase.GetSupportedFileExtensions();
        myFiles = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, model_path)).Where(x => filter.Contains("*" + FileUtils.GetFileExtension(x) + ";")).ToArray();

        GameObject[] loaded_models = new GameObject[myFiles.Length];

        for (int i = 0; i < myFiles.Length; i++)
        {
            var file = myFiles[i];

            Debug.Log("Load file: " + file);

            AssetLoaderOptions assetLoaderOptions = AssetLoaderOptions.CreateInstance();
            //assetLoaderOptions.RotationAngles = new Vector3(90f, 180f, 0f);
            assetLoaderOptions.AutoPlayAnimations = false;
            assetLoaderOptions.UseOriginalPositionRotationAndScale = true;

            AssetLoader assetLoader = new AssetLoader();

            //myGameObject = assetLoader.LoadFromFile(file, assetLoaderOptions, target_obj);
            myGameObject = assetLoader.LoadFromFile(file, assetLoaderOptions);
            myGameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
            // myGameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            loaded_models[i] = myGameObject;
        }
        return loaded_models;
    }


    GameObject[] loadComponentAsync(string model_path)
    {
        string[] myFiles;

        string filter = AssetLoaderBase.GetSupportedFileExtensions();
        myFiles = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, model_path)).Where(x => filter.Contains("*" + FileUtils.GetFileExtension(x) + ";")).ToArray();

        GameObject[] loaded_models = new GameObject[myFiles.Length];

        for (int i = 0; i < myFiles.Length; i++)
        {
            var file = myFiles[i];
            GameObject myGameObject = new GameObject();

            Debug.Log("Load file: " + file);

            AssetLoaderOptions assetLoaderOptions = AssetLoaderOptions.CreateInstance();
            //assetLoaderOptions.RotationAngles = new Vector3(90f, 180f, 0f);
            assetLoaderOptions.AutoPlayAnimations = false;
            assetLoaderOptions.UseOriginalPositionRotationAndScale = true;

            //AssetLoader assetLoader = new AssetLoader();
            using (AssetLoaderAsync assetLoader = new AssetLoaderAsync())
            {
                try
                {
                    assetLoader.LoadFromFile(file, assetLoaderOptions, myGameObject, delegate (GameObject loadedGameObject)
                    {
                        loadedGameObject.transform.position = new Vector3(0f, 0f, 0f);
                        Debug.Log("Async loaded index: " + i);
                    });
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }


                loaded_models[i] = myGameObject;
            }
        }

        return loaded_models;
    }


    ////load models in a single folder
    //void loadModels(string model_path, GameObject target_obj)
    //{
    //    string[] myFiles;
    //    GameObject myGameObject;

    //    string filter = AssetLoaderBase.GetSupportedFileExtensions();
    //    myFiles = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, model_path)).Where(x => filter.Contains("*" + FileUtils.GetFileExtension(x) + ";")).ToArray();

    //    for (int i = 0; i < myFiles.Length; i++)
    //    {
    //        var file = myFiles[i];

    //        Debug.Log("Load file: " + file);

    //        AssetLoaderOptions assetLoaderOptions = AssetLoaderOptions.CreateInstance();
    //        //assetLoaderOptions.RotationAngles = new Vector3(90f, 180f, 0f);
    //        assetLoaderOptions.AutoPlayAnimations = false;
    //        assetLoaderOptions.UseOriginalPositionRotationAndScale = true;

    //        AssetLoader assetLoader = new AssetLoader();

    //        myGameObject = assetLoader.LoadFromFile(file, assetLoaderOptions, target_obj);
    //        myGameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
    //        // myGameObject.transform.localScale = new Vector3(1f, 1f, 1f);

    //        if (target_obj != null)
    //            myGameObject.transform.parent = target_obj.transform;
    //        else
    //        {
    //            Debug.Log("ERROR loadModels: target is null" + model_path);
    //        }
    //    }
    //}
}
