/*
 * Handles settings menu events.
 * 
 * Handles updateing slider text for the volume sliders.
 * Handles changing difficulty using the arrow buttons.
 * 
 * Author: Jethro Muller
 * Date: 22-August-2015
 */
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

	public Text DifficultyText;
	private int currentDifficulty;
	private string[] difficulties = {"Easy", "Normal", "Hard"};

	private PlayerSettings settings;

	void Start ()
	{
		settings = GameObject.Find("SettingsMenuController").GetComponent<PlayerSettings>();
		LoadFromSettings();
	}

	private void LoadFromSettings() {
		currentDifficulty = settings.DifficultyIndex;
		DifficultyText.text = difficulties [currentDifficulty];
		
		GameObject.Find("MuteMusic").GetComponent<Toggle>().isOn = settings.IsMusicMuted();
		GameObject.Find("MuteSound").GetComponent<Toggle>().isOn = settings.IsSoundMuted();
		
		GameObject.Find("Sound").GetComponentInChildren<Slider>().value = settings.SoundVolume;
		GameObject.Find("Music").GetComponentInChildren<Slider>().value = settings.MusicVolume;
	}

	void Update() {
		if (settings.Started) {
			LoadFromSettings();
			settings.Started = false;
		}
	}

	// Updates the textbox that relates to the given slider
	public void UpdateText (Slider slider)
	{
		Text sliderText = slider.transform.parent.FindChild ("Text").GetComponent<Text> ();
		// Changes the associated text to a percentage based on the slider
		sliderText.text = slider.value.ToString() + "%";
	}

	// Increase or decrease the difficulty setting
	public void ChangeDifficulty (int increment)
	{
		// Increment the difficulty, the min and max statements clamp the value of currentDifficulty
		currentDifficulty += increment;
		currentDifficulty = Mathf.Min (currentDifficulty, difficulties.Length - 1);
		currentDifficulty = Mathf.Max (currentDifficulty, 0);

		DifficultyText.text = difficulties [currentDifficulty];
		settings.DifficultyIndex = currentDifficulty;
	}
}
