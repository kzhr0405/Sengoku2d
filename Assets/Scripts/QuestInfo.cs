using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class QuestInfo : MonoBehaviour {

	public void OnClick(){
		//SE
		AudioSource sound = GameObject.Find ("SEController").GetComponent<AudioSource> ();
		sound.PlayOneShot(sound.clip); 

		/*Popup*/
		string backPath = "Prefabs/Busyo/back";
		GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
		back.transform.SetParent (GameObject.Find ("Map").transform);
		back.transform.localScale = new Vector2 (1, 1);
		RectTransform backTransform = back.GetComponent<RectTransform> ();
		backTransform.anchoredPosition = new Vector3 (0, 0, 0);

		//Popup Screen
		string popupPath = "Prefabs/Busyo/board";
		GameObject popup = Instantiate (Resources.Load (popupPath)) as GameObject;
		popup.transform.SetParent (GameObject.Find ("Map").transform);
		popup.transform.localScale = new Vector2 (1, 1);
		RectTransform popupTransform = popup.GetComponent<RectTransform> ();
		popupTransform.anchoredPosition = new Vector3 (0, 0, 0);
		popup.name = "board";
		GameObject close = popup.transform.Find ("close").gameObject;

		//qa
		string qaPath = "Prefabs/Common/Question";
		GameObject qa = Instantiate (Resources.Load (qaPath)) as GameObject;
		qa.transform.SetParent(popup.transform);
		qa.transform.localScale = new Vector2 (1, 1);
		RectTransform qaTransform = qa.GetComponent<RectTransform> ();
		qaTransform.anchoredPosition = new Vector3 (-540, 285, 0);
		qa.name = "qa";
		qa.GetComponent<QA> ().qaId = 3;


		//Pop text
		string popTextPath = "Prefabs/Busyo/popText";
		GameObject popText = Instantiate (Resources.Load (popTextPath)) as GameObject;
		popText.transform.SetParent (popup.transform);
		popText.transform.localScale = new Vector2 (0.25f, 0.25f);
		RectTransform popTextTransform = popText.GetComponent<RectTransform> ();
		popTextTransform.anchoredPosition = new Vector3 (0, 275, 0);
		popText.name = "popText";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            popText.GetComponent<Text>().text = "Achievement";
        }else if(langId==3) {
            popText.GetComponent<Text>().text = "目标达成";
        }else {
            popText.GetComponent<Text>().text = "達成目標";
        }

        //Quest Scroll
        string scrollPath = "Prefabs/Map/quest/QuestScrollView";
		GameObject scroll = Instantiate (Resources.Load (scrollPath)) as GameObject;
		scroll.transform.SetParent (popup.transform);
		scroll.transform.localScale = new Vector2 (1, 1);
		scroll.transform.localPosition = new Vector2 (0,230);
        scroll.name = "QuestScrollView";

        //Quest Menu
        string QuestMenuPath = "Prefabs/Map/quest/QuestMenu";
        GameObject menu = Instantiate(Resources.Load(QuestMenuPath)) as GameObject;
        menu.transform.SetParent(popup.transform);
        menu.transform.localScale = new Vector2(1, 1);
        menu.name = "QuestMenu";
        QuestMenu QuestMenuDaily = menu.transform.Find("Daily").GetComponent<QuestMenu>();
        QuestMenu QuestMenuSpecial = menu.transform.Find("Special").GetComponent<QuestMenu>();
        QuestMenuDaily.scrollObj = scroll;
        QuestMenuSpecial.scrollObj = scroll;        

        /*
        //Slot
        List<int> activeSpecialList = new List<int> ();
		List<int> activeDailyList = new List<int> ();
		List<int> inactiveDailyList = new List<int> ();

		//Quest Data Orderby
		Entity_quest_mst questMst = Resources.Load ("Data/quest_mst") as Entity_quest_mst;
		for(int i=0; i<questMst.param.Count; i++){
			bool dailyFlg = questMst.param [i].daily;

			if (!dailyFlg) {
				//Special
				string tmp = "questSpecialFlg" + i.ToString ();
				bool activeFlg = PlayerPrefs.GetBool (tmp, false);
                
				if (activeFlg) {
                    //active
                    
					//received or not
					string tmp2 = "questSpecialReceivedFlg" + i.ToString ();
					bool activeFlg2 = PlayerPrefs.GetBool (tmp2, false);
                    if (!activeFlg2) {
						activeSpecialList.Add (i);
					}

				}
			} else {
				//Daily
				string tmp = "questDailyFlg" + i.ToString ();
				bool activeFlg = PlayerPrefs.GetBool (tmp, false);
				if (activeFlg) {
					//active

					//received or not
					string tmp2 = "questDailyReceivedFlg" + i.ToString ();
					bool activeFlg2 = PlayerPrefs.GetBool (tmp2, false);
					if (!activeFlg2) {
						activeDailyList.Add (i);
					}
				} else {
					//inactive
					inactiveDailyList.Add(i);
				}
			}
		}

		//Sum
		activeSpecialList.AddRange(activeDailyList);


		//Show Active QuestSlot
		GameObject content = scroll.transform.FindChild("Content").gameObject;
		string activeSlotPath = "Prefabs/Map/quest/ActiveQuestSlot";
		for(int j=0; j<activeSpecialList.Count; j++){
			int id = activeSpecialList [j];

			GameObject slot = Instantiate (Resources.Load (activeSlotPath)) as GameObject;
			slot.transform.SetParent (content.transform);
			slot.transform.localScale = new Vector2 (1, 1);

			string title = getQuestTitle (id);
			string exp = getQuestExp (id);
			string target = getQuestTarget (id);
			int amnt = getQuestAmnt (id);
			bool daily = getQuestDaily(id);
			string key = "";
			if (daily) {
				key = "questDailyReceivedFlg" + id.ToString ();
			} else {
				key = "questSpecialReceivedFlg" + id.ToString ();
			}

			GameObject itemImage = slot.transform.FindChild ("itemImage").gameObject;
			GameObject itemQty = slot.transform.FindChild ("itemQty").gameObject;
			GameObject titleValue = slot.transform.FindChild ("titleValue").gameObject;
			GameObject expValue = slot.transform.FindChild ("expValue").gameObject;

			if (target == "busyoDama") {
				string imagePath = "Prefabs/Common/Sprite/busyoDama";
				itemImage.GetComponent<Image> ().sprite = 
					Resources.Load (imagePath, typeof(Sprite)) as Sprite;
			} else {
				string imagePath = "Prefabs/Common/Sprite/money";
				itemImage.GetComponent<Image> ().sprite = 
					Resources.Load (imagePath, typeof(Sprite)) as Sprite;
			}

			itemQty.GetComponent<Text> ().text = amnt.ToString ();
			titleValue.GetComponent<Text> ().text = title;
			expValue.GetComponent<Text> ().text = exp;

			QuestReceive btnScript = slot.transform.FindChild ("ReceiveButton").GetComponent<QuestReceive>();
			btnScript.key = key;
			btnScript.target = target;
			btnScript.amnt = amnt;
			btnScript.slot = slot;

		}

		//Show Inactive QuestSlot
		string inactiveSlotPath = "Prefabs/Map/quest/InactiveQuestSlot";
		for(int k=0; k<inactiveDailyList.Count; k++){
			int id = inactiveDailyList [k];

			GameObject slot = Instantiate (Resources.Load (inactiveSlotPath)) as GameObject;
			slot.transform.SetParent (content.transform);
			slot.transform.localScale = new Vector2 (1, 1);

			string title = getQuestTitle (id);
			string exp = getQuestExp (id);
			string target = getQuestTarget (id);
			int amnt = getQuestAmnt (id);

			GameObject itemImage = slot.transform.FindChild ("itemImage").gameObject;
			GameObject itemQty = slot.transform.FindChild ("itemQty").gameObject;
			GameObject titleValue = slot.transform.FindChild ("titleValue").gameObject;
			GameObject expValue = slot.transform.FindChild ("expValue").gameObject;

			if (target == "busyoDama") {
				string imagePath = "Prefabs/Common/Sprite/busyoDama";
				itemImage.GetComponent<Image> ().sprite = 
					Resources.Load (imagePath, typeof(Sprite)) as Sprite;
			} else {
				string imagePath = "Prefabs/Common/Sprite/money";
				itemImage.GetComponent<Image> ().sprite = 
					Resources.Load (imagePath, typeof(Sprite)) as Sprite;
			}

			itemQty.GetComponent<Text> ().text = amnt.ToString ();
			titleValue.GetComponent<Text> ().text = title;
			expValue.GetComponent<Text> ().text = exp;

		}

		//Scroll Position
		ScrollRect scrollRect = scroll.GetComponent<ScrollRect>();
		scrollRect.verticalNormalizedPosition = 1;
        */
    }


    public string getQuestTitle(int id, int langId){
		Entity_quest_mst questMst = Resources.Load ("Data/quest_mst") as Entity_quest_mst;
        string title = "";
        if (langId == 2) {
            title = questMst.param[id].titleEng;
        }else if(langId==3) {
            title = questMst.param[id].titleSChn;
        }else { 
            title = questMst.param[id].title;
        }
        return title;
	}
	public string getQuestExp(int id, int langId) {
		Entity_quest_mst questMst = Resources.Load ("Data/quest_mst") as Entity_quest_mst;
        string exp = "";
        if (langId == 2) {
            exp = questMst.param[id].expEng;
        }else if(langId==3) {
            exp = questMst.param[id].expSChn;
        }else {
            exp = questMst.param[id].exp;
        }   
		return exp;
	}
	public string getQuestTarget(int id){
		Entity_quest_mst questMst = Resources.Load ("Data/quest_mst") as Entity_quest_mst;
		string target = questMst.param[id].target;
		return target;
	}
	public int getQuestAmnt(int id){
		Entity_quest_mst questMst = Resources.Load ("Data/quest_mst") as Entity_quest_mst;
		int amnt = questMst.param[id].amnt;
		return amnt;
	}
	public bool getQuestDaily(int id){
		Entity_quest_mst questMst = Resources.Load ("Data/quest_mst") as Entity_quest_mst;
		bool daily = questMst.param[id].daily;
		return daily;
	}




}
