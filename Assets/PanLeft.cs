using UnityEngine;
using System.Collections;

public class PanLeft : MonoBehaviour {

	public float speed = 2;
	public float tickerCount = 2;
	float resetAfterMultiplesOfWidth = 1;

	float pixelsMoved = 0;

	bool pastLeftEdge = false;

	RectTransform rectTransform;

	void Start () {
		rectTransform = (RectTransform)transform;

		resetAfterMultiplesOfWidth = Mathf.Max (0, tickerCount - Screen.width / (rectTransform.rect.width));
	}
	
	void Update () {

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
