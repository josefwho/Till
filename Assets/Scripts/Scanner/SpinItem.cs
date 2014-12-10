using UnityEngine;
using System.Collections;

public class SpinItem : MonoBehaviour {

	public Vector3 mouseStartPosition;
	public float speed = 2.0f;
	public float maxAngularVelocity = 3;
	public float drag = 10;
	public float angularDrag = 5;

	private float oldMaxAngularVelocity;

	private float oldDrag;
	private float oldAngularDrag;


	// Use this for initialization
	void Start () {
		mouseStartPosition.x = Mathf.Infinity;
	}

	void OnFirstGlobalMouseDown()
	{
		mouseStartPosition = Input.mousePosition;

		oldMaxAngularVelocity = transform.parent.rigidbody.maxAngularVelocity;
		transform.parent.rigidbody.maxAngularVelocity = maxAngularVelocity;

		oldDrag = transform.parent.rigidbody.drag;
		oldAngularDrag = transform.parent.rigidbody.angularDrag;

		transform.parent.rigidbody.drag = drag;
		transform.parent.rigidbody.angularDrag = angularDrag;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
			OnFirstGlobalMouseDown ();

		if (Input.GetMouseButtonUp (0) )
			OnFirstGlobalMouseUp ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (mouseStartPosition.x == Mathf.Infinity)
			return;

		Vector3 drag = Input.mousePosition - mouseStartPosition;
		transform.parent.gameObject.rigidbody.AddTorque (0, 0, -drag.x * speed);
		transform.parent.gameObject.rigidbody.AddTorque (drag.y*speed, 0, 0);

	}

	void OnFirstGlobalMouseUp()
	{
		mouseStartPosition.x = Mathf.Infinity;

		transform.parent.rigidbody.maxAngularVelocity = oldMaxAngularVelocity;
		
		transform.parent.rigidbody.drag = oldDrag;
		transform.parent.rigidbody.angularDrag = oldAngularDrag;
	}
}
