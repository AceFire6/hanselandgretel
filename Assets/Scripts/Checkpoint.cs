using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public ParticleSystem activeParticles;
	int id;

	// Use this for initialization
	void Start () {
		id = GetHashCode();
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			activeParticles.startColor = Color.green;
			GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
			gm.SetLastCheckpoint(id);
			GameObject.Find("SettingsController").GetComponent<PlayerSettings>().SetLastCheckpointPosition(transform.position, gm.GetCoins());
		}
	}

	public int GetID() {
		return id;
	}
}
