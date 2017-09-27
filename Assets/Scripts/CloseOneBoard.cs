﻿using UnityEngine;
using System.Collections;

public class CloseOneBoard : MonoBehaviour {

	public GameObject deleteObj;

	public void OnClick(){

        if(GameObject.Find("Canvas")) {
            GameObject.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "Default";
        }else if(GameObject.Find("Map")) {
            GameObject.Find("Map").GetComponent<Canvas>().sortingLayerName = "Default";
        }else if (GameObject.Find("Jinkei")) {
            GameObject.Find("Jinkei").GetComponent<Canvas>().sortingLayerName = "Default";
        }


        AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [1].Play ();


		Destroy (deleteObj);
		Destroy (gameObject);
	}

}
