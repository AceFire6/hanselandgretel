using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {

	public Transform arrow;
	private Vector3 positionOffset;

	void Start () 
	{
		positionOffset = new Vector3 (0, 0.45f, 0);
	}
	

	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.M)) 
		{
			positionOffset.x = transform.forward.x * 0.01f;
			Instantiate (arrow, transform.position + positionOffset,transform.rotation);
		}

	}
}
