/*
    Sample DragRigidbody script. Initially created by Unity Technologies.
    Improved  extended by Juan Manuel Palacios: jmpalacios@gmail.com, wattodna@gmail.com, @jmpalaciosp
*/
using UnityEngine;
using System.Collections;

public delegate void EngageDelegate();
public delegate void DisengageDelegate();


[RequireComponent(typeof(FixedJoint))]
public class FixedJointDragger : MonoBehaviour
{
	
	/* These public variables have to be initialized to suitable values in the Inspector */
	public float breakForce = Mathf.Infinity;
	public float breakTorque = Mathf.Infinity;
	public bool attachToCenterOfMass = true;
	
	/* These public variables have to be initialized to suitable values in the Inspector */
	public LayerMask raycastLayer = -1;
	public float raycastDistance;
	
	protected Joint joint;
	protected Camera gameCamera;
	
	protected EngageDelegate _engageDelegate = delegate{};
	public EngageDelegate engageDelegate
	{
		get
		{
			return _engageDelegate;
		}
		set
		{
			_engageDelegate = value;
		}
	}
	
	protected DisengageDelegate _disengageDelegate = delegate{};
	public DisengageDelegate disengageDelegate
	{
		get
		{
			return _disengageDelegate;
		}
		set
		{
			_disengageDelegate = value;
		}
	}
	
	
	protected void Reset()
	{
		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;
		FindJoint();
	}
	
	
	protected void FindJoint()
	{
		joint = GetComponent<FixedJoint>();
		if (!joint)
		{
			joint = gameObject.AddComponent<FixedJoint>();
		}
		Init();
	}
	
	
	protected void Init()
	{
		joint.breakForce = breakForce;
		joint.breakTorque = breakTorque;
	}
	
	
	protected Camera FindCamera()
	{
		return Camera.main;
	}
	
	
	protected void Awake()
	{
		FindJoint();
		gameCamera = FindCamera();
	}
	
	
	protected void Update()
	{
		if (!Input.GetMouseButtonDown(0))
		{
			return;
		}
		
		RaycastHit hit;
		Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
		if
			(
				!Physics.Raycast(ray, out hit, raycastDistance, raycastLayer) ||
				!hit.rigidbody ||
				hit.rigidbody.isKinematic
				)
		{
			return;
		}
		
		Engage(hit);
		StartCoroutine("DragObject", hit.distance);
	}
	
	
	protected void Engage(RaycastHit successfulHit)
	{
		if (!joint)
		{
			FindJoint();
		}
		transform.position = successfulHit.point;
		joint.connectedBody = successfulHit.rigidbody;
		
		if (attachToCenterOfMass)
		{
			Vector3 anchor = transform.TransformDirection(successfulHit.rigidbody.centerOfMass) + successfulHit.transform.position;
			anchor = transform.InverseTransformPoint(anchor);
			joint.anchor = anchor;
		}
		else
		{
			joint.anchor = Vector3.zero;
		}
		
		engageDelegate();
	}
	
	
	protected IEnumerator DragObject(float distance)
	{
		Ray ray;
		while
			(
				joint
				)
		{
			ray = gameCamera.ScreenPointToRay(Input.mousePosition);
			rigidbody.MovePosition(ray.GetPoint(distance));
			yield return new WaitForFixedUpdate();
		}
		
		Disengage();
	}
	
	
	protected void Disengage()
	{
		if (joint)
		{
			joint.connectedBody = null;
		}
		disengageDelegate();
	}
	
	
	protected void OnJointBreak(float breakForce)
	{
		StopCoroutine("DragObject");
		Disengage();
	}
	
	
}
