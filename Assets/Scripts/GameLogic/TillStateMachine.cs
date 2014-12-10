using UnityEngine;
using System.Collections;

public enum States
{
	Setup = 0,
		Idle,
			Drag,
				Scan,
					Throw,
						CustomerDone,
	ShiftDone
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
	public GameObject currentItem;
	public ItemStatus currentItemStatus;
	private GameObject pin;
	private float lerpStartTime;
	private Vector3 lerpStartPosition;
	public int countScannedObjects;
	public int countBasketObjects;
	public GUIText countScanned;
	public GUIText countBasket; 

	void Start()
	{
		currentState = States.Setup;
		currentItem = null;
		currentItemStatus = null;
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
				else if (currentState == States.Drag) {
						if (itemAtScanner)
							switchToState (States.Scan);
						else if (!itemGrabbed)
								switchToState (States.Idle);
				} else if (currentState == States.Scan) {
					if (currentItemStatus != null && currentItemStatus.scanned)
						switchToState (States.Idle);

			   }
	}


	void switchToState(States nextState)
	{
		States lastState = currentState;

		//call specific onExitState function
		if (lastState == States.Scan)
			onExitScan (nextState);

		currentState = nextState;

		//TODO: call specific onEnterState function
		if(nextState == States.Scan)
			onEnterScan(lastState);
	}
	
	
	void onEnterScan(States lastState)
	{
		if(currentItem != null)
		{
//			Destroy(currentItem.GetComponent<DragRigidBody>());
			
			currentItem.transform.Find("Dragger").gameObject.SetActive(false);
			currentItem.rigidbody.isKinematic = true;
			lerpStartTime = Time.time;
			lerpStartPosition = currentItem.transform.position;
			StartCoroutine("moveToPin");

//			currentItem.AddComponent<>
		}
	}

	void onExitScan(States nextState)
	{
		if (currentItem != null) 
		{
			unpinItem ();
			countScannedObjects ++;
			setCountText ();
		}
	}
	
	void onEnterBasket()
	{
		if (currentItem != null) 
		{
			countBasketObjects ++;
			setCountText();
		}

	}

	public void setCountText()
	{
		countScanned.text = "Items Scanned: " + countScannedObjects.ToString ();
		countBasket.text = "Items in Basket: " + countBasketObjects.ToString ();
	}

	void pinItem()
	{
		ConfigurableJoint joint = currentItem.AddComponent<ConfigurableJoint>();
		joint.xMotion = ConfigurableJointMotion.Locked;
		joint.yMotion = ConfigurableJointMotion.Locked;
		joint.zMotion = ConfigurableJointMotion.Locked;
		joint.anchor = Vector3.zero;
		joint.connectedBody = pin.rigidbody;

		currentItem.rigidbody.isKinematic = false;
		currentItem.rigidbody.useGravity = false;

		currentItem.transform.Find("Spinner").gameObject.SetActive(true);
		currentItem.transform.Find ("Barcode").GetChild(0).gameObject.SetActive (true);

	}


	void unpinItem()
	{
		currentItem.rigidbody.velocity = Vector3.zero;
		currentItem.rigidbody.angularVelocity = Vector3.zero;
		Destroy (currentItem.GetComponent<ConfigurableJoint> ());
		currentItem.rigidbody.useGravity = true;
		makeThrowable ();
	}
	 



	IEnumerator moveToPin()
	{
		while(Vector3.Distance(currentItem.transform.position, pin.transform.position) > moveToPinThreshold)
		{
			float t = (Time.time - lerpStartTime)/moveToPinDuration;
			currentItem.transform.position = Vector3.Lerp(lerpStartPosition, pin.transform.position, t);
			yield return null;
		}

		currentItem.transform.position = pin.transform.position;
		pinItem();

	}

	void makeThrowable()
	{
		currentItem.transform.Find("Spinner").gameObject.SetActive(false);
		currentItem.transform.Find("Dragger").gameObject.SetActive(true);
		currentItem.transform.Find ("Barcode").GetChild(0).gameObject.SetActive (false);
	}

	public void setCurrentItem(GameObject newItem)
	{
				if (newItem.tag == "ShoppingItem") {
						currentItem = newItem;
						currentItemStatus = newItem.GetComponent<ItemStatus> ();
				} else if (newItem == null) {
						currentItem = null;
						currentItemStatus = null;
				} else {
						print ("Trying to set " + newItem + "as current item but it doesnt have tag ShoppingItem");
				}
	}
}

