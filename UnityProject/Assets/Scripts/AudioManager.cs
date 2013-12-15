using UnityEngine;
using System.Collections;

// This is what you get for coding at 5 a.m.
public enum Sfx {
	JUMP, OUCH, MALE, FEMALE, EARTHQUAKE, ENGINE, EXP, LASER, LAUNCH, LOCK, ROCKET, DEATH, BULLET, ROCKFALL, FALL, LANDED, DESPAIR, HEADBUTT, PIGGYBACK, PATRIOT, PAVANNE
}

public class AudioManager : MonoBehaviour {

	private static AudioManager instance;
	public AudioClip jump;
	public AudioClip ouch;
	public AudioClip male;
	public AudioClip female;
	public AudioClip earthquake;
	public AudioClip engine;
	public AudioClip exp;
	public AudioClip laser;
	public AudioClip launch;
	public AudioClip lockon;
	public AudioClip rocket;
	public AudioClip death;
	public AudioClip bullet;
	public AudioClip rockfall;
	public AudioClip fall;
	public AudioClip landed;
	public AudioClip despair;
	public AudioClip headbutt;
	public AudioClip piggyback;
	public AudioClip patriot;
	public AudioClip pavanne;

	public static AudioManager Instance
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
	}

	public void playSound(Sfx id, Vector3 pos){
		switch(id){
		case Sfx.JUMP:
			AudioSource.PlayClipAtPoint(jump, pos);
			break;
		case Sfx.OUCH:
			AudioSource.PlayClipAtPoint(ouch, pos);
			break;
		case Sfx.MALE:
			AudioSource.PlayClipAtPoint(male, pos);
			break;
		case Sfx.FEMALE:
			AudioSource.PlayClipAtPoint(female, pos);
			break;
		case Sfx.EARTHQUAKE:
			AudioSource.PlayClipAtPoint(earthquake, pos);
			break;
		case Sfx.ENGINE:
			AudioSource.PlayClipAtPoint(engine, pos);
			break;
		case Sfx.EXP:
			AudioSource.PlayClipAtPoint(exp, pos);
			break;
		case Sfx.LASER:
			AudioSource.PlayClipAtPoint(laser, pos);
			break;
		case Sfx.LAUNCH:
			AudioSource.PlayClipAtPoint(launch, pos);
			break;
		case Sfx.LOCK:
			AudioSource.PlayClipAtPoint(lockon, pos);
			break;
		case Sfx.ROCKET:
			AudioSource.PlayClipAtPoint(rocket, pos);
			break;
		case Sfx.DEATH:
			AudioSource.PlayClipAtPoint(death, pos);
			break;
		case Sfx.BULLET:
			AudioSource.PlayClipAtPoint(bullet, pos);
			break;
		case Sfx.ROCKFALL:
			AudioSource.PlayClipAtPoint(rockfall, pos);
			break;
		case Sfx.FALL:
			AudioSource.PlayClipAtPoint(fall, pos);
			break;
		case Sfx.LANDED:
			AudioSource.PlayClipAtPoint(landed, pos);
			break;
		case Sfx.DESPAIR:
			AudioSource.PlayClipAtPoint(despair, pos);
			break;
		case Sfx.HEADBUTT:
			AudioSource.PlayClipAtPoint(headbutt, pos);
			break;
		case Sfx.PIGGYBACK:
			AudioSource.PlayClipAtPoint(piggyback, pos);
			break;
		case Sfx.PATRIOT:
			AudioSource.PlayClipAtPoint(patriot, pos);
			break;
		case Sfx.PAVANNE:
			AudioSource.PlayClipAtPoint(pavanne, pos);
			break;
		}
	}
}
