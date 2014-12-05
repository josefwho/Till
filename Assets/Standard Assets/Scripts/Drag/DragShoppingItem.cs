using UnityEngine;
using System.Collections;

public class DragShoppingItem : MonoBehaviour {

	public float spring = 50.0f;
	public float  damper = 5.0f;
	public float  drag = 10.0f;
	public float  angularDrag = 5.0f;
	public float  distance = 0.2f;
	public bool  attachToCenterOfMass = false;

	private TillStateMachine machine;
	private Camera cam;

	private SpringJoint springJoint;
	private GameObject jointCarrier;
	private float camToOwnerDistance;

	private bool mouseWasDown;

	// Use this for initialization
	void Start () 
	{
		machine = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();
		
		cam = Camera.main;
		if (GetComponent<Camera>())
			cam = GetComponent<Camera>();
	}

	void OnMouseDown()
	{
		if (mouseWasDown || jointCarrier.activeSelf)
			return;

		mouseWasDown = true;

		attach ();
		
		machine.itemGrabbed = true;
	}
	
	void OnMouseUp()
	{
		mouseWasDown = false;

		detach ();
	}

	void attach()
	{
		// We need to actually hit an object
		RaycastHit hit;
		if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition),out hit, 100))
			return;
		// We need to hit a rigidbody that is not kinematic
		if (!hit.rigidbody || hit.rigidbody.isKinematic)
			return;

		//are we hitting the gameObject that is our owner
		if (hit.collider.gameObject != gameObject.transform.parent.gameObject)
			return;

		if (!jointCarrier) {
			string name = "Joint Carrier " + gameObject.transform.parent.gameObject.name;
			jointCarrier = new GameObject (name);
			Rigidbody body = jointCarrier.AddComponent ("Rigidbody") as Rigidbody;
			springJoint = jointCarrier.AddComponent ("SpringJoint") as SpringJoint;
			body.isKinematic = true;
		} else
			jointCarrier.SetActive (true);

		GameObject owner = hit.collider.gameObject;

		jointCarrier.transform.position = hit.point;

		if (attachToCenterOfMass)
		{
			Vector3 anchor = transform.TransformDirection(owner.rigidbody.centerOfMass) + owner.rigidbody.transform.position;
			anchor = springJoint.transform.InverseTransformPoint(anchor);
			springJoint.anchor = anchor;
		}
		else
		{
			springJoint.anchor = Vector3.zero;
		}

		springJoint.spring = spring;
		springJoint.damper = damper;
		springJoint.maxDistance = distance;

		//connect to our owner
		springJoint.connectedBody = owner.rigidbody;

		//save the distance for moving the obejct
		camToOwnerDistance = hit.distance;
	}

	void detach()
	{
		if (jointCarrier)
			jointCarrier.SetActive (false);
	}

	// Update is called once per frame
	void Update () 
	{
		if (jointCarrier && jointCarrier.activeSelf)
		{
			Ray ray = cam.ScreenPointToRay (Input.mousePosition);
			jointCarrier.transform.position = ray.GetPoint(camToOwnerDistance);
		}

	}
}
