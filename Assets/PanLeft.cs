using UnityEngine;
using System.Collections;

public class PanLeft : MonoBehaviour {

	public float speed = 2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		float x = transform.position.x;
//		x -= speed;
//
//		transform.position

		transform.position -= new Vector3 (speed, 0, 0);
	}
}
