using UnityEngine;
using System.Collections;

public class OpeningScene : Scene {
	
	private CameraScript cam;
	private PlayerController player;
	private GameEngine engine;
	private Female _female_prefab;
	private Female fem;

	private bool credit;
	private bool started;
	private bool camera_picked;
	private bool pickup;
	private bool lastpart;
	public override void Initialize()
	{
		base.Initialize();
		cam = CameraScript.Instance;
		player = PlayerController.Instance;
		engine = GameEngine.Instance;
		_female_prefab = (Resources.Load ("Prefabs/Female") as GameObject).GetComponent<Female>();
		
		fem = Instantiate(_female_prefab, new Vector3(), Quaternion.identity) as Female;
		fem.gameObject.SetActive(true);
		fem.transform.position = new Vector3(-160, -6.845f, 0);
		cam.transform.position = new Vector3(-130.0f, 0.0f, -10.0f);
		player.transform.position = new Vector3(-170, -6.845f, 0);
		player.isAlone = true;

		player.canControl = false;
		StartCoroutine(Credit ());
	}

	public override SCENE_TYPE Next()
	{
		return SCENE_TYPE.GAME;
	}
	
	public override void Clean()
	{
		base.Clean();
	}

	IEnumerator Credit(){
		yield return new WaitForSeconds(1.0f);
		engine.SetLogo("J_");
		yield return new WaitForSeconds(0.2f);
		engine.SetLogo("JW_");
		yield return new WaitForSeconds(1.0f);
		engine.SetLogo("J_");
		yield return new WaitForSeconds(0.2f);
		engine.SetLogo("_");
		yield return new WaitForSeconds(0.2f);
		engine.SetLogo("");
		yield return new WaitForSeconds(0.2f);
		cam.FadeClear(1.0f);
		credit = true;
	}

	// Update is called once per frame
	public override void UpdateScene () {
		base.UpdateScene();

		if(!credit)
			return;

		if(!started){
			started = true;
			StartCoroutine(Opening());
		}

		if(!camera_picked && player.transform.position.x > -130){
			camera_picked = true;
			cam.chasingX = true;
		}

		if(!pickup && player.transform.position.x > -46){
			pickup = true;
			player.autoMove = false;
			player.AutoMove(0, 0);
			StartCoroutine(Pickup());
		}

		if(!lastpart && player.transform.position.x > -10){
			lastpart = true;
			player.autoMove = false;
			player.AutoMove(0, 0);
			StartCoroutine(Lastpart ());
		}
	}

	IEnumerator Opening(){
		player.autoMove = true;
		player.AutoMove(1.0f, 0);
		AudioManager.Instance.playSound(Sfx.EARTHQUAKE, Camera.main.transform.position);
		cam.Shake();
		fem.Run();
		yield return null;
	}

	IEnumerator Pickup(){
		engine.Dialogue2("Oh no. This is really bad.", 1.0f);
		yield return new WaitForSeconds(1.0f);
		engine.Dialogue("Don't worry. I got this.", 1.0f);
		yield return new WaitForSeconds(2.0f);
		engine.Dialogue2();
		yield return new WaitForSeconds(1.0f);
		engine.Dialogue();
		AudioManager.Instance.playSound(Sfx.PIGGYBACK, Camera.main.transform.position);
		Destroy(fem.gameObject);
		player.isAlone = false;
		yield return new WaitForSeconds(1.0f);
		player.autoMove = true;
		player.AutoMove(1.0f, 0);
	}

	IEnumerator Lastpart(){
		engine.Dialogue2("This is it.", 1.0f);
		yield return new WaitForSeconds(2.0f);
		engine.Dialogue2("The one and only exit.", 1.0f);
		yield return new WaitForSeconds(3.0f);
		engine.Dialogue2("We must escape\nbefore It gets us.", 1.5f);
		yield return new WaitForSeconds(3.0f);
		engine.Dialogue2();
		cam.chasingX = false;
		cam.moveToOrigin();
		yield return new WaitForSeconds(1.0f);
		_isDone = true;
	}
}
