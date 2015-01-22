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


	public GameObject scannerTriggerPublic;


	// Use this for initialization
	void Awake () 
	{
		machine = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();
		
		pin = GameObject.FindGameObjectWithTag ("Pin");
	}
	
	public override void OnTriggerEnter (Collider other) 
	{
		base.OnTriggerEnter (other);

		if (other.gameObject.tag == "ShoppingItem") 
		{
			startPinning(other.gameObject);
		}
	}
	
//	public void OnTriggerStay (Collider other) 
//	{
//		if (!pin.GetComponent<Pin>().pinning && currentItem == null && other.gameObject.tag == "ShoppingItem" && other.gameObject.GetComponent<ItemStatus>().scanned == 0) 
//		{
//			startPinning(other.gameObject);
//		}
//	}


	public void startPinning(GameObject toPin)
	{
		ItemStatus status = toPin.GetComponent<ItemStatus> ();
		if (pin.GetComponent<Pin> ().pinning || currentItem != null || (status !=null && status.autoDragged))
				return;

		currentItem = toPin;
		
		currentItem.transform.Find ("Dragger").gameObject.SetActive (false);
		currentItem.rigidbody.isKinematic = true;
		lerpStartTime = Time.time;
		lerpStartPosition = currentItem.transform.position;
		StartCoroutine ("moveToPin");
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
		pin.GetComponent<Pin>().pinItem(currentItem);

		currentItem = null;
		
	}
}
