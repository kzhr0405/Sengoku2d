using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class CloseGacyaSpecialConfirm : MonoBehaviour {

    public GameObject Gacya;
    public GameObject board;
    public GameObject back;

    public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        if (name == "Yes") {
            Destroy(Gacya.gameObject);
            PlayerPrefs.DeleteKey("specialBusyoHst");
            PlayerPrefs.Flush();

            audioSources[0].Play();
        }else {
            audioSources[1].Play();
        }
        GameObject.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "Default";
        Destroy(board);
        Destroy(back);

    }
}
