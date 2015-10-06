using UnityEngine;
using System.Collections;

public class Minion2DamageCollider : MonoBehaviour {

	public int attackDamage = 10;

	private MovementMinion2 movement;

	// Use this for initialization
	void Start () {
		movement = (MovementMinion2)GetComponentInParent<Movement> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider collision){
		GameObject other = collision.gameObject;
		if (other.tag == "Player" && movement.getIsAttacking()) {
			//We hit a player while attacking, so decrease the player's health by attackDamage
			other.GetComponent<Health>().TakeDamage(attackDamage);
		}
	}
}
