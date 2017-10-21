using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class NaiseiInfo : MonoBehaviour {

	public int getNaiseiEffect(string code, int lv){
        int effect = 0;
        if (lv!=0) {
		    List<int> naiseiEffectList = new List<int>();
		    Entity_naisei_mst naiseiMst = Resources.Load ("Data/naisei_mst") as Entity_naisei_mst;
		    int startline = 0;
		    for(int i=0;i<naiseiMst.param.Count;i++){
			    if(naiseiMst.param[i].code == code){
				    startline = i;
				    break;
			    }
		    }

		    object effectLst = naiseiMst.param[startline];
		    Type t = effectLst.GetType();

		    //Effect on Current Lv
		    String param1 = "effect" + lv;
		    FieldInfo f1 = t.GetField(param1);
		    effect = (int)f1.GetValue(effectLst);
        }
        return effect;
	}

    public string getNaiseiName(int id) {
        string value = "";
        Entity_naisei_mst naiseiMst = Resources.Load("Data/naisei_mst") as Entity_naisei_mst;

        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            value = naiseiMst.param[id].nameEng;
        }else {
            value = naiseiMst.param[id].name;
        }

        return value;
    }

    public string getNaiseiExp(int id) {
        string value = "";
        Entity_naisei_mst naiseiMst = Resources.Load("Data/naisei_mst") as Entity_naisei_mst;

        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            value = naiseiMst.param[id].expEng;
        }
        else {
            value = naiseiMst.param[id].exp;
        }

        return value;
    }
}
