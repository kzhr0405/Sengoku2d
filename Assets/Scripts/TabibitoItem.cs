using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class TabibitoItem : MonoBehaviour {

	public GameObject touchBackObj;

	public void OnClick(){

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (name == "YesButton") {
			Message msg = new Message ();
            int langId = PlayerPrefs.GetInt("langId");

            //check
            int meiseiItem = PlayerPrefs.GetInt ("meisei");
			if (meiseiItem >= 1) {
				audioSources [3].Play ();

				meiseiItem = meiseiItem - 1;
				PlayerPrefs.SetInt ("meisei", meiseiItem);

				NaiseiController script = GameObject.Find ("NaiseiController").GetComponent<NaiseiController> ();
				int kuniId = script.activeKuniId;
				string tmp1 = "naiseiLoginDate" + kuniId.ToString ();
				string tmp2	= "naiseiTabibitoCounter" + kuniId.ToString ();

				PlayerPrefs.DeleteKey (tmp1);
				PlayerPrefs.DeleteKey (tmp2);

				int tabibitoMaxNo = int.Parse (GameObject.Find ("TabibitoMaxValue").GetComponent<Text> ().text);
				GameObject.Find ("TabibitoCountDownValue").GetComponent<Text> ().text = tabibitoMaxNo.ToString ();
				script.remain = tabibitoMaxNo;
				float specialRatio = script.specialRatio;
                //specialRatio = specialRatio * 2;
                specialRatio = 20;
                script.specialRatio = specialRatio;

				PlayerPrefs.SetBool ("questDailyFlg151", true);
				PlayerPrefs.Flush ();

				//Extension Mark Handling
				MainStageController main = new MainStageController ();
				main.questExtension ();

				msg.makeMessage (msg.getMessage(121, langId));

			} else {
				audioSources [4].Play ();
				msg.makeMessage (msg.getMessage(120, langId));
			}
		} else {
			audioSources [1].Play ();
		}

		touchBackObj.GetComponent<CloseOneBoard> ().OnClick ();
	}
}
