using UnityEngine;
using System.Collections;

public class DisableBeltPartCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		//TODO: actually cahnge collision layer of shopiing items

//		if (other.gameObject.tag == "ConveyorBelt") 
//		{
//			other.gameObject.layer = 8;
//		}
	}
}
