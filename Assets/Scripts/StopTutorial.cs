using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class StopTutorial : MonoBehaviour {

	void Start () {
        bool tutorialDoneFlg = PlayerPrefs.GetBool("tutorialDoneFlg");
        if (!tutorialDoneFlg) Destroy(gameObject);
    }
	
	public void OnClick () {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[0].Play();

        Application.LoadLevel("mainStage");
    }
}
