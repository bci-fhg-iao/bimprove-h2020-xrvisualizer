using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using Normal.Realtime;

public class HTCIssueInput : MonoBehaviour
{
    public ControllerButton gripClick = ControllerButton.Grip; //Grab Pinch is the trigger, select from inspecter
    public ControllerButton triggerClick = ControllerButton.Trigger; //Grab Pinch is the trigger, select from inspecter
    public HandRole inputSource = HandRole.LeftHand;//which controller == to device

    public GameObject marker;
    private IssueManager issueManager;
    private LoadingScreen loadingScreen;
    private RouteManager routeManager;

    public bool issueMode = false;

    private void Start()
    {
        issueManager = FindObjectOfType<IssueManager>();
        loadingScreen = FindObjectOfType<LoadingScreen>();
        routeManager = FindObjectOfType<RouteManager>();
    }

    void Update()
    {
        if (ViveInput.GetPressDown(inputSource, triggerClick) && issueMode == true)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform && hit.transform.tag == "Issue")
                {
                    hit.transform.GetComponent<IssueBehaviour>().SwapInteractionState();
                }
            }
        }

        if (issueManager.netIssueMode == false && routeManager.routeModeOn == false)
        {

            if (ViveInput.GetPressDown(inputSource, gripClick) && issueMode == true)
            {
                marker.SetActive(true);
            }

            if (ViveInput.GetPressUp(inputSource, gripClick) && issueMode == true)
            {
                marker.SetActive(false);

                var currentIssue =
                Realtime.Instantiate("Issue",
                marker.transform.position,
                Quaternion.identity,
                ownedByClient: false,
                preventOwnershipTakeover: false,
                destroyWhenOwnerOrLastClientLeaves: false,
                useInstance: null);

                currentIssue.GetComponent<IssueBehaviour>().initialState = true;

                issueManager.GetComponent<IssueManager>().allIssues.Add(currentIssue);
            }
        }
    }

    public void YesIssueMode()
    {
        issueMode = true;
    }

    public void NoIssueMode()
    {
        issueMode = false;
        marker.SetActive(false);
    }
}