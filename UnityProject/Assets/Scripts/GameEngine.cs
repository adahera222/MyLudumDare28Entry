using UnityEngine;
using System.Collections;

public enum SCENE_TYPE {
	OPENING, TITLE, GAME, LAST, ENDING
};

public class GameEngine : MonoBehaviour {

	private Hashtable sceneList = new Hashtable();
	private Scene currentScene;
	private SCENE_TYPE currentSceneType;

	void Awake() {
		sceneList.Add (SCENE_TYPE.OPENING, gameObject.AddComponent<OpeningScene>());
		sceneList.Add (SCENE_TYPE.TITLE, gameObject.AddComponent<TitleScene>());
		sceneList.Add (SCENE_TYPE.GAME, gameObject.AddComponent<GameScene>());
		sceneList.Add (SCENE_TYPE.LAST, gameObject.AddComponent<LastScene>());
		sceneList.Add (SCENE_TYPE.ENDING, gameObject.AddComponent<EndingScene>());
	}

	// Use this for initialization
	void Start () {
		StartScene(SCENE_TYPE.GAME);
	}
	
	// Update is called once per frame
	void Update () {
		if(currentScene)
			currentScene.UpdateScene();
	}

	void StartScene(SCENE_TYPE type){
		if((Scene)sceneList[type]) {
			if(currentScene)
				currentScene.Clean();

			currentScene = (Scene)sceneList[type];
			currentSceneType = type;
			currentScene.Initialize();
		}
	}
}
