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

	private float oldDrag;
	private float oldAngularDrag;

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
		if (!Input.GetMouseButton(0) || springJoint) //|| (machine.currentState != States.Drag && machine.currentState != States.Idle && machine.currentState != States.Scan)  )
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
			string name = "Joint Carrier " + gameObject.transform.parent.gameObject.name;
			GameObject go = new GameObject(name);
			Rigidbody body  = go.AddComponent ("Rigidbody") as Rigidbody;
			springJoint = go.AddComponent ("SpringJoint") as SpringJoint;
			body.isKinematic = true;

			print ("DragRigidBody new springJoint: oldDrag is" + oldDrag + " new drag is:" + drag);

			oldDrag = hit.rigidbody.drag;
			oldAngularDrag = hit.rigidbody.angularDrag;

			machine.draggedItemCount++;
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
		
		springJoint.connectedBody.drag = drag;
		springJoint.connectedBody.angularDrag = angularDrag;

		StartCoroutine ("DragObject", hit.distance);
	}
	
	IEnumerator DragObject (float distance)
	{
		Camera mainCamera = FindCamera();
		while (Input.GetMouseButton (0) && springJoint)
		{
			Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
			springJoint.transform.position = ray.GetPoint(distance);
			print("dragging " +springJoint.connectedBody.name + " to new pos: " + springJoint.transform.position);
			yield return null;
		}
		detach ();
	
	}

	void OnDisable()
	{
		detach ();
	}

	void detach()
	{
		if (springJoint) 
		{	
			print ("DragRigidBody detaching and resetting drag to" + oldDrag);

			springJoint.connectedBody.drag = oldDrag;
			springJoint.connectedBody.angularDrag = oldAngularDrag;

			Destroy(springJoint.gameObject);
//			springJoint.gameObject.SetActive (false);
			springJoint = null;

			
			machine.draggedItemCount--;
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
