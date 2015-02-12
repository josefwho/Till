using UnityEngine;
using System.Collections;

public class ChangeMouseSprite : MonoBehaviour {


	public Texture2D cursorImage;
	public Texture2D grabbed;
	
	void Start()
	{
//		Screen.showCursor = false;
		Cursor.SetCursor (cursorImage, new Vector2(16f, 16f), CursorMode.ForceSoftware);

	}
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0)) {
			Cursor.SetCursor (grabbed, new Vector2(0.0f, 0.0f), CursorMode.ForceSoftware);
			Debug.Log("show grabbed");
				}

		if (Input.GetMouseButtonUp(0)) {
			Cursor.SetCursor (cursorImage, new Vector2(0.0f, 0.0f), CursorMode.Auto);
			Debug.Log("show hand");
				}
//		Vector3 mousePos = Input.mousePosition;
//		Rect pos = new Rect(mousePos.x, Screen.height - mousePos.y, cursorImage
//		                    GUI.Label (pos, cursorImage);
	}
	


}
