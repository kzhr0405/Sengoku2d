using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class preKassen : MonoBehaviour {

	public int jinkei = 0;
	public int enemyHei = 0;
	EnemyInstance enemyIns = new EnemyInstance();
	public int tempEnemySoudaisyo = 0;
	public bool isAttackedFlg;
	public bool isKessenFlg;
	public int kessenHyourou = 0;
	public bool roujyoFlg = false;
    public List<string> jinkeiBusyo_list = new List<string>();
    public int jinkeiLimit = 0;
    public int busyoCurrentQty = 0;
    public int weatherId = 0;
    public int activeKuniId;
    public List<int> enemyBusyoList;

    void Start () {

        //Sound
        BGMSESwitch bgm = new BGMSESwitch ();
		bgm.StopSEVolume ();
		bgm.StopBGMVolume ();
        int langId = PlayerPrefs.GetInt("langId");
        int senarioId = PlayerPrefs.GetInt("senarioId");

        //Flag
        isAttackedFlg = PlayerPrefs.GetBool ("isAttackedFlg");
		if (!isAttackedFlg) {
			isKessenFlg = PlayerPrefs.GetBool ("isKessenFlg");
		}

		//message
		string kassenMsg="Prefabs/Common/KassenMessage";
		if (isAttackedFlg || isKessenFlg) {
			GameObject msgObj = Instantiate (Resources.Load (kassenMsg)) as GameObject;
			msgObj.transform.SetParent (GameObject.Find ("Panel").transform);
			msgObj.transform.localScale = new Vector2 (1, 1);
			msgObj.transform.localPosition = new Vector2 (0, 0);
			RectTransform msgObjTransform = msgObj.GetComponent<RectTransform> ();
			msgObjTransform.sizeDelta = new Vector2 (1000,150);

			string msgTxt = "";
			int daimyoId = PlayerPrefs.GetInt("activeDaimyoId");
			Daimyo daimyo = new Daimyo ();
			string daimyoName = daimyo.getName (daimyoId,langId,senarioId);

			if (isAttackedFlg) {
				activeKuniId = PlayerPrefs.GetInt ("activeKuniId");
				KuniInfo kuni = new KuniInfo ();
				string kuniName = kuni.getKuniName (activeKuniId,langId);
                
                if (langId == 2) {
                    msgTxt = daimyoName + " is attacking "+kuniName+". Let's defend this country.";
                }else {
                    msgTxt = daimyoName + "が" + kuniName + "に侵攻しています。守り抜きましょうぞ。";
                }
			} else if (isKessenFlg) {
                if (langId == 2) {
                    msgTxt = "It's a time to have a showdown with " + daimyoName + ".";
                }else { 
                    msgTxt = daimyoName + "と雌雄を決する時です。腕が鳴りますな。";
                }
            }
			msgObj.transform.Find ("MessageText").GetComponent<Text> ().text = msgTxt;
		}

		string stageName = PlayerPrefs.GetString ("activeStageName");
		//Stage Name
		if (!isKessenFlg) {
            if (langId == 2) {
                GameObject.Find("KassenNameValue").GetComponent<Text>().text = "Battle of " + stageName;
            }else { 
                GameObject.Find ("KassenNameValue").GetComponent<Text> ().text = stageName + "の戦い";
            }
        } else {
			GameObject.Find ("KassenNameValue").GetComponent<Text> ().text = stageName;
		}

		//Wether Handling
		weatherId = getWeatherId();
		Color rainSnowColor = new Color (140f / 255f, 140f / 255f, 140f / 255f, 255f / 255f);

		//if passive kassen
		Stage stage = new Stage ();
		int stageMapId = 0;

        GameObject panel = GameObject.Find ("Panel").gameObject;
		if (isAttackedFlg) {
			//Passive
			GameObject tettaiBtn = GameObject.Find ("TettaiBtn").gameObject;
			GameObject hyourouIcon = GameObject.Find ("StartBtn").transform.Find ("hyourouIcon").gameObject;
			Destroy (tettaiBtn.gameObject);
			Destroy (hyourouIcon.gameObject);

			int activeKuniId  = PlayerPrefs.GetInt("activeKuniId");
			stageMapId = stage.getStageMap (activeKuniId, 10);
            if (stageMapId != 1 && stageMapId != 2 && stageMapId != 3) {
                stageMapId = 1;
            }

            string mapPath = "";
			string mapFrontPath = "";

			if (stageMapId != 1) {

				GameObject frontImage = panel.transform.Find ("FrontImage").gameObject;
				frontImage.GetComponent<Image> ().enabled = true;

				if (stageMapId == 2) {
					//mountain
					mapPath = "Prefabs/PreKassen/map/map2";
					panel.GetComponent<Image> ().sprite = 
						Resources.Load (mapPath, typeof(Sprite)) as Sprite;

					mapFrontPath = "Prefabs/PreKassen/frontMap/mapFront2";
					frontImage.GetComponent<Image> ().sprite = 
						Resources.Load (mapFrontPath, typeof(Sprite)) as Sprite;

					if (weatherId == 2 || weatherId == 3) {
						panel.GetComponent<Image> ().color = rainSnowColor;
						frontImage.GetComponent<Image> ().color = rainSnowColor;
					}


				} else if (stageMapId == 3) {
					//sea
					mapPath = "Prefabs/PreKassen/map/map3";
					panel.GetComponent<Image> ().sprite = 
						Resources.Load (mapPath, typeof(Sprite)) as Sprite;

					mapFrontPath = "Prefabs/PreKassen/frontMap/mapFront3";
					frontImage.GetComponent<Image> ().sprite = 
						Resources.Load (mapFrontPath, typeof(Sprite)) as Sprite;

					if (weatherId == 2 || weatherId == 3) {
						panel.GetComponent<Image> ().color = rainSnowColor;
						frontImage.GetComponent<Image> ().color = rainSnowColor;
					}
				}
			} else {
				if (weatherId == 2 || weatherId == 3) {
					panel.GetComponent<Image> ().color = rainSnowColor;
				}
			}
		} else {

            //Normal or Kessen
            if (isKessenFlg) {
				kessenHyourou = PlayerPrefs.GetInt ("kessenHyourou");

				GameObject startBtn = GameObject.Find ("StartBtn");
				startBtn.transform.Find ("hyourouIcon").Find ("hyourouNoValue").GetComponent<Text> ().text = kessenHyourou.ToString ();

				int harf = kessenHyourou / 2;
				GameObject tettaiBtn = GameObject.Find ("TettaiBtn");
				tettaiBtn.transform.Find ("hyourouIcon").Find ("hyourouNoValue").GetComponent<Text> ().text = harf.ToString ();

				int activeStageId =  UnityEngine.Random.Range(1,4);
				stageMapId = stage.getStageMap (1, activeStageId); 

				PlayerPrefs.SetInt("activeKuniId",1);
				PlayerPrefs.SetInt("activeStageId",activeStageId);

			} else {
				int activeKuniId  = PlayerPrefs.GetInt("activeKuniId");
				int activeStageId = PlayerPrefs.GetInt("activeStageId");
				stageMapId = stage.getStageMap (activeKuniId, activeStageId); 

			}

            if (stageMapId != 1 && stageMapId != 2 && stageMapId != 3) {
                stageMapId = 1;
            }
            string mapPath = "";
			string mapFrontPath = "";
			if (stageMapId != 1) {
				
				GameObject frontImage = panel.transform.Find("FrontImage").gameObject;
				frontImage.GetComponent<Image> ().enabled = true;

				if (stageMapId == 2) {
					//mountain
					mapPath = "Prefabs/PreKassen/map/map2";
					panel.GetComponent<Image> ().sprite = 
						Resources.Load (mapPath, typeof(Sprite)) as Sprite;

					mapFrontPath = "Prefabs/PreKassen/frontMap/mapFront2";
					frontImage.GetComponent<Image> ().sprite = 
						Resources.Load (mapFrontPath, typeof(Sprite)) as Sprite;

					if (weatherId == 2 || weatherId == 3) {
						panel.GetComponent<Image> ().color = rainSnowColor;
						frontImage.GetComponent<Image> ().color = rainSnowColor;
					}


				} else if (stageMapId == 3) {
					//sea
					mapPath = "Prefabs/PreKassen/map/map3";
					panel.GetComponent<Image> ().sprite = 
						Resources.Load (mapPath, typeof(Sprite)) as Sprite;

					mapFrontPath = "Prefabs/PreKassen/frontMap/mapFront3";
					frontImage.GetComponent<Image> ().sprite = 
						Resources.Load (mapFrontPath, typeof(Sprite)) as Sprite;

					if (weatherId == 2 || weatherId == 3) {
						panel.GetComponent<Image> ().color = rainSnowColor;
						frontImage.GetComponent<Image> ().color = rainSnowColor;
					}
				}
			}else {
				if (weatherId == 2 || weatherId == 3) {
					panel.GetComponent<Image> ().color = rainSnowColor;
				}
			}

        }

        /*Status Down by Weather & Map*/
        //Delete Previous minus status
        PlayerPrefs.DeleteKey("mntMinusStatus");
		PlayerPrefs.DeleteKey("seaMinusStatus");
		PlayerPrefs.DeleteKey("rainMinusStatus");
		PlayerPrefs.DeleteKey("snowMinusStatus");

		string txtPath = "Prefabs/PreKassen/TextSlot";
		GameObject content = GameObject.Find ("EffectContent").gameObject;
		if (stageMapId == 2) {
			//mnt
			List<int> idList = new List<int> (){10,20,30};
			int rdmId = UnityEngine.Random.Range(0,idList.Count);
			int minusRatio = idList[rdmId];

			GameObject slot = Instantiate (Resources.Load (txtPath)) as GameObject;
			slot.transform.SetParent (content.transform);
			GameObject text1 = slot.transform.Find ("Text1").gameObject;           
            if (langId == 2) {
                text1.GetComponent<Text> ().text = "Cavalry Unit Speed";
            }else {
                text1.GetComponent<Text>().text = "地形効果 騎馬隊 迅速";
            }
			text1.transform.Find ("Text2").GetComponent<Text> ().text = "-" + minusRatio + "%";
			slot.transform.localScale = new Vector2 (1,1);
			slot.transform.localPosition = new Vector3 (0,0,0);

			float minuValue = (100 - (float)minusRatio) / 100;
			PlayerPrefs.SetFloat("mntMinusStatus",minuValue);

		} else if(stageMapId == 3){
			//sea
			List<int> idList = new List<int> (){10,20,30};
			int rdmId = UnityEngine.Random.Range(0,idList.Count);
			int minusRatio = idList[rdmId];

			GameObject slot = Instantiate (Resources.Load (txtPath)) as GameObject;
			slot.transform.SetParent (content.transform);
			GameObject text1 = slot.transform.Find ("Text1").gameObject;
            if (langId == 2) {
                text1.GetComponent<Text> ().text = "Matchlock Unit Defence";
            }else {
                text1.GetComponent<Text>().text = "地形効果 鉄砲隊 守備";
            }
			text1.transform.Find ("Text2").GetComponent<Text> ().text = "-" + minusRatio + "%";
			slot.transform.localScale = new Vector2 (1,1);
			slot.transform.localPosition = new Vector3 (0,0,0);

			GameObject slot2 = Instantiate (Resources.Load (txtPath)) as GameObject;
			slot2.transform.SetParent (content.transform);
			GameObject text2 = slot2.transform.Find ("Text1").gameObject;
            if (langId == 2) {
                text2.GetComponent<Text>().text = "Bow Unit Defence";
            }else { 
                text2.GetComponent<Text> ().text = "地形効果 弓隊 守備";
            }
            text2.transform.Find ("Text2").GetComponent<Text> ().text = "-" + minusRatio + "%";
			slot2.transform.localScale = new Vector2 (1,1);
			slot2.transform.localPosition = new Vector3 (0,0,0);

			float minuValue = (100 - (float)minusRatio) / 100;
			PlayerPrefs.SetFloat("seaMinusStatus",minuValue);
		}
		if(weatherId == 2){
			//rain
			List<int> idList = new List<int> (){10,20,30};
			int rdmId = UnityEngine.Random.Range(0,idList.Count);
			int minusRatio = idList[rdmId];

			GameObject slot = Instantiate (Resources.Load (txtPath)) as GameObject;
			slot.transform.SetParent (content.transform);
			GameObject text = slot.transform.Find ("Text1").gameObject;
            if (langId == 2) {
                text.GetComponent<Text>().text = "Matchlock Unit Attack";
            }else {
                text.GetComponent<Text>().text = "気象効果 鉄砲隊 武勇";
            }
			text.transform.Find ("Text2").GetComponent<Text> ().text = "-" + minusRatio + "%";
			slot.transform.localScale = new Vector2 (1,1);
			slot.transform.localPosition = new Vector3 (0,0,0);

			GameObject slot2 = Instantiate (Resources.Load (txtPath)) as GameObject;
			slot2.transform.SetParent (content.transform);
			GameObject text2 = slot2.transform.Find ("Text1").gameObject;
            if (langId == 2) {
                text2.GetComponent<Text>().text = "Bow Unit Attack";
            }else {
                text2.GetComponent<Text> ().text = "気象効果 弓隊 武勇";
            }
			text2.transform.Find ("Text2").GetComponent<Text> ().text = "-" + minusRatio + "%";
			slot2.transform.localScale = new Vector2 (1,1);
			slot2.transform.localPosition = new Vector3 (0,0,0);

			float minuValue = (100 - (float)minusRatio) / 100;
			PlayerPrefs.SetFloat("rainMinusStatus",minuValue);
		
		}else if(weatherId == 3){
			//snow
			List<int> idList = new List<int> (){10,20,30};
			int rdmId = UnityEngine.Random.Range(0,idList.Count);
			int minusRatio = idList[rdmId];

			GameObject slot = Instantiate (Resources.Load (txtPath)) as GameObject;
			slot.transform.SetParent (content.transform);
			GameObject text = slot.transform.Find ("Text1").gameObject;
            if (langId == 2) {
                text.GetComponent<Text>().text = "Cavalry Unit Defence";
            }else {
                text.GetComponent<Text> ().text = "気象効果 騎馬隊 守備";
            }
			text.transform.Find ("Text2").GetComponent<Text> ().text = "-" + minusRatio + "%";
			slot.transform.localScale = new Vector2 (1,1);
			slot.transform.localPosition = new Vector3 (0,0,0);

			GameObject slot2 = Instantiate (Resources.Load (txtPath)) as GameObject;
			slot2.transform.SetParent (content.transform);
			GameObject text2 = slot2.transform.Find ("Text1").gameObject;
            if (langId == 2) {
                text2.GetComponent<Text>().text = "Matchlock Unit Attack";
            }else {
                text2.GetComponent<Text> ().text = "気象効果 鉄砲隊 武勇";
            }
			text2.transform.Find ("Text2").GetComponent<Text> ().text = "-" + minusRatio + "%";
			slot2.transform.localScale = new Vector2 (1,1);
			slot2.transform.localPosition = new Vector3 (0,0,0);

			GameObject slot3 = Instantiate (Resources.Load (txtPath)) as GameObject;
			slot3.transform.SetParent (content.transform);
			GameObject text3 = slot3.transform.Find ("Text1").gameObject;
            if (langId == 2) {
                text3.GetComponent<Text>().text = "Bow Unit Attack";
            }else {
                text3.GetComponent<Text> ().text = "気象効果 弓隊 武勇";
            }
			text3.transform.Find ("Text2").GetComponent<Text> ().text = "-" + minusRatio + "%";
			slot3.transform.localScale = new Vector2 (1,1);
			slot3.transform.localPosition = new Vector3 (0,0,0);

			GameObject slot4 = Instantiate (Resources.Load (txtPath)) as GameObject;
			slot4.transform.SetParent (content.transform);
			GameObject text4 = slot4.transform.Find ("Text1").gameObject;
            if (langId == 2) {
                text4.GetComponent<Text>().text = "All Unit Speed";
            }else {
                text4.GetComponent<Text> ().text = "気象効果 全隊 迅速";
            }
			text4.transform.Find ("Text2").GetComponent<Text> ().text = "-50%";
			slot4.transform.localScale = new Vector2 (1,1);
			slot4.transform.localPosition = new Vector3 (0,0,0);

			float minuValue = (100 - (float)minusRatio) / 100;
			PlayerPrefs.SetFloat("snowMinusStatus",minuValue);
		}
		if (stageMapId == 1 && weatherId == 1) {
			string noTxtPath = "Prefabs/PreKassen/NoSlot";
			GameObject slot = Instantiate (Resources.Load (noTxtPath)) as GameObject;
			slot.transform.SetParent (content.transform);
			slot.transform.localScale = new Vector2 (1,1);
			slot.transform.localPosition = new Vector3 (0,0,0);
		}
		//boubi addition
		if (isAttackedFlg) {
			
			int boubi = PlayerPrefs.GetInt("activeBoubi", 0);

			GameObject slot = Instantiate (Resources.Load (txtPath)) as GameObject;
			slot.transform.SetParent (content.transform);
			GameObject text = slot.transform.Find ("Text1").gameObject;
            if (langId == 2) {
                text.GetComponent<Text>().text = "Player Unit Defence";
            }else {
                text.GetComponent<Text> ().text = "防備効果 味方 守備";
            }
			text.transform.Find ("Text2").GetComponent<Text> ().text = "+" + boubi;
			slot.transform.localScale = new Vector2 (1,1);
			slot.transform.localPosition = new Vector3 (0,0,0);

		}


		PlayerPrefs.Flush ();

        /*Plyaer Jinkei*/
        jinkei = PlayerPrefs.GetInt ("jinkei");
        if (jinkei == 0) {
            jinkei = 1;
            PlayerPrefs.SetInt("jinkei", jinkei);
            PlayerPrefs.Flush();
        }
        changeFormButtonColor(jinkei);

        int stageId = PlayerPrefs.GetInt("activeStageId");
        bool strongFlg = false;
        if(stageId == 5 || stageId == 10) {
            strongFlg = true;
        }
        strongFlg = PlayerPrefs.GetBool("lastOneFlg");
        PlayerPrefs.DeleteKey("lastOneFlg");
        System.Diagnostics.Stopwatch sw5 = new System.Diagnostics.Stopwatch();
        prekassenPlayerJinkei(jinkei, weatherId, isAttackedFlg, false, strongFlg, senarioId);

    }


	public int getTaisyoMapId(int enemyJinkei){
		int taisyoMapId = 0;
		if (enemyJinkei == 1) {
			taisyoMapId = 15;
		} else if (enemyJinkei == 2) {
			taisyoMapId = 15;
		} else if (enemyJinkei == 3) {
			taisyoMapId = 14;
		} else if (enemyJinkei == 4) {
			taisyoMapId = 14;
		}
		return taisyoMapId;
	}


	public List<int> enemyJinkeiMaker(int enemyJinkei){

		/*Jinkei*/
		List<int> mapList = new List<int>();
		if (enemyJinkei == 1) {
			
			mapList = new List<int>(){4,5,8,9,12,13,14,18,19,24,25};
			
			//Disable 1,2,3,6,7,10,11,16,17,20,21,22,23
			foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("EnemySlot")){
				if(obs.name == "Slot1" || obs.name == "Slot2" ||obs.name == "Slot3" ||obs.name == "Slot6" ||
				   obs.name == "Slot7" ||obs.name == "Slot10" ||obs.name == "Slot11" ||obs.name == "Slot16" ||obs.name == "Slot17" ||
				   obs.name == "Slot20" ||obs.name == "Slot21" ||obs.name == "Slot22" ||obs.name == "Slot23"){
					
					obs.GetComponent<Image>().enabled = false;
					if(obs.transform.IsChildOf(obs.transform)){
						
						foreach ( Transform n in obs.transform ){
							GameObject.Destroy(n.gameObject);
						}	
					}			
				}
			}
			
		}else if (enemyJinkei == 2) {
			
			mapList = new List<int>(){1,2,3,8,9,14,18,19,21,22,23};
			
			//Disable 4,5,6,7,10,11,12,13,16,17,20,24,25
			foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("EnemySlot")){
				if(obs.name == "Slot4" || obs.name == "Slot5" ||obs.name == "Slot6" ||obs.name == "Slot7" ||
				   obs.name == "Slot10" ||obs.name == "Slot11" ||obs.name == "Slot12" ||obs.name == "Slot13" ||
				   obs.name == "Slot16" ||obs.name == "Slot17" ||obs.name == "Slot20" ||obs.name == "Slot24" ||obs.name == "Slot25"){
					
					obs.GetComponent<Image>().enabled = false;
					if(obs.transform.IsChildOf(obs.transform)){
						
						foreach ( Transform n in obs.transform ){
							GameObject.Destroy(n.gameObject);
						}	
					}			
				}
			}
			
		}else if (enemyJinkei == 3) {
			
			mapList = new List<int>(){1,5,6,10,11,12,15,17,18,19,23};
			
			//Disable 2,3,4,7,8,9,13,16,20,21,22,24,25
			foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("EnemySlot")){
				if(obs.name == "Slot2" || obs.name == "Slot3" ||obs.name == "Slot4" ||obs.name == "Slot7" ||
				   obs.name == "Slot8" ||obs.name == "Slot9" ||obs.name == "Slot13" ||obs.name == "Slot16" ||
				   obs.name == "Slot20" ||obs.name == "Slot21" ||obs.name == "Slot22" ||obs.name == "Slot24" ||obs.name == "Slot25"){
					
					obs.GetComponent<Image>().enabled = false;
					if(obs.transform.IsChildOf(obs.transform)){
						
						foreach ( Transform n in obs.transform ){
							GameObject.Destroy(n.gameObject);
						}	
					}			
				}
			}
			
		}else if (enemyJinkei == 4) {
			
			mapList = new List<int>(){4,5,8,9,12,13,16,17,18,21,22};
			
			//Disable 1,2,3,6,7,10,11,15,19,20,23,24,25
			foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("EnemySlot")){
				if(obs.name == "Slot1" || obs.name == "Slot2" ||obs.name == "Slot3" ||obs.name == "Slot6" ||
				   obs.name == "Slot7" ||obs.name == "Slot10" ||obs.name == "Slot11" ||obs.name == "Slot15" ||
				   obs.name == "Slot19" ||obs.name == "Slot20" ||obs.name == "Slot23" ||obs.name == "Slot24" ||obs.name == "Slot25"){
					
					obs.GetComponent<Image>().enabled = false;
					if(obs.transform.IsChildOf(obs.transform)){
						
						foreach ( Transform n in obs.transform ){
							GameObject.Destroy(n.gameObject);
						}	
					}			
				}
			}
		}
		return mapList;
	}


	//PowerType1
	//Busyo + Mob
	public int powerType1(List<int> mapList, int taisyoMapId, int linkNo, int activeDaimyoId, bool strongFlg){
        int langId = PlayerPrefs.GetInt("langId");
        int totalHei = 0;

		int activeBusyoQty = PlayerPrefs.GetInt ("activeBusyoQty");
		int activeBusyoLv = PlayerPrefs.GetInt ("activeBusyoLv");
		int activeButaiQty = PlayerPrefs.GetInt ("activeButaiQty");
		int activeButaiLv = PlayerPrefs.GetInt ("activeButaiLv");
		Entity_daimyo_mst daimyoMst = Resources.Load ("Data/daimyo_mst") as Entity_daimyo_mst;
		int daimyoBusyoId = daimyoMst.param[activeDaimyoId-1].busyoId;

		/*Busyo Master Setting Start*/
		//Active Busyo List
		List<string> metsubouDaimyoList = new List<string> ();
		string metsubouTemp = "metsubou" + activeDaimyoId;
		string metsubouDaimyoString = PlayerPrefs.GetString (metsubouTemp);
		char[] delimiterChars2 = {','};
		if (metsubouDaimyoString != null && metsubouDaimyoString != "") {
			if (metsubouDaimyoString.Contains (",")) {
				metsubouDaimyoList = new List<string> (metsubouDaimyoString.Split (delimiterChars2));
			} else {
				metsubouDaimyoList = new List<string> (metsubouDaimyoString.Split (delimiterChars2));
			}
		}
		metsubouDaimyoList.Add (activeDaimyoId.ToString ());


		Entity_busyo_mst busyoMst = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;
		List<int> busyoList = new List<int> ();
		
		for (int i=0; i<busyoMst.param.Count; i++) {
			int busyoId = busyoMst.param [i].id;
			int daimyoId = busyoMst.param [i].daimyoId;
			
			if (metsubouDaimyoList.Contains (daimyoId.ToString ())) {
				if (busyoId != daimyoBusyoId) {
					
					busyoList.Add (busyoId);
				}
			}
		}
        /*Busyo Master Setting End*/

        if (strongFlg) {
            BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();
            List<int> SrankList = new List<int>();
            List<int> ArankList = new List<int>();
            List<int> BrankList = new List<int>();
            List<int> CrankList = new List<int>();

            for (int i = 0; i < busyoList.Count; i++) {
                //Strong Sort S > C rank
                int temp = busyoList[i];
                string rank = BusyoInfoGet.getRank(temp);
                if (rank == "S") {
                    SrankList.Add(temp);
                }else if (rank == "A") {
                    ArankList.Add(temp);
                }else if (rank == "B") {
                    BrankList.Add(temp);
                }else if (rank == "C") {
                    CrankList.Add(temp);
                }
            }
            busyoList = new List<int>();
            busyoList.AddRange(SrankList);
            busyoList.AddRange(ArankList);
            busyoList.AddRange(BrankList);
            busyoList.AddRange(CrankList);

        } else {
            for (int i = 0; i < busyoList.Count; i++) {
                //Random Shuffle
                int temp = busyoList[i];
                int randomIndex = UnityEngine.Random.Range(0, busyoList.Count);
                busyoList[i] = busyoList[randomIndex];
                busyoList[randomIndex] = temp;
            }
        }

        for (int i = 0; i < mapList.Count; i++) {
			int temp = mapList [i];
			int randomIndex = UnityEngine.Random.Range (0, mapList.Count);
			mapList [i] = mapList [randomIndex];
			mapList [randomIndex] = temp;
		}
		
		
		/*Taisyo Setting Start*/
		GameObject EnemyJinkeiView = GameObject.Find ("EnemyJinkeiView").gameObject;
		int taisyoBusyoId = busyoList[0];
		
		StatusGet sts = new StatusGet();
		BusyoInfoGet info = new BusyoInfoGet();
		
		int hp = sts.getHp(taisyoBusyoId,activeBusyoLv);
		hp = hp * 100;

		//Roujyo Check
		if (!isAttackedFlg && !isKessenFlg) {
			int atk = sts.getBaseAtk (taisyoBusyoId);
			int dfc = sts.getBaseDfc (taisyoBusyoId);
			roujyoFlg = checkRoujyo (atk, dfc);
		}


		//Link Status Adjustment
		if (linkNo != 0) {
			float linkAdjst = (float)linkNo/10;
			float adjstHp = hp * linkAdjst;
			hp = hp + (int)adjstHp;
		}

		string TaisyoBusyoName = info.getName(taisyoBusyoId,langId);
		string TaisyoType = info.getHeisyu(taisyoBusyoId);

		int chldHp = activeButaiQty * enemyIns.getChildStatus (activeButaiLv, TaisyoType, linkNo);

		totalHei = hp + chldHp;

		string path = "Prefabs/Player/Unit/BusyoUnit";
		GameObject tsyBusyo = Instantiate(Resources.Load (path)) as GameObject;
		tsyBusyo.name = taisyoBusyoId.ToString ();

		string slotName = "Slot" + taisyoMapId;
		tsyBusyo.transform.SetParent(EnemyJinkeiView.transform.Find(slotName).transform);
		tsyBusyo.name = taisyoBusyoId.ToString();
		tsyBusyo.transform.localScale = new Vector2(-3,3);
		tsyBusyo.transform.localPosition = new Vector3(0, 0, 0);
		tempEnemySoudaisyo = int.Parse (tsyBusyo.name);

		//Roujyo Object Make
		if (roujyoFlg && !isAttackedFlg) {
			//Make Shiro on Soudaisyo
			makeRoujyoObj(true,EnemyJinkeiView.transform.Find(slotName).gameObject,tsyBusyo,1);
		}

		//Button
		string soudaisyoPath = "Prefabs/Jinkei/soudaisyo";
		GameObject soudaisyo = Instantiate (Resources.Load (soudaisyoPath)) as GameObject;
		soudaisyo.transform.SetParent (tsyBusyo.transform);
		soudaisyo.transform.localScale = new Vector2 (27, 12);
		soudaisyo.name = "enemySoudaisyo";
		soudaisyo.transform.localPosition = new Vector3(0, 11, 0);
		tsyBusyo.GetComponent<DragHandler> ().enabled = false;
		
		//Label & Text
		GameObject txtObj = tsyBusyo.transform.Find ("Text").gameObject;
		
		Vector2 txtScale = txtObj.transform.localScale;
		txtScale.x *= -1;
		txtObj.transform.localScale = txtScale;
		Vector2 txtPos = txtObj.transform.localPosition;
		txtPos.x *= -1;
		txtObj.transform.localPosition = txtPos;

		GameObject rblObj = tsyBusyo.transform.Find ("Rank").gameObject;
		Vector2 rblScale = rblObj.transform.localScale;
		rblScale.x *= -1;
		rblObj.transform.localScale = rblScale;
		Vector2 rblPos = rblObj.transform.localPosition;
		rblPos.x *= -1;
		rblObj.transform.localPosition = rblPos;

		GameObject tsyTxtObj = tsyBusyo.transform.Find ("enemySoudaisyo").transform.Find("Text").gameObject;
		Vector2 tsyScale = tsyTxtObj.transform.localScale;
		tsyScale.x *= -1;
		tsyTxtObj.transform.localScale = tsyScale;
		Vector2 tsyPos = tsyTxtObj.transform.localPosition;
		tsyPos.x *= -1;
		tsyTxtObj.transform.localPosition = tsyPos;

		/*Taisyo Setting End*/
		
		
		//Make Instance
		int busyoListCount = busyoList.Count-1;
		int torideQty = linkNo;
		for (int j=0; j<activeBusyoQty-1; j++) {
			
			if (busyoListCount > 0) {
				busyoListCount = busyoListCount - 1;
				int mapId = mapList [j];
				
				//samurai daisyo make
				int busyoHp = sts.getHp (35, activeBusyoLv);
				busyoHp = busyoHp* 100;

				//Link Status Adjustment
				if (linkNo != 0) {
					float linkAdjst = (float)linkNo/10;
					float adjstHp = busyoHp * linkAdjst;
					busyoHp = busyoHp + (int)adjstHp;
				}

				string busyoName = info.getName (35,langId);
				string[] texts = new string[] { "YR", "KB"};
				string busyoType = texts[UnityEngine.Random.Range(0, texts.Length - 1)];

				int chldHp2 = activeButaiQty * enemyIns.getChildStatus (activeButaiLv, busyoType, linkNo);
				totalHei = totalHei + busyoHp + chldHp2;

				string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
				GameObject chdBusyo = Instantiate(Resources.Load (busyoPath)) as GameObject;
				chdBusyo.name = "35";

				string chdSlotName = "Slot" + mapId;
				chdBusyo.transform.SetParent(EnemyJinkeiView.transform.Find(chdSlotName).transform);
				chdBusyo.transform.localScale = new Vector2(-3,3);
				chdBusyo.transform.localPosition = new Vector3(0, 0, 0);

				//Button
				chdBusyo.GetComponent<DragHandler> ().enabled = false;

				//Rabel & Text
				GameObject chTxtObj = chdBusyo.transform.Find ("Text").gameObject;
				Vector2 chTxtScale = chTxtObj.transform.localScale;
				chTxtScale.x *= -1;
				chTxtObj.transform.localScale = chTxtScale;
				Vector2 chTxtPos = chTxtObj.transform.localPosition;
				chTxtPos.x *= -1;
				chTxtObj.transform.localPosition = chTxtPos;

                GameObject chLblObj = chdBusyo.transform.Find("Rank").gameObject;
                Vector2 chLblScale = chLblObj.transform.localScale;
                chLblScale.x *= -1;
                chLblObj.transform.localScale = chLblScale;
                Vector2 chLblPos = chLblObj.transform.localPosition;
                chLblPos.x *= -1;
                chLblObj.transform.localPosition = chLblPos;



                //Roujyo
                if (roujyoFlg && !isAttackedFlg) {
					if (torideQty != 0) {
						int baseAtk = sts.getBaseAtk (35);
						int baseDfc = sts.getBaseDfc (35);

						if (checkRoujyo(baseAtk, baseDfc)) {
							torideQty = torideQty - 1;
							makeRoujyoObj (false, EnemyJinkeiView.transform.Find (chdSlotName).gameObject, chdBusyo, 1);
						}
					}
				}
			}
		}




		return totalHei;
	}




	//PowerType2
	//Busyo + Busyo
	public int powerType2(List<int> mapList, int taisyoMapId, int linkNo,  int activeDaimyoId, bool strongFlg){
        int langId = PlayerPrefs.GetInt("langId");
        int totalHei = 0;

		int activeBusyoQty = PlayerPrefs.GetInt ("activeBusyoQty");
		int activeBusyoLv = PlayerPrefs.GetInt ("activeBusyoLv");
		int activeButaiQty = PlayerPrefs.GetInt ("activeButaiQty");
		int activeButaiLv = PlayerPrefs.GetInt ("activeButaiLv");
		Entity_daimyo_mst daimyoMst = Resources.Load ("Data/daimyo_mst") as Entity_daimyo_mst;
		int daimyoBusyoId = daimyoMst.param[activeDaimyoId-1].busyoId;


		/*Busyo Master Setting Start*/
		//Active Busyo List
		List<string> metsubouDaimyoList = new List<string> ();
		string metsubouTemp = "metsubou" + activeDaimyoId;
		string metsubouDaimyoString = PlayerPrefs.GetString (metsubouTemp);
		char[] delimiterChars2 = {','};
		if (metsubouDaimyoString != null && metsubouDaimyoString != "") {
			if (metsubouDaimyoString.Contains (",")) {
				metsubouDaimyoList = new List<string> (metsubouDaimyoString.Split (delimiterChars2));
			} else {
				metsubouDaimyoList = new List<string> (metsubouDaimyoString.Split (delimiterChars2));
			}
		}
		metsubouDaimyoList.Add (activeDaimyoId.ToString ());
		
		Entity_busyo_mst busyoMst = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;
		List<int> busyoList = new List<int> ();
		
		for (int i=0; i<busyoMst.param.Count; i++) {
			int busyoId = busyoMst.param [i].id;
			int daimyoId = busyoMst.param [i].daimyoId;
			
			if (metsubouDaimyoList.Contains (daimyoId.ToString ())) {
				if (busyoId != daimyoBusyoId) {
					
					busyoList.Add (busyoId);
				}
			}
		}
        /*Busyo Master Setting End*/

        if (strongFlg) {
            BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();
            List<int> SrankList = new List<int>();
            List<int> ArankList = new List<int>();
            List<int> BrankList = new List<int>();
            List<int> CrankList = new List<int>();

            for (int i = 0; i < busyoList.Count; i++) {
                //Strong Sort S > C rank
                int temp = busyoList[i];
                string rank = BusyoInfoGet.getRank(temp);
                if (rank == "S") {
                    SrankList.Add(temp);
                }
                else if (rank == "A") {
                    ArankList.Add(temp);
                }
                else if (rank == "B") {
                    BrankList.Add(temp);
                }
                else if (rank == "C") {
                    CrankList.Add(temp);
                }
            }
            busyoList = new List<int>();
            busyoList.AddRange(SrankList);
            busyoList.AddRange(ArankList);
            busyoList.AddRange(BrankList);
            busyoList.AddRange(CrankList);
        }else {
            for (int i = 0; i < busyoList.Count; i++) {
                //Random Shuffle
                int temp = busyoList[i];
                int randomIndex = UnityEngine.Random.Range(0, busyoList.Count);
                busyoList[i] = busyoList[randomIndex];
                busyoList[randomIndex] = temp;
            }
        }

        for (int i = 0; i < mapList.Count; i++) {
			int temp = mapList [i];
			int randomIndex = UnityEngine.Random.Range (0, mapList.Count);
			mapList [i] = mapList [randomIndex];
			mapList [randomIndex] = temp;
		}
		
		
		/*Taisyo Setting Start*/
		GameObject EnemyJinkeiView = GameObject.Find ("EnemyJinkeiView").gameObject;
		int taisyoBusyoId = busyoList[0];
		
		StatusGet sts = new StatusGet();
		BusyoInfoGet info = new BusyoInfoGet();
		
		int hp = sts.getHp(taisyoBusyoId,activeBusyoLv);
		hp = hp * 100;

		//Roujyo Check
		if (!isAttackedFlg) {
			int atk = sts.getBaseAtk (taisyoBusyoId);
			int dfc = sts.getBaseDfc (taisyoBusyoId);
			roujyoFlg = checkRoujyo (atk, dfc);
		}

		//Link Status Adjustment
		if (linkNo != 0) {
			float linkAdjst = (float)linkNo/10;
			float adjstHp = hp * linkAdjst;
			hp = hp + (int)adjstHp;
		}

		string TaisyoBusyoName = info.getName(taisyoBusyoId,langId);
		string TaisyoType = info.getHeisyu(taisyoBusyoId);

		int chldHp = activeButaiQty * enemyIns.getChildStatus (activeButaiLv, TaisyoType, linkNo);
		totalHei = hp + chldHp;

		string path = "Prefabs/Player/Unit/BusyoUnit";
		GameObject tsyBusyo = Instantiate(Resources.Load (path)) as GameObject;
		tsyBusyo.name = taisyoBusyoId.ToString ();

		string slotName = "Slot" + taisyoMapId;
		tsyBusyo.transform.SetParent(EnemyJinkeiView.transform.Find(slotName).transform);
		tsyBusyo.name = taisyoBusyoId.ToString();
		tsyBusyo.transform.localScale = new Vector2(-3,3);
		tsyBusyo.transform.localPosition = new Vector3(0, 0, 0);
		tempEnemySoudaisyo = int.Parse (tsyBusyo.name);

		//Roujyo Object Make
		if (roujyoFlg && !isAttackedFlg) {
			//Make Shiro on Soudaisyo
			makeRoujyoObj(true,EnemyJinkeiView.transform.Find(slotName).gameObject,tsyBusyo,2);
		}


		//Button
		string soudaisyoPath = "Prefabs/Jinkei/soudaisyo";
		GameObject soudaisyo = Instantiate (Resources.Load (soudaisyoPath)) as GameObject;
		soudaisyo.transform.SetParent (tsyBusyo.transform);
		soudaisyo.transform.localScale = new Vector2 (27, 12);
		soudaisyo.name = "enemySoudaisyo";
		soudaisyo.transform.localPosition = new Vector3(0, 11, 0);
		tsyBusyo.GetComponent<DragHandler> ().enabled = false;

		//Text
		GameObject txtObj = tsyBusyo.transform.Find ("Text").gameObject;
		Vector2 txtScale = txtObj.transform.localScale;
		txtScale.x *= -1;
		txtObj.transform.localScale = txtScale;
		Vector2 txtPos = txtObj.transform.localPosition;
		txtPos.x *= -1;
		txtObj.transform.localPosition = txtPos;

		GameObject rblObj = tsyBusyo.transform.Find ("Rank").gameObject;
		Vector2 rblScale = rblObj.transform.localScale;
		rblScale.x *= -1;
		rblObj.transform.localScale = rblScale;
		Vector2 rblPos = rblObj.transform.localPosition;
		rblPos.x *= -1;
		rblObj.transform.localPosition = rblPos;

		GameObject tsyTxtObj = tsyBusyo.transform.Find ("enemySoudaisyo").transform.Find("Text").gameObject;
		Vector2 tsyScale = tsyTxtObj.transform.localScale;
		tsyScale.x *= -1;
		tsyTxtObj.transform.localScale = tsyScale;
		Vector2 tsyPos = tsyTxtObj.transform.localPosition;
		tsyPos.x *= -1;
		tsyTxtObj.transform.localPosition = tsyPos;

		/*Taisyo Setting End*/
		
		
		//Make Instance
		busyoList.Remove (taisyoBusyoId);
		int busyoListCount = busyoList.Count;
		int torideQty = linkNo;
		for (int j=0; j<activeBusyoQty-1; j++) {
			
			if (busyoListCount > 0) {
				int randomBusyoId = busyoList [j];
				busyoListCount = busyoListCount - 1;
				int mapId = mapList [j];

				//Status
				if (randomBusyoId != 0) {
					int busyoHp = sts.getHp (randomBusyoId, activeBusyoLv);
					busyoHp = busyoHp * 100;
					
					//Link Status Adjustment
					if (linkNo != 0) {
						float linkAdjst = (float)linkNo/10;
						float adjstHp = busyoHp * linkAdjst;
						busyoHp = busyoHp + (int)adjstHp;
					}
					
					string busyoName = info.getName (randomBusyoId,langId);
					string busyoType = info.getHeisyu (randomBusyoId);

					int chldHp2 = activeButaiQty * enemyIns.getChildStatus (activeButaiLv, busyoType, linkNo);
					totalHei = totalHei + busyoHp + chldHp2;

					string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
					GameObject chdBusyo = Instantiate(Resources.Load (busyoPath)) as GameObject;
					chdBusyo.name = randomBusyoId.ToString ();

					string chdSlotName = "Slot" + mapId;
					chdBusyo.transform.SetParent(EnemyJinkeiView.transform.Find(chdSlotName).transform);
					chdBusyo.name = randomBusyoId.ToString();
					chdBusyo.transform.localScale = new Vector2(-3,3);
					chdBusyo.transform.localPosition = new Vector3(0, 0, 0);

					//Button
					chdBusyo.GetComponent<DragHandler> ().enabled = false;
					
					//Text
					GameObject chTxtObj = chdBusyo.transform.Find ("Text").gameObject;
					Vector2 chTxtScale = chTxtObj.transform.localScale;
					chTxtScale.x *= -1;
					chTxtObj.transform.localScale = chTxtScale;
					Vector2 chTxtPos = chTxtObj.transform.localPosition;
					chTxtPos.x *= -1;
					chTxtObj.transform.localPosition = chTxtPos;

					GameObject chRblObj = chdBusyo.transform.Find ("Rank").gameObject;
					Vector2 chRblScale = chRblObj.transform.localScale;
					chRblScale.x *= -1;
					chRblObj.transform.localScale = chRblScale;
					Vector2 chRblPos = chRblObj.transform.localPosition;
					chRblPos.x *= -1;
					chRblObj.transform.localPosition = chRblPos;
				
					//Roujyo
					if (roujyoFlg && !isAttackedFlg) {
						if (torideQty != 0) {
							int baseAtk = sts.getBaseAtk (randomBusyoId);
							int baseDfc = sts.getBaseDfc (randomBusyoId);

							if (checkRoujyo(baseAtk, baseDfc)) {
								torideQty = torideQty - 1;
								makeRoujyoObj (false, EnemyJinkeiView.transform.Find (chdSlotName).gameObject, chdBusyo, 1);
							}
						}
					}
				}
			}else{
				//samurai daisyo make
				busyoListCount = busyoListCount - 1;
				int mapId = mapList [j];
				
				int busyoHp = sts.getHp (35, activeBusyoLv);
				busyoHp = busyoHp * 100;
				
				//Link Status Adjustment
				if (linkNo != 0) {
					float linkAdjst = (float)linkNo/10;
					float adjstHp = busyoHp * linkAdjst;
					busyoHp = busyoHp + (int)adjstHp;
				}
				
				string busyoName = info.getName (35,langId);
				string[] texts = new string[] { "YR", "KB"};
				string busyoType = texts[UnityEngine.Random.Range(0, texts.Length - 1)];

				int chldHp2 = activeButaiQty * enemyIns.getChildStatus (activeButaiLv, busyoType, linkNo);
				totalHei = totalHei + busyoHp + chldHp2;

				string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
				GameObject chdBusyo = Instantiate(Resources.Load (busyoPath)) as GameObject;
				chdBusyo.name = "35";

				string chdSlotName = "Slot" + mapId;
				chdBusyo.transform.SetParent(EnemyJinkeiView.transform.Find(chdSlotName).transform);
				chdBusyo.transform.localScale = new Vector2(-3,3);
				chdBusyo.transform.localPosition = new Vector3(0, 0, 0);

				//Button
				chdBusyo.GetComponent<DragHandler> ().enabled = false;
				
				//Rabel & Text
				GameObject chTxtObj = chdBusyo.transform.Find ("Text").gameObject;
				Vector2 chTxtScale = chTxtObj.transform.localScale;
				chTxtScale.x *= -1;
				chTxtObj.transform.localScale = chTxtScale;
				Vector2 chTxtPos = chTxtObj.transform.localPosition;
				chTxtPos.x *= -1;
				chTxtObj.transform.localPosition = chTxtPos;

                GameObject chRblObj = chdBusyo.transform.Find("Rank").gameObject;
                Vector2 chRblScale = chRblObj.transform.localScale;
                chRblScale.x *= -1;
                chRblObj.transform.localScale = chRblScale;
                Vector2 chRblPos = chRblObj.transform.localPosition;
                chRblPos.x *= -1;
                chRblObj.transform.localPosition = chRblPos;

                //Roujyo
                if (roujyoFlg && !isAttackedFlg) {
					if (torideQty != 0) {
						int baseAtk = sts.getBaseAtk (35);
						int baseDfc = sts.getBaseDfc (35);

						if (checkRoujyo(baseAtk, baseDfc)) {
							torideQty = torideQty - 1;
							makeRoujyoObj (false, EnemyJinkeiView.transform.Find (chdSlotName).gameObject, chdBusyo, 1);
						}
					}
				}
			}

		}

		return totalHei;
	}



	//PowerType3
	//Daimyo + Busyo
	public int powerType3(List<int> mapList, int taisyoMapId, int linkNo,  int activeDaimyoId, bool strongFlg){
		int totalHei = 0;
        int langId = PlayerPrefs.GetInt("langId");
        int activeBusyoQty = PlayerPrefs.GetInt ("activeBusyoQty");
		int activeBusyoLv = PlayerPrefs.GetInt ("activeBusyoLv");
		int activeButaiQty = PlayerPrefs.GetInt ("activeButaiQty");
		int activeButaiLv = PlayerPrefs.GetInt ("activeButaiLv");
		
		Entity_daimyo_mst daimyoMst = Resources.Load ("Data/daimyo_mst") as Entity_daimyo_mst;
		int daimyoBusyoId = daimyoMst.param[activeDaimyoId-1].busyoId;
		
		/*Taisyo Setting Start*/
		GameObject EnemyJinkeiView = GameObject.Find ("EnemyJinkeiView").gameObject;
		StatusGet sts = new StatusGet();
		BusyoInfoGet info = new BusyoInfoGet();
		
		int hp = sts.getHp(daimyoBusyoId,activeBusyoLv);
		hp = hp * 100;

		//Roujyo Check
		if (!isAttackedFlg) {
			int atk = sts.getBaseAtk (daimyoBusyoId);
			int dfc = sts.getBaseDfc (daimyoBusyoId);
			roujyoFlg = checkRoujyo (atk, dfc);
		}

		//Link Status Adjustment
		if (linkNo != 0) {
			float linkAdjst = (float)linkNo/10;
			float adjstHp = hp * linkAdjst;
			hp = hp + (int)adjstHp;
		}

		string daimyoBusyoName = info.getName(daimyoBusyoId,langId);
		string daimyoType = info.getHeisyu(daimyoBusyoId);

		int chldHp = activeButaiQty * enemyIns.getChildStatus (activeButaiLv, daimyoType, linkNo);
		totalHei = hp + chldHp;

		string path = "Prefabs/Player/Unit/BusyoUnit";
		GameObject tsyBusyo = Instantiate(Resources.Load (path)) as GameObject;
		tsyBusyo.name = daimyoBusyoId.ToString ();

		string slotName = "Slot" + taisyoMapId;
		tsyBusyo.transform.SetParent(EnemyJinkeiView.transform.Find(slotName).transform);
		tsyBusyo.name = daimyoBusyoId.ToString();
		tsyBusyo.transform.localScale = new Vector2(-3,3);
		tsyBusyo.transform.localPosition = new Vector3(0, 0, 0);
		tempEnemySoudaisyo = int.Parse (tsyBusyo.name);

		//Roujyo Object Make
		if (roujyoFlg && !isAttackedFlg) {
			//Make Shiro on Soudaisyo
			makeRoujyoObj(true,EnemyJinkeiView.transform.Find(slotName).gameObject,tsyBusyo,3);

		}

		//Button
		string soudaisyoPath = "Prefabs/Jinkei/soudaisyo";
		GameObject soudaisyo = Instantiate (Resources.Load (soudaisyoPath)) as GameObject;
		soudaisyo.transform.SetParent (tsyBusyo.transform);
		soudaisyo.transform.localScale = new Vector2 (27, 12);
		soudaisyo.name = "enemySoudaisyo";
		soudaisyo.transform.localPosition = new Vector3(0, 11, 0);
		tsyBusyo.GetComponent<DragHandler> ().enabled = false;
		
		//Text
		GameObject txtObj = tsyBusyo.transform.Find ("Text").gameObject;
		Vector2 txtScale = txtObj.transform.localScale;
		txtScale.x *= -1;
		txtObj.transform.localScale = txtScale;
		Vector2 txtPos = txtObj.transform.localPosition;
		txtPos.x *= -1;
		txtObj.transform.localPosition = txtPos;

		GameObject rblObj = tsyBusyo.transform.Find ("Rank").gameObject;
		Vector2 rblScale = rblObj.transform.localScale;
		rblScale.x *= -1;
		rblObj.transform.localScale = rblScale;
		Vector2 rblPos = rblObj.transform.localPosition;
		rblPos.x *= -1;
		rblObj.transform.localPosition = rblPos;

		GameObject tsyTxtObj = tsyBusyo.transform.Find ("enemySoudaisyo").transform.Find("Text").gameObject;
		Vector2 tsyScale = tsyTxtObj.transform.localScale;
		tsyScale.x *= -1;
		tsyTxtObj.transform.localScale = tsyScale;
		Vector2 tsyPos = tsyTxtObj.transform.localPosition;
		tsyPos.x *= -1;
		tsyTxtObj.transform.localPosition = tsyPos;
		/*Taisyo Setting End*/
		
		
		/*Busyo Setting Start*/
		if (activeBusyoQty > 1) {
			//Active Busyo List
			List<string> metsubouDaimyoList = new List<string> ();
			string metsubouTemp = "metsubou" + activeDaimyoId;
			string metsubouDaimyoString = PlayerPrefs.GetString (metsubouTemp);
			char[] delimiterChars2 = {','};
			if (metsubouDaimyoString != null && metsubouDaimyoString != "") {
				if (metsubouDaimyoString.Contains (",")) {
					metsubouDaimyoList = new List<string> (metsubouDaimyoString.Split (delimiterChars2));
				} else {
					metsubouDaimyoList = new List<string> (metsubouDaimyoString.Split (delimiterChars2));
				}
			}
			metsubouDaimyoList.Add (activeDaimyoId.ToString ());
			
			Entity_busyo_mst busyoMst = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;
			List<int> busyoListTmp = new List<int> ();
            List<int> busyoList = new List<int>();
            for (int i=0; i<busyoMst.param.Count; i++) {
				int busyoId = busyoMst.param [i].id;
				int daimyoId = busyoMst.param [i].daimyoId;
				
				if (metsubouDaimyoList.Contains (daimyoId.ToString ())) {
					if (busyoId != daimyoBusyoId) {
						busyoList.Add (busyoId);
					}
				}
			}
          
            if (strongFlg) {
                BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();
                List<int> SrankList = new List<int>();
                List<int> ArankList = new List<int>();
                List<int> BrankList = new List<int>();
                List<int> CrankList = new List<int>();

                for (int i=0; i<busyoList.Count; i ++) {
                    //Strong Sort S > C rank
                    int temp = busyoList[i];
                    string rank = BusyoInfoGet.getRank(temp);
                    if (rank == "S") {
                        SrankList.Add(temp);
                    }else if (rank == "A") {
                        ArankList.Add(temp);
                    }else if (rank == "B") {
                        BrankList.Add(temp);
                    }else if (rank == "C") {
                        CrankList.Add(temp);
                    }                                      
                }
                busyoList = new List<int>();
                busyoList.AddRange(SrankList);
                busyoList.AddRange(ArankList);
                busyoList.AddRange(BrankList);
                busyoList.AddRange(CrankList);
            }else { 
                for (int i = 0; i < busyoList.Count; i++) {                
                    //Random Shuffle
                    int temp = busyoList [i];
				    int randomIndex = UnityEngine.Random.Range (0, busyoList.Count);
				    busyoList [i] = busyoList [randomIndex];
				    busyoList [randomIndex] = temp;                
                }
            }

            for (int i = 0; i < mapList.Count; i++) {
				int temp = mapList [i];
				int randomIndex = UnityEngine.Random.Range (0, mapList.Count);
				mapList [i] = mapList [randomIndex];
				mapList [randomIndex] = temp;
			}
			
			
			//Make Instance
			int busyoListCount = busyoList.Count;
			int torideQty = linkNo;
			for (int j=0; j<activeBusyoQty-1; j++) {
				
				if (busyoListCount > 0) {
					int randomBusyoId = busyoList [j];
					busyoListCount = busyoListCount - 1;
					int mapId = mapList [j];
					
					
					//Status
					if (randomBusyoId != 0) {
						int busyoHp = sts.getHp (randomBusyoId, activeBusyoLv);
						busyoHp = busyoHp * 100;

						if (linkNo != 0) {
							float linkAdjst = (float)linkNo/10;
							float adjstHp = busyoHp * linkAdjst;
							busyoHp = busyoHp + (int)adjstHp;
						}

						string busyoName = info.getName (randomBusyoId,langId);
						string busyoType = info.getHeisyu (randomBusyoId);

						int chldHp2 = activeButaiQty * enemyIns.getChildStatus (activeButaiLv, busyoType, linkNo);
						totalHei = totalHei + busyoHp + chldHp2;

						string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
						GameObject chdBusyo = Instantiate(Resources.Load (busyoPath)) as GameObject;
						chdBusyo.name = randomBusyoId.ToString ();

						string chdSlotName = "Slot" + mapId;
						chdBusyo.transform.SetParent(EnemyJinkeiView.transform.Find(chdSlotName).transform);
						chdBusyo.name = randomBusyoId.ToString();
						chdBusyo.transform.localScale = new Vector2(-3,3);
						chdBusyo.transform.localPosition = new Vector3(0, 0, 0);
						
						//Button
						chdBusyo.GetComponent<DragHandler> ().enabled = false;
						
						//Text
						GameObject chTxtObj = chdBusyo.transform.Find ("Text").gameObject;
						Vector2 chTxtScale = chTxtObj.transform.localScale;
						chTxtScale.x *= -1;
						chTxtObj.transform.localScale = chTxtScale;
						Vector2 chTxtPos = chTxtObj.transform.localPosition;
						chTxtPos.x *= -1;
						chTxtObj.transform.localPosition = chTxtPos;
						
						GameObject chRblObj = chdBusyo.transform.Find ("Rank").gameObject;
						Vector2 chRblScale = chRblObj.transform.localScale;
						chRblScale.x *= -1;
						chRblObj.transform.localScale = chRblScale;
						Vector2 chRblPos = chRblObj.transform.localPosition;
						chRblPos.x *= -1;
						chRblObj.transform.localPosition = chRblPos;

						//Roujyo
						if (roujyoFlg && !isAttackedFlg) {
							if (torideQty != 0) {
								int baseAtk = sts.getBaseAtk (randomBusyoId);
								int baseDfc = sts.getBaseDfc (randomBusyoId);

								if (baseAtk < baseDfc * 2) {
									torideQty = torideQty - 1;
									makeRoujyoObj (false, EnemyJinkeiView.transform.Find (chdSlotName).gameObject, chdBusyo, 1);
								}
							}
						}
					}
				}else{
					//samurai daisyo make
					busyoListCount = busyoListCount - 1;
					int mapId = mapList [j];
					
					int busyoHp = sts.getHp (35, activeBusyoLv);
					busyoHp = busyoHp * 100;

					if (linkNo != 0) {
						float linkAdjst = (float)linkNo/10;
						float adjstHp = busyoHp * linkAdjst;
						busyoHp = busyoHp + (int)adjstHp;
					}

					string busyoName = info.getName (35,langId);
					string[] texts = new string[] { "YR", "KB"};
					string busyoType = texts[UnityEngine.Random.Range(0, texts.Length - 1)];

					int chldHp2 = activeButaiQty * enemyIns.getChildStatus (activeButaiLv, busyoType, linkNo);
					totalHei = totalHei + busyoHp + chldHp2;

					string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
					GameObject chdBusyo = Instantiate(Resources.Load (busyoPath)) as GameObject;
					chdBusyo.name = "35";

					string chdSlotName = "Slot" + mapId;
					chdBusyo.transform.SetParent(EnemyJinkeiView.transform.Find(chdSlotName).transform);
					chdBusyo.transform.localScale = new Vector2(-3,3);
					chdBusyo.transform.localPosition = new Vector3(0, 0, 0);
					
					//Button
					chdBusyo.GetComponent<DragHandler> ().enabled = false;
					
					//Rabel & Text
					GameObject chTxtObj = chdBusyo.transform.Find ("Text").gameObject;
					Vector2 chTxtScale = chTxtObj.transform.localScale;
					chTxtScale.x *= -1;
					chTxtObj.transform.localScale = chTxtScale;
					Vector2 chTxtPos = chTxtObj.transform.localPosition;
					chTxtPos.x *= -1;
					chTxtObj.transform.localPosition = chTxtPos;

                    GameObject chRblObj = chdBusyo.transform.Find("Rank").gameObject;
                    Vector2 chRblScale = chRblObj.transform.localScale;
                    chRblScale.x *= -1;
                    chRblObj.transform.localScale = chRblScale;
                    Vector2 chRblPos = chRblObj.transform.localPosition;
                    chRblPos.x *= -1;
                    chRblObj.transform.localPosition = chRblPos;

                    //Roujyo
                    if (roujyoFlg && !isAttackedFlg) {
						if (torideQty != 0) {
							int baseAtk = sts.getBaseAtk (35);
							int baseDfc = sts.getBaseDfc (35);

							if (checkRoujyo(baseAtk, baseDfc)) {
								torideQty = torideQty - 1;
								makeRoujyoObj (false, EnemyJinkeiView.transform.Find (chdSlotName).gameObject, chdBusyo, 1);
							}
						}
					}
				}
			}
		}
		return totalHei;
	}



	public int getWeatherId(){

		//get season
		string yearSeason = PlayerPrefs.GetString ("yearSeason");	
		char[] delimiterChars = {','};
		string[] yearSeasonList = yearSeason.Split (delimiterChars);
		int nowSeason = int.Parse (yearSeasonList [1]);

		int weatherId = 0; //1:normal, 2:rain, 3:snow

		if (nowSeason == 1 || nowSeason == 3) {
			//Spring & Fall
			float percent = UnityEngine.Random.value;
			percent = percent * 100;

			if (percent <= 70) {
				weatherId = 1;
			} else if (70 < percent && percent <= 90) {
				weatherId = 2;
			} else if (90 < percent && percent <= 100) {
				weatherId = 3;
			}
		} else if (nowSeason == 2) {
			//Summer
			float percent = UnityEngine.Random.value;
			percent = percent * 100;

			if (percent <= 60) {
				weatherId = 1;
			} else if (60 < percent && percent <= 100) {
				weatherId = 2;
			}

		} else if (nowSeason == 4) {
			//Winter

			//Check snow area or not
			int activeKuniId  = PlayerPrefs.GetInt("activeKuniId");
			KuniInfo kuni = new KuniInfo ();
			bool isSnowFlg = kuni.getKuniIsSnowFlg(activeKuniId);

			if (isSnowFlg) {
				weatherId = 3;
			} else {
				float percent = UnityEngine.Random.value;
				percent = percent * 100;

				if (percent <= 50) {
					weatherId = 1;
				} else if (50 < percent && percent <= 70) {
					weatherId = 2;
				} else if (70 < percent && percent <= 100) {
					weatherId = 3;
				}
			}
		}


		//Set Object by wether Id
		if (weatherId == 2) {
			//rain
			string path = "Prefabs/PreKassen/particle/PreRain";
			GameObject particle = Instantiate (Resources.Load (path)) as GameObject;
            particle.transform.SetParent(GameObject.Find("Panel").transform);
            particle.transform.localScale = new Vector3(1,1,1);
            particle.transform.localPosition = new Vector3(0, 360, 0);

        }
        else if(weatherId == 3){
			//snow
			string path = "Prefabs/PreKassen/particle/PreSnow";
			GameObject particle = Instantiate (Resources.Load (path)) as GameObject;
            particle.transform.SetParent(GameObject.Find("Panel").transform);
            particle.transform.localScale = new Vector3(1, 1, 1);
            particle.transform.localPosition = new Vector3(0, 360, 0);

        }

        return weatherId;
	}


	public bool checkRoujyo(int atk, int dfc){
		
		bool roujyoFlg = false;

		float yasenRatio = (float)atk/(atk + dfc) * 100;
		float percent = Random.value;
		percent = percent * 100;
		if (percent > yasenRatio) {
			roujyoFlg = true;
		}

		return roujyoFlg;
	}

	public void makeRoujyoObj(bool shiroFlg, GameObject slot, GameObject busyo, int powerType) {

		string Type = "";
		if (powerType == 1) {
			Type = "s";
		} else if (powerType == 2) {
			Type = "m";
		} else if (powerType == 3) {
			Type = "l";
		}

		if (shiroFlg) {
			//Shiro

			string shiroPath = "Prefabs/PreKassen/shiroIcon";
			GameObject shiroObj = Instantiate(Resources.Load (shiroPath)) as GameObject;
			shiroObj.transform.SetParent(slot.transform);
			shiroObj.transform.localScale = new Vector2(4,3);	
			shiroObj.transform.localPosition = new Vector3(0,0,0);
            string imagePath = "Prefabs/Naisei/Shiro/Sprite/shiro_" + Type;
            shiroObj.GetComponent<Image> ().sprite = 
			Resources.Load (imagePath, typeof(Sprite)) as Sprite;

			shiroObj.name = "shiro";
			busyo.SetActive (false);

		} else {
			//Toride
			string toridePath = "Prefabs/PreKassen/torideIcon";
			GameObject torideObj = Instantiate(Resources.Load (toridePath)) as GameObject;
			torideObj.transform.SetParent(slot.transform);
			torideObj.transform.localScale = new Vector2(4.0f,1.8f);
			torideObj.transform.localPosition = new Vector3(0,0,0);
			string imagePath = "Prefabs/Kassen/kassenTrd_" + Type;
			torideObj.GetComponent<Image> ().sprite = 
				Resources.Load (imagePath, typeof(Sprite)) as Sprite;
			torideObj.name = "toride";
			busyo.SetActive (false);


		}

	}

	public void makeMyRoujyoObj(GameObject slot,int shiroLv, int torideLv, GameObject startBtn){
		
        
		if (slot.name == "Slot12") {
            //Shiro
            if (!slot.transform.Find("shiro")) {
                string Type = "";
			    if (shiroLv < 8) {
				    Type = "s";
			    } else if (shiroLv < 15) {
				    Type = "m";
			    } else if (15<=shiroLv) {
				    Type = "l";
			    }

			    string shiroPath = "Prefabs/PreKassen/shiroIcon";
			    GameObject shiroObj = Instantiate(Resources.Load (shiroPath)) as GameObject;
			    shiroObj.transform.SetParent(slot.transform);
			    shiroObj.transform.localScale = new Vector2(4,3);	
			    shiroObj.transform.localPosition = new Vector3(0,0,0);

                string shiroTmp = "shiro" + activeKuniId;
                if (PlayerPrefs.HasKey(shiroTmp)) {
                    int shiroId = PlayerPrefs.GetInt(shiroTmp);
                    if (shiroId != 0) {
                        string imagePath = "Prefabs/Naisei/Shiro/Sprite/" + shiroId;
                        shiroObj.GetComponent<Image>().sprite =
                                        Resources.Load(imagePath, typeof(Sprite)) as Sprite;
                    }
                }else {
                    string imagePath = "Prefabs/Naisei/Shiro/Sprite/shiro_" + Type;
                    shiroObj.GetComponent<Image> ().sprite = 
				    Resources.Load (imagePath, typeof(Sprite)) as Sprite;
                }
			    shiroObj.name = "shiro";

			    startBtn.GetComponent<startKassen2> ().myShiroLv = shiroLv;
            }

		} else {
			//Toride

            if(!slot.transform.Find("shiro")) {
                string Type = "";
			    if (torideLv < 8) {
				    Type = "s";
			    } else if (torideLv < 15) {
				    Type = "m";
			    } else if (15<=torideLv) {
				    Type = "l";
			    }

			    string toridePath = "Prefabs/PreKassen/torideIcon";
			    GameObject torideObj = Instantiate(Resources.Load (toridePath)) as GameObject;
			    torideObj.transform.SetParent(slot.transform);
			    torideObj.transform.localScale = new Vector2(4.0f,1.8f);
			    torideObj.transform.localPosition = new Vector3(0,0,0);
			    string imagePath = "Prefabs/Kassen/kassenTrd_" + Type;
			    torideObj.GetComponent<Image> ().sprite = 
				    Resources.Load (imagePath, typeof(Sprite)) as Sprite;
			    torideObj.name = "toride";

			    startBtn.GetComponent<startKassen2> ().myTorideLvList.Add(torideLv);
			    string slotNo = slot.name.Substring (4);
			    startBtn.GetComponent<startKassen2> ().myTorideSlotNoList.Add (slotNo);
            }

		}





	}

    public void prekassenPlayerJinkei(int jinkeiId, int weatherId, bool isAttackedFlg, bool onlyPlayerFlg, bool strongFlg, int senarioId) {

        //reset disabled slot
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
            obs.GetComponent<Image>().enabled = true;
            if (obs.transform.IsChildOf(obs.transform)) {

                foreach (Transform n in obs.transform) {
                    GameObject.Destroy(n.gameObject);
                }
            }
        }

        jinkei = jinkeiId;
        jinkeiBusyo_list = new List<string>();

        startKassen2 startScript = GameObject.Find("StartBtn").GetComponent<startKassen2>();
        startScript.myJinkei = jinkei;
        startScript.weatherId = weatherId;
        startScript.isAttackedFlg = isAttackedFlg;

        //*************My Roujyou Preparation Start*************
        if (isAttackedFlg) {
            GameObject PlayerShiroTorideView = GameObject.Find("PlayerShiroTorideView").gameObject;

            //Delete Previous Toride
            foreach(Transform trd in PlayerShiroTorideView.transform) {
                if (trd.transform.IsChildOf(trd.transform)) {
                    foreach (Transform n in trd.transform) {
                        if (n.name != "shiro") {
                            GameObject.Destroy(n.gameObject);
                        }
                    }
                }
            }



            //Random Slot of Toride
            List<string> targetTorideList = new List<string>();
            if (jinkei == 1) {
                List<string> tmp = new List<string>() { "Slot1", "Slot2", "Slot7", "Slot8", "Slot11", "Slot13", "Slot14", "Slot17", "Slot18", "Slot21", "Slot22" };
                targetTorideList.AddRange(tmp);
            }
            else if (jinkei == 2) {
                List<string> tmp = new List<string>() { "Slot3", "Slot4", "Slot5", "Slot7", "Slot8", "Slot11", "Slot17", "Slot18", "Slot23", "Slot24", "Slot25" };
                targetTorideList.AddRange(tmp);
            }
            else if (jinkei == 3) {
                List<string> tmp = new List<string>() { "Slot3", "Slot7", "Slot8", "Slot9", "Slot11", "Slot14", "Slot15", "Slot16", "Slot20", "Slot21", "Slot25" };
                targetTorideList.AddRange(tmp);
            }
            else if (jinkei == 4) {
                List<string> tmp = new List<string>() { "Slot1", "Slot2", "Slot7", "Slot8", "Slot13", "Slot14", "Slot18", "Slot19", "Slot20", "Slot24", "Slot25" };
                targetTorideList.AddRange(tmp);
            }

            int activeKuniId = PlayerPrefs.GetInt("activeKuniId");
            string temp = "naisei" + activeKuniId.ToString();
            GameObject StartBtn = GameObject.Find("StartBtn").gameObject;
            if (PlayerPrefs.HasKey(temp)) {
                //Defalt=> 1,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0
                string naiseiString = PlayerPrefs.GetString(temp);
                List<string> naiseiList = new List<string>();
                char[] delimiterChars = { ',' };
                naiseiList = new List<string>(naiseiString.Split(delimiterChars));

                //shiro
                int shiroLv = int.Parse(naiseiList[0]);
                makeMyRoujyoObj(PlayerShiroTorideView.transform.Find("Slot12").gameObject, shiroLv, 0, StartBtn);

                //toride
                char[] delimiterChars2 = { ':' };
                for (int i = 1; i < naiseiList.Count; i++) {
                    string naiseiSlot = naiseiList[i];
                    List<string> naiseiSlotList = new List<string>();
                    naiseiSlotList = new List<string>(naiseiSlot.Split(delimiterChars2));
                    if (naiseiSlotList[0] == "12") {

                        //Location
                        int rdmId = UnityEngine.Random.Range(0, targetTorideList.Count);
                        string torideSlotName = targetTorideList[rdmId];
                        targetTorideList.RemoveAt(rdmId);

                        int torideLv = int.Parse(naiseiSlotList[1]);

                        makeMyRoujyoObj(PlayerShiroTorideView.transform.Find(torideSlotName).gameObject, 0, torideLv, StartBtn);

                        if(targetTorideList.Count == 0) {
                            break;
                        }

                    }
                }
            }
        }

        //*************My Roujyou Preparation End*************




        BusyoInfoGet busyoScript = new BusyoInfoGet();
        if (jinkei == 1) {
            int soudaisyo = PlayerPrefs.GetInt("soudaisyo1");


            //Clear Previous Unit
            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
                //Enable 1,2,7,8,11,12,13,14,17,18,21,22
                if (obs.name == "Slot1" || obs.name == "Slot2" || obs.name == "Slot7" || obs.name == "Slot8" ||
                    obs.name == "Slot11" || obs.name == "Slot12" || obs.name == "Slot13" || obs.name == "Slot14" ||
                    obs.name == "Slot17" || obs.name == "Slot18" || obs.name == "Slot21" || obs.name == "Slot22") {

                    obs.GetComponent<Image>().enabled = true;
                    string mapId = "1map" + obs.name.Substring(4);
                    if (PlayerPrefs.HasKey(mapId)) {
                        int busyoId = PlayerPrefs.GetInt(mapId);
                        jinkeiBusyo_list.Add(busyoId.ToString());

                        //Instantiate
                        string path = "Prefabs/Player/Unit/BusyoUnit";
                        GameObject chldBusyo = Instantiate(Resources.Load(path)) as GameObject;
                        chldBusyo.name = busyoId.ToString();
                        chldBusyo.transform.SetParent(obs.transform);
                        chldBusyo.name = busyoId.ToString();
                        chldBusyo.transform.localScale = new Vector2(3, 3);
                        chldBusyo.AddComponent<Senryoku>().GetPlayerSenryoku(chldBusyo.name);
                        chldBusyo.transform.localPosition = new Vector3(0, 0, 0);

                        //Button
                        chldBusyo.AddComponent<Button>();
                        chldBusyo.AddComponent<Soudaisyo>();
                        chldBusyo.GetComponent<Button>().onClick.AddListener(chldBusyo.GetComponent<Soudaisyo>().OnClick);

                        //soudaisyo
                        if (soudaisyo == int.Parse(chldBusyo.name)) {
                            chldBusyo.GetComponent<Soudaisyo>().OnClick();
                        }

                        //Add Kamon
                        string KamonPath = "Prefabs/Jinkei/Kamon";
                        GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
                        kamon.transform.SetParent(chldBusyo.transform);
                        kamon.transform.localScale = new Vector2(0.1f, 0.1f);
                        kamon.transform.localPosition = new Vector2(-15, -12);
                        int daimyoId = busyoScript.getDaimyoId(int.Parse(chldBusyo.name));
                        if (daimyoId == 0){
                            daimyoId = busyoScript.getDaimyoHst(int.Parse(chldBusyo.name));
                        }
                        string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
                        kamon.GetComponent<Image>().sprite =
                            Resources.Load(imagePath, typeof(Sprite)) as Sprite;

                        //Add Heisyu
                        string heisyu = busyoScript.getHeisyu(int.Parse(chldBusyo.name));
                        string heisyuPath = "Prefabs/Jinkei/" + heisyu;
                        GameObject heisyuObj = Instantiate(Resources.Load(heisyuPath)) as GameObject;
                        heisyuObj.transform.SetParent(chldBusyo.transform, false);
                        heisyuObj.transform.localPosition = new Vector2(10, -10);
                        heisyuObj.transform.SetAsFirstSibling();

                    }

                    //Disable 3,4,5,6,9,15,16,19,20,23,24,25
                }
                else {
                    obs.GetComponent<Image>().enabled = false;

                    if (obs.transform.IsChildOf(obs.transform)) {

                        foreach (Transform n in obs.transform) {
                            GameObject.Destroy(n.gameObject);
                        }

                    }
                }
            }


            //鶴翼
        }
        else if (jinkei == 2) {
            int soudaisyo = PlayerPrefs.GetInt("soudaisyo2");

            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
                //Enable 3,4,5,7,8,11,12,17,18,23,24,25
                if (obs.name == "Slot3" || obs.name == "Slot4" || obs.name == "Slot5" || obs.name == "Slot7" ||
                   obs.name == "Slot8" || obs.name == "Slot11" || obs.name == "Slot12" || obs.name == "Slot17" ||
                   obs.name == "Slot18" || obs.name == "Slot23" || obs.name == "Slot24" || obs.name == "Slot25") {


                    obs.GetComponent<Image>().enabled = true;
                    string mapId = "2map" + obs.name.Substring(4);
                    if (PlayerPrefs.HasKey(mapId)) {
                        int busyoId = PlayerPrefs.GetInt(mapId);
                        jinkeiBusyo_list.Add(busyoId.ToString());

                        //Instantiate
                        string path = "Prefabs/Player/Unit/BusyoUnit";
                        GameObject chldBusyo = Instantiate(Resources.Load(path)) as GameObject;
                        chldBusyo.name = busyoId.ToString();
                        chldBusyo.transform.parent = obs.transform;
                        chldBusyo.name = busyoId.ToString();
                        chldBusyo.transform.localScale = new Vector2(3, 3);
                        chldBusyo.AddComponent<Senryoku>().GetPlayerSenryoku(chldBusyo.name);
                        chldBusyo.transform.localPosition = new Vector3(0, 0, 0);

                        //Button
                        chldBusyo.AddComponent<Button>();
                        chldBusyo.AddComponent<Soudaisyo>();
                        chldBusyo.GetComponent<Button>().onClick.AddListener(chldBusyo.GetComponent<Soudaisyo>().OnClick);

                        //soudaisyo
                        if (soudaisyo == int.Parse(chldBusyo.name)) {
                            chldBusyo.GetComponent<Soudaisyo>().OnClick();
                        }

                        //Add Kamon
                        string KamonPath = "Prefabs/Jinkei/Kamon";
                        GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
                        kamon.transform.SetParent(chldBusyo.transform);
                        kamon.transform.localScale = new Vector2(0.1f, 0.1f);
                        kamon.transform.localPosition = new Vector2(-15, -12);
                        int daimyoId = busyoScript.getDaimyoId(int.Parse(chldBusyo.name));
                        if (daimyoId == 0)
                        {
                            daimyoId = busyoScript.getDaimyoHst(int.Parse(chldBusyo.name));
                        }
                        string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
                        kamon.GetComponent<Image>().sprite =
                            Resources.Load(imagePath, typeof(Sprite)) as Sprite;

                        //Add Heisyu
                        string heisyu = busyoScript.getHeisyu(int.Parse(chldBusyo.name));
                        string heisyuPath = "Prefabs/Jinkei/" + heisyu;
                        GameObject heisyuObj = Instantiate(Resources.Load(heisyuPath)) as GameObject;
                        heisyuObj.transform.SetParent(chldBusyo.transform, false);
                        heisyuObj.transform.localPosition = new Vector2(10, -10);
                        heisyuObj.transform.SetAsFirstSibling();
                    }

                    //Disable 1,2,6,9,10,13,14,15,16,19,20,21
                }
                else {
                    obs.GetComponent<Image>().enabled = false;

                    if (obs.transform.IsChildOf(obs.transform)) {

                        foreach (Transform n in obs.transform) {
                            GameObject.Destroy(n.gameObject);
                        }
                    }
                }
            }

        }
        else if (jinkei == 3) {
            int soudaisyo = PlayerPrefs.GetInt("soudaisyo3");

            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
                //Enable 3,7,8,9,11,12,14,15,16,20,21,25
                if (obs.name == "Slot3" || obs.name == "Slot7" || obs.name == "Slot8" || obs.name == "Slot9" ||
                   obs.name == "Slot11" || obs.name == "Slot12" || obs.name == "Slot14" || obs.name == "Slot15" ||
                   obs.name == "Slot16" || obs.name == "Slot20" || obs.name == "Slot21" || obs.name == "Slot25") {

                    obs.GetComponent<Image>().enabled = true;
                    string mapId = "3map" + obs.name.Substring(4);
                    if (PlayerPrefs.HasKey(mapId)) {
                        int busyoId = PlayerPrefs.GetInt(mapId);
                        jinkeiBusyo_list.Add(busyoId.ToString());

                        //Instantiate
                        string path = "Prefabs/Player/Unit/BusyoUnit";
                        GameObject chldBusyo = Instantiate(Resources.Load(path)) as GameObject;
                        chldBusyo.name = busyoId.ToString();
                        chldBusyo.transform.parent = obs.transform;
                        chldBusyo.name = busyoId.ToString();
                        chldBusyo.transform.localScale = new Vector2(3, 3);
                        chldBusyo.AddComponent<Senryoku>().GetPlayerSenryoku(chldBusyo.name);
                        chldBusyo.transform.localPosition = new Vector3(0, 0, 0);

                        //Button
                        chldBusyo.AddComponent<Button>();
                        chldBusyo.AddComponent<Soudaisyo>();
                        chldBusyo.GetComponent<Button>().onClick.AddListener(chldBusyo.GetComponent<Soudaisyo>().OnClick);

                        //soudaisyo
                        if (soudaisyo == int.Parse(chldBusyo.name)) {
                            chldBusyo.GetComponent<Soudaisyo>().OnClick();
                        }

                        //Add Kamon
                        string KamonPath = "Prefabs/Jinkei/Kamon";
                        GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
                        kamon.transform.SetParent(chldBusyo.transform);
                        kamon.transform.localScale = new Vector2(0.1f, 0.1f);
                        kamon.transform.localPosition = new Vector2(-15, -12);
                        int daimyoId = busyoScript.getDaimyoId(int.Parse(chldBusyo.name));
                        if (daimyoId == 0)
                        {
                            daimyoId = busyoScript.getDaimyoHst(int.Parse(chldBusyo.name));
                        }
                        string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
                        kamon.GetComponent<Image>().sprite =
                            Resources.Load(imagePath, typeof(Sprite)) as Sprite;

                        //Add Heisyu
                        string heisyu = busyoScript.getHeisyu(int.Parse(chldBusyo.name));
                        string heisyuPath = "Prefabs/Jinkei/" + heisyu;
                        GameObject heisyuObj = Instantiate(Resources.Load(heisyuPath)) as GameObject;
                        heisyuObj.transform.SetParent(chldBusyo.transform, false);
                        heisyuObj.transform.localPosition = new Vector2(10, -10);
                        heisyuObj.transform.SetAsFirstSibling();
                    }

                    //Disable 1,2,4,5,6,10,13,17,18,19,22,23,24
                }
                else {
                    obs.GetComponent<Image>().enabled = false;
                    if (obs.transform.IsChildOf(obs.transform)) {

                        foreach (Transform n in obs.transform) {
                            GameObject.Destroy(n.gameObject);
                        }
                    }
                }
            }

        }
        else if (jinkei == 4) {
            int soudaisyo = PlayerPrefs.GetInt("soudaisyo4");

            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
                //Enable 1,2,7,8,12,13,14,18,19,20,24,25
                if (obs.name == "Slot1" || obs.name == "Slot2" || obs.name == "Slot7" || obs.name == "Slot8" ||
                   obs.name == "Slot12" || obs.name == "Slot13" || obs.name == "Slot14" || obs.name == "Slot18" ||
                   obs.name == "Slot19" || obs.name == "Slot20" || obs.name == "Slot24" || obs.name == "Slot25") {

                    obs.GetComponent<Image>().enabled = true;
                    string mapId = "4map" + obs.name.Substring(4);
                    if (PlayerPrefs.HasKey(mapId)) {
                        int busyoId = PlayerPrefs.GetInt(mapId);
                        jinkeiBusyo_list.Add(busyoId.ToString());

                        //Instantiate
                        string path = "Prefabs/Player/Unit/BusyoUnit";
                        GameObject chldBusyo = Instantiate(Resources.Load(path)) as GameObject;
                        chldBusyo.name = busyoId.ToString();
                        chldBusyo.transform.SetParent(obs.transform);
                        chldBusyo.name = busyoId.ToString();
                        chldBusyo.transform.localScale = new Vector2(3, 3);
                        chldBusyo.AddComponent<Senryoku>().GetPlayerSenryoku(chldBusyo.name);
                        chldBusyo.transform.localPosition = new Vector3(0, 0, 0);

                        //Button
                        chldBusyo.AddComponent<Button>();
                        chldBusyo.AddComponent<Soudaisyo>();
                        chldBusyo.GetComponent<Button>().onClick.AddListener(chldBusyo.GetComponent<Soudaisyo>().OnClick);

                        //soudaisyo
                        if (soudaisyo == int.Parse(chldBusyo.name)) {
                            chldBusyo.GetComponent<Soudaisyo>().OnClick();
                        }

                        //Add Kamon
                        string KamonPath = "Prefabs/Jinkei/Kamon";
                        GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
                        kamon.transform.SetParent(chldBusyo.transform);
                        kamon.transform.localScale = new Vector2(0.1f, 0.1f);
                        kamon.transform.localPosition = new Vector2(-15, -12);
                        int daimyoId = busyoScript.getDaimyoId(int.Parse(chldBusyo.name));
                        if (daimyoId == 0)
                        {
                            daimyoId = busyoScript.getDaimyoHst(int.Parse(chldBusyo.name));
                        }
                        string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
                        kamon.GetComponent<Image>().sprite =
                            Resources.Load(imagePath, typeof(Sprite)) as Sprite;

                        //Add Heisyu
                        string heisyu = busyoScript.getHeisyu(int.Parse(chldBusyo.name));
                        string heisyuPath = "Prefabs/Jinkei/" + heisyu;
                        GameObject heisyuObj = Instantiate(Resources.Load(heisyuPath)) as GameObject;
                        heisyuObj.transform.SetParent(chldBusyo.transform, false);
                        heisyuObj.transform.localPosition = new Vector2(10, -10);
                        heisyuObj.transform.SetAsFirstSibling();
                    }

                    //Disable 3,4,5,6,9,10,11,15,16,17,21,22,23
                }
                else {
                    obs.GetComponent<Image>().enabled = false;
                    if (obs.transform.IsChildOf(obs.transform)) {

                        foreach (Transform n in obs.transform) {
                            GameObject.Destroy(n.gameObject);
                        }
                    }
                }
            }
        }
        GameObject playerImage = GameObject.Find("PlayerJinkeiView").gameObject;

        //Player Senryoku
        int totalHp = 0;
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
            if (obs.transform.childCount > 0) {
                foreach (Transform busyo in obs.transform) {
                    totalHp = totalHp + busyo.GetComponent<Senryoku>().totalHp;
                }
            }
        }
        PlayerPrefs.SetInt("jinkeiHeiryoku", totalHp);
        Text playerHeiText = GameObject.Find("PlayerHei").transform.Find("HeiValue").GetComponent<Text>();
        playerHeiText.text = totalHp.ToString();

        JinkeiPowerEffection powerEffection = new JinkeiPowerEffection();
        powerEffection.UpdateSenryoku();

        GameObject.Find("BusyoScrollMenu").transform.Find("ScrollView").transform.Find("Content").GetComponent<PrepBusyoScrollMenu>().jinkeiBusyo_list = jinkeiBusyo_list;

        jinkeiLimit = PlayerPrefs.GetInt("jinkeiLimit");
        int addLimit = 0;

        //Purchased Item Check
        if (PlayerPrefs.GetBool("addJinkei1"))
        {
            addLimit = 1;
        }
        if (PlayerPrefs.GetBool("addJinkei2"))
        {
            addLimit = addLimit + 1;
        }
        if (PlayerPrefs.GetBool("addJinkei3"))
        {
            addLimit = addLimit + 1;
        }
        if (PlayerPrefs.GetBool("addJinkei4"))
        {
            addLimit = addLimit + 1;
        }
        jinkeiLimit = jinkeiLimit + addLimit;
        

        busyoCurrentQty = jinkeiBusyo_list.Count;

        if(!onlyPlayerFlg) {
            /*------------*/
            /*Enemy Jinkei*/
            /*------------*/
            int powerType = PlayerPrefs.GetInt("activePowerType", 0);
            int linkNo = PlayerPrefs.GetInt("activeLink", 0);

            List<int> jinkeiList = new List<int>() { 1, 2, 3, 4 };
            int enemyJinkei = UnityEngine.Random.Range(1, jinkeiList.Count + 1);
            startScript.enemyJinkei = enemyJinkei;

            //Back Kamon
            int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
            string playerImagePath = "Prefabs/Kamon/" + myDaimyo.ToString();
            playerImage.GetComponent<Image>().sprite =
                Resources.Load(playerImagePath, typeof(Sprite)) as Sprite;

            int activeDaimyoId = PlayerPrefs.GetInt("activeDaimyoId");
            string enemyImagePath = "Prefabs/Kamon/" + activeDaimyoId.ToString();
            GameObject enemyImage = GameObject.Find("EnemyJinkeiView").gameObject;
            enemyImage.GetComponent<Image>().sprite =
                Resources.Load(enemyImagePath, typeof(Sprite)) as Sprite;
            startScript.activeDaimyoId = activeDaimyoId;

            //Set Busyo Jinkei
            List<int> mapList = new List<int>();
            if (powerType == 1) {
                //busyo + mob
                mapList = enemyJinkeiMaker(enemyJinkei);
                enemyHei = powerType1(mapList, getTaisyoMapId(enemyJinkei), linkNo, activeDaimyoId, strongFlg);
            }
            else if (powerType == 2) {
                //busyo only
                mapList = enemyJinkeiMaker(enemyJinkei);
                enemyHei = powerType2(mapList, getTaisyoMapId(enemyJinkei), linkNo, activeDaimyoId, strongFlg);
            }
            else if (powerType == 3 || powerType == 0) {
                //daimyo + busyo
                mapList = enemyJinkeiMaker(enemyJinkei);
                enemyHei = powerType3(mapList, getTaisyoMapId(enemyJinkei), linkNo, activeDaimyoId, strongFlg);
            }
            JinkeiPowerEffection JinkeiPowerEffection = new JinkeiPowerEffection();
            JinkeiPowerEffection.EnemySameDaimyoNum(activeDaimyoId,senarioId);
            Text enemyHeiText = GameObject.Find("EnemyHei").transform.Find("HeiValue").GetComponent<Text>();
            enemyHeiText.text = enemyHei.ToString();
            startScript.enemySoudaisyo = tempEnemySoudaisyo;


            startScript.myHei = totalHp;
            startScript.enemyHei = enemyHei;
            startScript.roujyouFlg = roujyoFlg;
        }
    }

    public void changeFormButtonColor(int jinkeiId) {
        Color pushedTabColor = new Color(118f / 255f, 103f / 255f, 16f / 255f, 255f / 255f);
        Color pushedTextColor = new Color(190f / 255f, 190f / 255f, 80f / 255f, 255f / 255f);
        GameObject JinkeiButton = GameObject.Find("JinkeiButton").gameObject;

        if(jinkeiId==1) {
            JinkeiButton.transform.Find("Gyorin").GetComponent<Image>().color = pushedTabColor;
            JinkeiButton.transform.Find("Gyorin").transform.Find("Text").GetComponent<Text>().color = pushedTextColor;
        }
        else if(jinkeiId==2) {
            JinkeiButton.transform.Find("Kakuyoku").GetComponent<Image>().color = pushedTabColor;
            JinkeiButton.transform.Find("Kakuyoku").transform.Find("Text").GetComponent<Text>().color = pushedTextColor;
        }
        else if (jinkeiId == 3) {
            JinkeiButton.transform.Find("Engetsu").GetComponent<Image>().color = pushedTabColor;
            JinkeiButton.transform.Find("Engetsu").transform.Find("Text").GetComponent<Text>().color = pushedTextColor;
        }
        else if (jinkeiId == 4) {
            JinkeiButton.transform.Find("Gankou").GetComponent<Image>().color = pushedTabColor;
            JinkeiButton.transform.Find("Gankou").transform.Find("Text").GetComponent<Text>().color = pushedTextColor;
        }



    }

}
