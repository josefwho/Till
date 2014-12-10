using UnityEngine;
using System.Collections;

public class ScanDetection : MonoBehaviour {
	
	public float scanDuration = 2.0f;

	public float currentScanDuration = 0.0f;
	
	
	// Use this for initialization
	void Awake () 
	{
	}
	
	// Update is called once per frame
	void OnTriggerStay (Collider other) 
	{
		if (other.gameObject.tag == "Scanner") 
		{
			currentScanDuration += Time.deltaTime;

			if(currentScanDuration > scanDuration)
			{
				transform.parent.parent.gameObject.GetComponent<ItemStatus>().scanned++;
				GameObject.Find("Scanner/Scanner Trigger").GetComponent<ScannerTrigger>().unpinItem();
			}
		}
		
	}
	void OnEnable()
	{
		currentScanDuration = 0.0f;
	}
}

 
