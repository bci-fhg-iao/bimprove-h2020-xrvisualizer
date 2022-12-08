using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class viveButtons : MonoBehaviour
{
    public SteamVR_Action_Boolean TriggerClick;
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;//which controller == to device
                                                                          // Use this for initialization

    private void Start() { } //Monobehaviours without a Start function cannot be disabled in Editor, just FYI
    // Update is called once per frame
    void Update() { }

    private void OnEnable()
    {
        TriggerClick.AddOnStateDownListener(Press, inputSource);
        TriggerClick.AddOnStateUpListener(Release, inputSource);
    }

    private void OnDisable()
    {
        TriggerClick.RemoveOnStateDownListener(Press, inputSource);
        TriggerClick.RemoveOnStateUpListener(Release, inputSource);
    }

    private void Press(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        //put your stuff here
        print("Press Success");
    }

    private void Release(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        //put your stuff here
        print("Release Success");
    }
}
