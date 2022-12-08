using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarVisualOptions : MonoBehaviour
{
    public Material baseMat;
    public Material featureMat;

    private bool state = false;

    public void ChangeVisibilityState()
    {
        if(!state)
        {
            baseMat.SetOverrideTag("RenderType", "Transparent");
            baseMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            baseMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            baseMat.SetInt("_ZWrite", 0);
            baseMat.DisableKeyword("_ALPHATEST_ON");
            baseMat.EnableKeyword("_ALPHABLEND_ON");
            baseMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            baseMat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            featureMat.SetOverrideTag("RenderType", "Transparent");
            featureMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            featureMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            featureMat.SetInt("_ZWrite", 0);
            featureMat.DisableKeyword("_ALPHATEST_ON");
            featureMat.EnableKeyword("_ALPHABLEND_ON");
            featureMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            featureMat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            state = true;
        }
        else if (state)
        {
            baseMat.SetOverrideTag("RenderType", "");
            baseMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            baseMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            baseMat.SetInt("_ZWrite", 1);
            baseMat.DisableKeyword("_ALPHATEST_ON");
            baseMat.DisableKeyword("_ALPHABLEND_ON");
            baseMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            baseMat.renderQueue = -1;

            featureMat.SetOverrideTag("RenderType", "");
            featureMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            featureMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            featureMat.SetInt("_ZWrite", 1);
            featureMat.DisableKeyword("_ALPHATEST_ON");
            featureMat.DisableKeyword("_ALPHABLEND_ON");
            featureMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            featureMat.renderQueue = -1;

            state = false;
        }

    }
}
