using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IssueReparenter : MonoBehaviour
{
    private GameObject[] issues;
    public GameObject parent;

    public void ReparentAllIssues()
    {
        issues = GameObject.FindGameObjectsWithTag("Issue");
        foreach(GameObject issue in issues)
        {
            if(issue.GetComponent<IssueBehaviour>().isSafetyIssue == true)
            {
                issue.GetComponent<IssueBehaviour>().destroyRoot.transform.SetParent(parent.transform);
            }
            else
            {
                issue.transform.SetParent(parent.transform);
            }
        }
    }
}
