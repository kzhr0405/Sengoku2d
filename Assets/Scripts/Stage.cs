using UnityEngine;
using System.Collections;

public class Stage : MonoBehaviour {

	Entity_stage_mst Mst  = Resources.Load ("Data/stage_mst") as Entity_stage_mst;

	public int getStageMap(int kuniId, int stageId){
		int stageMapId = 0;
		int targetline = ((kuniId - 1) * 10) + stageId - 1;

		stageMapId = Mst.param [targetline].stageMap;
        return stageMapId;
	}

	public string getStageName(int kuniId , int stageId){
		
		string stageName = "";
		int targetline = ((kuniId - 1) * 10) + stageId - 1;

        if (Application.systemLanguage != SystemLanguage.Japanese) {
            stageName = Mst.param [targetline].stageNameEng;
        }else {
            stageName = Mst.param[targetline].stageName;
        }
		return stageName;
	}

	public int getPowerTyp(int kuniId , int stageId){

		int powerTyp = 0;
		int targetline = ((kuniId - 1) * 10) + stageId - 1;

		powerTyp = Mst.param [targetline].powerTyp;

		return powerTyp;
	}


}
