using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;

public class NameSync : RealtimeComponent<NameSyncModel>
{
    public string _userName;
    public Text _nameDisplay;

    protected override void OnRealtimeModelReplaced(NameSyncModel previousModel, NameSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.userNameDidChange -= UserNameDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
                currentModel.userName = _userName;

            UpdateName();

            currentModel.userNameDidChange += UserNameDidChange;
        }
    }

    private void UserNameDidChange(NameSyncModel model, string value)
    {
        UpdateName();
    }

    private void UpdateName()
    {
        _userName = model.userName;
        _nameDisplay.text = _userName;
    }

    public void SetName(string String)
    {
        model.userName = String;
    }
}
