/* Handles the slashing of axes(Hansel). This script needs to be attached to the
 * axe/hand bone of Hansel's rig. This is so the collider (also attached to the rig)
 * will move with the animation allowing for the collisions detected to be accurate.
 * Damage is dealt when the weapon collider collides with an enemy while hansel is
 * attacking.
 * 
 * Author: Muhummad Patel
 * Date: 16-September-2015
 */
		
using UnityEngine;
using System.Collections;

public class PlayerSlashing : MonoBehaviour {
		
	public int slashDamage = 10;

	private Animator animator;
	private bool canSlash;

	private AudioSource AddAudio (AudioClip clip, bool loop, bool playAwake, float vol) {
		AudioSource newAudio = gameObject.AddComponent<AudioSource>();
		newAudio.clip = clip;
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol;
		return newAudio;
	}
	
	public AudioClip slashClip;
	private AudioSource slashAud;

	private AudioManager audioManager;

	private void initAudio () {
		audioManager = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioManager>();

		if (slashClip != null) {
			slashAud = AddAudio (slashClip, false, false, 1);
		}
	}

	void Start () 
	{
		animator = GetComponentInParent<Animator> ();

		initAudio ();
	}

	//The axe has hit something
	//If it's an enemy and hansel can attack, then decrease the enemy's health
	//If the gameobject doesn't have a health component, just ignore it.
	void OnCollisionStay(Collision collision) {
		if (canSlash && collision.gameObject.tag != "Player") {
			Health health = collision.gameObject.GetComponent<Health> ();
			if (health != null) {
				health.TakeDamage (slashDamage);
			}
		}
	}
	
	void Update () 
	{
		canSlash = animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.StandAndAttack") || animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.RunAndChop");

		//play slashing sound
		if(canSlash && slashAud!= null && !slashAud.isPlaying && !audioManager.isSoundMute){
			slashAud.volume = audioManager.soundVolume;
			slashAud.Play();
		}
	}
}