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
			GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
			gm.SetLastCheckpoint(id);
			GameObject.Find("SettingsController").GetComponent<PlayerSettings>().SetLastCheckpoinPosition(transform.position, gm.GetCoins());
		}
	}

	public int GetID() {
		return id;
	}
}
