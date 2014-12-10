using UnityEngine;
using System.Collections;

public class ScanDetection : MonoBehaviour {

	private TillStateMachine machine;

	public float scanDuration = 2.0f;

	public float currentScanDuration = 0.0f;
	
	
	// Use this for initialization
	void Awake () 
	{
		machine = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();
	}
	
	// Update is called once per frame
	void OnTriggerStay (Collider other) 
	{
		if (other.gameObject.tag == "Scanner") 
		{
			currentScanDuration += Time.deltaTime;

			if(currentScanDuration > scanDuration)
				machine.currentItemStatus.scanned = true;
		}
		
	}
	void OnEnable()
	{
		currentScanDuration = 0.0f;
	}
}

 
