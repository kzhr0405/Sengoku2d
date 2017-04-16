using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class LeaveSpdUp : MonoBehaviour {

    public bool nextFlg = false;

    public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();

        if (name == "YesButton") {
            audioSources[0].Play();

            if(nextFlg) {
                //next town
                SwitchTown SwitchTown = GameObject.Find("SwithTown").GetComponent<SwitchTown>();
                PlayerPrefs.SetInt("activeKuniId", SwitchTown.nextKuniId);
                PlayerPrefs.SetString("activeKuniName", SwitchTown.nextKuniName);
                PlayerPrefs.Flush();
                Application.LoadLevel("naisei");
            }else {
                //close
                PlayerPrefs.SetBool("fromNaiseiFlg", true);
                PlayerPrefs.Flush();
                audioSources[1].Play();
                Application.LoadLevel("mainStage");
            }
        }else {
            audioSources[1].Play();
            GameObject.Find("TouchBack").GetComponent<CloseBoard>().onClick();
        }
    }
}
