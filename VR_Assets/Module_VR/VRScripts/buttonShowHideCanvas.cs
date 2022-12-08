using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class buttonShowHideCanvas : MonoBehaviour
{
  public SteamVR_Action_Boolean TriggerClick = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
  public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;//which controller == to device
  SteamVR_Behaviour_Pose trackedObj;

  // Use this for initialization

  public GameObject MyCanvas;
  private bool isVisible = false;

  private void Start() {
    MyCanvas.SetActive(isVisible);
  } //Monobehaviours without a Start function cannot be disabled in Editor, just FYI
    // Update is called once per frame

  private void Update()
  {
    if (TriggerClick.changed)
    {
      Debug.Log(TriggerClick);
    }
    
  }

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
    isVisible = !isVisible;
    MyCanvas.SetActive(isVisible);
  }

  private void Release(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
  {
    //put your stuff here
    print("Release Success");
  }
}
