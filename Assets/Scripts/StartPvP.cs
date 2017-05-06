using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class StartPvP : MonoBehaviour {

    public Text textScript;
    public bool secondTimeFlg = false;
    public GameObject touchBackObj;

    public void OnClick() {

        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        if (textScript.text == "") {
            audioSources[4].Play();
            Message msg = new Message();
            if (!secondTimeFlg) {
                msg.makeMessage(msg.getMessage(140));
            }else {
                msg.makeMessage(msg.getMessage(142));
            }
        }else {
            //Text Registeration
            audioSources[5].Play();
            DataPvP pvpScript = new DataPvP();

            if (!secondTimeFlg) {
                //Init Data Registration                
                string userId = PlayerPrefs.GetString("userId");
                string userName = textScript.text;
                pvpScript.InsertPvP(userId, userName);

                PlayerPrefs.SetString("PvPName", userName);
                PlayerPrefs.Flush();

                //Scene Change
                Application.LoadLevel("pvp");
            }else {
                //Second Time
                string userId = PlayerPrefs.GetString("userId");
                string userName = textScript.text;
                pvpScript.UpdatePvPName(userId, userName);

                PlayerPrefs.SetString("PvPName", userName);
                PlayerPrefs.Flush();

                GameObject.Find("myName").GetComponent<Text>().text = userName;
                touchBackObj.GetComponent<CloseOneBoard>().OnClick();
            }
        }
    }
}
