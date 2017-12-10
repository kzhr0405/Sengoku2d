    using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System.Collections.Generic;

public class AreaButton : MonoBehaviour {
	
	public string type = "";
	public string lv = "";
	public bool blank = true;
	public int effect = 0;
	public int effectNextLv = 0;
	public int moneyNextLv = 0;
	public int requiredHyourou = 0;
	public int naiseiId = 0;
	public string naiseiName = "";

	public void OnClick(){
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        int langId = PlayerPrefs.GetInt("langId");
        Message Message = new Message();
        if (blank) {
            audioSources[0].Play();
            BusyoStatusButton pop = new BusyoStatusButton();
            GameObject board = pop.commonPopup(20);
            
            //New
            //Label
            GameObject.Find("popText").GetComponent<Text>().text = Message.getMessage(168, langId);
			
            //Set Scroll View
			string scrollPath = "Prefabs/Naisei/ScrollView";
			GameObject scroll = Instantiate (Resources.Load (scrollPath)) as GameObject;
			scroll.transform.SetParent (board.transform);
			scroll.transform.localScale = new Vector2 (1, 1);
			scroll.name = "ScrollView";
			RectTransform scrollTransform = scroll.GetComponent<RectTransform> ();
			scrollTransform.anchoredPosition = new Vector3 (0, 0, 0);

			//Naisei Master
			Entity_naisei_mst naiseiMst = Resources.Load ("Data/naisei_mst") as Entity_naisei_mst;

			//Re-make Common & Shigen
			List<string> avlNaiseiList = new List<string> ();
			char[] delimiterChars = {':'};
			string shigen = GameObject.Find("NaiseiController").GetComponent<NaiseiController>().shigen;
			if(shigen != "null"){
				if(shigen.Contains(":")){
					avlNaiseiList = new List<string> (shigen.Split (delimiterChars));
				}else{
					avlNaiseiList.Add(shigen);
				}
			}

			/***Tech Item Start***/
			List<string> avlTechList = new List<string> ();

			int tpQty = PlayerPrefs.GetInt("transferTP");
			int kbQty = PlayerPrefs.GetInt("transferKB");
			int snbQty = PlayerPrefs.GetInt("transferSNB");
			if(tpQty>0 && !avlNaiseiList.Contains("tp")){
				avlTechList.Add("tp");
			}
			if(kbQty>0 && !avlNaiseiList.Contains("kb")){
				avlTechList.Add("kb");
			}
			if(snbQty>0 && !avlNaiseiList.Contains("snb")){
				avlTechList.Add("snb");
			}
            /***Tech Item Finish***/



            NaiseiInfo naiseiInfo = new NaiseiInfo();
			for (int i=1; i<naiseiMst.param.Count; i++) {
				string code = naiseiMst.param [i].code;

				//if (code != "NotYet"){ // for test
				if ((naiseiMst.param [i].common == 1 && code != "NotYet") ||
				(avlNaiseiList.Contains(naiseiMst.param [i].code) && code != "NotYet")) {
					//Slot
					string slotPath = "Prefabs/Naisei/NaiseiSlot";
					GameObject slot = Instantiate (Resources.Load (slotPath)) as GameObject;
					slot.transform.SetParent (scroll.transform.Find ("NaiseiContent").transform);
					slot.transform.localScale = new Vector2 (1, 1);

					//Bldg
                    string naiseiNameText = naiseiInfo.getNaiseiName(i);
                    string naiseExpText = naiseiInfo.getNaiseiExp(i);


                    GameObject naiseiName = slot.transform.Find ("NaiseiName").gameObject;
					naiseiName.GetComponent<Text> ().text = naiseiNameText;
					string bldgPath = "Prefabs/Naisei/Bldg/" + naiseiMst.param [i].code + "_s";
					GameObject bldg = Instantiate (Resources.Load (bldgPath)) as GameObject;
					bldg.transform.SetParent (naiseiName.transform);
					RectTransform bldgTransform = bldg.GetComponent<RectTransform> ();
					bldgTransform.anchoredPosition = new Vector3 (0, -315, 0);
					bldg.transform.localScale = new Vector2 (3, 3);
					bldg.GetComponent<Button>().enabled = false;

					//Some Value
					naiseiName.transform.Find ("NaiseiExp").GetComponent<Text> ().text = naiseExpText;
                    string target = naiseiMst.param [i].target;
					int effect1 = naiseiMst.param [i].effect1;
                    
                    if (target == "money") {
                        naiseiName.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(169, langId);
                    }
                    else if (target == "hyourou") {
                        naiseiName.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(170, langId);
                    }
                    else if (target == "YR") {
                        naiseiName.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(171, langId);
                    }
                    else if (target == "KB") {
                        naiseiName.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(172, langId);
                    }
                    else if (target == "TP") {
                        naiseiName.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(173, langId);
                    }
                    else if (target == "YM") {
                        naiseiName.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(174, langId);
                    }
                    else if (target == "nbn") {
                        naiseiName.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(175, langId);
                    }
                    else if (target == "bnk") {
                        naiseiName.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(176, langId);
                    }
                    else if (target == "snb") {
                        naiseiName.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(177, langId);
                    }
                    else if (target == "bky") {
                        naiseiName.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(178, langId);
                    }
                    else if (target == "child") {
                        naiseiName.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(179, langId);
                    }
                    else if (target == "dfc") {
                        naiseiName.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(180, langId);
                    }
                    
					//Label & Effect Value Adjustment 
					if(code != "kzn"){
						if(code != "yr" &&code != "kb"&&code != "tp" &&code != "ym" && code != "snb"){
							if(code != "nbn" && code != "kgy" && code != "bky" && code !="hsy" && code !="trd"){
								naiseiName.transform.Find ("NaiseiEffectValue").GetComponent<Text> ().text = "+" + effect1;
							}else{
								naiseiName.transform.Find ("NaiseiUnit").GetComponent<Text>().enabled = false;
								naiseiName.transform.Find ("NaiseiEffectValue").GetComponent<Text> ().text = "+" + effect1;
							}
						}else{
							effect1 = effect1 * 2;
                            
                            //Rank


                            naiseiName.transform.Find ("NaiseiEffectValue").GetComponent<Text> ().text = "+" + effect1;
                            

						}
					}else{
						effect1 = effect1 * 4;
						naiseiName.transform.Find ("NaiseiEffectValue").GetComponent<Text> ().text = "+" + effect1;
					}

					naiseiName.transform.Find ("RequiredMoney").GetComponent<Text> ().text = naiseiMst.param [i].money1.ToString();
					naiseiName.transform.Find ("RequiredHyourou").GetComponent<Text> ().text = naiseiMst.param [i].hyourou.ToString();
					GameObject createButton = naiseiName.transform.Find("CreateButton").gameObject;
					createButton.GetComponent<BuildNaisei> ().panelId = int.Parse(name);
					createButton.GetComponent<BuildNaisei> ().naiseiId = i;
                    createButton.GetComponent<BuildNaisei>().naiseiName = naiseiNameText;
                    createButton.GetComponent<BuildNaisei> ().requiredMoney = naiseiMst.param [i].money1;
					createButton.GetComponent<BuildNaisei> ().requiredHyourou = naiseiMst.param [i].hyourou;

				}else if (naiseiMst.param [i].common == 0 && code != "NotYet" && avlTechList.Contains(naiseiMst.param [i].code)){
                    //Technical Item
                    string naiseiNameText = naiseiInfo.getNaiseiName(i);
                    string naiseExpText = naiseiInfo.getNaiseiExp(i);

                    //Slot
                    string slotPath = "Prefabs/Naisei/NaiseiSlotWithItem";
					GameObject slot = Instantiate (Resources.Load (slotPath)) as GameObject;
					slot.transform.SetParent (scroll.transform.Find ("NaiseiContent").transform);
					slot.transform.localScale = new Vector2 (1, 1);
					
					//Bldg
					GameObject naiseiName = slot.transform.Find ("NaiseiName").gameObject;
                    naiseiName.GetComponent<Text>().text = naiseiNameText;
                    string bldgPath = "Prefabs/Naisei/Bldg/" + naiseiMst.param [i].code + "_s";
					GameObject bldg = Instantiate (Resources.Load (bldgPath)) as GameObject;
					bldg.transform.SetParent (naiseiName.transform);
					RectTransform bldgTransform = bldg.GetComponent<RectTransform> ();
					bldgTransform.anchoredPosition = new Vector3 (0, -315, 0);
					bldg.transform.localScale = new Vector2 (3, 3);
					bldg.GetComponent<Button>().enabled = false;

                    //Some Value
                    naiseiName.transform.Find("NaiseiExp").GetComponent<Text>().text = naiseExpText;
                    string target = naiseiMst.param [i].target;
					int effect1 = naiseiMst.param [i].effect1;

					int techId = 0;
                   
                    if (target == "KB") {
                        naiseiName.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(172, langId);
                        techId = 2;
                    }
                    else if (target == "TP") {
                        naiseiName.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(173, langId);
                        techId = 1;
                    }
                    else if (target == "snb") {
                        naiseiName.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(177, langId);
                        techId = 3;
                    }
                    
					//Label & Effect Value Adjustment 
					effect1 = effect1 * 2;
					naiseiName.transform.Find ("NaiseiEffectValue").GetComponent<Text> ().text = "+" + effect1;

					naiseiName.transform.Find ("RequiredHyourou").GetComponent<Text> ().text = naiseiMst.param [i].hyourou.ToString();
					GameObject createButton = naiseiName.transform.Find("CreateButton").gameObject;
					createButton.GetComponent<BuildNaiseiWithItem> ().techId = techId;
					createButton.GetComponent<BuildNaiseiWithItem> ().panelId = int.Parse(name);
					createButton.GetComponent<BuildNaiseiWithItem> ().naiseiId = i;
                    createButton.GetComponent<BuildNaiseiWithItem>().naiseiName = naiseiNameText;
                    createButton.GetComponent<BuildNaiseiWithItem> ().requiredHyourou = naiseiMst.param [i].hyourou;

				}
			}

            //tutorial
            if (Application.loadedLevelName == "tutorialNaisei") {

                GameObject.Find("board(Clone)").transform.Find("close").gameObject.SetActive(false);
                
                foreach (Transform child in scroll.transform.Find("NaiseiContent").transform) {
                    if(child.transform.Find("NaiseiName").transform.Find("CreateButton").GetComponent<BuildNaisei>()) {
                        if(child.transform.Find("NaiseiName").transform.Find("CreateButton").GetComponent<BuildNaisei>().naiseiId != 1) {
                            child.gameObject.SetActive(false);
                        }
                    }else if(child.transform.Find("NaiseiName").transform.Find("CreateButton").GetComponent<BuildNaiseiWithItem>()) {
                        child.gameObject.SetActive(false);
                    }
                }

                TutorialController tutorialScript = new TutorialController();
                Vector2 vect = new Vector2(0, 30);
                GameObject createButton = GameObject.Find("CreateButton").gameObject;
                GameObject pointUp = tutorialScript.SetPointer(createButton, vect);
                pointUp.transform.localScale = new Vector2(100,100);

                //set zero
                GameObject.Find("RequiredMoney").GetComponent<Text>().text = "0";
                GameObject.Find("RequiredHyourou").GetComponent<Text>().text = "0";
                createButton.GetComponent<BuildNaisei>().requiredHyourou = 0;
                createButton.GetComponent<BuildNaisei>().requiredMoney = 0;

            }


        } else {
                
            BusyoStatusButton pop = new BusyoStatusButton();
            GameObject board = pop.commonPopup(21);
            audioSources[0].Play();

            //Update
            GameObject.Find ("popText").GetComponent<Text> ().text = Message.getMessage(168, langId);

			string naiseiUpdatePath = "Prefabs/Naisei/NaiseiUpdate";
			GameObject NaiseiUpdate = Instantiate (Resources.Load (naiseiUpdatePath)) as GameObject;
            NaiseiUpdate.transform.SetParent(board.transform);
			NaiseiUpdate.transform.localScale = new Vector2 (1, 1);
			RectTransform naiseiUpdateTransform = NaiseiUpdate.GetComponent<RectTransform> ();
			naiseiUpdateTransform.anchoredPosition = new Vector3 (0, -40, 0);

			string bldgRank = "";
			int nextLv = int.Parse(lv) + 1;
			if(nextLv<8){
				bldgRank = "s";
			}else if(nextLv < 15){
				bldgRank = "m";
			}else if(15 <= nextLv){
				bldgRank = "l";
			}
			string bldg = type + "_" + bldgRank;
			string pathMod = "";
			if(type == "shiro"){
				pathMod = "Shiro/";
			}else{
				pathMod = "Bldg/";
			}

			string bldgPath = "Prefabs/Naisei/" + pathMod + bldg;
			GameObject bldgObj = Instantiate (Resources.Load (bldgPath)) as GameObject;
			bldgObj.transform.SetParent(NaiseiUpdate.transform);
			bldgObj.transform.localScale = new Vector3 (1.5f, 1.5f, 1);
			RectTransform bldgTransform = bldgObj.GetComponent<RectTransform> ();
			bldgTransform.anchoredPosition = new Vector3 (-250, 0, 0);
			bldgObj.GetComponent<Button>().enabled = false;

            //Special Shiro
            if(type == "shiro") {
                string shiroTmp = "shiro" + GameObject.Find("NaiseiController").GetComponent<NaiseiController>().activeKuniId;
                if (PlayerPrefs.HasKey(shiroTmp)) {
                    int shiroId = PlayerPrefs.GetInt(shiroTmp);
                    if (shiroId != 0) {
                        string imagePath = "Prefabs/Naisei/Shiro/Sprite/" + shiroId;
                        bldgObj.GetComponent<Image>().sprite =
                                        Resources.Load(imagePath, typeof(Sprite)) as Sprite;
                    }
                }
            }

            //Detail Info
            GameObject baseObj = NaiseiUpdate.transform.Find("Base").gameObject;
			baseObj.transform.Find("FromLv").GetComponent<Text>().text = "Lv" + lv;
			baseObj.transform.Find("ToLv").GetComponent<Text>().text = nextLv.ToString();

            //Rank
            string nowRank = "";
            string nxtRank = "";
            if(type == "yr" || type == "kb" || type == "tp" || type == "ym" || type == "kb" || type == "snb") {
               
                if (int.Parse(lv) < 11) {
                    nowRank = Message.getMessage(181, langId);
                }
                else if (int.Parse(lv) < 16) {
                    nowRank = Message.getMessage(182, langId);
                }
                else {
                    nowRank = Message.getMessage(183, langId);
                }
                
                if ((int.Parse(lv) + 1) < 11) {
                    nxtRank = Message.getMessage(181, langId);
                }
                else if ((int.Parse(lv) + 1) < 16) {
                    nxtRank = Message.getMessage(182, langId);
                }
                else {
                    nxtRank = Message.getMessage(183, langId);
                }
                
            }

            baseObj.transform.Find("NowNaiseiEffectValue").GetComponent<Text>().text = nowRank + " +" + effect.ToString();
			baseObj.transform.Find("NextNaiseiEffectValue").GetComponent<Text>().text = nxtRank + " +" + effectNextLv.ToString();
			baseObj.transform.Find("RequiredMoney").GetComponent<Text>().text = moneyNextLv.ToString();
			baseObj.transform.Find("RequiredHyourou").GetComponent<Text>().text = requiredHyourou.ToString();


			if(type=="shiro"){
                
                baseObj.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(180,langId);

                baseObj.transform.Find ("NaiseiUnit").GetComponent<Text> ().enabled = false;                
                baseObj.transform.Find("BldgName").GetComponent<Text>().text = Message.getMessage(184,langId);

                baseObj.transform.Find ("NaiseiEffectLabel2").GetComponent<Text> ().enabled = true;
				baseObj.transform.Find("NowNaiseiEffectValue2").GetComponent<Text>().enabled = true;
				baseObj.transform.Find("NextNaiseiEffectValue2").GetComponent<Text>().enabled = true;
				baseObj.transform.Find ("arrow2").GetComponent<Image> ().enabled = true;
				baseObj.transform.Find("NowNaiseiEffectValue2").GetComponent<Text>().text = "+" + effect.ToString();
				baseObj.transform.Find("NextNaiseiEffectValue2").GetComponent<Text>().text = "+" + effectNextLv.ToString();

                //Special Castle Check
                string nowQty = PlayerPrefs.GetString("shiro");
                List<string> nowQtyList = new List<string>();
                char[] delimiterChars = { ',' };
                if (nowQty != "" && nowQty != null) {
                    //scroll view
                    string scrollPath = "Prefabs/Naisei/Shiro/ShiroScrollView";
                    GameObject scroll = Instantiate(Resources.Load(scrollPath)) as GameObject;
                    scroll.transform.SetParent(board.transform);
                        
                    GameObject content = scroll.transform.Find("Content").gameObject;
                    scroll.transform.localScale = new Vector2(0.8f, 0.8f);
                    scroll.transform.localPosition = new Vector2(-240, -240);

                    nowQtyList = new List<string>(nowQty.Split(delimiterChars));
                    string path = "Prefabs/Item/Shiro/shiro";
                    Shiro shiro = new Shiro();
                    for (int i = 0; i < nowQtyList.Count; i++) {
                        string imagePath = "Prefabs/Naisei/Shiro/Sprite/";
                        int qty = int.Parse(nowQtyList[i]);
                        if (qty != 0) {
                            int shiroId = i + 1;
                            GameObject item = Instantiate(Resources.Load(path)) as GameObject;
                            item.transform.SetParent(content.transform);
                            item.transform.localScale = new Vector2(1, 1);
                            item.transform.localPosition = new Vector3(0, 0, 0);
                            item.transform.Find("Qty").GetComponent<Text>().text = qty.ToString();

                            string name = shiro.getName(shiroId);
                            item.transform.Find("name").GetComponent<Text>().text = name;
                            imagePath = imagePath + shiroId;
                            item.transform.Find("image").GetComponent<Image>().sprite =
                                Resources.Load(imagePath, typeof(Sprite)) as Sprite;

                            //value
                            item.name = "shiro" + shiroId;
                            item.GetComponent<ItemInfo>().posessQty = qty;
                            item.GetComponent<ItemInfo>().itemId = shiroId;
                            item.GetComponent<ItemInfo>().itemName = name;

                        }
                    }
                    //scroll.GetComponent<ScrollRect>().enabled = false;
                }
                    
			}else{

				//Destroy Button
				string dstryPath = "Prefabs/Naisei/DestroyButton";
				GameObject dstryBtnObj = Instantiate (Resources.Load (dstryPath)) as GameObject;
				dstryBtnObj.transform.SetParent(GameObject.Find ("board(Clone)").transform);
				dstryBtnObj.transform.localPosition = new Vector2 (-480,-220);
				dstryBtnObj.transform.localScale = new Vector2 (1,1);
				dstryBtnObj.GetComponent<NaiseiDestroy> ().areaId = int.Parse(name);
				dstryBtnObj.GetComponent<NaiseiDestroy> ().activeKuniId = GameObject.Find ("NaiseiController").GetComponent<NaiseiController> ().activeKuniId;

				baseObj.transform.Find ("NaiseiEffectLabel2").GetComponent<Text> ().enabled = false;
				baseObj.transform.Find ("NowNaiseiEffectValue2").GetComponent<Text> ().enabled = false;
				baseObj.transform.Find ("arrow2").GetComponent<Image> ().enabled = false;
				baseObj.transform.Find ("NextNaiseiEffectValue2").GetComponent<Text> ().enabled = false;

               
                if (type == "shop") {
                    baseObj.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(169, langId);
                    baseObj.transform.Find("BldgName").GetComponent<Text>().text = Message.getMessage(185, langId);

                }
                else if (type == "ta") {
                    baseObj.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(170, langId);
                    baseObj.transform.Find("BldgName").GetComponent<Text>().text = Message.getMessage(186, langId);

                }
                else if (type == "yr") {
                    baseObj.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(171, langId);
                    baseObj.transform.Find("BldgName").GetComponent<Text>().text = Message.getMessage(187, langId);

                }
                else if (type == "kb") {
                    baseObj.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(172, langId);
                    baseObj.transform.Find("BldgName").GetComponent<Text>().text = Message.getMessage(188, langId);

                }
                else if (type == "tp") {
                    baseObj.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(173, langId);
                    baseObj.transform.Find("BldgName").GetComponent<Text>().text = Message.getMessage(189, langId);

                }
                else if (type == "ym") {
                    baseObj.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(174, langId);
                    baseObj.transform.Find("BldgName").GetComponent<Text>().text = Message.getMessage(190, langId);

                }
                else if (type == "kzn") {
                    baseObj.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(169, langId);
                    baseObj.transform.Find("BldgName").GetComponent<Text>().text = Message.getMessage(191, langId);

                }
                else if (type == "nbn") {
                    baseObj.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(175, langId);
                    baseObj.transform.Find("BldgName").GetComponent<Text>().text = Message.getMessage(192, langId);
                    baseObj.transform.Find("NaiseiUnit").GetComponent<Text>().enabled = false;

                }
                else if (type == "kgy") {
                    baseObj.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(176, langId);
                    baseObj.transform.Find("BldgName").GetComponent<Text>().text = Message.getMessage(193, langId);
                    baseObj.transform.Find("NaiseiUnit").GetComponent<Text>().enabled = false;

                }
                else if (type == "snb") {
                    baseObj.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(177, langId);
                    baseObj.transform.Find("BldgName").GetComponent<Text>().text = Message.getMessage(194, langId);

                }
                else if (type == "bky") {
                    baseObj.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(178, langId);
                    baseObj.transform.Find("BldgName").GetComponent<Text>().text = Message.getMessage(195, langId);
                    baseObj.transform.Find("NaiseiUnit").GetComponent<Text>().enabled = false;

                }
                else if (type == "hsy") {
                    baseObj.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(179, langId);
                    baseObj.transform.Find("BldgName").GetComponent<Text>().text = Message.getMessage(196, langId);
                    baseObj.transform.Find("NaiseiUnit").GetComponent<Text>().enabled = false;

                }
                else if (type == "trd") {
                    baseObj.transform.Find("NaiseiEffectLabel").GetComponent<Text>().text = Message.getMessage(180, langId);
                    baseObj.transform.Find("BldgName").GetComponent<Text>().text = Message.getMessage(197, langId);
                    baseObj.transform.Find("NaiseiUnit").GetComponent<Text>().enabled = false;
                }
                
			}

			//Button Setting
			GameObject updateBtn = baseObj.transform.Find ("NaiseiUpdateButton").gameObject;
			updateBtn.GetComponent<UpdateNaisei>().activeKuniId = GameObject.Find ("NaiseiController").GetComponent<NaiseiController>().activeKuniId;
			updateBtn.GetComponent<UpdateNaisei>().requiredMoney = moneyNextLv;
			updateBtn.GetComponent<UpdateNaisei>().requiredHyourou = requiredHyourou;
			updateBtn.GetComponent<UpdateNaisei>().areaId = name;
			updateBtn.GetComponent<UpdateNaisei>().naiseiId = naiseiId;
			updateBtn.GetComponent<UpdateNaisei>().targetLv = nextLv;
			updateBtn.GetComponent<UpdateNaisei>().naiseiName = naiseiName;
            
            if(int.Parse(lv)==20) {

                GameObject Base = NaiseiUpdate.transform.Find("Base").gameObject;
                foreach(Transform chld in Base.transform) {
                    if(chld.name == "arrow") {
                        Destroy(chld.gameObject);
                    }else if(chld.name == "arrow2") {
                        Destroy(chld.gameObject);
                    }else if(chld.name == "NextNaiseiEffectValue2") {
                        Destroy(chld.gameObject);
                    }
                }
                Destroy(Base.transform.Find("ToLv").gameObject);
                Destroy(Base.transform.Find("NextNaiseiEffectValue").gameObject);
                Destroy(Base.transform.Find("NaiseiUnit").gameObject);
                Base.transform.Find("RequiredMoney").GetComponent<Text>().text = "0";
                Base.transform.Find("RequiredHyourou").GetComponent<Text>().text = "0";
                Base.transform.Find("NaiseiUpdateButton").GetComponent<Button>().enabled = false;

                Color NGClorBtn = new Color(133 / 255f, 133 / 255f, 80 / 255f, 255f / 255f);
                Color NGClorTxt = new Color(90 / 255f, 90 / 255f, 40 / 255f, 255f / 255f);
                Base.transform.Find("NaiseiUpdateButton").GetComponent<Image>().color = NGClorBtn;
                Base.transform.Find("NaiseiUpdateButton").transform.Find("Text").GetComponent<Text>().color = NGClorTxt;
            }
        }
	}
}
