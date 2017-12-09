using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class AddHyourou : MonoBehaviour {

	public GameObject touchBackObj;

	public void OnClick(){

		if (name == "YesButton") {
			Message msg = new Message ();
            int langId = PlayerPrefs.GetInt("langId");
            //check
            int busyoDama = PlayerPrefs.GetInt ("busyoDama");
			if (busyoDama >= 100) {
				AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
				audioSources [3].Play ();

				int newHyourou = 100;
                if (Application.loadedLevelName != "shisya") {
                    GameObject.Find ("HyourouCurrentValue").GetComponent<Text> ().text = newHyourou.ToString ();
                }else {
                    GameObject.Find("Hyourou").transform.Find("Value").GetComponent<Text>().text = newHyourou.ToString();
                }

				int newBusyoDama = busyoDama - 100;
                if (Application.loadedLevelName != "naisei") {
                    GameObject.Find ("BusyoDamaValue").GetComponent<Text> ().text = newBusyoDama.ToString ();
                }

				PlayerPrefs.SetInt ("busyoDama",newBusyoDama);
				PlayerPrefs.SetInt ("hyourou",newHyourou);

                if (Application.loadedLevelName != "shisya" && Application.loadedLevelName != "naisei" && Application.loadedLevelName != "pvp") {
                    MainStageController mainScript = GameObject.Find ("GameController").GetComponent<MainStageController> ();
				    mainScript.hyourouFull = true;
				    mainScript.nowHyourou = 100;
				    mainScript.timer = 180;
				    GameObject.Find ("TimerValue").GetComponent<Text> ().text = "180";
                }
				PlayerPrefs.SetBool ("questDailyFlg37",true);
				PlayerPrefs.Flush ();

				//Extension Mark Handling
				MainStageController main = new MainStageController();
				main.questExtension();

				msg.makeMessage (msg.getMessage(4, langId));
				
			} else {
				msg.makeMessage (msg.getMessage(2, langId));
			}
		}

		touchBackObj.GetComponent<CloseOneBoard> ().OnClick ();

	}
}
