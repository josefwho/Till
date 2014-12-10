using UnityEngine;
using System.Collections;

public class ScannerTrigger : ItemTrigger 
{

	private TillStateMachine machine;

	private GameObject currentItem;
	private GameObject pin;

	private float lerpStartTime;
	private Vector3 lerpStartPosition;
	public float moveToPinThreshold = 0.1f;
	public float moveToPinDuration = 1.0f;


	// Use this for initialization
	void Awake () 
	{
		machine = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();
		
		pin = GameObject.FindGameObjectWithTag ("Pin");
	}
	
	// Update is called once per frame
	public override void OnTriggerEnter (Collider other) 
	{
		base.OnTriggerEnter (other);

		if (other.gameObject.tag == "ShoppingItem") 
		{
			currentItem = other.gameObject;

			currentItem.transform.Find("Dragger").gameObject.SetActive(false);
			currentItem.rigidbody.isKinematic = true;
			lerpStartTime = Time.time;
			lerpStartPosition = currentItem.transform.position;
			StartCoroutine("moveToPin");
		}

	}
	
	
	IEnumerator moveToPin()
	{
		while(Vector3.Distance(currentItem.transform.position, pin.transform.position) > moveToPinThreshold)
		{
			float t = (Time.time - lerpStartTime)/moveToPinDuration;
			currentItem.transform.position = Vector3.Lerp(lerpStartPosition, pin.transform.position, t);
			yield return null;
		}
		
		currentItem.transform.position = pin.transform.position;
		pinItem();
		
	}
	
	
	void pinItem()
	{
		ConfigurableJoint joint = currentItem.AddComponent<ConfigurableJoint>();
		joint.xMotion = ConfigurableJointMotion.Locked;
		joint.yMotion = ConfigurableJointMotion.Locked;
		joint.zMotion = ConfigurableJointMotion.Locked;
		joint.anchor = Vector3.zero;
		joint.connectedBody = pin.rigidbody;
		
		currentItem.rigidbody.isKinematic = false;
		currentItem.rigidbody.useGravity = false;
		
		currentItem.transform.Find("Spinner").gameObject.SetActive(true);
		currentItem.transform.Find ("Barcode").GetChild(0).gameObject.SetActive (true);
	}
	
	
	public void unpinItem()
	{
		currentItem.rigidbody.velocity = Vector3.zero;
		currentItem.rigidbody.angularVelocity = Vector3.zero;
		Destroy (currentItem.GetComponent<ConfigurableJoint> ());
		currentItem.rigidbody.useGravity = true;
		makeThrowable ();
	}
	
	
	
	void makeThrowable()
	{
		currentItem.transform.Find("Spinner").gameObject.SetActive(false);
		currentItem.transform.Find("Dragger").gameObject.SetActive(true);
		currentItem.transform.Find ("Barcode").GetChild(0).gameObject.SetActive (false);
	}
}
