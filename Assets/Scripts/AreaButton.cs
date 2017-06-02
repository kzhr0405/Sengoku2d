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
       
		if (blank) {
            audioSources[0].Play();
            BusyoStatusButton pop = new BusyoStatusButton();
            pop.commonPopup(20);

            //New
            //Label
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                GameObject.Find ("popText").GetComponent<Text> ().text = "Development";
            }else {
                GameObject.Find("popText").GetComponent<Text>().text = "内政開発";
            }
			//Set Scroll View
			string scrollPath = "Prefabs/Naisei/ScrollView";
			GameObject scroll = Instantiate (Resources.Load (scrollPath)) as GameObject;
			scroll.transform.SetParent (GameObject.Find ("board(Clone)").transform);
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
					slot.transform.SetParent (scroll.transform.FindChild ("NaiseiContent").transform);
					slot.transform.localScale = new Vector2 (1, 1);

					//Bldg
                    string naiseiNameText = naiseiInfo.getNaiseiName(i);
                    string naiseExpText = naiseiInfo.getNaiseiExp(i);


                    GameObject naiseiName = slot.transform.FindChild ("NaiseiName").gameObject;
					naiseiName.GetComponent<Text> ().text = naiseiNameText;
					string bldgPath = "Prefabs/Naisei/Bldg/" + naiseiMst.param [i].code + "_s";
					GameObject bldg = Instantiate (Resources.Load (bldgPath)) as GameObject;
					bldg.transform.SetParent (naiseiName.transform);
					RectTransform bldgTransform = bldg.GetComponent<RectTransform> ();
					bldgTransform.anchoredPosition = new Vector3 (0, -315, 0);
					bldg.transform.localScale = new Vector2 (3, 3);
					bldg.GetComponent<Button>().enabled = false;

					//Some Value
					naiseiName.transform.FindChild ("NaiseiExp").GetComponent<Text> ().text = naiseExpText;
                    string target = naiseiMst.param [i].target;
					int effect1 = naiseiMst.param [i].effect1;
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        if (target == "money") {
						    naiseiName.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Money";
					    } else if (target == "hyourou") {
						    naiseiName.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Stamina";
					    } else if (target == "YR") {
						    naiseiName.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Spear Item";
					    }else if (target == "KB") {
						    naiseiName.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Cavalry Item";
					    }else if (target == "TP") {
						    naiseiName.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Gun Item";
					    }else if (target == "YM") {
						    naiseiName.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Bow Item";
					    }else if (target == "nbn") {
						    naiseiName.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Christian";
					    }else if (target == "bnk") {
						    naiseiName.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Culture";
					    }else if (target == "snb") {
						    naiseiName.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Ninja";
					    }else if (target == "bky") {
						    naiseiName.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Buddhism";
					    }else if (target == "child") {
						    naiseiName.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Additional Soldier";
					    }else if (target == "dfc") {
						    naiseiName.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Defence";
					    }
                    }else {
                        if (target == "money") {
                            naiseiName.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "金";
                        }
                        else if (target == "hyourou") {
                            naiseiName.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "兵糧";
                        }
                        else if (target == "YR") {
                            naiseiName.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "槍刀素材";
                        }
                        else if (target == "KB") {
                            naiseiName.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "騎馬素材";
                        }
                        else if (target == "TP") {
                            naiseiName.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "鉄砲素材";
                        }
                        else if (target == "YM") {
                            naiseiName.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "弓矢素材";
                        }
                        else if (target == "nbn") {
                            naiseiName.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "基督教";
                        }
                        else if (target == "bnk") {
                            naiseiName.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "文化";
                        }
                        else if (target == "snb") {
                            naiseiName.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "忍";
                        }
                        else if (target == "bky") {
                            naiseiName.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "仏教";
                        }
                        else if (target == "child") {
                            naiseiName.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "足軽兵数";
                        }
                        else if (target == "dfc") {
                            naiseiName.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "防備";
                        }
                    }
					//Label & Effect Value Adjustment 
					if(code != "kzn"){
						if(code != "yr" &&code != "kb"&&code != "tp" &&code != "ym" && code != "snb"){
							if(code != "nbn" && code != "kgy" && code != "bky" && code !="hsy" && code !="trd"){
								naiseiName.transform.FindChild ("NaiseiEffectValue").GetComponent<Text> ().text = "+" + effect1;
							}else{
								naiseiName.transform.FindChild ("NaiseiUnit").GetComponent<Text>().enabled = false;
								naiseiName.transform.FindChild ("NaiseiEffectValue").GetComponent<Text> ().text = "+" + effect1;
							}
						}else{
							effect1 = effect1 * 2;
                            
                            //Rank


                            naiseiName.transform.FindChild ("NaiseiEffectValue").GetComponent<Text> ().text = "+" + effect1;
                            

						}
					}else{
						effect1 = effect1 * 4;
						naiseiName.transform.FindChild ("NaiseiEffectValue").GetComponent<Text> ().text = "+" + effect1;
					}

					naiseiName.transform.FindChild ("RequiredMoney").GetComponent<Text> ().text = naiseiMst.param [i].money1.ToString();
					naiseiName.transform.FindChild ("RequiredHyourou").GetComponent<Text> ().text = naiseiMst.param [i].hyourou.ToString();
					GameObject createButton = naiseiName.transform.FindChild("CreateButton").gameObject;
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
					slot.transform.SetParent (scroll.transform.FindChild ("NaiseiContent").transform);
					slot.transform.localScale = new Vector2 (1, 1);
					
					//Bldg
					GameObject naiseiName = slot.transform.FindChild ("NaiseiName").gameObject;
                    naiseiName.GetComponent<Text>().text = naiseiNameText;
                    string bldgPath = "Prefabs/Naisei/Bldg/" + naiseiMst.param [i].code + "_s";
					GameObject bldg = Instantiate (Resources.Load (bldgPath)) as GameObject;
					bldg.transform.SetParent (naiseiName.transform);
					RectTransform bldgTransform = bldg.GetComponent<RectTransform> ();
					bldgTransform.anchoredPosition = new Vector3 (0, -315, 0);
					bldg.transform.localScale = new Vector2 (3, 3);
					bldg.GetComponent<Button>().enabled = false;

                    //Some Value
                    naiseiName.transform.FindChild("NaiseiExp").GetComponent<Text>().text = naiseExpText;
                    string target = naiseiMst.param [i].target;
					int effect1 = naiseiMst.param [i].effect1;

					int techId = 0;
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        if (target == "KB") {
						    naiseiName.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Cavalry Item";
						    techId = 2;
					    }else if (target == "TP") {
						    naiseiName.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Gun Item";
						    techId = 1;
					    }else if (target == "snb") {
						    naiseiName.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Ninja";
						    techId = 3;
					    }
                    }else {
                        if (target == "KB") {
                            naiseiName.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "騎馬素材";
                            techId = 2;
                        }
                        else if (target == "TP") {
                            naiseiName.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "鉄砲素材";
                            techId = 1;
                        }
                        else if (target == "snb") {
                            naiseiName.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "忍";
                            techId = 3;
                        }
                    }
					//Label & Effect Value Adjustment 
					effect1 = effect1 * 2;
					naiseiName.transform.FindChild ("NaiseiEffectValue").GetComponent<Text> ().text = "+" + effect1;

					naiseiName.transform.FindChild ("RequiredHyourou").GetComponent<Text> ().text = naiseiMst.param [i].hyourou.ToString();
					GameObject createButton = naiseiName.transform.FindChild("CreateButton").gameObject;
					createButton.GetComponent<BuildNaiseiWithItem> ().techId = techId;
					createButton.GetComponent<BuildNaiseiWithItem> ().panelId = int.Parse(name);
					createButton.GetComponent<BuildNaiseiWithItem> ().naiseiId = i;
                    createButton.GetComponent<BuildNaiseiWithItem>().naiseiName = naiseiNameText;
                    createButton.GetComponent<BuildNaiseiWithItem> ().requiredHyourou = naiseiMst.param [i].hyourou;

				}
			}

            //tutorial
            if (Application.loadedLevelName == "tutorialNaisei") {

                GameObject.Find("board(Clone)").transform.FindChild("close").gameObject.SetActive(false);
                
                foreach (Transform child in scroll.transform.FindChild("NaiseiContent").transform) {
                    if(child.transform.FindChild("NaiseiName").transform.FindChild("CreateButton").GetComponent<BuildNaisei>().naiseiId != 1) {
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
            if (int.Parse(lv) == 20) {

                Message msg = new Message();
                msg.makeMessage(msg.getMessage(116));
                audioSources[4].Play();

            }else {
                
                BusyoStatusButton pop = new BusyoStatusButton();
                GameObject board = pop.commonPopup(21);
                audioSources[0].Play();

                //Update
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    GameObject.Find ("popText").GetComponent<Text> ().text = "Development";
                }else {
                    GameObject.Find("popText").GetComponent<Text>().text = "内政強化";
                }

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
                GameObject baseObj = NaiseiUpdate.transform.FindChild("Base").gameObject;
			    baseObj.transform.FindChild("FromLv").GetComponent<Text>().text = "Lv" + lv;
			    baseObj.transform.FindChild("ToLv").GetComponent<Text>().text = nextLv.ToString();

                //Rank
                string nowRank = "";
                string nxtRank = "";
                if(type == "yr" || type == "kb" || type == "tp" || type == "ym" || type == "kb" || type == "snb") {
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        if (int.Parse(lv) < 11) {
                            nowRank = "Low";
                        }else if (int.Parse(lv) < 16) {
                            nowRank = "Mid";
                        }else {
                            nowRank = "High";
                        }
                    }else {
                        if (int.Parse(lv) < 11) {
                            nowRank = "下";
                        }else if (int.Parse(lv) < 16) {
                            nowRank = "中";
                        }else {
                            nowRank = "上";
                        }
                    }
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        if ((int.Parse(lv) + 1) < 11) {
                            nxtRank = "Low";
                        }else if((int.Parse(lv) + 1) < 16){
                            nxtRank = "Mid";
                        }else {
                            nxtRank = "High";
                        }
                    }else {
                        if ((int.Parse(lv) + 1) < 11) {
                            nxtRank = "下";
                        }else if ((int.Parse(lv) + 1) < 16) {
                            nxtRank = "中";
                        }else {
                            nxtRank = "上";
                        }
                    }
                }

                baseObj.transform.FindChild("NowNaiseiEffectValue").GetComponent<Text>().text = nowRank + " +" + effect.ToString();
			    baseObj.transform.FindChild("NextNaiseiEffectValue").GetComponent<Text>().text = nxtRank + " +" + effectNextLv.ToString();
			    baseObj.transform.FindChild("RequiredMoney").GetComponent<Text>().text = moneyNextLv.ToString();
			    baseObj.transform.FindChild("RequiredHyourou").GetComponent<Text>().text = requiredHyourou.ToString();


			    if(type=="shiro"){
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        baseObj.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Defence";
                    }else {
                        baseObj.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "防備";
                    }
				    baseObj.transform.FindChild ("NaiseiUnit").GetComponent<Text> ().enabled = false;
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        baseObj.transform.FindChild ("BldgName").GetComponent<Text> ().text = "Castle";
                    }else {
                        baseObj.transform.FindChild("BldgName").GetComponent<Text>().text = "城";
                    }
				    baseObj.transform.FindChild ("NaiseiEffectLabel2").GetComponent<Text> ().enabled = true;
				    baseObj.transform.FindChild("NowNaiseiEffectValue2").GetComponent<Text>().enabled = true;
				    baseObj.transform.FindChild("NextNaiseiEffectValue2").GetComponent<Text>().enabled = true;
				    baseObj.transform.FindChild ("arrow2").GetComponent<Image> ().enabled = true;
				    baseObj.transform.FindChild("NowNaiseiEffectValue2").GetComponent<Text>().text = "+" + effect.ToString();
				    baseObj.transform.FindChild("NextNaiseiEffectValue2").GetComponent<Text>().text = "+" + effectNextLv.ToString();

                    //Special Castle Check
                    string nowQty = PlayerPrefs.GetString("shiro");
                    List<string> nowQtyList = new List<string>();
                    char[] delimiterChars = { ',' };
                    if (nowQty != "" && nowQty != null) {
                        //scroll view
                        string scrollPath = "Prefabs/Naisei/Shiro/ShiroScrollView";
                        GameObject scroll = Instantiate(Resources.Load(scrollPath)) as GameObject;
                        scroll.transform.SetParent(board.transform);
                        
                        GameObject content = scroll.transform.FindChild("Content").gameObject;
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
                                item.transform.FindChild("Qty").GetComponent<Text>().text = qty.ToString();

                                string name = shiro.getName(shiroId);
                                item.transform.FindChild("name").GetComponent<Text>().text = name;
                                imagePath = imagePath + shiroId;
                                item.transform.FindChild("image").GetComponent<Image>().sprite =
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

				    baseObj.transform.FindChild ("NaiseiEffectLabel2").GetComponent<Text> ().enabled = false;
				    baseObj.transform.FindChild ("NowNaiseiEffectValue2").GetComponent<Text> ().enabled = false;
				    baseObj.transform.FindChild ("arrow2").GetComponent<Image> ().enabled = false;
				    baseObj.transform.FindChild ("NextNaiseiEffectValue2").GetComponent<Text> ().enabled = false;

                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        if (type == "shop") {
					        baseObj.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Money";
					        baseObj.transform.FindChild ("BldgName").GetComponent<Text> ().text = "Store";

				        } else if (type == "ta") {
					        baseObj.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Stamina";
					        baseObj.transform.FindChild ("BldgName").GetComponent<Text> ().text = "Farm";

				        } else if (type == "yr") {
					        baseObj.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Spear Item";
					        baseObj.transform.FindChild ("BldgName").GetComponent<Text> ().text = "Sword Blacksmith";
					
				        } else if (type == "kb") {
					        baseObj.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Cavalry Item";
					        baseObj.transform.FindChild ("BldgName").GetComponent<Text> ().text = "Stable";
					
				        } else if (type == "tp") {
					        baseObj.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Gun Item";
					        baseObj.transform.FindChild ("BldgName").GetComponent<Text> ().text = "Gun Blacksmith";
					
				        } else if (type == "ym") {
					        baseObj.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Bow Item";
					        baseObj.transform.FindChild ("BldgName").GetComponent<Text> ().text = "Archery House";
					
				        }else if(type == "kzn"){
					        baseObj.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Money";
					        baseObj.transform.FindChild ("BldgName").GetComponent<Text> ().text = "Mine";
				
				        }else if(type == "nbn"){
					        baseObj.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Christian";
					        baseObj.transform.FindChild ("BldgName").GetComponent<Text> ().text = "Church";
					        baseObj.transform.FindChild ("NaiseiUnit").GetComponent<Text> ().enabled = false;
				
				        }else if(type == "kgy"){
					        baseObj.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Culture";
					        baseObj.transform.FindChild ("BldgName").GetComponent<Text> ().text = "Court Noble House";
					        baseObj.transform.FindChild ("NaiseiUnit").GetComponent<Text> ().enabled = false;
				
				        }else if(type == "snb"){
					        baseObj.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Ninja";
					        baseObj.transform.FindChild ("BldgName").GetComponent<Text> ().text = "Ninja Town";
				
				        }else if(type == "bky"){
					        baseObj.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Buddhism";
					        baseObj.transform.FindChild ("BldgName").GetComponent<Text> ().text = "Temple";
					        baseObj.transform.FindChild ("NaiseiUnit").GetComponent<Text> ().enabled = false;
				
				        }else if (type == "hsy") {
					        baseObj.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Additional Soldier";
					        baseObj.transform.FindChild ("BldgName").GetComponent<Text> ().text = "Barrack";
					        baseObj.transform.FindChild ("NaiseiUnit").GetComponent<Text> ().enabled = false;
				
				        }else if (type == "trd") {
					        baseObj.transform.FindChild ("NaiseiEffectLabel").GetComponent<Text> ().text = "Defence";
					        baseObj.transform.FindChild ("BldgName").GetComponent<Text> ().text = "Fort";
					        baseObj.transform.FindChild ("NaiseiUnit").GetComponent<Text> ().enabled = false;
				        }
                    }else {
                        if (type == "shop") {
                            baseObj.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "金";
                            baseObj.transform.FindChild("BldgName").GetComponent<Text>().text = "商人町";

                        }
                        else if (type == "ta") {
                            baseObj.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "兵糧";
                            baseObj.transform.FindChild("BldgName").GetComponent<Text>().text = "村落";

                        }
                        else if (type == "yr") {
                            baseObj.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "槍刀素材";
                            baseObj.transform.FindChild("BldgName").GetComponent<Text>().text = "刀鍛冶屋";

                        }
                        else if (type == "kb") {
                            baseObj.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "騎馬素材";
                            baseObj.transform.FindChild("BldgName").GetComponent<Text>().text = "厩舎";

                        }
                        else if (type == "tp") {
                            baseObj.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "鉄砲素材";
                            baseObj.transform.FindChild("BldgName").GetComponent<Text>().text = "鉄砲鍛冶屋";

                        }
                        else if (type == "ym") {
                            baseObj.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "弓矢素材";
                            baseObj.transform.FindChild("BldgName").GetComponent<Text>().text = "皮職人";

                        }
                        else if (type == "kzn") {
                            baseObj.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "金";
                            baseObj.transform.FindChild("BldgName").GetComponent<Text>().text = "鉱山";

                        }
                        else if (type == "nbn") {
                            baseObj.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "基督教";
                            baseObj.transform.FindChild("BldgName").GetComponent<Text>().text = "南蛮寺";
                            baseObj.transform.FindChild("NaiseiUnit").GetComponent<Text>().enabled = false;

                        }
                        else if (type == "kgy") {
                            baseObj.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "文化";
                            baseObj.transform.FindChild("BldgName").GetComponent<Text>().text = "公家館";
                            baseObj.transform.FindChild("NaiseiUnit").GetComponent<Text>().enabled = false;

                        }
                        else if (type == "snb") {
                            baseObj.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "忍";
                            baseObj.transform.FindChild("BldgName").GetComponent<Text>().text = "忍の里";

                        }
                        else if (type == "bky") {
                            baseObj.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "仏教";
                            baseObj.transform.FindChild("BldgName").GetComponent<Text>().text = "寺";
                            baseObj.transform.FindChild("NaiseiUnit").GetComponent<Text>().enabled = false;

                        }
                        else if (type == "hsy") {
                            baseObj.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "足軽兵数";
                            baseObj.transform.FindChild("BldgName").GetComponent<Text>().text = "兵舎";
                            baseObj.transform.FindChild("NaiseiUnit").GetComponent<Text>().enabled = false;

                        }
                        else if (type == "trd") {
                            baseObj.transform.FindChild("NaiseiEffectLabel").GetComponent<Text>().text = "防備";
                            baseObj.transform.FindChild("BldgName").GetComponent<Text>().text = "砦";
                            baseObj.transform.FindChild("NaiseiUnit").GetComponent<Text>().enabled = false;
                        }
                    }
			    }

			    //Button Setting
			    GameObject updateBtn = baseObj.transform.FindChild ("NaiseiUpdateButton").gameObject;
			    updateBtn.GetComponent<UpdateNaisei>().activeKuniId = GameObject.Find ("NaiseiController").GetComponent<NaiseiController>().activeKuniId;
			    updateBtn.GetComponent<UpdateNaisei>().requiredMoney = moneyNextLv;
			    updateBtn.GetComponent<UpdateNaisei>().requiredHyourou = requiredHyourou;
			    updateBtn.GetComponent<UpdateNaisei>().areaId = name;
			    updateBtn.GetComponent<UpdateNaisei>().naiseiId = naiseiId;
			    updateBtn.GetComponent<UpdateNaisei>().targetLv = nextLv;
			    updateBtn.GetComponent<UpdateNaisei>().naiseiName = naiseiName;
            

            }
        }
	}
}
