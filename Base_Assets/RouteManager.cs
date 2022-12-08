using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Normal.Realtime;

public class RouteManager : MonoBehaviour
{
    public bool isVR = false;
    public List<GameObject> routeCollection = new List<GameObject>();
    private Realtime realtime;
    public bool initialSet = false;
    public bool routeModeOn = false;
    public bool waypointInteractionMode = false;
    public bool radialMenuActive = false;
    public bool hasObjectAttatched = false;
    public bool interactionLock = false;
    public RadialMenuManager currentActiveMenu;
    private IssueManager issueManager;

    public GameObject parentPanel;
    public GameObject buttonPrefab;
    public GameObject marker;
    public GameObject playerPos;
    

    public void UpdateAllRoutes()
    {
        foreach (GameObject route in routeCollection)
        {
            route.GetComponent<RouteController>().RefreshUI(route.GetComponent<RouteController>().routeButton.GetComponentInChildren<Dropdown>().value);
        }
    }

    public IEnumerator UnlockCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        interactionLock = false;
    }
    private void Start()
    {
        issueManager = FindObjectOfType<IssueManager>();
        realtime = FindObjectOfType<Realtime>();
        StartCoroutine(FixRotationVR());
    }

    private IEnumerator PopulateRouteList()
    {
        yield return new WaitForSeconds(0.2f);
        UpdateRouteList();
    }

    private IEnumerator FixRotationVR()
    {
        yield return new WaitForSeconds(.01f);
        GameObject.FindGameObjectWithTag("Player").transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (realtime.connected == true && initialSet == false)
        {
            StartCoroutine(PopulateRouteList());
            initialSet = true;
        }
    }

    public void RouteSwapOn()
    {
        foreach (GameObject route in routeCollection)
        {
            route.GetComponent<RouteController>().flag.enabled = true;
            var canvas = route.GetComponentInChildren<CanvasGroup>();
            canvas.alpha = 1;
            canvas.interactable = true;
            canvas.blocksRaycasts = true;

            if(route.GetComponent<RouteController>().routeButton.GetComponent<RouteBehaviour>().dropdown.value > 0)
            {
                route.GetComponent<RouteController>().waypointCollection[route.GetComponent<RouteController>().routeButton.GetComponent<RouteBehaviour>().dropdown.value - 1].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            }

            foreach (GameObject wp in route.GetComponent<RouteController>().waypointCollection)
            {
                wp.GetComponent<MeshRenderer>().enabled = true;
                wp.GetComponent<WaypointSync>().line.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            }

            route.GetComponent<WaypointSync>().line.transform.GetChild(0).transform.GetComponent<MeshRenderer>().enabled = true;
        }
        issueManager.marker.SetActive(false);
        issueManager.netIssueMode = false;
        routeModeOn = true;
    }

    public void RouteSwapOff()
    {
        foreach (GameObject route in routeCollection)
        {
            route.GetComponent<RouteController>().flag.enabled = false;
            var canvas = route.GetComponentInChildren<CanvasGroup>();
            canvas.alpha = 0;
            canvas.interactable = false;
            canvas.blocksRaycasts = false;

            foreach (GameObject wp in route.GetComponent<RouteController>().waypointCollection)
            {
                wp.GetComponent<MeshRenderer>().enabled = false;
                wp.GetComponent<WaypointSync>().IsHighlighted(false);
                wp.GetComponent<WaypointSync>().IsSelected(false);
                wp.GetComponent<WaypointSync>().line.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                currentActiveMenu = null;
            }
            route.GetComponent<WaypointSync>().line.transform.GetChild(0).transform.GetComponent<MeshRenderer>().enabled = false;
        }
        routeModeOn = false;
    }

    public void UpdateRouteList()
    {
        foreach (GameObject routeButton in GameObject.FindGameObjectsWithTag("RouteUIButton"))
        {
            Destroy(routeButton);
        }

        routeCollection.Clear();
        foreach (GameObject route in GameObject.FindGameObjectsWithTag("Route"))
        {
            routeCollection.Add(route);
            AddButton(route, route.GetComponent<TitleSync>()._text);
        }

        routeCollection.Sort((a, b) => a.GetComponent<WaypointSync>()._routeIndex.CompareTo(b.GetComponent<WaypointSync>()._routeIndex));
    }

    private void AddButton(GameObject route, string myLabel)
    {
        // create Button from Prefab
        GameObject newObj = Instantiate(buttonPrefab, parentPanel.transform);

        route.GetComponent<RouteController>().routeButton = newObj.GetComponent<RouteBehaviour>();
        newObj.GetComponent<RouteBehaviour>().route = route;

        // change Label From Button
        Dropdown myDropdown = newObj.transform.Find("Dropdown").gameObject.transform.GetComponent<Dropdown>();
        myDropdown.ClearOptions();
        myDropdown.AddOptions(new List<string> { myLabel });
    }

    public void CreateNewRoute()
    {
            var currentRoute =
        Realtime.Instantiate("Route",
        playerPos.transform.position,
        Quaternion.identity,
        ownedByClient: false,
        preventOwnershipTakeover: false,
        destroyWhenOwnerOrLastClientLeaves: false,
        useInstance: null);

        currentRoute.GetComponent<RouteController>().initialState = true;
        currentRoute.GetComponent<WaypointSync>().SetIntegers(routeCollection.Count - 1, 0);
        routeCollection.Add(currentRoute);
    }
}
