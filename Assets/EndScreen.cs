using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndScreen : MonoBehaviour {

	private Text soldItemsRegular;
	private TillStateMachine till;

	// Use this for initialization
	void Start () {
		bool bla = false;

		till = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();
		soldItemsRegular = transform.Find("soldItemsRegular").GetComponent<Text> ();

		string replaceWith = string.Format(soldItemsRegular.text, till.countScannedObjects);
		soldItemsRegular.text = replaceWith;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
