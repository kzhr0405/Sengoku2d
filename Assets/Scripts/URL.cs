using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URL : MonoBehaviour {

    public string url = "http://samurai_wars.a-wiki.net/";

	public void OnClick () {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[0].Play();

        Application.OpenURL(url);
        
    }
}
