using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthorAssignment : MonoBehaviour
{
    private LoginManager loginManager;
    public TitleSync titleSync;
    public IssueBehaviour issueBehaviour;

    void Start()
    {
        StartCoroutine(SetState());
    }

    private IEnumerator SetState()
    {

        yield return new WaitForSeconds(0.1f);

        if (issueBehaviour.initialState == true)
        {
            loginManager = FindObjectOfType<LoginManager>();
            titleSync.SetStringDirect(loginManager._playerID);
        }
    }
}
