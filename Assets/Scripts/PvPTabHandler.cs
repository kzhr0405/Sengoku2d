﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PvPTabHandler : MonoBehaviour {

    public bool clicked = false;
    public GameObject kassenViewObj;
    public GameObject rankViewObj;
    public GameObject rankWeeklyViewObj;
    public bool isReadyFlg;
    public bool isReadyWeeklyFlg;

    public void Start() {

        //Initial
        if (name == "Kassen") {
            OnClick();
        }
    }


	public void OnClick() {

        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[0].Play();

        //Check Network Ready
        if (name == "Rank") {
            PvPDataStore PvPDataStore = GameObject.Find("PvPDataStore").GetComponent<PvPDataStore>();

            if(PvPDataStore.Top3HPQtyList.Count > 0 && PvPDataStore.winRank > 0) {
                isReadyFlg = true;
            }
        }else if(name == "RankWeekly"){
            PvPDataStore PvPDataStore = GameObject.Find("PvPDataStore").GetComponent<PvPDataStore>();
            if (PvPDataStore.Top10PtWeeklyBusyoList.Count > 0 && PvPDataStore.ptRankWeekly > 0) {
                isReadyFlg = true;
            }
        }else { 
            isReadyFlg = true;
        }

        if(isReadyFlg) {
            if (!clicked) {
                //Teb Changer
                Color pushedTabColor = new Color(118f / 255f, 103f / 255f, 16f / 255f, 255f / 255f);
                Color pushedTextColor = new Color(219f / 255f, 219f / 255f, 212f / 255f, 255f / 255f);
                Color normalTabColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
                Color normalTextColor = new Color(255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);

                GameObject UpperView = GameObject.Find("UpperView").gameObject;
                foreach (Transform obj in UpperView.transform) {
                    obj.GetComponent<Image>().color = normalTabColor;
                    obj.transform.Find("Text").GetComponent<Text>().color = normalTextColor;
                    obj.GetComponent<PvPTabHandler>().clicked = false;
                }
                GetComponent<Image>().color = pushedTabColor;
                transform.Find("Text").GetComponent<Text>().color = pushedTextColor;
                clicked = true;

                if(name == "Kassen") {
                    kassenViewObj.SetActive(true);
                    rankViewObj.SetActive(false);
                    rankWeeklyViewObj.SetActive(false);
                }else if(name == "Rank"){
                    kassenViewObj.SetActive(false);
                    rankWeeklyViewObj.SetActive(false);
                    rankViewObj.SetActive(true);

                    GameObject.Find("GameScene").GetComponent<PvPController>().ShowRank(rankViewObj);
                }else if(name == "RankWeekly") {
                    kassenViewObj.SetActive(false);
                    rankViewObj.SetActive(false);
                    rankWeeklyViewObj.SetActive(true);

                    GameObject.Find("GameScene").GetComponent<PvPController>().ShowRankWeekly(rankWeeklyViewObj);
                }
            }
        }else {
            Message msg = new Message();
            msg.makeMessage(msg.getMessage(143));
        }
    }

    
}
