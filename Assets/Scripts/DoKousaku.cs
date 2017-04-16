using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class DoKousaku : MonoBehaviour {

	public bool linkCutFlg = false;
	public GameObject scrollObj;
	public int cyouhouSnbRankId = 0;
	public int activeKuniId = 0;
	public int activeStageId = 0;
	public int busyoId = 0;
	public float dfc = 0;
	public float okRatio = 0;

	public void OnClick(){

		int nowHyourou = PlayerPrefs.GetInt ("hyourou");
		Message msg = new Message ();
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (name == "Back") {
			scrollObj.SetActive (false);
			audioSources [1].Play ();

		} else {

			if (nowHyourou < 2) {
				msg.makeUpperMessageOnBoard (msg.getMessage(7));

			} else {
				//Track
				int TrackBouryakuNo = PlayerPrefs.GetInt("TrackBouryakuNo",0);
				TrackBouryakuNo = TrackBouryakuNo + 1;
				PlayerPrefs.SetInt ("TrackBouryakuNo", TrackBouryakuNo);


				nowHyourou = nowHyourou - 2;
				PlayerPrefs.SetInt ("hyourou",nowHyourou);
				GameObject.Find ("HyourouCurrentValue").GetComponent<Text> ().text = nowHyourou.ToString ();

				//Do
				StartKassen script = GameObject.Find ("BattleButton").GetComponent<StartKassen> ();
				activeKuniId = script.activeKuniId;
				activeStageId = script.activeStageId;

				DoGaikou addUsedBusyo = new DoGaikou ();


				if (linkCutFlg) {
					//link cut

					//Make target list
					string temp = "kuniMap" + activeKuniId;
					GameObject kuniMap = GameObject.Find (temp).gameObject;

					List<string> targetList = new List<string> ();
					foreach (Transform obj in kuniMap.transform) {

						if (obj.tag == "Link") {
							string objName = obj.name;
							objName = objName.Replace ("link", "");

							List<string> linkList = new List<string> ();
							char[] delimiterChars = { '-' };
							linkList = new List<string> (objName.Split (delimiterChars));

							if (int.Parse (linkList [0]) == activeStageId) {
								targetList.Add (objName);
							} else if (int.Parse (linkList [1]) == activeStageId) {
								targetList.Add (objName);
							}
						}
					}



					okRatio = ((float)cyouhouSnbRankId * (float)dfc) / 8;
					if (targetList.Count != 0) {

						int okCount = 0;
						bool okMsgFlg = false;
						for (int i = 0; i < targetList.Count; i++) {

							//success or not
							float percent = UnityEngine.Random.value;
							percent = percent * 100;

							if (percent <= okRatio) {
								//Track
								int TrackBouryakuSuccessNo = PlayerPrefs.GetInt("TrackBouryakuSuccessNo",0);
								TrackBouryakuSuccessNo = TrackBouryakuSuccessNo + 1;
								PlayerPrefs.SetInt ("TrackBouryakuSuccessNo", TrackBouryakuSuccessNo);

								//OK Cut
								okMsgFlg = true;
								string target = "link" + targetList [i];
								GameObject targetObj = kuniMap.transform.FindChild (target).gameObject;
								Destroy (targetObj.gameObject);


								//Register Data
								string tempLinkuct = "linkcut" + activeKuniId;
								string linkcut = PlayerPrefs.GetString (tempLinkuct);
								string newLinkcut = "";
								if (linkcut != null && linkcut != "") {
									newLinkcut = linkcut + "," + targetList [i]; 
								} else {
									newLinkcut = targetList [i]; 
								}
								PlayerPrefs.SetString (tempLinkuct, newLinkcut);

								PlayerPrefs.SetBool ("questDailyFlg34", true);
								PlayerPrefs.Flush ();

								MainStageController mainStage = new MainStageController ();
								mainStage.questExtension ();

								//Reduce Cut No.
								List<string> linkList = new List<string> ();
								char[] delimiterChars = { '-' };
								linkList = new List<string> (targetList [i].Split (delimiterChars));

								string stage1 = "stage" + linkList [0];
								string stage2 = "stage" + linkList [1];

								foreach (Transform obj in kuniMap.transform) {
									if (obj.tag != "Link") {
										if (obj.name == stage1 || obj.name == stage2) {
											obj.GetComponent<ShowStageDtl> ().linkNo = obj.GetComponent<ShowStageDtl> ().linkNo - 1;
										}
									}
								}

								okCount = okCount + 1;

							} 
						}
						if (okMsgFlg) {
							int TrackLinkCutNo = PlayerPrefs.GetInt("TrackLinkCutNo",0);
							TrackLinkCutNo = TrackLinkCutNo + okCount;
							PlayerPrefs.SetInt ("TrackLinkCutNo", TrackLinkCutNo);
							PlayerPrefs.Flush ();
                            if (Application.systemLanguage != SystemLanguage.Japanese) {
                                msg.makeUpperMessageOnBoard ("You cut " + okCount + " line. \n Enemy power keep be down in this season.");
                            }else {
                                msg.makeUpperMessageOnBoard("連絡線を" + okCount + "本遮断しました。\n今季節中は敵戦力が減りますぞ。");
                            }
							addUsedBusyo.addUsedBusyo (busyoId);
						} else {
							msg.makeUpperMessageOnBoard (msg.getMessage(126)); 
						}

					} else {
						
						msg.makeUpperMessageOnBoard (msg.getMessage(127));
					}


				} else {
					//cyouryaku
					okRatio = ((float)cyouhouSnbRankId * (float)dfc) / 8;

					float percent = UnityEngine.Random.value;
					percent = percent * 100;

					if (percent <= okRatio) {
						//OK
						//Track
						int TrackBouryakuSuccessNo = PlayerPrefs.GetInt("TrackBouryakuSuccessNo",0);
						TrackBouryakuSuccessNo = TrackBouryakuSuccessNo + 1;
						PlayerPrefs.SetInt ("TrackBouryakuSuccessNo", TrackBouryakuSuccessNo);

						string cyouryaku = PlayerPrefs.GetString ("cyouryaku");
						string cyouryakuTmp = activeKuniId.ToString () + "cyouryaku" + activeStageId.ToString ();
						if (cyouryaku != null && cyouryaku != "") {
							cyouryaku = cyouryaku + "," + cyouryakuTmp;
						} else {
							cyouryaku = cyouryakuTmp;
						}
						PlayerPrefs.SetString ("cyouryaku", cyouryaku);


						int cyouryakuHeiQty = 0;
						if (cyouhouSnbRankId == 1) {
							cyouryakuHeiQty = UnityEngine.Random.Range (1, 3);
						} else if (cyouhouSnbRankId == 2) {
							cyouryakuHeiQty = UnityEngine.Random.Range (1, 4);
						} else if (cyouhouSnbRankId == 3) {
							cyouryakuHeiQty = UnityEngine.Random.Range (1, 6);
						}

						PlayerPrefs.SetInt (cyouryakuTmp, cyouryakuHeiQty);
						PlayerPrefs.SetBool ("questDailyFlg35", true);

						int TrackCyouryakuNo = PlayerPrefs.GetInt("TrackCyouryakuNo",0);
						TrackCyouryakuNo = TrackCyouryakuNo + cyouryakuHeiQty;
						PlayerPrefs.SetInt ("TrackCyouryakuNo", TrackCyouryakuNo);
						PlayerPrefs.Flush ();

						MainStageController mainStage = new MainStageController ();
						mainStage.questExtension ();
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            msg.makeUpperMessageOnBoard ("You are succeed to win over. \n Enemy " + cyouryakuHeiQty.ToString () + " unit will change to player unit in a battle.");
                        }else {
                            msg.makeUpperMessageOnBoard("調略に成功しましたぞ。\n合戦にて" + cyouryakuHeiQty.ToString() + "部隊が寝返ります。");
                        }
						addUsedBusyo.addUsedBusyo (busyoId);

					} else {
						msg.makeUpperMessageOnBoard (msg.getMessage(128));
					}

				}
					
				//Push Back Button
				scrollObj.SetActive (false);
			}
		}

	}
}
