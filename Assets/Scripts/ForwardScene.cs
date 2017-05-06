using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class ForwardScene : MonoBehaviour {

    public string fromSceneName = "";
    public string toSceneName = "";

    private void Start() {
        fromSceneName = Application.loadedLevelName;
        if(fromSceneName == "pvp") {
            toSceneName = "purchase";            
        }
    }



    public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[0].Play();

        if(fromSceneName == "pvp") {
            Destroy(GameObject.Find("PvPDataStore"));
        }
        PlayerPrefs.SetString("fromSceneName", fromSceneName);
        PlayerPrefs.Flush();

        Application.LoadLevel(toSceneName);
    }
}
