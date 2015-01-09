using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndScreen : MonoBehaviour {

	private Text soldItemsRegular;
	private TillStateMachine till;
	
	public float minimumWage = 1159.08f; //monthly wage in euros
	public int itemsNeededForMinimumWage = 5;
	public float itemWorth = 2;	//in euros/ will be deducted/added to minimumwage per item that is different from itemsNeeded

	// Use this for initialization
	void Start () {
		till = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();
		Text text = transform.Find("Background/soldItemsRegular").GetComponent<Text> ();

		//		string replaceWith = string.Format(text.text, till.countScannedObjects);
		text.text = string.Format(text.text, till.countSoldRegular);

		text = transform.Find("Background/soldItemsOvertime").GetComponent<Text> ();
		text.text = string.Format(text.text, till.countSoldOvertime);

		float wage = minimumWage;
		float itemDiff = till.countSoldRegular - itemsNeededForMinimumWage;
		wage += itemWorth * itemDiff;

		text = transform.Find("Background/wageDaily").GetComponent<Text> ();
		text.text = string.Format(text.text, wage/21.5f);

		text = transform.Find("Background/wageMonthly").GetComponent<Text> ();
		text.text = string.Format(text.text, wage);

	}

	// Update is called once per frame
	void Update () {
	
	}
}
