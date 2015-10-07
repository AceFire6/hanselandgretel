using UnityEngine;
using System.Collections;

public class ProjectMovement : MonoBehaviour {

	public Transform followObject;
	public float startHeight = 5.131f;
	private float height;

	void Start () 
	{
		height = startHeight;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 pos = followObject.position;
		transform.position = new Vector3 (pos.x,height,pos.z);
	}
}
