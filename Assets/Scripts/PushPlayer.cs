/*
 * Pushes the player slowly away from the edge of the screen when they are in danger
 * of leaving.
 * 
 * Author: Muhummad Patel
 * Date: 21-September-2015
 */

using UnityEngine;
using System.Collections;

public class PushPlayer : MonoBehaviour {

	public float xOffset = 0.001f;

	//If the player is colliding with the wall, then push the player away from the wall
	void OnTriggerStay(Collider other) {
		Debug.Log (other.gameObject.tag);
		if (other.gameObject.tag == "Player") {
			Vector3 pos = other.gameObject.transform.position;
			pos.x += xOffset;
			other.gameObject.transform.position = pos;
		}
	}
}
