using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class Senryoku : MonoBehaviour {

	public bool myDaimyoBusyoFlg;
	public int myDaimyoAddAtk = 0;
	public int myDaimyoAddDfc = 0;

	public int belongDaimyoId = 0;
	public int numSameDaimyo = 0;
	public int belongDaimyoAddAtk = 0;
	public int belongDaimyoAddDfc = 0;

	public int totalHp = 0;
	public int totalAtk = 0;
	public int totalDfc = 0;
    public int totalSpd = 0;
    public int lv = 0;
	public int chlv = 0;
    public int chQty = 0;

    public int shipId = 0;

    public void GetPlayerSenryoku(string busyoId){

        int i = 0;
        bool result = int.TryParse(busyoId, out i);
        if(result) {

            //Parent
            int myDaimyoBusyo = PlayerPrefs.GetInt("myDaimyoBusyo");
		    if (int.Parse(busyoId) == myDaimyoBusyo) {
			    myDaimyoBusyoFlg = true;
		    }

		    BusyoInfoGet busyo = new BusyoInfoGet();
		    belongDaimyoId = busyo.getDaimyoId (int.Parse(busyoId));
		    if (belongDaimyoId == 0) {
			    belongDaimyoId = busyo.getDaimyoHst(int.Parse(busyoId));
		    }
            shipId = busyo.getShipId(int.Parse(busyoId));

            lv = PlayerPrefs.GetInt (busyoId,1);
		    StatusGet sts = new StatusGet ();
		    int hp = sts.getHp (int.Parse (busyoId), lv);
		    int atk = sts.getAtk (int.Parse (busyoId), lv);
		    int dfc = sts.getDfc (int.Parse (busyoId), lv);
		    int spd = sts.getSpd (int.Parse (busyoId), lv);
		
		    int adjHp = hp * 100;
		    int adjAtk = atk * 10;
		    int adjDfc = dfc * 10;

            JyosyuHeiryoku jyosyuHei = new JyosyuHeiryoku();
            int addJyosyuHei = jyosyuHei.GetJyosyuHeiryoku(busyoId.ToString());
            
            KahouStatusGet kahouSts = new KahouStatusGet ();
		    string[] KahouStatusArray =kahouSts.getKahouForStatus (busyoId,adjHp,adjAtk,adjDfc,spd);
		    int totalBusyoHp =0;
		    int totalBusyoAtk =0;
		    int totalBusyoDfc =0;
            
		    string kanniTmp = "kanni" + busyoId;
		    float addAtkByKanni = 0;
		    float addHpByKanni = 0;
		    float addDfcByKanni = 0;

		    if (PlayerPrefs.HasKey (kanniTmp)) {
			    int kanniId = PlayerPrefs.GetInt (kanniTmp);
                if(kanniId != 0) {
			        Kanni kanni = new Kanni ();

			        //Status
			        string kanniTarget = kanni.getEffectTarget(kanniId);
			        int effect = kanni.getEffect(kanniId);
			        if(kanniTarget=="atk"){
				        addAtkByKanni = ((float)adjAtk * (float)effect)/100;
			        }else if(kanniTarget=="hp"){
				        addHpByKanni = ((float)adjHp * (float)effect)/100;
			        }else if(kanniTarget=="dfc"){
				        addDfcByKanni = ((float)adjDfc * (float)effect)/100;
			        }
                }
            }

		    totalBusyoAtk = adjAtk + int.Parse(KahouStatusArray[0]) + Mathf.FloorToInt (addAtkByKanni);
		    totalBusyoHp = adjHp + int.Parse(KahouStatusArray[1]) + Mathf.FloorToInt (addHpByKanni) + addJyosyuHei;
		    totalBusyoDfc = adjDfc + int.Parse(KahouStatusArray[2]) + Mathf.FloorToInt (addDfcByKanni);
            totalSpd = spd + int.Parse(KahouStatusArray[3]);
            if (Application.loadedLevelName == "preKaisen") {
                if (shipId == 1) {
                    totalBusyoHp = totalBusyoHp * 2;
                }else if (shipId == 2) {
                    totalBusyoHp = Mathf.FloorToInt((float)totalBusyoHp * 1.5f);
                }
            
            }

            //Child
            string heiId = "hei" + busyoId.ToString ();
		    string chParam = PlayerPrefs.GetString (heiId,"0");
            if (chParam == "0") {
                StatusGet statusScript = new StatusGet();
                string heisyu = statusScript.getHeisyu(int.Parse(busyoId));
                chParam = heisyu + ":1:1:1";
                PlayerPrefs.SetString(heiId, chParam);
                PlayerPrefs.Flush();
            }

            char[] delimiterChars = {':'};
            if(chParam.Contains(":")) {
		        string[] ch_list = chParam.Split (delimiterChars);
		
		        chQty = int.Parse (ch_list [1]);
		        chlv = int.Parse (ch_list [2]);
		        int ch_status = int.Parse (ch_list [3]);
		        int totalChldHp = 0;
		        int totalChldAtk = 0;
		        int totalChldDfc = 0;

		        ch_status = ch_status * 10;
		        int atkDfc = (int)sts.getChAtkDfc(ch_status, totalBusyoHp);
		        
		        totalChldHp = ch_status * chQty;
		        totalChldAtk = atkDfc * chQty;
		        totalChldDfc = atkDfc * chQty;
		
		        //Set value
		        totalHp = totalBusyoHp + totalChldHp;
		        totalAtk = totalBusyoAtk + totalChldAtk;
		        totalDfc = totalBusyoDfc + totalChldDfc;
            }
        }

        


	}


}
