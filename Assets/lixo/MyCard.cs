using UnityEngine;
using System.Collections;

public class MyCard : MonoBehaviour {

	public int i = 0;

	// Use this for initialization
	void Start () {
	
	}

	void Update () {
		// mouse click event
		if (Input.GetMouseButtonDown(0)) {
			Debug.Log("Pressed left click, casting ray ...");
			CastRay();
		}
	}
	
	void CastRay() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		// if raycast hits in something
		if( Physics.Raycast( ray, out hit, 100 ) ){
			// if raycast hits in my unflipped card
			//if( hit.collider.gameObject.tag == "card" ){
			if( hit.collider.gameObject == gameObject ){
				i++;
			}
		}
	}
}
