using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class AddMoney : MonoBehaviour {

	public GameObject touchBackObj;

	public void OnClick(){

		if (name == "YesButton") {
			Message msg = new Message ();

			//check
			int busyoDama = PlayerPrefs.GetInt ("busyoDama");
			if (busyoDama >= 100) {
				AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
				audioSources [3].Play ();

				int currentMoney = PlayerPrefs.GetInt ("money");
				currentMoney = currentMoney + 30000;
                if (currentMoney < 0) {
                    currentMoney = int.MaxValue;
                }
                GameObject.Find ("MoneyValue").GetComponent<Text> ().text = currentMoney.ToString ();
				PlayerPrefs.SetInt ("money",currentMoney);

				int newBusyoDama = busyoDama - 100;
				GameObject.Find ("BusyoDamaValue").GetComponent<Text> ().text = newBusyoDama.ToString ();
				PlayerPrefs.SetInt ("busyoDama",newBusyoDama);


				PlayerPrefs.SetBool ("questDailyFlg150",true);
				PlayerPrefs.Flush ();

				//Extension Mark Handling
				MainStageController main = new MainStageController();
				main.questExtension();

				msg.makeMessage (msg.getMessage(3));

			} else {
				msg.makeMessage (msg.getMessage(2));
			}
		}

		touchBackObj.GetComponent<CloseOneBoard> ().OnClick ();
	}
}
