using UnityEngine;
using System.Collections;

public class ButtonPlatform : MonoBehaviour
{

	private PuzzleController puzzCtrl;
	public float yDepression = 0.02f; //how far the button is depressed when the player stands on it
	public float height = 0.03f; //the height to elevate the player standing on the button

	private GameObject button; //the mesh of the actual button
	private Vector3 pressedPos; //the depressed position of the button
	private Vector3 unpressedPos; //the released position of the button

	void Start ()
	{
		button = transform.parent.gameObject; //get the button mesh

		//compute the depressed and unpressed positions
		unpressedPos = button.transform.position;
		pressedPos = button.transform.position;
		pressedPos.y -= yDepression;
	}

	//auto fired when the player walks onto the button
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player") {
			//tell the puzzleCOntroller that this button has been pressed
			puzzCtrl.ButtonPressed (this);
			button.transform.position = pressedPos;

			//pick the player up (they are standing on the button)
			Vector3 newPos = other.gameObject.transform.position;
			newPos.y += height;
			other.gameObject.transform.position = newPos;
		}
	}

	//auto fired when the player steps off the button
	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "Player") {
			//tell the puzzleController that the button has been released
			puzzCtrl.ButtonReleased (this);
			button.transform.position = unpressedPos;

			//put the player back on the floor
			Vector3 newPos = other.gameObject.transform.position;
			newPos.y -= height;
			other.gameObject.transform.position = newPos;
		}
	}

	//Lets us get a reference to the puzzleController that this button was assigned to
	public void SetPuzzleController (PuzzleController puzzCtrl)
	{
		this.puzzCtrl = puzzCtrl;
	}
}
