using UnityEngine;
using System.Collections;

public class FloorTrigger : MonoBehaviour {

	private TillStateMachine machine;
	
	
	// Use this for initialization
	void Awake () 
	{
		machine = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) 
	{
		if (other.gameObject.tag == "ShoppingItem") 
		{
			other.GetComponent<ItemStatus>().onFloor = true;
		}
		
	}

	// Update is called once per frame
	void OnTriggerExit (Collider other) 
	{
		if (other.gameObject.tag == "ShoppingItem") 
		{
			other.GetComponent<ItemStatus>().onFloor = false;
		}
		
	}
}
