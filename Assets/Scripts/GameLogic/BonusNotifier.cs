using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class BonusNotifier : MonoBehaviour
{
	public float showDuration = 0.6f;
	public Vector3 translateBy = Vector3.zero;
	public float fadeDuration = 0.2f;

	private float timeShown = 0;
	private Vector3 initialPosition = Vector3.zero;
	private Vector3 startPosition = Vector3.zero;
	private Text text;
	private Color initialTextColor = Color.white;

	private float totalDuration = 0;
	private Vector3 targetPosition = Vector3.zero;
	private Vector3 initialTargetPosition = Vector3.zero;

	private RectTransform rectTransform;
	private RectTransform canvasRectTransform;

		// Use this for initialization
	void Start ()
	{
		text = GetComponent<Text> ();

		rectTransform = GetComponent<RectTransform> ();
		canvasRectTransform = text.canvas.GetComponent<RectTransform> ();

		initialPosition = transform.position;
		initialTargetPosition = initialPosition + translateBy;
		
		startPosition = initialPosition;
		targetPosition = initialTargetPosition;

		initialTextColor = text.color;

		Color temp = initialTextColor;
		temp.a = 0;
		text.color = temp;

		totalDuration = fadeDuration * 2 + showDuration;

		enabled = false;
	}

	public void setNewPosition(Vector3 worldPosition)
	{
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);
		
		rectTransform.anchoredPosition = screenPoint - canvasRectTransform.sizeDelta / 2f;

		startPosition = transform.position;
		targetPosition = startPosition + translateBy;
	}

	void OnEnable ()
	{
//			StartCoroutine (animate());
//			StartCoroutine (translateBy ());
		
		timeShown = 0;
		
		if (text != null) {
		
			transform.position = initialPosition;

			Color temp = initialTextColor;
			temp.a = 0;
			text.color = temp;
		}
	}
	
	void OnDisable ()
	{
		
		startPosition = initialPosition;
		targetPosition = initialTargetPosition;

		if (text != null) {
			
			transform.position = startPosition;
			
			Color temp = initialTextColor;
			temp.a = 0;
			text.color = temp;
		}
		
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	timeShown += Time.deltaTime;

		//translating
		transform.position = Vector3.Lerp(startPosition, targetPosition, timeShown/totalDuration);

		//fading in
		if (timeShown < fadeDuration) 
		{
			Color temp = initialTextColor;
			temp.a = Mathf.Lerp (0, initialTextColor.a, timeShown / fadeDuration);
			text.color = temp;
		} 
		else 
		{
			//showing
			if(timeShown < fadeDuration + showDuration)
			{
				text.color = initialTextColor;
			}
			//fading out
			else if(timeShown < totalDuration)
			{
				Color temp = initialTextColor;
				temp.a = Mathf.Lerp (initialTextColor.a, 0 , (timeShown-(fadeDuration+showDuration)) / fadeDuration);
				text.color = temp;
				
			}
			//stopping
			else
			{
				Color temp = initialTextColor;
				temp.a = 0;
				text.color = temp;

//					gameObject.SetActive(false);
			enabled = false;
			}

		}
	}
}

