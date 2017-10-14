﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GacyaSpecialTouyou : MonoBehaviour {

    public int hireCount = 0;
    public int selectCount = 0;
    public bool doneFlg = false;
    AudioSource[] audioSources;

    private void Start() {
        audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
    }


    public void OnClick() {
        Message msg = new Message();

        if (selectCount == 0) {
            audioSources[4].Play();
            msg.makeMessage(msg.getMessage(157));
        }else {
            audioSources[0].Play();
            string text = "";
            //Common Process
            string backPath = "Prefabs/Busyo/back";
            GameObject back = Instantiate(Resources.Load(backPath)) as GameObject;
            back.transform.SetParent(GameObject.Find("Panel").transform,false);
            back.transform.localScale = new Vector2(1, 1);
 
            //Message Box
            string msgPath = "Prefabs/TouyouSpecial/TouyouConfirm";
            GameObject msgObj = Instantiate(Resources.Load(msgPath)) as GameObject;
            msgObj.transform.SetParent(GameObject.Find("Panel").transform, false);
            msgObj.transform.localScale = new Vector2(1, 1);
            GacyaSpecialTouyouConfirm GacyaSpecialTouyouConfirmYes = msgObj.transform.FindChild("Yes").GetComponent<GacyaSpecialTouyouConfirm>();
            GacyaSpecialTouyouConfirm GacyaSpecialTouyouConfirmNo = msgObj.transform.FindChild("No").GetComponent<GacyaSpecialTouyouConfirm>();
            GacyaSpecialTouyouConfirmYes.board = msgObj;
            GacyaSpecialTouyouConfirmYes.back = back;
            GacyaSpecialTouyouConfirmNo.board = msgObj;
            GacyaSpecialTouyouConfirmNo.back = back;

            if (hireCount > selectCount) {
                msgObj.transform.FindChild("Text").GetComponent<Text>().text = msg.getMessage(158);
            }else {
                msgObj.transform.FindChild("Text").GetComponent<Text>().text = msg.getMessage(159);
            }
            GameObject.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "unit";
        }
    }

    public void PlusCount() {
        selectCount++;
        transform.FindChild("a").GetComponent<Text>().text = selectCount.ToString();
    }
    public void MinusCount() {
        selectCount--;
        transform.FindChild("a").GetComponent<Text>().text = selectCount.ToString();
    }

    public void OnlyOneCount() {
        selectCount = 1;
        transform.FindChild("a").GetComponent<Text>().text = "1";
    }

}