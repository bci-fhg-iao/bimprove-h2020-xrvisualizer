using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IssueButtonBehaviour : MonoBehaviour
{
    public GameObject issue;
    private bool coroutineFinished = true;
    private bool coroutineFinished2 = true;

    public Transform my_Head;
    public Transform my_CameraRig;

    private Vector3 my_LookVector;
    private Vector3 my_TeleportPosition;

    public void JumpToIssue()
    {
        if(coroutineFinished == true)
        {
            GameObject.FindGameObjectWithTag("TransitionScreen").GetComponent<Animation>().Play("FadeScreen");
            StartCoroutine(LookAtDelayed());
            coroutineFinished = false;
        }
    }

    private IEnumerator LookAtDelayed()
    {
        yield return new WaitForSeconds(0.05f);
        //GameObject.FindGameObjectWithTag("Player").transform.position = issue.GetComponent<IssueBehaviour>().markerPosition.transform.position;

        my_Head = GameObject.FindGameObjectWithTag("MainCamera").transform;
        my_CameraRig = GameObject.FindGameObjectWithTag("Player").transform;

        // look vector - normalized to length of 1 meter
        my_LookVector = Vector3.Normalize(my_Head.forward);

        // teleport to Object dependent on your look direction
        my_TeleportPosition = issue.GetComponent<IssueBehaviour>().markerPosition.transform.position - my_LookVector;
        // get rid of local head position in tracking space
        my_TeleportPosition -= my_Head.localPosition;

        // teleport
        my_CameraRig.position = my_TeleportPosition;

        coroutineFinished = true;
    }

    public void MassJumpToIssue()
    {
        if (coroutineFinished2 == true)
        {
            GameObject.FindGameObjectWithTag("TransitionScreen").GetComponent<Animation>().Play("FadeScreen");
            StartCoroutine(DelayedMassJump());
            coroutineFinished2 = false;
        }
    }

    private IEnumerator DelayedMassJump()
    {
        var pos = issue.GetComponent<IssueBehaviour>().markerPosition.transform.position;
        GameObject.Find("Issue Manager").GetComponent<GroupTeleporterSync>().SetChangeInt(1, pos, issue.GetComponent<IssueBehaviour>().markerPosition.transform.position);
        yield return new WaitForSeconds(1f);
        coroutineFinished2 = true;
    }
}
