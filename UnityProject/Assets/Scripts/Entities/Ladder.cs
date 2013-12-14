using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {

	private tk2dSprite sprite;
	public bool leader;

	void Awake(){
		sprite = GetComponent<tk2dSprite>();
	}

	public Vector3 Upper {
		get {return transform.position + sprite.GetBounds().size;}
	}

	public void setPosition(Vector3 lower) {
		transform.position = new Vector3(0, lower.y, 1);
	}
}
