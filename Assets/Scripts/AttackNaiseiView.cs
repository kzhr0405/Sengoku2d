using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class AttackNaiseiView : MonoBehaviour {
	
	public int kuniId;
	public string kuniName;
	public int myDaimyoId;
	public int daimyoId;
	public string daimyoName;
	public bool openFlg = false;
	public bool clearFlg = false;
	public int activeBusyoQty;
	public int activeBusyoLv;
	public int activeButaiQty;
	public int activeButaiLv;
	public bool doumeiFlg = false;
	public float senryokuRatio;
	public int cyouhouSnbRankId = 0;
	List<string> noLinkStageList;

	//Sound
	public AudioSource sound;

    //Asset Manager
    //public AssetBundleManager assetBundleManager;
    //public Image image;

    //public IEnumerator Start() {
    //
    //assetBundleManager = GameObject.Find("AssetManager").GetComponent<AssetBundleManager>();
    //
    //    yield return StartCoroutine(assetBundleManager.LoadAssetBundleCoroutine());
    //
    //}


    public void OnClick(){
        int langId = PlayerPrefs.GetInt("langId");
        int senarioId = PlayerPrefs.GetInt("senarioId");
        cyouhouSnbRankId = transform.parent.gameObject.transform.Find("close").GetComponent<CloseBoard>().cyouhouSnbRankId;

        //SE
        sound = GameObject.Find ("SEController").GetComponent<AudioSource> ();
		sound.PlayOneShot (sound.clip); 


		/*Common Process*/
		string pathOfBoard = "Prefabs/Map/Popup";
		GameObject board = Instantiate (Resources.Load (pathOfBoard)) as GameObject;
		board.transform.SetParent(GameObject.Find ("Map").transform);
        board.transform.SetSiblingIndex(1);
        board.transform.localScale = new Vector2 (1, 0.8f);
		board.transform.localPosition = new Vector2 (0, 0);
		board.name = "AttackStagePopup";
		Daimyo daimyo = new Daimyo();
        char[] delimiterChars = { ',' };

        if (Application.loadedLevelName == "tutorialMain") {
            board.transform.Find("board").transform.Find("close").gameObject.SetActive(false);
        }
        
        /*Indivisual Process by Kuni*/
        string pathOfKuniMap = "Prefabs/Map/stage/kuniMap";
		GameObject kuniMap = Instantiate (Resources.Load (pathOfKuniMap)) as GameObject;
		board.transform.Find ("kuniName").GetComponent<Text> ().text = kuniName;

		string kuniImagePath = "Prefabs/Map/stage/MapSprite/stage" + kuniId.ToString ();
        string assetBundlePath = "stage" + kuniId.ToString();

        kuniMap.GetComponent<Image> ().sprite =
            Resources.Load (kuniImagePath, typeof(Sprite)) as Sprite;
            //assetBundleManager.GetSpriteFromAssetBundle(assetBundlePath); //Asset
            //assetBundleManager.assetBundleCache.Unload(false);

        //kuni
        GameObject boardObj = board.transform.Find("board").gameObject;
		kuniMap.transform.SetParent (boardObj.transform);
		kuniMap.transform.localScale = new Vector2 (9, 6);
		kuniMap.transform.localPosition = new Vector3 (-145, 4, 0);
		kuniMap.name = "kuniMap" + kuniId;
		
		
		//Clear Stage Setting
		string clearedStage = "kuni" + kuniId;
        string clearedStageString = "";
        if (Application.loadedLevelName != "tutorialMain") {
            clearedStageString = PlayerPrefs.GetString (clearedStage);
        }else {
            clearedStageString = "1,2,3,4,5,6,7,8,9,10";
        }
		List<string> clearedStageList = new List<string> ();
		if (clearedStageString != null && clearedStageString != "") {			
			clearedStageList = new List<string> (clearedStageString.Split (delimiterChars));			
		}
		
        //Data adjustment
        if(!clearFlg && clearedStageList.Count == 10) {
            clearedStageList = new List<string>();
            clearedStageString = "";
            PlayerPrefs.DeleteKey(clearedStage);
        }


		//Default Value
		Entity_stage_mst stageMst = Resources.Load ("Data/stage_mst") as Entity_stage_mst;
		int startline = 10 * kuniId - 10;
		string stagePath = "Prefabs/Map/stage/stage";
		string clearedPath = "Prefabs/Map/cleared";


		int stageId = 1;
		bool clearedFlg = false;
		int mySenryoku = daimyo.getSenryoku(myDaimyoId);
		int enemySenryoku = daimyo.getSenryoku(daimyoId);
		float senryokuRatio = (float)enemySenryoku / (float)mySenryoku;

		for (int i=startline; i<startline+10; i++) {
			GameObject stage = Instantiate (Resources.Load (stagePath)) as GameObject;
			stage.transform.SetParent (kuniMap.transform);
			stage.transform.localScale = new Vector2 (1, 1);
			stage.name = "stage" + stageId.ToString ();
			
			//Cleared Check
			if (clearedStageList.Contains (stageId.ToString ()) == true) {
				GameObject cleared = Instantiate (Resources.Load (clearedPath)) as GameObject;
				cleared.transform.SetParent (stage.transform);
				stage.GetComponent<ShowStageDtl> ().clearedFlg = true;
				cleared.transform.localScale = new Vector2 (3, 5);
				cleared.transform.localPosition = new Vector2 (0, 0);
				clearedFlg = true;
                cleared.name = "cleared";

            }

            //Get Stage Info
            string stageName = "";
			int locationX = stageMst.param [i].LocationX;
			int locationY = stageMst.param [i].LocationY;
			int powerType = stageMst.param [i].powerTyp;            
            if (langId == 2) {
                stageName = stageMst.param[i].stageNameEng;
            }else {
                stageName = stageMst.param[i].stageName;
            }
            RectTransform stageTransform = stage.GetComponent<RectTransform> ();
			stageTransform.anchoredPosition = new Vector3 (locationX, locationY, 0);


			//money and exp calculation
			int minExp = getMinExp(powerType,senryokuRatio);
			int maxExp = getMaxExp(powerType,senryokuRatio);
			int exp = UnityEngine.Random.Range (minExp, maxExp + 1);

			int minMoney = minExp * 2;
			int maxMoney = maxExp * 3;
			int money = UnityEngine.Random.Range (minMoney, maxMoney + 1);

			string itemGrp = getRandomItemGrp ();
			string itemTyp = "";
			int itemId = 0;
			int itemQty = 1;

			if (itemGrp == "item") {
				itemTyp = getRandomItemTyp(itemGrp);
				if (itemTyp == "tech") {
					itemId = getItemRank (66, 33);
				} else if (itemTyp == "Tama") {
					itemId = getItemRank (20, 5);
					if (itemId == 3) {
						itemQty = 100;
					} else if (itemId == 2) {
						itemQty = 20;
					} else if (itemId == 1) {
						itemQty = 5;
					}
				} else {
					itemId = getItemRank (20, 5);
				}

			} else if (itemGrp == "kahou") {
				itemTyp = getRandomItemTyp(itemGrp);
				Kahou kahou = new Kahou ();
				string kahouRank = getKahouRank ();
				itemId = kahou.getRamdomKahouId (itemTyp, kahouRank);
			}


			stage.GetComponent<ShowStageDtl> ().kuniId = kuniId;
			stage.GetComponent<ShowStageDtl> ().stageId = stageId;
			stage.GetComponent<ShowStageDtl> ().stageName = stageName;
			stage.GetComponent<ShowStageDtl> ().exp = exp;
			stage.GetComponent<ShowStageDtl> ().showExp = minExp + " - " + maxExp;
			stage.GetComponent<ShowStageDtl> ().money = money;
			stage.GetComponent<ShowStageDtl> ().showMoney = minMoney + " - " + maxMoney;
			stage.GetComponent<ShowStageDtl> ().itemGrp = itemGrp;
			stage.GetComponent<ShowStageDtl> ().itemTyp = itemTyp;
			stage.GetComponent<ShowStageDtl> ().itemId = itemId.ToString ();
			stage.GetComponent<ShowStageDtl> ().itemQty = itemQty;
			stage.GetComponent<ShowStageDtl> ().powerType = powerType;

			if(powerType==2){
				string imagePath = "Prefabs/Naisei/Shiro/Sprite/shiro_m";
				stage.transform.Find("shiroImage").GetComponent<SpriteRenderer> ().sprite = 
					Resources.Load (imagePath, typeof(Sprite)) as Sprite;
			}else if(powerType==3){
				string imagePath = "Prefabs/Naisei/Shiro/Sprite/shiro_l";
                stage.transform.Find("shiroImage").GetComponent<SpriteRenderer> ().sprite = 
					Resources.Load (imagePath, typeof(Sprite)) as Sprite;
			}


			
			stageId = stageId + 1;
		}


		//Line Setting
		Entity_stageLink_mst stageLinkMst = Resources.Load ("Data/stageLink_mst") as Entity_stageLink_mst;
		List<string> myStageLink = new List<string> ();
        for (int i=0; i<stageLinkMst.param.Count; i++) {
			int tempKuniId = stageLinkMst.param[i].kuniId;
			if(tempKuniId ==kuniId){
				myStageLink.Add(stageLinkMst.param[i].Link);
			}
		}
        List<string> myOriginalStageLink = new List<string>(myStageLink);

        //Reduce Linkcut kousaku
        string tempLinkuct = "linkcut" + kuniId;
		string linkcut = PlayerPrefs.GetString(tempLinkuct);
		List<string> linkcutList = new List<string> ();
		if (linkcut != null && linkcut != "") {
			if (linkcut.Contains (",")) {
				linkcutList = new List<string> (linkcut.Split (delimiterChars));
			} else {
				linkcutList.Add (linkcut);
			}
		} 

		myStageLink.RemoveAll (linkcutList.Contains);



		char[] delimiterChars2 = {'-'};
		string linkPath = "Prefabs/Map/stage/link";
		for (int j=0; j<myStageLink.Count; j++) {

			List<string> linkList = new List<string> (myStageLink[j].Split (delimiterChars2));
			string stage1Name = linkList[0];
			string stage2Name = linkList[1];

			//if(!clearedStageList.Contains(stage1Name) && !clearedStageList.Contains(stage2Name)){

				//1st
				string temp1 = "stage" + stage1Name;
				GameObject stage1 = kuniMap.transform.Find(temp1).gameObject;
				RectTransform rectStage1 = stage1.GetComponent<RectTransform>();
				Vector3 vect1 = new Vector3(rectStage1.anchoredPosition.x,rectStage1.anchoredPosition.y,0);

				//2nd
				string temp2 = "stage" + stage2Name;
				GameObject stage2 = kuniMap.transform.Find(temp2).gameObject;
				RectTransform rectStage2 = stage2.GetComponent<RectTransform>();
				Vector3 vect2 = new Vector3(rectStage2.anchoredPosition.x,rectStage2.anchoredPosition.y,0);



				//Set
				GameObject link = Instantiate (Resources.Load (linkPath)) as GameObject;
				link.transform.SetParent (kuniMap.transform);
				link.transform.localScale = new Vector2 (1, 1);
				RectTransform linkRectTransform =link.GetComponent<RectTransform>();
				linkRectTransform.anchoredPosition = new Vector3 (0, 0, 0);
				link.GetComponent<LineRenderer>().SetPosition(0,vect1);
				link.GetComponent<LineRenderer>().SetPosition(1,vect2);

            if (!clearedStageList.Contains(stage1Name) && !clearedStageList.Contains(stage2Name)) {
                //Line No
                stage1.GetComponent<ShowStageDtl>().linkNo = stage1.GetComponent<ShowStageDtl>().linkNo + 1;
				stage2.GetComponent<ShowStageDtl>().linkNo = stage2.GetComponent<ShowStageDtl>().linkNo + 1;


				
			}

            //Name
            link.name = "link" + stage1Name + "-" + stage2Name;
        }


		//Battle Initial Setting
		StartKassen sk = GameObject.Find ("BattleButton").GetComponent<StartKassen> ();
 		sk.activeBusyoQty = activeBusyoQty;
		sk.activeBusyoLv = activeBusyoLv;
		sk.activeButaiQty = activeButaiQty;
		sk.activeButaiLv = activeButaiLv;
		sk.activeDaimyoId = daimyoId;
		sk.doumeiFlg = doumeiFlg;

        /*Naisei Button*/
        string seiryoku = PlayerPrefs.GetString("seiryoku");
        List<string> seiryokuList = new List<string>();
        seiryokuList = new List<string>(seiryoku.Split(delimiterChars));

        //Only for all clear
        if (clearFlg) {
			string naiseiPath = "Prefabs/Map/NaiseiButton";
			GameObject naiseiBtn = Instantiate (Resources.Load (naiseiPath)) as GameObject;
			naiseiBtn.transform.SetParent (board.transform);
			naiseiBtn.GetComponent<StartNaisei> ().activeKuniId = kuniId;
			naiseiBtn.GetComponent<StartNaisei> ().activeKuniName = kuniName;
			naiseiBtn.GetComponent<StartNaisei> ().clearedFlg = clearedFlg;
			naiseiBtn.transform.localScale = new Vector2 (0.4f, 0.4f);
			naiseiBtn.transform.localPosition = new Vector2 (-490, -300);            
            if (langId == 2) {
                boardObj.transform.Find ("stageDtl").transform.Find ("BattleButton").transform.Find ("Text").GetComponent<Text> ().text = "Training";
            }else {
                boardObj.transform.Find("stageDtl").transform.Find("BattleButton").transform.Find("Text").GetComponent<Text>().text = "訓練";
            }
            GameObject firstStage = GameObject.Find("stage1");
            firstStage.GetComponent<ShowStageDtl>().OnClick();

            if (Application.loadedLevelName == "tutorialMain") {
                GameObject battleBtnObj = boardObj.transform.Find("stageDtl").transform.Find("BattleButton").gameObject;
                battleBtnObj.GetComponent<Button>().interactable = false;
                battleBtnObj.transform.Find("Text").GetComponent<Text>().color = new Color(220f / 255f, 190f / 255f, 40f / 255f, 100f / 255f);

                TutorialController tutorialScript = new TutorialController();
                Vector2 vect = new Vector2(0, 100);
                GameObject animObj = tutorialScript.SetPointer(naiseiBtn, vect);
                animObj.transform.localScale = new Vector2(300, 300);
            }
        } else{
            //enemy attack

            GameObject closeObj = boardObj.transform.Find("close").gameObject;
            EnemyEventHandler enemyEvent = board.transform.Find("board").GetComponent<EnemyEventHandler>();
            enemyEvent.doEnemyEvent(kuniMap, closeObj,kuniId, daimyoId, activeBusyoQty, activeBusyoLv, activeButaiQty, activeButaiLv);

            //open shiro
            List<int> closeStageIdList = new List<int> {1,2,3,4,5,6,7,8,9,10};

            //compare linkKuni with mySeiryoku
            List<int> linkKuniList = new List<int>();
            List<int> linkMyKuniList = new List<int>();
            
            KuniInfo kuniScript = new KuniInfo();
            linkKuniList = kuniScript.getMappingKuni(kuniId);
            for(int i=0; i<linkKuniList.Count;i++){
                int linkKuniId = linkKuniList[i];
                if(seiryokuList[linkKuniId-1] == myDaimyoId.ToString()) {
                    linkMyKuniList.Add(linkKuniId);
                }
            }

            //open initial stage
            for(int j=0; j<linkMyKuniList.Count; j++){
                int srcKuniId = linkMyKuniList[j];
                string linkStage = kuniScript.getLinkStage(srcKuniId,kuniId);
                linkStage = linkStage.Replace("stage", "");
                List<int> linkStageList = new List<int>(Array.ConvertAll(linkStage.Split(','),
                    new Converter<string, int>((s) => { return Convert.ToInt32(s); })));
                closeStageIdList.RemoveAll(linkStageList.Contains);
                                   
            }

            
            //open cleared kuni & linked stage
            List<int> clearedStageIntList = clearedStageList.ConvertAll(x => int.Parse(x));
            closeStageIdList.RemoveAll(clearedStageIntList.Contains);
            for(int l=0; l<clearedStageIntList.Count; l++){
                int srcStageId = clearedStageIntList[l];
               
                for(int m=0; m< myOriginalStageLink.Count;m++){
                    List<string> linkList = new List<string>(myOriginalStageLink[m].Split(delimiterChars2));
                    int stage1Id = int.Parse(linkList[0]);
                    int stage2Id = int.Parse(linkList[1]);
                    if(srcStageId == stage1Id){
                        if(!getStageGunzeiExistFlg(stage1Id, stage2Id)) {
                            closeStageIdList.Remove(stage2Id);
                        }
                    }else if(srcStageId == stage2Id){
                        if (!getStageGunzeiExistFlg(stage2Id, stage1Id)) {
                            closeStageIdList.Remove(stage1Id);
                        }
                    }
                }


            }
            

            Color closeColor = new Color(60f / 255f, 60f / 255f, 60f / 255f, 255f / 255f); //Black
            for (int k=0; k<closeStageIdList.Count; k++){
                int closeStageId = closeStageIdList[k];
                string stageName = "stage" + closeStageId.ToString();
                GameObject closeStageObj = kuniMap.transform.Find(stageName).gameObject;
                closeStageObj.GetComponent<Button>().enabled = false;
                closeStageObj.transform.Find("shiroImage").GetComponent<SpriteRenderer>().color = closeColor;
            }


            //Initial Setting
            int initStageId = 1;
            for (int i = 1; i <= 10; i++){
                if (!closeStageIdList.Contains(i)){
                    initStageId = i;
                }
            }

            string initStageName = "stage" + initStageId.ToString();
            GameObject firstStage = GameObject.Find(initStageName);
            firstStage.GetComponent<ShowStageDtl>().OnClick();
            

        }

        //Kousaku
        if (!clearFlg) {
			string pathOfScroll = "Prefabs/Map/kousaku/BusyoSelectScroll";
			GameObject scroll = Instantiate (Resources.Load (pathOfScroll)) as GameObject;
			scroll.transform.SetParent (board.transform);
			scroll.transform.localScale = new Vector2 (1, 1);
			RectTransform rectScroll = scroll.GetComponent<RectTransform> ();
			rectScroll.anchoredPosition3D = new Vector3 (410, 0, 0);
			rectScroll.sizeDelta = new Vector2 (300, 750);
			scroll.name = "BusyoSelectScroll";
			scroll.SetActive (false);

			KousakuConfirm kousakuScript = GameObject.Find ("LinkCutButton").GetComponent<KousakuConfirm> ();
			kousakuScript.cyouhouSnbRankId = cyouhouSnbRankId;
			kousakuScript.scrollObj = scroll;

			KousakuConfirm kousakuScript2 = GameObject.Find ("CyouryakuButton").GetComponent<KousakuConfirm> ();
			kousakuScript2.cyouhouSnbRankId = cyouhouSnbRankId;
			kousakuScript2.scrollObj = scroll;
		} else {
			GameObject LinkCutButton = GameObject.Find ("LinkCutButton").gameObject;
			LinkCutButton.GetComponent<Button> ().interactable = false;
			LinkCutButton.transform.Find ("Text").GetComponent<Text> ().color = new Color (220f / 255f, 190f / 255f, 40f / 255f, 100f / 255f);

			GameObject CyouryakuButton = GameObject.Find ("CyouryakuButton").gameObject;
			CyouryakuButton.GetComponent<Button> ().interactable = false;
			CyouryakuButton.transform.Find ("Text").GetComponent<Text> ().color = new Color (220f / 255f, 190f / 255f, 40f / 255f, 100f / 255f);

		}

        
		/*Kassen Event Controller Start*/
		KassenEvent kEvent = new KassenEvent ();
		kEvent.MakeEvent (clearFlg,kuniId,kuniMap,daimyoId,senarioId);

        viewKuniLink(board, kuniMap, seiryokuList,langId, senarioId);


        /*Kassen Event Controller End*/

    }

    public void viewKuniLink(GameObject board, GameObject kuniMap, List<string> seiryokuList, int langId, int senarioId) {
        KuniInfo kuniScript = new KuniInfo();
        List<int>linkKuniList = kuniScript.getMappingKuni(kuniId);
        
        List<int> linkAllKuniList = new List<int>();
        for (int i = 0; i < linkKuniList.Count; i++){
            int linkKuniId = linkKuniList[i];
            linkAllKuniList.Add(linkKuniId);
        }

        //view kuni arrow link
        Daimyo daimyoScript = new Daimyo();
        for (int i = 0; i < linkAllKuniList.Count; i++){
            int srcKuniId = linkAllKuniList[i];
            List<int> XYList = kuniScript.getLinkStageXY(srcKuniId, kuniId);
            string pathOfSrcKuni = "Prefabs/Map/Stage/ArrowKuniName";
            GameObject srcKuniNameObj = Instantiate(Resources.Load(pathOfSrcKuni)) as GameObject;
            srcKuniNameObj.name = "Arrow" + srcKuniId;
            srcKuniNameObj.transform.SetParent(board.transform.Find("board").transform);
            srcKuniNameObj.transform.localScale = new Vector2(0.1f, 0.15f);
            srcKuniNameObj.transform.localPosition = new Vector2(XYList[0], XYList[1]);
            srcKuniNameObj.GetComponent<Text>().text = kuniScript.getKuniName(srcKuniId,langId);

            string linkStage = kuniScript.getLinkStage(srcKuniId, kuniId);
            linkStage = linkStage.Replace("stage", "");
            List<int> linkStageList = new List<int>(Array.ConvertAll(linkStage.Split(','),
                new Converter<string, int>((s) => { return Convert.ToInt32(s); })));

            string arrowDaimyoPath = "Prefabs/Map/Stage/ArrowDaimyo";
            GameObject arrowDaimyo = Instantiate(Resources.Load(arrowDaimyoPath)) as GameObject;
            arrowDaimyo.transform.SetParent(srcKuniNameObj.transform);
            arrowDaimyo.transform.localScale = new Vector2(8, 7);
            arrowDaimyo.transform.localPosition = new Vector2(0, 0);
            arrowDaimyo.transform.Find("Effect").GetComponent<DamagePop>().divSpeed = 5;
            arrowDaimyo.transform.Find("Effect").GetComponent<DamagePop>().attackBoardflg = true;

            int arrowDaimyoId = int.Parse(seiryokuList[srcKuniId-1]);
            string daimyoName = daimyoScript.getName(arrowDaimyoId,langId, senarioId);
            arrowDaimyo.transform.Find("Effect").GetComponent<Text>().text = daimyoName;
            arrowDaimyo.transform.Find("Effect").transform.localScale = new Vector2(0.6f,0.6f);
            string kamonPath = "Prefabs/Kamon/MyDaimyoKamon/" + arrowDaimyoId.ToString();
            arrowDaimyo.GetComponent<Image>().sprite =
                Resources.Load(kamonPath, typeof(Sprite)) as Sprite;


            //Arrow
            string arrowPath = "Prefabs/PostKassen/Arrow";
            for (int j=0; j< linkStageList.Count;j++){
                GameObject arrow = Instantiate(Resources.Load(arrowPath)) as GameObject;
                arrow.transform.SetParent(srcKuniNameObj.transform);
                arrow.transform.localScale = new Vector2(200, 200);
                arrow.transform.localPosition = new Vector2(0, 0);
                string stageId = "stage" + linkStageList[j];
                GameObject tgtStageObj  = kuniMap.transform.Find(stageId).gameObject;
                Vector3 posDif = tgtStageObj.transform.position - arrow.transform.position;
                float angle = Mathf.Atan2(posDif.y, posDif.x) * Mathf.Rad2Deg;

                Vector3 euler = new Vector3(0, 0, angle);
                arrow.transform.rotation = Quaternion.Euler(euler);

                

            }
            
            
            
            

           
           
        }

        


    }



	public int getMinExp(int powerType, float senryokuRatio){
		int minExp = 0;
		int baseExp = 200;

		//power type
		if (powerType == 2) {
			baseExp = 250;
		} else if (powerType == 3) {		
			baseExp = 300;
		}

		//senryoku ratio
		int calcExp = Mathf.CeilToInt((float)baseExp * senryokuRatio);

		//-50%
		minExp = calcExp / 2;

		return minExp;
	}

	public int getMaxExp(int powerType, float senryokuRatio){
		int maxExp = 0;
		int baseExp = 200;

		//power type
		if (powerType == 2) {
			baseExp = 250;
		} else if (powerType == 3) {		
			baseExp = 300;
		}

		//senryoku ratio
		int calcExp = Mathf.CeilToInt((float)baseExp * senryokuRatio);

		//+50%
		maxExp = calcExp + Mathf.CeilToInt((float)calcExp/ 2);

		return maxExp;
	}

	public string getRandomItemGrp(){
		string itemGrp = "no"; //no or item or kahou

		float percent = UnityEngine.Random.value;
		percent = percent * 100;

		if (percent <= 10) {
			//kahou
			itemGrp = "kahou";
		} else if (10 < percent && percent <= 75) {
			//item
			itemGrp = "item";
		}

		return itemGrp;
	}

	public string getRandomItemTyp(string itemGrp){
		string itemTyp = "";

		if (itemGrp == "item") {
			float percent = UnityEngine.Random.value;
			percent = percent * 100;

			if (percent <= 41) {
				itemTyp = "Kanjyo";
			} else if (41 < percent && percent <= 44) {
				itemTyp = "cyoutei";
			} else if (44 < percent && percent <= 47) {
				itemTyp = "koueki";
			} else if (47 < percent && percent <= 55) {
				itemTyp = "CyouheiYR";
			} else if (55 < percent && percent <= 63) {
				itemTyp = "CyouheiKB";
			} else if (63 < percent && percent <= 71) {
				itemTyp = "CyouheiTP";
			} else if (71 < percent && percent <= 79) {
				itemTyp = "CyouheiYM";
			} else if (79 < percent && percent <= 87) {
				itemTyp = "Hidensyo";
			} else if (87 < percent && percent <= 95) {
				itemTyp = "Shinobi";
			} else if (95 < percent && percent <= 98) {
				itemTyp = "Tama";
			} else if (98 < percent && percent <= 100) {
				itemTyp = "tech";
			}

		} else if (itemGrp == "kahou") {
			List<string> kahouRandom = new List<string> (){"bugu","kabuto","gusoku","meiba","cyadougu","chishikisyo","heihousyo"};
			int rdm = UnityEngine.Random.Range(0,7);
			itemTyp = kahouRandom[rdm];
		}

		return itemTyp;
	}

	public string getKahouRank (){
		string kahouRank = "C";

		float percent = UnityEngine.Random.value;
		percent = percent * 100;

		if (percent <= 0.5f) {
			kahouRank = "S";
		} else if (0.5f < percent && percent <= 3) {
			kahouRank = "A";
		} else if (3 < percent && percent <= 30) {
			kahouRank = "B";
		} else if (30 < percent && percent <= 100) {
			kahouRank = "C";
		}

		return kahouRank;
	}

	public int getItemRank (int midPoint, int highPoint){
		int itemRank = 1; //1:Low, 2:Mid, 3:High

		float percent = UnityEngine.Random.value;
		percent = percent * 100;

		if (percent <= highPoint) {
			itemRank = 3;
		} else if (highPoint < percent && percent <= midPoint) {
			itemRank = 2;
		} else if (midPoint < percent && percent <= 100) {
			itemRank = 1;
		}

		return itemRank;
	}

    public bool getStageGunzeiExistFlg(int fromStageId, int toStageId) {
        bool existFlg = false;
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("StageGunzei")) {
            if (fromStageId == obs.GetComponent<TabStageGunzei>().fromStageId && toStageId == obs.GetComponent<TabStageGunzei>().toStageId) {
                existFlg = true;
            }else if(fromStageId == obs.GetComponent<TabStageGunzei>().toStageId && toStageId == obs.GetComponent<TabStageGunzei>().fromStageId) {
                existFlg = true;
            }
        }
        return existFlg;
    }
    

    
}