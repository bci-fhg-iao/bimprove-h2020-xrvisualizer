using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateAssignment : MonoBehaviour
{
    public TitleSync titleSync;
    public IssueBehaviour issueBehaviour;

    void Start()
    {
        StartCoroutine(SetState());
    }

    private IEnumerator SetState()
    {
        yield return new WaitForSeconds(0.1f);

        if(issueBehaviour.initialState == true)
        {
            titleSync.SetStringDirect(DateTime.Now.ToString());
        }
    }
}
