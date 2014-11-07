using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class MainCode : MonoBehaviour {

	private Queue _cardsClickedQueue = new Queue();
	private int _cardsMatched;
	private float _timer;
	private int _score;
	private bool _gameOver;
	private string _niceTime;

	public AudioClip cardsMatchSound;
	public AudioClip cardsDontMatchSound;
	public GUISkin customSkin;

	private void Awake(){
		ShufflingCards();
	}

	private void Start () {
		_cardsMatched = 0;
		_timer = 0;
		_score = 0;
		_gameOver = false;
		_niceTime = "00:00";
	}

	private Queue UniqueRandomNumbersQueue(){

		// queue of my unique random numbers
		Queue numbers = new Queue();

		// where the random number will be cache
		int randomNumber = Random.Range(0,19);

		// while I don't have 20 unique random numbers between 0 and 19
		while( numbers.Count < 20 ){    

			// if is a new number in the queue, I add in my queue
			if( !numbers.Contains(randomNumber) ) numbers.Enqueue(randomNumber);

			// generate another random number between 0 and 19
			randomNumber = Random.Range(0,20);
		}

		// debug
		//foreach (int value in numbers){ Debug.Log(value); }

		// return my queue with unique numbers random between 0 and 19
		return numbers;

	}


	private void ShufflingCards(){

		Vector2[] cardsPosition = {
			new Vector2 { x = -7, y =  0 },  // 1
			new Vector2 { x =  3, y = -7 },  // 2
			new Vector2 { x =  3, y = 14 },  // 3
			new Vector2 { x =  3, y =  0 },  // 4
			new Vector2 { x =  3, y =  7 },  // 5
			new Vector2 { x =  8, y =  0 },  // 6
			new Vector2 { x =  8, y = -7 },  // 7
			new Vector2 { x = -7, y = 14 },  // 8
			new Vector2 { x = -2, y = -7 },  // 9
			new Vector2 { x = -2, y =  7 },  // 10
			new Vector2 { x = -7, y =  7 },  // 11
			new Vector2 { x = -2, y = 14 },  // 12
			new Vector2 { x =  8, y = 14 },  // 13
			new Vector2 { x =  8, y =  7 },  // 14
			new Vector2 { x = -2, y =  0 },  // 15
			new Vector2 { x = -7, y = -7 },  // 16
			new Vector2 { x = -7, y = -14 },  // 17
			new Vector2 { x = -2, y = -14 },  // 18
			new Vector2 { x =  3, y = -14 },  // 19
			new Vector2 { x =  8, y = -14 }   // 20
		};

		Queue positions = UniqueRandomNumbersQueue();
		GameObject[] allCards = GameObject.FindGameObjectsWithTag("card");

		foreach (GameObject c in allCards) {
			c.transform.position = cardsPosition[(int) positions.Dequeue()];
		}

		/**
		 * DEBUG
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
		if (Input.GetMouseButtonDown(0)) {
			CastRay();
		}

		if( !_gameOver )
			_timer += Time.deltaTime;

	}

	void OnGUI() {

		GUI.skin = customSkin;

		if (GUI.Button(new Rect(130, 150, 50, 30), "Save")){
			SaveData( );
		}

		int minutes = Mathf.FloorToInt(_timer / 60F);
		int seconds = Mathf.FloorToInt(_timer - minutes * 60);
		
		_niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);

		GUI.Label(new Rect(Screen.width/2 - 41,0,100,30), _niceTime);

		if( _gameOver ){
			GUI.Box (new Rect (0,0,Screen.width,Screen.height), "");
			GUI.ModalWindow (0, new Rect (Screen.width/2 - 115, Screen.height/2 - 75,230 , 150), DoMyModalWindow, "Game Over");
		}
	}

	void DoMyModalWindow(int windowID) {

		GUI.BeginGroup (new Rect (10,33, 100, 70));
		GUI.Box (new Rect (0,0,100,70), "Your Time:");
		GUI.Label(new Rect(10,40,250,100), _niceTime);
		GUI.EndGroup ();
		
		GUI.BeginGroup (new Rect (120, 33, 100, 70));
		GUI.Box (new Rect (0,0,100,70), "Best Time:");
		GUI.Label(new Rect(10,40,250,100), "00:00");
		GUI.EndGroup ();
		
		if (GUI.Button(new Rect(55, 110, 120, 30), "Play again"))
			Application.LoadLevel("cartas");
		
	}

	private void CastRay() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if( Physics.Raycast( ray, out hit, 100 ) ){

			// verifico se existe menos que duas cartas na minha fila 
			if( _cardsClickedQueue.Count < 2 ){

				// se cliquei em uma carta e seu estado e' NAO_VIRADA
				if( hit.transform.gameObject.tag == "card" && hit.transform.GetComponent<Carta>().estado == Carta.Estado.NAO_VIRADA ){

					// girando a carta q eu clickei
					hit.transform.SendMessage("GirarCarta");

					// adicionando em uma fila temporaria
					_cardsClickedQueue.Enqueue( hit.transform.gameObject );

				}

			}

			// quando tiver duas cartas na minha fila, eu verifico se elas combinam ou nao
			if( _cardsClickedQueue.Count == 2 ){

				// removo da minha fila as duas cartas
				GameObject temp1 = (GameObject) _cardsClickedQueue.Dequeue();
				GameObject temp2 = (GameObject) _cardsClickedQueue.Dequeue();

				StartCoroutine( AsCartasSaoIguais( temp1 , temp2 ) );

			}

		}
	}

	private IEnumerator AsCartasSaoIguais( GameObject carta1 , GameObject carta2 ){

		yield return new WaitForSeconds(1.2f);
		if( carta1.GetComponent<Carta>().nome.Split('_')[1].Remove(1).ToCharArray()[0] == carta2.GetComponent<Carta>().nome.Split('_')[1].Remove(1).ToCharArray()[0] ){
			// manter viradas 
			carta1.transform.SendMessage("CartaCombinaComOutra");
			carta2.transform.SendMessage("CartaCombinaComOutra");

			audio.PlayOneShot(cardsMatchSound, 1);

			_cardsMatched++;

			_score += 10;

			if( _cardsMatched == 10 ) {
				Debug.Log("FIM DE JOGO! ");
				GameOver();
			}
		}
		else{
			// voltar as cartas para NAO_VIRADA
			carta1.transform.SendMessage("GirarCartaDeVolta");
			carta2.transform.SendMessage("GirarCartaDeVolta");

			audio.PlayOneShot(cardsDontMatchSound, 1);

			_score -= 5;
			if ( _score <= 0 ) _score = 0;
		}

	}

	private void GameOver(){
		_gameOver = true;
	}

	private bool NewBestTime(){

		// se meu tempo de agora e' melhor/menor que o meu best-time gravado
		// eu retorno true
		// senao false
		// trasnformar em numeros 
		// depois tudo em segundos .. e comparar se meu tempo agora e' menor com o tempo que eu salvei

	}

	void SaveData(){
		
		string xmlPath = Application.dataPath + @"/xml/data.xml";
		XmlDocument xmlDoc = new XmlDocument();
		
		if( File.Exists( xmlPath ) ){
			
			// loadnig XML file
			xmlDoc.Load( xmlPath );

			// getting XML root
			XmlElement nodeRoot = xmlDoc.DocumentElement;	

			xmlDoc.DocumentElement.RemoveAll();
			
			XmlElement nodePlayer = xmlDoc.CreateElement("player"); // creating node "player"
			
			XmlElement nodeTime = xmlDoc.CreateElement("time"); // creating node "name"
			nodeTime.InnerText = _niceTime+""; // putting the name in the node

			string bestTimeXML = xmlDoc.ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;
			Debug.Log( bestTimeXML );

			if( NewBestTime( bestTimeXML ) ){
				bestTimeXML = _niceTime+"";
			}

			XmlElement nodeBestTime = xmlDoc.CreateElement("time"); // creating node "name"
			nodeBestTime.InnerText = bestTimeXML+""; // putting the name in the node

			// adding name node inside player node
			nodePlayer.AppendChild( nodeTime );
			
			// adding player node inside root node
			nodeRoot.AppendChild( nodePlayer );
			
			// salving xml file
			xmlDoc.Save( xmlPath );
			
			Debug.Log("saved!!");
		}
	}

}
