using UnityEngine;
using System.Collections;

public class ConveyorTrigger : MonoBehaviour {

	private TillStateMachine machine;

	public int numObjectsInside;
	public bool empty = true;
	
	
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
			other.GetComponent<ItemStatus>().atConveyorBelt = true;

			empty = false;
			numObjectsInside++;
		}
		
	}

	// Update is called once per frame
	void OnTriggerExit (Collider other) 
	{
		if (other.gameObject.tag == "ShoppingItem") 
		{
			other.GetComponent<ItemStatus>().atConveyorBelt = false;

			if(--numObjectsInside <= 0)
			{
				numObjectsInside = 0; //just to be sure
				empty = true;
			}
		}
		
	}
}
