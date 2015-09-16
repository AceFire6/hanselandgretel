using UnityEngine;
using System.Collections;

public class WolfDamageCollider : MonoBehaviour {

	AIWolf wolfScript;
	// Use this for initialization
	void Start () 
	{
		wolfScript = GetComponentInParent<AIWolf> ();
	}


	void OnTriggerEnter(Collider collision)
	{
		GameObject obj = collision.gameObject;
		if (obj.tag == "Player")
		{
			if (wolfScript.isClawAttacking)
				obj.GetComponent<Health>().TakeDamage(10);
			else if (wolfScript.isLungeAttacking)
				obj.GetComponent<Health>().TakeDamage(40);
			Debug.Log (obj.GetComponent<Health>().totalHealth);
		}
		
	}

	// Update is called once per frame
	void Update () 
	{
	}
}
