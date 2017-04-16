using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class StatusGet : MonoBehaviour {

	Entity_busyo_mst busyoMst  = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;

	public int getHp(int busyoId, int lv){
		int returnValue = 0;
		float maxHp = (float)busyoMst.param[busyoId - 1].hp;
		float minHp = (float)busyoMst.param[busyoId - 1].minHp;

		if (lv == 1) {
			returnValue = (int)minHp;
		} else {
			float temp = 0;
			lv = lv - 1;
			temp = ((maxHp - minHp)/99)*lv + minHp;
			returnValue = Mathf.RoundToInt(temp);
		}
		return returnValue;
	}

	public int getAtk(int busyoId, int lv){
		int returnValue = 0;
		float maxAtk = (float)busyoMst.param[busyoId - 1].atk;
		float minAtk = (float)busyoMst.param[busyoId - 1].minAtk;
		
		if (lv == 1) {
			returnValue = (int)minAtk;
		} else {
			float temp = 0;
			lv = lv - 1;
			temp =((maxAtk - minAtk)/99)*lv + minAtk;
			returnValue = Mathf.RoundToInt(temp);
		}

		return returnValue;
	}


	public int getBaseAtk(int busyoId){
		int baseAtk = busyoMst.param[busyoId - 1].baseatk;

		return baseAtk;
	}

	public int getBaseDfc(int busyoId){
		int baseDfc = busyoMst.param[busyoId - 1].basedfc;

		return baseDfc;
	}


	public int getDfc(int busyoId, int lv){
		int returnValue = 0;
		float maxDfc = (float)busyoMst.param[busyoId - 1].dfc;
		float minDfc = (float)busyoMst.param[busyoId - 1].minDfc;
		
		if (lv == 1) {
			returnValue = (int)minDfc;
		} else {
			float temp = 0;
			lv = lv - 1;
			temp =((maxDfc - minDfc)/99)*lv + minDfc;
			returnValue = Mathf.RoundToInt(temp);
		}
		return returnValue;
	}

	public int getSpd(int busyoId, int lv){
        int returnValue = 0;
		float maxSpd = (float)busyoMst.param[busyoId - 1].spd;
		float minSpd = (float)busyoMst.param[busyoId - 1].minSpd;
		
		if (lv == 1) {
			returnValue = (int)minSpd;
		} else {
			float temp = 0;
			lv = lv - 1;
			temp =((maxSpd - minSpd)/99)*lv + minSpd;
			returnValue = Mathf.RoundToInt(temp);
		}

        return returnValue;
	}

	public ArrayList getSenpou(int busyoInt, bool engunFlg){
		ArrayList senpouArray = new ArrayList();

		Entity_busyo_mst busyoMst  = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;
		Entity_senpou_mst senpouMst  = Resources.Load ("Data/senpou_mst") as Entity_senpou_mst;

		//Get Senpou Id
		int senpouId = busyoMst.param [busyoInt-1].senpou_id;

		//Get Senpou Lv
		String senpouLvTmp = "senpou" + busyoInt;
		int senpouLv = PlayerPrefs.GetInt(senpouLvTmp,1);

		//Get Senpou Status
		object senpoulst = senpouMst.param[senpouId-1];
		Type t = senpoulst.GetType();
		String param = "lv" + senpouLv;
		FieldInfo f = t.GetField(param);
		int senpouStatus =(int)f.GetValue(senpoulst);

        if (Application.loadedLevelName != "touyou" && Application.loadedLevelName != "tutorialTouyou" && !engunFlg) {
            //Kahou Adjustment
            string kahouTemp = "kahou" + busyoInt;
            string busyoKahou = PlayerPrefs.GetString(kahouTemp);
            char[] delimiterChars = { ',' };
            string[] busyoKahouList = busyoKahou.Split(delimiterChars);

            Entity_kahou_heihousyo_mst Mst = Resources.Load("Data/kahou_heihousyo_mst") as Entity_kahou_heihousyo_mst;
            for (int i = 0; i < busyoKahouList.Length; i++) {
                if (i == 6) {
                    int kahouId = int.Parse(busyoKahouList[i]);
                    if (kahouId != 0) {
                        
                        //Senpou
                        float calcSenpou = ((float)senpouStatus * (float)Mst.param[kahouId - 1].kahouEffect) / 100;
                        senpouStatus = Mathf.CeilToInt((float)senpouStatus + calcSenpou);

                    }
                }
            }
        }

        //Contain in Array
        senpouArray.Add (senpouId);
		senpouArray.Add (senpouMst.param[senpouId-1].typ);
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            senpouArray.Add (senpouMst.param[senpouId-1].nameEng);
		    senpouArray.Add (senpouMst.param[senpouId-1].effectionEng);
        }else {
            senpouArray.Add(senpouMst.param[senpouId - 1].name);
            senpouArray.Add(senpouMst.param[senpouId - 1].effection);
        }
		senpouArray.Add (senpouMst.param[senpouId-1].each);
		senpouArray.Add (senpouMst.param[senpouId-1].ratio);
		senpouArray.Add (senpouMst.param[senpouId-1].term);
		senpouArray.Add (senpouStatus);
		senpouArray.Add (senpouLv);
        
        return senpouArray;
	}

	public ArrayList getEnemySenpou(int busyoId, int senpouLv, string kahouList) {
		ArrayList senpouArray = new ArrayList();

		Entity_busyo_mst busyoMst  = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;
		Entity_senpou_mst senpouMst  = Resources.Load ("Data/senpou_mst") as Entity_senpou_mst;

        if(senpouLv<1) {
            senpouLv = 1;
        }
		//Get Senpou Id
		int senpouId = busyoMst.param [busyoId-1].senpou_id;
        //Get Senpou Status
        object senpoulst = senpouMst.param[senpouId-1];
		Type t = senpoulst.GetType();
		String param = "lv" + senpouLv;
		FieldInfo f = t.GetField(param);
		int senpouStatus =(int)f.GetValue(senpoulst);
        
        if (Application.loadedLevelName == "pvpKassen") {
            //Kahou Adjustment
            char[] delimiterChars = { ',' };
            string[] busyoKahouList = kahouList.Split(delimiterChars);

            Entity_kahou_heihousyo_mst Mst = Resources.Load("Data/kahou_heihousyo_mst") as Entity_kahou_heihousyo_mst;
            for (int i = 0; i < busyoKahouList.Length; i++) {
                if (i == 6) {
                    int kahouId = int.Parse(busyoKahouList[i]);
                    if (kahouId != 0) {
                        float calcSenpou = ((float)senpouStatus * (float)Mst.param[kahouId - 1].kahouEffect) / 100;
                        senpouStatus = Mathf.CeilToInt((float)senpouStatus + calcSenpou);
                    }
                }
            }
        }
        
        //Contain in Array
        senpouArray.Add (senpouId);
		senpouArray.Add (senpouMst.param[senpouId-1].typ);
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            senpouArray.Add (senpouMst.param[senpouId-1].nameEng);
		    senpouArray.Add (senpouMst.param[senpouId-1].effectionEng);
        }else {
            senpouArray.Add(senpouMst.param[senpouId - 1].name);
            senpouArray.Add(senpouMst.param[senpouId - 1].effection);
        }
		senpouArray.Add (senpouMst.param[senpouId-1].each);
		senpouArray.Add (senpouMst.param[senpouId-1].ratio);
		senpouArray.Add (senpouMst.param[senpouId-1].term);
		senpouArray.Add (senpouStatus);
		senpouArray.Add (senpouLv);

		return senpouArray;
	}

    public bool getSenpouShipFlg(int senpouId) {       
        Entity_senpou_mst senpouMst = Resources.Load("Data/senpou_mst") as Entity_senpou_mst;
        bool shipFlg = senpouMst.param[senpouId - 1].shipFlg;
        
        return shipFlg;
    }

    public bool getSenpouOnlySeaFlg(int senpouId) {
        
        Entity_senpou_mst senpouMst = Resources.Load("Data/senpou_mst") as Entity_senpou_mst;
        bool onlySeaFlg = senpouMst.param[senpouId - 1].onlySeaFlg;

        return onlySeaFlg;
    }



    public string getBusyoName(int busyoInt){
		Entity_busyo_mst busyoMst  = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;
        string busyoName = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            busyoName = busyoMst.param [busyoInt-1].nameEng;
        }else {
            busyoName = busyoMst.param[busyoInt - 1].name;
        }
		return busyoName;

	}

	public string getHeisyu(int busyoId){
		Entity_busyo_mst busyoMst  = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;
		string heisyu = busyoMst.param [busyoId-1].heisyu;
		return heisyu;
		
	}


	public int getChHp(string type, int lv, int pa_hp){
		int returnValue = 0;
		if (lv == 0) {
			lv = 1;
		}

		Entity_lvch_mst stsMst = Resources.Load ("Data/lvch_mst") as Entity_lvch_mst;
		int startline = 0;

		if(type == "YR"){
			startline = 1;
		}else if(type == "TP"){
			startline = 2;
		}else if(type == "YM"){
			startline = 3;
		}

		object stslst = stsMst.param[startline];
		Type t = stslst.GetType();
		String param = "lv" + lv;
		FieldInfo f = t.GetField(param);
		int sts = (int)f.GetValue(stslst);
		returnValue = sts + pa_hp / 2;

		return returnValue;
	}

	public float getChAtkDfc(int ch_status, int parent_hp){
		//ch_status should be * 10 in advance
		//ch_status should be handled with LinkQty in advance

		float temp1 = (float)ch_status/10;
		float temp2 = (float)parent_hp/200;

		float atkDfc = temp1 + temp2;
		atkDfc = Mathf.Ceil (atkDfc);

		return atkDfc;
	}

}
