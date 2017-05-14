using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

public class NCMBDataMaker : MonoBehaviour {

	// Use this for initialization
	void Start () {

        
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvpWeekly");
        //string userId = PlayerPrefs.GetString("userId");
        //query.WhereEqualTo("userId", userId);
        query.OrderByDescending("totalPt");
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                foreach (NCMBObject obj in objList) {                 
                    obj["totalPt"] = 0;
                    obj.SaveAsync();                    
                }
            }
        });
        


    }

}
