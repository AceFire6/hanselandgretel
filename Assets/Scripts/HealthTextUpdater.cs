using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthTextUpdater : MonoBehaviour {
	public Text HealthText;

	public void UpdateText(float health) {
		HealthText.text = Math.Round(health * 100) + "%";
	}
}
