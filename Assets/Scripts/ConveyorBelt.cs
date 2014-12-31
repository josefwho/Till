using UnityEngine;
using System.Collections;

public class ConveyorBelt : MonoBehaviour {

	public float speed; //units/sec
	public float length;
	public GameObject partPrefab;

	// Use this for initialization
	void Start () {
//		GameObject item = Instantiate(prefab, pos, Quaternion.identity ) as GameObject;

		float partLength = partPrefab.transform.localScale.x;
		int partsNeeded = Mathf.CeilToInt(length / partLength);

		Vector3 pos = transform.position;
		Vector3 offset = new Vector3 (partLength, 0, 0);

		for (int i = 0; i < partsNeeded; i++) {
			GameObject part = Instantiate(partPrefab, pos, Quaternion.identity ) as GameObject;
			pos += offset;
				}
	}
	
	// Update is called once per frame
	void Update () {
//		if (Input.GetKey (KeyCode.Space))
//			transform.Translate(speed * Time.deltaTime, 0, 0);
	}
}
