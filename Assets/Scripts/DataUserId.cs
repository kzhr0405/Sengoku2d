using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using NCMB;

public class DataUserId : MonoBehaviour {

    public bool RegisteredFlg = false;

    public void InsertUserId (string userId) {
        NCMBObject userIdClass = new NCMBObject("dataStore");
        string platform = SystemInfo.operatingSystem;
        string appVer = Application.version;
        int kuniLv = PlayerPrefs.GetInt("kuniLv");
        int kuniExp = PlayerPrefs.GetInt("kuniExp");
        int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
        bool addJinkei1 = PlayerPrefs.GetBool("addJinkei1");
        bool addJinkei2 = PlayerPrefs.GetBool("addJinkei2");
        bool addJinkei3 = PlayerPrefs.GetBool("addJinkei3");
        bool addJinkei4 = PlayerPrefs.GetBool("addJinkei4");

        userIdClass["userId"] = userId;
        userIdClass["platform"] = platform;
        userIdClass["appVer"] = appVer;
        System.DateTime now = System.DateTime.Now;
        userIdClass["loginDate"] = now.ToString();
        userIdClass["kuniLv"] = kuniLv;
        userIdClass["kuniExp"] = kuniExp;
        userIdClass["myDaimyo"] = myDaimyo;
        userIdClass["addJinkei1"] = addJinkei1;
        userIdClass["addJinkei2"] = addJinkei2;
        userIdClass["addJinkei3"] = addJinkei3;
        userIdClass["addJinkei4"] = addJinkei4;

        userIdClass.SaveAsync();
        RegisteredFlg = true;
    }

    public void UpdateUserId(string userId) {

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("dataStore");
        query.WhereEqualTo("userId", userId);

        int kuniLv = PlayerPrefs.GetInt("kuniLv");
        int kuniExp = PlayerPrefs.GetInt("kuniExp");
        int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
        bool addJinkei1 = PlayerPrefs.GetBool("addJinkei1");
        bool addJinkei2 = PlayerPrefs.GetBool("addJinkei2");
        bool addJinkei3 = PlayerPrefs.GetBool("addJinkei3");
        bool addJinkei4 = PlayerPrefs.GetBool("addJinkei4");

        //common
        string yearSeason = PlayerPrefs.GetString("yearSeason");
        string seiryoku = PlayerPrefs.GetString("seiryoku");
        int money = PlayerPrefs.GetInt("money");
        int busyoDama = PlayerPrefs.GetInt("busyoDama");
        int syogunDaimyoId = PlayerPrefs.GetInt("syogunDaimyoId");
        string doumei = PlayerPrefs.GetString("doumei");
        Entity_quest_mst questMst = Resources.Load("Data/quest_mst") as Entity_quest_mst;
        List<int> questSpecialFlgId = new List<int>();
        List<bool> questSpecialReceivedFlgId = new List<bool>();

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
                    }else {
                        questSpecialReceivedFlgId.Add(true);
                    }
                }
            }            
        }

        //busyo
        string myBusyo = PlayerPrefs.GetString("myBusyo");
        string gacyaDaimyoHst = PlayerPrefs.GetString("gacyaDaimyoHst");
        List<string> myBusyoList = new List<string>();
        List<int> lvList = new List<int>();
        List<string> heiList = new List<string>();
        List<int> senpouLvList = new List<int>();
        List<int> sakuLvList = new List<int>();
        List<string> kahouList = new List<string>();
        List<int> addLvList = new List<int>();
        List<int> gokuiList = new List<int>();
        List<int> kanniList = new List<int>();

        char[] delimiterChars = { ',' };
        if (myBusyo.Contains(",")) {
            myBusyoList = new List<string>(myBusyo.Split(delimiterChars));
        }else {
            myBusyoList.Add(myBusyo);
        }
        for(int i=0; i< myBusyoList.Count; i++) {
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

            Debug.Log(gokuiId + "," + kanniId);

        }

        //item
        string myKanni = PlayerPrefs.GetString("myKanni");
        string availableBugu = PlayerPrefs.GetString("availableBugu");
        string availableKabuto = PlayerPrefs.GetString("availableKabuto");
        string availableGusoku = PlayerPrefs.GetString("availableGusoku");
        string availableMeiba = PlayerPrefs.GetString("availableMeiba");
        string availableCyadougu = PlayerPrefs.GetString("availableCyadougu");
        string availableHeihousyo = PlayerPrefs.GetString("availableHeihousyo");
        string availableChishikisyo = PlayerPrefs.GetString("availableChishikisyo");
        string kanjyo = PlayerPrefs.GetString("kanjyo");
        string cyouheiYR = PlayerPrefs.GetString("cyouheiYR");
        string cyouheiKB = PlayerPrefs.GetString("cyouheiKB");
        string CyouheiTP = PlayerPrefs.GetString("CyouheiTP");
        string CyouheiYM = PlayerPrefs.GetString("CyouheiYM");
        int hidensyoGe = PlayerPrefs.GetInt("hidensyoGe");
        int hidensyoCyu = PlayerPrefs.GetInt("hidensyoCyu");
        int hidensyoJyo = PlayerPrefs.GetInt("hidensyoJyo");
        int shinobiGe = PlayerPrefs.GetInt("shinobiGe");
        int shinobiCyu = PlayerPrefs.GetInt("shinobiCyu");
        int shinobiJyo = PlayerPrefs.GetInt("shinobiJyo");
        string kengouItem = PlayerPrefs.GetString("kengouItem");
        string gokuiItem = PlayerPrefs.GetString("gokuiItem");
        string nanbanItem = PlayerPrefs.GetString("nanbanItem");
        int transferTP = PlayerPrefs.GetInt("transferTP");
        int transferKB = PlayerPrefs.GetInt("transferKB");
        int meisei = PlayerPrefs.GetInt("meisei");
        string shiro = PlayerPrefs.GetString("shiro");
        string koueki = PlayerPrefs.GetString("koueki");
        string cyoutei = PlayerPrefs.GetString("cyoutei");

        //zukan
        string zukanBusyoHst = PlayerPrefs.GetString("zukanBusyoHst");
        string zukanBuguHst = PlayerPrefs.GetString("zukanBuguHst");
        string zukanGusokuHst = PlayerPrefs.GetString("zukanGusokuHst");
        string zukanKabutoHst = PlayerPrefs.GetString("zukanKabutoHst");
        string zukanMeibaHst = PlayerPrefs.GetString("zukanMeibaHst");
        string zukanCyadouguHst = PlayerPrefs.GetString("zukanCyadouguHst");
        string zukanChishikisyoHst = PlayerPrefs.GetString("zukanChishikisyoHst");
        string zukanHeihousyoHst = PlayerPrefs.GetString("zukanHeihousyoHst");
        string gameClearDaimyo = PlayerPrefs.GetString("gameClearDaimyo");
        string gameClearDaimyoHard = PlayerPrefs.GetString("gameClearDaimyoHard");

        //naisei
        Entity_kuni_mst kuniMst = Resources.Load("Data/kuni_mst") as Entity_kuni_mst;
        List<int> naiseiKuniList = new List<int>();
        List<string> naiseiList = new List<string>();
        List<int> naiseiShiroList = new List<int>();
        for (int i=0; i< kuniMst.param.Count; i++) {
            int kuniId = i + 1;
            string naiseiTmp = "naisei" + kuniId.ToString();
            if(PlayerPrefs.HasKey(naiseiTmp)) {
                naiseiKuniList.Add(kuniId);
                naiseiList.Add(PlayerPrefs.GetString(naiseiTmp));

                string shiroTmp = "shiro" + kuniId;
                int shiroId = PlayerPrefs.GetInt(shiroTmp,0);
                naiseiShiroList.Add(shiroId);
            }
        }

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
                        objList[0]["yearSeason"] = yearSeason;

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
                        objList[0]["CyouheiTP"] = CyouheiTP;
                        objList[0]["CyouheiYM"] = CyouheiYM;
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


}
