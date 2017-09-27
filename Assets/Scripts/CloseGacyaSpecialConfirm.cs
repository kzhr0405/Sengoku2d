using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGacyaSpecialConfirm : MonoBehaviour {

    public GameObject Gacya;
    public GameObject board;
    public GameObject back;

    public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        if (name == "Yes") {
            Destroy(Gacya.gameObject);
            audioSources[0].Play();
        }else {
            audioSources[1].Play();
        }
        GameObject.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "Default";
        Destroy(board);
        Destroy(back);

    }
}
