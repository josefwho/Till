using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetToUnlockNextString : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Text UIText = GetComponent<Text> ();
		GameObject controller = GameObject.FindGameObjectWithTag ("GameController");
		ProgressionManager progression = controller.GetComponent<ProgressionManager> ();
		UIText.text = "+++ " + progression.getUnlockNextString() + " +++";
	}
}
