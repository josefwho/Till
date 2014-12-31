using UnityEngine;
using System.Collections;

public class DisableBeltPartCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "ConveyorBelt") 
		{
			other.gameObject.layer = 8;
		}
	}
}
