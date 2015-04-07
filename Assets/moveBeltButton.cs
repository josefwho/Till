using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class moveBeltButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public ConveyorBelt conveyor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnPointerDown(PointerEventData data)
	{
		if (!conveyor)
			return;

		conveyor.startMovingBelt ();
	}

	public void OnPointerUp(PointerEventData data)
	{
		if (!conveyor)
			return;

		conveyor.stopMovingBelt ();
	}
}
