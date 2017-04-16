using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopItem : MonoBehaviour {

    public string text = "";

    public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[0].Play();

        GameObject canvas = null;
        if (Application.loadedLevelName == "busyo" || Application.loadedLevelName == "mainStage" || Application.loadedLevelName == "touyou" || Application.loadedLevelName == "zukan" || Application.loadedLevelName == "tutorialTouyou" || Application.loadedLevelName == "shisya") {
            canvas = GameObject.Find("Panel").gameObject;
        }
        
        Message msg = new Message();
        msg.makeIconExpMessage(text, canvas);

    }
}
