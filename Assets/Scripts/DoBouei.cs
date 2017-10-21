using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DoBouei : MonoBehaviour {

	public GameObject slot;
	public string key = "";
	public string kuniName = "";
	public int engunDaimyoId = 0;
	public string engunDaimyoName = "";
	public int engunKuniId = 0;
	public int dfcDaimyoId = 0;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
        int langId = PlayerPrefs.GetInt("langId");

        //Check
        if (GameObject.Find (key)) {
			//OK

			int hyourou = PlayerPrefs.GetInt ("hyourou");
			int newHyourou = hyourou - 10;
			PlayerPrefs.SetInt ("hyourou", newHyourou);
			GameObject.Find ("HyourouCurrentValue").GetComponent<Text> ().text = newHyourou.ToString ();

			//1.My Kuni is the biggiest ... 30%
			//2.My Yukoudo ... 50%
			//3.Other Yukoudo ... 20%
			float ratio = 0;

			//1
			bool myKuniQtyIsBiggestFlg = GameObject.Find ("GameController").GetComponent<MainStageController> ().myKuniQtyIsBiggestFlg;
			if (myKuniQtyIsBiggestFlg) {
				ratio = 30;
			}

			//2
			Gaikou gaikou = new Gaikou();
			int myGaikouValue = gaikou.getMyGaikou (engunDaimyoId);
			ratio = ratio + (float)myGaikouValue / 2;

			//3
			int otherGaikouValue = gaikou.getOtherGaikouValue (engunDaimyoId, dfcDaimyoId);
			ratio = ratio + (float)otherGaikouValue / 5;

			float percent = UnityEngine.Random.value;
			percent = percent * 100;
			if (percent <= ratio) {
				//OK
				audioSources [3].Play ();

				MainEventHandler mEvent = new MainEventHandler();
				string engunSts = engunDaimyoId + "-" + mEvent.getEngunSts(engunDaimyoId.ToString());
				int engunHei  = mEvent.getEngunHei(engunSts);

				GameObject gunzei = GameObject.Find (key).gameObject;
				string tmp = gunzei.GetComponent<Gunzei>().dstEngunSts;

				//Set Param

				string keyValue = PlayerPrefs.GetString (key);
				List<string> keyValueList = new List<string> ();
				char[] delimiterChars = {','};
                if(keyValue.Contains(",")) {
				    keyValueList = new List<string> (keyValue.Split (delimiterChars));
                }else {
                    keyValueList.Add(keyValue);
                }

                gunzei.GetComponent<Gunzei> ().dstEngunFlg = true;
				if (tmp != null && tmp != "") {
					string newDstEngunSts = tmp + ":" + engunSts;
					gunzei.GetComponent<Gunzei> ().dstEngunSts = newDstEngunSts;

					string tmpEngunHei = gunzei.GetComponent<Gunzei>().dstEngunHei;
					string newDstEngunHei = tmpEngunHei + ":" + engunHei.ToString();
					gunzei.GetComponent<Gunzei> ().dstEngunHei = newDstEngunHei;

                    string tmpDstEngunDaimyoId = gunzei.GetComponent<Gunzei>().dstEngunDaimyoId;
					string newDstEngunDaimyoId  = tmpDstEngunDaimyoId.ToString() + ":" + engunDaimyoId.ToString();
					gunzei.GetComponent<Gunzei> ().dstEngunDaimyoId = newDstEngunDaimyoId;

					//Set Data
					keyValue = keyValueList[0] + "," + keyValueList[1] + "," + keyValueList[2] + "," + keyValueList[3] + "," + keyValueList[4] + "," + keyValueList[5] + "," + keyValueList[6] + "," + keyValueList[7] + "," + keyValueList[8] + "," + keyValueList[9] + "," + newDstEngunDaimyoId + "," + newDstEngunHei + ","+ newDstEngunSts;

				} else {
					gunzei.GetComponent<Gunzei> ().dstEngunSts = engunSts;
					gunzei.GetComponent<Gunzei> ().dstEngunHei = engunHei.ToString();
					gunzei.GetComponent<Gunzei> ().dstEngunDaimyoId = engunDaimyoId.ToString();

					//Set Data
					keyValue = keyValueList[0] + "," + keyValueList[1] + "," + keyValueList[2] + "," + keyValueList[3] + "," + keyValueList[4] + "," + keyValueList[5] + "," + keyValueList[6] + "," + keyValueList[7] + "," + keyValueList[8] + "," + true + "," + engunDaimyoId.ToString() + "," + engunHei.ToString() + ","+ engunSts;

				}
				PlayerPrefs.SetString (key, keyValue);
				PlayerPrefs.Flush ();


				//Return & Message
				GameObject.Find("bakuhuReturn").GetComponent<BakuhuMenuReturn>().OnClick();
				Message msg = new Message ();
                string OKtext = "";
                
                if (langId == 2) {
                    OKtext = engunDaimyoName + " sent " + engunHei.ToString() + " soldiers to \n" + kuniName + " to support.";
                }else {
                    OKtext = engunDaimyoName + "殿が" + engunHei.ToString() + "の兵を\n" + kuniName + "救援に差し向けましたぞ。";
                }
				msg.makeMessageOnBoard (OKtext);



			} else {
				//NG
				audioSources [4].Play ();

				GameObject.Find("bakuhuReturn").GetComponent<BakuhuMenuReturn>().OnClick();
				int newYukoudo = gaikou.downMyGaikou(engunDaimyoId, myGaikouValue, 15);
				int reducedValue = myGaikouValue - newYukoudo;
				Message msg = new Message ();
                string NGtext = "";
                if (langId == 2) {
                    NGtext = engunDaimyoName + " declined our defence order. \n Friendship decreased " + reducedValue + " point.";
                } else {
                    NGtext = "援軍の儀、" + engunDaimyoName + "殿に断られ申した。\n当家との友好度が" + reducedValue + "下がります。";
                }
				DoGaikou doGaikou = new DoGaikou ();
				doGaikou.downYukouOnIcon(engunDaimyoId, newYukoudo);
				msg.makeMessageOnBoard (NGtext);
			}

		} else {
			//NG
			audioSources [4].Play ();
			GameObject.Find("bakuhuReturn").GetComponent<BakuhuMenuReturn>().OnClick();
			Message msg = new Message ();
            string NGtext = "";
            if (langId == 2) {
                NGtext = "My lord, it was too late. Battle already finished.";
            }else {
                NGtext = "御屋形様、既に勝敗は決してしまったようですぞ。";
            }
			msg.makeMessageOnBoard (NGtext);
			Destroy (slot.gameObject);

		}




	}
}
