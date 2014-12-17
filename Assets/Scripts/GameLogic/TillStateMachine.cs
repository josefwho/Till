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
	public GUIText countScanned;
	public GUIText countBasket;
	public GUIText countFloor;
	public Vector2 customerCountRange;
	public Vector2 itemCountRange;
	public float spawnRadius;

	private ItemTrigger floorTrigger;
	private ItemTrigger scannerTrigger;
	private ItemTrigger basketTrigger;

	private Customer currentCustomer;
	private ArrayList customers;

	public delegate void OnItemDestroy(GameObject toBeDestroyed);
	public static event OnItemDestroy itemDestroy ;
	
	void Start()
	{
		currentState = States.Setup;

		floorTrigger = GameObject.Find ("Floor/OnFloorTrigger").GetComponent<ItemTrigger>();
		scannerTrigger = GameObject.Find ("Scanner/Scanner Trigger").GetComponent<ItemTrigger>();
		basketTrigger = GameObject.Find ("basket/InBasketTrigger").GetComponent<ItemTrigger>();
		
		countScannedObjects = 0;
		countScanned.text = "Items Scanned: "+ countScannedObjects.ToString ();
		countBasket.text = "Items in Basket: " + basketTrigger.getObjectsInsideCount().ToString ();
		countFloor.text = "Items on Floor: " + floorTrigger.getObjectsInsideCount ().ToString ();

		customers = new ArrayList ();

		onEnterSetup ();
	}

	void onEnterSetup()
	{
		//get all shopping item prefabs to choose from
		Object[] itemPrefabs = Resources.LoadAll ("Prefabs/Items");

		//how many customers we will have this shift
		int customerCount = (int)Mathf.Round((Random.Range (customerCountRange [0], customerCountRange [1])));

		for (int c = 0; c < customerCount; c++) 
		{

			Customer customer = new Customer ();

			int itemCount = (int)Mathf.Round(Random.Range(itemCountRange[0], itemCountRange[1]));

			for (int i = 0; i < itemCount; i++) 
			{
				//get a random prefab
				Object prefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

				Vector3 pos = gameObject.transform.position;
				pos += new Vector3(Random.Range(-spawnRadius, spawnRadius), 2, Random.Range(-spawnRadius, spawnRadius));

				GameObject item = Instantiate(prefab, pos, Quaternion.identity ) as GameObject;
				item.SetActive(false);
				customer.shoppingItems.Add (item);
			}
			
			customers.Add (customer);
		}

		switchToState (States.NextCustomer);
	}

	void Update ()
	{
		setCountText ();

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
		if (currentCustomer != null) {
			
						for (int i = 0; i < currentCustomer.shoppingItems.Count; i++) {
								GameObject toBeDestroyed = (GameObject)currentCustomer.shoppingItems [i];
				itemDestroy(toBeDestroyed);
				Destroy (toBeDestroyed);
						}
				}

		if (customers.Count > 0) {
			currentCustomer = (Customer)customers [customers.Count - 1];
			customers.RemoveAt (customers.Count - 1);

			for (int i = 0; i < currentCustomer.shoppingItems.Count; i++) 
			{
				GameObject temp = (GameObject)currentCustomer.shoppingItems[i];
				temp.SetActive(true);
			}

			switchToState (States.InProgress);
		} else
			switchToState (States.ShiftDone);

	}

	public void setCountText()
	{
		countScanned.text = "Items Scanned: " + countScannedObjects.ToString ();
		countBasket.text = "Items in Basket: " + basketTrigger.getObjectsInsideCount().ToString ();
		countFloor.text = "Items on Floor: " + floorTrigger.getObjectsInsideCount().ToString ();
	}
}

