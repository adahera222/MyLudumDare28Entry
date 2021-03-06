﻿using UnityEngine;
using System.Collections;

public class LastScene : Scene {
	private CameraScript cam;
	private PlayerController player;
	private GameEngine engine; // Fuck the police
	private BossScript boss;

	private bool cam_arrived;
	private bool player_arrived;
	private bool player_arrived2;
	private bool scene_started;

	Female _female_prefab;

	public override void Initialize()
	{
		base.Initialize();
		_female_prefab = (Resources.Load ("Prefabs/Female") as GameObject).GetComponent<Female>();
		cam = CameraScript.Instance;
		player = PlayerController.Instance;
		engine = GameEngine.Instance;
		boss = BossScript.Instance;

		cam.transform.position = new Vector3(100.0f, 0.0f, -10.0f);
		cam.rising = true;
		player.freeze = false;
		player.transform.position = new Vector3(115.0f, 0.0f, 0.0f);
		cam.FadeClear(1.0f);
		player.canControl = false;
		StartCoroutine(DelayedGo());
		engine.lastGround.gameObject.SetActive(false);
		engine.TryAgain = false;
		engine.PlayerGoFirst = false;
		engine.GoodEnding = false;

		boss.PrepareFinale();

		cam_arrived = false;
		player_arrived = false;
		player_arrived2 = false;
		scene_started = false;
		player.isAlone = false;
		player.IsDead = false;
	}

	IEnumerator DelayedGo(){
		yield return new WaitForSeconds(0.1f);
		player.AutoMove(0, 1.0f);
	}

	public override void Clean()
	{
		base.Clean();
	}
	
	public override SCENE_TYPE Next()
	{
		return SCENE_TYPE.ENDING;
	}
	
	// Update is called once per frame
	public override void UpdateScene () {
		base.UpdateScene();	
		if(!cam_arrived && cam.transform.position.y > 30){
			cam_arrived = true;
			cam.rising = false;
		}

		if(!player_arrived && player.transform.position.y > 23){
			player_arrived = true;
			Vector3 pos = player.transform.position;
			pos.y = 24;
			player.AutoMove(-1, 0);
			player.transform.position = pos;
			engine.lastGround.gameObject.SetActive(true);
			player.climbing = false;
		}

		if(player_arrived && !player_arrived2 && player.transform.position.x < 108){
			player_arrived2 = true;
			player.AutoMove(0, 0);
		}
	
		if(!scene_started && player_arrived2 && cam_arrived){
			scene_started = true;
			StartCoroutine(Cutscene());
		}
	}

	IEnumerator Cutscene(){
		engine.Dialogue("The exit is too small", 1.0f);
		yield return new WaitForSeconds(3.0f);
		engine.Dialogue("One of us has to go first", 1.0f);
		yield return new WaitForSeconds(3.0f);
		engine.Dialogue2("Press Left if she goes.", 1.5f);
		yield return new WaitForSeconds(0.5f);
		engine.Dialogue("Press Right if he goes.", 1.5f);
		yield return new WaitForSeconds(1.5f);
		engine.PlayerGoFirst = true;
		while(true){
			if(Input.GetKeyDown(KeyCode.LeftArrow)){
				engine.PlayerGoFirst = false;
				break;
			} else if(Input.GetKeyDown(KeyCode.RightArrow))
				break;
			yield return null;
		}
		yield return null;
		engine.Dialogue();
		engine.Dialogue2();
		yield return new WaitForSeconds(1.0f);
		player.isAlone = true;
		bool choice = engine.PlayerGoFirst;
		if(choice){
			Vector3 pos = new Vector3(105f, 23.56365f, -0.1f);
			Female fem = Instantiate(_female_prefab, pos, Quaternion.identity) as Female;
			fem.gameObject.SetActive(true);
			tk2dSpriteAnimator anim = fem.GetComponent<tk2dSpriteAnimator>();
			yield return null;
			anim.Play("she_idle");
			player.AutoMove(0, 0.3f);
			yield return new WaitForSeconds(1.0f);
			fem.climbing = true;
			anim.Play ("she_climb");
			yield return new WaitForSeconds(1.0f);
			cam.FadeDark(1.0f);
			yield return new WaitForSeconds(1.0f);
			player.AutoMove(0, 0);
			engine.Dialogue();
			_isDone = true;
		} else {
			Vector3 pos = new Vector3(105f, 25f, 0.1f);
			Female fem = Instantiate(_female_prefab, pos, Quaternion.identity) as Female;
			fem.gameObject.SetActive(true);
			fem.climbing = true;
			engine.Dialogue2("Drat! My leg!", 1.0f);
			yield return new WaitForSeconds(3.0f);
			engine.Dialogue2("Sorry. I'm slowing you down.", 1.0f);
			yield return new WaitForSeconds(1.0f);
			engine.Dialogue("Everything's gonna be alright.", 1.0f);
			yield return new WaitForSeconds(1.0f);
			engine.Dialogue2();
			yield return new WaitForSeconds(1.0f);
			engine.Dialogue();

			boss.EndingPos(cam.transform.position.y);
			engine.endingBox.SetActive(true);
			yield return new WaitForSeconds(2.0f);
			engine.Dialogue("Wait, what?", 0.8f);
			engine.lastTarget.SetActive(true);
			AudioManager.Instance.playSound(Sfx.LOCK, engine.lastTarget.transform.position);
			cam.chasing= true;
			player.canControl = true;
			float lasttimer = 0;
			while(true){
				lasttimer += Time.deltaTime;
				if(lasttimer > 5.0f){
					engine.Dialogue();
					engine.endingBox.SetActive(false);
					engine.lastTarget.SetActive(false);
					boss.EndingShot(engine.lastTarget.transform.position);
					fem.climbing = false;
					yield return new WaitForSeconds(0.3f);
					player.Kill();
					tk2dSpriteAnimator anim = fem.GetComponent<tk2dSpriteAnimator>();
					anim.Play ("she_death");
					yield return new WaitForSeconds(1.0f);
					cam.FadeDark(1.0f);
					yield return new WaitForSeconds(3.0f);
					engine.TryAgain = true;
					Destroy(fem.gameObject);
					cam.chasing= false;
					boss.force_stop = true;
					player.IsDead = false;
					player.freeze = true;
					player.transform.position = new Vector3(115.0f, 0.0f, 0.0f);
					player.Halt();
					break;
				}
				if(engine.GoodEnding){
					engine.Dialogue();
					cam.Fade(Color.white, 0.1f);
					yield return new WaitForSeconds(1.0f);
					engine.endingBox.SetActive(false);
					engine.lastTarget.SetActive(false);
					Destroy(fem.gameObject);
					cam.chasing= false;
					player.canControl = false;
					boss.force_stop = true;
					break;
				}
				yield return null;
			}
			_isDone = true;
		}
		yield return null;
	}


}
