using UnityEngine;
using System.Collections;

public class ConveyorBeltPart : MonoBehaviour {

	public float speed; //units/sec

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space))
			transform.Translate(speed * Time.deltaTime, 0, 0);
	}
}
