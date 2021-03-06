﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class DouguSoukoScene : MonoBehaviour {

	public void OnClick () {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();
        Message Message = new Message();

        /*Scene Change*/
        GameObject.Find ("GameScene").GetComponent<SoukoScene> ().currentTab = "DouguScene";

		//button color change
		Color pushedTabColor = new Color (118f / 255f, 103f / 255f, 16f / 255f, 255f / 255f);
		Color pushedTextColor = new Color (219f / 255f, 219f / 255f, 212f / 255f, 255f / 255f);
		Color normalTabColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
		Color normalTextColor = new Color (255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
		
		//Clear Color
		GameObject kahou = GameObject.Find ("Kahou");
		GameObject dougu = GameObject.Find ("Dougu");
		
		kahou.GetComponent<Image> ().color = normalTabColor;
		dougu.GetComponent<Image> ().color = pushedTabColor;

		kahou.transform.Find ("Text").GetComponent<Text> ().color = normalTextColor;
		dougu.transform.Find ("Text").GetComponent<Text> ().color = pushedTextColor;

		/*Initialize Kahou View*/
		GameObject itemView = GameObject.Find ("ItemView");
		itemView.transform.Find ("GetMoney").GetComponent<Image>().enabled = false;
		GameObject sellBtn = GameObject.Find ("SellButton");
		sellBtn.GetComponent<Image>().enabled = false;
		sellBtn.GetComponent<Button>().enabled = false;
		sellBtn.transform.Find("Text").GetComponent<Text>().enabled = false;
		itemView.transform.Find ("KahouEffectValue").GetComponent<Text> ().text = "";
		itemView.transform.Find ("KahouEffectLabel").GetComponent<Text> ().text = "";
		GameObject.Find ("GetMoneyValue").GetComponent<Text> ().text = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            itemView.transform.Find ("ItemNameValue").GetComponent<Text> ().text = "Select Item";
        }
        else if (langId == 3) {
            itemView.transform.Find("ItemNameValue").GetComponent<Text>().text = "选择道具";
        }
        else {
            itemView.transform.Find("ItemNameValue").GetComponent<Text>().text = "道具選択";
        }
		GameObject.Find ("Background").GetComponent<Image>().enabled = false;
		GameObject.Find ("Fill").GetComponent<Image>().enabled = false;
		GameObject.Find ("Handle").GetComponent<Image>().enabled = false;
		GameObject.Find ("SellQty").GetComponent<Image>().enabled = false;
		GameObject.Find ("SellQtyValue").GetComponent<Text>().text = "";
		GameObject.Find ("DouguExpValue").GetComponent<Text>().text = "";

		foreach ( Transform n in itemView.transform ){
			if(n.tag == "Kahou"){
				GameObject.Destroy(n.gameObject);
			}
		}
		string defaultIconPath = "Prefabs/Item/Kahou/NoKabuto";
		GameObject defIcon = Instantiate (Resources.Load (defaultIconPath)) as GameObject;
		defIcon.transform.SetParent(itemView.transform);
		defIcon.transform.localScale = new Vector2 (1, 1);
		RectTransform defTransform = defIcon.GetComponent<RectTransform> ();
		defTransform.anchoredPosition3D = new Vector3 (0, 120, 0);
		defIcon.GetComponent<Button> ().enabled = false;
		defIcon.name = "NoKabuto";

		/*avairable Dougu*/
		//Clear Previous Data
		GameObject content = GameObject.Find ("Content");
		foreach ( Transform n in content.transform ){
			GameObject.Destroy(n.gameObject);
		}

		//Common
		char[] delimiterChars = {','};
		string kanjyoItemPath = "Prefabs/Item/Kanjyo/Kanjyo";
		string cyouheiItemPath = "Prefabs/Item/Cyouhei/";
		Color lowColor = new Color (0f / 255f, 0f / 255f, 219f / 255f, 255f / 255f);
		Color midColor = new Color (94f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
		Color highColor = new Color (84f / 255f, 103f / 255f, 0f / 255f, 255f / 255f);

		//1. Kanjyo
		//Ge
		string kanjyoString = PlayerPrefs.GetString("kanjyo");
		string[] kanjyo_list = kanjyoString.Split (delimiterChars);
		int kanjyoGeQty = int.Parse(kanjyo_list[0]);
		if(kanjyoGeQty != 0){
			GameObject lowKanjyoItem = Instantiate (Resources.Load (kanjyoItemPath)) as GameObject;
			lowKanjyoItem.transform.SetParent(GameObject.Find ("Content").transform);
			lowKanjyoItem.transform.localScale = new Vector2 (1, 1);
			lowKanjyoItem.transform.localPosition = new Vector3 (0, 0, 0);
			lowKanjyoItem.GetComponent<Image>().color = lowColor;            
            lowKanjyoItem.transform.Find("KanjyoRank").GetComponent<Text>().text = Message.getMessage(181, langId);
            
			lowKanjyoItem.transform.Find("Qty").GetComponent<Text>().text = kanjyoGeQty.ToString();
			lowKanjyoItem.name = "Kanjyo1";
			lowKanjyoItem.GetComponent<Button>().enabled = true;
			lowKanjyoItem.GetComponent<ItemInfo>().posessQty = kanjyoGeQty;
			RectTransform kanjyoImage = lowKanjyoItem.transform.Find("Kanjyo").GetComponent<RectTransform> ();
			kanjyoImage.sizeDelta = new Vector2 (140, 140);
		}

		//Cyu
		int kanjyoCyuQty = int.Parse(kanjyo_list[1]);
		if(kanjyoCyuQty != 0){
			GameObject midKanjyoItem = Instantiate (Resources.Load (kanjyoItemPath)) as GameObject;
			midKanjyoItem.transform.SetParent(GameObject.Find ("Content").transform);
			midKanjyoItem.transform.localScale = new Vector2 (1, 1);
			midKanjyoItem.transform.localPosition = new Vector3 (0, 0, 0);
			midKanjyoItem.GetComponent<Image>().color = midColor;
            midKanjyoItem.transform.Find("KanjyoRank").GetComponent<Text>().text = Message.getMessage(182, langId);
			midKanjyoItem.transform.Find("Qty").GetComponent<Text>().text = kanjyoCyuQty.ToString();
			midKanjyoItem.name = "Kanjyo2";
			midKanjyoItem.GetComponent<Button>().enabled = true;
			midKanjyoItem.GetComponent<ItemInfo>().posessQty = kanjyoCyuQty;
			RectTransform kanjyoImage = midKanjyoItem.transform.Find("Kanjyo").GetComponent<RectTransform> ();
			kanjyoImage.sizeDelta = new Vector2 (140, 140);

		}

		//Jyo
		int kanjyoJyoQty = int.Parse(kanjyo_list[2]);
		if(kanjyoJyoQty != 0){
			GameObject jyoKanjyoItem = Instantiate (Resources.Load (kanjyoItemPath)) as GameObject;
			jyoKanjyoItem.transform.SetParent(GameObject.Find ("Content").transform);
			jyoKanjyoItem.transform.localScale = new Vector2 (1, 1);
			jyoKanjyoItem.transform.localPosition = new Vector3 (0, 0, 0);
			jyoKanjyoItem.GetComponent<Image>().color = highColor;            
            jyoKanjyoItem.transform.Find("KanjyoRank").GetComponent<Text>().text = Message.getMessage(183, langId);            
			jyoKanjyoItem.transform.Find("Qty").GetComponent<Text>().text = kanjyoJyoQty.ToString();
			jyoKanjyoItem.name = "Kanjyo3";
			jyoKanjyoItem.GetComponent<Button>().enabled = true;
			jyoKanjyoItem.GetComponent<ItemInfo>().posessQty = kanjyoJyoQty;
			RectTransform kanjyoImage = jyoKanjyoItem.transform.Find("Kanjyo").GetComponent<RectTransform> ();
			kanjyoImage.sizeDelta = new Vector2 (140, 140);
		}

		//2. Cyouhei
		//cyouheiYR
		//Ge
		string cyouheiYRTmp = "cyouheiYR";
		string cyouheiYRPath = cyouheiItemPath + "CyouheiYR";
		string cyouheiYRString = PlayerPrefs.GetString(cyouheiYRTmp);
		string[] cyouheiYR_list = cyouheiYRString.Split (delimiterChars);
		int cyouheiYRGeQty = int.Parse(cyouheiYR_list[0]);
		if(cyouheiYRGeQty != 0){
			GameObject lowCyouheiYR = Instantiate (Resources.Load (cyouheiYRPath)) as GameObject;
			lowCyouheiYR.transform.SetParent(GameObject.Find ("Content").transform);
			lowCyouheiYR.transform.localScale = new Vector2 (1, 1);
			lowCyouheiYR.transform.localPosition = new Vector3 (0, 0, 0);
			lowCyouheiYR.GetComponent<Image>().color = lowColor;
            lowCyouheiYR.transform.Find("CyouheiRank").GetComponent<Text>().text = Message.getMessage(181, langId);            
			lowCyouheiYR.transform.Find("Qty").GetComponent<Text>().text = cyouheiYRGeQty.ToString();
			lowCyouheiYR.name = "CyouheiYR1";
			lowCyouheiYR.GetComponent<Button>().enabled = true;
			lowCyouheiYR.GetComponent<ItemInfo>().posessQty = cyouheiYRGeQty;
		}

		//Cyu
		int cyouheiYRCyuQty = int.Parse(cyouheiYR_list[1]);
		if(cyouheiYRCyuQty != 0){
			GameObject midCyouheiYR = Instantiate (Resources.Load (cyouheiYRPath)) as GameObject;
			midCyouheiYR.transform.SetParent(GameObject.Find ("Content").transform);
			midCyouheiYR.transform.localScale = new Vector2 (1, 1);
			midCyouheiYR.transform.localPosition = new Vector3 (0, 0, 0);
			midCyouheiYR.GetComponent<Image>().color = midColor;            
            midCyouheiYR.transform.Find("CyouheiRank").GetComponent<Text>().text = Message.getMessage(182, langId);            
			midCyouheiYR.transform.Find("Qty").GetComponent<Text>().text = cyouheiYRCyuQty.ToString();
			midCyouheiYR.name = "CyouheiYR2";
			midCyouheiYR.GetComponent<Button>().enabled = true;
			midCyouheiYR.GetComponent<ItemInfo>().posessQty = cyouheiYRCyuQty;
		}

		//Jyo
		int cyouheiYRJyoQty = int.Parse(cyouheiYR_list[2]);
		if(cyouheiYRJyoQty != 0){
			GameObject highCyouheiYR = Instantiate (Resources.Load (cyouheiYRPath)) as GameObject;
			highCyouheiYR.transform.SetParent(GameObject.Find ("Content").transform);
			highCyouheiYR.transform.localScale = new Vector2 (1, 1);
			highCyouheiYR.transform.localPosition = new Vector3 (0, 0, 0);
			highCyouheiYR.GetComponent<Image>().color = highColor;
            highCyouheiYR.transform.Find("CyouheiRank").GetComponent<Text>().text = Message.getMessage(183, langId);
			highCyouheiYR.transform.Find("Qty").GetComponent<Text>().text = cyouheiYRJyoQty.ToString();
			highCyouheiYR.name = "CyouheiYR3";
			highCyouheiYR.GetComponent<Button>().enabled = true;
			highCyouheiYR.GetComponent<ItemInfo>().posessQty = cyouheiYRJyoQty;
		}

		//cyouheiKB
		//Ge
		string cyouheiKBTmp = "cyouheiKB";
		string cyouheiKBPath = cyouheiItemPath + "CyouheiKB";
		string cyouheiKBString = PlayerPrefs.GetString(cyouheiKBTmp);
		string[] cyouheiKB_list = cyouheiKBString.Split (delimiterChars);
		int cyouheiKBGeQty = int.Parse(cyouheiKB_list[0]);
		if(cyouheiKBGeQty != 0){
			GameObject lowCyouheiKB = Instantiate (Resources.Load (cyouheiKBPath)) as GameObject;
			lowCyouheiKB.transform.SetParent(GameObject.Find ("Content").transform);
			lowCyouheiKB.transform.localScale = new Vector2 (1, 1);
			lowCyouheiKB.transform.localPosition = new Vector3 (0, 0, 0);
			lowCyouheiKB.GetComponent<Image>().color = lowColor;
            lowCyouheiKB.transform.Find("CyouheiRank").GetComponent<Text>().text = Message.getMessage(181, langId);
			lowCyouheiKB.transform.Find("Qty").GetComponent<Text>().text = cyouheiKBGeQty.ToString();
			lowCyouheiKB.name = "CyouheiKB1";
			lowCyouheiKB.GetComponent<Button>().enabled = true;
			lowCyouheiKB.GetComponent<ItemInfo>().posessQty = cyouheiKBGeQty;
		}
		
		//Cyu
		int cyouheiKBCyuQty = int.Parse(cyouheiKB_list[1]);
		if(cyouheiKBCyuQty != 0){
			GameObject midCyouheiKB = Instantiate (Resources.Load (cyouheiKBPath)) as GameObject;
			midCyouheiKB.transform.SetParent(GameObject.Find ("Content").transform);
			midCyouheiKB.transform.localScale = new Vector2 (1, 1);
			midCyouheiKB.transform.localPosition = new Vector3 (0, 0, 0);
			midCyouheiKB.GetComponent<Image>().color = midColor;
            midCyouheiKB.transform.Find("CyouheiRank").GetComponent<Text>().text = Message.getMessage(182, langId);
			midCyouheiKB.transform.Find("Qty").GetComponent<Text>().text = cyouheiKBCyuQty.ToString();
			midCyouheiKB.name = "CyouheiKB2";
			midCyouheiKB.GetComponent<Button>().enabled = true;
			midCyouheiKB.GetComponent<ItemInfo>().posessQty = cyouheiKBCyuQty;
		}
		
		//Jyo
		int cyouheiKBJyoQty = int.Parse(cyouheiKB_list[2]);
		if(cyouheiKBJyoQty != 0){
			GameObject highCyouheiKB = Instantiate (Resources.Load (cyouheiKBPath)) as GameObject;
			highCyouheiKB.transform.SetParent(GameObject.Find ("Content").transform);
			highCyouheiKB.transform.localScale = new Vector2 (1, 1);
			highCyouheiKB.transform.localPosition = new Vector3 (0, 0, 0);
			highCyouheiKB.GetComponent<Image>().color = highColor;
            highCyouheiKB.transform.Find("CyouheiRank").GetComponent<Text>().text = Message.getMessage(183, langId);
			highCyouheiKB.transform.Find("Qty").GetComponent<Text>().text = cyouheiKBJyoQty.ToString();
			highCyouheiKB.name = "CyouheiKB3";
			highCyouheiKB.GetComponent<Button>().enabled = true;
			highCyouheiKB.GetComponent<ItemInfo>().posessQty = cyouheiKBJyoQty;
		}

		//cyouheiTP
		//Ge
		string cyouheiTPTmp = "cyouheiTP";
		string cyouheiTPPath = cyouheiItemPath + "CyouheiTP";
		string cyouheiTPString = PlayerPrefs.GetString(cyouheiTPTmp);
		string[] cyouheiTP_list = cyouheiTPString.Split (delimiterChars);
		int cyouheiTPGeQty = int.Parse(cyouheiTP_list[0]);
		if(cyouheiTPGeQty != 0){
			GameObject lowCyouheiTP = Instantiate (Resources.Load (cyouheiTPPath)) as GameObject;
			lowCyouheiTP.transform.SetParent(GameObject.Find ("Content").transform);
			lowCyouheiTP.transform.localScale = new Vector2 (1, 1);
			lowCyouheiTP.transform.localPosition = new Vector3 (0, 0, 0);
			lowCyouheiTP.GetComponent<Image>().color = lowColor;
            lowCyouheiTP.transform.Find("CyouheiRank").GetComponent<Text>().text = Message.getMessage(181, langId);
			lowCyouheiTP.transform.Find("Qty").GetComponent<Text>().text = cyouheiTPGeQty.ToString();
			lowCyouheiTP.name =  "CyouheiTP1";
			lowCyouheiTP.GetComponent<Button>().enabled = true;
			lowCyouheiTP.GetComponent<ItemInfo>().posessQty = cyouheiTPGeQty;
		}
		
		//Cyu
		int cyouheiTPCyuQty = int.Parse(cyouheiTP_list[1]);
		if(cyouheiTPCyuQty != 0){
			GameObject midCyouheiTP = Instantiate (Resources.Load (cyouheiTPPath)) as GameObject;
			midCyouheiTP.transform.SetParent(GameObject.Find ("Content").transform);
			midCyouheiTP.transform.localScale = new Vector2 (1, 1);
			midCyouheiTP.transform.localPosition = new Vector3 (0, 0, 0);
			midCyouheiTP.GetComponent<Image>().color = midColor;
            midCyouheiTP.transform.Find("CyouheiRank").GetComponent<Text>().text = Message.getMessage(182, langId);
			midCyouheiTP.transform.Find("Qty").GetComponent<Text>().text = cyouheiTPCyuQty.ToString();
			midCyouheiTP.name = "CyouheiTP2";
			midCyouheiTP.GetComponent<Button>().enabled = true;
			midCyouheiTP.GetComponent<ItemInfo>().posessQty = cyouheiTPCyuQty;
		}
		
		//Jyo
		int cyouheiTPJyoQty = int.Parse(cyouheiTP_list[2]);
		if(cyouheiTPJyoQty != 0){
			GameObject highCyouheiTP = Instantiate (Resources.Load (cyouheiTPPath)) as GameObject;
			highCyouheiTP.transform.SetParent(GameObject.Find ("Content").transform);
			highCyouheiTP.transform.localScale = new Vector2 (1, 1);
			highCyouheiTP.transform.localPosition = new Vector3 (0, 0, 0);
			highCyouheiTP.GetComponent<Image>().color = highColor;
            highCyouheiTP.transform.Find("CyouheiRank").GetComponent<Text>().text = Message.getMessage(183, langId);
			highCyouheiTP.transform.Find("Qty").GetComponent<Text>().text = cyouheiTPJyoQty.ToString();
			highCyouheiTP.name = "CyouheiTP3";
			highCyouheiTP.GetComponent<Button>().enabled = true;
			highCyouheiTP.GetComponent<ItemInfo>().posessQty = cyouheiTPJyoQty;
		}

		//cyouheiYM
		//Ge
		string cyouheiYMTmp = "cyouheiYM";
		string cyouheiYMPath = cyouheiItemPath + "CyouheiYM";
		string cyouheiYMString = PlayerPrefs.GetString(cyouheiYMTmp);
		string[] cyouheiYM_list = cyouheiYMString.Split (delimiterChars);
		int cyouheiYMGeQty = int.Parse(cyouheiYM_list[0]);
		if(cyouheiYMGeQty != 0){
			GameObject lowCyouheiYM = Instantiate (Resources.Load (cyouheiYMPath)) as GameObject;
			lowCyouheiYM.transform.SetParent(GameObject.Find ("Content").transform);
			lowCyouheiYM.transform.localScale = new Vector2 (1, 1);
			lowCyouheiYM.transform.localPosition = new Vector3 (0, 0, 0);
			lowCyouheiYM.GetComponent<Image>().color = lowColor;
            lowCyouheiYM.transform.Find("CyouheiRank").GetComponent<Text>().text = Message.getMessage(181, langId);
			lowCyouheiYM.transform.Find("Qty").GetComponent<Text>().text = cyouheiYMGeQty.ToString();
			lowCyouheiYM.name = "CyouheiYM1";
			lowCyouheiYM.GetComponent<Button>().enabled = true;
			lowCyouheiYM.GetComponent<ItemInfo>().posessQty = cyouheiYMGeQty;
		}
		
		//Cyu
		int cyouheiYMCyuQty = int.Parse(cyouheiYM_list[1]);
		if(cyouheiYMCyuQty != 0){
			GameObject midCyouheiYM = Instantiate (Resources.Load (cyouheiYMPath)) as GameObject;
			midCyouheiYM.transform.SetParent(GameObject.Find ("Content").transform);
			midCyouheiYM.transform.localScale = new Vector2 (1, 1);
			midCyouheiYM.transform.localPosition = new Vector3 (0, 0, 0);
			midCyouheiYM.GetComponent<Image>().color = midColor;
            midCyouheiYM.transform.Find("CyouheiRank").GetComponent<Text>().text = Message.getMessage(182, langId);
			midCyouheiYM.transform.Find("Qty").GetComponent<Text>().text = cyouheiYMCyuQty.ToString();
			midCyouheiYM.name =  "CyouheiYM2";
			midCyouheiYM.GetComponent<Button>().enabled = true;
			midCyouheiYM.GetComponent<ItemInfo>().posessQty = cyouheiYMCyuQty;
		}
		
		//Jyo
		int cyouheiYMJyoQty = int.Parse(cyouheiYM_list[2]);
		if(cyouheiYMJyoQty != 0){
			GameObject highCyouheiYM = Instantiate (Resources.Load (cyouheiYMPath)) as GameObject;
			highCyouheiYM.transform.SetParent(GameObject.Find ("Content").transform);
			highCyouheiYM.transform.localScale = new Vector2 (1, 1);
			highCyouheiYM.transform.localPosition = new Vector3 (0, 0, 0);
			highCyouheiYM.GetComponent<Image>().color = highColor;
            highCyouheiYM.transform.Find("CyouheiRank").GetComponent<Text>().text = Message.getMessage(183, langId);
			highCyouheiYM.transform.Find("Qty").GetComponent<Text>().text = cyouheiYMJyoQty.ToString();
			highCyouheiYM.name = "CyouheiYM3";
			highCyouheiYM.GetComponent<Button>().enabled = true;
			highCyouheiYM.GetComponent<ItemInfo>().posessQty = cyouheiYMJyoQty;
		}

		//3. Hidensyo
		string hidensyoItemPath = "Prefabs/Item/Hidensyo/Hidensyo";
		//Ge
		int hidensyoGeQty = PlayerPrefs.GetInt("hidensyoGe");
		if(hidensyoGeQty != 0){
			GameObject lowHidensyo = Instantiate (Resources.Load (hidensyoItemPath)) as GameObject;
			lowHidensyo.transform.SetParent(GameObject.Find ("Content").transform);
			lowHidensyo.transform.localScale = new Vector2 (1, 1);
			lowHidensyo.transform.localPosition = new Vector3 (0, 0, 0);
			lowHidensyo.GetComponent<Image>().color = lowColor;
            lowHidensyo.transform.Find("HidensyoRank").GetComponent<Text>().text = Message.getMessage(181, langId);
			lowHidensyo.transform.Find("Qty").GetComponent<Text>().text = hidensyoGeQty.ToString();
			lowHidensyo.name = "Hidensyo1";
			lowHidensyo.GetComponent<Button>().enabled = true;
			lowHidensyo.GetComponent<ItemInfo>().posessQty = hidensyoGeQty;
			
		}

		//Cyu
		int hidensyoCyuQty = PlayerPrefs.GetInt("hidensyoCyu");
		if(hidensyoCyuQty != 0){
			GameObject midHidensyo = Instantiate (Resources.Load (hidensyoItemPath)) as GameObject;
			midHidensyo.transform.SetParent(GameObject.Find ("Content").transform);
			midHidensyo.transform.localScale = new Vector2 (1, 1);
			midHidensyo.transform.localPosition = new Vector3 (0, 0, 0);
			midHidensyo.GetComponent<Image>().color = midColor;
            midHidensyo.transform.Find("HidensyoRank").GetComponent<Text>().text = Message.getMessage(182, langId);            
			midHidensyo.transform.Find("Qty").GetComponent<Text>().text = hidensyoCyuQty.ToString();
			midHidensyo.name = "Hidensyo2";
			midHidensyo.GetComponent<Button>().enabled = true;
			midHidensyo.GetComponent<ItemInfo>().posessQty = hidensyoCyuQty;
		}

		//Jyo
		int hidensyoJyoQty = PlayerPrefs.GetInt("hidensyoJyo");
		if(hidensyoJyoQty != 0){
			GameObject highHidensyo = Instantiate (Resources.Load (hidensyoItemPath)) as GameObject;
			highHidensyo.transform.SetParent(GameObject.Find ("Content").transform);
			highHidensyo.transform.localScale = new Vector2 (1, 1);
			highHidensyo.transform.localPosition = new Vector3 (0, 0, 0);
			highHidensyo.GetComponent<Image>().color = highColor;
            highHidensyo.transform.Find("HidensyoRank").GetComponent<Text>().text = Message.getMessage(183, langId);
			highHidensyo.transform.Find("Qty").GetComponent<Text>().text = hidensyoJyoQty.ToString();
			highHidensyo.name = "Hidensyo3";
			highHidensyo.GetComponent<Button>().enabled = true;
			highHidensyo.GetComponent<ItemInfo>().posessQty = hidensyoJyoQty;
		}

		//4. Shinobi
		string shinobiItemPath = "Prefabs/Item/Shinobi/Shinobi";
		//Ge
		int shinobiGeQty = PlayerPrefs.GetInt("shinobiGe");
		if(shinobiGeQty != 0){
			GameObject lowShinobi = Instantiate (Resources.Load (shinobiItemPath)) as GameObject;
			lowShinobi.transform.SetParent(GameObject.Find ("Content").transform);
			lowShinobi.transform.localScale = new Vector2 (1, 1);
			lowShinobi.transform.localPosition = new Vector3 (0, 0, 0);
			lowShinobi.GetComponent<Image>().color = lowColor;
            lowShinobi.transform.Find("ShinobiRank").GetComponent<Text>().text = Message.getMessage(181, langId);            
			lowShinobi.transform.Find("Qty").GetComponent<Text>().text = shinobiGeQty.ToString();
			lowShinobi.name = "Shinobi1";
			lowShinobi.GetComponent<Button>().enabled = true;
			lowShinobi.GetComponent<ItemInfo>().posessQty = shinobiGeQty;
			
		}
		
		//Cyu
		int shinobiCyuQty = PlayerPrefs.GetInt("shinobiCyu");
		if(shinobiCyuQty != 0){
			GameObject midShinobi = Instantiate (Resources.Load (shinobiItemPath)) as GameObject;
			midShinobi.transform.SetParent(GameObject.Find ("Content").transform);
			midShinobi.transform.localScale = new Vector2 (1, 1);
			midShinobi.transform.localPosition = new Vector3 (0, 0, 0);
			midShinobi.GetComponent<Image>().color = midColor;
            midShinobi.transform.Find("ShinobiRank").GetComponent<Text>().text = Message.getMessage(182, langId);
			midShinobi.transform.Find("Qty").GetComponent<Text>().text = shinobiCyuQty.ToString();
			midShinobi.name = "Shinobi2";
			midShinobi.GetComponent<Button>().enabled = true;
			midShinobi.GetComponent<ItemInfo>().posessQty = shinobiCyuQty;
		}
		
		//Jyo
		int shinobiJyoQty = PlayerPrefs.GetInt("shinobiJyo");
		if(shinobiJyoQty != 0){
			GameObject highShinobi = Instantiate (Resources.Load (shinobiItemPath)) as GameObject;
			highShinobi.transform.SetParent(GameObject.Find ("Content").transform);
			highShinobi.transform.localScale = new Vector2 (1, 1);
			highShinobi.transform.localPosition = new Vector3 (0, 0, 0);
			highShinobi.GetComponent<Image>().color = highColor;
            highShinobi.transform.Find("ShinobiRank").GetComponent<Text>().text = Message.getMessage(183,langId);            
			highShinobi.transform.Find("Qty").GetComponent<Text>().text = shinobiJyoQty.ToString();
			highShinobi.name = "Shinobi3";
			highShinobi.GetComponent<Button>().enabled = true;
			highShinobi.GetComponent<ItemInfo>().posessQty = shinobiJyoQty;
		}


		//5. Kengou
		string kengouPath = "Prefabs/Item/kengou";

		string kengouString = PlayerPrefs.GetString("kengouItem");
		List<string> kengouList = new List<string> ();
		kengouList = new List<string> (kengouString.Split (delimiterChars));

		for(int i=0; i<kengouList.Count; i++){
			string qty = kengouList[i];

			if(qty != "0"){
				GameObject kengou = Instantiate (Resources.Load (kengouPath)) as GameObject;
				kengou.transform.SetParent(GameObject.Find ("Content").transform);
				kengou.transform.localScale = new Vector2 (1, 1);
				kengou.transform.localPosition = new Vector3 (0, 0, 0);
				kengou.transform.Find("Qty").GetComponent<Text>().text = qty.ToString();

				int temp = i + 1;
				kengou.name = "kengou" + temp.ToString();

				RectTransform kengouTransform = kengou.transform.Find("Image").GetComponent<RectTransform>();
				kengouTransform.sizeDelta = new Vector2(120,120);

				RectTransform rankTransform = kengou.transform.Find("Rank").GetComponent<RectTransform>();
				rankTransform.anchoredPosition3D = new Vector3(-70,20,0);
				rankTransform.localScale = new Vector2(0.13f,0.13f);
				ItemInfo item = new ItemInfo();
				kengou.transform.Find("Rank").GetComponent<Text>().text = item.getItemName(kengou.name);

				kengou.GetComponent<Button>().enabled = true;
				kengou.GetComponent<ItemInfo>().posessQty = int.Parse(qty);
			}
		}

		//6. Gokui
		string gokuiPath = "Prefabs/Item/gokui";
		
		string gokuiString = PlayerPrefs.GetString("gokuiItem");
		List<string> gokuiList = new List<string> ();
		gokuiList = new List<string> (gokuiString.Split (delimiterChars));
		
		for(int i=0; i<gokuiList.Count; i++){
			string qty = gokuiList[i];
			
			if(qty != "0"){
				GameObject gokui = Instantiate (Resources.Load (gokuiPath)) as GameObject;
				gokui.transform.SetParent(GameObject.Find ("Content").transform);
				gokui.transform.localScale = new Vector2 (1, 1);
				gokui.transform.localPosition = new Vector3 (0, 0, 0);
				gokui.transform.Find("Qty").GetComponent<Text>().text = qty.ToString();
				
				int temp = i + 1;
				gokui.name = "gokui" + temp.ToString();

				RectTransform rankTransform = gokui.transform.Find("Rank").GetComponent<RectTransform>();
				rankTransform.anchoredPosition3D = new Vector3(-70,20,0);
				rankTransform.localScale = new Vector2(0.09f,0.13f);
				ItemInfo item = new ItemInfo();
				gokui.transform.Find("Rank").GetComponent<Text>().text = item.getItemName(gokui.name);
				
				gokui.GetComponent<Button>().enabled = true;
				gokui.GetComponent<ItemInfo>().posessQty = int.Parse(qty);
			}
		}

		//7. Nanban

		string nanbanString = PlayerPrefs.GetString("nanbanItem");
		List<string> nanbanList = new List<string> ();
		nanbanList = new List<string> (nanbanString.Split (delimiterChars));
		
		for(int i=0; i<nanbanList.Count; i++){
			string qty = nanbanList[i];
			
			if(qty != "0"){
				int tempNanbanId = i + 1;
				string nanbanPath = "Prefabs/Item/nanban";
				nanbanPath = nanbanPath + tempNanbanId;
				GameObject nanban = Instantiate (Resources.Load (nanbanPath)) as GameObject;
				nanban.transform.SetParent(GameObject.Find ("Content").transform);
				nanban.transform.localScale = new Vector2 (1, 1);
				nanban.transform.localPosition = new Vector3 (0, 0, 0);
				nanban.transform.Find("Qty").GetComponent<Text>().text = qty.ToString();

				nanban.name = "nanban" + tempNanbanId.ToString();

				RectTransform nanbanTransform = nanban.transform.Find("Image").GetComponent<RectTransform>();
				nanbanTransform.sizeDelta = new Vector2(120,120);

				RectTransform rankTransform = nanban.transform.Find("Rank").GetComponent<RectTransform>();
				rankTransform.anchoredPosition3D = new Vector3(-70,20,0);
				rankTransform.localScale = new Vector2(0.13f,0.13f);
				ItemInfo item = new ItemInfo();
				nanban.transform.Find("Rank").GetComponent<Text>().text = item.getItemName(nanban.name);
				
				nanban.GetComponent<Button>().enabled = true;
				nanban.GetComponent<ItemInfo>().posessQty = int.Parse(qty);
			}
		}

		//8. Koueki
		showThreeItem("koueki");


		//9. Cyoutei
		showThreeItem("cyoutei");


		//10. Tech
		//TP
		int tpTechQty = PlayerPrefs.GetInt("transferTP");
		if(tpTechQty !=0){
			string path = "Prefabs/Item/Tech/Tech";
			GameObject tech = Instantiate (Resources.Load (path)) as GameObject;
			tech.transform.SetParent(GameObject.Find ("Content").transform);
			tech.transform.localScale = new Vector2 (1, 1);
			tech.transform.localPosition = new Vector3 (0, 0, 0);
			tech.name = "tech1";

			//Image
			string spritePath = "Prefabs/Item/Tech/Sprite/tp";
			tech.GetComponent<Image> ().sprite = 
				Resources.Load (spritePath, typeof(Sprite)) as Sprite;

			//Qty
			tech.transform.Find("Qty").GetComponent<Text>().text = tpTechQty.ToString();
			tech.GetComponent<ItemInfo>().posessQty = tpTechQty;
		}

		//KB
		int kbTechQty = PlayerPrefs.GetInt("transferKB");
		if(kbTechQty !=0){
			string path = "Prefabs/Item/Tech/Tech";
			GameObject tech = Instantiate (Resources.Load (path)) as GameObject;
			tech.transform.SetParent(GameObject.Find ("Content").transform);
			tech.transform.localScale = new Vector2 (1, 1);
			tech.transform.localPosition = new Vector3 (0, 0, 0);
			tech.name = "tech2";

			//Image
			string spritePath = "Prefabs/Item/Tech/Sprite/kb";
			tech.GetComponent<Image> ().sprite = 
				Resources.Load (spritePath, typeof(Sprite)) as Sprite;
			
			//Qty
			tech.transform.Find("Qty").GetComponent<Text>().text = kbTechQty.ToString();
			tech.GetComponent<ItemInfo>().posessQty = kbTechQty;
		}

		//SNB
		int snbTechQty = PlayerPrefs.GetInt("transferSNB");
		if(snbTechQty !=0){
			string path = "Prefabs/Item/Tech/Tech";
			GameObject tech = Instantiate (Resources.Load (path)) as GameObject;
			tech.transform.SetParent(GameObject.Find ("Content").transform);
			tech.transform.localScale = new Vector2 (1, 1);
			tech.transform.localPosition = new Vector3 (0, 0, 0);
			tech.name = "tech3";

			//Image
			string spritePath = "Prefabs/Item/Tech/Sprite/snb";
			tech.GetComponent<Image> ().sprite = 
				Resources.Load (spritePath, typeof(Sprite)) as Sprite;
			
			//Qty
			tech.transform.Find("Qty").GetComponent<Text>().text = snbTechQty.ToString();
			tech.GetComponent<ItemInfo>().posessQty = snbTechQty;
		}

		//Meisei
		int meiseiQty = PlayerPrefs.GetInt("meisei");
		if(meiseiQty !=0){
			string path = "Prefabs/Item/meisei";
			GameObject meisei = Instantiate (Resources.Load (path)) as GameObject;
			meisei.transform.SetParent(GameObject.Find ("Content").transform);
			meisei.transform.localScale = new Vector2 (1, 1);
			meisei.transform.localPosition = new Vector3 (0, 0, 0);
			meisei.name = "meisei";

			//Qty
			meisei.transform.Find("Qty").GetComponent<Text>().text = meiseiQty.ToString();
			meisei.GetComponent<ItemInfo>().posessQty = meiseiQty;
		}

        //Shiro
        showShiro();



    }

	public void showThreeItem(string itemCd){
		string nowQty = PlayerPrefs.GetString (itemCd);
        Message Message = new Message();
        List<string> nowQtyList = new List<string> ();
		char[] delimiterChars = {','};
		nowQtyList = new List<string> (nowQty.Split (delimiterChars));
        int langId = PlayerPrefs.GetInt("langId");
        string path = "Prefabs/Item/" + itemCd;

		if (nowQtyList [0] != "0") {
			//Ge
			GameObject item = Instantiate (Resources.Load (path)) as GameObject;
			item.transform.SetParent(GameObject.Find ("Content").transform);
			item.transform.localScale = new Vector2 (1, 1);
			item.transform.localPosition = new Vector3 (0, 0, 0);
			item.transform.Find("Qty").GetComponent<Text>().text = nowQtyList [0];
			GameObject itemName = item.transform.Find("Name").gameObject;

			string syoukaiName = "";
			if(itemCd == "koueki"){
                syoukaiName = Message.getMessage(232, langId);
				itemName.GetComponent<Text>().text = syoukaiName;
			}else if(itemCd == "cyoutei"){
                syoukaiName = Message.getMessage(233, langId);
				itemName.GetComponent<Text>().text = syoukaiName;
			}
			RectTransform itemNameRect = itemName.GetComponent<RectTransform>();
			itemNameRect.anchoredPosition3D = new Vector3(-70,20,0);
			itemNameRect.localScale = new Vector3(0.1f,0.15f,0);

			GameObject itemRank = item.transform.Find("Rank").gameObject;
            itemRank.GetComponent<Text>().text = Message.getMessage(181, langId);
			RectTransform itemRankRect = itemRank.GetComponent<RectTransform>();
			itemRankRect.anchoredPosition3D = new Vector3(30,-35,0);
			itemRankRect.localScale = new Vector3(0.18f,0.22f,0);

			item.name = itemCd;
			item.GetComponent<ItemInfo>().posessQty = int.Parse(nowQtyList [0]);
			item.GetComponent<ItemInfo>().syoukaiName = syoukaiName;
			item.GetComponent<ItemInfo>().itemId = 1;
			item.GetComponent<Button>().enabled = true;
		}
		if (nowQtyList [1] != "0") {
			//Cyu
			GameObject item = Instantiate (Resources.Load (path)) as GameObject;
			item.transform.SetParent(GameObject.Find ("Content").transform);
			item.transform.localScale = new Vector2 (1, 1);
			item.transform.localPosition = new Vector3 (0, 0, 0);
			item.transform.Find("Qty").GetComponent<Text>().text = nowQtyList [1];
			GameObject itemName = item.transform.Find("Name").gameObject;
			string syoukaiName = "";
			if(itemCd == "koueki"){
                syoukaiName = Message.getMessage(234, langId);
				itemName.GetComponent<Text>().text = syoukaiName;
			}else if(itemCd == "cyoutei"){
                syoukaiName = Message.getMessage(235, langId);                
				itemName.GetComponent<Text>().text = syoukaiName;
			}
			RectTransform itemNameRect = itemName.GetComponent<RectTransform>();
			itemNameRect.anchoredPosition3D = new Vector3(-70,20,0);
			itemNameRect.localScale = new Vector3(0.1f,0.15f,0);

			GameObject itemRank = item.transform.Find("Rank").gameObject;
            itemRank.GetComponent<Text>().text = Message.getMessage(182, langId);
            
			RectTransform itemRankRect = itemRank.GetComponent<RectTransform>();
			itemRankRect.anchoredPosition3D = new Vector3(30,-35,0);
			itemRankRect.localScale = new Vector3(0.18f,0.22f,0);

			item.name = itemCd;
			item.GetComponent<ItemInfo>().posessQty = int.Parse(nowQtyList [1]);
			item.GetComponent<ItemInfo>().syoukaiName = syoukaiName;
			item.GetComponent<ItemInfo>().itemId = 2;
			item.GetComponent<Button>().enabled = true;
		} 
		if (nowQtyList [2] != "0") {
			//Jyo
			GameObject item = Instantiate (Resources.Load (path)) as GameObject;
			item.transform.SetParent(GameObject.Find ("Content").transform);
			item.transform.localScale = new Vector2 (1, 1);
			item.transform.localPosition = new Vector3 (0, 0, 0);
			item.transform.Find("Qty").GetComponent<Text>().text = nowQtyList [2];
			GameObject itemName = item.transform.Find("Name").gameObject;
			string syoukaiName = "";
			if(itemCd == "koueki"){                
                syoukaiName = Message.getMessage(236, langId);                
				itemName.GetComponent<Text>().text = syoukaiName;
			}else if(itemCd == "cyoutei"){                
                syoukaiName = Message.getMessage(237, langId);            
				itemName.GetComponent<Text>().text = syoukaiName;
			}
			RectTransform itemNameRect = itemName.GetComponent<RectTransform>();
			itemNameRect.anchoredPosition3D = new Vector3(-70,20,0);
			itemNameRect.localScale = new Vector3(0.1f,0.15f,0);

			GameObject itemRank = item.transform.Find("Rank").gameObject;
            itemRank.GetComponent<Text>().text = Message.getMessage(183, langId);
			RectTransform itemRankRect = itemRank.GetComponent<RectTransform>();
			itemRankRect.anchoredPosition3D = new Vector3(30,-35,0);
			itemRankRect.localScale = new Vector3(0.18f,0.22f,0);

			item.name = itemCd;
			item.GetComponent<ItemInfo>().posessQty = int.Parse(nowQtyList [2]);
			item.GetComponent<ItemInfo>().syoukaiName = syoukaiName;
			item.GetComponent<ItemInfo>().itemId = 3;
			item.GetComponent<Button>().enabled = true;
		}
	}

    public void showShiro() {
        string nowQty = PlayerPrefs.GetString("shiro");
        List<string> nowQtyList = new List<string>();
        char[] delimiterChars = { ',' };
        if(nowQty != "" && nowQty != null) {
            if(nowQty.Contains(",")) {
                nowQtyList = new List<string>(nowQty.Split(delimiterChars));

                string path = "Prefabs/Item/Shiro/shiro";
                Shiro shiro = new Shiro();
                for (int i=0; i< nowQtyList.Count; i++) {
                    string imagePath = "Prefabs/Naisei/Shiro/Sprite/";
                    int qty = int.Parse(nowQtyList[i]);
                    if(qty != 0) {
                        int shiroId = i + 1;
                        GameObject item = Instantiate(Resources.Load(path)) as GameObject;
                        item.transform.SetParent(GameObject.Find("Content").transform);
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

                    }
                }
            }
        }
    }
}
