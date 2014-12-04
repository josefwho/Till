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
	public GameObject itemToPin;

	void Start()
	{
		currentState = States.Setup;
		itemToPin = null;
		setupDone = true;
	}

	void Update ()
	{
		if (currentState == States.Setup && setupDone) 
			switchToState (States.Idle);
		else if (currentState == States.Idle && itemGrabbed)
			switchToState (States.Drag);
		else if (currentState == States.Drag) 
		{
			if (itemAtScanner)
				switchToState (States.Scan);
			else if (!itemGrabbed)
				switchToState (States.Idle);
		} 
		else if (currentState == States.Scan) 
		{
			if (!itemAtScanner)
				switchToState (States.Drag);
		}
	}


	void switchToState(States nextState)
	{
		//TODO: call specific onExitState function

		States lastState = currentState;
		currentState = nextState;

		//TODO: call specific onEnterState function
		if(currentState == States.Scan)
			onEnterScan(lastState);
	}
	
	
	void onEnterScan(States lastState)
	{
		if(itemToPin != null)
		{
//			Destroy(itemToPin.GetComponent<DragRigidBody>());
			
			itemToPin.transform.Find("Dragger").gameObject.SetActive(false);

//			itemToPin.AddComponent<>
		}
	}
}

