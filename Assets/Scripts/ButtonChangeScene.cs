/*
 * Used to change the scene on a button press.
 * 
 * Author: Jethro Muller
 */
using UnityEngine;

public class ButtonChangeScene : MonoBehaviour
{
	
	public void ChangeScene (string sceneName)
	{
		if (sceneName == "Continue") {
			string continueLevel = GameObject.Find("MenuUtility").GetComponent<PlayerSettings>().MostRecentLevel;
			Application.LoadLevel (continueLevel);
		} else {
			Application.LoadLevel (sceneName);
		}
	}
}
