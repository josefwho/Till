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
			other.gameObject.GetComponent<ItemStatus>().inTrigger = gameObject;

			print (other.gameObject.name + " entered trigger " + name);
			objectsInside.Add(other.gameObject);
			empty = false;
		}
	}

	public virtual void OnTriggerExit (Collider other)
	{
		other.gameObject.GetComponent<ItemStatus>().inTrigger = null;
		print (other.gameObject.name + " LEFT trigger " + name);
		removeObject (other.gameObject);
	}

	void removeObject(GameObject toBeRemoved)
	{
		if (toBeRemoved.tag == "ShoppingItem") 
		{
			
			print (toBeRemoved.name + " was removed from trigger " + name);
			objectsInside.Remove(toBeRemoved);
			if(objectsInside.Count == 0)
			{
				empty = true;
			}
		}

		}

	public virtual float getScore()
	{
		return 0;
	}

	public int getObjectsInsideCount()
	{
		if (objectsInside == null)
						return 0;

		return objectsInside.Count;
	}
}
