using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public class Projectile : MonoBehaviour {

	private float lifespan;
	private float speed;
	private Vector3 dir;

	private bool firing;

	public void Fire(float angle, float spd, float life){
		firing = true;
		speed = spd;
		dir = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle), 0);
		lifespan = life;
	}

	public void Aim(float aim, float delay, Transform player){
		aimTimer = 0;
		StartCoroutine(AimAndDisappear(aim, delay, player));
	}

	public void Laser(Vector3 pos, Projectile exp){
		StartCoroutine(LaserAndExplosion(pos, exp));
	}

	public void Explode(){
		StartCoroutine(Explosion());
	}

	float aimTimer;
	IEnumerator AimAndDisappear(float aim, float delay, Transform player) {
		while(aimTimer < aim){
			Vector3 ppos = player.position;
			ppos.z = -0.1f;
			transform.position = ppos;

			aimTimer+= Time.deltaTime;
			yield return null;
		}
		Vector3 pos = player.position;
		pos.z = -0.1f;
		while(aimTimer < aim + delay){
			transform.position = pos;
			aimTimer+= Time.deltaTime;
			yield return null;
		}
		Destroy(gameObject, 0.5f);
	}

	IEnumerator LaserAndExplosion(Vector3 pos, Projectile exp){
		Tweener twn = HOTween.To (transform, 0.4f, new TweenParms().Prop ("position", pos));
		while(!twn.isComplete) {
			yield return null;
		}
		Projectile pj = Instantiate(exp, new Vector3(pos.x, pos.y, 0), Quaternion.identity) as Projectile;
		pj.gameObject.SetActive(true);
		pj.Explode();
		Destroy(gameObject);
	}

	IEnumerator Explosion(){
		tk2dSpriteAnimator anim = GetComponent<tk2dSpriteAnimator>();
		anim.Play("Explosion");
		while(anim.IsPlaying("Explosion")){
			yield return null;
		}
		Destroy(gameObject);
	}

	// Update is called once per frame
	void Update () {
		if(firing){
			lifespan -= Time.deltaTime;
			transform.Translate(dir * speed, Space.World);
			if(lifespan < 0){
				Destroy(gameObject, 0.5f);
			}
		}
	}
}
