using UnityEngine;
using System.Collections;

public class ScanDetection : MonoBehaviour {

	private TillStateMachine machine;
	
	
	// Use this for initialization
	void Awake () 
	{
		machine = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) 
	{
		if (other.gameObject.tag == "Scanner") 
		{
			machine.itemScanned = true;
			machine.itemAtScanner = false;
			machine.itemIsScanned = transform.parent.gameObject;
		}
		
	}
	
	void OnTriggerExit (Collider other)
		
	{
		if (other.gameObject.tag == "Scanner") 
		{
			machine.itemScanned = false;
		}
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
