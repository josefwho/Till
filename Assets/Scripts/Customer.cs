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
	
//	public GameObject image;

	// Use this for initialization
	void OnEnable () {
		shoppingItems = new ArrayList ();

		till = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();

		waitingTime = 0.0f;

		text = transform.GetComponentInChildren<Text> ();
		buttonObject = text.transform.parent.gameObject;
		buttonObject.SetActive (false);
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
		buttonObject.SetActive (true);
		text.text = "hey, you dropped my " + item.GetComponent<ItemStatus> ().name + " on the floor";
//		Debug.Log ("hey, you dropped my " + item.GetComponent<ItemStatus>().name + " on the floor");
		//playComplainSound();
		StartCoroutine (hideBubble ());
	}

	public void onMultipleScanned(GameObject item)
	{
		buttonObject.SetActive (true);
		text.text = "hey, you scanned my " + item.GetComponent<ItemStatus> ().name + " again, you fool!";
//		Debug.Log ("hey, you scanned my " + item.GetComponent<ItemStatus>().name + " again. WTF!");
		//playComplainSound();
		StartCoroutine (hideBubble ());

	}

	public void onNotMyItem(GameObject item)
	{
		buttonObject.SetActive (true);
		text.text = "hey, this is not my " + item.GetComponent<ItemStatus> ().name;
//		Debug.Log ("hey, this is not my " + item.GetComponent<ItemStatus>().name);
		//playComplainSound();
		StartCoroutine (hideBubble ());

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
		yield return new WaitForSeconds (2.0f);

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
	}

	IEnumerator fadeOutBubble()
	{	
		Image imageButton = buttonObject.GetComponent<Image> ();

		Color colorText = text.color;
		Color colorButton = imageButton.color;

		float startTime = 0.0f;
		float fadeTime = 0.5f;

		while (startTime < fadeTime) 
		{
			startTime += Time.deltaTime;

			float newAlpha = Mathf.Lerp(1, 0, startTime/fadeTime);
			colorText.a = newAlpha;
			colorButton.a = newAlpha;

			text.color = colorText;
			imageButton.color = colorButton;

			yield return null;
		}

		buttonObject.gameObject.SetActive (false);

		//reset alpha so it's visible when shown next time
		colorText.a = 1;
		colorButton.a = 1;
		text.color = colorText;
		imageButton.color = colorButton;
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


