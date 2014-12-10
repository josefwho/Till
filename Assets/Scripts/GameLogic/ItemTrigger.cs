using UnityEngine;
using System.Collections;

public class ItemTrigger : MonoBehaviour {

	protected ArrayList objectsInside;
	
	public bool empty = true;

	void Start()
	{
		objectsInside = new ArrayList ();
	}

	void OnDestroy()
	{

	}

	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "ShoppingItem")
		{
			objectsInside.Add(other.gameObject);
			empty = false;
		}
	}
	
	
	public virtual void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "ShoppingItem") 
		{
			objectsInside.Remove(other.gameObject);
			if(objectsInside.Count == 0)
			{
				empty = true;
			}
		}	
	}

	public int getObjectsInsideCount()
	{
		return objectsInside.Count;
	}
}
