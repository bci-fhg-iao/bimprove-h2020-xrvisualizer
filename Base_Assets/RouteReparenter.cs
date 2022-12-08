using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteReparenter : MonoBehaviour
{
    private GameObject[] routes;
    private GameObject[] waypoints;

    public GameObject parent;

    public void ReparentAllRoutes()
    {
        routes = GameObject.FindGameObjectsWithTag("Route");
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        foreach (GameObject route in routes)
        {
            route.transform.SetParent(parent.transform);
        }

        foreach (GameObject wp in waypoints)
        {
            wp.transform.SetParent(parent.transform);
        }
    }
}
