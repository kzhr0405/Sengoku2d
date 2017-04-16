using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using NCMB;

public class DataMap : MonoBehaviour {

    public void InsertMap(string userId) {
        string seiryoku = PlayerPrefs.GetString("seiryoku");
        string openKuni = PlayerPrefs.GetString("openKuni");
        string clearedKuni = PlayerPrefs.GetString("clearedKuni");
        string keyHistory = PlayerPrefs.GetString("keyHistory");
        List<string> keyHistoryList = new List<string>();
        char[] delimiterChars = { ',' };
        if (keyHistory != null && keyHistory != "") {
            if (keyHistory.Contains(",")) {
                keyHistoryList = new List<string>(keyHistory.Split(delimiterChars));
            }else {
                keyHistoryList.Add(keyHistory);
            }
        }
        string gunzei = "";
        for (int n = 0; n < keyHistoryList.Count; n++) {
            string keyTemp = keyHistoryList[n];
            string keyValue = PlayerPrefs.GetString(keyTemp);
            if (n==0) {
                gunzei = keyValue;
            }else {
                gunzei = gunzei + "," + keyValue;
            }
        }


        NCMBObject mapClass = new NCMBObject("map");
        mapClass["userId"] = userId;
        mapClass["seiryoku"] = seiryoku;
        mapClass["openKuni"] = openKuni;
        mapClass["clearedKuni"] = clearedKuni;
        mapClass["keyHistory"] = keyHistory;
        mapClass["gunzei"] = gunzei;
        mapClass.SaveAsync();
    }



    public void UpdateMap(string userId) {

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("map");
        query.WhereEqualTo("userId", userId);

        string seiryoku = PlayerPrefs.GetString("seiryoku");
        string openKuni = PlayerPrefs.GetString("openKuni");
        string clearedKuni = PlayerPrefs.GetString("clearedKuni");
        string keyHistory = PlayerPrefs.GetString("keyHistory");
        List<string> keyHistoryList = new List<string>();
        char[] delimiterChars = { ',' };
        if (keyHistory != null && keyHistory != "") {
            if (keyHistory.Contains(",")) {
                keyHistoryList = new List<string>(keyHistory.Split(delimiterChars));
            }
            else {
                keyHistoryList.Add(keyHistory);
            }
        }
        string gunzei = "";
        for (int n = 0; n < keyHistoryList.Count; n++) {
            string keyTemp = keyHistoryList[n];
            string keyValue = PlayerPrefs.GetString(keyTemp);
            if (n == 0) {
                gunzei = keyValue;
            }
            else {
                gunzei = gunzei + "," + keyValue;
            }
        }


        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                if (objList.Count == 0) { //never registered
                    InsertMap(userId);
                }else { //Update                    
                    objList[0]["seiryoku"] = seiryoku;
                    objList[0]["openKuni"] = openKuni;
                    objList[0]["clearedKuni"] = clearedKuni;
                    objList[0]["keyHistory"] = keyHistory;
                    objList[0]["gunzei"] = gunzei;
                    objList[0].SaveAsync();
                }
            }
        });
    }
}
