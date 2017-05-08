using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DataPvP : MonoBehaviour {

    public void InsertPvP(string userId, string PvPName) {
        int pvpHeiryoku = PlayerPrefs.GetInt("pvpHeiryoku");
        if (pvpHeiryoku == 0) {
            pvpHeiryoku = PlayerPrefs.GetInt("jinkeiHeiryoku");
        }
        int kuniLv = PlayerPrefs.GetInt("kuniLv");
        int jinkei = PlayerPrefs.GetInt("jinkei");
        string soudaisyoTmp = "soudaisyo" + jinkei.ToString();
        int soudaisyo = PlayerPrefs.GetInt(soudaisyoTmp);
        
        NCMBObject pvpClass = new NCMBObject("pvp");
        pvpClass["userId"] = userId;
        pvpClass["userName"] = PvPName;
        pvpClass["kuniLv"] = kuniLv;
        pvpClass["jinkeiHeiryoku"] = pvpHeiryoku;
        pvpClass["atkNo"] = 0;
        pvpClass["atkWinNo"] = 0;
        pvpClass["dfcNo"] = 0;
        pvpClass["dfcWinNo"] = 0;
        pvpClass["totalWinNo"] = 0;
        pvpClass["soudaisyo"] = soudaisyo;

        pvpClass.SaveAsync();
    }

    public void UpdatePvP() {

        string userId = PlayerPrefs.GetString("userId");
        string userName = PlayerPrefs.GetString("PvPName");
        int pvpHeiryoku = PlayerPrefs.GetInt("pvpHeiryoku");
        if(pvpHeiryoku == 0) {
            pvpHeiryoku = PlayerPrefs.GetInt("jinkeiHeiryoku");
        }
        int kuniLv = PlayerPrefs.GetInt("kuniLv");
        int jinkei = PlayerPrefs.GetInt("jinkei");
        string soudaisyoTmp = "soudaisyo" + jinkei.ToString();
        int soudaisyo = PlayerPrefs.GetInt(soudaisyoTmp);

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        query.WhereEqualTo("userId", userId);

        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                if (objList.Count == 0) {
                    //Won't be into this loop
                    InsertPvP(userId, userName);
                }else { //Update           
                    objList[0]["userName"] = userName;
                    objList[0]["kuniLv"] = kuniLv;
                    objList[0]["jinkeiHeiryoku"] = pvpHeiryoku;
                    objList[0]["soudaisyo"] = soudaisyo;
                    objList[0].SaveAsync();
                }
            }
        });
    }

    public void UpdatePvPName(string userId, string userName) {

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        query.WhereEqualTo("userId", userId);

        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                if (objList.Count == 0) {
                    //Won't be into this loop
                    InsertPvP(userId, userName);
                }
                else { //Update           
                    objList[0]["userName"] = userName;
                    objList[0].SaveAsync();
                }
            }
        });
    }




    public int getHPRank(int myJinkeiHeiryoku) {
        int myRank = 0;

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        query.WhereGreaterThan("jinkeiHeiryoku", myJinkeiHeiryoku);
        query.CountAsync((int count, NCMBException e) => {
            if (e != null) {
                //件数取得失敗
            }else {
                //件数取得成功
                myRank = count + 1;// 自分よりスコアが上の人がn人いたら自分はn+1位
            }
        });

        return myRank;
    }

    public int getPvPCount() {

        int allCount = 0;
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        query.CountAsync((int count, NCMBException e) => {
            if (e != null) {
                //件数取得失敗
                
            }else {
                //件数取得成功
                allCount = count;
            }
        });

        return allCount;
    }



}
