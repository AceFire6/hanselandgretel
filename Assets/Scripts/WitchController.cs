using UnityEngine;
using System.Collections;

public class WitchController : MonoBehaviour {

	public float rightMargin = 0.5f; //How far from the right edge of the screen the witch will fly

	public Transform fleeDestination; //where the witch will flee to when she "retreats to the house"
	public float fleeSpeed = 0.5f;

	private Camera cam;
	private float depth;

	private bool isFleeing = false; //When se4t to true, the witch will flee to the house & disappear

	void Start () {
		cam = Camera.main;
		depth = transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {


		if (isFleeing) {
			//just move towards the flee Point at fleeSpeed
			Vector3 pos = transform.position;
			transform.position = Vector3.Lerp(pos, fleeDestination.position, fleeSpeed * Time.deltaTime);
		} else {
			//Work out where the witch should be relative to the camera
			float rightBorderPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, depth)).x;
			float xPos = rightBorderPos - rightMargin;
			
			//make sure the witch is only moving forward
			float oldX = transform.position.x;
			xPos = Mathf.Max (xPos, oldX);
			
			//set the Witch's xPos to the newly calculated x position.
			Vector3 pos = transform.position;
			pos.x = xPos;
			transform.position = pos;
		}
	}

	public void Flee() {
		isFleeing = true;
	}
}
