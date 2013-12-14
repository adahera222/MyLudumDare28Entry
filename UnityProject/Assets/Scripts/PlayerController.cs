using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
[RequireComponent(typeof(tk2dSpriteAnimator))]
public class PlayerController : MonoBehaviour {

	// Player variables
	public float gravity = 90;
	public float speed = 30;
	public float acceleration = 100;
	public float jumpHeight = 40;
	public float powerjumpHeight = 75;
	public float climbspeed = 18;
	public float climbAcceleration = 300;

	private float currentSpeedX;
	private float currentSpeedY;
	private float targetSpeed;
	private Vector2 delta;
	private float moveDirX;
	private float moveDirY;
	
	private float blinkCounter;

	// States
	[HideInInspector]
	public bool jumping;
	[HideInInspector]
	public bool climbing;
	[HideInInspector]
	public bool upheld;
	[HideInInspector]
	public bool canControl = true;

	private PlayerPhysics playerPhysics;
	tk2dSpriteAnimator animator;
	tk2dSprite sprite;

	private static PlayerController instance;
	
	public static PlayerController Instance
	{
		get {
			if(instance == null)
			{
				Debug.LogError ("Error");
			}
			return instance;
		}
	}
	
	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
		animator = GetComponent<tk2dSpriteAnimator>();
		sprite = GetComponent<tk2dSprite>();
	}

	public void Restart() {
		playerPhysics.Restart();
		canControl = true;
	}

	// Update is called once per frame
	void Update () {
		if(playerPhysics.stopped){
			targetSpeed = 0;
			currentSpeedX = 0;
		}

		if(playerPhysics.grounded){
			delta.y = 0;
		}

		if(upheld && Input.GetAxisRaw("Vertical") == 0)
			upheld = false;

		if(canControl && Input.GetButtonDown("Jump")) {
			if(!climbing && playerPhysics.grounded) {
				delta.y = jumpHeight;
			} else if(climbing) {
				upheld = true;
				climbing = false;
				delta.y = playerPhysics.atEdge() ? powerjumpHeight : jumpHeight;
				playerPhysics.climbing = false;
			}
		}
		moveDirX = Input.GetAxisRaw("Horizontal");
		moveDirY = Input.GetAxisRaw("Vertical");

		if(!canControl) {
			moveDirX = 0;
			moveDirY = 0;
		}

		if(moveDirY > 0 && !climbing && playerPhysics.canGrab() && !upheld) {
			climbing = true;
			currentSpeedX = 0;
			playerPhysics.Grab();
		}

		if(!climbing) {
			targetSpeed = moveDirX * speed;
			currentSpeedX = IncrementTowards(currentSpeedX, targetSpeed, acceleration);

			delta.x = currentSpeedX;
			delta.y -= gravity * Time.deltaTime;
		} else {
			targetSpeed = moveDirY * climbspeed;
			currentSpeedY = IncrementTowards(currentSpeedY, targetSpeed, climbAcceleration);
			delta.x = 0;
			delta.y = currentSpeedY;
		}
		
		Animate();
		playerPhysics.Move(delta * Time.deltaTime, moveDirX);
	}

	void Animate() {
		if(delta.x != 0) {
			animator.Sprite.FlipX = delta.x < 0;
			playerPhysics.flipX = delta.x < 0;
		}

		if(climbing) {
			if(delta.y == 0)
				animator.Play ("ClimbIdle");
			else
				animator.Play ("Climbing");
		}
		else if(!playerPhysics.grounded) {
			if(delta.y <= 0)
				animator.Play ("Falling");
			else
				animator.Play("Rising");
		} else if(delta.x != 0)
			animator.Play("Running");
		else
			animator.Play("Idle");
	}

	private float IncrementTowards(float n, float target, float a) {
		if(n == target) {
			return n;
		} else {
			float dir = Mathf.Sign (target - n);
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target-n))? n : target;
		}
	}
	public bool IsDead {
		get {return playerPhysics.isDead;}
		set {playerPhysics.isDead = value;}
	}

	public void Kill(){
		canControl = false;
		climbing = false;
		playerPhysics.climbing = false;
	}

	public void Respawn(Vector3 pos) {
		canControl = true;
		playerPhysics.isDead = false;
		transform.position = new Vector3(0, pos.y, 0);
		climbing = true;
		currentSpeedX = 0;
		currentSpeedY = 0;
		playerPhysics.Grab();
		StartCoroutine(Invincible());
	}

	IEnumerator Invincible(){
		playerPhysics.invincible = true;
		StartCoroutine(Blink());
		yield return new WaitForSeconds(3.0f);
		playerPhysics.invincible = false;
	}

	IEnumerator Blink(){
		Color col;
		while(playerPhysics.invincible){
			blinkCounter += Time.deltaTime;
			if(blinkCounter > 0.15f){
				col = sprite.color;
				col.a = col.a == 0 ? 1.0f : 0.0f;
				sprite.color = col;
				blinkCounter = 0.0f;
			}
			yield return null;
		}
		col = sprite.color;
		col.a = 1.0f;
		sprite.color = col;
		yield return null;
	}
}
