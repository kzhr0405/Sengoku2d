using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class QuestReceive : MonoBehaviour {

    public int id = 0;
	public string key = "";
	public string target = "";
	public int amnt = 0;
	public GameObject slot;

    //count
    public int criteria = 0;
    public int count = 0;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		

        if(criteria == 0){
            //Change Flg
            audioSources[3].Play();
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
            deleteQuest(false, key, id);
        }
        else {
            if(count < criteria) {
                //error
                audioSources[4].Play();
                Message msg = new Message();
                int langId = PlayerPrefs.GetInt("langId");
                msg.makeMessageOnBoard(msg.getMessage(152,langId));
            }else {
                audioSources[3].Play();
                PlayerPrefs.SetBool(key, true);
                if (target == "busyoDama") {
                    int busyoDama = PlayerPrefs.GetInt("busyoDama");
                    int newBusyoDama = busyoDama + amnt;
                    PlayerPrefs.SetInt("busyoDama", newBusyoDama);
                    GameObject.Find("BusyoDamaValue").GetComponent<Text>().text = newBusyoDama.ToString();
                }
                PlayerPrefs.Flush();
                //Remove Quest
                Destroy(slot);
                deleteQuest(true, key, id);
            }
        }
		//Extension Mark Handling
		MainStageController main = new MainStageController();
		main.questExtension();


	}

    public void deleteQuest(bool countQuestFlg, string target, int id) {
        if (target.Contains("Daily")) {
            QuestMenu QuestMenu = GameObject.Find("QuestMenu").transform.Find("Daily").GetComponent<QuestMenu>();
            if (countQuestFlg) {
                QuestMenu.activeCountList.Remove(id);
            }else {
                QuestMenu.activeList.Remove(id);
            }
        }else if(target.Contains("Special")){
            QuestMenu QuestMenu = GameObject.Find("QuestMenu").transform.Find("Special").GetComponent<QuestMenu>();
            if (countQuestFlg) {
                QuestMenu.activeCountList.Remove(id);
            }else {
                QuestMenu.activeList.Remove(id);
            }
        }

        



    }


}
