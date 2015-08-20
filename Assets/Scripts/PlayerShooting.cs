using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {

	public Transform arrow;

	private float cooldown;
	private float timer;
	private Vector3 positionOffset;

	Animator animator;

	void Start () 
	{
		animator = GetComponent<Animator> ();
		positionOffset = new Vector3 (0, 0.55f, 0);
		cooldown = 0.292f * 2; //Time it takes to raise and lower bow
	}

	void Shoot()
	{
		positionOffset.x = transform.forward.x * 0.01f;
		Instantiate (arrow, transform.position + positionOffset,transform.rotation);
	}

	void Update () 
	{
		timer += Time.deltaTime;
		bool canShoot = !animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.Jump") && !animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.Strafe");
		if (Input.GetKeyDown (KeyCode.Space) && timer > cooldown && canShoot) 
		{
			timer = 0;
			Invoke ("Shoot", cooldown /2); //Instantiate after 0.292s when the bow reaches the fire position
		}

	}
}
