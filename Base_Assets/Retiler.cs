using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Retiler : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public bool isHorizontal = false;
    public Vector3 lastValues;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void FixedUpdate()
    {
        if (transform.parent.localScale != lastValues)
        {
            if (transform.parent.transform.localScale.x == 0.02f)
            {
                meshRenderer.material.mainTextureScale = new Vector2(Math.Abs(transform.parent.localScale.z), Math.Abs(transform.parent.localScale.y));
            }
            else if (transform.parent.localScale.y == 0.02f)
            {
                meshRenderer.material.mainTextureScale = new Vector2(Math.Abs(transform.parent.localScale.x), Math.Abs(transform.parent.localScale.z));
            }

            lastValues = transform.parent.localScale;
        }
    }
}
