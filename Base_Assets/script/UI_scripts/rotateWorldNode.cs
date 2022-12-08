using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rotateWorldNode : MonoBehaviour
{
  public Transform rotationTransformNode;
  // Start is called before the first frame update
  void Start()
  {
        
  }

  // Update is called once per frame
  void Update()
  {
        
  }
  public void rotateNodeY()
  {
    rotationTransformNode.Rotate(0f, 90f,0f, Space.Self); 
  }
  public void rotateNodeX()
  {
    rotationTransformNode.Rotate(90f, 0f, 0f, Space.Self);
  }
  public void rotateNodeZ()
  {
    rotationTransformNode.Rotate( 0f, 0f, 90f, Space.Self);
  }
  public void changeNodeX(float posXchange)
  {
    Vector3 posOffset = new Vector3(posXchange, 0f, 0f);
    rotationTransformNode.position += posOffset;
  }
  public void changeNodeY(float posYchange)
  {
    Vector3 posOffset = new Vector3(0f, posYchange, 0f);
    rotationTransformNode.position += posOffset;
  }
  public void changeNodeZ(float posZchange)
  {
    Vector3 posOffset = new Vector3(0f, 0f, posZchange);
    rotationTransformNode.position += posOffset;
  }
}
