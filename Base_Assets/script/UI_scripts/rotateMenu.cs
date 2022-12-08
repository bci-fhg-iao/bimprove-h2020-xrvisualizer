using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rotateMenu : MonoBehaviour
{
  public GameObject rotateObject;
  public GameObject frontObject;
  public GameObject backObject;

  private Vector3 rotationFrom = new Vector3(0f, 0f, 0f);
  private Vector3 rotationTo = new Vector3(0f, 180f, 0f);

  public string activeView = "front";

  private bool startlerp = false;
  private float starttime = 0f;
  public float mySpeed = 1f;

  // Start is called before the first frame update
  void Start()
  {
        
  }

  // Update is called once per frame
  void Update()
  {
    if (startlerp)
    {
      starttime += Time.deltaTime * mySpeed;
      rotateObject.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(rotationFrom), Quaternion.Euler(rotationTo), starttime);
      if (starttime > 1)
      {
        startlerp = false;
      }
    }
  }

  public void changeActiveView()
  {
    Debug.Log("changeActiveView");
    if (activeView.Equals("front"))
    {
      rotationFrom = new Vector3(0f, 0f, 0f);
      rotationTo = new Vector3(0f, 180f, 0f);
      activeView = "back";
      frontObject.GetComponent<CanvasGroup>().alpha = 0;
      frontObject.GetComponent<CanvasGroup>().interactable = false;

      backObject.GetComponent<CanvasGroup>().alpha = 1;
      backObject.GetComponent<CanvasGroup>().interactable = true;

      starttime = 0f;
      startlerp = true;
    } else
    {
      rotationFrom = new Vector3(0f, 180f, 0f);
      rotationTo = new Vector3(0f, 360f, 0f);

      activeView = "front";
      frontObject.GetComponent<CanvasGroup>().alpha = 1;
      frontObject.GetComponent<CanvasGroup>().interactable = true;

      backObject.GetComponent<CanvasGroup>().alpha = 0;
      backObject.GetComponent<CanvasGroup>().interactable = false;

      starttime = 0f;
      startlerp = true;
    }
  }
}
