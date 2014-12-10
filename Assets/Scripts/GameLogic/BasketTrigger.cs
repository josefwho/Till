using UnityEngine;
using System.Collections;

public class BasketTrigger : ItemTrigger 
{
	
	private TillStateMachine machine;
	
	
	// Use this for initialization
	void Awake () 
	{
		machine = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();
	}
	
	// Update is called once per frame
	override void OnTriggerEnter (Collider other) 
	{
		base.OnTriggerEnter (other);

		if (other.gameObject.tag == "ShoppingItem") 
		{
			machine.countBasketObjects++;
			machine.setCountText();
		}

		
	}

	// Update is called once per frame
	override void OnTriggerExit (Collider other) 
	{
		base.OnTriggerExit (other);

		if (other.gameObject.tag == "ShoppingItem") 
		{
			machine.countBasketObjects--;
			machine.setCountText();
		}

	}

}
