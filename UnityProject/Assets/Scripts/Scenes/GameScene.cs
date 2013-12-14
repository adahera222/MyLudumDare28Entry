using UnityEngine;
using System.Collections;

public class GameScene : Scene {
	
	private CameraScript cam;
	private LadderScript ladder;
	private PlayerController player;

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
		if(!bossStarted && cam.transform.position.y > 20) {
			bossStarted = true;
			cam.moveCamera(-15.0f);
		}

		// Death check
		if(!isDead && player.IsDead) {
			isDead = true;
			player.Kill();
			lives--;
			StartCoroutine(HandleDeath());
		}


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
