using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class KahouStatusGet : MonoBehaviour {
	
	public string[] getKahouForStatus(string busyoId,int hp, int atk, int dfc,int spd ){
		string temp = "kahou" + busyoId;
		string busyoKahou = PlayerPrefs.GetString (temp);
        if (busyoKahou == "" || busyoKahou == null) {
            busyoKahou = "0,0,0,0,0,0,0,0";
            PlayerPrefs.SetString(temp, busyoKahou);
            PlayerPrefs.Flush();
        }
        char[] delimiterChars = {','};
		string[] busyoKahouList = busyoKahou.Split (delimiterChars);

		string[] array = new string[4];
		for(int i=0;i<busyoKahouList.Length;i++){
			int kahouId = int.Parse(busyoKahouList[i]);
			if(i==0){
				string attackString ="0";
				if(kahouId !=0){
					//Buyuu
					Entity_kahou_bugu_mst Mst = Resources.Load ("Data/kahou_bugu_mst") as Entity_kahou_bugu_mst;

					if(Mst.param [kahouId - 1].unit == "%"){
						int calcAttack =  (atk * Mst.param [kahouId - 1].kahouEffect)/100;
						attackString = calcAttack.ToString();
					}else{
						attackString = 	(Mst.param [kahouId - 1].kahouEffect).ToString();		
					}
				}
				array[i] = attackString;
			}else if(i==1){
				string hpString ="0";
				if(kahouId !=0){
					//Tosotsu
					Entity_kahou_kabuto_mst Mst = Resources.Load ("Data/kahou_kabuto_mst") as Entity_kahou_kabuto_mst;
					
					if(Mst.param [kahouId - 1].unit == "%"){
						int calcHp = (hp * Mst.param [kahouId - 1].kahouEffect)/100;
						hpString = calcHp.ToString();
					}else{
						hpString = 	(Mst.param [kahouId - 1].kahouEffect).ToString();		
					}
				}
				array[i] = hpString;
			}else if(i==2){
				string dfcString ="0";
				if(kahouId !=0){
					//dfc
					Entity_kahou_gusoku_mst Mst = Resources.Load ("Data/kahou_gusoku_mst") as Entity_kahou_gusoku_mst;
					
					if(Mst.param [kahouId - 1].unit == "%"){
						int calcDfc = (dfc * Mst.param [kahouId - 1].kahouEffect)/100;
						dfcString = calcDfc.ToString();
					}else{
						dfcString = (Mst.param [kahouId - 1].kahouEffect).ToString();		
					}
				}
				array[i] = dfcString;
			}else if(i==3){
				string spdString ="0";
				if(kahouId !=0){
					//spd
					Entity_kahou_meiba_mst Mst = Resources.Load ("Data/kahou_meiba_mst") as Entity_kahou_meiba_mst;
					
					if(Mst.param [kahouId - 1].unit == "%"){
						int calcSpd =  (spd * Mst.param [kahouId - 1].kahouEffect)/100;
						//Kyuusai Sochi
						if(calcSpd==0){
							calcSpd=1;
						}
						spdString = calcSpd.ToString();

					}else{
						spdString = (Mst.param [kahouId - 1].kahouEffect).ToString();		
					}
				}
				array[i] = spdString;
			}
		}
		return array;
	}
	public string[] getKahouForSenpou(string busyoId, int senpouStatus){
		string temp = "kahou" + busyoId;
		string busyoKahou = PlayerPrefs.GetString (temp);
        if (busyoKahou == "" || busyoKahou == null) {
            busyoKahou = "0,0,0,0,0,0,0,0";
            PlayerPrefs.SetString(temp, busyoKahou);
            PlayerPrefs.Flush();
        }

        char[] delimiterChars = {','};
		string[] busyoKahouList = busyoKahou.Split (delimiterChars);

		//Type(Attack/Ratio/...) & Effection
		string[] array = new string[2];
		for(int i=0;i<busyoKahouList.Length;i++){
			int kahouId = int.Parse(busyoKahouList[i]);
			if(i==6){
				Entity_kahou_heihousyo_mst Mst = Resources.Load ("Data/kahou_heihousyo_mst") as Entity_kahou_heihousyo_mst;
				string senpouString ="0";

				if(kahouId !=0){
					//Senpou
					if(Mst.param[kahouId-1].senjyutsuTarget =="Attack"){
						if(Mst.param [kahouId - 1].unit == "%"){
							int calcSenpou =  (senpouStatus * Mst.param [kahouId - 1].kahouEffect)/100;
							senpouString = calcSenpou.ToString();
						}else{
							senpouString = 	(Mst.param [kahouId - 1].kahouEffect).ToString();		
						}
					}else{
						Debug.Log ("Not Yet except for Attack");
					}
					array[0] = Mst.param[kahouId-1].senjyutsuTarget;
					array[1] = senpouString;

				}
			}
		}
		return array;
	}

	public List<string> getKahouInfo(string kahouTyp, int kahouId){

		List<string> kahouInfoList = new List<string> ();

		if (kahouTyp == "bugu") {
			Entity_kahou_bugu_mst buguKahouMst  = Resources.Load ("Data/kahou_bugu_mst") as Entity_kahou_bugu_mst;
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                kahouInfoList.Add(buguKahouMst.param [kahouId-1].kahouNameEng);
                kahouInfoList.Add(buguKahouMst.param[kahouId - 1].kahouExpEng);
                kahouInfoList.Add(buguKahouMst.param[kahouId - 1].kahouTargetEng);
            }else {
                kahouInfoList.Add(buguKahouMst.param[kahouId - 1].kahouName);
                kahouInfoList.Add(buguKahouMst.param[kahouId - 1].kahouExp);
                kahouInfoList.Add(buguKahouMst.param[kahouId - 1].kahouTarget);
            }
			kahouInfoList.Add(buguKahouMst.param [kahouId-1].kahouEffect.ToString());
			kahouInfoList.Add(buguKahouMst.param [kahouId-1].unit.ToString());
			kahouInfoList.Add(buguKahouMst.param [kahouId-1].kahouBuy.ToString());
			kahouInfoList.Add(buguKahouMst.param [kahouId-1].kahouSell.ToString());
			kahouInfoList.Add(buguKahouMst.param [kahouId-1].kahouRatio.ToString());

		}else if(kahouTyp == "kabuto"){
			Entity_kahou_kabuto_mst kabutoKahouMst  = Resources.Load ("Data/kahou_kabuto_mst") as Entity_kahou_kabuto_mst;
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                kahouInfoList.Add(kabutoKahouMst.param[kahouId - 1].kahouNameEng);
                kahouInfoList.Add(kabutoKahouMst.param[kahouId - 1].kahouExpEng);
                kahouInfoList.Add(kabutoKahouMst.param[kahouId - 1].kahouTargetEng);
            }else {
                kahouInfoList.Add(kabutoKahouMst.param[kahouId - 1].kahouName);
                kahouInfoList.Add(kabutoKahouMst.param[kahouId - 1].kahouExp);
                kahouInfoList.Add(kabutoKahouMst.param[kahouId - 1].kahouTarget);
            }
            kahouInfoList.Add(kabutoKahouMst.param [kahouId-1].kahouEffect.ToString());
			kahouInfoList.Add(kabutoKahouMst.param [kahouId-1].unit.ToString());
			kahouInfoList.Add(kabutoKahouMst.param [kahouId-1].kahouBuy.ToString());
			kahouInfoList.Add(kabutoKahouMst.param [kahouId-1].kahouSell.ToString());
			kahouInfoList.Add(kabutoKahouMst.param [kahouId-1].kahouRatio.ToString());

		}else if(kahouTyp == "gusoku"){
			Entity_kahou_gusoku_mst gusokuKahouMst  = Resources.Load ("Data/kahou_gusoku_mst") as Entity_kahou_gusoku_mst;
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                kahouInfoList.Add(gusokuKahouMst.param[kahouId - 1].kahouNameEng);
                kahouInfoList.Add(gusokuKahouMst.param[kahouId - 1].kahouExpEng);
                kahouInfoList.Add(gusokuKahouMst.param[kahouId - 1].kahouTargetEng);
            }else {
                kahouInfoList.Add(gusokuKahouMst.param[kahouId - 1].kahouName);
                kahouInfoList.Add(gusokuKahouMst.param[kahouId - 1].kahouExp);
                kahouInfoList.Add(gusokuKahouMst.param[kahouId - 1].kahouTarget);
            }
            kahouInfoList.Add(gusokuKahouMst.param [kahouId-1].kahouEffect.ToString());
			kahouInfoList.Add(gusokuKahouMst.param [kahouId-1].unit.ToString());
			kahouInfoList.Add(gusokuKahouMst.param [kahouId-1].kahouBuy.ToString());
			kahouInfoList.Add(gusokuKahouMst.param [kahouId-1].kahouSell.ToString());
			kahouInfoList.Add(gusokuKahouMst.param [kahouId-1].kahouRatio.ToString());
		
		}else if(kahouTyp == "meiba"){
			Entity_kahou_meiba_mst meibaKahouMst  = Resources.Load ("Data/kahou_meiba_mst") as Entity_kahou_meiba_mst;
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                kahouInfoList.Add(meibaKahouMst.param[kahouId - 1].kahouNameEng);
                kahouInfoList.Add(meibaKahouMst.param[kahouId - 1].kahouExpEng);
                kahouInfoList.Add(meibaKahouMst.param[kahouId - 1].kahouTargetEng);
            }else {
                kahouInfoList.Add(meibaKahouMst.param[kahouId - 1].kahouName);
                kahouInfoList.Add(meibaKahouMst.param[kahouId - 1].kahouExp);
                kahouInfoList.Add(meibaKahouMst.param[kahouId - 1].kahouTarget);
            }
            kahouInfoList.Add(meibaKahouMst.param [kahouId-1].kahouEffect.ToString());
			kahouInfoList.Add(meibaKahouMst.param [kahouId-1].unit.ToString());
			kahouInfoList.Add(meibaKahouMst.param [kahouId-1].kahouBuy.ToString());
			kahouInfoList.Add(meibaKahouMst.param [kahouId-1].kahouSell.ToString());
			kahouInfoList.Add(meibaKahouMst.param [kahouId-1].kahouRatio.ToString());
		
		}else if(kahouTyp == "cyadougu"){
			Entity_kahou_cyadougu_mst cyadouguKahouMst  = Resources.Load ("Data/kahou_cyadougu_mst") as Entity_kahou_cyadougu_mst;
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                kahouInfoList.Add(cyadouguKahouMst.param[kahouId - 1].kahouNameEng);
                kahouInfoList.Add(cyadouguKahouMst.param[kahouId - 1].kahouExpEng);
                kahouInfoList.Add(cyadouguKahouMst.param[kahouId - 1].kahouTargetEng);
            }else {
                kahouInfoList.Add(cyadouguKahouMst.param[kahouId - 1].kahouName);
                kahouInfoList.Add(cyadouguKahouMst.param[kahouId - 1].kahouExp);
                kahouInfoList.Add(cyadouguKahouMst.param[kahouId - 1].kahouTarget);
            }
            kahouInfoList.Add(cyadouguKahouMst.param [kahouId-1].kahouEffect.ToString());
			kahouInfoList.Add(cyadouguKahouMst.param [kahouId-1].unit.ToString());
			kahouInfoList.Add(cyadouguKahouMst.param [kahouId-1].kahouBuy.ToString());
			kahouInfoList.Add(cyadouguKahouMst.param [kahouId-1].kahouSell.ToString());
			kahouInfoList.Add(cyadouguKahouMst.param [kahouId-1].kahouRatio.ToString());
		
		}else if(kahouTyp == "heihousyo"){
			Entity_kahou_heihousyo_mst heihousyoKahouMst  = Resources.Load ("Data/kahou_heihousyo_mst") as Entity_kahou_heihousyo_mst;
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                kahouInfoList.Add(heihousyoKahouMst.param[kahouId - 1].kahouNameEng);
                kahouInfoList.Add(heihousyoKahouMst.param[kahouId - 1].kahouExpEng);
                kahouInfoList.Add(heihousyoKahouMst.param[kahouId - 1].kahouTargetEng);
            }else {
                kahouInfoList.Add(heihousyoKahouMst.param[kahouId - 1].kahouName);
                kahouInfoList.Add(heihousyoKahouMst.param[kahouId - 1].kahouExp);
                kahouInfoList.Add(heihousyoKahouMst.param[kahouId - 1].kahouTarget);
            }
            kahouInfoList.Add(heihousyoKahouMst.param [kahouId-1].kahouEffect.ToString());
			kahouInfoList.Add(heihousyoKahouMst.param [kahouId-1].unit.ToString());
			kahouInfoList.Add(heihousyoKahouMst.param [kahouId-1].kahouBuy.ToString());
			kahouInfoList.Add(heihousyoKahouMst.param [kahouId-1].kahouSell.ToString());
			kahouInfoList.Add(heihousyoKahouMst.param [kahouId-1].kahouRatio.ToString());

		}else if(kahouTyp == "chishikisyo"){
			Entity_kahou_chishikisyo_mst chishikisyoKahouMst  = Resources.Load ("Data/kahou_chishikisyo_mst") as Entity_kahou_chishikisyo_mst;
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                kahouInfoList.Add(chishikisyoKahouMst.param[kahouId - 1].kahouNameEng);
                kahouInfoList.Add(chishikisyoKahouMst.param[kahouId - 1].kahouExpEng);
                kahouInfoList.Add(chishikisyoKahouMst.param[kahouId - 1].kahouTargetEng);
            }else {
                kahouInfoList.Add(chishikisyoKahouMst.param[kahouId - 1].kahouName);
                kahouInfoList.Add(chishikisyoKahouMst.param[kahouId - 1].kahouExp);
                kahouInfoList.Add(chishikisyoKahouMst.param[kahouId - 1].kahouTarget);
            }
            kahouInfoList.Add(chishikisyoKahouMst.param [kahouId-1].kahouEffect.ToString());
			kahouInfoList.Add(chishikisyoKahouMst.param [kahouId-1].unit.ToString());
			kahouInfoList.Add(chishikisyoKahouMst.param [kahouId-1].kahouBuy.ToString());
			kahouInfoList.Add(chishikisyoKahouMst.param [kahouId-1].kahouSell.ToString());
			kahouInfoList.Add(chishikisyoKahouMst.param [kahouId-1].kahouRatio.ToString());

		}
		
		return kahouInfoList;
	}

    public string[] getPvPKahouForStatus(string kahouList, int hp, int atk, int dfc, int spd) {
       
        char[] delimiterChars = { ',' };
        string[] busyoKahouList = kahouList.Split(delimiterChars);

        string[] array = new string[4];
        for (int i = 0; i < busyoKahouList.Length; i++) {
            int kahouId = int.Parse(busyoKahouList[i]);
            if (i == 0) {
                string attackString = "0";
                if (kahouId != 0) {
                    //Buyuu
                    Entity_kahou_bugu_mst Mst = Resources.Load("Data/kahou_bugu_mst") as Entity_kahou_bugu_mst;

                    if (Mst.param[kahouId - 1].unit == "%") {
                        int calcAttack = (atk * Mst.param[kahouId - 1].kahouEffect) / 100;
                        attackString = calcAttack.ToString();
                    }
                    else {
                        attackString = (Mst.param[kahouId - 1].kahouEffect).ToString();
                    }
                }
                array[i] = attackString;
            }
            else if (i == 1) {
                string hpString = "0";
                if (kahouId != 0) {
                    //Tosotsu
                    Entity_kahou_kabuto_mst Mst = Resources.Load("Data/kahou_kabuto_mst") as Entity_kahou_kabuto_mst;

                    if (Mst.param[kahouId - 1].unit == "%") {
                        int calcHp = (hp * Mst.param[kahouId - 1].kahouEffect) / 100;
                        hpString = calcHp.ToString();
                    }
                    else {
                        hpString = (Mst.param[kahouId - 1].kahouEffect).ToString();
                    }
                }
                array[i] = hpString;
            }
            else if (i == 2) {
                string dfcString = "0";
                if (kahouId != 0) {
                    //dfc
                    Entity_kahou_gusoku_mst Mst = Resources.Load("Data/kahou_gusoku_mst") as Entity_kahou_gusoku_mst;

                    if (Mst.param[kahouId - 1].unit == "%") {
                        int calcDfc = (dfc * Mst.param[kahouId - 1].kahouEffect) / 100;
                        dfcString = calcDfc.ToString();
                    }
                    else {
                        dfcString = (Mst.param[kahouId - 1].kahouEffect).ToString();
                    }
                }
                array[i] = dfcString;
            }
            else if (i == 3) {
                string spdString = "0";
                if (kahouId != 0) {
                    //spd
                    Entity_kahou_meiba_mst Mst = Resources.Load("Data/kahou_meiba_mst") as Entity_kahou_meiba_mst;

                    if (Mst.param[kahouId - 1].unit == "%") {
                        int calcSpd = (spd * Mst.param[kahouId - 1].kahouEffect) / 100;
                        //Kyuusai Sochi
                        if (calcSpd == 0) {
                            calcSpd = 1;
                        }
                        spdString = calcSpd.ToString();

                    }
                    else {
                        spdString = (Mst.param[kahouId - 1].kahouEffect).ToString();
                    }
                }
                array[i] = spdString;
            }
        }
        return array;
    }



}
