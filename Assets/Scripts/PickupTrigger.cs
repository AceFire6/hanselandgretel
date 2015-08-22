/*
 * Destroys the gameobject and updates the player's coin count upon collision with a player
 * 
 * Author: Aashiq Parker
 * Date: 22-August-2015
 */

using UnityEngine;
using System.Collections;

public class PickupTrigger : MonoBehaviour {

	public int value;

	void Start () 
	{
		value = 10;
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Player")  //Only disappears if its a player
		{
			GameObject.Find("GameManager").GetComponent<GameManager>().UpdateCoints(value);
			Destroy(this.gameObject);
		}
	}
}
