using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;


public class BoolSync : RealtimeComponent<BoolSyncModel>
{
    public bool _activationBool;

    protected override void OnRealtimeModelReplaced(BoolSyncModel previousModel, BoolSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.activationBoolDidChange -= BoolDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
                currentModel.activationBool = _activationBool;

            UpdateStates();

            currentModel.activationBoolDidChange += BoolDidChange;
        }
    }

    private void BoolDidChange(BoolSyncModel model, bool value)
    {
        UpdateStates();
    }

    private void UpdateStates()
    {
        _activationBool = model.activationBool;
    }

    public void SetBool(bool boolean)
    {
        model.activationBool = boolean;
    }
}
