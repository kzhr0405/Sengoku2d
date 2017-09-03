using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

public class DataReward : MonoBehaviour {

    public List<string> objectIdList;
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
                    objectIdList.Add(obj.ObjectId);
                    if (checkDataExist(obj, "title")) itemTitleList.Add(System.Convert.ToString(obj["title"]));
                    if (checkDataExist(obj, "grp")) itemGrpList.Add(System.Convert.ToString(obj["grp"]));
                    if (checkDataExist(obj, "rank")) itemRankList.Add(System.Convert.ToString(obj["rank"]));
                    if (checkDataExist(obj, "qty")) itemQtyList.Add(System.Convert.ToInt32(obj["qty"]));                 
                }
            }
        });
    }

    private bool checkDataExist(NCMBObject obj, string key) {
        try {
            object test = obj[key];
        }
        catch (NCMBException ex) {
            return false;
        }
        return true;
    }


}
