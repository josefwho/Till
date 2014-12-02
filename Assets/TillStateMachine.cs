using UnityEngine;
using System.Collections;

public class TillStateMachine : MonoBehaviour {

	private FSMSystem machine;
	
	public void SetTransition(Transition t) { machine.PerformTransition(t); }

	// Use this for initialization
	void Start () 
	{
		IdleState idle = new IdleState();
		idle.AddTransition (Transition.ItemGrabbed, StateID.Drag);

		DragState drag = new DragState ();
		drag.AddTransition (Transition.ItemAtScanner, StateID.Scan);

		machine = new FSMSystem();
		machine.AddState (idle);
		machine.AddState (drag);

//		machine
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public class IdleState : FSMState
{
	public IdleState()
	{
		stateID = StateID.Idle;
	}

	public override void Reason(GameObject owner)
	{
		//todo: when item is grabbed transition to next state
		if (false) 
		{
			owner.GetComponent<TillStateMachine>().SetTransition(Transition.ItemGrabbed);
		}
	}

	public override void Act(GameObject owner)
	{
				//do nothing
	}
}

public class DragState : FSMState
{
	public DragState() {
		stateID = StateID.Drag;
	}

	public override void Reason(GameObject owner)
	{
		//todo: when item at scanner transition to next state
	}
	
	public override void Act(GameObject owner)
	{
		//TODO: count how many objects have hit the floor
	}
}


