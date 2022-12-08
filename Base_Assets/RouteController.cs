using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class RouteController : MonoBehaviour
{
    public GameObject route;
    //public LineRenderer lineRenderer;
    public RouteBehaviour routeButton;
    public Canvas routeInformation;
    private RouteManager routeManager;
    private ExtendedFlycam player;
    private HTCRouteInput vrPlayer;
    public List<GameObject> waypointCollection = new List<GameObject>();
    public MeshRenderer flag;
    public TitleSync titleSync;
    public WaypointSync attatchedWPS;

    private GameObject[] allwaypoints;

    public bool initialState = false;
    public bool secondaryState = false;
    public bool wasCreated = false;

    private void Awake()
    {
        routeInformation.worldCamera = FindObjectOfType<Camera>();
        routeManager = FindObjectOfType<RouteManager>();
        player = FindObjectOfType<ExtendedFlycam>();
        vrPlayer = FindObjectOfType<HTCRouteInput>();
    }

    
    private IEnumerator WaitDisable()
    {
        yield return new WaitForSeconds(0f);

        if (routeManager.routeModeOn == true)
        {
            flag.GetComponent<MeshRenderer>().enabled = true;
        }
    }
    

    private void Start()
    {
        attatchedWPS = GetComponent<WaypointSync>();
        FindObjectOfType<RouteReparenter>().ReparentAllRoutes();
        routeManager.UpdateRouteList();

        if(routeManager.routeModeOn != true)
        {
            StartCoroutine(WaitDisable());
        }

        allwaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        foreach(GameObject wp in allwaypoints)
        {
            if(wp.GetComponent<WaypointSync>()._routeIndex == gameObject.GetComponent<WaypointSync>()._routeIndex)
            {
                waypointCollection.Add(wp);
                wp.GetComponent<MeshRenderer>().material.color = flag.GetComponent<MeshRenderer>().material.color;
            }
        }

        waypointCollection.Sort((a, b) => a.GetComponent<WaypointSync>()._waypointIndex.CompareTo(b.GetComponent<WaypointSync>()._waypointIndex));
    }

    public void RefreshUI(int integer)
    {    
        routeButton.dropdown.ClearOptions();
        routeButton.dropdown.AddOptions(new List<string> { titleSync._text });

        for (int i = 1; i < waypointCollection.Count + 1; i++)
        {
            if(titleSync._text == "")
            {
                routeButton.dropdown.AddOptions(new List<string> { "Waypoint" + i });
            }
            else
            {
                routeButton.dropdown.AddOptions(new List<string> { titleSync._text + " " + i });
            }
        }

        routeButton.dropdown.value = integer;
    }

    public void RefreshWPList()
    {
        waypointCollection.Clear();
        allwaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        foreach (GameObject wp in allwaypoints)
        {
            if (wp.GetComponent<WaypointSync>()._routeIndex == gameObject.GetComponent<WaypointSync>()._routeIndex)
            {
                waypointCollection.Add(wp);
                wp.GetComponent<MeshRenderer>().material.color = flag.GetComponent<MeshRenderer>().material.color;
                wp.GetComponent<WaypointSync>().line.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = flag.GetComponent<MeshRenderer>().material.color;
            }
        }

        waypointCollection.Sort((a, b) => a.GetComponent<WaypointSync>()._waypointIndex.CompareTo(b.GetComponent<WaypointSync>()._waypointIndex));

        for (int i = 0; i < waypointCollection.Count; i++)
        {

            if (i < waypointCollection.Count - 1)
            {
                waypointCollection[i].GetComponent<WaypointSync>().followingWP = waypointCollection[i + 1];
            }
            waypointCollection[i].GetComponent<WaypointSync>().AdaptLine();
        }

        if (waypointCollection.Count > 0)
        {
            attatchedWPS.followingWP = waypointCollection[0];
            attatchedWPS.AdaptLine();
            waypointCollection[waypointCollection.Count - 1].GetComponent<WaypointSync>().line.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public void EnterWaypointPlacementMode()
    {
        if (routeManager.isVR == false)
        {
            player.routePlacementMode = true;
            player.currentSelectedRoute = gameObject;
            player.waypointMarker.SetActive(true);
        }
        else if (routeManager.isVR == true)
        {
            vrPlayer.routePlacementMode = true;
            vrPlayer.currentSelectedRoute = gameObject;
            vrPlayer.marker.SetActive(true);

            if (secondaryState == false)
            {
                StartCoroutine(VRCoroutine());
            }
        }
        routeManager.waypointInteractionMode = true;
    }

    public void ExitWaypointPlacementMode()
    {
        if (routeManager.isVR == false)
        {
            player.routePlacementMode = false;
            player.currentSelectedRoute = null;
            player.waypointMarker.SetActive(false);
        }
        else if (routeManager.isVR == true)
        {
            vrPlayer.routePlacementMode = false;
            vrPlayer.currentSelectedRoute = null;
            vrPlayer.marker.SetActive(false);
            secondaryState = false;
        }
        routeManager.waypointInteractionMode = false;
    }

    public void AddWaypoint(GameObject waypoint)
    {
        if(routeManager.isVR == false)
        {
            waypointCollection.Add(waypoint);
            FindObjectOfType<RouteReparenter>().ReparentAllRoutes();
            waypoint.GetComponent<MeshRenderer>().material.color = flag.GetComponent<MeshRenderer>().material.color;
        }
        else 
        {
            if(secondaryState == true)
            {
                waypointCollection.Add(waypoint);
                FindObjectOfType<RouteReparenter>().ReparentAllRoutes();
                waypoint.GetComponent<MeshRenderer>().material.color = flag.GetComponent<MeshRenderer>().material.color;
            }
        }
        RefreshUI(0);
    }

    private IEnumerator VRCoroutine()
    {
        yield return new WaitForSeconds(1f);
        secondaryState = true;
    }

    public void DestroyRoute()
    {
        Realtime.Destroy(route);
        foreach (GameObject waypoint in waypointCollection)
        {
            Realtime.Destroy(waypoint);
        }
    }

    private void OnDestroy()
    {
        routeManager.UpdateRouteList();
    }
}
