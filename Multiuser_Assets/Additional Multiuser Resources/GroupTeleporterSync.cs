using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class GroupTeleporterSync : RealtimeComponent<GroupTeleporterSyncModel>
{
    public int changeInt;
    public Vector3 position;


    public Transform my_Head;
    public Transform my_CameraRig;

    private Vector3 my_LookVector;
    private Vector3 my_TeleportPosition;

    private IEnumerator DelayedPositionSwap()
    {
        Debug.Log("trying to massjump");
        GameObject.FindGameObjectWithTag("TransitionScreen").GetComponent<Animation>().Play("FadeScreen");
        yield return new WaitForSeconds(0.1f);

        my_Head = GameObject.FindGameObjectWithTag("MainCamera").transform;
        my_CameraRig = GameObject.FindGameObjectWithTag("Player").transform;

        // look vector - normalized to length of 1 meter
        my_LookVector = Vector3.Normalize(my_Head.forward);

        // teleport to Object dependent on your look direction
        my_TeleportPosition = new Vector3(model.xPos, model.yPos, model.zPos) - my_LookVector;
        // get rid of local head position in tracking space
        my_TeleportPosition -= my_Head.localPosition;

        // teleport
        my_CameraRig.position = my_TeleportPosition;
        Debug.Log("finished massjump");
    }

    protected override void OnRealtimeModelReplaced(GroupTeleporterSyncModel previousModel, GroupTeleporterSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.changeIntDidChange -= IntStateHasChanged;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
                currentModel.changeInt = changeInt;


            UpdateIntegerState();

            currentModel.changeIntDidChange += IntStateHasChanged;
        }
    }

    private void IntStateHasChanged(GroupTeleporterSyncModel model, int value)
    {
        UpdateIntegerState();
    }

    private void UpdateIntegerState()
    {
        changeInt = model.changeInt;
        position = new Vector3(model.xPos, model.yPos, model.zPos);
        StartCoroutine(DelayedPositionSwap());
    }

    public void SetChangeInt(int integer, Vector3 position, Vector3 target)
    {
        model.xPos = position.x;
        model.yPos = position.y;
        model.zPos = position.z;
        model.target = target;
        model.changeInt = model.changeInt + integer;
    }
}
