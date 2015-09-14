using UnityEngine;
using System.Collections;

public class LevelTransition : MonoBehaviour {

	public string toLevel;

	public enum FadeDirection{In, Out};
	public float speed = 0.5f;
	public Texture2D fadeTexture;

	private float alpha = 1.0f; //alpha value of the fadeTexture
	private FadeDirection direction = FadeDirection.In;

	void OnGUI () {
		int multiplier = (direction == FadeDirection.In)? -1: 1;
		alpha += multiplier * speed * Time.deltaTime; 

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = -1000;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeTexture);
	}

	public float Fade (FadeDirection dir) {
		direction = dir;
		return speed;
	}

	void OnLevelWasLoaded () {
		Fade (FadeDirection.In);
	}

	void OnTriggerEnter (Collider other) {
		Fade (FadeDirection.Out);
		StartCoroutine (LoadLevelAfter (speed * 2f));
	}

	IEnumerator LoadLevelAfter (float seconds) {
		yield return new WaitForSeconds (seconds);
		Application.LoadLevel (toLevel);
	}
}
