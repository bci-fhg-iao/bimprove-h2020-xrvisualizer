using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;

namespace TriLib
{
    namespace Samples
    {
        public class DiscrepancyManager : MonoBehaviour
        {
            [SerializeField]
            private string _currentString;
            [SerializeField]
            private int _currentInt;

            private bool hasBeenChecked = false;
            private bool secondaryCheck = false;
            private bool hasBeenCompared = false;

            public Realtime _realtime;
            private DiscrepancySync discrepancySync;
            private JF_PersistentDataPathLoadSampleMultiuser loaderScript;
            private GameObject building;
            public List<GameObject> geometrieList = new List<GameObject>();
            public CanvasGroup canvasGroup;

            

            private void Awake()
            {
                discrepancySync = transform.GetComponent<DiscrepancySync>();
                loaderScript = GameObject.Find("building").transform.GetComponent<JF_PersistentDataPathLoadSampleMultiuser>();
                building = GameObject.Find("building");
            }

            private void Update()
            {
                if(hasBeenChecked == false)
                {
                    LocalCheck();
                }

                if(hasBeenChecked == true && secondaryCheck == false && _realtime.clientID == 0)
                {
                    Debug.Log(_currentString);
                    discrepancySync.SetDiscrepancyString(_currentString);
                    SetStates();
                }

                if (_realtime.clientID > 0 && hasBeenChecked == true && hasBeenCompared == false)
                {
                    Compare();
                }

            }

            private IEnumerator CheckCoroutine()
            {
                yield return new WaitForSeconds(5f);
                hasBeenCompared = true;
            }

            private void LocalCheck()
            {
                if (loaderScript.loadingOperationFinished == true && hasBeenChecked == false)
                {
                    _currentInt = building.transform.childCount;
                    for (int i = 0; i < _currentInt; i++)
                    {
                        geometrieList.Add(building.transform.GetChild(i).gameObject);
                        _currentString = _currentString + geometrieList[i].name;
                    }
                    hasBeenChecked = true;

                    discrepancySync._discrepancyInt = _currentInt;
                    discrepancySync._discrepancyString = _currentString;
                }
            }

            private void SetStates()
            {
                discrepancySync.SetDiscrepancyInt(_currentInt);
                discrepancySync.SetDiscrepancyString(_currentString);
                secondaryCheck = true;
            }

            private void Compare()
            {
                var currentString = discrepancySync._discrepancyString;
                Debug.Log(currentString);
                if (_currentString != currentString)
                {
                    canvasGroup.alpha = 1;
                    hasBeenCompared = true;
                }
                StartCoroutine(CheckCoroutine());
            }
        }
    }
}
   
