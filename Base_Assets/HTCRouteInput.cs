using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using Normal.Realtime;

public class HTCRouteInput : MonoBehaviour
{
    public ControllerButton gripClick = ControllerButton.Grip; //Grab Pinch is the trigger, select from inspecter
    public ControllerButton touch = ControllerButton.PadTouch; //Grab Pinch is the trigger, select from inspecter
    public ControllerButton triggerClick = ControllerButton.Trigger; //Grab Pinch is the trigger, select from inspecter
    public HandRole inputSource = HandRole.LeftHand;//which controller == to device

    public GameObject marker;
    private RouteManager routeManager;
    public GameObject radialMenu;

    public GameObject currentSelectedRoute;
    public WaypointSync currentSelectedWaypoint;


    public bool routePlacementMode = false;
    public bool waypointDeletion = false;
    public bool selectionActive = false;

    private void Start()
    {
        routeManager = FindObjectOfType<RouteManager>();
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
                if(currentSelectedWaypoint)
                {
                    currentSelectedWaypoint.IsSelected(false);
                }
                selectionActive = false;     
            }
        }
    }

    private IEnumerator DelayInteractionLock()
    {
        yield return new WaitForSeconds(0.3f);
        routeManager.interactionLock = false;
    }

    void Update()
    {
        //WP MENU SPAWN
        if (ViveInput.GetPressDown(inputSource, triggerClick) && selectionActive == true && routeManager.currentActiveMenu == null && routeManager.interactionLock == false)
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
        if (ViveInput.GetPressDown(inputSource, triggerClick) && routeManager.hasObjectAttatched == true && routeManager.interactionLock == false)
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
        if (ViveInput.GetPress(inputSource, touch) && routeManager.radialMenuActive == true)
        {
            Destroy(routeManager.currentActiveMenu.gameObject);
            selectionActive = false;
        }

        //WAYPOINTPLACEMENT
        if (ViveInput.GetPressDown(inputSource, gripClick) && routePlacementMode == true && currentSelectedRoute.GetComponent<RouteController>().secondaryState == true)
        {
            var currentWaypoint =
            Realtime.Instantiate("Waypoint",
            marker.transform.position,
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

        //EXIT WAYPOINTS
        if (ViveInput.GetPressDown(inputSource, touch) && routePlacementMode == true)
        {
            currentSelectedRoute.GetComponent<RouteController>().ExitWaypointPlacementMode();
        }
    }
}