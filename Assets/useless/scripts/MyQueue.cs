using UnityEngine;
using System.Collections;

public class MyQueue : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Queue myQ = new Queue();
		myQ.Enqueue("Hello");
		myQ.Enqueue("World");
		myQ.Enqueue("!");
		Debug.Log( "Count: " + myQ.Count );
		PrintValues( myQ );
		Debug.Log( "remove:" + myQ.Dequeue());
		PrintValues( myQ ); 
	}

	void PrintValues( IEnumerable myCollection )  {
		foreach ( string obj in myCollection )
			Debug.Log( "-> " + obj );
	}

	// Update is called once per frame
	void Update () {
	
	}
}
