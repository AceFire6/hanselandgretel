// This makes the character turn to face the current movement speed per default.
var autoRotate : boolean = true;
var maxRotationSpeed : float = 360;

var playerNo : int = 1;
private var horizontal : String = "Horizontal";
private var strafe : String = "Strafe";
private var attack : String = "Attack";
private var jump : String = "Jump";

private var motor : CharacterMotor;
private var animator : Animator;

// Use this for initialization
function Awake () {
	motor = GetComponent(CharacterMotor);
	animator = GetComponent(Animator);
	
	if (playerNo == 2) {
		horizontal += "_2";
		strafe += "_2";
		attack += "_2";
		jump += "_2";
	}
}

// Update is called once per frame
function Update () {
	// Get the input vector from keyboard or analog stick
	var directionVector = new Vector3(Input.GetAxis(horizontal), 0, 0);//Input.GetAxis("Vertical"), 0);
	var isStrafing = (Input.GetAxis(strafe) > 0)? true: false;
	
	//Made the shooting key left cntrl in the mean time
	var isShooting = (Input.GetAxis(attack) > 0)? true: false;
	
	if (directionVector != Vector3.zero) {
		// Get the length of the directon vector and then normalize it
		// Dividing by the length is cheaper than normalizing when we already have the length anyway
		var directionLength = directionVector.magnitude;
		directionVector = directionVector / directionLength;
		
		// Make sure the length is no bigger than 1
		directionLength = Mathf.Min(1, directionLength);
		
		// Make the input vector more sensitive towards the extremes and less sensitive in the middle
		// This makes it easier to control slow speeds when using analog sticks
		directionLength = directionLength * directionLength;
		
		// Multiply the normalized direction vector by the modified length
		directionVector = directionVector * directionLength;
	}
	
	// Rotate the input vector into camera space so up is camera's up and right is camera's right
	directionVector = Camera.main.transform.rotation * directionVector;
	
	// Rotate input vector to be perpendicular to character's up vector
	var camToCharacterSpace = Quaternion.FromToRotation(-Camera.main.transform.forward, transform.up);
	directionVector = (camToCharacterSpace * directionVector);
	
	// Apply the direction to the CharacterMotor
	motor.inputMoveDirection = directionVector;
	//Debug.Log(directionVector);
	motor.inputJump = Input.GetButton(jump);
	
	// Set rotation to the move direction
	
	if (autoRotate && directionVector.sqrMagnitude > 0.01) {
		var toFace : Vector3 = (isStrafing)? (-1 * directionVector): directionVector;
		
		var newForward : Vector3 = toFace;
//		var newForward : Vector3 = ConstantSlerp(
//			transform.forward,
//			toFace,
//			maxRotationSpeed * Time.deltaTime
//		);
		newForward = ProjectOntoPlane(newForward, transform.up);
		transform.rotation = Quaternion.LookRotation(newForward, transform.up);
	}
	
	//Update AC logic
	animator.SetBool("isRunning", directionVector != Vector3.zero);
	animator.SetBool("isStrafing", isStrafing);
	animator.SetBool("isShooting" , isShooting);
	
	/* For jumping, I initally set the boolean like such:
			animator.SetBool("isJumping" , motor.inputJump);
	   The problem with this is that inputJump uses GetButton and not GetButtonDown
	   which means that when you press spacebar (even if you hold it down), Unity
	   only registers it as one press, and so the animation for jumping cancels mid air.
	   I then tried using GetButtonDown, but then you'd have to hold space bar throughout
	   the duration of the jump and if you were to keep holding it when you landed, the
	   animation would keep playing and wouldn't transition to idle. So what I did was 
	   include the jumping flag in the PlatformInputController script and set it based
	   on whether the player is grounded or not and it works perfectly like that */
}

function ProjectOntoPlane (v : Vector3, normal : Vector3) {
	return v - Vector3.Project(v, normal);
}

function ConstantSlerp (from : Vector3, to : Vector3, angle : float) {
	var value : float = Mathf.Min(1, angle / Vector3.Angle(from, to));
	return Vector3.Slerp(from, to, value);
}

// Require a character controller to be attached to the same game object
@script RequireComponent (CharacterMotor)
@script AddComponentMenu ("Character/Platform Input Controller")
