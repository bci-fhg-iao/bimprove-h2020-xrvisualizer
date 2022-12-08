using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Normal.Realtime;
using System.Linq;
using UnityEngine.Events;

namespace Normal.Realtime
{
    public class ConnectedPlayerList : MonoBehaviour
    {
        public Text _displayField;
        private RealtimeAvatarManager _avatarManager;

        private void Awake()
        {
            _avatarManager = FindObjectOfType<RealtimeAvatarManager>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                RefreshPlayerCount();
            }
        }

        public void RefreshPlayerCount()
        {
            Debug.Log("registered " + _avatarManager.avatars.Count() + " players");
            _displayField.text = _avatarManager.avatars.Count().ToString();
        }
    }
}

