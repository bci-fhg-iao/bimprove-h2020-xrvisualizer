using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Normal.Realtime;
using UnityEngine.UI;

public class ExtendedFlycam : MonoBehaviour
{

    /*
	EXTENDED FLYCAM
		Desi Quintans (CowfaceGames.com), 17 August 2012.
		Based on FlyThrough.js by Slin (http://wiki.unity3d.com/index.php/FlyThrough), 17 May 2011.
 
	LICENSE
		Free as in speech, and free as in beer.
 
	FEATURES
		WASD/Arrows:    Movement
		          Q:    Climb
		          E:    Drop
                      Shift:    Move faster
                    Control:    Move slower
                        End:    Toggle cursor locking to screen (you can also press Ctrl+P to toggle play mode on and off).
	*/

    public float cameraSensitivity = 90;
    public float climbSpeed = 4;
    public float normalMoveSpeed = 10;
    public float slowMoveFactor = 0.25f;
    public float fastMoveFactor = 3;

    public GameObject marker;
    public GameObject issueManager;
    public GameObject waypointMarker;
    public GameObject currentSelectedRoute;
    private placeObj thePlaceObj;
    private RouteManager routeManager;

    public GameObject radialMenu;
    public WaypointSync currentSelectedWaypoint;

    public bool netPlacementMode = false;
    public bool issueInteractionMode = false;
    public bool issuePlacementMode = false;
    public bool routePlacementMode = false;
    public bool waypointDeletion = false;
    public bool selectionActive = false;
    private Camera cam;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    public RealtimeAvatarManager avatarManager;
    public Realtime realtime;

    [SerializeField]
    private bool cameraIsActive = true;

    private bool laserActivationState = false;
    private bool nameSet = false;

    private IEnumerator DelayInteractionLock()
    {
        yield return new WaitForSeconds(0.3f);
        routeManager.interactionLock = false;
    }

    private IEnumerator NameCoroutine()
    {
        nameSet = true;
        yield return new WaitForSeconds(4f);

        GameObject mAvatar = avatarManager.localAvatar.gameObject;
        LoginManager mLogin = FindObjectOfType<LoginManager>();
        mAvatar.GetComponent<CoolEventHelper>().SetNameEvent(mLogin._playerID);
    }

    void Start()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        XRSettings.enabled = false;

        Screen.lockCursor = true;

        thePlaceObj = (placeObj)FindObjectOfType(typeof(placeObj));
        routeManager = (RouteManager)FindObjectOfType(typeof(RouteManager));
        avatarManager = (RealtimeAvatarManager)FindObjectOfType(typeof(RealtimeAvatarManager));
        realtime = FindObjectOfType<Realtime>();
        cam = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        if (routeManager.routeModeOn == true && routeManager.radialMenuActive == false && routeManager.hasObjectAttatched == false)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out hit, 8f))
            {
                if (hit.transform && hit.transform.tag == "Waypoint" || hit.transform && hit.transform.tag == "Route")
                {
                    hit.transform.GetComponent<WaypointSync>().IsSelected(true);
                    currentSelectedWaypoint = hit.transform.GetComponent<WaypointSync>();
                    selectionActive = true;
                }
            }
            else
            {
                if (currentSelectedWaypoint)
                {
                    currentSelectedWaypoint.IsSelected(false);
                }
                selectionActive = false;
            }
        }
    }

    void Update()
    {
        //WP MENU SPAWN
        if (Input.GetMouseButtonDown(0) && selectionActive == true && routeManager.currentActiveMenu == null && routeManager.interactionLock == false)
        {
            var menu = Instantiate(radialMenu, transform.position, transform.rotation, null);
            menu.GetComponent<RadialMenuManager>().waypointScript = currentSelectedWaypoint;
            menu.GetComponent<RadialMenuManager>().repositionMarker = marker.transform;

            if (currentSelectedWaypoint.isWaypoint == false)
            {
                menu.GetComponent<RadialMenuManager>().RouteWPDisabler();
            }
        }

        //DROP REPO WP
        if (Input.GetMouseButtonDown(0) && routeManager.hasObjectAttatched == true && routeManager.interactionLock == false)
        {
            currentSelectedWaypoint.GetComponent<WaypointSync>().attatchedTo = null;
            currentSelectedWaypoint.GetComponent<WaypointSync>().isBeingRepositioned = false;
            currentSelectedWaypoint.gameObject.transform.SetParent(GameObject.Find("building").transform);
            currentSelectedWaypoint.gameObject.transform.rotation = Quaternion.identity;
            currentSelectedWaypoint.line.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            selectionActive = false;
            routeManager.hasObjectAttatched = false;
            var route = currentSelectedWaypoint.GetComponent<WaypointSync>().route.GetComponent<RouteController>();

            for (int i = 0; i < route.waypointCollection.Count; i++)
            {
                route.waypointCollection[i].GetComponent<WaypointSync>().AdaptLine();
            }
            route.GetComponent<WaypointSync>().AdaptLine();
            StartCoroutine(DelayInteractionLock());
        }

        //EXIT RADIAL MENU
        if (Input.GetMouseButtonDown(1) && routeManager.radialMenuActive == true)
        {
            Destroy(routeManager.currentActiveMenu.gameObject);
            selectionActive = false;
        }

        //ISSUE INTERACTION AND PLACEMENT
        if (Input.GetMouseButtonDown(0) && issueInteractionMode == true)
        {
            thePlaceObj.trigger = false;

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform && hit.transform.tag == "Issue")
                {
                    hit.transform.GetComponent<IssueBehaviour>().SwapInteractionState();
                }
            }

            if (issuePlacementMode == true)
            {
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

        //NETISSUE PLACEMENT
        if(Input.GetMouseButtonDown(0) && netPlacementMode == true)
        {
            thePlaceObj.trigger = true;
        }

        if (Input.GetMouseButtonUp(0) && netPlacementMode == true)
        {
            thePlaceObj.trigger = false;
        }

        //NET PLACEMENT INTERACTION
        if (Input.GetKeyDown(KeyCode.LeftAlt) && issueManager.transform.GetComponent<IssueManager>().netIssueMode == true)
        {
            netPlacementMode = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt) && issueManager.transform.GetComponent<IssueManager>().netIssueMode == true)
        {
            netPlacementMode = false;
        }

        //ROUTEWAYPOINT PLACEMENT
        if (Input.GetMouseButtonDown(0) && routePlacementMode == true && waypointDeletion == false)
        {
            thePlaceObj.trigger = false;

            if (routePlacementMode == true)
            {
                var currentWaypoint =
                Realtime.Instantiate("Waypoint",
                   waypointMarker.transform.position,
                   Quaternion.identity,
                   ownedByClient: false,
                   preventOwnershipTakeover: false,
                   destroyWhenOwnerOrLastClientLeaves: false,
                   useInstance: null);


                currentWaypoint.GetComponent<WaypointSync>().route = currentSelectedRoute;
                currentSelectedRoute.GetComponent<RouteController>().AddWaypoint(currentWaypoint);
                routeManager.UpdateAllRoutes();
                currentWaypoint.GetComponent<WaypointSync>().SetIntegers(currentSelectedRoute.GetComponent<WaypointSync>()._routeIndex, currentSelectedRoute.GetComponent<RouteController>().waypointCollection.Count);
            }
        }
        
        //ROUTEWAYPOINT PLACEMENT MODE DISABLE
        if (Input.GetMouseButtonDown(1) && routePlacementMode == true)
        {
            currentSelectedRoute.GetComponent<RouteController>().ExitWaypointPlacementMode();
        }

        //ISSUE PLACEMENT INTERACTION
        if (Input.GetKeyDown(KeyCode.LeftAlt) && issueInteractionMode == true && issueManager.transform.GetComponent<IssueManager>().netIssueMode == false)
        {
            issuePlacementMode = true;
            marker.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt) && issueInteractionMode == true && issueManager.transform.GetComponent<IssueManager>().netIssueMode == false)
        {
            issuePlacementMode = false;
            marker.SetActive(false);
        }

        //CURSOR LOCK TOGGLE
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            cameraIsActive = !cameraIsActive;
            if (cameraIsActive)
            {
                Screen.lockCursor = true;
            } else
            {
                Screen.lockCursor = false;
            }
        }

        //LASERPOINTER INTERACTION
        if (Input.GetKeyDown(KeyCode.F))
        {

            laserActivationState = !laserActivationState;

            GameObject mAvatar = avatarManager.localAvatar.gameObject;

            Transform mXRAvi = mAvatar.transform.GetChild(0);
            Transform mLefthand = mXRAvi.GetChild(1);
            GameObject mLazor = mLefthand.GetChild(0).gameObject;

            LaserPointer mPointerScript = mLazor.GetComponent<LaserPointer>();
            BoolSync mBSync = mLazor.GetComponent<BoolSync>();

            mPointerScript.isDesktop = true;
            mBSync.SetBool(laserActivationState);
        }

        //NAME SET COROUNTINE
        if (realtime.connected && nameSet == false)
        {
            StartCoroutine(NameCoroutine());
        }
  
        //CONTROLLER MOVEMENT
        if (cameraIsActive)
        {
            rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, -90, 90);

            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            else
            {
                transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            }


            if (Input.GetKey(KeyCode.Q)) { transform.position += transform.up * climbSpeed * Time.deltaTime; }
            if (Input.GetKey(KeyCode.E)) { transform.position -= transform.up * climbSpeed * Time.deltaTime; }

            if (Input.GetKeyDown(KeyCode.End))
            {
                Screen.lockCursor = (Screen.lockCursor == false) ? true : false;
            }
        }

    }

    //METHODS FOR INTERACTION STATES
    public void NoIssueMode()
    {
        issueInteractionMode = false;
    }

    public void YesIssueMode()
    {
        issueInteractionMode = true;
        marker.SetActive(false);
    }
}


