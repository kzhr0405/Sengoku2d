using UnityEngine;
using System.Collections;

public class KillOtherBGM : MonoBehaviour {

	// Use this for initialization
	public void Start () {
		foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Sound")){
			if (obj.name == "BGMController") {
				Destroy (obj.gameObject);
			}
		}


	}
}
