using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System.Collections.Generic;

public class RonkouScene : MonoBehaviour {

    public GameObject firstSlot = null;
	public Color OKClorBtn = new Color (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
	public Color OKClorTxt = new Color (255f / 255f, 255f / 255f, 255f / 255f, 200f / 255f);
	public Color NGClorBtn = new Color (133 / 255f, 133 / 255f, 80 / 255f, 255f / 255f);
	public Color NGClorTxt = new Color (90 / 255f, 90 / 255f, 40 / 255f, 255f / 200f);

	public void OnClick () {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

		TabHandler tab = new TabHandler ();
		tab.tabButtonColor ("Ronkou");

		List<string> myBusyo_list = new List<string>();

		GameObject mainController = GameObject.Find ("GameScene");
		mainController.GetComponent<NowOnButton> ().onButton = "Ronkou";
		string minBusyoId = "";

		/*Initial Setting*/
		if(mainController.GetComponent<NowOnButton> ().lastButton == ""){

			minBusyoId = createScrollView(myBusyo_list,minBusyoId,mainController,true);
				
		}

		if (mainController.GetComponent<NowOnButton> ().lastButton != "Ronkou") {
			/*CenterView*/
			//Delete Previous CenterView

			Destroy (GameObject.Find ("BusyoStatus"));
			Destroy (GameObject.Find ("SenpouStatus"));
			Destroy (GameObject.Find ("KahouStatus"));
			Destroy(GameObject.Find ("SyoguMenu"));

			string centerViewPath = "Prefabs/Busyo/BusyoStatus";
			GameObject center = Instantiate (Resources.Load (centerViewPath)) as GameObject;
			center.transform.SetParent(GameObject.Find ("CenterView").transform);
			RectTransform center_transform = center.GetComponent<RectTransform> ();
			center_transform.anchoredPosition3D = new Vector3 (240, 31, 0);
			center.transform.localScale = new Vector2 (1, 1);
			center.name = "BusyoStatus";

			/*Busyo View*/
			string busyoId = mainController.GetComponent<NowOnBusyo>().OnBusyo;

			//Initial
			if(mainController.GetComponent<NowOnButton> ().lastButton == ""){
				busyoId = minBusyoId;
				createBusyoView(busyoId);
			}

            /*Centeral View*/ 
			createBusyoStatusView(busyoId);

			string text = GameObject.Find ("KahouAtkValue").GetComponent<Text> ().text;
			//Controller Setting
			mainController.GetComponent<NowOnButton> ().lastButton = "Ronkou";
		}
        if (mainController.GetComponent<NowOnButton>().lastButton == "") {
            firstSlot.GetComponent<BusyoView>().OnClick();
        }
    }

	public string createScrollView(List<string> myBusyo_list, string minBusyoId, GameObject mainController, bool initflg){
        //Scroll View
        string myBusyoString = "";       
        BusyoInfoGet busyoScript = new BusyoInfoGet();
        bool tutorialDoneFlg = PlayerPrefs.GetBool("tutorialDoneFlg");
        if (!tutorialDoneFlg || Application.loadedLevelName != "tutorialBusyo") {
            myBusyoString = PlayerPrefs.GetString ("myBusyo");
        }else {
            //retry tutorial
            myBusyoString = "19," + PlayerPrefs.GetInt("tutorialBusyo").ToString(); ;
        }
		char[] delimiterChars = {','};
		myBusyo_list.AddRange (myBusyoString.Split (delimiterChars));

        //Sort by Jinkei
        List<string> myBusyoList = new List<string>();
        List<string> jinkeiTrueBusyoList = new List<string>();
        if (!tutorialDoneFlg || Application.loadedLevelName != "tutorialBusyo") {            
            //remove dup
            List<string> nodup = new List<string>();
            string newMyBusyo = "";
            for (int i = 0; i < myBusyo_list.Count; i++) {
                string busyo = myBusyo_list[i];
                if (!nodup.Contains(busyo)) {
                    nodup.Add(busyo);

                    if (newMyBusyo == "") {
                        newMyBusyo = busyo;
                    }else {
                        newMyBusyo = newMyBusyo + "," + busyo;
                    }
                }
            }
            if (myBusyoString != newMyBusyo) PlayerPrefs.SetString("myBusyo", newMyBusyo);
           
            List<string> jinkeiFalseBusyoList = new List<string>();
            for (int i=0; i< nodup.Count; i++) {
                int busyoId = int.Parse(nodup[i]);
                bool jinkeiFlg = jinkeiBusyoCheck(busyoId);
                if(jinkeiFlg) {
                    jinkeiTrueBusyoList.Add(busyoId.ToString());
                }else {
                    jinkeiFalseBusyoList.Add(busyoId.ToString());
                }
            }
        
            myBusyoList.AddRange(jinkeiTrueBusyoList);           

            //Sort by Rank
            List<string> sList = new List<string>();
            List<string> aList = new List<string>();
            List<string> bList = new List<string>();
            List<string> cList = new List<string>();
            foreach (string busyoIdString in jinkeiFalseBusyoList) {
                string rank = busyoScript.getRank(int.Parse(busyoIdString));
                if (rank == "S") {
                    sList.Add(busyoIdString);
                }else if (rank == "A") {
                    aList.Add(busyoIdString);
                }else if (rank == "B") {
                    bList.Add(busyoIdString);
                }else {
                    cList.Add(busyoIdString);
                }
            }
            jinkeiFalseBusyoList = new List<string>();
            jinkeiFalseBusyoList.AddRange(sList);
            jinkeiFalseBusyoList.AddRange(aList);
            jinkeiFalseBusyoList.AddRange(bList);
            jinkeiFalseBusyoList.AddRange(cList);            

            myBusyoList.AddRange(jinkeiFalseBusyoList);

            //Sort by DaimyoId & LV
            List<Busyo> baseBusyoList = new List<Busyo>();
            BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();
            foreach (string busyoIdString in jinkeiFalseBusyoList) {
                int busyoId = int.Parse(busyoIdString);
                string busyoNameSort = BusyoInfoGet.getName(busyoId);
                string rank = BusyoInfoGet.getRank(busyoId);
                string heisyu = BusyoInfoGet.getHeisyu(busyoId);
                int daimyoId = BusyoInfoGet.getDaimyoId(busyoId);
                int daimyoHst = BusyoInfoGet.getDaimyoHst(busyoId);
                if (daimyoId == 0) daimyoId = daimyoHst;
                int lv = PlayerPrefs.GetInt(busyoId.ToString());
                baseBusyoList.Add(new Busyo(busyoId, busyoNameSort, rank, heisyu, daimyoId, daimyoHst, lv));                
            }
            List<Busyo> myBusyoDaimyoSortListTmp = new List<Busyo>(baseBusyoList);
            myBusyoDaimyoSortListTmp.Sort((a, b) => {
                int result = a.daimyoId - b.daimyoId;
                return result != 0 ? result : b.lv - a.lv;
            });

            List<string> myBusyoDaimyoSortList = new List<string>(jinkeiTrueBusyoList);
            foreach(Busyo busyo in myBusyoDaimyoSortListTmp) {
                string busyoId = busyo.busyoId.ToString();
                myBusyoDaimyoSortList.Add(busyoId);
            }

            List<Busyo> myBusyoLvSortListTmp = new List<Busyo>(baseBusyoList);
            myBusyoLvSortListTmp.Sort((a, b) => {
                int result = b.lv - a.lv;
                return result != 0 ? result : a.daimyoId - a.daimyoId;
            });

            List<string> myBusyoLvSortList = new List<string>(jinkeiTrueBusyoList);
            foreach (Busyo busyo in myBusyoLvSortListTmp) {
                string busyoId = busyo.busyoId.ToString();
                myBusyoLvSortList.Add(busyoId);
            }


            //Set rank sort
            if(GameObject.Find("BusyoSortDropdown")) {
                BusyoSort BusyoSort = GameObject.Find("BusyoSortDropdown").GetComponent<BusyoSort>();
                BusyoSort.myBusyoRankSortList = myBusyoList;
                BusyoSort.myBusyoDaimyoSortList = myBusyoDaimyoSortList;
                BusyoSort.myBusyoLvSortList = myBusyoLvSortList;
                BusyoSort.jinkeiTrueBusyoList = jinkeiTrueBusyoList;
            }
        }
        else {
            //retry tutorial
            myBusyoList.AddRange(myBusyo_list);
        }

        //Instantiate scroll view
        string scrollPath = "Prefabs/Busyo/Slot";
        GameObject content = GameObject.Find("Content");
        for (int j=0; j< myBusyoList.Count; j++) {
			//Slot
			GameObject prefab = Instantiate (Resources.Load (scrollPath)) as GameObject;
			prefab.transform.SetParent(content.transform);
			prefab.transform.localScale = new Vector3 (1, 1, 1);
			prefab.transform.localPosition = new Vector3(330,-75,0);
            
			//Busyo
			string busyoPath ="Prefabs/Player/Unit/BusyoUnit";
			GameObject busyo = Instantiate (Resources.Load (busyoPath)) as GameObject;
			busyo.name = myBusyoList[j].ToString();
			busyo.transform.SetParent(prefab.transform);
			busyo.transform.localScale = new Vector3 (4, 4, 4);
			busyo.transform.localPosition = new Vector3(100,-75,0);
			prefab.name = "Slot" + busyo.name;
			
			busyo.GetComponent<DragHandler> ().enabled = false;
            
            //Default
            if (initflg) {
                if (j==0) {
                    firstSlot = prefab.gameObject;
                }
            }

            //kamon
            string KamonPath = "Prefabs/Jinkei/Kamon";
            GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
            kamon.transform.SetParent(busyo.transform);
            kamon.transform.localScale = new Vector2(0.1f, 0.1f);
            kamon.transform.localPosition = new Vector2(-15, -12);
            int daimyoId = busyoScript.getDaimyoId(int.Parse(busyo.name));
            if (daimyoId == 0) {
                daimyoId = busyoScript.getDaimyoHst(int.Parse(busyo.name));
            }
            string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
            kamon.GetComponent<Image>().sprite =
                Resources.Load(imagePath, typeof(Sprite)) as Sprite;

            //Heisyu
            string heisyu = busyoScript.getHeisyu(int.Parse(busyo.name));
            string heisyuPath = "Prefabs/Jinkei/" + heisyu;
            GameObject heisyuObj = Instantiate(Resources.Load(heisyuPath)) as GameObject;
            heisyuObj.transform.SetParent(busyo.transform, false);
            heisyuObj.transform.localPosition = new Vector2(10, -10);
            heisyuObj.transform.SetAsFirstSibling();

            //Jinkei Exist
            if(jinkeiTrueBusyoList.Contains(busyo.name)) {
                prefab.GetComponent<BusyoView>().jinkeiFlg = true;
            }


        }

        minBusyoId = myBusyoList[0].ToString();
		mainController.GetComponent<NowOnBusyo>().OnBusyo = myBusyoList[0].ToString();
        string busyoName = busyoScript.getName(int.Parse(minBusyoId));
        mainController.GetComponent<NowOnBusyo>().OnBusyoName = busyoName;

        //Busyo Qty Limit
        int stockLimit = PlayerPrefs.GetInt ("stockLimit");
        int addSpace = PlayerPrefs.GetInt("space");
        GameObject.Find ("LimitBusyoQtyValue").GetComponent<Text>().text = stockLimit.ToString () + "<Color=#35C748FF>+" + addSpace + "</Color>";
        GameObject.Find ("NowBusyoQtyValue").GetComponent<Text>().text = myBusyoList.Count.ToString ();

        //Scroll Position
        content.transform.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0.0f;

        return minBusyoId;
	}


	public void createBusyoView(string busyoId){

		string path = "Prefabs/Player/Unit/BusyoUnit";
		GameObject Busyo = Instantiate (Resources.Load (path)) as GameObject;
		Busyo.name = busyoId;

		Busyo.transform.SetParent (GameObject.Find ("BusyoView").transform);
		Busyo.transform.localScale = new Vector2 (4, 4);
		
		RectTransform rect_transform = Busyo.GetComponent<RectTransform>();
		rect_transform.anchoredPosition3D = new Vector3(300,200,0);
		rect_transform.sizeDelta = new Vector2( 100, 100);

		Busyo.GetComponent<DragHandler> ().enabled = false;

        //Ship Rank
        string shipPath = "Prefabs/Busyo/ShipSts";
        GameObject ShipObj = Instantiate(Resources.Load(shipPath)) as GameObject;
        ShipObj.transform.SetParent(Busyo.transform);
        preKaisen kaisenScript = new preKaisen();
        int shipId = kaisenScript.getShipSprite(ShipObj, int.Parse(busyoId));
        ShipObj.transform.localPosition = new Vector3(-40, -40, 0);
        ShipObj.transform.localScale = new Vector2(0.4f, 0.4f);
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            if (shipId == 1) {
                ShipObj.transform.FindChild("Text").GetComponent<Text>().text = "High";
            }else if (shipId == 2) {
                ShipObj.transform.FindChild("Text").GetComponent<Text>().text = "Mid";
            }else if (shipId == 3) {
                ShipObj.transform.FindChild("Text").GetComponent<Text>().text = "Low";
            }
        }else {
            if (shipId == 1) {
                ShipObj.transform.FindChild("Text").GetComponent<Text>().text = "上";
            }else if (shipId == 2) {
                ShipObj.transform.FindChild("Text").GetComponent<Text>().text = "中";
            }else if (shipId == 3) {
                ShipObj.transform.FindChild("Text").GetComponent<Text>().text = "下";
            }
        }
        //Text Modification
        GameObject text = Busyo.transform.FindChild ("Text").gameObject;
		text.GetComponent<Text> ().color = new Color(255,255,255,255);
		RectTransform text_transform = text.GetComponent<RectTransform>();
		text_transform.anchoredPosition3D = new Vector3 (-70,30,0);
		text_transform.sizeDelta = new Vector2( 630, 120);
		text.transform.localScale = new Vector2 (0.2f,0.2f);

        //Keep busyo name
        BusyoInfoGet busyoScript = new BusyoInfoGet();
        string busyoName = busyoScript.getName(int.Parse(busyoId));
        GameObject.Find("GameScene").GetComponent<NowOnBusyo>().OnBusyoName = busyoName;

        //Rank Text Modification
        GameObject rank = Busyo.transform.FindChild ("Rank").gameObject;
		RectTransform rank_transform = rank.GetComponent<RectTransform>();
		rank_transform.anchoredPosition3D = new Vector3 (20,-50,0);
		rank_transform.sizeDelta = new Vector2( 200, 200);
		rank.GetComponent<Text>().fontSize = 200;
		
	}

	public void createBusyoStatusView(string busyoId){
        bool tutorialDoneFlg = PlayerPrefs.GetBool("tutorialDoneFlg");
        int lv = 1;
        if (!tutorialDoneFlg || Application.loadedLevelName != "tutorialBusyo") {
            lv = PlayerPrefs.GetInt (busyoId);
        }

		StatusGet sts = new StatusGet ();
		int hp = sts.getHp (int.Parse (busyoId), lv);
		int atk = sts.getAtk (int.Parse (busyoId), lv);
		int dfc = sts.getDfc (int.Parse (busyoId), lv);
		int spd = sts.getSpd (int.Parse (busyoId), lv);
		
		int adjHp = hp * 100;
		int adjAtk = atk * 10;
		int adjDfc = dfc * 10;

        //add lv
        string addLvTmp = "addlv" + busyoId.ToString();
        if(PlayerPrefs.HasKey(addLvTmp)) {
            string addLvValue = "+" + PlayerPrefs.GetString(addLvTmp);
            GameObject.Find("addLvValue").GetComponent<Text>().text = addLvValue.ToString();
        }else {
            GameObject.Find("addLvValue").GetComponent<Text>().text = "";
        }
        int maxLv = 100 + PlayerPrefs.GetInt(addLvTmp);

        GameObject.Find ("LvValue").GetComponent<Text> ().text = lv.ToString ();
		GameObject.Find ("TosotsuValue").GetComponent<Text> ().text = adjHp.ToString ();
		GameObject.Find ("BuyuuValue").GetComponent<Text> ().text = adjAtk.ToString ();
		GameObject.Find ("ChiryakuValue").GetComponent<Text> ().text = adjDfc.ToString ();
		GameObject.Find ("SpeedValue").GetComponent<Text> ().text = spd.ToString ();
        NowOnBusyo NowOnBusyoScript = GameObject.Find("GameScene").GetComponent<NowOnBusyo>();
        NowOnBusyoScript.HP = adjHp;

        //Exp
        string expId = "exp" + busyoId.ToString ();
		string expString = "";
        int nowExp = 0;
        if (!tutorialDoneFlg || Application.loadedLevelName != "tutorialBusyo") {
            nowExp = PlayerPrefs.GetInt(expId);
        }
		Exp exp = new Exp ();
		int requiredExp = 0;
		if (lv != maxLv) {
			requiredExp = exp.getExpforNextLv (lv);
		} else {
			requiredExp = exp.getExpLvMax(maxLv);
		}

        int diff = requiredExp - nowExp;
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            GameObject.Find ("ExpValue").GetComponent<Text> ().text = "another " + diff.ToString();
        }else {
            GameObject.Find("ExpValue").GetComponent<Text>().text = "あと" + diff.ToString();
        }

        //Kahou status
        int totalBusyoHp = 0;
        int finalAtk = 0;
        int finalHp = 0;
        int finalDfc = 0;
        int finalSpd = 0;

        if (tutorialDoneFlg && Application.loadedLevelName != "tutorialBusyo") {
            KahouStatusGet kahouSts = new KahouStatusGet ();
		    string[] KahouStatusArray =kahouSts.getKahouForStatus (busyoId,adjHp,adjAtk,adjDfc,spd);
		
		    //Kanni
		    string kanniTmp = "kanni" + busyoId;
		    float addAtkByKanni = 0;
		    float addHpByKanni = 0;
		    float addDfcByKanni = 0;

		    if (PlayerPrefs.HasKey (kanniTmp)) {
			    int kanniId = PlayerPrefs.GetInt (kanniTmp);
                if(kanniId !=0) {
			        Kanni kanni = new Kanni ();
			        string kanniIkai = kanni.getIkai (kanniId);
			        string kanniName = kanni.getKanni (kanniId);
			        GameObject.Find ("StatusKanni").transform.FindChild ("Value").GetComponent<Text> ().text = kanniIkai + "\n" + kanniName;

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
                }else {
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        GameObject.Find("StatusKanni").transform.FindChild("Value").GetComponent<Text>().text = "No Rank";
                    }else {
                        GameObject.Find("StatusKanni").transform.FindChild("Value").GetComponent<Text>().text = "官位無し";
                    }
                }
            } else {
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    GameObject.Find("StatusKanni").transform.FindChild("Value").GetComponent<Text>().text = "No Rank";
                }else {
                    GameObject.Find("StatusKanni").transform.FindChild("Value").GetComponent<Text>().text = "官位無し";
                }
            }

		    //Jyosyu
		    string jyosyuTmp = "jyosyuBusyo" + busyoId;
		    if (PlayerPrefs.HasKey (jyosyuTmp)) {
			    int kuniId = PlayerPrefs.GetInt(jyosyuTmp);
			    KuniInfo kuni = new KuniInfo();
			    string kuniName = kuni.getKuniName(kuniId);

                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    GameObject.Find("StatusJyosyu").transform.FindChild("Value").GetComponent<Text>().text = kuniName + "\nLord";
                }
                else {
                    GameObject.Find("StatusJyosyu").transform.FindChild("Value").GetComponent<Text>().text = kuniName + "\n城主";
                }

            } else {
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    GameObject.Find("StatusJyosyu").transform.FindChild("Value").GetComponent<Text>().text = "No Feud";
                }
                else {
                    GameObject.Find("StatusJyosyu").transform.FindChild("Value").GetComponent<Text>().text = "城無し";
                }
            }
            
		    //Show Additional Status
		    finalAtk = int.Parse (KahouStatusArray [0]) + Mathf.FloorToInt (addAtkByKanni);
		    finalHp = int.Parse (KahouStatusArray [1]) + Mathf.FloorToInt (addHpByKanni);
		    finalDfc= int.Parse (KahouStatusArray [2]) + Mathf.FloorToInt (addDfcByKanni);
		    finalSpd = int.Parse (KahouStatusArray [3]);

		    GameObject.Find ("KahouAtkValue").GetComponent<Text> ().text = "+" + finalAtk.ToString ();
		    GameObject.Find ("KahouHpValue").GetComponent<Text>().text = "+" + finalHp.ToString();
		    totalBusyoHp = adjHp + finalHp;
		    GameObject.Find ("KahouDfcValue").GetComponent<Text>().text = "+" + finalDfc.ToString();
		    GameObject.Find ("KahouSpdValue").GetComponent<Text>().text = "+" + finalSpd.ToString();
        }else {


            if (Application.systemLanguage != SystemLanguage.Japanese) {
                GameObject.Find("StatusKanni").transform.FindChild("Value").GetComponent<Text>().text = "No Rank";
            }else {
                GameObject.Find("StatusKanni").transform.FindChild("Value").GetComponent<Text>().text = "官位無し";
            }
            
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                GameObject.Find("StatusJyosyu").transform.FindChild("Value").GetComponent<Text>().text = "No Feud";
            }else {
                GameObject.Find("StatusJyosyu").transform.FindChild("Value").GetComponent<Text>().text = "城無し";
            }

        }

		//Butai Status
		string heiId = "hei" + busyoId.ToString ();
        string chParam = "";
        if (!tutorialDoneFlg || Application.loadedLevelName != "tutorialBusyo") {
            chParam = PlayerPrefs.GetString (heiId, "0");
        }else {
            //retry tutorial
            chParam = "TP:1:1:1";
        }
        StatusGet statusScript = new StatusGet();
        string chParamHeisyu = statusScript.getHeisyu(int.Parse(busyoId));
        if (chParam == "0" || chParam == "") {            
            chParam = chParamHeisyu + ":1:1:1";
            PlayerPrefs.SetString(heiId, chParam);
            PlayerPrefs.Flush();
        }
        
        char[] delimiterChars = {':'};
		string[] ch_list = chParam.Split (delimiterChars);

        //string ch_type = ch_list [0];
        string ch_type = chParamHeisyu;
        int ch_num = int.Parse (ch_list [1]);
        bool updateParam = false;
        if (ch_num > 20) {
            ch_num = 20;
            updateParam = true;
        }
		int ch_lv = int.Parse (ch_list [2]);
        if (ch_lv > 100) {
            ch_lv = 100;
            updateParam = true;
        }
        float ch_status = float.Parse (ch_list [3]);

        if(updateParam) {
            PlayerPrefs.SetString(heiId, ch_type + ":" + ch_num.ToString() + ":" + ch_lv.ToString() + ":" + ch_status.ToString());
        }

        string heisyu = "";
        Message msg = new Message();
        if (ch_type == "KB") {
            heisyu = msg.getMessage(55);
        }else if (ch_type == "YR") {
            heisyu = msg.getMessage(56);
        }else if (ch_type == "TP") {
            heisyu = msg.getMessage(57);
        }else if (ch_type == "YM") {
            heisyu = msg.getMessage(58);
        }

        GameObject.Find ("ChildNameValue").GetComponent<Text> ().text = heisyu;
		GameObject.Find ("ChildQtyValue").GetComponent<Text> ().text = ch_num.ToString ();
		GameObject.Find ("ChildLvValue").GetComponent<Text> ().text = ch_lv.ToString ();

		//Jyosyu Handling
		JyosyuHeiryoku jyosyuHei = new JyosyuHeiryoku ();
		float addHei = (float)jyosyuHei.GetJyosyuHeiryoku (busyoId);
		float hei = ch_status * 10;
        GameObject.Find ("ChildHeiryokuValue").GetComponent<Text> ().text = hei.ToString();
        float newHei = finalHp + addHei;
        GameObject.Find("KahouHpValue").GetComponent<Text>().text = "+" + newHei.ToString();


        int chAtkDfc = (int)sts.getChAtkDfc ((int)hei, totalBusyoHp);
		string chAtkDfcString = chAtkDfc.ToString () + "/" + chAtkDfc.ToString (); 
		GameObject.Find ("ChildStatusValue").GetComponent<Text> ().text = chAtkDfcString;
		
		//Child Image
		foreach (Transform n in GameObject.Find ("Img").transform) {
			GameObject.Destroy (n.gameObject);
		}
		string chPath = "Prefabs/Player/Unit/" + ch_type;
		GameObject chObj = Instantiate (Resources.Load (chPath)) as GameObject;
		chObj.transform.SetParent(GameObject.Find ("Img").transform);
		RectTransform chTransform = chObj.GetComponent<RectTransform> ();
		chTransform.anchoredPosition3D = new Vector3 (-200, -50, 0);
		chTransform.sizeDelta = new Vector2 (40, 40);
		chObj.transform.localScale = new Vector2 (4, 4);



		GameObject chigyo = GameObject.Find ("ButtonCyouhei");
		if (ch_num < 20) {
			chigyo.GetComponent<Image> ().color = OKClorBtn;
			chigyo.transform.FindChild ("Text").GetComponent<Text> ().color = OKClorTxt;
			chigyo.GetComponent<Button>().enabled = true;

			chigyo.GetComponent<BusyoStatusButton> ().ch_type = ch_type;
			chigyo.GetComponent<BusyoStatusButton> ().ch_heisyu = heisyu;
			chigyo.GetComponent<BusyoStatusButton> ().ch_num = ch_num;
			chigyo.GetComponent<BusyoStatusButton> ().ch_status = chAtkDfc;
			chigyo.GetComponent<BusyoStatusButton> ().ch_hp = hei;
			chigyo.GetComponent<BusyoStatusButton> ().pa_hp = totalBusyoHp / 100;
		} else {
			//MAX
			chigyo.GetComponent<Image> ().color = NGClorBtn;
			chigyo.transform.FindChild ("Text").GetComponent<Text> ().color = NGClorTxt;
			chigyo.GetComponent<Button>().enabled = false;
		}
		GameObject kunren = GameObject.Find ("ButtonKunren");
		if (ch_lv < 100) {
			kunren.GetComponent<Image> ().color = OKClorBtn;
			kunren.transform.FindChild ("Text").GetComponent<Text> ().color = OKClorTxt;
			kunren.GetComponent<Button>().enabled = true;

			kunren.GetComponent<BusyoStatusButton> ().ch_type = ch_type;
			kunren.GetComponent<BusyoStatusButton> ().ch_heisyu = heisyu;
			kunren.GetComponent<BusyoStatusButton> ().ch_lv = ch_lv;
			kunren.GetComponent<BusyoStatusButton> ().ch_status = chAtkDfc;
			kunren.GetComponent<BusyoStatusButton> ().ch_hp = hei;
			kunren.GetComponent<BusyoStatusButton> ().ch_num = ch_num;
			kunren.GetComponent<BusyoStatusButton> ().pa_hp = totalBusyoHp / 100;
		} else {
			//MAX
			kunren.GetComponent<Image> ().color = NGClorBtn;
			kunren.transform.FindChild ("Text").GetComponent<Text> ().color = NGClorTxt;
			kunren.GetComponent<Button>().enabled = false;
		}
		//Parametor Setting
		GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyo = busyoId;

        //Jinkei Flg
        GameObject BusyoView = GameObject.Find("BusyoView");
        if (!BusyoView.transform.FindChild("jinkei")) {
            if (jinkeiBusyoCheck(int.Parse(busyoId))) {
                string iconPath = "Prefabs/Busyo/Jinkei";
                GameObject jinkei = Instantiate(Resources.Load(iconPath)) as GameObject;
                jinkei.transform.SetParent(GameObject.Find("BusyoView").transform);
                jinkei.transform.localScale = new Vector2(0.3f, 0.3f);
                jinkei.transform.localPosition = new Vector2(220, 200);
                jinkei.name = "jinkei";
            }
        }
    }

    public bool jinkeiBusyoCheck(int tsuihouBusyoId) {
        bool jinkeiBusyoFlg = false;

        int jinkei = PlayerPrefs.GetInt("jinkei");
        List<int> slotList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };

        for (int i = 0; i < slotList.Count; i++) {
            string slotId = slotList[i].ToString();
            string mapId = jinkei.ToString() + "map" + slotId;
            if (jinkei == 1) {
                if (slotId == "1" || slotId == "2" || slotId == "7" || slotId == "8" ||
                    slotId == "11" || slotId == "12" || slotId == "13" || slotId == "14" ||
                   slotId == "17" || slotId == "18" || slotId == "21" || slotId == "22") {
                    int busyoId = PlayerPrefs.GetInt(mapId);
                    if (busyoId == tsuihouBusyoId) {
                        jinkeiBusyoFlg = true;
                        break;
                    }
                }
            }
            else if (jinkei == 2) {
                if (slotId == "3" || slotId == "4" || slotId == "5" || slotId == "7" ||
                  slotId == "8" || slotId == "11" || slotId == "12" || slotId == "17" ||
                   slotId == "18" || slotId == "23" || slotId == "24" || slotId == "25") {
                    int busyoId = PlayerPrefs.GetInt(mapId);
                    if (busyoId == tsuihouBusyoId) {
                        jinkeiBusyoFlg = true;
                        break;
                    }
                }
            }
            else if (jinkei == 3) {
                if (slotId == "3" || slotId == "7" || slotId == "8" || slotId == "9" ||
                   slotId == "11" || slotId == "12" || slotId == "14" || slotId == "15" ||
                  slotId == "16" || slotId == "20" || slotId == "21" || slotId == "25") {
                    int busyoId = PlayerPrefs.GetInt(mapId);
                    if (busyoId == tsuihouBusyoId) {
                        jinkeiBusyoFlg = true;
                        break;
                    }
                }
            }
            else if (jinkei == 4) {
                if (slotId == "1" || slotId == "2" || slotId == "7" || slotId == "8" ||
                   slotId == "12" || slotId == "13" || slotId == "14" || slotId == "18" ||
                   slotId == "19" || slotId == "20" || slotId == "24" || slotId == "25") {
                    int busyoId = PlayerPrefs.GetInt(mapId);
                    if (busyoId == tsuihouBusyoId) {
                        jinkeiBusyoFlg = true;
                        break;
                    }
                }
            }
        }

        return jinkeiBusyoFlg;
    }
}
