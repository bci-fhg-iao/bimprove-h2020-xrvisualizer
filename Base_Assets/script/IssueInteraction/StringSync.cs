using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;

public class StringSync : RealtimeComponent<StringSyncModel>
{
    public string _text;
    public Text _textDisplay;
    public InputField _textInput;

    protected override void OnRealtimeModelReplaced(StringSyncModel previousModel, StringSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.textDidChange -= StringDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
                currentModel.text = _text;

            UpdateString();

            currentModel.textDidChange += StringDidChange;
        }
    }

    private void StringDidChange(StringSyncModel model, string value)
    {
        UpdateString();
    }

    private void Start()
    {
        if (_text != "")
        {
            _textDisplay.text = _text;
            Debug.Log("text refreshed");
        }
    }

    private void UpdateString()
    {
        _text = model.text;
        _textDisplay.text = _textDisplay.text + "\n" + _text;
    }

    public void SetString()
    {
        _text = _textInput.text;
        model.text = _text;
    }
}
