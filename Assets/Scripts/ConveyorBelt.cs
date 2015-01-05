using UnityEngine;
using System.Collections;

public class ConveyorBelt : MonoBehaviour {

	public float speed; //units/sec
	public float length;
	public GameObject partPrefab;

	private ArrayList parts;
	private float newPartThreshold;
	private float distanceSinceLastNewPart;
	private float removeAtXPos;

	private TillStateMachine till;

	// Use this for initialization
	void Start () {

		parts = new ArrayList ();

		float partLength = partPrefab.transform.localScale.x * 0.8f;
		int partsNeeded = Mathf.CeilToInt(length / partLength);

		Vector3 pos = transform.position;
		Vector3 offset = new Vector3 (partLength, 0, 0);

		removeAtXPos = pos.x + partsNeeded * partLength;

		for (int i = 0; i < partsNeeded; i++) {
			GameObject part = Instantiate(partPrefab, pos, Quaternion.identity ) as GameObject;
			parts.Add(part);
			pos += offset;
				}
		
		distanceSinceLastNewPart = 0.0f;

		newPartThreshold = partLength ;

		till = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space))
		{
			if(distanceSinceLastNewPart > newPartThreshold)
			{
				GameObject part = Instantiate(partPrefab, transform.position, Quaternion.identity ) as GameObject;
				parts.Add(part);
				distanceSinceLastNewPart = 0.0f;
			}
			
			float offset = speed * Time.deltaTime;

			ArrayList toRemove = new ArrayList();
			foreach(GameObject p in parts)
			{
				p.transform.Translate(offset, 0, 0);

				if(p.transform.position.x > removeAtXPos)
					toRemove.Add(p);
			}

			foreach(GameObject o in toRemove)
			{
				parts.Remove(o);
				GameObject.Destroy(o);
			}
			toRemove.Clear();

			till.onBeltMoved(offset);

			distanceSinceLastNewPart += offset;
		}
	}
}
