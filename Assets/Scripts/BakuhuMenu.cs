using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class BakuhuMenu : MonoBehaviour {

	public GameObject board;
	public GameObject scrollView;
	public int daimyoId = 0;
	public string daimyoName = "";
	public int kuniId = 0;
	public int hyourouNo = 0;

    //Soujibujire
    public GameObject soubujireiConfirm;

    void Start() {
        string path = "Prefabs/Bakuhu/SoubujireiConfirm";
        soubujireiConfirm = Resources.Load(path) as GameObject;
    }


    public void OnClick(){

		int hyourou = PlayerPrefs.GetInt ("hyourou");
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
        int langId = PlayerPrefs.GetInt("langId");
        int senarioId = PlayerPrefs.GetInt("senarioId");

        if (name == "AtkOrderBtn") {

			if (hyourou >= 10) {
				audioSources [0].Play ();

				//Common
				//Back
				string returnPath = "Prefabs/Bakuhu/Return";
				GameObject returnObj = Instantiate (Resources.Load (returnPath)) as GameObject;
				returnObj.transform.SetParent (board.transform);
				returnObj.transform.localScale = new Vector2 (1, 1);
				returnObj.transform.localPosition = new Vector2 (-560, 290);
				returnObj.name = "bakuhuReturn";

				//Disabled
				scrollView.SetActive (false);

				//Base Obj
				string basePath = "Prefabs/Bakuhu/Base";
				GameObject baseObj = Instantiate (Resources.Load (basePath)) as GameObject;
				baseObj.transform.SetParent (board.transform);
				baseObj.transform.localScale = new Vector2 (1, 1);
				baseObj.name = "BakuhuBase";
				returnObj.GetComponent<BakuhuMenuReturn> ().deleteObj = baseObj;
				returnObj.GetComponent<BakuhuMenuReturn> ().scrollView = scrollView;
				returnObj.GetComponent<BakuhuMenuReturn> ().board = board;                
                if (langId == 2) {
                    board.transform.Find ("popText").GetComponent<Text> ().text = "Attack Order";
                }else {
                    board.transform.Find("popText").GetComponent<Text>().text = "討伐令";
                }
				string textPath = "Prefabs/Bakuhu/ToubatsuText";
				GameObject textObj = Instantiate (Resources.Load (textPath)) as GameObject;
				textObj.transform.SetParent (baseObj.transform);
				textObj.transform.localScale = new Vector2 (0.12f, 0.15f);
				textObj.transform.localPosition = new Vector2 (0, 206);
				textObj.name = "ToubatsuText";

				//View daimyo kuni
				string seiryoku = PlayerPrefs.GetString ("seiryoku");
				List<string> seiryokuList = new List<string> ();
				char[] delimiterChars = { ',' };
				seiryokuList = new List<string> (seiryoku.Split (delimiterChars));

				string kuniPath = "Prefabs/Map/Kuni/";
				Entity_kuni_mst kuniMst = Resources.Load ("Data/kuni_mst") as Entity_kuni_mst;
				Entity_daimyo_mst daimyoMst = Resources.Load ("Data/daimyo_mst") as Entity_daimyo_mst;
				int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");

				string kuniIconPath = "Prefabs/Bakuhu/BakuhuKuniMap";
				GameObject kuniMapView = Instantiate (Resources.Load (kuniIconPath)) as GameObject;
				kuniMapView.transform.SetParent (baseObj.transform);
				kuniMapView.transform.localScale = new Vector2 (0.8f, 0.65f);

				string kuniMapPath = "Prefabs/Bakuhu/BakuhuKuniIconView";
				GameObject kuniIconView = Instantiate (Resources.Load (kuniMapPath)) as GameObject;
				kuniIconView.transform.SetParent (baseObj.transform);
				kuniIconView.transform.localScale = new Vector2 (0.8f, 0.65f);
				kuniIconView.name = "BakuhuKuniIconView";

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
				int myKuniQty = GameObject.Find ("GameController").GetComponent<MainStageController> ().myKuniQty;
				bool myKuniQtyIsBiggestFlg = true;

				for (int i = 0; i < kuniMst.param.Count; i++) {
					int kuniId = kuniMst.param [i].kunId;

					string newKuniPath = kuniPath + kuniId.ToString ();
					int locationX = kuniMst.param [i].locationX;
					int locationY = kuniMst.param [i].locationY;

					GameObject kuni = Instantiate (Resources.Load (newKuniPath)) as GameObject;
					kuni.transform.SetParent (kuniIconView.transform);
					kuni.name = kuniId.ToString ();
					kuni.transform.localScale = new Vector2 (1, 1);

					//Seiryoku Handling
					int daimyoId = int.Parse (seiryokuList [kuniId - 1]);
					if (daimyoId == myDaimyo) {
						kuni.SetActive (false);
						float colorR = (float)daimyoMst.param [daimyoId - 1].colorR;
						float colorG = (float)daimyoMst.param [daimyoId - 1].colorG;
						float colorB = (float)daimyoMst.param [daimyoId - 1].colorB;
						Color kuniColor = new Color (colorR / 255f, colorG / 255f, colorB / 255f, 255f / 255f);

						kuniMapView.transform.Find (kuni.name).GetComponent<Image> ().color = kuniColor;
					} else {

                        Daimyo daimyoScript = new Daimyo();
                        string daimyoName = daimyoScript.getName(daimyoId,langId,senarioId);
                        kuni.GetComponent<SendParam> ().bakuhuFlg = true;
						kuni.GetComponent<SendParam> ().kuniId = kuniId;
						kuni.GetComponent<SendParam> ().daimyoId = daimyoId;
						kuni.GetComponent<SendParam> ().daimyoName = daimyoName;
						int daimyoBusyoIdTemp = daimyoMst.param [daimyoId - 1].busyoId;
						kuni.GetComponent<SendParam> ().daimyoBusyoId = daimyoBusyoIdTemp;

						/*Kuni Qty Start*/
						int kuniQty = 0;
						if (kuniQtyByDaimyoId [daimyoId - 1] == 0) {
							kuniQty = getKuniQty (daimyoId, seiryokuList);
							kuniQtyByDaimyoId [daimyoId - 1] = kuniQty; 
						} else {
							kuniQty = kuniQtyByDaimyoId [daimyoId - 1];
						}
						kuni.GetComponent<SendParam> ().kuniQty = kuniQty;
						if (kuniQty >= myKuniQty) {
							myKuniQtyIsBiggestFlg = false;
						}
						/*Kuni Qty End*/

						//Color Handling
						float colorR = (float)daimyoMst.param [daimyoId - 1].colorR;
						float colorG = (float)daimyoMst.param [daimyoId - 1].colorG;
						float colorB = (float)daimyoMst.param [daimyoId - 1].colorB;
						Color kuniColor = new Color (colorR / 255f, colorG / 255f, colorB / 255f, 255f / 255f);

						kuniMapView.transform.Find (kuni.name).GetComponent<Image> ().color = kuniColor;

						//Daimyo Kamon Image
						string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString ();
						kuni.GetComponent<Image> ().sprite = 
							Resources.Load (imagePath, typeof(Sprite)) as Sprite;

						RectTransform kuniTransform = kuni.GetComponent<RectTransform> ();
						kuniTransform.anchoredPosition = new Vector3 (locationX, locationY, 0);

					}
				}
				returnObj.GetComponent<BakuhuMenuReturn> ().myKuniQtyIsBiggestFlg = myKuniQtyIsBiggestFlg;
			} else {
				//Hyourou NG
				audioSources [4].Play ();

				Message msg = new Message ();
				//msg.makeMessageOnBoard (msg.getMessage(7));
                msg.hyourouMovieMessage();
            }

		} else if (name == "DfcOrderBtn") {
			//Boueirei
			if (hyourou >= 10) {
				

				//Gunzei Exist Check
				int bakuhuTobatsuDaimyoId = PlayerPrefs.GetInt ("bakuhuTobatsuDaimyoId");

				bool gunzeiFlg = false;
				KuniInfo kuni = new KuniInfo ();
				string seiryoku = PlayerPrefs.GetString ("seiryoku");
				List<string> seiryokuList = new List<string> ();
				char[] delimiterChars = { ',' };
				char[] delimiterChars2 = { ':' };
				seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
				int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");

                //Used Engun List
                List<string> dstEngunDaimyoIdList = new List<string>();
                foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Gunzei")) {
                    string dstEngunDaimyoId = obs.GetComponent<Gunzei>().dstEngunDaimyoId;
                    List<string> tmpDstEngunDaimyoIdList = new List<string>();
                    if (dstEngunDaimyoId != null && dstEngunDaimyoId != "") {
                        if (dstEngunDaimyoId.Contains(":")) {
                            tmpDstEngunDaimyoIdList = new List<string>(dstEngunDaimyoId.Split(delimiterChars2));
                        }
                        else {
                            tmpDstEngunDaimyoIdList.Add(dstEngunDaimyoId);
                        }
                    }
                    dstEngunDaimyoIdList.AddRange(tmpDstEngunDaimyoIdList);
                }


                List<string> okGunzeiList = new List<string> (); //key1:engunSrcKuni1:engunSrcKuni2...,key2:engunSrcKuni1:engunSrcKuni2....

				foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Gunzei")) {
					int checkDaimyoId = obs.GetComponent<Gunzei> ().dstDaimyoId;
					if (checkDaimyoId != bakuhuTobatsuDaimyoId) {

						int dstKuni = obs.GetComponent<Gunzei> ().dstKuni;
						int srcDaimyoId = obs.GetComponent<Gunzei> ().srcDaimyoId;
						int dstDaimyoId = obs.GetComponent<Gunzei> ().dstDaimyoId;
						List<int> openKuniList = new List<int> ();
						openKuniList = kuni.getMappingKuni (dstKuni);

						string tmpString = "";
						List<int> usedDaimyoList = new List<int> ();
						for (int i = 0; i < openKuniList.Count; i++) {
							int tmpDaimyoId = int.Parse (seiryokuList [openKuniList [i] - 1]);
							if (tmpDaimyoId != myDaimyo && tmpDaimyoId != srcDaimyoId && tmpDaimyoId != dstDaimyoId) {

								if (!usedDaimyoList.Contains (tmpDaimyoId)) {
									if (!dstEngunDaimyoIdList.Contains (tmpDaimyoId.ToString ())) {
										gunzeiFlg = true;
										usedDaimyoList.Add (tmpDaimyoId);

										if (tmpString == "") {
											tmpString = obs.name + ":" + openKuniList [i];
										} else {
											tmpString = tmpString + ":" + openKuniList [i];
										}
									}
								}
							}
						}
						if (tmpString != "") {
							okGunzeiList.Add (tmpString);
						}
					}
				}

				if (gunzeiFlg) {
					audioSources [0].Play ();

					//Common
					//Back
					string returnPath = "Prefabs/Bakuhu/Return";
					GameObject returnObj = Instantiate (Resources.Load (returnPath)) as GameObject;
					returnObj.transform.SetParent (board.transform);
					returnObj.transform.localScale = new Vector2 (1, 1);
					returnObj.transform.localPosition = new Vector2 (-560, 290);
					returnObj.name = "bakuhuReturn";                    
                    if (langId == 2) {
                        board.transform.Find ("popText").GetComponent<Text> ().text = "Defence Order";
                    }else {
                        board.transform.Find("popText").GetComponent<Text>().text = "防衛令";
                    }
					//Disabled
					scrollView.SetActive (false);

					//Base Obj
					string basePath = "Prefabs/Bakuhu/Base";
					GameObject baseObj = Instantiate (Resources.Load (basePath)) as GameObject;
					baseObj.transform.SetParent (board.transform);
					baseObj.transform.localScale = new Vector2 (1, 1);
					baseObj.name = "BakuhuBase";
					returnObj.GetComponent<BakuhuMenuReturn> ().deleteObj = baseObj;
					returnObj.GetComponent<BakuhuMenuReturn> ().scrollView = scrollView;
					returnObj.GetComponent<BakuhuMenuReturn> ().board = board;

					string boubiScrollPath = "Prefabs/Bakuhu/BoueiScrollView";
					GameObject boueiScrollObj = Instantiate (Resources.Load (boubiScrollPath)) as GameObject;
					boueiScrollObj.transform.SetParent (baseObj.transform);
					boueiScrollObj.transform.localScale = new Vector2 (1, 1);
                    boueiScrollObj.transform.localPosition = new Vector2(0, 0);
                    GameObject content = boueiScrollObj.transform.Find ("Content").gameObject;

					string uniSlotPath = "Prefabs/Bakuhu/BoueiSlot";
					string daimyoBusyoPath = "Prefabs/Player/Sprite/unit";
					Daimyo daimyo = new Daimyo ();
                    
                    for (int i = 0; i < okGunzeiList.Count; i++) {
						string tmp = okGunzeiList [i];
						List<string> okGunzeiUnitList = new List<string> ();
						okGunzeiUnitList = new List<string> (tmp.Split (delimiterChars2));

						string key = okGunzeiUnitList [0];
						GameObject gunzei = GameObject.Find (key).gameObject;
						int dstDaimyoId = gunzei.GetComponent<Gunzei> ().dstDaimyoId;
						int dstDaimyoBusyoId = daimyo.getDaimyoBusyoId (dstDaimyoId, senarioId);
						string dstDaimyoName = daimyo.getName (dstDaimyoId,langId, senarioId);
						int srcDaimyoId = gunzei.GetComponent<Gunzei> ().srcDaimyoId;
						int srcDaimyoBusyoId = daimyo.getDaimyoBusyoId (srcDaimyoId, senarioId);
						string srcDaimyoName = daimyo.getName (srcDaimyoId,langId, senarioId);
						int dstKuniId = gunzei.GetComponent<Gunzei> ().dstKuni;
						string kuniName = kuni.getKuniName (dstKuniId,langId);

						for (int j = 1; j < okGunzeiUnitList.Count; j++) {
							int engunKuniId = int.Parse (okGunzeiUnitList [j]);
							int engunDaimyoId = int.Parse (seiryokuList [engunKuniId - 1]);
							int engunDaimyoBusyoId = daimyo.getDaimyoBusyoId (engunDaimyoId,senarioId);
							string engunDaimyoName = daimyo.getName (engunDaimyoId,langId, senarioId);

							GameObject slot = Instantiate (Resources.Load (uniSlotPath)) as GameObject;
							slot.transform.SetParent (content.transform);
							slot.transform.localScale = new Vector2 (1, 1);                           
                            if (langId == 2) {
                                slot.transform.Find ("Kuni").GetComponent<Text> ().text = kuniName + " Defence";
                            }else {
                                slot.transform.Find("Kuni").GetComponent<Text>().text = kuniName + "防衛";
                            }
							string dfcDaimyoPath = daimyoBusyoPath + dstDaimyoBusyoId.ToString ();
							slot.transform.Find ("Dfc").transform.Find ("Image").GetComponent<Image> ().sprite = 
								Resources.Load (dfcDaimyoPath, typeof(Sprite)) as Sprite;
							string atkDaimyoPath = daimyoBusyoPath + srcDaimyoBusyoId.ToString ();
							slot.transform.Find ("Atk").transform.Find ("Image").GetComponent<Image> ().sprite = 
								Resources.Load (atkDaimyoPath, typeof(Sprite)) as Sprite;
							string engnDaimyoPath = daimyoBusyoPath + engunDaimyoBusyoId.ToString ();
							slot.transform.Find ("Egn").transform.Find ("Image").GetComponent<Image> ().sprite = 
								Resources.Load (engnDaimyoPath, typeof(Sprite)) as Sprite;

							slot.transform.Find ("DfcName").GetComponent<Text> ().text = dstDaimyoName;
							slot.transform.Find ("AtkName").GetComponent<Text> ().text = srcDaimyoName;
							slot.transform.Find ("EgnName").GetComponent<Text> ().text = engunDaimyoName;
                            if (langId == 2) {
                                slot.transform.Find("Exp").GetComponent<Text>().text = "Request " + engunDaimyoName + " for reinforcement";
                            }else {
                                slot.transform.Find("Exp").GetComponent<Text>().text = engunDaimyoName + "に援軍の出兵支持を出す。";
                            }
                            //Param
                            GameObject btn = slot.transform.Find ("BoueiBtn").gameObject;
							btn.GetComponent<DoBouei> ().key = key;
							btn.GetComponent<DoBouei> ().engunDaimyoId = engunDaimyoId;
							btn.GetComponent<DoBouei> ().engunKuniId = engunKuniId;
							btn.GetComponent<DoBouei> ().engunDaimyoName = engunDaimyoName;
							btn.GetComponent<DoBouei> ().dfcDaimyoId = dstDaimyoId;
							btn.GetComponent<DoBouei> ().slot = slot;
							btn.GetComponent<DoBouei> ().kuniName = kuniName;
						}
						
					}




				} else {
					audioSources [4].Play ();

					Message msg = new Message ();
					msg.makeMessageOnBoard (msg.getMessage(99));
				}
			} else {
				audioSources [4].Play ();

				//Hyourou NG
				Message msg = new Message ();
				//msg.makeMessageOnBoard (msg.getMessage(7));
                msg.hyourouMovieMessage();
            }

		} else if (name == "RelationshipBtn") {

			//Boueirei
			if (hyourou >= 5) {
				int bakuhuTobatsuDaimyoId = PlayerPrefs.GetInt ("bakuhuTobatsuDaimyoId");

				//Not Last Daimyo check
				string seiryoku = PlayerPrefs.GetString ("seiryoku");
				List<string> seiryokuList = new List<string> ();
				char[] delimiterChars = { ',' };
				seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
				int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");

				bool isLastDaimyoFlg = false;
				List<string> checkedDaimyoList = new List<string> ();
				foreach (string tmp in seiryokuList) {
					if (tmp != myDaimyo.ToString ()) {
						if (!checkedDaimyoList.Contains (tmp)) {
							checkedDaimyoList.Add (tmp);
						}
					}
				}
				//include other 2 daimyos = 3
				if (checkedDaimyoList.Count >= 2) {
					isLastDaimyoFlg = true;
				}

				if (isLastDaimyoFlg) {
					audioSources [0].Play ();

					//OK
					//Common
					//Back
					string returnPath = "Prefabs/Bakuhu/Return";
					GameObject returnObj = Instantiate (Resources.Load (returnPath)) as GameObject;
					returnObj.transform.SetParent (board.transform);
					returnObj.transform.localScale = new Vector2 (1, 1);
					returnObj.transform.localPosition = new Vector2 (-560, 290);
					returnObj.name = "bakuhuReturn";                    
                    if (langId == 2) {
                        board.transform.Find ("popText").GetComponent<Text> ().text = "Defence Order";
                    }else {
                        board.transform.Find("popText").GetComponent<Text>().text = "防衛令";
                    }
					//Disabled
					scrollView.SetActive (false);

					//Base Obj
					string basePath = "Prefabs/Bakuhu/Base";
					GameObject baseObj = Instantiate (Resources.Load (basePath)) as GameObject;
					baseObj.transform.SetParent (board.transform);
					baseObj.transform.localScale = new Vector2 (1, 1);
					baseObj.name = "BakuhuBase";
					returnObj.GetComponent<BakuhuMenuReturn> ().deleteObj = baseObj;
					returnObj.GetComponent<BakuhuMenuReturn> ().scrollView = scrollView;
					returnObj.GetComponent<BakuhuMenuReturn> ().board = board;
                    if (langId == 2) {
                        board.transform.Find ("popText").GetComponent<Text> ().text = "Mediation";
                    }else {
                        board.transform.Find("popText").GetComponent<Text>().text = "仲裁";
                    }
					//Scroll
					string scrollPath = "Prefabs/Bakuhu/CyusaiScrollView";
					GameObject uprScroll = Instantiate (Resources.Load (scrollPath)) as GameObject;
					uprScroll.transform.SetParent (baseObj.transform);
					uprScroll.transform.localPosition = new Vector2 (0, 95);
					uprScroll.transform.localScale = new Vector2 (1, 1);
					uprScroll.name = "CyusaiScrollViewUpper";
					GameObject uprContent = uprScroll.transform.Find ("Content").gameObject;
					GameObject btnScroll = Instantiate (Resources.Load (scrollPath)) as GameObject;
					btnScroll.transform.SetParent (baseObj.transform);
					btnScroll.transform.localPosition = new Vector2 (0, -170);
					btnScroll.transform.localScale = new Vector2 (1, 1);
					btnScroll.name = "CyusaiScrollViewBottom";
					GameObject btnContent = btnScroll.transform.Find ("Content").gameObject;

					//Upper Scroll
					string slotPath = "Prefabs/Bakuhu/CyusaiSlot";
					Daimyo daimyo = new Daimyo ();
					foreach (string daimyoId in checkedDaimyoList) {
						GameObject uprSlot = Instantiate (Resources.Load (slotPath)) as GameObject;
						uprSlot.transform.SetParent (uprContent.transform);
						uprSlot.transform.localScale = new Vector2 (1, 1);
						uprSlot.GetComponent<CyusaiDaimyoSelect> ().daimyoId = daimyoId;
						uprSlot.GetComponent<CyusaiDaimyoSelect> ().checkedDaimyoList = checkedDaimyoList;
						uprSlot.GetComponent<CyusaiDaimyoSelect> ().uprContent = uprContent;
						uprSlot.GetComponent<CyusaiDaimyoSelect> ().btnContent = btnContent;

						int daimyoBusyoId = daimyo.getDaimyoBusyoId (int.Parse (daimyoId), senarioId);
						string daimyoName = daimyo.getName (int.Parse (daimyoId),langId,senarioId);
						uprSlot.GetComponent<CyusaiDaimyoSelect> ().daimyoName = daimyoName;
						string daimyoBusyoPath = "Prefabs/Player/Sprite/unit" + daimyoBusyoId.ToString ();
						uprSlot.transform.Find ("Image").transform.Find ("Image").GetComponent<Image> ().sprite = 
							Resources.Load (daimyoBusyoPath, typeof(Sprite)) as Sprite;
						uprSlot.transform.Find ("DaimyoName").GetComponent<Text> ().text = daimyoName;

					}
					string cyusaiTxtPath = "Prefabs/Bakuhu/CyusaiText";
					GameObject cTxt = Instantiate (Resources.Load (cyusaiTxtPath)) as GameObject;
					cTxt.transform.SetParent (btnContent.transform);
					cTxt.transform.localScale = new Vector2 (0.12f, 0.15f);
					cTxt.name = "CyusaiText";



				} else {
					audioSources [4].Play ();

					Message msg = new Message ();
					msg.makeMessageOnBoard (msg.getMessage(100));
				}
			} else {
				audioSources [4].Play ();

				//Hyourou NG
				Message msg = new Message ();
				//msg.makeMessageOnBoard (msg.getMessage(7));
                msg.hyourouMovieMessage();
            }
				
		} else if (name == "SobujiKessenBtn") {

			if (hyourou >= 30) {
				bool myKuniQtyIsTwiceFlg = GameObject.Find ("GameController").GetComponent<MainStageController> ().myKuniQtyIsTwiceFlg;
				if (myKuniQtyIsTwiceFlg) {
					audioSources [0].Play ();

					//Confirm Button
					//Back Cover
					string backPath = "Prefabs/Busyo/back";
					GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
					back.transform.SetParent (GameObject.Find ("Map").transform);
					back.transform.localScale = new Vector2 (1, 1);
					RectTransform backTransform = back.GetComponent<RectTransform> ();
					backTransform.anchoredPosition3D = new Vector3 (0, 0, 0);

                    //Message Box
                    GameObject soubujireiConfirmObj = Instantiate(soubujireiConfirm) as GameObject;
                    soubujireiConfirmObj.transform.SetParent (GameObject.Find ("Map").transform);
                    soubujireiConfirmObj.transform.localScale = new Vector2 (1, 1);
					RectTransform msgTransform = soubujireiConfirmObj.GetComponent<RectTransform> ();
					msgTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
					msgTransform.name = "SoubujireiConfirmObj";

					GameObject YesBtn = soubujireiConfirmObj.transform.Find ("YesButton").gameObject;
					GameObject NoBtn = soubujireiConfirmObj.transform.Find ("NoButton").gameObject;
					YesBtn.GetComponent<DoSoubujirei> ().board = board;
					YesBtn.GetComponent<DoSoubujirei> ().confirm = soubujireiConfirmObj;
					YesBtn.GetComponent<DoSoubujirei> ().back = back;
					NoBtn.GetComponent<DoSoubujirei> ().confirm = soubujireiConfirmObj;
					NoBtn.GetComponent<DoSoubujirei> ().back = back;

				} else {
					audioSources [4].Play ();

					Message msg = new Message ();
					msg.makeMessageOnBoard (msg.getMessage(101));
				}
			} else {
				audioSources [4].Play ();

				//Hyourou NG
				Message msg = new Message ();
				//msg.makeMessageOnBoard (msg.getMessage(7));
                msg.hyourouMovieMessage();
            }
		} else if (name == "KessenBtn") {
			if (hyourou >= hyourouNo) {
				audioSources [0].Play ();

				//Confirm Button

				//Back Cover
				string backPath = "Prefabs/Busyo/back";
				GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
				back.transform.SetParent (GameObject.Find ("Map").transform);
				back.transform.localScale = new Vector2 (1, 1);
				RectTransform backTransform = back.GetComponent<RectTransform> ();
				backTransform.anchoredPosition3D = new Vector3 (0, 0, 0);

				//Message Box
				string msgPath = "Prefabs/Bakuhu/KessenConfirm";
				GameObject msg = Instantiate (Resources.Load (msgPath)) as GameObject;
				msg.transform.SetParent (GameObject.Find ("Map").transform);
				msg.transform.localScale = new Vector2 (1, 1);
				RectTransform msgTransform = msg.GetComponent<RectTransform> ();
				msgTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
				msgTransform.name = "KessenConfirm";                
                if (langId == 2) {
                    msg.transform.Find("Text").GetComponent<Text>().text = "Operate final war with " + daimyoName + ".";
                }else {
                    msg.transform.Find("Text").GetComponent<Text>().text = daimyoName + "に決戦を仕掛けます";
                }
				GameObject YesBtn = msg.transform.Find ("YesButton").gameObject;
				GameObject NoBtn = msg.transform.Find ("NoButton").gameObject;
				YesBtn.GetComponent<DoKessen> ().daimyoId = daimyoId;
				YesBtn.GetComponent<DoKessen> ().daimyoName = daimyoName;
				YesBtn.GetComponent<DoKessen> ().kuniId = kuniId;
				YesBtn.GetComponent<DoKessen> ().needHyourouNo = hyourouNo;
				NoBtn.GetComponent<DoKessen> ().confirm = msg;
				NoBtn.GetComponent<DoKessen> ().back = back;

				YesBtn.transform.Find ("hyourouIcon").transform.Find ("hyourouNoValue").GetComponent<Text> ().text = hyourouNo.ToString ();


			} else {
				audioSources [4].Play ();

				//Hyourou NG
				Message msg = new Message ();
				//msg.makeMessageOnBoard (msg.getMessage(7));
                msg.hyourouMovieMessage();
            }
		}




	}

	public int getKuniQty(int daimyoId, List<string> seiryokuList){
		int kuniId = 0;

		for(int i=0; i<seiryokuList.Count; i++){
			int daimyoIdTmp = int.Parse(seiryokuList [i]);
			if (daimyoId == daimyoIdTmp) {
				kuniId = kuniId + 1;
			}
		}
		return kuniId;
	}
}
