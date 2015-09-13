using UnityEngine;
using System;

public class KillOnCollision : MonoBehaviour {

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			collider.gameObject.GetComponent<Health>().Kill();
		}
	}
}
