using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

public class DataReward : MonoBehaviour {

    public List<string> itemTitleList;
    public List<string> itemGrpList;
    public List<string> itemTypList;
    public List<int> itemIdList;
    public List<int> itemQtyList;

    public void GetRewardMaster(string userId) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("reward");
        query.WhereEqualTo("userId", userId);
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                foreach (NCMBObject obj in objList) {
                    itemTitleList.Add(System.Convert.ToString(obj["title"]));
                    itemGrpList.Add(System.Convert.ToString(obj["grp"]));
                    itemTypList.Add(System.Convert.ToString(obj["typ"]));
                    itemIdList.Add(System.Convert.ToInt32(obj["id"]));
                    itemQtyList.Add(System.Convert.ToInt32(obj["qty"]));                    
                }
            }
        });
    }

    
}
