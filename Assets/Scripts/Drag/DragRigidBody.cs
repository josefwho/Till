using UnityEngine;
using System.Collections;

public class DragRigidBody : MonoBehaviour 
{
//	public float spring = 50.0f;
//	public float  damper = 5.0f;
	public float  drag = 10.0f;
	public float  angularDrag = 5.0f;
	public float  distance = 0.2f;
	public bool  attachToCenterOfMass = false;
	
	private SpringJoint springJoint;

	private float oldDrag;
	private float oldAngularDrag;

	private TillStateMachine machine;

	private ScannerTrigger trigger;

	private Vector3 dragOffset;


	// Use this for initialization
	void Awake () 
	{
		machine = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();
		trigger = GameObject.Find ("Scanner/Scanner Trigger").GetComponent<ScannerTrigger> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Make sure the user pressed the mouse down
		if (!Input.GetMouseButton(0) || machine.draggedItemCount > 0 || machine.isSpinning || springJoint) //|| (machine.currentState != States.Drag && machine.currentState != States.Idle && machine.currentState != States.Scan)  )
			return;


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

		if(Application.loadedLevel == 0)
		   {
			startDragging (hit.collider.gameObject, 14, false);
		}else
		startDragging (hit.collider.gameObject, 20, false);
	}

	public void startDragging(GameObject target, float dis, bool withOffset)
	{
		if (withOffset) {
						Ray ray = FindCamera ().ScreenPointToRay (Input.mousePosition);
			dragOffset = ray.GetPoint (distance) - target.transform.position;
				} else
						dragOffset = Vector3.zero;

		if (!springJoint)
		{
			ItemStatus s = transform.parent.gameObject.GetComponent<ItemStatus>();
			if(s != null && s.inTrigger == trigger.gameObject && s.scanned == 0)
			{
				trigger.startPinning(transform.parent.gameObject);
				return;
			}
			
			string name = "Joint Carrier " + gameObject.transform.parent.gameObject.name;
			GameObject go = new GameObject(name);
			Rigidbody body  = go.AddComponent ("Rigidbody") as Rigidbody;
			springJoint = go.AddComponent ("SpringJoint") as SpringJoint;
			body.isKinematic = true;
			
//			print ("DragRigidBody new springJoint: oldDrag is" + oldDrag + " new drag is:" + drag);
			
			oldDrag = target.rigidbody.drag;
			oldAngularDrag = target.rigidbody.angularDrag;
			
			machine.draggedItemCount++;
		}
		
		springJoint.transform.position = target.transform.position + dragOffset;
		if (attachToCenterOfMass)
		{
			Vector3 anchor = transform.TransformDirection(target.rigidbody.centerOfMass) + target.rigidbody.transform.position;
			anchor = springJoint.transform.InverseTransformPoint(anchor);
			springJoint.anchor = anchor;
		}
		else
		{
			springJoint.anchor = Vector3.zero;
		}
		
		springJoint.spring = 50;
		springJoint.damper = 5;
		springJoint.maxDistance = distance;
		springJoint.connectedBody = target.rigidbody;
		
		springJoint.connectedBody.drag = drag;
		springJoint.connectedBody.angularDrag = angularDrag;
		
		StartCoroutine ("DragObject", dis);
	}

	IEnumerator DragObject (float distance)
	{
		Camera mainCamera = FindCamera();

		Vector3 position;
		while (Input.GetMouseButton (0) && springJoint)
		{
			position = Input.mousePosition;
			
			if (Input.multiTouchEnabled)
			{
				if (Input.touchCount > 0)
					position = Input.GetTouch (0).position;
			}

			Ray ray = mainCamera.ScreenPointToRay (position);
//			Vector3 oldPosition = springJoint.transform.position;
			springJoint.transform.position = ray.GetPoint(distance) + dragOffset;
//			print("dragging " +springJoint.connectedBody.name + " to new pos: " + springJoint.transform.position);

			//adjust spring joint to magnitude of mouse movement
			float m = (springJoint.connectedBody.transform.position - springJoint.transform.position).magnitude;
			springJoint.spring = Mathf.Lerp(30, 100, m/4);
//			Debug.Log("magnitude is: " + m +  " springiness is: " + springJoint.spring);

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
//			print ("DragRigidBody detaching and resetting drag to" + oldDrag);

			springJoint.connectedBody.drag = oldDrag;
			springJoint.connectedBody.angularDrag = oldAngularDrag;

			Destroy(springJoint.gameObject);
//			springJoint.gameObject.SetActive (false);
			springJoint = null;

			ItemStatus s = transform.parent.gameObject.GetComponent<ItemStatus>();
			if(s != null)
    			s.autoDragged = false;
			
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
