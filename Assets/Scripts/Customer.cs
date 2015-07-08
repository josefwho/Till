using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Customer : MonoBehaviour {
	public ArrayList shoppingItems;
	public CustomerProfile profile;

	public float spawnDelay;

	private TillStateMachine till;

	public Text text;
	private GameObject buttonObject;
	private float waitingTime;
	private float maxWaitingTimePerItem;
	private bool showedWaitingText;
	private int countFreeItems;

	IEnumerator hideBubbleCoroutine = null;
	
//	public GameObject image;

	// Use this for initialization
	void OnEnable () {
		shoppingItems = new ArrayList ();

		till = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();

		waitingTime = 0.0f;
		maxWaitingTimePerItem = Random.Range (4, 8);

		text = transform.GetComponentInChildren<Text> ();
		buttonObject = text.transform.parent.gameObject;
		buttonObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
		waitingTime += Time.deltaTime;

		//TODO: show specific sentences when waitingTime reaches certain times. e.g. when waitingTime is bigger then 10 seconds say first boring sentence

		if (!showedWaitingText && waitingTime > shoppingItems.Count * maxWaitingTimePerItem) 
		{
			showedWaitingText = true;
			onWaitTooLong();
		}
	}

	public void showItems()
	{
		StartCoroutine (showingItems ());
	}

	//TODO: find best place to call this callback. maybe from TilLStateMachine or from floorTrigger<ItemTrigger>
	public void onItemOnFloor(GameObject item)
	{
		buttonObject.SetActive (true);
		playOnFloorSound();
		string reaction = profile.itemOnFloorReactions [Random.Range (0, profile.itemOnFloorReactions.Length)];
		if(reaction.IndexOf('{') > -1)
			reaction = string.Format(reaction, item.GetComponent<ItemStatus>().name);
		text.text = reaction;
//		Debug.Log ("hey, you dropped my " + item.GetComponent<ItemStatus>().name + " on the floor");
		//playComplainSound();
		if(hideBubbleCoroutine != null)
			StopCoroutine (hideBubbleCoroutine);
		hideBubbleCoroutine = hideBubble ();
		StartCoroutine (hideBubbleCoroutine);
	}

	void playOnFloorSound()
	{
		GetComponent<AudioSource>().Play();
		
	}

	public void onMultipleScanned(GameObject item)
	{
		buttonObject.SetActive (true);

		string reaction = profile.itemMultipleScannedReactions [Random.Range (0, profile.itemMultipleScannedReactions.Length)];
		if(reaction.IndexOf('{') > -1)
			reaction = string.Format(reaction, item.GetComponent<ItemStatus>().name);
		text.text = reaction;

//		Debug.Log ("hey, you scanned my " + item.GetComponent<ItemStatus>().name + " again. WTF!");
		//playComplainSound();
		
		if(hideBubbleCoroutine != null)
			StopCoroutine (hideBubbleCoroutine);
		hideBubbleCoroutine = hideBubble ();
		StartCoroutine (hideBubbleCoroutine);

	}

	public void onNotMyItem(GameObject item)
	{
		buttonObject.SetActive (true);

		string reaction = profile.notMyItemReactions [Random.Range (0, profile.notMyItemReactions.Length)];
		if(reaction.IndexOf('{') > -1)
			reaction = string.Format(reaction, item.GetComponent<ItemStatus>().name);
		text.text = reaction;

//		Debug.Log ("hey, this is not my " + item.GetComponent<ItemStatus>().name);
		//playComplainSound();
		
		if(hideBubbleCoroutine != null)
			StopCoroutine (hideBubbleCoroutine);
		hideBubbleCoroutine = hideBubble ();
		StartCoroutine (hideBubbleCoroutine);

	}
	
	public void onFreeItem(GameObject item)
	{
		++countFreeItems;	//number of free items given to this customer

//		if (countFreeItems < 2)
//			return;

//		int countFreeItemsShift = till.countUnscannedItems + countFreeItems;	//number of items not scanned in this shift

		buttonObject.SetActive (true);

		string reaction = profile.freeItemReactions [Random.Range (0, profile.freeItemReactions.Length)];
		if(reaction.IndexOf('{') > -1)
			reaction = string.Format(reaction, item.GetComponent<ItemStatus>().name);
		text.text = reaction;
		
		
		if(hideBubbleCoroutine != null)
			StopCoroutine (hideBubbleCoroutine);
		hideBubbleCoroutine = hideBubble ();
		StartCoroutine (hideBubbleCoroutine);
		
	}

	public void onWaitTooLong()
	{
		buttonObject.SetActive (true);

		text.text = profile.waitTooLongReactions [Random.Range (0, profile.waitTooLongReactions.Length)];;
		
		if(hideBubbleCoroutine != null)
			StopCoroutine (hideBubbleCoroutine);

		hideBubbleCoroutine = hideBubble ();
		StartCoroutine (hideBubbleCoroutine);
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
	

	IEnumerator hideBubble()
	{
		yield return new WaitForSeconds (3.0f);

		//fade it out
		Image imageButton = buttonObject.GetComponent<Image> ();
		
		Color colorText = text.color;
		Color colorButton = imageButton.color;
		
		float timePassed = 0.0f;
		float fadeTime = 0.5f;
		
		while (timePassed < fadeTime) 
		{
			//calculate time taken
			timePassed += Time.deltaTime;

			//calulate new alpha value according to how much time passed
			float newAlpha = Mathf.Lerp(1, 0, timePassed/fadeTime);
			colorText.a = newAlpha;
			colorButton.a = newAlpha;

			//set the new alpha via color
			text.color = colorText;
			imageButton.color = colorButton;

			//wait for next frame
			yield return null;
		}

		//make sure it's invisible
		buttonObject.gameObject.SetActive (false);
		
		//reset alpha so it's visible when shown next time
		colorText.a = 1;
		colorButton.a = 1;
		text.color = colorText;
		imageButton.color = colorButton;

		hideBubbleCoroutine = null;
	}

//	IEnumerator fadeOutBubble()
//	{	
//		Image imageButton = buttonObject.GetComponent<Image> ();
//
//		Color colorText = text.color;
//		Color colorButton = imageButton.color;
//
//		float startTime = 0.0f;
//		float fadeTime = 0.5f;
//
//		while (startTime < fadeTime) 
//		{
//			startTime += Time.deltaTime;
//
//			float newAlpha = Mathf.Lerp(1, 0, startTime/fadeTime);
//			colorText.a = newAlpha;
//			colorButton.a = newAlpha;
//
//			text.color = colorText;
//			imageButton.color = colorButton;
//
//			yield return null;
//		}
//
//		buttonObject.gameObject.SetActive (false);
//
//		//reset alpha so it's visible when shown next time
//		colorText.a = 1;
//		colorButton.a = 1;
//		text.color = colorText;
//		imageButton.color = colorButton;
//	}

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

//	void playComplainSound()
//	{
//		AudioSource complaint = transform.gameObject.GetComponent<AudioSource> ();
//		complaint.Play();
//		
//	}

	public bool allItemsInTrigger(ItemTrigger trigger)
	{
		bool result = true;
		foreach (GameObject i in shoppingItems) 
		{
			Vector3 pos = i.transform.position;
			if(pos.y < -5 || pos.y > 70 ||
			   pos.x < -20 || pos.x > 30 ||
			   pos.z < -23 || pos.z > 15)
			{
				i.rigidbody.velocity = Vector3.zero;
				i.transform.position = till.transform.position;
				continue;
			}


			ItemStatus s = i.GetComponent<ItemStatus>();
			if(s.inTrigger == null || s.inTrigger.GetInstanceID() != trigger.gameObject.GetInstanceID())
			{
				result = false;
				break;
}
		}

		return result;
	}

}


