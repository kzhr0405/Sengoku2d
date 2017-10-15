using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Reflection;
using System.Collections.Generic;
using System;

public class PlayerInstance : MonoBehaviour {


	public int makeInstance(int busyoId, int mapId, int hp, int atk, int dfc,int spd, ArrayList senpouArray, string busyoName, int soudaisyo, int boubi){
        int totalHeiryoku = 0;

		/*Parent Instantiate*/
		string path = "Prefabs/Player/" + busyoId;
		GameObject prefab = Instantiate(Resources.Load (path)) as GameObject;
		prefab.name = busyoId.ToString();

        //Senpou Script Parametor
        StatusGet senpouScript = new StatusGet();
        bool onlySeaFlg = senpouScript.getSenpouOnlySeaFlg((int)senpouArray[0]);

        if(!onlySeaFlg) {
		    prefab.GetComponent<SenpouController>().senpouId = (int)senpouArray[0];
		    prefab.GetComponent<SenpouController>().senpouTyp = senpouArray[1].ToString();
		    prefab.GetComponent<SenpouController>().senpouName = senpouArray[2].ToString();
		    prefab.GetComponent<SenpouController>().senpouEach = (float)senpouArray[4];
		    prefab.GetComponent<SenpouController>().senpouRatio = (float)senpouArray[5];
		    prefab.GetComponent<SenpouController>().senpouTerm = (float)senpouArray[6];
		    prefab.GetComponent<SenpouController>().senpouStatus = (int)senpouArray[7];
		    prefab.GetComponent<SenpouController>().senpouLv = (int)senpouArray[8];

            //Serihu
            Entity_serihu_mst serihuMst = Resources.Load("Data/serihu_mst") as Entity_serihu_mst;
            string serihu = "";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                serihu = serihuMst.param[busyoId - 1].senpouMsgEng;
            }else {
                serihu = serihuMst.param[busyoId - 1].senpouMsg;
            }
            prefab.GetComponent<SenpouController>().senpouSerihu = serihu;
        }else {
            Destroy(prefab.GetComponent<SenpouController>());
        }

        

		/*Player Status Setting*/
		//parametor setting
		int adjHp = hp*100;
		int adjAtk = atk * 10;
		int adjDfc = dfc * 10;

		//heisyu
		BusyoInfoGet info = new BusyoInfoGet ();
		string heisyu = info.getHeisyu (busyoId);

        JyosyuHeiryoku jyosyuHei = new JyosyuHeiryoku();
        float addJyosyuHei = (float)jyosyuHei.GetJyosyuHeiryoku(busyoId.ToString());

        //Kahou Adjustment
        KahouStatusGet kahouSts = new KahouStatusGet();
		string[] KahouStatusArray =kahouSts.getKahouForStatus (busyoId.ToString(),adjHp,adjAtk,adjDfc,spd);

		//Kanni Adjustment
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


        //Busyo Detail Info [Name & HP Bar]
        string dtlPath = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            dtlPath = "Prefabs/BusyoDtl/BusyoDtlPlayerEng";
        }else {
            dtlPath = "Prefabs/BusyoDtl/BusyoDtlPlayer";
        }
		GameObject dtl = Instantiate(Resources.Load (dtlPath)) as GameObject;
		dtl.transform.SetParent(prefab.transform);
		dtl.transform.localPosition = new Vector3 (0, 1.3f, -1);
		dtl.transform.localScale = new Vector3 (1.3f,1.3f,0);
        dtl.name = "BusyoDtlPlayer";

        //Name
        GameObject nameLabel = dtl.transform.FindChild("NameLabel").gameObject;
		nameLabel.GetComponent<TextMesh> ().text = busyoName;
		
		
		//Location by map id
		if (mapId == 1) {
			prefab.transform.position = new Vector2 (-65, 16);
			prefab.GetComponent<LineLocation>().nowLine = 1;
		} else if (mapId == 2) {
			prefab.transform.position = new Vector2 (-50, 16);
			prefab.GetComponent<LineLocation>().nowLine = 1;
		} else if (mapId == 3) {
			prefab.transform.position = new Vector2 (-35, 16);
			prefab.GetComponent<LineLocation>().nowLine = 1;

		} else if (mapId == 4) {
			prefab.transform.position = new Vector2 (-20, 16);
			prefab.GetComponent<LineLocation>().nowLine = 1;

		} else if (mapId == 5) {
			prefab.transform.position = new Vector2 (-5, 16);
			prefab.GetComponent<LineLocation>().nowLine = 1;

		} else if (mapId == 6) {
			prefab.transform.position = new Vector2 (-65, 8);
			prefab.GetComponent<LineLocation>().nowLine = 2;

		} else if (mapId == 7) {
			prefab.transform.position = new Vector2 (-50, 8);
			prefab.GetComponent<LineLocation>().nowLine = 2;

		} else if (mapId == 8) {
			prefab.transform.position = new Vector2 (-35, 8);
			prefab.GetComponent<LineLocation>().nowLine = 2;

		} else if (mapId == 9) {
			prefab.transform.position = new Vector2 (-20, 8);
			prefab.GetComponent<LineLocation>().nowLine = 2;

		} else if (mapId == 10) {
			prefab.transform.position = new Vector2 (-5, 8);
			prefab.GetComponent<LineLocation>().nowLine = 2;

		} else if (mapId == 11) {
			prefab.transform.position = new Vector2 (-65, 0);
			prefab.GetComponent<LineLocation>().nowLine = 3;

		} else if (mapId == 12) {
			prefab.transform.position = new Vector2 (-50, 0);
			prefab.GetComponent<LineLocation>().nowLine = 3;

		} else if (mapId == 13) {
			prefab.transform.position = new Vector2 (-35, 0);
			prefab.GetComponent<LineLocation>().nowLine = 3;

		} else if (mapId == 14) {
			prefab.transform.position = new Vector2 (-20, 0);
			prefab.GetComponent<LineLocation>().nowLine = 3;

		} else if (mapId == 15) {
			prefab.transform.position = new Vector2 (-5, 0);
			prefab.GetComponent<LineLocation>().nowLine = 3;

		} else if (mapId == 16) {
			prefab.transform.position = new Vector2 (-65, -8);
			prefab.GetComponent<LineLocation>().nowLine = 4;

		} else if (mapId == 17) {
			prefab.transform.position = new Vector2 (-50, -8);
			prefab.GetComponent<LineLocation>().nowLine = 4;

		} else if (mapId == 18) {
			prefab.transform.position = new Vector2 (-35, -8);
			prefab.GetComponent<LineLocation>().nowLine = 4;

		} else if (mapId == 19) {
			prefab.transform.position = new Vector2 (-20, -8);
			prefab.GetComponent<LineLocation>().nowLine = 4;

		} else if (mapId == 20) {
			prefab.transform.position = new Vector2 (-5, -8);
			prefab.GetComponent<LineLocation>().nowLine = 4;

		} else if (mapId == 21) {
			prefab.transform.position = new Vector2 (-65, -16);
			prefab.GetComponent<LineLocation>().nowLine = 5;

		} else if (mapId == 22) {
			prefab.transform.position = new Vector2 (-50, -16);
			prefab.GetComponent<LineLocation>().nowLine = 5;

		} else if (mapId == 23) {
			prefab.transform.position = new Vector2 (-45, -16);
			prefab.GetComponent<LineLocation>().nowLine = 5;

		} else if (mapId == 24) {
			prefab.transform.position = new Vector2 (-20, -16);
			prefab.GetComponent<LineLocation>().nowLine = 5;

		} else if (mapId == 25) {
			prefab.transform.position = new Vector2 (-5, -16);
			prefab.GetComponent<LineLocation>().nowLine = 5;

		}

		//Add Senryoku
		string key = "addSenryokuSlot" + mapId;
		if(PlayerPrefs.HasKey(key)){
			string atkDfc = PlayerPrefs.GetString (key);
			List<string> atkDfcList = new List<string> ();
			char[] delimiterChars2 = {','};
			atkDfcList = new List<string> (atkDfc.Split (delimiterChars2));
			adjAtk = adjAtk + int.Parse(atkDfcList [0]);
			adjDfc = adjDfc + int.Parse(atkDfcList [1]);
		}


        //HP Status        
		int adjHpWithKahou = adjHp + int.Parse(KahouStatusArray[1]) + Mathf.FloorToInt (addHpByKanni);
        GameObject minHpBar = dtl.transform.FindChild("MinHpBar").gameObject;
        minHpBar.GetComponent<BusyoHPBar>().initLife = adjHpWithKahou + addJyosyuHei;
        prefab.GetComponent<PlayerHP> ().life = adjHpWithKahou + addJyosyuHei;
        totalHeiryoku = adjHpWithKahou + Mathf.FloorToInt(addJyosyuHei); ;

        //spd adjust
        float adjSpd = ((float)spd + float.Parse(KahouStatusArray[3]))/10;

        prefab.GetComponent<UnitMover> ().heisyu = heisyu;
		if (prefab.GetComponent<PlayerAttack> ()) {
			prefab.GetComponent<PlayerAttack> ().attack = adjAtk + int.Parse(KahouStatusArray[0]) + Mathf.FloorToInt (addAtkByKanni);
            prefab.GetComponent<UnitMover>().speed = adjSpd;
        } else {
			if (heisyu == "TP") {
				prefab.GetComponent<AttackLong> ().attack = 5 * (adjAtk + int.Parse (KahouStatusArray [0]) + Mathf.FloorToInt (addAtkByKanni));
			} else if (heisyu == "YM") {
				prefab.GetComponent<AttackLong> ().attack = 3 * (adjAtk + int.Parse (KahouStatusArray [0]) + Mathf.FloorToInt (addAtkByKanni));
			}
			prefab.GetComponent<UnitMover> ().speed = adjSpd;
		}

		prefab.GetComponent<PlayerHP>().dfc = adjDfc + int.Parse(KahouStatusArray[2]) + Mathf.FloorToInt (addDfcByKanni) + boubi;
        


        //Soudaisyo
        if (busyoId == soudaisyo) {
			prefab.GetComponent<PlayerHP> ().taisyo = true;
		}
        
        //SE
        AudioController audio = new AudioController();
		audio.addComponentMoveAttack (prefab,heisyu);


        /*Child Instantiate*/
        //set child object
        string heiId = "hei" + busyoId.ToString();
        string chParam = "";
        bool tutorialDoneFlg = PlayerPrefs.GetBool("tutorialDoneFlg");
        if (!tutorialDoneFlg || Application.loadedLevelName != "tutorialKassen") {
            chParam = PlayerPrefs.GetString(heiId, "0");
        }else {
            //retry tutorial
            if(busyoId==19) {
                chParam = "TP:2:1:1";
            }else {
                StatusGet statusScript = new StatusGet();
                string chParamHeisyu = statusScript.getHeisyu(busyoId);
                chParam = chParamHeisyu + ":1:1:1";
            }
        }
        
        if (chParam == "0") {
            StatusGet statusScript = new StatusGet();
            string chParamHeisyu = statusScript.getHeisyu(busyoId);
            chParam = chParamHeisyu + ":1:1:1";
            PlayerPrefs.SetString(heiId, chParam);
            PlayerPrefs.Flush();
        }


        char[] delimiterChars = {':'};
		string[] ch_list = chParam.Split(delimiterChars);
        
        bool updateParam = false;
        //string ch_type = ch_list[0];
        string ch_type = heisyu;
        int ch_num = int.Parse (ch_list[1]);
        if (ch_num > 20) {
            ch_num = 20;
            updateParam = true;
        }
        int ch_lv = int.Parse (ch_list[2]);
        if (ch_lv > 100) {
            ch_lv = 100;
            updateParam = true;
        }
        float ch_status = float.Parse (ch_list[3]);
		ch_status = ch_status * 10;
        if (updateParam) {
            PlayerPrefs.SetString(heiId, ch_type + ":" + ch_num.ToString() + ":" + ch_lv.ToString() + ":" + ch_status.ToString());
        }

        string ch_path = "Prefabs/Player/" + ch_type;
		float y1 = 3.0f;
		float y2 = 3.0f;
		float y3 = 3.0f;
		float y4 = 3.0f;

		for(int i = 1; i <= ch_num; i++){
			//Make Relationship
			GameObject ch_prefab = Instantiate(Resources.Load (ch_path)) as GameObject;
			ch_prefab.transform.parent = prefab.transform;
			ch_prefab.name = "Child" + i.ToString();

			//Sashimono Making
			string sashimono_path = "Prefabs/Sashimono/" + busyoId;
			GameObject sashimono = Instantiate(Resources.Load (sashimono_path)) as GameObject;
			sashimono.transform.parent = ch_prefab.transform;
			sashimono.transform.localScale = new Vector2(0.3f,0.3f);


			if(ch_type == "YR"){
				sashimono.transform.localPosition = new Vector2(-1,0.6f);
				//Location
				if(i<6){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 3,prefab.transform.position.y + y1);
					y1 = y1 - 1.5f;

				}else if(5<i && i<11){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 6,prefab.transform.position.y + y2);
					y2 = y2 - 1.5f;

				}else if(10<i && i<16){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 9,prefab.transform.position.y + y3);
					y3 = y3 - 1.5f;

				}else if(15<i && i<21){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 12,prefab.transform.position.y + y4);
					y4 = y4 - 1.5f;
				}
			}else if(ch_type == "KB"){
				sashimono.transform.localPosition = new Vector2(-0.5f,1);
				//Location
				if(i<6){

					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 4,prefab.transform.position.y + y1);
					y1 = y1 - 1.5f;
					
				}else if(5<i && i<11){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 7,prefab.transform.position.y + y2);
					y2 = y2 - 1.5f;
					
				}else if(10<i && i<16){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 10,prefab.transform.position.y + y3);
					y3 = y3 - 1.5f;
					
				}else if(15<i && i<21){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 13,prefab.transform.position.y + y4);
					y4 = y4 - 1.5f;
				}
			}else if(ch_type == "TP"){
				sashimono.transform.localPosition = new Vector2(-0.8f,0.5f);
				//Location
				if(i<6){
					
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 4,prefab.transform.position.y + y1);
					y1 = y1 - 1.5f;
					
				}else if(5<i && i<11){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 7,prefab.transform.position.y + y2);
					y2 = y2 - 1.5f;
					
				}else if(10<i && i<16){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 10,prefab.transform.position.y + y3);
					y3 = y3 - 1.5f;
					
				}else if(15<i && i<21){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 13,prefab.transform.position.y + y4);
					y4 = y4 - 1.5f;
				}
			}else if(ch_type == "YM"){
				sashimono.transform.localPosition = new Vector2(-0.8f,0.5f);
				//Location
				if(i<6){
					
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 4,prefab.transform.position.y + y1);
					y1 = y1 - 1.5f;
					
				}else if(5<i && i<11){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 7,prefab.transform.position.y + y2);
					y2 = y2 - 1.5f;
					
				}else if(10<i && i<16){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 10,prefab.transform.position.y + y3);
					y3 = y3 - 1.5f;
					
				}else if(15<i && i<21){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 13,prefab.transform.position.y + y4);
					y4 = y4 - 1.5f;
				}
			}



			StatusGet sts = new StatusGet();
			int atkDfc = (int)sts.getChAtkDfc((int)ch_status, adjHpWithKahou);

			if (i == 1) {
				//Child Qty
				prefab.GetComponent<PlayerHP>().childQty = ch_num;

				//Child Unit HP
				float addTotalHei = ch_status;
				prefab.GetComponent<PlayerHP>().childHP = (int)addTotalHei;
                totalHeiryoku = totalHeiryoku + (int)addTotalHei * ch_num;
               
                //Attack
                if (ch_type == "YM") {
					prefab.GetComponent<AttackLong> ().childAttack = atkDfc * 3;
					prefab.GetComponent<Heisyu> ().atk = atkDfc * 3;
				} else if (ch_type == "TP") {
					prefab.GetComponent<AttackLong> ().childAttack = atkDfc * 5;
					prefab.GetComponent<Heisyu> ().atk = atkDfc * 5;
				} else {
					prefab.GetComponent<PlayerAttack> ().attack = prefab.GetComponent<PlayerAttack> ().attack + (ch_num*atkDfc);
					prefab.GetComponent<Heisyu> ().atk = atkDfc;
				}

				//Dfc
				prefab.GetComponent<PlayerHP> ().dfc = prefab.GetComponent<PlayerHP> ().dfc + (ch_num*atkDfc);
				prefab.GetComponent<Heisyu> ().dfc = atkDfc;
			}




		}
        
        return totalHeiryoku;
    }




	/*make engun instance*/
	public void makeEngunInstance(int busyoId, int hp, int atk, int dfc, int spd, ArrayList senpouArray, string busyoName, int ch_num, int ch_lv){
		
		/*Parent Instantiate*/
		string path = "Prefabs/Player/" + busyoId;
		GameObject prefab = Instantiate(Resources.Load (path)) as GameObject;
        prefab.name = busyoId.ToString();

        //Senpou Script Parametor
        StatusGet senpouScript = new StatusGet();
        bool onlySeaFlg = senpouScript.getSenpouOnlySeaFlg((int)senpouArray[0]);

        if (!onlySeaFlg) {
            prefab.GetComponent<SenpouController>().senpouId = (int)senpouArray[0];
		    prefab.GetComponent<SenpouController>().senpouTyp = senpouArray[1].ToString();
		    prefab.GetComponent<SenpouController>().senpouName = senpouArray[2].ToString();
		    prefab.GetComponent<SenpouController>().senpouEach = (float)senpouArray[4];
		    prefab.GetComponent<SenpouController>().senpouRatio = (float)senpouArray[5];
		    prefab.GetComponent<SenpouController>().senpouTerm = (float)senpouArray[6];
		    prefab.GetComponent<SenpouController>().senpouStatus = (int)senpouArray[7];
		    prefab.GetComponent<SenpouController>().senpouLv = (int)senpouArray[8];
            
            //Serihu
            Entity_serihu_mst serihuMst = Resources.Load("Data/serihu_mst") as Entity_serihu_mst;
            string serihu = "";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                serihu = serihuMst.param[busyoId - 1].senpouMsgEng;
            }else {
                serihu = serihuMst.param[busyoId - 1].senpouMsg;
            }
            prefab.GetComponent<SenpouController>().senpouSerihu = serihu;
        }else {
            Destroy(prefab.GetComponent<SenpouController>());
        }

        //Engun Flg
        prefab.GetComponent<Kunkou> ().engunFlg = true;



		/*Player Status Setting*/
		//parametor setting
		int adjHp = hp*100;
		int adjAtk = atk * 10;
		int adjDfc = dfc * 10;

        //Busyo Detail Info [Name & HP Bar]
        string dtlPath = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            dtlPath = "Prefabs/BusyoDtl/BusyoDtlPlayerEng";
        }else {
            dtlPath = "Prefabs/BusyoDtl/BusyoDtlPlayer";
        }
        GameObject dtl = Instantiate(Resources.Load (dtlPath)) as GameObject;
		dtl.transform.SetParent(prefab.transform);
		dtl.transform.localPosition = new Vector3 (0, 1.3f, -1);
		dtl.transform.localScale = new Vector3 (1.3f,1.3f,0);
        dtl.name = "BusyoDtlPlayer";
		//Name
		GameObject nameLabel = dtl.transform.FindChild("NameLabel").gameObject;
		nameLabel.GetComponent<TextMesh> ().text = busyoName;
		
		//HP Bar
		GameObject minHpBar = dtl.transform.FindChild("MinHpBar").gameObject;
		minHpBar.GetComponent<BusyoHPBar>().initLife = adjHp;
		
		//Location by map id
		prefab.transform.position = new Vector2 (-20, -16);
		prefab.GetComponent<LineLocation>().nowLine = 5;

		//heisyu
		BusyoInfoGet info = new BusyoInfoGet ();
		string ch_type = info.getHeisyu (busyoId);
		
		prefab.GetComponent<PlayerHP> ().life = adjHp;

        //adjust spd
        float adjSpd = (float)spd / 10;


        if (prefab.GetComponent<PlayerAttack> ()) {
			prefab.GetComponent<PlayerAttack> ().attack = adjAtk;
			prefab.GetComponent<UnitMover> ().speed = adjSpd;
		} else {
			
			if (ch_type == "TP") {
				prefab.GetComponent<AttackLong> ().attack = 5* adjAtk;
			} else if (ch_type == "YM") {
				prefab.GetComponent<AttackLong> ().attack = 3 * adjAtk;
			}
			prefab.GetComponent<UnitMover> ().speed = adjSpd;
		}
		prefab.GetComponent<PlayerHP>().dfc = adjDfc;
        prefab.GetComponent<UnitMover>().heisyu = ch_type;

        
        //SE
        AudioController audio = new AudioController();
		audio.addComponentMoveAttack (prefab,ch_type);



		/*Child Instantiate*/
		//set child object
		Entity_lvch_mst lvMst  = Resources.Load ("Data/lvch_mst") as Entity_lvch_mst;
		int startline = 0;
		if(ch_type=="KB"){
			startline = 0;
		}else if(ch_type=="YR"){
			startline = 1;
		}else if(ch_type=="TP"){
			startline = 2;
		}else if(ch_type=="YM"){
			startline = 3;
		}
		object stslst = lvMst.param[startline];
		Type t = stslst.GetType();
		String param = "lv" + ch_lv.ToString();
		FieldInfo f = t.GetField(param);
		int sts = (int)f.GetValue(stslst);

		float ch_status = (float)sts;
		ch_status = ch_status * 10;

		string ch_path = "Prefabs/Player/" + ch_type;
		
		float y1 = 3.0f;
		float y2 = 3.0f;
		float y3 = 3.0f;
		float y4 = 3.0f;
		
		for(int i = 1; i <= ch_num; i++){
			//Make Relationship
			GameObject ch_prefab = Instantiate(Resources.Load (ch_path)) as GameObject;
			ch_prefab.transform.parent = prefab.transform;
            ch_prefab.name = "Child" + i.ToString();

            //Sashimono Making
            string sashimono_path = "Prefabs/Sashimono/" + busyoId;
			GameObject sashimono = Instantiate(Resources.Load (sashimono_path)) as GameObject;
			sashimono.transform.parent = ch_prefab.transform;
			sashimono.transform.localScale = new Vector2(0.3f,0.3f);
			
			
			if(ch_type == "YR"){
				sashimono.transform.localPosition = new Vector2(-1,0.6f);
				//Location
				if(i<6){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 3,prefab.transform.position.y + y1);
					y1 = y1 - 1.5f;
					
				}else if(5<i && i<11){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 6,prefab.transform.position.y + y2);
					y2 = y2 - 1.5f;
					
				}else if(10<i && i<16){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 9,prefab.transform.position.y + y3);
					y3 = y3 - 1.5f;
					
				}else if(15<i && i<21){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 12,prefab.transform.position.y + y4);
					y4 = y4 - 1.5f;
				}
			}else if(ch_type == "KB"){
				sashimono.transform.localPosition = new Vector2(-0.5f,1);
				//Location
				if(i<6){
					
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 4,prefab.transform.position.y + y1);
					y1 = y1 - 1.5f;
					
				}else if(5<i && i<11){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 7,prefab.transform.position.y + y2);
					y2 = y2 - 1.5f;
					
				}else if(10<i && i<16){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 10,prefab.transform.position.y + y3);
					y3 = y3 - 1.5f;
					
				}else if(15<i && i<21){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 13,prefab.transform.position.y + y4);
					y4 = y4 - 1.5f;
				}
			}else if(ch_type == "TP"){
				sashimono.transform.localPosition = new Vector2(-0.8f,0.5f);
				//Location
				if(i<6){
					
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 4,prefab.transform.position.y + y1);
					y1 = y1 - 1.5f;
					
				}else if(5<i && i<11){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 7,prefab.transform.position.y + y2);
					y2 = y2 - 1.5f;
					
				}else if(10<i && i<16){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 10,prefab.transform.position.y + y3);
					y3 = y3 - 1.5f;
					
				}else if(15<i && i<21){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 13,prefab.transform.position.y + y4);
					y4 = y4 - 1.5f;
				}
			}else if(ch_type == "YM"){
				sashimono.transform.localPosition = new Vector2(-0.8f,0.5f);
				//Location
				if(i<6){
					
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 4,prefab.transform.position.y + y1);
					y1 = y1 - 1.5f;
					
				}else if(5<i && i<11){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 7,prefab.transform.position.y + y2);
					y2 = y2 - 1.5f;
					
				}else if(10<i && i<16){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 10,prefab.transform.position.y + y3);
					y3 = y3 - 1.5f;
					
				}else if(15<i && i<21){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x - 13,prefab.transform.position.y + y4);
					y4 = y4 - 1.5f;
				}
			}

			//Status
			if (i == 1) {
				//Child Qty
				prefab.GetComponent<PlayerHP>().childQty = ch_num;

				//Child Unit HP
				prefab.GetComponent<PlayerHP>().childHP = (int)ch_status;

				StatusGet stsScript = new StatusGet();
				int atkDfc = (int)stsScript.getChAtkDfc((int)ch_status, adjHp);

				//Attack
				if (ch_type == "YM") {
					prefab.GetComponent<AttackLong> ().childAttack = atkDfc * 3;
				} else if (ch_type == "TP") {
					prefab.GetComponent<AttackLong> ().childAttack = atkDfc * 5;
				} else {
					prefab.GetComponent<PlayerAttack> ().attack = prefab.GetComponent<PlayerAttack> ().attack + (ch_num*atkDfc);
				}

				//Dfc
				prefab.GetComponent<PlayerHP> ().dfc = prefab.GetComponent<PlayerHP> ().dfc + (ch_num*atkDfc);
			}


		}
		
	}

    public void makeKaisenInstance(int busyoId, int shipId, int mapId, int hp, int atk, int dfc, int spd, ArrayList senpouArray, string busyoName, int soudaisyo, int boubi, bool engunFlg, int engunButaiQty, int engunButaiSts) {
       
        /*Parent Instantiate*/
        string path = "Prefabs/Kaisen/" + shipId;
        GameObject prefab = Instantiate(Resources.Load(path)) as GameObject;
        prefab.name = busyoId.ToString();

        //Senpou Script Parametor
        StatusGet senpouScript = new StatusGet();
        bool shipFlg = senpouScript.getSenpouShipFlg((int)senpouArray[0]);

        if(shipFlg) {
            prefab.GetComponent<SenpouController>().senpouId = (int)senpouArray[0];
            prefab.GetComponent<SenpouController>().senpouTyp = senpouArray[1].ToString();
            prefab.GetComponent<SenpouController>().senpouName = senpouArray[2].ToString();
            prefab.GetComponent<SenpouController>().senpouEach = (float)senpouArray[4];
            prefab.GetComponent<SenpouController>().senpouRatio = (float)senpouArray[5];
            prefab.GetComponent<SenpouController>().senpouTerm = (float)senpouArray[6];
            prefab.GetComponent<SenpouController>().senpouStatus = (int)senpouArray[7];
            prefab.GetComponent<SenpouController>().senpouLv = (int)senpouArray[8];
            //Serihu
            Entity_serihu_mst serihuMst = Resources.Load("Data/serihu_mst") as Entity_serihu_mst;
            string serihu = "";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                serihu = serihuMst.param[busyoId - 1].senpouMsgEng;
            }else {
                serihu = serihuMst.param[busyoId - 1].senpouMsg;
            }
            prefab.GetComponent<SenpouController>().senpouSerihu = serihu;

        }else {
            Destroy(prefab.GetComponent<SenpouController>());
        }

        

        /*Player Status Setting*/
        //parametor setting
        int adjHp = hp * 100;
        int adjAtk = atk * 10;
        int adjDfc = dfc * 10;

        //Kahou Adjustment
        float addAtkByKanni = 0;
        float addHpByKanni = 0;
        float addDfcByKanni = 0;

        //Jyosyu Adjustment
        float addJyosyuHei = 0;

        string[] KahouStatusArray = null;
        if (!engunFlg) {
            KahouStatusGet kahouSts = new KahouStatusGet();
            KahouStatusArray = kahouSts.getKahouForStatus(busyoId.ToString(), adjHp, adjAtk, adjDfc, spd);
            //Kanni Adjustment
            string kanniTmp = "kanni" + busyoId;
            
            if (PlayerPrefs.HasKey(kanniTmp)) {
                int kanniId = PlayerPrefs.GetInt(kanniTmp);
                if(kanniId !=0) {
                    Kanni kanni = new Kanni();

                    //Status
                    string kanniTarget = kanni.getEffectTarget(kanniId);
                    int effect = kanni.getEffect(kanniId);
                    if (kanniTarget == "atk") {
                        addAtkByKanni = ((float)adjAtk * (float)effect) / 100;
                    }else if (kanniTarget == "hp") {
                        addHpByKanni = ((float)adjHp * (float)effect) / 100;
                    }else if (kanniTarget == "dfc") {
                        addDfcByKanni = ((float)adjDfc * (float)effect) / 100;
                    }
                }
            }
            JyosyuHeiryoku jyosyuHei = new JyosyuHeiryoku();
            addJyosyuHei = (float)jyosyuHei.GetJyosyuHeiryoku(busyoId.ToString());

        }

        //Busyo Detail Info [Name & HP Bar]
        string dtlPath = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            dtlPath = "Prefabs/BusyoDtl/BusyoDtlPlayerEng";
        }else {
            dtlPath = "Prefabs/BusyoDtl/BusyoDtlPlayer";
        }
        GameObject dtl = Instantiate(Resources.Load(dtlPath)) as GameObject;
        dtl.transform.SetParent(prefab.transform);
        dtl.transform.localPosition = new Vector3(0, 1, -1);
        dtl.transform.localScale = new Vector3(1, 1, 0);
        dtl.name = "BusyoDtlPlayer";
        //Name
        GameObject nameLabel = dtl.transform.FindChild("NameLabel").gameObject;
        nameLabel.GetComponent<TextMesh>().text = busyoName;
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            nameLabel.GetComponent<TextMesh>().fontSize = 40;
        }
        //Location by map id
        if (mapId == 1) {
            prefab.transform.position = new Vector2(-65, 16);
        }else if (mapId == 2) {
            prefab.transform.position = new Vector2(-50, 16);
        }else if (mapId == 3) {
            prefab.transform.position = new Vector2(-35, 16);
        }else if (mapId == 4) {
            prefab.transform.position = new Vector2(-20, 16);
        }else if (mapId == 5) {
            prefab.transform.position = new Vector2(-5, 16);
        }else if (mapId == 6) {
            prefab.transform.position = new Vector2(-65, 8);
        }else if (mapId == 7) {
            prefab.transform.position = new Vector2(-50, 8);
        }else if (mapId == 8) {
            prefab.transform.position = new Vector2(-35, 8);
        }else if (mapId == 9) {
            prefab.transform.position = new Vector2(-20, 8);
        }else if (mapId == 10) {
            prefab.transform.position = new Vector2(-5, 8);
        }else if (mapId == 11) {
            prefab.transform.position = new Vector2(-65, 0);
        }else if (mapId == 12) {
            prefab.transform.position = new Vector2(-50, 0);
        }else if (mapId == 13) {
            prefab.transform.position = new Vector2(-35, 0);
        }else if (mapId == 14) {
            prefab.transform.position = new Vector2(-20, 0);
        }else if (mapId == 15) {
            prefab.transform.position = new Vector2(-5, 0);
        }else if (mapId == 16) {
            prefab.transform.position = new Vector2(-65, -8);
        }else if (mapId == 17) {
            prefab.transform.position = new Vector2(-50, -8);
        }else if (mapId == 18) {
            prefab.transform.position = new Vector2(-35, -8);
        }else if (mapId == 19) {
            prefab.transform.position = new Vector2(-20, -8);
        }else if (mapId == 20) {
            prefab.transform.position = new Vector2(-5, -8);
        }else if (mapId == 21) {
            prefab.transform.position = new Vector2(-65, -16);
        }else if (mapId == 22) {
            prefab.transform.position = new Vector2(-50, -16);
        }else if (mapId == 23) {
            prefab.transform.position = new Vector2(-45, -16);
        }else if (mapId == 24) {
            prefab.transform.position = new Vector2(-20, -16);
        }else if (mapId == 25) {
            prefab.transform.position = new Vector2(-5, -16);
        }

        //Add Senryoku
        if(!engunFlg) {
            string key = "addSenryokuSlot" + mapId;
            if (PlayerPrefs.HasKey(key)) {
                string atkDfc = PlayerPrefs.GetString(key);
                List<string> atkDfcList = new List<string>();
                char[] delimiterChars2 = { ',' };
                atkDfcList = new List<string>(atkDfc.Split(delimiterChars2));
                adjAtk = adjAtk + int.Parse(atkDfcList[0]);
                adjDfc = adjDfc + int.Parse(atkDfcList[1]);
            }
        }

        //Adjust Status & Set
        int adjHpWithKahou = 0;
        float adjAtkWithKahou = 0;
        float adjDfcWithKahou = 0;
        float adjSpd = 0;

        if (!engunFlg) {
            adjHpWithKahou = adjHp + int.Parse(KahouStatusArray[1]) + Mathf.FloorToInt(addHpByKanni);
            adjAtkWithKahou = (float)adjAtk + int.Parse(KahouStatusArray[0]) + Mathf.FloorToInt(addAtkByKanni);
            adjDfcWithKahou = (float)adjDfc + int.Parse(KahouStatusArray[2]) + Mathf.FloorToInt(addDfcByKanni) + boubi;
            adjSpd = ((float)spd + float.Parse(KahouStatusArray[3])) / 10;
        }else {
            adjHpWithKahou = adjHp;
            adjAtkWithKahou = (float)adjAtk;
            adjDfcWithKahou = (float)adjDfc;
            adjSpd = (float)spd/10;
        }

        if (shipId == 1) {
            adjAtkWithKahou = Mathf.FloorToInt((float)adjAtkWithKahou * 1.4f);
            adjDfcWithKahou = Mathf.FloorToInt((float)adjDfcWithKahou * 1.4f);
            adjSpd = Mathf.FloorToInt((float)adjSpd * 0.4f);
        }else if (shipId == 2) {
            adjAtkWithKahou = Mathf.FloorToInt((float)adjAtkWithKahou * 1.2f);
            adjDfcWithKahou = Mathf.FloorToInt((float)adjDfcWithKahou * 1.2f);
            adjSpd = Mathf.FloorToInt((float)adjSpd * 0.6f);
        }else if(shipId==3) {
            adjAtkWithKahou = Mathf.FloorToInt((float)adjAtkWithKahou * 0.8f);
            adjDfcWithKahou = Mathf.FloorToInt((float)adjDfcWithKahou * 0.8f);
            adjSpd = Mathf.FloorToInt((float)adjSpd * 0.8f);
        }
        if (adjSpd<=0) {
            adjSpd = 1;
        }
        GameObject minHpBar = dtl.transform.FindChild("MinHpBar").gameObject;
        minHpBar.GetComponent<BusyoHPBar>().initLife = adjHpWithKahou + addJyosyuHei;
        prefab.GetComponent<PlayerHP>().life = adjHpWithKahou + addJyosyuHei;
        prefab.GetComponent<PlayerAttack>().attack = adjAtkWithKahou;
        prefab.GetComponent<UnitMover>().speed = adjSpd;
        prefab.GetComponent<UnitMover>().heisyu = "SHP";
        prefab.GetComponent<PlayerHP>().dfc = adjDfcWithKahou;
        

        //Soudaisyo
        if (!engunFlg) {
            if (busyoId == soudaisyo) {
                prefab.GetComponent<PlayerHP>().taisyo = true;
            }
        }

        //SE
        AudioController audio = new AudioController();
        audio.addComponentMoveAttack(prefab, "SHP");


        /*Child Instantiate*/
        //set child object
        int ch_num = 0;
        int ch_lv = 0;
        float ch_status = 0;
        if (!engunFlg) {
            string heiId = "hei" + busyoId.ToString();
            string chParam = PlayerPrefs.GetString(heiId, "0");
            if (chParam == "0") {
                StatusGet statusScript = new StatusGet();
                string chParamHeisyu = statusScript.getHeisyu(busyoId);
                chParam = chParamHeisyu + ":1:1:1";
                PlayerPrefs.SetString(heiId, chParam);
                PlayerPrefs.Flush();
            }

            char[] delimiterChars = { ':' };
            string[] ch_list = chParam.Split(delimiterChars);
            ch_num = int.Parse(ch_list[1]);
            ch_lv = int.Parse(ch_list[2]);
            ch_status = float.Parse(ch_list[3]);
        }else {
            ch_num = engunButaiQty;
            ch_status = engunButaiSts;

        }
        ch_status = ch_status * 10;

        string ch_path = "Prefabs/Kaisen/3";
        for (int i = 1; i <= ch_num; i++) {
            //Make Relationship
            GameObject ch_prefab = Instantiate(Resources.Load(ch_path)) as GameObject;
            ch_prefab.transform.SetParent(prefab.transform);
            ch_prefab.name = "Child" + i.ToString();
            ch_prefab.transform.localScale = new Vector2(0.7f,0.7f);
            ch_prefab.GetComponent<SpriteRenderer>().sortingOrder = 3;
            ch_prefab.tag = "PlayerChild";

            Destroy(ch_prefab.GetComponent<Rigidbody2D>());
            Destroy(ch_prefab.GetComponent<UnitMover>());
            Destroy(ch_prefab.GetComponent<Kunkou>());
            Destroy(ch_prefab.GetComponent<PolygonCollider2D>());
            Destroy(ch_prefab.GetComponent<PlayerHP>());
            Destroy(ch_prefab.GetComponent<PlayerAttack>());
            Destroy(ch_prefab.GetComponent<SenpouController>());
            
            //Location
            if(i==1) {
                ch_prefab.transform.localPosition = new Vector2(1.8f, 0);
            }else if(i==2) {
                ch_prefab.transform.localPosition = new Vector2(1.8f, 0.5f);
            }else if (i == 3) {
                ch_prefab.transform.localPosition = new Vector2(1.8f, -0.5f);
            }else if (i == 4) {
                ch_prefab.transform.localPosition = new Vector2(1.8f, 1.0f);
            }else if (i == 5) {
                ch_prefab.transform.localPosition = new Vector2(1.8f, -1.0f);
            }else if (i == 6) {
                ch_prefab.transform.localPosition = new Vector2(0, 0.5f);
            }else if (i == 7) {
                ch_prefab.transform.localPosition = new Vector2(0, -0.5f);
            }else if (i == 8) {
                ch_prefab.transform.localPosition = new Vector2(0, 1.0f);
            }else if (i == 9) {
                ch_prefab.transform.localPosition = new Vector2(0, -1.0f);
            }else if (i == 10) {
                ch_prefab.transform.localPosition = new Vector2(-1.8f, 0);
            }else if (i == 11) {
                ch_prefab.transform.localPosition = new Vector2(-1.8f, 0.5f);
            }else if (i == 12) {
                ch_prefab.transform.localPosition = new Vector2(-1.8f, -0.5f);
            }else if (i == 13) {
                ch_prefab.transform.localPosition = new Vector2(-1.8f,  1.0f);
            }else if (i == 14) {
                ch_prefab.transform.localPosition = new Vector2(-1.8f,  -1.0f);
            }else if (i == 15) {
                ch_prefab.transform.localPosition = new Vector2(1.8f, 1.5f);
            }else if (i == 16) {
                ch_prefab.transform.localPosition = new Vector2(1.8f, -1.5f);
            }else if (i == 17) {
                ch_prefab.transform.localPosition = new Vector2(0, 1.5f);
            }else if (i == 18) {
                ch_prefab.transform.localPosition = new Vector2(0, -1.5f);
            }else if (i == 19) {
                ch_prefab.transform.localPosition = new Vector2(-1.8f, 1.5f);
            }else if (i == 20) {
                ch_prefab.transform.localPosition = new Vector2(-1.8f,  -1.5f);
            }
            

            StatusGet sts = new StatusGet();
            int atkDfc = (int)sts.getChAtkDfc((int)ch_status, adjHpWithKahou);

            if (i == 1) {
                //Child Qty
                prefab.GetComponent<PlayerHP>().childQty = ch_num;

                //Child Unit HP
                prefab.GetComponent<PlayerHP>().childHP = (int)ch_status;

                //Attack
                prefab.GetComponent<PlayerAttack>().attack = prefab.GetComponent<PlayerAttack>().attack + (ch_num * atkDfc);
                
                //Dfc
                prefab.GetComponent<PlayerHP>().dfc = prefab.GetComponent<PlayerHP>().dfc + (ch_num * atkDfc);
            }
        }
       
    }
}
