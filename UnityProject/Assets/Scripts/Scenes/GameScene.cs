using UnityEngine;
using System.Collections;

public class GameScene : Scene {

	private bool started;
	private CameraScript cam;
	private LadderScript ladder;
	private PlayerController player;

	public override void Initialize()
	{
		base.Initialize();
		cam = CameraScript.Instance;
		ladder = LadderScript.Instance;
		player = PlayerController.Instance;

		ladder.Initialize(cam.deathBox.transform.position);
	}
	
	public override void Clean()
	{
		base.Clean();
	}
	
	// Update is called once per frame
	public override void UpdateScene () {
		base.UpdateScene();
		if(!started && player.climbing){
			started = true;
			cam.rising = true;
		}
	}
}
