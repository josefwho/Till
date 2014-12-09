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

	private int countScannedObjects;
	private int countBasketObjects;
	public GUIText countScanned;
	public GUIText countBasket; 

	void Start()
	{
		currentState = States.Setup;
		itemToPin = null;
		setupDone = true;

		pin = GameObject.FindGameObjectWithTag ("Pin");

		countScannedObjects = 0;
		countScanned.text = "Items Scanned: "+ countScannedObjects.ToString ();
		countBasketObjects = 0;
		countBasket.text = "Items in Basket: " + countBasketObjects.ToString ();

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
			if (itemScanned)
				switchToState (States.Throw);
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

		if (currentState == States.Throw)
			onEnterThrow (lastState);
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

	void onEnterThrow(States lastState)
	{
		if (itemToPin != null) 
			{
				unpinItem();
				countScannedObjects ++;
				setCountText();
			itemToPin.transform.Find("Already Scanned").gameObject.SetActive(true);
//			itemToPin.AddComponent<>;
			}
	}

		void setCountText()
		{
			countScanned.text = "Items Scanned: " + countScannedObjects.ToString ();
			countBasket.text = "Items in Basket: " + countBasketObjects.ToString ();
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

		itemToPin.transform.Find("Spinner").gameObject.SetActive(true);

	}


	void unpinItem()
	{
		Destroy (itemToPin.GetComponent<ConfigurableJoint> ());
		itemToPin.rigidbody.useGravity = true;
		makeThrowableAgain ();
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

	void makeThrowableAgain()
	{
		itemToPin.transform.Find("Spinner").gameObject.SetActive(false);
		itemToPin.transform.Find("Dragger").gameObject.SetActive(true);
	}

}

