using UnityEngine;
using System.Collections;

public class ScanDetection : MonoBehaviour {

	private TillStateMachine machine;
	
	
	// Use this for initialization
	void Awake () 
	{
		machine = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) 
	{
		if (other.gameObject.tag == "Scanner") 
		{
			machine.itemScanned = true;
		}
		
	}
	/*
	void OnTriggerExit (Collider other)
		
	{
		if (other.gameObject.tag == "Scanner") 
		{
			machine.itemScanned = false;
		}
	}
	*/
}

 
