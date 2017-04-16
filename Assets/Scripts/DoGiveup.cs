using UnityEngine;
using System.Collections;

public class DoGiveup : MonoBehaviour {

	public GameObject backBoard;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (name == "YesButton") {
			audioSources [4].Play ();

			foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Player")){
				Destroy (obs.gameObject);
			}
			backBoard.GetComponent<CloseOneBoard> ().OnClick ();
		} else {
			backBoard.GetComponent<CloseOneBoard> ().OnClick ();
			audioSources [1].Play ();

		}


	}
}
