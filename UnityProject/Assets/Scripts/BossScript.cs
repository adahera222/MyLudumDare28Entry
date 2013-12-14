using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public enum MOVE_TYPE {
	JUSTRISE,
	BULLETHELL,
	LASEREYE,
	SKYROCKET,
	SKYROCKET2
};

public class BossScript : MonoBehaviour {

	public GameObject Boss;
	public Projectile _Bullet_Prefab;
	public GameObject _Rock_Prefab;
	public GameObject _Rocket_Prefab;
	public GameObject _Laser_Prefab;

	public Transform bullet_nozzle;
	public Transform laser_eye;

	public Transform player_pos;

	public tk2dSprite thrust1;
	public tk2dSprite thrust2;

	private bool rising;
	private static BossScript instance;
	public static BossScript Instance
	{
		get {
			if(instance == null)
			{
				Debug.LogError ("Error");
			}
			return instance;
		}
	}
	
	void Awake () {
		instance = this;
	}

	public void Restart(){
		Boss.SetActive(false);
		Boss.transform.position = new Vector3(-26.90941f, 0, 0);
	}

	public void Activate(){
		Boss.SetActive(true);
	}

	public void MakeMove(MOVE_TYPE type, float cy) {
		switch(type){
		case MOVE_TYPE.JUSTRISE:
			StartCoroutine(JustRise(cy));
			break;
		case MOVE_TYPE.BULLETHELL:
			StartCoroutine(RiseAndShoot(cy));
			break;
		case MOVE_TYPE.LASEREYE:
			break;
		case MOVE_TYPE.SKYROCKET:
			break;
		case MOVE_TYPE.SKYROCKET2:
			break;
		default:
			break;
		}
	}

	IEnumerator JustRise(float cy){
		yield return StartCoroutine(Rise(cy));
	}

	IEnumerator RiseAndShoot(float cy){
		yield return StartCoroutine(Rise(cy));

		Shoot(0f, 0.35f);
		Shoot(15f, 0.35f);
		Shoot(-15f, 0.35f);
		yield return new WaitForSeconds(0.5f);
		Shoot(0f, 0.35f);
		Shoot(15f, 0.35f);
		Shoot(-15f, 0.35f);
		yield return new WaitForSeconds(0.5f);
		Shoot(0f, 0.35f);
		Shoot(15f, 0.35f);
		Shoot(-15f, 0.35f);
	}

	IEnumerator Rise(float py){
		rising = true;
		float deltay = py - Boss.transform.position.y + 20;
		StartCoroutine(Thrust());
		Tweener twn = HOTween.To (Boss.transform, 2.0f, new TweenParms().Prop ("position", new Vector3(0, deltay, 0), true));
		while(!twn.isComplete) {
			yield return null;
		}
		rising = false;
	}

	private void Shoot(float angle, float speed){

		Projectile pj = Instantiate(_Bullet_Prefab, new Vector3(bullet_nozzle.position.x, bullet_nozzle.position.y, 0), Quaternion.identity) as Projectile;
		pj.gameObject.SetActive(true);
		pj.Activate(GetAngle() + angle, speed, 3);
	}

	private float GetAngle(){
		Vector3 dir = player_pos.position - bullet_nozzle.position;
		Vector2 from = Vector2.up;
		Vector2 to = new Vector2(dir.x, dir.y);

		float ang = Vector2.Angle(from, to);
		Vector3 cross = Vector3.Cross(from, to);
		if(cross.z > 0)
			ang = 360 - ang;

		return ang;
	}

	private float thrustCounter;
	IEnumerator Thrust(){
		Color col;
		while(rising){
			thrustCounter += Time.deltaTime;
			if(thrustCounter > 0.06f){
				col = thrust1.color;
				col.a = col.a == 0 ? 1.0f : 0.0f;
				thrust1.color = col;
				thrust2.color = col;
				thrustCounter = 0.0f;
			}
			yield return null;
		}
		col = thrust1.color;
		col.a = 0.0f;
		thrust1.color = col;
		thrust2.color = col;
		yield return null;
	}
}
