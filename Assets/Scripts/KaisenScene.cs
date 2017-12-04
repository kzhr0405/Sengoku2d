using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class KaisenScene : MonoBehaviour {

    public int activeKuniId = 0;
    public int activeStageId = 0;
    public GameObject wallPrefab;
    public GameObject map;
    public int soudaisyo;
    public int totalSenpouLv = 0;
    public int soudaisyoHp;
    public int soudaisyoAtk;
    public int soudaisyoDfc;
    public int soudaisyoSpd;
    public int enemySoudaisyo;
    public int aveSenpouLv = 0;
    public List<string> sameDaimyoList;
    public List<string> sameDaimyoNumList;

    //saku
    public bool sakuFlg;
    public int sakuId;
    public int sakuEffect;
    public GameObject sakuBtn;
    public string sakuHeisyu;
    public float sakuHeiSts;
    public int sakuBusyoId;
    public float sakuBusyoSpeed;
    
    //enemy saku (no PvP)
    public int totalSakuLv;
    public int aveSakuLv;
    public List<EnemySaku> EnemySakuList;

    void Start () {

        GameScene gameSceneScript = new GameScene();

        //Sound
        BGMSESwitch bgm = new BGMSESwitch();
        bgm.StopSEVolume();
        bgm.StopKassenBGMVolume();

        //Taiko
        StartCoroutine("taikoMusic");

        //Kill Prevous BGM
        KillOtherBGM kill = new KillOtherBGM();
        kill.Start();

        //Giveup button
        bool isAttackedFlg = PlayerPrefs.GetBool("isAttackedFlg");
        if (isAttackedFlg) {
            GameObject.Find("GiveupBtn").SetActive(false);

        }

        //Auto button
        bool Auto2Flg = PlayerPrefs.GetBool("Auto2Flg");
        if (Auto2Flg) {
            GameObject.Find("AutoBtn").transform.Find("Num").GetComponent<Text>().text = "2";
            GameObject.Find("AutoBtn").GetComponent<AutoAttack>().speed = 2;
        }

        //EnemySameDaimyoNum
        string sameDaimyo = PlayerPrefs.GetString("sameDaimyo");
        string sameDaimyoNum = PlayerPrefs.GetString("sameDaimyoNum");
        PlayerPrefs.DeleteKey("sameDaimyo");
        PlayerPrefs.DeleteKey("sameDaimyoNum");

        char[] delimiterCharsNum = { ',' };
        if (sameDaimyo != null && sameDaimyo != "") {
            if (sameDaimyo.Contains(",")) {
                sameDaimyoList = new List<string>(sameDaimyo.Split(delimiterCharsNum));
                sameDaimyoNumList = new List<string>(sameDaimyoNum.Split(delimiterCharsNum));
            }else {
                sameDaimyoList.Add(sameDaimyo);
                sameDaimyoNumList.Add(sameDaimyoNum);
            }
        }

        //Dinamic Map
        activeKuniId = PlayerPrefs.GetInt("activeKuniId");
        activeStageId = PlayerPrefs.GetInt("activeStageId");
        GameObject wall = Instantiate(wallPrefab);
        wall.name = "wall";
        kaisenWeatherHandling(map);

        //Get Minus Status
        float rainMinusRatio = PlayerPrefs.GetFloat("rainMinusStatus", 0);
        float snowMinusRatio = PlayerPrefs.GetFloat("snowMinusStatus", 0);

        /*Player Setting*/
        int jinkei = PlayerPrefs.GetInt("jinkei", 0);
        List<int> myBusyoList = new List<int>();
        if (jinkei == 1) {
            soudaisyo = PlayerPrefs.GetInt("soudaisyo1");
            if (PlayerPrefs.HasKey("1map1")) {
                int mapId = 1;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("1map2")) {
                int mapId = 2;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("1map7")) {
                int mapId = 7;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("1map8")) {
                int mapId = 8;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("1map11")) {
                int mapId = 11;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("1map12")) {
                int mapId = 12;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId, rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("1map13")) {
                int mapId = 13;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId, rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("1map14")) {
                int mapId = 14;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("1map17")) {
                int mapId = 17;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("1map18")) {
                int mapId = 18;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId, rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("1map21")) {
                int mapId = 21;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("1map22")) {
                int mapId = 22;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId, rainMinusRatio, snowMinusRatio));
            }
        }else if (jinkei == 2) {
            soudaisyo = PlayerPrefs.GetInt("soudaisyo2");

            if (PlayerPrefs.HasKey("2map3")) {
                int mapId = 3;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId, rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("2map4")) {
                int mapId = 4;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("2map5")) {
                int mapId = 5;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("2map7")) {
                int mapId = 7;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("2map8")) {
                int mapId = 8;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId, rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("2map11")) {
                int mapId = 11;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("2map12")) {
                int mapId = 12;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("2map17")) {
                int mapId = 17;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("2map18")) {
                int mapId = 18;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("2map23")) {
                int mapId = 23;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId, rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("2map24")) {
                int mapId = 24;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("2map25")) {
                int mapId = 25;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }
        }else if (jinkei == 3) {
            soudaisyo = PlayerPrefs.GetInt("soudaisyo3");

            if (PlayerPrefs.HasKey("3map3")) {
                int mapId = 3;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("3map7")) {
                int mapId = 7;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("3map8")) {
                int mapId = 8;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("3map9")) {
                int mapId = 9;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId, rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("3map11")) {
                int mapId = 11;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("3map12")) {
                int mapId = 12;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("3map14")) {
                int mapId = 14;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("3map15")) {
                int mapId = 15;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("3map16")) {
                int mapId = 16;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("3map20")) {
                int mapId = 20;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("3map21")) {
                int mapId = 21;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("3map25")) {
                int mapId = 25;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }
        }else if (jinkei == 4) {
            soudaisyo = PlayerPrefs.GetInt("soudaisyo4");

            if (PlayerPrefs.HasKey("4map1")) {
                int mapId = 1;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("4map2")) {
                int mapId = 2;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("4map7")) {
                int mapId = 7;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("4map8")) {
                int mapId = 8;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId, rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("4map12")) {
                int mapId = 12;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId, rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("4map13")) {
                int mapId = 13;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("4map14")) {
                int mapId = 14;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("4map18")) {
                int mapId = 18;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId, rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("4map19")) {
                int mapId = 19;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("4map20")) {
                int mapId = 20;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("4map24")) {
                int mapId = 24;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId,  rainMinusRatio, snowMinusRatio));
            }if (PlayerPrefs.HasKey("4map25")) {
                int mapId = 25;
                myBusyoList.Add(getStsAndMakeInstance(jinkei, mapId, rainMinusRatio, snowMinusRatio));
            }
        }

        //Saku
        BusyoInfoGet info = new BusyoInfoGet();
        StatusGet sts = new StatusGet();
        GameObject content = GameObject.Find("Content").gameObject;
        string slotPath = "Prefabs/Saku/Slot";
        Saku saku = new Saku();

        foreach (Transform n in content.transform) {
            GameObject.Destroy(n.gameObject);
        }
        
        foreach (int busyoId in myBusyoList) {
           

            List<string> sakuList = new List<string>();
            sakuList = saku.getSakuInfo(busyoId);
            
            if(saku.getSakuShipFlg(int.Parse(sakuList[0]))) {
                GameObject slot = Instantiate(Resources.Load(slotPath)) as GameObject;
                string sakuPath = "Prefabs/Saku/saku" + sakuList[0];
                GameObject sakuIcon = Instantiate(Resources.Load(sakuPath)) as GameObject;
                sakuIcon.transform.SetParent(slot.transform);
                sakuIcon.transform.localScale = new Vector2(0.45f, 0.45f);
                sakuIcon.GetComponent<Button>().enabled = false;

                slot.transform.SetParent(content.transform);
                slot.transform.localScale = new Vector2(1, 1);

                slot.GetComponent<Saku>().sakuId = int.Parse(sakuList[0]);
                totalSakuLv = totalSakuLv + int.Parse(sakuList[3]);
                slot.GetComponent<Saku>().sakuEffect = int.Parse(sakuList[4]);

                if (sakuList[0] == "3") {
                    //hukuhei
                    //Heisyu
                    slot.GetComponent<Saku>().sakuHeisyu = info.getHeisyu(busyoId);
                    //Hei Status
                    string heiId = "hei" + busyoId.ToString();
                    string chParam = PlayerPrefs.GetString(heiId, "0");
                    if (chParam == "0" || chParam == "") {
                        StatusGet statusScript = new StatusGet();
                        string chParamHeisyu = statusScript.getHeisyu(busyoId);
                        chParam = chParamHeisyu + ":1:1:1";
                        PlayerPrefs.SetString(heiId, chParam);
                        PlayerPrefs.Flush();
                    }

                    char[] delimiterChars = { ':' };
                    string[] ch_list = chParam.Split(delimiterChars);
                    slot.GetComponent<Saku>().sakuHeiSts = float.Parse(ch_list[3]);
                    slot.GetComponent<Saku>().sakuBusyoId = busyoId;

                    //Busyo Speed
                    int sakuBusyoLv = PlayerPrefs.GetInt(busyoId.ToString());
                    float adjSpd = (float)sts.getSpd(busyoId, sakuBusyoLv) / 10;
                    slot.GetComponent<Saku>().sakuBusyoSpeed = adjSpd;
                }
            }
        }
        aveSakuLv = totalSakuLv / myBusyoList.Count;

        //Nanban
        string nanbanString = PlayerPrefs.GetString("nanbanItem");
        List<string> nanbanList = new List<string>();
        char[] delimiterChars3 = { ',' };
        nanbanList = new List<string>(nanbanString.Split(delimiterChars3));

        for (int i = 0; i < nanbanList.Count; i++) {
            int qty = int.Parse(nanbanList[i]);
            if (qty != 0) {
                if(i==0 || i==1) {
                    GameObject slot = Instantiate(Resources.Load(slotPath)) as GameObject;

                    string nanbanPath = "";
                    if (i == 0) {
                        nanbanPath = "Prefabs/Saku/saku8";
                    }
                    else if (i == 1) {
                        nanbanPath = "Prefabs/Saku/saku9";
                    }

                    GameObject sakuIcon = Instantiate(Resources.Load(nanbanPath)) as GameObject;
                    sakuIcon.transform.SetParent(slot.transform);
                    sakuIcon.transform.localScale = new Vector2(0.45f, 0.45f);
                    sakuIcon.GetComponent<Button>().enabled = false;

                    slot.transform.SetParent(content.transform);
                    slot.transform.localScale = new Vector2(1, 1);

                    if (i == 0) {
                        slot.GetComponent<Saku>().sakuId = 8;
                    }
                    else if (i == 1) {
                        slot.GetComponent<Saku>().sakuId = 9;
                    }
                    else if (i == 2) {
                        slot.GetComponent<Saku>().sakuId = 10;
                    }

                    int temp = i + 1;
                    ItemInfo item = new ItemInfo();
                    string itemCd = "nanban" + temp.ToString();
                    int effect = item.getItemEffect(itemCd);
                    slot.GetComponent<Saku>().sakuEffect = effect;
                }
            }
        }


        /*エネミー配置*/
        int AIType = 1;
        if (getRandomBool()) AIType = 4;

        int linkNo = PlayerPrefs.GetInt("activeLink", 0);
        enemySoudaisyo = PlayerPrefs.GetInt("enemySoudaisyo");
        
        if (PlayerPrefs.HasKey("emap1")) {
            int mapId = 1;
            getEnemyStsAndMakeInstance(linkNo, mapId, rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap2")) {
            int mapId = 2;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap3")) {
            int mapId = 3;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap4")) {
            int mapId = 4;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap5")) {
            int mapId = 5;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap6")) {
            int mapId = 6;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap7")) {
            int mapId = 7;
            getEnemyStsAndMakeInstance(linkNo, mapId, rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap8")) {
            int mapId = 8;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap9")) {
            int mapId = 9;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap10")) {
            int mapId = 10;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap11")) {
            int mapId = 11;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap12")) {
            int mapId = 12;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap13")) {
            int mapId = 13;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap14")) {
            int mapId = 14;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap15")) {
            int mapId = 15;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap16")) {
            int mapId = 16;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap17")) {
            int mapId = 17;
            getEnemyStsAndMakeInstance(linkNo, mapId, rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap18")) {
            int mapId = 18;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap19")) {
            int mapId = 19;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap20")) {
            int mapId = 20;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap21")) {
            int mapId = 21;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap22")) {
            int mapId = 22;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap23")) {
            int mapId = 23;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap24")) {
            int mapId = 24;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }if (PlayerPrefs.HasKey("emap25")) {
            int mapId = 25;
            getEnemyStsAndMakeInstance(linkNo, mapId,  rainMinusRatio, snowMinusRatio, AIType);
        }

        GameObject.Find("timer").GetComponent<Timer>().EnemySakuList = EnemySakuList;

        /*Dynamic Enemy Setting Finish*/
        //合戦開始エフェクト
        string pathBack = "Prefabs/PreKassen/backGround";
        GameObject back = Instantiate(Resources.Load(pathBack)) as GameObject;
        back.transform.localScale = new Vector2(30, 15);

        string pathLight = "Prefabs/PreKassen/lightning";
        GameObject light = Instantiate(Resources.Load(pathLight)) as GameObject;
        light.transform.localScale = new Vector2(10, 10);
        
    }

    void Update() {
        //Saku

        //On
        if (sakuFlg) {
            Vector2 TouchPosition = AppUtil.GetTouchPosition();
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
                    GameObject saku = Instantiate(Resources.Load(sakuPath)) as GameObject;
                    Vector2 worldPos = Camera.main.ScreenToWorldPoint(TouchPosition);
                    RectTransform sakuTransform = saku.GetComponent<RectTransform>();
                    sakuTransform.anchoredPosition = new Vector2(worldPos.x, worldPos.y);

                    if (saku.GetComponent<SakuCollider>() != null) {
                        saku.GetComponent<SakuCollider>().sakuId = sakuId;
                        saku.GetComponent<SakuCollider>().sakuEffect = sakuEffect;

                        if (sakuId == 8) {
                            GameScene gameSceneScript = new GameScene();
                            gameSceneScript.reduceNanbanItem();
                        }else if(sakuId==9) {
                            saku.GetComponent<SakuCollider>().vect = worldPos;

                        }

                    }else {
                        saku.GetComponent<SakuMaker>().sakuId = sakuId;
                        saku.GetComponent<SakuMaker>().sakuEffect = sakuEffect;
                        saku.GetComponent<SakuMaker>().vect = worldPos;
                        saku.GetComponent<SakuMaker>().sakuHeisyu = sakuHeisyu;
                        saku.GetComponent<SakuMaker>().sakuHeiSts = sakuHeiSts;
                        saku.GetComponent<SakuMaker>().sakuBusyoId = sakuBusyoId;
                        saku.GetComponent<SakuMaker>().sakuBusyoSpeed = sakuBusyoSpeed;
                    }

                    Destroy(sakuBtn);
                }
                else {
                    Debug.Log("Menu Overapp");
                }
            }
        }


    }









    IEnumerator taikoMusic() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[9].Play(); //horagai
        audioSources[12].Play(); //bgm

        audioSources[5].Play();
        yield return new WaitForSeconds(0.5f);
        audioSources[5].Play();
        yield return new WaitForSeconds(0.5f);
        audioSources[5].Play();
        yield return new WaitForSeconds(0.5f);
        audioSources[5].Play();
    }


    public int getStsAndMakeInstance(int jinkei, int mapId, float rainMinusRatio, float snowMinusRatio) {

        //Get Status
        string map = jinkei.ToString() + "map" + mapId;
        int busyoId = PlayerPrefs.GetInt(map);

        string busyoString = busyoId.ToString();
        int lv = PlayerPrefs.GetInt(busyoString);

        StatusGet sts = new StatusGet();
        BusyoInfoGet busyoInfo = new BusyoInfoGet();
        int shipId = busyoInfo.getShipId(busyoId);
        int hp = sts.getHp(busyoId, lv);
        int atk = sts.getAtk(busyoId, lv);
        int dfc = sts.getDfc(busyoId, lv);
        int spd = sts.getSpd(busyoId, lv);
        string busyoName = sts.getBusyoName(busyoId);
        ArrayList senpouArray = sts.getSenpou(busyoId,false);

        //Make Average Senpou Lv
        totalSenpouLv = totalSenpouLv + (int)senpouArray[8];

        //Weather Minus
        if (rainMinusRatio != 0 || snowMinusRatio != 0) {
            float tmp = (float)spd * 0.5f;
            if (tmp < 1) {
                tmp = 1;
            }
            spd = Mathf.FloorToInt(tmp);
        }

        //Soudaisyo
        if (busyoId == soudaisyo) {
            soudaisyoHp = hp;
            soudaisyoAtk = atk;
            soudaisyoDfc = dfc;
            soudaisyoSpd = spd / 10;
        }

        //Boubi
        int boubi = 0;
        if (activeStageId == 0) {
            //Passive
            boubi = PlayerPrefs.GetInt("activeBoubi", 0);
        }
        
        //View Object & pass status to it. 
        PlayerInstance inst = new PlayerInstance();
        inst.makeKaisenInstance(busyoId, shipId, mapId, hp, atk, dfc, spd, senpouArray, busyoName, soudaisyo, boubi, false,0,0);

        return busyoId;
    }

    public void getEnemyStsAndMakeInstance(int linkNo, int mapId,  float rainMinusRatio, float snowMinusRatio, int AIType) {

        string map = "emap" + mapId;
        int busyoId = PlayerPrefs.GetInt(map);

        int activeBusyoLv = PlayerPrefs.GetInt("activeBusyoLv");
        int activeButaiQty = PlayerPrefs.GetInt("activeButaiQty");
        int activeButaiLv = PlayerPrefs.GetInt("activeButaiLv");

        StatusGet sts = new StatusGet();
        BusyoInfoGet info = new BusyoInfoGet();
        Saku saku = new Saku();
        EnemyInstance inst = new EnemyInstance();
        int shipId = info.getShipId(busyoId);
        int hp = sts.getHp(busyoId, activeBusyoLv);
        int atk = sts.getAtk(busyoId, activeBusyoLv);
        int dfc = sts.getDfc(busyoId, activeBusyoLv);
        int spd = sts.getSpd(busyoId, activeBusyoLv);
        string busyoName = sts.getBusyoName(busyoId);
        string heisyu = sts.getHeisyu(busyoId);
        int sakuId = info.getSakuId(busyoId);
        if (saku.getSakuShipFlg(sakuId)) {
            int childStatus = inst.getChildStatus(activeButaiLv, heisyu, 0);
            EnemySakuList.Add(saku.getEnemySaku(sakuId, aveSakuLv, busyoId, heisyu, childStatus, spd));
        }
        int playerBusyoQty = PlayerPrefs.GetInt("jinkeiBusyoQty");
        aveSenpouLv = Mathf.CeilToInt(totalSenpouLv / playerBusyoQty);
        ArrayList senpouArray = sts.getEnemySenpou(busyoId, aveSenpouLv, "");

        //Weather Minus
        if (rainMinusRatio != 0 || snowMinusRatio != 0) {
            float tmp = (float)spd * 0.5f;
            if (tmp < 1) {
                tmp = 1;
            }
            spd = Mathf.FloorToInt(tmp);
        }
        
        bool enemyTaisyoFlg = false;
        if (busyoId == enemySoudaisyo) {
            enemyTaisyoFlg = true;
        }

        //same daimyo  
        BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();
        int senarioId = PlayerPrefs.GetInt("senarioId");
        int daimyoId = BusyoInfoGet.getDaimyoId(busyoId,senarioId);
        int num = 0;
        if (sameDaimyoList.Contains(daimyoId.ToString())) {
            int i = sameDaimyoList.IndexOf(daimyoId.ToString());
            num = int.Parse(sameDaimyoNumList[i]);
            int addRatio = (num - 1) * 5;
            atk = atk + Mathf.FloorToInt(((float)atk * (float)addRatio) / 100);
            dfc = dfc + Mathf.FloorToInt(((float)dfc * (float)addRatio) / 100);
        }
        
        //View Object & pass status to it.
        inst.makeKaisenInstance(mapId, busyoId, shipId,activeButaiLv, heisyu, activeButaiQty, hp, atk, dfc, spd, busyoName, linkNo, enemyTaisyoFlg, senpouArray,AIType);
    }

    public void kaisenWeatherHandling(GameObject map) {
        Color rainSnowColor = new Color(140f / 255f, 140f / 255f, 140f / 255f, 255f / 255f);

        //Dinamic Weather
        int weatherId = PlayerPrefs.GetInt("weather");

        if (weatherId != 1) {
            if (weatherId == 2) {
                string particlePath = "Prefabs/PreKassen/particle/RainParticle";
                GameObject rain = Instantiate(Resources.Load(particlePath)) as GameObject;
                rain.transform.SetParent(GameObject.Find("Canvas").transform);
                rain.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                rain.transform.localPosition = new Vector3(0, 220, 0);
            }
            else if (weatherId == 3) {
                string particlePath = "Prefabs/PreKassen/particle/SnowParticle";
                GameObject snow = Instantiate(Resources.Load(particlePath)) as GameObject;
                snow.transform.SetParent(GameObject.Find("Canvas").transform);
                snow.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                snow.transform.localPosition = new Vector3(0, 220, 0);
            }

            GameObject mapMid = map.transform.Find("map mid").gameObject;
            GameObject mapRight = map.transform.Find("map right").gameObject;
            GameObject mapLeft = map.transform.Find("map left").gameObject;

            foreach (Transform a in mapMid.transform) {
                foreach (Transform b in a) {
                    b.GetComponent<SpriteRenderer>().color = rainSnowColor;
                }
            }
            foreach (Transform a in mapRight.transform) {
                foreach (Transform b in a) {
                    b.GetComponent<SpriteRenderer>().color = rainSnowColor;
                }
            }
            foreach (Transform a in mapLeft.transform) {
                foreach (Transform b in a) {
                    b.GetComponent<SpriteRenderer>().color = rainSnowColor;
                }
            }

        }

    }

    public static bool getRandomBool() {
        return UnityEngine.Random.Range(0, 2) == 0;
    }

}

