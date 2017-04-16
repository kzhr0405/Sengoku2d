using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class QuestReceive : MonoBehaviour {

	public string key = "";
	public string target = "";
	public int amnt = 0;
	public GameObject slot;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [3].Play ();

		//Change Flg
		PlayerPrefs.SetBool(key,true);
		if (target == "money") {
			int money = PlayerPrefs.GetInt ("money");
			int newMoney = money + amnt;
            if (newMoney < 0) {
                newMoney = int.MaxValue;
            }
            PlayerPrefs.SetInt ("money",newMoney);
			GameObject.Find ("MoneyValue").GetComponent<Text> ().text = newMoney.ToString ();

		} else if(target == "busyoDama"){
			int busyoDama = PlayerPrefs.GetInt ("busyoDama");
			int newBusyoDama = busyoDama + amnt;
			PlayerPrefs.SetInt ("busyoDama",newBusyoDama);
			GameObject.Find ("BusyoDamaValue").GetComponent<Text> ().text = newBusyoDama.ToString ();
		}

		PlayerPrefs.Flush ();

		//Remove Quest
		Destroy(slot);

		//Extension Mark Handling
		MainStageController main = new MainStageController();
		main.questExtension();


	}
}
