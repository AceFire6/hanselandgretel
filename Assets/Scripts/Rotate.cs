/*
 * Simple script to rotate the GameObject slowly. Attach this to pickups like the coins dropped behind
 * minions.
 * 
 * Author: Muhummad Patel
 * Date: 17-August-2015
 */

using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public float speed = 150.0f;

	void Update () {
		transform.Rotate(Vector3.up, speed * Time.deltaTime);
	}
}
