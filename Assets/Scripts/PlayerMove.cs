using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	private GameObject _charactor;

	// Use this for initialization
	void Start () {
		//Get Charactor
		this._charactor = GameObject.Find ("player1");	
	}
	
	// Update is called once per frame
	void Update() {

		//Get direction of Key
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		//Give vector to charactor
		this._charactor.transform.Translate(new Vector3 (horizontal, vertical, 0.01f)); 
	}
}
