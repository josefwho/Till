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

	public float moveToPinThreshold = 0.1f;
	public float moveToPinDuration = 1.0f;

	public States currentState;
	public GameObject itemToPin;
	private GameObject pin;
	private float lerpStartTime;
	private Vector3 lerpStartPosition;

	void Start()
	{
		currentState = States.Setup;
		itemToPin = null;
		setupDone = true;

		pin = GameObject.FindGameObjectWithTag ("Pin");
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
			itemToPin.rigidbody.isKinematic = true;
			lerpStartTime = Time.time;
			lerpStartPosition = itemToPin.transform.position;
			StartCoroutine("moveToPin");

//			itemToPin.AddComponent<>
		}
	}

	void pinItem()
	{
		ConfigurableJoint joint = itemToPin.AddComponent<ConfigurableJoint>();
		joint.xMotion = ConfigurableJointMotion.Locked;
		joint.yMotion = ConfigurableJointMotion.Locked;
		joint.zMotion = ConfigurableJointMotion.Locked;
		joint.anchor = Vector3.zero;
		joint.connectedBody = pin.rigidbody;

		itemToPin.rigidbody.isKinematic = false;
		itemToPin.rigidbody.useGravity = false;
		itemToPin.transform.Find("Dragger").gameObject.SetActive(true);

	}

	IEnumerator moveToPin()
	{
		while(Vector3.Distance(itemToPin.transform.position, pin.transform.position) > moveToPinThreshold)
		{
			float t = (Time.time - lerpStartTime)/moveToPinDuration;
			itemToPin.transform.position = Vector3.Lerp(lerpStartPosition, pin.transform.position, t);
			yield return null;
		}

		itemToPin.transform.position = pin.transform.position;
		pinItem();

	}

}

