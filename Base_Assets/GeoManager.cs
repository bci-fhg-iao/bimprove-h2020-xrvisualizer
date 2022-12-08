using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

namespace TriLib
{
    namespace Samples
    {
        public class GeoManager : MonoBehaviour
        {


            public loadAssetBundle assetLoader;
            public JF_PersistentDataPathLoadSampleMultiuser loader;
            public List<GameObject> geoModules = new List<GameObject>();

            private void Awake()
            {
                loader = FindObjectOfType<JF_PersistentDataPathLoadSampleMultiuser>();
                assetLoader = FindObjectOfType<loadAssetBundle>();
            }

            void OnEnable()
            {
                foreach (GameObject geoID in GameObject.FindGameObjectsWithTag("GameObjectID"))
                {
                    geoModules.Add(geoID);
                }

                if (GameObject.FindGameObjectsWithTag("GameObjectID").Length == 0)
                {
                    StartCoroutine(ObjectIDCreation());
                }

                geoModules.Sort((a, b) => a.GetComponent<UISync>()._uiInt.CompareTo(b.GetComponent<UISync>()._uiInt));

                
                for (int i = 0; i < loader.allToggles.Count; i++)
                {

                    geoModules[i].GetComponent<UISync>().SetInt(i);
                    geoModules[i].GetComponent<GeoIDBehaviour>().connectedToggle = loader.allToggles[i];
                    geoModules[i].GetComponent<GeoIDBehaviour>().connectedObject = assetLoader.createdObjects[i];
                }
                
            }

            IEnumerator ObjectIDCreation()
            {
                yield return new WaitForSeconds(0.2f);

                if (GameObject.FindGameObjectsWithTag("GameObjectID").Length == 0)
                {
                    for (int i = 0; i < loader.allToggles.Count; i++)
                    {
                        var currentGeoIdObject =
                            Realtime.Instantiate("GeometryID",
                            transform.position,
                            Quaternion.identity,
                            ownedByClient: false,
                            preventOwnershipTakeover: false,
                            destroyWhenOwnerOrLastClientLeaves: false,
                            useInstance: null);

                        currentGeoIdObject.GetComponent<UISync>().SetInt(i);
                        geoModules.Add(currentGeoIdObject);
                        currentGeoIdObject.GetComponent<GeoIDBehaviour>().connectedToggle = loader.allToggles[i];
                        currentGeoIdObject.GetComponent<GeoIDBehaviour>().connectedObject = assetLoader.createdObjects[i];
                    }
                }
            }
        }
    }
}

