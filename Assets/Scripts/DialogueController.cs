using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueController : MonoBehaviour {

	[System.Serializable]
	public class Dialogue {
		public string Name;
		public string Line;
	}

	[System.Serializable]
	public class NamedPortrait {
		public string Name;
		public Sprite Portrait;
	}

	public NamedPortrait[] CharacterPortraits;
	public Dialogue[] DialogueContent;

	private IDictionary<string, Sprite> portraitDict;
	private int progressIndex;
	private Image portrait;
	private Text dialogueText;
	private Text nameText;
	private bool runDialog;

	void Start() {
		progressIndex = 0;
		portraitDict = new Dictionary<string, Sprite>();
		foreach (NamedPortrait np in CharacterPortraits) {
			portraitDict.Add(np.Name, np.Portrait);
		}
		nameText = GameObject.Find ("NameText").GetComponent<Text>();
		dialogueText = GameObject.Find ("DialogueText").GetComponent<Text>();
		portrait = GameObject.Find ("CharacterPortrait").GetComponent<Image>();
	}

	void Update() {
		if (runDialog && (progressIndex < DialogueContent.Length)) {
			nameText.text = DialogueContent[progressIndex].Name;
			dialogueText.text = DialogueContent[progressIndex].Line;
			string charName = DialogueContent[progressIndex].Name;
			Sprite temp;
			portraitDict.TryGetValue(charName, out temp);
			portrait.sprite = temp;

			if (Input.GetKeyDown(KeyCode.LeftControl)) {
				progressIndex++;
			}
		}

		if (progressIndex > DialogueContent.Length - 1) {
			Time.timeScale = 1;
			GameObject.Find("GameUI").GetComponent<UIHandler>().ToggleNamePlates();
			runDialog = false;
		}
	}

	public bool DialogFinished() {
		return !runDialog;
	}

	public void StartDialogue() {
		runDialog = true;
		GameObject.Find("GameUI").GetComponent<UIHandler>().ToggleNamePlates();
		Time.timeScale = 0;
	}
}
