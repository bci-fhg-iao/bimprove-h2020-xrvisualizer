using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICGV_UI : MonoBehaviour
{
    public GameObject Architecture;
    public GameObject Architecture_transp;
    public GameObject WhiteTextArchitecture;

    void Start()
    {
        Architecture_transp.SetActive (false);
        WhiteTextArchitecture.SetActive (false);
    }

    public void ToggleArchitecture()
    {
        if (Architecture.activeInHierarchy == true)
        {
            Architecture.SetActive (false);
            WhiteTextArchitecture.SetActive (true);
            Architecture_transp.SetActive (true);
        }
        else
        {
            Architecture.SetActive(true);
            WhiteTextArchitecture.SetActive (false);
            Architecture_transp.SetActive (false);
        }
    }
}
