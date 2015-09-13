using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public Light ActiveLight;
	int id;

	// Use this for initialization
	void Start () {
		id = GetHashCode();
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			ActiveLight.color = Color.green;
			GameObject.Find("GameManager").GetComponent<GameManager>().SetLastCheckpoint(id);
		}
	}

	public int GetID() {
		return id;
	}
}
