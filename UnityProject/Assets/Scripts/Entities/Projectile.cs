using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	private float lifespan;
	private float speed;
	private Vector3 dir;

	public void Activate(float angle, float spd, float life){
		speed = spd;
		dir = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle), 0);
		lifespan = life;
	}

	public void Activate(Vector3 d, float spd, float life){
		speed = spd;
		dir = new Vector3(d.x, d.y, 0);
		lifespan = life;
	}

	public void Activate(Vector3 d, float angle, float spd, float life){
		speed = spd;
		dir = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle), 0).normalized;
		dir += new Vector3(d.x, d.y, 0).normalized;
		dir.Normalize();
		lifespan = life;
	}

	// Update is called once per frame
	void Update () {
		lifespan -= Time.deltaTime;
		transform.Translate(dir * speed, Space.World);
		if(lifespan < 0){
			Destroy(gameObject, 0.5f);
		}
	}
}
