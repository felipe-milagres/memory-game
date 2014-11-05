using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {

	// card states
	public enum state { NOT_FLIPPED , FLIPPED , MATCHED };
	public state myCardState;
	public string myCardName;

	private void Start () {
		// inicial state
		myCardState = state.NOT_FLIPPED;
		myCardName = gameObject.transform.name;

		//Debug.Log (myCardName);
	}

	public void FlipCard() {

		//if( myCardState == state.NOT_FLIPPED ){
			myCardState = state.FLIPPED;
			StartCoroutine( RotateMe(Vector3.up * 180, 0.8f) );
		//}

	}

	public void FlipBackCard() {

		Debug.Log (myCardName + ":" + myCardState);

		//if( myCardState == state.FLIPPED ){
			myCardState = state.NOT_FLIPPED;
			StartCoroutine( RotateMe(Vector3.down * 180, 1) );
		//}

		Debug.Log (myCardName + ":" + myCardState);

	}

	public void CardMatch() {
		Debug.Log( "function CardMatch" );
		myCardState = state.MATCHED;
	}

	private IEnumerator RotateMe( Vector3 byAngles, float inTime ) {

		Quaternion fromAngle = transform.rotation;
		Quaternion toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);

		for( float t = 0f; t < 1; t += Time.deltaTime/inTime ) {

			transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
			yield return null;

		}

		// avoid 181 or 180.99 degrees
		transform.rotation = toAngle;

	}

}