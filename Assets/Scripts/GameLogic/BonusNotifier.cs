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
	private Text text;
	private Color initialTextColor = Color.white;

	private float totalDuration = 0;
	private Vector3 targetPosition = Vector3.zero;

		// Use this for initialization
	void Start ()
	{
		initialPosition = transform.position;
		targetPosition = initialPosition + translateBy;

		text = GetComponent<Text> ();
		initialTextColor = text.color;

		Color temp = initialTextColor;
		temp.a = 0;
		text.color = temp;

		totalDuration = fadeDuration * 2 + showDuration;

		enabled = false;
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
		if (text != null) {
			
			transform.position = initialPosition;
			
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
			transform.position = Vector3.Lerp(initialPosition, targetPosition, timeShown/totalDuration);

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

