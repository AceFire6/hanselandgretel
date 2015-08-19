using UnityEngine;
using System.Collections;

public class PuzzleController : MonoBehaviour
{
	//The two buttons used by this puzzle
	public ButtonPlatform button1;
	public ButtonPlatform button2;
	
	private enum State
	{
		b1Pressed,
		b2Pressed,
		b1Released,
		b2Released}
	;
	private State currentState = State.b1Released; //default/start state

	private enum Direction {
		Up,
		Down
	}; //Used to tell the methods whether to raise/lower the pillars


	public GameObject[] pillars; //the pillars that will be raised/lowered
	public float maxPillarDepression = 0.5f; //how far down the pillars will sink
	private int numPillars;
	private Vector3[] pillarUpPoss; //positions to raise the pillars to
	private Vector3[] pillarDownPoss; //positions to lower the pillars to


	public float upSpeed = 2.0f;
	public float crossingDelay = 2.5f; //time that the bridge stays up before sinking
	public float downSpeed = 1.5f;

	private float crossingDelayTimer = 0.0f;

	void Start ()
	{
		//give the buttons references to this PuzzleController
		button1.SetPuzzleController (this);
		button2.SetPuzzleController (this);

		//Compute the up and down positions for each pillar
		numPillars = pillars.Length;
		pillarUpPoss = new Vector3[numPillars];
		pillarDownPoss = new Vector3[numPillars];
		for (int i=0; i < numPillars; i++) {
			Vector3 pos = pillars [i].transform.position;
			pillarUpPoss [i] = pos;
			
			pos.y -= maxPillarDepression;
			pillarDownPoss [i] = pos;
			pillars [i].transform.position = pillarDownPoss [i];
		}
	}

	void Update ()
	{
		bool firstPillarUp = Vector3.Distance (pillars [0].transform.position, pillarUpPoss [0]) < 0.01f;
		bool lastPillarUp = Vector3.Distance (pillars [numPillars - 1].transform.position, pillarUpPoss [numPillars - 1]) < 0.01f;
		if ( firstPillarUp && lastPillarUp) {

			//if the bridge is up, wait for players to cross
			crossingDelayTimer += Time.deltaTime;
			if (crossingDelayTimer >= crossingDelay) {
				//crossing time over, then lower it
				crossingDelayTimer = 0;
				UpdatePillarPositions ();
			}

		} else {
			//bridge is either being raised or lowered, so update pillar positions
			UpdatePillarPositions ();
		}

	}

	//Moves pillars either up or down based on the current state.
	//Pillars move up in a wave motion starting from the side where the button was pressed.
	//Pillars also go back down starting from the side where the button was released.
	void UpdatePillarPositions ()
	{
		switch (currentState) {
			case State.b1Pressed:
			LeftToRight(Direction.Up);
			break;
			
			case State.b2Pressed:
			RightToLeft(Direction.Up);
			break;
			
			case State.b1Released:
			LeftToRight(Direction.Down);
			break;
			
			case State.b2Released:
			RightToLeft(Direction.Down);
			break;
		}
	}

	//Moves the pillars either up or down (based on dir) starting with the pillar on the left
	private void LeftToRight (Direction dir){
		GameObject p = pillars[0];
		Vector3 targetPos = (dir == Direction.Up)? pillarUpPoss[0] : pillarDownPoss[0];
		float speed = (dir == Direction.Up)? upSpeed: downSpeed;

		p.transform.position = Vector3.Lerp (p.transform.position, targetPos, speed * Time.deltaTime);
		for (int i = 1; i < pillars.Length; i++) {
			targetPos = pillars [i].transform.position;
			targetPos.y = pillars [i - 1].transform.position.y;
			
			pillars [i].transform.position = Vector3.Lerp (pillars [i].transform.position, targetPos, speed * Time.deltaTime);
		}
	}

	//Moves the pillars either up or down (based on dir) starting with the pillar on the right
	private void RightToLeft(Direction dir){
		GameObject p = pillars[numPillars - 1];
		Vector3 targetPos = (dir == Direction.Up)? pillarUpPoss[numPillars - 1] : pillarDownPoss[numPillars - 1];
		float speed = (dir == Direction.Up)? upSpeed: downSpeed;

		p.transform.position = Vector3.Lerp (p.transform.position, targetPos, speed * Time.deltaTime);
		for (int i = numPillars - 2; i >= 0; i--) {
			targetPos = pillars [i].transform.position;
			targetPos.y = pillars [i + 1].transform.position.y;
			
			pillars [i].transform.position = Vector3.Lerp (pillars [i].transform.position, targetPos, speed * Time.deltaTime);
		}
	}

	//Buttons use this to notify us that they have been pressed.
	//update state and disable the other button
	public void ButtonPressed (ButtonPlatform pressedButton)
	{
		if (pressedButton == button1) {
			button2.enabled = false;
			currentState = State.b1Pressed;
		} else {
			button1.enabled = false;
			currentState = State.b2Pressed;
		}
	}

	//Buttons use this to notify us that they have been released.
	//Enables both buttons and updates state.
	public void ButtonReleased (ButtonPlatform releasedButton)
	{
		button1.enabled = true;
		button2.enabled = true;

		if (releasedButton == button1) {
			currentState = State.b1Released;
		} else {
			currentState = State.b2Released;
		}
	}
}
