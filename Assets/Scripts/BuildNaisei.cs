using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuildNaisei : MonoBehaviour {

	public int panelId = 0;
	public int naiseiId = 0;
	public string naiseiName = "";
	public int requiredMoney = 0;
	public int requiredHyourou = 0;
	int activeKuniId = 0;

	// Use this for initialization
	public void OnClick () {

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		//Money Check
		Message msg = new Message(); 
		int nowMoney = PlayerPrefs.GetInt ("money");
		int nowHyourou = PlayerPrefs.GetInt ("hyourou");

		if (nowMoney < requiredMoney) {
			//Error
			//Message
			audioSources [4].Play ();
			msg.makeMessage(msg.getMessage(6));
			
		} else {
			if (nowHyourou < requiredHyourou) {
				//Error
				audioSources [4].Play ();
				msg.makeMessage(msg.getMessage(7));

			} else {
				audioSources [3].Play ();

				activeKuniId = GameObject.Find ("NaiseiController").GetComponent<NaiseiController> ().activeKuniId;
				string temp = "naisei" + activeKuniId.ToString ();

				//Defalt=> 1,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0
				string naiseiString = PlayerPrefs.GetString (temp);
				List<string> naiseiList = new List<string> ();
				char[] delimiterChars = {','};
				naiseiList = new List<string> (naiseiString.Split (delimiterChars));

				string newNaiseiString = "";
				for (int i=0; i<naiseiList.Count; i++) {
					if (i == panelId) {
						string newParam = naiseiId.ToString () + ":1";
						newNaiseiString = newNaiseiString + "," + newParam;

					} else {
						if (newNaiseiString == "") {
							newNaiseiString = naiseiList [i];
						} else {
							newNaiseiString = newNaiseiString + "," + naiseiList [i];
						}
					}
				}


				/*Reduce Money & Hyourou*/
				//Money
				int resultMoney = nowMoney - requiredMoney;
				int resultHyourou = nowHyourou - requiredHyourou;
				PlayerPrefs.SetInt("money",resultMoney);
				PlayerPrefs.SetInt("hyourou",resultHyourou);

				//Track
				int TrackBuildMoneyNo = PlayerPrefs.GetInt("TrackBuildMoneyNo",0);
				TrackBuildMoneyNo = TrackBuildMoneyNo + requiredMoney;
				PlayerPrefs.SetInt("TrackBuildMoneyNo",TrackBuildMoneyNo);


				PlayerPrefs.SetString (temp, newNaiseiString);
				PlayerPrefs.SetBool ("questDailyFlg16",true);
				PlayerPrefs.Flush();

                //Message
                string OKtext = "";
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    OKtext = "You built " + naiseiName + ".\n The country is thriving.";
                }else {
                    OKtext = naiseiName + "を建築しましたぞ。\n国が栄えますな。";
                }
				msg.makeMessage (OKtext);

                //Close Tab
                if (Application.loadedLevelName == "tutorialNaisei") {
                    Destroy(GameObject.Find("Back(Clone)").gameObject);
                    Destroy(GameObject.Find("board(Clone)").gameObject);
                    PlayerPrefs.SetInt("tutorialId", 3);
                    PlayerPrefs.Flush();
                }else { 
                    GameObject.Find ("close").GetComponent<CloseBoard> ().onClick ();
                }

                //Initialization
                NaiseiController naisei = new NaiseiController ();
				naisei.Start ();

                if (Application.loadedLevelName == "tutorialNaisei") {
                    GameObject tBtnObj = GameObject.Find("tButton").gameObject;
                    Destroy(tBtnObj.transform.FindChild("12").gameObject);

                    GameObject NaiseiViewObj = GameObject.Find("NaiseiView").gameObject;
                    GameObject builtObj = NaiseiViewObj.transform.FindChild("12").gameObject;
                    builtObj.transform.SetParent(tBtnObj.transform);
                    builtObj.GetComponent<Button>().enabled = false;
                }
            }
		}
	}
}
