using UnityEngine;
using Normal.Realtime;

namespace Normal.Realtime
{
    [RequireComponent(typeof(RealtimeAvatarVoice))]
    public class RealtimeAvatarVoiceScaleCustom : MonoBehaviour
    {
        private RealtimeAvatarVoice _voice;

        void Awake()
        {
            // Get a reference to the RealtimeAvatarVoice component
            _voice = GetComponent<RealtimeAvatarVoice>();
        }

        void Update()
        {
            // Get the voice volume
            float voiceVolume = _voice.voiceVolume;

            // Use the voice volume to calculate the scale of our head (between 1.0f and 4.0f)
            float scale = 1.0f + voiceVolume * 3.5f;

            // Apply the scale to the this game object
            transform.localScale = new Vector3(1, scale, 1);
        }
    }
}

