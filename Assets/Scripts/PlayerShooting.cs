/*
 * Script to control spawning arrows whenever Gretel shoots. Based on the animation rather than the keyboard press
 * since the movement script handles the keyboard presses and it prevents firing when the player is jumping.
 * 
 * Author: Aashiq Parker
 * Date: 21-August-2015
 */

using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {

	public Transform arrow;

	private float cooldown;
	private float timer;
	private Vector3 positionOffset; //Uses an offset to place the arrow since the bow moves up and down because of the
										//animation but the position doesn't change
	Animator animator;

	private AudioSource AddAudio (AudioClip clip, bool loop, bool playAwake, float vol) {
		AudioSource newAudio = gameObject.AddComponent<AudioSource>();
		newAudio.clip = clip;
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol;
		return newAudio;
	}

	public AudioClip shootClip;
	private AudioSource shootAud;
	
	private void initAudio () {
		
		if (shootClip != null) {
			shootAud = AddAudio (shootClip, false, false, 1);
		}
	}

	void Start () 
	{
		animator = GetComponent<Animator> ();
		positionOffset = new Vector3 (0, 0.5f, 0);
		cooldown = 0.5f;

		initAudio ();
	}

	void Shoot()
	{
		//play shoot sound
		if (shootAud != null && !shootAud.isPlaying) {
			shootAud.Play();
		}

		positionOffset.x = transform.forward.x * 0.1f;
		Instantiate (arrow, transform.position + positionOffset,transform.rotation);
	}

	void Update () 
	{
		timer += Time.deltaTime;				//Check if the player performing the shooting animation.
		//bool shooting = animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.StandAndShootv2") || animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.RunAndShoot");  
		//Temporary until we have proper animations for shooting in all states
		bool shooting = Input.GetAxis("Attack") > 0;
		if (timer > cooldown && shooting)
		{
			timer = 0;
			Shoot ();
		}

	}
}
