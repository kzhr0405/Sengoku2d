using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Reflection;
using System.Collections.Generic;
using System;

public class BusyoStatusButton : MonoBehaviour {

	public string buttonName;

	//kanjyo
	public int pa_lv;

	//chigyo,kunren
	public string ch_type;
	public string ch_heisyu;
	public int ch_num;
	public int ch_lv;
	public float ch_status;
	public float ch_hp;
	public string busyoName;
	public string busyoId;
	public int pa_hp;

    public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();
        Message Message = new Message();
        busyoName = GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyoName;
		busyoId = GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyo;

        //commonPopup ();
        int langId = PlayerPrefs.GetInt("langId");

        if (name == "kanjyo"){
            commonPopup(16);
            GameObject.Find("popText").GetComponent<Text>().text = Message.getMessage(212,langId);

            //Busyo View
            string path = "Prefabs/Player/Unit/BusyoUnit";
			GameObject Busyo = Instantiate (Resources.Load (path)) as GameObject;
			Busyo.name = busyoId.ToString ();
			Busyo.transform.SetParent (GameObject.Find ("board(Clone)").transform);
			Busyo.transform.localScale = new Vector2 (3, 3);
			Busyo.GetComponent<DragHandler>().enabled = false;
			RectTransform busyo_transform = Busyo.GetComponent<RectTransform>();
			busyo_transform.anchoredPosition3D = new Vector3(300,350,0);
			busyo_transform.sizeDelta = new Vector2( 100, 100);

			//Text Modification
			GameObject text = Busyo.transform.Find ("Text").gameObject;
			text.GetComponent<Text> ().color = new Color(255,255,255,255);
			RectTransform text_transform = text.GetComponent<RectTransform>();
			text_transform.anchoredPosition3D = new Vector3 (-70,30,0);
			text_transform.sizeDelta = new Vector2( 630, 120);
			text.transform.localScale = new Vector2 (0.2f,0.2f);

			//Rank Text Modification
			GameObject rank = Busyo.transform.Find ("Rank").gameObject;
			RectTransform rank_transform = rank.GetComponent<RectTransform>();
			rank_transform.anchoredPosition3D = new Vector3 (20,-50,0);
			rank_transform.sizeDelta = new Vector2( 200, 200);
			rank.GetComponent<Text>().fontSize = 200;

			//Common for Kanjyo
			string kanjyoPath = "Prefabs/Busyo/Kanjyo";
			GameObject kanjyo = Instantiate (Resources.Load (kanjyoPath)) as GameObject;
			kanjyo.transform.SetParent (GameObject.Find ("board(Clone)").transform);
			kanjyo.transform.localScale = new Vector2 (1, 1);
			RectTransform kanjyo_transform = kanjyo.GetComponent<RectTransform>();
			kanjyo_transform.anchoredPosition3D = new Vector3(0,0,0);

			//Busyo Lv 
			GameObject.Find ("PopLvValue").GetComponent<Text>().text = pa_lv.ToString();
			
			//Exp Status Bar
			Exp exp = new Exp();

			GameObject expSlider = GameObject.Find ("ExpSlider");
			int nextExp =exp.getDifExpforNextLv(pa_lv);
			string tempExp = "exp" + busyoId;
			int nowExp = PlayerPrefs.GetInt(tempExp);
			int startExp = nowExp - exp.getExpforNextLv(pa_lv-1);

			expSlider.GetComponent<Slider>().maxValue = nextExp;
			expSlider.GetComponent<Slider>().value = (float)startExp;


			GameObject.Find ("CurrentExpValue").GetComponent<Text>().text = startExp.ToString();
			GameObject.Find ("NextLvExpValue").GetComponent<Text>().text = nextExp.ToString();


			Item item =new Item();

			//Low kanjyo fields
			string kanjyoItemPath = "Prefabs/Item/Kanjyo/Kanjyo";
			GameObject lowKanjyoItem = Instantiate (Resources.Load (kanjyoItemPath)) as GameObject;
			lowKanjyoItem.transform.SetParent(GameObject.Find ("KakyuKanjyo").transform);
			lowKanjyoItem.transform.localScale = new Vector2 (0.8f, 0.8f);
			RectTransform lowKanjyoTransform = lowKanjyoItem.GetComponent<RectTransform> ();
			lowKanjyoTransform.anchoredPosition3D = new Vector3 (-210, 125, 0);
			lowKanjyoTransform.sizeDelta = new Vector2 (100, 100);
			RectTransform lowKanjyoRank = lowKanjyoItem.transform.Find("KanjyoRank").GetComponent<RectTransform>();
			lowKanjyoRank.anchoredPosition3D = new Vector3(-30,30,0);
			RectTransform lowKanjyoRect = lowKanjyoItem.transform.Find("Kanjyo").GetComponent<RectTransform>();
			lowKanjyoRect.sizeDelta = new Vector2 (100, 100);
			Color lowColor = new Color (86f / 255f, 87f / 255f, 255f / 255f, 255f / 255f);
			lowKanjyoItem.GetComponent<Image>().color = lowColor;            
            lowKanjyoItem.transform.Find("KanjyoRank").GetComponent<Text>().text = Message.getMessage(181,langId);

            lowKanjyoItem.name = "Kanjyo1";

			//Item Effect
			int effectForLow =item.getEffect(lowKanjyoItem.name);
			GameObject.Find ("KakyuKanjyoExpValue").GetComponent<Text>().text = effectForLow.ToString();

			//Middle kanjyo fields
			GameObject midKanjyoItem = Instantiate (Resources.Load (kanjyoItemPath)) as GameObject;
			midKanjyoItem.transform.SetParent(GameObject.Find ("CyukyuKanjyo").transform);
			midKanjyoItem.transform.localScale = new Vector2 (0.8f, 0.8f);
			RectTransform midKanjyoTransform = midKanjyoItem.GetComponent<RectTransform> ();
			midKanjyoTransform.anchoredPosition3D = new Vector3 (-210, 125, 0);
			midKanjyoTransform.sizeDelta = new Vector2 (100, 100);
			RectTransform midKanjyoRank = midKanjyoItem.transform.Find("KanjyoRank").GetComponent<RectTransform>();
			midKanjyoRank.anchoredPosition3D = new Vector3(-30,30,0);
			RectTransform midKanjyoRect = midKanjyoItem.transform.Find("Kanjyo").GetComponent<RectTransform>();
			midKanjyoRect.sizeDelta = new Vector2 (100, 100);
			Color midColor = new Color (236f / 255f, 93f / 255f, 93f / 255f, 255f / 255f);
			midKanjyoItem.GetComponent<Image>().color = midColor;
            midKanjyoItem.transform.Find("KanjyoRank").GetComponent<Text>().text = Message.getMessage(182,langId);

            midKanjyoItem.name = "Kanjyo2";

			//Item Effect
			int effectForMid =item.getEffect(midKanjyoItem.name);
			GameObject.Find ("CyukyuKanjyoExpValue").GetComponent<Text>().text = effectForMid.ToString();


			//Hight kanjyo fields
			GameObject highKanjyoItem = Instantiate (Resources.Load (kanjyoItemPath)) as GameObject;
			highKanjyoItem.transform.SetParent(GameObject.Find ("JyokyuKanjyo").transform);
			highKanjyoItem.transform.localScale = new Vector2 (0.8f, 0.8f);
			RectTransform hightKanjyoTransform = highKanjyoItem.GetComponent<RectTransform> ();
			hightKanjyoTransform.anchoredPosition3D = new Vector3 (-210, 125, 0);
			hightKanjyoTransform.sizeDelta = new Vector2 (100, 100);
			RectTransform highKanjyoRank = highKanjyoItem.transform.Find("KanjyoRank").GetComponent<RectTransform>();
			highKanjyoRank.anchoredPosition3D = new Vector3(-30,30,0);
			RectTransform highKanjyoRect = highKanjyoItem.transform.Find("Kanjyo").GetComponent<RectTransform>();
			highKanjyoRect.sizeDelta = new Vector2 (100, 100);
			Color hightColor = new Color (207f / 255f, 232f / 255f, 95f / 255f, 255f / 255f);
			highKanjyoItem.GetComponent<Image>().color = hightColor;
            highKanjyoItem.transform.Find("KanjyoRank").GetComponent<Text>().text = Message.getMessage(183,langId);

            highKanjyoItem.name = "Kanjyo3";

			//Item Effect
			int effectForHight =item.getEffect(highKanjyoItem.name);
			GameObject.Find ("JyokyuKanjyoExpValue").GetComponent<Text>().text = effectForHight.ToString();

			//Posessing QTY
			string kanjyoQtyString = PlayerPrefs.GetString ("kanjyo");
			char[] delimiterChars = {','};
			string[] kanjyoList = kanjyoQtyString.Split (delimiterChars);
			GameObject.Find ("KakyuKanjyoQtyValue").GetComponent<Text>().text = kanjyoList[0];
			GameObject.Find ("CyukyuKanjyoQtyValue").GetComponent<Text>().text = kanjyoList[1];
			GameObject.Find ("JyokyuKanjyoQtyValue").GetComponent<Text>().text = kanjyoList[2];

			//Setting Value on Button
			GameObject.Find ("DoKakyuKanjyo").GetComponent<DoKanjyo>().kanjyoList = kanjyoList;
			GameObject.Find ("DoCyukyuKanjyo").GetComponent<DoKanjyo>().kanjyoList = kanjyoList;
			GameObject.Find ("DoJyokyuKanjyo").GetComponent<DoKanjyo>().kanjyoList = kanjyoList;

		}else if(name == "ButtonCyouhei"){
            commonPopup(17);
            cyouheiView(ch_type, langId);

		}else if(name == "ButtonKunren"){
            commonPopup(18);            
            GameObject.Find("popText").GetComponent<Text>().text = Message.getMessage(213,langId);

            string chigyouPath = "Prefabs/Busyo/Kunren";
			GameObject chigyo = Instantiate (Resources.Load (chigyouPath)) as GameObject;
			chigyo.transform.SetParent(GameObject.Find ("board(Clone)").transform);
			chigyo.transform.localScale = new Vector2 (1, 1);
			RectTransform chigyoTransform = chigyo.GetComponent<RectTransform> ();
			chigyoTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
			chigyo.name = "Kunren";

			//Butai Name
			GameObject.Find ("PopTextButaiName").GetComponent<Text>().text = busyoName + " " + ch_heisyu;
			string chPath = "Prefabs/Player/Unit/" + ch_type;
			GameObject chObj = Instantiate (Resources.Load (chPath)) as GameObject;
			chObj.transform.SetParent(GameObject.Find ("Kunren").transform);
			chObj.transform.localScale = new Vector2 (8, 8);
			
			RectTransform chTransform = chObj.GetComponent<RectTransform> ();
			chTransform.anchoredPosition3D = new Vector3 (-260, 0, 0);

			//Butai Level
			GameObject.Find ("LvFrom").GetComponent<Text>().text = ch_lv.ToString();
			GameObject.Find ("LvTo").GetComponent<Text>().text = (ch_lv + 1).ToString();

			//Butai Status
			GameObject.Find ("PopHpValue").GetComponent<Text>().text = ch_hp.ToString();
			GameObject.Find ("PopAtkValue").GetComponent<Text>().text = ch_status.ToString();
			GameObject.Find ("PopDfcValue").GetComponent<Text>().text = ch_status.ToString();
			GameObject.Find ("PopButaiNoValue").GetComponent<Text>().text = ch_num.ToString();

			/* Slider Setting*/
			//Required Money
			Entity_ch_exp_mst kunrenMst = Resources.Load ("Data/ch_exp_mst") as Entity_ch_exp_mst;

			int MaxLv = 100; //Max - 1
			int myMoney = PlayerPrefs.GetInt ("money");
			int totalMoney=0;
			List<int> requredMoneyByLv = new List<int>();

			//Slider Initial Setting
			Slider lvSlider = GameObject.Find ("KunrenSlider").GetComponent<Slider>(); 
			lvSlider.minValue = ch_lv + 1;
			lvSlider.value = ch_lv + 1;
			lvSlider.GetComponent<LvSlider>().toLv = GameObject.Find ("LvTo");
			lvSlider.GetComponent<LvSlider>().hp = GameObject.Find ("PopHpValueUp");
			lvSlider.GetComponent<LvSlider>().atk = GameObject.Find ("PopAtkValueUp");
			lvSlider.GetComponent<LvSlider>().dfc = GameObject.Find ("PopDfcValueUp");
			lvSlider.GetComponent<LvSlider>().requiredMoney = GameObject.Find ("RequiredMoneyValue");


			int limitLv=0; 

			//Lv100 Check
			int totalMoneyMax = kunrenMst.param[99].totalMoney;
			if(myMoney >= totalMoneyMax){
				lvSlider.maxValue = 100;
				limitLv = 100;
			}else{
				//Check Limitation of Lv up & Money
				for(int k=ch_lv; k<MaxLv; k++ ){
					int requiredMoney = kunrenMst.param[k].requiredMoney;
					totalMoney = totalMoney + requiredMoney;

					if(myMoney < totalMoney){
						//Limitation of Lv up
						//Setup Slider Limitation
						lvSlider.maxValue = k;
						limitLv=k;
						break;
					}else {
                        if(k==99) {
                            lvSlider.maxValue = 100;
                            limitLv = 100;
                            break;
                        }
                    }
				}
			}

			//Money List
			int paiedTotalMoney = kunrenMst.param[ch_lv-1].totalMoney;
			for(int j=0; j<100;j++ ){
				int totalRequiredMoney = kunrenMst.param[j].totalMoney - paiedTotalMoney;
				requredMoneyByLv.Add(totalRequiredMoney);
			}

			//Can Lvup
			if(limitLv!=ch_lv){

				//ChildStatusGet
				List<int> statusByLv = new List<int>();
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

                //for(int i=ch_lv+1; i<limitLv+1;i++)
				for(int i=1; i<limitLv+1;i++){
					String param = "lv" + i;
					FieldInfo f = t.GetField(param);
					int sts = (int)f.GetValue(stslst);
					sts = sts + pa_hp / 2;
					statusByLv.Add(sts);
				}


				lvSlider.GetComponent<LvSlider>().moneyList = requredMoneyByLv;
				lvSlider.GetComponent<LvSlider>().statusList = statusByLv;
				lvSlider.GetComponent<LvSlider>().pa_hp = pa_hp;

				//Initial Setting
				GameObject.Find ("RequiredMoneyValue").GetComponent<Text>().text = requredMoneyByLv[ch_lv].ToString();
				GameObject.Find ("PopHpValueUp").GetComponent<Text>().text = ((statusByLv[ch_lv] - pa_hp/2)*10).ToString();
				GameObject.Find ("PopAtkValueUp").GetComponent<Text>().text = statusByLv[ch_lv].ToString();
				GameObject.Find ("PopDfcValueUp").GetComponent<Text>().text = statusByLv[ch_lv].ToString();
			
			}else{

				//Cannot Level up Case

				//disable slider
				lvSlider.value = ch_lv;
				GameObject.Find ("KunrenSlider").GetComponent<Slider>().enabled = false;

				//Money
				Color shortageColor = new Color (203f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
				Color greyColor = new Color (0f / 255f, 0f / 255f, 0f / 255f, 121f / 255f);
				GameObject rMoney = GameObject.Find ("RequiredMoneyValue");
				rMoney.GetComponent<Text>().text = totalMoney.ToString();
				rMoney.GetComponent<Text>().color = shortageColor;

				//Lv
				GameObject.Find ("LvTo").GetComponent<Text>().color = greyColor;

				//Next Lv Statu
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

				int nextLv = ch_lv + 1;
				String param = "lv" + nextLv.ToString();
				FieldInfo f = t.GetField(param);
				int sts = (int)f.GetValue(stslst);
				sts = sts + pa_hp / 2;

				GameObject hp = GameObject.Find ("PopHpValueUp");
				GameObject atk = GameObject.Find ("PopAtkValueUp");
				GameObject dfc = GameObject.Find ("PopDfcValueUp");
				hp.GetComponent<Text>().text = ((sts - pa_hp/2)*10).ToString();
				hp.GetComponent<Text>().color = greyColor;
				atk.GetComponent<Text>().text = sts.ToString();
				atk.GetComponent<Text>().color = greyColor;
				dfc.GetComponent<Text>().text = sts.ToString();
				dfc.GetComponent<Text>().color = greyColor;
				GameObject.Find ("GiveKunren").GetComponent<DoKunren>().moneyOK = false;

			}
		}

	}

	public GameObject commonPopup(int qaId){
		//Common Process
		//Back Cover
		string backPath = "Prefabs/Busyo/back";
		GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
		back.transform.SetParent(GameObject.Find ("Panel").transform);
		back.transform.localScale = new Vector2 (1, 1);
		RectTransform backTransform = back.GetComponent<RectTransform> ();
		backTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
		
		//Popup Screen
		string popupPath = "Prefabs/Busyo/board";
		GameObject popup = Instantiate (Resources.Load (popupPath)) as GameObject;
		popup.transform.SetParent(GameObject.Find ("Panel").transform);
		popup.transform.localScale = new Vector2 (1, 1);
		RectTransform popupTransform = popup.GetComponent<RectTransform> ();
		popupTransform.anchoredPosition3D = new Vector3 (0, 0, 0);

        //qa
        string qaPath = "Prefabs/Common/Question";
        GameObject qa = Instantiate(Resources.Load(qaPath)) as GameObject;
        qa.transform.SetParent(popup.transform);
        qa.transform.localScale = new Vector2(1, 1);
        RectTransform qaTransform = qa.GetComponent<RectTransform>();
        qaTransform.anchoredPosition = new Vector3(-540, 285, 0);
        qa.name = "qa";
        qa.GetComponent<QA>().qaId = qaId;

        //Pop text
        string popTextPath = "Prefabs/Busyo/popText";
		GameObject popText = Instantiate (Resources.Load (popTextPath)) as GameObject;
		popText.transform.SetParent(GameObject.Find ("board(Clone)").transform);
		popText.transform.localScale = new Vector2 (0.35f, 0.35f);
		RectTransform popTextTransform = popText.GetComponent<RectTransform> ();
		popTextTransform.anchoredPosition3D = new Vector3 (0, 260, 0);
		popText.name = "popText";

        //tutorial
        if (Application.loadedLevelName == "tutorialBusyo") {
            Destroy(popup.transform.Find("close").gameObject);                     
        }



        return popup;

    }


	public void cyouheiView(string ch_type, int langId) {
        Message Message = new Message();
        GameObject.Find("popText").GetComponent<Text>().text = Message.getMessage(214,langId);        
		//Cyouhei
		string chigyouPath = "Prefabs/Busyo/Cyouhei";
		GameObject chigyo = Instantiate (Resources.Load (chigyouPath)) as GameObject;
		chigyo.transform.SetParent(GameObject.Find ("board(Clone)").transform);
		chigyo.transform.localScale = new Vector2 (1, 1);
		RectTransform chigyoTransform = chigyo.GetComponent<RectTransform> ();
		chigyoTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
		chigyo.name = "Cyouhei";
		
		//Butai Name
		GameObject.Find ("PopTextButaiName").GetComponent<Text>().text = busyoName + " " + ch_heisyu;
		string chPath = "Prefabs/Player/Unit/" + ch_type;
		GameObject chObj = Instantiate (Resources.Load (chPath)) as GameObject;
		chObj.transform.SetParent(GameObject.Find ("Cyouhei").transform);
		chObj.transform.localScale = new Vector2 (8, 8);
		
		RectTransform chTransform = chObj.GetComponent<RectTransform> ();
		chTransform.anchoredPosition3D = new Vector3 (-260, 0, 0);
		//Butai Status
		GameObject.Find ("PopHpValue").GetComponent<Text>().text = ch_hp.ToString();
		GameObject.Find ("PopAtkValue").GetComponent<Text>().text = ch_status.ToString();
		GameObject.Find ("PopDfcValue").GetComponent<Text>().text = ch_status.ToString();
		GameObject.Find ("PopTextNumFrom").GetComponent<Text>().text = ch_num.ToString();
		GameObject.Find ("PopTextNumTo").GetComponent<Text>().text = (ch_num + 1).ToString();
		
		//Required Item
		Entity_cyouhei_mst Mst = Resources.Load ("Data/cyouhei_mst") as Entity_cyouhei_mst;
		string itemType = Mst.param [ch_num].requiredItemTyp;
        int itemQty = 0;
        int money = 0;
        if (Application.loadedLevelName != "tutorialBusyo") {
            itemQty = Mst.param [ch_num].requiredItemQty;
		    money = Mst.param [ch_num].requiredMoney;
        }else {
            Destroy(transform.Find("point_up").gameObject);
            TutorialController tutorialScript = new TutorialController();
            Vector2 vect = new Vector2(0, 50);
            GameObject btn = GameObject.Find("DoCyouheiButton").gameObject;
            GameObject animObj = tutorialScript.SetPointer(btn, vect);
            animObj.transform.localScale = new Vector2(100, 100);
        }
        GameObject.Find("RequiredChigyouValue").GetComponent<Text>().text = "/" + itemQty;
        GameObject.Find("RequiredMoneyValue").GetComponent<Text>().text = money.ToString();

        //Item Icon Setting
        //Low:5657FFFF
        //Mid:EC5D5DFF
        //High:CFE85FFF
        GameObject popBack = GameObject.Find ("PopBack").gameObject;
		foreach(Transform obj in popBack.transform){
			if (obj.tag == "Kahou") {
				Destroy (obj.gameObject);
			}
		}

		string target = "Cyouhei" + ch_type;
		string itemPath = "Prefabs/Item/Cyouhei/" + target;
		GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
		item.transform.SetParent(popBack.transform);
		item.transform.localScale = new Vector2 (0.8f, 0.8f);
		RectTransform itemTransform = item.GetComponent<RectTransform> ();
		itemTransform.anchoredPosition3D = new Vector3 (-100, -50, 0);
		itemTransform.sizeDelta = new Vector2( 100, 100);
		item.name = target;
		
		//Change Color & Rank , Current Item QTY
		string requiredItemTyp = Mst.param [ch_num].requiredItemTyp;
		string itemColumn = "cyouhei" + ch_type;
		string itemString = PlayerPrefs.GetString (itemColumn);
		char[] delimiterChars = {','};
		string[] itemList = itemString.Split (delimiterChars);
		
		Color shortageColor = new Color (203f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
		GameObject DoCyouhei = GameObject.Find ("DoCyouheiButton");
		DoCyouhei.GetComponent<DoCyouhei>().itemType = requiredItemTyp;
		DoCyouhei.GetComponent<DoCyouhei>().ch_type = ch_type;

        
		if(requiredItemTyp=="low"){
			//Blue
			Color activeColor = new Color (86f / 255f, 87f / 255f, 255f / 255f, 255f / 255f);
			item.GetComponent<Image>().color = activeColor;            
            item.transform.Find("CyouheiRank").GetComponent<Text>().text = Message.getMessage(181,langId);
            
            if (Application.loadedLevelName == "tutorialBusyo") {
                GameObject.Find ("CurrentChigyouValue").GetComponent<Text>().text = 0.ToString();
            }else {
                GameObject.Find("CurrentChigyouValue").GetComponent<Text>().text = itemList[0];
            }
			DoCyouhei.GetComponent<DoCyouhei>().nowItem = int.Parse(itemList[0]);
			//Shortage
			if(int.Parse(itemList[0])<itemQty){
				GameObject.Find ("CurrentChigyouValue").GetComponent<Text>().color = shortageColor;
				DoCyouhei.GetComponent<DoCyouhei>().itemOK = false;
			}
			
		}else if(requiredItemTyp=="middle"){
			//Red
			Color activeColor = new Color (236f / 255f, 93f / 255f, 93f / 255f, 255f / 255f);
			item.GetComponent<Image>().color = activeColor;            
            item.transform.Find("CyouheiRank").GetComponent<Text>().text = Message.getMessage(182,langId);
            
			GameObject.Find ("CurrentChigyouValue").GetComponent<Text>().text = itemList[1];
			DoCyouhei.GetComponent<DoCyouhei>().nowItem = int.Parse(itemList[1]);
			//Shortage
			if(int.Parse(itemList[1])<itemQty){
				GameObject.Find ("CurrentChigyouValue").GetComponent<Text>().color = shortageColor;
				DoCyouhei.GetComponent<DoCyouhei>().itemOK = false;
			}
			
			
		}else if(requiredItemTyp=="hight"){
			//Gold
			Color activeColor = new Color (207f / 255f, 232f / 255f, 95f / 255f, 255f / 255f);
			item.GetComponent<Image>().color = activeColor;
            item.transform.Find("CyouheiRank").GetComponent<Text>().text = Message.getMessage(183,langId);
            
			GameObject.Find ("CurrentChigyouValue").GetComponent<Text>().text = itemList[2];
			DoCyouhei.GetComponent<DoCyouhei>().nowItem = int.Parse(itemList[2]);
			
			//Shortage
			if(int.Parse(itemList[2])<itemQty){
				GameObject.Find ("CurrentChigyouValue").GetComponent<Text>().color = shortageColor;
				DoCyouhei.GetComponent<DoCyouhei>().itemOK = false;
			}
		}
		
		
		//Money Check
		int myMoney = PlayerPrefs.GetInt ("money");

		if(money>myMoney){
			GameObject.Find ("RequiredMoneyValue").GetComponent<Text>().color = shortageColor;
			GameObject.Find ("DoCyouheiButton").GetComponent<DoCyouhei>().moneyOK = false;
		}
		
		//Button Setting(OK,NG)
		DoCyouhei.GetComponent<DoCyouhei>().requiredItem = itemQty;
		DoCyouhei.GetComponent<DoCyouhei>().requiredMoney = money;
		DoCyouhei.GetComponent<DoCyouhei>().nowMoney = myMoney;

    }
	
	
}
