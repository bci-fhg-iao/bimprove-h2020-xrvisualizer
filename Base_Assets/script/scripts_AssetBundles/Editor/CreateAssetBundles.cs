using UnityEditor;
using UnityEngine;
using System.IO;

//  - create Gameobject, name will be used for prefab and assetbundle
//  - add geometry, adjust settings of gameobject
//  - create prefab -> drag GameObject from scene to folder prefabs
//  - in Unity-inspector go to settings of the generated prefab -> name of assetbundle = prefab-name !!!
//  - delete/deactivate original GameObject to avoid confusion on testing...
//  - run in Unity-Menu:  Assets -> Build AssetBundle

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundle")]
    static void ExportResource()
    {
        // // use path "streaming-assets"
        string folderName = "AssetBundles";
        string filePath = Path.Combine(Application.streamingAssetsPath, folderName);

        // external path for assetbundles
        //string filePath = "C:\\__assetBundles";

        //Build for Windows platform
        BuildPipeline.BuildAssetBundles(filePath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);

        //Uncomment to build for other platforms
        //BuildPipeline.BuildAssetBundles(filePath, BuildAssetBundleOptions.None, BuildTarget.iOS);
        //BuildPipeline.BuildAssetBundles(filePath, BuildAssetBundleOptions.None, BuildTarget.Android);
        //BuildPipeline.BuildAssetBundles(filePath, BuildAssetBundleOptions.None, BuildTarget.WebGL);
        //BuildPipeline.BuildAssetBundles(filePath, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);

        //Refresh the Project folder
        AssetDatabase.Refresh();
    }
}
