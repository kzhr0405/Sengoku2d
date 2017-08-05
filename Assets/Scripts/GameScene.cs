using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameScene : MonoBehaviour {

	//base
	public GameObject mapPrefab;
	public GameObject treePrefab;
	public GameObject wallPrefab;
	public int activeKuniId = 0;
	public int activeStageId = 0;

	//enemy
	public int enemySoudaisyo;
	public int totalSenpouLv = 0;
	public int aveSenpouLv = 0;

	//saku
	public bool sakuFlg;
	public int sakuId;
	public int sakuEffect;
	public GameObject sakuBtn;
	public string sakuHeisyu;
	public float sakuHeiSts;
	public int sakuBusyoId;
	public float sakuBusyoSpeed;
	public int soudaisyo;

	//saku kengou
	public int kengouQty;
	public string kengouCd;
	public string kengouName;
	public int soudaisyoHp;
	public int soudaisyoAtk;
	public int soudaisyoDfc;
	public int soudaisyoSpd;

    //scroll
    public Vector2 scrollVect;

    //pvp
    public bool pvpFlg;
    public PvPDataStore DataStore;
    public int pvpStageId = 1;

	// Use this for initialization
	void Start () {

        //Sound
        BGMSESwitch bgm = new BGMSESwitch();
        bgm.StopSEVolume();
        bgm.StopKassenBGMVolume();

        //Taiko
        StartCoroutine("taikoMusic");

		//Kill Prevous BGM
		KillOtherBGM kill = new KillOtherBGM();
		kill.Start ();

        //Dinamic Map
        float mntMinusRatio = 0;
        float seaMinusRatio = 0;
        float rainMinusRatio = 0;
        float snowMinusRatio = 0;

        //Scroll vect
        scrollVect = GameObject.Find("ScrollView").GetComponent<RectTransform>().anchoredPosition;         

        pvpFlg = PlayerPrefs.GetBool("pvpFlg");
        PlayerPrefs.DeleteKey("pvpFlg");
        PlayerPrefs.Flush();

        //Auto button
        bool Auto2Flg = PlayerPrefs.GetBool("Auto2Flg");
        if(Auto2Flg) {
            GameObject.Find("AutoBtn").transform.FindChild("Num").GetComponent<Text>().text = "2";
            GameObject.Find("AutoBtn").GetComponent<AutoAttack>().speed = 2;
        }

        if (!pvpFlg) {
            if(GameObject.Find("PvPName")) {
                Destroy(GameObject.Find("PvPName").gameObject);
            }
            
		    Stage stage = new Stage ();

            //Giveup button
            bool isAttackedFlg = false;
            if (Application.loadedLevelName != "tutorialKassen") {
                isAttackedFlg = PlayerPrefs.GetBool ("isAttackedFlg");
                activeKuniId = PlayerPrefs.GetInt("activeKuniId");
                activeStageId = PlayerPrefs.GetInt("activeStageId");
            }else {
                isAttackedFlg = true;
                activeKuniId = 1;
                activeStageId = 10;
            }

            
            if (isAttackedFlg) {
			    GameObject.Find ("GiveupBtn").SetActive (false);

			    //Shiro & Toride Setting
			    makeShiroTorideObject();
    		}
            
            if (activeStageId != 0) {
                //Active
			    int stageMapId = stage.getStageMap (activeKuniId, activeStageId);

			    string mapPath = "";
			    string mapFrontPath = "";
			    GameObject wall = Instantiate (wallPrefab);
			    wall.name = "wall";

			    if (stageMapId != 1) {
				    if (stageMapId == 2) {
					    //mountain
					    mapPath = "Prefabs/PreKassen/map2";
					    GameObject map = Instantiate (Resources.Load (mapPath)) as GameObject;

					    mapFrontPath = "Prefabs/PreKassen/mapFront2";
					    GameObject mapFront = Instantiate (Resources.Load (mapFrontPath)) as GameObject;

					    weatherHandling(stageMapId, map, mapFront);

				    } else if (stageMapId == 3) {
					    //sea
					    mapPath = "Prefabs/PreKassen/map3";
					    GameObject map = Instantiate (Resources.Load (mapPath)) as GameObject;

					    mapFrontPath = "Prefabs/PreKassen/mapFront3";
					    GameObject mapFront = Instantiate (Resources.Load (mapFrontPath)) as GameObject;

					    weatherHandling(stageMapId, map, mapFront);
				    }
			    } else {
				
				    Instantiate (treePrefab);

				    mapPath = "Prefabs/PreKassen/map1";
				    GameObject map = Instantiate (Resources.Load (mapPath)) as GameObject;

				    weatherHandling(stageMapId, map, null);
			    }

		    } else {
			    //Passive
			    int stageMapId = stage.getStageMap (activeKuniId, 10); 
                if(stageMapId == 4) {
                    stageMapId = 1;
                }

			    string mapPath = "";
			    string mapFrontPath = "";
			    Instantiate (wallPrefab);

			    if (stageMapId != 1) {
				    if (stageMapId == 2) {
					    //mountain
					    mapPath = "Prefabs/PreKassen/map2";
					    GameObject map = Instantiate (Resources.Load (mapPath)) as GameObject;
					    mapFrontPath = "Prefabs/PreKassen/mapFront2";
					    GameObject mapFront = Instantiate (Resources.Load (mapFrontPath)) as GameObject;
					    weatherHandling(stageMapId, map, mapFront);
				    } else if (stageMapId == 3) {
					    //sea
					    mapPath = "Prefabs/PreKassen/map3";
					    GameObject map = Instantiate (Resources.Load (mapPath)) as GameObject;
					    mapFrontPath = "Prefabs/PreKassen/mapFront3";
					    GameObject mapFront = Instantiate (Resources.Load (mapFrontPath)) as GameObject;
					    weatherHandling(stageMapId, map, mapFront);
				    }
			    } else {
				    Instantiate (mapPrefab);
				    Instantiate (treePrefab);

				    weatherHandling(stageMapId, mapPrefab, null);
			    }
		    }

		    /*Get Minus Status*/
		    mntMinusRatio = PlayerPrefs.GetFloat("mntMinusStatus",0);
		    seaMinusRatio = PlayerPrefs.GetFloat("seaMinusStatus",0);
		    rainMinusRatio = PlayerPrefs.GetFloat("rainMinusStatus",0);
		    snowMinusRatio = PlayerPrefs.GetFloat("snowMinusStatus",0);

        } else {
            /*PvP*/
            //GameObject.Find("GiveupBtn").SetActive(false);
            //GameObject.Find("AutoBtn").SetActive(false);
            DataStore = GameObject.Find("PvPDataStore").GetComponent<PvPDataStore>();

            //timer
            GameObject.Find("timer").GetComponent<Timer>().enabled = false;
            GameObject.Find("timer").transform.FindChild("timerText").GetComponent<Text>().text = "∞";

            //Name
            GameObject PvPName = GameObject.Find("PvPName").gameObject;
            PvPName.transform.FindChild("Player").GetComponent<Text>().text = DataStore.myUserName;
            PvPName.transform.FindChild("Enemy").GetComponent<Text>().text = DataStore.enemyUserName;

            string mapPath = "";
            string mapFrontPath = "";
            GameObject wall = Instantiate(wallPrefab);
            wall.name = "wall";
            pvpStageId = PlayerPrefs.GetInt("pvpStageId",1);
            int weatherId = getPvPWeatherId();

            if (pvpStageId != 1) {
                if (pvpStageId == 2) {
                    //mountain
                    mapPath = "Prefabs/PreKassen/map2";
                    GameObject map = Instantiate(Resources.Load(mapPath)) as GameObject;
                    mapFrontPath = "Prefabs/PreKassen/mapFront2";
                    GameObject mapFront = Instantiate(Resources.Load(mapFrontPath)) as GameObject;
                    weatherHandling(pvpStageId, map, mapFront);

                    List<int> idList = new List<int>() { 10, 20, 30 };
                    int rdmId = UnityEngine.Random.Range(0, idList.Count);
                    int minusRatio = idList[rdmId];
                    mntMinusRatio = (100 - (float)minusRatio) / 100;
                }else if (pvpStageId == 3) {
                    //sea
                    mapPath = "Prefabs/PreKassen/map3";
                    GameObject map = Instantiate(Resources.Load(mapPath)) as GameObject;
                    mapFrontPath = "Prefabs/PreKassen/mapFront3";
                    GameObject mapFront = Instantiate(Resources.Load(mapFrontPath)) as GameObject;
                    weatherHandling(pvpStageId, map, mapFront);

                    List<int> idList = new List<int>() { 10, 20, 30 };
                    int rdmId = UnityEngine.Random.Range(0, idList.Count);
                    int minusRatio = idList[rdmId];
                    seaMinusRatio = (100 - (float)minusRatio) / 100;
                }
            }else {
                Instantiate(treePrefab);
                mapPath = "Prefabs/PreKassen/map1";
                GameObject map = Instantiate(Resources.Load(mapPath)) as GameObject;
                weatherHandling(pvpStageId, map, null);
            }

            if (weatherId == 2) {
                List<int> idList = new List<int>() { 10, 20, 30 };
                int rdmId = UnityEngine.Random.Range(0, idList.Count);
                int minusRatio = idList[rdmId];
                rainMinusRatio = (100 - (float)minusRatio) / 100;
            }else if(weatherId == 3) {
                List<int> idList = new List<int>() { 10, 20, 30 };
                int rdmId = UnityEngine.Random.Range(0, idList.Count);
                int minusRatio = idList[rdmId];
                snowMinusRatio = (100 - (float)minusRatio) / 100;
            }

        }

		/*プレイヤー配置*/
		//ユーザ陣形データのロード
		int jinkei =PlayerPrefs.GetInt("jinkei",1);
		List<int> myBusyoList = new List<int> ();
        bool tutorialDoneFlg = PlayerPrefs.GetBool("tutorialDoneFlg");
        if (!tutorialDoneFlg || Application.loadedLevelName != "tutorialKassen") {
            //1.魚麟
            if (jinkei == 1) {
			    soudaisyo = PlayerPrefs.GetInt("soudaisyo1");
			    if(PlayerPrefs.HasKey("1map1")){
				    int mapId = 1;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio,0));
			    }
			    if(PlayerPrefs.HasKey("1map2")){
				    int mapId = 2;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("1map7")){
				    int mapId = 7;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("1map8")){
				    int mapId = 8;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("1map11")){
				    int mapId = 11;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("1map12")){
				    int mapId = 12;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("1map13")){
				    int mapId = 13;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("1map14")){
				    int mapId = 14;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("1map17")){
				    int mapId = 17;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("1map18")){
				    int mapId = 18;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("1map21")){
				    int mapId = 21;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("1map22")){
				    int mapId = 22;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }



		    //2.鶴翼
		    }else if(jinkei == 2){
			    soudaisyo = PlayerPrefs.GetInt("soudaisyo2");

			    if(PlayerPrefs.HasKey("2map3")){
				    int mapId = 3;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("2map4")){
				    int mapId = 4;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("2map5")){
				    int mapId = 5;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("2map7")){
				    int mapId = 7;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("2map8")){
				    int mapId = 8;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("2map11")){
				    int mapId = 11;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("2map12")){
				    int mapId = 12;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("2map17")){
				    int mapId = 17;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("2map18")){
				    int mapId = 18;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("2map23")){
				    int mapId = 23;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("2map24")){
				    int mapId = 24;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("2map25")){
				    int mapId = 25;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }

		    }
		    //3.偃月
		    else if(jinkei == 3){
			    soudaisyo = PlayerPrefs.GetInt("soudaisyo3");

			    if(PlayerPrefs.HasKey("3map3")){
				    int mapId = 3;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("3map7")){
				    int mapId = 7;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("3map8")){
				    int mapId = 8;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("3map9")){
				    int mapId = 9;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("3map11")){
				    int mapId = 11;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("3map12")){
				    int mapId = 12;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("3map14")){
				    int mapId = 14;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("3map15")){
				    int mapId = 15;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("3map16")){
				    int mapId = 16;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("3map20")){
				    int mapId = 20;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("3map21")){
				    int mapId = 21;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("3map25")){
				    int mapId = 25;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
		    }

		    //4.雁行
		    else if(jinkei == 4){
			    soudaisyo = PlayerPrefs.GetInt("soudaisyo4");

			    if(PlayerPrefs.HasKey("4map1")){
				    int mapId = 1;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("4map2")){
				    int mapId = 2;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("4map7")){
				    int mapId = 7;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("4map8")){
				    int mapId = 8;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("4map12")){
				    int mapId = 12;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("4map13")){
				    int mapId = 13;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("4map14")){
				    int mapId = 14;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("4map18")){
				    int mapId = 18;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("4map19")){
				    int mapId = 19;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("4map20")){
				    int mapId = 20;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("4map24")){
				    int mapId = 24;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
			    if(PlayerPrefs.HasKey("4map25")){
				    int mapId = 25;
				    myBusyoList.Add(getStsAndMakeInstance(jinkei,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 0));
			    }
		    }
        }else {
            //retry tutorial
            myBusyoList.Add(getStsAndMakeInstance(jinkei, 12, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, 19));
            myBusyoList.Add(getStsAndMakeInstance(jinkei, 13, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio, PlayerPrefs.GetInt("tutorialBusyo")));
        }

		//Saku
		BusyoInfoGet info = new BusyoInfoGet();
		StatusGet sts = new StatusGet();
		GameObject content = GameObject.Find ("Content").gameObject;
		string slotPath = "Prefabs/Saku/Slot";
		Saku saku = new Saku ();

		foreach ( Transform n in content.transform ){
			GameObject.Destroy(n.gameObject);
		}

        
        foreach (int busyoId in myBusyoList){
			GameObject slot = Instantiate (Resources.Load (slotPath)) as GameObject;

			List<string> sakuList = new List<string>();
			string tmp = "gokui" + busyoId;
			if (PlayerPrefs.HasKey (tmp)) {
				int gokuiId = PlayerPrefs.GetInt (tmp);
				sakuList = saku.getGokuiInfo(busyoId, gokuiId);
			} else {
				sakuList = saku.getSakuInfo (busyoId);
			}
            
			string sakuPath = "Prefabs/Saku/saku" + sakuList[0];
			GameObject sakuIcon = Instantiate (Resources.Load (sakuPath)) as GameObject;
			sakuIcon.transform.SetParent (slot.transform);
			sakuIcon.transform.localScale = new Vector2 (0.45f, 0.45f);
			sakuIcon.GetComponent<Button>().enabled = false;

			slot.transform.SetParent (content.transform);
			slot.transform.localScale = new Vector2 (1, 1);

			slot.GetComponent<Saku>().sakuId = int.Parse(sakuList[0]);
			slot.GetComponent<Saku>().sakuEffect = int.Parse(sakuList[4]);

			if(sakuList[0] == "3"){
				//hukuhei
				//Heisyu
				slot.GetComponent<Saku>().sakuHeisyu = info.getHeisyu(busyoId);
				//Hei Status
				string heiId = "hei" + busyoId.ToString();
				string chParam = PlayerPrefs.GetString(heiId,"0");
                if (chParam == "0") {
                    StatusGet statusScript = new StatusGet();
                    string heisyu = statusScript.getHeisyu(busyoId);
                    chParam = heisyu + ":1:1:1";
                    PlayerPrefs.SetString(heiId, chParam);
                    PlayerPrefs.Flush();
                }

                if (chParam.Contains(":")) {
				    char[] delimiterChars = {':'};
				    string[] ch_list = chParam.Split(delimiterChars);
				    slot.GetComponent<Saku>().sakuHeiSts =  float.Parse (ch_list[3]);				    
                }else {
                    slot.GetComponent<Saku>().sakuHeiSts = 1;
                }
                slot.GetComponent<Saku>().sakuBusyoId = busyoId;

                //Busyo Speed
                int sakuBusyoLv = PlayerPrefs.GetInt(busyoId.ToString());
                float adjSpd = (float)sts.getSpd(busyoId, sakuBusyoLv)/10;
                slot.GetComponent<Saku>().sakuBusyoSpeed = adjSpd;
            }
		}

        if (!tutorialDoneFlg || Application.loadedLevelName != "tutorialKassen") {
            //Kengou
            string kengouString = PlayerPrefs.GetString("kengouItem");
		    List<string> kengouList = new List<string> ();
		    char[] delimiterChars3 = {','};
		    kengouList = new List<string> (kengouString.Split (delimiterChars3));

		    for (int i=0; i<kengouList.Count; i++) {
			    int qty = int.Parse(kengouList[i]);
			    if(qty != 0){
				    GameObject slot = Instantiate (Resources.Load (slotPath)) as GameObject;
				    string kengouPath = "Prefabs/Saku/saku7";
				    GameObject sakuIcon = Instantiate (Resources.Load (kengouPath)) as GameObject;
				    sakuIcon.transform.SetParent (slot.transform);
				    sakuIcon.transform.localScale = new Vector2 (0.45f, 0.45f);
				    sakuIcon.GetComponent<Button>().enabled = false;

				    slot.transform.SetParent (content.transform);
				    slot.transform.localScale = new Vector2 (1, 1);

				    ItemInfo item = new ItemInfo();
				    int temp = i + 1;
				    string itemCd = "kengou" + temp.ToString();
				    string kengouName = item.getItemName(itemCd);
				    sakuIcon.transform.FindChild("sakuIconText").GetComponent<Text>().text = kengouName;
				    sakuIcon.transform.FindChild("sakuIconText").transform.localScale = new Vector2 (0.35f,0.35f);

				    slot.GetComponent<Saku>().sakuId = 7;

				    int effect = item.getItemEffect(itemCd);
				    slot.GetComponent<Saku>().sakuEffect = effect;
				    slot.GetComponent<Saku>().kengouCd = itemCd;
				    slot.GetComponent<Saku>().kengouQty = qty;
				    slot.GetComponent<Saku>().kengouName = kengouName;
			    }
		    }


		    //Nanban
		    string nanbanString = PlayerPrefs.GetString("nanbanItem");
		    List<string> nanbanList = new List<string> ();
		    nanbanList = new List<string> (nanbanString.Split (delimiterChars3));

		    for (int i=0; i<nanbanList.Count; i++) {
			    int qty = int.Parse(nanbanList[i]);
			    if(qty != 0){
				    GameObject slot = Instantiate (Resources.Load (slotPath)) as GameObject;

				    string nanbanPath = "";
				    if(i==0){
					    nanbanPath = "Prefabs/Saku/saku8";
				    }else if(i==1){
					    nanbanPath = "Prefabs/Saku/saku9";
				    }else if(i==2){
					    nanbanPath = "Prefabs/Saku/saku10";
				    }

				    GameObject sakuIcon = Instantiate (Resources.Load (nanbanPath)) as GameObject;
				    sakuIcon.transform.SetParent (slot.transform);
				    sakuIcon.transform.localScale = new Vector2 (0.45f, 0.45f);
				    sakuIcon.GetComponent<Button>().enabled = false;
				
				    slot.transform.SetParent (content.transform);
				    slot.transform.localScale = new Vector2 (1, 1);

				    if(i==0){
					    slot.GetComponent<Saku>().sakuId = 8;
				    }else if(i==1){
					    slot.GetComponent<Saku>().sakuId = 9;
				    }else if(i==2){
					    slot.GetComponent<Saku>().sakuId = 10;
				    }

				    int temp = i + 1;
				    ItemInfo item = new ItemInfo();
				    string itemCd = "nanban" + temp.ToString();
				    int effect = item.getItemEffect(itemCd);
				    slot.GetComponent<Saku>().sakuEffect = effect;


				    if(i == 2){
					    //teppou youhei
					    slot.GetComponent<Saku>().sakuHeisyu = "TP";
					    //Hei Status
					    string heiId = "hei" + soudaisyo.ToString();
					    string chParam = PlayerPrefs.GetString(heiId,"0");
                        if (chParam == "0") {
                            StatusGet statusScript = new StatusGet();
                            string heisyu = statusScript.getHeisyu(soudaisyo);
                            chParam = heisyu + ":1:1:1";
                            PlayerPrefs.SetString(heiId, chParam);
                            PlayerPrefs.Flush();
                        }
                        if (chParam.Contains(":")) {
                            char[] delimiterChars = { ':' };
                            string[] ch_list = chParam.Split(delimiterChars);
                            slot.GetComponent<Saku>().sakuHeiSts = float.Parse(ch_list[3]);
                        }
                        else {
                            slot.GetComponent<Saku>().sakuHeiSts = 1;
                        }
                        slot.GetComponent<Saku>().sakuBusyoId =  soudaisyo;
					
					    //Busyo Speed
					    int sakuBusyoLv = PlayerPrefs.GetInt(soudaisyo.ToString());
                        float adjSpd = (float)sts.getSpd(soudaisyo, sakuBusyoLv) / 10;
                        slot.GetComponent<Saku>().sakuBusyoSpeed = adjSpd;
				    }

			    }
		    }
        }


        /*エネミー配置*/
        if (!pvpFlg) {

            int linkNo = PlayerPrefs.GetInt("activeLink",0);
		    enemySoudaisyo = PlayerPrefs.GetInt("enemySoudaisyo");


		    if(PlayerPrefs.HasKey("emap1")){
			    int mapId = 1;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap2")){
			    int mapId = 2;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap3")){
			    int mapId = 3;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap4")){
			    int mapId = 4;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap5")){
			    int mapId = 5;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap6")){
			    int mapId = 6;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap7")){
			    int mapId = 7;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap8")){
			    int mapId = 8;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap9")){
			    int mapId = 9;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap10")){
			    int mapId = 10;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap11")){
			    int mapId = 11;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap12")){
			    int mapId = 12;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap13")){
			    int mapId = 13;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap14")){
			    int mapId = 14;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap15")){
			    int mapId = 15;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap16")){
			    int mapId = 16;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap17")){
			    int mapId = 17;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap18")){
			    int mapId = 18;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap19")){
			    int mapId = 19;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap20")){
			    int mapId = 20;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap21")){
			    int mapId = 21;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap22")){
			    int mapId = 22;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap23")){
			    int mapId = 23;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap24")){
			    int mapId = 24;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
		    if(PlayerPrefs.HasKey("emap25")){
			    int mapId = 25;
			    getEnemyStsAndMakeInstance(linkNo,mapId, mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
		    }
        }else {
            //PvP
            List<int> PvPBusyoList = new List<int>();
            List<int> PvPLvList = new List<int>();
            List<string> PvPHeiList = new List<string>();
            List<int> PvPSenpouLvList = new List<int>();
            List<int> PvPSakuLvList = new List<int>();
            List<string> PvPKahouList = new List<string>();
            int soudaisyo = 0;
            if (pvpStageId==1) {
                PvPBusyoList = DataStore.PvP1BusyoList;
                PvPLvList = DataStore.PvP1LvList;
                PvPHeiList = DataStore.PvP1HeiList;
                PvPSenpouLvList = DataStore.PvP1SenpouLvList;
                PvPSakuLvList = DataStore.PvP1SakuLvList;
                PvPKahouList = DataStore.PvP1KahouList;
                soudaisyo = DataStore.soudaisyo1;
            }else if(pvpStageId==2) {
                PvPBusyoList = DataStore.PvP2BusyoList;
                PvPLvList = DataStore.PvP2LvList;
                PvPHeiList = DataStore.PvP2HeiList;
                PvPSenpouLvList = DataStore.PvP2SenpouLvList;
                PvPSakuLvList = DataStore.PvP2SakuLvList;
                PvPKahouList = DataStore.PvP2KahouList;
                soudaisyo = DataStore.soudaisyo2;
            }else if(pvpStageId==3) {
                PvPBusyoList = DataStore.PvP3BusyoList;
                PvPLvList = DataStore.PvP3LvList;
                PvPHeiList = DataStore.PvP3HeiList;
                PvPSenpouLvList = DataStore.PvP3SenpouLvList;
                PvPSakuLvList = DataStore.PvP3SakuLvList;
                PvPKahouList = DataStore.PvP3KahouList;
                soudaisyo = DataStore.soudaisyo3;
            }




            List<int> plus4List = new List<int>() { 1, 6, 11, 16, 21 };
            List<int> plus2List = new List<int>() { 2, 7, 12, 17, 22 };
            List<int> minus2List = new List<int>() { 4, 9, 14, 19, 24 };
            List<int> minus4List = new List<int>() { 5, 10, 15, 20, 25 };

             
            
            int counter = 0;
            for (int i = 0; i < PvPBusyoList.Count; i++) {

                int busyoId = PvPBusyoList[i];
                if (busyoId != 0) {                       
                    int busyoLv = PvPLvList[counter];
                    string heiString = PvPHeiList[counter];
                    char[] delimiterChars = { ':' };
                    string[] heilist = heiString.Split(delimiterChars);
                    int butaiQty = int.Parse(heilist[1]);
                    int butaiLv = int.Parse(heilist[2]);
                    float butaiStatus = float.Parse(heilist[3]);

                    int senpouLv = PvPSenpouLvList[counter];
                    int sakuLv = PvPSakuLvList[counter];
                    string kahouList = PvPKahouList[counter];

                    //map Id modification
                    int mapId = i + 1;
                    if(plus4List.Contains(mapId)) {
                        mapId = mapId + 4;
                    }else if(plus2List.Contains(mapId)) {
                        mapId = mapId + 2;
                    }else if (minus4List.Contains(mapId)) {
                        mapId = mapId - 4;
                    }else if (minus2List.Contains(mapId)) {
                        mapId = mapId - 2;
                    }
                    getPvPStsAndMakeInstance(mapId, busyoId, busyoLv, butaiQty, butaiLv, butaiStatus, senpouLv, sakuLv, kahouList, soudaisyo,mntMinusRatio, seaMinusRatio, rainMinusRatio, snowMinusRatio);
                    counter = counter + 1;
                }
            }
            

        }

        if (Application.loadedLevelName == "tutorialKassen") {
            StopEveryObject();
        }else { 

            /*Dynamic Enemy Setting Finish*/
            //合戦開始エフェクト
            string pathBack = "Prefabs/PreKassen/backGround";
		    GameObject back = Instantiate(Resources.Load (pathBack)) as GameObject;
		    back.transform.localScale = new Vector2 (30, 15);

		    string pathLight = "Prefabs/PreKassen/lightning";
		    GameObject light = Instantiate(Resources.Load (pathLight)) as GameObject;
		    light.transform.localScale = new Vector2 (10, 10);

        }



    }

	void Update(){
		//Saku

		//On
		if (sakuFlg) {
			Vector2 TouchPosition = AppUtil.GetTouchPosition ();
			if (TouchPosition != Vector2.zero) {

                //Check Menu Overap
                bool menuWrapFlg = false;
                Vector2 preWorldPos = Camera.main.ScreenToWorldPoint(TouchPosition);
                Collider2D[] collition2d = Physics2D.OverlapPointAll(preWorldPos);
                if (collition2d.Length > 0) {
                    foreach (Collider2D collider in collition2d) {
                        if (collider.transform.gameObject.name == "ScrollView" || collider.transform.gameObject.name == "wall_upper" || collider.transform.gameObject.name == "wall_lower") {
                            menuWrapFlg = true;
                        }
                    }                    
                }
            
                if (!menuWrapFlg) {
                    sakuFlg = false;
					string sakuPath = "Prefabs/Saku/SakuEvent/" + sakuId;
					GameObject saku = Instantiate(Resources.Load (sakuPath)) as GameObject;
					Vector2 worldPos = Camera.main.ScreenToWorldPoint(TouchPosition);

					if (11 <= sakuId && sakuId <= 15) {
						//Gokui
						Vector2 cameraVector = GameObject.Find("Main Camera").transform.localPosition;
						RectTransform sakuTransform = saku.GetComponent<RectTransform> ();
						sakuTransform.anchoredPosition = cameraVector;

					} else {
						RectTransform sakuTransform = saku.GetComponent<RectTransform> ();
						sakuTransform.anchoredPosition = new Vector2 (worldPos.x, worldPos.y);
					}


					if(saku.GetComponent<SakuCollider>() != null){
						saku.GetComponent<SakuCollider>().sakuId = sakuId;
						saku.GetComponent<SakuCollider>().sakuEffect = sakuEffect;

						if(sakuId==8){
							reduceNanbanItem();
						}

					}else{
						saku.GetComponent<SakuMaker>().sakuId = sakuId;
						saku.GetComponent<SakuMaker>().sakuEffect = sakuEffect;
						saku.GetComponent<SakuMaker>().vect = worldPos;
						saku.GetComponent<SakuMaker>().sakuHeisyu = sakuHeisyu;
						saku.GetComponent<SakuMaker>().sakuHeiSts = sakuHeiSts;
						saku.GetComponent<SakuMaker>().sakuBusyoId = sakuBusyoId;
						saku.GetComponent<SakuMaker>().sakuBusyoSpeed = sakuBusyoSpeed;
						saku.GetComponent<SakuMaker>().kengouCd = kengouCd;
						saku.GetComponent<SakuMaker>().kengouQty = kengouQty;


						if(sakuId==5){
							//Yasen Chikujyo
							AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
							audioSources [5].Play ();
                            audioSources[6].Play();
                            saku.GetComponent<PlayerHP>().life = sakuEffect;
							saku.transform.FindChild("BusyoDtlPlayer").transform.FindChild("MinHpBar").GetComponent<BusyoHPBar>().initLife = sakuEffect;
						}

					}

					Destroy(sakuBtn);
                    
				}else{
					Debug.Log ("Menu Overapp");
				}
			}
		}


	}



	public int getStsAndMakeInstance(int jinkei, int mapId, float mntMinusRatio, float seaMinusRatio, float rainMinusRatio, float snowMinusRatio, int tutorialBusyoId){

		String map = jinkei.ToString() + "map" + mapId;
        //Get Status
        int busyoId = 0;
        int lv = 1;
        if (tutorialBusyoId == 0) {
		    busyoId = PlayerPrefs.GetInt(map);
            string busyoString = busyoId.ToString();
            lv = PlayerPrefs.GetInt(busyoString);
        }else {
            busyoId = tutorialBusyoId;
        }
        
		StatusGet sts = new StatusGet ();
		int hp = sts.getHp (busyoId, lv);
		int atk = sts.getAtk (busyoId, lv);
		int dfc = sts.getDfc (busyoId, lv);
		int spd = sts.getSpd (busyoId, lv);
		string busyoName = sts.getBusyoName (busyoId);
		ArrayList senpouArray = sts.getSenpou (busyoId, false);


		//Make Average Senpou Lv
		totalSenpouLv = totalSenpouLv + (int)senpouArray[8];


		//Map & Weather Minus
		string heisyu = sts.getHeisyu (busyoId);
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

		if (busyoId == soudaisyo) {
			soudaisyoHp = hp;
			soudaisyoAtk = atk;
			soudaisyoDfc = dfc;
			soudaisyoSpd = spd/10;
		}

		int boubi = 0;
		if (activeStageId == 0) {
			//Passive
			boubi = PlayerPrefs.GetInt("activeBoubi", 0);
		}


		//View Object & pass status to it. 
		PlayerInstance inst = new PlayerInstance ();
		inst.makeInstance (busyoId, mapId, hp, atk, dfc, spd, senpouArray, busyoName, soudaisyo, boubi);

		return busyoId;
	}


	public void getEnemyStsAndMakeInstance(int linkNo, int mapId, float mntMinusRatio, float seaMinusRatio, float rainMinusRatio, float snowMinusRatio){
		
		String map = "emap" + mapId;
		int busyoId = PlayerPrefs.GetInt(map);

		int activeBusyoLv = PlayerPrefs.GetInt ("activeBusyoLv");
		int activeButaiQty = PlayerPrefs.GetInt ("activeButaiQty");
		int activeButaiLv = PlayerPrefs.GetInt ("activeButaiLv");

		StatusGet sts = new StatusGet ();
		BusyoInfoGet info = new BusyoInfoGet();
		int hp = sts.getHp (busyoId, activeBusyoLv);
		int atk = sts.getAtk (busyoId, activeBusyoLv);
		int dfc = sts.getDfc (busyoId, activeBusyoLv);
		int spd = sts.getSpd (busyoId, activeBusyoLv);
		string busyoName = sts.getBusyoName (busyoId);
		string heisyu = sts.getHeisyu (busyoId);

		int playerBusyoQty = PlayerPrefs.GetInt ("jinkeiBusyoQty");
		aveSenpouLv = Mathf.CeilToInt(totalSenpouLv / playerBusyoQty);
		ArrayList senpouArray = sts.getEnemySenpou(busyoId, aveSenpouLv, "");

		//Map & Weather Minus
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

		bool enemyTaisyoFlg = false;
		if (busyoId == enemySoudaisyo) {
			enemyTaisyoFlg = true;

		}

		//View Object & pass status to it.
		EnemyInstance inst = new EnemyInstance ();
        if (Application.loadedLevelName == "tutorialKassen") {
            activeButaiLv = 10;
            activeButaiQty = 20;
        }
        inst.makeInstance(mapId, busyoId, activeButaiLv, heisyu, activeButaiQty, hp, atk, dfc, spd, busyoName,linkNo,enemyTaisyoFlg,senpouArray,"");
	}

    public void getPvPStsAndMakeInstance(int mapId, int busyoId, int busyoLv, int butaiQty, int butaiLv, float butaiStatus, int senpouLv, int sakuLv, string kahouList, int soudaisyo, float mntMinusRatio, float seaMinusRatio, float rainMinusRatio, float snowMinusRatio) {

        //Get Basic Info.
        StatusGet sts = new StatusGet();
        BusyoInfoGet info = new BusyoInfoGet();
        int hp = sts.getHp(busyoId, busyoLv);
        int atk = sts.getAtk(busyoId, busyoLv);
        int dfc = sts.getDfc(busyoId, busyoLv);
        int spd = sts.getSpd(busyoId, busyoLv);
        string busyoName = sts.getBusyoName(busyoId);
        string heisyu = sts.getHeisyu(busyoId);
        ArrayList senpouArray = sts.getEnemySenpou(busyoId, senpouLv, kahouList);

        //Map & Weather Minus
        if (mntMinusRatio != 0) {
            if (heisyu == "KB") {
                float tmp = (float)spd * mntMinusRatio;
                if (tmp < 1) {
                    tmp = 1;
                }
                spd = Mathf.FloorToInt(tmp);
            }
        }else if (seaMinusRatio != 0) {
            if (heisyu == "TP") {
                float tmp = (float)dfc * seaMinusRatio;
                if (tmp < 1) {
                    tmp = 1;
                }
                dfc = Mathf.FloorToInt(tmp);
            }else if (heisyu == "YM") {
                float tmp = (float)dfc * seaMinusRatio;
                if (tmp < 1) {
                    tmp = 1;
                }
                dfc = Mathf.FloorToInt(tmp);
            }
        }if (rainMinusRatio != 0) {
            if (heisyu == "TP") {
                float tmp = (float)atk * rainMinusRatio;
                if (tmp < 1) {
                    tmp = 1;
                }
                atk = Mathf.FloorToInt(tmp);
            }else if (heisyu == "YM") {
                float tmp = (float)atk * rainMinusRatio;
                if (tmp < 1) {
                    tmp = 1;
                }
                atk = Mathf.FloorToInt(tmp);
            }
        }else if (snowMinusRatio != 0) {
            float tmp = (float)spd * 0.5f;
            if (tmp < 1) {
                tmp = 1;
            }
            spd = Mathf.FloorToInt(tmp);

            if (heisyu == "TP") {
                float tmp2 = (float)atk * snowMinusRatio;
                if (tmp2 < 1) {
                    tmp2 = 1;
                }
                atk = Mathf.FloorToInt(tmp2);
            }else if (heisyu == "YM") {
                float tmp2 = (float)atk * snowMinusRatio;
                if (tmp2 < 1) {
                    tmp2 = 1;
                }
                atk = Mathf.FloorToInt(tmp2);
            }else if (heisyu == "KB") {
                float tmp2 = (float)dfc * snowMinusRatio;
                if (tmp2 < 1) {
                    tmp2 = 1;
                }
                dfc = Mathf.FloorToInt(tmp2);
            }
        }

        bool enemyTaisyoFlg = false;
        if (busyoId == soudaisyo) {
            enemyTaisyoFlg = true;
        }

        //View Object & pass status to it.
        EnemyInstance inst = new EnemyInstance();
        inst.makeInstance(mapId, busyoId, butaiLv, heisyu, butaiQty, hp, atk, dfc, spd, busyoName, 0, enemyTaisyoFlg, senpouArray, kahouList);
    }




    public void reduceNanbanItem(){

		string nanbanString = PlayerPrefs.GetString("nanbanItem");
		List<string> nanbanList = new List<string> ();
		char[] delimiterChars = {','};
		nanbanList = new List<string> (nanbanString.Split (delimiterChars));
		
		int qty = int.Parse(nanbanList[0]);
		
		int remainQty = qty - 1;
		nanbanList[0] = remainQty.ToString();
		
		string newNanbanString = "";
		for(int i=0; i<nanbanList.Count; i++){
			
			if(i==0){
				newNanbanString = nanbanList[i];
			}else{
				newNanbanString = newNanbanString +  "," + nanbanList[i];
			}
		}

		PlayerPrefs.SetString("nanbanItem",newNanbanString);
		PlayerPrefs.Flush();

	}


	public void weatherHandling(int stageMapId, GameObject map, GameObject mapFront){
		Color rainSnowColor = new Color (140f / 255f, 140f / 255f, 140f / 255f, 255f / 255f);

		//Dinamic Weather
		int weatherId  = PlayerPrefs.GetInt("weather");

		if (weatherId != 1) {
			if (weatherId == 2) {
				//mountain
				string particlePath = "Prefabs/PreKassen/particle/RainParticle";
				GameObject rain = Instantiate (Resources.Load (particlePath)) as GameObject;
                rain.transform.SetParent(GameObject.Find("Canvas").transform);
                rain.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                rain.transform.localPosition = new Vector3(0, 220, 0);

            } else if (weatherId == 3) {
				//sea
				string particlePath = "Prefabs/PreKassen/particle/SnowParticle";
				GameObject snow = Instantiate (Resources.Load (particlePath)) as GameObject;
                snow.transform.SetParent(GameObject.Find("Canvas").transform);
                snow.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                snow.transform.localPosition = new Vector3(0, 220, 0);
            }

			if (stageMapId == 1) {
				//nml
				map.GetComponent<SpriteRenderer> ().color = rainSnowColor;
				foreach (Transform n in map.transform) {
					n.GetComponent<SpriteRenderer> ().color = rainSnowColor;
				}
			}else{
				//mnt + sea
				map.GetComponent<SpriteRenderer>().color = rainSnowColor;
				foreach ( Transform n in map.transform ){
					n.GetComponent<SpriteRenderer> ().color = rainSnowColor;
				}

				foreach ( Transform n in mapFront.transform ){
					n.GetComponent<SpriteRenderer> ().color = rainSnowColor;
				}

			}

		}
			
	}

	IEnumerator taikoMusic(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [9].Play (); //horagai
        audioSources[12].Play (); //bgm

		audioSources [5].Play ();
		yield return new WaitForSeconds (0.5f);
		audioSources [5].Play ();
		yield return new WaitForSeconds (0.5f);
		audioSources [5].Play ();
		yield return new WaitForSeconds (0.5f);
		audioSources [5].Play ();
	}

	public void makeShiroTorideObject(){

        //Shiro
        //Tutorial
        int shiroLv = 1;
        if (Application.loadedLevelName != "tutorialKassen") {
            shiroLv = PlayerPrefs.GetInt("pSRLv");
        }else {
            shiroLv = 20;
        }
		string objPath = "Prefabs/Kassen/pShiro";
		GameObject shiroObj = Instantiate(Resources.Load (objPath)) as GameObject;
		shiroObj.transform.localScale = new Vector2 (2,1.5f);
		setPlayerObjectOnMap (12, shiroObj);
		shiroObj.name = "shiro";

		string stageName = PlayerPrefs.GetString ("activeStageName");
		shiroObj.transform.FindChild ("BusyoDtlPlayer").transform.FindChild ("NameLabel").GetComponent<TextMesh> ().text = stageName;

		//Sprite
        string shiroTmp = "shiro" + activeKuniId;
        if (PlayerPrefs.HasKey(shiroTmp)) {
            int shiroId = PlayerPrefs.GetInt(shiroTmp);
            if (shiroId != 0) {
                string imagePath = "Prefabs/Naisei/Shiro/Sprite/" + shiroId;
                shiroObj.GetComponent<SpriteRenderer>().sprite =
                                Resources.Load(imagePath, typeof(Sprite)) as Sprite;
            }
        }else {
            string Type = "";
            if (shiroLv < 8) {
                Type = "s";
            }
            else if (shiroLv < 15) {
                Type = "m";
            }
            else if (15 <= shiroLv) {
                Type = "l";
            }
            string imagePath = "Prefabs/Naisei/Shiro/Sprite/shiro_" + Type;
            shiroObj.GetComponent<SpriteRenderer>().sprite =
                Resources.Load(imagePath, typeof(Sprite)) as Sprite;
        }



        //HP
        NaiseiInfo naisei = new NaiseiInfo();
		int tmpShiroEffect = naisei.getNaiseiEffect("shiro", shiroLv);
		int shiroEffect = 2000 + tmpShiroEffect * 25;
		shiroObj.GetComponent<PlayerHP> ().initLife = shiroEffect;
		shiroObj.GetComponent<PlayerHP> ().life = shiroEffect;
		shiroObj.transform.FindChild ("BusyoDtlPlayer").transform.FindChild ("MinHpBar").GetComponent<BusyoHPBar> ().initLife = shiroEffect;

		//Toride
		for (int i = 1; i < 26; i++) {
			string pTRMap ="pTRLv" + i.ToString ();
			if (PlayerPrefs.HasKey (pTRMap)) {

				int torideLv = PlayerPrefs.GetInt (pTRMap);
				string trdPath = "Prefabs/Kassen/pToride";
				GameObject torideObj = Instantiate(Resources.Load (trdPath)) as GameObject;
				torideObj.transform.localScale = new Vector2 (3,3);
				setPlayerObjectOnMap (i, torideObj);
				torideObj.name = "toride";

				string torideType = "";
				if (torideLv < 8) {
					torideType = "s";
				} else if (torideLv < 15) {
					torideType = "m";
				} else if (15<=torideLv) {
					torideType = "l";
				}

				string trdimagePath = "Prefabs/Kassen/kassenTrd_" + torideType;
				torideObj.GetComponent<SpriteRenderer> ().sprite = 
					Resources.Load (trdimagePath, typeof(Sprite)) as Sprite;


				//HP
				int tmpTrdEffect = naisei.getNaiseiEffect("trd", torideLv);
				int trdEffect = 1000 + tmpTrdEffect * 20;
				torideObj.GetComponent<PlayerHP> ().initLife = trdEffect;
				torideObj.GetComponent<PlayerHP> ().life = trdEffect;
				torideObj.transform.FindChild ("BusyoDtlPlayer").transform.FindChild ("MinHpBar").GetComponent<BusyoHPBar> ().initLife = trdEffect;

			}
		}


	}

	public void setPlayerObjectOnMap(int mapId, GameObject obj){
		if (mapId == 1) {
			obj.transform.position = new Vector2 (-65, 16);
		} else if (mapId == 2) {
			obj.transform.position = new Vector2 (-50, 16);
		} else if (mapId == 3) {
			obj.transform.position = new Vector2 (-35, 16);
		} else if (mapId == 4) {
			obj.transform.position = new Vector2 (-20, 16);
		} else if (mapId == 5) {
			obj.transform.position = new Vector2 (-5, 16);
		} else if (mapId == 6) {
			obj.transform.position = new Vector2 (-65, 8);
		} else if (mapId == 7) {
			obj.transform.position = new Vector2 (-50, 8);
		} else if (mapId == 8) {
			obj.transform.position = new Vector2 (-35, 8);
		} else if (mapId == 9) {
			obj.transform.position = new Vector2 (-20, 8);
		} else if (mapId == 10) {
			obj.transform.position = new Vector2 (-5, 8);
		} else if (mapId == 11) {
			obj.transform.position = new Vector2 (-65, 0);
		} else if (mapId == 12) {
			obj.transform.position = new Vector2 (-50, 0);
		} else if (mapId == 13) {
			obj.transform.position = new Vector2 (-35, 0);
		} else if (mapId == 14) {
			obj.transform.position = new Vector2 (-20, 0);
		} else if (mapId == 15) {
			obj.transform.position = new Vector2 (-5, 0);
		} else if (mapId == 16) {
			obj.transform.position = new Vector2 (-65, -8);
		} else if (mapId == 17) {
			obj.transform.position = new Vector2 (-50, -8);
		} else if (mapId == 18) {
			obj.transform.position = new Vector2 (-35, -8);
		} else if (mapId == 19) {
			obj.transform.position = new Vector2 (-20, -8);
		} else if (mapId == 20) {
			obj.transform.position = new Vector2 (-5, -8);
		} else if (mapId == 21) {
			obj.transform.position = new Vector2 (-65, -16);
		} else if (mapId == 22) {
			obj.transform.position = new Vector2 (-50, -16);
		} else if (mapId == 23) {
			obj.transform.position = new Vector2 (-35, -16);
		} else if (mapId == 24) {
			obj.transform.position = new Vector2 (-20, -16);
		} else if (mapId == 25) {
			obj.transform.position = new Vector2 (-5, -16);
		}
	}

    public void StopEveryObject() {
        //Tutorial
        
        //Enemy
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")) {
            if (obj.GetComponent<Homing>()) {
                obj.GetComponent<Homing>().enabled = false;
            }
            else {
                obj.GetComponent<HomingLong>().enabled = false;
            }
        }
        //Player
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player")) {
            if (obj.GetComponent<UnitMover>()) {
                obj.GetComponent<UnitMover>().enabled = false;
            }

        }
        
    }

    public int getPvPWeatherId() {

        //get season
        string yearSeason = PlayerPrefs.GetString("yearSeason");
        char[] delimiterChars = { ',' };
        string[] yearSeasonList = yearSeason.Split(delimiterChars);
        int nowSeason = int.Parse(yearSeasonList[1]);

        int weatherId = 0; //1:normal, 2:rain, 3:snow

        if (nowSeason == 1 || nowSeason == 3) {
            //Spring & Fall
            float percent = UnityEngine.Random.value;
            percent = percent * 100;

            if (percent <= 70) {
                weatherId = 1;
            }else if (70 < percent && percent <= 90) {
                weatherId = 2;
            }else if (90 < percent && percent <= 100) {
                weatherId = 3;
            }
        }else if (nowSeason == 2) {
            //Summer
            float percent = UnityEngine.Random.value;
            percent = percent * 100;

            if (percent <= 60) {
                weatherId = 1;
            }else if (60 < percent && percent <= 100) {
                weatherId = 2;
            }        
        }else if (nowSeason == 4) {
            //Winter
            float percent = UnityEngine.Random.value;
            percent = percent * 100;

            if (percent <= 50) {
                weatherId = 1;
            }else if (50 < percent && percent <= 70) {
                weatherId = 2;
            }else if (70 < percent && percent <= 100) {
                weatherId = 3;
            }           
        }

        return weatherId;
    }



}