using UnityEngine;

using System.Collections;

public class ProgressionManager : MonoBehaviour {

	TillStateMachine till;
	BonusManager bonus;

	string[] unlockables;
	int unlockedIndex = 0;

	public bool unlockAll = false;

	// Use this for initialization
	void Awake () {
		till = GetComponent<TillStateMachine> ();
		bonus = GetComponent<BonusManager> ();

		string[] temp = { "Hippie", "RichLady", "premium" };
		unlockables = temp;

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Debug.isDebugBuild && Input.GetKey (KeyCode.R) && Input.GetKey (KeyCode.LeftCommand)) 
		{
			resetProgress();
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
		} else if (isUnlocked ("vetegableFruitSet")) {
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
	}

	void resetProgress()
	{
		PlayerPrefs.DeleteAll ();
	}
}
