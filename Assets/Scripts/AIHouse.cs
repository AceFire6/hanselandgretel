using UnityEngine;
using System.Collections;

public class AIHouse : MonoBehaviour {

	private GameObject[] players;
	private GameObject closestPlayer;

	private float stompCD;
	private float jumpCD;

	private float stompDuration = 2.375f;
	private float jumpDuration = 2.417f;

	private float stompRange;
	private float jumpRange;

	private float stompTimer;
	private float jumpTimer;

	private bool isIdle;
	private bool isAttacking;
	private bool isChasing;
	private bool isBackingOff;

	private bool isStompAttacking;
	private bool isJumpAttacking;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
