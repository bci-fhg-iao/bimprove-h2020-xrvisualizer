using UnityEngine;
using System.Collections;

public class metadata : MonoBehaviour
{

    public string[] keys;
    public string[] values;

    public string getID()
    {
        if (keys.Length > 19 && keys[19] == "Id")
        {
            return keys[19];
        }
        else
            return "no_ID";

    }
}