using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class SwitchTown : MonoBehaviour {

    public bool nextExistFlg = false;
    public int nextKuniId = 0;
    public  string nextKuniName = "";
    public int currentKuniId = 0;
    public bool spdUpFlg = false;

    public void Start() {
        currentKuniId = PlayerPrefs.GetInt("activeKuniId");

        string seiryoku = PlayerPrefs.GetString("seiryoku");
        List<string> seiryokuList = new List<string>();
        char[] delimiterChars = { ',' };
        seiryokuList = new List<string>(seiryoku.Split(delimiterChars));

        int myDaimyoId = PlayerPrefs.GetInt("myDaimyo");
        List<int> myKuniList = new List<int>();
        for (int m = 0; m < seiryokuList.Count; m++) {
            int DaimyoId = int.Parse(seiryokuList[m]);
            if (DaimyoId == myDaimyoId) {
                myKuniList.Add(m+1);
            }
        }


        if(myKuniList.Count > 1) {
            nextExistFlg = true;
            for (int i=0; i< myKuniList.Count; i++) {
                int kuniId = myKuniList[i];
                if(currentKuniId == kuniId) {
                    if((i + 1) == myKuniList.Count) {
                        //back to first
                        nextKuniId = myKuniList[0];
                    }else {
                        //next
                        nextKuniId = myKuniList[i + 1];
                    }
                }
            }
            KuniInfo kuniScript = new KuniInfo();
            int langId = PlayerPrefs.GetInt("langId");
            nextKuniName = kuniScript.getKuniName(nextKuniId,langId);
            
            if (langId == 2) {
                transform.FindChild("Text").GetComponent<Text>().text = "Next Town\n" + nextKuniName;
            }else {
                transform.FindChild("Text").GetComponent<Text>().text = "次の国へ\n" + nextKuniName;
            }


        }
        else {
            Destroy(gameObject);
        }
    }



    public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[0].Play();
        
        if(nextExistFlg) {
            if (!spdUpFlg) {
                PlayerPrefs.SetInt("activeKuniId", nextKuniId);
                PlayerPrefs.SetString("activeKuniName", nextKuniName);
                PlayerPrefs.Flush();
                Application.LoadLevel("naisei");
            }else {
                string backPath = "Prefabs/Common/TouchBack";
                GameObject back = Instantiate(Resources.Load(backPath)) as GameObject;
                back.transform.SetParent(GameObject.Find("Panel").transform);
                back.transform.localScale = new Vector2(1, 1);
                RectTransform backTransform = back.GetComponent<RectTransform>();
                backTransform.anchoredPosition3D = new Vector3(0, 0, 0);
                back.name = "TouchBack";

                //Message Box
                string msgPath = "Prefabs/Naisei/LeaveSpdUpConfirm";
                GameObject msg = Instantiate(Resources.Load(msgPath)) as GameObject;
                msg.transform.SetParent(back.transform);
                msg.transform.localScale = new Vector2(1, 1);
                RectTransform msgTransform = msg.GetComponent<RectTransform>();
                msgTransform.anchoredPosition3D = new Vector3(0, 0, 0);
                msgTransform.name = "LeaveSpdUpConfirm";

                //flg
                msg.transform.FindChild("YesButton").GetComponent<LeaveSpdUp>().nextFlg = true;
            }
        }else {
            Debug.Log("error");
        }

    }
}