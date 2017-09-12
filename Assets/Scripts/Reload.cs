using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : MonoBehaviour {

    public void OnClick() {

        Destroy(GameObject.Find("PvPDataStore").gameObject);
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[0].Play();
        Application.LoadLevel("pvp");
    }
}
