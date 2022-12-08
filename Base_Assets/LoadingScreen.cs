using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class LoadingScreen : MonoBehaviour
{
    private Realtime realtime;
    private bool state = true;
    [SerializeField]
    private int secondsToLoad;

    private void Start()
    {
        realtime = FindObjectOfType<Realtime>();
    }

    private void Update()
    {
        
        if(realtime.connected == true && state == true)
        {
            StartCoroutine(DestroyLoadingScreen());
        }
        
    }

    IEnumerator DestroyLoadingScreen()
    {
        yield return new WaitForSeconds(secondsToLoad);
        Destroy(gameObject);
    }

    public void DestroyLoadingScreenQuick()
    {
        Destroy(gameObject);
    }
}
