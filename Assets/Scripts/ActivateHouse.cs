using UnityEngine;
using System.Collections;

public class ActivateHouse : MonoBehaviour {

	public GameObject house;
	public GameObject houseHealthBar;
	public GameObject DialogueUI;

	private DialogueController dialogueController;

	private AIHouse aiHouse;
	private Animator animator;

	private bool triggered = false;

	// Use this for initialization
	void Start () {
		dialogueController = DialogueUI.GetComponent<DialogueController>();

		aiHouse = house.GetComponent<AIHouse> ();
		animator = house.GetComponent<Animator> ();
	}

	void Update () {
		if (dialogueController.DialogFinished() && triggered) {
			houseHealthBar.SetActive(true);

			Destroy(this);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (!triggered && other.gameObject.tag == "Player") {
			triggered = true;

			aiHouse.isActive = true;
			animator.SetTrigger("activate");		}
	}
}
