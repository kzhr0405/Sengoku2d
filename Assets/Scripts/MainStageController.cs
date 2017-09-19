using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainStageController : MonoBehaviour {

	public bool myKuniQtyIsBiggestFlg = true;
	public bool myKuniQtyIsTwiceFlg = true;
	public float timer;
	public GameObject timerObj;
	public bool hyourouFull = false;
	public int hyourouMax = 100;
	public int nowHyourou;
	public bool doneCyosyuFlg = false;
	public double cyosyuMstTime = 10800;
    public int mstHour = 3;
    public int myDaimyo;
	public int myKuniQty = 0;
	public GameObject currentHyourou;
	public double yearTimer;
	public string lastLoginDateString = "";

    public int addJinkeiNo = 0;
    public int minusBusyoQty = 0;
    public bool hardFlg;

    //Season
    public bool seasonChangedFlg = false;
    public int nowSeason;

    //Event 
    public float rdmEventTimer;
	public float rdmEventMin = 30;
	public float rdmEventMax = 180;
    public bool eventStopFlg = false;

    public bool gameOverFlg = false;
	public bool gameClearFlg = false;
	public float remainClearTime = 5.0f;

	//SE
	public AudioSource[] audioSources;

    //AD
    public bool adRunFlg = false;

    //IAP
    public bool iapRunFlg = false;

    //Tutorial
    public bool tutorialDoneFlg = false;

    //public reward
    public GameObject reward;
    Text HyourouCurrentValue;

    //Seiryoku KuniQty
    public List<int> daimyoKuniQtyList;
    public List<int> daimyoList;

    //movie
    public GameObject MessageStaminaObject;

    public void Start () {


        Resources.UnloadUnusedAssets();

        //Set Object
        HyourouCurrentValue = currentHyourou.GetComponent<Text>();
        tutorialDoneFlg = PlayerPrefs.GetBool("tutorialDoneFlg");
        GameObject.Find("HyourouMaxValue").GetComponent<Text>().text = hyourouMax.ToString();
        nowHyourou = PlayerPrefs.GetInt("hyourou");
        currentHyourou.GetComponent<Text>().text = nowHyourou.ToString();
        
        /*Sound Controller*/
        if (GameObject.Find ("BGMController") == null) {			
			string bgmPath = "Prefabs/Common/SoundController/BGMController";
			GameObject bgmObj = Instantiate (Resources.Load (bgmPath)) as GameObject;
			bgmObj.name = "BGMController";
		}

		if (GameObject.Find ("SEController") == null) {		
			string sePath = "Prefabs/Common/SoundController/SEController";
			GameObject seObj = Instantiate (Resources.Load (sePath)) as GameObject;
			seObj.name = "SEController";

		}
		BGMSESwitch bgm = new BGMSESwitch ();
		bgm.StopSEVolume ();
		bgm.StopBGMVolume ();

        //Timer
        timerObj = GameObject.Find("TimerValue").gameObject;

		//SE
		audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [5].Play ();

        //Reward
        if (Application.loadedLevelName != "tutorialMain") {
            if (GameObject.Find("DataStore")) {
                DataReward DataReward = GameObject.Find("DataStore").GetComponent<DataReward>();
                if (DataReward.itemGrpList.Count > 0) {
                    reward.SetActive(true);
                }
            }
        }

		/*--------------------*/
		/*Game Over*/
		/*--------------------*/
		gameOverFlg = PlayerPrefs.GetBool("gameOverFlg");
		if (gameOverFlg) {
			audioSources [4].Play ();
			SceneManager.LoadScene ("clearOrGameOver");	
		
		} else {
			/*--------------------*/
			/*Game Clear*/
			/*--------------------*/
			gameClearFlg = PlayerPrefs.GetBool ("gameClearFlg");
			if (gameClearFlg) {
				
				audioSources [3].Play ();
				SceneManager.LoadScene ("clearOrGameOver");
				
			} else {

				Entity_kuni_mst kuniMst = Resources.Load ("Data/kuni_mst") as Entity_kuni_mst;
				Entity_kuni_mapping_mst kuniMappingMst = Resources.Load ("Data/kuni_mapping_mst") as Entity_kuni_mapping_mst;
				Daimyo daimyo = new Daimyo();

				//Base Info.
				int kuniLv = PlayerPrefs.GetInt ("kuniLv");
				int money = PlayerPrefs.GetInt ("money");
				int busyoDama = PlayerPrefs.GetInt ("busyoDama");
                bool soubujireiFlg = false;
                if (Application.loadedLevelName != "tutorialMain") {
                    soubujireiFlg = PlayerPrefs.GetBool ("soubujireiFlg");
                }
				GameObject.Find ("KuniLvValue").GetComponent<Text> ().text = kuniLv.ToString ();
				GameObject.Find ("MoneyValue").GetComponent<Text> ().text = money.ToString ();
				GameObject.Find ("BusyoDamaValue").GetComponent<Text> ().text = busyoDama.ToString ();

                if (Application.loadedLevelName != "tutorialMain") {
                    myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
                }else {
                    myDaimyo = 1;
                }
				string myDaimyoName = daimyo.getName (myDaimyo);
				GameObject.Find ("DaimyoValue").GetComponent<Text> ().text = myDaimyoName;

                int syogunDaimyoId = 0;
                if (Application.loadedLevelName != "tutorialMain") {
                    syogunDaimyoId = PlayerPrefs.GetInt ("syogunDaimyoId");
                }else {
                    syogunDaimyoId = 14; //ashikaga
                }
				if (syogunDaimyoId != myDaimyo) {
					if (GameObject.Find ("Bakuhu")) {
						GameObject.Find ("Bakuhu").gameObject.SetActive (false);
					}
				}

                //My Daimyo Kamon on Status
                string myDaimyoStatusPath = "Prefabs/Kamon/MyDaimyoKamon/" + myDaimyo.ToString();
                GameObject.Find("Kamon").GetComponent<Image>().sprite =
                    Resources.Load(myDaimyoStatusPath, typeof(Sprite)) as Sprite;

                //Mode
                if (Application.loadedLevelName != "tutorialMain") {
                    hardFlg = PlayerPrefs.GetBool("hardFlg");
                }
                string modeString = "";
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    if (hardFlg) {
                        modeString = "Hard";
                    }else {
                        modeString = "Normal";
                    }
                }else {
                    if (hardFlg) {
                        modeString = "煉獄";
                    }else {
                        modeString = "普通";
                    }
                }
                GameObject.Find("Mode").transform.FindChild("Text").GetComponent<Text>().text = modeString;
                

                //Adjust Enemy Busyo Qty by Purchased Jinkei
                if (PlayerPrefs.GetBool("addJinkei1")) {
                    addJinkeiNo = 1;
                }
                if (PlayerPrefs.GetBool("addJinkei2")) {
                    addJinkeiNo = addJinkeiNo + 1;
                }
                if (PlayerPrefs.GetBool("addJinkei3")) {
                    addJinkeiNo = addJinkeiNo + 1;
                }
                if (PlayerPrefs.GetBool("addJinkei4")) {
                    addJinkeiNo = addJinkeiNo + 1;
                }
                float minustPercent = 20;
                for (int i = 0; i < addJinkeiNo; i++) {
                    float percent = Random.value;
                    percent = percent * 100;
                    if (percent <= minustPercent) {
                        minusBusyoQty = minusBusyoQty + 1; 
                    }
                }
                //maxmum adjustment
                if(minusBusyoQty != 0) {
                    if(addJinkeiNo<3) {
                        minusBusyoQty = 1;
                    }else {
                        if(minusBusyoQty>2) {
                            minusBusyoQty = 2;
                        }
                    }
                }

                //Kuni List
                string openKuni = "";
                if (Application.loadedLevelName != "tutorialMain") {
                    openKuni = PlayerPrefs.GetString ("openKuni");
                }else {
                    openKuni = "1,2,3,4";
                }
                
				List<string> openKuniList = new List<string> ();
				char[] delimiterChars = {','};
				if (openKuni != null && openKuni != "") {
					if (openKuni.Contains (",")) {
						openKuniList = new List<string> (openKuni.Split (delimiterChars));
					} else {
						openKuniList.Add (openKuni);
					}
				}

				GameObject kuniIconView = GameObject.Find ("KuniIconView");

                string clearedKuni = "";
                if (Application.loadedLevelName != "tutorialMain") {
                    clearedKuni = PlayerPrefs.GetString("clearedKuni");
                }else {
                    clearedKuni = "1";
                }
                
                List<string> clearedKuniList = new List<string> ();
				if (clearedKuni != null && clearedKuni != "") {
					if (clearedKuni.Contains (",")) {
						clearedKuniList = new List<string> (clearedKuni.Split (delimiterChars));
					} else {
						clearedKuniList.Add (clearedKuni);
					}
				}

				/*View Every Kuni by Master*/
				GameObject KuniMap = GameObject.Find ("KuniMap");

                //Seiryoku Default Setting
                string seiryoku = "";
                if (Application.loadedLevelName != "tutorialMain") {
                    seiryoku = PlayerPrefs.GetString ("seiryoku");
                    if(seiryoku == "") seiryoku = "1,2,3,4,5,6,7,8,3,4,9,10,12,11,13,14,15,16,3,17,18,17,19,8,19,19,20,21,22,23,24,25,26,27,28,29,30,31,31,32,33,34,35,35,36,37,38,38,38,38,31,31,31,39,40,41,41,41,41,42,43,44,45,45,46";
                }
                else {
                    seiryoku = "1,2,3,4,5,6,7,8,3,4,9,10,12,11,13,14,15,16,3,17,18,17,19,8,19,19,20,21,22,23,24,25,26,27,28,29,30,31,31,32,33,34,35,35,36,37,38,38,38,38,31,31,31,39,40,41,41,41,41,42,43,44,45,45,46";
                }
                
                List<string> seiryokuList = new List<string> ();
				seiryokuList = new List<string> (seiryoku.Split (delimiterChars));

				//Count my Kuni QTY
				for (int m=0; m<seiryokuList.Count; m++) {
					int seiryokuId = int.Parse (seiryokuList [m]);
					if (seiryokuId == myDaimyo) {
						myKuniQty = myKuniQty + 1;
					}
                }

                //count enemy kuni qty
                Dictionary<string, int> dic = new Dictionary<string, int>();
                foreach (string key in seiryokuList) {
                    if (dic.ContainsKey(key)) dic[key]++; else dic.Add(key, 1);
                }
                
                //My Doumei
                Color doumeiColor = new Color (100f / 255f, 130f / 255f, 255f / 255f, 255f / 255f); //Blue
                string myDoumei = "";
                if (Application.loadedLevelName != "tutorialMain") {
                    myDoumei = PlayerPrefs.GetString ("doumei");
                }
				List<string> myDoumeiList = new List<string> ();
				if (myDoumei != null && myDoumei != "") {
					if (myDoumei.Contains (",")) {
						myDoumeiList = new List<string> (myDoumei.Split (delimiterChars));
					} else {
						myDoumeiList.Add (myDoumei);
					}
				}

				string kuniPath = "Prefabs/Map/Kuni/";
                KuniInfo kuniScript = new KuniInfo();
                kuniScript.updateOpenKuni(myDaimyo,seiryoku);

                for (int i=0; i<kuniMst.param.Count; i++) {
					int kuniId = kuniMst.param [i].kunId;

					string newKuniPath = kuniPath + kuniId.ToString ();
					int locationX = kuniMst.param [i].locationX;
					int locationY = kuniMst.param [i].locationY;

					GameObject kuni = Instantiate (Resources.Load (newKuniPath)) as GameObject;

					kuni.transform.SetParent (kuniIconView.transform);
					kuni.name = kuniId.ToString ();
					kuni.GetComponent<SendParam> ().kuniId = kuniId;
					kuni.GetComponent<SendParam> ().kuniName = kuniScript.getKuniName(kuniId);
					kuni.transform.localScale = new Vector2 (1, 1);
					kuni.GetComponent<SendParam> ().naiseiItem = kuniMst.param [i].naisei;
					kuni.GetComponent<SendParam> ().soubujireiFlg = soubujireiFlg;

					//Seiryoku Handling
					int daimyoId = int.Parse (seiryokuList [kuniId - 1]);
					string daimyoName = daimyo.getName (daimyoId);

					kuni.GetComponent<SendParam> ().daimyoId = daimyoId;
					kuni.GetComponent<SendParam> ().daimyoName = daimyoName;
					int daimyoBusyoIdTemp = daimyo.getDaimyoBusyoId (daimyoId);

					kuni.GetComponent<SendParam> ().daimyoBusyoId = daimyoBusyoIdTemp;
					BusyoInfoGet busyo = new BusyoInfoGet ();
					if (daimyoBusyoIdTemp != 0) {
						kuni.GetComponent<SendParam> ().daimyoBusyoAtk = busyo.getMaxAtk (daimyoBusyoIdTemp);
						kuni.GetComponent<SendParam> ().daimyoBusyoDfc = busyo.getMaxDfc (daimyoBusyoIdTemp);
					}

					//Enemy Senryoku
					int busyoQty = 0;
					int busyoLv = 0;
					int butaiQty = 0;
					int butaiLv = 0;
					int enemyKuniQty = 1;
				
					//Count QTY of Enemy Kuni
					List<string> checkedKuniList = new List<string> ();
					if (daimyoId != myDaimyo) {
						
                        //For peformance improvement
                        //enemyKuniQty = countLinkedKuniQty (1, kuniId, daimyoId, seiryokuList, checkedKuniList);
                        //need coding!
                        foreach (string key in dic.Keys) {
                            if (int.Parse(key) == daimyoId) enemyKuniQty = dic[key];
                        }

                        if (enemyKuniQty>=myKuniQty){
							myKuniQtyIsBiggestFlg = false;
						}
						if (2 * enemyKuniQty > myKuniQty) {
							myKuniQtyIsTwiceFlg = false;
						}
					} else {
						enemyKuniQty = myKuniQty;
					}

					EnemySenryokuCalc calc = new EnemySenryokuCalc ();
                    busyoQty = calc.EnemyBusyoQtyCalc (myKuniQty, enemyKuniQty, minusBusyoQty);
					if (busyoQty > 12) {
						busyoQty = 12;
					}
					int senryokuRatio = daimyo.getSenryoku (daimyoId);
					busyoLv = calc.EnemyBusyoLvCalc (senryokuRatio);
					butaiQty = calc.EnemyButaiQtyCalc (enemyKuniQty, myKuniQty);
					butaiLv = calc.EnemyButaiLvCalc (senryokuRatio);

					kuni.GetComponent<SendParam> ().busyoQty = busyoQty;
					kuni.GetComponent<SendParam> ().busyoLv = busyoLv;
					kuni.GetComponent<SendParam> ().butaiQty = butaiQty;
					kuni.GetComponent<SendParam> ().butaiLv = butaiLv;
					kuni.GetComponent<SendParam> ().kuniQty = enemyKuniQty;
				

					//Color Handling
					float colorR = (float)daimyo.getColorR(daimyoId);
					float colorG = (float)daimyo.getColorG(daimyoId);
					float colorB = (float)daimyo.getColorB(daimyoId);
					Color kuniColor = new Color (colorR / 255f, colorG / 255f, colorB / 255f, 255f / 255f);

					KuniMap.transform.FindChild (kuni.name).GetComponent<Image> ().color = kuniColor;

					//Daimyo Kamon Image
					string imagePath = "Prefabs/Kamon/" + daimyoId.ToString ();
					kuni.GetComponent<Image> ().sprite = 
						Resources.Load (imagePath, typeof(Sprite)) as Sprite;

					RectTransform kuniTransform = kuni.GetComponent<RectTransform> ();
					kuniTransform.anchoredPosition = new Vector3 (locationX, locationY, 0);

					//My Doumei Check
					if (myDoumei != null && myDoumei != "") {
						if (myDoumeiList.Contains (daimyoId.ToString ())) {
							kuni.GetComponent<SendParam> ().doumeiFlg = true;
							kuni.GetComponent<Image> ().color = doumeiColor;
						}
					}

					//My daimyo Check
					if (daimyoId == myDaimyo) {
						string myDaimyoPath = "Prefabs/Kamon/MyDaimyoKamon/" + myDaimyo.ToString ();
						kuni.GetComponent<Image> ().sprite = 
							Resources.Load (myDaimyoPath, typeof(Sprite)) as Sprite;
						kuni.GetComponent<SendParam> ().clearFlg = true;

						int myHeiryoku = PlayerPrefs.GetInt ("jinkeiHeiryoku");
						kuni.GetComponent<SendParam> ().heiryoku = myHeiryoku;

					} else {
						//Not my daimyo
						//Yukoudo
						string gaikouTemp = "gaikou" + daimyoId;
						int myYukouValue = PlayerPrefs.GetInt (gaikouTemp);
						kuni.GetComponent<SendParam> ().myYukouValue = myYukouValue;


						if (daimyoBusyoIdTemp != 0) {
							StatusGet sts = new StatusGet ();
							int hp = sts.getHp (daimyoBusyoIdTemp, busyoLv);
							int hpResult = hp * 100 * busyoQty;
							string type = sts.getHeisyu (daimyoBusyoIdTemp);
							int chHp = sts.getChHp (type, butaiLv, hp);
							chHp = chHp * butaiQty * busyoQty * 10;

							int totalHei = hpResult + chHp;
							kuni.GetComponent<SendParam> ().heiryoku = totalHei;
						}

						//Action Policy Setting(agrresive false or true)
						if(enemyKuniQty>=3){
							kuni.GetComponent<SendParam> ().aggressiveFlg = getAggressiveFlg (0); // big country
						}else {
							kuni.GetComponent<SendParam> ().aggressiveFlg = getAggressiveFlg (1); // small country
						}

						//Cyouhou Flg
						string cyouhouTmp = "cyouhou" + kuniId;
						if (PlayerPrefs.HasKey (cyouhouTmp)) {
							int cyouhouSnbRankId = PlayerPrefs.GetInt (cyouhouTmp);
							kuni.GetComponent<SendParam> ().cyouhouSnbRankId = cyouhouSnbRankId;
						}
					}
				}

				//Color Change for kuni icon "Open but never cleared"
				Color openKuniColor = new Color (255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f); //Yellow

				for (int i=0; i<openKuniList.Count; i++) {
					string openKuniId = openKuniList [i];
					//Flg Change
					GameObject targetOpenKuni = GameObject.Find ("KuniIconView").transform.FindChild (openKuniId).gameObject;
					targetOpenKuni.GetComponent<SendParam> ().openFlg = true;
					bool doumeiFlg = targetOpenKuni.GetComponent<SendParam> ().doumeiFlg;

					//Color Change
					if (!clearedKuniList.Contains (openKuniId)) {
						if (!doumeiFlg) {
							targetOpenKuni.GetComponent<Image> ().color = openKuniColor;
						}
					}
				}
                bool rengouFlg = PlayerPrefs.GetBool("rengouFlg");
                string rengouDaimyo = PlayerPrefs.GetString("rengouDaimyo");
                UpdateRengouKuniIcon(rengouFlg, rengouDaimyo);
                
                //From Naisei or Kassen Check
                bool fromNaiseiFlg = PlayerPrefs.GetBool ("fromNaiseiFlg");
				bool fromKassenFlg = PlayerPrefs.GetBool ("fromKassenFlg");
                bool fromShisyaFlg = PlayerPrefs.GetBool("fromShisyaFlg");
                bool isAttackedFlg = PlayerPrefs.GetBool ("isAttackedFlg");
				bool isKessenFlg = PlayerPrefs.GetBool ("isKessenFlg");
				int winValue = PlayerPrefs.GetInt ("winChecker");
				if (!isAttackedFlg && !isKessenFlg) {

                    if (fromNaiseiFlg || fromKassenFlg) {
						//Click
						int activeKuniId = PlayerPrefs.GetInt ("activeKuniId");
						GameObject.Find ("KuniIconView").transform.FindChild (activeKuniId.ToString ()).GetComponent<SendParam> ().OnClick ();
                        
                        GameObject.Find ("AttackButton").GetComponent<AttackNaiseiView> ().OnClick ();
						string kassenWinLoseFlee = PlayerPrefs.GetString ("kassenWinLoseFlee");

						//Kassen Comment
						if (fromKassenFlg && kassenWinLoseFlee != null) {

							List<string> kassenWinLoseFleeList = new List<string> ();
							kassenWinLoseFleeList = new List<string> (kassenWinLoseFlee.Split (delimiterChars));
							int enemyDaimyoId = 0;
							if (kassenWinLoseFleeList [1] == "3" || kassenWinLoseFleeList [1] == "4") {
								enemyDaimyoId = int.Parse (kassenWinLoseFleeList [0]);
							} else {
								enemyDaimyoId = int.Parse (seiryokuList [activeKuniId - 1]);
							}
							KassenEvent kEvent = new KassenEvent ();
							kEvent.MakeKassenComment (kassenWinLoseFlee, enemyDaimyoId, activeKuniId);
						}

						PlayerPrefs.SetBool ("fromNaiseiFlg", false);
						PlayerPrefs.SetBool ("fromKassenFlg", false);
						PlayerPrefs.DeleteKey ("kassenWinLoseFlee");
						PlayerPrefs.Flush ();

                    }else {
                        //random kick event
                        if(tutorialDoneFlg && Application.loadedLevelName != "tutorialMain") {
                            if (!eventStopFlg) {
                                if(!fromShisyaFlg) {
                                    int rdmId = UnityEngine.Random.Range(1, 11);
                                    if(rdmId >= 5) {
                                        MainEventHandler gameEvent = new MainEventHandler();
                                        gameEvent.mainHandler(myKuniQtyIsBiggestFlg, myKuniQtyIsTwiceFlg);                                        
                                    }
                                }else {
                                    PlayerPrefs.SetBool("fromShisyaFlg", false);
                                    PlayerPrefs.Flush();
                                }
                            }
                        }
                    }
				} else {
                    //Kessen or IsAttacked
                   
                    int activeDaimyoId = PlayerPrefs.GetInt ("activeDaimyoId");
					string comment = "";
					bool msgOnFlg = false;
					if (isAttackedFlg && winValue == 2) {
						msgOnFlg = true;
						KuniInfo kuni = new KuniInfo ();
						int activekuniId = PlayerPrefs.GetInt ("activeKuniId");
						string kuniName = kuni.getKuniName (activekuniId);

                        string stageString = "kuni" + activekuniId.ToString();
                        string addComment = "";
                        if (!PlayerPrefs.HasKey(stageString)) {
                            
                            if (Application.systemLanguage != SystemLanguage.Japanese) {
                                addComment = "I took away all the castles！";
                            }else {
                                addComment = "全ての城を奪ってやったぞ！";
                            }
                        }
                        else {
                            string clearedStageString = PlayerPrefs.GetString(stageString);
                            List<string> clearedStageList = new List<string>();
                            if (clearedStageString != null && clearedStageString != "") {
                                if(!clearedStageString.Contains(",")) {
                                    clearedStageList.Add(clearedStageString);
                                }else {
                                    clearedStageList = new List<string>(clearedStageString.Split(delimiterChars));
                                }
                            }
                            int stageQty = 10 - clearedStageList.Count;
                            
                            if (Application.systemLanguage != SystemLanguage.Japanese) {
                                addComment = "I took " + stageQty.ToString() + " castles!";
                            }else {
                                addComment = stageQty.ToString() + "つの城を奪ってやったぞ！";
                            }
                        }
                        
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            comment = "hehehe, I can imagine " + myDaimyoName + " is regreting by lost " + kuniName + "\n" + addComment;
                        }else {
                            comment = "ふふふ、" + myDaimyoName + "め。" + kuniName + "を盗られて悔しがってる姿が目に浮かぶわ。\n" + addComment;
                        }

                    } else if (isAttackedFlg && winValue == 1) {
						msgOnFlg = true;
						KuniInfo kuni = new KuniInfo ();
						int activekuniId = PlayerPrefs.GetInt ("activeKuniId");
						string kuniName = kuni.getKuniName (activekuniId);
						
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            comment = "Darn! I didn't get "+ kuniName + " staked everything I had";
                        }else {
                            comment = "ぬう、乾坤一擲の力を以ってしても" + kuniName + "を落とせぬとは。";
                        }

                    } else if (isKessenFlg && winValue == 1) {
						msgOnFlg = true;

                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            comment = "I lost the final battle. My glory only last for a time. How will my name remain in history.";
                        }else {
                            comment = "決戦に負けてしもうた。儚い世であったわ。我が名は後世にどう残るのかのう。";
                        }


                    } else if (isKessenFlg && winValue == 2) {
						msgOnFlg = true;
						string daimyoName = daimyo.getName (activeDaimyoId);
						
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            comment = "Hehe, I defeated " + myDaimyoName + " in the final battle.";
                        }else {
                            comment = "ははは、決戦に勝ったぞ。" + myDaimyoName + "如きに敗れる" + daimyoName + "ではないわ。";
                        }


                    } else if (isKessenFlg && winValue == 0) {
						msgOnFlg = true;
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            comment = "Hahaha, he ran away. I will take you on anytime.";
                        }else {
                            comment = "ははは､無様じゃのう。逃げ出しおったわ。いつでも相手して進ぜよう。";
                        }


                        int nowKessenHyourou = PlayerPrefs.GetInt ("hyourou");
						int kessenHyourou = PlayerPrefs.GetInt ("kessenHyourou");
						int half = kessenHyourou / 2;
						int newKessenHyourou = nowKessenHyourou - half;
						PlayerPrefs.SetInt ("hyourou",newKessenHyourou);
                        HyourouCurrentValue.text = newKessenHyourou.ToString ();
						PlayerPrefs.Flush ();
					}
					PlayerPrefs.DeleteKey ("winChecker");

					if (msgOnFlg) {
						PlayerPrefs.DeleteKey ("isAttackedFlg");
						PlayerPrefs.DeleteKey ("isKessenFlg");

						string eventCommentPath = "Prefabs/Common/EventCommentOnMap";
						GameObject commentObj = Instantiate (Resources.Load (eventCommentPath)) as GameObject;
						commentObj.transform.SetParent (GameObject.Find ("Map").transform);
						commentObj.transform.localScale = new Vector2 (2.0f, 2.0f);
						commentObj.transform.localPosition = new Vector2 (0, 0);
						commentObj.GetComponent<FadeoutSenpou> ().enabled = true;
						commentObj.name = "EventCommentOnMap";
						commentObj.transform.FindChild ("SerihuText").GetComponent<Text> ().text = comment;

						int daimyoBusyoId = daimyo.getDaimyoBusyoId (activeDaimyoId);
						string daimyoImagePath = "Prefabs/Player/Sprite/unit" + daimyoBusyoId.ToString ();
						commentObj.transform.FindChild ("Mask").transform.FindChild ("BusyoImage").GetComponent<Image> ().sprite =
							Resources.Load (daimyoImagePath, typeof(Sprite)) as Sprite;
					}
				}

                changerableByTime (seiryokuList);                
			}
		}
	}	

	void Update(){

        if(tutorialDoneFlg && Application.loadedLevelName != "tutorialMain") {
		    if (!gameClearFlg) {
			    if (!gameOverFlg) {
				    //Hyourou Check
				    if (hyourouFull == true) {
                        nowHyourou = int.Parse (currentHyourou.GetComponent<Text> ().text);
					    if (nowHyourou < hyourouMax) {
						    hyourouFull = false;
					    }

					    //Hyourou Count Down
				    } else if (hyourouFull == false) {

					    timer -= Time.deltaTime;
				
					    if (timer > 0.0f) {
						    //On Play
						    timerObj.GetComponent<Text> ().text = ((int)timer).ToString ();
					
					    } else {

                            //Add Hyourou
                            nowHyourou = int.Parse (HyourouCurrentValue.text);
                            nowHyourou = nowHyourou + 1;

						    if (nowHyourou >= hyourouMax) {
							    hyourouFull = true;
						    }

                            HyourouCurrentValue.text = nowHyourou.ToString ();
						    PlayerPrefs.SetInt ("hyourou", nowHyourou);
						    System.DateTime now = System.DateTime.Now;
						    PlayerPrefs.SetString ("lasttime", now.ToString ());
						    PlayerPrefs.Flush ();
					
						    //Reset
						    timerObj.GetComponent<Text> ().text = "180";
						    timer = 180;

					    }	
				    }

				    //Year & Season
                    if(!seasonChangedFlg) {
				        yearTimer -= Time.deltaTime;
				        if (yearTimer <= 0.0f) {

                            //check
                            bool boardCheckFlg = false;
                            GameObject[] boards = GameObject.FindGameObjectsWithTag("Board");
                            GameObject[] onBoards = GameObject.FindGameObjectsWithTag("OnBoard");
                            if (boards.Length > 0 || onBoards.Length > 0) boardCheckFlg = true;

                            if (!boardCheckFlg) {
                                //Season Change
                                eventStopFlg = true;
                                seasonChangedFlg = true;
                                doneCyosyuFlg = false;
                                PlayerPrefs.SetBool("doneCyosyuFlg", false);
                                PlayerPrefs.Flush();
                                SeasonChange();                        
                                yearTimer = cyosyuMstTime;
                            }
                        }
			    
                    }


				    //Date Change Check
				    lastLoginDateString = PlayerPrefs.GetString ("loginDate");
				    if (lastLoginDateString == null || lastLoginDateString == "") {
					    lastLoginDateString = System.DateTime.Today.ToString ();
					    PlayerPrefs.SetString ("loginDate", lastLoginDateString);
					    PlayerPrefs.SetBool ("questDailyFlg14", true);
					    PlayerPrefs.Flush ();
				    }
				    System.DateTime loginDate = System.DateTime.Parse (lastLoginDateString);
				    System.TimeSpan loginSpan = System.DateTime.Today - loginDate;
				    double loginSpanDay = loginSpan.TotalDays;

				    if (loginSpanDay >= 1) {
					    //Change Date
					    //Quest Flg Change
					    List<int> dailyQuestList = new List<int> ();

					    //Quest Data Orderby
					    Entity_quest_mst questMst = Resources.Load ("Data/quest_mst") as Entity_quest_mst;
					    for (int i = 0; i < questMst.param.Count; i++) {
						    bool dailyFlg = questMst.param [i].daily;

						    if (dailyFlg) {
							    string tmp = "questDailyFlg" + i.ToString ();
							    string tmp2 = "questDailyReceivedFlg" + i.ToString ();

							    PlayerPrefs.DeleteKey (tmp);
							    PlayerPrefs.DeleteKey (tmp2);
						    }
					    }

                        Entity_quest_count_mst questCountMst = Resources.Load("Data/quest_count_mst") as Entity_quest_count_mst;
                        for (int i = 0; i < questCountMst.param.Count; i++) {
                            bool dailyFlg = questCountMst.param[i].daily;

                            if (dailyFlg) {
                                string tmp = "questDailyCountReceivedFlg" + i.ToString();                                
                                PlayerPrefs.DeleteKey(tmp);
                            }
                        }
                        

                        lastLoginDateString = System.DateTime.Today.ToString ();
					    PlayerPrefs.SetString ("loginDate", lastLoginDateString);

					    PlayerPrefs.SetBool ("questDailyFlg14", true);
					    PlayerPrefs.Flush ();
				    }

                    //Event Handler                
                    if(!eventStopFlg) {
                        rdmEventTimer -= Time.deltaTime;
				        if (rdmEventTimer <= 0) {
					        MainEventHandler gameEvent = new MainEventHandler ();
					        gameEvent.mainHandler (myKuniQtyIsBiggestFlg, myKuniQtyIsTwiceFlg);

					        rdmEventTimer = UnityEngine.Random.Range (rdmEventMin, rdmEventMax);

				        }
				        PlayerPrefs.SetFloat ("rdmEventTimer", rdmEventTimer);
                    }
                
			    }
		    } else {
			    remainClearTime-= Time.deltaTime;
			    if (remainClearTime <= 0) {
				    audioSources [5].Play ();

				    SceneManager.LoadScene ("clearOrGameOver");
			    }
		    }
        }
	}

	public void SetSeason(int seasonId){

        if (Application.systemLanguage != SystemLanguage.Japanese) {
            if (seasonId == 1) {
                GameObject.Find("SeasonValue").GetComponent<Text>().text = "Spring";

            }
            else if (seasonId == 2) {
                GameObject.Find("SeasonValue").GetComponent<Text>().text = "Summer";

            }
            else if (seasonId == 3) {
                GameObject.Find("SeasonValue").GetComponent<Text>().text = "Autumn";

            }
            else if (seasonId == 4) {
                GameObject.Find("SeasonValue").GetComponent<Text>().text = "Winter";
            }
        }else { 
            if (seasonId == 1){
			    GameObject.Find ("SeasonValue").GetComponent<Text> ().text = "春";

		    }else if(seasonId == 2){
			    GameObject.Find ("SeasonValue").GetComponent<Text> ().text = "夏";

		    }else if(seasonId == 3){
			    GameObject.Find ("SeasonValue").GetComponent<Text> ().text = "秋";

		    }else if(seasonId == 4){
			    GameObject.Find ("SeasonValue").GetComponent<Text> ().text = "冬";
		    }
        }
    }

	public void deleteKeyHistory(string tKey){

		string keyHistory = PlayerPrefs.GetString ("keyHistory");
		char[] delimiterChars = {','};
		List<string> keyHistoryList = new List<string>();
		if (keyHistory != null && keyHistory != "") {
			if(keyHistory.Contains(",")){
				keyHistoryList = new List<string> (keyHistory.Split (delimiterChars));
			}else{
				keyHistoryList.Add(keyHistory);
			}
		}
		keyHistoryList.Remove(tKey);
		string newKeyHistory = "";
		for(int i=0; i<keyHistoryList.Count; i++){
			if(i==0){
				newKeyHistory = keyHistoryList[i];
			}else{
				newKeyHistory = newKeyHistory + "," + keyHistoryList[i];
			}
		}
		PlayerPrefs.SetString("keyHistory",newKeyHistory);
		PlayerPrefs.DeleteKey (tKey);
		
		PlayerPrefs.Flush ();
		
		
	}


	public int countLinkedKuniQty(int kuniQty, int myKuniId, int myDaimyoId, List<string>seiryokuList, List<string>checkedKuniList){

		List<int> dstKuniList = new List<int>();
		KuniInfo kuni = new KuniInfo ();

		dstKuniList = kuni.getMappingKuni(myKuniId);

		foreach(int dstKuniId in dstKuniList){
			if (!checkedKuniList.Contains (dstKuniId.ToString())) {

				string tmpDaimyoId = seiryokuList[dstKuniId - 1];
				checkedKuniList.Add (dstKuniId.ToString ());
				checkedKuniList.Add (myKuniId.ToString ());

				if (int.Parse (tmpDaimyoId) == myDaimyoId) {

					kuniQty = countLinkedKuniQty (kuniQty, dstKuniId, myDaimyoId, seiryokuList, checkedKuniList) + 1;

				}

			}
		}

		return kuniQty;
	}

	public bool getAggressiveFlg (int kuniSize){

		bool aggressiveFlg = false;

		float percent = UnityEngine.Random.value;
		percent = percent * 100;

		if (kuniSize == 0) {
			//big
			if (percent <= 70) {
				aggressiveFlg = true;
			}

		} else if (kuniSize == 1) {
			//small
			if (percent <= 30) {
				aggressiveFlg = true;
			}
		}


		return aggressiveFlg;
	}

	public void questExtension(){
		bool remainActiveQuest = false;

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
						remainActiveQuest = true;
						break;
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
						remainActiveQuest = true;
						break;
					}
				}
			}
		}

		if (GameObject.Find ("Quest")) {
			GameObject quest = GameObject.Find ("Quest").gameObject;
			if (remainActiveQuest) {
				quest.transform.FindChild ("Ex").GetComponent<Image> ().enabled = true;
			} else {
				quest.transform.FindChild ("Ex").GetComponent<Image> ().enabled = false;
			}
		}
	}

	public void changerableByTime(List<string> seiryokuList) {
        
		Daimyo daimyo = new Daimyo ();
		char[] delimiterChars = { ',' };

		/*Timer Handling*/
		//Last Log-In Time
		string timestring = PlayerPrefs.GetString ("lasttime");
		if (timestring == null || timestring == "")
			timestring = System.DateTime.Now.ToString ();
		System.DateTime datetime = System.DateTime.Parse (timestring);
		System.TimeSpan span = System.DateTime.Now - datetime;

		//経過時間を秒,時間で取得
		double spantime = span.TotalSeconds;


		//spantimeでスタミナの回復分を求める
		double staminaDouble = spantime / 180;
		int addHyourou = (int)staminaDouble;
		int amariSec = (int)spantime - (addHyourou * 180);
		amariSec = 180 - amariSec;


        //Hyourou Full Check
		if (hyourouMax <= nowHyourou) {
			hyourouFull = true;
            HyourouCurrentValue.text = hyourouMax.ToString ();
			PlayerPrefs.SetInt ("hyourou", hyourouMax);
			PlayerPrefs.Flush ();

		} else {

			if (addHyourou > 0) {
				int newHyourou = nowHyourou + addHyourou;

				if (hyourouMax <= newHyourou) {
					hyourouFull = true;
					PlayerPrefs.SetInt ("hyourou", hyourouMax);
					PlayerPrefs.Flush ();
                    HyourouCurrentValue.text = hyourouMax.ToString ();

				} else {
					hyourouFull = false;
					PlayerPrefs.SetInt ("hyourou", newHyourou);
					PlayerPrefs.Flush ();
                    HyourouCurrentValue.text = newHyourou.ToString ();

					//Timer
					GameObject.Find ("TimerValue").GetComponent<Text> ().text = amariSec.ToString ();
					timer = (float)amariSec;
				}

				//終了時の処理
				// 現在の時刻を取得
				System.DateTime now = System.DateTime.Now;
				// 文字列に変換して保存
				PlayerPrefs.SetString ("lasttime", now.ToString ());
				PlayerPrefs.Flush ();

			} else {
				hyourouFull = false;
				PlayerPrefs.SetInt ("hyourou", nowHyourou);
				PlayerPrefs.Flush ();
                HyourouCurrentValue.text = nowHyourou.ToString ();

				//Timer
				GameObject.Find ("TimerValue").GetComponent<Text> ().text = amariSec.ToString ();
				timer = (float)amariSec;
			}
		}


		/*Year & Season -Auto Count- Start*/
		string lastSeasonChangeTime = PlayerPrefs.GetString ("lastSeasonChangeTime");
		if (lastSeasonChangeTime == null || lastSeasonChangeTime == "") {
			lastSeasonChangeTime = System.DateTime.Now.ToString ();
			PlayerPrefs.SetString ("lastSeasonChangeTime", lastSeasonChangeTime);
			PlayerPrefs.Flush ();
		}
		System.DateTime SChangeTime = System.DateTime.Parse (lastSeasonChangeTime);
		System.TimeSpan scSpan = System.DateTime.Now - SChangeTime;
		double scSpanHour = scSpan.TotalHours;
		double scSpanSec = scSpan.TotalSeconds;

		string yearSeason = PlayerPrefs.GetString ("yearSeason");
		if (yearSeason == null || yearSeason == "") {
			PlayerPrefs.SetString ("yearSeason", "1560,1");
			yearSeason = "1560,1";
			PlayerPrefs.Flush ();
		}

		string[] yearSeasonList = yearSeason.Split (delimiterChars);
		int nowYear = int.Parse (yearSeasonList [0]);
		nowSeason = int.Parse (yearSeasonList [1]);
        //        if (scSpanHour >= mstHour) {

        //check
        bool boardCheckFlg = false;
        GameObject[] boards = GameObject.FindGameObjectsWithTag("Board");
        GameObject[] onBoards = GameObject.FindGameObjectsWithTag("OnBoard");
        if (boards.Length > 0 || onBoards.Length > 0) boardCheckFlg = true;

        if (scSpanHour >= mstHour) {
        //if (scSpanHour >= 0) {//test
            if (!boardCheckFlg) {
                eventStopFlg = true;
                seasonChangedFlg = true;
                doneCyosyuFlg = false;
                PlayerPrefs.SetBool("doneCyosyuFlg", false);
                PlayerPrefs.Flush();
                SeasonChange();
                yearTimer = cyosyuMstTime;
            }
        }else {
            seasonChangedFlg = false;
            yearTimer = cyosyuMstTime - scSpanSec;
		}

		GameObject.Find ("YearValue").GetComponent<Text> ().text = nowYear.ToString ();
		SetSeason (nowSeason);



        /*Year & Season End*/


        /*--------------------*/
        /*Gunzei*/
        /*--------------------*/
        if (tutorialDoneFlg && Application.loadedLevelName != "tutorialMain") {
            string keyHistory = PlayerPrefs.GetString ("keyHistory");
		    List<string> keyHistoryList = new List<string> ();
		    if (keyHistory != null && keyHistory != "") {
			    if (keyHistory.Contains (",")) {
				    keyHistoryList = new List<string> (keyHistory.Split (delimiterChars));
			    } else {
				    keyHistoryList.Add (keyHistory);
			    }
		    }

		    for (int n = 0; n < keyHistoryList.Count; n++) {
			    string keyTemp = keyHistoryList [n];
			    bool ExistFlg = false;
			    foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Gunzei")){
				    if(obs.name == keyTemp){
					    ExistFlg = true;
				    }
			    }
			    if (!ExistFlg) {

				    if (PlayerPrefs.HasKey (keyTemp)) {
					    string keyValue = PlayerPrefs.GetString (keyTemp);
					    if (keyValue != null) {
						    List<string> keyValueList = new List<string> ();
						    keyValueList = new List<string> (keyValue.Split (delimiterChars));
                            
                            string gunzeiTime = keyValueList [0];
						    System.DateTime gunzeiDatetime = System.DateTime.Parse (gunzeiTime);
						    System.TimeSpan gunzeiSpan = System.DateTime.Now - gunzeiDatetime;
						    double gunzeiSpantime = gunzeiSpan.TotalSeconds;

						    List<string> srcDstKuniList = new List<string> ();
						    char[] keyDelimiterChars = { '-' };
						    srcDstKuniList = new List<string> (keyTemp.Split (keyDelimiterChars));
						    int srcDaimyoId = int.Parse (keyValueList [1]);
						    int dstDaimyoId = int.Parse (keyValueList [2]);
						    int srcKuni = int.Parse (srcDstKuniList [0]);
						    int dstKuni = int.Parse (srcDstKuniList [1]);
						    bool dstEngunFlg = bool.Parse (keyValueList [9]);
						    string dstEngunDaimyoId = keyValueList [10];
						    string dstEngunHei = keyValueList [11];
						    string dstEngunSts = keyValueList [12];

                            if (gunzeiSpantime >= 300) {
                            //if (gunzeiSpantime >= 0) { //test
							    
							    //Simulation
							    Gunzei gunzei = new Gunzei ();
							    myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
							    if (dstDaimyoId != myDaimyo && seiryokuList[dstKuni-1] != myDaimyo.ToString()) {
                                    int enemyHei = gunzei.heiryokuCalc (int.Parse (srcDstKuniList [1]));

								    int engunTotalHei = 0;
								    if (dstEngunFlg) {
									    char[] delimiterChars2 = { ':' };
									    List<string> engunHeiList = new List<string> ();
                                        if(dstEngunHei.Contains(":")) {
									        engunHeiList = new List<string> (dstEngunHei.Split (delimiterChars2));
                                        
                                            for (int k = 0; k < engunHeiList.Count; k++) {
										        engunTotalHei = engunTotalHei + int.Parse (engunHeiList [k]);
									        }
                                        }
                                    }

								    enemyHei = enemyHei + engunTotalHei;

								    int ratio = 0;
								    int myHei = int.Parse (keyValueList [5]);
								    if ((myHei + enemyHei) > 0) {
									    ratio = 100 * myHei / (myHei + enemyHei);
									    if (ratio < 1) {
										    ratio = 1;
									    }
								    }

								    bool winFlg = CheckByProbability (ratio);

								    if (winFlg) {
									    bool noGunzeiFlg = true;
									    gunzei.win (keyTemp, int.Parse (keyValueList [1]), int.Parse (keyValueList [2]), noGunzeiFlg, dstKuni);

								    } else {
									    deleteKeyHistory (keyTemp);
								    }
							    } else {
                                    if (seiryokuList[dstKuni - 1] == myDaimyo.ToString()) {
                                        //自大名が勢力を拡大したことにより、途中で攻撃対象の大名が自大名に変わったケース
                                        dstDaimyoId = myDaimyo;
                                        dstEngunFlg = false;
                                        dstEngunDaimyoId = "";
                                        dstEngunSts = "";
                                    }
								    MyDaimyoWasAttacked atked = new MyDaimyoWasAttacked ();
								    atked.wasAttacked (keyTemp, srcKuni, dstKuni, srcDaimyoId, dstDaimyoId, dstEngunFlg, dstEngunDaimyoId, dstEngunSts);

							    }
						    } else {

							    //View Previous
							    string path = "Prefabs/Map/Gunzei";
							    GameObject Gunzei = Instantiate (Resources.Load (path)) as GameObject;			
							    Gunzei.transform.SetParent (GameObject.Find ("Panel").transform.FindChild("GunzeiView").transform);

							    Gunzei.GetComponent<Gunzei> ().key = keyTemp;
							    Gunzei.GetComponent<Gunzei> ().srcKuni = int.Parse (srcDstKuniList [0]);
							    Gunzei.GetComponent<Gunzei> ().dstKuni = int.Parse (srcDstKuniList [1]);
							    Gunzei.GetComponent<Gunzei> ().spantime = gunzeiSpantime;
							    Gunzei.GetComponent<Gunzei> ().srcDaimyoId = srcDaimyoId;
							    Gunzei.GetComponent<Gunzei> ().dstDaimyoId = dstDaimyoId;
							    Gunzei.GetComponent<Gunzei> ().srcDaimyoName = keyValueList [3];
							    Gunzei.GetComponent<Gunzei> ().dstDaimyoName = keyValueList [4];
							    Gunzei.GetComponent<Gunzei> ().myHei = int.Parse (keyValueList [5]);
							    Gunzei.GetComponent<Gunzei> ().dstEngunFlg = bool.Parse (keyValueList [9]);
							    Gunzei.GetComponent<Gunzei> ().dstEngunDaimyoId = keyValueList [10];
							    Gunzei.GetComponent<Gunzei> ().dstEngunHei = keyValueList [11];
							    Gunzei.GetComponent<Gunzei> ().dstEngunSts = keyValueList [12];
							    Gunzei.name = keyTemp;

							    RectTransform GunzeiTransform = Gunzei.GetComponent<RectTransform> ();
							    GunzeiTransform.anchoredPosition = new Vector3 (int.Parse (keyValueList [6]), int.Parse (keyValueList [7]), 0);

							    if (keyValueList [8] == "right") {
								    Gunzei.transform.localScale = new Vector2 (1, 1);
							    } else {
								    Gunzei.transform.localScale = new Vector2 (-1, 1);
                                    Gunzei.transform.FindChild("MsgBack").localScale = new Vector2(-1, 1);
                                    Gunzei.GetComponent<Gunzei> ().leftFlg = true;
							    }
						    }
					    } else {
						    PlayerPrefs.DeleteKey (keyTemp);
						    PlayerPrefs.Flush ();

					    }
				    } else {

					    //Delete History
					    deleteKeyHistory(keyTemp	);

				    }
			    }
		    }

		    //Metsubou Flg Check
		    if (PlayerPrefs.HasKey ("metsubou")) {
			    AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
			    audioSources [4].Play ();
			    audioSources [6].Play ();

			    string metsubou = PlayerPrefs.GetString ("metsubou");
			    List<string> metsubouList = new List<string> ();
			    if (metsubou.Contains (",")) {
				    metsubouList = new List<string> (metsubou.Split (delimiterChars));
			    } else {
				    metsubouList.Add (metsubou);
			    }

			    //Metsubou Message
			    string pathOfBack = "Prefabs/Event/TouchEventBack";
			    GameObject back = Instantiate (Resources.Load (pathOfBack)) as GameObject;
			    back.transform.SetParent (GameObject.Find ("Panel").transform);
			    back.transform.localScale = new Vector2 (1, 1);
			    back.transform.localPosition = new Vector2 (0, 0);

			    //make board
			    string pathOfBoard = "Prefabs/Event/EventBoard";
			    GameObject board = Instantiate (Resources.Load (pathOfBoard)) as GameObject;
			    board.transform.SetParent (GameObject.Find ("Panel").transform);
			    board.transform.localScale = new Vector2 (1, 1);

			    back.GetComponent<CloseEventBoard> ().deleteObj = board;
			    back.GetComponent<CloseEventBoard> ().deleteObj2 = back;
			    board.transform.FindChild ("close").GetComponent<CloseEventBoard> ().deleteObj = board;
			    board.transform.FindChild ("close").GetComponent<CloseEventBoard> ().deleteObj2 = back;

			    string pathOfScroll = "Prefabs/Event/Metsubou";
			    GameObject scroll = Instantiate (Resources.Load (pathOfScroll)) as GameObject;
			    scroll.transform.SetParent (board.transform);
			    scroll.transform.localScale = new Vector2 (1, 1);

			    string pathOfSlot = "Prefabs/Event/MetsubouSlot";
			    GameObject contents = scroll.transform.FindChild ("MetsubouScrollView/MetsubouContent").gameObject;
			    char[] delimiterChars2 = { ':' };
			    foreach (string text in metsubouList) {
				    GameObject slot = Instantiate (Resources.Load (pathOfSlot)) as GameObject;
				    slot.transform.SetParent (contents.transform);
				    List<string> metsubouTextList = new List<string> ();
				    metsubouTextList = new List<string> (text.Split (delimiterChars2));
				    string srcDaimyoName = daimyo.getName (int.Parse (metsubouTextList [0]));
				    string dstDaimyoName = daimyo.getName (int.Parse (metsubouTextList [1]));
                    string metsubouText = "";
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        metsubouText = dstDaimyoName + " was was destroyed completly by " + srcDaimyoName + ".";
                    }else {
                        metsubouText = dstDaimyoName + "は" + srcDaimyoName + "に滅ぼされました";
                    }

                    slot.transform.FindChild ("MetsubouText").GetComponent<Text> ().text = metsubouText;
				    slot.transform.localScale = new Vector2 (1, 1);

				    //Metsubou Daimyo Handling
				    string srcMetsubouTemp = "metsubou" + metsubouTextList [0];
				    string srcMetsubou = PlayerPrefs.GetString (srcMetsubouTemp);
				    string dstMetsubouTemp = "metsubou" + metsubouTextList [1];
				    string dstMetsubou = PlayerPrefs.GetString (dstMetsubouTemp);

				    string newSrcMetsubou = "";
				    if (srcMetsubou != null && srcMetsubou != "") {
					    newSrcMetsubou = srcMetsubou + "," + metsubouTextList [1];
				    } else {
					    newSrcMetsubou = metsubouTextList [1];
				    }
				    if (dstMetsubou != null && dstMetsubou != "") {
					    newSrcMetsubou = newSrcMetsubou + "," + dstMetsubou;
				    }
				    PlayerPrefs.SetString (srcMetsubouTemp, newSrcMetsubou);

			    }

			    PlayerPrefs.DeleteKey ("metsubou");
			    PlayerPrefs.Flush ();
		    }


            /*--------------------*/
            /*Enemy Action Event*/
            /*--------------------*/       
            rdmEventTimer = PlayerPrefs.GetFloat ("rdmEventTimer", 0); 
		    if (rdmEventTimer <= 0) {
			    rdmEventTimer = UnityEngine.Random.Range (rdmEventMin, rdmEventMax);
		    }
        }

		//Quest
		questExtension ();

	}

	void OnApplicationPause (bool pauseStatus) {
		if (!pauseStatus) {
            if (!adRunFlg && !iapRunFlg) {
                //アプリを終了しないでホーム画面からアプリを起動して復帰した時                
                //destroy data store
                if(GameObject.Find("DataStore")) {
                    Destroy(GameObject.Find("DataStore").gameObject);
                }
                //back to top
				PlayerPrefs.Flush();//通常のFlushを行い、現在の状態をファイルへ反映する
                SceneManager.LoadScene ("top");
            }
		} else {
			//アプリがバックグラウンドに移行する時
			PlayerPrefs.Flush(true);//専用のFlushを行い、現在の状態を一時ファイルへ書き出す
		}
	}

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

    public void SeasonChange() {

        //Common
        eventStopFlg = true;

        GameObject map = GameObject.Find("Map").gameObject;
        string pathOfBack = "Prefabs/Common/TouchBackForOne";
        GameObject back = Instantiate(Resources.Load(pathOfBack)) as GameObject;
        back.transform.SetParent(map.transform);
        back.transform.localScale = new Vector2(1, 1);
        back.transform.localPosition = new Vector3(0, 0, 0);
        
        string path = "Prefabs/Cyosyu/seasonChangeBoard";
        GameObject seasonChangeBoard = Instantiate(Resources.Load(path)) as GameObject;
        seasonChangeBoard.transform.SetParent(map.transform);
        seasonChangeBoard.transform.localScale = new Vector2(1, 1);
        seasonChangeBoard.transform.localPosition = new Vector3(0, 0, 0);

        //Serifu
        string yearSeason = PlayerPrefs.GetString("yearSeason");
        if (yearSeason == null || yearSeason == "") {
            PlayerPrefs.SetString("yearSeason", "1560,1");
            yearSeason = "1560,1";
            PlayerPrefs.Flush();
        }
        char[] delimiterChars = { ',' };
        string[] yearSeasonList = yearSeason.Split(delimiterChars);
        int nowYear = int.Parse(yearSeasonList[0]);
        nowSeason = int.Parse(yearSeasonList[1]);
        
        //Seasonal Data Change
        if (nowSeason == 4) {
            nowYear = nowYear + 1;
            nowSeason = 1;
        }else {
            nowSeason = nowSeason + 1;
        }

        string seasonText = GetSeasonText(nowSeason);
        seasonChangeBoard.transform.FindChild("textBoard").transform.FindChild("Text").GetComponent<TextReader>().scenarios.Add(seasonText);
        back.GetComponent<CloseOneBoard>().deleteObj = seasonChangeBoard;
        
        string newYearSeason = nowYear.ToString() + "," + nowSeason.ToString();
        PlayerPrefs.DeleteKey("bakuhuTobatsuDaimyoId");
        PlayerPrefs.SetString("yearSeason", newYearSeason);

        string lastSeasonChangeTime = System.DateTime.Now.ToString();
        PlayerPrefs.SetString("lastSeasonChangeTime", lastSeasonChangeTime);

        PlayerPrefs.DeleteKey("usedBusyo");
        DoNextSeason DoNextSeason = new DoNextSeason();
        DoNextSeason.deleteLinkCut();
        DoNextSeason.deleteWinOver();
        PlayerPrefs.Flush();

        GameObject.Find("YearValue").GetComponent<Text>().text = nowYear.ToString();
        SetSeason(nowSeason);

        PlayerPrefs.SetBool("questDailyFlg38", true);
        PlayerPrefs.Flush();

        //Cyosyu Data Register
        CyosyuDataRegister(nowSeason, seasonChangeBoard);
        
        seasonChangedFlg = false;
    }

    public void CyosyuDataRegister(int seasonId, GameObject seasonChangeBoard) {

        Entity_naisei_mst naiseiMst = Resources.Load("Data/naisei_mst") as Entity_naisei_mst;
        NaiseiController naisei = new NaiseiController();
        int totalMoney = 0;
        int kozanMoney = 0;
        int totalHyourou = 0;
        int totalYRL = 0;
        int totalKBL = 0;
        int totalYML = 0;
        int totalTPL = 0;
        int totalYRM = 0;
        int totalKBM = 0;
        int totalYMM = 0;
        int totalTPM = 0;
        int totalYRH = 0;
        int totalKBH = 0;
        int totalYMH = 0;
        int totalTPH = 0;
        int totalSNBL = 0;
        int totalSNBM = 0;
        int totalSNBH = 0;

        string seiryoku = PlayerPrefs.GetString("seiryoku");
        List<string> seiryokuList = new List<string>();
        List<string> mySeiryokuList = new List<string>();
        char[] delimiterChars = { ',' };

        seiryokuList = new List<string>(seiryoku.Split(delimiterChars));
        int myDaimyoId = PlayerPrefs.GetInt("myDaimyo");

        //Get my Kuni
        for (int i = 0; i < seiryokuList.Count; i++) {
            int seiryokuId = int.Parse(seiryokuList[i]);
            if (seiryokuId == myDaimyoId) {
                int kuniId = i + 1;
                mySeiryokuList.Add(kuniId.ToString());
            }
        }
        
        //Item Calculation
        for (int i = 0; i < mySeiryokuList.Count; i++) {

            int kuniKozan = 0;
            int kuniSyogyo = 0;

            int kuniId = int.Parse(mySeiryokuList[i]);
            string temp = "kuni" + mySeiryokuList[i];
            string clearedKuni = PlayerPrefs.GetString(temp);
            //Shiro Qty
            if (clearedKuni != null && clearedKuni != "") {
                List<string> shiroList = new List<string>();
                shiroList = new List<string>(clearedKuni.Split(delimiterChars));
                string naiseiTemp = "naisei" + mySeiryokuList[i];
                string naiseiString = PlayerPrefs.GetString(naiseiTemp);

                if (PlayerPrefs.HasKey(naiseiTemp)) {
                    List<string> naiseiList = new List<string>();
                    naiseiList = new List<string>(naiseiString.Split(delimiterChars));
                    char[] delimiterChars2 = { ':' };
                    List<string> deletePanelList = new List<string>();

                    for (int j = 1; j < naiseiList.Count; j++) {
                        List<string> naiseiContentList = new List<string>();
                        naiseiContentList = new List<string>(naiseiList[j].Split(delimiterChars2));

                        if (naiseiContentList[0] != "0") {
                            string type = naiseiMst.param[int.Parse(naiseiContentList[0])].code;
                            List<int> naiseiEffectList = new List<int>();                            
                            naiseiEffectList = naisei.getNaiseiList(type, int.Parse(naiseiContentList[1]));

                            //Status
                            if (type == "shop") {
                                kuniSyogyo = kuniSyogyo + naiseiEffectList[0];
                            }else if (type == "kzn") {
                                kuniKozan = kuniKozan + naiseiEffectList[0];

                            }else if (type == "ta") {
                                totalHyourou = totalHyourou + naiseiEffectList[0];
                            }else if (type == "yr") {
                                if (int.Parse(naiseiContentList[1]) < 11) {
                                    //Low
                                    totalYRL = totalYRL + naiseiEffectList[0];
                                }else if (int.Parse(naiseiContentList[1]) < 16) {
                                    //Middle
                                    totalYRM = totalYRM + naiseiEffectList[0];
                                }else if (15 <= int.Parse(naiseiContentList[1])) {
                                    //High
                                    totalYRH = totalYRH + naiseiEffectList[0];
                                }
                            }else if (type == "kb") {
                                if (int.Parse(naiseiContentList[1]) < 11) {
                                    //Low
                                    totalKBL = totalKBL + naiseiEffectList[0];
                                }else if (int.Parse(naiseiContentList[1]) < 16) {
                                    //Middle
                                    totalKBM = totalKBM + naiseiEffectList[0];
                                }else if (15 <= int.Parse(naiseiContentList[1])) {
                                    //High
                                    totalKBH = totalKBH + naiseiEffectList[0];
                                }
                            }else if (type == "ym") {
                                if (int.Parse(naiseiContentList[1]) < 11) {
                                    //Low
                                    totalYML = totalYML + naiseiEffectList[0];
                                }else if (int.Parse(naiseiContentList[1]) < 16) {
                                    //Middle
                                    totalYMM = totalYMM + naiseiEffectList[0];
                                }else if (15 <= int.Parse(naiseiContentList[1])) {
                                    //High
                                    totalYMH = totalYMH + naiseiEffectList[0];                                
                                }
                            }else if (type == "tp") {
                                if (int.Parse(naiseiContentList[1]) < 11) {
                                    //Low
                                    totalTPL = totalTPL + naiseiEffectList[0];
                                }else if (int.Parse(naiseiContentList[1]) < 16) {
                                    //Middle
                                    totalTPM = totalTPM + naiseiEffectList[0];
                                }else if (15 <= int.Parse(naiseiContentList[1])) {
                                    //High
                                    totalTPH = totalTPH + naiseiEffectList[0];
                                }
                            }else if (type == "snb") {
                                if (int.Parse(naiseiContentList[1]) < 11) {
                                    //Low
                                    totalSNBL = totalSNBL + naiseiEffectList[0];
                                }else if (int.Parse(naiseiContentList[1]) < 16) {
                                    //Middle
                                    totalSNBM = totalSNBM + naiseiEffectList[0];
                                }else if (15 <= int.Parse(naiseiContentList[1])) {
                                    //High
                                    totalSNBH = totalSNBH + naiseiEffectList[0];
                                }
                            }
                        }
                    }

                    //Jyosyu Addition
                    string jyosyuTemp = "jyosyu" + kuniId;

                    if (PlayerPrefs.HasKey(jyosyuTemp)) {
                        int jyosyuId = PlayerPrefs.GetInt(jyosyuTemp);
                        StatusGet sts = new StatusGet();
                        int lv = PlayerPrefs.GetInt(jyosyuId.ToString());
                        float naiseiSts = (float)sts.getDfc(jyosyuId, lv);

                        float hpSts = (float)sts.getHp(jyosyuId, lv);
                        float atkSts = (float)sts.getAtk(jyosyuId, lv);

                        float tempKuniSyogyo = (float)kuniSyogyo;
                        tempKuniSyogyo = tempKuniSyogyo + (tempKuniSyogyo * naiseiSts / 200);
                        kuniSyogyo = (int)tempKuniSyogyo;

                        float tempKuniKozan = (float)kuniKozan;
                        tempKuniKozan = tempKuniKozan + (tempKuniKozan * naiseiSts / 200);
                        kuniKozan = (int)tempKuniKozan;


                    }
                }
            }
            kozanMoney = kozanMoney + kuniKozan;
            totalMoney = totalMoney + kuniSyogyo;
        }

        //Visualize & Data Register
        GameObject Syukaku = seasonChangeBoard.transform.FindChild("Syukaku").gameObject;
        if (nowSeason==1) {
            //spring
            Syukaku.transform.FindChild("TargetMoney").transform.FindChild("TargetMoneyValue").GetComponent<Text>().text = (totalMoney + kozanMoney).ToString();

            //data
            int nowMoney = PlayerPrefs.GetInt("money");
            int resultMoney = nowMoney + totalMoney + kozanMoney;
            if (resultMoney < 0) {
                resultMoney = int.MaxValue;
            }
            PlayerPrefs.SetInt("money", resultMoney);
            GameObject.Find("MoneyValue").GetComponent<Text>().text = resultMoney.ToString();

            int TrackGetMoneyNo = PlayerPrefs.GetInt("TrackGetMoneyNo", 0);
            TrackGetMoneyNo = TrackGetMoneyNo + totalMoney + kozanMoney;
            PlayerPrefs.SetInt("TrackGetMoneyNo", TrackGetMoneyNo);

        } else if(nowSeason==2 || nowSeason == 4) {
            //summer or winter
            Syukaku.transform.FindChild("TargetMoney").transform.FindChild("TargetMoneyValue").GetComponent<Text>().text = kozanMoney.ToString();

            GameObject TargetGunjyu = Syukaku.transform.FindChild("TargetGunjyu").gameObject;
            if(totalYRH != 0) {
                TargetGunjyu.transform.FindChild("YR").transform.FindChild("CyouheiYRValueH").GetComponent<Text>().text = totalYRH.ToString();
            }
            if (totalYRM != 0) {
                TargetGunjyu.transform.FindChild("YR").transform.FindChild("CyouheiYRValueM").GetComponent<Text>().text = totalYRM.ToString();
            }
            if (totalYRL != 0) {
                TargetGunjyu.transform.FindChild("YR").transform.FindChild("CyouheiYRValueL").GetComponent<Text>().text = totalYRL.ToString();
            }

            if (totalKBH != 0) {
                TargetGunjyu.transform.FindChild("KB").transform.FindChild("CyouheiKBValueH").GetComponent<Text>().text = totalKBH.ToString();
            }
            if (totalKBM != 0) {
                TargetGunjyu.transform.FindChild("KB").transform.FindChild("CyouheiKBValueM").GetComponent<Text>().text = totalKBM.ToString();
            }
            if (totalKBL != 0) {
                TargetGunjyu.transform.FindChild("KB").transform.FindChild("CyouheiKBValueL").GetComponent<Text>().text = totalKBL.ToString();
            }

            if(totalYMH != 0) {
                TargetGunjyu.transform.FindChild("YM").transform.FindChild("CyouheiYMValueH").GetComponent<Text>().text = totalYMH.ToString();
            }
            if (totalYMM != 0) {
                TargetGunjyu.transform.FindChild("YM").transform.FindChild("CyouheiYMValueM").GetComponent<Text>().text = totalYMM.ToString();
            }
            if (totalYML != 0) {
                TargetGunjyu.transform.FindChild("YM").transform.FindChild("CyouheiYMValueL").GetComponent<Text>().text = totalYML.ToString();
            }

            if (totalTPH != 0) {
                TargetGunjyu.transform.FindChild("TP").transform.FindChild("CyouheiTPValueH").GetComponent<Text>().text = totalTPH.ToString();
            }
            if (totalTPM != 0) {
                TargetGunjyu.transform.FindChild("TP").transform.FindChild("CyouheiTPValueM").GetComponent<Text>().text = totalTPM.ToString();
            }
            if (totalTPL != 0) {
                TargetGunjyu.transform.FindChild("TP").transform.FindChild("CyouheiTPValueL").GetComponent<Text>().text = totalTPL.ToString();
            }

            if (totalSNBH != 0) {
                TargetGunjyu.transform.FindChild("SNB").transform.FindChild("SNBValueH").GetComponent<Text>().text = totalSNBH.ToString();
            }
            if (totalSNBM != 0) {
                TargetGunjyu.transform.FindChild("SNB").transform.FindChild("SNBValueM").GetComponent<Text>().text = totalSNBM.ToString();
            }
            if (totalSNBL != 0) {
                TargetGunjyu.transform.FindChild("SNB").transform.FindChild("SNBValueL").GetComponent<Text>().text = totalSNBL.ToString();
            }


            //data
            //YR
            if (totalYRL != 0 || totalYRM != 0 || totalYRH != 0) {
                string cyoheiYRString = PlayerPrefs.GetString("cyouheiYR");
                List<string> cyoheiYRList = new List<string>();
                cyoheiYRList = new List<string>(cyoheiYRString.Split(delimiterChars));

                int newYRL = totalYRL;
                int newYRM = totalYRM;
                int newYRH = totalYRH;

                newYRL = newYRL + int.Parse(cyoheiYRList[0]);
                newYRM = newYRM + int.Parse(cyoheiYRList[1]);
                newYRH = newYRH + int.Parse(cyoheiYRList[2]);

                string newCyoheiYR = newYRL + "," + newYRM + "," + newYRH;
                PlayerPrefs.SetString("cyouheiYR", newCyoheiYR);
            }
            //KB
            if (totalKBL != 0 || totalKBM != 0 || totalKBH != 0) {
                string cyoheiKBString = PlayerPrefs.GetString("cyouheiKB");
                List<string> cyoheiKBList = new List<string>();
                cyoheiKBList = new List<string>(cyoheiKBString.Split(delimiterChars));
                int newKBL = totalKBL;
                int newKBM = totalKBM;
                int newKBH = totalKBH;

                newKBL = newKBL + int.Parse(cyoheiKBList[0]);
                newKBM = newKBM + int.Parse(cyoheiKBList[1]);
                newKBH = newKBH + int.Parse(cyoheiKBList[2]);

                string newCyoheiKB = newKBL + "," + newKBM + "," + newKBH;
                PlayerPrefs.SetString("cyouheiKB", newCyoheiKB);
            }
            //YM
            if (totalYML != 0 || totalYMM != 0 || totalYMH != 0) {
                string cyoheiYMString = PlayerPrefs.GetString("cyouheiYM");
                List<string> cyoheiYMList = new List<string>();
                cyoheiYMList = new List<string>(cyoheiYMString.Split(delimiterChars));
                int newYML = totalYML;
                int newYMM = totalYMM;
                int newYMH = totalYMH;

                newYML = newYML + int.Parse(cyoheiYMList[0]);
                newYMM = newYMM + int.Parse(cyoheiYMList[1]);
                newYMH = newYMH + int.Parse(cyoheiYMList[2]);

                string newCyoheiYM = newYML + "," + newYMM + "," + newYMH;
                PlayerPrefs.SetString("cyouheiYM", newCyoheiYM);
            }
            //TP
            if (totalTPL != 0 || totalTPM != 0 || totalTPH != 0) {
                string cyoheiTPString = PlayerPrefs.GetString("cyouheiTP");
                List<string> cyoheiTPList = new List<string>();
                cyoheiTPList = new List<string>(cyoheiTPString.Split(delimiterChars));
                int newTPL = totalTPL;
                int newTPM = totalTPM;
                int newTPH = totalTPH;

                newTPL = newTPL + int.Parse(cyoheiTPList[0]);
                newTPM = newTPM + int.Parse(cyoheiTPList[1]);
                newTPH = newTPH + int.Parse(cyoheiTPList[2]);

                string newCyoheiTP = newTPL + "," + newTPM + "," + newTPH;
                PlayerPrefs.SetString("cyouheiTP", newCyoheiTP);
            }
            //SNB
            if (totalSNBL != 0 || totalSNBM != 0 || totalSNBH != 0) {
                if (totalSNBL != 0) {
                    int SNBQty = PlayerPrefs.GetInt("shinobiGe");
                    int newQty = SNBQty + totalSNBL;

                    PlayerPrefs.SetInt("shinobiGe", newQty);
                }
                if (totalSNBM != 0) {
                    int SNBQty = PlayerPrefs.GetInt("shinobiCyu");
                    int newQty = SNBQty + totalSNBM;

                    PlayerPrefs.SetInt("shinobiCyu", newQty);
                }
                if (totalSNBH != 0) {
                    int SNBQty = PlayerPrefs.GetInt("shinobiJyo");
                    int newQty = SNBQty + totalSNBH;

                    PlayerPrefs.SetInt("shinobiJyo", newQty);
                }

            }
            int TrackGetSozaiNo = PlayerPrefs.GetInt("TrackGetSozaiNo", 0);
            TrackGetSozaiNo = TrackGetSozaiNo + totalYRL + totalKBL + totalYML + totalTPL + totalYRM + totalKBM + totalYMM + totalTPM + totalYRH + totalKBH + totalYMH + totalTPH + totalSNBL + totalSNBM + totalSNBH;
            PlayerPrefs.SetInt("TrackGetSozaiNo", TrackGetSozaiNo);            

            if (kozanMoney != 0) {
                int nowMoney = PlayerPrefs.GetInt("money");
                int resultMoney = nowMoney + kozanMoney;
                if (resultMoney < 0) {
                    resultMoney = int.MaxValue;
                }
                PlayerPrefs.SetInt("money", resultMoney);
                GameObject.Find("MoneyValue").GetComponent<Text>().text = resultMoney.ToString();

                int TrackGetMoneyNo = PlayerPrefs.GetInt("TrackGetMoneyNo", 0);
                TrackGetMoneyNo = TrackGetMoneyNo + kozanMoney;
                PlayerPrefs.SetInt("TrackGetMoneyNo", TrackGetMoneyNo);
            }


        }else if (nowSeason == 3) {
            //autumn
            Syukaku.transform.FindChild("TargetMoney").transform.FindChild("TargetMoneyValue").GetComponent<Text>().text = kozanMoney.ToString();
            Syukaku.transform.FindChild("TargetHyourou").transform.FindChild("TargetHyourouValue").GetComponent<Text>().text = totalHyourou.ToString();

            //data
            int nowHyourou = PlayerPrefs.GetInt("hyourou");
            int maxHyourou = PlayerPrefs.GetInt("hyourouMax");
            int resultHyourou = nowHyourou + totalHyourou;
            if (resultHyourou > maxHyourou) resultHyourou = maxHyourou;
            PlayerPrefs.SetInt("hyourou", resultHyourou);
            HyourouCurrentValue.text = resultHyourou.ToString();
                     
            int TrackGetHyourouNo = PlayerPrefs.GetInt("TrackGetHyourouNo", 0);
            TrackGetHyourouNo = TrackGetHyourouNo + totalHyourou;
            PlayerPrefs.SetInt("TrackGetHyourouNo", TrackGetHyourouNo);

            if (kozanMoney != 0) {
                int nowMoney = PlayerPrefs.GetInt("money");
                int resultMoney = nowMoney + kozanMoney;
                if (resultMoney < 0) {
                    resultMoney = int.MaxValue;
                }
                PlayerPrefs.SetInt("money", resultMoney);
                GameObject.Find("MoneyValue").GetComponent<Text>().text = resultMoney.ToString();

                int TrackGetMoneyNo = PlayerPrefs.GetInt("TrackGetMoneyNo", 0);
                TrackGetMoneyNo = TrackGetMoneyNo + kozanMoney;
                PlayerPrefs.SetInt("TrackGetMoneyNo", TrackGetMoneyNo);
            }
        }
        PlayerPrefs.Flush();


    }





    public string GetSeasonText(int seasonId) {

        string seasonText = "";

        if (Application.systemLanguage != SystemLanguage.Japanese) {
            if (seasonId == 1) {
                seasonText = "Spring has come. You have levied money in this season. My lord, please develop your town, store or mine to increase income.";
            }else if (seasonId == 2) {
                seasonText = "Summer has come. You have levied weapon items in this season. My lord, please develop your town, Blacksmith of each item to increase income.";
            }else if (seasonId == 3) {
                seasonText = "Autumn has come. You have levied stamina in this season. My lord, please develop your town, farm to increase this income.";
            }else if (seasonId == 4) {
                seasonText = "Winter has come. You have levied weapon items in this season. My lord, please develop your town, Blacksmith of each item to increase income.";
            }
        }else {
            if (seasonId == 1) {
                seasonText = "春になりました。商人町と鉱山から金を徴収しました。金が無くては何も出来ませぬ、内政開発に励みましょうぞ。";
            }else if (seasonId == 2) {
                seasonText = "夏になりました。鍛冶屋などの職人町から素材を徴収しました。部隊を増やしたり忍を派遣できますゆえ、内政開発に励みましょうぞ。";
            }else if (seasonId == 3) {
                seasonText = "秋になりました。村落から兵糧を徴収しました。腹が減っては戦は出来ぬ、内政開発に励みましょうぞ。";
            }else if (seasonId == 4) {
                seasonText = "おお寒い、冬になりました。鍛冶屋などの職人町から素材を徴収しました。部隊を増やしたり忍を派遣できますゆえ、内政開発に励みましょうぞ。";
            }
        }

        return seasonText;
    }

    public void UpdateRengouKuniIcon(bool rengouFlg, string rengouDaimyo) {
        if(rengouFlg) {
            if(rengouDaimyo != "" && rengouDaimyo != null) {
                char[] delimiterChars = { ',' };
                List<string> rengouDaimyoList = new List<string>();
                rengouDaimyoList = new List<string>(rengouDaimyo.Split(delimiterChars));

                GameObject KuniIconView = GameObject.Find("KuniIconView").gameObject;
                Color rengouColor = new Color(255f / 255f, 80f / 255f, 0f / 255f, 150f / 255f); //Red
                foreach (Transform kuni in KuniIconView.transform) {
                    int daimyoId = kuni.GetComponent<SendParam>().daimyoId;
                    if(rengouDaimyoList.Contains(daimyoId.ToString())) {
                        //Change Color
                        Outline Outline = kuni.GetComponent<Outline>();
                        Outline.effectColor = rengouColor;
                    }
                }
            }
        }
    }
}
