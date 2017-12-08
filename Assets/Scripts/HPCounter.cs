using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System.Collections.Generic;

public class HPCounter : MonoBehaviour {

	public string targetTag;
	public float life = 0;
	public bool flag = false;
	public bool isAttackedFlg = false;
	public bool isKessenFlg = false;
	public EnemyHP EnemyHpSclipt;
	public PlayerHP PlayerHpSclipt;
	char[] delimiterChars = { ',' };
	public AudioSource[] audioSources;

    //Init Heiryoku
    public float initPlayerHei = 0;
    public float initEnemyHei = 0;


    void Start () {
		audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [5].Play ();

		isAttackedFlg = PlayerPrefs.GetBool("isAttackedFlg");
		isKessenFlg = PlayerPrefs.GetBool("isKessenFlg");

		if (this.tag == "EnemyBar") {
			targetTag = "Enemy";
			
		} else if (this.tag == "PlayerBar") {
			targetTag = "Player";
		}

        if(isAttackedFlg) {
            initPlayerHei = getTotalHp("Player");
            initEnemyHei = getTotalHp("Enemy");
        }

	}
	
	// Update is called once per frame
	void Update () {
		life = getTotalHp (targetTag);

		//Show Heiryoku
		if (life >= 0) {
			gameObject.transform.Find("HpText").GetComponent<Text>().text = life.ToString();
		}

		if (!flag) {
			if (life == 0) {
				flag = true;
                Time.timeScale = 1;

                //Stop Opposite Timer
                if(name == "playerHp") {
                    GameObject.Find("enemyHp").GetComponent<HPCounter>().flag = true;
                }else {
                    GameObject.Find("playerHp").GetComponent<HPCounter>().flag = true;
                }



                if(GameObject.Find("ScrollView")) {
                    GameObject.Find ("ScrollView").SetActive (false);
                }
                if (GameObject.Find ("GiveupBtn")) {
					GameObject.Find ("GiveupBtn").SetActive (false);
				}
                if (GameObject.Find("AutoBtn")) {
                    GameObject.Find("AutoBtn").SetActive(false);
                }
                GameObject canvas = GameObject.Find ("Canvas").gameObject;

                if (Application.loadedLevelName == "tutorialKassen") {
                    string backPath = "Prefabs/PostKassen/back";
                    GameObject backObj = Instantiate(Resources.Load(backPath)) as GameObject;
                    backObj.transform.SetParent(canvas.transform);
                    backObj.transform.localScale = new Vector2(70, 63);
                    backObj.transform.localPosition = new Vector2(0, 0);

                    string blackPath = "Prefabs/PostKassen/black";
                    GameObject blackObj = Instantiate(Resources.Load(blackPath)) as GameObject;
                    blackObj.transform.SetParent(canvas.transform);
                    blackObj.transform.localScale = new Vector2(330, 300);
                    blackObj.transform.localPosition = new Vector2(0, 0);

                    string nextbtnPath = "Prefabs/Tutorial/tutorialBttnList";
                    GameObject bttnListObj = Instantiate(Resources.Load(nextbtnPath)) as GameObject;
                    bttnListObj.transform.SetParent(canvas.transform);
                    bttnListObj.transform.localScale = new Vector2(1, 1);
                    bttnListObj.transform.localPosition = new Vector2(0, 20);

                    GameObject.Find("timer").GetComponent<Timer>().enabled = false;

                    if (targetTag == "Player") {
                        //lose
                        audioSources[4].Play();
                        Color color = Color.blue;
                        int langId = PlayerPrefs.GetInt("langId");
                        if (langId == 2) {
                            if (GameObject.Find("winlose").GetComponent<TextMesh>()) {
                                GameObject.Find("winlose").GetComponent<TextMesh>().text = "Lose";
                            }
                        }else {
                            if (GameObject.Find("winlose").GetComponent<TextMesh>()) {
                                GameObject.Find("winlose").GetComponent<TextMesh>().text = "敗北";
                            }
                        }
                        GameObject.Find("winlose").GetComponent<TextMesh>().color = color;

                        if (langId == 2) {
                            bttnListObj.transform.Find("nextbtn").transform.Find("Text").GetComponent<Text>().text = "Restart";
                        }else {
                            bttnListObj.transform.Find("nextbtn").transform.Find("Text").GetComponent<Text>().text = "再戦する";
                        }
                        bttnListObj.transform.Find("nextbtn").GetComponent<BackStageButton>().tutorialRestartFlg = true;

                    }else {
                        //Win
                        audioSources[3].Play();
                        audioSources[7].Play();
                        int langId = PlayerPrefs.GetInt("langId");
                        if (langId == 2) {
                            GameObject.Find("winlose").GetComponent<TextMesh>().text = "Win";
                        }else {
                            GameObject.Find("winlose").GetComponent<TextMesh>().text = "勝利";
                        }
                        string particlePath = "Prefabs/PostKassen/particle";
                        GameObject particleObj = Instantiate(Resources.Load(particlePath)) as GameObject;
                        particleObj.transform.SetParent(canvas.transform);
                        particleObj.transform.localPosition = new Vector2(0, 60);

                    }

                } else {

                    if (targetTag == "Player") {
				        //Game Over
				        string backPath = "Prefabs/PostKassen/back";
				        GameObject backObj = Instantiate(Resources.Load (backPath)) as GameObject;
				        backObj.transform.SetParent (canvas.transform);
				        backObj.transform.localScale = new Vector2(70,63);
				        backObj.transform.localPosition = new Vector2 (0,0);

				        //Chane word
				        Color color = Color.blue;
                        int langId = PlayerPrefs.GetInt("langId");
                        if (langId == 2) {
                            if (GameObject.Find("winlose").GetComponent<TextMesh>()) {
                                GameObject.Find("winlose").GetComponent<TextMesh>().text = "Lose";
                            }
                        } else {
                            if (GameObject.Find("winlose").GetComponent<TextMesh>()) {
                                GameObject.Find("winlose").GetComponent<TextMesh>().text = "敗北";
                            }
                        }
				        GameObject.Find ("winlose").GetComponent<TextMesh>().color = color;
					
				        string blackPath = "Prefabs/PostKassen/black";
				        GameObject blackObj = Instantiate(Resources.Load (blackPath)) as GameObject;
				        blackObj.transform.SetParent (canvas.transform);
				        blackObj.transform.localScale = new Vector2(330,300);
				        blackObj.transform.localPosition = new Vector2 (0,0);

				        string makimonoPath = "Prefabs/PostKassen/makimono";
				        GameObject makimonoObj = Instantiate(Resources.Load (makimonoPath)) as GameObject;
				        makimonoObj.transform.SetParent (canvas.transform);
				        makimonoObj.transform.localScale = new Vector2(1,1);
				        makimonoObj.transform.localPosition = new Vector2(0,-135);

				        //Button List
				        string nextbtnPath = "Prefabs/PostKassen/bttnList";
				        GameObject bttnListObj = Instantiate(Resources.Load (nextbtnPath)) as GameObject;
				        bttnListObj.transform.SetParent (canvas.transform);
				        bttnListObj.transform.localScale = new Vector2(1,1);		
				        bttnListObj.transform.localPosition = new Vector2 (0,0);

				        //Time Stop
				        GameObject.Find ("timer").GetComponent<Timer>().enabled = false;


                        bool pvpFlg = false;
                        if (GameObject.Find("GameScene").GetComponent<GameScene>()) {
                            pvpFlg = GameObject.Find("GameScene").GetComponent<GameScene>().pvpFlg;
                        }
                        if (!pvpFlg) {
                            //lose Stage Name
                            string stageNamePath = "Prefabs/PostKassen/loseStageName";
				            GameObject stageNameObj = Instantiate(Resources.Load (stageNamePath)) as GameObject;
				            stageNameObj.transform.SetParent (canvas.transform);
				            stageNameObj.transform.localScale = new Vector2(1,1);
				            stageNameObj.transform.localPosition = new Vector2 (0,-102);
				            string stageName = PlayerPrefs.GetString("activeStageName");
                        

				            //Check is attacked flag
				            if (isAttackedFlg) {
					            //Lose

					            audioSources [4].Play ();
					            PlayerPrefs.SetInt("winChecker",2);
                                if (langId == 2) {
                                    stageNameObj.transform.Find ("stageNameValue").GetComponent<Text> ().text = stageName + " Failed";
                                }else {
                                    stageNameObj.transform.Find("stageNameValue").GetComponent<Text>().text = stageName + "失敗";
                                }

                                int activeKuniId = PlayerPrefs.GetInt("activeKuniId");
                                string tempKuni = "kuni" + activeKuniId.ToString();

                                //My Daimyo Lose 
                                Gunzei lose = new Gunzei ();
					            string tKey = PlayerPrefs.GetString ("activeKey");
					            int tSrcDaimyoId = PlayerPrefs.GetInt ("activeSrcDaimyoId");
					            int tDstDaimyoId = PlayerPrefs.GetInt ("activeDstDaimyoId");
					            bool noGunzeiFlg = true;
					            lose.win (tKey, tSrcDaimyoId, tDstDaimyoId, noGunzeiFlg, activeKuniId);

                                //Delete Cleared Kuni                            
                                string clearedKuni = PlayerPrefs.GetString("clearedKuni");
                                List<string> clearedKuniList = new List<string>();
                                if (clearedKuni != null && clearedKuni != "") {
                                    if (clearedKuni.Contains(",")) {
                                        clearedKuniList = new List<string>(clearedKuni.Split(delimiterChars));
                                    }else {
                                        clearedKuniList.Add(clearedKuni);
                                    }
                                }

                                string tempActiveKuni = activeKuniId.ToString();
                                clearedKuniList.Remove(tempActiveKuni);
                                string newClearedKuni = "";
                                for (int i = 0; i < clearedKuniList.Count; i++) {
                                    if (i == 0) {
                                        newClearedKuni = clearedKuniList[i];
                                    }
                                    else {
                                        newClearedKuni = newClearedKuni + "," + clearedKuniList[i];
                                    }
                                }
                                PlayerPrefs.SetString("clearedKuni", newClearedKuni);

                                if (newClearedKuni == null || newClearedKuni == "") {
                                    PlayerPrefs.SetBool("gameOverFlg", true);
                                    PlayerPrefs.Flush();
                                    Debug.Log("gameOver Set");

                                }
                            
                                //Delete Jyosyu
                                string tempJyosyu = "jyosyu" + activeKuniId.ToString();
                                int jyosyu = PlayerPrefs.GetInt(tempJyosyu);
                                if(jyosyu !=0) {
                                    string jyosyuHei = "jyosyuHei" + jyosyu;
                                    PlayerPrefs.DeleteKey(jyosyuHei);
                                    string jyosyuBusyo = "jyosyuBusyo" + jyosyu;
                                    PlayerPrefs.DeleteKey(jyosyuBusyo);
                                    PlayerPrefs.DeleteKey(tempJyosyu);
                                }

                                //*************************************************
                                //
                                //Lost Shiro shall be changed depends on Degree of Damage
                                //
                                //*************************************************
                                int saveShiroQty = 0;
                                float currentEnemyHei = getTotalHp("Enemy");
                                float tmpValue = (initEnemyHei - currentEnemyHei) / initPlayerHei;
                                if(1.6<=tmpValue) {
                                    saveShiroQty = 9;
                                }else if(1.4<=tmpValue && tmpValue<1.6) {
                                    saveShiroQty = 8;
                                }else if (1.2 <= tmpValue && tmpValue < 1.4) {
                                    saveShiroQty = 7;
                                }else if (1.0 <= tmpValue && tmpValue < 1.2) {
                                    saveShiroQty = 6;
                                }else if (0.8 <= tmpValue && tmpValue < 1.0) {
                                    saveShiroQty = 5;
                                }else if (0.7 <= tmpValue && tmpValue < 0.8) {
                                    saveShiroQty = 4;
                                }else if (0.6 <= tmpValue && tmpValue < 0.7) {
                                    saveShiroQty = 3;
                                }else if (0.5 <= tmpValue && tmpValue < 0.6) {
                                    saveShiroQty = 2;
                                }else if (0.4 <= tmpValue && tmpValue < 0.5) {
                                    saveShiroQty = 1;
                                }else if (tmpValue < 0.4) {
                                    saveShiroQty = 0;
                                }
                                //Delete Stage Clear

                                if (saveShiroQty != 0) {
                                    //Add Stage by Damage
                                    List<string> newClearedStageList = new List<string>();
                                    List<string> stageTmpList = new List<string>() {"1","2","3", "4", "5", "6", "7", "8", "9", "10" };
                                    for (int i=0; i < saveShiroQty; i++) {
                                        int rdmStage = UnityEngine.Random.Range(0, stageTmpList.Count);
                                        string stageId = stageTmpList[rdmStage];
                                        newClearedStageList.Add(stageId);
                                        stageTmpList.Remove(stageId);
                                    }

                                    string newClearedStageString = "";
                                    for(int i=0; i<newClearedStageList.Count; i++) {
                                        if(i==0) {
                                            newClearedStageString = newClearedStageList[i];
                                        }else {
                                            newClearedStageString = newClearedStageString + "," + newClearedStageList[i];
                                        }
                                    }
                                    PlayerPrefs.SetString(tempKuni, newClearedStageString);
                                
                                }
                            
					            //Delete open
					            KuniInfo kuni = new KuniInfo ();
                                int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
                                string seiryoku = PlayerPrefs.GetString("seiryoku");
                                kuni.updateOpenKuni (myDaimyo,seiryoku);
					            PlayerPrefs.Flush ();
					
				            } else {

					            if (!isKessenFlg) {
						            //Lose
						            audioSources [4].Play ();
                                    if (langId == 2) {
                                        stageNameObj.transform.Find ("stageNameValue").GetComponent<Text> ().text = stageName + " Failed";
                                    }else {
                                        stageNameObj.transform.Find("stageNameValue").GetComponent<Text>().text = stageName + "攻略失敗";
                                    }
						            int stageId = PlayerPrefs.GetInt ("activeStageId");
						            int kuniId = PlayerPrefs.GetInt ("activeKuniId");

						            string clearedStage = "kuni" + kuniId;
						            string clearedStageString = PlayerPrefs.GetString (clearedStage);
						            List<string> clearedStageList = new List<string> ();
						            char[] delimiterChars = { ',' };
						            if (clearedStageString != null && clearedStageString != "") {
							            clearedStageList = new List<string> (clearedStageString.Split (delimiterChars));
						            }
						            if (!clearedStageList.Contains (stageId.ToString ())) {
							            PlayerPrefs.SetString ("kassenWinLoseFlee", stageId.ToString () + ",0");
						            }
					            } else {
						            //Kessen Lose
						            stageNameObj.transform.Find ("stageNameValue").GetComponent<Text> ().text = stageName;
						            kessenResult (false);
						
					            }
				            }
                        }else {
                            //PvP Data Register
                            //Player Lose & Enemy Win
                            PvPDataStore DataStore = GameObject.Find("PvPDataStore").GetComponent<PvPDataStore>();
                            DataStore.UpdatePvPDfcNo(DataStore.enemyUserId, DataStore.todayNCMB);
                            DataStore.UpdatePvPDfcWinNo(DataStore.enemyUserId, DataStore.todayNCMB);

                            //Popup,
                            string pvpPopPath = "Prefabs/PvP/GetPt";
                            GameObject popPvPObj = Instantiate(Resources.Load(pvpPopPath)) as GameObject;
                            popPvPObj.transform.SetParent(canvas.transform);
                            popPvPObj.transform.localScale = new Vector2(0.8f, 0.8f);
                            popPvPObj.transform.localPosition = new Vector2(0, 0);
                            if (langId == 2) {
                                popPvPObj.GetComponent<Text>().text = "Pt -" + DataStore.getPt;
                            }else {
                                popPvPObj.GetComponent<Text>().text = "武功 -" + DataStore.getPt;
                            }
                            
                        }

                    } else if (targetTag == "Enemy") {
				        //Win
				        audioSources [3].Play ();
				        audioSources [7].Play ();

                        bool pvpFlg = false;
                        if (GameObject.Find("GameScene").GetComponent<GameScene>()) {
                            pvpFlg = GameObject.Find("GameScene").GetComponent<GameScene>().pvpFlg;
                        }                        
                        if (!pvpFlg) {
                            int TrackWinNo = PlayerPrefs.GetInt("TrackWinNo",0);
				            TrackWinNo = TrackWinNo + 1;
				            PlayerPrefs.SetInt("TrackWinNo",TrackWinNo);
				            PlayerPrefs.Flush ();

				            if(isAttackedFlg){
					            //history
					            string tKey = PlayerPrefs.GetString("activeKey");
					            MainStageController main = new MainStageController();
					            main.deleteKeyHistory(tKey);
					            PlayerPrefs.SetInt("winChecker",1);
					            PlayerPrefs.Flush();
				            }

				            bool twiceHeiFlg = PlayerPrefs.GetBool ("twiceHeiFlg");
				            if (twiceHeiFlg) {
					            PlayerPrefs.SetBool ("questDailyFlg15",true);
					            PlayerPrefs.DeleteKey ("twiceHeiFlg");
					            PlayerPrefs.Flush();
				            }
                        }else {
                            //quest for pvp
                            int PvPWinTotal = PlayerPrefs.GetInt("PvPWinTotal",0);
                            PvPWinTotal = PvPWinTotal + 1;
                            PlayerPrefs.SetInt("PvPWinTotal", PvPWinTotal);
                            if(PvPWinTotal==1) {
                                PlayerPrefs.SetBool("questSpecialFlg152", true);
                            }else if(PvPWinTotal == 5) {
                                PlayerPrefs.SetBool("questSpecialFlg153", true);
                            }else if(PvPWinTotal == 20) {
                                PlayerPrefs.SetBool("questSpecialFlg154", true);
                            }
                            else if (PvPWinTotal == 50) {
                                PlayerPrefs.SetBool("questSpecialFlg155", true);
                            }
                            else if (PvPWinTotal == 100) {
                                PlayerPrefs.SetBool("questSpecialFlg156", true);
                            }
                            else if (PvPWinTotal == 500) {
                                PlayerPrefs.SetBool("questSpecialFlg157", true);
                            }
                            else if (PvPWinTotal == 1000) {
                                PlayerPrefs.SetBool("questSpecialFlg158", true);
                            }                            
                            PlayerPrefs.Flush();
                            
                        }

				        string backPath = "Prefabs/PostKassen/back";
				        GameObject backObj = Instantiate(Resources.Load (backPath)) as GameObject;
				        backObj.transform.SetParent (canvas.transform);
				        backObj.transform.localScale = new Vector2(70,63);
				        backObj.transform.localPosition = new Vector2 (0,0);

				        string particlePath = "Prefabs/PostKassen/particle";
				        GameObject particleObj = Instantiate(Resources.Load (particlePath)) as GameObject;
				        particleObj.transform.SetParent (canvas.transform);
				        particleObj.transform.localPosition = new Vector2(0,60);

				        string blackPath = "Prefabs/PostKassen/black";
				        GameObject blackObj = Instantiate(Resources.Load (blackPath)) as GameObject;
				        blackObj.transform.SetParent (canvas.transform);
				        blackObj.transform.localScale = new Vector2(330,300);
				        blackObj.transform.localPosition = new Vector2 (0,0);

				        string makimonoPath = "Prefabs/PostKassen/makimono";
				        GameObject makimonoObj = Instantiate(Resources.Load (makimonoPath)) as GameObject;
				        makimonoObj.transform.SetParent (canvas.transform);
				        makimonoObj.transform.localScale = new Vector2(1,1);
				        makimonoObj.transform.localPosition = new Vector2(0,-135);

				        string stageName = PlayerPrefs.GetString("activeStageName");

                        int langId = PlayerPrefs.GetInt("langId");
                        if (langId == 2) {
                            if (GameObject.Find("winlose").GetComponent<TextMesh>()) {
                                GameObject.Find("winlose").GetComponent<TextMesh>().text = "Win";
                            }
                        }else {
                            if (GameObject.Find("winlose").GetComponent<TextMesh>()) {
                                GameObject.Find("winlose").GetComponent<TextMesh>().text = "勝利";
                            }
                        }


                        //Item List
                        string itemListPath = "Prefabs/PostKassen/itemList";
                        GameObject itemListObj = Instantiate(Resources.Load(itemListPath)) as GameObject;
                        itemListObj.transform.SetParent(canvas.transform);
                        itemListObj.transform.localScale = new Vector2(1, 1);
                        itemListObj.transform.localPosition = new Vector2(0, -136);

                        if(pvpFlg) {
                            isAttackedFlg = false;
                        }
                        if (!isAttackedFlg) {
					        if (!isKessenFlg) {                              
                                if(pvpFlg) {
                                    PvPDataStore DataStore = GameObject.Find("PvPDataStore").GetComponent<PvPDataStore>();
                                    if (langId == 2) {
                                        itemListObj.transform.Find("stageName").transform.Find("stageNameValue").GetComponent<Text>().text = DataStore.enemyUserName;
                                    }else {
                                        itemListObj.transform.Find("stageName").transform.Find("stageNameValue").GetComponent<Text>().text = DataStore.enemyUserName;
                                    }
                                }else {
                                    if (langId == 2) {
                                        itemListObj.transform.Find ("stageName").transform.Find ("stageNameValue").GetComponent<Text> ().text = stageName + " Cleared";
                                    }else {
                                        itemListObj.transform.Find("stageName").transform.Find("stageNameValue").GetComponent<Text>().text = stageName + "攻略";
                                    }
                                }
					        } else {
						        itemListObj.transform.Find ("stageName").transform.Find ("stageNameValue").GetComponent<Text> ().text = stageName;
					        }
					        /*Item or Kahou*/
					        string activeItemGrp = PlayerPrefs.GetString ("activeItemGrp");                           
					        if (activeItemGrp != "no" && activeItemGrp != "" ) {
								
						        string activeItemType = PlayerPrefs.GetString ("activeItemType");
						        int activeItemId = PlayerPrefs.GetInt ("activeItemId");
						        int activeItemQty = PlayerPrefs.GetInt ("activeItemQty");

                                //Get Item
                                string cyouheiPath = "Prefabs/Item/Cyouhei/" + activeItemType;
						        string kanjyoPath = "Prefabs/Item/Kanjyo/Kanjyo";
						        string hidensyoPath = "Prefabs/Item/Hidensyo/Hidensyo";
						        string shinobiPath = "Prefabs/Item/Shinobi/Shinobi";
						        char[] delimiterChars = { ',' };

						        if (activeItemGrp == "item") {

							        //Cyouhei
							        if (activeItemType.Contains ("Cyouhei") == true) {
								        string newCyouheiString = "";

								        makeItemIcon (cyouheiPath, activeItemId.ToString (), itemListObj);
								        if (activeItemType.Contains ("YR") == true) {
									        string cyouheiString = PlayerPrefs.GetString ("cyouheiYR");
									        string[] cyouheiList = cyouheiString.Split (delimiterChars);
									        if (activeItemId == 1) {
										        int tempQty = int.Parse (cyouheiList [0]);
										        tempQty = tempQty + activeItemQty;
										        newCyouheiString = tempQty.ToString () + "," + cyouheiList [1] + "," + cyouheiList [2];

									        } else if (activeItemId == 2) {
										        int tempQty = int.Parse (cyouheiList [1]);
										        tempQty = tempQty + activeItemQty;
										        newCyouheiString = cyouheiList [0] + "," + tempQty.ToString () + "," + cyouheiList [2];

									        } else if (activeItemId == 3) {
										        int tempQty = int.Parse (cyouheiList [2]);
										        tempQty = tempQty + activeItemQty;
										        newCyouheiString = cyouheiList [0] + "," + cyouheiList [1] + "," + tempQty.ToString ();
									        }
                                            if (newCyouheiString != "") {
                                                PlayerPrefs.SetString ("cyouheiYR", newCyouheiString);
                                            }
								        } else if (activeItemType.Contains ("KB") == true) {
									        string cyouheiString = PlayerPrefs.GetString ("cyouheiKB");
									        string[] cyouheiList = cyouheiString.Split (delimiterChars);
									        if (activeItemId == 1) {
										        int tempQty = int.Parse (cyouheiList [0]);
										        tempQty = tempQty + activeItemQty;
										        newCyouheiString = tempQty.ToString () + "," + cyouheiList [1] + "," + cyouheiList [2];
											
									        } else if (activeItemId == 2) {
										        int tempQty = int.Parse (cyouheiList [1]);
										        tempQty = tempQty + activeItemQty;
										        newCyouheiString = cyouheiList [0] + "," + tempQty.ToString () + "," + cyouheiList [2];
											
									        } else if (activeItemId == 3) {
										        int tempQty = int.Parse (cyouheiList [2]);
										        tempQty = tempQty + activeItemQty;
										        newCyouheiString = cyouheiList [0] + "," + cyouheiList [1] + "," + tempQty.ToString ();
									        }
                                            if (newCyouheiString != "") {
                                                PlayerPrefs.SetString ("cyouheiKB", newCyouheiString);                                            
                                            }
								        } else if (activeItemType.Contains ("TP") == true) {
									        string cyouheiString = PlayerPrefs.GetString ("cyouheiTP");
									        string[] cyouheiList = cyouheiString.Split (delimiterChars);
									        if (activeItemId == 1) {
										        int tempQty = int.Parse (cyouheiList [0]);
										        tempQty = tempQty + activeItemQty;
										        newCyouheiString = tempQty.ToString () + "," + cyouheiList [1] + "," + cyouheiList [2];
											
									        } else if (activeItemId == 2) {
										        int tempQty = int.Parse (cyouheiList [1]);
										        tempQty = tempQty + activeItemQty;
										        newCyouheiString = cyouheiList [0] + "," + tempQty.ToString () + "," + cyouheiList [2];
											
									        } else if (activeItemId == 3) {
										        int tempQty = int.Parse (cyouheiList [2]);
										        tempQty = tempQty + activeItemQty;
										        newCyouheiString = cyouheiList [0] + "," + cyouheiList [1] + "," + tempQty.ToString ();
									        }
                                            if (newCyouheiString != "") {
                                                PlayerPrefs.SetString ("cyouheiTP", newCyouheiString);
                                            }

								        } else if (activeItemType.Contains ("YM") == true) {
									        string cyouheiString = PlayerPrefs.GetString ("cyouheiYM");
									        string[] cyouheiList = cyouheiString.Split (delimiterChars);
									        if (activeItemId == 1) {
										        int tempQty = int.Parse (cyouheiList [0]);
										        tempQty = tempQty + activeItemQty;
										        newCyouheiString = tempQty.ToString () + "," + cyouheiList [1] + "," + cyouheiList [2];
											
									        } else if (activeItemId == 2) {
										        int tempQty = int.Parse (cyouheiList [1]);
										        tempQty = tempQty + activeItemQty;
										        newCyouheiString = cyouheiList [0] + "," + tempQty.ToString () + "," + cyouheiList [2];
											
									        } else if (activeItemId == 3) {
										        int tempQty = int.Parse (cyouheiList [2]);
										        tempQty = tempQty + activeItemQty;
										        newCyouheiString = cyouheiList [0] + "," + cyouheiList [1] + "," + tempQty.ToString ();
									        }
                                            if (newCyouheiString != "") {
                                                PlayerPrefs.SetString ("cyouheiYM", newCyouheiString);
                                            }
								        }


								        //Kanjyo
							        } else if (activeItemType == "Kanjyo") {
								        makeItemIcon (kanjyoPath, activeItemId.ToString (), itemListObj);

								        string newKanjyoString = "";
								        string kanjyoString = PlayerPrefs.GetString ("kanjyo");
								        string[] kanjyoList = kanjyoString.Split (delimiterChars);

								        if (activeItemId == 1) {
									        int tempQty = int.Parse (kanjyoList [0]);
									        tempQty = tempQty + activeItemQty;
									        newKanjyoString = tempQty.ToString () + "," + kanjyoList [1] + "," + kanjyoList [2];
										
								        } else if (activeItemId == 2) {
									        int tempQty = int.Parse (kanjyoList [1]);
									        tempQty = tempQty + activeItemQty;
									        newKanjyoString = kanjyoList [0] + "," + tempQty.ToString () + "," + kanjyoList [2];
										
								        } else if (activeItemId == 3) {
									        int tempQty = int.Parse (kanjyoList [2]);
									        tempQty = tempQty + activeItemQty;
									        newKanjyoString = kanjyoList [0] + "," + kanjyoList [1] + "," + tempQty.ToString ();
								        }
								        PlayerPrefs.SetString ("kanjyo", newKanjyoString);
								
								        //Hidensyo
							        } else if (activeItemType == "Hidensyo") {
								        makeItemIcon (hidensyoPath, activeItemId.ToString (), itemListObj);

								        if (activeItemId == 1) {
									        int hidensyoQty = PlayerPrefs.GetInt ("hidensyoGe");
									        hidensyoQty = hidensyoQty + activeItemQty;
									        PlayerPrefs.SetInt ("hidensyoGe", hidensyoQty);

								        } else if (activeItemId == 2) {
									        int hidensyoQty = PlayerPrefs.GetInt ("hidensyoCyu");
									        hidensyoQty = hidensyoQty + activeItemQty;
									        PlayerPrefs.SetInt ("hidensyoCyu", hidensyoQty);

								        } else if (activeItemId == 3) {
									        int hidensyoQty = PlayerPrefs.GetInt ("hidensyoJyo");
									        hidensyoQty = hidensyoQty + activeItemQty;
									        PlayerPrefs.SetInt ("hidensyoJyo", hidensyoQty);
								        }

								        //Shinobi
							        } else if (activeItemType == "Shinobi") {
								        makeItemIcon (shinobiPath, activeItemId.ToString (), itemListObj);
								        RectTransform rect = itemListObj.transform.Find ("itemIcon").transform.Find ("Shinobi").GetComponent<RectTransform> ();
								        rect.sizeDelta = new Vector2 (100, 100);


								        if (activeItemId == 1) {
									        int newQty = 0;
									        int shinobiQty = PlayerPrefs.GetInt ("shinobiGe");
									        newQty = shinobiQty + activeItemQty;
									        PlayerPrefs.SetInt ("shinobiGe", newQty);

								        } else if (activeItemId == 2) {
									        int newQty = 0;
									        int shinobiQty = PlayerPrefs.GetInt ("shinobiCyu");
									        newQty = shinobiQty + activeItemQty;
									        PlayerPrefs.SetInt ("shinobiCyu", newQty);

								        } else if (activeItemId == 3) {
									        int newQty = 0;
									        int shinobiQty = PlayerPrefs.GetInt ("shinobiJyo");
									        newQty = shinobiQty + activeItemQty;
									        PlayerPrefs.SetInt ("shinobiJyo", newQty);

								        }								
								
								        //tech
							        } else if (activeItemType == "tech") {
								        string path = "Prefabs/Item/Tech/Tech";
								        GameObject tech = Instantiate (Resources.Load (path)) as GameObject;
								        tech.transform.SetParent (itemListObj.transform);
								        tech.transform.localScale = new Vector2 (0.4f, 0.4f);
								        RectTransform techTransform = tech.GetComponent<RectTransform> ();
								        techTransform.sizeDelta = new Vector2 (100, 100);
								        techTransform.anchoredPosition3D = new Vector3 (650, 110, 0);
								        tech.GetComponent<Button> ().enabled = false;

								        string spritePath = "";
								        if (activeItemId == 1) {
									        //TP
									        int qty = PlayerPrefs.GetInt ("transferTP", 0);
									        int newQty = qty + activeItemQty;
									        PlayerPrefs.SetInt ("transferTP", newQty);
									        spritePath = "Prefabs/Item/Tech/Sprite/tp";

								        } else if (activeItemId == 2) {
									        int qty = PlayerPrefs.GetInt ("transferKB", 0);
									        int newQty = qty + activeItemQty;
									        PlayerPrefs.SetInt ("transferKB", newQty);
									        spritePath = "Prefabs/Item/Tech/Sprite/kb";

								        } else if (activeItemId == 3) {
									        int qty = PlayerPrefs.GetInt ("transferSNB", 0);
									        int newQty = qty + activeItemQty;
									        PlayerPrefs.SetInt ("transferSNB", newQty);
									        spritePath = "Prefabs/Item/Tech/Sprite/snb";
								        }
										
								        tech.GetComponent<Image> ().sprite = 
									        Resources.Load (spritePath, typeof(Sprite)) as Sprite;
									
								
								        //cyoutei or koueki
							        } else if (activeItemType == "cyoutei" || activeItemType == "koueki") {

								        if (activeItemType == "cyoutei") {
									        string syoukaijyoPath = "Prefabs/Item/cyoutei";
									        GameObject icon = Instantiate (Resources.Load (syoukaijyoPath)) as GameObject;
									        icon.transform.SetParent (itemListObj.transform);
									        icon.transform.localPosition = new Vector3 (250, 35, 0);
									        icon.transform.localScale = new Vector2 (0.4f, 0.4f);
									        icon.GetComponent<Button> ().enabled = false;

									        if (activeItemId == 1) {
										        GameObject nameObj = icon.transform.Find ("Name").gameObject;
										        
										        nameObj.transform.localScale = new Vector2 (0.08f, 0.1f);
                                                if (langId == 2) {
                                                    nameObj.GetComponent<Text>().text = "Yamashina";
                                                    icon.transform.Find ("Rank").GetComponent<Text> ().text = "Low";
                                                }else {
                                                    nameObj.GetComponent<Text>().text = "山科言継";
                                                    icon.transform.Find("Rank").GetComponent<Text>().text = "下";
                                                }
									        } else if (activeItemId == 2) {
										        GameObject nameObj = icon.transform.Find ("Name").gameObject;
										        
										        nameObj.transform.localScale = new Vector2 (0.08f, 0.1f);
                                                if (langId == 2) {
                                                    nameObj.GetComponent<Text>().text = "Sanjyo";
                                                    icon.transform.Find ("Rank").GetComponent<Text> ().text = "Mid";
                                                }else {
                                                    nameObj.GetComponent<Text>().text = "三条西実枝";
                                                    icon.transform.Find("Rank").GetComponent<Text>().text = "中";
                                                }
									        } else if (activeItemId == 3) {
										        GameObject nameObj = icon.transform.Find ("Name").gameObject;										        
										        nameObj.transform.localScale = new Vector2 (0.08f, 0.1f);
                                                if (langId == 2) {
                                                    nameObj.GetComponent<Text>().text = "Konoe";
                                                    icon.transform.Find ("Rank").GetComponent<Text> ().text = "High";
                                                }else {
                                                    nameObj.GetComponent<Text>().text = "近衛前久";
                                                    icon.transform.Find("Rank").GetComponent<Text>().text = "上";
                                                }
									        }

								        } else if (activeItemType == "koueki") {
									        string syoukaijyoPath = "Prefabs/Item/koueki";
									        GameObject icon = Instantiate (Resources.Load (syoukaijyoPath)) as GameObject;
									        icon.transform.SetParent (itemListObj.transform);
									        icon.transform.localPosition = new Vector3 (250, 35, 0);
									        icon.transform.localScale = new Vector2 (0.4f, 0.4f);
									        icon.GetComponent<Button> ().enabled = false;

									        if (activeItemId == 1) {
										        GameObject nameObj = icon.transform.Find ("Name").gameObject;										       
										        nameObj.transform.localScale = new Vector2 (0.08f, 0.1f);
                                                if (langId == 2) {
                                                    nameObj.GetComponent<Text>().text = "Kato";
                                                    icon.transform.Find ("Rank").GetComponent<Text> ().text = "Low";
                                                }else {
                                                    nameObj.GetComponent<Text>().text = "加藤浄与";
                                                    icon.transform.Find("Rank").GetComponent<Text>().text = "下";
                                                }
									        } else if (activeItemId == 2) {
										        GameObject nameObj = icon.transform.Find ("Name").gameObject;										        
										        nameObj.transform.localScale = new Vector2 (0.08f, 0.1f);
                                                if (langId == 2) {
                                                    nameObj.GetComponent<Text>().text = "Shimai";
                                                    icon.transform.Find ("Rank").GetComponent<Text> ().text = "Mid";
                                                }else {
                                                    nameObj.GetComponent<Text>().text = "島井宗室";
                                                    icon.transform.Find("Rank").GetComponent<Text>().text = "中";
                                                }
									        } else if (activeItemId == 3) {
										        GameObject nameObj = icon.transform.Find ("Name").gameObject;										        
										        nameObj.transform.localScale = new Vector2 (0.08f, 0.1f);
                                                if (langId == 2) {
                                                    nameObj.GetComponent<Text>().text = "Cyaya";
                                                    icon.transform.Find ("Rank").GetComponent<Text> ().text = "High";
                                                }else {
                                                    nameObj.GetComponent<Text>().text = "茶屋四郎次郎";
                                                    icon.transform.Find("Rank").GetComponent<Text>().text = "上";
                                                }
									        }
								        }

								        TabibitoItemGetter syoukaijyo = new TabibitoItemGetter ();
								        syoukaijyo.registerKouekiOrCyoutei (activeItemType, activeItemId);
								
							        } else if (activeItemType == "Tama") {

								        string path = "Prefabs/Item/Tama";
								        GameObject icon = Instantiate (Resources.Load (path)) as GameObject;
								        icon.transform.SetParent (itemListObj.transform);
								        icon.transform.localPosition = new Vector3 (250, 35, 0);
								        icon.transform.localScale = new Vector2 (0.4f, 0.4f);
								        icon.GetComponent<Button> ().enabled = false;
								
								        int nowQty = PlayerPrefs.GetInt ("busyoDama");
								        int newQty = nowQty + activeItemQty;
								        PlayerPrefs.SetInt ("busyoDama", newQty);

							        }


						        } else if (activeItemGrp == "kahou") {
							        //Kahou
							        string kahouIconPath = "Prefabs/Item/Kahou/" + activeItemType + activeItemId;
							        GameObject kahouIcon = Instantiate (Resources.Load (kahouIconPath)) as GameObject;
							        kahouIcon.transform.SetParent (itemListObj.transform);
							        kahouIcon.name = "itemIcon";
							        kahouIcon.transform.localPosition = new Vector3 (250, 35, 0);
							        kahouIcon.transform.localScale = new Vector2 (0.4f, 0.4f);
							        RectTransform kahouTransform = kahouIcon.GetComponent<RectTransform> ();
							        kahouTransform.sizeDelta = new Vector2 (100, 100);

							        GameObject rank = kahouIcon.transform.Find ("Rank").gameObject;
							        rank.transform.localScale = new Vector2 (0.3f, 0.3f);
							        rank.transform.localPosition = new Vector2 (20, -20);

							        //Register
							        addKahou (activeItemType, activeItemId);
						        }

						        //Qty
						        string itemQtyPath = "Prefabs/PostKassen/itemQty";
						        GameObject itemQtyObj = Instantiate (Resources.Load (itemQtyPath)) as GameObject;
						        itemQtyObj.transform.SetParent (itemListObj.transform);
						        itemQtyObj.transform.localScale = new Vector2 (0.09f, 0.09f);
						        itemQtyObj.transform.localPosition = new Vector2 (290, 35);
						        itemQtyObj.GetComponent<Text> ().text = "x " + activeItemQty.ToString ();

							
						        PlayerPrefs.Flush ();
					        }

				        } else {
                            if(!pvpFlg) {
                                if (langId == 2) {
                                    itemListObj.transform.Find ("stageName").transform.Find("stageNameValue").GetComponent<Text> ().text = stageName + " Sucecss";
                                }else {
                                    itemListObj.transform.Find("stageName").transform.Find("stageNameValue").GetComponent<Text>().text = stageName + "成功";
                                }
                            }else {
                                PvPDataStore DataStore = GameObject.Find("PvPDataStore").GetComponent<PvPDataStore>();
                                if (langId == 2) {
                                    itemListObj.transform.Find("stageName").transform.Find("stageNameValue").GetComponent<Text>().text = DataStore.enemyUserName;
                                }else {
                                    itemListObj.transform.Find("stageName").transform.Find("stageNameValue").GetComponent<Text>().text = DataStore.enemyUserName;
                                }
                            }
                        }
					
				        //Money
				        int activeStageMoney = PlayerPrefs.GetInt("activeStageMoney",0);
				        GameObject.Find ("moneyAmt").GetComponent<Text>().text = activeStageMoney.ToString();
				        int currentMoney = PlayerPrefs.GetInt("money");
				        currentMoney = currentMoney + activeStageMoney;
                        if(currentMoney < 0) {
                            currentMoney = int.MaxValue;
                        }                            
				        PlayerPrefs.SetInt("money",currentMoney);

				        int TrackEarnMoney = PlayerPrefs.GetInt ("TrackEarnMoney",0);
				        TrackEarnMoney = TrackEarnMoney + activeStageMoney;
				        PlayerPrefs.SetInt ("TrackEarnMoney",TrackEarnMoney);


				        //kuniExp
				        int activeStageExp = PlayerPrefs.GetInt("activeStageExp",0);
				        GameObject.Find ("expAmt").GetComponent<Text>().text = activeStageExp.ToString();
				        int currentKuniExp = PlayerPrefs.GetInt ("kuniExp");
				        currentKuniExp = currentKuniExp + activeStageExp;
				        int kuniLv = PlayerPrefs.GetInt ("kuniLv");
				        Exp kuniExp = new Exp();
				        int newKuniLv = kuniExp.getKuniLv(kuniLv,currentKuniExp);

				        if(newKuniLv>kuniLv){
					        //lv up
					        int jinkeiLimit = kuniExp.getJinkeiLimit(newKuniLv);
					        int stockLimit = kuniExp.getStockLimit(newKuniLv);
					        PlayerPrefs.SetInt("jinkeiLimit",jinkeiLimit);
					        PlayerPrefs.SetInt("stockLimit",stockLimit);

					        //Popup
					        Message msg = new Message ();
                            string text = "";
                            if (langId == 2) {
                                text = "Our Country increased Lv " + newKuniLv + ". \n Stamina recovered all.";
                            }else {
                                text = "当家の威信が" + newKuniLv + "に上がりましたぞ。\n兵糧が全回復します。";
                            }

                            //BusyoQty Up Check
                            int oldJinkeiLimit = kuniExp.getJinkeiLimit(kuniLv);
                            int newJinkeiLimit = kuniExp.getJinkeiLimit(newKuniLv);

                            if(newJinkeiLimit > oldJinkeiLimit) {
                                string addText = "";
                                if (langId == 2) {
                                    addText = "\n And also increased the number of battle available samurai";
                                }else {
                                    addText = "\nまた出陣可能な武将数も一人増えました。";
                                }
                                text = text + addText;
                            }
                            GameObject msgObj = msg.makeKassenMessage (text);
                            msgObj.GetComponent<FadeuGUI>().fadetime = 5.0f;
                            msgObj.transform.Find("MessageText").transform.localScale = new Vector3(0.15f,0.15f,0);

                            PlayerPrefs.SetInt("hyourou",100);

				        }else{
					        Debug.Log ("No level up");
				        }

				        /*Cleared Flag*/
				        if (!isAttackedFlg) {
					        if (!isKessenFlg) {
						        int activeKuniId = PlayerPrefs.GetInt ("activeKuniId");
						        int activeStageId = PlayerPrefs.GetInt ("activeStageId");
						        string temp = "kuni" + activeKuniId.ToString ();

						        List<string> clearedStageList = new List<string> ();
						        string clearedStageString = PlayerPrefs.GetString (temp);

						        int stageId = PlayerPrefs.GetInt ("activeStageId");
						        if (clearedStageString != null && clearedStageString != "") {
							        //after 1st time
							        char[] delimiterChars = { ',' };
							        clearedStageList = new List<string> (clearedStageString.Split (delimiterChars));
							        if (!clearedStageList.Contains (activeStageId.ToString ())) {
								        PlayerPrefs.SetString ("kassenWinLoseFlee", stageId.ToString () + ",2");

								        clearedStageString = clearedStageString + "," + activeStageId.ToString ();

								        //1st Kuni Clear Check
								        string[] commaCounter = clearedStageString.Split (delimiterChars);
								        int counter = commaCounter.Length;

								        if (counter == 10) {
									        kuniClear (activeKuniId);

								        }
							        }
						        } else {
							        //1st time
							        clearedStageString = activeStageId.ToString ();
							        PlayerPrefs.SetString ("kassenWinLoseFlee", stageId.ToString () + ",2");

						        }
						        PlayerPrefs.SetString (temp, clearedStageString);


					        } else {
						        kessenResult (true);
					        }
				        }

				        PlayerPrefs.SetInt ("kuniLv", newKuniLv);
				        PlayerPrefs.SetInt ("kuniExp", currentKuniExp);
				        PlayerPrefs.Flush ();

                        if (pvpFlg) {
                            //PvP Data Register
                            //Player Win & Enemy lose

                            //Win&Lose No Update
                            PvPDataStore DataStore = GameObject.Find("PvPDataStore").GetComponent<PvPDataStore>();
                            DataStore.UpdatePvPAtkWinNo(DataStore.userId, DataStore.todayNCMB);
                            DataStore.UpdatePvPDfcNo(DataStore.enemyUserId, DataStore.todayNCMB);

                            //Point Update
                            DataStore.UpdatePvPPt(DataStore.userId, true, DataStore.getPt * 2);
                            DataStore.UpdatePvPPt(DataStore.enemyUserId, false, DataStore.getPt * 2);

                            //Popup,
                            string pvpPopPath = "Prefabs/PvP/GetPt";
                            GameObject popPvPObj = Instantiate(Resources.Load(pvpPopPath)) as GameObject;
                            popPvPObj.transform.SetParent(canvas.transform);
                            popPvPObj.transform.localScale = new Vector2(0.8f, 0.8f);
                            popPvPObj.transform.localPosition = new Vector2(0, 0);
                            if (langId == 2) {
                                popPvPObj.GetComponent<Text>().text = "Pt +" + DataStore.getPt ;
                            }else {
                                popPvPObj.GetComponent<Text>().text = "武功 +" + DataStore.getPt;
                            }
                        }

                        //Button List
                        string nextbtnPath = "Prefabs/PostKassen/bttnList";
				        GameObject bttnListObj = Instantiate(Resources.Load (nextbtnPath)) as GameObject;
				        bttnListObj.transform.SetParent (canvas.transform);
				        bttnListObj.transform.localScale = new Vector2(1,1);
				        bttnListObj.transform.localPosition = new Vector2 (0,0);

				        //Time Stop
				        GameObject.Find ("timer").GetComponent<Timer>().enabled = false;


				        //Get Exp
				        SenkouButton senkou = new SenkouButton();
				        List<BusyoSenkou> senkouList = new List<BusyoSenkou>();
				        senkouList=senkou.getSenkou ();
				        for(int i=0;i<senkouList.Count;i++){

					        int busyoId = senkouList[i].id;
					        int senkouAmt = senkouList[i].senkou;
					        Exp exp = new Exp();

					        //Modify by Cyadougu Kahou
					        senkouAmt = exp.getExpbyCyadougu(busyoId,senkouAmt);

					        //Busyo Exp
					        string tempExp = "exp" + busyoId;
					        int nowExp = PlayerPrefs.GetInt(tempExp);
					        int newExp = nowExp + senkouAmt;

                            //Busyo Lv
                            int nowLv = PlayerPrefs.GetInt(busyoId.ToString());
                            string addLvTmp = "addlv" + busyoId.ToString();                            
                            int maxLv = 100 + PlayerPrefs.GetInt(addLvTmp);
                            
                            if (maxLv>200) {
                                maxLv = 200;
                            }

                            int newLv = exp.getLvbyTotalExp(nowLv,newExp, maxLv);                            
                            if (newLv == maxLv) {
						        newExp = exp.getExpLvMax (maxLv);
					        }

					        PlayerPrefs.SetInt(busyoId.ToString(), newLv);
					        PlayerPrefs.SetInt(tempExp, newExp);

                            //Adjustment
                            if(nowLv>maxLv) {                                
                                PlayerPrefs.SetInt(busyoId.ToString(), maxLv);
                                PlayerPrefs.SetInt(tempExp, exp.getExpLvMax(maxLv));
                            }
                            
					        PlayerPrefs.Flush();
				        }


                        // 勝利したので10%の確率でレビュー誘導
                        ReviewManager.Request10Parcent(transform.parent);
			        }
                }
            }
		}
	}

	public float getTotalHp(string targetTag){
		//初期化
		life = 0;
		
		//タグ指定されたオブジェクトを配列で取得する
		foreach (GameObject obs in  GameObject.FindGameObjectsWithTag(targetTag)){
			if (obs.GetComponent<Heisyu> ()) {
				if (obs.GetComponent<Heisyu> ().heisyu != "saku") {
					if (targetTag == "Enemy") {
						EnemyHpSclipt = obs.GetComponent<EnemyHP> ();
						life =	life + EnemyHpSclipt.life + (EnemyHpSclipt.childHP * (EnemyHpSclipt.childQty - 1)) + EnemyHpSclipt.childHPTmp;;
					} else if (targetTag == "Player") {
						PlayerHpSclipt = obs.GetComponent<PlayerHP> ();
						life =	life + PlayerHpSclipt.life + (PlayerHpSclipt.childHP * (PlayerHpSclipt.childQty - 1)) + PlayerHpSclipt.childHPTmp;

					}
				}
			}
		}

		return life;
	}

	public void addKahou(string kahouType, int kahouId){
		Kahou kahou = new Kahou ();

		if(kahouType=="bugu"){
			kahou.registerBugu (kahouId);
		}else if(kahouType=="kabuto"){
			kahou.registerKabuto (kahouId);
		}else if(kahouType=="gusoku"){
			kahou.registerGusoku (kahouId);
		}else if(kahouType=="meiba"){
			kahou.registerMeiba (kahouId);
		}else if(kahouType=="cyadougu"){
			kahou.registerCyadougu (kahouId);
		}else if(kahouType=="heihousyo"){
			kahou.registerHeihousyo (kahouId);
		}else if(kahouType=="chishikisyo"){
			kahou.registerChishikisyo (kahouId);
		}

	}

	public void makeItemIcon(string path, string itemId, GameObject item){
		Color lowColor = new Color (0f / 255f, 0f / 255f, 219f / 255f, 255f / 255f);
		Color midColor = new Color (94f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
		Color highColor = new Color (84f / 255f, 103f / 255f, 0f / 255f, 255f / 255f);

		GameObject itemIcon = Instantiate (Resources.Load (path)) as GameObject;
		itemIcon.transform.SetParent(item.transform);
		itemIcon.transform.localScale = new Vector2 (0.4f, 0.4f);
		itemIcon.name = "itemIcon";
		RectTransform iconTransform = itemIcon.GetComponent<RectTransform> ();
		iconTransform.anchoredPosition = new Vector3 (250, 35, 0);
		iconTransform.sizeDelta = new Vector2 (100, 100);
		itemIcon.GetComponent<Button>().enabled = false;


        //Color
        int langId = PlayerPrefs.GetInt("langId");
        if (itemId == "1") {
			itemIcon.GetComponent<Image> ().color = lowColor;

			foreach (Transform obj in itemIcon.transform){
				if(obj.tag == "ItemRank"){
                    if (langId == 2) {
                        obj.gameObject.GetComponent<Text>().text = "Low";
                    }else {
                        obj.gameObject.GetComponent<Text>().text = "下";
                    }
				}
			}

		} else if (itemId == "2") {
			itemIcon.GetComponent<Image> ().color = midColor;
			foreach (Transform obj in itemIcon.transform){
				if(obj.tag == "ItemRank"){
                    if (langId == 2) {
                        obj.gameObject.GetComponent<Text>().text = "Mid";
                    }else {
                        obj.gameObject.GetComponent<Text>().text = "中";
                    }
				}
			}

		} else if (itemId == "3") {
			itemIcon.GetComponent<Image> ().color = highColor;
			foreach (Transform obj in itemIcon.transform){
				if(obj.tag == "ItemRank"){
                    if (langId == 2) {
                        obj.gameObject.GetComponent<Text>().text = "High";
                    }else {
                        obj.gameObject.GetComponent<Text>().text = "上";
                    }
				}
			}
		}


	}

	public void kuniClear(int activeKuniId){
		
		//1st time
		string seiryoku = PlayerPrefs.GetString ("seiryoku");
		List<string> seiryokuList = new List<string>();
		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
		int befDaimyoId = int.Parse(seiryokuList[activeKuniId-1]);

		string clearedKuni = PlayerPrefs.GetString("clearedKuni");
		if(clearedKuni !=null && clearedKuni !=""){
			clearedKuni = clearedKuni + "," + activeKuniId.ToString();
		}else{
			clearedKuni = activeKuniId.ToString();
		}
		PlayerPrefs.SetString("clearedKuni",clearedKuni);
		PlayerPrefs.SetString ("kassenWinLoseFlee", befDaimyoId.ToString() + ",3");

		/*Quest Start*/
		//1. 1st Kuni Clear
		//Make current data
		string kuniClearHist = PlayerPrefs.GetString("kuniClearHist");
		List<string> kuniClearHistList = new List<string> ();

		if (kuniClearHist != null && kuniClearHist != "") {
			if (kuniClearHist.Contains (",")) {
				kuniClearHistList = new List<string> (kuniClearHist.Split (delimiterChars));
			} else {
				kuniClearHistList.Add (kuniClearHist);
			}
		}

		//1st time check
		if (!kuniClearHistList.Contains (activeKuniId.ToString())) {
			string newkuniClearHist = "";
			if (kuniClearHist != null && kuniClearHist != "") {
				newkuniClearHist = kuniClearHist + "," + activeKuniId.ToString ();
			} else {
				newkuniClearHist = activeKuniId.ToString ();
			}
			PlayerPrefs.SetString ("kuniClearHist",newkuniClearHist);
			int id = activeKuniId + 38;
			string tmp = "questSpecialFlg" + id.ToString();
			PlayerPrefs.SetBool (tmp,true);
			PlayerPrefs.Flush ();
		}

		//2. 1st Daimyo Metsubou
		//metsubou check

		int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
		seiryokuList[activeKuniId-1] = myDaimyo.ToString();

		if(!seiryokuList.Contains(befDaimyoId.ToString())){
			//metsubou
			PlayerPrefs.SetString ("kassenWinLoseFlee", befDaimyoId.ToString() + ",4");

			//1st time check

			//Make current data
			string daimyoMetsubouHist = PlayerPrefs.GetString("daimyoMetsubouHist");
			List<string> daimyoMetsubouHistList = new List<string> ();

			if (daimyoMetsubouHist != null && daimyoMetsubouHist != "") {
				if (daimyoMetsubouHist.Contains (",")) {
					daimyoMetsubouHistList = new List<string> (daimyoMetsubouHist.Split (delimiterChars));
				} else {
					daimyoMetsubouHistList.Add (daimyoMetsubouHist);
				}
			}

			//1st time check
			if (!daimyoMetsubouHistList.Contains (befDaimyoId.ToString())) {
				string newDaimyoMetsubouHist = "";
				if (daimyoMetsubouHist != null && daimyoMetsubouHist != "") {
					newDaimyoMetsubouHist = daimyoMetsubouHist + "," + befDaimyoId.ToString ();
				} else {
					newDaimyoMetsubouHist = befDaimyoId.ToString ();
				}
				PlayerPrefs.SetString ("daimyoMetsubouHist",newDaimyoMetsubouHist);
				int id = befDaimyoId + 103;
				string tmp = "questSpecialFlg" + id.ToString();
				PlayerPrefs.SetBool (tmp,true);
				PlayerPrefs.Flush ();
			}

		}
		/*Quest End*/




		//Open Kuni
		KuniInfo kuni = new KuniInfo();
		kuni.registerOpenKuni(activeKuniId);
		string newSeiryoku = "";
		bool gameClearFlg = true;
		for(int i=0; i<seiryokuList.Count; i++){
			if(i==0){
				newSeiryoku = seiryokuList[i];
			}else{
				newSeiryoku = newSeiryoku + "," + seiryokuList[i];
			}

			//game clear check
			if(gameClearFlg){
				if(seiryokuList[i] != myDaimyo.ToString()){
					gameClearFlg = false;
				}
			}
		}
		PlayerPrefs.SetBool("gameClearFlg",gameClearFlg);
		PlayerPrefs.SetString("seiryoku",newSeiryoku);


		//Cyouhou Delete
		string cyouhouTmp = "cyouhou" + activeKuniId;
		if (PlayerPrefs.HasKey (cyouhouTmp)) {
			PlayerPrefs.DeleteKey(cyouhouTmp);

			string cyouhou = PlayerPrefs.GetString("cyouhou");
			List<string> cyouhouList = new List<string> ();
			if (cyouhou != null && cyouhou != "") {
				if(cyouhou.Contains(",")){
					cyouhouList = new List<string> (cyouhou.Split (delimiterChars));
				}else{
					cyouhouList.Add(cyouhou);
				}
			}

			cyouhouList.Remove (activeKuniId.ToString());
			string newCyouhou = "";
			for(int j=0;j<cyouhouList.Count;j++){
				if (j == 0) {
					newCyouhou = cyouhouList[j];
				} else {
					newCyouhou = newCyouhou + "," + cyouhouList[j];
				}
			}
			PlayerPrefs.SetString ("cyouhou",newCyouhou);

		}

        //Delete Enemy Gunzei
        deleteEnemyGunzeiData(activeKuniId);



    }


	public void kessenResult(bool winFlg){
		int daimyoId = PlayerPrefs.GetInt("activeDaimyoId");
		int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
		string seiryoku = PlayerPrefs.GetString ("seiryoku");
		List<string> seiryokuList = new List<string> ();
		char[] delimiterChars = {','};
		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
		List<string> newSeiryokuList = new List<string> (seiryokuList);
		string clearedKuni = PlayerPrefs.GetString ("clearedKuni");
		KuniInfo kuni = new KuniInfo ();

		if (winFlg) {
			//Win
			PlayerPrefs.SetInt("winChecker",1);
			for (int i = 0; i < seiryokuList.Count; i++) {
				int tmpDaimyoId = int.Parse(seiryokuList [i]);

				if (tmpDaimyoId == daimyoId) {
					//Change
					int kuniId = i + 1;
					
					//1.update seiryoku
					newSeiryokuList [kuniId - 1] = myDaimyo.ToString();

					//2.update cleaered kuni & kuni1,2,3
					clearedKuni = clearedKuni + "," + kuniId.ToString ();
					string tmp = "kuni" + kuniId.ToString ();
					PlayerPrefs.SetString (tmp,"1,2,3,4,5,6,7,8,9,10");

					//3.update openkuni
					kuni.registerOpenKuni(kuniId);

					//4.cyouhou delete
					string cyouhouTmp = "cyouhou" + kuniId;
					if (PlayerPrefs.HasKey (cyouhouTmp)) {
						PlayerPrefs.DeleteKey(cyouhouTmp);
						string cyouhou = PlayerPrefs.GetString("cyouhou");
						List<string> cyouhouList = new List<string> ();
						if (cyouhou != null && cyouhou != "") {
							if(cyouhou.Contains(",")){
								cyouhouList = new List<string> (cyouhou.Split (delimiterChars));
							}else{
								cyouhouList.Add(cyouhou);
							}
						}
						cyouhouList.Remove (kuniId.ToString());
						string newCyouhou = "";
						for(int a=0;a<cyouhouList.Count;a++){
							if (a == 0) {
								newCyouhou = cyouhouList[a];
							} else {
								newCyouhou = newCyouhou + "," + cyouhouList[a];
							}
						}
						PlayerPrefs.SetString ("cyouhou",newCyouhou);
					}
				}
			}

			PlayerPrefs.SetString("clearedKuni",clearedKuni);

		} else {
			//Lose
			PlayerPrefs.SetInt("winChecker",2);
			List<string> clearedKuniList = new List<string> ();
			if (clearedKuni != null && clearedKuni != "") {
				if (clearedKuni.Contains (",")) {
					clearedKuniList = new List<string> (clearedKuni.Split (delimiterChars));
				} else {
					clearedKuniList.Add (clearedKuni);
				}
			}

			for (int i = 0; i < seiryokuList.Count; i++) {
				int tmpDaimyoId = int.Parse(seiryokuList [i]);

				if (tmpDaimyoId == daimyoId) {
					int kuniId = i + 1;
					List<int> openKuniIdList = new List<int> ();
					openKuniIdList = kuni.getMappingKuni(kuniId);
					for (int j = 0; j < openKuniIdList.Count; j++) {
						int openKuniId = openKuniIdList [j];
						if(newSeiryokuList[openKuniId-1] == myDaimyo.ToString()){
							newSeiryokuList [openKuniId - 1] = daimyoId.ToString();

							//delete cleared kuni
							clearedKuniList.Remove (openKuniId.ToString());

							//Delete Jyosyu
							string tempJyosyu = "jyosyu" + openKuniId.ToString ();
							PlayerPrefs.DeleteKey (tempJyosyu);

							//Delete Stage Clear
							string tempKuni = "kuni" + openKuniId.ToString ();
							PlayerPrefs.DeleteKey (tempKuni);

						}
					}
				}
			}
			string newClearedKuni = "";
			for (int i = 0; i < clearedKuniList.Count; i++) {
				if (i == 0) {
					newClearedKuni = clearedKuniList [i];
				} else {
					newClearedKuni = newClearedKuni + "," + clearedKuniList [i];
				}
			}
			PlayerPrefs.SetString ("clearedKuni", newClearedKuni);
			if (newClearedKuni == null || newClearedKuni == "") {
				PlayerPrefs.SetBool ("gameOverFlg", true);
				PlayerPrefs.Flush ();
				Debug.Log ("gameOver Set");
			}


		}

		//Create New Seiryoku
		string newSeiryoku = "";
		bool gameClearFlg = true;
		for (int l = 0; l < newSeiryokuList.Count; l++) {
			if(l==0){
				newSeiryoku = newSeiryokuList[l];
			}else{
				newSeiryoku = newSeiryoku + "," + newSeiryokuList[l];
			}
			if(gameClearFlg){
				if(newSeiryokuList[l] != myDaimyo.ToString()){
					gameClearFlg = false;
				}
			}
		}
		PlayerPrefs.SetBool("gameClearFlg",gameClearFlg);
		PlayerPrefs.SetString("seiryoku",newSeiryoku);

		//Delete open
		kuni.updateOpenKuni (myDaimyo, newSeiryoku);
		PlayerPrefs.Flush ();

	}

    public void deleteEnemyGunzeiData(int kuniId) {
        for(int i=1; i<11; i++) {
            string key = kuniId.ToString() + "-" + i.ToString();
            string deleteLabel1 = "attack" + key;
            string deleteLabel2 = "remain" + key;
            PlayerPrefs.DeleteKey(deleteLabel1);
            PlayerPrefs.DeleteKey(deleteLabel2);
        }
        PlayerPrefs.Flush();
    }

    

}
