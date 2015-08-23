/*
 * Handles the in-game user interface and the activating of the pause menu.
 * 
 * Author: Jethro Muller
 * Date: 23-August-2015
 */

using UnityEngine;

public class UIHandler : MonoBehaviour {

	public GameObject HanselNamePlate;
	public GameObject GretelNamePlate;
	public Canvas UIOverlay;
	public GameObject PausePanel;

	private GameObject [] players;
	private GameObject [] namePlates;
	private bool paused;

	void Start () {
		// Spawns the nameplates for each player.
		players = GameObject.FindGameObjectsWithTag ("Player");
		namePlates = new GameObject[players.Length];

		for (int i = 0; i < players.Length; i++) {
			if (players[i].name == "Hansel") {
				namePlates[i] = (GameObject)Instantiate(HanselNamePlate);
			} else {
				namePlates[i] = (GameObject)Instantiate(GretelNamePlate);
			}
			namePlates[i].transform.SetParent(UIOverlay.transform, false);
		}
		paused = false;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			paused = !paused;
			PausePanel.gameObject.SetActive(paused);
		}
	}

	// Handles unpausing for the resume button in the pause menu
	public void SetPause() {
		paused = false;
		PausePanel.gameObject.SetActive(paused);
	}
}
