using UnityEngine;
using System.Collections;

public class ConveyorBelt : MonoBehaviour {

	public float speed; //units/sec
	public float length;
	public GameObject partPrefab;

	private ArrayList parts;

	// Use this for initialization
	void Start () {

		parts = new ArrayList ();

		float partLength = partPrefab.transform.localScale.x;
		int partsNeeded = Mathf.CeilToInt(length / partLength);

		Vector3 pos = transform.position;
		Vector3 offset = new Vector3 (partLength, 0, 0);

		for (int i = 0; i < partsNeeded; i++) {
			GameObject part = Instantiate(partPrefab, pos, Quaternion.identity ) as GameObject;
			parts.Add(part);
			pos += offset;
				}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space))
		{
			for (int i = 0; i < parts.Count; i++) {
				((GameObject)parts[i]).transform.Translate(speed * Time.deltaTime, 0, 0);
			}
		}
	}
}
