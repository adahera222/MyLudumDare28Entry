using UnityEngine;
using System.Collections;

public class Female : MonoBehaviour {
	public Transform player;
	public bool climbing;
	
	private float timer = 0;
	private bool moving;
	private bool running;
	private tk2dSpriteAnimator anim;
	public void Run(){
		anim = GetComponent<tk2dSpriteAnimator>();
		anim.Play ("she_run");
		running = true;
	}

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

		if(running){
			Vector3 delta = new Vector3(23.0f,0,0);
			transform.Translate(delta * Time.deltaTime, Space.World);

			if(transform.position.x > -45){
				GameEngine.Instance.Dialogue2("Ouch", 0.5f);
				AudioManager.Instance.playSound(Sfx.OUCH, transform.position);
				running = false;
				anim.Play("she_hurt");
			}
		}
	}
}
