using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changePanel : MonoBehaviour
{
    public GameObject rotateObject;
    public GameObject panel_1;
    public GameObject panel_2;
    public GameObject panel_3;
    public GameObject panel_4;
    public GameObject panel_5;

    public void changeActiveView(int activeView)
    {

        switch (activeView)
        {
            case 1:
                panel_1.GetComponent<CanvasGroup>().alpha = 1;
                panel_1.GetComponent<CanvasGroup>().interactable = true;
                panel_1.GetComponent<CanvasGroup>().blocksRaycasts = true;

                panel_2.GetComponent<CanvasGroup>().alpha = 0;
                panel_2.GetComponent<CanvasGroup>().interactable = false;
                panel_2.GetComponent<CanvasGroup>().blocksRaycasts = false;

                panel_3.GetComponent<CanvasGroup>().alpha = 0;
                panel_3.GetComponent<CanvasGroup>().interactable = false;
                panel_3.GetComponent<CanvasGroup>().blocksRaycasts = false;

                panel_4.GetComponent<CanvasGroup>().alpha = 0;
                panel_4.GetComponent<CanvasGroup>().interactable = false;
                panel_4.GetComponent<CanvasGroup>().blocksRaycasts = false;

                panel_5.GetComponent<CanvasGroup>().alpha = 0;
                panel_5.GetComponent<CanvasGroup>().interactable = false;
                panel_5.GetComponent<CanvasGroup>().blocksRaycasts = false;
                break;

            case 2:
                panel_1.GetComponent<CanvasGroup>().alpha = 0;
                panel_1.GetComponent<CanvasGroup>().interactable = false;
                panel_1.GetComponent<CanvasGroup>().blocksRaycasts = false;

                panel_2.GetComponent<CanvasGroup>().alpha = 1;
                panel_2.GetComponent<CanvasGroup>().interactable = true;
                panel_2.GetComponent<CanvasGroup>().blocksRaycasts = true;

                panel_3.GetComponent<CanvasGroup>().alpha = 0;
                panel_3.GetComponent<CanvasGroup>().interactable = false;
                panel_3.GetComponent<CanvasGroup>().blocksRaycasts = false;

                panel_4.GetComponent<CanvasGroup>().alpha = 0;
                panel_4.GetComponent<CanvasGroup>().interactable = false;
                panel_4.GetComponent<CanvasGroup>().blocksRaycasts = false;

                panel_5.GetComponent<CanvasGroup>().alpha = 0;
                panel_5.GetComponent<CanvasGroup>().interactable = false;
                panel_5.GetComponent<CanvasGroup>().blocksRaycasts = false;
                break;

            case 3:
                panel_1.GetComponent<CanvasGroup>().alpha = 0;
                panel_1.GetComponent<CanvasGroup>().interactable = false;
                panel_1.GetComponent<CanvasGroup>().blocksRaycasts = false;

                panel_2.GetComponent<CanvasGroup>().alpha = 0;
                panel_2.GetComponent<CanvasGroup>().interactable = false;
                panel_2.GetComponent<CanvasGroup>().blocksRaycasts = false;

                panel_3.GetComponent<CanvasGroup>().alpha = 1;
                panel_3.GetComponent<CanvasGroup>().interactable = true;
                panel_3.GetComponent<CanvasGroup>().blocksRaycasts = true;
                panel_3.GetComponentInChildren<Dropdown>().value = 0;

                panel_4.GetComponent<CanvasGroup>().alpha = 0;
                panel_4.GetComponent<CanvasGroup>().interactable = false;
                panel_4.GetComponent<CanvasGroup>().blocksRaycasts = false;

                panel_5.GetComponent<CanvasGroup>().alpha = 0;
                panel_5.GetComponent<CanvasGroup>().interactable = false;
                panel_5.GetComponent<CanvasGroup>().blocksRaycasts = false;
                break;

            case 4:
                panel_1.GetComponent<CanvasGroup>().alpha = 0;
                panel_1.GetComponent<CanvasGroup>().interactable = false;
                panel_1.GetComponent<CanvasGroup>().blocksRaycasts = false;

                panel_2.GetComponent<CanvasGroup>().alpha = 0;
                panel_2.GetComponent<CanvasGroup>().interactable = false;
                panel_2.GetComponent<CanvasGroup>().blocksRaycasts = false;

                panel_3.GetComponent<CanvasGroup>().alpha = 0;
                panel_3.GetComponent<CanvasGroup>().interactable = false;
                panel_3.GetComponent<CanvasGroup>().blocksRaycasts = false;

                panel_4.GetComponent<CanvasGroup>().alpha = 1;
                panel_4.GetComponent<CanvasGroup>().interactable = true;
                panel_4.GetComponent<CanvasGroup>().blocksRaycasts = true;

                panel_5.GetComponent<CanvasGroup>().alpha = 0;
                panel_5.GetComponent<CanvasGroup>().interactable = false;
                panel_5.GetComponent<CanvasGroup>().blocksRaycasts = false;
                break;


            case 5:
                panel_1.GetComponent<CanvasGroup>().alpha = 0;
                panel_1.GetComponent<CanvasGroup>().interactable = false;
                panel_1.GetComponent<CanvasGroup>().blocksRaycasts = false;

                panel_2.GetComponent<CanvasGroup>().alpha = 0;
                panel_2.GetComponent<CanvasGroup>().interactable = false;
                panel_2.GetComponent<CanvasGroup>().blocksRaycasts = false;

                panel_3.GetComponent<CanvasGroup>().alpha = 0;
                panel_3.GetComponent<CanvasGroup>().interactable = false;
                panel_3.GetComponent<CanvasGroup>().blocksRaycasts = false;

                panel_4.GetComponent<CanvasGroup>().alpha = 0;
                panel_4.GetComponent<CanvasGroup>().interactable = false;
                panel_4.GetComponent<CanvasGroup>().blocksRaycasts = false;

                panel_5.GetComponent<CanvasGroup>().alpha = 1;
                panel_5.GetComponent<CanvasGroup>().interactable = true;
                panel_5.GetComponent<CanvasGroup>().blocksRaycasts = true;
                break;

        }
    }
}

