using UnityEngine;
using System.Collections;

public class Scene : MonoBehaviour {
	protected bool _isDone;
	public bool isDone { get { return _isDone; } }

	public virtual void Awake ()
	{
		
	}
	
	public virtual void UpdateScene()
	{
		
	}
	
	public virtual void Initialize()
	{
		_isDone = false;
	}
	
	public virtual void Clean()
	{
		
	}
	
	public virtual void Pause()
	{
		
	}
	
	public virtual void Unpause()
	{
		
	}

	public virtual SCENE_TYPE Next()
	{
		return SCENE_TYPE.OPENING;
	}

	public void SceneFinished()
	{
		_isDone = true;
	}
}
