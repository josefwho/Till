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

	public string[] itemOnFloorReactions = {"You fool!"};
	public string[] waitTooLongReactions = {"You fool!"};
	public string[] notMyItemReactions = {"You fool!"};
	public string[] itemMultipleScannedReactions = {"You fool!"};
	public string[] freeItemReactions = {"You fool!"};

	public CustomerVariation getRandomVariation()
	{
		int index = Random.Range (0, variations.Length);
		int safetyCounter = 0;

		//pick the variation if its probability is high enough
		while (safetyCounter < 100) 
		{
			if (variations[index].probability >= Random.value)
				return variations[index];
			
			index = Random.Range (0, variations.Length);
			safetyCounter++;
		}
		
		return variations[0];
	}
}

public class CustomerManager : MonoBehaviour {


	private Dictionary<string, CustomerProfile> profiles;
	private ArrayList profileNames;	//needed to pick a random profile

	private ProductRange products;
	ProgressionManager progression;

	void Awake () {
		profileNames = new ArrayList ();
		profiles = new Dictionary<string, CustomerProfile> ();

		products = GetComponent<ProductRange> ();
		progression = GetComponent<ProgressionManager> ();

		TextAsset[] spreadsheets = Resources.LoadAll<TextAsset> ("Spreadsheets/CustomerProfiles");

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

			if(lines.Length > 9)
			{
				//REACTIONS
				//"Item On Floor!"
				int start = lines[7].IndexOf('"')+1;
				string raw = lines[7].Substring(start, lines[7].LastIndexOf('"')-start);
				currentProfile.itemOnFloorReactions = raw.Split(';');
				//"Waiting Too Long!"
				start = lines[8].IndexOf('"')+1;
				raw = lines[8].Substring(start, lines[8].LastIndexOf('"')-start);
				currentProfile.waitTooLongReactions = raw.Split(';');
				//"Item Multiple Scanned!"
				start = lines[9].IndexOf('"')+1;
				raw = lines[9].Substring(start, lines[9].LastIndexOf('"')-start);
				currentProfile.itemMultipleScannedReactions = raw.Split(';');
				//"Not My Item!"
				start = lines[10].IndexOf('"')+1;
				raw = lines[10].Substring(start, lines[10].LastIndexOf('"')-start);
				currentProfile.notMyItemReactions = raw.Split(';');
				//"Free Item!"
				start = lines[11].IndexOf('"')+1;
				raw = lines[11].Substring(start, lines[11].LastIndexOf('"')-start);
				currentProfile.freeItemReactions = raw.Split(';');
			}



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
			if (profiles.TryGetValue (profileNames [index] as string, out profile) && progression.isCustomerUnlocked(profile) && profile.probability >= Random.value)
				return profile;

			index = Random.Range (0, profileNames.Count);
			safetyCounter++;
		}

		return profile;
	}

	
	
	public string[] itemWishList(CustomerVariation variation)
	{
		//the tags we should union together - aka boolean "or" - are delimited by ;
		string[] tagsToUnite = variation.tags.Split (';');
			
		HashSet<string> itemNames = new HashSet<string>();
		foreach(string tU in tagsToUnite)
		{
			string tUTrimmed = tU.Trim();

			Dictionary<string,string> items;

			HashSet<string> uniteWith = null;

			//we might have to first intersect tags in this string -  aka boolean "and"
			if(tUTrimmed.IndexOf('+') > -1)
			{
				string[] tagsToIntersect = tUTrimmed.Split ('+');
				foreach(string tI in tagsToIntersect)
				{
					string tITrimmed = tI.Trim();

					items = products.itemsWithTag(tITrimmed);
					//make sure we have items with this tag
					if(items == null)
					{
						Debug.LogWarning("couldn't find items for tag " + tITrimmed);
						continue;
					}

					//the first iteration in the for loop
					if(uniteWith == null)
					{
						uniteWith = new HashSet<string>(items.Keys);
					}
					//all other iterations
					else
					{
						uniteWith.IntersectWith(items.Keys);
					}
				}
			}
			//we don't have to intersect tags, so just find all items with the tag in tUTrimmed
			else
			{
				items = products.itemsWithTag(tUTrimmed);
				//make sure we have items with this tag
				if(items == null)
				{
					Debug.LogWarning("couldn't find items for tag " + tUTrimmed);
					continue;
				}

				uniteWith = new HashSet<string>(items.Keys);
			}

			//finally unite!
			itemNames.UnionWith(uniteWith);
		}

		//finally we need to add the specific items
		if (variation.specificItems.Length > 0) 
		{
			string[] specificItems = variation.specificItems.Split (';');
			Debug.Log ("Specific items are: " + variation.specificItems);
			foreach (string sI in specificItems) 
			{
				string sITrimmed = sI.Trim();
				if(products.hasItem(sITrimmed))
				{
					HashSet<string> oneItemSet = new HashSet<string>();
					oneItemSet.Add(sITrimmed);
					itemNames.UnionWith(oneItemSet);
				}
				else
					Debug.LogWarning("couldn't find specific item " + sITrimmed);
			}
		}

		
		string[] temp = null;
		if (itemNames != null) 
		{
			temp = new string[itemNames.Count];
			itemNames.CopyTo (temp);
		}

		//debug
//		Debug.Log ("got these items for tags: " + variation.tags);
//		for (int i = 0; i < temp.Length; i++) 
//		{
//			Debug.Log(temp[i]);
//		}

		return temp;
	}
}
