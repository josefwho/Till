using UnityEngine;
using System.Collections;

public class BasketTrigger : ItemTrigger {

//	private TillStateMachine till;
//	
//	void Start()
//	{
//		base.Start ();
//		
//		till = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();
//		
//	}

//	public override virtual float getScore()
//	{
//		float score = 0;
//
//		for (int i = 0; i < objectsInside.Count; i++) 
//		{
//			GameObject temp = objectsInside[i] as GameObject;
//			if(temp.GetComponent<ItemStatus>().scanned == 1)
//			{
//				score += 1.0f;
//			}
//			if(temp.GetComponent<ItemStatus>().scanned > 1)
//			{
//				score -= 1.0f;
//			}
//		}
//		return score;
//	}

	public override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter (other);
		
		if (other.gameObject.tag == "ShoppingItem")
		{
			ItemStatus status = other.gameObject.GetComponent<ItemStatus>();

			if(status && status.scanned == 0)
			{
				if(!status.givenForFree)
					status.customer.onFreeItem(other.gameObject);

				status.givenForFree = true;
			}
		}
	}
}

