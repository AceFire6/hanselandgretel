/*
 * Handles the in-game user interface and the activating of the pause menu.
 * 
 * Author: Jethro Muller
 * Date: 23-August-2015
 */

using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

	public GameObject HanselNamePlate;
	public GameObject GretelNamePlate;
	public Canvas UIOverlay;
	public GameObject PausePanel;
	public GameObject SettingsMenu;

	private GameObject [] players;
	private GameObject [] namePlates;
	private bool paused;
	private bool settings;
	private bool namePlatesActive;

	void Start () {
		namePlatesActive = true;
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
		settings = false;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (settings) {
				settings = false;
				gameObject.GetComponentInChildren<PlayerSettings>().SaveSettings();
				ToggleSettingsMenu(settings);
			} else {
				Time.timeScale = 1;
				paused = !paused;
				if (paused) {
					Time.timeScale = 0;
				}

				PausePanel.gameObject.SetActive(paused);
			}
		}
		if (!paused && !settings && namePlatesActive) {
			for (int i = 0; i < players.Length; i++) {
				namePlates[i].GetComponentInChildren<Slider>().value = players[i].GetComponent<Health>().GetHealthPercent();
			}
		}
	}

	// Handles unpausing for the resume button in the pause menu
	public void SetPause() {
		paused = false;
		Time.timeScale = 1;
		PausePanel.gameObject.SetActive(paused);
	}

	public void ToggleSettingsMenu(bool settingsState) {
		settings = settingsState;
		foreach (GameObject namePlate in namePlates) {
			namePlate.SetActive(!settingsState);
		}
		SettingsMenu.gameObject.SetActive(settingsState);
	}
}
