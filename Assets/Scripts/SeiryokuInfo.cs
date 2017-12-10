using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class SeiryokuInfo : MonoBehaviour {

	string kuniName = "";
	int shiro = 0;
	public int totalMoney = 0;
	public int kozanMoney = 0;
	public int totalHyourou = 0;
	public int totalYRL = 0;
	public int totalKBL = 0;
	public int totalYML = 0;
	public int totalTPL = 0;
	public int totalYRM = 0;
	public int totalKBM = 0;
	public int totalYMM = 0;
	public int totalTPM = 0;
	public int totalYRH = 0;
	public int totalKBH = 0;
	public int totalYMH = 0;
	public int totalTPH = 0;
	public int totalSNBL = 0;
	public int totalSNBM = 0;
	public int totalSNBH = 0;

	public void OnClick () {

        MainStageController MainStageController = GameObject.Find("GameController").GetComponent<MainStageController>();

        //SE
        AudioSource sound = GameObject.Find ("SEController").GetComponent<AudioSource> ();
		sound.PlayOneShot(sound.clip); 

		//Initialization
		totalMoney = 0;
		kozanMoney = 0;
		totalHyourou = 0;
		totalYRL = 0;
		totalKBL = 0;
		totalYML = 0;
		totalTPL = 0;
		totalYRM = 0;
		totalKBM = 0;
		totalYMM = 0;
		totalTPM = 0;
		totalYRH = 0;
		totalKBH = 0;
		totalYMH = 0;
		totalTPH = 0;
		totalSNBL = 0;
		totalSNBM = 0;
		totalSNBH = 0;

		/*Popup*/
		string backPath = "Prefabs/Busyo/back";
		GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
		back.transform.SetParent(GameObject.Find ("Map").transform);
		back.transform.localScale = new Vector2 (1, 1);
		RectTransform backTransform = back.GetComponent<RectTransform> ();
		backTransform.anchoredPosition = new Vector3 (0, 0, 0);
		
		//Popup Screen
		string popupPath = "Prefabs/Busyo/board";
		GameObject popup = Instantiate (Resources.Load (popupPath)) as GameObject;
		popup.transform.SetParent(GameObject.Find ("Map").transform);
		popup.transform.localScale = new Vector2 (1, 1);
		RectTransform popupTransform = popup.GetComponent<RectTransform> ();
		popupTransform.anchoredPosition = new Vector3 (0, 0, 0);
		popup.name = "board";

		//qa
		string qaPath = "Prefabs/Common/Question";
		GameObject qa = Instantiate (Resources.Load (qaPath)) as GameObject;
		qa.transform.SetParent(popup.transform);
		qa.transform.localScale = new Vector2 (1, 1);
		RectTransform qaTransform = qa.GetComponent<RectTransform> ();
		qaTransform.anchoredPosition = new Vector3 (-540, 285, 0);
		qa.name = "qa";
		qa.GetComponent<QA> ().qaId = 2;


		//Pop text
		string popTextPath = "Prefabs/Busyo/popText";
		GameObject popText = Instantiate (Resources.Load (popTextPath)) as GameObject;
		popText.transform.SetParent(popup.transform);
		popText.transform.localScale = new Vector2 (0.25f, 0.25f);
		RectTransform popTextTransform = popText.GetComponent<RectTransform> ();
		popTextTransform.anchoredPosition = new Vector3 (0, 260, 0);
		popText.name = "popText";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            popText.GetComponent<Text>().text = "Finance";
        }else if(langId==3) {
            popText.GetComponent<Text>().text = "内政状况";
        }else { 
            popText.GetComponent<Text> ().text = "内政状況";
        }

        //Cyosyu
        string cyosyuPath = "Prefabs/Cyosyu/CyosyuObj";
        GameObject CyosyuObj = Instantiate(Resources.Load(cyosyuPath)) as GameObject;
        CyosyuObj.transform.SetParent(popup.transform);
        CyosyuObj.transform.localScale = new Vector2(1, 1);



        string seiryoku = "";
        if (Application.loadedLevelName == "tutorialMain") {
            seiryoku = "1,2,3,4,5,6,7,8,3,4,9,10,12,11,13,14,15,16,3,17,18,17,19,8,19,19,20,21,22,23,24,25,26,27,28,29,30,31,31,32,33,34,35,35,36,37,38,38,38,38,31,31,31,39,40,41,41,41,41,42,43,44,45,45,46";
        }else {
            seiryoku = PlayerPrefs.GetString("seiryoku");
        }
        List<string> seiryokuList = new List<string> ();
		List<string> mySeiryokuList = new List<string> ();
		char[] delimiterChars = {','};

		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
		int myDaimyoId = PlayerPrefs.GetInt ("myDaimyo");

		//Get my Kuni
		for (int i=0; i<seiryokuList.Count; i++) {
			int seiryokuId = int.Parse (seiryokuList [i]);
			if (seiryokuId == myDaimyoId) {
				int kuniId = i + 1;
				mySeiryokuList.Add (kuniId.ToString());
			}
		}


		//Kuni Loop Start
		int naiseiBldg = 0;
		int syogyo = 0;
		int nogyo = 0;
		int gunjyu = 0;
		int ashigaru = 0;
		int boubi = 0;
		int bukkyo = 0;
		int kirisuto = 0;
		int bunka = 0;
        
        //seiryoku loop
		for (int i=0; i<mySeiryokuList.Count; i++) {

            int kuniKozan = 0;
            int kuniSyogyo = 0;

            int kuniId = int.Parse(mySeiryokuList[i]);
			string temp = "kuni" + mySeiryokuList[i];
			string clearedKuni = PlayerPrefs.GetString (temp);
			//Shiro Qty
			if(clearedKuni != null && clearedKuni != ""){
				List<string> shiroList = new List<string>();
				shiroList = new List<string>(clearedKuni.Split (delimiterChars));
				shiro = shiroList.Count;

				//Kuni Name
				Entity_kuni_mst kuniMst = Resources.Load ("Data/kuni_mst") as Entity_kuni_mst;
				kuniName = kuniMst.param[kuniId-1].kuniName;

				string naiseiTemp = "naisei" + mySeiryokuList[i];
				string naiseiString = PlayerPrefs.GetString (naiseiTemp);
                if (PlayerPrefs.HasKey (naiseiTemp)) {
					
					List<string> naiseiList = new List<string>();
					naiseiList = new List<string>(naiseiString.Split (delimiterChars));
					char[] delimiterChars2 = {':'};
					List<string> deletePanelList = new List<string>();


					for(int j=1; j<naiseiList.Count;j++){

                        List<string> naiseiContentList = new List<string>();
						naiseiContentList = new List<string>(naiseiList[j].Split (delimiterChars2));
						
						
						if(naiseiContentList[0] != "0"){
							//Exist
							Entity_naisei_mst naiseiMst = Resources.Load ("Data/naisei_mst") as Entity_naisei_mst;
							string type = naiseiMst.param [int.Parse(naiseiContentList[0])].code;
							naiseiBldg = naiseiBldg + 1;

							//Effect by Level
							List<int> naiseiEffectList = new List<int>();
							NaiseiController naisei = new NaiseiController();
							naiseiEffectList = naisei.getNaiseiList(type, int.Parse(naiseiContentList[1]));


							//Status
							if(type == "shop"){
                                kuniSyogyo = kuniSyogyo + naiseiEffectList[0];
                            }else if(type == "kzn"){
                                kuniKozan = kuniKozan + naiseiEffectList[0];

                            }else if(type == "ta"){
								nogyo = nogyo + naiseiEffectList[0];
								totalHyourou = totalHyourou + naiseiEffectList[0];
							}else if(type == "yr"){
								if(int.Parse(naiseiContentList[1])<11){
									//Low
									gunjyu = gunjyu + naiseiEffectList[0];
									totalYRL = totalYRL + naiseiEffectList[0];

								}else if(int.Parse(naiseiContentList[1]) < 16){
									//Middle
									gunjyu = gunjyu + naiseiEffectList[0];
									totalYRM = totalYRM + naiseiEffectList[0];

								}else if(15 <= int.Parse(naiseiContentList[1])){
									//High
									gunjyu = gunjyu + naiseiEffectList[0];
									totalYRH = totalYRH + naiseiEffectList[0];

								}


							}else if(type == "kb"){
								if(int.Parse(naiseiContentList[1])<11){
									//Low
									gunjyu = gunjyu + naiseiEffectList[0];
									totalKBL = totalKBL + naiseiEffectList[0];
									
								}else if(int.Parse(naiseiContentList[1]) < 16){
									//Middle
									gunjyu = gunjyu + naiseiEffectList[0];
									totalKBM = totalKBM + naiseiEffectList[0];
									
								}else if(15 <= int.Parse(naiseiContentList[1])){
									//High
									gunjyu = gunjyu + naiseiEffectList[0];
									totalKBH = totalKBH + naiseiEffectList[0];
									
								}

							}else if(type == "ym"){
								if(int.Parse(naiseiContentList[1])<11){
									//Low
									gunjyu = gunjyu + naiseiEffectList[0];
									totalYML = totalYML + naiseiEffectList[0];
									
								}else if(int.Parse(naiseiContentList[1]) < 16){
									//Middle
									gunjyu = gunjyu + naiseiEffectList[0];
									totalYMM = totalYMM + naiseiEffectList[0];
									
								}else if(15 <= int.Parse(naiseiContentList[1])){
									//High
									gunjyu = gunjyu + naiseiEffectList[0];
									totalYMH = totalYMH + naiseiEffectList[0];
									
								}
							}else if(type == "tp"){
								if(int.Parse(naiseiContentList[1])<11){
									//Low
									gunjyu = gunjyu + naiseiEffectList[0];
									totalTPL = totalTPL + naiseiEffectList[0];
									
								}else if(int.Parse(naiseiContentList[1]) < 16){
									//Middle
									gunjyu = gunjyu + naiseiEffectList[0];
									totalTPM = totalTPM + naiseiEffectList[0];
									
								}else if(15 <= int.Parse(naiseiContentList[1])){
									//High
									gunjyu = gunjyu + naiseiEffectList[0];
									totalTPH = totalTPH + naiseiEffectList[0];
									
								}

							}else if(type == "snb"){
								if(int.Parse(naiseiContentList[1])<11){
									//Low
									gunjyu = gunjyu + naiseiEffectList[0];
									totalSNBL = totalSNBL + naiseiEffectList[0];
									
								}else if(int.Parse(naiseiContentList[1]) < 16){
									//Middle
									gunjyu = gunjyu + naiseiEffectList[0];
									totalSNBM = totalSNBM + naiseiEffectList[0];
									
								}else if(15 <= int.Parse(naiseiContentList[1])){
									//High
									gunjyu = gunjyu + naiseiEffectList[0];
									totalSNBH = totalSNBH + naiseiEffectList[0];
									
								}
							}else if(type == "trd"){
								boubi = boubi + naiseiEffectList[0];
							}else if(type == "nbn"){
								kirisuto = kirisuto + naiseiEffectList[0];
							}else if(type == "kgy"){
								bunka = bunka + naiseiEffectList [0];			
							}else if(type == "bky"){
								bukkyo = bukkyo + naiseiEffectList [0];			
							}else if(type == "hsy"){
								ashigaru = ashigaru + naiseiEffectList [0];				
							}



						}
					}

					//Shiro
					int shiroLv = int.Parse(naiseiList[0]);
					NaiseiController naise = new NaiseiController();
					List<int> naiseiShiroEffectList = new List<int>();
					naiseiShiroEffectList = naise.getNaiseiList("shiro", shiroLv);
					ashigaru = ashigaru + naiseiShiroEffectList[0];
					boubi = boubi + naiseiShiroEffectList[0];


					//Jyosyu Addition
					string jyosyuTemp = "jyosyu" + kuniId;

					if (PlayerPrefs.HasKey (jyosyuTemp)) {
						int jyosyuId = PlayerPrefs.GetInt (jyosyuTemp);
                        if(jyosyuId!=0) {
						    StatusGet sts = new StatusGet();
						    int lv = PlayerPrefs.GetInt (jyosyuId.ToString());
						    float naiseiSts = (float)sts.getDfc(jyosyuId,lv);
						
						    float hpSts = (float)sts.getHp(jyosyuId,lv);
						    float atkSts = (float)sts.getAtk(jyosyuId,lv);

                            float tempKuniSyogyo = (float)kuniSyogyo;
                            tempKuniSyogyo = tempKuniSyogyo + (tempKuniSyogyo * naiseiSts / 200);
                            kuniSyogyo = (int)tempKuniSyogyo;

                            float tempKuniKozan = (float)kuniKozan;
                            tempKuniKozan = tempKuniKozan + (tempKuniKozan * naiseiSts / 200);
                            kuniKozan = (int)tempKuniKozan;
                        }

                    }
                }
			}
            kozanMoney = kozanMoney + kuniKozan;
            totalMoney = totalMoney + kuniSyogyo;
            syogyo = syogyo + (kuniKozan * 4 + kuniSyogyo);

        }//Kuni Loop Finish


        /*visualize*/
        //Upper Board
        GameObject spring = CyosyuObj.transform.Find("spring").gameObject;
        spring.transform.Find("TargetMoney").transform.Find("Value").GetComponent<Text>().text = (totalMoney + kozanMoney).ToString();

        GameObject summerWinter = CyosyuObj.transform.Find("summerWinter").gameObject;
        summerWinter.transform.Find("TargetMoney").transform.Find("Value").GetComponent<Text>().text = kozanMoney.ToString();
        GameObject TargetGunjyu = summerWinter.transform.Find("TargetGunjyu").gameObject;
        if (totalYRH != 0) {
            TargetGunjyu.transform.Find("YR").transform.Find("CyouheiYRValueH").GetComponent<Text>().text = totalYRH.ToString();
        }
        if (totalYRM != 0) {
            TargetGunjyu.transform.Find("YR").transform.Find("CyouheiYRValueM").GetComponent<Text>().text = totalYRM.ToString();
        }
        if (totalYRL != 0) {
            TargetGunjyu.transform.Find("YR").transform.Find("CyouheiYRValueL").GetComponent<Text>().text = totalYRL.ToString();
        }
        if (totalKBH != 0) {
            TargetGunjyu.transform.Find("KB").transform.Find("CyouheiKBValueH").GetComponent<Text>().text = totalKBH.ToString();
        }
        if (totalKBM != 0) {
            TargetGunjyu.transform.Find("KB").transform.Find("CyouheiKBValueM").GetComponent<Text>().text = totalKBM.ToString();
        }
        if (totalKBL != 0) {
            TargetGunjyu.transform.Find("KB").transform.Find("CyouheiKBValueL").GetComponent<Text>().text = totalKBL.ToString();
        }
        if (totalYMH != 0) {
            TargetGunjyu.transform.Find("YM").transform.Find("CyouheiYMValueH").GetComponent<Text>().text = totalYMH.ToString();
        }
        if (totalYMM != 0) {
            TargetGunjyu.transform.Find("YM").transform.Find("CyouheiYMValueM").GetComponent<Text>().text = totalYMM.ToString();
        }
        if (totalYML != 0) {
            TargetGunjyu.transform.Find("YM").transform.Find("CyouheiYMValueL").GetComponent<Text>().text = totalYML.ToString();
        }
        if (totalTPH != 0) {
            TargetGunjyu.transform.Find("TP").transform.Find("CyouheiTPValueH").GetComponent<Text>().text = totalTPH.ToString();
        }
        if (totalTPM != 0) {
            TargetGunjyu.transform.Find("TP").transform.Find("CyouheiTPValueM").GetComponent<Text>().text = totalTPM.ToString();
        }
        if (totalTPL != 0) {
            TargetGunjyu.transform.Find("TP").transform.Find("CyouheiTPValueL").GetComponent<Text>().text = totalTPL.ToString();
        }
        if (totalSNBH != 0) {
            TargetGunjyu.transform.Find("SNB").transform.Find("SNBValueH").GetComponent<Text>().text = totalSNBH.ToString();
        }
        if (totalSNBM != 0) {
            TargetGunjyu.transform.Find("SNB").transform.Find("SNBValueM").GetComponent<Text>().text = totalSNBM.ToString();
        }
        if (totalSNBL != 0) {
            TargetGunjyu.transform.Find("SNB").transform.Find("SNBValueL").GetComponent<Text>().text = totalSNBL.ToString();
        }

        GameObject autumn = CyosyuObj.transform.Find("autumn").gameObject;
        autumn.transform.Find("TargetMoney").transform.Find("Value").GetComponent<Text>().text = kozanMoney.ToString();
        autumn.transform.Find("TargetHyourou").transform.Find("Value").GetComponent<Text>().text = totalHyourou.ToString();

        //Color
        int nowSeason = MainStageController.nowSeason;
        if (nowSeason==1) {
            summerWinter.transform.Find("summer").gameObject.AddComponent<TextBlinker>();
        }else if(nowSeason==2) {
            autumn.transform.Find("autumn").gameObject.AddComponent<TextBlinker>();
        }else if(nowSeason==3) {
            summerWinter.transform.Find("winter").gameObject.AddComponent<TextBlinker>();
        }else if(nowSeason==4) {
            spring.transform.Find("spring").gameObject.AddComponent<TextBlinker>();
        }


        //Lower Board
        GameObject status = CyosyuObj.transform.Find("statusBack").gameObject;

		//Kamon
		string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + myDaimyoId.ToString ();
		GameObject kamon = status.transform.Find ("Kamon").gameObject;
		kamon.GetComponent<Image> ().sprite = 
			Resources.Load (imagePath, typeof(Sprite)) as Sprite;
        if (langId == 2) {
            kamon.transform.Find("Value").GetComponent<Text>().text = GameObject.Find("DaimyoValue").GetComponent<Text>().text + " Status";
        }else if(langId==3) {
            kamon.transform.Find("Value").GetComponent<Text>().text = GameObject.Find("DaimyoValue").GetComponent<Text>().text + "  本国状况";
        } else {
            kamon.transform.Find("Value").GetComponent<Text>().text = GameObject.Find("DaimyoValue").GetComponent<Text>().text + "  国状況";
        }
        //Kuni
        status.transform.Find("Shiro").transform.Find("No").GetComponent<Text>().text = mySeiryokuList.Count.ToString();

		//Naisei Bldg.
		status.transform.Find("Naisei").transform.Find("No").GetComponent<Text>().text = (naiseiBldg + mySeiryokuList.Count).ToString();

		//Tabibito
		int tabibitoQty = PlayerPrefs.GetInt ("HstTabibito");
		status.transform.Find("Tabibito").transform.Find("No").GetComponent<Text>().text = tabibitoQty.ToString();
			
		//Nanbansen
		int nanbansenQty = PlayerPrefs.GetInt ("HstNanbansen");
		status.transform.Find("Ship").transform.Find("No").GetComponent<Text>().text = nanbansenQty.ToString();

		//Syogyo
		status.transform.Find("StatusSyogyo").transform.Find("SyogyoValue").GetComponent<Text>().text = syogyo.ToString();

		//Nogyo
		status.transform.Find("StatusNougyo").transform.Find("NougyoValue").GetComponent<Text>().text = nogyo.ToString();

		//Gunjyu
		status.transform.Find("StatusGunjyu").transform.Find("GunjyuValue").GetComponent<Text>().text = (gunjyu*2).ToString();

		//Ashigaru
		status.transform.Find("StatusAshigaru").transform.Find("AshigaruValue").GetComponent<Text>().text = ashigaru.ToString();

		//Boubi
		status.transform.Find("StatusBoubi").transform.Find("BoubiValue").GetComponent<Text>().text = boubi.ToString();

		//Bukkyo
		status.transform.Find("StatusBukkyo").transform.Find("BukkyoValue").GetComponent<Text>().text = bukkyo.ToString();

		//Kirisuto
		status.transform.Find("StatusKirisuto").transform.Find("KirisutoValue").GetComponent<Text>().text = kirisuto.ToString();

		//Bunka
		status.transform.Find("StatusBunka").transform.Find("BunkaValue").GetComponent<Text>().text = bunka.ToString();

        //tutorial
        if (Application.loadedLevelName == "tutorialMain") {
            Destroy(transform.Find("point_up").gameObject);
            TutorialController TutorialController = new TutorialController();
            Vector2 vect = new Vector2(0, 50);
            GameObject closeObj = popup.transform.Find("close").gameObject;
            GameObject animObj = TutorialController.SetPointer(closeObj, vect);
            animObj.transform.localScale = new Vector2(120, 120);
            
            GameObject SubButtonViewLeft = GameObject.Find("SubButtonViewLeft").gameObject;
            GameObject.Find("SeiryokuInfo").transform.SetParent(SubButtonViewLeft.transform);
        }
    }

    public string GetSeason(int seasonId) {
        
        string seasonName = "";

        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            if (seasonId == 1) {
                seasonName = "Spring";
            }else if (seasonId == 2) {
                seasonName = "Summer";
            }else if (seasonId == 3) {
                seasonName = "Autumn";
            }else if (seasonId == 4) {
                seasonName = "Winter";
            }
        }else {
            if (seasonId == 1) {
                seasonName = "春";
            }else if (seasonId == 2) {
                seasonName = "夏";
            }else if (seasonId == 3) {
                seasonName = "秋";
            }else if (seasonId == 4) {
                seasonName = "冬";
            }
        }

        return seasonName;
    }

    ScenarioSelect ScenarioSelect = new ScenarioSelect();

}
