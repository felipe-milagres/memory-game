using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class MainCode : MonoBehaviour {

	private Queue _cardsClickedQueue = new Queue();// queue of cards who was clicked by the player
	private int _cardsMatched; // number of cards who got matched (max 10)
	private float _timer; 
	private bool _gameOver;
	private string _niceTime; // timer with mask MM:SS
	private string _niceBestTime; // my best time with mask MM:SS

	public AudioClip cardsMatchSound; // audio to play when my cards match
	public AudioClip cardsDontMatchSound; // audio to play when my cards don't match
	public GUISkin customSkin;

	private void Awake() {
		// everytime when my game start, my cards positions will be random
		ShufflingCards();
	}

	private void Start() {
		//initiating variables
		_cardsMatched = 0;
		_timer = 0;
		_gameOver = false;
		_niceTime = "00:00";
		_niceBestTime = "00:00";
	}

	// return a queue with unique numbers between 0 and 19
	private Queue UniqueRandomNumbersQueue() {

		// queue of my unique random numbers
		Queue numbers = new Queue();

		// where the random number will be cache
		int randomNumber = Random.Range( 0,19 );

		// while I don't have 20 unique random numbers between 0 and 19
		while( numbers.Count < 20 ){    

			// if is a new number in the queue, I add in my queue
			if( !numbers.Contains( randomNumber ) ) numbers.Enqueue( randomNumber );

			// generate another random number between 0 and 19
			randomNumber = Random.Range( 0,20 );
		}

		//DEBUG - show Queue elements
		//foreach (int value in numbers){ Debug.Log(value); }

		// return my queue with unique numbers random between 0 and 19
		return numbers;

	}


	private void ShufflingCards() {

		// cards position in the scene
		Vector2[] cardsPosition = {
			new Vector2 { x = -7, y =  0  },  // 1
			new Vector2 { x =  3, y = -7  },  // 2
			new Vector2 { x =  3, y = 14  },  // 3
			new Vector2 { x =  3, y =  0  },  // 4
			new Vector2 { x =  3, y =  7  },  // 5
			new Vector2 { x =  8, y =  0  },  // 6
			new Vector2 { x =  8, y = -7  },  // 7
			new Vector2 { x = -7, y = 14  },  // 8
			new Vector2 { x = -2, y = -7  },  // 9
			new Vector2 { x = -2, y =  7  },  // 10
			new Vector2 { x = -7, y =  7  },  // 11
			new Vector2 { x = -2, y = 14  },  // 12
			new Vector2 { x =  8, y = 14  },  // 13
			new Vector2 { x =  8, y =  7  },  // 14
			new Vector2 { x = -2, y =  0  },  // 15
			new Vector2 { x = -7, y = -7  },  // 16
			new Vector2 { x = -7, y = -14 },  // 17
			new Vector2 { x = -2, y = -14 },  // 18
			new Vector2 { x =  3, y = -14 },  // 19
			new Vector2 { x =  8, y = -14 }   // 20
		};

		// creating random positions
		Queue positions = UniqueRandomNumbersQueue();

		// getting all my cards
		GameObject[] allCards = GameObject.FindGameObjectsWithTag( "card" );

		foreach( GameObject c in allCards ){
			// myCard.transform.position(x,y) = vectorWithPositions[ randomNumerOfMyQueue ];
			// ex: carta_A1.transform.position = cardsPosition[2];
			c.transform.position = cardsPosition[ (int) positions.Dequeue() ];
		}

		/*
		//DEBUG - show Queue elements
		GameObject[] delete = GameObject.FindGameObjectsWithTag("debug");
		int i = 0;
		foreach (GameObject c in allCards) {
			c.transform.position = delete[i].transform.position;
			i++;
		}
		*/
	}

	private void Update() {
		// mouse click event
		if( Input.GetMouseButtonDown(0) ) {
			CastRay(); // line 165
		}

		// if game is not over, timer still counting
		if( !_gameOver )
			_timer += Time.deltaTime;

	}

	private void OnGUI() {

		// custon skin { font-family , font-size }
		GUI.skin = customSkin;

		// timer
		int minutes = Mathf.FloorToInt(_timer / 60F);
		int seconds = Mathf.FloorToInt(_timer - minutes * 60);
		// timer mask
		_niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
		// print timer on the screen
		GUI.Label(new Rect(Screen.width/2 - 41,0,100,30), _niceTime);

		// if games over, "GAME OVER ModalWindow" show up
		if( _gameOver ){
			GUI.Box (new Rect (0,0,Screen.width,Screen.height), "");
			GUI.ModalWindow (0, new Rect (Screen.width/2 - 115, Screen.height/2 - 75,230 , 150), DoMyModalWindow, "Game Over");
		}
	}

	// GAME OVER ModalWindow
	private void DoMyModalWindow(int windowID) {

		// current time
		GUI.BeginGroup (new Rect (10,33, 100, 70));
		GUI.Box (new Rect (0,0,100,70), "Your Time:");
		GUI.Label(new Rect(10,40,250,100), _niceTime);
		GUI.EndGroup ();

		// best time
		GUI.BeginGroup (new Rect (120, 33, 100, 70));
		GUI.Box (new Rect (0,0,100,70), "Best Time:");
		GUI.Label(new Rect(10,40,250,100), _niceBestTime);
		GUI.EndGroup ();

		// restart the game 
		if (GUI.Button(new Rect(55, 110, 120, 30), "Play again"))
			Application.LoadLevel("cartas");
		
	}

	// used to see if I clicked in a card
	private void CastRay() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		// if my raycast hit in something
		if( Physics.Raycast( ray, out hit, 100 ) ){

			// if there are less than 2 cards in my queue
			if( _cardsClickedQueue.Count < 2 ){

				// if I clicked in a card and the card-status is NAO_VIRADA
				if( hit.transform.gameObject.tag == "card" && hit.transform.GetComponent<Carta>().estado == Carta.Estado.NAO_VIRADA ){

					// animation of turning/rotating/spinning this card 
					hit.transform.SendMessage("GirarCarta");

					// adding this card in my queue
					_cardsClickedQueue.Enqueue( hit.transform.gameObject );

				}

			}

			// when there are two cards in my queue, I check if the cards match or don't match
			if( _cardsClickedQueue.Count == 2 ){

				// remove both cards from the queue
				GameObject temp1 = (GameObject) _cardsClickedQueue.Dequeue();
				GameObject temp2 = (GameObject) _cardsClickedQueue.Dequeue();

				// call a function to see if the cards match
				StartCoroutine( CardsMatch( temp1 , temp2 ) );

			}

		}
	}

	private IEnumerator CardsMatch( GameObject carta1 , GameObject carta2 ){
		// wait a few seconds cause I need wait all the cards complete the animation
		yield return new WaitForSeconds(1.2f);

		// if carta1 name is the same of carta2 , the cards match
		// "carta_A1" => Split('_')[1] = "A1" => Remove(1) "A"  == is equal to == "carta_A2" => Split('_')[1] = "A2" => Remove(1) "A"
		if( carta1.GetComponent<Carta>().nome.Split('_')[1].Remove(1).ToCharArray()[0] == carta2.GetComponent<Carta>().nome.Split('_')[1].Remove(1).ToCharArray()[0] ){

			// keep card position and change cards status to COMBINADA
			carta1.transform.SendMessage("CartaCombinaComOutra");
			carta2.transform.SendMessage("CartaCombinaComOutra");

			// play audio "cards match sound"
			audio.PlayOneShot(cardsMatchSound, 1);

			// increase the number of cards who got match 
			_cardsMatched++;

			// if this number is 10 , means all cards got match , so , GAME OVER
			if( _cardsMatched == 10 ) {
				Debug.Log("FIM DE JOGO! ");
				GameOver();  // line 239
			}
		}
		else{
			// flip back the cards and change cards status to NAO_VIRADA
			carta1.transform.SendMessage("GirarCartaDeVolta");
			carta2.transform.SendMessage("GirarCartaDeVolta");

			// play audio "cards don't match sound"
			audio.PlayOneShot(cardsDontMatchSound, 1);

		}

	}

	private void GameOver() {
		// save the time in a database (aka XML)
		SaveData();
		_gameOver = true;
	}

	
	private int minutesToSeconds( string time ){
		// split MM:SS to MM and SS
		int minutes = int.Parse( time.Split(':')[0] );
		int seconds = int.Parse( time.Split(':')[1] );
		return ( minutes * 60 ) + seconds;
	}

	private bool NewBestTime( string time , string bestTime ){
		bool retorno = false;
		if( minutesToSeconds( time ) < minutesToSeconds( bestTime ) ){
			retorno = true;
		}
		return retorno;
	}

	private void SaveData(){
		// get my database ( it's a XML file )
		ParseXML database = new ParseXML("data");

		// if my new time is better than the time in my database
		if( NewBestTime( _niceTime , database.getElementXML( "best_time" ) ) ){
			_niceBestTime = _niceTime;
			database.changeValueOf ("best_time", _niceTime);
		}
	
	}


}
