using System.Collections;
using System.Collections.Generic;
using TriLib;
using UnityEngine;

public class RealtimeAssetLoader : MonoBehaviour
{
    // Start is called before the first frame update
    //www.mrtzhckr.de/3dfiles/Statue_Opt_8P.FBX
    //www.mrtzhckr.de/3dfiles/primitive1.fbx
    //www.mrtzhckr.de/3dfiles/Example.fbx
    //www.mrtzhckr.de/3dfiles/IFC_convert.3ds
    // www.mrtzhckr.de/3dfiles/ifctest.ifc
    public string _uri;
    public string _ext;
    public GameObject TextPrefab;
    private AssetDownloader _assetdownloader;
    void Awake()
    {
        _assetdownloader = GetComponent<AssetDownloader>();
    }
    void Start()
    {
        _assetdownloader.DownloadAsset(_uri, _ext, null, null, null, this.gameObject, _assetdownloader.ProgressCallback);
        this.gameObject.name = _uri;
        TextPrefab = GameObject.Find(_uri + "Text");
       // TextPrefab.GetComponent<StringSync>().SetLoadState(true);
    }


}
