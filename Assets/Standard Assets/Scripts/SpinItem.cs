using UnityEngine;
using System.Collections;

public class SpinItem : MonoBehaviour {

	public Vector3 mouseStartPosition;
	public float speed = 2.0f;
	public float maxAngularVelocity = 3;

	private float oldMaxAngularVelocity;


	// Use this for initialization
	void Start () {
		mouseStartPosition.x = Mathf.Infinity;
	}

	void OnFirstGlobalMouseDown()
	{
		mouseStartPosition = Input.mousePosition;

		oldMaxAngularVelocity = transform.parent.rigidbody.maxAngularVelocity;
		transform.parent.rigidbody.maxAngularVelocity = maxAngularVelocity;

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
	}
}
