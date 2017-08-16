using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

public class RecoveryDataStore : MonoBehaviour {

    //common
    public AudioSource[] audioSources;
    public string inputUserId;

    //user id
    public int userIdCount = -1;
    public int kuniLv = -1;
    public bool addJinkei1 = false;
    public bool addJinkei2 = false;
    public bool addJinkei3 = false;
    public bool addJinkei4 = false;

    //recovery
    public int dataRecoveryCount = -1;

    //pvp jinkei
    public int pvpJinkeiCount = -1;
    public List<int> busyoList = new List<int>();
    public List<int> lvList = new List<int>();
    public List<string> heiList = new List<string>();
    public List<string> kahouList = new List<string>();
    public List<int> senpouLvList = new List<int>();
    public List<int> sakuLvList = new List<int>();

    //pvp
    public string userName = "";

    void Start() {
        audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
    }

    public void GetUserId(string userId) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("userId");
        query.WhereEqualTo("userId", userId);
        inputUserId = userId;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            if (e == null) {
                userIdCount = objList.Count;
                if(userIdCount != 0) {
                    foreach (NCMBObject obj in objList) {
                        kuniLv = System.Convert.ToInt32(obj["kuniLv"]);
                        addJinkei1 = System.Convert.ToBoolean(obj["addJinkei1"]);
                        addJinkei2 = System.Convert.ToBoolean(obj["addJinkei2"]);
                        addJinkei3 = System.Convert.ToBoolean(obj["addJinkei3"]);
                        addJinkei4 = System.Convert.ToBoolean(obj["addJinkei4"]);
                    }
                }else {
                    audioSources[4].Play();
                    Message msg = new Message();
                    msg.makeMessage(msg.getMessage(148));
                    ResetValue();
                }
            }else {
                audioSources[4].Play();
                Message msg = new Message();
                msg.makeMessage(msg.getMessage(113));
                ResetValue();
            }
        });
    }

    public void GetDataRecoveryCount(string userId) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("dataRecovery");
        query.WhereEqualTo("userId", userId);
        query.CountAsync((int count, NCMBException e) => {
            if (e != null) {
                audioSources[4].Play();
                Message msg = new Message();
                msg.makeMessage(msg.getMessage(113));
                ResetValue();
            }else {
                //件数取得成功
                dataRecoveryCount = count;
                if(dataRecoveryCount != 0) {
                    audioSources[4].Play();
                    Message msg = new Message();
                    msg.makeMessage(msg.getMessage(149));
                    ResetValue();
                }
            }
        });
    }

    public void GetPvPJinkei(string userId) {
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
                    }

                }else {
                    audioSources[4].Play();
                    Message msg = new Message();
                    msg.makeMessage(msg.getMessage(150));
                    ResetValue();
                }
            }else {
                audioSources[4].Play();
                Message msg = new Message();
                msg.makeMessage(msg.getMessage(113));
                ResetValue();
            }
        });
    }

    public void GetPvP(string userId) {
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
                msg.makeMessage(msg.getMessage(113));
                ResetValue();
            }
        });

    }

    public void InsertDataRecovery(string userId) {
        NCMBObject query = new NCMBObject("dataRecovery");
        query["userId"] = userId;
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
        dataRecoveryCount = -1;
        pvpJinkeiCount = -1;
        busyoList = new List<int>();
        lvList = new List<int>();
        heiList = new List<string>();
        kahouList = new List<string>();
        senpouLvList = new List<int>();
        sakuLvList = new List<int>();

        DataRecovery DataRecovery = GameObject.Find("Start").GetComponent<DataRecovery>();
        DataRecovery.Fetched1 = false;
        DataRecovery.Fetched2 = false;
        DataRecovery.Fetched3 = false;
        DataRecovery.inputUserId = "";

    }
}
