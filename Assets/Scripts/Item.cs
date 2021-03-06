﻿using UnityEngine;
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
        int langId = PlayerPrefs.GetInt("langId");
        Message Message = new Message();
        string temp = "";
		if (shigenType == 1) {
			temp = "cyouheiKB";
            shigenName = Message.getMessage(172, langId);
		}else if (shigenType == 2) {
			temp = "cyouheiYR";
            shigenName = Message.getMessage(171, langId);
		}else if (shigenType == 3) {
			temp = "cyouheiTP";
            shigenName = Message.getMessage(173, langId);            
		}else if (shigenType == 4) {
			temp = "cyouheiYM";
            shigenName = Message.getMessage(174, langId);
		} 

		string itemString = PlayerPrefs.GetString (temp);
		itemList = new List<string> (itemString.Split (delimiterChars));
		string newItemString = "";

		if(shigenRank == 1){
			int itemQty = int.Parse(itemList[0]);
			itemQty = itemQty + addQty;
			newItemString = itemQty + "," + itemList[1] + "," + itemList[2];
            if (langId == 2) {
                rankName = "Low ";
            }else if(langId==3) {
                rankName = "下级";
            }
            else {
                rankName = "下級";
            }

		}else if(shigenRank == 2){
			int itemQty = int.Parse(itemList[1]);
			itemQty = itemQty + addQty;
			newItemString =  itemList[0] + "," + itemQty + "," + itemList[2];
            if (langId == 2) {
                rankName = "Mid ";
            }
            else if (langId == 3) {
                rankName = "中级";
            }
            else {
                rankName = "中級";
            }
		}else if(shigenRank == 3){
			int itemQty = int.Parse(itemList[2]);
			itemQty = itemQty + addQty;
			newItemString = itemList[0] +  "," + itemList[1] + "," + itemQty;
            if (langId == 2) {
                rankName = "High ";
            }
            else if (langId == 3) {
                rankName = "上级";
            }
            else {
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

        int langId = PlayerPrefs.GetInt("langId");
        for (int i=0; i<Mst.param.Count; i++){
			string itemCodeOnMst = Mst.param[i].itemCode;
			
			if(itemCodeOnMst==itemCode){
                if (langId == 2) {
                    exp = Mst.param [i].itemExpEng;
                }else if (langId == 3) {
                    exp = Mst.param[i].itemExpSChn;
                } else {
                    exp = Mst.param[i].itemExp;
                }
				break;
			}
		}
		return exp;
	}

	public string getItemName(string itemCode){
		string itemName = "";

        int langId = PlayerPrefs.GetInt("langId");
        for (int i=0; i<Mst.param.Count; i++){
			string MstItemCode = Mst.param [i].itemCode;
			if(itemCode == MstItemCode ){
                if (langId == 2) {
                    itemName =Mst.param[i].itemNameEng;
                }else if (langId == 3) {
                    itemName = Mst.param[i].itemNameSChn;
                }else {
                    itemName = Mst.param[i].itemName;
                }
			}
		}
		return itemName;
	}


}
