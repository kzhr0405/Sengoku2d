using UnityEngine;
using System.Collections;

public class CloseOneBoard : MonoBehaviour {

	public GameObject deleteObj;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [1].Play ();

		Destroy (deleteObj);
		Destroy (gameObject);
	}

}
