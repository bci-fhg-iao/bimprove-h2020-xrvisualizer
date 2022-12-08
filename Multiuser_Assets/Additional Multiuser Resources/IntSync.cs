using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;

public class IntSync : RealtimeComponent<IntSyncModel>
{
    public int _dropdownInt;

    protected override void OnRealtimeModelReplaced(IntSyncModel previousModel, IntSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.dropdownIntDidChange -= IntDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
                currentModel.dropdownInt = _dropdownInt;

            UpdateStates();

            currentModel.dropdownIntDidChange += IntDidChange;
        }
    }

    private void IntDidChange(IntSyncModel model, int value)
    {
        UpdateStates();
    }

    private void UpdateStates()
    {
        _dropdownInt = model.dropdownInt;
    }

    public void SetInt(int integer)
    {
        model.dropdownInt = integer;
    }
}