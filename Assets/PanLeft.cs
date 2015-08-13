using UnityEngine;
using System.Collections;

public class PanLeft : MonoBehaviour {

	public float speed = 2;
	public float tickerCount = 2;

	float pixelsMoved = 0;

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

		if ((pixelsMoved += speed) > tickerCount * 328) {
			pixelsMoved = 0;
			transform.position += new Vector3(tickerCount * 328, 0, 0);
		}

	}


}
