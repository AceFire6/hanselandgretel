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
		Application.LoadLevel (sceneName);
	}
}
