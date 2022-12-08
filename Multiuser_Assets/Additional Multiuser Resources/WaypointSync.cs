using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class WaypointSync : RealtimeComponent<WaypointSyncModel>
{
    public int _routeIndex;
    public int _waypointIndex;
    private RouteManager routeManager;
    public bool isWaypoint = false;
    public bool isBeingRepositioned = false;
    public GameObject route;
    public CanvasGroup canvas;
    public MeshRenderer outlineShader;
    public MeshRenderer outlineShader2;

    public GameObject line;
    public GameObject followingWP;

    public GameObject attatchedTo;

    public Material matHighlighted;
    public Material matSelected;

    private Vector3 lastposition;

    public void AdaptLine()
    {
        if(followingWP)
        {
            float distance = Vector3.Distance(transform.position, followingWP.transform.position);
            line.transform.localScale = new Vector3(1, 1, distance * 10);
            line.transform.LookAt(followingWP.transform, Vector3.back);
        }
    }

    private void FixedUpdate()
    {
        if(lastposition != transform.position)
        {
            lastposition = transform.position;
            route.GetComponent<RouteController>().RefreshWPList();
        }

        if(isBeingRepositioned == true)
        {
            transform.position = attatchedTo.transform.position;
        }
    }

    private void Start()
    {
        routeManager = FindObjectOfType<RouteManager>();
        routeManager.UpdateRouteList();
        StartCoroutine(WaitDisable());
    }

    private IEnumerator WaitDisable()
    {
        if (isWaypoint == true)
        {
            route = routeManager.routeCollection[_routeIndex];
        }
        yield return new WaitForSeconds(0.05f);

        if (routeManager.routeModeOn == true)
        {
            if(isWaypoint == true)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                line.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            }

            if (isWaypoint != true)
            {
                route.GetComponent<RouteController>().flag.enabled = true;
                line.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                canvas.alpha = 1;
                canvas.interactable = true;
                canvas.blocksRaycasts = true;
            }
            transform.SetParent(GameObject.Find("building").transform);
        }
        route.GetComponent<RouteController>().RefreshWPList();
    }

    protected override void OnRealtimeModelReplaced(WaypointSyncModel previousModel, WaypointSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.routeIndexDidChange -= WaypointIndexDidChange;
        }

        if (previousModel != null)
        {
            previousModel.waypointIndexDidChange -= WaypointIndexDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
                currentModel.waypointIndex = _waypointIndex;

            UpdateIndexes();

            currentModel.waypointIndexDidChange += WaypointIndexDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
                currentModel.routeIndex = _routeIndex;

            UpdateIndexes();

            currentModel.routeIndexDidChange += WaypointIndexDidChange;
        }
    }

    private void WaypointIndexDidChange(WaypointSyncModel model, int value)
    { 
        UpdateIndexes();
    }

    private void UpdateIndexes()
    {
        _routeIndex = model.routeIndex;
        _waypointIndex = model.waypointIndex;
    }

    public void SetIntegers(int route, int waypoint)
    {
        model.waypointIndex = waypoint;
        model.routeIndex = route;
    }

    public void PickupSetDownWP(Transform parent)
    {
        if (attatchedTo == null)
        {
            transform.GetComponent<RealtimeView>().RequestOwnership();
            transform.GetComponent<RealtimeTransform>().RequestOwnership();
            isBeingRepositioned = true;
            attatchedTo = parent.gameObject;
        }

        if(routeManager.isVR == true)
        {
            FindObjectOfType<HTCRouteInput>().selectionActive = false;
        }
        else
        {
            FindObjectOfType<ExtendedFlycam>().selectionActive = false;
        }
    }

    public void DeleteWaypoint()
    {
        Realtime.Destroy(gameObject);

        if (routeManager.isVR == true)
        {
            FindObjectOfType<HTCRouteInput>().selectionActive = false;
        }
        else
        {
            FindObjectOfType<ExtendedFlycam>().selectionActive = false;
        }
    }

    public void IsHighlighted(bool state)
    {
        outlineShader2.material = matHighlighted;
        outlineShader2.enabled = state;
    }

    public void IsSelected(bool state)
    {
        outlineShader.material = matSelected;
        outlineShader.enabled = state;
    }

    public void CloneWaypoint()
    {
        var newElement = 
        Realtime.Instantiate("Waypoint",
        transform.position + new Vector3(0f, 0.3f, 0f),
        Quaternion.identity,
        ownedByClient: false,
        preventOwnershipTakeover: false,
        destroyWhenOwnerOrLastClientLeaves: false,
        useInstance: null);

        route.GetComponent<RouteController>().AddWaypoint(newElement);
        routeManager.UpdateAllRoutes();

        var waypointArray = route.GetComponent<RouteController>().waypointCollection;
        for (int i = _waypointIndex; i < waypointArray.Count; i++)
        {
            waypointArray[i].GetComponent<WaypointSync>().SetIntegers(_routeIndex, waypointArray[i].GetComponent<WaypointSync>()._waypointIndex + 1);
        }
        newElement.GetComponent<WaypointSync>().SetIntegers(_routeIndex, _waypointIndex + 1);
    }

    private void OnDestroy()
    {
        var waypointArray = route.GetComponent<RouteController>().waypointCollection;
        for (int i = _waypointIndex; i < waypointArray.Count; i++)
        {
            waypointArray[i].GetComponent<WaypointSync>()._waypointIndex -= 1;
        }
        route.GetComponent<RouteController>().RefreshWPList();
    }
}