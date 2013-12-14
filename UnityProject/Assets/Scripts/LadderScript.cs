using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LadderScript : MonoBehaviour {

	public Ladder _ladder_prefab;
	public Transform death_pos;

	private List<Ladder> list;
	private int ladder_count = 12;

	private float last_height = float.PositiveInfinity;
	private int last_index = 0;

	private Ladder leading_ladder;

	private static LadderScript instance;
	public static LadderScript Instance
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
		list = new List<Ladder>(ladder_count);
	}

	void Update () {
		if(last_height < death_pos.position.y - 10.0f) {
			StackLadder(last_index);
			SetMinimum();
		}
	}

	public void Initialize(Vector3 pos) {
		if(list.Count == 0) {
			for(var i=0; i<ladder_count; i++){
				Ladder newLadder = Instantiate(_ladder_prefab) as Ladder;
				newLadder.transform.parent = this.gameObject.transform;
				newLadder.gameObject.SetActive(false);
				list.Add(newLadder);
			}
		}

		for(var i=0; i<ladder_count; i++) {
			if(i == 0) {
				StackLadder(i, pos);
				last_height = leading_ladder.transform.position.y;
				last_index = i;
			}
			else
				StackLadder(i);
		}
	}

	public void Destroy(){
	}

	void SetMinimum(){
		last_height = float.PositiveInfinity;
		for(var i=0; i<ladder_count; i++){
			Ladder lad = list[i];
			if(last_height > lad.transform.position.y) {
				last_height = lad.transform.position.y;
				last_index = i;
			}
		}
	}

	void StackLadder(int index, Vector3 upper){
		Ladder lad = list[index];

		lad.setPosition(upper);
		lad.gameObject.SetActive(true);

		if(leading_ladder)
			leading_ladder.leader = false;

		leading_ladder = lad;
		leading_ladder.leader = true;
	}

	void StackLadder(int index){
		if(leading_ladder == null)
			Debug.LogError("No leader");
		StackLadder(index, leading_ladder.Upper);
	}
}
