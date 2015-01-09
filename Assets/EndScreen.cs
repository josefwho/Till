using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndScreen : MonoBehaviour {

	private Text soldItemsRegular;
	private TillStateMachine till;

	// Use this for initialization
	void Start () {
		bool bla = false;

		till = gameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();
		soldItemsRegular = transform.Find("soldItemsRegular").GetComponent<Text> ();

		string defaultText = soldItemsRegular.text;
		addScoreToText (out defaultText, till.countScannedObjects);
	}

	void addScoreToText(out string text, int score )
	{
//		text.find
	}

	// Update is called once per frame
	void Update () {
	
	}
}
