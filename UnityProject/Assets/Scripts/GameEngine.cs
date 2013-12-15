using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public enum SCENE_TYPE {
	OPENING, TITLE, GAME, LAST, ENDING
};

public class GameEngine : MonoBehaviour {

	private Hashtable sceneList = new Hashtable();
	private Scene currentScene;

	// Fuck organization. Let's just make it work.
	public tk2dSprite lastGround;
	public tk2dTextMesh dialogue;
	public tk2dTextMesh logoText;
	public GameObject choicePic;
	public GameObject lastTarget;
	public GameObject endingBox;

	[HideInInspector]
	public bool PlayerGoFirst;
	[HideInInspector]
	public bool GoodEnding;
	[HideInInspector]
	public bool TryAgain;

	private static GameEngine instance;
	
	public static GameEngine Instance
	{
		get {
			if(instance == null)
			{
				Debug.LogError ("Error");
			}
			return instance;
		}
	}

	void Awake() {
		instance = this;
		lastGround.gameObject.SetActive(false);
		sceneList.Add (SCENE_TYPE.OPENING, gameObject.AddComponent<OpeningScene>());
		sceneList.Add (SCENE_TYPE.TITLE, gameObject.AddComponent<TitleScene>());
		sceneList.Add (SCENE_TYPE.GAME, gameObject.AddComponent<GameScene>());
		sceneList.Add (SCENE_TYPE.LAST, gameObject.AddComponent<LastScene>());
		sceneList.Add (SCENE_TYPE.ENDING, gameObject.AddComponent<EndingScene>());
	}

	// Use this for initialization
	void Start () {
		StartScene(SCENE_TYPE.LAST);
	}
	
	// Update is called once per frame
	void Update () {
		if(currentScene)
			currentScene.UpdateScene();

		if(currentScene && currentScene.isDone)
			StartScene(currentScene.Next());
	}

	void StartScene(SCENE_TYPE type){
		if((Scene)sceneList[type]) {
			if(currentScene)
				currentScene.Clean();

			currentScene = (Scene)sceneList[type];
			currentScene.Initialize();
		}
	}

	public void Dialogue(string text, float dur){
		dialogue.text = "";
		dialogue.Commit();
		HOTween.To (dialogue, dur, new TweenParms().Prop ("text", text).Ease(EaseType.Linear).OnUpdate(()=>{ dialogue.Commit(); }));
	}

	public void Dialogue(){
		dialogue.text = "";
		dialogue.Commit();
	}

	public IEnumerator DisplayChoice(){
		PlayerGoFirst = false;
		choicePic.SetActive(true);
		while(true){
			if(Input.GetKeyDown(KeyCode.LeftArrow)){
				PlayerGoFirst = true;
				break;
			} else if(Input.GetKeyDown(KeyCode.RightArrow))
				break;
			yield return null;
		}
		choicePic.SetActive(false);
		yield return null;
	}
}
