using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.SceneManagement;

public class DoSoubujirei : MonoBehaviour {

	public GameObject board;
	public GameObject back;
	public GameObject confirm;
	public AudioSource[] audioSources;
    public GameObject SoubujiOK;
    public GameObject SoubujiNG;
    public bool testMode = false;

	public void OnClick(){
        Daimyo daimyo = new Daimyo();
        audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (name == "YesButton") {
			//Hyourou
			int hyourou = PlayerPrefs.GetInt ("hyourou");
			int newHyourou = hyourou - 30;
			PlayerPrefs.SetInt ("hyourou", newHyourou);
			GameObject.Find ("HyourouCurrentValue").GetComponent<Text> ().text = newHyourou.ToString ();

			//Soubuji Flg
			PlayerPrefs.SetBool ("soubujireiFlg", true);

			//Listup
			string seiryoku = PlayerPrefs.GetString ("seiryoku");
			List<string> seiryokuList = new List<string> ();
			char[] delimiterChars = { ',' };
			seiryokuList = new List<string> (seiryoku.Split (delimiterChars));

			MainStageController script = GameObject.Find ("GameController").GetComponent<MainStageController> ();
			int myKuniQty = script.myKuniQty;
			int myDaimyo = script.myDaimyo;

			List<int> daimyoIdList = new List<int> ();
			List<int> mainKuniIdList = new List<int> ();
			List<int> kuniQtyByDaimyoId = new List<int> () {
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			for (int i = 0; i < seiryokuList.Count; i++) {
				int daimyoId = int.Parse (seiryokuList [i]);
				if (daimyoId != myDaimyo) {
					int kuniId = i + 1;

					if (!daimyoIdList.Contains (daimyoId)) {
						daimyoIdList.Add (daimyoId);
						mainKuniIdList.Add (kuniId);
					}
					kuniQtyByDaimyoId [daimyoId - 1] = kuniQtyByDaimyoId [daimyoId - 1] + 1;
				}
			}


			//Do Soubujirei
			List<string> newSeiryokuList = new List<string> (seiryokuList);
			string clearedKuni = PlayerPrefs.GetString ("clearedKuni");
			KuniInfo kuni = new KuniInfo ();
			GameObject kuniIconView = GameObject.Find ("KuniIconView").gameObject;
			GameObject panel = GameObject.Find ("Panel").gameObject;
			GameObject kuniMap = GameObject.Find ("KuniMap").gameObject;
			Doumei doumei = new Doumei ();
			bool allClearedFlg = true;

			for (int k = 0; k < daimyoIdList.Count; k++) {
				int daimyoId = daimyoIdList [k];
				string daimyoName = daimyo.getName (daimyoId);
				int kuniQty = 0;
				kuniQty = kuniQtyByDaimyoId [daimyoId - 1];

				float ratio = 0;
				ratio =	(100 - ((float)kuniQty / (float)myKuniQty * 500 ));
                
                if (ratio<0) {
                    ratio = 0;
                }
                if(testMode) {
                    ratio = 100;
                }


				//Debug.Log (daimyoName+":"+kuniQty +"/"+myKuniQty+"="+ratio);
				float percent = UnityEngine.Random.value;
				percent = percent * 100;
                if (percent <= ratio) {
					//OK
					for (int j = 0; j < seiryokuList.Count; j++) {
						if (seiryokuList [j] == daimyoId.ToString ()) {
							int kuniId = j + 1;

							//1.update seiryoku
							newSeiryokuList [kuniId - 1] = myDaimyo.ToString ();

							//2.update cleaered kuni & kuni1,2,3
							clearedKuni = clearedKuni + "," + kuniId.ToString ();
							string tmp = "kuni" + kuniId.ToString ();
							PlayerPrefs.SetString (tmp, "1,2,3,4,5,6,7,8,9,10");

							//3.update openkuni
							kuni.registerOpenKuni (kuniId);

							//4.cyouhou delete
							//Cyouhou Delete
							string cyouhouTmp = "cyouhou" + kuniId;
							if (PlayerPrefs.HasKey (cyouhouTmp)) {
								PlayerPrefs.DeleteKey (cyouhouTmp);
								string cyouhou = PlayerPrefs.GetString ("cyouhou");
								List<string> cyouhouList = new List<string> ();
								if (cyouhou != null && cyouhou != "") {
									if (cyouhou.Contains (",")) {
										cyouhouList = new List<string> (cyouhou.Split (delimiterChars));
									} else {
										cyouhouList.Add (cyouhou);
									}
								}
								cyouhouList.Remove (kuniId.ToString ());
								string newCyouhou = "";
								for (int a = 0; a < cyouhouList.Count; a++) {
									if (a == 0) {
										newCyouhou = cyouhouList [a];
									} else {
										newCyouhou = newCyouhou + "," + cyouhouList [a];
									}
								}
								PlayerPrefs.SetString ("cyouhou", newCyouhou);
							}

                            //5. naisei
                            string naiseiTmp = "naisei" + kuniId;
                            if(!PlayerPrefs.HasKey(naiseiTmp)) {
                                PlayerPrefs.SetString(naiseiTmp, "1,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0");
                            }

                            //Icon Change
                            IconMapValueUpdate (kuniId, myDaimyo, kuniIconView, kuniMap, myKuniQty);
						}
					}
					//Gunzei Check
					foreach (GameObject GunzeiObj in  GameObject.FindGameObjectsWithTag("Gunzei")) {
						int gunzeiSrcDaimyoId = GunzeiObj.GetComponent<Gunzei> ().srcDaimyoId;
						if (daimyoId == gunzeiSrcDaimyoId) {
							deleteGunzei (GunzeiObj);
						}
					}

					//Message
					int msgKuniId = mainKuniIdList [k];
					okNgMessage (true, msgKuniId, kuniIconView, panel, daimyoName);


				} else {
					//NG

					allClearedFlg = false;

					//Yukoudo 0
					string tempGaikou = "gaikou" + daimyoId;
					PlayerPrefs.SetInt (tempGaikou, 0);

					//doumei clear
					bool doumeiFlg = doumei.myDoumeiExistCheck (daimyoId);
					if (doumeiFlg) {
						doumei.deleteDoumeiMyDaimyo (myDaimyo.ToString (), daimyoId.ToString ());
					}

					//Icon color & value
					kuni.deleteDoumeiKuniIcon (daimyoId);

					//Message
					int msgKuniId = mainKuniIdList [k];
					okNgMessage (false, msgKuniId, kuniIconView, panel, daimyoName);
				}
			}

			string newSeiryoku = "";
			for (int l = 0; l < newSeiryokuList.Count; l++) {
				if (l == 0) {
					newSeiryoku = newSeiryokuList [l];
				} else {
					newSeiryoku = newSeiryoku + "," + newSeiryokuList [l];
				}
			}
			PlayerPrefs.SetString ("seiryoku", newSeiryoku);
			PlayerPrefs.SetString ("clearedKuni", clearedKuni);
			PlayerPrefs.Flush ();


			//Update OpenKuniIcon
			kuni.updateOpenKuni ();

			//Close
			board.transform.FindChild ("close").GetComponent<CloseBoard> ().onClick ();
			soubujiFlgOn (kuniIconView);

			if (allClearedFlg) {
				GameObject.Find ("GameController").GetComponent<MainStageController> ().gameClearFlg = true;
				PlayerPrefs.SetBool ("gameClearFlg", true);
				PlayerPrefs.Flush ();
			}
		} else {
			audioSources [1].Play ();
		}

		Destroy (back.gameObject);
		Destroy (confirm.gameObject);

	}
		
	public void okNgMessage(bool okFlg, int kuniId,GameObject kuniIconView, GameObject panel,string daimyoName){

		
		if (okFlg) {
			audioSources [3].Play ();
            SoubujiOK = Instantiate(SoubujiOK) as GameObject;
            SoubujiOK.transform.SetParent(panel.transform);
            SoubujiOK.transform.localScale = new Vector2(2, 2);

            Vector2 kuniIconPosition = kuniIconView.transform.FindChild(kuniId.ToString()).transform.localPosition;
            SoubujiOK.transform.localPosition = kuniIconPosition;
            SoubujiOK.transform.FindChild("Name").GetComponent<Text>().text = daimyoName;
        }else {
			audioSources [4].Play ();
            SoubujiNG = Instantiate(SoubujiNG) as GameObject;
            SoubujiNG.transform.SetParent(panel.transform);
            SoubujiNG.transform.localScale = new Vector2(2, 2);

            Vector2 kuniIconPosition = kuniIconView.transform.FindChild(kuniId.ToString()).transform.localPosition;
            SoubujiNG.transform.localPosition = kuniIconPosition;
            SoubujiNG.transform.FindChild("Name").GetComponent<Text>().text = daimyoName;
        }
		
		

	}


	public void IconMapValueUpdate(int kuniId, int changeDaimyoId, GameObject kuniIconView, GameObject kuniMap, int myKuniQty){
        Daimyo daimyo = new Daimyo();
        GameObject targetKuniIcon = kuniIconView.transform.FindChild (kuniId.ToString ()).gameObject;
		targetKuniIcon.gameObject.AddComponent<IconFadeChange> ();
		targetKuniIcon.gameObject.AddComponent<IconFadeChange> ().toDaimyoId = changeDaimyoId;

		float colorR = daimyo.getColorR (changeDaimyoId);
		float colorG = daimyo.getColorG (changeDaimyoId);
		float colorB = daimyo.getColorB (changeDaimyoId);
		Color newColor = new Color (colorR / 255f, colorG / 255f, colorB / 255f, 255f / 255f);

		GameObject kuniMapObj = kuniMap.transform.FindChild (kuniId.ToString ()).gameObject;
		kuniMapObj.GetComponent<Image> ().color = newColor;

		//Change Name of target Kuni by daimyo info
		SendParam script = targetKuniIcon.GetComponent<SendParam> ();
		script.daimyoId = changeDaimyoId;
		string daimyoName = daimyo.getName (changeDaimyoId);
		script.daimyoName = daimyoName;
		int daimyoBusyoId = daimyo.getDaimyoBusyoId (changeDaimyoId);
		script.daimyoBusyoId = daimyoBusyoId;
		myKuniQty = myKuniQty + 1;
		script.kuniQty = myKuniQty;
		script.openFlg = true;
		script.clearFlg = true;
		int myHeiryoku = PlayerPrefs.GetInt ("jinkeiHeiryoku");
		script.heiryoku = myHeiryoku;
	}

	public void deleteGunzei(GameObject GunzeiObj){
		Destroy(GunzeiObj);

		//Delete Key
		string gunzeiKey = GunzeiObj.name;
		PlayerPrefs.DeleteKey(gunzeiKey);

		//Delete Key History
		char[] delimiterChars = {','};
		string keyHistory = PlayerPrefs.GetString ("keyHistory");
		List<string> keyHistoryList = new List<string>();
		if (keyHistory != null && keyHistory != "") {
			if(keyHistory.Contains(",")){
				keyHistoryList = new List<string> (keyHistory.Split (delimiterChars));
			}else{
				keyHistoryList.Add(keyHistory);
			}
		}
		keyHistoryList.Remove(gunzeiKey);
		string newKeyHistory = "";
		for(int i=0; i<keyHistoryList.Count; i++){
			if(i==0){
				newKeyHistory = keyHistoryList[i];
			}else{
				newKeyHistory = newKeyHistory + "," + keyHistoryList[i];
			}
		}
		PlayerPrefs.SetString("keyHistory",newKeyHistory);
		PlayerPrefs.Flush ();
	}

	public void soubujiFlgOn(GameObject kuniIconView){
		foreach (Transform icon in kuniIconView.transform) {
			icon.GetComponent<SendParam> ().soubujireiFlg = true;
		}
	}

}
