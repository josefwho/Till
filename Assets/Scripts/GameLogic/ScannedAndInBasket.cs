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
			other.GetComponent<ItemStatus>().inBasket = true;

			if(other.GetComponent<ItemStatus>().inBasket && other.GetComponent<ItemStatus>().scanned)
			{
			machine.countBasketObjects++;
			machine.setCountText();
			}
		}
		
	}

	// Update is called once per frame
	void OnTriggerExit (Collider other) 
	{
		if (other.gameObject.tag == "ShoppingItem") 
		{
			other.GetComponent<ItemStatus>().inBasket = false;
		}
	}

}
