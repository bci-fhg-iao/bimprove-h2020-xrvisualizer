using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class vivePlaceObjListener : MonoBehaviour
{
    public ControllerButton triggerClick = ControllerButton.Trigger; //Grab Pinch is the trigger, select from inspecter
    public HandRole inputSource = HandRole.RightHand;//which controller == to device
                                                     // Use this for initialization
    private placeObj thePlaceObj;
    private IssueManager issueManager;
    private RouteManager routeManager;

    // Start is called before the first frame update
    void Start()
    {
        issueManager = (IssueManager)FindObjectOfType(typeof(IssueManager));
        routeManager = (RouteManager)FindObjectOfType(typeof(RouteManager));
        thePlaceObj = (placeObj)FindObjectOfType(typeof(placeObj));
        if (!thePlaceObj) Debug.Log("ViveInput : thePlaceObj not Found! -- start");
    }

    // Update is called once per frame
    void Update()
    {
        if (issueManager.netIssueMode == true && routeManager.routeModeOn == false)
        {
            if (ViveInput.GetPressDownEx(inputSource, triggerClick))
            {
                //Debug.Log("Trigger was pressed");
                thePlaceObj.trigger = true;
            }
            if (ViveInput.GetPressUpEx(inputSource, triggerClick))
            {
                //Debug.Log("Trigger was released");
                thePlaceObj.trigger = false;
            }
        }
    }
}
