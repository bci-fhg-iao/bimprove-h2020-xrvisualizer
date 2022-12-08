using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class ColorSync : RealtimeComponent<ColorSyncModel>
{
    public MeshRenderer _meshRenderer;
    public MeshRenderer _line;
    private RouteController _routeController;

    public Color currentColor;
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;
    public Color color5;

    private void Awake()
    {
        _routeController = gameObject.GetComponent<RouteController>();
    }

    protected override void OnRealtimeModelReplaced(ColorSyncModel previousModel, ColorSyncModel currentModel)
    {
        if (previousModel != null)
        {
            // Unregister from events
            previousModel.colorDidChange -= ColorDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
                currentModel.color = _meshRenderer.material.color;

            // Update the mesh render to match the new model
            UpdateMeshRendererColor();

            // Register for events so we'll know if the color changes later
            currentModel.colorDidChange += ColorDidChange;
        }
    }

    private void ColorDidChange(ColorSyncModel model, Color value)
    {
        // Update the mesh renderer
        UpdateMeshRendererColor();
    }

    private void UpdateMeshRendererColor()
    {
        // Get the color from the model and set it on the mesh renderer.
        _meshRenderer.material.color = model.color;
        _line.material.color = model.color;
        currentColor = model.color;
        StartCoroutine(DelayedUpdate());
    }

    private IEnumerator DelayedUpdate()
    {
        yield return new WaitForSeconds(0.05f);
        if (_routeController.waypointCollection.Count > 0)
        {
            foreach (GameObject waypoint in _routeController.waypointCollection)
            {
                waypoint.GetComponent<MeshRenderer>().material.color = model.color;
                waypoint.GetComponent<WaypointSync>().line.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = model.color;
                Debug.Log("adjusted color");
            }
        }
    }

    public void SetColor(Color color)
    {
        // Set the color on the model
        // This will fire the colorChanged event on the model, which will update the renderer for both the local player and all remote players.
        model.color = color;
        currentColor = color;
    }

    public void SetToC1()
    {
        SetColor(color1);
    }

    public void SetToC2()
    {
        SetColor(color2);
    }

    public void SetToC3()
    {
        SetColor(color3);
    }

    public void SetToC4()
    {
        SetColor(color4);
    }

    public void SetToC5()
    {
        SetColor(color5);
    }
}