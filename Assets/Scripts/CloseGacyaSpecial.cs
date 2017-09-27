using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGacyaSpecial : MonoBehaviour {

	public void OnClick() {
        //Check Done or Not
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[1].Play();

        bool doneFlg = transform.parent.transform.FindChild("Button").GetComponent<GacyaSpecialTouyou>().doneFlg;
        if(doneFlg) {
            //close
            GameObject gacya = transform.parent.transform.parent.gameObject;
            Destroy(gacya);

        } else {
            //close confirm board
            //Common Process
            string backPath = "Prefabs/Busyo/back";
            GameObject back = Instantiate(Resources.Load(backPath)) as GameObject;
            back.transform.SetParent(GameObject.Find("Panel").transform, false);
            back.transform.localScale = new Vector2(1, 1);

            //Message Box
            string msgPath = "Prefabs/TouyouSpecial/TouyouCloseConfirm";
            GameObject msgObj = Instantiate(Resources.Load(msgPath)) as GameObject;
            msgObj.transform.SetParent(GameObject.Find("Panel").transform, false);
            msgObj.transform.localScale = new Vector2(1, 1);
            CloseGacyaSpecialConfirm CloseGacyaSpecialConfirmYes = msgObj.transform.FindChild("Yes").GetComponent<CloseGacyaSpecialConfirm>();
            CloseGacyaSpecialConfirm CloseGacyaSpecialConfirmNo = msgObj.transform.FindChild("No").GetComponent<CloseGacyaSpecialConfirm>();
            GameObject gacya = transform.parent.transform.parent.gameObject;
            CloseGacyaSpecialConfirmYes.Gacya = gacya;
            CloseGacyaSpecialConfirmYes.board = msgObj;
            CloseGacyaSpecialConfirmYes.back = back;
            CloseGacyaSpecialConfirmNo.board = msgObj;
            CloseGacyaSpecialConfirmNo.back = back;

            GameObject.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "unit";

        }
    }
}
