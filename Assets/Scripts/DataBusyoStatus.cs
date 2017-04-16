using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using NCMB;

public class DataBusyoStatus : MonoBehaviour {

    public void InsertBusyoStatus(string userId, string busyoId) {
        
        NCMBObject busyoStatusClass = new NCMBObject("myBusyoStatus");
            
        string tempHei = "hei" + busyoId;
        string tempSenpou = "senpou" + busyoId;
        string tempSaku = "saku" + busyoId;
        string tempKahou = "kahou" + busyoId;
        string tempExp = "exp" + busyoId;
        string hei = PlayerPrefs.GetString(tempHei);
        int senpou = PlayerPrefs.GetInt(tempSenpou);
        int saku = PlayerPrefs.GetInt(tempSaku);
        string kahou = PlayerPrefs.GetString(tempKahou);
        int lv = PlayerPrefs.GetInt(busyoId);
        int exp = PlayerPrefs.GetInt(tempExp);

        busyoStatusClass["userId"] = userId;
        busyoStatusClass["busyoId"] = busyoId;
        busyoStatusClass["hei"] = hei;
        busyoStatusClass["senpou"] = senpou;
        busyoStatusClass["saku"] = saku;
        busyoStatusClass["kahou"] = kahou;
        busyoStatusClass["lv"] = lv;
        busyoStatusClass["exp"] = exp;
        busyoStatusClass.SaveAsync();
        

    }



    public void UpdateBusyoStatus(string userId, string busyoId) {
        
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("myBusyoStatus");
        query.WhereEqualTo("userId", userId);
        query.WhereEqualTo("busyoId", busyoId);

        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                if (objList.Count == 0) { //never registered
                    InsertBusyoStatus(userId, busyoId);
                }else { //Update
                    
                    string tempHei = "hei" + busyoId;
                    string tempSenpou = "senpou" + busyoId;
                    string tempSaku = "saku" + busyoId;
                    string tempKahou = "kahou" + busyoId;
                    string tempExp = "exp" + busyoId;
                    string hei = PlayerPrefs.GetString(tempHei);
                    int senpou = PlayerPrefs.GetInt(tempSenpou);
                    int saku = PlayerPrefs.GetInt(tempSaku);
                    string kahou = PlayerPrefs.GetString(tempKahou);
                    int exp = PlayerPrefs.GetInt(tempExp);
                    int lv = PlayerPrefs.GetInt(busyoId);

                    objList[0]["busyoId"] = busyoId;
                    objList[0]["hei"] = hei;
                    objList[0]["senpou"] = senpou;
                    objList[0]["saku"] = saku;
                    objList[0]["kahou"] = kahou;
                    objList[0]["exp"] = exp;
                    objList[0]["lv"] = lv ;
                    objList[0].SaveAsync();
                }
            }
        });
        

        
    }
}
