using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

public class NCMBDataMaker : MonoBehaviour {

	// Use this for initialization
	void Start () {

        //425まで実施ずみ
        int fromRank = 426;
        int toRank = 430;

        List<string> grpList = new List<string>{"busyo","kaho","syokaijyo","money"};
        List<int> qtyList = new List<int> { 1, 2, 1, 30000 };
        List<string> rankList = new List<string> { "B", "B", "B", "C" };

        for (int i= fromRank; i< toRank + 1; i++) {
            for(int j=0; j<4; j++) {
                NCMBObject query = new NCMBObject("reward");
                query["title"] = "合戦場報酬" + i + "位";
                query["grp"] = grpList[j];
                query["qty"] = qtyList[j];
                query["rank"] = rankList[j];
                query.SaveAsync();
            }
        }

    }

}
