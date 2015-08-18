using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanLeft : MonoBehaviour {

	public float speed = 2;
	float initialSpeed = 2;
	public float tickerCount = 2;
	float resetAfterMultiplesOfWidth = 1;

	float pixelsMoved = 0;

	bool pastLeftEdge = false;

	RectTransform rectTransform;

	void Start () {
		initialSpeed = speed;
		rectTransform = (RectTransform)transform;

		resetAfterMultiplesOfWidth = Mathf.Max (0, tickerCount - Screen.width / (rectTransform.rect.width));

//		
//		if (Screen.width >= 1680) {
//			GetComponent<Text> ().fontSize = 60;
//		}
//		else if (Screen.width >= 1440) {
//				GetComponent<Text> ().fontSize = 30;
//		}
	}
	
	void Update () {

		if (Input.GetKeyDown (KeyCode.Space)) {
			speed = initialSpeed * 4;
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			speed = initialSpeed;
		}

		transform.position -= new Vector3 (speed, 0, 0);

		if (!pastLeftEdge && rectTransform.anchoredPosition.x < (-1 * (Screen.width + rectTransform.rect.width))) {
				pastLeftEdge = true;
		}

		if (pastLeftEdge && (pixelsMoved += speed) > resetAfterMultiplesOfWidth * rectTransform.rect.width) {
			pixelsMoved = 0;
			pastLeftEdge = false;
			rectTransform.anchoredPosition = new Vector3(rectTransform.rect.width/2, 0, 0);
		}

	}


}
