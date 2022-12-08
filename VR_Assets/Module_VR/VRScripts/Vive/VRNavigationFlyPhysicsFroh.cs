using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class VRNavigationFlyPhysicsFroh : MonoBehaviour
{
  public Transform inputDeviceTransform;
  public RealtimeAvatarManager avatarManager;
  public Realtime realtime;
  public Transform mainTransform;

  // walk
  private Rigidbody flyRigidBody;
  private CapsuleCollider flyCollider;
  public float flySpeed = 1f;

  public float rigidBodyMass = 1f;
  public float rigidBodyDrag = 2f;
  public float maxVelocityFactor = 4f;

  public float colliderHeight = 1.8f;
  public float colliderRadius = 0.4f;

  public bool trigger = false;
  private bool isMoving = false;
  public GameObject cylinder;
  public float cylinderHeight = .2f;

  private bool nameSet = false;

  private DeviceSync deviceSync;
  // Use this for initialization
  void Start()
  {
    ////////////////////////////
    // add walk
    flyRigidBody = this.gameObject.AddComponent<Rigidbody>();
    flyRigidBody.useGravity = false;
    flyRigidBody.isKinematic = false;
    flyRigidBody.angularDrag = 0;
    flyRigidBody.freezeRotation = true;
    flyRigidBody.mass = rigidBodyMass;
    // set collider
    flyCollider = this.gameObject.AddComponent<CapsuleCollider>();
    flyCollider.center = new Vector3(0, 0.8f, 0);
    flyCollider.radius = colliderRadius;
    flyCollider.height = colliderHeight;
    // create cylinder for stand
    /*
    cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
    cylinder.GetComponent<Collider>().enabled = false;
    cylinder.transform.parent = this.transform;
    cylinder.transform.localPosition= new Vector3(0f, cylinderHeight / 2f, 0f);
    cylinder.transform.localScale = new Vector3(colliderRadius * 2f, cylinderHeight, colliderRadius * 2f);
    */
  }


  private IEnumerator NameCoroutine()
  {
    nameSet = true;
    yield return new WaitForSeconds(1f);

    GameObject mAvatar = avatarManager.localAvatar.gameObject;
    LoginManager mLogin = FindObjectOfType<LoginManager>();
    mAvatar.GetComponent<CoolEventHelper>().SetNameEvent(mLogin._playerID);
    mainTransform.rotation = Quaternion.identity;
    }

    // Navigation with physics
  void FixedUpdate()
  {
    //Debug.Log(navigationModes.Count);
    flyNavigation();

    if (realtime.connected && nameSet == false)
    {
      StartCoroutine(NameCoroutine());
    }
  }

  void flyNavigation()
  {
    if (trigger)
    {
      if (!isMoving)
      {
        flyCollider.enabled = true;
        flyRigidBody.drag = 0f;
      }
      Vector3 pointDirection = inputDeviceTransform.forward;
      float movementSpeed = Vector3.Distance(Vector3.zero, flyRigidBody.velocity);
      if (movementSpeed < maxVelocityFactor * flySpeed)
      {
        flyRigidBody.AddForce(pointDirection * flySpeed);
      }
      else
      {
        // although we reach this it is not needed somehow !!!!
        //Debug.Log("max speed has been reached -- activate drag");
        flyRigidBody.drag = rigidBodyDrag/100f;
      }
    }
    else
    {
      flyRigidBody.drag = rigidBodyDrag;
      isMoving = false;
      if (flyRigidBody.velocity.magnitude < 0.01)
      {
        //Debug.Log("unity answers saves the day!");
        flyCollider.enabled = false;
      }
    }
  }
}
