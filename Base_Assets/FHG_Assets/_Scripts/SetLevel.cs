using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLevel : MonoBehaviour {


 	[SerializeField]
	private GameObject vrCamRig;
	public List<Transform> levels;
	public int currentLevel = 0;
	// Use this for initialization
	void Start () {
		vrCamRig = GameObject.Find("[CameraRig]_2");
	}
	
	// Update is called once per frame
	void Update () {
		SetLevels();
	}

	void SetLevels()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if(currentLevel==4)
			{
				currentLevel = 0;
			}
			else {
				currentLevel++;
			}
			vrCamRig.transform.position = new Vector3(vrCamRig.transform.position.x, levels[currentLevel].position.y, vrCamRig.transform.position.z);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			if (currentLevel == 0)
			{
				currentLevel = 4;
			}
			else
			{
				currentLevel--;
			}
			vrCamRig.transform.position = new Vector3(vrCamRig.transform.position.x, levels[currentLevel].position.y, vrCamRig.transform.position.z);
		}
	}
}
