using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;

public class UISync : RealtimeComponent<UISyncModel>
{
    public int _uiInt;
    public bool _uiBool;
    public bool geoModule = false;

    protected override void OnRealtimeModelReplaced(UISyncModel previousModel, UISyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.uiIntDidChange -= IntDidChange;
        }

        if (previousModel != null)
        {
            previousModel.uiBoolDidChange -= BoolDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
                currentModel.uiInt = _uiInt;

            UpdateStates();

            currentModel.uiIntDidChange += IntDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
                currentModel.uiBool = _uiBool;

            UpdateStates();

            currentModel.uiBoolDidChange += BoolDidChange;
        }
    }

    private void IntDidChange(UISyncModel model, int value)
    {
        UpdateStates();
    }

    private void BoolDidChange(UISyncModel model, bool value)
    {
        UpdateStates();
    }

    private void UpdateStates()
    {
        _uiInt = model.uiInt;
        _uiBool = model.uiBool;
        if(geoModule == true && transform.GetComponent<GeoIDBehaviour>().connectedObject != null)
        {
            Debug.Log("applied initial state");
            transform.GetComponent<GeoIDBehaviour>().connectedObject.SetActive(model.uiBool);
        }
        else if(geoModule == true)
        {
            StartCoroutine(DelayedSet());
        }
    }

    public void SetBool(bool boolean)
    {
        model.uiBool = boolean;
        _uiBool = boolean;
    }

    public void SetInt(int integer)
    {
        model.uiInt = integer;
    }

    private IEnumerator DelayedSet()
    {
        yield return new WaitForSeconds(0.5f);
        if(transform.GetComponent<GeoIDBehaviour>().connectedObject != null)
        {
            transform.GetComponent<GeoIDBehaviour>().connectedObject.SetActive(model.uiBool);
            transform.GetComponent<GeoIDBehaviour>().connectedToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(model.uiBool);
        }
        /*
        yield return new WaitForSeconds(0.5f);
        if (transform.GetComponent<GeoIDBehaviour>().connectedObject != null)
        {
            transform.GetComponent<GeoIDBehaviour>().connectedToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(model.uiBool);
            transform.GetComponent<GeoIDBehaviour>().connectedObject.SetActive(model.uiBool);
        }
        */
    }
}