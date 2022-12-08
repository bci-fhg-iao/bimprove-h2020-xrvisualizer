using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Normal.Realtime;

public class LoginManager : MonoBehaviour
{
    public InputField _nameInput;
    public InputField _roomName;
    public Realtime _realtime;
    public string _roomID;
    public string _playerID;
    private GameObject _building;
    public Text _displayRoomName;
    public GameObject _extras;
    public GameObject _disableGroup;
    public GameObject _prePlayer;
    public GameObject _vrPlayer;

    private void Awake()
    {
        _building = GameObject.Find("building");
    }

    public void OfflineMode()
    {
        _building.GetComponent<RealtimeView>().enabled = false;
        _building.GetComponent<RealtimeTransform>().enabled = false;
        _disableGroup.gameObject.SetActive(false);
        _prePlayer.SetActive(false);
        _vrPlayer.SetActive(true);
        if (_extras != null)
        {
            _extras.SetActive(false);
        }
    }

    public void Connect()
    {
        if (_nameInput.text != "" && _roomName.text != "")
        {
            _realtime.Connect(_roomID, null);

            _disableGroup.gameObject.SetActive(false);
            _prePlayer.SetActive(false);
            _vrPlayer.SetActive(true);
        }
    }

    public void NameInputSet()
    {
        _playerID = _nameInput.text;
    }

    public void RoomInputSet()
    {
        _roomID = _roomName.text;
        _displayRoomName.text = _roomName.text;
    }
}


