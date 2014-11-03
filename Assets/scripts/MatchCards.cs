using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchCards : MonoBehaviour {
	
	//List<GameObject> cardsClicked = new List<GameObject>();
	Queue cardsClickedQueue = new Queue();

	private void Update() {
		// mouse click event
		if (Input.GetMouseButtonDown(0)) {
			CastRay();
		}
	}
	
	private void CastRay() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		// if raycast hits in something
		if( Physics.Raycast( ray, out hit, 100 ) ){

			if( cardsClickedQueue.Count < 2 ){
				// if raycast hits in my unflipped card
				if( hit.collider.gameObject.tag == "card" && hit.transform.GetComponent<Card>().myCardState == Card.state.NOT_FLIPPED ){

					// flipping my card
					hit.transform.SendMessage("FlipCard");

					// caching my card in a queue
					cardsClickedQueue.Enqueue( hit.transform.gameObject );
				}
			}

			// two cards in my queue
			if( cardsClickedQueue.Count == 2 ){
				
				GameObject temp1 = (GameObject) cardsClickedQueue.Dequeue();
				GameObject temp2 = (GameObject) cardsClickedQueue.Dequeue();

				if( Match ( temp1 , temp2 ) ){
					// changing the cards status FLIPPED to MATCHED
					StartCoroutine( FixMatchCards( temp1 , temp2 ) );
				}
				else {
					// changing the cards status FLIPPED to NOT_FLIPPED, except MATCHED
					StartCoroutine ( FlipBackCards( temp1 , temp2 ) );
				}

				// cleaning cards in cache
				temp1 = temp2 = null;

			}
		}
	}

	private IEnumerator FixMatchCards( GameObject gm1 , GameObject gm2 ) {
		Debug.Log( "Match");
		yield return new WaitForSeconds(1);
		gm1.transform.SendMessage("CardMatch");
		gm2.transform.SendMessage("CardMatch");
	}

	private IEnumerator FlipBackCards( GameObject gm1 , GameObject gm2 ) {
		Debug.Log( "NOT Match");
		yield return new WaitForSeconds(1);
		gm1.transform.SendMessage("FlipBackCard");
		gm2.transform.SendMessage("FlipBackCard");
	}

	private bool Match( GameObject gm1 , GameObject gm2 ) {
		// if my cards have the same Letter 
		// "card_A1" => slipt('_') => "A1" => Remove(1) => "A" => ToCharArray() => 'A'
		if( gm1.GetComponent<Card>().myCardName.Split('_')[1].Remove(1).ToCharArray()[0] == 
		    gm2.GetComponent<Card>().myCardName.Split('_')[1].Remove(1).ToCharArray()[0] ) 
			return true;
		else 
			return false; 
	}
}
