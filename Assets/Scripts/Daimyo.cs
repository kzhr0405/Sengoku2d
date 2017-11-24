using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class Daimyo : MonoBehaviour {

	Entity_daimyo_mst daimyoMst = Resources.Load ("Data/daimyo_mst") as Entity_daimyo_mst;

    public string getName (int daimyoId, int langId, int senarioId ) {
		string daimyoName = "";
		if (daimyoId != 0) {
            int busyoId = 0;         
            if (senarioId == 1) {
                busyoId = daimyoMst.param[daimyoId - 1].busyoId1;
            }else if (senarioId == 2) {
                busyoId = daimyoMst.param[daimyoId - 1].busyoId2;
            } else if(senarioId == 3) {
                busyoId = daimyoMst.param [daimyoId - 1].busyoId3;
            }else {
                busyoId = daimyoMst.param[daimyoId - 1].busyoId;
            }
            BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();
            //Debug.Log(senarioId + "," + daimyoId + "," + busyoId);
            daimyoName = BusyoInfoGet.getName(busyoId, langId);

        }
		return daimyoName;
	}

	public int getDaimyoBusyoId (int daimyoId, int senarioId) {
        int busyoId = 0;
        if (senarioId == 1) {
            busyoId = daimyoMst.param[daimyoId - 1].busyoId1;
        }else if (senarioId == 2) {
            busyoId = daimyoMst.param[daimyoId - 1].busyoId2;
        }else if (senarioId == 3) {
            busyoId = daimyoMst.param[daimyoId - 1].busyoId3;
        }else {
            busyoId = daimyoMst.param[daimyoId - 1].busyoId;
        }
        return busyoId;
	}

	public int getSenryoku (int daimyoId) {
		int senryoku = daimyoMst.param[daimyoId-1].senryoku;
		return senryoku;
	}

	public bool daimyoBusyoCheck(int busyoId){
		bool daimyoFlg = false;

		for(int i=0; i<daimyoMst.param.Count; i++){
			int tempBusyoId = daimyoMst.param[i].busyoId;
			if(busyoId == tempBusyoId){
				daimyoFlg = true;
			}
		}
		return daimyoFlg;
	}

	public float getColorR(int daimyoId){
		float colorR = 0;
		colorR = (float)daimyoMst.param[daimyoId-1].colorR;
		return colorR;
	}

	public float getColorG(int daimyoId){
		float colorG = 0;
		colorG = (float)daimyoMst.param[daimyoId-1].colorG;
		return colorG;
	}

	public float getColorB(int daimyoId){
		float colorB = 0;
		colorB = (float)daimyoMst.param[daimyoId-1].colorB;
		return colorB;
	}

	public bool checkRemain1DaimyoOnMain(int myDaimyo){
		bool remain1DaimyoFlg = false;

		string seiryoku = PlayerPrefs.GetString ("seiryoku");
		List<string> seiryokuList = new List<string> ();
		char[] delimiterChars = {','};
		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
		List<int> checkedList = new List<int> ();

		for (int i = 0; i < seiryokuList.Count; i++) {
			int daimyoId = int.Parse(seiryokuList [i]);
			if (daimyoId != myDaimyo) {
				if (!checkedList.Contains (daimyoId)) {
					checkedList.Add (daimyoId);
				}
			}
		}

		if (checkedList.Count == 1) {
			remain1DaimyoFlg = true;
		}

		return remain1DaimyoFlg;
	}

    public string getClanName(int daimyoId, int langId) {
        string clanName = "";
        if (daimyoId != 0) {
            if (langId == 2) {
                clanName = daimyoMst.param[daimyoId - 1].clanNameEng;
            } else if(langId==3) {
                clanName = daimyoMst.param[daimyoId - 1].clanNameSChn;
            } else {
                clanName = daimyoMst.param[daimyoId - 1].clanName;
            }
        }
        return clanName;
    }
}
