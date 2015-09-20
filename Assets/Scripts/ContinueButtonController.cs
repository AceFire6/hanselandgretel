using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonController : MonoBehaviour {

	private PlayerSettings settings;
	private bool updatedFromSettings;

	// Use this for initialization
	void Start () {
		settings = GameObject.Find("MenuUtility").GetComponent<PlayerSettings>();
		updatedFromSettings = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (settings.Loaded && !updatedFromSettings) {
			if (settings.MostRecentLevel != "") {
				gameObject.GetComponent<Button>().interactable = true;
			}
			updatedFromSettings = true;
		}
	}
}
