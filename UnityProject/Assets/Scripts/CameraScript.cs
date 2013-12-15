using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public class CameraScript : MonoBehaviour {

	public float riseSpeed = 1;
	public tk2dSprite deathBox;
	public Transform respawnBox;
	public Transform player;
	public GameObject leftPillar;
	public GameObject rightPillar;
	public tk2dSprite fade;

	private float shake_decay;
	private float shake_intensity;
	private Vector3 originalPosition;
	private Vector3 vel = Vector3.zero;

	[HideInInspector]
	public bool rising;
	[HideInInspector]
	public bool shaking;
	[HideInInspector]
	public bool chasing;

	private static CameraScript instance;

	public static CameraScript Instance
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
		fade.gameObject.SetActive(true);
	}

	// Use this for initialization
	void Start () {
		leftPillar.SetActive(false);
		rightPillar.SetActive(false);
	}

	public void Restart() {
		rising = false;
		leftPillar.SetActive(false);
		rightPillar.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		if(rising) {
			Vector3 p = transform.position;
			p.y += riseSpeed * Time.deltaTime;
			transform.position = p;

			if(shaking)
				originalPosition.y += riseSpeed * Time.deltaTime;
		}

		if(shaking){
			if(shake_intensity>0){
				Vector3 pos = originalPosition + Random.insideUnitSphere * shake_intensity;
				pos.z = originalPosition.z;
				transform.position = pos;
				shake_intensity -= shake_decay;
			} else {
				shaking = false;
				transform.position = originalPosition;
			}
		}

		if(chasing) {
			Vector3 pos = player.position;
			pos.z = transform.position.z;
			transform.position = Vector3.SmoothDamp(transform.position, pos, ref vel, 0.2f);
		}
	}

	public void moveCamera(float x) {
		StartCoroutine(MoveX (x));
	}

	IEnumerator MoveX(float x) {
		Tweener twn = HOTween.To (camera.transform, 1.0f, new TweenParms().Prop ("position", new Vector3(x, 0, 0), true).Ease(EaseType.EaseInQuad));
		while(!twn.isComplete) {
			yield return null;
		}
		leftPillar.SetActive(true);
		rightPillar.SetActive(true);
	}

	public void Shake(){
		originalPosition = transform.position;
		shake_intensity = .3f;
		shake_decay = 0.002f;
		shaking = true;
	}

	public void LongShake(){
		originalPosition = transform.position;
		shake_intensity = .5f;
		shake_decay = 0.001f;
		shaking = true;
	}

	public void Fade(Color end, float duration){
		StartCoroutine(DoFade(end, duration));
	}

	public void FadeClear(float duration){
		Color color = fade.color;
		color.a = 0;
		StartCoroutine(DoFade(color, duration));
	}

	public void FadeDark(float duration){
		Color color = fade.color;
		color.a = 1;
		StartCoroutine(DoFade(color, duration));
	}

	IEnumerator DoFade(Color end, float d){
		Tweener twn = HOTween.To (fade, d, new TweenParms().Prop("color", end));
		while(!twn.isComplete){
			yield return null;
		}
	}
}
