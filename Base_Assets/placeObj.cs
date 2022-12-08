using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;

public class placeObj : MonoBehaviour
{
    public bool trigger = false;
    private bool ltrigger = false;
    public GameObject previewCube;
    public Transform scaleCube;
    public Transform refEmpty;
    private Vector3 cubeRelative;
    public GameObject prefabCube;
    public GraphicRaycaster graphicRaycaster;
    public MonoBehaviour raycastTarget;
    private int triggerstate = 0;
    private Vector3 pos1 = Vector3.zero;
    private Vector3 pos2 = Vector3.zero;
    private Vector3 vectordir = Vector3.zero;
    private GameObject cube;

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Triggerstate == " + triggerstate);
        // smallstatemachine 
        // triggerstate = 0 = nothing
        // triggerstate = 1 = horizontal line
        // triggerstate = 2 = scale XY
        if (triggerstate == 0 && trigger && trigger != ltrigger)
        {
            graphicRaycaster.enabled = false;
            if(raycastTarget != null)
            {
                raycastTarget.enabled = false;
            }

            triggerstate = 1;
        } else if (triggerstate == 1 && trigger && trigger != ltrigger)
        {
            triggerstate = 2;
        } else if (triggerstate == 2 && trigger && trigger != ltrigger)
        {
            if (triggerstate != 0)
            {
                cube.transform.GetComponentInChildren<NetIssueBehaviour>().DecoupleObject();
                graphicRaycaster.enabled = true;
                if (raycastTarget != null)
                {
                    raycastTarget.enabled = true;
                }
            }
            triggerstate = 0;
        }

        // What will happen on triggerstate 2
        if (triggerstate == 1)
        {
            // was just pressed yet
            if (ltrigger && !trigger)
            {
                createCube();
            }
            // was just released yet
            else if (!trigger)
            {
                // horizontal or vertical placement method
                placeObjHorizontal();
            }
        }
        else if (triggerstate == 2)
        {
            scaleObjXY();
        }
        //Debug.Log("triggerstate == " + triggerstate + " // trigger == " + trigger);
        // save last state of trigger
        ltrigger = trigger;
    }

    void createCube ()
    {
        /*
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = previewCube.transform.position;
        cube.transform.localScale = new Vector3(.02f, .02f, .02f);
        cube.name = "RefCube";
        cube.GetComponent<BoxCollider>().enabled = false;
        */

        //cube = Instantiate(prefabCube, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        cube = 
            Realtime.Instantiate("SafetyIssue",
            transform.position,
            Quaternion.identity,
            ownedByClient: true,
            preventOwnershipTakeover: false,
            destroyWhenOwnerOrLastClientLeaves: false,
            useInstance: null);

        cube.transform.position = previewCube.transform.position;
        cube.name = "SafetyIssue";
        refEmpty = cube.transform.GetChild(0);
        scaleCube = cube.transform.GetChild(1);
        scaleCube.transform.localScale = new Vector3(.02f, .02f, .02f);

        pos1 = previewCube.transform.position;

        cube.GetComponentInChildren<IssueBehaviour>().initialState = true;
        // set ray
    }
    void placeObjHorizontal()
    {
        //
        // actualize cube preview
        pos2 = previewCube.transform.position;
        vectordir = pos1 - pos2;

        // calc position middle
        //Vector3 newpos = pos1 + (vectordir / 2f);
        //newpos.y = pos1.y;

        // direction vector without Y (height)
        Vector3 vectordirNew = vectordir;
        vectordirNew.y = 0f;

        // rangeXZ == scale in Z direction // calculte newScale vector for Object
        float rangeXZ = Vector3.Distance(Vector3.zero, vectordirNew);
        Vector3 newScale = new Vector3(0.02f, 0.02f, rangeXZ);

        // actualize pos scale
        cube.transform.rotation = Quaternion.LookRotation(vectordirNew);
        //cube.transform.position = newpos;
        scaleCube.transform.localScale = newScale;
    }
    void scaleObjXY ()
    {
        refEmpty.position = previewCube.transform.position;
        cubeRelative = refEmpty.localPosition;
        //Debug.Log("cubeRelative = " + cubeRelative);

        float distanceXZ = cubeRelative.x;
        float distanceY = cubeRelative.y;
        //Debug.Log("distanceXZ = " + distanceXZ + " // distanceY = " + distanceY);


        if (distanceY * distanceY > distanceXZ* distanceXZ)
        {
            Vector3 newScale = scaleCube.transform.localScale;
            newScale.x = 0.02f;
            newScale.y = distanceY;
            scaleCube.transform.localScale = newScale;
        } else
        {
            Vector3 newScale = scaleCube.transform.localScale;
            newScale.x = distanceXZ* (-1);
            newScale.y = 0.02f;
            scaleCube.transform.localScale = newScale;
        }

    }

}



