using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveInputFlyPhysicsFroh : MonoBehaviour
{
  public SteamVR_Action_Boolean triggerClick; //Grab Pinch is the trigger, select from inspecter
  public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;//which controller == to device
                                                                       // Use this for initialization
  private VRNavigationFlyPhysicsFroh theNavigation;


  // Use this for initialization
  void Start()
  {
    theNavigation = (VRNavigationFlyPhysicsFroh)FindObjectOfType(typeof(VRNavigationFlyPhysicsFroh));
    if (!theNavigation) Debug.Log("ViveInput : Navigation not Found! -- start");
  }

  private void OnEnable()
  {
    triggerClick.AddOnStateDownListener(Press, inputSource);
    triggerClick.AddOnStateUpListener(Release, inputSource);
  }

  private void OnDisable()
  {
    triggerClick.RemoveOnStateDownListener(Press, inputSource);
    triggerClick.RemoveOnStateUpListener(Release, inputSource);
  }

  private void Press(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
  {
    Debug.Log("Trigger was pressed");
    theNavigation.trigger = true;
  }

  private void Release(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
  {
    //Debug.Log("Trigger was released");
    theNavigation.trigger = false;
  }
}
