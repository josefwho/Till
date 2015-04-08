﻿using UnityEngine;
using System.Collections;

public class ProgressionManager : MonoBehaviour {

	TillStateMachine till;
	BonusManager bonus;

	// Use this for initialization
	void Start () {
		till = GetComponent<TillStateMachine> ();
		bonus = GetComponent<BonusManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool isCustomerUnlocked(CustomerProfile profile)
	{
		if (profile.name == "Family" || profile.name == "Proletarian") 
		{
			return true;
		}

		return isUnlocked(profile.name);
	}

	public void progress(float wage, float profit)
	{
		if (!isUnlocked ("Hippie")) 
			unlock ("Hippie");

		if (!isUnlocked ("RichLady") && wage > 1000)
			unlock ("RichLady");
		
		if (!isUnlocked ("PremiumSet") && bonus.maxBonus >= 10)
			unlock ("PremiumSet");


		PlayerPrefs.Save ();
	}

	bool isUnlocked(string key)
	{
		return PlayerPrefs.GetInt (key) == 1;
	}

	void unlock(string key)
	{
		PlayerPrefs.SetInt (key, 1);
	}
}
