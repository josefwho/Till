using UnityEngine;
using System.Collections;

public enum States
{
	Setup = 0,
	Idle,
	Drag,
	Scan,
	Throw,
	Done
}

public class TillStateMachine : MonoBehaviour 
{

	public bool setupDone;
	public bool itemGrabbed;
	public bool itemAtScanner;
	public bool itemScanned;
	public bool itemInBasket;
	public bool itemOnFloor;

	public States currentState;

	void Start()
	{
		currentState = States.Setup;

		setupDone = true;
	}

	void Update ()
	{
		if (currentState == States.Setup && setupDone) 
			switchToState (States.Idle);
		else if (currentState == States.Idle && itemGrabbed)
			switchToState (States.Drag);
		else if (currentState == States.Drag && !itemGrabbed)
			switchToState (States.Idle);

	}


	void switchToState(States nextState)
	{
		//TODO: call specific onExitState function

		currentState = nextState;

		//TODO: call specific onEnterState function
	}
}

