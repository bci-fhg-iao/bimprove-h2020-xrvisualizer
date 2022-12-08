using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;


public class CoolEventHelper : MonoBehaviour
{
    public RealtimeAvatarManager _avatarManager;
    public ConnectedPlayerList _connectedPLayer;
    public LoginManager _loginManager;
    public NameSync _nameSync;

    public void SetNameEvent(string Name)
    {
        Debug.Log(_loginManager._playerID);
        _nameSync.SetName(Name);
    }

    private void Awake()
    {
        _avatarManager = FindObjectOfType<RealtimeAvatarManager>();
        _connectedPLayer = FindObjectOfType<ConnectedPlayerList>();
        _loginManager = FindObjectOfType<LoginManager>();

        _connectedPLayer.RefreshPlayerCount();
    }

    private void Start()
    {
        _connectedPLayer.RefreshPlayerCount();
    }

    private void OnDestroy()
    {
        _connectedPLayer.RefreshPlayerCount();
    }
}
