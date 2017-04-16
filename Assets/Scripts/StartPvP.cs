using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class StartPvP : MonoBehaviour {

    public Text textScript;

	public void OnClick() {

        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        if (textScript.text == "") {
            audioSources[4].Play();
            Message msg = new Message();
            msg.makeMessage(msg.getMessage(140));

        }else {
            //Text Registeration
            audioSources[5].Play();

            //Init Data Registration
            DataPvP pvpScript = new DataPvP();
            string userId = PlayerPrefs.GetString("userId");
            string userName = textScript.text;
            pvpScript.InsertPvP(userId, userName);

            PlayerPrefs.SetString("PvPName", userName);
            PlayerPrefs.Flush();

            //Scene Change
            Application.LoadLevel("pvp");
        }
    }
}
