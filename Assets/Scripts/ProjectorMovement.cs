using UnityEngine;
using System.Collections;

public class ProjectorMovement : MonoBehaviour {

	public Transform followObject;
	public float startHeight = 5.131f;

	private float height;
	private float heightDiff;
	
	private AIHouse ai;

	void Start () 
	{
		height = startHeight;
		ai = followObject.GetComponent<AIHouse> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		heightDiff = ai.heightDiff;
		Vector3 pos = followObject.position;
		height += -1f * heightDiff * 0.4f;
		transform.position = new Vector3 (pos.x,height,pos.z);
	}


}
