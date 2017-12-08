using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System.Collections.Generic;

public class SyouninMenu : MonoBehaviour {

	public void OnClick(){

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
        int langId = PlayerPrefs.GetInt("langId");
        GameObject board = GameObject.Find ("SyouninBoard").gameObject;
		Message msg = new Message (); 
		GameObject actionValue = GameObject.Find ("ActionValue").gameObject;
		int actionRemainQty = int.Parse(actionValue.GetComponent<Text> ().text);
		char[] delimiterChars = {','};

		if (actionRemainQty <= 0) {
			audioSources [4].Play ();
			msg.makeMessage (msg.getMessage(42));
			
			serihuChanger (msg.getMessage(43));
			
		} else{

			CloseLayer CloseLayerScript = GameObject.Find ("CloseSyoukaijyo").GetComponent<CloseLayer>();

			if (name == "Kahou") {
				audioSources [0].Play ();

				string path = "Prefabs/Syounin/MenuKahou";
				GameObject menu = Instantiate (Resources.Load (path)) as GameObject;
				menu.transform.SetParent (board.transform);
				menu.transform.localScale = new Vector2 (1, 1);
				menu.transform.localPosition = new Vector2 (0, -150);
				menu.transform.Find ("Close").GetComponent<CloseMenu> ().obj = menu;
				menu.name = "MenuKahou";

				//Product Show
				string kahouCdString = CloseLayerScript.kahouCdString;
				List<string> kahouCdList = new List<string> ();
				kahouCdList = new List<string> (kahouCdString.Split (delimiterChars));

				string kahouIdString = CloseLayerScript.kahouIdString;
				List<string> kahouIdList = new List<string> ();
				kahouIdList = new List<string> (kahouIdString.Split (delimiterChars));

				GameObject content = menu.transform.Find("ScrollView").transform.Find("Content").gameObject;
				GameObject money = menu.transform.Find("MoneyValue").gameObject;
				GameObject btn = menu.transform.Find("DoKahouButton").gameObject;

				KahouStatusGet kahouSts = new KahouStatusGet();

				for(int i=0; i<kahouCdList.Count; i++){
					string kahouCd = kahouCdList[i];
					int kahouId = int.Parse(kahouIdList[i]);
					string kahouCdId = kahouCd + kahouId;

					string kahouPath = "Prefabs/Item/Kahou/" + kahouCdId;
					int tmp = i + 1;
					string slotName = "item" + tmp.ToString(); 
					GameObject itemSlot = content.transform.Find(slotName).gameObject;
					itemSlot.GetComponent<SyouninProductSelect>().Content = content;

					//status
					List<string> kahouStsList = new List<string> ();
					kahouStsList = kahouSts.getKahouInfo(kahouCd, kahouId);
					itemSlot.GetComponent<SyouninProductSelect>().kahouName = kahouStsList[0];
					itemSlot.GetComponent<SyouninProductSelect>().kahouEffectLabel = kahouStsList[2];
					itemSlot.GetComponent<SyouninProductSelect>().kahouEffectValue = kahouStsList[3];
					float price = float.Parse(kahouStsList[5]);
					float discount = CloseLayerScript.discount;
					float finalPrice = price * discount;
					itemSlot.GetComponent<SyouninProductSelect>().price = Mathf.CeilToInt(finalPrice);
					itemSlot.GetComponent<SyouninProductSelect>().Money = money;
					itemSlot.GetComponent<SyouninProductSelect>().Btn = btn;
					itemSlot.GetComponent<SyouninProductSelect>().menuName = name;

					itemSlot.GetComponent<SyouninProductSelect>().kahouCd = kahouCd;
					itemSlot.GetComponent<SyouninProductSelect>().kahouId = kahouId;

					//kahou icon
					GameObject kahouObj = Instantiate (Resources.Load (kahouPath)) as GameObject;
					kahouObj.transform.SetParent (itemSlot.transform);
					kahouObj.transform.localScale = new Vector2 (1, 1);
					kahouObj.GetComponent<Button>().enabled = false;

					//Adjust
					RectTransform rect = kahouObj.transform.Find("Rank").GetComponent<RectTransform>();
					rect.anchoredPosition3D = new Vector3(20,-30,0);
					kahouObj.transform.Find("Rank").localScale = new Vector3(0.3f,0.3f,0);
				}

				//Initial
				content.transform.Find("item1").GetComponent<SyouninProductSelect>().OnClick();
				serihuChanger (msg.getMessage(44));

			}else if(name == "Busshi"){
				audioSources [0].Play ();

				string path = "Prefabs/Syounin/MenuBusshi";
				GameObject menu = Instantiate (Resources.Load (path)) as GameObject;
				menu.transform.SetParent (board.transform);
				menu.transform.localScale = new Vector2 (1, 1);
				menu.transform.localPosition = new Vector2 (0, -150);
				menu.transform.Find ("Close").GetComponent<CloseMenu> ().obj = menu;
				menu.name = "MenuBusshi";

				GameObject content = menu.transform.Find("ScrollView").transform.Find("Content").gameObject;

				//Prepared Value
				string busshiQtyString = CloseLayerScript.busshiQtyString;
				List<string> busshiQtyList = new List<string> ();
				busshiQtyList = new List<string> (busshiQtyString.Split (delimiterChars));
				
				string busshiRankString = CloseLayerScript.busshiRankString;
				List<string> busshiRankList = new List<string> ();
				busshiRankList = new List<string> (busshiRankString.Split (delimiterChars));

				Item item = new Item();
				float discount = CloseLayerScript.discount;

				//YR
				string YRpath = "Prefabs/Item/Cyouhei/CyouheiYR";
				GameObject YRObj = Instantiate (Resources.Load (YRpath)) as GameObject;
				GameObject item1 = content.transform.Find("item1").gameObject;
				YRObj.transform.SetParent (item1.transform);
				YRObj.transform.localScale = new Vector2 (1, 1);
				YRObj.GetComponent<Button>().enabled = false;
				YRObj.transform.Find("Qty").GetComponent<Text>().text = busshiQtyList[0];
				YRObj.name = "CyouheiYR";
                YRObj.transform.Find("Qty").transform.localScale = new Vector3(0.3f,0.35f,0);
                YRObj.transform.Find("Qty").transform.localPosition = new Vector3(0, -30, 0);                
                colorByRankChanger(YRObj, busshiRankList[0]);

				string itemCdYR = YRObj.name + busshiRankList[0];
				float unitPriceYR = (float)item.getUnitPrice(itemCdYR);
				float finalPriceYR = unitPriceYR * discount;
				item1.GetComponent<SyouninProductSelect>().price = Mathf.CeilToInt(finalPriceYR);
				item1.GetComponent<SyouninProductSelect>().busshiQty = int.Parse(busshiQtyList[0]);
				item1.GetComponent<SyouninProductSelect>().menuName = name;
				item1.GetComponent<SyouninProductSelect>().busshiCd = itemCdYR;

				//KB
				string KBpath = "Prefabs/Item/Cyouhei/CyouheiKB";
				GameObject KBObj = Instantiate (Resources.Load (KBpath)) as GameObject;
				GameObject item2 = content.transform.Find("item2").gameObject;
				KBObj.transform.SetParent (item2.transform);
				KBObj.transform.localScale = new Vector2 (1, 1);
				KBObj.GetComponent<Button>().enabled = false;
				KBObj.transform.Find("Qty").GetComponent<Text>().text = busshiQtyList[1];
				KBObj.name = "CyouheiKB";
                KBObj.transform.Find("Qty").transform.localScale = new Vector3(0.3f, 0.35f, 0);
                KBObj.transform.Find("Qty").transform.localPosition = new Vector3(0, -30, 0);
                colorByRankChanger(KBObj, busshiRankList[1]);

				string itemCdKB = KBObj.name + busshiRankList[1];
				float unitPriceKB = (float)item.getUnitPrice(itemCdKB);
				float finalPriceKB = unitPriceKB * discount;
				item2.GetComponent<SyouninProductSelect>().price = Mathf.CeilToInt(finalPriceKB);
				item2.GetComponent<SyouninProductSelect>().busshiQty = int.Parse(busshiQtyList[1]);
				item2.GetComponent<SyouninProductSelect>().menuName = name;
				item2.GetComponent<SyouninProductSelect>().busshiCd = itemCdKB;


				//TP
				string TPpath = "Prefabs/Item/Cyouhei/CyouheiTP";
				GameObject TPObj = Instantiate (Resources.Load (TPpath)) as GameObject;
				GameObject item3 = content.transform.Find("item3").gameObject;
				TPObj.transform.SetParent (item3.transform);
				TPObj.transform.localScale = new Vector2 (1, 1);
				TPObj.GetComponent<Button>().enabled = false;
				TPObj.transform.Find("Qty").GetComponent<Text>().text = busshiQtyList[2];
				TPObj.name = "CyouheiTP";
                TPObj.transform.Find("Qty").transform.localScale = new Vector3(0.3f, 0.35f, 0);
                TPObj.transform.Find("Qty").transform.localPosition = new Vector3(0, -30, 0);
                colorByRankChanger(TPObj, busshiRankList[2]);

				string itemCdTP = TPObj.name + busshiRankList[2];
				float unitPriceTP = (float)item.getUnitPrice(itemCdTP);
				float finalPriceTP = unitPriceTP * discount;
				item3.GetComponent<SyouninProductSelect>().price = Mathf.CeilToInt(finalPriceTP);
				item3.GetComponent<SyouninProductSelect>().busshiQty = int.Parse(busshiQtyList[2]);
				item3.GetComponent<SyouninProductSelect>().menuName = name;
				item3.GetComponent<SyouninProductSelect>().busshiCd = itemCdTP;


				//YM
				string YMath = "Prefabs/Item/Cyouhei/CyouheiYM";
				GameObject YMObj = Instantiate (Resources.Load (YMath)) as GameObject;
				GameObject item4 = content.transform.Find("item4").gameObject;
				YMObj.transform.SetParent (item4.transform);
				YMObj.transform.localScale = new Vector2 (1, 1);
				YMObj.GetComponent<Button>().enabled = false;
				YMObj.transform.Find("Qty").GetComponent<Text>().text = busshiQtyList[3];
				YMObj.name = "CyouheiYM";
                YMObj.transform.Find("Qty").transform.localScale = new Vector3(0.3f, 0.35f, 0);
                YMObj.transform.Find("Qty").transform.localPosition = new Vector3(0, -30, 0);
                colorByRankChanger(YMObj, busshiRankList[3]);

				string itemCdYM = YMObj.name + busshiRankList[3];
				float unitPriceYM = (float)item.getUnitPrice(itemCdYM);
				float finalPriceYM = unitPriceYM * discount;
				item4.GetComponent<SyouninProductSelect>().price = Mathf.CeilToInt(finalPriceYM);
				item4.GetComponent<SyouninProductSelect>().busshiQty = int.Parse(busshiQtyList[3]);
				item4.GetComponent<SyouninProductSelect>().menuName = name;
				item4.GetComponent<SyouninProductSelect>().busshiCd = itemCdYM;


				//SNB
				string SNBpath = "Prefabs/Item/Shinobi/Shinobi";
				GameObject SNBObj = Instantiate (Resources.Load (SNBpath)) as GameObject;
				GameObject item5 = content.transform.Find("item5").gameObject;
				SNBObj.transform.SetParent (item5.transform);
				SNBObj.transform.localScale = new Vector2 (1, 1);
				SNBObj.GetComponent<Button>().enabled = false;
				SNBObj.transform.Find("Qty").GetComponent<Text>().text = busshiQtyList[4];
				SNBObj.name = "Shinobi";
                SNBObj.transform.Find("Qty").transform.localScale = new Vector3(0.3f, 0.35f, 0);
                SNBObj.transform.Find("Qty").transform.localPosition = new Vector3(0, -30, 0);
                RectTransform trnContent = SNBObj.transform.Find("Shinobi").GetComponent<RectTransform>();
				trnContent.sizeDelta = new Vector3(95,120,0);
				colorByRankChanger(SNBObj, busshiRankList[4]);

				string itemCdSNB = SNBObj.name + busshiRankList[4];
				float unitPriceSNB = (float)item.getUnitPrice(itemCdSNB);
				float finalPriceSNB = unitPriceSNB * discount;
				item5.GetComponent<SyouninProductSelect>().price = Mathf.CeilToInt(finalPriceSNB);
				item5.GetComponent<SyouninProductSelect>().busshiQty = int.Parse(busshiQtyList[4]);
				item5.GetComponent<SyouninProductSelect>().menuName = name;
				item5.GetComponent<SyouninProductSelect>().busshiCd = itemCdSNB;

				//Initial
				item1.GetComponent<SyouninProductSelect>().OnClick();

				serihuChanger (msg.getMessage(45));

			}else if(name == "Yasen"){
				audioSources [0].Play ();

				string path = "Prefabs/Syounin/MenuYasen";
				GameObject menu = Instantiate (Resources.Load (path)) as GameObject;
				menu.transform.SetParent (board.transform);
				menu.transform.localScale = new Vector2 (1, 1);
				menu.transform.localPosition = new Vector2 (0, -150);
				menu.transform.Find ("Close").GetComponent<CloseMenu> ().obj = menu;
				menu.name = "MenuYasen";

				int yasenAmt = CloseLayerScript.yasenAmt;
				menu.transform.Find("MoneyValue").GetComponent<Text>().text = yasenAmt.ToString();
				GameObject btn = menu.transform.Find("DoYasenButton").gameObject;
				btn.GetComponent<DoSyouninMenu>().price = yasenAmt;

				serihuChanger (msg.getMessage(46));

			}else if(name == "Youjinbou"){
				audioSources [0].Play ();

				string path = "Prefabs/Syounin/MenuRounin";
				GameObject menu = Instantiate (Resources.Load (path)) as GameObject;
				menu.transform.SetParent (board.transform);
				menu.transform.localScale = new Vector2 (1, 1);
				menu.transform.localPosition = new Vector2 (0, -150);
				menu.transform.Find ("Close").GetComponent<CloseMenu> ().obj = menu;
				menu.name = "MenuRounin";

				//Preparation
				float discount = CloseLayerScript.discount;
				int rdmKengouId = CloseLayerScript.rdmKengouId;
				GameObject btn = menu.transform.Find("DoRouninButton").gameObject;

				string kengouPath = "Prefabs/Item/kengou";
				GameObject kengou = Instantiate (Resources.Load (kengouPath)) as GameObject;
				kengou.transform.SetParent (menu.transform);
				kengou.transform.localScale = new Vector2 (1.0f, 1.25f);
				kengou.GetComponent<Button>().enabled = false;

				RectTransform kengouRect = kengou.GetComponent<RectTransform>();
				kengouRect.anchoredPosition3D = new Vector3(-200,-30,0);

				GameObject rank = kengou.transform.Find("Rank").gameObject;
				RectTransform kengouRankRect = rank.GetComponent<RectTransform>();
				kengouRankRect.anchoredPosition3D = new Vector3(-50,20,0);
				rank.transform.localScale = new Vector2(0.09f, 0.09f);

				Item item = new Item();
				string itemCd = "kengou" + rdmKengouId;
				string kengouName = item.getItemName(itemCd);
				string exp = item.getExplanation(itemCd);
				float unitPrice = (float)item.getUnitPrice(itemCd);
				rank.GetComponent<Text>().text = kengouName;

				float finalPrice = unitPrice * discount;
				btn.GetComponent<DoSyouninMenu>().price = Mathf.CeilToInt(finalPrice);
				btn.GetComponent<DoSyouninMenu>().kengouId = rdmKengouId;

				GameObject info = menu.transform.Find("Info").gameObject;
				info.transform.Find("Name").GetComponent<Text>().text = kengouName;
				info.transform.Find("EffectLabel").GetComponent<Text>().text = exp;
				menu.transform.Find("MoneyValue").GetComponent<Text>().text = Mathf.CeilToInt(finalPrice).ToString();
				serihuChanger (msg.getMessage(47));

			}else if(name == "Cyakai"){
				
				//check cyadougu
				List<string> kahouList = new List<string>();
				int myDaimyoId = PlayerPrefs.GetInt("myDaimyo");
                int senarioId = PlayerPrefs.GetInt("senarioId");
                Daimyo Daimyo = new Daimyo();
                int daimyoBusyoId = Daimyo.getDaimyoBusyoId(myDaimyoId, senarioId);
                BusyoInfoGet busyoInfo = new BusyoInfoGet ();
				string daimyoBusyoName = busyoInfo.getName (daimyoBusyoId,langId);

				string tmp = "kahou" + daimyoBusyoId;
				string kahouString = PlayerPrefs.GetString (tmp);
				kahouList = new List<string>(kahouString.Split (delimiterChars));
				if (kahouList [4] == "0" && kahouList [5] == "0") {
					audioSources [4].Play ();
                    string text = "";
                    if (langId == 2) {
                        text = "Lord " + daimyoBusyoName + " don't have any teaware.";
                    }else {
                        text = daimyoBusyoName + "様は\n茶器をお持ちでないようですな。";
                    }
                    msg.makeMessageOnBoard (text);
					serihuChanger (text);

				} else {
					audioSources [0].Play ();
					string path = "Prefabs/Syounin/MenuCyakai";
					GameObject menu = Instantiate (Resources.Load (path)) as GameObject;
					menu.transform.SetParent (board.transform);
					menu.transform.localScale = new Vector2 (1, 1);
					menu.transform.localPosition = new Vector2 (0, -150);
					menu.transform.Find ("Close").GetComponent<CloseMenu> ().obj = menu;
					menu.name = "MenuCyakai";
					GameObject btn = menu.transform.Find("DoCyakaiButton").gameObject;

					//Cyadougu History
					string cyakaiDouguHst = PlayerPrefs.GetString("cyakaiDouguHst");
					List<string> cyakaiDouguHstlist = new List<string> ();
					if (cyakaiDouguHst != "" && cyakaiDouguHst != null) {
						if (cyakaiDouguHst.Contains (",")) {
							cyakaiDouguHstlist = new List<string> (cyakaiDouguHst.Split (delimiterChars));
						} else {
							cyakaiDouguHstlist.Add (cyakaiDouguHst);
						}
					}


					//Change Kahou Icon
					bool doneCyadouguFlg1 = false;
					bool doneCyadouguFlg2 = false;

					int targetKuniQty = 1;//base
					Kahou kahou = new Kahou ();
					if (kahouList [4] != "0") {
						string kahouId1 = kahouList [4];

						string cyadouguId = "cyadougu" + kahouId1;
						string cyadouguPath = "Prefabs/Item/Kahou/" + cyadouguId;
						GameObject cya = Instantiate (Resources.Load (cyadouguPath)) as GameObject;
						cya.transform.SetParent(menu.transform);
						cya.transform.localScale = new Vector3 (0.3f, 0.38f, 0);
						RectTransform transform = cya.GetComponent<RectTransform> ();
						transform.anchoredPosition3D = new Vector3 (-180, -60, 0);
						cya.GetComponent<Button> ().enabled = false;

						if (cyakaiDouguHstlist.Contains (kahouId1)) {
							doneCyadouguFlg1 = true;
						} else {
							cyakaiDouguHstlist.Add (kahouId1);
						}

						string kahouRank = kahou.getKahouRank ("cyadougu",int.Parse(kahouId1));
						if (!doneCyadouguFlg1) {
							if (kahouRank == "S") {
								targetKuniQty = targetKuniQty + 2;
							} else if (kahouRank == "A") {
								targetKuniQty = targetKuniQty + 1;
							} 
						}


					} else {
						//Not Exist
						string cyadouguPath = "Prefabs/Item/Kahou/NoCyadougu";
						GameObject cya = Instantiate (Resources.Load (cyadouguPath)) as GameObject;
						cya.transform.SetParent(menu.transform);
						cya.transform.localScale = new Vector3 (0.3f, 0.38f, 0);
						RectTransform transform = cya.GetComponent<RectTransform> ();
						transform.anchoredPosition3D = new Vector3 (-180, -60, 0);
						cya.GetComponent<Button> ().enabled = false;
					}

					if (kahouList [5] != "0") {
						string kahouId2 = kahouList [5];

						string cyadouguId = "cyadougu" + kahouId2;
						string cyadouguPath = "Prefabs/Item/Kahou/" + cyadouguId;
						GameObject cya = Instantiate (Resources.Load (cyadouguPath)) as GameObject;
						cya.transform.SetParent(menu.transform);
						cya.transform.localScale = new Vector3 (0.3f, 0.38f, 0);
						RectTransform transform = cya.GetComponent<RectTransform> ();
						transform.anchoredPosition3D = new Vector3 (-130, -60, 0);
						cya.GetComponent<Button> ().enabled = false;

						if (cyakaiDouguHstlist.Contains (kahouId2)) {
							doneCyadouguFlg2 = true;
						} else {
							cyakaiDouguHstlist.Add (kahouId2);
						}

						string kahouRank = kahou.getKahouRank ("cyadougu",int.Parse(kahouId2));
                        if (!doneCyadouguFlg1) {
                            if (kahouRank == "S") {
                                targetKuniQty = targetKuniQty + 2;
                            }else if (kahouRank == "A") {
                                targetKuniQty = targetKuniQty + 1;
                            }
                        }


                    } else {
						//Not Exist
						string cyadouguPath = "Prefabs/Item/Kahou/NoCyadougu";
						GameObject cya = Instantiate (Resources.Load (cyadouguPath)) as GameObject;
						cya.transform.SetParent(menu.transform);
						cya.transform.localScale = new Vector3 (0.3f, 0.38f, 0);
						RectTransform transform = cya.GetComponent<RectTransform> ();
						transform.anchoredPosition3D = new Vector3 (-130, -60, 0);
						cya.GetComponent<Button> ().enabled = false;
					}

					//Change Kuni Qty
					if(targetKuniQty < 1){
						targetKuniQty = 1;
					}

					menu.transform.Find("Tabibito").transform.Find("Text").GetComponent<Text>().text = " X "+targetKuniQty.ToString(); 


					//Set Button Value
					btn.GetComponent<DoSyouninMenu>().price = 10000;
					btn.GetComponent<DoSyouninMenu>().targetKuniQty = targetKuniQty;
					btn.GetComponent<DoSyouninMenu>().cyakaiDouguHstlist = cyakaiDouguHstlist;
					string serihu = "";
					if (!doneCyadouguFlg1 && !doneCyadouguFlg2) {
						btn.GetComponent<DoSyouninMenu> ().doneCyadouguFlg = false;
                        serihu = msg.getMessage(48);
					} else {
						btn.GetComponent<DoSyouninMenu> ().doneCyadouguFlg = true;
						serihu = msg.getMessage(49);
                    }
					serihuChanger (serihu);

				}
			}else if(name == "Gijyutsu"){
				audioSources [0].Play ();

				string path = "Prefabs/Syounin/MenuTech";
				GameObject menu = Instantiate (Resources.Load (path)) as GameObject;
				menu.transform.SetParent (board.transform);
				menu.transform.localScale = new Vector2 (1, 1);
				menu.transform.localPosition = new Vector2 (0, -150);
				menu.transform.Find ("Close").GetComponent<CloseMenu> ().obj = menu;
				menu.name = "MenuTech";
				GameObject btn = menu.transform.Find("DoTechButton").gameObject;

				int techId = CloseLayerScript.techId;

				//Image
				string spritePath = "";
				if(techId == 1){
					spritePath = "Prefabs/Item/Tech/Sprite/tp";
				}else if(techId == 2){
					spritePath = "Prefabs/Item/Tech/Sprite/kb";
				}else if(techId == 3){
					spritePath = "Prefabs/Item/Tech/Sprite/snb";
				}
				GameObject techImage = menu.transform.Find("Tech").gameObject;
				techImage.GetComponent<Image> ().sprite = 
					Resources.Load (spritePath, typeof(Sprite)) as Sprite;

				//Detail Info
				Item item = new Item();
				string itemCd = "tech" + techId;
				string techName = item.getItemName(itemCd);
				string exp = item.getExplanation(itemCd);
				float unitPrice = (float)item.getUnitPrice(itemCd);
				float discount = CloseLayerScript.discount;

				float finalPrice = unitPrice * discount;
				btn.GetComponent<DoSyouninMenu>().price = Mathf.CeilToInt(finalPrice);
				btn.GetComponent<DoSyouninMenu>().techId = techId;
				
				GameObject info = menu.transform.Find("Info").gameObject;
				info.transform.Find("Name").GetComponent<Text>().text = techName;
				info.transform.Find("EffectLabel").GetComponent<Text>().text = exp;
				menu.transform.Find("MoneyValue").GetComponent<Text>().text = Mathf.CeilToInt(finalPrice).ToString();
				serihuChanger (msg.getMessage(50));

			}


		}
	}

	public void serihuChanger(string serihu){
		GameObject.Find("Serihu").transform.Find("Text").GetComponent<Text>().text = serihu;
	}

	public void colorByRankChanger(GameObject obj, string rank){

		Color lowColor = new Color (0f / 255f, 0f / 255f, 219f / 255f, 255f / 255f);
		Color midColor = new Color (94f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
		Color highColor = new Color (84f / 255f, 103f / 255f, 0f / 255f, 255f / 255f);
        int langId = PlayerPrefs.GetInt("langId");
        if (rank=="1"){
			obj.GetComponent<Image>().color = lowColor;
            if (obj.name == "CyouheiKB" || obj.name == "CyouheiTP" || obj.name == "CyouheiYR" || obj.name == "CyouheiYM") {
                
                if (langId == 2) {
                    obj.transform.Find("CyouheiRank").GetComponent<Text>().text = "Low";
                }
                else {
                    obj.transform.Find("CyouheiRank").GetComponent<Text>().text = "下";
                }
            }
            else if (obj.name == "Shinobi") {
                if (langId == 2) {
                    obj.transform.Find("ShinobiRank").GetComponent<Text>().text = "Low";
                }
                else {
                    obj.transform.Find("ShinobiRank").GetComponent<Text>().text = "下";
                }
            }
        }else if(rank=="2"){
			obj.GetComponent<Image>().color = midColor;
			if(obj.name == "CyouheiKB" || obj.name == "CyouheiTP"|| obj.name == "CyouheiYR" || obj.name =="CyouheiYM"){
                if (langId == 2) {
                    obj.transform.Find("CyouheiRank").GetComponent<Text>().text = "Mid";
                }else {
                    obj.transform.Find("CyouheiRank").GetComponent<Text>().text = "中";
                }
			}else if(obj.name == "Shinobi"){
                if (langId == 2) {
                    obj.transform.Find("ShinobiRank").GetComponent<Text>().text = "Mid";
                }else {
                    obj.transform.Find("ShinobiRank").GetComponent<Text>().text = "中";
                }
			}
			
		}else if(rank=="3"){
			obj.GetComponent<Image>().color = highColor;
			if(obj.name == "CyouheiKB" || obj.name == "CyouheiTP"|| obj.name == "CyouheiYR" || obj.name =="CyouheiYM"){
                if (langId == 2) {
                    obj.transform.Find("CyouheiRank").GetComponent<Text>().text = "High";
                }else {
                    obj.transform.Find("CyouheiRank").GetComponent<Text>().text = "上";
                }
			}else if(obj.name == "Shinobi"){
                if (langId == 2) {
                    obj.transform.Find("ShinobiRank").GetComponent<Text>().text = "High";
                }else {
                    obj.transform.Find("ShinobiRank").GetComponent<Text>().text = "上";
                }
			}
		}


		
	}

}
