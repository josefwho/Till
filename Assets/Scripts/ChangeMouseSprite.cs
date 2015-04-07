using UnityEngine;
using System.Collections;

public class ChangeMouseSprite : MonoBehaviour {


	public Texture2D cursorImage;
	public Texture2D grabbed;
	
	void Start()
	{
//		Screen.showCursor = false;
		//TODO scale offset to resolution 
		Vector2 hotSpot = new Vector2(cursorImage.width/2, cursorImage.height/2);
		Cursor.SetCursor (cursorImage, hotSpot, CursorMode.ForceSoftware);

	}
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0)) {
			Vector2 hotSpot = new Vector2(grabbed.width/2, grabbed.height/2);
			Cursor.SetCursor (grabbed, hotSpot, CursorMode.Auto);
			Debug.Log("show grabbed");
				}

		if (Input.GetMouseButtonUp(0)) {
			Vector2 hotSpot = new Vector2(cursorImage.width/2, cursorImage.height/2);
			Cursor.SetCursor (cursorImage, hotSpot, CursorMode.Auto);
			Debug.Log("show hand");
				}
//		Vector3 mousePos = Input.mousePosition;
//		Rect pos = new Rect(mousePos.x, Screen.height - mousePos.y, cursorImage
//		                    GUI.Label (pos, cursorImage);
	}
	


}
