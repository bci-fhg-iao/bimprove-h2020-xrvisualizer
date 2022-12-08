using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Normal.Realtime;

public class IssueBehaviour : MonoBehaviour
{
    public GameObject issue;
    public GameObject destroyRoot;
    public bool isSafetyIssue = false;
    public Canvas issueInformation;
    public IssueManager issueManager;
    public IssueButtonBehaviour issueButton;
    public GameObject markerPosition;
    public GameObject netIssue;
    public bool initialState = false;

    private void Awake()
    {
        issueInformation.worldCamera = FindObjectOfType<Camera>();
        issueManager = FindObjectOfType<IssueManager>();
    }

    private IEnumerator WaitDisable()
    {
        yield return new WaitForSeconds(0.1f);

        if (issueManager.issueOnCheck == true)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = !issue.GetComponent<MeshRenderer>().enabled;
        }
    }

    private void Start()
    {
        FindObjectOfType<IssueReparenter>().ReparentAllIssues();
        issueManager.UpdateIssueList();
        StartCoroutine(WaitDisable());
    }

    public void SwapInteractionState()
    {
        issue.GetComponent<MeshRenderer>().enabled = !issue.GetComponent<MeshRenderer>().enabled;
        issue.GetComponent<BoxCollider>().enabled = !issue.GetComponent<BoxCollider>().enabled;

        if (issueInformation.GetComponent<CanvasGroup>().alpha != 1)
        {
            issueInformation.GetComponent<CanvasGroup>().alpha = 1;
            issueInformation.GetComponent<CanvasGroup>().interactable = true;
            issueInformation.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            issueInformation.GetComponent<CanvasGroup>().alpha = 0;
            issueInformation.GetComponent<CanvasGroup>().interactable = false;
            issueInformation.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void DestroyIssue()
    {
        if(isSafetyIssue == false)
        {
            Realtime.Destroy(issue);
        }
        else if(isSafetyIssue == true)
        {
            Realtime.Destroy(destroyRoot);
        }
    }

    private void OnDestroy()
    {
        issueManager.UpdateIssueList();
    }
}
