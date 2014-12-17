using UnityEngine;
using System.Collections;

public class BasketTrigger : ItemTrigger {
	
	// Use this for initialization
	public virtual float getScore()
	{
		float score = 0;

		for (int i = 0; i < objectsInside.Count; i++) 
		{
			GameObject temp = objectsInside[i] as GameObject;
			if(temp.GetComponent<ItemStatus>().scanned == 1)
			{
				score += 1.0f;
			}
			if(temp.GetComponent<ItemStatus>().scanned > 1)
			{
				score -= 1.0f;
			}
		}
		return score;
	}
}

