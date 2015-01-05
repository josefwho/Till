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

	public float spawnDelay = 0.1f;

	private ItemTrigger floorTrigger;
	private ItemTrigger scannerTrigger;
	private ItemTrigger basketTrigger;
	private ItemTrigger newCustomerTrigger;

	private Customer currentCustomer;
	private Customer nextCustomer;
	private ArrayList customers;
	private bool isSpawningItems;

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

			if(!isSpawningItems && nextCustomer == null && newCustomerTrigger.empty)
			{
				nextCustomer = getNewCustomer();
				StartCoroutine(spawnItems(nextCustomer));
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

		if (timeTaken < 120.0f) {
			if(nextCustomer == null)
			{
				nextCustomer = getNewCustomer();
				StartCoroutine(spawnItems(nextCustomer));
			}

			currentCustomer = nextCustomer;
			nextCustomer = null;

			switchToState (States.InProgress);
		} else
			switchToState (States.ShiftDone);

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

			float time = Mathf.Lerp(startTime, endTime, timeTaken/shiftDuration);
			int hours = Mathf.FloorToInt(time);
			int minutes = (int)((time-hours)*60) ;
			timeTakenText.text = "Time: " + hours.ToString() + ":" + minutes.ToString();
		}
	}

	Customer getNewCustomer()
	{
		Customer customer = new Customer ();
		
		CustomerProfile profile = gameObject.GetComponent<CustomerManager>().getRandomProfile();
		CustomerVariation variation = profile.getRandomVariation();
		
		
		Debug.Log ("new customer is of type " + profile.name + "." + variation.type);
		
		string[] wishList = gameObject.GetComponent<CustomerManager>().itemWishList(variation);
		
		int itemCount = (int)Random.Range(variation.countRange[0], variation.countRange[1]);
		Debug.Log ("he/she has " + itemCount + " items on the belt:");
		
		for (int i = 0; i < itemCount; i++) 
		{
			//get a random prefab
			string prefabName = wishList[Random.Range(0, wishList.Length)];
			
			Debug.Log(prefabName);
			
			Object prefab = Resources.Load ("Prefabs/Items/"+prefabName);
			
			if(prefab == null)
				prefab = Resources.Load ("Prefabs/Items/dummy");
			
			Vector3 pos = gameObject.transform.position;
			pos += new Vector3(Random.Range(-spawnRadius, spawnRadius), 2, Random.Range(-spawnRadius, spawnRadius));
			pos += new Vector3(betweenItemOffset*(i+1), 0, 0);
			pos.x = Mathf.Max(pos.x, -8.5f);
			
			GameObject item = Instantiate(prefab, pos, Quaternion.identity ) as GameObject;
			item.SetActive(false);
			customer.shoppingItems.Add (item);
		}
		
		customers.Add (customer);

		return customer;
	}

	IEnumerator spawnItems(Customer customer)
	{
		isSpawningItems = true;
		for (int i = 0; i < customer.shoppingItems.Count; i++) 
		{
			GameObject temp = (GameObject)customer.shoppingItems[i];
			temp.SetActive(true);

			yield return new WaitForSeconds(spawnDelay);
		}
		isSpawningItems = false;
	}
	
}

