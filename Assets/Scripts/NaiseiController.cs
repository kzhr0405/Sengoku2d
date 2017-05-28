using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;
using System;

public class NaiseiController : MonoBehaviour {

	public int activeKuniId = 0;
	int syogyo = 0;
	int nogyo = 0;
	int gunjyu = 0;
	int ashigaru = 0;
	public int boubi = 0;
	public int bukkyo = 0;
	public int kirisuto = 0;
	public int bunka = 0;
	public int jyosyuId = 0;
	public string shigen = "";

	//Tabibito
	public GameObject tabibitoObj;
	public float nanbansenRatio = 30;
	public int total = 0;
	public int counter = 0;
	public int remain = 0;
	public string loginTemp;
	public bool isSeaFlg = false;
	public bool isNanbansenFlg = false;

	public bool isCyouteiFlg = false;
	public bool isSyouninFlg = false;
	public bool isNanbanFlg = false;
	public bool isBukkyoFlg = false;

	public AudioSource[] audioSources;

	public void Start () {
		//SE
		audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		BGMSESwitch bgm = new BGMSESwitch ();
		bgm.StopSEVolume ();
		bgm.StopBGMVolume ();

        Message msg = new Message();

        //Get Temp Kuni Id & Stage Id
        activeKuniId = PlayerPrefs.GetInt ("activeKuniId");
		string temp = "naisei" + activeKuniId.ToString ();

		/*Initial Setting*/
		//Kuni Name & jyosyu
		string title = "";
		string kuniName = PlayerPrefs.GetString ("activeKuniName");

		string jyosyuTemp = "jyosyu" + activeKuniId;

		//Clear Image
		foreach ( Transform n in GameObject.Find ("JyosyuImage").transform){
			GameObject.Destroy(n.gameObject);
		}

		//Question
		GameObject.Find("Question").GetComponent<QA> ().qaId = 8;


		if (PlayerPrefs.HasKey (jyosyuTemp)) {
			//Jyosyu Exist
			jyosyuId = PlayerPrefs.GetInt (jyosyuTemp);
			string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
			GameObject busyo = Instantiate (Resources.Load (busyoPath)) as GameObject;
			busyo.name = jyosyuId.ToString ();
			busyo.transform.SetParent (GameObject.Find ("JyosyuImage").transform);
			busyo.transform.localScale = new Vector2 (4, 4);
			busyo.GetComponent<DragHandler>().enabled = false;
			RectTransform busyo_transform = busyo.GetComponent<RectTransform>();
			busyo_transform.anchoredPosition3D = new Vector3(40,40,0);
			busyo_transform.sizeDelta = new Vector2( 23, 23);

			foreach(Transform n in busyo.transform){
				GameObject.Destroy(n.gameObject);
			}

			StatusGet sts = new StatusGet();
			string jyosyuName = sts.getBusyoName(jyosyuId);
			title = kuniName + "・" + jyosyuName;

			//Ninmei Button
			GameObject btn = GameObject.Find ("Ninmei").gameObject;
			btn.GetComponent<Ninmei>().kaininFlg = true;
			btn.transform.FindChild("NinmeiText").GetComponent<Text>().text = msg.getMessage(114);
			btn.GetComponent<Ninmei>().jyosyuId = jyosyuId;
			btn.GetComponent<Ninmei>().jyosyuName = jyosyuName;

		} else {
            //Jyosyu Not Exist
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                title = kuniName;
            }else {
                title = kuniName + "・蔵入地";
            }

			//Ninmei Button
			GameObject btn = GameObject.Find ("Ninmei").gameObject;
			btn.GetComponent<Ninmei>().kaininFlg = false;
			btn.transform.FindChild("NinmeiText").GetComponent<Text>().text = msg.getMessage(115);
		}

		GameObject.Find ("StageNameValue").GetComponent<Text> ().text = title;


		//Money
		int money = PlayerPrefs.GetInt("money");
		GameObject.Find ("MoneyValue").GetComponent<Text> ().text = money.ToString();

		//Hyourou
		int hyourou = PlayerPrefs.GetInt("hyourou");
		GameObject.Find ("HyourouCurrentValue").GetComponent<Text> ().text = hyourou.ToString();

		//HyourouMax
		int hyourouMax = PlayerPrefs.GetInt("hyourouMax");
		GameObject.Find ("HyourouMaxValue").GetComponent<Text> ().text = hyourouMax.ToString();

		//Naisei Icon List
		KuniInfo kuni = new KuniInfo ();
		shigen = kuni.getKuniNaisei (activeKuniId);


		//Special Flag
		if(activeKuniId == 16){
			isCyouteiFlg = true;
		}
		if (activeKuniId == 38 || activeKuniId == 39 || activeKuniId == 58) {
			isSyouninFlg = true;
		}
		if (activeKuniId == 50 || activeKuniId == 56 || activeKuniId == 57|| activeKuniId == 60|| activeKuniId == 63|| activeKuniId == 64) {
			isNanbanFlg = true;
		}


		//Sea or Tree
		isSeaFlg = kuni.getKuniIsSeaFlg (activeKuniId);
		GameObject otherObj = GameObject.Find ("Other").gameObject;
		if (isSeaFlg) {
			if(otherObj.transform.FindChild("treeUpper") != null){
				otherObj.transform.FindChild("treeUpper").gameObject.SetActive(false);
			}
		} else {
			if(otherObj.transform.FindChild("sea1") != null){
				otherObj.transform.FindChild("sea1").gameObject.SetActive(false);
				otherObj.transform.FindChild("sea2").gameObject.SetActive(false);
			}
		}



		/*Open Panel*/
		//Clear Previous Panel
		foreach ( Transform n in GameObject.Find ("NaiseiView").transform){
			GameObject.Destroy(n.gameObject);
		}
		//Clear Previous Mask Panel
		foreach ( Transform n in GameObject.Find ("MaskView").transform){
			GameObject.Destroy(n.gameObject);
		}

		//Open Panel & MaskPanel
 		panelByShiro(activeKuniId);
		panelByKuniLv();

		if (PlayerPrefs.HasKey (temp)) {
			/*initial setting*/
			string naiseiString = PlayerPrefs.GetString (temp);
			List<string> naiseiList = new List<string>();
			char[] delimiterChars = {','};
			naiseiList = new List<string>(naiseiString.Split (delimiterChars));

			/*Naisei Bldg Handling*/
			char[] delimiterChars2 = {':'};
			List<string> deletePanelList = new List<string>();
			Entity_naisei_mst naiseiMst = Resources.Load ("Data/naisei_mst") as Entity_naisei_mst;

			for(int i=1; i<naiseiList.Count;i++){

				List<string> naiseiContentList = new List<string>();
				naiseiContentList = new List<string>(naiseiList[i].Split (delimiterChars2));


				if(naiseiContentList[0] != "0"){
					//Exist
					string bldgRank = "";
					if(int.Parse(naiseiContentList[1])<8){
						bldgRank = "s";
					}else if(int.Parse(naiseiContentList[1]) < 15){
						bldgRank = "m";
					}else if(15 <= int.Parse(naiseiContentList[1])){
						bldgRank = "l";
					}

					//Add Delete Target
					deletePanelList.Add(i.ToString());

					//Make New Panel
					string type = naiseiMst.param [int.Parse(naiseiContentList[0])].code;
					string bldg = type + "_" + bldgRank;
					string bldgPath = "Prefabs/Naisei/Bldg/" + bldg;
					GameObject bldgObj = Instantiate (Resources.Load (bldgPath)) as GameObject;
					bldgObj.transform.parent = GameObject.Find ("NaiseiView").transform;
					bldgObj.transform.localScale = new Vector3 (1, 1, 1);
					setBldg(bldgObj, i);
					bldgObj.name = i.ToString();
					bldgObj.GetComponent<AreaButton>().blank = false;
					bldgObj.GetComponent<AreaButton>().type = type;
					bldgObj.GetComponent<AreaButton>().lv = naiseiContentList[1];
					bldgObj.GetComponent<AreaButton>().naiseiId = int.Parse(naiseiContentList[0]);

					//Effect by Level
					List<int> naiseiEffectList = new List<int>();
					naiseiEffectList = getNaiseiList(type, int.Parse(naiseiContentList[1]));
					if(type != "kzn"){
						if(type != "yr" &&type != "kb"&&type != "tp" &&type != "ym" &&type !="snb"){
							bldgObj.GetComponent<AreaButton>().effect = naiseiEffectList[0];
							bldgObj.GetComponent<AreaButton>().effectNextLv = naiseiEffectList[1];
						}else{
							bldgObj.GetComponent<AreaButton>().effect = naiseiEffectList[0] * 2;
							bldgObj.GetComponent<AreaButton>().effectNextLv = naiseiEffectList[1] * 2;
						}
					}else{
						bldgObj.GetComponent<AreaButton>().effect = naiseiEffectList[0] * 4;
						bldgObj.GetComponent<AreaButton>().effectNextLv = naiseiEffectList[1] * 4;
					}

					bldgObj.GetComponent<AreaButton>().moneyNextLv = naiseiEffectList[2];
					bldgObj.GetComponent<AreaButton>().requiredHyourou =naiseiMst.param [int.Parse(naiseiContentList[0])].hyourou;
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        bldgObj.GetComponent<AreaButton>().naiseiName =naiseiMst.param [int.Parse(naiseiContentList[0])].nameEng;
                    }else {
                        bldgObj.GetComponent<AreaButton>().naiseiName = naiseiMst.param[int.Parse(naiseiContentList[0])].name;
                    }

					//Status
					if(type == "shop"){
						syogyo = syogyo + naiseiEffectList[0];
					}else if(type == "ta"){
						nogyo = nogyo + naiseiEffectList[0];
					}else if(type == "yr" ||type == "kb"||type == "tp" ||type == "ym"||type == "snb"){
						int tempCalc = naiseiEffectList[0] * 2;
						gunjyu = gunjyu + tempCalc;
					}else if(type == "ashigaru"){
						ashigaru = ashigaru + naiseiEffectList[0];
					}else if(type == "trd"){
						boubi = boubi + naiseiEffectList[0];
					}else if(type == "kzn"){
						int tempCalc = naiseiEffectList[0] * 4;
						syogyo = syogyo + tempCalc;
					}else if(type == "nbn"){
						int tempCalc = naiseiEffectList[0];
						kirisuto = kirisuto + tempCalc;
					}else if(type == "kgy"){
						int tempCalc = naiseiEffectList[0];
						bunka = bunka + tempCalc;					
					}else if(type == "bky"){
						int tempCalc = naiseiEffectList[0];
						bukkyo = bukkyo + tempCalc;					
					}else if(type == "hsy"){
						int tempCalc = naiseiEffectList[0];
						ashigaru = ashigaru + tempCalc;					
					}else{
						Debug.Log ("Not Yet");

					}
				}
			}

			//Clear Duplicated Panel
			foreach ( Transform n in GameObject.Find ("NaiseiView").transform){
				if(deletePanelList.Contains(n.name)){
					if(n.tag == "Area"){
						GameObject.Destroy(n.gameObject);
					}
				}
			}

			/*shiro setting*/
			string rank = "";
			if(int.Parse(naiseiList[0])<8){
				rank = "s";
				setShiro(rank, naiseiList[0]);
			}else if(int.Parse(naiseiList[0]) < 15){
				rank = "m";
				setShiro(rank, naiseiList[0]);
			}else if(15 <= int.Parse(naiseiList[0])){
				rank = "l";
				setShiro(rank, naiseiList[0]);
			}
			
			string maskPath = "Prefabs/Naisei/MaskPanel";
			GameObject maskPanel = Instantiate (Resources.Load (maskPath)) as GameObject;
			maskPanel.transform.parent = GameObject.Find ("MaskView").transform;
			maskPanel.transform.localScale = new Vector3 (1, 1, 1);
			RectTransform maskPanel_transform = maskPanel.GetComponent<RectTransform>();
			maskPanel_transform.anchoredPosition3D = new Vector3(0,-20,0);
			maskPanel.name = "shiro_" + rank;


			//Status Handling

			//Status Jyosyu Modification
			if (PlayerPrefs.HasKey (jyosyuTemp)) {
				StatusGet sts = new StatusGet();
				int lv = PlayerPrefs.GetInt (jyosyuId.ToString());
				float naiseiSts = (float)sts.getDfc(jyosyuId,lv);

				float hpSts = (float)sts.getHp(jyosyuId,lv);
				float atkSts = (float)sts.getAtk(jyosyuId,lv);
				float boubiSts = (hpSts + atkSts)/2;

				float tempSyogyo = (float)syogyo;
				tempSyogyo = tempSyogyo + (tempSyogyo * naiseiSts/200);
				float tempNogyo = (float)nogyo;
				tempNogyo = tempNogyo + (tempNogyo * naiseiSts/200);
				float tempBoubi = (float)boubi;
				tempBoubi = tempBoubi + (tempBoubi * naiseiSts/200);

				syogyo = (int)tempSyogyo;
				nogyo = (int)tempNogyo;
				boubi = (int)tempBoubi;

			}

			GameObject.Find("SyogyoValue").GetComponent<Text>().text = syogyo.ToString();
			GameObject.Find("NougyoValue").GetComponent<Text>().text = nogyo.ToString();
			GameObject.Find("GunjyuValue").GetComponent<Text>().text = gunjyu.ToString();
			GameObject.Find("AshigaruValue").GetComponent<Text>().text = ashigaru.ToString();
			GameObject.Find("BoubiValue").GetComponent<Text>().text = boubi.ToString();
			GameObject.Find("BukkyoValue").GetComponent<Text>().text = bukkyo.ToString();
			GameObject.Find("KirisutoValue").GetComponent<Text>().text = kirisuto.ToString();
			GameObject.Find("BunkaValue").GetComponent<Text>().text = bunka.ToString();
			GameObject.Find("BukkyoValue").GetComponent<Text>().text = bukkyo.ToString();


			//jyosyu status update
			if (PlayerPrefs.HasKey (jyosyuTemp)) {
				int jyosyuId = PlayerPrefs.GetInt (jyosyuTemp);
				string heiTmp = "jyosyuHei" + jyosyuId;

				PlayerPrefs.SetInt (heiTmp,ashigaru);
			}
			string boubiTmp = "boubi" + activeKuniId;
			PlayerPrefs.SetInt (boubiTmp,boubi);




		} else {
			//Error
			Debug.Log ("ERROR");
		}

		/***************************/
		/*Tabibito Controller Start*/
		/***************************/

		tabibitoObj = GameObject.Find ("Tabibito").gameObject;
		
		loginTemp = "naiseiLoginDate" + activeKuniId.ToString ();
		string loginTimeString = PlayerPrefs.GetString (loginTemp);
		if (loginTimeString == null || loginTimeString == "") {
			loginTimeString = System.DateTime.Today.ToString ();
			PlayerPrefs.SetString (loginTemp,loginTimeString);
			PlayerPrefs.Flush();
		}
		
		System.DateTime loginTime = System.DateTime.Parse (loginTimeString);
		System.TimeSpan span = System.DateTime.Today - loginTime;
		double spanDay = span.TotalDays;
		
		string counterTemp = "naiseiTabibitoCounter" + activeKuniId.ToString ();
		if (spanDay >= 1) {
			//Reset
			PlayerPrefs.SetInt (counterTemp,0);
			PlayerPrefs.Flush();
			counter = 0;
			
		}else{
			//Get Counted No
			counter = PlayerPrefs.GetInt (counterTemp,0);
		}
		
		//Set Tabibito Max
		if (isCyouteiFlg) {
			bunka = bunka * 2;
		}
		if (isSyouninFlg) {
			bunka = Mathf.CeilToInt((float)bunka * 1.5f);
		}
		if (isNanbanFlg) {
			kirisuto = kirisuto * 2;
			nanbansenRatio = nanbansenRatio * 2;
		}

		total = (boubi + bukkyo + kirisuto + bunka)/10;
		remain = total - counter;
		tabibitoObj.transform.FindChild ("TabibitoMaxValue").GetComponent<Text> ().text = total.ToString ();
        if(remain<0) {
            remain = 0;
        }
		tabibitoObj.transform.FindChild ("TabibitoCountDownValue").GetComponent<Text> ().text = remain.ToString ();



        /***************************/
        /*Tabibito Controller End  */
        /***************************/



        //Tutorial
        int tutorialId = PlayerPrefs.GetInt("tutorialId");
        if (Application.loadedLevelName == "tutorialNaisei" && tutorialId == 3) {
            GameObject tBtnObj = GameObject.Find("tButton").gameObject;
            Destroy(tBtnObj.transform.FindChild("12").gameObject);

            GameObject NaiseiViewObj = GameObject.Find("NaiseiView").gameObject;
            NaiseiViewObj.transform.FindChild("12").transform.SetParent(tBtnObj.transform);

            if(tutorialId==3) {
                TextController txtScript = GameObject.Find("TextBoard").transform.FindChild("Text").GetComponent<TextController>();
                txtScript.SetText(3);
                txtScript.SetNextLine();
                txtScript.tutorialId = 3;
                txtScript.actOnFlg = false;
            }



        }

    }

	public float tabibitoSecMst = 5;
	public float tabibitoSec = 5;
	public float specialRatio = 20;


	void Update(){

		/***************************/
		/*Tabibito Controller Start*/
		/***************************/

		if(remain>0){
			//Count by 5sec
			tabibitoSec -= Time.deltaTime;
			if (tabibitoSec <= 0.0) {

				int rdm = UnityEngine.Random.Range(0,2); //1:happen, 0:not happen
				if(rdm==1){

					int tabibitoNo = PlayerPrefs.GetInt ("HstTabibito");
					tabibitoNo = tabibitoNo + 1;
					PlayerPrefs.SetInt ("HstTabibito",tabibitoNo);

					//Track
					int TrackTabibitoNo = PlayerPrefs.GetInt("TrackTabibitoNo",0);
					TrackTabibitoNo = TrackTabibitoNo + 1;
					PlayerPrefs.SetInt("TrackTabibitoNo",TrackTabibitoNo);

					//Check Special ratio
					float percent = UnityEngine.Random.value;
					percent = percent * 100;

					string targetTyp = "";
					if(percent <= specialRatio){
						//Special Tabibito
						int TrackIjinNo = PlayerPrefs.GetInt("TrackIjinNo",0);
						TrackIjinNo = TrackIjinNo + 1;
						PlayerPrefs.SetInt("TrackIjinNo",TrackIjinNo);

						//Shuffle
						List<int> tabibitoRandomList = new List<int> ();

						if(boubi!=0){
							for(int i=0; i<boubi; i++){
								tabibitoRandomList.Add(0);
								//tabibitoRandomList.Add(2); //test
							}
						}
						if(bunka!=0){
							for(int i=0; i<bunka; i++){
								tabibitoRandomList.Add(1);
								//tabibitoRandomList.Add(2); //test
							}
						}
						if(bukkyo!=0){
							for(int i=0; i<bukkyo; i++){
								tabibitoRandomList.Add(2);
							}
						}
						if(kirisuto!=0){
							for(int i=0; i<kirisuto; i++){
								tabibitoRandomList.Add(3);
								//tabibitoRandomList.Add(2); //test

							}
						}
						int tempId = UnityEngine.Random.Range(0,tabibitoRandomList.Count);
						int tbbtId = tabibitoRandomList[tempId];


						if(tbbtId == 0){
							//Create Boubi Tabibito
							targetTyp = "boubi";
							makeTabibitoInstance(targetTyp);

						}else if(tbbtId == 1){
							//Create Bunka Tabibito
							targetTyp = "bunka";
							makeTabibitoInstance(targetTyp);


						}else if(tbbtId == 2){
							//Create Bukkyo Tabibito
							targetTyp = "bukkyo";
							makeTabibitoInstance(targetTyp);


						}else if(tbbtId == 3){
							//Create Nanban Tabibito
							//Nanban Sen or Nanban Jin
							if(!isNanbansenFlg){
								float shipPercent = UnityEngine.Random.value;
								shipPercent = shipPercent * 100;

								if(shipPercent <= nanbansenRatio){
									//Nanbansen
									makeNanbansen();
									isNanbansenFlg = true;

									int nanbansenNo = PlayerPrefs.GetInt ("HstNanbansen");
									nanbansenNo = nanbansenNo + 1;
									PlayerPrefs.SetInt ("HstNanbansen",nanbansenNo);

									tabibitoNo = tabibitoNo - 1;
									PlayerPrefs.SetInt ("HstTabibito",tabibitoNo);

								}else{
									//Nanbanjin
									targetTyp = "nanban";
									makeTabibitoInstance(targetTyp);
								}
							}else{
								//Nanbanjin
								targetTyp = "nanban";
								makeTabibitoInstance(targetTyp);

							}

						}

					}else{

						//Common
						targetTyp = "common";
						makeTabibitoInstance(targetTyp);

					}

                    if (Application.loadedLevelName != "tutorialNaisei") {
                        //Reduce Counter
                        string nowTime = System.DateTime.Today.ToString ();
					    PlayerPrefs.SetString (loginTemp,nowTime);

					    string counterTemp = "naiseiTabibitoCounter" + activeKuniId.ToString ();
					    int tempCounter = PlayerPrefs.GetInt (counterTemp);
					    tempCounter = tempCounter + 1;
					    PlayerPrefs.SetInt (counterTemp,tempCounter);
					    PlayerPrefs.Flush();
					
					    //Change Label
					    remain = remain -1;
					    tabibitoObj.transform.FindChild ("TabibitoCountDownValue").GetComponent<Text> ().text = remain.ToString ();
									
					    //Reset Timer
					    tabibitoSec = tabibitoSecMst;
                    }

				}else{
					//Skip Tabibito
					tabibitoSec = tabibitoSecMst;
				}
			}

		}


		//Reset Check



		/***************************/
		/*Tabibito Controller End  */
		/***************************/
	
	}












	public List<int> getNaiseiList(string type, int lv){
		List<int> naiseiEffectList = new List<int>();
		Entity_naisei_mst naiseiMst = Resources.Load ("Data/naisei_mst") as Entity_naisei_mst;
		int startline = 0;
		for(int i=0;i<naiseiMst.param.Count;i++){
			if(naiseiMst.param[i].code == type){
				startline = i;
				break;
			}
		}

		object effectLst = naiseiMst.param[startline];
		Type t = effectLst.GetType();

		//Effect on Current Lv
		String param1 = "effect" + lv;
		FieldInfo f1 = t.GetField(param1);
		naiseiEffectList.Add((int)f1.GetValue(effectLst));

		//Effect on Next Lv
		int nextLv = lv + 1;
        if(nextLv<=20) {
		    String param2 = "effect" + nextLv;
		    FieldInfo f2 = t.GetField(param2);
		    naiseiEffectList.Add((int)f2.GetValue(effectLst));

		    //Money for Next Lv
		    String param3 = "money" + nextLv;
		    FieldInfo f3 = t.GetField(param3);
		    naiseiEffectList.Add((int)f3.GetValue(effectLst));
        }else {
            naiseiEffectList.Add(0);
            naiseiEffectList.Add(0);
        }
        return naiseiEffectList;
	}


	public void panelByShiro(int activeKuniId){
		int count = 0;
		string temp = "kuni" + activeKuniId.ToString ();
		string clearedStage = PlayerPrefs.GetString (temp);
		char[] delimiterChars = {','};
		string[] ch_list = clearedStage.Split (delimiterChars);
		count = ch_list.Length;

		if(count >= 1){
			Vector2 vect15 = new Vector2(-85,-120); 
			Vector2 vect16 = new Vector2(180,-120); 
			Vector2 vect17 = new Vector2(445,-120); 

			panelSet(15,vect15);
			panelSet(16,vect16);
			panelSet(17,vect17);

			if(count >= 2){
				Vector2 vect11 = new Vector2(-265,-20); 
				panelSet(11,vect11);

				if(count >= 3){
					Vector2 vect12 = new Vector2(265,-20); 
					panelSet(12,vect12);

					if(count >= 4){
						Vector2 vect6 = new Vector2(-445,80); 
						panelSet(6,vect6);
						
						if(count >= 5){
							Vector2 vect7 = new Vector2(-180,80); 
							panelSet(7,vect7);
							
							if(count >= 6){
								Vector2 vect8 = new Vector2(85,80); 
								panelSet(8,vect8);

								if(count >= 7){
									Vector2 vect21 = new Vector2(360,-220); 
									panelSet(21,vect21);

									if(count >= 8){
										Vector2 vect2 = new Vector2(-360,180); 
										panelSet(2,vect2);
										
										if(count >= 9){
											Vector2 vect10 = new Vector2(-530,-20); 
											panelSet(10,vect10);

											if(count >= 10){
												Vector2 vect13 = new Vector2(530,-20); 
												panelSet(13,vect13);
												
												
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}




		
	}

	public void panelByKuniLv(){
		int count = 0;
		string temp = "kuniLv";
		int kuniLv = PlayerPrefs.GetInt (temp);
		count = kuniLv / 10;

		if (count >= 1) {
			Vector3 vect1 = new Vector3 (-625, 180,0); 
			panelSet (1, vect1);
		
			if (count >= 2) {
				Vector3 vect3 = new Vector3 (-95, 180,0); 
				panelSet (3, vect3);

				if (count >= 3) {
					Vector3 vect4 = new Vector3 (170, 180,0); 
					panelSet (4, vect4);

					if (count >= 4) {
						Vector3 vect5 = new Vector3 (435, 180,0); 
						panelSet (5, vect5);

						if (count >= 5) {
							Vector3 vect9 = new Vector3 (350, 80,0); 
							panelSet (9, vect9);

							if (count >= 6) {
								Vector3 vect14 = new Vector3 (-350,-120,0 ); 
								panelSet (14, vect14);

								if (count >= 7) {
									Vector3 vect18 = new Vector3 (-435,-220,0 ); 
									panelSet (18, vect18);

									if (count >= 8) {
										Vector3 vect19 = new Vector3 (-170,-220,0 ); 
										panelSet (19, vect19);

										if (count >= 9) {
											Vector3 vect20 = new Vector3 (95,-220,0 ); 
											panelSet (20, vect20);

											if (count >= 10) {
												Vector3 vect22 = new Vector3 (625,-220,0 ); 
												panelSet (22, vect22);
											}

										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	public void panelSet(int panelId, Vector3 vect){
		//Panel
		string path = "Prefabs/Naisei/Panel";
		GameObject panel = Instantiate (Resources.Load (path)) as GameObject;
		panel.transform.parent = GameObject.Find ("NaiseiView").transform;
		panel.transform.localScale = new Vector3 (1, 1, 1);
		RectTransform panel_transform = panel.GetComponent<RectTransform>();
		panel_transform.anchoredPosition3D = vect;
		panel.name = panelId.ToString();

		//Mask Panel
		string maskPath = "Prefabs/Naisei/MaskPanel";
		GameObject maskPanel = Instantiate (Resources.Load (maskPath)) as GameObject;
		maskPanel.transform.parent = GameObject.Find ("MaskView").transform;
		maskPanel.transform.localScale = new Vector3 (1, 1, 1);
		maskPanel.transform.localPosition = new Vector3 (maskPanel.transform.localPosition.x, maskPanel.transform.localPosition.y, 0);
		RectTransform maskPanel_transform = maskPanel.GetComponent<RectTransform>();
		maskPanel_transform.anchoredPosition3D = vect;
		maskPanel.name = panelId.ToString();
	}

	public void setShiro(string type, string lv){

		string shiroType = "shiro_" + type; 
		string path = "Prefabs/Naisei/Shiro/" + shiroType ;
		GameObject shiro = Instantiate (Resources.Load (path)) as GameObject;
		shiro.transform.parent = GameObject.Find ("NaiseiView").transform;
		shiro.transform.localScale = new Vector3 (1, 1, 1);

        //special shiro
        string shiroTmp = "shiro" + activeKuniId;
        if(PlayerPrefs.HasKey(shiroTmp)) {
            int shiroId = PlayerPrefs.GetInt(shiroTmp);
            if(shiroId != 0) {
                string imagePath = "Prefabs/Naisei/Shiro/Sprite/" + shiroId;
                shiro.GetComponent<Image>().sprite =
                                Resources.Load(imagePath, typeof(Sprite)) as Sprite;
            }
        }
        
		RectTransform shiro_transform = shiro.GetComponent<RectTransform>();
		shiro_transform.anchoredPosition3D = new Vector3 (shiro.transform.position.x, shiro.transform.position.y, 0);
		shiro.name = shiroType;

		shiro.GetComponent<AreaButton>().type = "shiro";
		shiro.GetComponent<AreaButton> ().blank = false;
		shiro.GetComponent<AreaButton> ().lv = lv;
		shiro.GetComponent<AreaButton>().naiseiId = 0;

		List<int> naiseiEffectList = new List<int>();
		Entity_naisei_mst naiseiMst = Resources.Load ("Data/naisei_mst") as Entity_naisei_mst;
		naiseiEffectList = getNaiseiList("shiro", int.Parse(lv));
		shiro.GetComponent<AreaButton>().effect = naiseiEffectList[0];
		shiro.GetComponent<AreaButton>().effectNextLv = naiseiEffectList[1];
		shiro.GetComponent<AreaButton>().moneyNextLv = naiseiEffectList[2];
		shiro.GetComponent<AreaButton>().requiredHyourou =naiseiMst.param [0].hyourou;
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            shiro.GetComponent<AreaButton>().naiseiName =naiseiMst.param [0].nameEng;
        }else {
            shiro.GetComponent<AreaButton>().naiseiName = naiseiMst.param[0].name;
        }

		ashigaru = ashigaru + naiseiEffectList[0];
		boubi = boubi + naiseiEffectList[0];
	}

	public void setBldg(GameObject bldgObj, int panelId){

		RectTransform bldgTransform = bldgObj.GetComponent<RectTransform> ();

		if (panelId == 1) {
			Vector3 vect1 = new Vector3 (-625, 180,0); 
			bldgTransform.anchoredPosition3D = vect1;

		} else if (panelId == 2) {
			Vector3 vect2 = new Vector3 (-360, 180,0); 
			bldgTransform.anchoredPosition3D = vect2;

		} else if (panelId == 3) {
			Vector3 vect3 = new Vector3 (-95, 180,0);
			bldgTransform.anchoredPosition3D = vect3;

		} else if (panelId == 4) {
			Vector3 vect4 = new Vector3 (170, 180,0);
			bldgTransform.anchoredPosition3D = vect4;

		} else if (panelId == 5) {
			Vector3 vect5 = new Vector3 (435, 180,0); 
			bldgTransform.anchoredPosition3D = vect5;

		} else if (panelId == 6) {
			Vector3 vect6 = new Vector3 (-445, 80,0); 
			bldgTransform.anchoredPosition3D = vect6;

		} else if (panelId == 7) {
			Vector3 vect7 = new Vector3 (-180, 80,0); 
			bldgTransform.anchoredPosition3D = vect7;

		} else if (panelId == 8) {
			Vector3 vect8 = new Vector3 (85, 80,0); 
			bldgTransform.anchoredPosition3D = vect8;

		} else if (panelId == 9) {
			Vector3 vect9 = new Vector3 (350, 80,0);
			bldgTransform.anchoredPosition3D = vect9;

		} else if (panelId == 10) {
			Vector3 vect10 = new Vector3 (-530, -20,0); 
			bldgTransform.anchoredPosition3D = vect10;

		} else if (panelId == 11) {
			Vector3 vect11 = new Vector3 (-265, -20,0);
			bldgTransform.anchoredPosition3D = vect11;

		} else if (panelId == 12) {
			Vector3 vect12 = new Vector3 (265, -20,0); 
			bldgTransform.anchoredPosition3D = vect12;

		} else if (panelId == 13) {
			Vector2 vect13 = new Vector3 (530, -20,0); 
			bldgTransform.anchoredPosition3D = vect13;

		} else if (panelId == 14) {
			Vector3 vect14 = new Vector3 (-350, -120,0); 
			bldgTransform.anchoredPosition3D = vect14;

		} else if (panelId == 15) {
			Vector3 vect15 = new Vector3 (-85, -120,0); 
			bldgTransform.anchoredPosition3D = vect15;

		} else if (panelId == 16) {
			Vector3 vect16 = new Vector3 (180, -120,0); 
			bldgTransform.anchoredPosition3D = vect16;

		} else if (panelId == 17) {
			Vector3 vect17 = new Vector3 (445, -120,0); 
			bldgTransform.anchoredPosition3D = vect17;

		} else if (panelId == 18) {
			Vector3 vect18 = new Vector3 (-435, -220,0); 
			bldgTransform.anchoredPosition3D = vect18;

		} else if (panelId == 19) {
			Vector3 vect19 = new Vector3 (-170, -220,0); 
			bldgTransform.anchoredPosition3D = vect19;

		} else if (panelId == 20) {
			Vector3 vect20 = new Vector3 (95, -220,0); 
			bldgTransform.anchoredPosition3D = vect20;

		} else if (panelId == 21) {
			Vector3 vect21 = new Vector3 (360, -220,0); 
			bldgTransform.anchoredPosition3D = vect21;

		} else if (panelId == 22) {
			Vector3 vect22 = new Vector3 (625, -220,0); 
			bldgTransform.anchoredPosition3D = vect22;

		}
	}

	public void makeTabibitoInstance(string targetTyp){

		//Rank Check
		float rankPercent = UnityEngine.Random.value;
		rankPercent = rankPercent * 100;
		string targetRank = "";


		//For test
		/*
		if (rankPercent <= 100) {
			//S
			targetRank = "S";
		}
		*/

        if(targetTyp== "common") {
		    if (rankPercent <= 1) {
			    //S
			    targetRank = "S";
		    } else if (1 < rankPercent && rankPercent <= 10) {
			    //A
			    targetRank = "A";

		    }else if (10 < rankPercent && rankPercent <= 45) {
			    //B
			    targetRank = "B";

		    }else if (45 < rankPercent) {
			    //C
			    targetRank = "C";
		    }
        }else {
            if(targetTyp == "boubi") {
                if (rankPercent <= 0.05f) {
                    //S
                    targetRank = "S";
                }else if (0.05f < rankPercent && rankPercent <= 3) {
                    //A
                    targetRank = "A";

                }else if (3 < rankPercent && rankPercent <= 20) {
                    //B
                    targetRank = "B";

                }else if (20 < rankPercent) {
                    //C
                    targetRank = "C";
                }
            }else {
                if (rankPercent <= 0.5f) {
                    //S
                    targetRank = "S";
                }else if (0.5f < rankPercent && rankPercent <= 10) {
                    //A
                    targetRank = "A";

                }else if (10 < rankPercent && rankPercent <= 40) {
                    //B
                    targetRank = "B";

                }else if (40 < rankPercent) {
                    //C
                    targetRank = "C";

                }
            }
        }

        //Extract Target Type & Target Rank
        Entity_tabibito_mst tabibitoMst = Resources.Load ("Data/tabibito_mst") as Entity_tabibito_mst;
		List<int> idList = new List<int> ();
		
		for (int i=0; i<tabibitoMst.param.Count; i++) {
			string tempTyp = tabibitoMst.param[i].Typ;
			string tempRank = tabibitoMst.param[i].Rank;

			if(tempTyp == targetTyp && tempRank == targetRank){
				idList.Add(tabibitoMst.param[i].Id);
			}
		}

		//Random Extract
		int rdmId = UnityEngine.Random.Range(0,idList.Count);
		int targetTabibitoId = idList[rdmId];



		//Select Start Point
		List<string> startPointList = new List<string> (){"goalA","goalB","goalC","goalD","goalE","goalF","goalG","goalH","goalI","goalJ","goalK","goalL","goalM","goalN"};
		int rdmId2 = UnityEngine.Random.Range(0,startPointList.Count);
		string startPoint = startPointList[rdmId2];

		GameObject tabibitoView = GameObject.Find ("TabibitoView").gameObject;
		GameObject startPointObj = tabibitoView.transform.FindChild(startPoint).gameObject;

		//Instance
		int grpId = 0;
		string tabibitoPath = "";
		if (targetTyp == "common") {
			tabibitoPath = "Prefabs/Naisei/Tabibito/" + targetTabibitoId;
		} else if (targetTyp == "boubi") {
			tabibitoPath = "Prefabs/Naisei/Tabibito/kengou";
		} else if (targetTyp == "nanban") {
			tabibitoPath = "Prefabs/Naisei/Tabibito/nanban";
		} else if (targetTyp == "bunka") {
			grpId = tabibitoMst.param [targetTabibitoId - 1].GrpID;
            tabibitoPath = "Prefabs/Naisei/Tabibito/bunka" + grpId;
		} else if (targetTyp == "bukkyo") {
			tabibitoPath = "Prefabs/Naisei/Tabibito/bukkyo";
		}
		GameObject prefab = Instantiate(Resources.Load (tabibitoPath)) as GameObject;
		prefab.transform.SetParent(tabibitoView.transform);


		prefab.GetComponent<TabibitoMove> ().destPoint = startPointObj.GetComponent<TabibitoKiller> ().OppositObj;
		prefab.transform.localPosition = new Vector3(startPointObj.transform.localPosition.x, startPointObj.transform.localPosition.y, 0);

		//Add Notice Button
		string ntcBtnPath = "Prefabs/Naisei/Tabibito/NoticeBtn";
		GameObject btn = Instantiate(Resources.Load (ntcBtnPath)) as GameObject;
		btn.transform.SetParent(prefab.transform);
		btn.transform.localPosition = new Vector2 (60, 60);


		if (targetTyp != "nanban" && (targetTyp != "bunka" || grpId != 3)) {
			btn.transform.localScale = new Vector2 (25, 25);
			if (startPoint == "goalA" || startPoint == "goalB" || startPoint == "goalC" || startPoint == "goalD" || startPoint == "goalI" || startPoint == "goalJ" || startPoint == "goalK") {
				prefab.transform.localScale = new Vector2 (0.7f, 0.7f);
			} else {
				prefab.transform.localScale = new Vector2 (-0.7f, 0.7f);
			}
		} else {
			btn.transform.localScale = new Vector2 (20, 20);
			if (startPoint == "goalA" || startPoint == "goalB" || startPoint == "goalC" || startPoint == "goalD" || startPoint == "goalI" || startPoint == "goalJ" || startPoint == "goalK") {
				prefab.transform.localScale = new Vector2 (0.9f, 0.9f);
			} else {
				prefab.transform.localScale = new Vector2 (-0.9f, 0.9f);
			}
		}

        if (Application.systemLanguage != SystemLanguage.Japanese) {
            btn.GetComponent<TabibitoNoticeBtn> ().targetGrp= tabibitoMst.param [targetTabibitoId - 1].GrpEng;
        }else {
            btn.GetComponent<TabibitoNoticeBtn>().targetGrp = tabibitoMst.param[targetTabibitoId - 1].Grp;
        }
		btn.GetComponent<TabibitoNoticeBtn> ().targetGrpId= grpId;
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            btn.GetComponent<TabibitoNoticeBtn> ().targetName = tabibitoMst.param [targetTabibitoId - 1].NameEng;
		    btn.GetComponent<TabibitoNoticeBtn> ().targetExp = tabibitoMst.param [targetTabibitoId - 1].ExpEng;
        }else {
            btn.GetComponent<TabibitoNoticeBtn>().targetName = tabibitoMst.param[targetTabibitoId - 1].Name;
            btn.GetComponent<TabibitoNoticeBtn>().targetExp = tabibitoMst.param[targetTabibitoId - 1].Exp;
        }
		string rank = tabibitoMst.param [targetTabibitoId - 1].Rank;
		btn.GetComponent<TabibitoNoticeBtn> ().targetRank = rank;

		string itemCd = tabibitoMst.param [targetTabibitoId - 1].ItemMst;
		int itemId = tabibitoMst.param [targetTabibitoId - 1].ItemMstId;

		if (itemId == 0) {
			//Random Get Kahou Item
			Kahou kahou = new Kahou();
			string kahouRank = "";
			float kahouPercent = UnityEngine.Random.value;
			kahouPercent = kahouPercent * 100;

			if(rank=="S"){
				//S:A=30%:70%
				if(kahouPercent<=30){
					kahouRank = "S";
				}else{
					kahouRank = "A";
				}
			}else if(rank=="A"){
				//A:B=30%:70%
				if(kahouPercent<=30){
					kahouRank = "A";
				}else{
					kahouRank = "B";
				}
			}else if(rank=="B"){
				//B:C=30%:70%
				if(kahouPercent<=30){
					kahouRank = "B";
				}else{
					kahouRank = "C";
				}
			}

			itemId = kahou.getRamdomKahouId(itemCd, kahouRank);


		}

		btn.GetComponent<TabibitoNoticeBtn> ().itemCd = itemCd;
		btn.GetComponent<TabibitoNoticeBtn> ().itemId = itemId;
		btn.GetComponent<TabibitoNoticeBtn> ().itemQty = tabibitoMst.param [targetTabibitoId - 1].ItemQty;

	}

	public void makeNanbansen(){
		
		audioSources [3].Play ();

		string shipPath = "Prefabs/Naisei/ship";
		GameObject ship = Instantiate(Resources.Load (shipPath)) as GameObject;
		GameObject tabibitoView = GameObject.Find ("TabibitoView").gameObject;
		ship.transform.SetParent(tabibitoView.transform);
		ship.transform.localScale = new Vector2 (1, 1);

		RectTransform ship_transform = ship.GetComponent<RectTransform>();
		ship_transform.anchoredPosition3D = new Vector3(570,180,0);

		Item item = new Item ();
		Nanbansen nanbansen = ship.GetComponent<Nanbansen> ();

		//Get TP Item
		string itemTPCd = "";
		int itemTPId = 0;
		int itemTPQty = 0;
		float itemTPPrice = 0;

		float tpPercent = UnityEngine.Random.value;
		tpPercent = tpPercent * 100;

		if (tpPercent <= 30) {
			//Jyo
			itemTPCd = "CyouheiTP3";
			itemTPId = 3;
		} else if(30 < tpPercent && tpPercent <= 60){
			//Cyu
			itemTPCd = "CyouheiTP2";
			itemTPId = 2;

		}else if(60 < tpPercent){
			//Ge
			itemTPCd = "CyouheiTP1";
			itemTPId = 1;

		}

		itemTPQty = UnityEngine.Random.Range(1,6); //1-5 Qty
		itemTPPrice = randomPriceChange(itemTPCd, itemTPQty);
		nanbansen.itemTPCd = itemTPCd;
		nanbansen.itemTPId = itemTPId;
		nanbansen.itemTPQty = itemTPQty;
		nanbansen.itemTPPrice = itemTPPrice;
		nanbansen.itemTPExp = item.getExplanation (itemTPCd);


		//Get Saku Item
		string itemSakuCd = "";
		int itemSakuQty = 1;
		float itemSakuPrice = 0;
		int itemSakuId = 0;

		float sakuPercent = UnityEngine.Random.value;
		sakuPercent = sakuPercent * 100;
		
		if (sakuPercent <= 30) {
			itemSakuCd = "nanban1";
			itemSakuId = 1;
		} else if(30 < sakuPercent && sakuPercent <= 60){
			itemSakuCd = "nanban2";
			itemSakuId = 2;
		}else if(60 < sakuPercent){
			itemSakuCd = "nanban3";
			itemSakuId = 3;
		}
		itemSakuPrice = randomPriceChange(itemSakuCd, itemSakuQty);
		nanbansen.itemSakuCd = itemSakuCd;
		nanbansen.itemSakuId = itemSakuId;
		nanbansen.itemSakuPrice = itemSakuPrice;
		nanbansen.itemSakuExp = item.getExplanation (itemSakuCd);


		//Get Kahou Item
		string itemKahouCd = "";
		int itemKahouQty = 1;
		float itemKahouPrice = 0;
		
		float kahouPercent = UnityEngine.Random.value;
		kahouPercent = kahouPercent * 100;
		string kahouRank = "";

		if (kahouPercent <= 10) {
			//S
			kahouRank = "S";
		} else if(10 < kahouPercent && kahouPercent <= 30){
			//A
			kahouRank = "A";
		}else if(30 < kahouPercent  && kahouPercent <= 60){
			//B
			kahouRank = "B";
		}else if(60 < kahouPercent){
			//C
			kahouRank = "C";
		}
		Kahou kahou = new Kahou ();
		List<string> kahouRandom = new List<string> (){"bugu","kabuto","gusoku","meiba","cyadougu","chishikisyo","heihousyo"};
		int rdm = UnityEngine.Random.Range(0,7);
		itemKahouCd = kahouRandom[rdm];
		int itemKahouId = kahou.getRamdomKahouId(itemKahouCd, kahouRank);

		List<string> kahouInfo = new List<string> (); 
		KahouStatusGet kahouSts = new KahouStatusGet (); 
		kahouInfo = kahouSts.getKahouInfo(itemKahouCd, itemKahouId);

		float priceChange = UnityEngine.Random.Range (1, 51);
		int updown = UnityEngine.Random.Range (0,2); //0:Up, 1:Down

		float kahouPrice = 0;
		float kahouUnitPrice = float.Parse (kahouInfo [5]);
		float diff = kahouUnitPrice * priceChange/100;
		if(updown==0){
			kahouPrice = kahouUnitPrice + diff;
		}else{
			kahouPrice = kahouUnitPrice - diff;
		}

		nanbansen.itemKahouCd = itemKahouCd;
		nanbansen.itemKahouId = itemKahouId;
		nanbansen.itemKahouPrice = kahouPrice;
		nanbansen.itemKahouExp = kahouInfo [1];

		PlayerPrefs.SetBool ("questSpecialFlg3",true);
		PlayerPrefs.Flush ();

	}

	public float randomPriceChange(string itemCd, int itemTPQty){

		float itemTPPrice = 0;

		Item item = new Item ();
		float unitPrice = (float)item.getUnitPrice (itemCd);
		
		float priceChange = UnityEngine.Random.Range (1, 51);
		int updown = UnityEngine.Random.Range (0,2); //0:Up, 1:Down
		
		float diff = (unitPrice * itemTPQty)*priceChange/100;
		if(updown==0){
			itemTPPrice = (unitPrice * itemTPQty) + diff;
		}else{
			itemTPPrice = (int)(unitPrice * itemTPQty) - diff;
		}

		return itemTPPrice;
	}
}
