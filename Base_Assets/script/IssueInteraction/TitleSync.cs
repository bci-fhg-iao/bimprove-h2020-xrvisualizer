using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;

public class TitleSync : RealtimeComponent<TitleSyncModel>
{
    public string _text;
    public Text _textDisplay;
    public InputField _textInput;
    public bool isForRoute = false;

    protected override void OnRealtimeModelReplaced(TitleSyncModel previousModel, TitleSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.titleDidChange -= StringDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
                currentModel.title = _text;

            UpdateString();

            currentModel.titleDidChange += StringDidChange;
        }
    }

    private void StringDidChange(TitleSyncModel model, string value)
    {
        UpdateString();
    }

    private void Start()
    {
        if (_text != "")
        {
            _textDisplay.text = _text;
        }
    }

    private void UpdateString()
    {
        _text = model.title;
        _textDisplay.text = _text;

        if(isForRoute == false)
        {
            FindObjectOfType<IssueManager>().UpdateIssueList();
        }
        else
        {
            FindObjectOfType<RouteManager>().UpdateRouteList();
        }
    }

    public void SetString()
    {
        if(_textInput != null)
        {
            _text = _textInput.text;
        }
        model.title = _text;
    }

    public void SetStringDirect(string input)
    {
        _text = input;
        model.title = _text;
    }
}
