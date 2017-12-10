using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class SeasonTimer : MonoBehaviour {

    public double timer = 0;
     
    public void OnClick() {

        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[0].Play();

        Message msg = new Message();
        GameObject GameController = GameObject.Find("GameController").gameObject;
        MainStageController MainStageController = GameController.GetComponent<MainStageController>();
        timer = MainStageController.yearTimer;
        string hms = "";
        string msgText = "";
        if (timer >0) {
            TimeSpan ts = new TimeSpan(0, 0, (int)timer);
            hms = ts.ToString();

            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                msgText = "Next season will come after " + hms;
            }else if(langId==3) {
                msgText = "到下一个季节还剩" + hms + "。";
            }
            else {
                msgText = "次の季節まであと" + hms + "ですぞ。";
            }
        }else {
            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                msgText = "Next season has come.";
            }
            else if (langId == 3) {
                msgText = "下一个季节已来临。";
            }
            else {
                msgText = "次の季節はもう来ておりますぞ。";
            }
        }        
        msg.makeMessage(msgText);
    }    
}
