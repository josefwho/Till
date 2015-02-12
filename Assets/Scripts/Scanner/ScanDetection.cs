using UnityEngine;
using System.Collections;

public class ScanDetection : MonoBehaviour {
	
	public float scanDuration = 2.0f;

	public float currentScanDuration = 0.0f;

	private TillStateMachine machine;

	private GameObject scannerField;

	
	
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

			if(currentScanDuration > scanDuration/2)
			{
				machine.onItemScanned(transform.parent.parent.gameObject.GetComponent<ItemStatus>());

				GameObject.FindGameObjectWithTag("Pin").GetComponent<Pin>().unpinItem();

				playScanSound(other);
				OnTriggerExit(other);
			}
			//else
			//changeColor(other, Color.grey);
		}
		
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Scanner") 
		{
			changeIntensity(other.gameObject, 8.0f, 6.0f);
			currentScanDuration = 0.0f;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Scanner") 
		{
			changeIntensity(other.gameObject, 0.0f, 4.0f);
		}
	}

	void playScanSound(Collider other)
	{
		other.GetComponent<AudioSource>().Play();
		
	}


	void changeIntensity(GameObject scanner, float intensity, float range)
	{
		GameObject scannerLight = scanner.transform.root.FindChild ("scanner_field_voxel/scanner_light").gameObject;
		Light debugLight = scannerLight.light;
		debugLight.intensity = intensity;
		debugLight.range = range;
	}

}

 
