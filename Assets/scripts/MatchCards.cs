using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchCards : MonoBehaviour {
	
	//List<GameObject> cardsClicked = new List<GameObject>();
	Queue cardsClickedQueue = new Queue();

	private void Start() { //Debug.Log( cardsClicked.Count );
	}

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

					// caching my card in a list
					//cardsClicked.Add( hit.transform.gameObject );
					cardsClickedQueue.Enqueue( hit.transform.gameObject );

				}


			}

			// two cards in my list
			if( cardsClickedQueue.Count == 2 ){
				
				GameObject temp1 = (GameObject) cardsClickedQueue.Dequeue();
				GameObject temp2 = (GameObject) cardsClickedQueue.Dequeue();

				if( Match ( temp1 , temp2 ) ){
					Debug.Log( "Match");
				}
				else {
					Debug.Log( "NOT Match");
					StartCoroutine ( Example( temp1 , temp2 ) );
				}
				//Debug.Log("--> " +  cardsClicked[0] + " " + cardsClicked[1]);
				/*
				if( Match() ){
					// changing the cards status FLIPPED to MATCHED
					//cardsClicked[0].transform.SendMessage("CardMatch");
					//cardsClicked[1].transform.SendMessage("CardMatch");
					Debug.Log( "Match");
				}
				else { 
					// virar todas as cartas que estao com o estado de FLIPPED para NOT_FLIPPED. nao virar os MATCHED

					//Debug.Log( "sad");
					//StartCoroutine ( Example( cardsClicked ) );
					//Debug.Log( "dsa");
				}
*/
				// cleaning cards in cache
				//cardsClicked.Clear();
				//Debug.Log( ">>>>>> cardsClicked.Clear" );
			}
		}
	}
	/*
	void RotteMe() {
		//Debug.Log( "A" );
		// I have to wait my cards flip complety first ----> colocar uma verificacao de se a carta esta virando , enquanto estiver virando o FlipBackCard nao e executado 
		//yield return new WaitForSeconds(1); 
		//Debug.Log( "B" );
		cardsClicked[0].transform.SendMessage("FlipBackCard");
		cardsClicked[1].transform.SendMessage("FlipBackCard");	
		//Debug.Log( "C" );
	}

	private bool Match() {
		// if my cards have the same Letter 
		// "card_A1" => slipt('_') => "A1" => Remove(1) => "A" => ToCharArray() => 'A'
		/*
		if( cardsClicked[0].GetComponent<Card>().myCardName.Split('_')[1].Remove(1).ToCharArray()[0] == 
		    cardsClicked[1].GetComponent<Card>().myCardName.Split('_')[1].Remove(1).ToCharArray()[0] ) 
			return true;
		else 
			return false; 
	}*/
			
	private bool Match( GameObject gm1 , GameObject gm2 ) {
		// if my cards have the same Letter 
		// "card_A1" => slipt('_') => "A1" => Remove(1) => "A" => ToCharArray() => 'A'

		if( gm1.GetComponent<Card>().myCardName.Split('_')[1].Remove(1).ToCharArray()[0] == 
		   gm2.GetComponent<Card>().myCardName.Split('_')[1].Remove(1).ToCharArray()[0] ) 
				return true;
			else 
				return false; 
	}

	IEnumerator Example( GameObject gm1 , GameObject gm2 ) {

		Debug.Log(Time.time);
		yield return new WaitForSeconds(5);
		gm1.transform.SendMessage("FlipBackCard");
		gm2.transform.SendMessage("FlipBackCard");
		Debug.Log(Time.time);

	}

	IEnumerator Example( List<GameObject> list ) {

		List<GameObject> cardsClicked1 = list;

		Debug.Log(Time.time);
		yield return new WaitForSeconds(5);
		cardsClicked1[0].transform.SendMessage("FlipBackCard");
		cardsClicked1[1].transform.SendMessage("FlipBackCard");
		Debug.Log(Time.time);

		cardsClicked1.Clear ();
	}

}
