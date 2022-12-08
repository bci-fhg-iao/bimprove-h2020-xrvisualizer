using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIDAssigner : MonoBehaviour
{
    private string guid;

    void Start()
    {
        StartCoroutine(SetState());
    }

    private IEnumerator SetState()
    {
        yield return new WaitForSeconds(1f);
        if (transform.GetComponent<TitleSync>()._text == "")
        {
            guid = System.Guid.NewGuid().ToString();
            transform.GetComponent<TitleSync>().SetStringDirect(guid);
        }
    }
}
