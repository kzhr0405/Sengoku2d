using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {

	public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[2].Play();
        Destroy(gameObject);
    }
}
