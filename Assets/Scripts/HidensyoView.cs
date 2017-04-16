using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Reflection;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class HidensyoView : MonoBehaviour {


	public void OnClick () {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

		//Common
		string busyoName = GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyoName;
		string busyoId = GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyo;

		BusyoStatusButton pop = new BusyoStatusButton ();
		pop.commonPopup(22);
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            GameObject.Find ("popText").GetComponent<Text> ().text ="Give Skillbook";
        }else {
            GameObject.Find("popText").GetComponent<Text>().text = "秘伝書授与";
        }
		//Busyo View
		string path = "Prefabs/Player/Unit/BusyoUnit";
		GameObject Busyo = Instantiate (Resources.Load (path)) as GameObject;
		Busyo.name = busyoId.ToString ();
		Busyo.transform.SetParent (GameObject.Find ("board(Clone)").transform);
		Busyo.transform.localScale = new Vector2 (3, 3);
		Busyo.GetComponent<DragHandler>().enabled = false;
		RectTransform busyo_transform = Busyo.GetComponent<RectTransform>();
		busyo_transform.anchoredPosition = new Vector3(300,350,0);
		busyo_transform.sizeDelta = new Vector2( 100, 100);

		//Text Modification
		GameObject text = Busyo.transform.FindChild ("Text").gameObject;
		text.GetComponent<Text> ().color = new Color(255,255,255,255);
		RectTransform text_transform = text.GetComponent<RectTransform>();
		text_transform.anchoredPosition = new Vector3 (-70,30,0);
		text_transform.sizeDelta = new Vector2( 630, 120);
		text.transform.localScale = new Vector2 (0.2f,0.2f);
		
		//Rank Text Modification
		GameObject rank = Busyo.transform.FindChild ("Rank").gameObject;
		RectTransform rank_transform = rank.GetComponent<RectTransform>();
		rank_transform.anchoredPosition = new Vector3 (20,-50,0);
		rank_transform.sizeDelta = new Vector2( 200, 200);
		rank.GetComponent<Text>().fontSize = 200;

		//Hidensyo
		string hidensyoPath = "Prefabs/Busyo/Hidensyo";
		GameObject hidensyo = Instantiate (Resources.Load (hidensyoPath)) as GameObject;
		hidensyo.transform.SetParent (GameObject.Find ("board(Clone)").transform);
		hidensyo.transform.localScale = new Vector2 (1, 1);
		RectTransform hidensyo_transform = hidensyo.GetComponent<RectTransform>();
		hidensyo_transform.anchoredPosition = new Vector3(0,0,0);
		hidensyo.name = "Hidensyo";

		//Senpou Detail
		StatusGet sts = new StatusGet();
		ArrayList senpouArray = sts.getSenpou(int.Parse(busyoId),false);

		int senpouId = (int)senpouArray[0];
		GameObject.Find ("SenpouNameValue").GetComponent<Text>().text =senpouArray[2].ToString();
		int senpouLv = (int)senpouArray[8];
		GameObject.Find ("LvFrom").GetComponent<Text>().text =senpouLv.ToString();
		int nextLv = senpouLv + 1;
		GameObject.Find ("LvTo").GetComponent<Text>().text = nextLv.ToString();

		//Get Next Senpou
		List<string> senpouList = getSenpouNextLv (senpouId,nextLv);

		string senpouExp = senpouArray[3].ToString();
		float senpouEach = (float)senpouArray[4];
		float senpouRatio = (float)senpouArray[5];
		float senpouTerm = (float)senpouArray[6];
		int senpouStatus = (int)senpouArray[7];

		int diff = int.Parse (senpouList [0]) - senpouStatus;
		string adjSenpouStatus = senpouStatus.ToString() + "<color=#35d74bFF>" + "(+" + diff + ")" + "</color>";

        if (Application.systemLanguage != SystemLanguage.Japanese) {
            senpouExp = senpouExp.Replace("ABC", adjSenpouStatus);
            senpouExp = senpouExp.Replace("DEF", senpouEach.ToString());
            senpouExp = senpouExp.Replace("GHI", senpouRatio.ToString());
            senpouExp = senpouExp.Replace("JKL", senpouTerm.ToString());
        }
        else {
            senpouExp = senpouExp.Replace("A", adjSenpouStatus);
            senpouExp = senpouExp.Replace("B", senpouEach.ToString());
            senpouExp = senpouExp.Replace("C", senpouRatio.ToString());
            senpouExp = senpouExp.Replace("D", senpouTerm.ToString());
        }

        GameObject.Find ("PopSenpouExpValue").GetComponent<Text> ().text = senpouExp;
		Text itemText = GameObject.Find ("RequiredItemValue").GetComponent<Text> ();
		itemText.text = senpouList[2];

		Image hImage = GameObject.Find ("HidensyoItem").GetComponent<Image> ();
		Text hRank = GameObject.Find ("HidensyoRank").GetComponent<Text> ();
		string senpouType = senpouList [1];
		Color shortageColor = new Color (203f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
		GameObject DoHidensyoObj = GameObject.Find ("GiveHidensyo");
		DoHidensyoObj.GetComponent<DoHidensyo> ().requiredItemQty = int.Parse (itemText.text);
		DoHidensyoObj.GetComponent<DoHidensyo> ().busyoId = busyoId;
		DoHidensyoObj.GetComponent<DoHidensyo> ().nextSenpouLv = nextLv;

		if(senpouType=="low"){
			Color lowColor = new Color (86f / 255f, 87f / 255f, 255f / 255f, 255f / 255f);
			hImage.color = lowColor;
			if (Application.systemLanguage != SystemLanguage.Japanese) {
                hRank.text = "Low";
            }else {
                hRank.text = "下";
            }
            int hidensyoGeQty = PlayerPrefs.GetInt ("hidensyoGe");
			DoHidensyoObj.GetComponent<DoHidensyo>().itemType = senpouType;
			if(hidensyoGeQty < int.Parse(itemText.text)){
				itemText.color = shortageColor;
				DoHidensyoObj.GetComponent<DoHidensyo>().requiredItem = false;
			}

		}else if(senpouType=="middle"){
			Color midColor = new Color (236f / 255f, 93f / 255f, 93f / 255f, 255f / 255f);
			hImage.color = midColor;
			if (Application.systemLanguage != SystemLanguage.Japanese) {
                hRank.text = "Mid";
            }else {
                hRank.text = "中";
            }
            int hidensyoCyuQty = PlayerPrefs.GetInt ("hidensyoCyu");
			DoHidensyoObj.GetComponent<DoHidensyo>().itemType = senpouType;
			if(hidensyoCyuQty < int.Parse(itemText.text)){
				itemText.color = shortageColor;
				DoHidensyoObj.GetComponent<DoHidensyo>().requiredItem = false;
			}
		}else if(senpouType=="high"){
			Color hightColor = new Color (207f / 255f, 232f / 255f, 95f / 255f, 255f / 255f);
			hImage.color = hightColor;
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                hRank.text = "High";
            }else {
                hRank.text = "上";
            }
            int hidensyoJyoQty = PlayerPrefs.GetInt ("hidensyoJyo");
			DoHidensyoObj.GetComponent<DoHidensyo>().itemType = senpouType;
			if(hidensyoJyoQty < int.Parse(itemText.text)){
				itemText.color = shortageColor;
				DoHidensyoObj.GetComponent<DoHidensyo>().requiredItem = false;
			}
		}


		Text moneyAmt = GameObject.Find ("RequiredMoneyValue").GetComponent<Text> ();
		moneyAmt.text = senpouList[3];
		DoHidensyoObj.GetComponent<DoHidensyo> ().requiredMoneyAmt = int.Parse(moneyAmt.text);

		int money = PlayerPrefs.GetInt ("money");
		if(money < int.Parse (moneyAmt.text)){
			moneyAmt.color = shortageColor;
			DoHidensyoObj.GetComponent<DoHidensyo>().requiredMoney = false;
		}
	}

	public List<string> getSenpouNextLv(int senpouId, int nextLv){

		Entity_senpou_mst senpouMst  = Resources.Load ("Data/senpou_mst") as Entity_senpou_mst;
		Entity_senpouItem_mst senpouItemMst  = Resources.Load ("Data/senpouItem_mst") as Entity_senpouItem_mst;

		List<string> senpouList = new List<string>();

		//Next Status
		object senpoulst = senpouMst.param[senpouId-1];
		Type t = senpoulst.GetType();
		String param = "lv" + nextLv;
		FieldInfo f = t.GetField(param);
		int nextStatus =(int)f.GetValue(senpoulst);

		//Required Item
		string requiredItemTyp = senpouItemMst.param [nextLv-1].requiredItemTyp;
		int requiredItemQty = senpouItemMst.param [nextLv-1].requiredItemQty;
		int requiredMoney = senpouItemMst.param [nextLv-1].requiredMoney;

		senpouList.Add(nextStatus.ToString ());
		senpouList.Add (requiredItemTyp);
		senpouList.Add (requiredItemQty.ToString());
		senpouList.Add (requiredMoney.ToString());

		return senpouList;
	}

}
