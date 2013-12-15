using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public class EndingScene : Scene {
	private bool endingPlayed = false;
	private CameraScript cam;

	public override void Initialize()
	{
		base.Initialize();
		cam = CameraScript.Instance;
		endingPlayed = false;
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
		if(!endingPlayed){
			endingPlayed = true;
			if(GameEngine.Instance.PlayerGoFirst)
				StartCoroutine(Ending1());
			else if(GameEngine.Instance.GoodEnding)
				StartCoroutine(Ending2());
		}
	}

	IEnumerator Ending1(){
		cam.transform.position = new Vector3(222, 30, -10);
		GameObject obj = GameEngine.Instance.ending1;
		tk2dSprite head = obj.transform.Find("Exithead").GetComponent<tk2dSprite>();
		Female fem = obj.transform.Find ("Female").GetComponent<Female>();
		tk2dSpriteAnimator bad = obj.transform.Find ("BadEnding").GetComponent<tk2dSpriteAnimator>();
		GameObject scope = obj.transform.Find ("Scope").gameObject;
		scope.SetActive(false);
		fem.transform.localPosition = new Vector3(0, -3.77383f, 0.2f);
		head.SetSprite("exit_head1");
		cam.FadeClear(1.0f);
		yield return new WaitForSeconds(1.0f);
		fem.climbing = true;
		yield return new WaitForSeconds(2.0f);
		scope.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		head.SetSprite("exit_head2");
		yield return new WaitForSeconds(1.5f);
		fem.climbing = false;
		cam.transform.position = new Vector3(222, 75, -10);
		bad.Play("Ending");
		yield return null;
	}

	IEnumerator Ending2(){
		cam.transform.position = new Vector3(319, 30, -10);
		cam.Shake();
		GameObject obj = GameEngine.Instance.ending2;
		tk2dSpriteAnimator anim = obj.GetComponent<tk2dSpriteAnimator>();
		anim.Play("ending");
		HOTween.To (obj.transform, 5.0f, new TweenParms().Prop ("position", new Vector3(0, 5, 0), true).Ease(EaseType.Linear));
		cam.FadeClear(1.0f);
		yield return new WaitForSeconds(5.0f);
		cam.FadeDark(1.0f);
		yield return new WaitForSeconds(1.0f);
		cam.Fade(Color.black, 1.0f);
		yield return new WaitForSeconds(1.0f);
		cam.transform.position = new Vector3(319, 64, -10);
		cam.FadeClear(3.0f);
		yield return null;
	}
}
