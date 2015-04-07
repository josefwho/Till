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

	public RectTransform stampBelow;
	public RectTransform stampAbove;
	public RectTransform stampFired;
	public Vector3 scaleFactor = new Vector3(10.0f, 10.0f, 10.0f);
	public float scaleDuration = 0.3f;

	private bool fired = false;
	private bool belowWage = false;
	private bool aboveWage = false;

	public float waitingSeconds = 5.0f;
	
	string databaseUrl = "http://brokenrul.es/games/Chesto/submit.php";

	// Use this for initialization
	void Start () {
		till = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();

		Text text = transform.Find("Submit Score Button/Text").GetComponent<Text> ();
		text.text = string.Format (text.text, till.countSoldRegular + till.countSoldOvertime);

		text = transform.Find("Background/soldItemsRegular/Number").GetComponent<Text> ();
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
		text.text = string.Format(text.text, till.countMultipleScannedItems * itemWorth * 2);
		
//		text = transform.Find("Background/notScanned").GetComponent<Text> ();
//		text.text = string.Format(text.text, till.countUnscannedItems);
//		text = transform.Find("Background/notScannedNumber").GetComponent<Text> ();
//		text.text = string.Format(text.text, till.countUnscannedItems * itemWorth * 2);

		text = transform.Find("Background/bonusNumber").GetComponent<Text> ();
		text.text = string.Format(text.text, till.bonus);

		wage -= (till.countMultipleScannedItems) * itemWorth * 2; //(till.countMultipleScannedItems + till.countUnscannedItems)
		wage += till.bonus;
		
		float diff = wage - minimumWage;

		if (diff < -1 * fireAtDiff)
				wage = 0;

		text = transform.Find("Background/wageMonthlyFinalNumber").GetComponent<Text> ();
		text.text = string.Format(text.text, wage.ToString("N2"));

		StartCoroutine (stagingStamps (diff, text));

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
		float profit = (till.countSoldRegular + till.countSoldOvertime) * marginPerItem;

		profit -= wage/21.65f;

		form.AddField("profit", profit.ToString());

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

	IEnumerator fadeInManager()
	{	
		Color colorManager = transform.Find("kessler").GetComponent<Image> ().color;
		Color colorBubble = transform.Find("kessler/bubble").GetComponent<Image> ().color;
		Color colorTextFired = transform.Find("kessler/fired").GetComponent<Text> ().color;
		Color colorTextWorkHarder = transform.Find("kessler/workHarder").GetComponent<Text> ().color;
		Color colorTextCostingALot = transform.Find("kessler/costingALot").GetComponent<Text> ().color;
		
		float startTime = 0.0f;
		float fadeTime = 0.5f;
		
		while (startTime < fadeTime) 
		{
			startTime += Time.deltaTime;
			
			float newAlpha = Mathf.Lerp(0, 1, startTime/fadeTime);
			colorManager.a = newAlpha;
			colorBubble.a = newAlpha;

			if(aboveWage){
				colorTextCostingALot.a = newAlpha;
			}else if(belowWage && fired){
				colorTextFired.a = newAlpha;
			}else{
				colorTextWorkHarder.a = newAlpha;
			}


			transform.Find("kessler").GetComponent<Image> ().color = colorManager;
			transform.Find("kessler/bubble").GetComponent<Image> ().color = colorBubble;
			transform.Find("kessler/fired").GetComponent<Text> ().color = colorTextFired;
			transform.Find("kessler/workHarder").GetComponent<Text> ().color = colorTextWorkHarder;
			transform.Find("kessler/costingALot").GetComponent<Text> ().color = colorTextCostingALot;
			
			yield return null;
		}

		
		//reset alpha so it's visible when shown next time
//		colorManager.a = 1;
//		colorBubble.a = 1;
//		colorText.a = 1;
	}

	IEnumerator stagingStamps(float diff, Text text)
	{
		//find out which stamp to show
		GameObject stamp;
		if (diff < 0) {
			stamp = transform.Find ("stamp below").gameObject;
			text.color = Color.red;
			belowWage = true;
			} else {
			stamp = transform.Find ("stamp above").gameObject;
			text.color = Color.green;
			aboveWage = true;

			
		}
		yield return new WaitForSeconds(waitingSeconds);
		stamp.GetComponent<Image> ().enabled = true;
		StartCoroutine(scale(new Vector3(0.75f, 0.75f, 0.75f), (RectTransform)stamp.transform));

		if (diff < -1 * fireAtDiff) {
			fired =true;
		}

		StartCoroutine(fadeInManager());
		//show fired stamp
		yield return new WaitForSeconds(waitingSeconds);
		if (fired) {
				transform.Find ("stamp fired").GetComponent<Image> ().enabled = true;
				StartCoroutine(scale(new Vector3(1.0f, 1.0f, 1.0f), stampFired));
		}
			
	}

	IEnumerator scale(Vector3 targetScale, RectTransform stamp)
	{
		float timeTaken = 0.0f;
		Vector3 startScale = scaleFactor;
		stamp.localScale = startScale;
		while (stamp.localScale.x > targetScale.x) 
		{
			Vector3 curScale = Vector3.Lerp(startScale, targetScale, timeTaken/scaleDuration);
			stamp.localScale = curScale;
			
			yield return null;
			
			timeTaken += Time.deltaTime;
		}
	}

}
