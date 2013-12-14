using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public float riseSpeed = 1;
	public tk2dSprite deathBox;
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
	
	}
	
	// Update is called once per frame
	void Update () {
		if(rising) {
			Vector3 p = transform.position;
			p.y += riseSpeed * Time.deltaTime;
			transform.position = p;
		}
	}
}
