using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class Item : MonoBehaviour {

	Entity_item_mst Mst  = Resources.Load ("Data/item_mst") as Entity_item_mst;


	public string getRandomShigen(int shigenType, int shigenRank, int addQty){
		//shigenType KB,YR,TP,YM=1,2,3,4
		//shigenRank 下、中、上=1,2,3
		string shigenName = "";
		string rankName = "";

		List<string> itemList = new List<string> ();
		char[] delimiterChars = {','};

		string temp = "";
		if (shigenType == 1) {
			temp = "cyouheiKB";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                shigenName = "Cavalry";
            }else {
                shigenName = "馬素材";
            }
		}else if (shigenType == 2) {
			temp = "cyouheiYR";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                shigenName = "Spear";
            }else {
                shigenName = "槍素材";
            }
		}else if (shigenType == 3) {
			temp = "cyouheiTP";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                shigenName = "Gun";
            }else {
                shigenName = "鉄砲素材";
            }
		}else if (shigenType == 4) {
			temp = "cyouheiYM";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                shigenName = "Bow";
            }else {
                shigenName = "弓素材";
            }
		} 

		string itemString = PlayerPrefs.GetString (temp);
		itemList = new List<string> (itemString.Split (delimiterChars));
		string newItemString = "";

		if(shigenRank == 1){
			int itemQty = int.Parse(itemList[0]);
			itemQty = itemQty + addQty;
			newItemString = itemQty + "," + itemList[1] + "," + itemList[2];
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                rankName = "Low ";
            }else {
                rankName = "下級";
            }

		}else if(shigenRank == 2){
			int itemQty = int.Parse(itemList[1]);
			itemQty = itemQty + addQty;
			newItemString =  itemList[0] + "," + itemQty + "," + itemList[2];
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                rankName = "Mid ";
            }else {
                rankName = "中級";
            }
		}else if(shigenRank == 3){
			int itemQty = int.Parse(itemList[2]);
			itemQty = itemQty + addQty;
			newItemString = itemList[0] +  "," + itemList[1] + "," + itemQty;
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                rankName = "High ";
            }else {
                rankName = "上級";
            }
		}
        if(newItemString != "") {
		    PlayerPrefs.SetString (temp,newItemString);
		    PlayerPrefs.Flush ();
        }
        shigenName = rankName + shigenName;
		return shigenName;

	}

	
	public int getEffect (string itemCode) {
		int effect=0;

		for(int i=0; i<Mst.param.Count; i++){
			string itemCodeOnMst = Mst.param[i].itemCode;

			if(itemCodeOnMst==itemCode){
				effect = Mst.param [i].effect;
				break;
			}
		}
		return effect;
	}

	public int getUnitPrice (string itemCode) {
		int unitPrice=0;
		
		for(int i=0; i<Mst.param.Count; i++){
			string itemCodeOnMst = Mst.param[i].itemCode;
			
			if(itemCodeOnMst==itemCode){
				unitPrice = Mst.param [i].buy;
				break;
			}
		}
		return unitPrice;
	}

	public string getExplanation (string itemCode) {
		string exp="";
		
		for(int i=0; i<Mst.param.Count; i++){
			string itemCodeOnMst = Mst.param[i].itemCode;
			
			if(itemCodeOnMst==itemCode){
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    exp = Mst.param [i].itemExpEng;
                }else {
                    exp = Mst.param[i].itemExp;
                }
				break;
			}
		}
		return exp;
	}

	public string getItemName(string itemCode){
		string itemName = "";

		
		for(int i=0; i<Mst.param.Count; i++){
			string MstItemCode = Mst.param [i].itemCode;
			if(itemCode == MstItemCode ){
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    itemName =Mst.param[i].itemNameEng;
                }else {
                    itemName = Mst.param[i].itemName;
                }
			}
		}
		return itemName;
	}


}
