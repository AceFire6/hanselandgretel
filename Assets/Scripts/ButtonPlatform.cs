using UnityEngine;
using System.Collections;

public class ButtonPlatform : MonoBehaviour {

	public float yDepression = 0.02f;
	public float height = 0.03f;
	private Vector3 unpressedPos;
	private Vector3 pressedPos;

	private GameObject button;

	// Use this for initialization
	void Start ()
	{
		button = transform.parent.gameObject;
		unpressedPos = button.transform.position;
		pressedPos = unpressedPos;
		pressedPos.y -= yDepression;
	}

	float timer = 0.0f;
	// Update is called once per frame
	void Update ()
	{
//		timer+= Time.deltaTime;
//		if(timer > 2.0f){
//			transform.position =  unpressedPos;
//			timer = 0;
//		}
	
	}

	//onStay instead?
	void OnTriggerEnter (Collider other)
	{
		Debug.Log ("Yello");
		if(other.gameObject.tag == "Player")
		{		button.transform.position = pressedPos;
			Vector3 newPos = other.gameObject.transform.position;
			newPos.y += height;
			other.gameObject.transform.position = newPos;
		}
	}

	void OnTriggerExit (Collider other)
	{
		Debug.Log ("Leave");
		if(other.gameObject.tag == "Player"){
			button.transform.position =  unpressedPos;
			Vector3 newPos = other.gameObject.transform.position;
			newPos.y -= height;
			other.gameObject.transform.position = newPos;
		}
	}
}
