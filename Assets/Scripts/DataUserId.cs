using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using NCMB;

public class DataUserId : MonoBehaviour {

    public bool RegisteredFlg = false;

    int kuniLv = 0;
    int kuniExp = 0;
    int myDaimyo = 0;
    bool addJinkei1 = false;
    bool addJinkei2 = false;
    bool addJinkei3 = false;
    bool addJinkei4 = false;
    string yearSeason = "";
    string seiryoku = "";
    int money = 0;
    int busyoDama = 0;
    int syogunDaimyoId = 0;
    string doumei = "";
    List<int> questSpecialFlgId = new List<int>();
    List<bool> questSpecialReceivedFlgId = new List<bool>();
    List<bool> questSpecialCountReceivedFlg = new List<bool>();
    string myBusyo = "";
    string gacyaDaimyoHst = "";
    List<string> myBusyoList = new List<string>();
    List<int> lvList = new List<int>();
    List<string> heiList = new List<string>();
    List<int> senpouLvList = new List<int>();
    List<int> sakuLvList = new List<int>();
    List<string> kahouList = new List<string>();
    List<int> addLvList = new List<int>();
    List<int> gokuiList = new List<int>();
    List<int> kanniList = new List<int>();
    int movieCount = 0;
    int space = 0;

    string myKanni = "";
    string availableBugu = "";
    string availableKabuto = "";
    string availableGusoku = "";
    string availableMeiba = "";
    string availableCyadougu = "";
    string availableHeihousyo = "";
    string availableChishikisyo = "";
    string kanjyo = "";
    string cyouheiYR = "";
    string cyouheiKB = "";
    string cyouheiTP = "";
    string cyouheiYM = "";
    int hidensyoGe = 0;
    int hidensyoCyu = 0;
    int hidensyoJyo = 0;
    int shinobiGe = 0;
    int shinobiCyu = 0;
    int shinobiJyo = 0;
    string kengouItem = "";
    string gokuiItem = "";
    string nanbanItem = "";
    int transferTP = 0;
    int transferKB = 0;
    int meisei = 0;
    string shiro = "";
    string koueki = "";
    string cyoutei = "";

    //zukan
    string zukanBusyoHst = "";
    string zukanBuguHst = "";
    string zukanGusokuHst = "";
    string zukanKabutoHst = "";
    string zukanMeibaHst = "";
    string zukanCyadouguHst = "";
    string zukanChishikisyoHst = "";
    string zukanHeihousyoHst = "";
    string gameClearDaimyo = "";
    string gameClearDaimyoHard = "";

    //naisei
    List<int> naiseiKuniList = new List<int>();
    List<string> naiseiList = new List<string>();
    List<int> naiseiShiroList = new List<int>();

    public void InsertUserId (string userId) {
        getAllData();
        NCMBObject userIdClass = new NCMBObject("dataStore");
        System.DateTime now = System.DateTime.Now;

        userIdClass["userId"] = userId;
        userIdClass["loginDate"] = now.ToString();
        userIdClass["platform"] = SystemInfo.operatingSystem;
        userIdClass["appVer"] = Application.version;
        userIdClass["kuniLv"] = kuniLv;
        userIdClass["kuniExp"] = kuniExp;
        userIdClass["myDaimyo"] = myDaimyo;
        userIdClass["addJinkei1"] = addJinkei1;
        userIdClass["addJinkei2"] = addJinkei2;
        userIdClass["addJinkei3"] = addJinkei3;
        userIdClass["addJinkei4"] = addJinkei4;

        /**Data Store**/
        //basic
        userIdClass["seiryoku"] = seiryoku;
        userIdClass["money"] = money;
        userIdClass["busyoDama"] = busyoDama;
        userIdClass["syogunDaimyoId"] = syogunDaimyoId;
        userIdClass["doumei"] = doumei;
        userIdClass["questSpecialFlgId"] = questSpecialFlgId;
        userIdClass["questSpecialReceivedFlgId"] = questSpecialReceivedFlgId;
        userIdClass["questSpecialCountReceivedFlg"] = questSpecialCountReceivedFlg;
        userIdClass["yearSeason"] = yearSeason;
        userIdClass["movieCount"] = movieCount;
        userIdClass["space"] = space;

        //busyo
        userIdClass["gacyaDaimyoHst"] = gacyaDaimyoHst;
        userIdClass["myBusyoList"] = myBusyoList;
        userIdClass["lvList"] = lvList;
        userIdClass["heiList"] = heiList;
        userIdClass["senpouLvList"] = senpouLvList;
        userIdClass["sakuLvList"] = sakuLvList;
        userIdClass["kahouList"] = kahouList;
        userIdClass["addLvList"] = addLvList;
        userIdClass["gokuiList"] = gokuiList;
        userIdClass["kanniList"] = kanniList;

        //kaho
        userIdClass["availableBugu"] = availableBugu;
        userIdClass["availableKabuto"] = availableKabuto;
        userIdClass["availableGusoku"] = availableGusoku;
        userIdClass["availableMeiba"] = availableMeiba;
        userIdClass["availableCyadougu"] = availableCyadougu;
        userIdClass["availableHeihousyo"] = availableHeihousyo;
        userIdClass["availableChishikisyo"] = availableChishikisyo;

        //item
        userIdClass["myKanni"] = myKanni;
        userIdClass["kanjyo"] = kanjyo;
        userIdClass["cyouheiYR"] = cyouheiYR;
        userIdClass["cyouheiKB"] = cyouheiKB;
        userIdClass["cyouheiTP"] = cyouheiTP;
        userIdClass["cyouheiYM"] = cyouheiYM;
        userIdClass["hidensyoGe"] = hidensyoGe;
        userIdClass["hidensyoCyu"] = hidensyoCyu;
        userIdClass["hidensyoJyo"] = hidensyoJyo;
        userIdClass["shinobiGe"] = shinobiGe;
        userIdClass["shinobiCyu"] = shinobiCyu;
        userIdClass["shinobiJyo"] = shinobiJyo;
        userIdClass["kengouItem"] = kengouItem;
        userIdClass["gokuiItem"] = gokuiItem;
        userIdClass["nanbanItem"] = nanbanItem;
        userIdClass["transferTP"] = transferTP;
        userIdClass["transferKB"] = transferKB;
        userIdClass["meisei"] = meisei;
        userIdClass["shiro"] = shiro;
        userIdClass["koueki"] = koueki;
        userIdClass["cyoutei"] = cyoutei;

        //zukan
        userIdClass["zukanBusyoHst"] = zukanBusyoHst;
        userIdClass["zukanBuguHst"] = zukanBuguHst;
        userIdClass["zukanGusokuHst"] = zukanGusokuHst;
        userIdClass["zukanKabutoHst"] = zukanKabutoHst;
        userIdClass["zukanMeibaHst"] = zukanMeibaHst;
        userIdClass["zukanCyadouguHst"] = zukanCyadouguHst;
        userIdClass["zukanChishikisyoHst"] = zukanChishikisyoHst;
        userIdClass["zukanHeihousyoHst"] = zukanHeihousyoHst;
        userIdClass["gameClearDaimyo"] = gameClearDaimyo;
        userIdClass["gameClearDaimyoHard"] = gameClearDaimyoHard;

        //naisei
        userIdClass["naiseiKuniList"] = naiseiKuniList;
        userIdClass["naiseiList"] = naiseiList;
        userIdClass["naiseiShiroList"] = naiseiShiroList;

        userIdClass.SaveAsync();
        RegisteredFlg = true;
    }

    public void UpdateUserId(string userId) {

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("dataStore");
        query.WhereEqualTo("userId", userId);

        getAllData();

        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                if (objList.Count == 0) { //never registered
                    InsertUserId(userId);
                }else { //registered
                    string loginDate = System.Convert.ToString(objList[0]["loginDate"]);
                    System.DateTime now = System.DateTime.Now;
                    if (now.ToString() != loginDate) {
                        objList[0]["loginDate"] = now.ToString();
                        objList[0]["platform"] = SystemInfo.operatingSystem;
                        objList[0]["appVer"] = Application.version;
                        objList[0]["kuniLv"] = kuniLv;
                        objList[0]["kuniExp"] = kuniExp;
                        objList[0]["myDaimyo"] = myDaimyo;
                        objList[0]["addJinkei1"] = addJinkei1;
                        objList[0]["addJinkei2"] = addJinkei2;
                        objList[0]["addJinkei3"] = addJinkei3;
                        objList[0]["addJinkei4"] = addJinkei4;

                        /**Data Store**/
                        //basic
                        objList[0]["seiryoku"] = seiryoku;
                        objList[0]["money"] = money;
                        objList[0]["busyoDama"] = busyoDama;
                        objList[0]["syogunDaimyoId"] = syogunDaimyoId;
                        objList[0]["doumei"] = doumei;
                        objList[0]["questSpecialFlgId"] = questSpecialFlgId;
                        objList[0]["questSpecialReceivedFlgId"] = questSpecialReceivedFlgId;
                        objList[0]["questSpecialCountReceivedFlg"] = questSpecialCountReceivedFlg;
                        objList[0]["yearSeason"] = yearSeason;                        
                        objList[0]["movieCount"] = movieCount;
                        objList[0]["space"] = space;

                        //busyo
                        objList[0]["gacyaDaimyoHst"] = gacyaDaimyoHst;
                        objList[0]["myBusyoList"] = myBusyoList;
                        objList[0]["lvList"] = lvList;
                        objList[0]["heiList"] = heiList;
                        objList[0]["senpouLvList"] = senpouLvList;
                        objList[0]["sakuLvList"] = sakuLvList;
                        objList[0]["kahouList"] = kahouList;
                        objList[0]["addLvList"] = addLvList;
                        objList[0]["gokuiList"] = gokuiList;
                        objList[0]["kanniList"] = kanniList;

                        //kaho
                        objList[0]["availableBugu"] = availableBugu;
                        objList[0]["availableKabuto"] = availableKabuto;
                        objList[0]["availableGusoku"] = availableGusoku;
                        objList[0]["availableMeiba"] = availableMeiba;
                        objList[0]["availableCyadougu"] = availableCyadougu;
                        objList[0]["availableHeihousyo"] = availableHeihousyo;
                        objList[0]["availableChishikisyo"] = availableChishikisyo;

                        //item
                        objList[0]["myKanni"] = myKanni;
                        objList[0]["kanjyo"] = kanjyo;
                        objList[0]["cyouheiYR"] = cyouheiYR;
                        objList[0]["cyouheiKB"] = cyouheiKB;
                        objList[0]["cyouheiTP"] = cyouheiTP;
                        objList[0]["cyouheiYM"] = cyouheiYM;
                        objList[0]["hidensyoGe"] = hidensyoGe;
                        objList[0]["hidensyoCyu"] = hidensyoCyu;
                        objList[0]["hidensyoJyo"] = hidensyoJyo;
                        objList[0]["shinobiGe"] = shinobiGe;
                        objList[0]["shinobiCyu"] = shinobiCyu;
                        objList[0]["shinobiJyo"] = shinobiJyo;
                        objList[0]["kengouItem"] = kengouItem;
                        objList[0]["gokuiItem"] = gokuiItem;
                        objList[0]["nanbanItem"] = nanbanItem;
                        objList[0]["transferTP"] = transferTP;
                        objList[0]["transferKB"] = transferKB;
                        objList[0]["meisei"] = meisei;
                        objList[0]["shiro"] = shiro;
                        objList[0]["koueki"] = koueki;
                        objList[0]["cyoutei"] = cyoutei;

                        //zukan
                        objList[0]["zukanBusyoHst"] = zukanBusyoHst;
                        objList[0]["zukanBuguHst"] = zukanBuguHst;
                        objList[0]["zukanGusokuHst"] = zukanGusokuHst;
                        objList[0]["zukanKabutoHst"] = zukanKabutoHst;
                        objList[0]["zukanMeibaHst"] = zukanMeibaHst;
                        objList[0]["zukanCyadouguHst"] = zukanCyadouguHst;
                        objList[0]["zukanChishikisyoHst"] = zukanChishikisyoHst;
                        objList[0]["zukanHeihousyoHst"] = zukanHeihousyoHst;
                        objList[0]["gameClearDaimyo"] = gameClearDaimyo;
                        objList[0]["gameClearDaimyoHard"] = gameClearDaimyoHard;

                        //naisei
                        objList[0]["naiseiKuniList"] = naiseiKuniList;
                        objList[0]["naiseiList"] = naiseiList;
                        objList[0]["naiseiShiroList"] = naiseiShiroList;

                        objList[0].SaveAsync();
                        RegisteredFlg = true;
                    }
                }
            }
        });
    }

    void getAllData() {

        kuniLv = PlayerPrefs.GetInt("kuniLv");
        if (kuniLv <= 0) {
            kuniLv = 1;
            PlayerPrefs.SetInt("kuniLv", kuniLv);
        }
        kuniExp = PlayerPrefs.GetInt("kuniExp");
        myDaimyo = PlayerPrefs.GetInt("myDaimyo");
        addJinkei1 = PlayerPrefs.GetBool("addJinkei1");
        addJinkei2 = PlayerPrefs.GetBool("addJinkei2");
        addJinkei3 = PlayerPrefs.GetBool("addJinkei3");
        addJinkei4 = PlayerPrefs.GetBool("addJinkei4");

        //common
        yearSeason = PlayerPrefs.GetString("yearSeason");
        seiryoku = PlayerPrefs.GetString("seiryoku");
        money = PlayerPrefs.GetInt("money");
        busyoDama = PlayerPrefs.GetInt("busyoDama");
        syogunDaimyoId = PlayerPrefs.GetInt("syogunDaimyoId");
        doumei = PlayerPrefs.GetString("doumei");
        movieCount = PlayerPrefs.GetInt("movieCount");
        space = PlayerPrefs.GetInt("space");

        Entity_quest_mst questMst = Resources.Load("Data/quest_mst") as Entity_quest_mst;
        for (int i = 0; i < questMst.param.Count; i++) {
            bool dailyFlg = questMst.param[i].daily;
            if (!dailyFlg) {
                string tmp = "questSpecialFlg" + i.ToString();
                bool activeFlg = PlayerPrefs.GetBool(tmp, false);
                if (activeFlg) {
                    questSpecialFlgId.Add(i);
                    string tmp2 = "questSpecialReceivedFlg" + i.ToString();
                    bool activeFlg2 = PlayerPrefs.GetBool(tmp2, false);
                    if (!activeFlg2) {
                        questSpecialReceivedFlgId.Add(false);
                    }
                    else {
                        questSpecialReceivedFlgId.Add(true);
                    }
                }
            }
        }
        Entity_quest_count_mst questCountMst = Resources.Load("Data/quest_count_mst") as Entity_quest_count_mst;
        for (int i = 0; i < questCountMst.param.Count; i++) {
            bool dailyFlg = questCountMst.param[i].daily;
            if (!dailyFlg) {
                string tmp = "questSpecialCountReceivedFlg" + i.ToString();
                bool activeFlg = PlayerPrefs.GetBool(tmp, false);
                if (activeFlg) {
                    questSpecialCountReceivedFlg.Add(true);                    
                }else {
                    questSpecialCountReceivedFlg.Add(false);
                }
            }
        }


        //busyo
        myBusyo = PlayerPrefs.GetString("myBusyo");
        gacyaDaimyoHst = PlayerPrefs.GetString("gacyaDaimyoHst");

        char[] delimiterChars = { ',' };
        if (myBusyo.Contains(",")) {
            myBusyoList = new List<string>(myBusyo.Split(delimiterChars));
        }else {
            myBusyoList.Add(myBusyo);
        }

        for (int i = 0; i < myBusyoList.Count; i++) {
            string busyoId = myBusyoList[i];

            int busyoLv = PlayerPrefs.GetInt(busyoId);
            lvList.Add(busyoLv);

            string heiTmp = "hei" + busyoId;
            string heiString = PlayerPrefs.GetString(heiTmp);
            heiList.Add(heiString);

            string senpouTmp = "senpou" + busyoId;
            int senpouLv = PlayerPrefs.GetInt(senpouTmp);
            senpouLvList.Add(senpouLv);

            string sakuTmp = "saku" + busyoId;
            int sakuLv = PlayerPrefs.GetInt(sakuTmp);
            sakuLvList.Add(sakuLv);

            string kahouTmp = "kahou" + busyoId;
            string kahouString = PlayerPrefs.GetString(kahouTmp);
            kahouList.Add(kahouString);

            //addLevel
            string addLvTmp = "addlv" + busyoId.ToString();
            int addlv = PlayerPrefs.GetInt(addLvTmp);
            addLvList.Add(addlv);

            //gokui
            string gokuiTmp = "gokui" + busyoId;
            int gokuiId = PlayerPrefs.GetInt(gokuiTmp);
            gokuiList.Add(gokuiId);

            //kanni
            string kanniTmp = "kanni" + busyoId;
            int kanniId = PlayerPrefs.GetInt(kanniTmp);
            kanniList.Add(kanniId);
            
        }

        //item
        myKanni = PlayerPrefs.GetString("myKanni");
        availableBugu = PlayerPrefs.GetString("availableBugu");
        availableKabuto = PlayerPrefs.GetString("availableKabuto");
        availableGusoku = PlayerPrefs.GetString("availableGusoku");
        availableMeiba = PlayerPrefs.GetString("availableMeiba");
        availableCyadougu = PlayerPrefs.GetString("availableCyadougu");
        availableHeihousyo = PlayerPrefs.GetString("availableHeihousyo");
        availableChishikisyo = PlayerPrefs.GetString("availableChishikisyo");
        kanjyo = PlayerPrefs.GetString("kanjyo");
        cyouheiYR = PlayerPrefs.GetString("cyouheiYR");
        cyouheiKB = PlayerPrefs.GetString("cyouheiKB");
        cyouheiTP = PlayerPrefs.GetString("cyouheiTP");
        cyouheiYM = PlayerPrefs.GetString("cyouheiYM");
        hidensyoGe = PlayerPrefs.GetInt("hidensyoGe");
        hidensyoCyu = PlayerPrefs.GetInt("hidensyoCyu");
        hidensyoJyo = PlayerPrefs.GetInt("hidensyoJyo");
        shinobiGe = PlayerPrefs.GetInt("shinobiGe");
        shinobiCyu = PlayerPrefs.GetInt("shinobiCyu");
        shinobiJyo = PlayerPrefs.GetInt("shinobiJyo");
        kengouItem = PlayerPrefs.GetString("kengouItem");
        gokuiItem = PlayerPrefs.GetString("gokuiItem");
        nanbanItem = PlayerPrefs.GetString("nanbanItem");
        transferTP = PlayerPrefs.GetInt("transferTP");
        transferKB = PlayerPrefs.GetInt("transferKB");
        meisei = PlayerPrefs.GetInt("meisei");
        shiro = PlayerPrefs.GetString("shiro");
        koueki = PlayerPrefs.GetString("koueki");
        cyoutei = PlayerPrefs.GetString("cyoutei");

        //zukan
        zukanBusyoHst = PlayerPrefs.GetString("zukanBusyoHst");
        zukanBuguHst = PlayerPrefs.GetString("zukanBuguHst");
        zukanGusokuHst = PlayerPrefs.GetString("zukanGusokuHst");
        zukanKabutoHst = PlayerPrefs.GetString("zukanKabutoHst");
        zukanMeibaHst = PlayerPrefs.GetString("zukanMeibaHst");
        zukanCyadouguHst = PlayerPrefs.GetString("zukanCyadouguHst");
        zukanChishikisyoHst = PlayerPrefs.GetString("zukanChishikisyoHst");
        zukanHeihousyoHst = PlayerPrefs.GetString("zukanHeihousyoHst");
        gameClearDaimyo = PlayerPrefs.GetString("gameClearDaimyo");
        gameClearDaimyoHard = PlayerPrefs.GetString("gameClearDaimyoHard");

        //naisei
        Entity_kuni_mst kuniMst = Resources.Load("Data/kuni_mst") as Entity_kuni_mst;
        for (int i = 0; i < kuniMst.param.Count; i++) {
            int kuniId = i + 1;
            string naiseiTmp = "naisei" + kuniId.ToString();
            if (PlayerPrefs.HasKey(naiseiTmp)) {
                naiseiKuniList.Add(kuniId);
                naiseiList.Add(PlayerPrefs.GetString(naiseiTmp));

                string shiroTmp = "shiro" + kuniId;
                int shiroId = PlayerPrefs.GetInt(shiroTmp, 0);
                naiseiShiroList.Add(shiroId);
            }
        }
    }

    
}
