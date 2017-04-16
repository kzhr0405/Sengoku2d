using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class Kanni : MonoBehaviour {

	Entity_kanni_mst kanniMst = Resources.Load ("Data/kanni_mst") as Entity_kanni_mst;

	public int getRandomKanni(int syoukaijyoRank, int kuniQty){

		int kanniId = 0;

		//MySeiryoku
		string seiryoku = PlayerPrefs.GetString ("seiryoku");
		List<string> seiryokuList = new List<string> ();
		char[] delimiterChars = {','};
		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
		int myDaimyoId = PlayerPrefs.GetInt ("myDaimyo");
		List<int> mySeiryokuList = new List<int> ();

		for (int a=0; a<seiryokuList.Count; a++) {
			int seiryokuId = int.Parse (seiryokuList [a]);
			if (seiryokuId == myDaimyoId) {
				int kuniId = a + 1;
				mySeiryokuList.Add(kuniId);
			}
		}

		//MyKanni
		string myKanni = PlayerPrefs.GetString ("myKanni");
		List<string> myKanniList = new List<string> ();
		if (myKanni != null && myKanni != "") {
			if(myKanni.Contains(",")){
				myKanniList = new List<string> (myKanni.Split (delimiterChars));
			}else{
				myKanniList.Add(myKanni);
			}
		}


		//Make TargetList
		List<int> targetKanniList = new List<int> ();
		char[] delimiterChars2 = {':'};
		for(int i=0; i<kanniMst.param.Count; i++){
			int tmpSyoukaijyoRank = kanniMst.param[i].SyoukaijyoRank;

			if(tmpSyoukaijyoRank<=syoukaijyoRank){

				int needKuniQty = kanniMst.param[i].NeedKuniQty;
				if(needKuniQty<=kuniQty){

					string kuniId = kanniMst.param[i].TargetKuni;
					if(kuniId == "null"){
						int tmp = i + 1;
						if(!myKanniList.Contains(tmp.ToString ())){
							targetKanniList.Add(tmp);
						}
					}else{
						if(kuniId.Contains(":")){
							List<string> kuniIdList = new List<string> ();
							kuniIdList = new List<string> (kuniId.Split (delimiterChars2));

							for(int j=0; j<kuniIdList.Count; j++){
								int tempKuniId = int.Parse(kuniIdList[j]);

								if(mySeiryokuList.Contains(tempKuniId)){
									if(!myKanniList.Contains(tempKuniId.ToString ())){
										targetKanniList.Add(tempKuniId);
									}
								}
							}

						}else{
							int tmp = i + 1;
							if(mySeiryokuList.Contains(tmp)){
								if(!myKanniList.Contains(tmp.ToString ())){
									targetKanniList.Add(tmp);
								}
							}
						}
					}
				}
			}
		}

		//Random Select
		if (targetKanniList.Count > 0) {
			int rdmId = UnityEngine.Random.Range(0,targetKanniList.Count);
			kanniId = targetKanniList [rdmId];
		}

		return kanniId;
	}

	public string getKanniName(int kanniId){
		string totalName = "";
		
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            totalName = kanniMst.param[kanniId - 1].IkaiEng;
        }else {
            string ikai = kanniMst.param[kanniId - 1].Ikai;
            string kanni = kanniMst.param[kanniId - 1].Kanni;
            totalName = ikai + kanni;
        }
		return totalName;	
	}

	public string getKanni(int kanniId){

		string kanni = kanniMst.param [kanniId - 1].Kanni;
		
		return kanni;	
	}

	public string getIkai(int kanniId){
        string ikai = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            ikai = kanniMst.param[kanniId - 1].IkaiEng;
        }else {
            ikai = kanniMst.param[kanniId - 1].Ikai;
        }
		return ikai;	
	}

	public string getEffectLabel(int kanniId){
		string effectLabel = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            effectLabel = kanniMst.param[kanniId - 1].EffectLabelEng;
        }else {
            effectLabel = kanniMst.param[kanniId - 1].EffectLabel;
        }
		return effectLabel;	
	}

	public int getEffect(int kanniId){
		int effect = 0;
		effect = kanniMst.param[kanniId - 1].Effect;

		return effect;	
	}

	public string getEffectTarget(int kanniId){
		string effectTarget = "";
		effectTarget = kanniMst.param[kanniId - 1].EffectTarget;
		
		return effectTarget;	
	}

	public int getKuniKanni(int kuniId){
		int kanniId = 0;

		for (int i=0; i<kanniMst.param.Count; i++) {
			string tmpKuniId = kanniMst.param[i].TargetKuni;

			if(tmpKuniId != "null"){
				if(tmpKuniId.Contains(":")){

					List<string> tmpKuniIdList = new List<string> ();
					char[] delimiterChars = {':'};
					tmpKuniIdList = new List<string> (tmpKuniId.Split (delimiterChars));
					if(tmpKuniIdList.Contains(kuniId.ToString())){
						kanniId = kanniMst.param[i].No;
						break;
					}

				}else{
					if(kuniId == int.Parse(tmpKuniId)){
						kanniId = kanniMst.param[i].No;
						break;
					}
				}
			}
		}

		return kanniId;
	}
}
