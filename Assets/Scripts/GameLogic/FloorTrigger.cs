using UnityEngine;
using System.Collections;

public class FloorTrigger : ItemTrigger {

	// Use this for initialization
	public virtual float getScore()
	{
		float malus = 0;
		for (int i = 0; i < objectsInside.Count; i++) 
		{
			malus -= 1.0f;
		}
		return malus;
	}
}
