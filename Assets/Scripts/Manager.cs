using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void commentOnMinimumWage()
	{
		Debug.Log ("You are now making more than minimum wage!");

		//TODO: call your co-routine to show manager and let her speak from here
	}
	
	public void commentOnOvertime()
	{
		Debug.Log ("Don't think CHESTO is going to pay you for your overtime!");

		//TODO: call your co-routine to show manager and let her speak from here
	}
}
