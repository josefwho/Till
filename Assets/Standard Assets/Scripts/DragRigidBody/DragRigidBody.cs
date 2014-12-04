using UnityEngine;
using System.Collections;

public class DragRigidBody : MonoBehaviour 
{
	public float spring = 50.0f;
	public float  damper = 5.0f;
	public float  drag = 10.0f;
	public float  angularDrag = 5.0f;
	public float  distance = 0.2f;
	public bool  attachToCenterOfMass = false;
	
	private SpringJoint springJoint;

	private TillStateMachine machine;


	// Use this for initialization
	void Awake () 
	{
		machine = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Make sure the user pressed the mouse down
		if (!Input.GetMouseButtonDown (0))
			return;
		
		//	//TODO: make sure only one item can be gradde at the same time
		//	if(machine.itemDragged)
		//		return;


		var mainCamera = FindCamera();
		
		// We need to actually hit an object
		RaycastHit hit;
		if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition),out hit, 100))
			return;
		// We need to hit a rigidbody that is not kinematic
		if (!hit.rigidbody || hit.rigidbody.isKinematic)
			return;

		if (hit.collider.gameObject != gameObject.transform.parent.gameObject)
			return;

		if (!springJoint)
		{
			GameObject go = new GameObject("Rigidbody dragger");
			Rigidbody body  = go.AddComponent ("Rigidbody") as Rigidbody;
			springJoint = go.AddComponent ("SpringJoint") as SpringJoint;
			body.isKinematic = true;
		}
		
		springJoint.transform.position = hit.point;
		if (attachToCenterOfMass)
		{
			Vector3 anchor = transform.TransformDirection(hit.rigidbody.centerOfMass) + hit.rigidbody.transform.position;
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
		springJoint.connectedBody = hit.rigidbody;

		machine.itemGrabbed = true;

		StartCoroutine ("DragObject", hit.distance);
	}
	
	IEnumerator DragObject (float distance)
	{
		float oldDrag = springJoint.connectedBody.drag;
		float oldAngularDrag = springJoint.connectedBody.angularDrag;
		springJoint.connectedBody.drag = drag;
		springJoint.connectedBody.angularDrag = angularDrag;
		Camera mainCamera = FindCamera();
		while (Input.GetMouseButton (0))
		{
			Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
			springJoint.transform.position = ray.GetPoint(distance);
			yield return null;
		}

		
		springJoint.connectedBody.drag = oldDrag;
		springJoint.connectedBody.angularDrag = oldAngularDrag;
		detach ();
	
	}

	void OnDisable()
	{
		detach ();
	}

	void detach()
	{
		if (!springJoint) 
		{
			Destroy(springJoint.gameObject);
			springJoint = null;
			machine.itemGrabbed = false;
		}
	}
	
	Camera FindCamera ()
	{
		if (GetComponent<Camera>())
			return GetComponent<Camera>();
		else
			return Camera.main;
	}
}
