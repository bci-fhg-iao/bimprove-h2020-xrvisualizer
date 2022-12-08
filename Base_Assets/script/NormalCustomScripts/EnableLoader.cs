using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

namespace TriLib
{
    namespace Samples
    {
        public class EnableLoader : MonoBehaviour
        {
            public GameObject _loader;
            public Realtime _realtime;
            private JF_PersistentDataPathLoadSampleMultiuser script;
            private bool realtimeSet = false;

            public void Start()
            {
                script = _loader.GetComponent<JF_PersistentDataPathLoadSampleMultiuser>();
            }

            public void Update()
            {
                if (script.loadingOperationFinished == true && script.enabled == true && realtimeSet == false)
                {
                    _realtime.enabled = true;
                    realtimeSet = true;
                }
                
            }
        }
    }

}

