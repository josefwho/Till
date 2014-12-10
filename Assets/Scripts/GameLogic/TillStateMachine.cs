using UnityEngine;
using System.Collections;

public enum States
{
	Setup = 0,
		NextCustomer,
			InProgress,
				CustomerDone,
					ShiftDone
}

public class Customer
{
	public ArrayList shoppingItems;

	public Customer()
	{
		shoppingItems = new ArrayList ();
	}
}

public class TillStateMachine : MonoBehaviour 
{
	public bool setupDone;
	public bool shiftDone;

	public States currentState;

	public int countScannedObjects;
	public int countBasketObjects;
	public GUIText countScanned;
	public GUIText countBasket; 

	private ItemTrigger floorTrigger;
	private ItemTrigger scannerTrigger;
	private ItemTrigger basketTrigger;

	private Customer currentCustomer;
	private ArrayList customers;

	void Start()
	{
		currentState = States.Setup;

		countScannedObjects = 0;
		countScanned.text = "Items Scanned: "+ countScannedObjects.ToString ();
		countBasketObjects = 0;
		countBasket.text = "Items in Basket: " + countBasketObjects.ToString ();

		floorTrigger = GameObject.Find ("Floor/OnFloorTrigger").GetComponent<ItemTrigger>();
		scannerTrigger = GameObject.Find ("Scanner/Scanner Trigger").GetComponent<ItemTrigger>();
		basketTrigger = GameObject.Find ("basket/InBasketTrigger").GetComponent<ItemTrigger>();

		customers = new ArrayList ();

		onEnterSetup ();
	}

	void onEnterSetup()
	{
		Customer customer = new Customer ();

		GameObject[] allItems = GameObject.FindGameObjectsWithTag ("ShoppingItem");

		for (int i = 0; i < allItems.Length; i++) 
		{
			customer.shoppingItems.Add (allItems[i]);
		}

		customers.Add (customer);

		switchToState (States.NextCustomer);
	}

	void Update ()
	{
		if (currentState == States.InProgress) 
		{
			if(currentCustomer.shoppingItems.Count == floorTrigger.getObjectsInsideCount() + basketTrigger.getObjectsInsideCount())
				switchToState(States.NextCustomer);
		}
	}


	void switchToState(States nextState)
	{
		States lastState = currentState;

		//TODO: call specific onExitState function
//		if (lastState == States.Scan)
//			onExitScan (nextState);

		currentState = nextState;

		//call specific onEnterState function
		if(nextState == States.NextCustomer)
			onEnterNextCustomer(lastState);
	}
	
	
	void onEnterNextCustomer(States lastState)
	{
		if (customers.Count > 0) {
			currentCustomer = (Customer)customers [customers.Count - 1];
			customers.RemoveAt (customers.Count - 1);

			switchToState (States.InProgress);
		} else
			switchToState (States.ShiftDone);

	}

	public void setCountText()
	{
		countScanned.text = "Items Scanned: " + countScannedObjects.ToString ();
		countBasket.text = "Items in Basket: " + countBasketObjects.ToString ();
	}
}

