using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class Gunzei : MonoBehaviour {

	public string key = "";
	public int srcKuni = 0;
	public int dstKuni = 0;
	public int srcDaimyoId = 0;
	public string srcDaimyoName = "";
	public int dstDaimyoId = 0;
	public string dstDaimyoName = "";
	public int myHei = 0;
	public bool leftFlg = false;
	public double starttime = 300;
	public double spantime = 0;
	public float timer = 0;
	public int myDaimyoId = 0;
	public bool atkFlg;
    public GameObject kuniIconView;
	public bool dstEngunFlg = false;
	public string dstEngunDaimyoId = ""; //2:3:5
	public string dstEngunHei = "";
	public string dstEngunSts = ""; //BusyoId-BusyoLv-ButaiQty-ButaiLv: ....
	public AudioSource[] audioSources;
    public bool stopFlg = false;

    void Start () {
		myDaimyoId = PlayerPrefs.GetInt("myDaimyo");
		double calctime = starttime - spantime; 
		kuniIconView = GameObject.Find ("KuniIconView");


		if (calctime <= 0) {
			if(!atkFlg){
				attack();
				atkFlg = true;
			}
		} else {
			timer = (float)calctime;
			//random time
			timer = UnityEngine.Random.Range(timer/2,timer);
		}




	}

	void Update () {

        if(!stopFlg) {
		    timer -= Time.deltaTime;

		    if (timer < 0.0f) {
			    if(!atkFlg){
				    attack();
				    atkFlg = true;
			    }
		    }
        }
    }

	public void attack(){
		audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		//Same Daimyo Check
		int latestDaimyoId = kuniIconView.transform.Find (dstKuni.ToString ()).GetComponent<SendParam> ().daimyoId;
		GameObject MsgBack = this.transform.Find ("MsgBack").gameObject;
		GameObject MsgText = MsgBack.transform.Find ("MsgText").gameObject;

		if (dstDaimyoId == latestDaimyoId) {
		
			if (dstDaimyoId != myDaimyoId) {
				audioSources [7].Play ();

				int enemyHei = heiryokuCalc (dstKuni);

				//Engun
				int engunTotalHei = 0;
				if(dstEngunFlg){
					char[] delimiterChars2 = {':'};
					List<string> engunHeiList = new List<string>();
					engunHeiList = new List<string> (dstEngunHei.Split (delimiterChars2));

					for(int k=0; k<engunHeiList.Count; k++){
						engunTotalHei = engunTotalHei + int.Parse(engunHeiList[k]);
					}
				}

				enemyHei = enemyHei + engunTotalHei;

				int ratio = 0;
				if ((myHei + enemyHei) != 0) {
					ratio = 100 * myHei / (myHei + enemyHei);
					if (ratio < 1) {
						ratio = 1;
					}	
				}

				MainEventHandler kassenEvent = new MainEventHandler ();
				bool winFlg = kassenEvent.CheckByProbability (ratio);
                int langId = PlayerPrefs.GetInt("langId");

                //Entity_kuni_mst kuniMst = Resources.Load ("Data/kuni_mst") as Entity_kuni_mst;
                KuniInfo kuniScript = new KuniInfo();
                string dstKuniName = kuniScript.getKuniName(dstKuni,langId);

				//Gaikou
				Gaikou gaikou = new Gaikou();
				gaikou.downGaikouByAttack(srcDaimyoId,dstDaimyoId);

				if (winFlg) {
					bool noGunzeiFlg = false;
                    string syouhai = "";                   
                    if (langId == 2) {
                        syouhai = srcDaimyoName + "\n" + "Conquered " + dstKuniName;
                    }else {
                        syouhai = srcDaimyoName + "\n" + dstKuniName + "を攻略";
                    }
					MsgBack.GetComponent<Image> ().enabled = true;
					MsgText.GetComponent<Text> ().enabled = true;
					MsgText.GetComponent<Text> ().text = syouhai;
					
					win (key, srcDaimyoId, dstDaimyoId, noGunzeiFlg, dstKuni);

                    fire(dstKuni);

                } else {
                    string syouhai = "";
                    if (langId == 2) {
                        syouhai = dstDaimyoName + "\n" + "Defended " + dstKuniName;
                    }else {
                        syouhai = dstDaimyoName + "\n" + dstKuniName + "を防衛";
                    }
                    MsgBack.GetComponent<Image> ().enabled = true;
					MsgText.GetComponent<Text> ().enabled = true;
					MsgText.GetComponent<Text> ().text = syouhai;
					
					gameObject.GetComponent<GunzeiFadeOut> ().enabled = true;

					MainStageController main = new MainStageController ();
					main.deleteKeyHistory (key);
				}
			
			} else {
				MyDaimyoWasAttacked atked = new MyDaimyoWasAttacked ();
				atked.wasAttacked (key, srcKuni, dstKuni, srcDaimyoId, dstDaimyoId, dstEngunFlg, dstEngunDaimyoId, dstEngunSts);
			}
		} else {
			audioSources [1].Play ();
            string syouhai = "";
            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                syouhai = srcDaimyoName + " withdrawed";
            }else {
                syouhai = srcDaimyoName + "撤退";
            }
            MsgBack.GetComponent<Image> ().enabled = true;
			MsgText.GetComponent<Text> ().enabled = true;
			MsgText.GetComponent<Text> ().text = syouhai;
			
			gameObject.GetComponent<GunzeiFadeOut> ().enabled = true;
			
			MainStageController main = new MainStageController ();
			main.deleteKeyHistory (key);
		}
	}
	
	
	public int heiryokuCalc(int kuni){
		GameObject targetKuni = GameObject.Find ("KuniIconView");
		int enemyTotalHei = 0;
		if (targetKuni.transform.Find (kuni.ToString ())) {
			SendParam sendParam = targetKuni.transform.Find (kuni.ToString ()).GetComponent<SendParam> ();
			int daimyoBusyoId = sendParam.daimyoBusyoId;
			int busyoLv = sendParam.busyoLv;
			int busyoQty = sendParam.busyoQty;
			int butaiLv = sendParam.butaiLv;
			int butaiQty = sendParam.butaiQty;


			if (daimyoBusyoId != 0) {
				StatusGet sts = new StatusGet ();
				int hp = sts.getHp (daimyoBusyoId, busyoLv);
				int hpResult = hp * 100 * busyoQty;
				string type = sts.getHeisyu (daimyoBusyoId);
				int chHp = sts.getChHp (type, butaiLv, hp);
				chHp = chHp * butaiQty * busyoQty * 10;
				enemyTotalHei = hpResult + chHp;
			}
		}
		return enemyTotalHei;
	}


	public void win(string tKey, int tSrcDaimyoId, int tDstDaimyoId, bool noGunzeiFlg, int dstKuni) {
        int langId = PlayerPrefs.GetInt("langId");
        int senarioId = PlayerPrefs.GetInt("senarioId");
        int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");

		//Kuni Change
		string seiryoku = PlayerPrefs.GetString ("seiryoku");
		char[] delimiterChars = {','};
		List<string> seiryokuList = new List<string>();
		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));

		List<string> srcDstKuniList = new List<string> ();
		char[] keyDelimiterChars = {'-'};
		srcDstKuniList = new List<string> (tKey.Split (keyDelimiterChars));

		seiryokuList[int.Parse(srcDstKuniList[1])-1] = tSrcDaimyoId.ToString();
		string newSeiryoku = "";
		for(int i=0; i<seiryokuList.Count; i++){
			if(i==0){
				newSeiryoku = seiryokuList[i];
			}else{
				newSeiryoku = newSeiryoku + "," + seiryokuList[i];
			}
		}
		PlayerPrefs.SetString("seiryoku",newSeiryoku);
		List<string> newSeiryokuList = new List<string>();
		newSeiryokuList = new List<string> (newSeiryoku.Split (delimiterChars));

        //Delete Enemy Gunzei
        HPCounter dltScript = new HPCounter();
        dltScript.deleteEnemyGunzeiData(int.Parse(srcDstKuniList[1]));

        //Delete Stage Cleared
        string clearedString = "kuni" + srcDstKuniList[1];
        PlayerPrefs.DeleteKey(clearedString);

        if (noGunzeiFlg==true) {
			//Metsubou Check
			if(!newSeiryokuList.Contains(tDstDaimyoId.ToString())){
				//Metsubou
				string metsubou =PlayerPrefs.GetString("metsubou");
				if(metsubou == null || metsubou == ""){
					metsubou =  tSrcDaimyoId.ToString() + ":" + tDstDaimyoId.ToString();
				}else{
					metsubou = metsubou + "," + tSrcDaimyoId.ToString() + ":" + tDstDaimyoId.ToString();
				}				
				PlayerPrefs.SetString("metsubou",metsubou);
			}

		} else {
			//Pattern of Has past
			gameObject.GetComponent<GunzeiFadeOut> ().enabled = true;
			
			//Metsubou Check
			if(!newSeiryokuList.Contains(tDstDaimyoId.ToString())){
				audioSources [4].Play ();
				audioSources [6].Play ();

				//Metsubou Message
				string pathOfBack = "Prefabs/Event/TouchEventBack";
				GameObject back = Instantiate (Resources.Load (pathOfBack)) as GameObject;
				back.transform.SetParent (GameObject.Find ("Panel").transform);
				back.transform.localScale = new Vector2 (1, 1);
				back.transform.localPosition = new Vector2 (0, 0);
				
				//make board
				string pathOfBoard = "Prefabs/Event/EventBoard";
				GameObject board = Instantiate (Resources.Load (pathOfBoard)) as GameObject;
				board.transform.SetParent (GameObject.Find ("Panel").transform);
				board.transform.localScale = new Vector2 (1, 1);

				back.GetComponent<CloseEventBoard> ().deleteObj = board;
				back.GetComponent<CloseEventBoard> ().deleteObj2 = back;
				board.transform.Find ("close").GetComponent<CloseEventBoard> ().deleteObj = board;
				board.transform.Find ("close").GetComponent<CloseEventBoard> ().deleteObj2 = back;

				string pathOfScroll = "Prefabs/Event/Metsubou";
				GameObject scroll = Instantiate (Resources.Load (pathOfScroll)) as GameObject;
				scroll.transform.SetParent (board.transform);
				scroll.transform.localScale = new Vector2 (1, 1);
				
				string pathOfSlot = "Prefabs/Event/MetsubouSlot";
				GameObject contents = scroll.transform.Find ("MetsubouScrollView/MetsubouContent").gameObject;
				GameObject slot = Instantiate (Resources.Load (pathOfSlot)) as GameObject;
				slot.transform.SetParent (contents.transform);
                Daimyo daimyoScript = new Daimyo();
                string srcDaimyoName = daimyoScript.getName(tSrcDaimyoId,langId,senarioId);
                string dstDaimyoName = daimyoScript.getName(tDstDaimyoId,langId,senarioId);
                string metsubouText = "";
                
                if (langId == 2) {
                    metsubouText = dstDaimyoName + " was destroyed completly by " + srcDaimyoName + ".";
                }else {
                    metsubouText = dstDaimyoName + "は" + srcDaimyoName + "に滅ぼされました";
                }


                slot.transform.Find ("MetsubouText").GetComponent<Text> ().text = metsubouText;
				slot.transform.localScale = new Vector2 (1, 1);

                //pointer
                back.GetComponent<CloseEventBoard>().metsubouKuniList.Add(dstKuni);
                board.transform.Find("close").GetComponent<CloseEventBoard>().metsubouKuniList.Add(dstKuni);
                back.GetComponent<CloseEventBoard>().activityUpdateFlg = true;
                board.transform.Find("close").GetComponent<CloseEventBoard>().activityUpdateFlg = true;

                //Metsubou Daimyo Handling
                string srcMetsubouTemp = "metsubou" + tSrcDaimyoId.ToString();
                string srcMetsubou = PlayerPrefs.GetString(srcMetsubouTemp);
                string dstMetsubouTemp = "metsubou" + tDstDaimyoId.ToString();
                string dstMetsubou = PlayerPrefs.GetString(dstMetsubouTemp);

                string newSrcMetsubou = "";
                if (srcMetsubou != null && srcMetsubou != "") {
                    newSrcMetsubou = srcMetsubou + "," + tDstDaimyoId.ToString();
                }else {
                    newSrcMetsubou = tDstDaimyoId.ToString();
                }
                if (dstMetsubou != null && dstMetsubou != "") {
                    newSrcMetsubou = newSrcMetsubou + "," + dstMetsubou;
                }
                PlayerPrefs.SetString(srcMetsubouTemp, newSrcMetsubou);


            }
		}
        
        string keyHistory = PlayerPrefs.GetString ("keyHistory");
		List<string> keyHistoryList = new List<string>();
		if (keyHistory != null && keyHistory != "") {
			if(keyHistory.Contains(",")){
				keyHistoryList = new List<string> (keyHistory.Split (delimiterChars));
			}else{
				keyHistoryList.Add(keyHistory);
			}
		}
		keyHistoryList.Remove(tKey);
		string newKeyHistory = "";
		for(int i=0; i<keyHistoryList.Count; i++){
			if(i==0){
				newKeyHistory = keyHistoryList[i];
			}else{
				newKeyHistory = newKeyHistory + "," + keyHistoryList[i];
			}
		}
		PlayerPrefs.SetString("keyHistory",newKeyHistory);
		PlayerPrefs.DeleteKey (tKey);		
		PlayerPrefs.Flush ();

		if (myDaimyo == tDstDaimyoId) {
			//Scene Change
			Application.LoadLevel("mainStage");

		} else {
			//Load Main Controler
			int tSrcKuni= int.Parse(srcDstKuniList[0]);
			int tDstKuni= int.Parse(srcDstKuniList[1]);

			changeKuniIconAndParam (tSrcKuni, tDstKuni, tSrcDaimyoId, tDstDaimyoId);
		}

	}

	public void changeKuniIconAndParam(int srcKuni, int dstKuni, int srcDaimyoId, int dstDaimyoId){
        int langId = PlayerPrefs.GetInt("langId");
        int senarioId = PlayerPrefs.GetInt("senarioId");
        //Change Icon
        string imagePath = "Prefabs/Kamon/" + srcDaimyoId.ToString ();
		GameObject kuniIconView = GameObject.Find ("KuniIconView");
		GameObject targetKuni = kuniIconView.transform.Find(dstKuni.ToString()).gameObject;

		targetKuni.GetComponent<Image> ().sprite = 
			Resources.Load (imagePath, typeof(Sprite)) as Sprite;	

		//Change Tochi Color
		GameObject KuniMap = GameObject.Find ("KuniMap");
        Daimyo daimyoScript = new Daimyo();
        float colorR = daimyoScript.getColorR(srcDaimyoId);
        float colorG = daimyoScript.getColorG(srcDaimyoId);
        float colorB = daimyoScript.getColorB(srcDaimyoId);
        Color kuniColor = new Color (colorR / 255f, colorG / 255f, colorB / 255f, 255f / 255f);
		KuniMap.transform.Find (dstKuni.ToString()).GetComponent<Image> ().color = kuniColor;

		//Change Yukoudo
		string tmp = "gaikou" + srcDaimyoId;
		int yukoudo = PlayerPrefs.GetInt (tmp);
		targetKuni.GetComponent<SendParam> ().myYukouValue = yukoudo;

		/*Change Param*/
		//Set Senryoku
		string seiryoku = PlayerPrefs.GetString ("seiryoku");
		List<string> seiryokuList = new List<string> ();
		char[] delimiterChars = {','};
		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));


		/*Get Winner Kuni Qty*/
		EnemySenryokuCalc calc = new EnemySenryokuCalc ();
		int myKuniQty = GameObject.Find ("GameController").GetComponent<MainStageController> ().myKuniQty;
		//int enemyKuniQty = kuniIconView.transform.FindChild (srcKuni.ToString ()).GetComponent<SendParam> ().kuniQty;
		//int newEnemyKuniQty = enemyKuniQty + 1;

		MainStageController main = new MainStageController ();

		for (int i = 0; i < seiryokuList.Count; i++) {
			int tmpDaimyoId = int.Parse(seiryokuList [i]);

			if(tmpDaimyoId == srcDaimyoId){
				List<string> checkedKuniList = new List<string> ();
				int tmpKuniId = i + 1;
				int newWinnerKuniQty = main.countLinkedKuniQty(1, tmpKuniId, tmpDaimyoId, seiryokuList, checkedKuniList);

				//Get New Senryoku
				int busyoQty = calc.EnemyBusyoQtyCalc (myKuniQty, newWinnerKuniQty,0);
                int senryokuRatio = daimyoScript.getSenryoku(srcDaimyoId);
                int busyoLv = calc.EnemyBusyoLvCalc (senryokuRatio);
				int butaiQty = calc.EnemyButaiQtyCalc (newWinnerKuniQty, myKuniQty);
				int butaiLv = calc.EnemyButaiLvCalc (senryokuRatio);

				//Change Name of target Kuni by daimyo info
				targetKuni.GetComponent<SendParam> ().daimyoId = srcDaimyoId;
                targetKuni.GetComponent<SendParam>().daimyoName = daimyoScript.getName(srcDaimyoId,langId,senarioId);//daimyoMst.param [srcDaimyoId - 1].daimyoName;
                targetKuni.GetComponent<SendParam>().daimyoBusyoId = daimyoScript.getDaimyoBusyoId(srcDaimyoId,senarioId);//daimyoMst.param [srcDaimyoId - 1].busyoId;

                SendParam winnerParam = kuniIconView.transform.Find (tmpKuniId.ToString ()).GetComponent<SendParam> ();
				winnerParam.busyoQty = busyoQty;
				winnerParam.busyoLv = busyoLv;
				winnerParam.butaiQty = butaiQty;
				winnerParam.butaiLv = butaiLv;
				winnerParam.kuniQty = newWinnerKuniQty;

			}else if(tmpDaimyoId == dstDaimyoId){
				List<string> checkedKuniList = new List<string> ();
				int tmpKuniId = i + 1;
				int newLoserKuniQty = main.countLinkedKuniQty(1, tmpKuniId, tmpDaimyoId, seiryokuList, checkedKuniList);

				//Get New Senryoku
				int busyoQty = calc.EnemyBusyoQtyCalc (myKuniQty, newLoserKuniQty,0);
                int senryokuRatio = daimyoScript.getSenryoku(dstDaimyoId);//daimyoMst.param [dstDaimyoId - 1].senryoku;
                int busyoLv = calc.EnemyBusyoLvCalc (senryokuRatio);
				int butaiQty = calc.EnemyButaiQtyCalc (newLoserKuniQty, myKuniQty);
				int butaiLv = calc.EnemyButaiLvCalc (senryokuRatio);

				SendParam loserParam = kuniIconView.transform.Find (tmpKuniId.ToString ()).GetComponent<SendParam> ();
				loserParam.busyoQty = busyoQty;
				loserParam.busyoLv = busyoLv;
				loserParam.butaiQty = butaiQty;
				loserParam.butaiLv = butaiLv;
				loserParam.kuniQty = newLoserKuniQty;
			}
		}







		//Icon Color Change by Doumei Situation
		List<string> myDoumeiList = new List<string> ();
		string doumeiString = PlayerPrefs.GetString ("doumei");
		if (doumeiString != null && doumeiString != "") {
			if (doumeiString.Contains (",")) {
				myDoumeiList = new List<string> (doumeiString.Split (delimiterChars));
			} else {
				myDoumeiList.Add (doumeiString);
			}
		}
		if (myDoumeiList.Contains (srcDaimyoId.ToString())) {
			Color doumeiColor = new Color (100f / 255f, 130f / 255f, 255f / 255f, 255f / 255f);
			targetKuni.GetComponent<SendParam> ().doumeiFlg = true;
			targetKuni.GetComponent<Image>().color = doumeiColor;

		}

		if (!myDoumeiList.Contains (srcDaimyoId.ToString()) && myDoumeiList.Contains (dstDaimyoId.ToString())) {
			Color unDoumeiColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
			targetKuni.GetComponent<SendParam> ().doumeiFlg = false;
			targetKuni.GetComponent<Image>().color = unDoumeiColor;

		}

        bool rengouFlg = PlayerPrefs.GetBool("rengouFlg");
        string rengouDaimyo = PlayerPrefs.GetString("rengouDaimyo");
        MainStageController MainStageController = new MainStageController();
        MainStageController.UpdateRengouKuniIcon(rengouFlg, rengouDaimyo);




    }


	public void deleteGunzei(GameObject Gunzei){
		//Delete Gunzei
		Destroy(Gunzei);

		//Delete Key
		string gunzeiKey = Gunzei.name;
		PlayerPrefs.DeleteKey(gunzeiKey);

		//Delete Key History
		char[] delimiterChars = {','};
		string keyHistory = PlayerPrefs.GetString ("keyHistory");
		List<string> keyHistoryList = new List<string>();
		if (keyHistory != null && keyHistory != "") {
			if(keyHistory.Contains(",")){
				keyHistoryList = new List<string> (keyHistory.Split (delimiterChars));
			}else{
				keyHistoryList.Add(keyHistory);
			}
		}
		keyHistoryList.Remove(gunzeiKey);
		string newKeyHistory = "";
		for(int i=0; i<keyHistoryList.Count; i++){
			if(i==0){
				newKeyHistory = keyHistoryList[i];
			}else{
				newKeyHistory = newKeyHistory + "," + keyHistoryList[i];
			}
		}
		PlayerPrefs.SetString("keyHistory",newKeyHistory);

		PlayerPrefs.Flush ();
	}

    public void fire(int kuniIconId) {

        GameObject kuniIconObj = GameObject.Find("KuniIconView").transform.Find(kuniIconId.ToString()).gameObject;

        string path = "Prefabs/Saku/sakuEvent/6";
        GameObject fireObj = Instantiate(Resources.Load(path)) as GameObject;
        fireObj.transform.SetParent(kuniIconObj.transform);
        fireObj.transform.localScale = new Vector2(20, 20);
        fireObj.transform.localPosition = new Vector2(0, 0);
        foreach (Transform chld in fireObj.transform) {
            if(chld.name != "sakuAnimation") {
                Destroy(chld.gameObject);
            }
        }
        fireObj.GetComponent<BoxCollider2D>().enabled = false;
        fireObj.GetComponent<SpriteRenderer>().enabled = false;



    }


}
