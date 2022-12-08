using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Normal.Realtime;

public class IssueManager : MonoBehaviour
{
    public List<GameObject> allIssues = new List<GameObject>();
    private Realtime realtime;
    public bool initialSet = false;
    public bool issueOnCheck = false;
    public GameObject marker;
    public Dropdown dropdown;

    public bool netIssueMode = false;

    public GameObject parentPanel;
    public GameObject buttonPrefab;

    public void SwitchIssueState()
    {
        var activeOption = dropdown.value;

        switch (activeOption)
        {
            // Generic Issue
            case 0:
                netIssueMode = false;
                marker.SetActive(false);
                Debug.Log("Generic Issue");
                break;

            // Net Issue - Vertical
            case 1:
                netIssueMode = true;
                marker.SetActive(true);
                Debug.Log("Net Issue");
                break;
        }

    }

    private void Start()
    {
        realtime = FindObjectOfType<Realtime>();
    }

    private IEnumerator PopulateIssueList()
    {
        yield return new WaitForSeconds(1f);
        UpdateIssueList();
    }

    private void Update()
    {
        if (realtime.connected == true && initialSet == false)
        {
            StartCoroutine(PopulateIssueList());
            initialSet = true;
        }
    }

    public void IssueSwapOn()
    {
        issueOnCheck = true;
        foreach (GameObject issue in allIssues)
        {
            issue.GetComponent<BoxCollider>().enabled = true;
            issue.GetComponent<MeshRenderer>().enabled = true;
            issue.GetComponent<IssueBehaviour>().issueInformation.GetComponent<CanvasGroup>().alpha = 0;
            issue.GetComponent<IssueBehaviour>().issueInformation.GetComponent<CanvasGroup>().interactable = false;
            issue.GetComponent<IssueBehaviour>().issueInformation.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void IssueSwapOff()
    {

        issueOnCheck = false;
        foreach (GameObject issue in allIssues)
        {
            issue.GetComponent<BoxCollider>().enabled = false;
            issue.GetComponent<MeshRenderer>().enabled = false;
            issue.GetComponent<IssueBehaviour>().issueInformation.GetComponent<CanvasGroup>().alpha = 0;
            issue.GetComponent<IssueBehaviour>().issueInformation.GetComponent<CanvasGroup>().interactable = false;
            issue.GetComponent<IssueBehaviour>().issueInformation.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        netIssueMode = false;
        marker.SetActive(false);
    }

    public void UpdateIssueList()
    {
        if(GameObject.FindGameObjectsWithTag("IssueUIButton").Length != 0)
        {
            foreach (GameObject issueButton in GameObject.FindGameObjectsWithTag("IssueUIButton"))
            {
                Destroy(issueButton);
            }
        }

        allIssues.Clear();

        foreach (GameObject issue in GameObject.FindGameObjectsWithTag("Issue"))
        {
            allIssues.Add(issue);
            AddButton(issue, issue.GetComponent<TitleSync>()._text);
        }
    }

    private void AddButton(GameObject issue, string myLabel)
    {
        // create Button from Prefab
        GameObject newObj = Instantiate(buttonPrefab, parentPanel.transform);

        issue.GetComponent<IssueBehaviour>().issueButton = newObj.GetComponent<IssueButtonBehaviour>();
        newObj.GetComponent<IssueButtonBehaviour>().issue = issue;

        // change Label From Button
        GameObject myLabelObj = newObj.transform.Find("Label").gameObject;
        Text myTextScript = myLabelObj.GetComponent<Text>();
        myTextScript.text = myLabel;
    }
}
