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
	
	// Update is called once per frame
	public override void OnTriggerEnter (Collider other) 
	{
		base.OnTriggerEnter (other);

		if (other.gameObject.tag == "ShoppingItem") 
		{
				currentItem = other.gameObject;

				currentItem.transform.Find ("Dragger").gameObject.SetActive (false);
				currentItem.rigidbody.isKinematic = true;
				lerpStartTime = Time.time;
				lerpStartPosition = currentItem.transform.position;
				StartCoroutine ("moveToPin");
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
		pin.GetComponent<Pin>().pinItem(currentItem);
		
	}
}
