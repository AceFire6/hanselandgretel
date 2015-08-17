using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	public float speed = 3.0f;
	public float rotSpeed = 250.0f;

	private Vector3 deltaMovement;
	private Vector3 rotation;
	private Animator objAnimator;
	private Rigidbody objRigidbody;
	
	void Start () {
		objAnimator = GetComponent<Animator> ();
		objRigidbody = GetComponent<Rigidbody> ();
		rotation = objRigidbody.rotation.eulerAngles;
	}

	void Update () {
		//float h = Input.GetAxisRaw ("Horizontal");
		//float v = Input.GetAxisRaw ("Vertical");
		
		Move (deltaMovement.x, deltaMovement.y);
		Rotate (rotation.x, rotation.y);
		Animate (deltaMovement.x, deltaMovement.y);
	}

	public void SetDeltaMovement(float x, float y){
		deltaMovement = new Vector3(x, y, 0.0f);
	}

	//todo: make this use a public var as well
	public void SetDeltaRotation(float x, float y){
		rotation += new Vector3(x, y, 0.0f);

		if(rotation.x > 360){
			rotation.x -= 360;
		}
		if(rotation.y > 360){
			rotation.y -= 360;
		}
	}

	void Rotate(float x, float y){
		if(objRigidbody.rotation.eulerAngles != rotation){
			Quaternion rot = Quaternion.Lerp(objRigidbody.rotation, Quaternion.Euler(rotation), rotSpeed);
			objRigidbody.MoveRotation ( rot);
		}
	}

	void Move(float horizontal, float vertical){
		deltaMovement.Set (horizontal, vertical, 0.0f);
		deltaMovement = deltaMovement.normalized * speed * Time.deltaTime;

		objRigidbody.MovePosition (transform.position + deltaMovement);
	}

	void Animate(float horizontal, float vertical) {
		objAnimator.SetBool ("isFlying", horizontal != 0);
	}
}
