using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ext_gameOBject
{
    // erzeugt transparente Kopien aller Materialien im Game-Object und setzt die Transparenz auf einen festen Wert
    public static void makeTransparent(this GameObject obj, float transparency)
    {
        if (obj != null)
        {
            Renderer myRenderer = null;
            Material myMaterial = null;
            Color myColor = new Color();

            foreach (Transform childTrans in obj.GetComponentsInChildren<Transform>(true)) //include inactive
            {
                myRenderer = childTrans.GetComponent<Renderer>();

                if (myRenderer != null) //Wenn Geometrie-Knoten
                {
                    //int matSize = myRenderer.materials.Length;
                    int matSize = myRenderer.sharedMaterials.Length;
                    if (matSize > 0)
                    {
                        Material[] newMaterials = new Material[matSize];
                        for (int i = 0; i < matSize; i++)
                        {
                            //myMaterial = Instantiate(myRenderer.materials[i]);
                            myMaterial = Material.Instantiate(myRenderer.sharedMaterials[i]);
                            myMaterial.name = "transp_" + myMaterial.name;
                            myMaterial.ToFadeMode();

                            myColor = myMaterial.color;
                            myColor.a = transparency;
                            myMaterial.color = myColor;

                            newMaterials[i] = myMaterial;
                        }

                        //myRenderer.materials = newMaterials;
                        myRenderer.sharedMaterials = newMaterials;
                    }
                }
            }
        }
    }

    public static void setShadows(this GameObject obj, bool cast_shadows, bool receive_shadows)
    {
        if (obj != null)
        {
            string obj_name;
            Renderer myRenderer = null;

            foreach (Transform childTrans in obj.GetComponentsInChildren<Transform>(true)) //include inactive
            {
                obj_name = childTrans.gameObject.name;
                myRenderer = childTrans.GetComponent<Renderer>();

                if (myRenderer != null) //Wenn Geometrie-Knoten
                {
                    myRenderer.receiveShadows = receive_shadows;
                    if (cast_shadows)
                        myRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    else
                        myRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
            }
        }
        else
        {
            Debug.Log("ERROR [model_components->setShadows]: GameObject is null");
        }
    }
}