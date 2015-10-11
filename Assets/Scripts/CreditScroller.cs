using UnityEngine;
using System.Collections;

public class CreditScroller : MonoBehaviour {

	public float ScrollSpeed = 1.5F;
	public float StopPoint = 2480F;

	private RectTransform rectTransform;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 pos = rectTransform.anchoredPosition;
		if (pos.y < StopPoint) {
			pos.Set(pos.x, pos.y + ScrollSpeed);
			rectTransform.anchoredPosition = pos;
		}
	}
}
