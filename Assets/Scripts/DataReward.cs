using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

public class DataReward : MonoBehaviour {

    public List<string> itemTitleList;
    public List<string> itemGrpList;
    public List<string> itemRankList;
    public List<int> itemQtyList;

    public void GetRewardMaster(string userId) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("reward");
        query.WhereEqualTo("userId", userId);
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                foreach (NCMBObject obj in objList) {
                    itemTitleList.Add(System.Convert.ToString(obj["title"]));
                    itemGrpList.Add(System.Convert.ToString(obj["grp"]));
                    itemRankList.Add(System.Convert.ToString(obj["rank"]));
                    itemQtyList.Add(System.Convert.ToInt32(obj["qty"]));                 
                }
            }
        });
    }

    
}
