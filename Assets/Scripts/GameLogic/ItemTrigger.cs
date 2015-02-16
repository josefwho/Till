using UnityEngine;
using System.Collections;

public class ItemTrigger : MonoBehaviour {

	protected ArrayList objectsInside;
	
	public bool empty = true;

	public void Start()
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

//			print (other.gameObject.name + " entered trigger " + name);
			objectsInside.Add(other.gameObject);
			empty = false;
		}
	}

	public virtual void OnTriggerExit (Collider other)
	{
//		print (other.gameObject.name + " LEFT trigger " + name);
		removeObject (other.gameObject);
	}

	void removeObject(GameObject toBeRemoved)
	{
		if (toBeRemoved.tag == "ShoppingItem") 
		{
			ItemStatus s = toBeRemoved.GetComponent<ItemStatus>();
			if(s.inTrigger != null && s.inTrigger.GetInstanceID() == gameObject.GetInstanceID())
				toBeRemoved.GetComponent<ItemStatus>().inTrigger = null;

//			print (toBeRemoved.name + " was removed from trigger " + name);
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

	public bool isObjectInside(GameObject item)
	{
		if (empty)
			return false;

		foreach (GameObject o in objectsInside) 
		{
			if(o.GetInstanceID() == item.GetInstanceID())
				return true;
			if(o.GetInstanceID() == item.transform.root.GetInstanceID())
				return true;
		}

		return false;
	}
}
