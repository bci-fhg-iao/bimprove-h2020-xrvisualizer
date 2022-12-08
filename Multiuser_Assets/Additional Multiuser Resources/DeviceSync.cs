using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;


public class DeviceSync : RealtimeComponent<DeviceSyncModel>
{
    public int _deviceType;

    protected override void OnRealtimeModelReplaced(DeviceSyncModel previousModel, DeviceSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.deviceTypeDidChange -= DeviceTypeDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
                currentModel.deviceType = _deviceType;

            UpdateStates();

            currentModel.deviceTypeDidChange += DeviceTypeDidChange;
        }
    }

    private void DeviceTypeDidChange(DeviceSyncModel model, int value)
    {
        UpdateStates();
    }

    private void UpdateStates()
    {
        _deviceType = model.deviceType;
    }

    public void SetBool(int integer)
    {
        model.deviceType = integer;
    }
}
