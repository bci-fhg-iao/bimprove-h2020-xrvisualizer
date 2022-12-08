using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class old_placeObj : MonoBehaviour
{
    public bool trigger = false;
    private bool ltrigger = false;
    public GameObject previewCube;

    private int triggerstate = 0;
    private Vector3 pos1 = Vector3.zero;
    private Vector3 pos2 = Vector3.zero;
    private Vector3 vectordir = Vector3.zero;
    private GameObject cube;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // smallstatemachine 
        // triggerstate = 0 = nothing
        // triggerstate = 1 = pressed
        // triggerstate = 2 = released
        if (triggerstate == 0 && trigger)
        {
            triggerstate = 1;
        }
        else if (triggerstate == 1 && !trigger)
        {
            triggerstate = 2;
        }
        else if (triggerstate == 2)
        {
            triggerstate = 0;
        }

        // What will happen on triggerstate 2
        if (triggerstate == 1)
        {
            // was just pressed yet
            if (!ltrigger)
            {
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = previewCube.transform.position;
                cube.transform.localScale = new Vector3(.02f, .02f, .02f);
                cube.name = "ShowCube";

                cube.GetComponent<BoxCollider>().enabled = false;

                pos1 = previewCube.transform.position;
            }

            // actualize cube preview
            pos2 = previewCube.transform.position;
            vectordir = pos2 - pos1;


            // calc position middle
            Vector3 newpos = pos1 + (vectordir / 2f);

            // actualize pos scale
            cube.transform.position = newpos;
            cube.transform.localScale = vectordir;
            cube.transform.rotation = Quaternion.LookRotation(vectordir);

        }


        Debug.Log("triggerstate == " + triggerstate + " // trigger == " + trigger);
        // save last state of trigger
        ltrigger = trigger;
    }








}



