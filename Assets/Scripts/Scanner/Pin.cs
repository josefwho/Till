using UnityEngine;
using System.Collections;

public class Pin : MonoBehaviour {

	private GameObject itemToPin;
	
	public float maxAngularVelocity = 3;
	public float drag = 10;
	public float angularDrag = 5;

	public GameObject scannerTriggerPublic;
	public GameObject lastUnpinnedObject;
	
	private float oldMaxAngularVelocity;
	
	private float oldDrag;
	private float oldAngularDrag;

	public void pinItem(GameObject newItem)
	{
		scannerTriggerPublic.SetActive (false);

		itemToPin = newItem;

		ConfigurableJoint joint = itemToPin.AddComponent<ConfigurableJoint>();
		joint.xMotion = ConfigurableJointMotion.Locked;
		joint.yMotion = ConfigurableJointMotion.Locked;
		joint.zMotion = ConfigurableJointMotion.Locked;
		joint.anchor = Vector3.zero;
		joint.connectedBody = rigidbody;
		
		itemToPin.rigidbody.isKinematic = false;
		itemToPin.rigidbody.useGravity = false;

		oldMaxAngularVelocity = itemToPin.rigidbody.maxAngularVelocity;
		itemToPin.rigidbody.maxAngularVelocity = maxAngularVelocity;
		
		oldDrag = itemToPin.rigidbody.drag;
		oldAngularDrag = itemToPin.rigidbody.angularDrag;
		
		itemToPin.rigidbody.drag = drag;
		itemToPin.rigidbody.angularDrag = angularDrag;
		
		print ("pinItem: oldDrag is " + oldDrag + " new drag is:" + drag);
		
		itemToPin.transform.Find("Spinner").gameObject.SetActive(true);
		itemToPin.transform.Find ("Barcode").GetChild(0).gameObject.SetActive (true);
	}
	
	
	public void unpinItem()
	{
		itemToPin.rigidbody.maxAngularVelocity = oldMaxAngularVelocity;
		
		print ("unpinItem resetting to oldDrag: " + oldDrag);

		itemToPin.rigidbody.drag = oldDrag;
		itemToPin.rigidbody.angularDrag = oldAngularDrag;

		Destroy (itemToPin.GetComponent<ConfigurableJoint> ());
		
		itemToPin.rigidbody.velocity = Vector3.zero;
		itemToPin.rigidbody.angularVelocity = Vector3.zero;

		itemToPin.rigidbody.useGravity = true;
		makeThrowable ();

		lastUnpinnedObject = itemToPin;

		scannerTriggerPublic.SetActive (true);
	}
	
	
	
	void makeThrowable()
	{
		itemToPin.transform.Find("Spinner").gameObject.SetActive(false);
		itemToPin.transform.Find("Dragger").gameObject.SetActive(true);
		itemToPin.transform.Find ("Barcode").GetChild(0).gameObject.SetActive (false);
	}
}
