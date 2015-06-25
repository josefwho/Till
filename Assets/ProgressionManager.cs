using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ProgressionManager : MonoBehaviour {

	TillStateMachine till;
	BonusManager bonus;

	string[] unlockables;
	int unlockedIndex = 0;

	public Canvas popup;
	public CanvasGroup popupGroup;
	Text nextUnlockable;
	Text newCustomerUnlocked;
	Text newItemsUnlocked;

	public bool unlockAll = false;

	Dictionary<string, string> howToUnlockNext;
	Sprite[] unlockableSprites;

	// Use this for initialization
	void Awake () {
		till = GetComponent<TillStateMachine> ();
		bonus = GetComponent<BonusManager> ();

		if (popup)
		{
//			popupGroup = popup.GetComponent<CanvasGroup>();
//			popupGroup.alpha = 0;
//			popupGroup.interactable = false;
			popup.enabled = false;

			Transform t = popup.transform.Find("Content/BackgroundPurpleBottom/nextUnlockable");
			if(t)
				nextUnlockable = t.GetComponent<Text>();
			t = popup.transform.Find ("Content/newCustomerUnlocked");
			if(t)
				newCustomerUnlocked = t.GetComponent<Text>();

			t = popup.transform.Find ("Content/newItemsUnlocked");
			if(t)
				newItemsUnlocked = t.GetComponent<Text>();
		}

		unlockableSprites = Resources.LoadAll<Sprite> ("UnlockableSprites");

		string[] temp = { "Hippie", "junk", "Hipster", "alcohol", "vegetableFruitSet", "RichLady", "premium", "exotic", "Business", "nofood", "Janitor"  };
		unlockables = temp;

		howToUnlockNext = new Dictionary<string, string> ();
		howToUnlockNext.Add ("Hippie", "Achieve minimum wage to unlock more.");
		howToUnlockNext.Add ("junk", "Press the \"Visit chesto.com\" button to unlock more.");
		howToUnlockNext.Add ("Hipster", "Work in over time until 21:00 to unlock more.");
		howToUnlockNext.Add ("alcohol", "Give a customer a shopping item for free to unlock more.");
		howToUnlockNext.Add ("vegetableFruitSet", "Get a bonus of 6 or higher to unlock more.");
		howToUnlockNext.Add ("RichLady", "Scan an item more than once to unlock more.");
		howToUnlockNext.Add ("premium", "Work less than 15 minutes in unpaid overtime to unlock more.");
		howToUnlockNext.Add ("exotic", "Get a bonus of 15 or higher to unlock more.");
		howToUnlockNext.Add ("Business", "Earn 1200 € or more to unlock more.");
		howToUnlockNext.Add ("nofood", "Drop 15 or more items on the floor to unlock the last customer.");


	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Debug.isDebugBuild && Input.GetKey (KeyCode.R) && Input.GetKey (KeyCode.LeftCommand)) 
		{
			resetProgress();
		}

		
		if (Debug.isDebugBuild && Input.GetKeyDown (KeyCode.U) && Input.GetKey (KeyCode.LeftCommand)) 
		{
			unlockNext();
		}
		


		if (popup.enabled && Input.GetMouseButtonUp (0))
		{
//			popupGroup.alpha = 0;
//			popupGroup.interactable = false;
			popup.enabled = false;
		}

	}

	public bool isCustomerUnlocked(CustomerProfile profile)
	{
		if (unlockAll)
			return true;

		if (profile.name == "Family" || profile.name == "Proletarian") 
		{
			return true;
		}

		return isUnlocked(profile.name);
	}

	public bool isItemUnlocked(ItemInfo item)
	{
		if (unlockAll)
			return true;

		if (item.tags.IndexOf ("starterset") > -1)
			return true;

		foreach (string key in unlockables) 
		{			
			if (item.tags.IndexOf (key) > -1)
				return isUnlocked (key);
		}

//		if (item.tags.IndexOf ("HippieSet") > -1)
//			return isUnlocked ("Hippie");
//
//		if (item.tags.IndexOf ("RichLadySet") > -1)
//			return isUnlocked ("RichLady");

//		if(item.tags.IndexOf("PremiumSet"

		return false;
	}

	public void progress(float wage, float profit, float minimumWage)
	{
		if (unlockAll || isUnlocked ("Janitor")) {
				return;
		} else if (isUnlocked ("nofood")) {
				if(till.countItemsOnFloor > 15)
						unlock ("Janitor");
		} else if (isUnlocked ("Business")) {
				if (wage > 1200)
						unlock ("nofood");
		} else if (isUnlocked ("exotic")) {
				if (bonus.maxBonus > 15)
						unlock ("Business");
		} else if (isUnlocked ("premium")) {
				if (till.timeTaken < till.shiftDuration + 15)
						unlock ("exotic");
		} else if (isUnlocked ("RichLady")) {
				if (till.countMultipleScannedItems > 0)
						unlock ("premium");
		} else if (isUnlocked ("vegetableFruitSet")) {
				if (bonus.maxBonus > 6)
						unlock ("RichLady");
		} else if (isUnlocked ("alcohol")) {
				if (till.countUnscannedItems > 0)
					unlock ("vegetableFruitSet");
		} else if (isUnlocked ("Hipster")) {
				if (till.timeTaken > till.shiftDuration + 60)
					unlock ("alcohol");
	//this step is called directly once the "visit chesto.com button has been pressed"
//				} else if (isUnlocked ("junk")) {
//						if (till.chestoComButtonPressed)
//							unlock ("Hipster");
		} else if (isUnlocked ("Hippie")) {
				if (wage > minimumWage)
					unlock ("junk");
		} else {
			unlock ("Hippie");
		}
		
		
//		if (!isUnlocked ("Hippie")) 
//			unlock ("Hippie");
//
//		if (!isUnlocked ("RichLady") && wage > 1000)
//			unlock ("RichLady");
//		
//		if (!isUnlocked ("premium") && bonus.maxBonus >= 10)
//			unlock ("premium");
	}

	public bool isUnlocked(string key)
	{
		return unlockAll || PlayerPrefs.GetInt (key) == 1;
	}

	public void unlock(string key)
	{
		Debug.Log ("unlocked " + key);

		PlayerPrefs.SetInt (key, 1);
		
		PlayerPrefs.Save ();

		if (popup)
		{
			foreach(Sprite s in unlockableSprites)
			{
				if(s.name == key)
				{
					popup.transform.Find ("Content/Sprite").GetComponent<Image>().sprite = s;
				}
			}

			//a customer always starts with an uppercase letter
			newItemsUnlocked.enabled = !char.IsUpper(key[0]);
			newCustomerUnlocked.enabled = char.IsUpper(key[0]);

			nextUnlockable.text = howToUnlockNext[key];
			popup.enabled = true;
//			popupGroup.alpha = 1;
//			popupGroup.interactable = true;
		}
	}

	void resetProgress()
	{
		PlayerPrefs.DeleteAll ();
	}

	void unlockNext()
	{
		if (unlockAll || isUnlocked ("Janitor")) {
			return;
		} else if (isUnlocked ("nofood")) {
			unlock ("Janitor");
		} else if (isUnlocked ("Business")) {
			unlock ("nofood");
		} else if (isUnlocked ("exotic")) {
			unlock ("Business");
		} else if (isUnlocked ("premium")) {
			unlock ("exotic");
		} else if (isUnlocked ("RichLady")) {
			unlock ("premium");
		} else if (isUnlocked ("vegetableFruitSet")) {
			unlock ("RichLady");
		} else if (isUnlocked ("alcohol")) {
			unlock ("vegetableFruitSet");
		} else if (isUnlocked ("Hipster")) {
			unlock ("alcohol");
		} else if (isUnlocked ("junk")) {
			unlock ("Hipster");
		} else if (isUnlocked ("Hippie")) {
			unlock ("junk");
		} else {
			unlock ("Hippie");
		}
	}
}
