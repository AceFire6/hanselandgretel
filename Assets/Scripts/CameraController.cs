/*
 * Controls the movement of the camera. The camera will centre itself between the two targets
 * (the two players). As the players move together or apart, the camera will adjust it's 
 * field of view to zoom in or out so as to keep both players in view. The zoom is clamped to
 * the specified range. Mathf.Lerp is used to adjust the field of view smoothly. The speed of
 * the lerping can also be adjusted.
 * 
 * Author: Muhummad Patel
 * Date: 18-August-2015
 */

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float minFov = 45.0f;
	public float maxFov = 90.0f;
	public float padding = 15.0f; //border to keep around the players
	public float zoomSpeed = 1.5f; //speed of lerping between FOV's
	public float followSpeed = 2.0f; //speed of lerping to follow players
	public float camLead = 1.0f; // How far ahead the players can see

	private GameObject player1;
	private GameObject player2;

	public float camElevation = 0.7f;

	protected void Start ()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		if (players.Length == 2) {
			player1 = players[0];
			player2 = players[1];
		} else {
			player1 = player2 = players[0];
		}
	}

	void FixedUpdate ()
	{
		//Centre the camera between the two players smoothly using lerp
		Vector3 centre = (player1.transform.position + player2.transform.position) / 2;
		Vector3 newPos = new Vector3(centre.x + camLead, centre.y + camElevation, camera.transform.position.z);
		camera.transform.position = Vector3.Lerp(camera.transform.position, newPos, followSpeed * Time.deltaTime);


		float playerToCentre = 0.5f * Vector3.Distance(player1.transform.position, player2.transform.position); //opposite side
		float centreToCamera = Mathf.Abs(camera.transform.position.z); //adjacent side
		
		float newFov = 2 * Mathf.Rad2Deg * Mathf.Atan(playerToCentre / centreToCamera); //solve for theta(fov) * 2
		newFov *= (16f/9f) / ((float)camera.pixelWidth / camera.pixelHeight); //multiply by aspect ratio
		newFov += padding; //add padding
		newFov = Mathf.Clamp(newFov, minFov, maxFov);

		//Update camera's field of view
		camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, newFov, zoomSpeed * Time.deltaTime);
	}
}
