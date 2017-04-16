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
		popText.transform.localScale = new Vector2 (0.35f, 0.35f);
		RectTransform popTextTransform = popText.GetComponent<RectTransform> ();
		popTextTransform.anchoredPosition = new Vector3 (0, 260, 0);
		popText.name = "popText";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            popText.GetComponent<Text>().text = "Finance";
        }else { 
            popText.GetComponent<Text> ().text = "内政状況";
        }

        //Header
        string headerPath = "Prefabs/Map/seiryoku/Syukaku";
		GameObject header = Instantiate (Resources.Load (headerPath)) as GameObject;
		header.transform.SetParent(popup.transform);
		header.transform.localScale = new Vector2 (1, 1);
		header.name = "Syukaku";
		RectTransform headerTransform = header.GetComponent<RectTransform> ();
		headerTransform.anchoredPosition = new Vector3 (0, 135, 0);

        if (Application.systemLanguage != SystemLanguage.Japanese) {
            header.transform.FindChild("NowSeasonText").GetComponent<Text>().text = GameObject.Find("SeasonValue").GetComponent<Text>().text + " Gain";
        }else {
            header.transform.FindChild ("NowSeasonText").GetComponent<Text> ().text = GameObject.Find ("SeasonValue").GetComponent<Text> ().text + "収穫";
        }

        //Button
        string btnPath = "Prefabs/Map/seiryoku/GetShigenBtn";
		GameObject btn = Instantiate (Resources.Load (btnPath)) as GameObject;
		btn.transform.SetParent(popup.transform);
		btn.transform.localScale = new Vector2 (1, 1);
		btn.name = "GetShigenBtn";
		RectTransform btnTransform = btn.GetComponent<RectTransform> ();
		btnTransform.anchoredPosition = new Vector3 (420, 160, 0);


		string seiryoku = PlayerPrefs.GetString ("seiryoku");
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
                                //syogyo = syogyo + naiseiEffectList[0];
                                //totalMoney = totalMoney + naiseiEffectList[0];
                                kuniSyogyo = kuniSyogyo + naiseiEffectList[0];
                            }else if(type == "kzn"){
                                //syogyo = syogyo + (naiseiEffectList[0] * 4);
                                //kozanMoney = kozanMoney + naiseiEffectList[0];
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
						StatusGet sts = new StatusGet();
						int lv = PlayerPrefs.GetInt (jyosyuId.ToString());
						float naiseiSts = (float)sts.getDfc(jyosyuId,lv);
						
						float hpSts = (float)sts.getHp(jyosyuId,lv);
						float atkSts = (float)sts.getAtk(jyosyuId,lv);

                        //float tempSyogyo = (float)syogyo;
                        //tempSyogyo = tempSyogyo + (tempSyogyo * naiseiSts/200);
                        //syogyo = (int)tempSyogyo;

                        float tempKuniSyogyo = (float)kuniSyogyo;
                        tempKuniSyogyo = tempKuniSyogyo + (tempKuniSyogyo * naiseiSts / 200);
                        kuniSyogyo = (int)tempKuniSyogyo;

                        float tempKuniKozan = (float)kuniKozan;
                        tempKuniKozan = tempKuniKozan + (tempKuniKozan * naiseiSts / 200);
                        kuniKozan = (int)tempKuniKozan;

                        //float tempNogyo = (float)nogyo;
                        //tempNogyo = tempNogyo + (tempNogyo * naiseiSts/200);
                        //nogyo = (int)tempNogyo;

                    }
                }
			}
            kozanMoney = kozanMoney + kuniKozan;
            totalMoney = totalMoney + kuniSyogyo;
            syogyo = syogyo + (kuniKozan * 4 + kuniSyogyo);

        }//Kuni Loop Finish


		/*visualize*/
		string statusPath = "Prefabs/Map/seiryoku/StatusBack";
		GameObject status = Instantiate (Resources.Load (statusPath)) as GameObject;
		status.transform.SetParent(popup.transform);
		status.transform.localScale = new Vector2 (1, 1);
		status.transform.localPosition = new Vector2 (0, -120);
		status.name = "StatusBack";

		//Kamon
		string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + myDaimyoId.ToString ();
		GameObject kamon = status.transform.FindChild ("Kamon").gameObject;
		kamon.GetComponent<Image> ().sprite = 
			Resources.Load (imagePath, typeof(Sprite)) as Sprite;
        if (Application.systemLanguage == SystemLanguage.Japanese) {
            kamon.transform.FindChild ("Value").GetComponent<Text> ().text = GameObject.Find ("DaimyoValue").GetComponent<Text> ().text + "  国状況";
        }else {
            kamon.transform.FindChild("Value").GetComponent<Text>().text = GameObject.Find("DaimyoValue").GetComponent<Text>().text + " Status";
        }
        //Kuni
        status.transform.FindChild("Shiro").transform.FindChild("No").GetComponent<Text>().text = mySeiryokuList.Count.ToString();

		//Naisei Bldg.
		status.transform.FindChild("Naisei").transform.FindChild("No").GetComponent<Text>().text = (naiseiBldg + mySeiryokuList.Count).ToString();

		//Tabibito
		int tabibitoQty = PlayerPrefs.GetInt ("HstTabibito");
		status.transform.FindChild("Tabibito").transform.FindChild("No").GetComponent<Text>().text = tabibitoQty.ToString();
			
		//Nanbansen
		int nanbansenQty = PlayerPrefs.GetInt ("HstNanbansen");
		status.transform.FindChild("Ship").transform.FindChild("No").GetComponent<Text>().text = nanbansenQty.ToString();

		//Syogyo
		status.transform.FindChild("StatusSyogyo").transform.FindChild("SyogyoValue").GetComponent<Text>().text = syogyo.ToString();

		//Nogyo
		status.transform.FindChild("StatusNougyo").transform.FindChild("NougyoValue").GetComponent<Text>().text = nogyo.ToString();

		//Gunjyu
		status.transform.FindChild("StatusGunjyu").transform.FindChild("GunjyuValue").GetComponent<Text>().text = (gunjyu*2).ToString();

		//Ashigaru
		status.transform.FindChild("StatusAshigaru").transform.FindChild("AshigaruValue").GetComponent<Text>().text = ashigaru.ToString();

		//Boubi
		status.transform.FindChild("StatusBoubi").transform.FindChild("BoubiValue").GetComponent<Text>().text = boubi.ToString();

		//Bukkyo
		status.transform.FindChild("StatusBukkyo").transform.FindChild("BukkyoValue").GetComponent<Text>().text = bukkyo.ToString();

		//Kirisuto
		status.transform.FindChild("StatusKirisuto").transform.FindChild("KirisutoValue").GetComponent<Text>().text = kirisuto.ToString();

		//Bunka
		status.transform.FindChild("StatusBunka").transform.FindChild("BunkaValue").GetComponent<Text>().text = bunka.ToString();



		//Set Total Amount
		GetAllShigen btnScript = btn.GetComponent<GetAllShigen>();
		btnScript.totalMoney = totalMoney;
		btnScript.totalKozanMoney = kozanMoney;
		btnScript.totalHyourou = totalHyourou;
		btnScript.totalYRL = totalYRL;
		btnScript.totalKBL = totalKBL;
		btnScript.totalYML = totalYML;
		btnScript.totalTPL = totalTPL;
		btnScript.totalYRM = totalYRM;
		btnScript.totalKBM = totalKBM;
		btnScript.totalYMM = totalYMM;
		btnScript.totalTPM = totalTPM;
		btnScript.totalYRH = totalYRH;
		btnScript.totalKBH = totalKBH;
		btnScript.totalYMH = totalYMH;
		btnScript.totalTPH = totalTPH;
		btnScript.totalSNBL = totalSNBL;
		btnScript.totalSNBM = totalSNBM;
		btnScript.totalSNBH = totalSNBH;


        //tutorial
        if (Application.loadedLevelName == "tutorialMain") {
            GameObject.Find("board").transform.FindChild("close").gameObject.SetActive(false);

            TutorialController tutorialScript = new TutorialController();
            Vector2 vect = new Vector2(0, 30);
            GameObject GetShigenBtn = GameObject.Find("GetShigenBtn").gameObject;
            GameObject animObj = tutorialScript.SetPointer(GetShigenBtn, vect);
            animObj.transform.localScale = new Vector2(100, 100);
        }
    }
}
