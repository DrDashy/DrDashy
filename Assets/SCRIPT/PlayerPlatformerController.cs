
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

	public float maxSpeed = 7;
	public float jumpTakeOffSpeed = 15;
	public float amoSpeed;
	private SpriteRenderer spriteRenderer;
	private Animator anim;
	public Rigidbody2D projectile;
	public bool CanFire;
	public float cooldown;

	public AudioSource audiosource;


	// Use this for initialization
	void Awake () 
	{
		audiosource = GetComponent<AudioSource> ();
		CanFire = true;
		spriteRenderer = GetComponent<SpriteRenderer> (); 
		anim = GetComponent<Animator> ();
	}

	protected override void ComputeVelocity ()
	{
		
		if (gameObject.transform.position.y < -20) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}


		Vector2 move = Vector2.zero;

		move.x = Input.GetAxis ("Horizontal");

		if ((Input.GetButtonDown ("Jump") || Input.GetButtonDown ("Jump2")) && grounded) {
			velocity.y = jumpTakeOffSpeed;
		} else if (Input.GetButtonUp ("Jump") || Input.GetButtonUp ("Jump2")) {
			if (velocity.y > 0) {
				velocity.y = velocity.y * 0.5f;
			}
		}



		//SHOOTING SECTION
		if (CanFire) {
			if (Input.GetButtonDown ("Fire1")||Input.GetButtonDown ("Fire2")) {
				Rigidbody2D clone;
				clone = Instantiate (projectile, transform.position, Quaternion.identity) as Rigidbody2D;



				if (spriteRenderer.flipX) {
					clone.velocity = transform.TransformDirection (Vector2.left * amoSpeed);
				} else {
					clone.velocity = transform.TransformDirection (Vector2.right * amoSpeed);
				}
				CanFire = false;
				StartCoroutine(WaitToShoot());
			}
		}
		//FLIP SPRITE
		bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.0f) : (move.x < 0.0f));
		if (flipSprite) {
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}

	

		targetVelocity = move * maxSpeed;



		//ANNIMATION
		if (move.x != 0f) {
			anim.SetBool ("IsRuning", true);
			anim.SetBool ("IsIdle", false);
		} else {
			anim.SetBool ("IsRuning", false);
			anim.SetBool ("IsIdle", true);
		}
		if (grounded == false) {
			anim.SetBool ("IsJumping", true);
			anim.SetBool ("IsRuning", false);
			anim.SetBool ("IsIdle", false);
			if (velocity.y < 2) {
				anim.SetBool ("IsJumping2", true);
			}
		} else {
			anim.SetBool ("IsJumping", false);
			anim.SetBool ("IsJumping2", false);
		}
	}



	void OnTriggerEnter2D(Collider2D col) {
		if(col.gameObject.tag == "kill")
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		if(col.gameObject.tag == "ennemi")
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if(col.gameObject.tag == "pic")
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		if(col.gameObject.tag == "MovingPlateform")
		{
			this.transform.SetParent (col.transform);
		}
	
	}

	void OnCollisionExit2D(Collision2D col){
		if(col.gameObject.tag == "MovingPlateform"){
			this.transform.SetParent (null);

		}
	}

	IEnumerator WaitToShoot()
	{

		yield return new WaitForSeconds(cooldown);
		CanFire = true;
	}





}