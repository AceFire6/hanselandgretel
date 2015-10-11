using UnityEngine;
using System.Collections;

public class WitchFleeTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Witch") {
			//Make the witch Flee
			other.GetComponent<WitchController>().Flee();
		}
	}
}
