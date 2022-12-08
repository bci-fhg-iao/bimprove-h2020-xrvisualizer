using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// name of AssetBundle-file has to be equal to the original prefab, used to generate the asset-bundle !!!

public class loadAssetBundle : MonoBehaviour
{
    // // use path "streaming-assets"
    public string subFolderName = "AssetBundles";
    string _folderAssetBundles;
    public List<GameObject> createdObjects = new List<GameObject>();
    public List<string> createdObjNames = new List<string>();

    // external path for assetbundles
    //public string _folderAssetBundles = "C:\\__assetBundles";

    // Start is called before the first frame update
    void Start()
    {
        _folderAssetBundles = Path.Combine(Application.streamingAssetsPath, subFolderName);
        //loadAssetBundleFolder(_folderAssetBundles);

        //loadBundledPrefab("demo_01", "demo_01");
        //loadSingleAssetBundle("demo_01");
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        loadAssetBundleFolder(_folderAssetBundles);
    //    }
    //}

    // load from GuiLoader
    public bool loadAssetBundleFolderFromGuiloader()
    {
        _folderAssetBundles = Path.Combine(Application.streamingAssetsPath, subFolderName);
        string path = _folderAssetBundles;
        bool success = true;
        createdObjects = new List<GameObject>();
        createdObjNames = new List<string>();
        GameObject _loadedGameObject;

        // filenames including full path:
        //string[] bundleFiles = Directory.GetFiles(path, "*.manifest", SearchOption.AllDirectories);

        //filenames only, requires .NET 3.5 for LINQ
        string[] bundleFiles = Directory.GetFiles(path, "*.manifest").Select(Path.GetFileName).ToArray();

        for (int i = 0; i < bundleFiles.Length; i++)
        {
            string file = bundleFiles[i].Replace(".manifest", "");  //remove file-ending .manifest

            AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(path, file)); //file ->  name of asset-bundle
            if (myLoadedAssetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                success = false;
            }

            GameObject prefab = myLoadedAssetBundle.LoadAsset<GameObject>(file);  //file ->  name of prefab (name of prefab = name of asset-bundle)
            _loadedGameObject= Instantiate(prefab);
            _loadedGameObject.name = _loadedGameObject.name.Replace("(Clone)", "");
            createdObjects.Add(_loadedGameObject);
            createdObjNames.Add(_loadedGameObject.name);

            myLoadedAssetBundle.Unload(false);
        }

        if (success)
        {
            Debug.Log("Loading BundleAssets: Finished!");
        }
        else
        {
            Debug.Log("Loading BundleAssets: Finished with errors!");
        }
        return success;
    }

    bool loadAssetBundleFolder(string path)
    {
        bool success = true;

        // filenames including full path:
        //string[] bundleFiles = Directory.GetFiles(path, "*.manifest", SearchOption.AllDirectories);

        //filenames only, requires .NET 3.5 for LINQ
        string[] bundleFiles = Directory.GetFiles(path, "*.manifest").Select(Path.GetFileName).ToArray();

        for (int i = 0; i < bundleFiles.Length; i++)
        {
            string file = bundleFiles[i].Replace(".manifest", "");  //remove file-ending .manifest

            AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(path, file)); //file ->  name of asset-bundle
            if (myLoadedAssetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                success = false;
            }

            GameObject prefab = myLoadedAssetBundle.LoadAsset<GameObject>(file);  //file ->  name of prefab (name of prefab = name of asset-bundle)
            GameObject instance = Instantiate(prefab);
            instance.name = instance.name.Replace("(Clone)", "");

            myLoadedAssetBundle.Unload(false);
        }

        if (success)
        {
            Debug.Log("Loading BundleAssets: Finished!");
        }
        else
        {
            Debug.Log("Loading BundleAssets: Finished with errors!");
        }
        return success;
    }


    void loadBundledPrefab(string nameAssetBundle, string namePrefab)
    {
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(_folderAssetBundles, nameAssetBundle)); //name of asset-bundle
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            // return;
        }

        Debug.Log("Loaded");

        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>(namePrefab); // name of prefab != name of asset-bundle
        GameObject instance = Instantiate(prefab);
        instance.name = instance.name.Replace("(Clone)", "");

        myLoadedAssetBundle.Unload(false);
    }


    void loadSingleAssetBundle(string nameAssetBundle)
    {
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(_folderAssetBundles, nameAssetBundle)); //name of asset-bundle
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle: "+ nameAssetBundle);
            // return;
        }

        Debug.Log("Loaded");

        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>(nameAssetBundle); // name of prefab = name of asset-bundle
        GameObject instance = Instantiate(prefab);
        instance.name = instance.name.Replace("(Clone)", "");

        myLoadedAssetBundle.Unload(false);
    }


    ////for dev-testing only:
    //void testLoader()
    //{
    //    // Debug.Log(Path.Combine(Application.streamingAssetsPath, "testbundle"));

    //    var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles", "test1")); //name of asset-bundle: test1
    //    if (myLoadedAssetBundle == null)
    //    {
    //        Debug.Log("Failed to load AssetBundle!");
    //        // return;
    //    }

    //    Debug.Log("Loaded");

    //    var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("test"); // name of prefab: test
    //    Instantiate(prefab);

    //    myLoadedAssetBundle.Unload(false);
    //}

    //void load_demo_01()
    //{
    //    string filename = "demo_01";

    //    var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles", filename)); //name of asset-bundle
    //    if (myLoadedAssetBundle == null)
    //    {
    //        Debug.Log("Failed to load AssetBundle!");
    //        // return;
    //    }

    //    Debug.Log("Loaded");

    //    var prefab = myLoadedAssetBundle.LoadAsset<GameObject>(filename); // name of prefab
    //    Instantiate(prefab);

    //    myLoadedAssetBundle.Unload(false);
    //}
    
}
