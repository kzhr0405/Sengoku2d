using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class DataRecoveryConfirm : MonoBehaviour {

    public GameObject back;
    public GameObject msg;
    

    //pvp jinkei
    public List<int> busyoList = new List<int>();
    public List<int> lvList = new List<int>();
    public List<string> heiList = new List<string>();
    public List<string> kahouList = new List<string>();
    public List<int> senpouLvList = new List<int>();
    public List<int> sakuLvList = new List<int>();
    public List<int> countSpecialList = new List<int>();
    public List<int> countDailyList = new List<int>();

    public void OnClick() {

        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();

        if (name == "YesButton") {
            audioSources[0].Play();
            RecoveryDataStore RecoveryDataStore = GameObject.Find("RecoveryDataStore").GetComponent<RecoveryDataStore>();

            //Common
            PlayerPrefs.SetBool("tutorialDoneFlg", true);
            if (RecoveryDataStore.kuniLv > 100) RecoveryDataStore.kuniLv = 100;
            if (RecoveryDataStore.kuniLv < 1) RecoveryDataStore.kuniLv = 1;
            PlayerPrefs.SetInt("kuniLv", RecoveryDataStore.kuniLv);
            Exp expScript = new Exp();
            int jinkeiLimit = expScript.getJinkeiLimit(RecoveryDataStore.kuniLv);
            int stockLimit = expScript.getStockLimit(RecoveryDataStore.kuniLv);
            PlayerPrefs.SetInt("jinkeiLimit", jinkeiLimit);
            PlayerPrefs.SetInt("stockLimit", stockLimit);
            PlayerPrefs.SetString("userId", RecoveryDataStore.inputUserId);
            if (RecoveryDataStore.addJinkei1) {
                PlayerPrefs.SetBool("addJinkei1", true);
            }else {
                PlayerPrefs.SetBool("addJinkei1", false);
            }
            if (RecoveryDataStore.addJinkei2) {
                PlayerPrefs.SetBool("addJinkei2", true);
            }else {
                PlayerPrefs.SetBool("addJinkei2", false);
            }
            if (RecoveryDataStore.addJinkei3) {
                PlayerPrefs.SetBool("addJinkei3", true);
            }else {
                PlayerPrefs.SetBool("addJinkei3", false);
            }
            if (RecoveryDataStore.addJinkei4) {
                PlayerPrefs.SetBool("addJinkei4", true);
            }else {
                PlayerPrefs.SetBool("addJinkei4", false);
            }
            PlayerPrefs.DeleteKey("rengouFlg");
            PlayerPrefs.DeleteKey("rengouDaimyo");
            
            if (RecoveryDataStore.dataStore_userId==0) {
                /***dataStore***/
                PlayerPrefs.SetInt("kuniExp", RecoveryDataStore.kuniExp);
                PlayerPrefs.SetString("yearSeason", RecoveryDataStore.yearSeason);
                PlayerPrefs.SetInt("movieCount", RecoveryDataStore.movieCount);
                PlayerPrefs.SetInt("space", RecoveryDataStore.space);
                if(RecoveryDataStore.pvpHeiryoku !=0 ) PlayerPrefs.SetInt("pvpHeiryoku", RecoveryDataStore.pvpHeiryoku);
                string seiryoku = RecoveryDataStore.seiryoku;
                if (seiryoku == "") seiryoku = "1,2,3,4,5,6,7,8,3,4,9,10,12,11,13,14,15,16,3,17,18,17,19,8,19,19,20,21,22,23,24,25,26,27,28,29,30,31,31,32,33,34,35,35,36,37,38,38,38,38,31,31,31,39,40,41,41,41,41,42,43,44,45,45,46";
                PlayerPrefs.SetString("seiryoku", seiryoku);
                KuniInfo kuniScript = new KuniInfo();
                kuniScript.updateOpenKuni(RecoveryDataStore.myDaimyo, seiryoku);
                kuniScript.updateClearedKuni(RecoveryDataStore.myDaimyo, seiryoku);
                PlayerPrefs.SetInt("money", RecoveryDataStore.money);
                PlayerPrefs.SetInt("busyoDama", RecoveryDataStore.busyoDama);
                PlayerPrefs.SetInt("syogunDaimyoId", RecoveryDataStore.syogunDaimyoId);
                PlayerPrefs.SetString("doumei", RecoveryDataStore.doumei);

                for (int i = 0; i < RecoveryDataStore.questDailyFlgId.Count; i++) {
                    int questId = RecoveryDataStore.questDailyFlgId[i];
                    string tmp = "questDailyFlg" + questId.ToString();
                    string tmp2 = "questDailyReceivedFlg" + questId.ToString();
                    PlayerPrefs.SetBool(tmp, true);
                    bool questReceivedFlg = RecoveryDataStore.questDailyReceivedFlgId[i];
                    if (questReceivedFlg) {
                        PlayerPrefs.SetBool(tmp2, true);
                    }else {
                        PlayerPrefs.SetBool(tmp2, false);
                    }
                }
                
                for (int i=0; i<RecoveryDataStore.questSpecialFlgId.Count; i++) {
                    int questId = RecoveryDataStore.questSpecialFlgId[i];
                    string tmp = "questSpecialFlg" + questId.ToString();
                    string tmp2 = "questSpecialReceivedFlg" + questId.ToString();
                    PlayerPrefs.SetBool(tmp, true);
                
                    bool questReceivedFlg = RecoveryDataStore.questSpecialReceivedFlgId[i];
                    if (questReceivedFlg) {                    
                        PlayerPrefs.SetBool(tmp2, true);                
                    }else {
                        PlayerPrefs.SetBool(tmp2, false);
                    }
                }

                Entity_quest_count_mst questCountMst = Resources.Load("Data/quest_count_mst") as Entity_quest_count_mst;
                for (int i = 0; i < questCountMst.param.Count; i++) {
                    bool dailyFlg = questCountMst.param[i].daily;
                    if(!dailyFlg) {
                        countSpecialList.Add(i);
                    }else {
                        countDailyList.Add(i);
                    }
                }
                if(RecoveryDataStore.questSpecialCountReceivedFlg.Count != 0) {
                    for (int i = 0; i < countSpecialList.Count; i++) {
                        int id = countSpecialList[i];
                        string tmp = "questSpecialCountReceivedFlg" + id.ToString();
                        bool questReceivedFlg = RecoveryDataStore.questSpecialCountReceivedFlg[i];
                        if (questReceivedFlg) {
                            PlayerPrefs.SetBool(tmp, true);
                        }else {
                            PlayerPrefs.SetBool(tmp, false);
                        }
                    }
                }
                if (RecoveryDataStore.questDailyCountReceivedFlg.Count != 0) {
                    for (int i = 0; i < countDailyList.Count; i++) {
                        int id = countDailyList[i];
                        string tmp = "questDailyCountReceivedFlg" + id.ToString();
                        bool questReceivedFlg = RecoveryDataStore.questDailyCountReceivedFlg[i];
                        if (questReceivedFlg) {
                            PlayerPrefs.SetBool(tmp, true);
                        }else {
                            PlayerPrefs.SetBool(tmp, false);
                        }
                    }
                }

                int count = 0;
                string myBusyo = "";
                for (int i=0;i<RecoveryDataStore.busyoList.Count; i++) {
                    int busyoId = RecoveryDataStore.busyoList[i];
                
                    if(busyoId != 0) {                                    
                        //add busyo                    
                        if (myBusyo == null || myBusyo == "") {
                            myBusyo = busyoId.ToString();
                        }else {
                            myBusyo = myBusyo + "," + busyoId.ToString();
                        }                                            
                        //add parametor
                        int lv = RecoveryDataStore.lvList[count];
                        string hei = RecoveryDataStore.heiList[count];
                        int senpouLv = RecoveryDataStore.senpouLvList[count];
                        int sakuLv = RecoveryDataStore.sakuLvList[count];
                        string kahou = RecoveryDataStore.kahouList[count];
                        
                        //lv
                        PlayerPrefs.SetInt(busyoId.ToString(), lv);
                        if (lv <= 0) lv = 1;

                        //hei
                        string heiTmp = "hei" + busyoId.ToString();                    
                        PlayerPrefs.SetString(heiTmp, hei);

                        //senpou
                        string senpou = "senpou" + busyoId.ToString();
                        PlayerPrefs.SetInt(senpou, senpouLv); //Lv

                        //saku
                        string saku = "saku" + busyoId.ToString();
                        PlayerPrefs.SetInt(saku, sakuLv); //Lv

                        //kahou
                        string kahouTmp = "kahou" + busyoId.ToString();
                        PlayerPrefs.SetString(kahouTmp, kahou);

                        //exp
                        string exp = "exp" + busyoId.ToString();
                        int totalBusyoExp = expScript.getExpforNextLv(lv - 1);
                        PlayerPrefs.SetInt(exp, totalBusyoExp);

                        //addLv
                        string addlvTmp = "addlv" + busyoId.ToString();
                        int addlv = RecoveryDataStore.addLvList[count];
                        if (addlv != 0) {
                            PlayerPrefs.SetInt(addlvTmp, addlv);
                        }

                        //gokui
                        string gokuiTmp = "gokui" + busyoId.ToString();
                        int gokui = RecoveryDataStore.gokuiList[count];
                        if (gokui != 0) {
                            PlayerPrefs.SetInt(gokuiTmp, gokui);
                        }

                        //gokui
                        string kanniTmp = "kanni" + busyoId.ToString();
                        if (RecoveryDataStore.kanniList.Count != 0) {
                            int kanni = RecoveryDataStore.kanniList[count];
                            if (kanni != 0) {
                                PlayerPrefs.SetInt(kanniTmp, kanni);
                            }
                        }
                        //jyosyuHei
                        string jyosyuHeiTmp = "jyosyuHei" + busyoId.ToString();
                        if (RecoveryDataStore.jyosyuHeiList.Count != 0) {
                            if(RecoveryDataStore.jyosyuHeiList.Count > count) {
                               int jyosyuHei = RecoveryDataStore.jyosyuHeiList[count];
                               if (jyosyuHei != 0) {
                                    PlayerPrefs.SetInt(jyosyuHeiTmp, jyosyuHei);
                               }
                            }
                        }
                        //jyosyuBusyo
                        string jyosyuBusyoTmp = "jyosyuBusyo" + busyoId.ToString();
                        if(RecoveryDataStore.jyosyuBusyoList.Count !=0) {
                            if (RecoveryDataStore.jyosyuBusyoList.Count > count) {
                                int jyosyuBusyo = RecoveryDataStore.jyosyuBusyoList[count];
                                if (jyosyuBusyo != 0) {
                                    PlayerPrefs.SetInt(jyosyuBusyoTmp, jyosyuBusyo);
                                }
                            }
                        }
                        count = count + 1;
                    }
                }
                PlayerPrefs.SetInt("myDaimyo", RecoveryDataStore.myDaimyo);
                Daimyo daimyoScript = new Daimyo();
                PlayerPrefs.SetInt("myDaimyoBusyo", daimyoScript.getDaimyoBusyoId(RecoveryDataStore.myDaimyo));
                PlayerPrefs.SetString("myBusyo", myBusyo);
                PlayerPrefs.SetString("myKanni", RecoveryDataStore.myKanni);
                PlayerPrefs.SetString("availableBugu", RecoveryDataStore.availableBugu);
                PlayerPrefs.SetString("availableKabuto",RecoveryDataStore.availableKabuto);
                PlayerPrefs.SetString("availableGusoku",RecoveryDataStore.availableGusoku);
                PlayerPrefs.SetString("availableMeiba",RecoveryDataStore.availableMeiba);
                PlayerPrefs.SetString("availableCyadougu", RecoveryDataStore.availableCyadougu);
                PlayerPrefs.SetString("availableHeihousyo", RecoveryDataStore.availableHeihousyo);
                PlayerPrefs.SetString("availableChishikisyo", RecoveryDataStore.availableChishikisyo);
                PlayerPrefs.SetString("kanjyo", RecoveryDataStore.kanjyo);
                if (RecoveryDataStore.cyouheiYR == "" || RecoveryDataStore.cyouheiYR == null) RecoveryDataStore.cyouheiYR = "0,0,0";
                PlayerPrefs.SetString("cyouheiYR", RecoveryDataStore.cyouheiYR);
                if (RecoveryDataStore.cyouheiKB == "" || RecoveryDataStore.cyouheiKB == null) RecoveryDataStore.cyouheiKB = "0,0,0";
                PlayerPrefs.SetString("cyouheiKB", RecoveryDataStore.cyouheiKB);
                if (RecoveryDataStore.cyouheiTP == "" || RecoveryDataStore.cyouheiTP == null) RecoveryDataStore.cyouheiTP = "0,0,0";
                PlayerPrefs.SetString("cyouheiTP", RecoveryDataStore.cyouheiTP);
                if (RecoveryDataStore.cyouheiYM == "" || RecoveryDataStore.cyouheiYM == null) RecoveryDataStore.cyouheiYM = "0,0,0";
                PlayerPrefs.SetString("cyouheiYM", RecoveryDataStore.cyouheiYM);
                PlayerPrefs.SetInt("hidensyoGe", RecoveryDataStore.hidensyoGe);
                PlayerPrefs.SetInt("hidensyoCyu", RecoveryDataStore.hidensyoCyu);
                PlayerPrefs.SetInt("hidensyoJyo", RecoveryDataStore.hidensyoJyo);
                PlayerPrefs.SetInt("shinobiGe", RecoveryDataStore.shinobiGe);
                PlayerPrefs.SetInt("shinobiCyu", RecoveryDataStore.shinobiCyu);
                PlayerPrefs.SetInt("shinobiJyo", RecoveryDataStore.shinobiJyo);
                PlayerPrefs.SetString("kengouItem", RecoveryDataStore.kengouItem);
                PlayerPrefs.SetString("gokuiItem", RecoveryDataStore.gokuiItem);
                PlayerPrefs.SetString("nanbanItem", RecoveryDataStore.nanbanItem);
                PlayerPrefs.SetInt("transferTP", RecoveryDataStore.transferTP);
                PlayerPrefs.SetInt("transferKB", RecoveryDataStore.transferKB);
                PlayerPrefs.SetInt("meisei", RecoveryDataStore.meisei);
                PlayerPrefs.SetString("shiro", RecoveryDataStore.shiro);
                PlayerPrefs.SetString("koueki", RecoveryDataStore.koueki);
                PlayerPrefs.SetString("cyoutei", RecoveryDataStore.cyoutei);
                PlayerPrefs.SetString("zukanBusyoHst", RecoveryDataStore.zukanBusyoHst);
                PlayerPrefs.SetString("zukanBuguHst", RecoveryDataStore.zukanBuguHst);
                PlayerPrefs.SetString("zukanGusokuHst", RecoveryDataStore.zukanGusokuHst);
                PlayerPrefs.SetString("zukanKabutoHst", RecoveryDataStore.zukanKabutoHst);
                PlayerPrefs.SetString("zukanMeibaHst", RecoveryDataStore.zukanMeibaHst);
                PlayerPrefs.SetString("zukanCyadouguHst", RecoveryDataStore.zukanCyadouguHst);
                PlayerPrefs.SetString("zukanChishikisyoHst", RecoveryDataStore.zukanChishikisyoHst);
                PlayerPrefs.SetString("zukanHeihousyoHst", RecoveryDataStore.zukanHeihousyoHst);
                PlayerPrefs.SetString("gameClearDaimyo", RecoveryDataStore.gameClearDaimyo);
                PlayerPrefs.SetString("gameClearDaimyoHard", RecoveryDataStore.gameClearDaimyoHard);

                //Gaikou
                Gaikou gaikou = new Gaikou();
                for (int l = 2; l < 47; l++) {
                    int value = gaikou.getGaikouValue(RecoveryDataStore.myDaimyo, l);
                    string temp = "gaikou" + l.ToString();
                    PlayerPrefs.SetInt(temp, value);
                }

                //jinkei
                PlayerPrefs.SetInt("jinkei", RecoveryDataStore.jinkeiId);
                PlayerPrefs.SetInt("soudaisyo" + RecoveryDataStore.jinkeiId, RecoveryDataStore.soudaisyo);
                for (int i = 0; i < 25; i++) {
                    int busyoId = RecoveryDataStore.busyoMapList[i];
                    int id = i + 1;
                    if (busyoId == 0) {
                        string tmp = RecoveryDataStore.jinkeiId.ToString() + "map" + id.ToString();
                        PlayerPrefs.DeleteKey(tmp);
                    }else {                        
                        string tmp = RecoveryDataStore.jinkeiId.ToString() + "map" + id.ToString();
                        PlayerPrefs.SetInt(tmp, busyoId);
                    }
                }

                //naisei
                for (int i = 0; i < RecoveryDataStore.naiseiKuniList.Count; i++) {
                    int kuniId = RecoveryDataStore.naiseiKuniList[i];
                    string naiseiTmp = "naisei" + kuniId.ToString();
                    PlayerPrefs.SetString(naiseiTmp, RecoveryDataStore.naiseiList[i]);

                    if (RecoveryDataStore.naiseiShiroList.Count != 0) {
                        if (RecoveryDataStore.naiseiShiroList[i] !=0) {                       
                           string shiroTmp = "shiro" + kuniId.ToString();
                            PlayerPrefs.SetInt(shiroTmp, RecoveryDataStore.naiseiShiroList[i]);
                        }
                    }
                    if (RecoveryDataStore.jyosyuList.Count != 0) {
                        if (RecoveryDataStore.jyosyuList[i] != 0) {
                            if (RecoveryDataStore.jyosyuList.Count > i) {
                                string jyosyuTmp = "jyosyu" + kuniId.ToString();
                                PlayerPrefs.SetInt(jyosyuTmp, RecoveryDataStore.jyosyuList[i]);
                            }
                        }
                    }
                }
                
                //Add Qty & PvP Name
                PlayerPrefs.SetInt("myBusyoQty", RecoveryDataStore.busyoList.Count);
                string userName = RecoveryDataStore.userName;
                if (userName == "") userName = "unknown";
                PlayerPrefs.SetString("PvPName", userName);

            } else {

                /***userId + pvpJinkei***/
                int totalExp = expScript.getTotalExp(RecoveryDataStore.kuniLv);
                PlayerPrefs.SetInt("kuniExp", totalExp);
                PlayerPrefs.SetInt("myDaimyo", RecoveryDataStore.myDaimyo);
                Daimyo daimyoScript = new Daimyo();
                PlayerPrefs.SetInt("myDaimyoBusyo", daimyoScript.getDaimyoBusyoId(RecoveryDataStore.myDaimyo));

                //pvp jinkei
                string myBusyoString = PlayerPrefs.GetString("myBusyo");
                List<string> myBusyoList = new List<string>();
                char[] delimiterChars = { ',' };
                if (myBusyoString != null && myBusyoString != "") {
                    if (myBusyoString.Contains(",")) {
                        myBusyoList = new List<string>(myBusyoString.Split(delimiterChars));
                    }
                    else {
                        myBusyoList.Add(myBusyoString);
                    }
                }

                //zukan
                string zukanBusyoHst = PlayerPrefs.GetString("zukanBusyoHst");
                List<string> zukanBusyoHstList = new List<string>();
                if (zukanBusyoHst != null && zukanBusyoHst != "") {
                    if (zukanBusyoHst.Contains(",")) {
                        zukanBusyoHstList = new List<string>(zukanBusyoHst.Split(delimiterChars));
                    }
                    else {
                        zukanBusyoHstList.Add(zukanBusyoHst);
                    }
                }

                //Daimyo Busyo History       
                Daimyo daimyo = new Daimyo();
                string gacyaDaimyoHst = PlayerPrefs.GetString("gacyaDaimyoHst");
                List<string> gacyaDaimyoHstList = new List<string>();
                if (gacyaDaimyoHst != null && gacyaDaimyoHst != "") {
                    if (gacyaDaimyoHst.Contains(",")) {
                        gacyaDaimyoHstList = new List<string>(gacyaDaimyoHst.Split(delimiterChars));
                    }
                    else {
                        gacyaDaimyoHstList.Add(gacyaDaimyoHst);
                    }
                }

                int count = 0;
                for (int i = 0; i < RecoveryDataStore.busyoList.Count; i++) {
                    int busyoId = RecoveryDataStore.busyoList[i];

                    if (busyoId != 0) {
                        //add busyo             
                        if (!myBusyoList.Contains(busyoId.ToString())) {
                            if (myBusyoString == null || myBusyoString == "") {
                                myBusyoString = busyoId.ToString();
                            }
                            else {
                                myBusyoString = myBusyoString + "," + busyoId.ToString();
                            }
                            PlayerPrefs.SetString("myBusyo", myBusyoString);
                        }

                        //add zukan
                        if (!zukanBusyoHstList.Contains(busyoId.ToString())) {
                            if (zukanBusyoHst == null || zukanBusyoHst == "") {
                                zukanBusyoHst = busyoId.ToString();
                            }
                            else {
                                zukanBusyoHst = zukanBusyoHst + "," + busyoId.ToString();
                            }
                            PlayerPrefs.SetString("zukanBusyoHst", zukanBusyoHst);
                        }

                        //add daimyo busyo
                        if (daimyo.daimyoBusyoCheck(busyoId)) {
                            if (!gacyaDaimyoHstList.Contains(busyoId.ToString())) {
                                if (gacyaDaimyoHst == null || gacyaDaimyoHst == "") {
                                    gacyaDaimyoHst = busyoId.ToString();
                                }
                                else {
                                    gacyaDaimyoHst = gacyaDaimyoHst + "," + busyoId.ToString();
                                }
                                PlayerPrefs.SetString("gacyaDaimyoHst", gacyaDaimyoHst);
                            }
                        }

                        //add parametor
                        int lv = RecoveryDataStore.lvList[count];
                        string hei = RecoveryDataStore.heiList[count];
                        int senpouLv = RecoveryDataStore.senpouLvList[count];
                        int sakuLv = RecoveryDataStore.sakuLvList[count];
                        string kahou = RecoveryDataStore.kahouList[count];

                        //lv
                        PlayerPrefs.SetInt(busyoId.ToString(), lv);

                        //hei
                        string heiTmp = "hei" + busyoId.ToString();
                        PlayerPrefs.SetString(heiTmp, hei);

                        //senpou
                        string senpou = "senpou" + busyoId.ToString();
                        PlayerPrefs.SetInt(senpou, senpouLv); //Lv

                        //saku
                        string saku = "saku" + busyoId.ToString();
                        PlayerPrefs.SetInt(saku, sakuLv); //Lv

                        //kahou
                        string kahouTmp = "kahou" + busyoId.ToString();
                        PlayerPrefs.SetString(kahouTmp, kahou);

                        //exp
                        string exp = "exp" + busyoId.ToString();
                        int totalBusyoExp = expScript.getExpforNextLv(lv - 1);
                        PlayerPrefs.SetInt(exp, totalBusyoExp);

                        count = count + 1;
                    }
                }
                PlayerPrefs.Flush();

                //Add Qty & PvP Name
                string myBusyoStringCount = PlayerPrefs.GetString("myBusyo");
                List<string> myBusyoListCount = new List<string>();
                if (myBusyoStringCount.Contains(",")) {
                    myBusyoListCount = new List<string>(myBusyoStringCount.Split(delimiterChars));
                }else {
                    myBusyoListCount.Add(myBusyoStringCount);
                }
                PlayerPrefs.SetInt("myBusyoQty", myBusyoListCount.Count);
                string userName = RecoveryDataStore.userName;
                if (userName == "") userName = "unknown";
                PlayerPrefs.SetString("PvPName", userName);
            }

            PlayerPrefs.Flush();            

            //add data recovery history
            RecoveryDataStore.UpdateDataRecovery(RecoveryDataStore.inputUserId);


            //Final Process
            audioSources[3].Play();
            Destroy(back.gameObject);
            Destroy(msg.gameObject);
            GameObject.Find("Start").GetComponent<Button>().enabled = false;
            Message msgScript = new Message();
            msgScript.makeMessage(msgScript.getMessage(151));

        }else {
            audioSources[1].Play();
            GameObject.Find("Button").GetComponent<BackMain>().OnClick();
        }
    }

}
