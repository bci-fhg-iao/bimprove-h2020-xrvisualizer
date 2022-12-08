// changes transparency of given Transparent-Material
// keys: + -
// on exit: restores original value of transparency

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class change_trans : MonoBehaviour
{
    public Material m_material;
    public float m_delta_quotient = 25.0f;
    Color m_org_color;

    // Use this for initialization
    void Start()
    {
        if (m_material != null)
        {
            Color myColor = m_material.color;

            if (myColor != null)
            {
                m_org_color = m_material.color;
            }
        }
    }

    void OnDestroy()
    {
        if (m_material != null && m_org_color != null)
        {
            m_material.color = m_org_color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_material != null)
        {
            if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
                deltaTransp(-1);
            else if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
                deltaTransp(+1);
        }

    }

    void deltaTransp(float delta) //
    {
        if (m_material != null)
        {
            Color myColor = m_material.color;
            float alpha = 0.0f;

            if (myColor != null)
            {
                alpha = m_material.color.a + delta / m_delta_quotient;
                Debug.Log("Old: " + m_material.color.a);

                if (alpha < 0)
                    alpha = 0.0f;
                else if (alpha > 1)
                    alpha = 1.0f;
            }

            myColor.a = alpha;
            m_material.color = myColor;
            Debug.Log("New: " + alpha);
        }
    }
}
