using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class GacyaSpecial : MonoBehaviour {

    public string typ = "";
    public int requiredStone = 0;
    public int gacyaCount = 0;
    public int hireCount = 0;

    void Start () {

        string typ = transform.parent.name;
        if (typ == "nml") {
            requiredStone = 1000;
            gacyaCount = 30;
            hireCount = 10;
        }else if(typ == "heisyu") {
            requiredStone = 2500;
            gacyaCount = 30;
            hireCount = 10;
        }else if (typ == "daimyo") {
            requiredStone = 5000;
            gacyaCount = 30;
            hireCount = 10;
        }else if (typ == "s") {
            requiredStone = 10000;
            gacyaCount = 10;
            hireCount = 1;
        }
    }


    public void OnClick () {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        int busyoDama = PlayerPrefs.GetInt("busyoDama");
        if (busyoDama >= requiredStone) {
            //OK
            audioSources[8].Play();
            int newBusyoDama = busyoDama - requiredStone;
            //PlayerPrefs.SetInt("busyoDama",newBusyoDama);
            //GameObject.Find("BusyoDamaValue").GetComponent<Text>().text = newBusyoDama.ToString();

            TouyouSpecialController TouyouSpecialController = GameObject.Find("Controller").GetComponent<TouyouSpecialController>();
            Dictionary<int, Busyo> tmpBusyoListDic = new Dictionary<int, Busyo>();
            tmpBusyoListDic = TouyouSpecialController.busyoListDic;
            TouyouSpecialController.doGacyaSpecial(transform.parent.name, gacyaCount, hireCount, tmpBusyoListDic);

        } else {
            //Error
            Message msg = new Message();
            audioSources[4].Play();
            msg.makeMessage(msg.getMessage(2));
        }
	}
}
