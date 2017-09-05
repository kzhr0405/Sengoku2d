using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DataDelete : MonoBehaviour {

	public void GunzeiDelete (string userId) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("deleteGunzei");
        query.WhereEqualTo("userId", userId);

        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                if (objList.Count != 0) { //delete
                    string objectId = objList[0].ObjectId;
                    
                    NCMBObject deleteQuery = new NCMBObject("deleteGunzei");
                    deleteQuery.ObjectId = objectId;
                    deleteQuery.DeleteAsync();

                    HPCounter deleteGunzeiScript = new HPCounter();
                    for (int i = 1; i < 66; i++) {
                        PlayerPrefs.DeleteKey("keyHistory");
                        PlayerPrefs.Flush();

                        deleteGunzeiScript.deleteEnemyGunzeiData(i);                        
                    }
                }
            }
        });
    }
}
