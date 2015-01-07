using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour {
	public ArrayList shoppingItems;
	public CustomerProfile profile;

	public float spawnDelay;

	private TillStateMachine till;

	
//	public GameObject image;

	// Use this for initialization
	void OnEnable () {
		shoppingItems = new ArrayList ();

		till = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void spawnItems()
	{
		transform.GetChild (0).gameObject.SetActive (true);

		StartCoroutine (spawningItems ());
	}
	
	IEnumerator spawningItems()
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
