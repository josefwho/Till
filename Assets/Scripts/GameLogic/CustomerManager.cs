using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Variations
{
	Stereotype = 0,
	Standard,
	AntiStereotype
}

public class CustomerVariation
{
	public float probability;
	public string tags;
	public Vector2 countRange;
	public string specificItems;
}

public class CustomerProfile
{
	public string name;
	public float probability;
	public string prefabPath;
	public CustomerVariation[] variations = new CustomerVariation[3];
}

public class CustomerManager : MonoBehaviour {

	public TextAsset[] spreadsheets;

	private Dictionary<string, CustomerProfile> profiles;
	private ArrayList profileNames;	//needed to pick a random profile

	// Use this for initialization
	void Start () {
		profileNames = new ArrayList ();
		profiles = new Dictionary<string, CustomerProfile> ();

		foreach (TextAsset s in spreadsheets) 
		{
			print ("reading in data of profile " + s.name);
			
			string[] lines = s.text.Split ('\n');

			//get the name - it's the 2nd column on the 2nd line
			string[] columns = lines[1].Split (',');
			string name = columns[1];

			if(profiles.ContainsKey(name))
			{
				print("WARNING: a profile with name " + name + " already exists! Ignoring this one!");
				continue;
			}

			profileNames.Add(name);

			CustomerProfile currentProfile = new CustomerProfile();
			currentProfile.name = name;

			//get the probability - it's the 2nd column on the 3rd line
			columns = lines[2].Split (',');
			currentProfile.probability = (float)System.Convert.ToDouble(columns[1]);

			//now get the probability for our variations - columns 3 - 5
			for (int v = 0; v < 2; v++) {
				
				currentProfile.variations[v] = new CustomerVariation();
				currentProfile.variations[v].probability = (float)System.Convert.ToDouble(columns[v+2]);
			}

			//get the prefabfolder path - it's the 2nd column on the 7th line
			columns = lines[6].Split (',');
			currentProfile.prefabPath = columns[1];

			//now get the item wishlist of our variations - lines 4 - 6; columns 3 - 5
			//first item count range
			columns = lines[3].Split (',');
			for (int v = 0; v < 2; v++) {
				string[] countRangeText = columns[v+2].Split('-');
				currentProfile.variations[v].countRange = new Vector2(System.Convert.ToInt16(countRangeText[0].Trim()), System.Convert.ToInt16(countRangeText[1].Trim()));

			}
			//tags
			columns = lines[4].Split (',');
			for (int v = 0; v < 2; v++)
				currentProfile.variations[v].tags = columns[v+2];
			//specific items
			columns = lines[5].Split (',');
			for (int v = 0; v < 2; v++)
				currentProfile.variations[v].specificItems = columns[v+2];

			//finally add the CustomerProfile to our dictionary containing all customers
			profiles.Add (name, currentProfile);

			print ("done reading in values");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public CustomerProfile getRandomProfile()
	{
		int index = Random.Range (0, profileNames.Count);
		CustomerProfile profile = null;
		int safetyCounter = 0;
		//pick the random customer if it exists and if it's probability is high enough
		while (safetyCounter < 100) 
		{
			if (profiles.TryGetValue (profileNames [index] as string, out profile) && profile.probability > Random.value)
				return profile;

			safetyCounter++;
		}

		return null;
	}
}
