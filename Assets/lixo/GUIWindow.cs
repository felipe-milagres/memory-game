using UnityEngine;
using System.Collections;

public class GUIWindow : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//public Rect windowRect = new Rect(100, 100, 320, 250);
	public Rect windowRect = new Rect(Screen.width/2 - 300, 100, 600, 600);

	public Rect rect = new Rect(0, 0, 100, 100);
	void Example() {
		print(rect.center);
		rect.center = new Vector2(10, 10);
	}

	void OnGUI() {
		//windowRect = GUI.ModalWindow(0, windowRect, DoMyWindow, "My Window");

		GUI.ModalWindow (0, new Rect (Screen.width/2 - 115, Screen.height/2 - 75,230 , 150), DoMyWindow, "Game Over");


	}

	void DoMyWindow(int windowID) {

		GUI.BeginGroup (new Rect (10,25, 100, 70));
			GUI.Box (new Rect (0,0,100,70), "Time:   ");
				GUI.Label(new Rect(10,20,250,100), "00:00");
		GUI.EndGroup ();

		GUI.BeginGroup (new Rect (120, 25, 100, 70));
			GUI.Box (new Rect (0,0,100,70), "Best Time:");
				GUI.Label(new Rect(10,20,250,100), "00:00");
		GUI.EndGroup ();

		if (GUI.Button(new Rect(65, 105, 100, 30), "Play again"))
			print("Got a click");

	}
}
