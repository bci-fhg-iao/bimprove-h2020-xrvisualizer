using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;
using Unity.Linq;

public class VoiceChatDisable : MonoBehaviour
{
    public AudioListener _audioListener;
    private Transform head;
    private RealtimeAvatarVoice localVoice;
    private Realtime realtime;
    private RealtimeAvatarManager avatarManager;

    private bool isOnline = false;

    public bool selfLock = false;
    public bool othersLock = false;
    public bool fullLock = false;

    public Toggle _selfSound;
    public Toggle _othersSound;
    public Toggle _fullSound;

    private void Start()
    {
        realtime = FindObjectOfType<Realtime>();
        avatarManager = FindObjectOfType<RealtimeAvatarManager>();
    }

    IEnumerator WaitForConnect()
    {
        yield return new WaitForSeconds(2f);
        head = FindObjectOfType<RealtimeAvatarManager>().localAvatar.head;
        localVoice = head.GetComponentInChildren<RealtimeAvatarVoice>();
        isOnline = true;
    }

    private void Update()
    {
        if (realtime.connected && !isOnline)
        {
            StartCoroutine(WaitForConnect());
        }
    }

    public void ToggleSound()
    {
        fullLock = !fullLock;

        ToggleSoundSelf();
        ToggleSoundOthers();

        _selfSound.interactable = !_selfSound.interactable;
        _othersSound.interactable = !_othersSound.interactable;
    }

    public void ToggleSoundSelf()
    {
        localVoice.mute = !localVoice.mute;
        selfLock = !selfLock;

        if (selfLock && !fullLock)
        {
            _fullSound.interactable = false;
        }
        else if (othersLock && !fullLock)
        {
            _fullSound.interactable = false;
        }
        else if (!othersLock && !selfLock && !fullLock)
        {
            _fullSound.interactable = true;
        }
    }

    public void ToggleSoundOthers()
    {

        AudioOutput[] audioSources;
        audioSources = GameObject.FindObjectsOfType<AudioOutput>();

        foreach (AudioOutput source in audioSources)
        {
            source.enabled = !source.enabled;
        }

        othersLock = !othersLock;

        if (selfLock && !fullLock)
        {
            _fullSound.interactable = false;
        }
        else if (othersLock && !fullLock)
        {
            _fullSound.interactable = false;
        }
        else if (!othersLock && !selfLock && !fullLock)
        {
            _fullSound.interactable = true;
        }
    }
}
