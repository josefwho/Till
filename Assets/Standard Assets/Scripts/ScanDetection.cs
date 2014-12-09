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
				machine.itemScanned = true;
		}
		
	}
	
	void OnTriggerExit (Collider other)
		
	{
		if (other.gameObject.tag == "Scanner") 
		{
			currentScanDuration = 0.0f;
		}
	}

	void OnEnable()
	{
		currentScanDuration = 0.0f;
	}
}


/* PSEUDOCODE
 * 
 * innerhalb der Klasse definier ich eine StateMachine
 * 
 * im Awake hol ich mir die TillStateMachine, um sie benutzen zu können
 * 
 * eine Methode scannedItemLongEnough überprüft, ob sich das Triggerobjekt des Shopping Items lange genug mit dem Scanner trifft (collidet)
 * void scannedItemLongEnough (Collider ???)
 * 
 * if (???)
 * machine.itemScanned = true;
 * 
 * 
 * 
 * 
 * 
 * 
 */
