using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {
	public int sceneIndex;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void load()
	{
		Time.timeScale = 1.0f;
		Application.LoadLevel (sceneIndex);
	}

	public void unpause()
	{
		Time.timeScale = 1.0f;
		GameObject.Find ("Pause Canvas").GetComponent<Canvas> ().enabled = false;
	}
}
