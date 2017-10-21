using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class KouekiMenu : MonoBehaviour {

	public GameObject kouekiObj;
	public GameObject nowMenuObj;

	public void OnClick(){

		if (name == "Buy") {
			if(nowMenuObj.name != "BuyMenu"){
				clickBuy();
			}
		}
	}


	public void changeColor(string name){

		
	}

	public void clickBuy(){

		//Change Color
		Color onImageColor = new Color (25f / 255f, 25f / 255f, 25f / 255f, 255f / 255f);
		Color onTextColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
		//Color offImageColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 150f / 255f);
		//Color offTextColor = new Color (50f / 255f, 50f / 255f, 50f / 255f, 255f / 255f);

		GameObject buyMenu = kouekiObj.transform.FindChild ("Buy").gameObject;
		buyMenu.GetComponent<Image> ().color = onImageColor;
		buyMenu.transform.FindChild ("Text").GetComponent<Text> ().color = onTextColor;

		//GameObject changeMenu = kouekiObj.transform.FindChild ("Change").gameObject;
		//changeMenu.GetComponent<Image> ().color = offImageColor;
		//changeMenu.transform.FindChild ("Text").GetComponent<Text> ().color = offTextColor;


		//Instantiate Obj
		string buyPath = "Prefabs/Map/gaikou/BuyMenu";
		GameObject buyObj = Instantiate (Resources.Load (buyPath)) as GameObject;
		buyObj.transform.SetParent (kouekiObj.transform);
		buyObj.transform.localScale = new Vector3 (1, 1, 1);
		buyObj.name = "BuyMenu";

		//Set Now Obj
		buyMenu.GetComponent<KouekiMenu> ().nowMenuObj = buyObj;
		//changeMenu.GetComponent<KouekiMenu> ().nowMenuObj = buyObj;
		GameObject slider = buyObj.transform.FindChild("Slider").gameObject;
		slider.GetComponent<BuySlider> ().buyQtyValue = buyObj.transform.FindChild("Qty").gameObject;
		slider.GetComponent<BuySlider> ().PayMoneyValue = buyObj.transform.FindChild("MoneyAmt").gameObject;
		GameObject buyBtn = buyObj.transform.FindChild ("DoButton").gameObject;

		//Product List
		CloseBoard close = GameObject.Find ("close").GetComponent<CloseBoard> ();
		int yukoudo = close.yukoudo;
		bool doumeiFlg = close.doumeiFlg;

		string naiseiItem = close.naiseiItem;
		List<string> naiseiItemList = new List<string> ();
		char[] delimiterChars = {':'};
		if (naiseiItem != "null" && naiseiItem != "") {
			if (naiseiItem.Contains (":")) {
				naiseiItemList = new List<string> (naiseiItem.Split (delimiterChars));
			} else {
				naiseiItemList.Add (naiseiItem);
			}
		}
		
		GameObject content = kouekiObj.transform.FindChild("BuyMenu").transform.FindChild("ScrollView").transform.FindChild("Content").gameObject;
		int kuniQty = close.kuniQty;

		if(naiseiItemList.Count != 0){

			//item
			bool naiseiExistFlg = false;
			foreach(string naiseiName in naiseiItemList){
				if (naiseiName == "kb") {
					string itemPath = "Prefabs/Item/Cyouhei/CyouheiKB";
					showKouekiItem(kuniQty, content, itemPath, "CyouheiKB", yukoudo, slider, buyBtn, doumeiFlg);
					naiseiExistFlg = true;
				} else if (naiseiName == "tp") {
					string itemPath = "Prefabs/Item/Cyouhei/CyouheiTP";
					showKouekiItem(kuniQty, content, itemPath, "CyouheiTP", yukoudo, slider, buyBtn, doumeiFlg);
					naiseiExistFlg = true;
				} else if (naiseiName == "snb") {
					string itemPath = "Prefabs/Item/Shinobi/Shinobi";
					showKouekiItem(kuniQty, content, itemPath, "Shinobi", yukoudo, slider, buyBtn, doumeiFlg);
					naiseiExistFlg = true;
				} 
			}

			if(!naiseiExistFlg){
				showKouekiItem(kuniQty, content, "Prefabs/Item/Cyouhei/CyouheiYR", "CyouheiYR", yukoudo, slider, buyBtn, doumeiFlg);
				showKouekiItem(kuniQty, content, "Prefabs/Item/Cyouhei/CyouheiYM", "CyouheiYM", yukoudo, slider, buyBtn, doumeiFlg);
			}

			//hidensyo
			showKouekiItem(kuniQty, content, "Prefabs/Item/Hidensyo/Hidensyo", "Hidensyo", yukoudo, slider, buyBtn, doumeiFlg);
			
		}else{

			//No Naisei Item
			showKouekiItem(kuniQty, content, "Prefabs/Item/Cyouhei/CyouheiYR", "CyouheiYR", yukoudo, slider, buyBtn, doumeiFlg);
			showKouekiItem(kuniQty, content, "Prefabs/Item/Cyouhei/CyouheiYM", "CyouheiYM", yukoudo, slider, buyBtn, doumeiFlg);
			showKouekiItem(kuniQty, content, "Prefabs/Item/Hidensyo/Hidensyo", "Hidensyo", yukoudo, slider, buyBtn, doumeiFlg);

		}


		//Initialization
		foreach (Transform obj in content.transform) {
			obj.GetComponent<ItemInfo>().OnClick();
			break;
		}

	}

	public void showKouekiItem(int kuniQty, GameObject content, string itemPath, string itemCd, int yukoudo, GameObject slider, GameObject buyBtn, bool doumeiFlg){
		
		Color lowColor = new Color (0f / 255f, 0f / 255f, 219f / 255f, 255f / 255f);
		Color midColor = new Color (94f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
		Color highColor = new Color (84f / 255f, 103f / 255f, 0f / 255f, 255f / 255f);
		
		int rank = getItemRank(kuniQty);
        int langId = PlayerPrefs.GetInt("langId");
        
        for (int i=1;i<=rank;i++){
			string slotPath = "Prefabs/Map/gaikou/KouekiSlot";
			GameObject slot = Instantiate (Resources.Load (slotPath)) as GameObject;
			slot.transform.SetParent (content.transform);
			slot.transform.localScale = new Vector3 (1, 1, 1);
			
			GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
			item.transform.SetParent (slot.transform);
			item.transform.localScale = new Vector3 (1, 1, 1);
			
			if(i==1){
				item.GetComponent<Image>().color = lowColor;
                if (itemCd == "CyouheiKB" || itemCd == "CyouheiTP" || itemCd == "CyouheiYR" || itemCd == "CyouheiYM") {
                    if (langId == 2) {
                        item.transform.FindChild("CyouheiRank").GetComponent<Text>().text = "Low";
                    }else {
                        item.transform.FindChild("CyouheiRank").GetComponent<Text>().text = "下";
                    }

                }else if (itemCd == "Shinobi") {
                    if (langId == 2) {
                        item.transform.FindChild("ShinobiRank").GetComponent<Text>().text = "Low";
                    }else {
                        item.transform.FindChild("ShinobiRank").GetComponent<Text>().text = "下";
                    }
                }else if (itemCd == "Hidensyo") {
                    if (langId == 2) {
                        item.transform.FindChild("HidensyoRank").GetComponent<Text>().text = "Low";
                    }else {
                        item.transform.FindChild("HidensyoRank").GetComponent<Text>().text = "下";
                    }
                }
            }
            else if(i==2){
				item.GetComponent<Image>().color = midColor;
				if(itemCd == "CyouheiKB" || itemCd == "CyouheiTP"|| itemCd == "CyouheiYR" || itemCd =="CyouheiYM"){
                    if (langId == 2) {
                        item.transform.FindChild("CyouheiRank").GetComponent<Text>().text = "Mid";
                    }else {
                        item.transform.FindChild("CyouheiRank").GetComponent<Text>().text = "中";
                    }
                        
				}else if(itemCd == "Shinobi"){
                    if (langId == 2) {
                        item.transform.FindChild("ShinobiRank").GetComponent<Text>().text = "Mid";
                    }else {
                        item.transform.FindChild("ShinobiRank").GetComponent<Text>().text = "中";
                    }
				}else if(itemCd == "Hidensyo"){
                    if (langId == 2) {
                        item.transform.FindChild("HidensyoRank").GetComponent<Text>().text = "Mid";
                    }else { 
                        item.transform.FindChild("HidensyoRank").GetComponent<Text>().text = "中";
                    }
                }
				
			}else if(i==3){
				item.GetComponent<Image>().color = highColor;
				if(itemCd == "CyouheiKB" || itemCd == "CyouheiTP"|| itemCd == "CyouheiYR" || itemCd =="CyouheiYM"){
                    if (langId == 2) {
                        item.transform.FindChild("CyouheiRank").GetComponent<Text>().text = "High";
                    }else { 
                        item.transform.FindChild("CyouheiRank").GetComponent<Text>().text = "上";
                    }
                }
                else if(itemCd == "Shinobi"){
                    if (langId == 2) {
                        item.transform.FindChild("ShinobiRank").GetComponent<Text>().text = "High";
                    }else {
                        item.transform.FindChild("ShinobiRank").GetComponent<Text>().text = "上";
                    }
                }
                else if(itemCd == "Hidensyo"){
                    if (langId == 2) {
                        item.transform.FindChild("HidensyoRank").GetComponent<Text>().text = "High";
                    }else {
                        item.transform.FindChild("HidensyoRank").GetComponent<Text>().text = "上";
                    }
                }
			}
			
			//Common
			slot.name = itemCd + i.ToString();
			slot.GetComponent<ItemInfo>().yukoudo = yukoudo;
			
			//Adjust Price by Yukoudo
			List<string> itemInfoList = new List<string> ();
			ItemInfo itemScript = new ItemInfo();
			itemInfoList = itemScript.getItemInfo (slot.name);
			float unitPrice = float.Parse(itemInfoList[4]);
			float rate = 2 - (float)yukoudo/100;
			if(doumeiFlg){
				rate = rate - 0.2f;
			}
			int finalPrice = Mathf.CeilToInt(unitPrice * rate);
			slot.GetComponent<ItemInfo>().buyUnitPirce = finalPrice;
			slot.GetComponent<ItemInfo>().buySlider = slider;
			slot.GetComponent<ItemInfo>().Content = content;
			slot.GetComponent<ItemInfo>().buyBtn = buyBtn;

			//Size Adjustment
			if(itemCd == "Shinobi"){
				RectTransform rect = item.transform.FindChild("Shinobi").GetComponent<RectTransform>();
				rect.sizeDelta = new Vector2(100,120);
			}
			
			
		}	
	}

	public int getItemRank(int kuniQty){
		int rank = 0;
		if(5<=kuniQty){
			rank = 3;				
		}else if(3<=kuniQty && kuniQty<5){	
			rank = 2;		
		}else if(kuniQty<3){		
			rank = 1;
		}		
		return rank;
	}
}
