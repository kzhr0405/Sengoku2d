using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class LangSettingConfirm : MonoBehaviour {

    public int langId = 0;
    public GameObject back;
    public GameObject confirm;

    public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();

        if (name == "YesButton") {
            audioSources[5].Play();
            PlayerPrefs.SetInt("langId", langId);
            PlayerPrefs.Flush();
            Application.LoadLevel("top");
        }else {
            audioSources[1].Play();
            //Back
            Destroy(back);
            Destroy(confirm);
        }
    }
}
