using UnityEngine;
using System.Collections;

public class MainXML : MonoBehaviour{

	// Use this for initialization
	void Start () {
		ParseXML pXML = new ParseXML( "data" );
		
		//Debug.Log( pXML.getElementXML( "best_time" ) );
		//Debug.Log( minutesToSeconds( "15:00" ) );

		pXML.ReadXML ();
		pXML.changeValueOf ("best_time", "25:00");
		pXML.ReadXML();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// int timeInSeconds = minutesToSeconds( "15:00" );
	private int minutesToSeconds( string time ){
		// split MM:SS to MM and SS
		int minutes = int.Parse( time.Split(':')[0] );
		int seconds = int.Parse( time.Split(':')[1] );
		return ( minutes * 60 ) + seconds;
	}
}
