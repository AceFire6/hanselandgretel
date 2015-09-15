/*
 * LevelTransition script to fade into the current level and fade out to the next level.
 * When this level is loaded, this script will fade in from black.
 * When the player triggers the exit portal, the script will fade out to black and
 * then load up the next scene/level.
 * 
 * Author: Muhummad Patel
 * Date: 15-September-2015
 */

using UnityEngine;
using System.Collections;

public class LevelTransition : MonoBehaviour {

	public string toLevel; //name of level we're transitioning to

	public enum FadeDirection{In, Out};
	public float speed = 0.5f;
	public Texture2D fadeTexture; //just a 2x2 black texture to do the fading

	private float alpha = 1.0f; //alpha value of the fadeTexture
	private FadeDirection direction = FadeDirection.In;

	//Gui update function called repeatedly to updatethe GUI.
	void OnGUI () {
		//compute the alpha value of the overlay (based on fade direction)
		int multiplier = (direction == FadeDirection.In)? -1: 1;
		alpha += multiplier * speed * Time.deltaTime; 
		alpha = Mathf.Clamp01 (alpha);

		//draw the overlay with newly computed alpha value
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = -1000;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeTexture);
	}

	//tell it which way to fade
	public float Fade (FadeDirection dir) {
		direction = dir;
		return speed;
	}

	//When the level is loaded, fade in from black
	void OnLevelWasLoaded () {
		Fade (FadeDirection.In);
	}

	//When the player triggers the exit portal, fade out to black and load the next scene
	void OnTriggerEnter (Collider other) {
		Fade (FadeDirection.Out);
		StartCoroutine (LoadLevelAfter (speed * 2f));
	}

	//Load toLevel aftee the given number of seconds.
	IEnumerator LoadLevelAfter (float seconds) {
		yield return new WaitForSeconds (seconds);
		Application.LoadLevel (toLevel);
	}
}
