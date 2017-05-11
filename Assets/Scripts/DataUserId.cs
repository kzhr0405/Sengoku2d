using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using NCMB;

public class DataUserId : MonoBehaviour {

    public bool RegisteredFlg = false;

    

    public void InsertUserId (string userId) {
        NCMBObject userIdClass = new NCMBObject("userId");
        string platform = SystemInfo.operatingSystem;
        string appVer = Application.version;
        int kuniLv = PlayerPrefs.GetInt("kuniLv");
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
        userIdClass["myDaimyo"] = myDaimyo;
        userIdClass["addJinkei1"] = addJinkei1;
        userIdClass["addJinkei2"] = addJinkei2;
        userIdClass["addJinkei3"] = addJinkei3;
        userIdClass["addJinkei4"] = addJinkei4;

        userIdClass.SaveAsync();
        RegisteredFlg = true;
    }

    public void UpdateUserId(string userId) {

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("userId");
        query.WhereEqualTo("userId", userId);

        int kuniLv = PlayerPrefs.GetInt("kuniLv");
        int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
        bool addJinkei1 = PlayerPrefs.GetBool("addJinkei1");
        bool addJinkei2 = PlayerPrefs.GetBool("addJinkei2");
        bool addJinkei3 = PlayerPrefs.GetBool("addJinkei3");
        bool addJinkei4 = PlayerPrefs.GetBool("addJinkei4");

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
                        objList[0]["myDaimyo"] = myDaimyo;
                        objList[0]["addJinkei1"] = addJinkei1;
                        objList[0]["addJinkei2"] = addJinkei2;
                        objList[0]["addJinkei3"] = addJinkei3;
                        objList[0]["addJinkei4"] = addJinkei4;
                        objList[0].SaveAsync();
                        RegisteredFlg = true;
                    }
                }
            }
        });
    }


}
