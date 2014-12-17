using UnityEngine;
using System.Collections;

public class SpinItem : MonoBehaviour {

	public Vector3 mouseStartPosition;
	public float speed = 2.0f;

	private Vector3 lastMousePosition;

	// Use this for initialization
	void Start () {
		mouseStartPosition.x = Mathf.Infinity;
	}

	void OnFirstGlobalMouseDown()
	{
		mouseStartPosition = Input.mousePosition;
		lastMousePosition = mouseStartPosition;
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

		Vector3 dragTotal = Input.mousePosition - mouseStartPosition;

		Vector3 dragDelta = Input.mousePosition - lastMousePosition;

		float xSign = (dragDelta.x<0) ? -1.0f : 1.0f;
		float ySign = (dragDelta.y<0) ? -1.0f : 1.0f;

		transform.parent.gameObject.rigidbody.AddTorque (0, 0, -xSign * Mathf.Pow(Mathf.Abs(dragDelta.x), 1.5f) * speed);
		transform.parent.gameObject.rigidbody.AddTorque (ySign * Mathf.Pow (Mathf.Abs(dragDelta.y), 1.5f) * speed, 0, 0);
		

		lastMousePosition = Input.mousePosition;
	}

	void OnFirstGlobalMouseUp()
	{
		mouseStartPosition.x = Mathf.Infinity;
	}
}
