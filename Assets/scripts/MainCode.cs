using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainCode : MonoBehaviour {

	private Queue cartasClicadasFila = new Queue();
	private int cartasCombinadas;
	private float timer;
	private int score;
	private bool gameOver;
	public AudioClip cardsMatchSound;

	private void Awake(){
		ShufflingCards();
	}

	private void Start () {
		cartasCombinadas = 0;
		timer = 0;
		score = 0;
		gameOver = false;
	}

	Queue UniqueRandomNumbersQueue(){

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
			new Vector2 { x = -12, y = 14 }, new Vector2 { x = -6, y = 14 }, new Vector2 { x = 0, y = 14 }, new Vector2 { x = 6, y = 14 }, new Vector2 { x = 12, y = 14 },
			new Vector2 { x = -12, y =  7 }, new Vector2 { x = -6, y =  7 }, new Vector2 { x = 0, y =  7 }, new Vector2 { x = 6, y =  7 }, new Vector2 { x = 12, y =  7 }, 
			new Vector2 { x = -12, y =  0 }, new Vector2 { x = -6, y =  0 }, new Vector2 { x = 0, y =  0 }, new Vector2 { x = 6, y =  0 }, new Vector2 { x = 12, y =  0 }, 
			new Vector2 { x = -12, y = -7 }, new Vector2 { x = -6, y = -7 }, new Vector2 { x = 0, y = -7 }, new Vector2 { x = 6, y = -7 }, new Vector2 { x = 12, y = -7 }  
		};

		Queue positions = UniqueRandomNumbersQueue();

		GameObject[] allCards = GameObject.FindGameObjectsWithTag("card");

		foreach (GameObject c in allCards) {
			c.transform.position = cardsPosition[(int) positions.Dequeue()];
		}

	}

	private void Update() {
		// mouse click event
		if (Input.GetMouseButtonDown(0)) {
			CastRay();
		}

		if( !gameOver )
			timer += Time.deltaTime;

	}

	void OnGUI() {
		int minutes = Mathf.FloorToInt(timer / 60F);
		int seconds = Mathf.FloorToInt(timer - minutes * 60);
		
		string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
		
		GUI.Label(new Rect(10,5,250,100), niceTime);

		GUI.Label(new Rect(10,20,250,100), "score: " + score);

		if (GUI.Button(new Rect(10, 70, 80, 30), "Restart"))
			Application.LoadLevel("cartas");
	}
	
	private void CastRay() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if( Physics.Raycast( ray, out hit, 100 ) ){

			// verifico se existe menos que duas cartas na minha fila 
			if( cartasClicadasFila.Count < 2 ){

				// se cliquei em uma carta e seu estado e' NAO_VIRADA
				if( hit.transform.gameObject.tag == "card" && hit.transform.GetComponent<Carta>().estado == Carta.Estado.NAO_VIRADA ){

					// girando a carta q eu clickei
					hit.transform.SendMessage("GirarCarta");

					// adicionando em uma fila temporaria
					cartasClicadasFila.Enqueue( hit.transform.gameObject );

				}

			}

			// quando tiver duas cartas na minha fila, eu verifico se elas combinam ou nao
			if( cartasClicadasFila.Count == 2 ){

				// removo da minha fila as duas cartas
				GameObject temp1 = (GameObject) cartasClicadasFila.Dequeue();
				GameObject temp2 = (GameObject) cartasClicadasFila.Dequeue();

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

			cartasCombinadas++;

			score += 10;

			if( cartasCombinadas == 10 ) {
				Debug.Log("FIM DE JOGO! ");
				GameOver();
			}
		}
		else{
			// voltar as cartas para NAO_VIRADA
			carta1.transform.SendMessage("GirarCartaDeVolta");
			carta2.transform.SendMessage("GirarCartaDeVolta");

			score -= 5;
			if ( score <= 0 ) score = 0;
		}

	}

	private void GameOver(){
		gameOver = true;

	}

}
