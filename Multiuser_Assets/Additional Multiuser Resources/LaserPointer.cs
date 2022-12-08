using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class LaserPointer : MonoBehaviour
{
    public BoolSync _boolSync;
    public LineRenderer _laser;
    public float _laserLenght = 20f;
    private bool _activationState = false;
    public bool isDesktop = true;
    public GameObject LeftHandPosition;
    public GameObject RightHandPosition;

    private void Start()
    {
        if (GameObject.Find("VROrigin") != null)
        {
            isDesktop = false;
        }
    }

    private void Update()
    {

        if (_boolSync._activationBool != _activationState)
        {
            _activationState = _boolSync._activationBool;
            _laser.enabled = _activationState;
        }

        if (_activationState == true && isDesktop == true)
        {
            _laser.SetPosition(0, LeftHandPosition.transform.position + new Vector3(0, -0.05f ,0 ));
            _laser.SetPosition(1, LeftHandPosition.transform.position + LeftHandPosition.transform.forward * _laserLenght);
        }

        if (_activationState == true && isDesktop == false)
        {
            _laser.SetPosition(0, LeftHandPosition.transform.position);
            _laser.SetPosition(1, LeftHandPosition.transform.position + LeftHandPosition.transform.forward * _laserLenght);
        }
    }
}
