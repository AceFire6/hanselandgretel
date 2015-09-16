using UnityEngine;
using System.Collections;

public class WolfDamageCollider : MonoBehaviour {

	AIWolf wolfScript;

	/* To ensure that the player can only get hit by an attack once*/
	private float lungeAttackDuration = 3f;
	private float clawAttackDuration = 1.958f;;
	private float lungeAttackTimer;
	private float clawAttackTimer;

	// Use this for initialization
	void Start () 
	{
		wolfScript = GetComponentInParent<AIWolf> ();
		lungeAttackTimer = lungeAttackDuration;
		clawAttackTimer = clawAttackDuration;
	}


	void OnTriggerEnter(Collider collision)
	{
		GameObject obj = collision.gameObject;
		if (obj.tag == "Player")
		{
			if (wolfScript.isClawAttacking && (clawAttackTimer >= clawAttackDuration))
			{
				clawAttackTimer = 0f;
				obj.GetComponent<Health>().TakeDamage(10);
			}
			else if (wolfScript.isLungeAttacking && (lungeAttackTimer >= lungeAttackDuration))
			{
				lungeAttackTimer = 0f;
				obj.GetComponent<Health>().TakeDamage(35);
			}
			Debug.Log (obj.GetComponent<Health>().totalHealth);
		}
		
	}

	// Update is called once per frame
	void Update () 
	{
		lungeAttackTimer += Time.deltaTime;
		clawAttackTimer += Time.deltaTime;
	}
}
