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
				scrollObj.transform.FindChild ("Back").GetComponent<DoKousaku> ().scrollObj = scrollObj;
				GameObject doBtnObj = scrollObj.transform.FindChild ("Do").gameObject;
				doBtnObj.GetComponent<DoKousaku> ().scrollObj = scrollObj;

				//Make Scroll
				GameObject content = scrollObj.transform.FindChild("ScrollView").transform.FindChild("Content").gameObject;

				foreach (Transform obj in content.transform) {
					Destroy (obj.gameObject);
				}


				string slotPath = "Prefabs/Map/kousaku/Slot";
				string dfcPath = "Prefabs/Map/kousaku/TextObj";
				for(int i=0; i<myBusyoList.Count; i++){

					string busyoId = myBusyoList [i];

					//Slot
					GameObject slot = Instantiate (Resources.Load (slotPath)) as GameObject;
					slot.transform.SetParent(content.transform);
					slot.transform.localScale = new Vector3 (1, 1, 1);

					//Busyo
					string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
					GameObject busyo = Instantiate (Resources.Load (busyoPath)) as GameObject;
					busyo.name = busyoId;
					busyo.transform.SetParent(slot.transform);
					busyo.transform.localScale = new Vector3 (4, 5, 0);
					busyo.name = busyoId;
					slot.name = "Slot" + busyo.name;

					busyo.GetComponent<DragHandler> ().enabled = false;			

					//Chiryaku
					GameObject txtObj = Instantiate (Resources.Load (dfcPath)) as GameObject;
					txtObj.transform.SetParent(busyo.transform);
					txtObj.transform.localScale = new Vector3 (1, 1, 0);
					txtObj.transform.localPosition = new Vector3 (5, -12, 0);

					StatusGet sts = new StatusGet();
					int lv = PlayerPrefs.GetInt (busyoId);
					float chiryakuSts = (float)sts.getDfc(int.Parse(busyoId),lv);
					chiryakuSts = chiryakuSts *10;

					txtObj.transform.FindChild ("Value").GetComponent<Text> ().text = chiryakuSts.ToString ();

					//Set Param
					KousakuBusyoSelect script = slot.GetComponent<KousakuBusyoSelect>();
					script.busyoId = int.Parse(busyoId);
					script.dfc = chiryakuSts;
					script.doBtnObj = doBtnObj;
					script.content = content;

					//Initial Setting
					if (i == 0) {
						slot.GetComponent<KousakuBusyoSelect> ().OnClick ();
					}


				}

				scrollObj.transform.FindChild ("Do").GetComponent<DoKousaku> ().cyouhouSnbRankId = cyouhouSnbRankId;
				if (name == "CyouryakuButton") {
                    scrollObj.transform.FindChild("Question").GetComponent<QA>().qaId = 31;
                    if (Application.systemLanguage == SystemLanguage.Japanese) {
                        scrollObj.transform.FindChild ("Text").GetComponent<Text> ().text = "調略";
					    scrollObj.transform.FindChild ("Do").transform.FindChild ("Text").GetComponent<Text> ().text = "調略";					    
                    }else {
                        scrollObj.transform.FindChild("Text").GetComponent<Text>().text = "Win Over";
                        scrollObj.transform.FindChild("Do").transform.FindChild("Text").GetComponent<Text>().text = "Win Over";
                    }
                    doBtnObj.GetComponent<DoKousaku>().linkCutFlg = false;
                } else {
                    scrollObj.transform.FindChild("Question").GetComponent<QA>().qaId = 30;

                    if (Application.systemLanguage == SystemLanguage.Japanese) {
                        scrollObj.transform.FindChild ("Text").GetComponent<Text> ().text = "連絡線遮断";
                        scrollObj.transform.FindChild("Do").transform.FindChild("Text").GetComponent<Text>().text = "遮断";
                    } else{
                        scrollObj.transform.FindChild("Text").GetComponent<Text>().text = "Link Cut";
                        scrollObj.transform.FindChild("Do").transform.FindChild("Text").GetComponent<Text>().text = "Cut";
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
