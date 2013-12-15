using UnityEngine;
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
		engine.Dialogue();
		yield return StartCoroutine(engine.DisplayChoice());

		bool choice = engine.PlayerGoFirst;
		if(choice){
			Vector3 pos = new Vector3(105f, 23f, 0.1f);
			Female fem = Instantiate(_female_prefab, pos, Quaternion.identity) as Female;
			fem.gameObject.SetActive(true);
			engine.Dialogue("I'll help you up there", 1.0f);
			player.AutoMove(0, 0.5f);
			yield return new WaitForSeconds(2.0f);
			engine.Dialogue();
			cam.FadeDark(3.0f);
			_isDone = true;
		} else {
			Vector3 pos = new Vector3(105f, 25f, 0.1f);
			Female fem = Instantiate(_female_prefab, pos, Quaternion.identity) as Female;
			fem.gameObject.SetActive(true);
			fem.climbing = true;
			engine.Dialogue("Curse my leg", 1.0f);
			yield return new WaitForSeconds(2.0f);
			engine.Dialogue("I can't climb fast", 1.0f);
			yield return new WaitForSeconds(2.0f);
			engine.Dialogue("Don't worry. Take your time", 1.0f);
			yield return new WaitForSeconds(1.0f);
			cam.chasing= true;
			player.canControl = true;
			boss.EndingPos(cam.transform.position.y);
			engine.endingBox.SetActive(true);
			yield return new WaitForSeconds(2.0f);
			engine.Dialogue("Wait, what?", 0.8f);
			engine.lastTarget.SetActive(true);
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
					yield return new WaitForSeconds(1.0f);
					cam.FadeDark(1.0f);
					yield return new WaitForSeconds(3.0f);
					engine.TryAgain = true;
					Destroy(fem.gameObject);
					cam.chasing= false;
					boss.force_stop = true;
					break;
				}
				if(engine.GoodEnding){
					engine.Dialogue();
					cam.Fade(Color.white, 0.1f);
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
