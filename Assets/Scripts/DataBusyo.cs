using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using NCMB;

public class DataBusyo : MonoBehaviour {

    public void InsertBusyo(string userId) {
        string myBusyoString = PlayerPrefs.GetString("myBusyo");
        string zukanBusyoHst = PlayerPrefs.GetString("zukanBusyoHst");
        string gacyaDaimyoHst = PlayerPrefs.GetString("gacyaDaimyoHst");

        NCMBObject busyoClass = new NCMBObject("myBusyo");
        busyoClass["userId"] = userId;
        busyoClass["myBusyo"] = myBusyoString;
        busyoClass["zukanBusyoHst"] = zukanBusyoHst;
        busyoClass["gacyaDaimyoHst"] = gacyaDaimyoHst;
        busyoClass.SaveAsync();
    }



    public void UpdateBusyo(string userId) {

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("myBusyo");
        query.WhereEqualTo("userId", userId);

        string myBusyoString = PlayerPrefs.GetString("myBusyo");
        string zukanBusyoHst = PlayerPrefs.GetString("zukanBusyoHst");
        string gacyaDaimyoHst = PlayerPrefs.GetString("gacyaDaimyoHst");

        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                if (objList.Count == 0) { //never registered
                    InsertBusyo(userId);
                }else { //Update                    
                    objList[0]["myBusyo"] = myBusyoString;
                    objList[0]["zukanBusyoHst"] = zukanBusyoHst;
                    objList[0]["gacyaDaimyoHst"] = gacyaDaimyoHst;
                    objList[0].SaveAsync();
                }
            }
        });
    }
}
