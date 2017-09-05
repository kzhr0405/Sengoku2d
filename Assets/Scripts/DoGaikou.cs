	using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class DoGaikou : MonoBehaviour {

	public int busyoId = 0;
	public string busyoName = "";
	public int daimyoId = 0;
	public string daimyoName = "";
	public int busyoChiryaku = 0;
	public int paiedMoney = 0;
	public bool moneyOKflg = false;
	public bool hyourouOKflg = false;
	public float doumeiRatio = 0;
	public float kyoutouRatio = 0;
	public float doukatsuRatio = 0;
	public int myYukoudo = 0;
	public string kuniName = "";
	public int targetKuniId = 0;
	public int itemQty = 0;
	public int srcKuniId = 0;
	public int srcDaimyoId = 0;
	public int targetDaimyoId = 0;
	public string srcDaimyoName = "";
	public string targetDaimyoName = "";
	public AudioSource[] audioSources;

	public void Start(){
		daimyoName = GameObject.Find ("DaimyoNameValue").GetComponent<Text> ().text;
		audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
	}

	// Use this for initialization
	public void OnClick () {
		
		Message msg = new Message();
		Gaikou gaikou = new Gaikou ();
		CloseBoard closeScript = GameObject.Find ("close").GetComponent<CloseBoard> ();
		daimyoId = closeScript.daimyoId;

		if (hyourouOKflg) {
			if (moneyOKflg) {

				//Track
				int TrackGaikouNo = PlayerPrefs.GetInt("TrackGaikouNo",0);
				TrackGaikouNo = TrackGaikouNo + 1;
				PlayerPrefs.SetInt("TrackGaikouNo",TrackGaikouNo);


				if (name == "DoMitsugiBtn") {
					
					string tempGaikou = "gaikou" + daimyoId;
					int nowYukoudo = 0;
					if (PlayerPrefs.HasKey (tempGaikou)) {
						nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
					} else {
						nowYukoudo = 50;
					}

					float percent = UnityEngine.Random.value;
					percent = percent * 100;
					float tmpYukoudo = (float)nowYukoudo;
					if (tmpYukoudo < 5) {
						tmpYukoudo = 5;
					}

					if (percent <= tmpYukoudo*2) {
                        //Success
                        audioSources[3].Play();
                        reduceMoneyHyourou ();
						addUsedBusyo (busyoId);

						//Doumei
						bool doumeiFlg = closeScript.doumeiFlg;

						//Add Yukoudo
						// AddYukoudo = (Money/200)*chiryaku/500
						int addYukoudo = (paiedMoney / 200) + (busyoChiryaku / 100);
						if (addYukoudo <= 0) {
							addYukoudo = 1;
						}


						if (doumeiFlg) {
							addYukoudo = addYukoudo * 2;
						}


						int newYukoudo = nowYukoudo + addYukoudo;
						if (newYukoudo > 100) {
							newYukoudo = 100;
						}
						PlayerPrefs.SetInt (tempGaikou, newYukoudo);
						GameObject.Find ("YukouValue").GetComponent<Text> ().text = newYukoudo.ToString (); 

						//Change new yukoudo
						closeScript.yukoudo = newYukoudo;

                        //Message
                        string OKtext = ""; 
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            OKtext = "Gave money "+ paiedMoney + " to " + daimyoName + ".\n Friendship increased " + addYukoudo + " point";
                        }else {
                            OKtext = daimyoName + "に金" + paiedMoney + "の貢物をしました。\n友好度が" + addYukoudo + "上がりますぞ。";
                        }
                        msg.makeMessage (OKtext);
						PlayerPrefs.SetBool ("questDailyFlg28", true);

						PlayerPrefs.Flush ();

						//Extension Mark Handling
						MainStageController main = new MainStageController ();
						main.questExtension ();

						upYukouOnIcon (daimyoId, newYukoudo);

					} else {
                        //Fail
                        audioSources[4].Play();
                        paiedMoney = 0;
						reduceMoneyHyourou ();

                        //Message
                        string NGtext = "";
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            NGtext = daimyoName + " declined our money.\n He doesn't want to build a good relationship with us";
                        }else {
                            NGtext = daimyoName + "に貢物を体よく断られ申した。\n当家と関係を修復する気はないようですな。";
                        }
                        msg.makeMessage (NGtext);
					}
					//Back
					GameObject.Find ("return").GetComponent<MenuReturn> ().OnClick ();


				} else if (name == "DoDoumeiBtn") {
			
					reduceMoneyHyourou ();

					//Doumei
					float percent = Random.value;
					percent = percent * 100;
					
					if (percent <= doumeiRatio) {
						//Doumei Success
						audioSources [3].Play ();

						//Track
						int TrackDoumeiNo = PlayerPrefs.GetInt("TrackDoumeiNo",0);
						TrackDoumeiNo = TrackDoumeiNo + 1;
						PlayerPrefs.SetInt("TrackDoumeiNo",TrackDoumeiNo);


						addUsedBusyo (busyoId);

						string doumei = PlayerPrefs.GetString ("doumei");
						if (doumei == null || doumei == "") {
							doumei = daimyoId.ToString ();
						} else {
							doumei = doumei + "," + daimyoId.ToString ();
						}

						//Data
						int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
						string cpuDoumeiTemp = "doumei" + daimyoId.ToString ();
						string cpuDoumei = PlayerPrefs.GetString (cpuDoumeiTemp);
						if (cpuDoumei != null & cpuDoumei != "") {
							cpuDoumei = cpuDoumei + "," + myDaimyo.ToString ();
						} else {
							cpuDoumei = myDaimyo.ToString ();
						}
						PlayerPrefs.SetString (cpuDoumeiTemp, cpuDoumei);
						PlayerPrefs.SetString ("doumei", doumei);

						//Change Target Flg & Kuni Icon Color
						string seiryoku = PlayerPrefs.GetString ("seiryoku");
						char[] delimiterChars = { ',' };
						List<string> seiryokuList = new List<string> ();
						seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
						GameObject KuniIconView = GameObject.Find ("KuniIconView").gameObject;

						Color doumeiColor = new Color (100f / 255f, 130f / 255f, 255f / 255f, 255f / 255f); //Blue
						for (int i = 0; i < seiryokuList.Count; i++) {
							int tempDaimyoId = int.Parse (seiryokuList [i]);

							if (tempDaimyoId == daimyoId) {
								int kuniId = i + 1;
								GameObject kuniIcon = KuniIconView.transform.FindChild (kuniId.ToString ()).gameObject;
								kuniIcon.GetComponent<Image> ().color = doumeiColor;
								kuniIcon.GetComponent<SendParam> ().doumeiFlg = true;
							}
						}

						PlayerPrefs.SetBool ("questSpecialFlg2", true);
						//Extension Mark Handling
						MainStageController main = new MainStageController ();
						main.questExtension ();

                        //Msg
                        string OKtext = "";
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            OKtext = "Congratulations.\n" + "We concluded an alliance with" + daimyoName + ".\n" + "we got some of strategic options.";
                        }else {
                            OKtext = "教悦至極にございます。" + daimyoName + "と同盟を結びましたぞ。\n" + "戦略の幅が広がりますな。";
                        }
                        msg.makeMessage (OKtext);

						//If Gunzei Exist
						foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Gunzei")){
							int srcDaimyoId = obs.GetComponent<Gunzei>().srcDaimyoId;
							int dstDaimyoId = obs.GetComponent<Gunzei>().dstDaimyoId;
							if(srcDaimyoId == daimyoId && dstDaimyoId == myDaimyo){
								Gunzei gunzeiScript = new Gunzei ();
								gunzeiScript.deleteGunzei(obs);
							}
						}


						Destroy (GameObject.Find ("smallBoard(Clone)"));
						Destroy (GameObject.Find ("TouchBack(Clone)"));

					} else {
						//Doumie Failed
						audioSources [4].Play ();

						int maxReduceValue = 3;
						int nowYukoudo = gaikou.getMyGaikou (daimyoId);
						int newYukoudo = gaikou.downMyGaikou (daimyoId, nowYukoudo, maxReduceValue);
						int reduceYukoudo = nowYukoudo - newYukoudo;
						closeScript.yukoudo = newYukoudo;

						GameObject.Find ("YukouValue").GetComponent<Text> ().text = newYukoudo.ToString ();
                        string NGtext = "";
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            NGtext = daimyoName + " declined our proposal. \n" + "Friendship decreased " + reduceYukoudo + " point";
                        }else {
                            NGtext = daimyoName + "に体よく断られ申した。\n友好度が" + reduceYukoudo + "下がりますぞ。";
                        }
                        msg.makeMessage (NGtext);

						downYukouOnIcon (daimyoId, newYukoudo);

						//Back
						GameObject.Find ("return").GetComponent<MenuReturn> ().OnClick ();
					}

					PlayerPrefs.Flush ();

					
				} else if (name == "DoKyoutouBtn") {

					reduceMoneyHyourou ();

					//Kyoutou Check
					float percent = Random.value;
					percent = percent * 100;
					if (percent <= kyoutouRatio) {
						audioSources [3].Play ();

						//Success
						addUsedBusyo (busyoId);

						string playerKyoutouList = PlayerPrefs.GetString ("playerKyoutouList", "");
						MainEventHandler kyoutou = new MainEventHandler ();
						if (playerKyoutouList == null || playerKyoutouList == "") {
							playerKyoutouList = targetKuniId + "-" + kyoutou.getEngunSts (daimyoId.ToString ());
						} else {
							playerKyoutouList = playerKyoutouList + ":" + +targetKuniId + "-" + kyoutou.getEngunSts (daimyoId.ToString ());
						}
						PlayerPrefs.SetString ("playerKyoutouList", playerKyoutouList);

                        //Msg
                        string OKtext = "";
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            OKtext = "Good. " + daimyoName + "will support our party in " + kuniName + " attack";
                        }else {
                            OKtext = daimyoName + "殿が" + kuniName + "攻めに\n加勢してくれますぞ。百人力ですな。";
                        }
                        msg.makeMessage (OKtext);


					} else {
						//Fail
						audioSources [4].Play ();

						//Doumie Failed
						int maxReduceValue = 3;
						int nowYukoudo = gaikou.getMyGaikou (daimyoId);
						int newYukoudo = gaikou.downMyGaikou (daimyoId, nowYukoudo, maxReduceValue);
						int reduceYukoudo = nowYukoudo - newYukoudo;
						GameObject.Find ("YukouValue").GetComponent<Text> ().text = newYukoudo.ToString (); 	
						closeScript.yukoudo = newYukoudo;

                        string NGtext = "";
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            NGtext = daimyoName + " declined our proposal. \n" + "Friendship decreased " + reduceYukoudo + " point";
                        }else {
                            NGtext = daimyoName + "に体よく断られ申した。\n友好度が" + reduceYukoudo + "下がりますぞ。";
                        }
                        msg.makeMessage (NGtext);

						downYukouOnIcon (daimyoId, newYukoudo);


					}

					PlayerPrefs.Flush ();
					
					//Back
					GameObject.Find ("return").GetComponent<MenuReturn> ().OnClick ();



				} else if (name == "DoDoukatsuBtn") {

					reduceMoneyHyourou ();

					float percent = Random.value;
					percent = percent * 100;
					
					if (percent <= doukatsuRatio) {
						//Success
						audioSources [3].Play ();
						addUsedBusyo (busyoId);

						int kuniQty = closeScript.kuniQty; 
						int getMoney = 0;
						//Money or Item 0:money, 1:item
						int moneyOrItem = UnityEngine.Random.Range (0, 2);
						//Kahou or Shizai 0:kahou, 1:shizai 
						int kahouOrShizai = UnityEngine.Random.Range (0, 2);
						string kahouName = "";
						string shigenName = "";
						int addQty = 0;
						int kahouRank = 0; //kahouRank S,A,B,C=1,2,3,4
						//shigen Type
						int shigenType = 0; //KB,YR,TP,YM=1,2,3,4
						
						if (moneyOrItem == 0) {
							//money
							int temGetMoney = UnityEngine.Random.Range (1000, 1501);
							getMoney = temGetMoney * kuniQty;
							int nowMoney = PlayerPrefs.GetInt ("money");
							nowMoney = nowMoney + getMoney;
                            if (nowMoney < 0) {
                                nowMoney = int.MaxValue;
                            }
                            PlayerPrefs.SetInt ("money", nowMoney);
							PlayerPrefs.Flush ();
							
						} else {
							//item
							//Kahou or Shizai 0:kahou, 1:shizai 
							kahouOrShizai = UnityEngine.Random.Range (0, 2);
							if (kahouOrShizai == 0) {
								//kahou
								Kahou kahou = new Kahou ();
								////Bugu, Gusoku, Kabuto, Meiba, Heihousyo, Cyadougu, Chishikisyo(1,2,3,4,5,6)
								int kahouType = UnityEngine.Random.Range (1, 7);
								
								float khPercent = Random.value;
								khPercent = khPercent * 100;
								if (5 <= kuniQty) {
									if (khPercent <= 1) {
										//S
										kahouRank = 1;
										kahouName = kahou.getRamdomKahou (kahouType, kahouRank);
									} else if (1 < khPercent && khPercent <= 30) {
										//A
										kahouRank = 2;
										kahouName = kahou.getRamdomKahou (kahouType, kahouRank);
									} else if (30 < khPercent) {
										//B
										kahouRank = 3;
										kahouName = kahou.getRamdomKahou (kahouType, kahouRank);
									}
									
								} else if (3 <= kuniQty && kuniQty < 5) {
									if (khPercent <= 0.5f) {
										//S
										kahouRank = 1;
										kahouName = kahou.getRamdomKahou (kahouType, kahouRank);
									} else if (0.5f < khPercent && khPercent <= 10) {
										//A
										kahouRank = 2;
										kahouName = kahou.getRamdomKahou (kahouType, kahouRank);
									} else if (10 < khPercent && khPercent <= 40) {
										//B
										kahouRank = 3;
										kahouName = kahou.getRamdomKahou (kahouType, kahouRank);
									} else if (40 < khPercent) {
										//C
										kahouRank = 4;
										kahouName = kahou.getRamdomKahou (kahouType, kahouRank);
									}
									
								} else if (kuniQty < 3) {
									//(A,B,C : 5, 35, 60%)
									if (khPercent <= 3) {
										//A
										kahouRank = 2;
										kahouName = kahou.getRamdomKahou (kahouType, kahouRank);
									} else if (3 < khPercent && khPercent <= 31) {
										//B
										kahouRank = 3;
										kahouName = kahou.getRamdomKahou (kahouType, kahouRank);
									} else if (31 < khPercent) {
										//C
										kahouRank = 3;
										kahouName = kahou.getRamdomKahou (kahouType, kahouRank);
									}
									
								}

								
							} else {
								
								//shizai
								shigenType = UnityEngine.Random.Range (1, 5);
								float sgPercent = Random.value;
								sgPercent = sgPercent * 100;
								addQty = UnityEngine.Random.Range (1, 6);
								Item item = new Item ();
								int shigenRank = 0;//下、中、上=1,2,3
								
								if (5 <= kuniQty) {
									//(上,中,下  40,40, 20%)
									if (sgPercent <= 40) {
										shigenRank = 3;
									} else if (40 < sgPercent && sgPercent <= 81) {
										shigenRank = 2;
									} else if (81 < sgPercent) {
										shigenRank = 1;
									}
									shigenName = item.getRandomShigen (shigenType, shigenRank, addQty);
									
								} else if (3 <= kuniQty && kuniQty < 5) {
									//(上,中,下  20,50,30%)
									if (sgPercent <= 20) {
										shigenRank = 3;
									} else if (20 < sgPercent && sgPercent <= 51) {
										shigenRank = 2;
									} else if (51 < sgPercent) {
										shigenRank = 1;
									}
									shigenName = item.getRandomShigen (shigenType, shigenRank, addQty);
									
								} else if (kuniQty < 3) {
									//(上,中,下  5,25,70%)
									if (sgPercent <= 5) {
										shigenRank = 3;
									} else if (5 < sgPercent && sgPercent <= 26) {
										shigenRank = 2;
									} else if (26 < sgPercent) {
										shigenRank = 1;
									}
									shigenName = item.getRandomShigen (shigenType, shigenRank, addQty);
								}
							}
						}
						
						
						//Message
						PlayerPrefs.SetBool ("questDailyFlg29", true);
						//Extension Mark Handling
						MainStageController main = new MainStageController ();
						main.questExtension ();
                        string OKtext = "";
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            OKtext = "My lord, successed to threat " + daimyoName;
                        }else {
                            OKtext = "御屋形様、恫喝に成功しましたぞ。\n" + daimyoName + "が";
                        }
						string addText = "";
						if (moneyOrItem == 0) {
                            if (Application.systemLanguage != SystemLanguage.Japanese) {
                                addText = " gave money " + getMoney + "to us.\n";
                            }else {
                                addText = "金" + getMoney + "を送って参りました。\n";
                            }
						} else {
							if (kahouOrShizai == 0) {
                                //kahou
                                if (Application.systemLanguage != SystemLanguage.Japanese) {
                                    addText = " gave treasure " + kahouName + "to us.\n";
                                }else {
                                    addText = "家宝、" + kahouName + "を送って参りました。\n";
                                }

                            }else {
                                //shizai＋
                                if (Application.systemLanguage != SystemLanguage.Japanese) {
                                    addText = " gave " + addQty  + " " + shigenName + " to us.\n";
                                }else {
                                    addText = shigenName + "を" + addQty + "個送って参りました。\n";
                                }

                            }
                        }

						int maxReduceValue = 5;
						int nowYukoudo = gaikou.getMyGaikou (daimyoId);
						int newYukoudo = gaikou.downMyGaikou (daimyoId, nowYukoudo, maxReduceValue);
						int reduceYukoudo = nowYukoudo - newYukoudo;
						GameObject.Find ("YukouValue").GetComponent<Text> ().text = newYukoudo.ToString ();
						closeScript.yukoudo = newYukoudo;
                        string reducceText = "";
                        if (Application.systemLanguage != SystemLanguage.Japanese) {   
                            reducceText = "Friendship decreased " + reduceYukoudo + " point";
                        }else {
                            reducceText = "友好度が" + reduceYukoudo + "下がりますぞ。";
                        }
                             

						OKtext = OKtext + addText + reducceText;
						msg.makeMessage (OKtext);

						downYukouOnIcon (daimyoId, newYukoudo);


					} else {
						//Failed
						audioSources [4].Play ();

						int maxReduceValue = 10;
						int nowYukoudo = gaikou.getMyGaikou (daimyoId);
						int newYukoudo = gaikou.downMyGaikou (daimyoId, nowYukoudo, maxReduceValue);
						int reduceYukoudo = nowYukoudo - newYukoudo;
						GameObject.Find ("YukouValue").GetComponent<Text> ().text = newYukoudo.ToString ();
						closeScript.yukoudo = newYukoudo;

                        //Message
                        string NGtext = "";
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            NGtext = daimyoName + " declined our proposal. \n" + "Friendship decreased " + reduceYukoudo + " point";
                        }else {
                            NGtext = daimyoName + "に体よく断られ申した。\n友好度が" + reduceYukoudo + "下がりますぞ。";
                        }
                        msg.makeMessage (NGtext);

						downYukouOnIcon (daimyoId, newYukoudo);

					}
					
					PlayerPrefs.Flush ();
					
					//Back
					GameObject.Find ("return").GetComponent<MenuReturn> ().OnClick ();

				} else if (name == "DoSyuppeiBtn") {

					reduceMoneyHyourou ();

					//Syuppei Check
					float percent = Random.value;
					percent = percent * 100;
					if (percent <= kyoutouRatio) {
						audioSources [3].Play ();

						//Success
						//Track
						int TrackSyuppeiNo = PlayerPrefs.GetInt("TrackSyuppeiNo",0);
						TrackSyuppeiNo = TrackSyuppeiNo + 1;
						PlayerPrefs.SetInt ("TrackSyuppeiNo", TrackSyuppeiNo);


						//Success
						addUsedBusyo (busyoId);

						//Process
						string path = "Prefabs/Map/Gunzei";
						GameObject Gunzei = Instantiate (Resources.Load (path)) as GameObject;			
						Gunzei.transform.SetParent (GameObject.Find ("Panel").transform);
						Gunzei.transform.localScale = new Vector2 (1, 1);

						//Location
						KuniInfo kuni = new KuniInfo();
						int srcX = kuni.getKuniLocationX(srcKuniId);
						int srcY = kuni.getKuniLocationY(srcKuniId);
						int dstX = kuni.getKuniLocationX(targetKuniId);
						int dstY = kuni.getKuniLocationY(targetKuniId);
						string direction = "";
						Gunzei gunzei = new Gunzei ();

						if(srcX < dstX){
							Gunzei.transform.localScale = new Vector2 (1, 1);
							direction = "right";
						}else{
							Gunzei.transform.localScale = new Vector2 (-1, 1);
							direction = "left";
							Gunzei.GetComponent<Gunzei> ().leftFlg = true;

						}

						int aveX = (srcX + dstX)/2;
						int aveY = (srcY + dstY)/2;
						RectTransform GunzeiTransform = Gunzei.GetComponent<RectTransform> ();
						GunzeiTransform.anchoredPosition = new Vector3 (aveX, aveY, 0);

						string key = srcKuniId.ToString () + "-" + targetKuniId.ToString ();
						Gunzei.GetComponent<Gunzei> ().key = key;
						Gunzei.GetComponent<Gunzei> ().srcKuni = srcKuniId;
						Gunzei.GetComponent<Gunzei> ().srcDaimyoId = srcDaimyoId;
						Gunzei.GetComponent<Gunzei> ().srcDaimyoName = srcDaimyoName;
						Gunzei.GetComponent<Gunzei> ().dstKuni = targetKuniId;
						Gunzei.GetComponent<Gunzei> ().dstDaimyoId = targetDaimyoId;
						Gunzei.GetComponent<Gunzei> ().dstDaimyoName = targetDaimyoName;
						int myHei = gunzei.heiryokuCalc (srcKuniId);

						//random myHei from -50%-myHeis
						List<float> randomPercent = new List<float>{0.8f,0.9f,1.0f};
						int rmd = UnityEngine.Random.Range(0,randomPercent.Count);
						float per = randomPercent [rmd];
						myHei = Mathf.CeilToInt (myHei * per);

						Gunzei.GetComponent<Gunzei> ().myHei = myHei;
						Gunzei.name = key;

						//Engun from Doumei
						Doumei doumei = new Doumei();
						List<string> doumeiDaimyoList = doumei.doumeiExistCheck(targetDaimyoId, srcDaimyoId.ToString());
                        bool dstEngunFlg = false;
						string dstEngunDaimyoId = ""; //2:3:5 
						string dstEngunHei = "";
						string dstEngunSts = ""; //BusyoId-BusyoLv-ButaiQty-ButaiLv:
						int totalEngunHei = 0;

						string seiryoku = PlayerPrefs.GetString ("seiryoku");
						char[] delimiterChars = {','};
						List<string> seiryokuList = new List<string>();
						seiryokuList = new List<string> (seiryoku.Split (delimiterChars));


						//Trace Check
						List<string> okDaimyoList = new List<string> ();
						List<string> checkList = new List<string> ();
						okDaimyoList = doumei.traceNeighborDaimyo(targetKuniId, targetDaimyoId, doumeiDaimyoList, seiryokuList, checkList,okDaimyoList);

						if(okDaimyoList.Count !=0){
							//Doumei & Neghbor Daimyo Exist

							for(int k=0; k<okDaimyoList.Count; k++){
								string engunDaimyo = okDaimyoList[k];
								int yukoudo = gaikou.getExistGaikouValue(int.Parse(engunDaimyo), targetDaimyoId);

								//engun check
								MainEventHandler mainEvent = new MainEventHandler();
								dstEngunFlg = mainEvent.CheckByProbability (yukoudo);

								if(dstEngunFlg){
									//Engun OK
									dstEngunFlg = true;
									if(dstEngunDaimyoId !=null && dstEngunDaimyoId !=""){
										dstEngunDaimyoId = dstEngunDaimyoId + ":" + engunDaimyo;
										string tempEngunSts = engunDaimyo + "-" + mainEvent.getEngunSts(engunDaimyo);
										int tempEngunHei = mainEvent.getEngunHei(tempEngunSts);
										dstEngunHei = dstEngunHei + ":" + tempEngunHei.ToString();
										totalEngunHei = totalEngunHei + tempEngunHei;
										dstEngunSts = dstEngunSts + ":" + tempEngunSts;

									}else{
										dstEngunDaimyoId = engunDaimyo;
										string tempEngunSts = engunDaimyo + "-" + mainEvent.getEngunSts(engunDaimyo);
										int tempEngunHei = mainEvent.getEngunHei(tempEngunSts);
										dstEngunHei = tempEngunHei.ToString();
										totalEngunHei = tempEngunHei;
										dstEngunSts = tempEngunSts;

									}
								}
							}
							Gunzei.GetComponent<Gunzei> ().dstEngunFlg = dstEngunFlg;
							Gunzei.GetComponent<Gunzei> ().dstEngunDaimyoId = dstEngunDaimyoId;
							Gunzei.GetComponent<Gunzei> ().dstEngunHei = dstEngunHei;
							Gunzei.GetComponent<Gunzei> ().dstEngunSts = dstEngunSts;
						}

						//Set Value
						//CreateTime,srcDaimyoId,dstDaimyoId,srcDaimyoName,dstDaimyoName, srcHei,locationX,locationY,left or right, engunFlg, engunDaimyoId(A:B:C), dstEngunHei(1000:2000:3000), dstEngunSts
						string keyValue = "";
						string createTime = System.DateTime.Now.ToString ();
						keyValue = createTime + "," + srcDaimyoId + "," + targetDaimyoId + "," + srcDaimyoName + "," + targetDaimyoName + "," + myHei + "," + aveX + "," + aveY + "," + direction + "," + dstEngunFlg + "," + dstEngunDaimyoId + "," + dstEngunHei + ","+ dstEngunSts;
						PlayerPrefs.SetString (key, keyValue);
						string keyHistory = PlayerPrefs.GetString ("keyHistory");
						if(keyHistory == null || keyHistory == ""){
							keyHistory = key;
						}else{
							keyHistory = keyHistory + "," + key;
						}
						PlayerPrefs.SetString ("keyHistory", keyHistory);


                        //Msg
                        string OKtext = "";
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            OKtext = "Lord " + daimyoName + " is sending " + myHei + " soldiers to " + kuniName;
                        }else {
                            OKtext = daimyoName + "殿が" + kuniName + "攻めのため、\n" + myHei + "人の兵を起こしましたぞ。";
                        }

                            string AddText = "";
                        if(totalEngunHei !=0) {
                            if (Application.systemLanguage != SystemLanguage.Japanese) {
                                AddText = targetDaimyoName + " has a support army " + totalEngunHei + " soldiers";
                            }else {
                                AddText = targetDaimyoName + "に" + totalEngunHei + "の援軍がいるようです。";
                            }
                                
                            OKtext = OKtext + "\n" + AddText;
                        }
						msg.makeMessage (OKtext);


					} else {
						//Fail
						audioSources [4].Play ();

						int maxReduceValue = 3;
						int nowYukoudo = gaikou.getMyGaikou (daimyoId);
						int newYukoudo = gaikou.downMyGaikou (daimyoId, nowYukoudo, maxReduceValue);
						int reduceYukoudo = nowYukoudo - newYukoudo;
						GameObject.Find ("YukouValue").GetComponent<Text> ().text = newYukoudo.ToString (); 	
						closeScript.yukoudo = newYukoudo;

                        string NGtext = "";
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            NGtext = daimyoName + " declined our proposal. \n" + "Friendship decreased " + reduceYukoudo + " point";
                        }else {
                            NGtext = daimyoName + "に体よく断られ申した。\n友好度が" + reduceYukoudo + "下がりますぞ。";
                        }
                        msg.makeMessage (NGtext);

						downYukouOnIcon (daimyoId, newYukoudo);

					}

					PlayerPrefs.Flush ();

					//Back
					GameObject.Find ("return").GetComponent<MenuReturn> ().OnClick ();

				}

			} else {
				//Message
				audioSources [4].Play ();

				string NGtext = msg.getMessage(6);
				msg.makeMessage (NGtext);
			
			}
		} else {
			//Message
			audioSources [4].Play ();

			//string NGtext = msg.getMessage(7);
			//msg.makeMessage (NGtext);
            msg.hyourouMovieMessage();
        }
	}

	public void reduceMoneyHyourou (){

		//Reduce Hyourou
		int nowHyourou = PlayerPrefs.GetInt ("hyourou");
		int newHyourou = nowHyourou - 5;
		PlayerPrefs.SetInt ("hyourou", newHyourou);
		GameObject.Find ("HyourouCurrentValue").GetComponent<Text> ().text = newHyourou.ToString ();
		
		//Reduce Money
		int nowMoney = PlayerPrefs.GetInt ("money");
		int newMoney = nowMoney - paiedMoney;
        PlayerPrefs.SetInt ("money", newMoney);
		GameObject.Find ("MoneyValue").GetComponent<Text> ().text = newMoney.ToString ();

		int TrackGaikouMoneyNo = PlayerPrefs.GetInt("TrackGaikouMoneyNo",0);
		TrackGaikouMoneyNo = TrackGaikouMoneyNo + paiedMoney;
		PlayerPrefs.SetInt("TrackGaikouMoneyNo",TrackGaikouMoneyNo);
	}

	public void upYukouOnIcon(int daimyoId, int value){
		GameObject kuniIconView = GameObject.Find ("KuniIconView").gameObject;

		foreach (Transform obj in kuniIconView.transform) {
			if (obj.GetComponent<SendParam> ().daimyoId == daimyoId) {
				obj.GetComponent<SendParam> ().myYukouValue = value;
			}
		}
	}

	public void downYukouOnIcon(int daimyoId, int value){
		GameObject kuniIconView = GameObject.Find ("KuniIconView").gameObject;

		foreach (Transform obj in kuniIconView.transform) {
			if (obj.GetComponent<SendParam> ().daimyoId == daimyoId) {
				obj.GetComponent<SendParam> ().myYukouValue = value;
			}
		}
	}

	public void addUsedBusyo(int busyoId){

		string usedBusyo = PlayerPrefs.GetString ("usedBusyo");
		string newUsedBusyo = "";

		if (usedBusyo != null && usedBusyo != "") {
			newUsedBusyo = usedBusyo + "," + busyoId.ToString();
		} else {
			newUsedBusyo = busyoId.ToString();
		}

		PlayerPrefs.SetString ("usedBusyo",newUsedBusyo);
		PlayerPrefs.Flush ();
	}




}
