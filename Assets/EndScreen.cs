using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndScreen : MonoBehaviour {

	private Text soldItemsRegular;

	// Use this for initialization
	void Start () {
		bool bla = false;

		soldItemsRegular = transform.Find("soldItemsRegular").GetComponent<Text> ();

		string defaultText = soldItemsRegular.text;
		addScoreToText(out defaultText, 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
