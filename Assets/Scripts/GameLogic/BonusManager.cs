using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BonusManager : MonoBehaviour
{
	
	public float currentBonus = 0;
	public int bonusIncrement = 1;
	public float bonusDecreaseSpeed = 0.3f;

	public Text currentBonusText = null;
	public GameObject bonusAddedNotifier = null;

	// Use this for initialization
	void Start ()
	{
		if (bonusAddedNotifier != null) 
		{
//						bonusAddedNotifier.SetActive (false);
			bonusAddedNotifier.GetComponent<BonusNotifier>().enabled = false;
				}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (currentBonus > 0) 
		{
			currentBonus = Mathf.Max(currentBonus - bonusDecreaseSpeed * Time.deltaTime, 0);
		}

		currentBonusText.text = currentBonus.ToString ("N3");
	}

	public void onItemScanned(ItemStatus status)
	{
		currentBonus += bonusIncrement;
		
		if (bonusAddedNotifier != null) 
		{
//			bonusAddedNotifier.SetActive (true);
			bonusAddedNotifier.GetComponent<BonusNotifier>().enabled = true;
				}
	}

	public void resetBonus()
	{
		currentBonus = 0.0f;
	}
}

