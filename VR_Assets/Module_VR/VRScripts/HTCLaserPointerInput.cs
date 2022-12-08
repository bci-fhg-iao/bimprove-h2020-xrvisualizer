using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using Normal.Realtime;

public class HTCLaserPointerInput : MonoBehaviour
{
    public ControllerButton triggerClick = ControllerButton.Grip; //Grab Pinch is the trigger, select from inspecter
    public HandRole inputSource = HandRole.LeftHand;//which controller == to device

    public RealtimeAvatarManager _avatarManager;
    public HTCIssueInput issueInput;
    public HTCRouteInput routeInput;

    [SerializeField]
    private bool _isActivated = false;

    private void Start()
    {
        _avatarManager = FindObjectOfType<RealtimeAvatarManager>();
    }

    void Update()
    {
        if (ViveInput.GetPressDownEx(inputSource, triggerClick) && issueInput.issueMode == false && routeInput.routePlacementMode == false)
        {
            _isActivated = !_isActivated;

            GameObject mAvatar = _avatarManager.localAvatar.gameObject;

            Transform mXRAvi = mAvatar.transform.GetChild(0);
            Transform mLefthand = mXRAvi.GetChild(1);
            GameObject mLazor = mLefthand.GetChild(0).gameObject;

            BoolSync mBSync = mLazor.GetComponent<BoolSync>();
            mBSync.SetBool(_isActivated);

            //_avatarManager.localAvatar.transform.Find("Laser").transform.GetComponent<BoolSync>().SetBool(_isActivated);
        }
    }
}