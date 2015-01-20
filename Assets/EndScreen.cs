using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndScreen : MonoBehaviour {

	private Text soldItemsRegular;
	private TillStateMachine till;
	
	public float minimumWage = 1159.08f; //monthly wage in euros
	public int itemsNeededForMinimumWage = 5;
	public float itemWorth = 2;	//in euros/ will be deducted/added to minimumwage per item that is different from itemsNeeded
	public int fireAtItemDiff = 10;

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

		text = transform.Find("Background/wageMonthlyNumber").GetComponent<Text> ();
		text.text = string.Format(text.text, wage.ToString("N2"));

		text = transform.Find("Background/multipleScans").GetComponent<Text> ();
		text.text = string.Format(text.text, till.countMultipleScannedItems);
		
		text = transform.Find("Background/multipleScansNumber").GetComponent<Text> ();
		text.text = string.Format(text.text, till.countMultipleScannedItems * itemWorth);
		
		text = transform.Find("Background/wageMonthlyFinalNumber").GetComponent<Text> ();
		text.text = string.Format(text.text, (wage - till.countMultipleScannedItems * itemWorth).ToString("N2"));
		
		text = transform.Find("Background/wageDaily").GetComponent<Text> ();
		text.text = string.Format(text.text, (wage/21.5f).ToString("N2"));


		//find out which stamp to show
		GameObject stamp;
		if (itemDiff < 0) 
		{
			stamp = transform.Find ("stamp below").gameObject;
			//show fired stamp
			if(itemDiff < -1 * fireAtItemDiff)
				transform.Find("stamp fired").GetComponent<Image> ().enabled = true;

		}
		else
			stamp = transform.Find("stamp above").gameObject;

		Text stampText = stamp.transform.Find ("Text").GetComponent<Text> ();
		stamp.GetComponent<Image> ().enabled = true;
		stampText.text = (Mathf.Abs(itemWorth * itemDiff)).ToString() + " €";

	}

	// Update is called once per frame
	void Update () {
	
	}
}
