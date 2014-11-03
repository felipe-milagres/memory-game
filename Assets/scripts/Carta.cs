using UnityEngine;
using System.Collections;

public class Carta : MonoBehaviour {

	public enum Estado { NAO_VIRADA , VIRADA , GIRANDO , COMBINADA };

	public Estado estado;
	public string nome;

	// Use this for initialization
	private void Start () {
		estado = Estado.NAO_VIRADA;
		nome = gameObject.transform.name;

		//Debug.Log(">> " + nome + ":" + estado);
	}

	public void GirarCarta(){
		if( estado == Estado.NAO_VIRADA ){
			//estado = Estado.VIRADA;
			estado = Estado.GIRANDO;
			StartCoroutine( RotacionarCarta(1 , Estado.VIRADA ) ); // tempo animacao , novo estado apos a animacao
			Debug.Log(estado);
		}
	}

	public void GirarCartaDeVolta(){
		if( estado == Estado.VIRADA ){
			//estado = Estado.NAO_VIRADA;
			estado = Estado.GIRANDO;
			StartCoroutine( RotacionarCarta(1 , Estado.NAO_VIRADA ) ); // tempo animacao , novo estado apos a animacao
			Debug.Log(estado);
		}
	}

	public void CartaCombinaComOutra(){
		if( estado == Estado.VIRADA ){
			estado = Estado.COMBINADA;
			Debug.Log(estado);
		}
	}

	private IEnumerator RotacionarCarta( float duracao , Estado novoEstado ) {

		if( transform.rotation.eulerAngles.y == 0 || transform.rotation.eulerAngles.y == 180 ) {

			Vector3 angulo = Vector3.down * 180;
			Quaternion anguloInicial = transform.rotation;
			Quaternion anguloFinal = Quaternion.Euler(transform.eulerAngles + angulo);
			
			for( float t = 0f; t < 1; t += Time.deltaTime/duracao ) {
				transform.rotation = Quaternion.Lerp(anguloInicial, anguloFinal, t);
				yield return null;			
			}
			// avoid 181 or 180.99 degrees
			transform.rotation = anguloFinal;
			estado = novoEstado;
		}
	}

}
