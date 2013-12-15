using UnityEngine;
using System.Collections;

public class GameScene : Scene {

	MOVE_TYPE[] pattern_type = new MOVE_TYPE[9]{
		MOVE_TYPE.BULLETHELL1,
		MOVE_TYPE.LASEREYE1,
		MOVE_TYPE.SKYROCKET1,
		MOVE_TYPE.BULLETHELL2,
		MOVE_TYPE.LASEREYE2,
		MOVE_TYPE.SKYROCKET2,
		MOVE_TYPE.BULLETHELL3,
		MOVE_TYPE.LASEREYE3,
		MOVE_TYPE.SKYROCKET3,
	};

	private CameraScript cam;
	private LadderScript ladder;
	private PlayerController player;
	private BossScript boss;

	private int lives = 3;
	private bool isDead;
	private bool started;
	private bool bossStarted;
	private AudioSource source;
	public override void Initialize()
	{
		base.Initialize();
		cam = CameraScript.Instance;
		ladder = LadderScript.Instance;
		player = PlayerController.Instance;
		boss = BossScript.Instance;

		cam.transform.position = new Vector3(0.0f, 0.0f, -10.0f);
		player.transform.position = new Vector3(-7.611201f, -6.845f, 0.0f);

		started = false;
		cam.rising = false;
		isDead = false;
		bossStarted = false;
		ladder.Initialize(cam.deathBox.transform.position);
		lives = 3;
		player.isAlone = false;
		player.canControl = true;
		player.autoMove = false;
		cam.FadeClear(1.0f);
		GameEngine.Instance.invisibleWall.SetActive(true);

		source = Camera.main.GetComponent<AudioSource>();
	}
	
	public override void Clean()
	{
		player.Restart();
		cam.Restart();
		boss.Restart();
		base.Clean();
	}

	public override SCENE_TYPE Next()
	{
		return SCENE_TYPE.LAST;
	}

	public void Restart()
	{
		cam.transform.position = new Vector3(0.0f, 0.0f, -10.0f);
		player.transform.position = new Vector3(-6.0f, -6.845f, 0.0f);

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
			source.Play ();
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
			boss.MakeMove(pattern_type[i]);
			while(boss.makingMove){
				if(lives == 0)
					yield break;
				yield return null;
			}
		}
		yield return new WaitForSeconds(5.0f);
		cam.rising = false;
		cam.FadeDark(2.0f);
		player.canControl = false;
		player.AutoMove(0, 0.5f);
		yield return new WaitForSeconds(2.0f);
		player.AutoMove(0, 0);
		source.Stop ();
		_isDone = true;
		yield return null;
	}

	IEnumerator HandleDeath(){
		yield return new WaitForSeconds(1.0f);
		if(lives > 0){
			player.Respawn(cam.respawnBox.position);
			isDead = false;
		} else {
			source.Stop ();
			cam.rising = false;
			cam.FadeDark(2.0f);
			yield return new WaitForSeconds(2.0f);
			cam.FadeClear(2.0f);
			Restart();
		}
	}
}
