using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuManager : MonoBehaviour
{
    public WaypointSync waypointScript;
    public Transform repositionMarker;
    private RouteManager routeManager;
    public Button buttonClone;
    public Button buttonDelete;

    public void RouteWPDisabler()
    {
        buttonClone.interactable = false;
        buttonDelete.interactable = false;
    }

    private void Start()
    {
        routeManager = FindObjectOfType<RouteManager>();
        routeManager.radialMenuActive = true;
        routeManager.currentActiveMenu = this;
    }

    public IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    public void Clone()
    {
        StartCoroutine(DestroyCoroutine());
        waypointScript.CloneWaypoint();
    }

    public void Delete()
    {
        StartCoroutine(DestroyCoroutine());
        waypointScript.DeleteWaypoint();
    }

    public void Reposition()
    {
        routeManager.interactionLock = true;
        routeManager.hasObjectAttatched = true;
        StartCoroutine(DestroyCoroutine());
        waypointScript.PickupSetDownWP(repositionMarker);
    }

    public void OnDestroy()
    {
        routeManager.radialMenuActive = false;
        routeManager.interactionLock = false;
        waypointScript.route.GetComponent<RouteController>().RefreshWPList();
    }
}
