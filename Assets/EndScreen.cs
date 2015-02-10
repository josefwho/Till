using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndScreen : MonoBehaviour {

	private Text soldItemsRegular;
	private TillStateMachine till;
	private float wage = 0;
	
	public float minimumWage = 1159.08f; //monthly wage in euros
	public int itemsNeededForMinimumWage = 5;
	public float itemWorth = 2;	//in euros/ will be deducted/added to minimumwage per item that is different from itemsNeeded
	public int fireAtDiff = 150;
	public float marginPerItem = 3;
	
	
	string databaseUrl = "http://brokenrul.es/games/Chesto/submit.php";

	// Use this for initialization
	void Start () {
		till = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();
		Text text = transform.Find("Background/soldItemsRegular/Number").GetComponent<Text> ();

		//		string replaceWith = string.Format(text.text, till.countScannedObjects);
		text.text = string.Format(text.text, till.countSoldRegular);

		text = transform.Find("Background/soldItemsOvertime/Number").GetComponent<Text> ();
		text.text = string.Format(text.text, till.countSoldOvertime);

		wage = minimumWage;
		float itemDiff = till.countSoldRegular - itemsNeededForMinimumWage;
		wage += itemWorth * itemDiff;

		text = transform.Find("Background/multipleScans").GetComponent<Text> ();
		text.text = string.Format(text.text, till.countMultipleScannedItems);
		text = transform.Find("Background/multipleScansNumber").GetComponent<Text> ();
		text.text = string.Format(text.text, till.countMultipleScannedItems * itemWorth);
		
		text = transform.Find("Background/notScanned").GetComponent<Text> ();
		text.text = string.Format(text.text, till.countUnscannedItems);
		text = transform.Find("Background/notScannedNumber").GetComponent<Text> ();
		text.text = string.Format(text.text, till.countUnscannedItems * itemWorth);

		wage -= till.countMultipleScannedItems * itemWorth + till.countUnscannedItems * itemWorth;

		text = transform.Find("Background/wageMonthlyFinalNumber").GetComponent<Text> ();
		text.text = string.Format(text.text, wage.ToString("N2"));

		float diff = wage - minimumWage;


		//find out which stamp to show
		GameObject stamp;
		if (diff < 0) {
						stamp = transform.Find ("stamp below").gameObject;
						text.color = Color.red;
						//show fired stamp
						if (diff < -1 * fireAtDiff)
								transform.Find ("stamp fired").GetComponent<Image> ().enabled = true;

				} else {
						stamp = transform.Find ("stamp above").gameObject;
						text.color = Color.green;

				}

		stamp.GetComponent<Image> ().enabled = true;

		submit ();
//		Text stampText = stamp.transform.Find ("Text").GetComponent<Text> ();
//		stampText.text = (Mathf.Abs(itemWorth * itemDiff)).ToString() + " €";

	}

	// Update is called once per frame
	void Update () {
	
	}

	public void goToWebsite()
	{
		Application.OpenURL("http://brokenrul.es/games/Chesto/");
	}

	public void submit()
	{
		// Create a Web Form
		WWWForm form = new WWWForm();
		form.AddField("itemsRegular", till.countSoldRegular);
		form.AddField("itemsOvertime", till.countSoldOvertime);
		form.AddField("profit", ((till.countSoldRegular + till.countSoldOvertime) * marginPerItem - wage/21.65).ToString());

//		WWW w = new WWW(databaseUrl, form);
		StartCoroutine(sendData (form));
	}

	IEnumerator sendData(WWWForm form)
	{
		// Upload
		WWW w = new WWW(databaseUrl, form);
		
		yield return w;
		
		if (!string.IsNullOrEmpty(w.error))
			Debug.LogError(w.error);
		else
			Debug.Log("Finished Uploading score");
	}
}
