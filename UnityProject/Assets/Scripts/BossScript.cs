using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public enum MOVE_TYPE {
	JUSTRISE,
	BULLETHELL,
	LASEREYE,
	SKYROCKET,
	SKYROCKET2
};

public class BossScript : MonoBehaviour {

	public GameObject Boss;

	private static BossScript instance;
	public static BossScript Instance
	{
		get {
			if(instance == null)
			{
				Debug.LogError ("Error");
			}
			return instance;
		}
	}
	
	void Awake () {
		instance = this;
	}

	public void Restart(){
		Boss.SetActive(false);
		Boss.transform.position = new Vector3(-26.90941f, 0, 0);
	}

	public void Activate(){
		Boss.SetActive(true);
	}

	public void MakeMove(MOVE_TYPE type, float py) {
		switch(type){
		case MOVE_TYPE.JUSTRISE:
			StartCoroutine(JustRise(py));
			break;
		case MOVE_TYPE.BULLETHELL:
			break;
		case MOVE_TYPE.LASEREYE:
			break;
		case MOVE_TYPE.SKYROCKET:
			break;
		case MOVE_TYPE.SKYROCKET2:
			break;
		default:
			break;
		}
	}

	IEnumerator JustRise(float py){
		yield return StartCoroutine(Rise(py));
	}

	IEnumerator Rise(float py){
		float deltay = py - Boss.transform.position.y + 20;
		Tweener twn = HOTween.To (Boss.transform, 1.5f, new TweenParms().Prop ("position", new Vector3(0, deltay, 0), true).Ease(EaseType.EaseInBack));
		while(!twn.isComplete) {
			yield return null;
		}
	}
}
