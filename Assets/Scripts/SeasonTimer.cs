using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SeasonTimer : MonoBehaviour {

    public double timer = 0;
     
    public void OnClick() {
        Message msg = new Message();
        GameObject GameController = GameObject.Find("GameController").gameObject;
        MainStageController MainStageController = GameController.GetComponent<MainStageController>();
        timer = MainStageController.yearTimer;
        string hms = "";
        string msgText = "";
        if (timer >0) {
            TimeSpan ts = new TimeSpan(0, 0, (int)timer);
            hms = ts.ToString();

            if (Application.systemLanguage != SystemLanguage.Japanese) {
                msgText = "Next season will come after " + hms;
            }else {
                msgText = "次の季節まであと" + hms + "ですぞ。";
            }
        }else {
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                msgText = "Next season has come.";
            }else {
                msgText = "次の季節はもう来ておりますぞ";
            }
        }        
        msg.makeMessage(msgText);
    }    
}
