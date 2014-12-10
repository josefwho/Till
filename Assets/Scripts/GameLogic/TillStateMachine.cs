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


	public States currentState;
	public GameObject currentItem;
	public ItemStatus currentItemStatus;
	private GameObject pin;
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

