#pragma warning disable 649
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using Normal.Realtime;
using System.Collections;

namespace TriLib
{
    namespace Samples
    {
        /// <summary>
        /// Represents a sample that lets user load assets from "persistentDataPath"
        /// </summary>
        public class JF_PersistentDataPathLoadSampleMultiuser : MonoBehaviour
        {
            public loadAssetBundle myAssetBundleLoader;
            /// <summary>
            /// Reference to available assets.
            /// </summary>
            private string[] _files;

            /// <summary>
            /// Reference to latest loaded <see cref="UnityEngine.GameObject"/>.
            /// </summary>
            private GameObject _loadedGameObject;

            // my strings
            List<string> keyStrA = new List<string>();
            List<string> keyNumA = new List<string>();

            public GameObject myPannel;
            public Dropdown myDropdownScript;
            public handleDropdownChanged myDropdownChanged;
            public GameObject myToggleButton;

            public GameObject emptyObject;

            private string lastNumKey = "";

            //Multiuser References
            public List<GameObject> allToggles = new List<GameObject>();
            public bool loadingOperationFinished = false;
            public int indexPosition = 0;


            void Start()
            {
                
                    //myPannel = GameObject.Find("Panel_1");
                    myDropdownScript = GameObject.Find("DropdownArchitektur").GetComponent<Dropdown>();
                    myDropdownChanged = GameObject.Find("DropdownArchitektur").GetComponent<handleDropdownChanged>();
                    //myToggleButton = GameObject.Find("ToggleBlueprint_Appcenter");
                    
                    // read loadable data from datapath and store it in _files
                    var filter = AssetLoaderBase.GetSupportedFileExtensions();
                    _files = Directory.GetFiles(Application.dataPath).Where(x => filter.Contains("*" + FileUtils.GetFileExtension(x) + ";")).ToArray();
                    // get all keywords from FileNames
                    fillKeyA();
                    Debug.Log(_files.Length);
                    // load data and fill UI 
                    for (int i = 0; i < _files.Length; i++)
                    {
                        var file = _files[i];

                        AssetLoaderOptions assetLoaderOptions = AssetLoaderOptions.CreateInstance();
                        assetLoaderOptions.AutoPlayAnimations = false;
                        assetLoaderOptions.UseOriginalPositionRotationAndScale = true;

                        using (var assetLoader = new AssetLoader())
                        {
                            _loadedGameObject = assetLoader.LoadFromFile(file, assetLoaderOptions, gameObject);
                            _loadedGameObject.name = keyStrA[i];
                            _loadedGameObject.transform.localEulerAngles = Vector3.zero;
                            _loadedGameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                            _loadedGameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
                            optimizeMaterials(_loadedGameObject);
                            


                            /* 
                            _loadedGameObject.transform.localEulerAngles = new Vector3(180f, 0, 0);
                            _loadedGameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                            _loadedGameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
                             */
                        }

                        if (keyNumA[i].Equals("01"))
                        {
                            myDropdownScript.AddOptions(new List<string> { keyStrA[i] });
                            myDropdownChanged.myObjects.Add(_loadedGameObject);
                        }
                        else
                        {
                            addNewToggleButton(_loadedGameObject, keyStrA[i]);
                        }

                    }
                //
                // load Asset Bundles
                //
                bool isDone = myAssetBundleLoader.loadAssetBundleFolderFromGuiloader();
                if (isDone)
                {
                    for (int i = 0; i < myAssetBundleLoader.createdObjNames.Count; i++)
                    {
                        if (myAssetBundleLoader.createdObjects[i] != null)
                            myAssetBundleLoader.createdObjects[i].transform.parent = this.transform;
                        addNewToggleButton(myAssetBundleLoader.createdObjects[i], myAssetBundleLoader.createdObjNames[i]);
                    }
                }

                // add options Architektur Aus
                myDropdownScript.AddOptions(new List<string> { "keine Architektur" });
                    myDropdownChanged.myObjects.Add(emptyObject);

                    loadingOperationFinished = true;
                    
                
            }

            private void Update()
            {
                //Debug.Log(_files.Length);
            }

            private void addNewToggleButton(GameObject theObject, string myLabel)
            {
                    // create ToggleButton from Prefab
                    GameObject newObj = Instantiate(myToggleButton, myPannel.transform);
                    newObj.transform.name = indexPosition.ToString();
                    indexPosition = indexPosition + 1;

                    // add Objets to toggle with button
                    toggleObjectList myToggleObjectListScript = newObj.GetComponent<toggleObjectList>();
                    myToggleObjectListScript.ObjectsToToggle.Add(theObject);

                    // change Labe From Togglebutton
                    GameObject myLabelObj = newObj.transform.Find("Label").gameObject;
                    Text myTextScript = myLabelObj.GetComponent<Text>();
                    myTextScript.text = myLabel;

                    
                    if (!allToggles.Contains(newObj))
                    {
                        allToggles.Add(newObj);
                    }
            }

            private void fillKeyA()
            {
                foreach (var file in _files)
                {
                    string[] spearator = { "\\" };
                    Int32 count = 100;
                    var splitstringarr1 = file.Split(spearator, count, StringSplitOptions.RemoveEmptyEntries);
                    spearator[0] = "_";
                    var splitstringarr2 = splitstringarr1[splitstringarr1.Length - 1].Split(spearator, count, StringSplitOptions.RemoveEmptyEntries);
                    // Debug.Log("--- Hallo2 " + splitstringarr2[1]);
                    // add Key name to List
                    keyStrA.Add(splitstringarr2[1]);
                    keyNumA.Add(splitstringarr2[0]);
                }
            }
            private string checkKeyword(string searchStr)
            {
                string foundStr = "";
                foreach (string keyStr in keyStrA)
                {
                    var isElement = searchStr.Contains(keyStr);
                    if (isElement)
                    {
                        foundStr = keyStr;
                    }

                }

                return foundStr;
            }
            private void optimizeMaterials(GameObject obj)
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

                                // Materialien mit Glas in Transparenz-Modus und Transparenz verdoppeln
                                if (myMaterial.name.Contains("Glas") || myMaterial.name.Contains("glas"))
                                {
                                    myMaterial.ToFadeMode();
                                    myColor = myMaterial.color;
                                    myColor.a = myColor.a / 2.0f;
                                    myMaterial.color = myColor;
                                }
                                else
                                {
                                    // Glanzeffekte auf Null, da Trilib diese falsch interpretiert
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
    }
}
