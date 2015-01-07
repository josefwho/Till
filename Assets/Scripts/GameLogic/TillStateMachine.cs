using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum States
{
	Setup = 0,
		NextCustomer,
			InProgress,
				CustomerDone,
					ShiftDone
}

public class CustomerTemp
{
	public ArrayList shoppingItems;
	public CustomerProfile profile;

	public GameObject image;

	public CustomerTemp()
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
//	public Vector2 customerCountRange;
//	public Vector2 itemCountRange;
	public float spawnRadius = 2;
	public float score;
	public GUIText scoreText;
	public float timeTaken;
	public float shiftDuration = 120.0f;		//in seconds
	public GUIText timeTakenText;

	public float betweenItemOffset = -0.1f;

	public int draggedItemCount;
	public bool isSpinning;

	public float spawnDelay = 0.1f;

	private ItemTrigger floorTrigger;
	private ItemTrigger scannerTrigger;
	private ItemTrigger basketTrigger;
	private ItemTrigger newCustomerTrigger;

	private Customer currentCustomer;
	private Customer nextCustomer;
	private ArrayList customers;
	public bool isSpawningItems;

	private Object nextCustomerSign;

	public delegate void OnItemDestroy(GameObject toBeDestroyed);
	public static event OnItemDestroy itemDestroy ;
	
	void Start()
	{
		currentState = States.Setup;

		floorTrigger = GameObject.Find ("Floor/OnFloorTrigger").GetComponent<ItemTrigger>();
		scannerTrigger = GameObject.Find ("Scanner/Scanner Trigger").GetComponent<ItemTrigger>();
		basketTrigger = GameObject.Find ("basket/InBasketTrigger").GetComponent<ItemTrigger>();
		newCustomerTrigger = GameObject.Find ("NewCustomerTrigger").GetComponent<ItemTrigger>();
		
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
//		Object[] itemPrefabs = Resources.LoadAll ("Prefabs/Items");
		
		nextCustomer = null;

		nextCustomerSign = Resources.Load ("Prefabs/next_customer_sign");

		score = 0;

		timeTaken = 0;
		draggedItemCount = 0;

		switchToState (States.NextCustomer);
	}

	void Update ()
	{
		setCountText ();

		if (currentState == States.InProgress) 
		{
			if(currentCustomer.shoppingItems.Count == floorTrigger.getObjectsInsideCount() + basketTrigger.getObjectsInsideCount())
				switchToState(States.NextCustomer);

			if(!isSpawningItems && timeTaken < shiftDuration && nextCustomer == null && newCustomerTrigger.empty)
			{
				nextCustomer = getNewCustomer();
			}
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
		if(nextState == States.ShiftDone)
			onEnterShiftDone(lastState);
	}
	
	
	void onEnterNextCustomer(States lastState)
	{
		if (currentCustomer != null) {

			score += ((BasketTrigger)basketTrigger).getScore()*2;
			score += ((FloorTrigger)floorTrigger).getScore();

						for (int i = 0; i < currentCustomer.shoppingItems.Count; i++) {
								GameObject toBeDestroyed = (GameObject)currentCustomer.shoppingItems [i];
				itemDestroy(toBeDestroyed);
				Destroy (toBeDestroyed);
						}
				}
		

		if(currentCustomer != null)
			currentCustomer.leave();

		if (timeTaken > 120.0f && nextCustomer == null) 
		{
			switchToState (States.ShiftDone);
		}
		else
		{
			if(nextCustomer == null && timeTaken < 120.0f)
			{
				nextCustomer = getNewCustomer();
			}

			currentCustomer = nextCustomer;
			nextCustomer = null;

			switchToState (States.InProgress);
		}

	}

	void onEnterShiftDone(States lastState)
	{
		score += (4.0f*60.0f - timeTaken);
	}
	
	public void setCountText()
	{
		countScanned.text = "Items Scanned: " + countScannedObjects.ToString ();
		countBasket.text = "Items in Basket: " + basketTrigger.getObjectsInsideCount().ToString ();
		countFloor.text = "Items on Floor: " + floorTrigger.getObjectsInsideCount().ToString ();
		scoreText.text = "Total Score: " + score.ToString();

		//mindestlohn 1.159,08 Netto

		if (currentState != States.ShiftDone) 
		{
			timeTaken += Time.deltaTime;

			float startTime = 7;	// 07:00 
			float endTime = 18; // 18:00

			float time = startTime + (endTime - startTime) * timeTaken/shiftDuration;
			int hours = Mathf.FloorToInt(time);
			int minutes = (int)((time-hours)*60) ;
			timeTakenText.text = "Time: " + hours.ToString() + ":" + minutes.ToString();
		}
	}

	public void onBeltMoved(float offset)
	{
		if (currentCustomer != null )
			currentCustomer.onBeltMoved(offset);

		if (nextCustomer != null)
			nextCustomer.onBeltMoved(offset);
	}

	Customer getNewCustomer()
	{	
		CustomerProfile profile = gameObject.GetComponent<CustomerManager>().getRandomProfile();
		CustomerVariation variation = profile.getRandomVariation();
		
		//first choose a prefab for our customer 
		Object[] customerPrefabs = Resources.LoadAll ("Prefabs/Customers/"+profile.name);
		
		GameObject customerPrefab;
		if (customerPrefabs.Length == 0)
			customerPrefab = Resources.Load ("Prefabs/Customers/dummy_customer") as GameObject;
		else
			customerPrefab = customerPrefabs [Random.Range (0, customerPrefabs.Length)] as GameObject;

		//now spawn a GameObject from the found prefab
		GameObject customerObj = Instantiate(customerPrefab, customerPrefab.transform.position, customerPrefab.transform.rotation ) as GameObject;
		Customer customer = customerObj.GetComponent<Customer> ();				//we just need the customer component for later reference

		customer.profile = profile;
		customer.spawnDelay = spawnDelay;

		Debug.Log ("new customer is of type " + profile.name + "." + variation.type);

		//now make the items this customer is buying
		string[] wishList = gameObject.GetComponent<CustomerManager>().itemWishList(variation);
		
		int itemCount = (int)Random.Range(variation.countRange[0], variation.countRange[1]);
		Debug.Log ("he/she has " + itemCount + " items on the belt:");
		
		for (int i = 0; i < itemCount; i++) 
		{
			//get a random prefab from the customers wishlist
			string prefabName = wishList[Random.Range(0, wishList.Length)];
			
			Debug.Log(prefabName);
			
			Object prefab = Resources.Load ("Prefabs/Items/"+prefabName);
			
			if(prefab == null)
				prefab = Resources.Load ("Prefabs/Items/dummy");

			//position them in a way that they wont all spawn at the same place
			Vector3 pos = gameObject.transform.position;
			pos += new Vector3(Random.Range(-spawnRadius, spawnRadius), 2, Random.Range(-spawnRadius, spawnRadius));
			pos += new Vector3(betweenItemOffset*(i+1), 0, 0);
			pos.x = Mathf.Max(pos.x, -8.5f);
			
			GameObject item = Instantiate(prefab, pos, Quaternion.identity ) as GameObject;
			item.SetActive(false);

			//let the customer know what he/she is buying
			customer.shoppingItems.Add (item);
		}

		//keep track of all customers
		customers.Add (customer);

		
		//spawn next customer sign;
		if (customers.Count > 1) 
		{
			Vector3 signPos = transform.position;
			signPos.y += 2;
			signPos.x = newCustomerTrigger.collider.bounds.max.x - 1;
			Instantiate (nextCustomerSign, signPos, Quaternion.identity);
		}

		customer.showItems();

		return customer;
	}
	
}

