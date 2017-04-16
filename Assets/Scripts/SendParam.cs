using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class SendParam : MonoBehaviour {

	public bool soubujireiFlg = false;
	public bool bakuhuFlg = false;
	public int kuniId;
	public string kuniName;
	public int daimyoId;
	public string daimyoName;
	public int daimyoBusyoId;
	public int daimyoBusyoAtk;
	public int daimyoBusyoDfc;
	public bool openFlg = false;
	public bool clearFlg = false;
	public int busyoQty = 0;
	public int busyoLv = 0;
	public int butaiQty = 0;
	public int butaiLv = 0;
	public int heiryoku = 0;
	public int myYukouValue = 0;
	public int kuniQty = 0;
	public bool doumeiFlg = false;
	public string naiseiItem = "";
	public bool aggressiveFlg = false;
	public int cyouhouSnbRankId = 0;

	//Initial Daimyo Select
	public bool gameClearFlg = false;
	public bool busyoHaveFlg = false;

	//Sound
	public AudioSource sound;

	//Toubatsu
	public GameObject toubatsu;


	public void OnClick(){

		//SE
		sound = GameObject.Find ("SEController").GetComponent<AudioSource> ();
		sound.PlayOneShot (sound.clip); 

		if (!bakuhuFlg) {

            /*Common Process*/
            if (Application.loadedLevelName != "tutorialMain") {
                string pathOfBack = "Prefabs/Common/TouchBack";
			    GameObject back = Instantiate (Resources.Load (pathOfBack)) as GameObject;
			    back.transform.parent = GameObject.Find ("Panel").transform;
			    back.transform.localScale = new Vector2 (1, 1);
			    back.transform.localPosition = new Vector2 (0, 0);
            }

			if (Application.loadedLevelName != "clearOrGameOver") {

				string pathOfBoard = "Prefabs/Map/smallBoard";
				GameObject board = Instantiate (Resources.Load (pathOfBoard)) as GameObject;
				board.transform.parent = GameObject.Find ("Panel").transform;
				board.transform.localScale = new Vector2 (1, 1);

                if (Application.loadedLevelName == "tutorialMain") {
                    board.transform.FindChild("close").gameObject.SetActive(false);
                }

                /*Value Setting*/
                //Kuni Name
                GameObject.Find ("kuniName").GetComponent<Text> ().text = kuniName;

				//Daimyo Name
				GameObject.Find ("DaimyoNameValue").GetComponent<Text> ().text = daimyoName;

				//Kamon
				string kamonPath = "Prefabs/Kamon/" + daimyoId.ToString ();
				GameObject kamon = GameObject.Find ("KamonBack");
				kamon.GetComponent<Image> ().sprite = 
					Resources.Load (kamonPath, typeof(Sprite)) as Sprite;

				//Daimyo Busyo View
				string busyoViewPath = "Prefabs/Map/daimyoView";
				GameObject daimyoView = Instantiate (Resources.Load (busyoViewPath)) as GameObject;
				daimyoView.transform.SetParent (kamon.transform);
				daimyoView.transform.localScale = new Vector2 (1, 1);
				RectTransform busyoTransform = daimyoView.GetComponent<RectTransform> ();
				busyoTransform.anchoredPosition = new Vector3 (90, 125, 0);
				busyoTransform.sizeDelta = new Vector2 (180, 230);

				string daimyoPath = "Prefabs/Player/Sprite/unit" + daimyoBusyoId.ToString ();
				daimyoView.GetComponent<Image> ().sprite = 
					Resources.Load (daimyoPath, typeof(Sprite)) as Sprite;


				/*
				string busyoPath = "Prefabs/Player/Unit/" + daimyoBusyoId;
				GameObject busyo = Instantiate (Resources.Load (busyoPath)) as GameObject;
				busyo.transform.SetParent (kamon.transform);
				busyo.transform.localScale = new Vector2 (1, 1);
				busyo.GetComponent<DragHandler> ().enabled = false;

				RectTransform busyoTransform = busyo.GetComponent<RectTransform> ();
				busyoTransform.anchoredPosition = new Vector3 (90, 100, 0);
				busyoTransform.sizeDelta = new Vector2 (180, 200);

				foreach (Transform n in busyo.transform) {
					GameObject.Destroy (n.gameObject);
				}
				*/

				//Doumei
				if (doumeiFlg) {
					string doumeiPath = "Prefabs/Common/Doumei";
					GameObject doumei = Instantiate (Resources.Load (doumeiPath)) as GameObject;
					doumei.transform.SetParent (kamon.transform);
					doumei.transform.localScale = new Vector2 (1, 1);
					RectTransform doumeiTransform = doumei.GetComponent<RectTransform> ();
					doumeiTransform.anchoredPosition = new Vector3 (-50, 80, 0);
					doumei.name = "Doumei";

				}

				//Naisei Shigen Icon
				List<string> naiseiIconList = new List<string> ();
				char[] delimiterChars = { ':' };
				if (naiseiItem != "null" && naiseiItem != "") {
					if (naiseiItem.Contains (":")) {
						naiseiIconList = new List<string> (naiseiItem.Split (delimiterChars));
					} else {
						naiseiIconList.Add (naiseiItem);
					}

					//Base
					string nasieiBasePath = "Prefabs/Map/Common/NaiseiList";
					GameObject naiseiBase = Instantiate (Resources.Load (nasieiBasePath)) as GameObject;
					naiseiBase.transform.SetParent (board.transform);
					naiseiBase.transform.localScale = new Vector2 (1, 1);
					RectTransform naiseiBaseTransform = naiseiBase.GetComponent<RectTransform> ();
					naiseiBaseTransform.anchoredPosition = new Vector3 (405, -80, 0);


					//Icon
					string nasieiIconPath = "Prefabs/Map/Common/NaiseiItem";
					for (int i = 0; i < naiseiIconList.Count; i++) {
						GameObject naiseiIcon = Instantiate (Resources.Load (nasieiIconPath)) as GameObject;
						naiseiIcon.transform.SetParent (naiseiBase.transform);
						naiseiIcon.transform.localScale = new Vector2 (1, 1);

						string naiseiName = naiseiIconList [i];
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            if (naiseiName == "kb") {
							    naiseiIcon.transform.FindChild ("Text").GetComponent<Text> ().text = "H";
                                naiseiIcon.GetComponent<IconExp>().IconId = 5;

                            } else if (naiseiName == "tp") {
							    naiseiIcon.transform.FindChild ("Text").GetComponent<Text> ().text = "G";
                                naiseiIcon.GetComponent<IconExp>().IconId = 6;

                            }
                            else if (naiseiName == "kzn") {
							    naiseiIcon.transform.FindChild ("Text").GetComponent<Text> ().text = "M";
                                naiseiIcon.GetComponent<IconExp>().IconId = 7;

                            }
                            else if (naiseiName == "snb") {
							    naiseiIcon.transform.FindChild ("Text").GetComponent<Text> ().text = "N";
                                naiseiIcon.GetComponent<IconExp>().IconId = 8;

                            }
                            else if (naiseiName == "nbn") {
							    naiseiIcon.transform.FindChild ("Text").GetComponent<Text> ().text = "W";
                                naiseiIcon.GetComponent<IconExp>().IconId = 9;

                            }
                            else if (naiseiName == "mkd") {
							    naiseiIcon.transform.FindChild ("Text").GetComponent<Text> ().text = "E";
                                naiseiIcon.GetComponent<IconExp>().IconId = 10;

                            }
                            else if(naiseiName == "syn") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "T";
                                naiseiIcon.GetComponent<IconExp>().IconId = 11;

                            }
                        }
                        else {
                            if (naiseiName == "kb") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "馬";
                                naiseiIcon.GetComponent<IconExp>().IconId = 5;
                            }
                            else if (naiseiName == "tp") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "砲";
                                naiseiIcon.GetComponent<IconExp>().IconId = 6;
                            }
                            else if (naiseiName == "kzn") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "鉱";
                                naiseiIcon.GetComponent<IconExp>().IconId = 7;
                            }
                            else if (naiseiName == "snb") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "忍";
                                naiseiIcon.GetComponent<IconExp>().IconId = 8;
                            }
                            else if (naiseiName == "nbn") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "南";
                                naiseiIcon.GetComponent<IconExp>().IconId = 9;
                            }
                            else if (naiseiName == "mkd") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "帝";
                                naiseiIcon.GetComponent<IconExp>().IconId = 10;
                            }
                            else if (naiseiName == "syn") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "商";
                                naiseiIcon.GetComponent<IconExp>().IconId = 11;

                            }
                        }
					}
				}


				//Heiryoku Calc
				GameObject yukouValue = GameObject.Find ("YukouValue");
				GameObject atkBtn = GameObject.Find ("AttackButton");
				GameObject gaikouBtn = GameObject.Find ("GaikouButton");
				GameObject bouryakuhouBtn = GameObject.Find ("BouryakuButton");

				Color NGClorBtn = new Color (133 / 255f, 133 / 255f, 80 / 255f, 255f / 255f);
				Color NGClorTxt = new Color (90 / 255f, 90 / 255f, 40 / 255f, 255f / 255f);


				if (clearFlg == false) {

                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        atkBtn.transform.FindChild("Text").GetComponent<Text>().text = "Attack";
                    }else {
                        atkBtn.transform.FindChild("Text").GetComponent<Text>().text = "侵略";
                    }

                    if (cyouhouSnbRankId != 0) {
						GameObject.Find ("HeiryokuValue").GetComponent<Text> ().text = heiryoku.ToString ();

						//Shinobi Icon
						string shinobiItemPath = "Prefabs/Item/Shinobi/Shinobi";
						GameObject shinobi = Instantiate (Resources.Load (shinobiItemPath)) as GameObject;	
						shinobi.transform.SetParent (board.transform);
						shinobi.transform.localScale = new Vector2 (0.25f, 0.31f);
						shinobi.name = "shinobi";
						RectTransform snbTransform = shinobi.GetComponent<RectTransform> ();
						snbTransform.anchoredPosition = new Vector3 (-251, 250, 0);
						shinobi.GetComponent<Button> ().enabled = false;

						if (cyouhouSnbRankId == 1) {
							Color lowColor = new Color (0f / 255f, 0f / 255f, 219f / 255f, 255f / 255f);
							shinobi.GetComponent<Image> ().color = lowColor;
                            if (Application.systemLanguage != SystemLanguage.Japanese) {
                                shinobi.transform.FindChild("ShinobiRank").GetComponent<Text>().text = "Low";
                            }else {
                                shinobi.transform.FindChild ("ShinobiRank").GetComponent<Text> ().text = "下";
                            }

                        }else if (cyouhouSnbRankId == 2) {
							Color midColor = new Color (94f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
							shinobi.GetComponent<Image> ().color = midColor;
                            if (Application.systemLanguage != SystemLanguage.Japanese) {
                                shinobi.transform.FindChild ("ShinobiRank").GetComponent<Text> ().text = "Mid";
                            }else {
                                shinobi.transform.FindChild("ShinobiRank").GetComponent<Text>().text = "中";
                            }
						} else if (cyouhouSnbRankId == 3) {
							Color highColor = new Color (84f / 255f, 103f / 255f, 0f / 255f, 255f / 255f);
							shinobi.GetComponent<Image> ().color = highColor;
                            if (Application.systemLanguage != SystemLanguage.Japanese) {
                                shinobi.transform.FindChild ("ShinobiRank").GetComponent<Text> ().text = "High";
                            }else {
                                shinobi.transform.FindChild("ShinobiRank").GetComponent<Text>().text = "上";
                            }
						}


					} else {
						GameObject.Find ("HeiryokuValue").GetComponent<Text> ().text = "?";
					}


					//Yukoudo
					yukouValue.GetComponent<Text> ().text = myYukouValue.ToString ();

					//Cyouhou
					atkBtn.GetComponent<AttackNaiseiView> ().cyouhouSnbRankId = cyouhouSnbRankId;

				} else {
					//Cleard
					GameObject.Find ("HeiryokuValue").GetComponent<Text> ().text = heiryoku.ToString ();

					//Yukoudo
					yukouValue.GetComponent<Text> ().text = "-";

					//Enable Gaiko & Cyouhou
					gaikouBtn.GetComponent<Image> ().color = NGClorBtn;
					gaikouBtn.GetComponent<Button> ().enabled = false;
					gaikouBtn.transform.FindChild ("Text").GetComponent<Text> ().color = NGClorTxt;


					bouryakuhouBtn.GetComponent<Image> ().color = NGClorBtn;
					bouryakuhouBtn.GetComponent<Button> ().enabled = false;
					bouryakuhouBtn.transform.FindChild ("Text").GetComponent<Text> ().color = NGClorTxt;

                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        atkBtn.transform.FindChild ("Text").GetComponent<Text> ().text = "Develop";
                    }else {
                        atkBtn.transform.FindChild("Text").GetComponent<Text>().text = "内政";
                    }
                    if (Application.loadedLevelName == "tutorialMain") {
                        TutorialController tutorialScript = new TutorialController();
                        Vector2 vect = new Vector2(0, 50);
                        GameObject animObj = tutorialScript.SetPointer(atkBtn, vect);
                        animObj.transform.localScale = new Vector2(150, 150);
                    }
                }
                
				//Enable Attack
				if (openFlg == false && clearFlg == false) {
					atkBtn.GetComponent<Image> ().color = NGClorBtn;
					//test
					atkBtn.GetComponent<Button> ().enabled = false;
					atkBtn.transform.FindChild ("Text").GetComponent<Text> ().color = NGClorTxt;

				}
				//Enable Gaikou
				if (soubujireiFlg) {
					gaikouBtn.GetComponent<Image> ().color = NGClorBtn;
					gaikouBtn.GetComponent<Button> ().enabled = false;
					gaikouBtn.transform.FindChild ("Text").GetComponent<Text> ().color = NGClorTxt;

				}

				//Doumei Flg
				if (doumeiFlg) {
					atkBtn.GetComponent<AttackNaiseiView> ().doumeiFlg = doumeiFlg;
					gaikouBtn.GetComponent<GaikouView> ().doumeiFlg = doumeiFlg;

					atkBtn.GetComponent<Image> ().color = NGClorBtn;
					atkBtn.GetComponent<Button> ().enabled = false;
					atkBtn.transform.FindChild ("Text").GetComponent<Text> ().color = NGClorTxt;
				}

                //Set Hidden Value
                if (Application.loadedLevelName != "tutorialMain") {
                    GameObject close = GameObject.Find ("close").gameObject;
				    close.GetComponent<CloseBoard> ().title = kuniName;
				    close.GetComponent<CloseBoard> ().daimyoId = daimyoId;
				    close.GetComponent<CloseBoard> ().daimyoBusyoId = daimyoBusyoId;
				    close.GetComponent<CloseBoard> ().daimyoBusyoName = daimyoName;
				    close.GetComponent<CloseBoard> ().doumeiFlg = doumeiFlg;
				    close.GetComponent<CloseBoard> ().kuniQty = kuniQty;
				    close.GetComponent<CloseBoard> ().kuniId = kuniId;
				    close.GetComponent<CloseBoard> ().daimyoBusyoAtk = daimyoBusyoAtk;
				    close.GetComponent<CloseBoard> ().daimyoBusyoDfc = daimyoBusyoDfc;
				    close.GetComponent<CloseBoard> ().yukoudo = myYukouValue;
				    close.GetComponent<CloseBoard> ().naiseiItem = naiseiItem;
                

				    bool cyouhouFlg = false;
				    if (cyouhouSnbRankId != 0) {
					    cyouhouFlg = true;
				    }
				    close.GetComponent<CloseBoard> ().cyouhouFlg = cyouhouFlg;
				    close.GetComponent<CloseBoard> ().cyouhouSnbRankId = cyouhouSnbRankId;
                }

                //Set Button Value
                AttackNaiseiView attkNaiseView = GameObject.Find ("AttackButton").GetComponent<AttackNaiseiView> ();
				attkNaiseView.kuniId = kuniId;
				attkNaiseView.kuniName = kuniName;
				attkNaiseView.myDaimyoId = GameObject.Find ("GameController").GetComponent<MainStageController> ().myDaimyo;
				attkNaiseView.daimyoId = daimyoId;
				attkNaiseView.daimyoName = daimyoName;
				attkNaiseView.openFlg = openFlg;
				attkNaiseView.clearFlg = clearFlg;
				attkNaiseView.activeBusyoQty = busyoQty;
				attkNaiseView.activeBusyoLv = busyoLv;
				attkNaiseView.activeButaiQty = butaiQty;
				attkNaiseView.activeButaiLv = butaiLv;



				//Cyoutei Button
				if (kuniId == 16) {
					//Yamashiro
					string pathOfButton = "Prefabs/Cyoutei/CyouteiIcon";
					GameObject btn = Instantiate (Resources.Load (pathOfButton)) as GameObject;
					btn.transform.SetParent (board.transform);
					btn.transform.localScale = new Vector2 (1, 1);
					btn.transform.localPosition = new Vector2 (225, -220);
					btn.name = "CyouteiIcon";
					
					btn.GetComponent<CyouteiPop> ().yukoudo = myYukouValue;
					btn.GetComponent<CyouteiPop> ().myDaimyoFlg = clearFlg;
					btn.GetComponent<CyouteiPop> ().occupiedDaimyoId = daimyoId;
					btn.GetComponent<CyouteiPop> ().occupiedDaimyoName = daimyoName;
				}
				
				//Syounin Button
				if (kuniId == 38 || kuniId == 39 || kuniId == 58) {
					
					//Hakata or Sakai
					string pathOfButton = "Prefabs/Syounin/SyouninIcon";
					GameObject btn = Instantiate (Resources.Load (pathOfButton)) as GameObject;
					btn.transform.SetParent (board.transform);
					btn.transform.localScale = new Vector2 (1, 1);
					btn.transform.localPosition = new Vector2 (225, -220);
					btn.name = "SyouninIcon";
					
					btn.GetComponent<SyouninPop> ().yukoudo = myYukouValue;
					btn.GetComponent<SyouninPop> ().myDaimyoFlg = clearFlg;
					btn.GetComponent<SyouninPop> ().occupiedDaimyoName = daimyoName;
					
					if (kuniId == 38 || kuniId == 39) {
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            btn.transform.FindChild("Text").GetComponent<Text>().text = "Sakai";
                            btn.transform.FindChild("Text").GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                            btn.transform.FindChild("Text").GetComponent<Text>().fontSize = 200;
                        } else {
                            btn.transform.FindChild("Text").GetComponent<Text>().text = "堺";
                        }
                        btn.GetComponent<SyouninPop> ().sakaiFlg = true;
					} else if (kuniId == 58) {
						
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            btn.transform.FindChild("Text").GetComponent<Text>().text = "Hakata";
                            btn.transform.FindChild("Text").GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                            btn.transform.FindChild("Text").GetComponent<Text>().fontSize = 200;
                        }else {
                            btn.transform.FindChild("Text").GetComponent<Text>().text = "博多";
                        }
                    }
				}



			} else {

				//Select Initial Daimyo Screen
				string pathOfBoard = "Prefabs/clearOrGameOver/DaimyoSelectBoard";
				GameObject board = Instantiate (Resources.Load (pathOfBoard)) as GameObject;
				board.transform.parent = GameObject.Find ("Panel").transform;
				board.transform.localScale = new Vector2 (1, 1);
				GameObject selectBtn = board.transform.FindChild ("SelectButton").gameObject;

				//Kuni Name
				GameObject.Find ("kuniName").GetComponent<Text> ().text = kuniName;
				
				//Daimyo Name
				GameObject.Find ("DaimyoNameValue").GetComponent<Text> ().text = daimyoName;
				
				//Kamon
				string kamonPath = "Prefabs/Kamon/" + daimyoId.ToString ();
				GameObject kamon = GameObject.Find ("KamonBack");
				kamon.GetComponent<Image> ().sprite = 
					Resources.Load (kamonPath, typeof(Sprite)) as Sprite;

				//Daimyo Busyo View
				string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
				GameObject busyo = Instantiate (Resources.Load (busyoPath)) as GameObject;
				busyo.name = daimyoBusyoId.ToString();
				busyo.transform.SetParent (kamon.transform);
				busyo.transform.localScale = new Vector2 (1, 1);
				busyo.GetComponent<DragHandler> ().enabled = false;
				
				RectTransform busyoTransform = busyo.GetComponent<RectTransform> ();
				busyoTransform.anchoredPosition = new Vector3 (90, 100, 0);
				busyoTransform.sizeDelta = new Vector2 (180, 200);
				
				foreach (Transform n in busyo.transform) {
					GameObject.Destroy (n.gameObject);
				}

				//Kuni Qty
				GameObject.Find ("KuniQtyValue").GetComponent<Text> ().text = kuniQty.ToString ();

				//Once Cleared Flg
				if (gameClearFlg) {
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        GameObject.Find("KouryakuFlg").transform.FindChild("Label").GetComponent<Text>().text = " Cleared";
                    }else {
                        GameObject.Find("KouryakuFlg").transform.FindChild("Label").GetComponent<Text>().text = "攻略済";
                    }
                        
				}else {
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        GameObject.Find("KouryakuFlg").transform.FindChild("Label").GetComponent<Text>().text = " Never Cleared";
                    }else {
                        GameObject.Find("KouryakuFlg").transform.FindChild("Label").GetComponent<Text>().text = "未攻略";
                    }
                }

				//Status
				//Daimyo Have Flg
				char[] delimiterChars = { ':' };
				int lv = 0;
				StatusGet sts = new StatusGet ();

                //Default Status
                lv = 1;
				GameObject.Find ("ButaiQtyValue").GetComponent<Text> ().text = "1";
				GameObject.Find ("ButaiLvValue").GetComponent<Text> ().text = "1";
				

				//Hp
				int hp = sts.getHp (daimyoBusyoId, lv);
				hp = hp * 100;
				GameObject.Find ("HPValue").GetComponent<Text> ().text = hp.ToString ();
				
				//Atk
				int atk = sts.getAtk (daimyoBusyoId, lv);
				atk = atk * 10;
				GameObject.Find ("AtkValue").GetComponent<Text> ().text = atk.ToString ();
				
				//Dfc
				int dfc = sts.getDfc (daimyoBusyoId, lv);
				dfc = dfc * 10;
				GameObject.Find ("DfcValue").GetComponent<Text> ().text = dfc.ToString ();
				
				//Spd
				int spd = sts.getSpd (daimyoBusyoId, lv);
				GameObject.Find ("SpdValue").GetComponent<Text> ().text = spd.ToString ();

				//Heisyu
				string heisyu = sts.getHeisyu (daimyoBusyoId);
				string heisyuKanji = "";
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    if (heisyu == "YR") {
					    heisyuKanji = "Spear";
				    } else if (heisyu == "KB") {
					    heisyuKanji = "Cavalry";
				    } else if (heisyu == "YM") {
					    heisyuKanji = "Bow";
				    } else if (heisyu == "TP") {
					    heisyuKanji = "Gun";
				    }
                }else {
                    if (heisyu == "YR") {
                        heisyuKanji = "槍";
                    }else if (heisyu == "KB") {
                        heisyuKanji = "騎馬";
                    }else if (heisyu == "YM") {
                        heisyuKanji = "弓";
                    }else if (heisyu == "TP") {
                        heisyuKanji = "鉄砲";
                    }
                }
				GameObject.Find ("HeisyuValue").GetComponent<Text> ().text = heisyuKanji.ToString ();

				//Naisei Shigen Icon
				List<string> naiseiIconList = new List<string> ();
				if (naiseiItem != "null" && naiseiItem != "") {
					if (naiseiItem.Contains (":")) {
						naiseiIconList = new List<string> (naiseiItem.Split (delimiterChars));
					} else {
						naiseiIconList.Add (naiseiItem);
					}
					
					//Base
					string nasieiBasePath = "Prefabs/Map/Common/NaiseiList";
					GameObject naiseiBase = Instantiate (Resources.Load (nasieiBasePath)) as GameObject;
					naiseiBase.transform.SetParent (board.transform);
					naiseiBase.transform.localScale = new Vector2 (1, 1);
					RectTransform naiseiBaseTransform = naiseiBase.GetComponent<RectTransform> ();
					naiseiBaseTransform.anchoredPosition = new Vector3 (405, -80, 0);
					
					
					//Icon
					string nasieiIconPath = "Prefabs/Map/Common/NaiseiItem";
					for (int i = 0; i < naiseiIconList.Count; i++) {
						GameObject naiseiIcon = Instantiate (Resources.Load (nasieiIconPath)) as GameObject;
						naiseiIcon.transform.SetParent (naiseiBase.transform);
						naiseiIcon.transform.localScale = new Vector2 (1, 1);
						
						string naiseiName = naiseiIconList [i];
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            if (naiseiName == "kb") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "H";
                                naiseiIcon.GetComponent<IconExp>().IconId = 5;
                            }
                            else if (naiseiName == "tp") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "G";
                                naiseiIcon.GetComponent<IconExp>().IconId = 6;
                            }
                            else if (naiseiName == "kzn") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "M";
                                naiseiIcon.GetComponent<IconExp>().IconId = 7;
                            }
                            else if (naiseiName == "snb") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "N";
                                naiseiIcon.GetComponent<IconExp>().IconId = 8;
                            }
                            else if (naiseiName == "nbn") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "W";
                                naiseiIcon.GetComponent<IconExp>().IconId = 9;
                            }
                            else if (naiseiName == "mkd") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "E";
                                naiseiIcon.GetComponent<IconExp>().IconId = 10;
                            }
                            else if (naiseiName == "syn") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "T";
                                naiseiIcon.GetComponent<IconExp>().IconId = 11;

                            }
                        }
                        else {
                            if (naiseiName == "kb") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "馬";
                                naiseiIcon.GetComponent<IconExp>().IconId = 5;
                            }
                            else if (naiseiName == "tp") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "砲";
                                naiseiIcon.GetComponent<IconExp>().IconId = 6;
                            }
                            else if (naiseiName == "kzn") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "鉱";
                                naiseiIcon.GetComponent<IconExp>().IconId = 7;
                            }
                            else if (naiseiName == "snb") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "忍";
                                naiseiIcon.GetComponent<IconExp>().IconId = 8;
                            }
                            else if (naiseiName == "nbn") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "南";
                                naiseiIcon.GetComponent<IconExp>().IconId = 9;
                            }
                            else if (naiseiName == "mkd") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "帝";
                                naiseiIcon.GetComponent<IconExp>().IconId = 10;
                            }
                            else if (naiseiName == "syn") {
                                naiseiIcon.transform.FindChild("Text").GetComponent<Text>().text = "商";
                                naiseiIcon.GetComponent<IconExp>().IconId = 11;

                            }
                        }
                        
					}
				}
				selectBtn.GetComponent<SelectDaimyo> ().daimyoId = daimyoId;
				selectBtn.GetComponent<SelectDaimyo> ().daimyoName = daimyoName;
				selectBtn.GetComponent<SelectDaimyo> ().daimyoBusyoId = daimyoBusyoId;
				selectBtn.GetComponent<SelectDaimyo> ().busyoHaveFlg = busyoHaveFlg;
				selectBtn.GetComponent<SelectDaimyo> ().heisyu = heisyu;

			}


		} else {
			//Bakuhu Menu
			GameObject BakuhuBase = GameObject.Find("BakuhuBase").gameObject;

			if (BakuhuBase.transform.FindChild ("ToubatsuText") != null) {
				Destroy (BakuhuBase.transform.FindChild ("ToubatsuText").gameObject);	
			}

			if (BakuhuBase.transform.FindChild ("ToubatsuSelect") == null) {
				string toubatsuPath = "Prefabs/Bakuhu/ToubatsuSelect";
				toubatsu = Instantiate (Resources.Load (toubatsuPath)) as GameObject;
				toubatsu.transform.SetParent (BakuhuBase.transform);
				toubatsu.transform.localScale = new Vector2 (1,1);
				toubatsu.name = "ToubatsuSelect";
			} else {
				toubatsu = BakuhuBase.transform.FindChild ("ToubatsuSelect").gameObject;
			}

			string kamonImagePath = "Prefabs/Kamon/" + daimyoId.ToString ();
			toubatsu.transform.FindChild("ToubatsuTarget").transform.FindChild("Kamon").GetComponent<Image> ().sprite = 
				Resources.Load (kamonImagePath, typeof(Sprite)) as Sprite;

			string imagePath = "Prefabs/Player/Sprite/unit" + daimyoBusyoId.ToString ();
			toubatsu.transform.FindChild("ToubatsuTarget").transform.FindChild("Daimyo").GetComponent<Image> ().sprite = 
				Resources.Load (imagePath, typeof(Sprite)) as Sprite;

            if (Application.systemLanguage != SystemLanguage.Japanese) {
                toubatsu.transform.FindChild ("Exp").GetComponent<Text> ().text = 
				"Would you declare " + daimyoName+ " attack order to surrounding parties?";
            }else {
                toubatsu.transform.FindChild("Exp").GetComponent<Text>().text =
                daimyoName + "の討伐令を周辺大名に出しますか？";
            }
			
			//Blinker
			GameObject BakuhuKuniIconView = BakuhuBase.transform.FindChild("BakuhuKuniIconView").gameObject;
			foreach(Transform obj in BakuhuKuniIconView.transform){
				SendParam script = obj.GetComponent<SendParam> ();
				if (script.daimyoId != daimyoId) {
					if (obj.GetComponent<ImageBlinker> ()) {
						Destroy (obj.GetComponent<ImageBlinker> ());
						obj.GetComponent<Image>().color = new Color (255, 255, 255);
					}
				} else {
					if (!obj.GetComponent<ImageBlinker> ()) {
						obj.gameObject.AddComponent<ImageBlinker>();
					}
				}
			}

			//Set Param to Button
			toubatsu.transform.FindChild("ToubatsuBtn").GetComponent<DoTobatsu>().targetDaimyoId = daimyoId;
			toubatsu.transform.FindChild("ToubatsuBtn").GetComponent<DoTobatsu>().targetDaimyoName = daimyoName;
			toubatsu.transform.FindChild ("ToubatsuBtn").GetComponent<DoTobatsu> ().kuniQty = kuniQty;



		}
	}
}