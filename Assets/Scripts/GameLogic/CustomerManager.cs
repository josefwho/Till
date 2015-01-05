using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerVariation
{
	public string type;
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
	
	public CustomerVariation getRandomVariation()
	{
		int index = Random.Range (0, variations.Length);
		int safetyCounter = 0;

		//pick the variation if its probability is high enough
		while (safetyCounter < 100) 
		{
			if (variations[index].probability > Random.value)
				return variations[index];
			
			index = Random.Range (0, variations.Length);
			safetyCounter++;
		}
		
		return variations[0];
	}
}

public class CustomerManager : MonoBehaviour {

	public TextAsset[] spreadsheets;

	private Dictionary<string, CustomerProfile> profiles;
	private ArrayList profileNames;	//needed to pick a random profile

	// Use this for initialization
	void Awake () {
		profileNames = new ArrayList ();
		profiles = new Dictionary<string, CustomerProfile> ();

		foreach (TextAsset s in spreadsheets) 
		{
//			print ("reading in data of profile " + s.name);
			
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
			for (int v = 0; v < 3; v++) {
				
				currentProfile.variations[v] = new CustomerVariation();
				currentProfile.variations[v].probability = (float)System.Convert.ToDouble(columns[v+2]);
			}
			currentProfile.variations[0].type = "Stereotype";
			currentProfile.variations[1].type = "Standard";
			currentProfile.variations[2].type = "Anti-Stereotype";
			
			//get the prefabfolder path - it's the 2nd column on the 7th line
			columns = lines[6].Split (',');
			currentProfile.prefabPath = columns[1];

			//now get the item wishlist of our variations - lines 4 - 6; columns 3 - 5
			//first item count range
			columns = lines[3].Split (',');
			for (int v = 0; v < 3; v++) {
				string[] countRangeText = columns[v+2].Split('-');
				currentProfile.variations[v].countRange = new Vector2(System.Convert.ToInt16(countRangeText[0].Trim()), System.Convert.ToInt16(countRangeText[1].Trim()));

			}
			//tags
			columns = lines[4].Split (',');
			for (int v = 0; v < 3; v++)
				currentProfile.variations[v].tags = columns[v+2];
			//specific items
			columns = lines[5].Split (',');
			for (int v = 0; v < 3; v++)
				currentProfile.variations[v].specificItems = columns[v+2];

			//finally add the CustomerProfile to our dictionary containing all customers
			profiles.Add (name, currentProfile);

//			print ("done reading in values");
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

			index = Random.Range (0, profileNames.Count);
			safetyCounter++;
		}

		return profile;
	}

	
	
	public string[] itemWishList(CustomerVariation variation)
	{
		string[] splitTags = variation.tags.Split (';');
		
		ProductRange products = gameObject.GetComponent<ProductRange> ();
			
		HashSet<string> itemNames = null;
		foreach(string t in splitTags)
		{
			string trimmedT = t.Trim();

			Dictionary<string,string> items = products.itemsWithTag(trimmedT);
			if(items == null)
			{
				Debug.LogWarning("couldn't find items for tag " + trimmedT);
				continue;
			}
			
			if(itemNames == null)
			{
				itemNames = new HashSet<string>(products.itemsWithTag(trimmedT).Keys);
			}
			else
			{
				itemNames.UnionWith(products.itemsWithTag(trimmedT).Keys);
			}
		}

		string[] temp = new string[itemNames.Count];
		itemNames.CopyTo (temp);

		return temp;
	}
}
