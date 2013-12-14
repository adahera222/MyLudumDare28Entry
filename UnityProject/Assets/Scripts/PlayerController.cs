using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
[RequireComponent(typeof(tk2dSpriteAnimator))]
public class PlayerController : MonoBehaviour {

	// Player variables
	public float gravity = 20;
	public float speed = 12;
	public float acceleration = 30;
	public float jumpHeight = 12;

	private float currentSpeed;
	private float targetSpeed;
	private Vector2 delta;
	private float moveDirX;

	// States
	private bool jumping;
	private bool climbing;

	private PlayerPhysics playerPhysics;
	tk2dSpriteAnimator animator;

	// Use this for initialization
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
		animator = GetComponent<tk2dSpriteAnimator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(playerPhysics.stopped){
			targetSpeed = 0;
			currentSpeed = 0;
		}

		if(playerPhysics.grounded){
			delta.y = 0;
		}

		if(Input.GetButtonDown("Jump")) {
			if(playerPhysics.grounded) {
				delta.y = jumpHeight;
			}
		}
		moveDirX = Input.GetAxisRaw("Horizontal");
		targetSpeed = moveDirX * speed;
		currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);

		delta.x = currentSpeed;
		delta.y -= gravity * Time.deltaTime;
		playerPhysics.Move(delta * Time.deltaTime, moveDirX);
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
}
