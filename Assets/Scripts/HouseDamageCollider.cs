using UnityEngine;
using System.Collections;

public class HouseDamageCollider : MonoBehaviour {

	AIHouse houseScript;
	
	/* To ensure that the player can only get hit by an attack once*/
	private float stompDuration = 3f;
	private float jumpDuration = 1.958f;
	private float stompTimer = 2.375f;
	private float jumpTimer = 2.471f;
	public enum Type
	{
		jumpAttack,
		stompAttack
	};
	public Type type;
	
	// Use this for initialization
	void Start () 
	{
		houseScript = GetComponentInParent<AIHouse> ();
		stompTimer = stompDuration;
		jumpTimer = jumpDuration;
	}
	
	
	void OnTriggerEnter(Collider collision)
	{
		GameObject obj = collision.gameObject;

		if (houseScript.isJumpAttacking)
			type = Type.jumpAttack;
		else
			type = Type.stompAttack;

		if (obj.tag == "Player")
		{
			if ((type == Type.stompAttack) && houseScript.isStompAttacking && (stompTimer >= stompDuration))
			{
				stompTimer = 0f;
				obj.GetComponent<Health>().TakeDamage(15);
			}
			else if ((type == Type.jumpAttack) && houseScript.isJumpAttacking && (jumpTimer >= jumpDuration))
			{
				jumpTimer = 0f;
				obj.GetComponent<Health>().TakeDamage(35);
			}
			Debug.Log (obj.GetComponent<Health>().totalHealth);
		}
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		stompTimer += Time.deltaTime;
		jumpTimer += Time.deltaTime;
	}
}
