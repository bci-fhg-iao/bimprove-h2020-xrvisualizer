using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using UnityEngine.UI;

public class HTCShowHideCanvas : MonoBehaviour
{
  public ControllerButton triggerClick = ControllerButton.Grip; //Grab Pinch is the trigger, select from inspecter
  public HandRole inputSource = HandRole.RightHand;//which controller == to device

  public CanvasGroup MyCanvas;
  [SerializeField]
  private bool isVisible = true;

  // Start is called before the first frame update
  void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if (ViveInput.GetPressDownEx(inputSource, triggerClick))
      {
        isVisible = !isVisible;
        SwapVisibilityState(isVisible);
      }
    }

    private void SwapVisibilityState(bool state)
    {
       if(state == true)
       {
            MyCanvas.alpha = 0;
            MyCanvas.interactable = false;
            MyCanvas.blocksRaycasts = false;
       }
       else
       {
            MyCanvas.alpha = 1;
            MyCanvas.interactable = true;
            MyCanvas.blocksRaycasts = true;
       }
    }

}
