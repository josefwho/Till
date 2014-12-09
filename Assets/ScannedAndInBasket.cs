using UnityEngine;
using System.Collections;

public class ScannedAndInBasket : MonoBehaviour 
{
	
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
			machine.itemInBasket = true;
//			machine.itemToPin = other.gameObject;
		}
		
	}

}
