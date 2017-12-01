using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class Timer : MonoBehaviour {

	public float startTime = 180.0f; // seconds
	public float engunTime = 170.0f;
	public float timer;
	public bool paused = false;
	bool flag = false;
	public bool engunTimerflg = false;

	//Engun
	public bool playerEngunFlg = false;
	public bool enemyEngunFlg = false;
	public string playerEngunList = "";
	public string enemyEngunList = "";

    //Kokunin
    public bool kokuninFlg = false;
    public float kokuninTime = 170.0f;
    public string enemyKokuninList = "";

    //Cyouryaku
    public bool cyouryakuFlg;
	public int cyouryakuHeiQty = 0;
	public float cyouryakuTime = 0; //165-175
	public string cyouryakuTmp = "";
	public int activeKuniId;

	//Weaher & Map
	public float mntMinusRatio = 0;
	public float seaMinusRatio = 0;
	public float rainMinusRatio = 0;
	public float snowMinusRatio = 0;

	public bool isAttackedFlg = false;
	public bool isKessenFlg = false;

	//SE
	public AudioSource[] audioSources;

    //Enemy Saku
    public List<EnemySaku> EnemySakuList;
    public List<float> EnemySakuTimerList;
    public float sakuTimer;
    public int sakuTimerId = 0;

    public Text timerTxt;
    public bool PvPFlg;

    private void Start(){
		reset();

        mntMinusRatio = PlayerPrefs.GetFloat("mntMinusStatus",0);
		seaMinusRatio = PlayerPrefs.GetFloat("seaMinusStatus",0);
		rainMinusRatio = PlayerPrefs.GetFloat("rainMinusStatus",0);
		snowMinusRatio = PlayerPrefs.GetFloat("snowMinusStatus",0);

        //Player Engun Check
        if (Application.loadedLevelName != "tutorialKassen") {
            playerEngunList = PlayerPrefs.GetString("playerEngunList");
        }else {
            playerEngunList = "1-158-50-20-10:1-141-50-20-10:1-52-50-20-10";
        }

		if (playerEngunList == null || playerEngunList == "") {
			playerEngunList = PlayerPrefs.GetString("tempKyoutouList");
		}
		if (playerEngunList != null && playerEngunList != "") {
            if (Application.loadedLevelName != "tutorialKassen") {
                engunTime = UnityEngine.Random.Range(150f, 175f);
            }else {
                engunTime = 170f;
            }
            playerEngunFlg = true;
		}
        
		//Enemy Engun Check
		enemyEngunList = PlayerPrefs.GetString("enemyEngunList");
		if (enemyEngunList != null && enemyEngunList != "") {
            engunTime = UnityEngine.Random.Range(150f, 175f);
            enemyEngunFlg = true;
		}


		//Cyouryaku Check
		activeKuniId  = PlayerPrefs.GetInt("activeKuniId");
		int activeStageId = PlayerPrefs.GetInt("activeStageId");
		cyouryakuTmp = activeKuniId.ToString() + "cyouryaku" +activeStageId.ToString();
		if (PlayerPrefs.HasKey (cyouryakuTmp)) {
			cyouryakuHeiQty = PlayerPrefs.GetInt (cyouryakuTmp);
			cyouryakuTime = UnityEngine.Random.Range (165f, 175f);
		} else {
			cyouryakuFlg = true;
		}
        isAttackedFlg = PlayerPrefs.GetBool("isAttackedFlg");

        //ikki Check
        if (isAttackedFlg) {
            string kokuninTmp = "kokunin" + activeKuniId;
            enemyKokuninList = PlayerPrefs.GetString(kokuninTmp);
            PlayerPrefs.DeleteKey(kokuninTmp);
            if (enemyKokuninList != null && enemyKokuninList !=  "") {
                kokuninFlg = true;
                kokuninTime = UnityEngine.Random.Range(160f, 175f);
            }
        }
        
        //Enemy Saku
        for (int i = 0; i < 12; i++) {
            EnemySakuTimerList.Add(UnityEngine.Random.Range(140, 175));
        }
        EnemySakuTimerList.Sort((x, y) => y.CompareTo(x));
        sakuTimer = EnemySakuTimerList[sakuTimerId];

        timerTxt = gameObject.transform.Find("timerText").GetComponent<Text>();

    }
	
	private void reset(){
		timer = startTime;
	}
	
	private void Update(){
		
        if(!paused) {
            timer -= Time.deltaTime;

		    if (timer > 0.0f) {
                //On Play
                if(!PvPFlg) {
                    timerTxt.text = ((int)timer).ToString();

			        //Engun Time
			        if(!engunTimerflg){
				        if(timer < engunTime){
					        engunTimerflg = true;

					        if(playerEngunFlg){
						        playerEngunInstance(playerEngunList,mntMinusRatio,seaMinusRatio,rainMinusRatio,snowMinusRatio);
					        }
					        if(enemyEngunFlg){
						        enemyEngunInstance(enemyEngunList,mntMinusRatio,seaMinusRatio,rainMinusRatio,snowMinusRatio, false);
					        }
				        }
			        }
                    //kokunin
                    if(kokuninFlg) {
                        if (timer < kokuninTime) {
                            Debug.Log(enemyKokuninList);
                            if(enemyKokuninList != "") {
                                enemyEngunInstance(enemyKokuninList, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, true);
                            }
                            kokuninFlg = false;
                        }
                    }
                    
			        //cyouryaku
			        if (!cyouryakuFlg) {
				        if (timer < cyouryakuTime) {
					        cyouryakuFlg = true;
					        cyouryaku(cyouryakuHeiQty,cyouryakuTmp);
				        }
			        }
                }
                //Enemy Saku
                if(EnemySakuList.Count > sakuTimerId) {
                    if (timer< sakuTimer) {
                        //run
                        EnemySaku EnemySaku = EnemySakuList[sakuTimerId];
                        RunEnemySaku(EnemySaku);
                        sakuTimerId++;
                        if(sakuTimerId < 12) sakuTimer = EnemySakuTimerList[sakuTimerId];
                    }
                }


            } else {
                if (!PvPFlg) {
                    if (!flag) {
                        Time.timeScale = 1;
                        audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
				        audioSources [5].Play ();

				        GameObject canvas = GameObject.Find ("Canvas").gameObject;

				        //Player battle stop
				        canvas.transform.Find ("playerHp").GetComponent<HPCounter> ().flag = true;
				        canvas.transform.Find ("enemyHp").GetComponent<HPCounter> ().flag = true;

				        //Enable Button
                        if(GameObject.Find("ScrollView")) {
				            GameObject.Find ("ScrollView").SetActive (false);
                        }
                        if (GameObject.Find ("GiveupBtn")) {
					        GameObject.Find ("GiveupBtn").SetActive (false);
				        }
                        if (GameObject.Find("AutoBtn")) {
                            GameObject.Find("AutoBtn").SetActive(false);
                        }
                        if (!isAttackedFlg) {
                            //Player Attacked
                            //Game Over
                            string backPath = "Prefabs/PostKassen/back";
					        GameObject backObj = Instantiate (Resources.Load (backPath)) as GameObject;
					        backObj.transform.SetParent (canvas.transform);
					        backObj.transform.localScale = new Vector2 (70, 63);
					        backObj.transform.localPosition = new Vector2 (0,0);

					        //Chane word
					        Color color = Color.blue;
                            int langId = PlayerPrefs.GetInt("langId");
                            if (langId == 2) {                               
                                GameObject.Find ("winlose").GetComponent<TextMesh> ().text = "Timeup";
                            }else {
                                GameObject.Find("winlose").GetComponent<TextMesh>().text = "時間切れ";
                            }
					        GameObject.Find ("winlose").GetComponent<TextMesh> ().color = color;
					        audioSources [4].Play ();
					        busouKaijyo ();

					        string blackPath = "Prefabs/PostKassen/black";
					        GameObject blackObj = Instantiate (Resources.Load (blackPath)) as GameObject;
					        blackObj.transform.SetParent (canvas.transform);
					        blackObj.transform.localScale = new Vector2 (330, 300);
					        blackObj.transform.localPosition = new Vector2 (0,0);

					        string makimonoPath = "Prefabs/PostKassen/makimono";
					        GameObject makimonoObj = Instantiate (Resources.Load (makimonoPath)) as GameObject;
					        makimonoObj.transform.SetParent (canvas.transform);
					        makimonoObj.transform.localScale = new Vector2 (1, 1);
					        makimonoObj.transform.localPosition = new Vector2 (0, -135);
					
					        //Button List
					        string nextbtnPath = "Prefabs/PostKassen/bttnList";
					        GameObject bttnListObj = Instantiate (Resources.Load (nextbtnPath)) as GameObject;
					        bttnListObj.transform.SetParent (canvas.transform);
					        bttnListObj.transform.localScale = new Vector2 (1, 1);		
					        bttnListObj.transform.localPosition = new Vector2 (0,0);

					        bool isKessenFlg = PlayerPrefs.GetBool("isKessenFlg");
					        if(isKessenFlg){
						        HPCounter kessen = new HPCounter ();
						        kessen.kessenResult(false);
					        }
                            
				        } else {
                            //Enemy Attacked
                            //history
                            if (Application.loadedLevelName != "tutorialKassen") {
                                string tKey = PlayerPrefs.GetString("activeKey");
					            MainStageController main = new MainStageController();
					            main.deleteKeyHistory(tKey);
					            PlayerPrefs.DeleteKey("isAttacked");
					            PlayerPrefs.Flush();
                        
					            bool twiceHeiFlg = PlayerPrefs.GetBool ("twiceHeiFlg");
					            if (twiceHeiFlg) {
						            PlayerPrefs.SetBool ("questDailyFlg15",true);
						            PlayerPrefs.DeleteKey ("twiceHeiFlg");
						            PlayerPrefs.Flush();
					            }
                        
                                //View
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

                                int langId = PlayerPrefs.GetInt("langId");
                                if (langId == 2) {
                                    GameObject.Find ("winlose").GetComponent<TextMesh> ().text = "Timeup";
                                }else {
                                    GameObject.Find("winlose").GetComponent<TextMesh>().text = "時勝切れ";
                                }
					            string stageName = PlayerPrefs.GetString("activeStageName");
					            audioSources [3].Play ();
					            audioSources [7].Play ();
					            busouKaijyo ();

					            //Item List
					            string itemListPath = "Prefabs/PostKassen/itemList";
					            GameObject itemListObj = Instantiate(Resources.Load (itemListPath)) as GameObject;
					            itemListObj.transform.SetParent (canvas.transform);
					            itemListObj.transform.localScale = new Vector2(1,1);
					            itemListObj.transform.localPosition = new Vector2 (0,-136);

					            //Money
					            int activeStageMoney = PlayerPrefs.GetInt("activeStageMoney",0);
					            GameObject.Find ("moneyAmt").GetComponent<Text>().text = activeStageMoney.ToString();
					            int currentMoney = PlayerPrefs.GetInt("money");
					            currentMoney = currentMoney + activeStageMoney;
                                if (currentMoney < 0) {
                                    currentMoney = int.MaxValue;
                                }
                                PlayerPrefs.SetInt("money",currentMoney);

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
                                }else{
						            Debug.Log ("No level up");
					            }
                                
					            //Button List
					            string nextbtnPath = "Prefabs/PostKassen/bttnList";
					            GameObject bttnListObj = Instantiate (Resources.Load (nextbtnPath)) as GameObject;
					            bttnListObj.transform.SetParent (canvas.transform);
					            bttnListObj.transform.localScale = new Vector2 (1, 1);		
					            bttnListObj.transform.localPosition = new Vector2 (0,0);

                                if (langId == 2) {
                                    itemListObj.transform.Find ("stageName").transform.Find("stageNameValue").GetComponent<Text> ().text = stageName + " Succeed";
                                }else {
                                    itemListObj.transform.Find("stageName").transform.Find("stageNameValue").GetComponent<Text>().text = stageName + "成功";
                                }

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
						            PlayerPrefs.SetInt(tempExp, newExp);

						            //Busyo Lv
						            int nowLv = PlayerPrefs.GetInt(busyoId.ToString());
                                    string addLvTmp = "addlv" + busyoId.ToString();
                                    int maxLv = 100 + PlayerPrefs.GetInt(addLvTmp);
                                    if (maxLv > 200) {
                                        maxLv = 200;
                                    }
                                    int newLv = exp.getLvbyTotalExp(nowLv,newExp, maxLv);
						            PlayerPrefs.SetInt(busyoId.ToString(), newLv);

						            PlayerPrefs.Flush();
					            }
                            }else {
                                busouKaijyo();

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
                            
                                //Win
                                audioSources[3].Play();
                                audioSources[7].Play();

                                string particlePath = "Prefabs/PostKassen/particle";
                                GameObject particleObj = Instantiate(Resources.Load(particlePath)) as GameObject;
                                particleObj.transform.SetParent(canvas.transform);
                                particleObj.transform.localPosition = new Vector2(0, 60);
                            }				
				        }

				        //Time Stop
				        GameObject.Find ("timer").GetComponent<Timer> ().enabled = false;
				        flag = true;
                        PlayerPrefs.Flush();
                    }
			    }
            }
        }
	}

	public void playerEngunInstance(string playerEngunList,float mntMinusRatio,float seaMinusRatio,float rainMinusRatio,float snowMinusRatio){

		List<string> daimyoEnguniList = new List<string> ();
		char[] delimiterChars = {':'};
		char[] delimiterChars2 = {'-'};
		if(playerEngunList.Contains(":")){
			daimyoEnguniList = new List<string> (playerEngunList.Split (delimiterChars));
		}else{
			daimyoEnguniList.Add(playerEngunList);
		}

		for(int i=0; i<daimyoEnguniList.Count; i++){
			StatusGet sts = new StatusGet ();
			string daimyoEngunString = daimyoEnguniList[i];
			List<string> unitEnguniList = new List<string> ();
			unitEnguniList = new List<string> (daimyoEngunString.Split (delimiterChars2));
			int busyoId = int.Parse(unitEnguniList[1]);
			string heisyu = sts.getHeisyu (busyoId);

			if(busyoId!=0){
				int busyoLv = int.Parse(unitEnguniList[2]);
				int butaiQty = int.Parse(unitEnguniList[3]);
				int butaiLv = int.Parse(unitEnguniList[4]);


				int hp = sts.getHp (busyoId, busyoLv);
				int atk = sts.getAtk (busyoId, busyoLv);
				int dfc = sts.getDfc (busyoId, busyoLv);
				int spd = sts.getSpd (busyoId, busyoLv);
				string busyoName = sts.getBusyoName (busyoId);
				ArrayList senpouArray = sts.getSenpou (busyoId, true);

				if (mntMinusRatio != 0) {
					if (heisyu == "KB") {
						float tmp = (float)spd * mntMinusRatio;
						if (tmp < 1) {
							tmp = 1;
						}
						spd = Mathf.FloorToInt (tmp);
					}
				}else if (seaMinusRatio != 0) {
					if (heisyu == "TP") {
						float tmp = (float)dfc * seaMinusRatio;
						if (tmp < 1) {
							tmp = 1;
						}
						dfc = Mathf.FloorToInt (tmp);
					}else if (heisyu == "YM") {
						float tmp = (float)dfc * seaMinusRatio;
						if (tmp < 1) {
							tmp = 1;
						}
						dfc = Mathf.FloorToInt (tmp);
					}
				}
				if (rainMinusRatio != 0) {
					if (heisyu == "TP") {
						float tmp = (float)atk * rainMinusRatio;
						if (tmp < 1) {
							tmp = 1;
						}
						atk = Mathf.FloorToInt (tmp);
					}else if (heisyu == "YM") {
						float tmp = (float)atk * rainMinusRatio;
						if (tmp < 1) {
							tmp = 1;
						}
						atk = Mathf.FloorToInt (tmp);
					}		
				}else if(snowMinusRatio != 0) {
					float tmp = (float)spd * 0.5f;
					if (tmp < 1) {
						tmp = 1;
					}
					spd = Mathf.FloorToInt (tmp);

					if (heisyu == "TP") {
						float tmp2 = (float)atk * snowMinusRatio;
						if (tmp2 < 1) {
							tmp2 = 1;
						}
						atk = Mathf.FloorToInt (tmp2);
					}else if (heisyu == "YM") {
						float tmp2 = (float)atk * snowMinusRatio;
						if (tmp2 < 1) {
							tmp2 = 1;
						}
						atk = Mathf.FloorToInt (tmp2);
					}else if (heisyu == "KB") {
						float tmp2 = (float)dfc * snowMinusRatio;
						if (tmp2 < 1) {
							tmp2 = 1;
						}
						dfc = Mathf.FloorToInt (tmp2);
					}
				}

				//View Object & pass status to it. 
				PlayerInstance inst = new PlayerInstance ();
                if (Application.loadedLevelName != "kaisen") {
                    inst.makeEngunInstance(busyoId, hp, atk, dfc, spd, senpouArray, busyoName, butaiQty, butaiLv);
                } else {
                    BusyoInfoGet busyoScript = new BusyoInfoGet();
                    int shipId = busyoScript.getShipId(busyoId);
                    inst.makeKaisenInstance(busyoId, shipId, 25, hp, atk, dfc, spd, senpouArray, busyoName, butaiQty, butaiLv, true, butaiQty, butaiLv);
                }
			}
		}

        //auto check
        if(GameObject.Find("AutoBtn")) {
            if(GameObject.Find("AutoBtn").GetComponent<AutoAttack>().onFlg) {
                AutoAttack autoScript = new AutoAttack();
                autoScript.changeAutoScript();
            }
        }

		Message msg = new Message ();
		msg.makeKassenMessage (msg.getMessage(130));


	}


	public void enemyEngunInstance(string enemyEngunList,float mntMinusRatio,float seaMinusRatio,float rainMinusRatio,float snowMinusRatio, bool kokuninFlg){
		
		List<string> daimyoEnguniList = new List<string> ();
		char[] delimiterChars = {':'};
		char[] delimiterChars2 = {'-'};
		if(enemyEngunList.Contains(":")){
			daimyoEnguniList = new List<string> (enemyEngunList.Split (delimiterChars));
		}else{
			daimyoEnguniList.Add(enemyEngunList);
		}

        StatusGet sts = new StatusGet();
        for (int i=0; i<daimyoEnguniList.Count; i++){
			string daimyoEngunString = daimyoEnguniList[i];
			List<string> unitEnguniList = new List<string> ();
			unitEnguniList = new List<string> (daimyoEngunString.Split (delimiterChars2));
			int busyoId = int.Parse(unitEnguniList[1]);
			string heisyu = sts.getHeisyu (busyoId);

			if(busyoId!=0){
				int busyoLv = int.Parse(unitEnguniList[2]);
				int butaiQty = int.Parse(unitEnguniList[3]);
				int butaiLv = int.Parse(unitEnguniList[4]);

				int hp = sts.getHp (busyoId, busyoLv);
				int atk = sts.getAtk (busyoId, busyoLv);
				int dfc = sts.getDfc (busyoId, busyoLv);
				int spd = sts.getSpd (busyoId, busyoLv);
				string busyoName = sts.getBusyoName (busyoId);

                int aveSenpouLv = 0;
                if (Application.loadedLevelName != "kaisen") {
                    aveSenpouLv =  GameObject.Find ("GameScene").GetComponent<GameScene> ().aveSenpouLv;
                }else {
                    aveSenpouLv = GameObject.Find("GameScene").GetComponent<KaisenScene>().aveSenpouLv;
                }
				ArrayList senpouArray = sts.getEnemySenpou(busyoId, aveSenpouLv, "");


				if (mntMinusRatio != 0) {
					if (heisyu == "KB") {
						float tmp = (float)spd * mntMinusRatio;
						if (tmp < 1) {
							tmp = 1;
						}
						spd = Mathf.FloorToInt (tmp);
					}
				}else if (seaMinusRatio != 0) {
					if (heisyu == "TP") {
						float tmp = (float)dfc * seaMinusRatio;
						if (tmp < 1) {
							tmp = 1;
						}
						dfc = Mathf.FloorToInt (tmp);
					}else if (heisyu == "YM") {
						float tmp = (float)dfc * seaMinusRatio;
						if (tmp < 1) {
							tmp = 1;
						}
						dfc = Mathf.FloorToInt (tmp);
					}
				}
				if (rainMinusRatio != 0) {
					if (heisyu == "TP") {
						float tmp = (float)atk * rainMinusRatio;
						if (tmp < 1) {
							tmp = 1;
						}
						atk = Mathf.FloorToInt (tmp);
					}else if (heisyu == "YM") {
						float tmp = (float)atk * rainMinusRatio;
						if (tmp < 1) {
							tmp = 1;
						}
						atk = Mathf.FloorToInt (tmp);
					}		
				}else if(snowMinusRatio != 0) {
					float tmp = (float)spd * 0.5f;
					if (tmp < 1) {
						tmp = 1;
					}
					spd = Mathf.FloorToInt (tmp);

					if (heisyu == "TP") {
						float tmp2 = (float)atk * snowMinusRatio;
						if (tmp2 < 1) {
							tmp2 = 1;
						}
						atk = Mathf.FloorToInt (tmp2);
					}else if (heisyu == "YM") {
						float tmp2 = (float)atk * snowMinusRatio;
						if (tmp2 < 1) {
							tmp2 = 1;
						}
						atk = Mathf.FloorToInt (tmp2);
					}else if (heisyu == "KB") {
						float tmp2 = (float)dfc * snowMinusRatio;
						if (tmp2 < 1) {
							tmp2 = 1;
						}
						dfc = Mathf.FloorToInt (tmp2);
					}
				}

				//View Object & pass status to it. 
				EnemyInstance inst = new EnemyInstance ();
				BusyoInfoGet info = new BusyoInfoGet ();
				string ch_type = info.getHeisyu (busyoId);
				int mapId = 22;
                int AIType = 1;
                if (getRandomBool()) AIType = 4;


                if (Application.loadedLevelName != "kaisen") {
                    inst.makeInstance(mapId, busyoId, butaiLv, ch_type, butaiQty, hp, atk, dfc, spd, busyoName, 0, false, senpouArray,"",0,0, AIType);
                }else {
                    BusyoInfoGet busyoScript = new BusyoInfoGet();
                    int shipId = busyoScript.getShipId(busyoId);
                    inst.makeKaisenInstance(mapId, busyoId, shipId, butaiLv, ch_type, butaiQty, hp, atk, dfc, spd, busyoName, 0, false, senpouArray, AIType);
                }                
			}
		}
		Message msg = new Message ();
        if(kokuninFlg) {
            msg.makeKassenMessage(msg.getMessage(162));
        }else { 
    		msg.makeKassenMessage (msg.getMessage(131));
        }

    }


	public void cyouryaku(int heiQty, string cyouryakuTmp){

		//Cyouryaku Target
		int counter = 0;
		foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("EnemyChild")){

			//Param
			EnemyHP enemyHpScript = obs.transform.parent.GetComponent<EnemyHP>();
			Heisyu enemyAtkDfcScript = obs.transform.parent.GetComponent<Heisyu>();
			float child_hp = enemyHpScript.childHP;
			float child_atk = enemyAtkDfcScript.atk;
			float child_dfc = enemyAtkDfcScript.dfc;
			string heisyu = enemyAtkDfcScript.heisyu;
			Vector2 child_location = obs.transform.position;

			string busyoId = obs.transform.parent.name;
			float child_spd = 0;
			if (obs.transform.parent.GetComponent<Homing> ()) {
				child_spd = obs.transform.parent.GetComponent<Homing> ().speed;
			} else {
				child_spd = obs.transform.parent.GetComponent<HomingLong> ().speed;			
			}

			//Reduce Qty & Status
			enemyHpScript.childQty = enemyHpScript.childQty - 1;
			if (enemyAtkDfcScript.heisyu == "YR" || enemyAtkDfcScript.heisyu == "KB" || enemyAtkDfcScript.heisyu == "SHP") {
				obs.transform.parent.GetComponent<EnemyAttack> ().attack = obs.transform.parent.GetComponent<EnemyAttack> ().attack - child_atk;
				obs.transform.parent.GetComponent<EnemyHP> ().dfc = obs.transform.parent.GetComponent<EnemyHP> ().dfc - child_dfc;

			} else {
				obs.transform.parent.GetComponent<AttackLong> ().childAttack = obs.transform.parent.GetComponent<AttackLong> ().childAttack - child_atk;
				obs.transform.parent.GetComponent<EnemyHP> ().dfc = obs.transform.parent.GetComponent<EnemyHP> ().dfc - child_dfc;
			}

			//Delete
			Destroy(obs.gameObject);

            //Create
            string ch_path = "";
            if (Application.loadedLevelName == "kaisen") {
                ch_path = "Prefabs/Kaisen/3";
            }else { 
                ch_path = "Prefabs/Player/hukuhei" + heisyu;
            }
            GameObject ch_prefab = Instantiate (Resources.Load (ch_path)) as GameObject;
			ch_prefab.name = "hukuhei";

            if (Application.loadedLevelName != "kaisen") {
                string sashimono_path = "Prefabs/Sashimono/" + busyoId;
			    GameObject sashimono = Instantiate (Resources.Load (sashimono_path)) as GameObject;
			    sashimono.transform.parent = ch_prefab.transform;
			    sashimono.transform.localScale = new Vector2 (0.3f, 0.3f);

			    if (heisyu == "YR") {
				    sashimono.transform.localPosition = new Vector2 (-1, 0.6f);
				    ch_prefab.transform.position = child_location;

			    } else if (heisyu == "KB") {
				    sashimono.transform.localPosition = new Vector2 (-0.5f, 1);
				    ch_prefab.transform.position = child_location;

			    } else if (heisyu == "TP") {
				    sashimono.transform.localPosition = new Vector2 (-0.8f, 0.5f);
				    ch_prefab.transform.position = child_location;

			    } else if (heisyu == "YM") {
				    sashimono.transform.localPosition = new Vector2 (-0.8f, 0.5f);
				    ch_prefab.transform.position = child_location;
			    }

			    ch_prefab.GetComponent<PlayerHP> ().life = child_hp;
			    if (ch_prefab.GetComponent<PlayerAttack> ()) {
				    ch_prefab.GetComponent<PlayerAttack> ().attack = child_atk;
			    } else {
				    ch_prefab.GetComponent<AttackLong> ().attack = child_atk;
			    }
			    ch_prefab.GetComponent<PlayerHP> ().dfc = child_dfc;

			    if (ch_prefab.GetComponent<Homing> () != null) {
				    ch_prefab.GetComponent<Homing> ().speed = child_spd;
			    } else if (ch_prefab.GetComponent<HomingLong> () != null) {
				    ch_prefab.GetComponent<HomingLong> ().speed = child_spd;
			    }
            }else {
                //kaisen
                if(ch_prefab.GetComponent<UnitMover>()) {
                    Destroy(ch_prefab.GetComponent<UnitMover>());
                }
                Destroy(ch_prefab.GetComponent<Kunkou>());
                Destroy(ch_prefab.GetComponent<SenpouController>());
                ch_prefab.AddComponent<Homing>();
                ch_prefab.GetComponent<PlayerHP>().life = child_hp;
                ch_prefab.GetComponent<PlayerAttack>().attack = child_atk;
                ch_prefab.GetComponent<PlayerHP>().dfc = child_dfc;
                ch_prefab.GetComponent<Homing>().speed = child_spd;
                ch_prefab.transform.position = child_location;
            }

			//SE
//			AudioController audio = new AudioController();
			AudioController.addComponentMoveAttack (ch_prefab,heisyu);

			//Effect
			string effect_path = "Prefabs/Effect/betray";
			GameObject effect = Instantiate (Resources.Load (effect_path)) as GameObject;
			effect.transform.SetParent (ch_prefab.transform);
			effect.transform.localScale = new Vector2 (1, 1);
			effect.transform.localPosition = new Vector2 (0, 0);


			//Counter
			counter = counter + 1;

			//Break
			if (counter == heiQty) {
				break;
			}
		}



		//Delete Data
		string cyoryakuHst = PlayerPrefs.GetString("cyouryaku");
		char[] delimiterChars = {','};
		List<string> cyoryakuHstList = new List<string> ();
		if(cyoryakuHst != null && cyoryakuHst != ""){
			if (cyoryakuHst.Contains (",")) {
				cyoryakuHstList = new List<string> (cyoryakuHst.Split (delimiterChars));
			} else {
				cyoryakuHstList.Add (cyoryakuHst);
			}
		}
		cyoryakuHstList.Remove (cyouryakuTmp);

		string newCyoryakuHst = "";
		for (int i=0; i < cyoryakuHstList.Count; i++) {
			if (i == 0) {
				newCyoryakuHst = cyoryakuHstList[i];
			} else {
				newCyoryakuHst = newCyoryakuHst + "," + cyoryakuHstList[i];
			
			}
		}
		PlayerPrefs.SetString ("cyouryaku",newCyoryakuHst);
		PlayerPrefs.DeleteKey (cyouryakuTmp);
		PlayerPrefs.Flush ();


		Message msg = new Message ();
		msg.makeKassenMessage (msg.getMessage(132));
	}

	public void busouKaijyo(){

		foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Player")){
			Destroy (obs);
		}

		foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Enemy")){
			Destroy (obs);
		}

	}

    void RunEnemySaku(EnemySaku EnemySaku) {

        int runPlace = EnemySaku.runPlace; //0:My Place, 1:Enemy Attack, 2:Middle
        string sakuPath = "Prefabs/Saku/SakuEvent/" + EnemySaku.id;
        GameObject saku = Instantiate(Resources.Load(sakuPath)) as GameObject;
        saku.layer = LayerMask.NameToLayer("EnemySaku");
        Vector2 worldPos = new Vector3(0, 0, 0);//Camera.main.ScreenToWorldPoint(TouchPosition);
        if(runPlace==0) {
            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Enemy")) {
                if (obs.name != "hukuhei" && obs.name != "shiro" && obs.name != "toride") {
                    worldPos = obs.transform.localPosition;
                    break;
                }
            }
        }else if(runPlace==1){

            List<GameObject> objList = new List<GameObject>();
            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Player")) {
                if(obs.name != "hukuhei" && obs.name != "shiro" && obs.name != "toride") {
                    worldPos = obs.transform.localPosition;
                    break;
                }
            }            
            
        } else if(runPlace==2) {
            Vector2 pPos = new Vector3(0, 0, 0);
            Vector2 ePos = new Vector3(0, 0, 0);
            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Enemy")) {
                ePos = obs.transform.localPosition;
                break;
            }

            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Player")) {
                pPos = obs.transform.localPosition;
                break;
            }           
            worldPos = new Vector3((pPos.x + ePos.x)/2, (pPos.y + ePos.y) / 2, 0);
        }
        
        
        if (11 <= EnemySaku.id && EnemySaku.id <= 15) {
            //Gokui
            Vector2 cameraVector = GameObject.Find("Main Camera").transform.localPosition;
            RectTransform sakuTransform = saku.GetComponent<RectTransform>();
            sakuTransform.anchoredPosition = cameraVector;

        }else {
            RectTransform sakuTransform = saku.GetComponent<RectTransform>();
            sakuTransform.anchoredPosition = new Vector2(worldPos.x, worldPos.y);
        }
        
        if (saku.GetComponent<SakuCollider>() != null) {
            saku.GetComponent<SakuCollider>().sakuId = EnemySaku.id;
            saku.GetComponent<SakuCollider>().sakuEffect = EnemySaku.status;

        }else {
            saku.GetComponent<SakuMaker>().sakuId = EnemySaku.id;
            saku.GetComponent<SakuMaker>().sakuEffect = EnemySaku.status;
            saku.GetComponent<SakuMaker>().vect = worldPos;
            saku.GetComponent<SakuMaker>().sakuHeisyu = EnemySaku.sakuHeisyu;
            saku.GetComponent<SakuMaker>().sakuHeiSts = EnemySaku.sakuHeiSts;
            saku.GetComponent<SakuMaker>().sakuBusyoId = EnemySaku.sakuBusyoId;
            saku.GetComponent<SakuMaker>().sakuBusyoSpeed = EnemySaku.sakuBusyoSpeed;

            if (EnemySaku.id == 5) {
                //Yasen Chikujyo
                AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
                audioSources[5].Play();
                audioSources[6].Play();
                Destroy(saku.GetComponent<PlayerHP>());
                saku.AddComponent<EnemyHP>().life = EnemySaku.status;
                saku.transform.Find("BusyoDtlPlayer").transform.Find("MinHpBar").GetComponent<BusyoHPBar>().initLife = EnemySaku.status;
                saku.tag = "Enemy";
                saku.layer = LayerMask.NameToLayer("EnemyNoColl");
                string barPath = "Prefabs/PostKassen/Sprite/minHpBar3";
                saku.transform.Find("BusyoDtlPlayer").transform.Find("MinHpBar").GetComponent<SpriteRenderer>().sprite =                    
                    Resources.Load(barPath, typeof(Sprite)) as Sprite;
            }

        }
    }

    public static bool getRandomBool() {
        return UnityEngine.Random.Range(0, 2) == 0;
    }

}