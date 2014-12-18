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
//	public Vector2 customerCountRange;
//	public Vector2 itemCountRange;
	public float spawnRadius = 2;
	public float score;
	public GUIText scoreText;
	public float timeTaken;
	public GUIText timeTakenText;

	public float betweenItemOffset = -0.1f;

	public int draggedItemCount;

	public float spawnDelay = 0.1f;

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

		score = 0;

		timeTaken = 0;
		draggedItemCount = 0;

		//how many customers we will have this shift
		int customerCount = 4; //(int)Mathf.Round((Random.Range (customerCountRange [0], customerCountRange [1])));

		int[] itemCounts = {6,14,32,32,18,6};

		for (int c = 0; c < customerCount; c++) 
		{

			Customer customer = new Customer ();

//			int itemCount = itemCounts[c];

			for (int i = 0; i < itemCounts[c]; i++) 
			{
				//get a random prefab
				Object prefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

				Vector3 pos = gameObject.transform.position;
				pos += new Vector3(Random.Range(-spawnRadius, spawnRadius), 2, Random.Range(-spawnRadius, spawnRadius));
				pos += new Vector3(betweenItemOffset*(i+1), 0, 0);
				pos.x = Mathf.Max(pos.x, -8.5f);

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

		if (customers.Count > 0) {
			currentCustomer = (Customer)customers [0];
			customers.RemoveAt (0);

			StartCoroutine("spawnItems");

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
			int minutes = (int)timeTaken / 60 ;
			timeTakenText.text = "Time Taken: " + minutes.ToString() + ":" + Mathf.Round(timeTaken % 60);
		}
	}

	IEnumerator spawnItems()
	{
		for (int i = 0; i < currentCustomer.shoppingItems.Count; i++) 
		{
			GameObject temp = (GameObject)currentCustomer.shoppingItems[i];
			temp.SetActive(true);

			yield return new WaitForSeconds(spawnDelay);
		}
	}
	
}

