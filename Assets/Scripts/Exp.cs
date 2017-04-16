using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class Exp : MonoBehaviour {

	Entity_busyo_exp_mst Mst  = Resources.Load ("Data/busyo_exp_mst") as Entity_busyo_exp_mst;
	Entity_kuni_lv_mst KuniMst = Resources.Load ("Data/kuni_lv_mst") as Entity_kuni_lv_mst;

	public int getExpforNextLv(int nowLv){

		int totalExp = Mst.param [nowLv].totalExp;

		return totalExp;
	}

	public int getDifExpforNextLv(int nowLv){
		int requiredExp = Mst.param [nowLv].requiredExp;
		
		return requiredExp;
	}

	public int getLvbyTotalExp(int nowLv ,int nowExp, int maxLv) {
		int tempExp=0;
		int resultLv=nowLv;

        //LvMax Check
        bool lvMaxFlg = false;
		if(nowLv== maxLv-1) {
            lvMaxFlg = checkLvMax(nowExp, maxLv);
		}else if(nowLv== maxLv) {
            lvMaxFlg = true;
		}

		if (lvMaxFlg == false) {
			for (int i=nowLv; tempExp<nowExp; i++) {
				tempExp = Mst.param [i].totalExp;
				if (nowExp < tempExp) {
					resultLv = Mst.param [i - 1].lv;
					break;
				}
			}
		} else {
			resultLv = maxLv;
		}

		return resultLv;
	}

	public int getExpbyCyadougu(int busyoId, int nowExp){

		int resultExp = nowExp;
		string temp = "kahou" + busyoId;
		string busyoKahou = PlayerPrefs.GetString (temp);

		if (busyoKahou != null && busyoKahou != "") {
			char[] delimiterChars = {','};
			string[] busyoKahouList = busyoKahou.Split (delimiterChars);

			for (int i=0; i<busyoKahouList.Length; i++) {
				if (i == 4 || i == 5) {
					int kahouId = int.Parse (busyoKahouList [i]);
					Entity_kahou_cyadougu_mst Mst = Resources.Load ("Data/kahou_cyadougu_mst") as Entity_kahou_cyadougu_mst;
					
					if (kahouId != 0) {
						//Cyadougu
						if (Mst.param [kahouId - 1].unit == "%") {
							resultExp = resultExp + (resultExp * Mst.param [kahouId - 1].kahouEffect) / 100;
						} else {
							resultExp = resultExp + (Mst.param [kahouId - 1].kahouEffect);
						}
					}
				}
			}
		}
		return resultExp;
	}

	public int getKuniLv(int nowLv ,int nowExp){

		int tempExp=0;
		int resultLv=nowLv;

		//Lv100 Check
		bool lv100Flg = false;
		if(nowLv==99){
			lv100Flg = checkKuniLv100(nowExp);
		}else if(nowLv==100){
			lv100Flg = true;
		}

		if (lv100Flg == false) {
			for (int i = nowLv; tempExp < nowExp; i++) {
				tempExp = KuniMst.param [i].totalExp;
				if (nowExp < tempExp) {
					resultLv = KuniMst.param [i - 1].lv;
					break;
				}
			}
		} else {
			resultLv = 100;
		}
		return resultLv;
	}
    //Check Kuni Lv 100
    public bool checkKuniLv100(int newExp) {
        //Lv 100 Experience
        bool lv100Flg = false;
        int maxExp = KuniMst.param[99].totalExp;
        if (newExp >= maxExp) {
            lv100Flg = true;
        }
        return lv100Flg;
    }

    public int getJinkeiLimit(int nowLv){
		int jinkeiLimit = KuniMst.param [nowLv-1].busyoJinkeiLimit;        

		return jinkeiLimit;
	}
	public int getStockLimit(int nowLv){
		int stockLimit = KuniMst.param [nowLv-1].busyoStockLimit;
		return stockLimit;
	}

	public int getKuniExpforNextLv(int nowLv){
		int totalExp = 0;
		if (nowLv != 100) {
			totalExp = KuniMst.param [nowLv].totalExp;
		} else {
			totalExp = PlayerPrefs.GetInt ("kuniExp");
		}
		return totalExp;
	}

	//Check Busyo Lv Max
	public bool checkLvMax(int newExp, int maxLv){
		bool lvMaxFlg = false;
		int maxExp = Mst.param [maxLv - 1].totalExp;
		if(newExp >= maxExp){
            lvMaxFlg = true;
		}
		return lvMaxFlg;
	}

	public int getExpLvMax(int maxLv) {
		//Lv Max Experience
		int maxExp = Mst.param [maxLv-1].totalExp;
		return maxExp;
	}

    public bool checkLv100(int newExp) {
        bool lvMaxFlg = false;
        int maxExp = Mst.param[99].totalExp;
        if (newExp >= maxExp) {
            lvMaxFlg = true;
        }
        return lvMaxFlg;
    }

    public int getExpLv100() {
        //Lv Max Experience
        int maxExp = Mst.param[99].totalExp;
        return maxExp;
    }


}
