using UnityEngine;
using System.Collections;

public class Female : MonoBehaviour {
	public Transform player;
	public bool climbing;

	private float timer = 0;
	private bool moving;
	void Update () {
		if(climbing){
			timer += Time.deltaTime;
			if(timer > 0.5f){
				moving = !moving;
				timer = 0;
			}
			if(moving) {
				Vector3 delta = new Vector3(0,2.0f,0);
				transform.Translate(delta * Time.deltaTime, Space.World);
			}

		}
	}
}
