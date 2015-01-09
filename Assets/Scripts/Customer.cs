using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour {
	public ArrayList shoppingItems;
	public CustomerProfile profile;

	public float spawnDelay;

	private TillStateMachine till;

	private float waitingTime;
//	public GameObject image;

	// Use this for initialization
	void OnEnable () {
		shoppingItems = new ArrayList ();

		till = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();

		waitingTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
		waitingTime += Time.deltaTime;

		//TODO: show specific sentences when waitingTime reaches certain times. e.g. when waitingTime is bigger then 10 seconds say first boring sentence
		//how to get a child gameobject of customer
//		Transform textT = transform.FindChild ("texts");
//		GameObject texts = textT.gameObject;
//		
//		texts = transform.FindChild ("texts").gameObject;
	}

	public void showItems()
	{
		StartCoroutine (showingItems ());
	}

	//TODO: find best place to call this callback. maybe from TilLStateMachine or from floorTrigger<ItemTrigger>
	public void onItemOnFloor(GameObject item)
	{
		Debug.Log ("hey, you dropped my " + item.GetComponent<ItemStatus>().name + " on the floor");
		//TODO: show correct sentence on top of our head
	}

	public void onMultipleScanned(GameObject item)
	{
		Debug.Log ("hey, you scanned my " + item.GetComponent<ItemStatus>().name + " again. WTF!");
		//TODO: show sentence that customer is annoyed

	}

	public void onNotMyItem(GameObject item)
	{
		Debug.Log ("hey, this is not my " + item.GetComponent<ItemStatus>().name);

		//TODO: show sentence "hey, i don't want to buy this piece of shit"
	}

	IEnumerator showingItems()
	{
		
		//then spawn his/her items 
		till.isSpawningItems = true;
		for (int i = 0; i < shoppingItems.Count; i++) 
		{
			GameObject temp = (GameObject)shoppingItems[i];
			temp.SetActive(true);
			
			yield return new WaitForSeconds(spawnDelay);
		}
		till.isSpawningItems = false;
	}

	public void leave()
	{
		StartCoroutine (leaving ());
	}
	
	
	IEnumerator leaving()
	{
		while (transform.position.x < 11.0f) 
		{
			transform.Translate (0.05f,0,0.05f, Space.World);
			
			yield return null;
		}
		
		transform.GetChild(0).gameObject.SetActive (false);
	}



	public void onBeltMoved(float offset)
	{
		if (transform.position.x < 3.5f)
			transform.Translate (offset, 0, 0);
	}
}
