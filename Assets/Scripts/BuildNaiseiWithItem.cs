using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuildNaiseiWithItem : MonoBehaviour {

	public int panelId = 0;
	public int naiseiId = 0;
	public string naiseiName = "";
	public int requiredMoney = 0;
	public int requiredHyourou = 0;
	public int techId = 0;
	public int activeKuniId = 0;


	public void OnClick(){

        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        int langId = PlayerPrefs.GetInt("langId");

        //Item Check
        Message msg = new Message();
		string tempParam = "";
		int qty = 0;
		if (techId == 1) {
			tempParam = "transferTP";
		} else if (techId == 2) {
			tempParam = "transferKB";
		} else if (techId == 3) {
			tempParam = "transferSNB";
		}

		qty = PlayerPrefs.GetInt (tempParam);
		int nowHyourou = PlayerPrefs.GetInt ("hyourou");
		
		if (qty < 1) {
			//Error
			//Message
			msg.makeMessage(msg.getMessage(129));
            audioSources[4].Play();

        } else {
			if (nowHyourou < requiredHyourou) {
				//Error
				msg.makeMessage(msg.getMessage(7));
                audioSources[4].Play();

            } else {
                audioSources[3].Play();

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
				
				
				/*Reduce Item & Hyourou*/
				//Item
				int resultItem = qty - 1;
				int resultHyourou = nowHyourou - requiredHyourou;
				PlayerPrefs.SetInt(tempParam,resultItem);
				PlayerPrefs.SetInt("hyourou",resultHyourou);
				
				
				PlayerPrefs.SetString (temp, newNaiseiString);
				PlayerPrefs.Flush();

                //Message
                string OKtext = "";
                if (langId == 2) {
                    OKtext = "You built " + naiseiName + ".\n The country is thriving.";
                }else {
                    OKtext = naiseiName + "を建築しましたぞ。\n国が栄えますな。";
                }
                
                msg.makeMessage (OKtext);
				
				//Close Tab
				GameObject.Find ("close").GetComponent<CloseBoard> ().onClick ();
				
				//Initialization
				NaiseiController naisei = new NaiseiController ();
				naisei.Start ();
			}
		}
	}
}
