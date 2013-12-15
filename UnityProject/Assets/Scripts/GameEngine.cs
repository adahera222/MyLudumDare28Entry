using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public enum SCENE_TYPE {
	OPENING, GAME, LAST, ENDING
};

public class GameEngine : MonoBehaviour {

	private Hashtable sceneList = new Hashtable();
	private Scene currentScene;

	// Fuck organization. Let's just make it work.
	public tk2dSprite lastGround;
	public tk2dTextMesh dialogue;
	public tk2dTextMesh dialogue2;
	public tk2dTextMesh logoText;
	public GameObject lastTarget;
	public GameObject endingBox;

	public GameObject ending1;
	public GameObject ending2;

	public GameObject invisibleWall;

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
		sceneList.Add (SCENE_TYPE.GAME, gameObject.AddComponent<GameScene>());
		sceneList.Add (SCENE_TYPE.LAST, gameObject.AddComponent<LastScene>());
		sceneList.Add (SCENE_TYPE.ENDING, gameObject.AddComponent<EndingScene>());
	}

	// Use this for initialization
	void Start () {
		StartScene(SCENE_TYPE.OPENING);
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
		AudioManager.Instance.playSound(Sfx.MALE, Camera.main.transform.position);
		dialogue.text = "";
		dialogue.Commit();
		HOTween.To (dialogue, dur, new TweenParms().Prop ("text", text).Ease(EaseType.Linear).OnUpdate(()=>{ dialogue.Commit(); }));
	}

	public void Dialogue(){
		dialogue.text = "";
		dialogue.Commit();
	}

	public void Dialogue2(string text, float dur){
		if(text != "Ouch")
			AudioManager.Instance.playSound(Sfx.FEMALE, Camera.main.transform.position);
		dialogue2.text = "";
		dialogue2.Commit();
		HOTween.To (dialogue2, dur, new TweenParms().Prop ("text", text).Ease(EaseType.Linear).OnUpdate(()=>{ dialogue2.Commit(); }));
	}
	
	public void Dialogue2(){
		dialogue2.text = "";
		dialogue2.Commit();
	}

	public void SetLogo(string text){
		logoText.text = text;
		logoText.Commit ();
	}
}
