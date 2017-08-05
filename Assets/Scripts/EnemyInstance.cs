using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class EnemyInstance : MonoBehaviour {


	public void makeInstance(int mapId, int busyoId, int lv, string ch_type, int ch_num, int hp, int atk, int dfc,int spd, string busyoName, int linkNo, bool taisyo, ArrayList senpouArray, string kahouList) {

        /*Roujyo Start*/
        bool shiroFlg = false;
        bool torideFlg = false;
        GameObject buildingObj = null;
        bool pvpFlg = GameObject.Find("GameScene").GetComponent<GameScene>().pvpFlg;

        if (!pvpFlg) {
            
		    string eSRMap ="eSRMap" + mapId.ToString();
		    shiroFlg = PlayerPrefs.GetBool (eSRMap);
		    PlayerPrefs.DeleteKey (eSRMap);

		    if (!shiroFlg) {
			    string eTRMap = "eTRMap" + mapId.ToString ();
			    torideFlg = PlayerPrefs.GetBool (eTRMap);
			    PlayerPrefs.DeleteKey (eTRMap);
		    }

		    if (shiroFlg) {
			    string objPath = "Prefabs/Kassen/eShiro";
			    buildingObj = Instantiate(Resources.Load (objPath)) as GameObject;
			    buildingObj.transform.localScale = new Vector2(2, 1.5f);
                setEnemyObjectOnMap (mapId, buildingObj);
			    buildingObj.name = "shiro";

			    string stageName = PlayerPrefs.GetString ("activeStageName");
			    buildingObj.transform.FindChild ("BusyoDtlEnemy").transform.FindChild ("NameLabel").GetComponent<TextMesh> ().text = stageName;

			    //HP
			    int powerType = PlayerPrefs.GetInt("activePowerType");
			    string Type = "";
			    if (powerType == 1) {
				    Type = "s";
			    } else if (powerType == 2) {
				    Type = "m";
			    } else if (powerType == 3) {
				    Type = "l";
			    }
                string imagePath = "Prefabs/Naisei/Shiro/Sprite/shiro_" + Type;
                buildingObj.GetComponent<SpriteRenderer> ().sprite = 
				    Resources.Load (imagePath, typeof(Sprite)) as Sprite;

		    } else {
			    if (torideFlg) {
				    string objPath = "Prefabs/Kassen/eToride";
				    buildingObj = Instantiate(Resources.Load (objPath)) as GameObject;
				    buildingObj.transform.localScale = new Vector2 (3,3);
				    setEnemyObjectOnMap (mapId, buildingObj);
				    buildingObj.name = "toride";

				    //HP
				    int powerType = PlayerPrefs.GetInt("activePowerType");
				    string Type = "";
				    if (powerType == 1) {
					    Type = "s";
				    } else if (powerType == 2) {
					    Type = "m";
				    } else if (powerType == 3) {
					    Type = "l";
				    }

				    string imagePath = "Prefabs/Kassen/kassenTrd_" + Type;
				    buildingObj.GetComponent<SpriteRenderer> ().sprite = 
					    Resources.Load (imagePath, typeof(Sprite)) as Sprite;
			    }
		    }
        }
		/*Roujyo End*/


		string path = "Prefabs/Player/" + busyoId;
		GameObject prefab = Instantiate(Resources.Load (path)) as GameObject;
		prefab.name = busyoId.ToString ();

        /**Player to Enemy**/
        Vector3 scale = prefab.transform.localScale;
        float x = prefab.transform.localScale.x;
        scale.x = scale.x  * - 1;
        prefab.transform.localScale = scale;
        Destroy(prefab.GetComponent<PlayerHP>());
        prefab.AddComponent<EnemyHP>();
        Destroy(prefab.GetComponent<Kunkou>());
        Destroy(prefab.GetComponent<UnitMover>());
        if(prefab.GetComponent<PlayerAttack>()) {
            Destroy(prefab.GetComponent<PlayerAttack>());
            prefab.AddComponent<EnemyAttack>();
            prefab.AddComponent<Homing>();
        }else {
            prefab.AddComponent<HomingLong>();
            if (ch_type == "YM") {
                    prefab.GetComponent<AttackLong>().bullet = Resources.Load("Prefabs/Enemy/EnemyArrow") as GameObject;
            }else {
                    prefab.GetComponent<AttackLong>().bullet = Resources.Load("Prefabs/Enemy/EnemyBullet") as GameObject;
            }            
        }
        prefab.tag = "Enemy";
        prefab.layer = LayerMask.NameToLayer("Enemy");
        /**Player to Enemy End**/




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
        
		//Busyo Detail Info [Name & HP Bar]
        string dtlPath = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            dtlPath = "Prefabs/BusyoDtl/BusyoDtlEnemyEng";
        }else {
            dtlPath = "Prefabs/BusyoDtl/BusyoDtlEnemy";
        }
        GameObject dtl = Instantiate(Resources.Load (dtlPath)) as GameObject;
		dtl.transform.SetParent(prefab.transform);
        dtl.transform.localPosition = new Vector3(0, 1.3f, -1);
        dtl.transform.localScale = new Vector3(-1.3f, 1.3f, 0);
        dtl.name = "BusyoDtlEnemy";

        //Name
        GameObject nameLabel = dtl.transform.FindChild("NameLabel").gameObject;
		nameLabel.GetComponent<TextMesh> ().text = busyoName;
		

		//Location by map id
		if (mapId == 1) {
			prefab.transform.position = new Vector2 (5, 16);
			prefab.GetComponent<LineLocation>().nowLine = 1;
		} else if (mapId == 2) {
			prefab.transform.position = new Vector2 (20, 16);
			prefab.GetComponent<LineLocation>().nowLine = 1;
		} else if (mapId == 3) {
			prefab.transform.position = new Vector2 (35, 16);
			prefab.GetComponent<LineLocation>().nowLine = 1;
		} else if (mapId == 4) {
			prefab.transform.position = new Vector2 (50, 16);
			prefab.GetComponent<LineLocation>().nowLine = 1;
		} else if (mapId == 5) {
			prefab.transform.position = new Vector2 (65, 16);
			prefab.GetComponent<LineLocation>().nowLine = 1;
		} else if (mapId == 6) {
			prefab.transform.position = new Vector2 (5, 8);
			prefab.GetComponent<LineLocation>().nowLine = 2;
		} else if (mapId == 7) {
			prefab.transform.position = new Vector2 (20, 8);
			prefab.GetComponent<LineLocation>().nowLine = 2;
		} else if (mapId == 8) {
			prefab.transform.position = new Vector2 (35, 8);
			prefab.GetComponent<LineLocation>().nowLine = 2;
		} else if (mapId == 9) {
			prefab.transform.position = new Vector2 (50, 8);
			prefab.GetComponent<LineLocation>().nowLine = 2;
		} else if (mapId == 10) {
			prefab.transform.position = new Vector2 (65, 8);
			prefab.GetComponent<LineLocation>().nowLine = 2;
		} else if (mapId == 11) {
			prefab.transform.position = new Vector2 (5, 0);
			prefab.GetComponent<LineLocation>().nowLine = 3;
		} else if (mapId == 12) {
			prefab.transform.position = new Vector2 (20, 0);
			prefab.GetComponent<LineLocation>().nowLine = 3;
		} else if (mapId == 13) {
			prefab.transform.position = new Vector2 (35, 0);
			prefab.GetComponent<LineLocation>().nowLine = 3;
		} else if (mapId == 14) {
			prefab.transform.position = new Vector2 (50, 0);
			prefab.GetComponent<LineLocation>().nowLine = 3;
		} else if (mapId == 15) {
			prefab.transform.position = new Vector2 (65, 0);
			prefab.GetComponent<LineLocation>().nowLine = 3;
		} else if (mapId == 16) {
			prefab.transform.position = new Vector2 (5, -8);
			prefab.GetComponent<LineLocation>().nowLine = 4;
		} else if (mapId == 17) {
			prefab.transform.position = new Vector2 (20, -8);
			prefab.GetComponent<LineLocation>().nowLine = 4;
		} else if (mapId == 18) {
			prefab.transform.position = new Vector2 (35, -8);
			prefab.GetComponent<LineLocation>().nowLine = 4;
		} else if (mapId == 19) {
			prefab.transform.position = new Vector2 (50, -8);
			prefab.GetComponent<LineLocation>().nowLine = 4;
		} else if (mapId == 20) {
			prefab.transform.position = new Vector2 (65, -8);
			prefab.GetComponent<LineLocation>().nowLine = 4;
		} else if (mapId == 21) {
			prefab.transform.position = new Vector2 (5, -16);
			prefab.GetComponent<LineLocation>().nowLine = 5;
		} else if (mapId == 22) {
			prefab.transform.position = new Vector2 (20, -16);
			prefab.GetComponent<LineLocation>().nowLine = 5;
		} else if (mapId == 23) {
			prefab.transform.position = new Vector2 (35, -16);
			prefab.GetComponent<LineLocation>().nowLine = 5;
		} else if (mapId == 24) {
			prefab.transform.position = new Vector2 (50, -16);
			prefab.GetComponent<LineLocation>().nowLine = 5;
		} else if (mapId == 25) {
			prefab.transform.position = new Vector2 (65, -16);
			prefab.GetComponent<LineLocation>().nowLine = 5;
		}

    
        
        //Link Adjustment
        float linkAdjst = (float)linkNo/10;
		hp = hp * 100;
        atk = atk * 10;
        dfc = dfc * 10;
		if (linkNo != 0) {
			float adjstHp = hp * linkAdjst;
			hp = hp + (int)adjstHp;
			float adjstDfc = dfc * linkAdjst;
			dfc = dfc + (int)adjstDfc;
		}

        /*Kahou Adjustment*/
        string[] KahouStatusArray;
        float spdWithKahou = (float)spd;
        if (pvpFlg) {
            KahouStatusGet KahouStatusGet = new KahouStatusGet();
            KahouStatusArray = KahouStatusGet.getPvPKahouForStatus(kahouList, hp, atk, dfc, spd);
            hp = hp + int.Parse(KahouStatusArray[1]);
            atk = atk + int.Parse(KahouStatusArray[0]);
            dfc = dfc + int.Parse(KahouStatusArray[2]);
            spdWithKahou = ((float)spd + float.Parse(KahouStatusArray[3]));
        }

        //HP Bar
        GameObject minHpBar = dtl.transform.FindChild("MinHpBar").gameObject;
		minHpBar.GetComponent<BusyoHPBar>().initLife = hp;
		prefab.GetComponent<EnemyHP>().life = hp;

        //adjust spd
        float adjSpd = spdWithKahou / 10;

        if (prefab.GetComponent<EnemyAttack> ()) {
			if (linkNo != 0) {
				float adjstAtk = atk * linkAdjst;
				atk = atk + (int)adjstAtk;

			}
			prefab.GetComponent<EnemyAttack> ().attack = atk;
			if (adjSpd <= 0) {
                adjSpd = 1;
			}
			prefab.GetComponent<Homing> ().speed = adjSpd;
			prefab.GetComponent<Homing>().leftFlg = true;

		} else {
			if(ch_type == "YM"){
				atk = atk * 3;
			}else if(ch_type == "TP"){
				atk = atk * 5;
			}
			if (linkNo != 0) {
				float adjstAtk = atk * linkAdjst;
				atk = atk + (int)adjstAtk;
			}

			prefab.GetComponent<AttackLong> ().attack = atk;
			if (adjSpd <= 0) {
                adjSpd = 1;
			}
			prefab.GetComponent<HomingLong> ().speed = adjSpd;
			prefab.GetComponent<HomingLong>().leftFlg = true;
		}
		prefab.GetComponent<EnemyHP> ().dfc = dfc;

		if (taisyo) {
			prefab.GetComponent<EnemyHP> ().taisyo = true;

		}

		//SE
		AudioController audio = new AudioController();
		audio.addComponentMoveAttack (prefab,ch_type);


		//Child Instantiate
		//set child object
		string ch_path = "Prefabs/Enemy/" + ch_type;
		float y1 = 3.0f;
		float y2 = 3.0f;
		float y3 = 3.0f;
		float y4 = 3.0f;
		StatusGet sts = new StatusGet();

		for(int i = 1; i <= ch_num; i++){
			//Make Relationship
			GameObject ch_prefab = Instantiate(Resources.Load (ch_path)) as GameObject;
			ch_prefab.transform.SetParent(prefab.transform);
			ch_prefab.name = "Child" + i.ToString();

			//Sashimono Making
			string sashimono_path = "Prefabs/Sashimono/" + busyoId;
			//string sashimono_path = "Prefabs/Sashimono/1";
			GameObject sashimono = Instantiate(Resources.Load (sashimono_path)) as GameObject;
			sashimono.transform.SetParent(ch_prefab.transform);


			//reverse Horizonal
			sashimono.transform.localScale = new Vector2(0.3f,0.3f);
			sashimono.transform.localEulerAngles = new Vector3(sashimono.transform.localEulerAngles.x, sashimono.transform.localEulerAngles.y, 10);

			if(ch_type == "YR"){
				sashimono.transform.localPosition = new Vector2(-1,0.6f);
				//Location
				if(i<6){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x + 3,prefab.transform.position.y + y1);
					y1 = y1 - 1.5f;
					
				}else if(5<i && i<11){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x + 6,prefab.transform.position.y + y2);
					y2 = y2 - 1.5f;
					
				}else if(10<i && i<16){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x + 9,prefab.transform.position.y + y3);
					y3 = y3 - 1.5f;
					
				}else if(15<i && i<21){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x + 12,prefab.transform.position.y + y4);
					y4 = y4 - 1.5f;
				}
			}else if(ch_type == "KB"){
				sashimono.transform.localPosition = new Vector2(-0.5f,1);
				//Location
				if(i<6){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x + 4,prefab.transform.position.y + y1);
					y1 = y1 - 1.5f;
					
				}else if(5<i && i<11){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x + 7,prefab.transform.position.y + y2);
					y2 = y2 - 1.5f;
					
				}else if(10<i && i<16){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x + 10,prefab.transform.position.y + y3);
					y3 = y3 - 1.5f;
					
				}else if(15<i && i<21){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x + 13,prefab.transform.position.y + y4);
					y4 = y4 - 1.5f;
				}
			}else if(ch_type == "TP"){
				sashimono.transform.localPosition = new Vector2(-0.8f,0.5f);
				//Location
				if(i<6){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x + 4,prefab.transform.position.y + y1);
					y1 = y1 - 1.5f;
					
				}else if(5<i && i<11){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x + 7,prefab.transform.position.y + y2);
					y2 = y2 - 1.5f;
					
				}else if(10<i && i<16){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x + 10,prefab.transform.position.y + y3);
					y3 = y3 - 1.5f;
					
				}else if(15<i && i<21){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x + 13,prefab.transform.position.y + y4);
					y4 = y4 - 1.5f;
				}
			}else if(ch_type == "YM"){
				sashimono.transform.localPosition = new Vector2(-0.8f,0.5f);
				//Location
				if(i<6){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x + 4,prefab.transform.position.y + y1);
					y1 = y1 - 1.5f;
					
				}else if(5<i && i<11){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x + 7,prefab.transform.position.y + y2);
					y2 = y2 - 1.5f;
					
				}else if(10<i && i<16){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x + 10,prefab.transform.position.y + y3);
					y3 = y3 - 1.5f;
					
				}else if(15<i && i<21){
					ch_prefab.transform.position =  new Vector2(prefab.transform.position.x + 13,prefab.transform.position.y + y4);
					y4 = y4 - 1.5f;
				}
			}

			int ch_status = getChildStatus(lv, ch_type, linkNo);

			//Round up because of Link adjustment might be under 0
			int atkDfc = Mathf.CeilToInt(sts.getChAtkDfc(ch_status, hp));

			if (i == 1) {
				//Child Qty
				prefab.GetComponent<EnemyHP>().childQty = ch_num;

				//Child Unit HP
				prefab.GetComponent<EnemyHP>().childHP = ch_status;

				//Attack
				if (ch_type == "YM") {
					prefab.GetComponent<AttackLong> ().childAttack = atkDfc * 3;
					prefab.GetComponent<Heisyu> ().atk = atkDfc * 3;
				} else if (ch_type == "TP") {
					prefab.GetComponent<AttackLong> ().childAttack = atkDfc * 5;
					prefab.GetComponent<Heisyu> ().atk = atkDfc * 5;
				} else {
					prefab.GetComponent<EnemyAttack> ().attack = prefab.GetComponent<EnemyAttack> ().attack + (ch_num*atkDfc);
					prefab.GetComponent<Heisyu> ().atk = atkDfc;
				}

				//Dfc
				prefab.GetComponent<EnemyHP> ().dfc = prefab.GetComponent<EnemyHP> ().dfc + (ch_num*atkDfc);
				prefab.GetComponent<Heisyu> ().dfc = atkDfc;

			}



		}

		//Busyo Config in Shiro or Toride
		if (shiroFlg || torideFlg) {

			//Choose AI Type
			int baseAtk = sts.getBaseAtk (busyoId);
			int baseDfc = sts.getBaseDfc (busyoId);
			int AIType = getAIType(baseAtk,baseDfc, taisyo);

			buildingObj.GetComponent<ShiroSearch> ().busyoObjList.Add(prefab);
			if (ch_type == "YM" || ch_type == "TP") {
				prefab.GetComponent<HomingLong> ().backShiroObj = buildingObj;
				prefab.GetComponent<HomingLong> ().enabled = false;
			} else {
				prefab.GetComponent<Homing> ().backShiroObj = buildingObj;
				prefab.GetComponent<Homing> ().enabled = false;

				//YR & KB
				if (shiroFlg && AIType == 3) {
					AIType = 1;
				}

			}

			//Size
			buildingObj.GetComponent<ShiroSearch> ().busyoObjSize.Add(prefab.transform.localScale);
			prefab.transform.localScale = new Vector2 (0,0);

			//AI
			buildingObj.GetComponent<ShiroSearch> ().AITypeList.Add(AIType);

			//HP
			List<float> randomList = new List<float>(){1.5f,2.0f,2.5f,3.0f,3.5f,4.0f,4.5f,5.0f};
			int rdm = UnityEngine.Random.Range (0, randomList.Count);
			float randomValue = randomList [rdm];
			buildingObj.transform.FindChild("BusyoDtlEnemy").transform.FindChild("MinHpBar").GetComponent<BusyoHPBar>().initLife = (float)hp * randomValue;
			buildingObj.GetComponent<EnemyHP>().life = (float)hp*randomValue;



		}

	}

	public int getAIType(int atk, int dfc, bool taisyo){
		int AIType = 1;
		//1.Zone(30% taisyo*2 ), 2.Atk(AtkPercent), 3.Dfc(100-AtkPercent)
		float basePercent = 30;
		if (taisyo) {
			basePercent = basePercent*2;
		}

		float percent = UnityEngine.Random.value;
		percent = percent * 100;
		if (percent > basePercent) {
			
			float AtkPercent = (float)atk / (atk + dfc) * 100;
			float percent2 = UnityEngine.Random.value;
			percent2 = percent2 * 100;
			if (percent2 > AtkPercent) {
				AIType = 2;
			} else {
				AIType = 3;			
			}
		}

		return AIType;
	}


	public int getChildStatus(int lv, string ch_type, int linkNo){
		Entity_lvch_mst chLvMst  = Resources.Load ("Data/lvch_mst") as Entity_lvch_mst;
		
		int startline = 0;
		if(ch_type == "YR"){
			startline = 0;
		}else if (ch_type == "KB"){
			startline = 1;
		}else if (ch_type == "TP"){
			startline = 2;
		}else if (ch_type == "YM"){
			startline = 3;
		}
		object chStsLst = chLvMst.param[startline];
		Type t = chStsLst.GetType();
		string param = "lv" + lv;
		FieldInfo f = t.GetField(param);
		
		int ch_status =(int)f.GetValue(chStsLst);
		ch_status = ch_status * 10;

		//Adjustment
		if (linkNo != 0) {
			float linkAdjst = (float)linkNo/10;
			float adjstSts = ch_status * linkAdjst;
			ch_status = ch_status + (int)adjstSts;
		}

		return ch_status;
	}

	public void setEnemyObjectOnMap(int mapId, GameObject obj){
		if (mapId == 1) {
			obj.transform.position = new Vector2 (5, 16);
		} else if (mapId == 2) {
			obj.transform.position = new Vector2 (20, 16);
		} else if (mapId == 3) {
			obj.transform.position = new Vector2 (35, 16);
		} else if (mapId == 4) {
			obj.transform.position = new Vector2 (50, 16);
		} else if (mapId == 5) {
			obj.transform.position = new Vector2 (65, 16);
		} else if (mapId == 6) {
			obj.transform.position = new Vector2 (5, 8);
		} else if (mapId == 7) {
			obj.transform.position = new Vector2 (20, 8);
		} else if (mapId == 8) {
			obj.transform.position = new Vector2 (35, 8);
		} else if (mapId == 9) {
			obj.transform.position = new Vector2 (50, 8);
		} else if (mapId == 10) {
			obj.transform.position = new Vector2 (65, 8);
		} else if (mapId == 11) {
			obj.transform.position = new Vector2 (5, 0);
		} else if (mapId == 12) {
			obj.transform.position = new Vector2 (20, 0);
		} else if (mapId == 13) {
			obj.transform.position = new Vector2 (35, 0);
		} else if (mapId == 14) {
			obj.transform.position = new Vector2 (50, 0);
		} else if (mapId == 15) {
			obj.transform.position = new Vector2 (65, 0);
		} else if (mapId == 16) {
			obj.transform.position = new Vector2 (5, -8);
		} else if (mapId == 17) {
			obj.transform.position = new Vector2 (20, -8);
		} else if (mapId == 18) {
			obj.transform.position = new Vector2 (35, -8);
		} else if (mapId == 19) {
			obj.transform.position = new Vector2 (50, -8);
		} else if (mapId == 20) {
			obj.transform.position = new Vector2 (65, -8);
		} else if (mapId == 21) {
			obj.transform.position = new Vector2 (5, -16);
		} else if (mapId == 22) {
			obj.transform.position = new Vector2 (20, -16);
		} else if (mapId == 23) {
			obj.transform.position = new Vector2 (35, -16);
		} else if (mapId == 24) {
			obj.transform.position = new Vector2 (50, -16);
		} else if (mapId == 25) {
			obj.transform.position = new Vector2 (65, -16);
		}
	}

    public void makeKaisenInstance(int mapId, int busyoId, int shipId, int lv, string ch_type, int ch_num, int hp, int atk, int dfc, int spd, string busyoName, int linkNo, bool taisyo, ArrayList senpouArray) {

        string path = "Prefabs/Kaisen/" + shipId;
        GameObject prefab = Instantiate(Resources.Load(path)) as GameObject;
        prefab.name = busyoId.ToString();
        prefab.tag = "Enemy";
        prefab.layer = LayerMask.NameToLayer("Enemy");
        Destroy(prefab.GetComponent<PlayerHP>());
        Destroy(prefab.GetComponent<PlayerAttack>());
        prefab.AddComponent<EnemyHP> ();
        prefab.AddComponent<EnemyAttack>();

        //Senpou Script Parametor
        StatusGet senpouScript = new StatusGet();
        bool shipFlg = senpouScript.getSenpouShipFlg((int)senpouArray[0]);
        if (shipFlg) {
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

        //Script Adjust
        Destroy(prefab.GetComponent<UnitMover>());
        prefab.AddComponent<Homing>();




        //Busyo Detail Info [Name & HP Bar]
        string dtlPath = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            dtlPath = "Prefabs/BusyoDtl/BusyoDtlEnemyEng";
        }else {
            dtlPath = "Prefabs/BusyoDtl/BusyoDtlEnemy";
        }
        GameObject dtl = Instantiate(Resources.Load(dtlPath)) as GameObject;
        dtl.transform.SetParent(prefab.transform);
        dtl.transform.localPosition = new Vector3(0, 1, -1);
        dtl.transform.localScale = new Vector3(1, 1, 0);
        dtl.name = "BusyoDtlEnemy";

        //Name
        GameObject nameLabel = dtl.transform.FindChild("NameLabel").gameObject;
        nameLabel.GetComponent<TextMesh>().text = busyoName;
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            nameLabel.GetComponent<TextMesh>().fontSize = 40;
        }
        //Location by map id
        if (mapId == 1) {
            prefab.transform.position = new Vector2(5, 16);
        }else if (mapId == 2) {
            prefab.transform.position = new Vector2(20, 16);
        }else if (mapId == 3) {
            prefab.transform.position = new Vector2(35, 16);
        }else if (mapId == 4) {
            prefab.transform.position = new Vector2(50, 16);
        }else if (mapId == 5) {
            prefab.transform.position = new Vector2(65, 16);
        }else if (mapId == 6) {
            prefab.transform.position = new Vector2(5, 8);
        }else if (mapId == 7) {
            prefab.transform.position = new Vector2(20, 8);
        }else if (mapId == 8) {
            prefab.transform.position = new Vector2(35, 8);
        }else if (mapId == 9) {
            prefab.transform.position = new Vector2(50, 8);
        }else if (mapId == 10) {
            prefab.transform.position = new Vector2(65, 8);
        }else if (mapId == 11) {
            prefab.transform.position = new Vector2(5, 0);
        }else if (mapId == 12) {
            prefab.transform.position = new Vector2(20, 0);
        }else if (mapId == 13) {
            prefab.transform.position = new Vector2(35, 0);
        }else if (mapId == 14) {
            prefab.transform.position = new Vector2(50, 0);
        }else if (mapId == 15) {
            prefab.transform.position = new Vector2(65, 0);
        }else if (mapId == 16) {
            prefab.transform.position = new Vector2(5, -8);
        }else if (mapId == 17) {
            prefab.transform.position = new Vector2(20, -8);
        }else if (mapId == 18) {
            prefab.transform.position = new Vector2(35, -8);
        }else if (mapId == 19) {
            prefab.transform.position = new Vector2(50, -8);
        }else if (mapId == 20) {
            prefab.transform.position = new Vector2(65, -8);
        }else if (mapId == 21) {
            prefab.transform.position = new Vector2(5, -16);
        }else if (mapId == 22) {
            prefab.transform.position = new Vector2(20, -16);
        }else if (mapId == 23) {
            prefab.transform.position = new Vector2(35, -16);
        }else if (mapId == 24) {
            prefab.transform.position = new Vector2(50, -16);
        }else if (mapId == 25) {
            prefab.transform.position = new Vector2(65, -16);
        }
        
        //Link Adjustment
        float linkAdjst = (float)linkNo / 10;
        hp = hp * 100;
        dfc = dfc * 10;

        if (linkNo != 0) {
            float adjstHp = hp * linkAdjst;
            hp = hp + (int)adjstHp;
            float adjstDfc = dfc * linkAdjst;
            dfc = dfc + (int)adjstDfc;
        }

        //Adjust
        float adjSpd = (float)spd / 10;
        if (adjSpd <= 0) {
            adjSpd = 1;
        }

        if (shipId == 1) {
            hp = hp * 2;
            dfc = dfc * 2;
            adjSpd = Mathf.FloorToInt((float)adjSpd * 0.5f);
        }else if (shipId == 2) {
            hp = Mathf.FloorToInt((float)hp * 1.5f);
            dfc = Mathf.FloorToInt((float)dfc * 1.5f);
            adjSpd = Mathf.FloorToInt((float)adjSpd * 0.6f);
        }else if (shipId == 3) {
            adjSpd = Mathf.FloorToInt((float)adjSpd * 0.8f);
        }
            atk = atk * 10;
        if (linkNo != 0) {
            float adjstAtk = atk * linkAdjst;
            atk = atk + (int)adjstAtk;
        }
        if (adjSpd <= 0) {
            adjSpd = 1;
        }
        GameObject minHpBar = dtl.transform.FindChild("MinHpBar").gameObject;
        minHpBar.GetComponent<BusyoHPBar>().initLife = hp;
        prefab.GetComponent<EnemyHP>().life = hp;
        prefab.GetComponent<EnemyAttack>().attack = atk;       
        prefab.GetComponent<Homing>().speed = adjSpd;
        prefab.GetComponent<EnemyHP>().dfc = dfc;

        if (taisyo) {
            prefab.GetComponent<EnemyHP>().taisyo = true;
        }

        //SE
        AudioController audio = new AudioController();
        audio.addComponentMoveAttack(prefab, "SHP");

        //Child Instantiate
        //set child object
        string ch_path = "Prefabs/Kaisen/3";
        StatusGet sts = new StatusGet();

        for (int i = 1; i <= ch_num; i++) {
            //Make Relationship
            GameObject ch_prefab = Instantiate(Resources.Load(ch_path)) as GameObject;
            ch_prefab.transform.SetParent(prefab.transform);
            ch_prefab.name = "Child" + i.ToString();
            ch_prefab.transform.localScale = new Vector2(0.7f, 0.7f);
            ch_prefab.GetComponent<SpriteRenderer>().sortingOrder = 3;
            ch_prefab.tag = "EnemyChild";

            Destroy(ch_prefab.GetComponent<Rigidbody2D>());
            Destroy(ch_prefab.GetComponent<UnitMover>());
            Destroy(ch_prefab.GetComponent<Kunkou>());
            Destroy(ch_prefab.GetComponent<PolygonCollider2D>());
            Destroy(ch_prefab.GetComponent<PlayerAttack>());
            Destroy(ch_prefab.GetComponent<PlayerHP>());
            Destroy(ch_prefab.GetComponent<SenpouController>());

            //Location
            if (i == 1) {
                ch_prefab.transform.localPosition = new Vector2(1.8f, 0);
            }else if (i == 2) {
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
                ch_prefab.transform.localPosition = new Vector2(-1.8f, 1.0f);
            }else if (i == 14) {
                ch_prefab.transform.localPosition = new Vector2(-1.8f, -1.0f);
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
                ch_prefab.transform.localPosition = new Vector2(-1.8f, -1.5f);
            }


            int ch_status = getChildStatus(lv, ch_type, linkNo);

            //Round up because of Link adjustment might be under 0
            int atkDfc = Mathf.CeilToInt(sts.getChAtkDfc(ch_status, hp));

            if (i == 1) {
                //Child Qty
                prefab.GetComponent<EnemyHP>().childQty = ch_num;

                //Child Unit HP
                prefab.GetComponent<EnemyHP>().childHP = ch_status;

                //Attack
                prefab.GetComponent<EnemyAttack>().attack = prefab.GetComponent<EnemyAttack>().attack + (ch_num * atkDfc);

                //Dfc
                prefab.GetComponent<EnemyHP>().dfc = prefab.GetComponent<EnemyHP>().dfc + (ch_num * atkDfc);
            }



        }
    }
}
