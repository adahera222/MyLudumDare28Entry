using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public class CameraScript : MonoBehaviour {

	public float riseSpeed = 1;
	public tk2dSprite deathBox;
	public Transform respawnBox;
	public GameObject leftPillar;
	public GameObject rightPillar;
	[HideInInspector]
	public bool rising;

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
	}

	// Use this for initialization
	void Start () {
		leftPillar.SetActive(false);
		rightPillar.SetActive(false);
	}

	public void Restart() {
		leftPillar.SetActive(false);
		rightPillar.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		if(rising) {
			Vector3 p = transform.position;
			p.y += riseSpeed * Time.deltaTime;
			transform.position = p;
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
}
