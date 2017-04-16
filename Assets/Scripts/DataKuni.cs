using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using NCMB;

public class DataKuni : MonoBehaviour {

    public void InsertKuni(string userId, int kuniId) {
        string naiseiTmp = "naisei" + kuniId.ToString();
        string jyosyuTmp = "jyosyu" + kuniId.ToString();

        string naisei = PlayerPrefs.GetString(naiseiTmp);
        string jyosyu = PlayerPrefs.GetString(jyosyuTmp);
        NCMBObject kuniClass = new NCMBObject("kuni");
        kuniClass["userId"] = userId;
        kuniClass["kuniId"] = kuniId;
        kuniClass["naisei"] = naisei;
        kuniClass["jyosyu"] = jyosyu;
        kuniClass.SaveAsync();
    }



    public void UpdateKuni(string userId, int kuniId) {

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("kuni");
        query.WhereEqualTo("userId", userId);
        query.WhereEqualTo("kuniId", kuniId);

        string naiseiTmp = "naisei" + kuniId.ToString();
        string jyosyuTmp = "jyosyu" + kuniId.ToString();
        
        string naisei = PlayerPrefs.GetString(naiseiTmp);
        string jyosyu = PlayerPrefs.GetString(jyosyuTmp);
        

        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                if (objList.Count == 0) { //never registered
                    InsertKuni(userId,kuniId);
                }else { //Update                    
                    objList[0]["kuniId"] = kuniId;
                    objList[0]["naisei"] = naisei;
                    objList[0]["jyosyu"] = jyosyu;
                    objList[0].SaveAsync();
                }
            }
        });
    }
}
