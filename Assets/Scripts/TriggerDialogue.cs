using UnityEngine;
using System.Collections;

public class TriggerDialogue : MonoBehaviour {

	public GameObject DialogueUI;
	private DialogueController dialogueController;
	private bool triggered;

	void Start() {
		triggered = false;
		dialogueController = DialogueUI.GetComponent<DialogueController>();
	}

	void Update() {
		if (dialogueController.DialogFinished() && triggered) {
			DialogueUI.SetActive(false);
			Destroy(this);
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Player")
		{
			triggered = true;
			DialogueUI.SetActive(true);
			dialogueController.StartDialogue();
		}
	}
}
