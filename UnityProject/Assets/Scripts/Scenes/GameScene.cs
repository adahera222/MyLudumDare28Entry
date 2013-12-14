using UnityEngine;
using System.Collections;

public class GameScene : Scene {

	MOVE_TYPE[] pattern_type = new MOVE_TYPE[10]{
		MOVE_TYPE.JUSTRISE,
		MOVE_TYPE.JUSTRISE,
		MOVE_TYPE.JUSTRISE,
		MOVE_TYPE.JUSTRISE,
		MOVE_TYPE.JUSTRISE,
		MOVE_TYPE.JUSTRISE,
		MOVE_TYPE.JUSTRISE,
		MOVE_TYPE.JUSTRISE,
		MOVE_TYPE.JUSTRISE,
		MOVE_TYPE.JUSTRISE
	};
	float[] pattern_delay = new float[10]{
		1.0f,
		3.0f,
		3.0f,
		3.0f,
		2.0f,
		2.0f,
		2.0f,
		2.0f,
		2.0f,
		2.0f,
	};

	private CameraScript cam;
	private LadderScript ladder;
	private PlayerController player;
	private BossScript boss;

	private int lives = 3;
	private bool isDead;
	private bool started;
	private bool bossStarted;

	public override void Initialize()
	{
		base.Initialize();
		cam = CameraScript.Instance;
		ladder = LadderScript.Instance;
		player = PlayerController.Instance;
		boss = BossScript.Instance;

		cam.transform.position = new Vector3(0.0f, 0.0f, -10.0f);
		player.transform.position = new Vector3(-6.0f, -1f, 0.0f);

		started = false;
		cam.rising = false;
		isDead = false;
		bossStarted = false;
		ladder.Initialize(cam.deathBox.transform.position);
		lives = 3;
	}
	
	public override void Clean()
	{
		base.Clean();
	}

	public void Restart()
	{
		cam.transform.position = new Vector3(0.0f, 0.0f, -10.0f);
		player.transform.position = new Vector3(-6.0f, -1f, 0.0f);

		started = false;
		cam.rising = false;
		isDead = false;
		bossStarted = false;
		ladder.Initialize(cam.deathBox.transform.position);
		lives = 3;
		player.Restart();
		cam.Restart();
		boss.Restart();
	}

	// Update is called once per frame
	public override void UpdateScene () {
		base.UpdateScene();
		// Mark the beginning
		if(!started && player.climbing){
			started = true;
			cam.rising = true;
		}

		// Mark the beginning of the boss
		if(!bossStarted && cam.transform.position.y > 25) {
			bossStarted = true;
			cam.moveCamera(-15.0f);
			StartCoroutine(BossPattern());
		}

		// Death check
		if(!isDead && player.IsDead) {
			isDead = true;
			player.Kill();
			lives--;
			StartCoroutine(HandleDeath());
		}


	}

	IEnumerator BossPattern(){
		boss.Activate();
		for(var i=0; i<pattern_type.Length; i+=1){
			boss.MakeMove(pattern_type[i], cam.transform.position.y);
			yield return new WaitForSeconds(pattern_delay[i]);
		}
		yield return null;
	}

	IEnumerator HandleDeath(){
		yield return new WaitForSeconds(3.0f);
		if(lives > 0){
			player.Respawn(cam.respawnBox.position);
			isDead = false;
		} else {
			cam.rising = false;
			Restart();
		}
	}
}
