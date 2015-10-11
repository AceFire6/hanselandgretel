using UnityEngine;
using System.Collections;

public class BobUpAndDown : MonoBehaviour {

	public float displacement = 2.5f;
	public float bobSpeed = 100f;
	
	private float originalPos;
	private float angle = -90f;
	
	
	void Start () {
		originalPos = transform.localPosition.y;
	}
	
	void FixedUpdate() {
		angle += bobSpeed * Time.deltaTime;
		if (angle > 270) {
			angle -= 360;
		}

		Vector3 pos = transform.localPosition;
		pos.y = originalPos + displacement * (1 + Mathf.Sin(angle * Mathf.Deg2Rad)) / 2;
		transform.localPosition = pos;
	}
}
