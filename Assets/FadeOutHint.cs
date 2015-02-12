using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeOutHint : MonoBehaviour {
	public float duration;
	public KeyCode key;
	public float fadeDuration;

	private float currentDuration;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		currentDuration += Time.deltaTime;

		if (Input.GetKeyDown(key) || currentDuration > duration)
		{
			StartCoroutine(fadeOut());
		}
	}

	IEnumerator fadeOut()
	{
		Image background = GetComponent<Image> ();
		Color backgroundColor = background.color;
		float backgroundA = background.color.a;

		Text text = transform.GetComponentsInChildren<Text> ()[0];
		Color textColor = text.color;

		float curFadeDur = 0;
		while (curFadeDur < fadeDuration) 
		{
			curFadeDur += Time.deltaTime;
			textColor.a = Mathf.Lerp(1, 0, curFadeDur/fadeDuration);
			text.color = textColor;

			backgroundColor.a = Mathf.Lerp(backgroundA, 0, curFadeDur/fadeDuration); 
			background.color = backgroundColor;

			yield return null;
		}

		gameObject.SetActive (false);
	}
}
