/*
 * Makes the light component attached to this gameObject flicker randomly. The light
 * switches from full intensity to a lesser (tweakable) intensity at random intervals.
 * 
 * Author: Muhummad Patel
 * Date: 19-August-2015
 */

using UnityEngine;
using System.Collections;

public class FlickerLight : MonoBehaviour {

	//factor to scale down the light intensity by to achieve the dimming effect
	public float flickerIntensity = 0.9f;
	public float frequency = 0.8f; //frequency of the flickering

	private Light objLight;
	private Color originalColor;

	void Start ()
	{
		objLight = GetComponent<Light> ();
		originalColor = GetComponent<Light> ().color; //the original rgb colour of the light
	}

	void Update () 
	{
		//randomly decide whether to switch intensities
		if (Random.value > frequency) {
			if (objLight.color == originalColor) {
				//scale the rgb values down by flickerIntensity
				objLight.color = originalColor * flickerIntensity;
			} else {
				objLight.color = originalColor; //switch back to normal intensity
			}
		}
	}
}
