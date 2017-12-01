using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class KousakuConfirm : MonoBehaviour {

	public int cyouhouSnbRankId = 0;
	public GameObject scrollObj;


	public void OnClick(){
        
        //Common
        int langId = PlayerPrefs.GetInt("langId");
        Message msg = new Message();
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		//Check Shinobi
		if (cyouhouSnbRankId != 0) {
			char[] delimiterChars = { ',' };
			List<string> myBusyoList = new List<string> ();
			string myBusyoString = PlayerPrefs.GetString ("myBusyo");

			if (myBusyoString.Contains (",")) {
				myBusyoList = new List<string> (myBusyoString.Split (delimiterChars));
			} else {
				myBusyoList.Add (myBusyoString);
			}

			//reduce used busyo
			List<string> usedBusyoList = new List<string>();
			string usedBusyo = PlayerPrefs.GetString ("usedBusyo");
			if (usedBusyo != null && usedBusyo != "") {
				usedBusyoList = new List<string> (usedBusyo.Split (delimiterChars));
				myBusyoList.RemoveAll (usedBusyo.Contains);
			}

			//Check Busyo
			if(myBusyoList.Count != 0){

				audioSources [0].Play();

				scrollObj.SetActive (true);
				scrollObj.transform.Find ("Back").GetComponent<DoKousaku> ().scrollObj = scrollObj;
				GameObject doBtnObj = scrollObj.transform.Find ("Do").gameObject;
				doBtnObj.GetComponent<DoKousaku> ().scrollObj = scrollObj;

				//Make Scroll
				GameObject content = scrollObj.transform.Find("ScrollView").transform.Find("Content").gameObject;
                foreach (Transform obj in content.transform) {
					Destroy (obj.gameObject);
				}

                //Sort by Rank
                List<Busyo> baseBusyoList = new List<Busyo>();
                BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();
                StatusGet sts = new StatusGet();
                foreach (string busyoIdString in myBusyoList) {
                    int busyoId = int.Parse(busyoIdString);
                    string busyoName = BusyoInfoGet.getName(busyoId,langId);
                    string rank = BusyoInfoGet.getRank(busyoId);
                    int lv = 1;
                    float dfcSts = (float)sts.getDfc(busyoId, lv);
                    float hpSts = (float)sts.getHp(busyoId, lv);
                    float atkSts = (float)sts.getAtk(busyoId, lv);
                    baseBusyoList.Add(new Busyo(busyoId, busyoName, rank,0, "", 0, 0, lv, hpSts, atkSts, dfcSts, 0,0,0));
                }
                List<Busyo> myBusyoSortList = new List<Busyo>(baseBusyoList);
                myBusyoSortList.Sort((a, b) => {
                    float result = b.dfc - a.dfc;
                    return (int)result;
                });


                string slotPath = "Prefabs/Map/kousaku/Slot";
				string dfcPath = "Prefabs/Map/kousaku/TextObj";
                bool firstFlg = false;
                foreach (Busyo Busyo in myBusyoSortList) {

					//Slot
					GameObject slot = Instantiate (Resources.Load (slotPath)) as GameObject;
					slot.transform.SetParent(content.transform);
					slot.transform.localScale = new Vector3 (1, 1, 1);

					//Busyo
					string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
					GameObject busyo = Instantiate (Resources.Load (busyoPath)) as GameObject;
					busyo.transform.SetParent(slot.transform);
					busyo.transform.localScale = new Vector3 (4, 5, 0);
					busyo.name = Busyo.busyoId.ToString();
					slot.name = "Slot" + busyo.name;

					busyo.GetComponent<DragHandler> ().enabled = false;			

					//Chiryaku
					GameObject txtObj = Instantiate (Resources.Load (dfcPath)) as GameObject;
					txtObj.transform.SetParent(busyo.transform);
					txtObj.transform.localScale = new Vector3 (1, 1, 0);
					txtObj.transform.localPosition = new Vector3 (5, -12, 0);

					float chiryakuSts = Busyo.dfc *10;
					txtObj.transform.Find ("Value").GetComponent<Text> ().text = chiryakuSts.ToString ();

					//Set Param
					KousakuBusyoSelect script = slot.GetComponent<KousakuBusyoSelect>();
                    script.busyoId = Busyo.busyoId;
					script.dfc = chiryakuSts;
					script.doBtnObj = doBtnObj;
					script.content = content;

					//Initial Setting
                    if (!firstFlg) {
                        slot.GetComponent<KousakuBusyoSelect>().OnClick();
                        firstFlg = true;
                    }


                }

                scrollObj.transform.Find ("Do").GetComponent<DoKousaku> ().cyouhouSnbRankId = cyouhouSnbRankId;
				if (name == "CyouryakuButton") {
                    scrollObj.transform.Find("Question").GetComponent<QA>().qaId = 31;                    
                    if (langId == 2) {
                        scrollObj.transform.Find("Text").GetComponent<Text>().text = "Win Over";
                        scrollObj.transform.Find("Do").transform.Find("Text").GetComponent<Text>().text = "Win Over";
                    }else {
                        scrollObj.transform.Find("Text").GetComponent<Text>().text = "調略";
                        scrollObj.transform.Find("Do").transform.Find("Text").GetComponent<Text>().text = "調略";
                    }
                    doBtnObj.GetComponent<DoKousaku>().linkCutFlg = false;
                } else {
                    scrollObj.transform.Find("Question").GetComponent<QA>().qaId = 30;
                    if (langId == 2) {
                        scrollObj.transform.Find("Text").GetComponent<Text>().text = "Link Cut";
                        scrollObj.transform.Find("Do").transform.Find("Text").GetComponent<Text>().text = "Cut";
                    }else {
                        scrollObj.transform.Find("Text").GetComponent<Text>().text = "連絡線遮断";
                        scrollObj.transform.Find("Do").transform.Find("Text").GetComponent<Text>().text = "遮断";
                    }
					doBtnObj.GetComponent<DoKousaku> ().linkCutFlg = true;
				}

			}else{
				audioSources [4].Play();
				msg.makeUpperMessageOnBoard (msg.getMessage(8));
			}

		} else {
			audioSources [4].Play();
			msg.makeUpperMessageOnBoard (msg.getMessage(51));
		}
	}
}
