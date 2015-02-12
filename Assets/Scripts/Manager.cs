using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Manager : MonoBehaviour {
	public Vector3 targetPosition = new Vector3  (-1.12f, 2.45f, -0.46f);
	public float walkSpeed = 2;

	private Text speechBubbleText;
	private Vector3 originalPosition;
	private Vector3 direction;

	// Use this for initialization
	void Start () {

		originalPosition = transform.position;
		direction = (targetPosition - originalPosition).normalized;

		speechBubbleText = transform.Find ("Canvas/Button/Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void commentOnMinimumWage()
	{
 		Debug.Log ("You are now making more than minimum wage!");

		speechBubbleText.text = "You are now making more than minimum wage!";

		StartCoroutine (enterManager());
		//TODO: call your co-routine to show manager and let her speak from here
	}
	
	public void commentOnOvertime()
	{
		Debug.Log ("Don't think CHESTO is going to pay you for your overtime!");
		
		speechBubbleText.text = "Don't think CHESTO is going to pay you for your overtime!";

		StartCoroutine (enterManager());
		//TODO: call your co-routine to show manager and let her speak from here
	}

	IEnumerator enterManager()
	{
				while ((transform.position - targetPosition).magnitude > 0.1f) {
						transform.Translate (direction * walkSpeed * Time.deltaTime, Space.World);

						yield return null;
				}

				yield return new WaitForSeconds (7.0f);

		
		while ((transform.position - originalPosition).magnitude > 0.1f)
		{
			transform.Translate (-direction * walkSpeed * Time.deltaTime, Space.World);
			
			yield return null;
		}
	}
}
