using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System.Collections.Generic;

public class DoNextSeason : MonoBehaviour {

	public GameObject touchBackObj;

	public void OnClick(){
		
		if (name == "YesButton") {
			Message msg = new Message ();

			//check
			int busyoDama = PlayerPrefs.GetInt ("busyoDama");
			if (busyoDama >= 100) {

				AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
				audioSources [3].Play ();

				//Busyo Dama
				int newBusyoDama = busyoDama - 100;
				GameObject.Find ("BusyoDamaValue").GetComponent<Text> ().text = newBusyoDama.ToString ();
				PlayerPrefs.SetInt ("busyoDama",newBusyoDama);


                //Season Change
                /*
				
				string yearSeason = PlayerPrefs.GetString ("yearSeason");
				char[] delimiterChars = {','};
				string[] yearSeasonList = yearSeason.Split (delimiterChars);
				int nowYear = int.Parse (yearSeasonList [0]);
				int nowSeason = int.Parse (yearSeasonList [1]);

				if(nowSeason==4){
					nowYear = nowYear + 1;
					nowSeason  = 1;
				}else{
					nowSeason = nowSeason + 1;
				}
				string newYearSeason = nowYear.ToString() + "," + nowSeason.ToString();
				PlayerPrefs.SetString ("yearSeason", newYearSeason);	

                string lastSeasonChangeTime = System.DateTime.Now.ToString ();
				PlayerPrefs.SetString ("lastSeasonChangeTime", lastSeasonChangeTime);
				PlayerPrefs.SetBool ("doneCyosyuFlg", false);
				mainScript.doneCyosyuFlg = false;
				PlayerPrefs.DeleteKey ("usedBusyo");
                deleteLinkCut();
                deleteWinOver();
                */

                PlayerPrefs.SetBool ("questDailyFlg36",true);
				PlayerPrefs.DeleteKey ("bakuhuTobatsuDaimyoId");
				PlayerPrefs.Flush ();

                //Extension Mark Handling
                //MainStageController main = new MainStageController();
                //main.questExtension();

                //Change Label
                //GameObject.Find ("YearValue").GetComponent<Text> ().text = nowYear.ToString ();			
                //mainScript.SetSeason(nowSeason);

                MainStageController mainScript = GameObject.Find("GameController").GetComponent<MainStageController>();
                mainScript.yearTimer = 1;

				msg.makeMessageOnBoard (msg.getMessage(1));

			} else {
				msg.makeMessageOnBoard (msg.getMessage(2));
			}
		}
		touchBackObj.GetComponent<CloseOneBoard> ().OnClick ();
	
	
	}

    public void deleteLinkCut() {
        for (int i = 1; i < 66; i++) {
            string linkuctTmp = "linkcut" + i.ToString();
            PlayerPrefs.DeleteKey(linkuctTmp);
        }
        PlayerPrefs.Flush();
    }

    public void deleteWinOver() {
        string cyouryaku = PlayerPrefs.GetString("cyouryaku");
        char[] delimiterChars = { ',' };
        List<string> cyouryakuList = new List<string>();
        if (cyouryaku != null && cyouryaku != "") {
            if (cyouryaku.Contains(",")) {
                cyouryakuList = new List<string>(cyouryaku.Split(delimiterChars));
            }else {
                cyouryakuList.Add(cyouryaku);
            }
        }
        for (int i = 0; i < cyouryakuList.Count; i++) {
            PlayerPrefs.DeleteKey(cyouryakuList[i]);
        }
        PlayerPrefs.DeleteKey("cyouryaku");
        PlayerPrefs.Flush();
    }


}
