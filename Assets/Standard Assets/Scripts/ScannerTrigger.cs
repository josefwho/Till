using UnityEngine;
using System.Collections;

public class ScannerTrigger : MonoBehaviour 
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
			machine.itemAtScanner = true;
			machine.itemToPin = other.gameObject;
		}

	}

	void OnTriggerExit (Collider other)
	
	{
		if (other.gameObject.tag == "ShoppingItem") 
		{
			machine.itemAtScanner = false;
		}
	}
}
