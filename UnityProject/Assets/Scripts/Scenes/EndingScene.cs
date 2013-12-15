using UnityEngine;
using System.Collections;

public class EndingScene : Scene {
	public override void Initialize()
	{
		base.Initialize();
		if(GameEngine.Instance.TryAgain) {
			_isDone = true;
			next = SCENE_TYPE.LAST;
		}
	}
	SCENE_TYPE next = SCENE_TYPE.ENDING;
	public override SCENE_TYPE Next()
	{
		return next;
	}

	public override void Clean()
	{
		base.Clean();
	}
	
	// Update is called once per frame
	public override void UpdateScene () {
		base.UpdateScene();
	}
}
