using UnityEngine;
using System.Collections;

public class ShowStatistic : MonoBehaviour {

	public enum Statistics { Fired, BelowMinimumWage, AboveMinimumWage, TotalItemsScanned, TotalShifts }
	public Statistics show = Statistics.Fired;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
