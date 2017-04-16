using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPopMenu : MonoBehaviour {

    public bool busyoFlg = false;

	public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[0].Play();

        if (busyoFlg) {
            //Create Popup
            GameObject.Find("BusyoScrollMenu").GetComponent<PopScrollSlider>().SlideIn();
           
        }
        else {
            GameObject.Find("JinkeiScrollMenu").GetComponent<PopScrollSlider>().SlideIn();

        }

    }
}
