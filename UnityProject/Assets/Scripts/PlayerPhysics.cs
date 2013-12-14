using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// As inpired by YouTube Platformer Tutorial
[RequireComponent (typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {
	public LayerMask collisionMask;
	public int debug_value;

	private BoxCollider collider;
	private Vector3 s;
	private Vector3 c;

	private Vector3 originalSize;
	private Vector3 originalCentre;
	private float colliderScale;
	
	private int divX = 3;
	private int divY = 3;

	private float skin = .005f;

	[HideInInspector]
	public bool flipX;
	[HideInInspector]
	public bool grounded;
	[HideInInspector]
	public bool stopped;
	[HideInInspector]
	public bool canClimb;
	[HideInInspector]
	public bool climbing;
	[HideInInspector]
	public bool isDead;
	[HideInInspector]
	public bool invincible;

	private List<Ladder> ladder_list;

	Ray ray;
	RaycastHit hit;

	// Use this for initialization
	void Start () {
		ladder_list = new List<Ladder>();
		collider = GetComponent<BoxCollider>();
		colliderScale = transform.localScale.x;

		originalSize = collider.size;
		originalCentre = collider.center;
		SetCollider(originalSize,originalCentre);
	}

	public void Restart() {
		isDead = false;
	}

	public void Move(Vector2 moveAmount, float moveDirX){
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		Vector2 p = transform.position;

		grounded = false;

		for(int i = 0; i<divX; i+=1){
			float dir = Mathf.Sign(deltaY);
			float x = (p.x + c.x - s.x/2) + s.x/(divX - 1) * i;
			float y = p.y + c.y + s.y/2 * dir;

			// Quick solution for unexpected issue
			if(flipX) x -= 0.3f;

			ray = new Ray(new Vector2(x,y), new Vector2(0, dir));
			Debug.DrawRay(ray.origin,ray.direction);

			if(Physics.Raycast(ray, out hit, Mathf.Abs (deltaY) + skin, collisionMask)) {
				float dst = Vector3.Distance(ray.origin, hit.point);
				deltaY = (dst > skin) ? (dst - skin) * dir : 0;
				grounded = true;
				break;
			}
		}

		stopped = false;
		if(deltaX != 0){
			for(int i = 0;i<divY; i+=1){
				float dir = Mathf.Sign(deltaX);
				float x = p.x + c.x + s.x/2 * dir;
				float y = p.y + c.y - s.y/2 + s.y/(divY-1) * i;

				// Quick solution for unexpected issue
				if(flipX) x -= 0.3f;

				ray = new Ray(new Vector2(x,y), new Vector2(dir, 0));
				Debug.DrawRay(ray.origin,ray.direction);

				if(Physics.Raycast(ray, out hit, Mathf.Abs (deltaX) + skin, collisionMask)) {
					float dst = Vector3.Distance(ray.origin, hit.point);
					deltaX = (dst > skin) ? (dst - skin) * dir : 0;
					stopped = true;
					break;
				}
			}
		}

		// Literally the edge case
		if(!grounded && !stopped){
			Vector3 pdir = new Vector3(deltaX, deltaY);
			Vector3 o = new Vector3(p.x + c.x + s.x/2 * Mathf.Sign (deltaX), p.y + c.y + s.y/2 * Mathf.Sign (deltaY));

			// Quick solution for unexpected issue
			if(flipX) o.x -= 0.3f;

			ray = new Ray(o, pdir.normalized);

			Debug.DrawRay(o, ray.direction);
			if(Physics.Raycast(ray, Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY), collisionMask)) {
				grounded = true;
				deltaY = 0;
			}
		}

		if(climbing && deltaY != 0 && ladder_list.Count == 1) {
			foreach(Ladder l in ladder_list) {
				if(l.leader && l.transform.position.y + 3 < transform.position.y)
					deltaY = Mathf.Min (0, deltaY);
			}
		}
		Vector2 value = new Vector2(deltaX, deltaY);
		debug_value = ladder_list.Count;
		transform.Translate(value, Space.World);
	}

	public void SetCollider(Vector3 size, Vector3 centre) {
		collider.size = size;
		collider.center = centre;
		
		s = size * colliderScale;
		c = centre * colliderScale;
	}

	public void Grab(){
		Vector2 p = transform.position;
		p.x = 0;
		transform.position = p;
		climbing = true;
	}

	public bool canGrab(){
		if(!canClimb)
			return false;
		if(ladder_list.Count > 1)
			return true;
		foreach(Ladder l in ladder_list) {
			if(l.leader && l.transform.position.y + 3 < transform.position.y)
				return false;
		}
		return true;
	}

	public bool atEdge(){
		if(ladder_list.Count > 1)
			return false;
		foreach(Ladder l in ladder_list) {
			if(l.leader && l.transform.position.y + 1 < transform.position.y)
				return true;
		}
		return false;
	}

	void OnTriggerEnter(Collider c){
		if(c.gameObject.tag == "Ladder") {
			canClimb = true;
			ladder_list.Add (c.GetComponent<Ladder>());
		} else if(!invincible && c.gameObject.tag == "Death") {
			isDead = true;
		} else if(c.gameObject.tag == "FallDeath") {
			isDead = true;
			invincible = false;
		}
	}

	void OnTriggerExit(Collider c){
		if(c.gameObject.tag == "Ladder") {
			canClimb = false;
			ladder_list.Remove(c.GetComponent<Ladder>());
		}
	}
}
