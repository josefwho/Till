using UnityEngine;
using System.Collections;

public class ItemTrigger : MonoBehaviour {

	protected ArrayList objectsInside;
	
	public bool empty = true;

	void Start()
	{
		objectsInside = new ArrayList ();
	}

	void OnEnable()
	{
		TillStateMachine.itemDestroy += removeObject;
	}


	void OnDisable()
	{
		TillStateMachine.itemDestroy -= removeObject;
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
		removeObject (other.gameObject);
	}

	void removeObject(GameObject toBeRemoved)
	{
		if (toBeRemoved.tag == "ShoppingItem") 
		{
			objectsInside.Remove(toBeRemoved);
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
