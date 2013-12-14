using UnityEngine;
using System.Collections;


// As inpired by YouTube Platformer Tutorial
[RequireComponent (typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {
	public LayerMask collisionMask;

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
	public bool grounded;
	[HideInInspector]
	public bool stopped;

	Ray ray;
	RaycastHit hit;

	// Use this for initialization
	void Start () {
		collider = GetComponent<BoxCollider>();
		colliderScale = transform.localScale.x;

		originalSize = collider.size;
		originalCentre = collider.center;
		SetCollider(originalSize,originalCentre);
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
			ray = new Ray(o, pdir.normalized);

			if(Physics.Raycast(ray, Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY), collisionMask)) {
				grounded = true;
				deltaY = 0;
			}
		}

		Vector2 value = new Vector2(deltaX, deltaY);
		transform.Translate(value, Space.World);
	}

	public void SetCollider(Vector3 size, Vector3 centre) {
		collider.size = size;
		collider.center = centre;
		
		s = size * colliderScale;
		c = centre * colliderScale;
	}
}
