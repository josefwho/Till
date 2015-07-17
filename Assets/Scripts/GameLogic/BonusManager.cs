using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BonusManager : MonoBehaviour
{
	
	public float currentBonus = 1;
	public int bonusIncrement = 1;
	public float bonusDecreaseSpeed = 0.3f;
	public float increaseDecreaseSpeedThreshold = 3;
	public float maxBonus = 0;

	public Text currentBonusText = null;
	public GameObject bonusAddedNotifier = null;

	private string initialBonusText;
	float sinceLastScan;

	// Use this for initialization
	void Start ()
	{
		if (bonusAddedNotifier != null) 
		{
//			bonusAddedNotifier.SetActive (false);
//			bonusAddedNotifier.GetComponent<BonusNotifier>().enabled = false;
		}

		Color temp = currentBonusText.color;
		temp.a = 1;
		currentBonusText.color = temp;

		initialBonusText = currentBonusText.text;

		currentBonusText.text = string.Format (initialBonusText, "0");

		sinceLastScan = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (currentBonus > 0) 
		{
			float decrease = bonusDecreaseSpeed;

//			if(sinceLastScan > increaseDecreaseSpeedThreshold)
//			{
//				decrease = bonusDecreaseSpeed * Mathf.Lerp(1,2,(sinceLastScan-increaseDecreaseSpeedThreshold)/3.0f);
//
//				Debug.Log("faster decrease speed: " + decrease);
//			}

 			currentBonus = Mathf.Max(currentBonus - decrease * Time.deltaTime, 0);
			
			sinceLastScan += Time.deltaTime;
			
//			int flooredBonus = Mathf.FloorToInt (currentBonus);
//			
//			Color temp = currentBonusText.color;
//
//			if(currentBonus > 1)
//			{
////				temp.a = Mathf.Lerp (0.3f, 1, currentBonus - flooredBonus);
//			}
//			else
//			{
//				temp.a = 0;
//			}
//				
//			currentBonusText.color = temp;
//
////			Debug.Log("cB: " + currentBonus.ToString("N2") + " fB: " + flooredBonus + " a: " + temp.a);
		}
		////		currentBonusText.text = string.Format(initialBonusText, calculateBonus ().ToString());
	}

	public void onItemScanned(ItemStatus status)
	{
		
		if (bonusAddedNotifier != null && currentBonus > 1) 
		{
			float roundedBonus = calculateBonus(); 

			currentBonusText.text = string.Format(initialBonusText, roundedBonus.ToString());
			//			bonusAddedNotifier.SetActive (true);
			bonusAddedNotifier.GetComponent<BonusNotifier>().enabled = true;

			if(roundedBonus > maxBonus)
				maxBonus = roundedBonus;
		}

		currentBonus += bonusIncrement;


		sinceLastScan = 0.0f;
	}

	public void resetBonus()
	{
		if (currentBonus > 1) 
		{
			currentBonusText.text = string.Format (initialBonusText, "Bonus Lost").Substring(1);

			bonusAddedNotifier.GetComponent<BonusNotifier> ().enabled = true;

			bonusAddedNotifier.GetComponent<BonusNotifier> ().setNewPosition(new Vector3(-4.8f, 3.0f, 1.7f));
		}

		currentBonus = 0.0f;
	}

	public int calculateBonus()
	{
		return Mathf.FloorToInt(currentBonus) * 2;
	}
}

