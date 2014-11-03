using UnityEngine;
using System.Collections;

public class AdmCartas : MonoBehaviour {

	Queue cartasClicadasFila = new Queue();

	private void Update() {
		// mouse click event
		if (Input.GetMouseButtonDown(0)) {
			CastRay();
		}
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

		yield return new WaitForSeconds(1);
		if( carta1.GetComponent<Carta>().nome.Split('_')[1].Remove(1).ToCharArray()[0] == carta2.GetComponent<Carta>().nome.Split('_')[1].Remove(1).ToCharArray()[0] ){
			// manter viradas 
			carta1.transform.SendMessage("CartaCombinaComOutra");
			carta2.transform.SendMessage("CartaCombinaComOutra");
		}
		else{
			// voltar as cartas para NAO_VIRADA
			carta1.transform.SendMessage("GirarCartaDeVolta");
			carta2.transform.SendMessage("GirarCartaDeVolta");
		}

	}

}
