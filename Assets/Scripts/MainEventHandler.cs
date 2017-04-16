using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class MainEventHandler : MonoBehaviour {
	//SE
	AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

	//Base
	public int kassenRatio = 50;
	//public int kassenRatio = 0;
	public int gaikouRatio = 50;

	//Indivisual
	public int yukoudoUpMissRatio = 30;
	public int yukoudoDownMissRatio = 30;
	public bool shisyaSceneFlg = false;

	Daimyo daimyo = new Daimyo ();
	Gaikou gaikou = new Gaikou ();

    //shisya
    public int syogunShisyaRatio = 10;
    public int cyouteiShisyaRatio = 10;
    public int otherShisyaRatio = 10;

    //Event History for Pointer
    public List<int> kassenDaimyoList = new List<int>();
    public List<int> upDaimyo1List = new List<int>();
    public List<int> upDaimyo2List = new List<int>();
    public List<int> downDaimyo1List = new List<int>();
    public List<int> downDaimyo2List = new List<int>();

    public void mainHandler(){
		
		/*Basic Info*/
		//make kuni list
		string seiryoku = PlayerPrefs.GetString ("seiryoku");
		List<string> seiryokuList = new List<string> ();
		char[] delimiterChars = {','};
		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
		int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
		GameObject targetKuni = GameObject.Find ("KuniIconView");

		/*Kassen*/
		//1. Randome choice Kassen Qty between 0,1,2,3
		//2. Ratio 50% check
		//3. Random choice Kassen Souce Kuni 1-65
		//4. Destination Check by Mapping
		//		if Same Daimyo between Kassen Souce & Destination Kuni => Skip
		//      else => Make guntai Instance
		//        	 4-1. Destination == myDaimyo
		//			 4-2. Destination == has own relation or master relation  
		//

		//Message
		List<string> messageList = new List<string> ();

		//Count Kassen Qty
		int kassenQty = CountEnemyKassenAction ();

		for (int i=1; i<kassenQty+1; i++) {
			//Check Kassen or Not

			bool kassenFlg = CheckByProbability (kassenRatio);
			if (kassenFlg == true) {
				//target kuni extraction

				while (true) {
					int randomKuni = UnityEngine.Random.Range (1, 66);
					string eDaimyo = seiryokuList [randomKuni - 1];
					string tDaimyo = "";
					bool doumeiFlg = false;

					if (eDaimyo != myDaimyo.ToString ()) {
						//Check Kuni Mapping
						List<int> targetKuniList = new List<int> ();
						KuniInfo kuni = new KuniInfo ();
						targetKuniList = kuni.getMappingKuni (randomKuni);
						
						//Yukoudo Check
						int worstGaikouDaimyo = 0;
						int worstGaikouValue = 100;
						int worstGaikouKuni = 0;
						int worstHeiryokuValue = 100000000;

						SendParam srcSendParam = targetKuni.transform.FindChild (randomKuni.ToString ()).GetComponent<SendParam> ();
						bool aggressiveFlg = srcSendParam.aggressiveFlg;

						for (int k=0; k<targetKuniList.Count; k++) {						
							SendParam sendParam = targetKuni.transform.FindChild (targetKuniList [k].ToString ()).GetComponent<SendParam> ();

							if (aggressiveFlg) {
								//Find worst gaikou daimyo


								tDaimyo = seiryokuList [targetKuniList [k] - 1];
								int gaikouValue = 0;

								if (eDaimyo != tDaimyo) {
									if (tDaimyo == myDaimyo.ToString ()) {
										//Get Gaikou Value

										string eGaikouM = "gaikou" + eDaimyo;
										gaikouValue = PlayerPrefs.GetInt (eGaikouM);

									} else {
										//Gaikou Data Check
										string gaikouTemp = "";
										if (int.Parse (eDaimyo) < int.Parse (tDaimyo)) {
											gaikouTemp = eDaimyo + "gaikou" + tDaimyo;
										} else {
											gaikouTemp = tDaimyo + "gaikou" + eDaimyo;
										}

										if (PlayerPrefs.HasKey (gaikouTemp)) {
											//exsit
											gaikouValue = PlayerPrefs.GetInt (gaikouTemp);

										} else {
											//non exist
											//gaikou check
											gaikouValue = gaikou.getGaikouValue (int.Parse (eDaimyo), int.Parse (tDaimyo));

										}
									}

									//Compare with Previous one
									if (worstGaikouValue > gaikouValue) {
										worstGaikouValue = gaikouValue;
										worstGaikouDaimyo = int.Parse (tDaimyo);
										worstGaikouKuni = targetKuniList [k];
									}
								}
							
							
							} else {
								//Find worst heiryoku daimyo

								tDaimyo = seiryokuList [targetKuniList [k] - 1];

								if (eDaimyo != tDaimyo) {								
									//Heiryoku Check
									int heiryoku = sendParam.heiryoku;

									//Compare with Previous one
									if (worstHeiryokuValue > heiryoku) {
										worstHeiryokuValue = heiryoku;

										int gaikouValue = 0;
										if (tDaimyo == myDaimyo.ToString ()) {
											//Get Gaikou Value
											string eGaikouM = "gaikou" + eDaimyo;
											gaikouValue = PlayerPrefs.GetInt (eGaikouM);

										} else {
											//Gaikou Data Check
											string gaikouTemp = "";
											if (int.Parse (eDaimyo) < int.Parse (tDaimyo)) {
												gaikouTemp = eDaimyo + "gaikou" + tDaimyo;
											} else {
												gaikouTemp = tDaimyo + "gaikou" + eDaimyo;
											}
											if (PlayerPrefs.HasKey (gaikouTemp)) {
												//exsit
												gaikouValue = PlayerPrefs.GetInt (gaikouTemp);
											} else {
												//non exist
												gaikouValue = gaikou.getGaikouValue (int.Parse (eDaimyo), int.Parse (tDaimyo));
											}
										}
										worstGaikouValue = gaikouValue;
										worstGaikouDaimyo = int.Parse (tDaimyo);
										worstGaikouKuni = targetKuniList [k];

									}
								}

							}

						}//Loop End

						//Create Guntai Instance
						if (worstGaikouValue != 100) {
							int kassenRatio2 = 100 - worstGaikouValue;

							//doumei check
							string doumeiCheck = "doumei" + eDaimyo;
							if(PlayerPrefs.HasKey(doumeiCheck)){
								string cDoumei = PlayerPrefs.GetString(doumeiCheck);
								List<string> cDoumeiList = new List<string>();
								if(cDoumei.Contains(",")){
									cDoumeiList = new List<string> (cDoumei.Split (delimiterChars));

								}else{
									cDoumeiList.Add(cDoumei);
								}

								//If Doumei Daimyo -> Half Ratio
								if(cDoumeiList.Contains(worstGaikouDaimyo.ToString())){
									doumeiFlg = true;
									kassenRatio2 = kassenRatio2/2;
								}else{
									doumeiFlg = false;
								}
							}

							bool GaikouValueFlg = CheckByProbability (kassenRatio2);
							if (GaikouValueFlg) {
								//Make Guntai Instance
								string key = randomKuni.ToString () + "-" + worstGaikouKuni.ToString ();
								bool ExistFlg = false;

								//Exist Check Same Daimyo
								foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Gunzei")){
									int gunzeiSrcDaimyoId = obs.GetComponent<Gunzei>().srcDaimyoId;

									if(int.Parse(eDaimyo) == gunzeiSrcDaimyoId){
										ExistFlg = true;
									}
								}

								if(!ExistFlg){
									string path = "Prefabs/Map/Gunzei";
									GameObject Gunzei = Instantiate (Resources.Load (path)) as GameObject;			
									Gunzei.transform.SetParent (GameObject.Find ("Panel").transform);
									Gunzei.transform.localScale = new Vector2 (1, 1);

									//Location
									int srcX = kuni.getKuniLocationX(randomKuni);
									int srcY = kuni.getKuniLocationY(randomKuni);
									int dstX = kuni.getKuniLocationX(worstGaikouKuni);
									int dstY = kuni.getKuniLocationY(worstGaikouKuni);
									string direction = "";
									Gunzei gunzei = new Gunzei ();

									if(srcX < dstX){
										Gunzei.transform.localScale = new Vector2 (1, 1);
										direction = "right";
									}else{
										Gunzei.transform.localScale = new Vector2 (-1, 1);
                                        Gunzei.transform.FindChild("MsgBack").localScale = new Vector2(-1, 1);
                                        direction = "left";
										Gunzei.GetComponent<Gunzei> ().leftFlg = true;

									}

									int aveX = (srcX + dstX)/2;
									int aveY = (srcY + dstY)/2;
									RectTransform GunzeiTransform = Gunzei.GetComponent<RectTransform> ();
									GunzeiTransform.anchoredPosition = new Vector3 (aveX, aveY, 0);

									Gunzei.GetComponent<Gunzei> ().key = key;
									Gunzei.GetComponent<Gunzei> ().srcKuni = randomKuni;
									Gunzei.GetComponent<Gunzei> ().srcDaimyoId = int.Parse (eDaimyo);
									string srcDaimyoName = daimyo.getName (int.Parse (eDaimyo));
									Gunzei.GetComponent<Gunzei> ().srcDaimyoName = srcDaimyoName;
									Gunzei.GetComponent<Gunzei> ().dstKuni = worstGaikouKuni;
									Gunzei.GetComponent<Gunzei> ().dstDaimyoId = worstGaikouDaimyo;
									string dstDaimyoName = daimyo.getName (worstGaikouDaimyo);
									Gunzei.GetComponent<Gunzei> ().dstDaimyoName = dstDaimyoName;
									int myHei = gunzei.heiryokuCalc (randomKuni);

									//random myHei from -50%-myHei
									List<float> randomPercent = new List<float>{0.8f,0.9f,1.0f};
									int rmd = UnityEngine.Random.Range(0,randomPercent.Count);
									float per = randomPercent [rmd];
									myHei = Mathf.CeilToInt (myHei * per);

									Gunzei.GetComponent<Gunzei> ().myHei = myHei;
									Gunzei.name = key;

									//Engun from Doumei
									Doumei doumei = new Doumei();
									List<string> doumeiDaimyoList = new List<string> ();
									bool dstEngunFlg = false;
									string dstEngunDaimyoId = ""; //2:3:5 
									string dstEngunHei = "";
									string dstEngunSts = ""; //Daimyo-BusyoId-BusyoLv-ButaiQty-ButaiLv:
									int totalEngunHei = 0;

									doumeiDaimyoList = doumei.doumeiExistCheck(worstGaikouDaimyo,eDaimyo);

									if(doumeiDaimyoList.Count != 0){
										//Doumei Exist

										//Trace Check
										List<string> okDaimyoList = new List<string> ();
										List<string> checkList = new List<string> ();
										okDaimyoList = doumei.traceNeighborDaimyo(worstGaikouKuni, worstGaikouDaimyo, doumeiDaimyoList, seiryokuList, checkList,okDaimyoList);

										if(okDaimyoList.Count !=0){
											//Doumei & Neghbor Daimyo Exist

											for(int k=0; k<okDaimyoList.Count; k++){
												string engunDaimyo = okDaimyoList[k];

												if (int.Parse (engunDaimyo) != myDaimyo) {
													int yukoudo = gaikou.getExistGaikouValue (int.Parse (engunDaimyo), worstGaikouDaimyo);

													//engun check
													dstEngunFlg = CheckByProbability (yukoudo);
													if (dstEngunFlg) {
														//Engun OK
														dstEngunFlg = true;
														if (dstEngunDaimyoId != null && dstEngunDaimyoId != "") {
															dstEngunDaimyoId = dstEngunDaimyoId + ":" + engunDaimyo;
															string tempEngunSts = engunDaimyo + "-" + getEngunSts(engunDaimyo);
															int tempEngunHei = getEngunHei (tempEngunSts);
															dstEngunHei = dstEngunHei + ":" + tempEngunHei.ToString ();
															totalEngunHei = totalEngunHei + tempEngunHei;
															dstEngunSts = dstEngunSts + ":" + tempEngunSts;

														} else {
															dstEngunDaimyoId = engunDaimyo;
															string tempEngunSts = engunDaimyo + "-" + getEngunSts(engunDaimyo);
															int tempEngunHei = getEngunHei (tempEngunSts);
															dstEngunHei = tempEngunHei.ToString ();
															totalEngunHei = tempEngunHei;
															dstEngunSts = tempEngunSts;

														}
													}
												} else {
													//my daimyo engun
													string doumeiDaimyoName = daimyo.getName(worstGaikouDaimyo);
													messageList = MakeEngunShisya(3, key,worstGaikouDaimyo, doumeiDaimyoName, worstGaikouKuni, int.Parse (eDaimyo), messageList);

												
												}
											}
											Gunzei.GetComponent<Gunzei> ().dstEngunFlg = dstEngunFlg;
											Gunzei.GetComponent<Gunzei> ().dstEngunDaimyoId = dstEngunDaimyoId;
											Gunzei.GetComponent<Gunzei> ().dstEngunHei = dstEngunHei;
											Gunzei.GetComponent<Gunzei> ().dstEngunSts = dstEngunSts;
										}
									}

									//Set Value
									//CreateTime,srcDaimyoId,dstDaimyoId,srcDaimyoName,dstDaimyoName, srcHei,locationX,locationY,left or right, engunFlg, engunDaimyoId(A:B:C), dstEngunHei(1000:2000:3000), dstEngunSts
									string keyValue = "";
									string createTime = System.DateTime.Now.ToString ();
									keyValue = createTime + "," + eDaimyo + "," + worstGaikouDaimyo + "," + srcDaimyoName + "," + dstDaimyoName + "," + myHei + "," + aveX + "," + aveY + "," + direction + "," + dstEngunFlg + "," + dstEngunDaimyoId + "," + dstEngunHei + ","+ dstEngunSts;
									PlayerPrefs.SetString (key, keyValue);
									string keyHistory = PlayerPrefs.GetString ("keyHistory");
									if(keyHistory == null || keyHistory == ""){
										keyHistory = key;
									}else{
										keyHistory = keyHistory + "," + key;
									}
									PlayerPrefs.SetString ("keyHistory", keyHistory);

									if (worstGaikouDaimyo == myDaimyo) {
										string enemyDaimyoName = daimyo.getName (int.Parse(eDaimyo));
										messageList = MakeKyouhakuShisya (8,key,int.Parse(eDaimyo),enemyDaimyoName,worstGaikouKuni,messageList);

                                        if(CheckMyDoumei(int.Parse(eDaimyo))) {
                                            string myAttackedDoumei = PlayerPrefs.GetString("doumei");
                                            List<string> myAttackedDoumeiList = new List<string>();
                                            if (myAttackedDoumei != null && myAttackedDoumei != "") {
                                                if (myAttackedDoumei.Contains(",")) {
                                                    myAttackedDoumeiList = new List<string>(myAttackedDoumei.Split(delimiterChars));
                                                }
                                                else {
                                                    myAttackedDoumeiList.Add(myAttackedDoumei);
                                                }
                                            }
                                            ClearMyDoumei(eDaimyo, myAttackedDoumeiList, myDaimyo);
                                            messageList = messageList = MakeShisya(6, srcDaimyoName, messageList, int.Parse(eDaimyo));
                                        }
									}

									flush ();

									string kassenText = "";
									if(!dstEngunFlg){
                                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                                            kassenText = srcDaimyoName + " is attacking " + dstDaimyoName + " by " + myHei + " soldiers";
                                        }else { 
                                            kassenText = srcDaimyoName + "が" + dstDaimyoName + "討伐の兵" + myHei + "人を起こしました。";
                                        }
                                    }else{
                                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                                            kassenText = srcDaimyoName + " is attacking " + dstDaimyoName + " by " + myHei + " soldiers"+"\n" + "Defender's allianced country is coming to support by " + totalEngunHei + " soldiers"; ;
                                        }else {
                                            kassenText = srcDaimyoName + "が" + dstDaimyoName + "討伐の兵" + myHei + "人を起こしました。\n防衛側の同盟国が援軍" + totalEngunHei + "人を派兵しました。";
                                        }
                                    }
									messageList.Add (kassenText);
                                   
                                    //pointer
                                    kassenDaimyoList.Add(int.Parse(eDaimyo));



                                    if (doumeiFlg){
										//Delete doumei
										doumei.deleteDoumei(eDaimyo, worstGaikouDaimyo.ToString());

										//Zero Gaikou
										string tempYukou = "";
										if(tDaimyo == myDaimyo.ToString()){
											tempYukou = "gaikou" + eDaimyo;
										}else{
											if(worstGaikouDaimyo < int.Parse(eDaimyo)){
												tempYukou = worstGaikouDaimyo.ToString() + "gaikou" + eDaimyo;
											}else{
												tempYukou = eDaimyo + "gaikou" + worstGaikouDaimyo.ToString();
											}
										}
										PlayerPrefs.SetInt(tempYukou,0);


										PlayerPrefs.Flush ();

									}
								}
							}
						}
						break;
					}
				}
			}
		}
	
		/************/
		/***Gaikou***/
		/************/
		int gaikouQty = CountEnemyGaikouAction ();
		
		for (int i=1; i<gaikouQty+1; i++) {

			bool gaikouFlg = CheckByProbability (gaikouRatio);

			if (gaikouFlg == true) {
				//Do gaikou

				int randomKuni = UnityEngine.Random.Range (1, 66);
				int srcDaimyoId = int.Parse (seiryokuList [randomKuni - 1]);
				string srcDaimyoName = daimyo.getName (srcDaimyoId);

				if (srcDaimyoId != myDaimyo) {	

					List<int> targetKuniList = new List<int> ();
					KuniInfo kuni = new KuniInfo ();
					targetKuniList = kuni.getMappingKuni (randomKuni);

					//Yukoudo Check
					int bestGaikouDaimyo = 0;
					int bestGaikouValue = 0;
					int bestGaikouKuni = 0;
					int bestHeiryokuValue = 0;

					SendParam sendParamSrc = targetKuni.transform.FindChild (randomKuni.ToString ()).GetComponent<SendParam> ();
					bool aggressiveFlg = sendParamSrc.aggressiveFlg;

					for (int k=0; k<targetKuniList.Count; k++) {

						SendParam sendParam = targetKuni.transform.FindChild (targetKuniList [k].ToString ()).GetComponent<SendParam> ();
						if (aggressiveFlg) {
							//Find best gaikou daimyo

							int dstDaimyoId = int.Parse (seiryokuList [targetKuniList [k] - 1]);
							int gaikouValue = 0;

							if (srcDaimyoId != dstDaimyoId) {
								if (dstDaimyoId == myDaimyo) {
									//Get Gaikou Value

									string eGaikouM = "gaikou" + srcDaimyoId;
									gaikouValue = PlayerPrefs.GetInt (eGaikouM);

								} else {
									//Gaikou Data Check
									string gaikouTemp = "";
									if (srcDaimyoId < dstDaimyoId) {
										gaikouTemp = srcDaimyoId + "gaikou" + dstDaimyoId;
									} else {
										gaikouTemp = dstDaimyoId + "gaikou" + srcDaimyoId;
									}

									if (PlayerPrefs.HasKey (gaikouTemp)) {
										//exsit
										gaikouValue = PlayerPrefs.GetInt (gaikouTemp);

									} else {
										//non exist
										//gaikou check
										gaikouValue = gaikou.getGaikouValue (srcDaimyoId, dstDaimyoId);

									}
								}


								//Compare with Previous one
								//Best one
								if (gaikouValue > bestGaikouValue) {
									bestGaikouValue = gaikouValue;
									bestGaikouDaimyo = dstDaimyoId;
									bestGaikouKuni = targetKuniList [k];

								}
							}


						} else {
							//Find best heiryoku daimyo
							int dstDaimyoId = int.Parse (seiryokuList [targetKuniList [k] - 1]);

							if (srcDaimyoId != dstDaimyoId) {								
								//Heiryoku Check
								int heiryoku = sendParam.heiryoku;

								//Compare with Previous one
								if (heiryoku >= bestHeiryokuValue) {
									bestHeiryokuValue = heiryoku;

									int gaikouValue = 0;
									if (dstDaimyoId == myDaimyo) {
										//Get Gaikou Value
										string eGaikouM = "gaikou" + srcDaimyoId;
										gaikouValue = PlayerPrefs.GetInt (eGaikouM);

									} else {
										//Gaikou Data Check
										string gaikouTemp = "";
										if (srcDaimyoId < dstDaimyoId) {
											gaikouTemp = srcDaimyoId + "gaikou" + dstDaimyoId;
										} else {
											gaikouTemp = dstDaimyoId + "gaikou" + srcDaimyoId;
										}
										if (PlayerPrefs.HasKey (gaikouTemp)) {
											//exsit
											gaikouValue = PlayerPrefs.GetInt (gaikouTemp);
										} else {
											//non exist
											gaikouValue = gaikou.getGaikouValue (srcDaimyoId, dstDaimyoId);
										}
									}
									bestGaikouValue = gaikouValue;
									bestGaikouDaimyo = dstDaimyoId;
									bestGaikouKuni = targetKuniList [k];

								}


							}

						}

					}//Loop End


					//Gaikou Action
					float percent = UnityEngine.Random.value;
					percent = percent * 100;

					int gaikouAction = 0;

					if (bestGaikouDaimyo != 0) {
						if (bestGaikouDaimyo == myDaimyo) {
							if (percent <= 40) {
								gaikouAction = 1; //Mitsugi
							} else if (40 < percent && percent <= 70) {
								gaikouAction = 2; //Ryugen
							} else if (70 < percent && percent <= 75) {
								gaikouAction = 3; //Doumei
							} else if (75 < percent && percent <= 85) {
								gaikouAction = 4; //Doukatsu
							} else if (85 < percent && percent <= 95) {
								gaikouAction = 5; //Koueki
							} else if (95 < percent && percent <= 100) {
								gaikouAction = 6; //Oocyanoe
							}
						} else {
							if (percent <= 40) {
								gaikouAction = 1; //Mitsugi
							} else if (40 < percent && percent <= 98) {
								gaikouAction = 2; //Ryugen
							} else if (98 < percent && percent <= 100) {
								gaikouAction = 3; //Doumei
							}
						}
					}

					if (gaikouAction == 1) {
						//1. yukoudo up(Mitsugimono) 

						bool yukoudoUpFlg = CheckByProbability (yukoudoUpMissRatio);

						if (yukoudoUpFlg == true) {
							//Choose Target Daimyo

							if (bestGaikouDaimyo != 0) {
								string dstDaimyoName = daimyo.getName (bestGaikouDaimyo);
								Gaikou gaikou = new Gaikou ();
								int yukoudo = 0;

								if (bestGaikouDaimyo == myDaimyo) {
									messageList = MakeShisya (7, srcDaimyoName, messageList, srcDaimyoId);
									yukoudo = gaikou.getMyGaikou (srcDaimyoId);

								} else {
									//In the case between other daimyos
									int addYukoudo = UpYukouValueWithOther (srcDaimyoId, bestGaikouDaimyo);
                                    string yukouUpText = "";
                                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                                        yukouUpText = srcDaimyoName + " gave gift to " + dstDaimyoName + ". Friendship increased " + addYukoudo + " point";
                                    }else {
                                        yukouUpText = srcDaimyoName + "が" + dstDaimyoName + "に貢物をしました。友好度が" + addYukoudo + "上がります。";
                                    }
                                    upDaimyo1List.Add(srcDaimyoId);
                                    upDaimyo2List.Add(bestGaikouDaimyo);

                                    messageList.Add (yukouUpText);
									yukoudo = gaikou.getOtherGaikouValue(srcDaimyoId,  bestGaikouDaimyo);
								}

								//Doumei Vonus
								if (CheckByProbability (yukoudo / 10)) {
									messageList = makeDoumei (bestGaikouDaimyo, myDaimyo, srcDaimyoId, srcDaimyoName, messageList);
								}
							}
						}

					} else if (gaikouAction == 2) {
						//2. yukoudo down(Ryugen)

						bool yukoudoDownFlg = CheckByProbability (yukoudoDownMissRatio);

						if (yukoudoDownFlg == true) {
							//Choose Target Daimyo
							int reduceYukoudo = 0;

							if (bestGaikouDaimyo != 0) {
								if (bestGaikouDaimyo == myDaimyo) {
									//My Daimyo
									string dstDaimyoName = "";
									reduceYukoudo = DownYukouValueWithMyDaimyo (myDaimyo, srcDaimyoId);
									dstDaimyoName = daimyo.getName (srcDaimyoId);
                                    string yukouDownText = "";
                                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                                        yukouDownText = "Someone spread bad rumor between your country and " + dstDaimyoName + ". Friendship decreased " + reduceYukoudo + " point";
                                    }else {
                                        yukouDownText = "何者かが当家と" + dstDaimyoName + "間に流言を流し、友好度が" + reduceYukoudo + "下がりました。";
                                    }
                                    messageList.Add (yukouDownText);
                                    downDaimyo1List.Add(myDaimyo);
                                    downDaimyo2List.Add(srcDaimyoId);

                                } else {
									reduceYukoudo = DownYukouValueWithOther (srcDaimyoId, bestGaikouDaimyo);
									string dst1stDaimyoName = daimyo.getName (srcDaimyoId);
									string dst2ndDaimyoName = daimyo.getName (bestGaikouDaimyo);
                                    string yukouDownText = "";
                                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                                       yukouDownText = "Someone spread bad rumor between " + dst1stDaimyoName + " and " + dst2ndDaimyoName + ". Friendship decreased " + reduceYukoudo + " point";
                                    }else {
                                       yukouDownText = "何者かが" + dst1stDaimyoName + "と" + dst2ndDaimyoName + "間に流言を流し、友好度が" + reduceYukoudo + "下がりました。";
                                    }
                                    messageList.Add (yukouDownText);
                                    downDaimyo1List.Add(srcDaimyoId);
                                    downDaimyo2List.Add(bestGaikouDaimyo);
                                }
							}
						}

					} else if (gaikouAction == 3) {
						//3. doumei
						Gaikou gaiko = new Gaikou();
						int yukoudo = gaikou.getExistGaikouValue(srcDaimyoId, bestGaikouDaimyo);
						bool doumeiFlg = CheckByProbability (yukoudo/2);

						if (doumeiFlg == true) {
							messageList = makeDoumei (bestGaikouDaimyo, myDaimyo, srcDaimyoId, srcDaimyoName, messageList);
						}


					} else if (gaikouAction == 4) {
						//Doukatsu
						messageList = MakeShisya(4, srcDaimyoName, messageList, srcDaimyoId);
					} else if (gaikouAction == 5) {
						//Koueki
						messageList = MakeShisya(5, srcDaimyoName, messageList, srcDaimyoId);
					} else if (gaikouAction == 6) {
						//Oocyanoe
						messageList = MakeShisya(9, srcDaimyoName, messageList, srcDaimyoId);
					}
				}
			}
		}

		/************/
		/***Syogun***/
		/************/
		//Exist Check
		if(CheckByProbability (syogunShisyaRatio)){
			int syogunDaimyoId = PlayerPrefs.GetInt("syogunDaimyoId");
			if (syogunDaimyoId != myDaimyo) {

				string syogunDaimyoName = daimyo.getName (syogunDaimyoId);
				if (seiryokuList.Contains (syogunDaimyoId.ToString ())) {
					if(CheckByProbability (20)){

						float percent = UnityEngine.Random.value;
						percent = percent * 100;

						if (percent <= 50) {
							//Musin
							messageList = MakeShisya(14, syogunDaimyoName, messageList,syogunDaimyoId);
						} else if (50 < percent && percent <= 80) {
							//Cyusai
							List<int> targetDaimyoList = new List<int>();
							for (int i = 0; i < seiryokuList.Count; i++) {
								int tmpDaimyo = int.Parse(seiryokuList [i]);
								if (tmpDaimyo != myDaimyo && tmpDaimyo != syogunDaimyoId) {
									targetDaimyoList.Add (tmpDaimyo);
								}
							}
							if (targetDaimyoList.Count != 0) {
								int rdm = UnityEngine.Random.Range (0, targetDaimyoList.Count);
								int randomDaimyoId = targetDaimyoList [rdm];
								messageList = MakeShisya (10, syogunDaimyoName, messageList, randomDaimyoId);
							}
						} else if (80 < percent && percent <= 90) {
							//Toubatus
							//Check Toubatsu Target Exist
							bool existFlg = false;
							int targetDaimyoId = 0;
							int maxKuniQty = 0;
							int daimyoCount = 0;
							GameObject KuniIconView = GameObject.Find ("KuniIconView").gameObject;
							List<int> checkedDaimyoList = new List<int> ();
							foreach (Transform kuni in KuniIconView.transform) {
								int tmpDaimyoId = kuni.GetComponent<SendParam> ().daimyoId;
								if (tmpDaimyoId != syogunDaimyoId) {
									if (!checkedDaimyoList.Contains (tmpDaimyoId)) {
										checkedDaimyoList.Add (tmpDaimyoId);
										int qty = kuni.GetComponent<SendParam> ().kuniQty;
										if (qty > maxKuniQty) {
											maxKuniQty = qty;
											targetDaimyoId = tmpDaimyoId;
										}
									}	
								}
							}
							if (checkedDaimyoList.Count != 1) { //not just my self
                                if(targetDaimyoId != myDaimyo) {
								    messageList = MakeShisya (11, syogunDaimyoName, messageList, targetDaimyoId);
                                }
                            }

						} else if (90 < percent && percent <= 100) {
							//Bouei
							bool gunzeiFlg = false;
							string key = "";
							int srcDaimyoId = 0;
							int dstDaimyoId = 0;
							int dstKuniId = 0;

							foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Gunzei")){
								srcDaimyoId = obs.GetComponent<Gunzei>().srcDaimyoId;
								if(srcDaimyoId != syogunDaimyoId && srcDaimyoId != myDaimyo){
									dstDaimyoId = obs.GetComponent<Gunzei>().dstDaimyoId;
									if (dstDaimyoId != myDaimyo) {
										dstKuniId = obs.GetComponent<Gunzei>().dstKuni;
										KuniInfo kuni = new KuniInfo ();
										List<int> mappingKuniList = new List<int> ();
										mappingKuniList = kuni.getMappingKuni (dstKuniId);

										for (int i = 0; i < mappingKuniList.Count; i++) {
											int tmp = mappingKuniList [i];
											if (int.Parse(seiryokuList [tmp - 1]) == myDaimyo) {
												key = obs.GetComponent<Gunzei>().key;
												gunzeiFlg = true;
												break;
											}
										}
										if (gunzeiFlg) {
											break;
										}

									}
								}
							}
							if (gunzeiFlg) {
								messageList = MakeBoueiShisya (12, key, syogunDaimyoName, srcDaimyoId, dstDaimyoId, dstKuniId, messageList);
							}
						} 
					}

				} else {
                    //Not Exist
					if(CheckByProbability (10)){

						bool existFlg1 = false;
						int maxDaimyoId = 0;
						int maxKuniQty = 0;
						GameObject KuniIconView = GameObject.Find ("KuniIconView").gameObject;
						List<int> checkedDaimyoList = new List<int> ();
						foreach (Transform kuni in KuniIconView.transform) {
							int tmpDaimyoId = kuni.GetComponent<SendParam> ().daimyoId;							
							if (!checkedDaimyoList.Contains (tmpDaimyoId)) {
								checkedDaimyoList.Add (tmpDaimyoId);
								int qty = kuni.GetComponent<SendParam> ().kuniQty;
								if (qty > maxKuniQty) {
									maxKuniQty = qty;
									maxDaimyoId = tmpDaimyoId;
									existFlg1 = true;
								}
							}
						}

						//Check
						bool existFlg2 = false;
						if (existFlg1) {
							List<int> needKuni = new List<int>{6,11,12,13,16,17,38,39};
							for (int i = 0; i < needKuni.Count; i++) {
								int tmpDaimyo = int.Parse(seiryokuList[needKuni [i]-1]);

								if (tmpDaimyo == maxDaimyoId) {
									existFlg2 = true;
								} else {
									existFlg2 = false;
								}
							}
						}

						if (existFlg2) {
							if (CheckByProbability (20)) {
								//Syogun syunin
                                if(maxDaimyoId != myDaimyo) {
								    PlayerPrefs.SetInt("syogunDaimyoId",maxDaimyoId);
								    string maxDaimyoName = daimyo.getName (maxDaimyoId);
                                    string yukouUpText = "";
                                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                                        yukouUpText = maxDaimyoName + " was assigned to shogun. New shogunate has been opened.";
                                    }else {
                                        yukouUpText = maxDaimyoName + "殿が征夷大将軍に任命されました。新たな幕府が開かれます。";
                                    }
                                    messageList.Add (yukouUpText);

                                }else {
                                    if (CheckByProbability(50)) {
                                        Debug.Log("woooooooo");
                                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                                            messageList = MakeNonDaimyoShisya(13, "Royal Court Senior Messanger", messageList, 0);
                                        }else {
                                            messageList = MakeNonDaimyoShisya(13, "朝廷より重要な使者", messageList, 0);
                                        }
                                    }
                                }
                            }
                        }
					}
				}
			}
		}





		/************/
		/***Cyoutei***/
		/************/
		if (CheckByProbability (cyouteiShisyaRatio)) {
			float percent = UnityEngine.Random.value;
			percent = percent * 100;

			if (percent <= 30) {
                //Musin
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    messageList = MakeNonDaimyoShisya(15, "Nobleman", messageList, 0);
                } else {
                    messageList = MakeNonDaimyoShisya(15, "貴族", messageList, 0);
                }
                    
			} else if (30 < percent && percent <= 70) {
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    messageList = MakeNonDaimyoShisya(16, "Royal Court Messanger", messageList, 0);
                }else {
                    messageList = MakeNonDaimyoShisya(16, "朝廷より使者", messageList, 0);
                }
			} else if (70 < percent && percent <= 100) {

				List<int> targetDaimyoList = new List<int>();
				for (int i = 0; i < seiryokuList.Count; i++) {
					int tmpDaimyo = int.Parse(seiryokuList [i]);
					if (tmpDaimyo != myDaimyo) {
						targetDaimyoList.Add (tmpDaimyo);
					}
				}
				if (targetDaimyoList.Count != 0) {
					int rdm = UnityEngine.Random.Range (0, targetDaimyoList.Count);
					int randomDaimyoId = targetDaimyoList [rdm];
					
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        messageList = MakeNonDaimyoShisya(17, "Royal Court Messanger", messageList, randomDaimyoId);
                    }else {
                        messageList = MakeNonDaimyoShisya(17, "朝廷より使者", messageList, randomDaimyoId);
                    }
                }


			}
		}

        /************/
        /***Other***/
        /************/
        
		if (CheckByProbability (otherShisyaRatio)) {
			float percent = UnityEngine.Random.value;
			percent = percent * 100;

			if (percent <= 20) {
                //Musin
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    messageList = MakeNonDaimyoShisya(18, "Merchant", messageList, 0);
                }else {
                    messageList = MakeNonDaimyoShisya(18, "商人", messageList, 0);
                }
                    
			} else if (20 < percent && percent <= 40) {
				
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    messageList = MakeNonDaimyoShisya(18, "Westerner", messageList, 0);
                }else {
                    messageList = MakeNonDaimyoShisya(19, "南蛮人", messageList, 0);
                }
            } else if (40 < percent && percent <= 60) {
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    messageList = MakeNonDaimyoShisya(20, "Monk", messageList, 0);
                }else {
                    messageList = MakeNonDaimyoShisya(20, "遊行中の僧侶", messageList, 0);
                }
            } else if (60 < percent && percent <= 100) {
				
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    messageList = MakeNonDaimyoShisya(21, "Local samurai", messageList, 0);
                }else {
                    messageList = MakeNonDaimyoShisya(21, "国人衆", messageList, 0);
                }
            }
		}



		//Check Shinobi Cyouhou
		string cyouhou = PlayerPrefs.GetString("cyouhou");
		List<string> cyouhouList = new List<string> ();
		if (cyouhou != null && cyouhou != "") {
			if(cyouhou.Contains(",")){
				cyouhouList = new List<string> (cyouhou.Split (delimiterChars));
			}else{
				cyouhouList.Add(cyouhou);
			}
		}
		for(int i=0; i<cyouhouList.Count; i++){
			int kuniId = int.Parse(cyouhouList [i]);
			string snbTmp = "cyouhou" + kuniId.ToString ();
			int rank = PlayerPrefs.GetInt(snbTmp);

			float missPercent = 0;
			if(rank == 1){
				missPercent = 30;

			}else if(rank == 2){
				missPercent = 15;

			}else if(rank == 3){
				missPercent = 5;

			}

			float percent = Random.value;
			percent = percent * 100;

			if (percent < missPercent) {
				//Miss
				PlayerPrefs.DeleteKey(snbTmp);
				cyouhouList.Remove (kuniId.ToString());
				string newCyouhou = "";
				for(int j=0;j<cyouhouList.Count;j++){
					if (j == 0) {
						newCyouhou = cyouhouList[j];
					} else {
						newCyouhou = newCyouhou + "," + cyouhouList[j];
					}
				}
				PlayerPrefs.SetString ("cyouhou",newCyouhou);


				//Reset Chouhou Id
				targetKuni.transform.FindChild(kuniId.ToString()).GetComponent<SendParam>().cyouhouSnbRankId = 0;


				//Message
				KuniInfo kuni = new KuniInfo();
				string kuniName = kuni.getKuniName(kuniId);

				int daimyoId = int.Parse(seiryokuList [kuniId - 1]);
				Daimyo daimyo = new Daimyo ();
				string daimyoName = daimyo.getName (daimyoId); 

				int reduceYukoudo = DownYukouValueWithMyDaimyo (myDaimyo, daimyoId);
                string cyouhouMissText = "";
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    cyouhouMissText = "Your ninja who had been hiding in " + kuniName + " was cought. \nFriendship with " + daimyoName + " decreased " + reduceYukoudo + " point";
                }else {
                    cyouhouMissText = kuniName + "に潜伏中の忍が見つかりました。\n" + daimyoName + "との友好度が" + reduceYukoudo + "下がりました。";
                }

                messageList.Add (cyouhouMissText);

                

            }

		}
		PlayerPrefs.Flush();



		/*Decrease Yukou with Doumei koku*/
		string myDoumei = PlayerPrefs.GetString ("doumei");
		List<string> myDoumeiList = new List<string> ();
		if (myDoumei != null && myDoumei != "") {
			if(myDoumei.Contains(",")){
				myDoumeiList = new List<string> (myDoumei.Split (delimiterChars));
			}else{
				myDoumeiList.Add(myDoumei);
			}
		}
		for (int j=0; j<myDoumeiList.Count; j++) {
            string doumeiDaimyo = myDoumeiList[j];
            string yukouTemp = "gaikou" + doumeiDaimyo;
            int yukouWithDoumei = PlayerPrefs.GetInt(yukouTemp);
            int reduceYukou = UnityEngine.Random.Range(0, 1);
            yukouWithDoumei = yukouWithDoumei - reduceYukou;

            PlayerPrefs.SetInt(yukouTemp, yukouWithDoumei);

            /*Doumei Clear Check*/
            if (yukouWithDoumei < 20) {
                ClearMyDoumei(doumeiDaimyo, myDoumeiList, myDaimyo);
            }
		}

		/*Decrease Yukou with Doumei koku in other county*/
		List<string> activeDaimyoList = new List<string> ();
		for (int k=0; k<seiryokuList.Count; k++) {
			string daimyoId = seiryokuList[k];

			if(int.Parse(daimyoId) != myDaimyo && !activeDaimyoList.Contains(daimyoId)){
				activeDaimyoList.Add(daimyoId);
			}
		}

		for(int l=0; l<activeDaimyoList.Count; l++){
			string activeDaimyoId = activeDaimyoList[l];
			string temp = "doumei" + activeDaimyoId;
			if(PlayerPrefs.HasKey(temp)){
				string doumeiString = PlayerPrefs.GetString(temp);

				if(doumeiString != null && doumeiString != ""){

					List<string> doumeiTargetList = new List<string> ();
					if(doumeiString.Contains(",")){
						doumeiTargetList = new List<string> (doumeiString.Split (delimiterChars));
					}else{
						doumeiTargetList.Add(doumeiString);
					}

					for(int m=0; m<doumeiTargetList.Count; m++){
						string targetDaimyoId = doumeiTargetList[m];

						string gaikoTemp = "";
						if(int.Parse(activeDaimyoId)<int.Parse(targetDaimyoId)){
							gaikoTemp = activeDaimyoId + "gaikou" + targetDaimyoId;
						}else{
							gaikoTemp = targetDaimyoId + "gaikou" + activeDaimyoId;
						}
						if(PlayerPrefs.HasKey(gaikoTemp)){
							int gaikouValue = PlayerPrefs.GetInt(gaikoTemp);

							if(gaikouValue <20){
								//Doumei Clear
								//src to dst
								doumeiTargetList.Remove(targetDaimyoId);
								string newDoumei1 = "";
								for(int n=0; n<doumeiTargetList.Count; n++){
									if(n==0){
										newDoumei1 = doumeiTargetList[n];
									}else{
										newDoumei1 = newDoumei1 +"," + doumeiTargetList[n];
									}
								}
								PlayerPrefs.SetString(temp,newDoumei1);

								//dst to src
								string tgtTemp =  "doumei" + targetDaimyoId;
								string tgtDoumeiString = PlayerPrefs.GetString(tgtTemp);
								List<string> tgtDoumeiList = new List<string> ();
								if(tgtDoumeiString!=null && tgtDoumeiString != ""){
									if(tgtDoumeiString.Contains(",")){
										tgtDoumeiList = new List<string> (tgtDoumeiString.Split (delimiterChars));
									}else{
										tgtDoumeiList.Add(tgtDoumeiString);
									}
									tgtDoumeiList.Remove(activeDaimyoId);

									string newDoumei2 = "";
									for(int n=0; n<tgtDoumeiList.Count; n++){
										if(n==0){
											newDoumei2 = tgtDoumeiList[n];
										}else{
											newDoumei2 = newDoumei2 +"," + tgtDoumeiList[n];
										}
									}
									PlayerPrefs.SetString(tgtTemp,newDoumei2);
								}

								string srcDaimyoName = daimyo.getName(int.Parse(activeDaimyoId));
								string dstDaimyoName = daimyo.getName(int.Parse(targetDaimyoId));
                                string doumeiClearText = "";
                                if (Application.systemLanguage != SystemLanguage.Japanese) {
                                    doumeiClearText = "Alliance was renounced between " + srcDaimyoName + " and " + dstDaimyoName + " due to deterioration of relationship";
                                }else {
                                    doumeiClearText = "友好度悪化により、" + srcDaimyoName + "と" + dstDaimyoName + "間の同盟が解消しました。";
                                }
                                messageList.Add (doumeiClearText);
								PlayerPrefs.Flush();
							}
						}
					}
				}
			}
		}


		//Delete Cyouryaku Data
		string cyoryakuHst = PlayerPrefs.GetString("cyouryaku");
		List<string> cyoryakuHstList = new List<string> ();

		if(cyoryakuHst != null && cyoryakuHst != ""){
			if (cyoryakuHst.Contains (",")) {
				cyoryakuHstList = new List<string> (cyoryakuHst.Split (delimiterChars));
			} else {
				cyoryakuHstList.Add (cyoryakuHst);
			}

			for (int i=0; i<cyoryakuHstList.Count; i++) {
				string tmp = cyoryakuHstList [i];
				PlayerPrefs.DeleteKey (tmp);
			}
			PlayerPrefs.DeleteKey ("cyouryaku");
			PlayerPrefs.Flush ();
		}

		
		/*Message*/
		if (messageList.Count != 0) {
			audioSources[5].Play();

			/*Common Process*/
			//make back
			string pathOfBack = "Prefabs/Event/TouchEventBack";
			GameObject back = Instantiate(Resources.Load (pathOfBack)) as GameObject;
			back.transform.SetParent(GameObject.Find ("Panel").transform);
			back.transform.localScale = new Vector2 (1,1);
			back.transform.localPosition = new Vector2 (0,0);

			//make board
			string pathOfBoard = "Prefabs/Event/EventBoard";
			GameObject board = Instantiate(Resources.Load (pathOfBoard)) as GameObject;
			board.transform.SetParent(GameObject.Find ("Panel").transform);
			board.transform.localScale = new Vector2 (1,1);

            back.GetComponent<CloseEventBoard> ().deleteObj = board;
			back.GetComponent<CloseEventBoard> ().deleteObj2 = back;
			board.transform.FindChild("close").GetComponent<CloseEventBoard> ().deleteObj = board;
			board.transform.FindChild("close").GetComponent<CloseEventBoard> ().deleteObj2 = back;

			string pathOfScroll = "Prefabs/Event/EventScrollView";
			GameObject scroll = Instantiate(Resources.Load (pathOfScroll)) as GameObject;
			scroll.transform.SetParent(board.transform);
			scroll.transform.localScale = new Vector2 (1,1);
			RectTransform scrollTransform = scroll.GetComponent<RectTransform> ();
			scrollTransform.anchoredPosition = new Vector3 (0, -40, 0);

			string pathOfSlot = "Prefabs/Event/EventSlot";
			foreach (string text in messageList) {
				GameObject slot = Instantiate(Resources.Load (pathOfSlot)) as GameObject;
				slot.transform.SetParent(scroll.transform.FindChild("Content").transform);
				slot.transform.FindChild("EventText").GetComponent<Text>().text = text;
				slot.transform.localScale = new Vector2 (1,1);
			}

			board.transform.FindChild("close").GetComponent<CloseEventBoard> ().shisyaSceneFlg = shisyaSceneFlg;	
			board.transform.FindChild ("close").GetComponent<CloseEventBoard> ().deleteObj2.GetComponent<CloseEventBoard> ().shisyaSceneFlg = shisyaSceneFlg;

            //Pointer
            back.GetComponent<CloseEventBoard>().activityUpdateFlg = true;
            back.GetComponent<CloseEventBoard>().downDaimyo1List = downDaimyo1List;
            back.GetComponent<CloseEventBoard>().downDaimyo2List = downDaimyo2List;
            back.GetComponent<CloseEventBoard>().upDaimyo1List = upDaimyo1List;
            back.GetComponent<CloseEventBoard>().upDaimyo2List = upDaimyo2List;
            back.GetComponent<CloseEventBoard>().kassenDaimyoList = kassenDaimyoList;

            board.transform.FindChild("close").GetComponent<CloseEventBoard>().activityUpdateFlg = true;
            board.transform.FindChild("close").GetComponent<CloseEventBoard>().downDaimyo1List = downDaimyo1List;
            board.transform.FindChild("close").GetComponent<CloseEventBoard>().downDaimyo2List = downDaimyo2List;
            board.transform.FindChild("close").GetComponent<CloseEventBoard>().upDaimyo1List = upDaimyo1List;
            board.transform.FindChild("close").GetComponent<CloseEventBoard>().upDaimyo2List = upDaimyo2List;
            board.transform.FindChild("close").GetComponent<CloseEventBoard>().kassenDaimyoList = kassenDaimyoList;
        }


    }

	//Random check whether enemy start army to kassen
	public int CountEnemyKassenAction () {
		int kassenQty = 0;
		
		kassenQty = UnityEngine.Random.Range(0,4);

		return kassenQty;
	}

	public int CountEnemyGaikouAction () {
		int gaikouQty = 0;

		gaikouQty = UnityEngine.Random.Range(0,21);

		return gaikouQty;
	}

	//Random check whether enemy start army to kassen
	public bool CheckByProbability (int ratio) {
		bool checkFlg = false;

		float percent = Random.value;
		percent = percent * 100;
		ratio = 100 - ratio;
		if(percent > ratio){
			checkFlg = true;
		}
		return checkFlg;
	}

	public int UpYukouValueWithMyDaimyo(int myDaimyo, int dstDaimyoId){
		string temp = "gaikou" + dstDaimyoId.ToString ();

		int currentYukoudo = PlayerPrefs.GetInt (temp);
		int addYukoudo = UnityEngine.Random.Range(1,11);
		int total = currentYukoudo + addYukoudo;
		if (total >= 100) {
			total = 100;	
		}
		PlayerPrefs.SetInt (temp,total);
		flush ();

		return addYukoudo;
	}

	public int UpYukouValueWithOther(int srcDaimyoId, int dstDaimyoId){

		string temp = "";
		int addYukoudo = 0;

		if (srcDaimyoId < dstDaimyoId) {
			temp = srcDaimyoId.ToString() + "gaikou" + dstDaimyoId.ToString ();

		} else {
			temp = dstDaimyoId.ToString() + "gaikou" + srcDaimyoId.ToString ();
		}

		if (PlayerPrefs.HasKey (temp)) {
			int currentYukoudo = PlayerPrefs.GetInt (temp);
			addYukoudo = UnityEngine.Random.Range (5, 16);
			int total = currentYukoudo + addYukoudo;
			if (total >= 100) {
				total = 100;	
			}
			PlayerPrefs.SetInt (temp, total);
			flush ();

		} else {
			//1st time
			int gaikouValue = gaikou.getGaikouValue (srcDaimyoId, dstDaimyoId);
			addYukoudo = UnityEngine.Random.Range (5, 16);
			int total = gaikouValue + addYukoudo;
			if (total >= 100) {
				total = 100;	
			}
			PlayerPrefs.SetInt (temp, total);
			flush ();
		
		}



		return addYukoudo;
	}

	public int DownYukouValueWithMyDaimyo(int myDaimyo, int dstDaimyoId){
		string temp = "gaikou" + dstDaimyoId.ToString ();
		
		int currentYukoudo = PlayerPrefs.GetInt (temp);
		int reduceYukoudo = UnityEngine.Random.Range(5,16);
		int total = currentYukoudo - reduceYukoudo;
		if (total < 0) {
			total = 0;	
		}
		PlayerPrefs.SetInt (temp,total);
		flush ();

        DoGaikou gaikoScript = new DoGaikou();
        gaikoScript.downYukouOnIcon(dstDaimyoId, total);

        return reduceYukoudo;
	}

	public int DownYukouValueWithOther(int srcDaimyoId, int dstDaimyoId){
		
		string temp = "";
		int reduceYukoudo = 0;

		if (srcDaimyoId < dstDaimyoId) {
			temp = srcDaimyoId.ToString() + "gaikou" + dstDaimyoId.ToString ();
			
		} else {
			temp = dstDaimyoId.ToString() + "gaikou" + srcDaimyoId.ToString ();
		}

		if (PlayerPrefs.HasKey (temp)) {
			int currentYukoudo = PlayerPrefs.GetInt (temp);
			reduceYukoudo = UnityEngine.Random.Range (5, 16);
			int total = currentYukoudo - reduceYukoudo;
			if (total < 0) {
				total = 0;	
			}
			PlayerPrefs.SetInt (temp, total);
			flush ();
		} else {
			//1st time
			int gaikouValue = gaikou.getGaikouValue (srcDaimyoId, dstDaimyoId);
			reduceYukoudo = UnityEngine.Random.Range (5, 16);
			int total = gaikouValue - reduceYukoudo;
			if (total < 0) {
				total = 0;	
			}
			PlayerPrefs.SetInt (temp, total);
			flush ();

		}

		return reduceYukoudo;

	}


	public int randomEngunBusyo(int activeDaimyoId){
		int engunBusyoId = 0;

		Entity_busyo_mst busyoMst  = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;
		Entity_daimyo_mst daimyoMst = Resources.Load ("Data/daimyo_mst") as Entity_daimyo_mst;
		int daimyoBusyoId = daimyoMst.param[activeDaimyoId-1].busyoId;
		List<int> busyoList = new List<int> ();
		
		for(int i=0; i<busyoMst.param.Count; i++){
			int busyoId = busyoMst.param[i].id;
			int daimyoId = busyoMst.param[i].daimyoId;
			
			if(daimyoId == activeDaimyoId){

				busyoList.Add (busyoId);
			}
		}
		int rdmId = UnityEngine.Random.Range(0,busyoList.Count);
		if (busyoList.Count != 0) {
			engunBusyoId = busyoList [rdmId];
		}
		return engunBusyoId;
	}

	public string getEngunSts(string engunDaimyoId){
		string engunSts = "";//BusyoId-BusyoLv-ButaiQty-ButaiLv:
		
		string seiryoku = PlayerPrefs.GetString ("seiryoku");
		char[] delimiterChars = {','};
		List<string> seiryokuList = new List<string>();
		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
		
		int kuniId = 0;
		for(int i=0; i<seiryokuList.Count;i++){
			if(engunDaimyoId == seiryokuList[i]){
				kuniId = i + 1;
			}
		}
		
		GameObject targetKuni = GameObject.Find ("KuniIconView");
		SendParam sendParam = targetKuni.transform.FindChild (kuniId.ToString ()).GetComponent<SendParam> ();

		int busyoId = randomEngunBusyo(int.Parse(engunDaimyoId));
		int busyoLv = sendParam.busyoLv;
		int butaiQty = sendParam.butaiQty;
		int butaiLv = sendParam.butaiLv;


		engunSts = busyoId.ToString () + "-" + busyoLv.ToString () + "-" + butaiQty.ToString () + "-" + butaiLv.ToString ();

		return engunSts;
	}

	public int getEngunHei(string engunSts){
		int totalHei = 0;

		char[] delimiterChars = {'-'};
		List<string> engunStsList = new List<string>();
		engunStsList = new List<string> (engunSts.Split (delimiterChars));

		int busyoId = int.Parse(engunStsList [1]);
		int busyoLv = int.Parse(engunStsList [2]);
		int butaiQty = int.Parse(engunStsList [3]);
		int butaiLv = int.Parse(engunStsList [4]);


		if (busyoId != 0) {
			StatusGet sts = new StatusGet ();
			int hp = sts.getHp (busyoId, busyoLv);
			int hpResult = hp * 100;
			string type = sts.getHeisyu (busyoId);
			int chHp = sts.getChHp (type, butaiLv, hp);
			chHp = chHp * butaiQty * 10;
			totalHei = hpResult + chHp;
		}

		return totalHei;

	}


	public void flush(){
		PlayerPrefs.Flush ();
	}


	public List<string> makeDoumei(int bestGaikouDaimyo, int myDaimyo, int srcDaimyoId, string srcDaimyoName, List<string> messageList){

        if(bestGaikouDaimyo != srcDaimyoId) {

		    if (bestGaikouDaimyo == myDaimyo) {
			
			    //doumei check
			    Doumei doumei = new Doumei();
			    bool myDoumeiExistFlg =doumei.myDoumeiExistCheck (srcDaimyoId);
			    if (!myDoumeiExistFlg) {
				    //Shisya
				    Debug.Log ("myDoumei!!!");
				    messageList = MakeShisya (1, srcDaimyoName, messageList, srcDaimyoId);
			    }

		    } else if(srcDaimyoId != myDaimyo) {
			    //Exist Check
			    string doumeiTmp = "doumei" + srcDaimyoId;
			    string doumeiString = PlayerPrefs.GetString (doumeiTmp);
			    List<string> doumeiList = new List<string> ();
			    char[] delimiterChars = {','};
			    if (doumeiString != null && doumeiString != "") {
				    if (doumeiString.Contains (",")) {
					    doumeiList = new List<string> (doumeiString.Split (delimiterChars));
				    } else {
					    doumeiList.Add (doumeiString);
				    }
			    }

			    if (!doumeiList.Contains (bestGaikouDaimyo.ToString())) {
				    //Not Exist Case
				    //Doumei Data Register

				    string newDoumei1 = "";
				    if (doumeiString != null && doumeiString != "") {
					    newDoumei1 = doumeiString + "," + bestGaikouDaimyo;
				    } else {
					    newDoumei1 = bestGaikouDaimyo.ToString();
				    }
				    PlayerPrefs.SetString (doumeiTmp, newDoumei1);

				    string doumeiTmp2 = "doumei" + bestGaikouDaimyo;
				    string doumeiString2 = PlayerPrefs.GetString (doumeiTmp2);
				    string newDoumei2 = "";
				    if (doumeiString2 != null && doumeiString2 != "") {
					    newDoumei2 = doumeiString2 + "," + srcDaimyoId;
				    } else {
					    newDoumei2 = srcDaimyoId.ToString();
				    }
				    PlayerPrefs.SetString (doumeiTmp2, newDoumei2);

				    flush ();

				    string dst1stDaimyoName = daimyo.getName (srcDaimyoId);
				    string dst2ndDaimyoName = daimyo.getName (bestGaikouDaimyo);
                    string doumeiText = "";
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        doumeiText = dst1stDaimyoName + " and " + dst2ndDaimyoName + " concluded an alliance";
                    }else {
                        doumeiText = dst1stDaimyoName + "と" + dst2ndDaimyoName + "間に同盟が成立しました。";
                    }
                    messageList.Add (doumeiText);
			    }

		    }
        }
		return messageList;
	}

	public List<string> MakeShisya(int shisyaId, string srcDaimyoName, List<string>messageList,int srcDaimyoId){
		

		//Check
		string tmp = "shisya" + shisyaId.ToString ();
		string shisyaString =PlayerPrefs.GetString(tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}
		if (!shisyaList.Contains (srcDaimyoId.ToString())) {

            string yukouUpText = "";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                yukouUpText = srcDaimyoName + "'s message has come to see you";
            }else {
                yukouUpText = srcDaimyoName + "殿の使者が参っております。";
            }
            messageList.Add (yukouUpText);

			PlayerPrefs.SetBool ("shisyaFlg", true);


			if (shisyaString != null && shisyaString != "") {
				PlayerPrefs.SetString (tmp, shisyaString + "," + srcDaimyoId.ToString ());
			} else {
				PlayerPrefs.SetString (tmp, srcDaimyoId.ToString ());
			}
			shisyaSceneFlg = true;
		}

		return messageList;
	}

	public List<string> MakeEngunShisya(int shisyaId, string key, int doumeiDaimyoId, string doumeiDaimyoName, int targetKuniId, int attackDaimyoId, List<string>messageList){

        string yukouUpText = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            yukouUpText = doumeiDaimyoName + "'s messanger has come to see you";
        }else {
            yukouUpText = doumeiDaimyoName + "殿の使者が参っております。";
        }
        messageList.Add (yukouUpText);

		PlayerPrefs.SetBool("shisyaFlg",true);
		string tmp = "shisya" + shisyaId.ToString ();
		string shisyaString =PlayerPrefs.GetString(tmp);
		if (shisyaString != null && shisyaString != "") {
			PlayerPrefs.SetString (tmp,shisyaString + "," + doumeiDaimyoId.ToString() + ":" + attackDaimyoId + ":" + targetKuniId + ":" + key);
		} else {
			PlayerPrefs.SetString (tmp, doumeiDaimyoId.ToString() + ":" + attackDaimyoId + ":" + targetKuniId + ":" + key);
		}
		shisyaSceneFlg = true;	

		PlayerPrefs.Flush ();
		return messageList;
	}

	public List<string> MakeKyouhakuShisya (int shisyaId, string key,int enemyDaimyoId, string enemyDaimyoName, int targetKuniId, List<string>messageList){
        string yukouUpText = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            yukouUpText = enemyDaimyoName + "'s messanger has come to see you";
        }else {
            yukouUpText = enemyDaimyoName + "殿の使者が参っております。";
        }
		messageList.Add (yukouUpText);

		PlayerPrefs.SetBool("shisyaFlg",true);
		string tmp = "shisya" + shisyaId.ToString ();
		string shisyaString =PlayerPrefs.GetString(tmp);
		if (shisyaString != null && shisyaString != "") {
			PlayerPrefs.SetString (tmp,shisyaString + "," + enemyDaimyoId + ":" + targetKuniId + ":" + key);
		} else {
			PlayerPrefs.SetString (tmp, enemyDaimyoId + ":" + targetKuniId + ":" + key);
		}
		shisyaSceneFlg = true;		

		PlayerPrefs.Flush ();
		return messageList;
	}


	public List<string> MakeBoueiShisya (int shisyaId, string key, string syogunDaimyoName, int srcDaimyoId, int dstDaimyoId, int targetKuniId, List<string>messageList){
        string yukouUpText = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            yukouUpText = syogunDaimyoName + "'s messanger has come to see you";
        }
        else {
            yukouUpText = syogunDaimyoName + "殿の使者が参っております。";
        }
		messageList.Add (yukouUpText);

		PlayerPrefs.SetBool("shisyaFlg",true);
		string tmp = "shisya" + shisyaId.ToString ();
		string shisyaString =PlayerPrefs.GetString(tmp);
		if (shisyaString != null && shisyaString != "") {
			PlayerPrefs.SetString (tmp,shisyaString + "," + srcDaimyoId + ":" + dstDaimyoId +":"+ targetKuniId + ":" + key);
		} else {
			PlayerPrefs.SetString (tmp,  + srcDaimyoId + ":" + dstDaimyoId + ":" + targetKuniId + ":" + key);
		}
		shisyaSceneFlg = true;	

		PlayerPrefs.Flush ();
		return messageList;
	}

	public List<string> MakeNonDaimyoShisya(int shisyaId, string name, List<string>messageList, int daimyoId){

        string yukouUpText = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            yukouUpText = name + " has come to see you";
        }else {
            yukouUpText = name + "が参っております。";
        }
		messageList.Add (yukouUpText);

		PlayerPrefs.SetBool("shisyaFlg",true);
		string tmp = "shisya" + shisyaId.ToString ();
		if (shisyaId == 17) {
			PlayerPrefs.SetString (tmp,daimyoId.ToString());
		} else {
			PlayerPrefs.SetString (tmp, "TRUE");
		}
		shisyaSceneFlg = true;	

		return messageList;
	}

    public bool CheckMyDoumei(int daimyoId) {
        bool flg = false;
        char[] delimiterChars = { ',' };

        string myDoumei = PlayerPrefs.GetString("doumei");
        List<string> myDoumeiList = new List<string>();
        if (myDoumei != null && myDoumei != "") {
            if (myDoumei.Contains(",")) {
                myDoumeiList = new List<string>(myDoumei.Split(delimiterChars));
            }else {
                myDoumeiList.Add(myDoumei);
            }
        }
        if(myDoumeiList.Contains(daimyoId.ToString())) {
            flg = true;
        }


        return flg;
    }

    public void ClearMyDoumei(string doumeiDaimyo, List<string> myDoumeiList, int myDaimyo) {
        
        //My Doumei Clear
        myDoumeiList.Remove(doumeiDaimyo);
        string newMyDoumei = "";
        for (int i = 0; i < myDoumeiList.Count; i++) {
            if (i == 0) {
                newMyDoumei = myDoumeiList[i];
            }
            else {
                newMyDoumei = newMyDoumei + "," + myDoumeiList[i];
            }
        }
        PlayerPrefs.SetString("doumei", newMyDoumei);

        //Opposite Doumei Clear
        char[] delimiterChars = { ',' };
        string otherTemp = "doumei" + doumeiDaimyo;
        string otherDoumei = PlayerPrefs.GetString(otherTemp);
        List<string> otherDoumeiList = new List<string>();
        if (otherDoumei != null && otherDoumei != "") {
            if (otherDoumei.Contains(",")) {
                otherDoumeiList = new List<string>(otherDoumei.Split(delimiterChars));
            }
            else {
                otherDoumeiList.Add(otherDoumei);
            }
        }
        otherDoumeiList.Remove(myDaimyo.ToString());
        string newOtherDoumei = "";
        for (int i = 0; i < otherDoumeiList.Count; i++) {
            if (i == 0) {
                newOtherDoumei = otherDoumeiList[i];
            }
            else {
                newOtherDoumei = newOtherDoumei + "," + otherDoumeiList[i];
            }
        }
        PlayerPrefs.SetString(otherTemp, newOtherDoumei);

        //Icon & Flg Change
        KuniInfo kuni = new KuniInfo();
        kuni.deleteDoumeiKuniIcon(int.Parse(doumeiDaimyo));

        PlayerPrefs.Flush();        
    }
}
