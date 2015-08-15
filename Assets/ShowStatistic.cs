using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShowStatistic : MonoBehaviour {

	public enum Statistics { Fired, BelowMinimumWage, AboveMinimumWage, TotalItemsScanned, TotalShifts }
	public Statistics show = Statistics.Fired;

	// Use this for initialization
	void Start () {

		Dictionary<string, string> descriptions = new Dictionary<string, string> ();
		descriptions.Add ("Fired", "Fired Count: ");
		descriptions.Add ("BelowMinimumWage", "Below Minimum Wage Count: ");
		descriptions.Add ("AboveMinimumWage", "Above Minimum Wage Count: ");
		descriptions.Add ("TotalItemsScanned", "Total Items Scanned: ");
		descriptions.Add ("TotalShifts", "Total Shifts Played: ");
		
		Text UIText = GetComponent<Text> ();
		string key = show.ToString ();

		UIText.text = "+++ " + descriptions[key] + PlayerPrefs.GetInt (key) + " +++";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
