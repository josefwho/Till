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
	public CustomerProfile profile;

	public GameObject image;

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
	public bool isSpinning;

	public float spawnDelay = 0.1f;

	private ItemTrigger floorTrigger;
	private ItemTrigger scannerTrigger;
	private ItemTrigger basketTrigger;
	private ItemTrigger newCustomerTrigger;

	private Customer currentCustomer;
	private Customer nextCustomer;
	private ArrayList customers;
	private bool isSpawningItems;

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

			if(!isSpawningItems && timeTaken < 120.0f && nextCustomer == null && newCustomerTrigger.empty)
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
		
		
		StartCoroutine(customerLeaves(currentCustomer));

		if (timeTaken > 120.0f && nextCustomer == null) 
		{
			switchToState (States.ShiftDone);
		}
		else
		{
			if(nextCustomer == null && timeTaken < 120.0f)
			{
				nextCustomer = getNewCustomer();
				StartCoroutine(spawnItems(nextCustomer));
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
		if (currentCustomer != null && currentCustomer.image.transform.position.x < 3.5f)
			currentCustomer.image.transform.Translate (offset, 0, 0);

		if (nextCustomer != null && nextCustomer.image.transform.position.x < 3.5f)
			nextCustomer.image.transform.Translate (offset, 0, 0);
	}

	Customer getNewCustomer()
	{
		Customer customer = new Customer ();
		
		CustomerProfile profile = gameObject.GetComponent<CustomerManager>().getRandomProfile();
		CustomerVariation variation = profile.getRandomVariation();

		customer.profile = profile;

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
		if (customers.Count > 1) 
		{
			Vector3 signPos = transform.position;
			signPos.y += 2;
			signPos.x = newCustomerTrigger.collider.bounds.max.x - 1;
			Instantiate (nextCustomerSign, signPos, Quaternion.identity);
		}

		//first spawn customer image
		Object[] customerImages = Resources.LoadAll ("Prefabs/Customers/"+customer.profile.name);

		GameObject customerImage;
		if (customerImages.Length == 0)
			customerImage = Resources.Load ("Prefabs/Customers/dummy_customer") as GameObject;
		else
			customerImage = customerImages [Random.Range (0, customerImages.Length)] as GameObject;

		customer.image = Instantiate(customerImage, customerImage.transform.position, customerImage.transform.rotation ) as GameObject;

		//then spawn his/her items 
		isSpawningItems = true;
		for (int i = 0; i < customer.shoppingItems.Count; i++) 
		{
			GameObject temp = (GameObject)customer.shoppingItems[i];
			temp.SetActive(true);

			yield return new WaitForSeconds(spawnDelay);
		}
		isSpawningItems = false;
	}

	IEnumerator customerLeaves(Customer customer)
	{
		while (customer.image.transform.position.x < 11.0f) 
		{
			customer.image.transform.Translate (0.05f,0,0.05f, Space.World);

			yield return null;
		}

		customer.image.SetActive (false);
	}
	
}

