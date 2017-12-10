using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class RecoveryDataStore : MonoBehaviour {

    //common
    public AudioSource[] audioSources;
    public string inputUserId;
    public int dataStore_userId = -1; //dataStore=0, userId=1
    public int senarioId = -1;

    //user id
    public int userIdCount = -1;
    public int kuniLv = -1;
    public int myDaimyo = 0;
    public bool addJinkei1 = false;
    public bool addJinkei2 = false;
    public bool addJinkei3 = false;
    public bool addJinkei4 = false;
    public bool scenario1 = false;
    public bool scenario2 = false;
    public bool scenario3 = false;

    //recovery
    public int dataRecoveryCount = -1;

    //pvp
    public string userName = "";
    public int pvpJinkeiCount = -1;

    /**data store**/
    //common
    public string yearSeason = "";
    public string seiryoku = "";
    public int money = 0;
    public int busyoDama = 0;
    public int syogunDaimyoId = 0;
    public string doumei = "";
    public List<int> questDailyFlgId = new List<int>();
    public List<bool> questDailyReceivedFlgId = new List<bool>();
    public List<int> questSpecialFlgId = new List<int>();
    public List<bool> questSpecialReceivedFlgId = new List<bool>();
    public List<bool> questSpecialCountReceivedFlg = new List<bool>();
    public List<bool> questDailyCountReceivedFlg = new List<bool>();
    public int kuniExp;
    public int movieCount = 0;
    public int space = 0;
    public int pvpHeiryoku = 0;

    //busyo
    public string myBusyo = "";
    public List<int> busyoList = new List<int>();
    public List<int> lvList = new List<int>();
    public List<string> heiList = new List<string>();
    public List<string> kahouList = new List<string>();
    public List<int> jyosyuHeiList = new List<int>();
    public List<int> jyosyuBusyoList = new List<int>();
    public List<int> senpouLvList = new List<int>();
    public List<int> sakuLvList = new List<int>();
    public List<int> addLvList = new List<int>();
    public List<int> gokuiList = new List<int>();
    public List<int> kanniList = new List<int>();

    //jinkeiMap
    public List<int> busyoMapList = new List<int>();
    public int pvpJinkeiMapCount = -1;
    public int soudaisyo = -1;
    public int jinkeiId = -1;

    //item
    public string myKanni = "";
    public string availableBugu = "";
    public string availableKabuto = "";
    public string availableGusoku = "";
    public string availableMeiba = "";
    public string availableCyadougu = "";
    public string availableHeihousyo = "";
    public string availableChishikisyo = "";
    public string kanjyo = "";
    public string cyouheiYR = "";
    public string cyouheiKB = "";
    public string cyouheiTP = "";
    public string cyouheiYM = "";
    public int hidensyoGe = 0;
    public int hidensyoCyu = 0;
    public int hidensyoJyo = 0;
    public int shinobiGe = 0;
    public int shinobiCyu = 0;
    public int shinobiJyo = 0;
    public string kengouItem = "";
    public string gokuiItem = "";
    public string nanbanItem = "";
    public int transferTP = 0;
    public int transferKB = 0;
    public int meisei = 0;
    public string shiro = "";
    public string koueki = "";
    public string cyoutei = "";

    //zukan
    public string zukanBusyoHst = "";
    public string zukanBuguHst = "";
    public string zukanGusokuHst = "";
    public string zukanKabutoHst = "";
    public string zukanMeibaHst = "";
    public string zukanCyadouguHst = "";
    public string zukanChishikisyoHst = "";
    public string zukanHeihousyoHst = "";
    public string gameClearDaimyo = "";
    public string gameClearDaimyoHard = "";

    //naisei
    public List<int> naiseiKuniList = new List<int>();
    public List<string> naiseiList = new List<string>();
    public List<int> naiseiShiroList = new List<int>();
    public List<int> jyosyuList = new List<int>();

    void Start() {
        audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
    }

    //Old
    public void GetUserId(string userId, int langId) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("userId");        
        query.WhereEqualTo("userId", userId);
        inputUserId = userId;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            if (e == null) {
                userIdCount = objList.Count;
                if (userIdCount != 0) {
                    foreach (NCMBObject obj in objList) {
                        kuniLv = System.Convert.ToInt32(obj["kuniLv"]);
                        addJinkei1 = System.Convert.ToBoolean(obj["addJinkei1"]);
                        addJinkei2 = System.Convert.ToBoolean(obj["addJinkei2"]);
                        addJinkei3 = System.Convert.ToBoolean(obj["addJinkei3"]);
                        addJinkei4 = System.Convert.ToBoolean(obj["addJinkei4"]);
                        myDaimyo = System.Convert.ToInt32(obj["myDaimyo"]);
                        GetPvPJinkei(userId,langId);                        
                    }
                }else {
                    audioSources[4].Play();
                    Message msg = new Message();
                    msg.makeMessage(msg.getMessage(148,langId));
                    ResetValue();
                }
            }
            else {
                audioSources[4].Play();
                Message msg = new Message();
                msg.makeMessage(msg.getMessage(113,langId));
                ResetValue();
            }
        });
    }

    //New
    public void GetDataStore(string userId, int langId) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("dataStore");
        query.WhereEqualTo("userId", userId);
        inputUserId = userId;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            if (e == null) {
                userIdCount = objList.Count;
                if (userIdCount != 0) {
                    foreach (NCMBObject obj in objList) {
                        if (checkDataExist(obj, "senarioId")) senarioId = System.Convert.ToInt32(obj["senarioId"]);
                        kuniLv = System.Convert.ToInt32(obj["kuniLv"]);
                        kuniExp = System.Convert.ToInt32(obj["kuniExp"]);
                        myDaimyo = System.Convert.ToInt32(obj["myDaimyo"]);
                        addJinkei1 = System.Convert.ToBoolean(obj["addJinkei1"]);
                        addJinkei2 = System.Convert.ToBoolean(obj["addJinkei2"]);
                        addJinkei3 = System.Convert.ToBoolean(obj["addJinkei3"]);
                        addJinkei4 = System.Convert.ToBoolean(obj["addJinkei4"]);
                        if (checkDataExist(obj, "scenario1")) scenario1 = System.Convert.ToBoolean(obj["scenario1"]);
                        if (checkDataExist(obj, "scenario2")) scenario2 = System.Convert.ToBoolean(obj["scenario2"]);
                        if (checkDataExist(obj, "scenario3")) scenario3 = System.Convert.ToBoolean(obj["scenario3"]);
                        yearSeason = System.Convert.ToString(obj["yearSeason"]);
                        seiryoku = System.Convert.ToString(obj["seiryoku"]);
                        money = System.Convert.ToInt32(obj["money"]);
                        busyoDama = System.Convert.ToInt32(obj["busyoDama"]);
                        syogunDaimyoId = System.Convert.ToInt32(obj["syogunDaimyoId"]);
                        doumei = System.Convert.ToString(obj["doumei"]);
                        if (checkDataExist(obj, "movieCount")) movieCount = System.Convert.ToInt32(obj["movieCount"]);
                        if (checkDataExist(obj, "space")) space = System.Convert.ToInt32(obj["space"]);
                        if (checkDataExist(obj, "pvpHeiryoku")) pvpHeiryoku = System.Convert.ToInt32(obj["pvpHeiryoku"]);

                        if(checkDataExist(obj, "questDailyFlgId")){
                            ArrayList arraylistDaily = (ArrayList)obj["questDailyFlgId"];
                            foreach (object o in arraylistDaily) questDailyFlgId.Add(System.Convert.ToInt32(o));
                        }
                        if (checkDataExist(obj, "questDailyReceivedFlgId")) {
                            ArrayList arraylistDailyReceived = (ArrayList)obj["questDailyReceivedFlgId"];
                            foreach (object o in arraylistDailyReceived) questDailyReceivedFlgId.Add(System.Convert.ToBoolean(o));
                        }
                        if (checkDataExist(obj, "questSpecialFlgId")) {
                            ArrayList arraylist1 = (ArrayList)obj["questSpecialFlgId"];
                            foreach (object o in arraylist1) questSpecialFlgId.Add(System.Convert.ToInt32(o));
                        }
                        if (checkDataExist(obj, "questSpecialReceivedFlgId")) {
                            ArrayList arraylist2 = (ArrayList)obj["questSpecialReceivedFlgId"];
                            foreach (object o in arraylist2) questSpecialReceivedFlgId.Add(System.Convert.ToBoolean(o));
                        }

                        ArrayList arraylist3 = (ArrayList)obj["myBusyoList"];
                        foreach (object o in arraylist3) busyoList.Add(System.Convert.ToInt32(o));

                        ArrayList arraylist4 = (ArrayList)obj["lvList"];
                        foreach (object o in arraylist4) lvList.Add(System.Convert.ToInt32(o));

                        ArrayList arraylist5 = (ArrayList)obj["heiList"];
                        foreach (object o in arraylist5) heiList.Add(System.Convert.ToString(o));

                        ArrayList arraylist6 = (ArrayList)obj["senpouLvList"];
                        foreach (object o in arraylist6) senpouLvList.Add(System.Convert.ToInt32(o));

                        ArrayList arraylist7 = (ArrayList)obj["sakuLvList"];
                        foreach (object o in arraylist7) sakuLvList.Add(System.Convert.ToInt32(o));

                        ArrayList arraylist8 = (ArrayList)obj["kahouList"];
                        foreach (object o in arraylist8) kahouList.Add(System.Convert.ToString(o));

                        ArrayList arraylist9 = (ArrayList)obj["addLvList"];
                        foreach (object o in arraylist9) addLvList.Add(System.Convert.ToInt32(o));

                        ArrayList arraylist10 = (ArrayList)obj["gokuiList"];
                        foreach (object o in arraylist10) gokuiList.Add(System.Convert.ToInt32(o));

                        if (checkDataExist(obj, "kanniList")) {
                            ArrayList arraylist11 = (ArrayList)obj["kanniList"];
                            foreach (object o in arraylist11) kanniList.Add(System.Convert.ToInt32(o));
                        }
                        if (checkDataExist(obj, "jyosyuHeiList")) {
                            ArrayList arraylistJyosyuHei = (ArrayList)obj["jyosyuHeiList"];
                            foreach (object o in arraylistJyosyuHei) jyosyuHeiList.Add(System.Convert.ToInt32(o));
                        }
                        if (checkDataExist(obj, "jyosyuBusyoList")) {
                            ArrayList arraylistJyosyuBusyoList = (ArrayList)obj["jyosyuBusyoList"];
                            foreach (object o in arraylistJyosyuBusyoList) jyosyuBusyoList.Add(System.Convert.ToInt32(o));
                        }

                        myKanni = System.Convert.ToString(obj["myKanni"]);
                        availableBugu = System.Convert.ToString(obj["availableBugu"]);
                        availableKabuto = System.Convert.ToString(obj["availableKabuto"]);
                        availableGusoku = System.Convert.ToString(obj["availableGusoku"]);
                        availableMeiba = System.Convert.ToString(obj["availableMeiba"]);
                        availableCyadougu = System.Convert.ToString(obj["availableCyadougu"]);
                        availableHeihousyo = System.Convert.ToString(obj["availableHeihousyo"]);
                        availableChishikisyo = System.Convert.ToString(obj["availableChishikisyo"]);
                        kanjyo = System.Convert.ToString(obj["kanjyo"]);
                        cyouheiYR = System.Convert.ToString(obj["cyouheiYR"]);
                        cyouheiKB = System.Convert.ToString(obj["cyouheiKB"]);
                        if (checkDataExist(obj, "cyouheiTP")) cyouheiTP = System.Convert.ToString(obj["cyouheiTP"]);
                        else if(checkDataExist(obj, "CyouheiTP")) cyouheiTP = System.Convert.ToString(obj["CyouheiTP"]);
                        if (checkDataExist(obj, "cyouheiYM")) cyouheiYM = System.Convert.ToString(obj["cyouheiYM"]);
                        else if (checkDataExist(obj, "CyouheiYM")) cyouheiYM = System.Convert.ToString(obj["CyouheiYM"]);
                        hidensyoGe = System.Convert.ToInt32(obj["hidensyoGe"]);
                        hidensyoCyu = System.Convert.ToInt32(obj["hidensyoCyu"]);
                        hidensyoJyo = System.Convert.ToInt32(obj["hidensyoJyo"]);
                        shinobiGe = System.Convert.ToInt32(obj["shinobiGe"]);
                        shinobiCyu = System.Convert.ToInt32(obj["shinobiCyu"]);
                        shinobiJyo = System.Convert.ToInt32(obj["shinobiJyo"]);
                        kengouItem = System.Convert.ToString(obj["kengouItem"]);
                        gokuiItem = System.Convert.ToString(obj["gokuiItem"]);
                        nanbanItem = System.Convert.ToString(obj["nanbanItem"]);
                        transferTP = System.Convert.ToInt32(obj["transferTP"]);
                        transferKB = System.Convert.ToInt32(obj["transferKB"]);
                        meisei = System.Convert.ToInt32(obj["meisei"]);
                        shiro = System.Convert.ToString(obj["shiro"]);
                        koueki = System.Convert.ToString(obj["koueki"]);
                        cyoutei = System.Convert.ToString(obj["cyoutei"]);
                        zukanBusyoHst = System.Convert.ToString(obj["zukanBusyoHst"]);
                        zukanBuguHst = System.Convert.ToString(obj["zukanBuguHst"]);
                        zukanGusokuHst = System.Convert.ToString(obj["zukanGusokuHst"]);
                        zukanKabutoHst = System.Convert.ToString(obj["zukanKabutoHst"]);
                        zukanMeibaHst = System.Convert.ToString(obj["zukanMeibaHst"]);
                        zukanCyadouguHst = System.Convert.ToString(obj["zukanCyadouguHst"]);
                        zukanChishikisyoHst = System.Convert.ToString(obj["zukanChishikisyoHst"]);
                        zukanHeihousyoHst = System.Convert.ToString(obj["zukanHeihousyoHst"]);
                        gameClearDaimyo = System.Convert.ToString(obj["gameClearDaimyo"]);
                        gameClearDaimyoHard = System.Convert.ToString(obj["gameClearDaimyoHard"]);

                        ArrayList arraylist12 = (ArrayList)obj["naiseiKuniList"];
                        foreach (object o in arraylist12) naiseiKuniList.Add(System.Convert.ToInt32(o));

                        ArrayList arraylist13 = (ArrayList)obj["naiseiList"];
                        foreach (object o in arraylist13) naiseiList.Add(System.Convert.ToString(o));

                        ArrayList arraylist14 = (ArrayList)obj["naiseiShiroList"];
                        foreach (object o in arraylist14) naiseiShiroList.Add(System.Convert.ToInt32(o));

                        if (checkDataExist(obj, "jyosyuList")) {
                            ArrayList arraylistJyosyuList = (ArrayList)obj["jyosyuList"];
                            foreach (object o in arraylistJyosyuList) jyosyuList.Add(System.Convert.ToInt32(o));
                        }

                        if (checkDataExist(obj, "questSpecialCountReceivedFlg")) {
                            ArrayList arraylist15 = (ArrayList)obj["questSpecialCountReceivedFlg"];
                            foreach (object o in arraylist15) questSpecialCountReceivedFlg.Add(System.Convert.ToBoolean(o));
                        }
                        if (checkDataExist(obj, "questDailyCountReceivedFlg")) {
                            ArrayList arraylist16 = (ArrayList)obj["questDailyCountReceivedFlg"];
                            foreach (object o in arraylist16) questDailyCountReceivedFlg.Add(System.Convert.ToBoolean(o));
                        }
                        GetPvPJinkeiMap(userId, langId);
                    }
                }else {
                    //Old
                    GetUserId(userId, langId);
                }
            }else {
                audioSources[4].Play();
                Message msg = new Message();
                msg.makeMessage(msg.getMessage(113,langId));
                ResetValue();
            }
        });
    }

    public void GetDataRecoveryCount(string userId, int langId) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("dataRecovery");
        query.WhereEqualTo("userId", userId);
        query.CountAsync((int count, NCMBException e) => {
            if (e != null) {
                audioSources[4].Play();
                Message msg = new Message();
                msg.makeMessage(msg.getMessage(113,langId));
                ResetValue();
            }else {
                //件数取得成功
                dataRecoveryCount = count;
                if(dataRecoveryCount != 0) {
                    audioSources[4].Play();
                    Message msg = new Message();
                    msg.makeMessage(msg.getMessage(149,langId));
                    ResetValue();
                }
            }
        });
    }

    
    public void GetPvPJinkei(string userId, int langId) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvpJinkei");
        query.WhereEqualTo("userId", userId);
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            if (e == null) {
                pvpJinkeiCount = objList.Count;
                if (pvpJinkeiCount != 0) {
                    foreach (NCMBObject obj in objList) {
    
                        //busyo
                        for (int i = 0; i < 25; i++) {
                            int id = i + 1;
                            string mapId = "map" + id.ToString();
                            int busyoId = System.Convert.ToInt32(obj[mapId]);
                            busyoList.Add(busyoId);
                        }

                        ArrayList arraylist1 = (ArrayList)obj["lvList"];
                        foreach (object o in arraylist1) lvList.Add(System.Convert.ToInt32(o));

                        ArrayList arraylist2 = (ArrayList)obj["heiList"];
                        foreach (object o in arraylist2) heiList.Add(System.Convert.ToString(o));

                        ArrayList arraylist3 = (ArrayList)obj["senpouLvList"];
                        foreach (object o in arraylist3) senpouLvList.Add(System.Convert.ToInt32(o));

                        ArrayList arraylist4 = (ArrayList)obj["sakuLvList"];
                        foreach (object o in arraylist4) sakuLvList.Add(System.Convert.ToInt32(o));

                        ArrayList arraylist5 = (ArrayList)obj["kahouList"];
                        foreach (object o in arraylist5) kahouList.Add(System.Convert.ToString(o));

                        dataStore_userId = 1;
                    }

                }else {
                    audioSources[4].Play();
                    Message msg = new Message();
                    msg.makeMessage(msg.getMessage(150,langId));
                    ResetValue();
                }
            }else {
                audioSources[4].Play();
                Message msg = new Message();
                msg.makeMessage(msg.getMessage(113,langId));
                ResetValue();
            }
        });
    }

    public void GetPvPJinkeiMap(string userId, int langId) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvpJinkei");
        query.WhereEqualTo("userId", userId);
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            if (e == null) {
                pvpJinkeiMapCount = objList.Count;
                if (pvpJinkeiMapCount != 0) {
                    foreach (NCMBObject obj in objList) {
                        soudaisyo = System.Convert.ToInt32(obj["soudaisyo"]);
                        jinkeiId = System.Convert.ToInt32(obj["jinkeiId"]);
                        for (int i = 0; i < 25; i++) {
                            int id = i + 1;
                            string mapId =  "map" + id.ToString();
                            int busyoId = System.Convert.ToInt32(obj[mapId]);
                            busyoMapList.Add(busyoId);
                        }
                        

                        dataStore_userId = 0;
                    }
                    
                }else {
                    audioSources[4].Play();
                    Message msg = new Message();
                    msg.makeMessage(msg.getMessage(150,langId));
                    ResetValue();
                }
            }else {
                audioSources[4].Play();
                Message msg = new Message();
                msg.makeMessage(msg.getMessage(113,langId));
                ResetValue();
            }
        });
    }


    public void GetPvP(string userId, int langId) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        query.WhereEqualTo("userId", userId);
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            if (e == null) {
                foreach (NCMBObject obj in objList) {
                    userName = System.Convert.ToString(obj["userName"]);
                }
            }
            else {
                audioSources[4].Play();
                Message msg = new Message();
                msg.makeMessage(msg.getMessage(113,langId));
                ResetValue();
            }
        });

    }


    public void UpdateDataRecovery(string userId) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("dataRecovery");
        query.WhereEqualTo("userId", userId);
        
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                if (objList.Count == 0) { //never registered
                    InsertDataRecovery(userId);
                }else { //Update  
                    foreach (NCMBObject obj in objList) {
                        int count = 1;
                        if (checkDataExist(obj, "count")) count = System.Convert.ToInt32(objList[0]["count"]) + 1;
                        objList[0]["count"] = count;
                        objList[0].SaveAsync();
                    }
                }
            }
        });
    }


    public void InsertDataRecovery(string userId) {        
        NCMBObject query = new NCMBObject("dataRecovery");
        query["userId"] = userId;
        query["count"] = 1;
        query.SaveAsync();
    }

    public void ResetValue() {
        inputUserId = "";
        userIdCount = -1;
        kuniLv = -1;
        addJinkei1 = false;
        addJinkei2 = false;
        addJinkei3 = false;
        addJinkei4 = false;
        scenario1 = false;
        scenario2 = false;
        scenario3 = false;
        dataRecoveryCount = -1;
        busyoList = new List<int>();
        lvList = new List<int>();
        heiList = new List<string>();
        kahouList = new List<string>();
        jyosyuHeiList = new List<int>();
        jyosyuBusyoList = new List<int>();
        senpouLvList = new List<int>();
        sakuLvList = new List<int>();

        DataRecovery DataRecovery = GameObject.Find("Start").GetComponent<DataRecovery>();
        DataRecovery.Fetched1 = false;
        DataRecovery.Fetched2 = false;
        DataRecovery.Fetched3 = false;
        DataRecovery.inputUserId = "";

    }

    private bool checkDataExist(NCMBObject obj, string key) {
        try {
            object test = obj[key];
        }
        catch (NCMBException ex) {
            return false;
        }
        return true;
    }
}
