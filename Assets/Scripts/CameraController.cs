using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float minFov = 45.0f;
	public float maxFov = 90.0f;
	public float padding = 15.0f;
	public float zoomSpeed = 1.5f;

	//Change these player vars to private when we actually have 2 players in the scene
	public GameObject player1;
	public GameObject player2;

	public float camElevation = 0.6f;

	protected void Start ()
	{
		//Use these when we actually have 2 players on screen
		//GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		//player1 = players[0];
		//player2 = players[1];
	}

	void Update ()
	{
		//Centre the camera between the two players
		Vector3 centre = (player1.transform.position + player2.transform.position) / 2;
		camera.transform.position = new Vector3(centre.x, centre.y + camElevation, camera.transform.position.z);


		float playerToCentre = 0.5f * Vector3.Distance(player1.transform.position, player2.transform.position); //opposite side
		float centreToCamera = Mathf.Abs(camera.transform.position.z); //adjacent side
		
		float newFov = 2 * Mathf.Rad2Deg * Mathf.Atan(playerToCentre / centreToCamera); //solve for theta(fov) * 2
		newFov *= (16f/9f) / ((float)camera.pixelWidth / camera.pixelHeight); //multiply by aspect ratio
		newFov += padding; //add padding
		newFov = Mathf.Clamp(newFov, minFov, maxFov);

		camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, newFov, zoomSpeed * Time.deltaTime); //Update camera's field of view
	}
}
