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

			if(currentScanDuration > scanDuration)
			{
				transform.parent.parent.gameObject.GetComponent<ItemStatus>().scanned++;
				GameObject.Find("Scanner/Scanner Trigger").GetComponent<ScannerTrigger>().unpinItem();

				playScanSound(other);
				OnTriggerExit(other);

				machine.countScannedObjects++;
				machine.setCountText ();
			}
			//else
			//changeColor(other, Color.grey);
		}
		
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Scanner") 
		{
			changeColor (other.gameObject, Color.green);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Scanner") 
		{
			changeColor (other.gameObject, Color.grey);
		}
	}
	
	void OnEnable()
	{
		currentScanDuration = 0.0f;
	}

	void playScanSound(Collider other)
	{
		other.GetComponent<AudioSource>().Play();
		
	}
	
	void changeColor(GameObject other, Color col)
	{
		other.renderer.material.shader = Shader.Find("Diffuse");
		other.renderer.material.SetColor("_Color", col);
		
		//other.renderer.material.shader = Shader.Find("Specular");
		//other.renderer.material.SetColor("_SpecColor", Color.red);
	}

}

 
