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

            int stockLimit = PlayerPrefs.GetInt("stockLimit");
            int space = PlayerPrefs.GetInt("space");
            stockLimit = stockLimit + space;

            int myBusyoQty = PlayerPrefs.GetInt("myBusyoQty");
            int stockReq = 10;
            if(transform.parent.name == "s") stockReq = 1;            
            if (myBusyoQty + stockReq > stockLimit) {
                //Error
                audioSources[4].Play();
                Message msg = new Message();
                string Text = "";
                int langId = PlayerPrefs.GetInt("langId");
                if (langId == 2) {
                    Text = "You need " + stockReq.ToString() + " samurai space for special gacya. Now you have just " + (stockLimit - myBusyoQty).ToString() + " space.";
                }else {
                    Text = "特別ガチャには" + stockReq.ToString() + "人分の武将の空きが必要ですぞ。今は" +(stockLimit-myBusyoQty).ToString() + "人分しか空きがありませぬ。";
                }
                msg.makeSpaceBuyBoard(Text);

            }else {
                //OK
                audioSources[8].Play();
                int newBusyoDama = busyoDama - requiredStone;
                PlayerPrefs.SetInt("busyoDama",newBusyoDama);
                PlayerPrefs.Flush();
                GameObject.Find("BusyoDamaValue").GetComponent<Text>().text = newBusyoDama.ToString();

                TouyouSpecialController TouyouSpecialController = GameObject.Find("Controller").GetComponent<TouyouSpecialController>();
                Dictionary<int, Busyo> tmpBusyoListDic = new Dictionary<int, Busyo>();
                tmpBusyoListDic = TouyouSpecialController.busyoListDic;
                string gacyaName = transform.parent.transform.FindChild("Text").GetComponent<Text>().text;
                TouyouSpecialController.doGacyaSpecial(transform.parent.name, gacyaCount, hireCount, tmpBusyoListDic, gacyaName);

                GameObject.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "unit";
            }
        } else {
            //Error
            Message msg = new Message();
            audioSources[4].Play();
            msg.makeMessage(msg.getMessage(2));
        }
	}
}
