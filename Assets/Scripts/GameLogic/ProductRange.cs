using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemInfo
{
	public string name;
	public string readableName;
	public string tags;

	public ItemInfo(string newName, string newTags, string newReadableName)
	{
		name = newName;
		readableName = newReadableName;
		tags = newTags;
	}
}


public class ProductRange : MonoBehaviour {

	public TextAsset spreadsheet;

	private Dictionary<string, ItemInfo> allItems = new Dictionary<string, ItemInfo>();
	private Dictionary<string, Dictionary<string, string>> tagsToItems = new Dictionary<string, Dictionary<string, string>>();

//	private Dictionary<string, string> basic = new Dictionary<string, string>();
//	private Dictionary<string, string> premium = new Dictionary<string, string>();
//	private Dictionary<string, string> organic = new Dictionary<string, string>();
//	private Dictionary<string, string> luxury = new Dictionary<string, string>();
//	private Dictionary<string, string> cheap = new Dictionary<string, string>();

	// Use this for initialization
	void Awake () 
	{
		string[] lines = spreadsheet.text.Split ('\n');

//		print ("reading all lines of");

		foreach (string l in lines) 
		{
			string[] columns = l.Split (',');
			if(columns[0] == "Namen" || columns[0].Length < 1)
				continue;
			
			allItems.Add (columns [0], new ItemInfo(columns[0], columns[2], columns[3]));

			string[] tags = columns[2].Split (';');

			foreach(string t in tags)
			{
				string trimmedT = t.Trim();
				Dictionary<string, string> itemsWithCurrentTag;
				//make a new dictionary if we don't have one already
				if(!tagsToItems.TryGetValue(trimmedT, out itemsWithCurrentTag))
				{
					itemsWithCurrentTag = new Dictionary<string, string>();
//					print("adding new dictionary for tag " + trimmedT);
					tagsToItems.Add(trimmedT, itemsWithCurrentTag);
				}

				itemsWithCurrentTag.Add(columns[0], columns[2]);

			}
		}
//		print ("done reading in values");
	}

	public Dictionary<string,string> itemsWithTag(string tag)
	{
		
		Dictionary<string, string> temp;
		if(tagsToItems.TryGetValue(tag, out temp))
		{
			return temp;
		}

		return null;
	}

	public bool hasItem(string itemName)
	{
		return allItems.ContainsKey (itemName);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("p")) 
		{
			Dictionary<string, string> temp;
			if(tagsToItems.TryGetValue("premium", out temp))
			{
				print("all premium items:");
				foreach(var pair in temp)
					print (pair.Key);
			}
		}
		if (Input.GetKeyDown ("c")) 
		{
			Dictionary<string, string> temp;
			if(tagsToItems.TryGetValue("cheap", out temp))
			{
				print("all cheap items:");
				foreach(var pair in temp)
					print (pair.Key);
			}
		}
		if (Input.GetKeyDown ("a")) 
		{
			print("all items:");
			foreach(var pair in allItems)
				print (pair.Key + "has tags: " + pair.Value);
		}
	}

	public string readeableName(string itemName)
	{
		ItemInfo item;
		if(allItems.TryGetValue(itemName, out item))
		   return item.readableName;
		else
		   return null;
	}
}
