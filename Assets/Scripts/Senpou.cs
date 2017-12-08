using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class Senpou : MonoBehaviour {

    Entity_senpou_mst senpouMst = Resources.Load("Data/senpou_mst") as Entity_senpou_mst;
    public string getName (int senpouId) {
        string senpouName = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            senpouName = senpouMst.param[senpouId - 1].nameEng;
        }else if (langId == 3) {
            senpouName = senpouMst.param[senpouId - 1].nameSChn;
        }else {
            senpouName = senpouMst.param[senpouId - 1].name;
        }
        return senpouName;
    }
    
    public string getEffectionLv1(int senpouId) {
        int senpouStatus = senpouMst.param[senpouId - 1].lv1;
        int each = (int)senpouMst.param[senpouId - 1].each;
        int ratio = (int)senpouMst.param[senpouId - 1].ratio;
        int term = (int)senpouMst.param[senpouId - 1].term;
        string senpouExp = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            senpouExp = senpouMst.param[senpouId - 1].effectionEng;
            senpouExp = senpouExp.Replace("ABC", senpouStatus.ToString());
            senpouExp = senpouExp.Replace("DEF", each.ToString());
            senpouExp = senpouExp.Replace("GHI", ratio.ToString());
            senpouExp = senpouExp.Replace("JKL", term.ToString());
        }else if (langId == 3) {
            senpouExp = senpouMst.param[senpouId - 1].effectionSChn;
            senpouExp = senpouExp.Replace("A", senpouStatus.ToString());
            senpouExp = senpouExp.Replace("B", each.ToString());
            senpouExp = senpouExp.Replace("C", ratio.ToString());
            senpouExp = senpouExp.Replace("D", term.ToString());
        }else {
            senpouExp = senpouMst.param[senpouId - 1].effection;
            senpouExp = senpouExp.Replace("A", senpouStatus.ToString());
            senpouExp = senpouExp.Replace("B", each.ToString());
            senpouExp = senpouExp.Replace("C", ratio.ToString());
            senpouExp = senpouExp.Replace("D", term.ToString());
        }
        return senpouExp;
    }
}
