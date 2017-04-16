using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class DoTsuihouConfirm : MonoBehaviour {

	// Use this for initialization
	public void OnClick () {
		
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		string busyoName = GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyoName;
		string busyoId = GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyo;
		int daimyoBusyoId = PlayerPrefs.GetInt ("myDaimyoBusyo");

		if (busyoId == daimyoBusyoId.ToString ()) {
			audioSources [4].Play ();
			Message msgNoBtn = new Message();
            string text = "";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                text = "My lord, why will you banish yourself?";
            }else {
                text = "御屋形様、ご自身を追放されるとは\nどういうおつもりですか。";
            }
			msgNoBtn.makeMessage(text);

		} else {
            bool jinkeiBusyoFlg = jinkeiBusyoCheck(int.Parse(busyoId));
            if (jinkeiBusyoFlg) {
                //Error
                audioSources[4].Play();
                Message errorMsg = new Message();
                errorMsg.makeMessage(errorMsg.getMessage(138));

            }else {
                
                audioSources [0].Play ();

			    //Common Process
			    //Back Cover
			    string backPath = "Prefabs/Busyo/back";
			    GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
			    back.transform.SetParent(GameObject.Find ("Panel").transform);
			    back.transform.localScale = new Vector2 (1, 1);
			    RectTransform backTransform = back.GetComponent<RectTransform> ();
			    backTransform.anchoredPosition3D = new Vector3 (0, 0, 0);

			    //Message Box
			    string msgPath = "Prefabs/Busyo/TsuihouConfirm";
			    GameObject msg = Instantiate (Resources.Load (msgPath)) as GameObject;
			    msg.transform.SetParent(GameObject.Find ("Panel").transform);
			    msg.transform.localScale = new Vector2 (1, 1);
			    RectTransform msgTransform = msg.GetComponent<RectTransform> ();
			    msgTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
			    msgTransform.name = "TsuihouConfirm";

			    GameObject msgObj = GameObject.Find ("TsuihouText");
			    string msgText = msgObj.GetComponent<Text> ().text;


			    //Message Text Mod
			    if (Application.systemLanguage != SystemLanguage.Japanese) {
                    msgText = "My lord, do you want to banish "+busyoName+"?";
                }else {
                    msgText = "御屋形様、誠に"+busyoName+"を追放なさるのですか？";
                }
                msgObj.GetComponent<Text> ().text = msgText;

			    //Add busyoId
			    GameObject.Find ("YesButton").GetComponent<DoTsuihou> ().busyoId = int.Parse (busyoId);
			    GameObject.Find ("YesButton").GetComponent<DoTsuihou> ().busyoName = busyoName;
			    GameObject.Find ("NoButton").GetComponent<DoTsuihou> ().busyoId = int.Parse (busyoId);
            }
		}
	}

    public bool jinkeiBusyoCheck(int tsuihouBusyoId) {
        bool jinkeiBusyoFlg = false;

        int jinkei = PlayerPrefs.GetInt("jinkei");
        List<int> slotList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };

        for (int i = 0; i < slotList.Count; i++) {
            string slotId = slotList[i].ToString();
            string mapId = jinkei.ToString() + "map" + slotId;
            if (jinkei == 1) {
                if (slotId == "1" || slotId == "2" || slotId == "7" || slotId == "8" ||
                    slotId == "11" || slotId == "12" || slotId == "13" || slotId == "14" ||
                   slotId == "17" || slotId == "18" || slotId == "21" || slotId == "22") {
                    int busyoId = PlayerPrefs.GetInt(mapId);
                    if (busyoId == tsuihouBusyoId) {
                        jinkeiBusyoFlg = true;
                        break;
                    }
                }
            }
            else if (jinkei == 2) {
                if (slotId == "3" || slotId == "4" || slotId == "5" || slotId == "7" ||
                  slotId == "8" || slotId == "11" || slotId == "12" || slotId == "17" ||
                   slotId == "18" || slotId == "23" || slotId == "24" || slotId == "25") {
                    int busyoId = PlayerPrefs.GetInt(mapId);
                    if (busyoId == tsuihouBusyoId) {
                        jinkeiBusyoFlg = true;
                        break;
                    }
                }
            }
            else if (jinkei == 3) {
                if (slotId == "3" || slotId == "7" || slotId == "8" || slotId == "9" ||
                   slotId == "11" || slotId == "12" || slotId == "14" || slotId == "15" ||
                  slotId == "16" || slotId == "20" || slotId == "21" || slotId == "25") {
                    int busyoId = PlayerPrefs.GetInt(mapId);
                    if (busyoId == tsuihouBusyoId) {
                        jinkeiBusyoFlg = true;
                        break;
                    }
                }
            }
            else if (jinkei == 4) {
                if (slotId == "1" || slotId == "2" || slotId == "7" || slotId == "8" ||
                   slotId == "12" || slotId == "13" || slotId == "14" || slotId == "18" ||
                   slotId == "19" || slotId == "20" || slotId == "24" || slotId == "25") {
                    int busyoId = PlayerPrefs.GetInt(mapId);
                    if (busyoId == tsuihouBusyoId) {
                        jinkeiBusyoFlg = true;
                        break;
                    }
                }
            }
        }

        return jinkeiBusyoFlg;
    }
}
