using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMessageStamina : MonoBehaviour {

    public GameObject board;
    public GameObject panel;

    public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[1].Play();
        
        panel.GetComponent<Canvas>().sortingLayerName = "Default";
        board.SetActive(false);

    }
}
