 using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class ShisyaSelect : MonoBehaviour {

	//Common
	public GameObject Content;
	public GameObject baseObj;
	public int shisyaId = 0;
	public string title = "";
	public string serihu = "";
	public string shisyaName = "";

	//From Daimyo
	public string key = "";
	public int srcDaimyoId = 0;
	public string srcDaimyoName = "";
	public int targetKuniId = 0;
	public string targetKuniName = "";
	public int dstDaimyoId = 0;
	public string dstDaimyoName = "";
	public int syogunDaimyoId = 0;
	public string syogunDaimyoName = "";



	//Requried
	public int moneyNo = 0;
	public int hyourouNo = 0;
	public string itemCd = "";
	public string itemId = "";
	public string itemDataCd = "";
	public string itemName = "";

	public GameObject Required1;
	public GameObject Required2;
	public GameObject Circle;
	public GameObject CyouteiSelectScrollView;
	public GameObject SyouninSelectScrollView;
    public GameObject RequestBuyItem;

	public void Awake(){
		Required1 = GameObject.Find ("Required1").gameObject;
		Required2 = GameObject.Find ("Required2").gameObject;
		Circle = GameObject.Find ("Circle").gameObject;
		CyouteiSelectScrollView = GameObject.Find("CyouteiSelectScrollView").gameObject;
		SyouninSelectScrollView = GameObject.Find("SyouninSelectScrollView").gameObject;
        RequestBuyItem = GameObject.Find("RequestBuyItem").gameObject;
        
    }


	public void OnClick(){

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources[2].Play();

		Color Select = new Color (76f / 255f, 50f / 255f, 18f / 255f, 80f / 255f);
		Color unSelect = new Color (255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);
		foreach (Transform obj in Content.transform) {
			obj.GetComponent<Image>().color = unSelect;
		}
		GetComponent<Image> ().color = Select;


		baseObj = GameObject.Find ("Base").gameObject;
		baseObj.transform.Find ("Title").GetComponent<Text> ().text = title;
		baseObj.transform.Find ("Mask").transform.Find ("Image").GetComponent<Image> ().sprite = transform.Find ("Image").GetComponent<Image> ().sprite;
		baseObj.transform.Find ("Comment").transform.Find ("Text").GetComponent<Text> ().text = serihu;
		baseObj.transform.Find ("Name").GetComponent<Text> ().text = shisyaName;

		reqruiedItemView(shisyaId);

        //Request to purchase item
        if(shisyaId == 5 || shisyaId == 19) {
            RequestBuyItem.gameObject.SetActive(true);
            Kahou kahou = new Kahou();
            string rank = kahou.getKahouRank(itemCd, int.Parse(itemId));
            string kahouPath = "";
            if (rank == "C") {
                kahouPath = "Prefabs/Kahou/" + itemCd + "C";
            }else {
                kahouPath = "Prefabs/Kahou/" + itemCd + itemId;
            }
            RequestBuyItem.GetComponent<Image>().sprite =
            Resources.Load(kahouPath, typeof(Sprite)) as Sprite;
            //string kahouMsg = kahou.getRamdomKahou

            KahouStatusGet kahouSts = new KahouStatusGet();
            List<string> kahouInfoList = new List<string>();
            kahouInfoList = kahouSts.getKahouInfo(itemCd, int.Parse(itemId));
            string effect = kahouInfoList[0] + "\n" + kahouInfoList[2] + " +" + kahouInfoList[3] + kahouInfoList[4];
            RequestBuyItem.GetComponent<PopItem>().text = effect;
        }else {
            RequestBuyItem.gameObject.SetActive(false);
        }
        
		//Yes/No Button
        if(shisyaId==6 || shisyaId == 22) {
            //Doumei Haki or Surrender
			//disable button
			Color OKbtnColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
			Color OKtxtColor = new Color (190f / 255f, 190f / 255f, 0f / 255f, 255f / 255f);
			Color NGColor = new Color (118f / 255f, 118f / 255f, 45f / 255f, 255f / 255f);
			GameObject ysBtn = GameObject.Find ("YesButton").gameObject;
			ysBtn.GetComponent<Button> ().enabled = true;
			ysBtn.GetComponent<Image> ().color = OKbtnColor;
			ysBtn.transform.Find ("Text").GetComponent<Text> ().color = OKtxtColor;

			GameObject noBtn = GameObject.Find ("NoButton").gameObject;
			noBtn.GetComponent<Button>().enabled = false;
			noBtn.GetComponent<Image> ().color = NGColor;
			noBtn.transform.Find("Text").GetComponent<Text> ().color = NGColor;

			ysBtn.GetComponent<DoShisya> ().slot = gameObject;
			noBtn.GetComponent<DoShisya> ().slot = gameObject;
		
        } else {
            Color OKbtnColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            Color OKtxtColor = new Color(190f / 255f, 190f / 255f, 0f / 255f, 255f / 255f);
            GameObject ysBtn = GameObject.Find("YesButton").gameObject;
            ysBtn.GetComponent<Button>().enabled = true;
            ysBtn.GetComponent<Image>().color = OKbtnColor;
            ysBtn.transform.Find("Text").GetComponent<Text>().color = OKtxtColor;

            GameObject noBtn = GameObject.Find("NoButton").gameObject;
            noBtn.GetComponent<Button>().enabled = true;
            noBtn.GetComponent<Image>().color = OKbtnColor;
            noBtn.transform.Find("Text").GetComponent<Text>().color = OKtxtColor;

            ysBtn.GetComponent<DoShisya>().slot = gameObject;
            noBtn.GetComponent<DoShisya>().slot = gameObject;
        }
	}

	public void reqruiedItemView(int shisyaId){
		Shisya shisya = new Shisya ();
		bool selectFlg = shisya.getSelectFlg (shisyaId);


		if (!selectFlg) {
			//initialization
			if (GameObject.Find ("Required1")) {
				GameObject.Find ("Required1").SetActive (true);
				if (GameObject.Find ("Required2")) {
					GameObject.Find ("Required2").SetActive (true);
				}
				GameObject.Find ("Circle").SetActive (true);
				if (GameObject.Find ("CyouteiSelectScrollView")) {
					GameObject.Find ("CyouteiSelectScrollView").SetActive (false);
				}
				if (GameObject.Find ("SyouninSelectScrollView")) {
					GameObject.Find ("SyouninSelectScrollView").SetActive (false);
				}
			} else {
				BackMain script = GameObject.Find ("Back").GetComponent<BackMain> ();
				Required1.gameObject.SetActive (true);
				Required2.gameObject.SetActive (true);
				Circle.gameObject.SetActive (true);
				CyouteiSelectScrollView.gameObject.SetActive (false);
				SyouninSelectScrollView.gameObject.SetActive (false);
			}

			//Item 1
			string requried1 = shisya.getYesRequried1 (shisyaId);
			if (requried1 == "no") {
				baseObj.transform.Find ("Required1").gameObject.SetActive (false);

			} else if (requried1 == "random") {
				baseObj.transform.Find ("Required1").gameObject.SetActive (true);

				if (moneyNo != 0) {
					string item1Path = "Prefabs/Common/Sprite/money";
					baseObj.transform.Find ("Required1").GetComponent<Image> ().sprite = 
						Resources.Load (item1Path, typeof(Sprite)) as Sprite;

					baseObj.transform.Find ("Required1").transform.Find ("Value").GetComponent<Text> ().text = "x " + moneyNo.ToString ();
				} else {
					Kahou kahou = new Kahou ();
					string rank = kahou.getKahouRank(itemCd, int.Parse(itemId));
					string kahouPath = "";
					if (rank == "C") {
						kahouPath = "Prefabs/Kahou/" + itemCd + "C";
					} else {
						kahouPath = "Prefabs/Kahou/" + itemCd + itemId;
					}
					baseObj.transform.Find ("Required1").GetComponent<Image> ().sprite = 
					Resources.Load (kahouPath, typeof(Sprite)) as Sprite;

					baseObj.transform.Find ("Required1").transform.Find ("Value").GetComponent<Text> ().text = "x 1";
				}

			} else if (requried1 == "randomKahou") {
				baseObj.transform.Find ("Required1").gameObject.SetActive (true);

				//Kahou
				string item1Path = "Prefabs/Common/Sprite/money";
				baseObj.transform.Find ("Required1").GetComponent<Image> ().sprite = 
					Resources.Load (item1Path, typeof(Sprite)) as Sprite;
				
				baseObj.transform.Find ("Required1").transform.Find ("Value").GetComponent<Text> ().text = "x " + moneyNo.ToString();


			} else {
				baseObj.transform.Find ("Required1").gameObject.SetActive (true);
				List<string> requried1List = new List<string> ();

				char[] delimiterChars = { ':' };
				if (requried1.Contains (":")) {
					requried1List = new List<string> (requried1.Split (delimiterChars));
				} else {
					requried1List.Add (requried1);
				}
				string item1Path = "Prefabs/Common/Sprite/" + requried1List [0];
				baseObj.transform.Find ("Required1").GetComponent<Image> ().sprite = 
					Resources.Load (item1Path, typeof(Sprite)) as Sprite;

				baseObj.transform.Find ("Required1").transform.Find("Value").GetComponent<Text>().text =  "x " + requried1List [1];
			}


			//Item 2
			string requried2 = shisya.getYesRequried2 (shisyaId);
			if (requried2 == "no") {
				baseObj.transform.Find ("Required2").gameObject.SetActive (false);

			} else {

				baseObj.transform.Find ("Required2").gameObject.SetActive (true);
				List<string> requried2List = new List<string> ();
				char[] delimiterChars = { ':' };
				if (requried2.Contains (":")) {
					requried2List = new List<string> (requried2.Split (delimiterChars));
				} else {
					requried2List.Add (requried2);
				}
				string item2Path = "Prefabs/Common/Sprite/" + requried2List [0];
				baseObj.transform.Find ("Required2").GetComponent<Image> ().sprite = 
					Resources.Load (item2Path, typeof(Sprite)) as Sprite;

				baseObj.transform.Find ("Required2").transform.Find("Value").GetComponent<Text>().text =  "x " + requried2List [1];
			}




		} else {
			//Select
			if (GameObject.Find ("Required1")) {
				GameObject.Find ("Required1").SetActive (false);
				if (GameObject.Find ("Required2")) {
					GameObject.Find ("Required2").SetActive (false);
				}
				GameObject.Find ("Circle").SetActive (false);
			}
			if (shisyaId == 16) {
				//Cyoutei
				if (GameObject.Find ("CyouteiSelectScrollView")) {
					GameObject viewObj = GameObject.Find ("CyouteiSelectScrollView").gameObject;
					viewObj.SetActive (true);
					viewObj.transform.Find ("CyouteiContent").transform.Find ("Ge").GetComponent<ShisyaSyoukaijyoSelect> ().Start ();
					viewObj.transform.Find ("CyouteiContent").transform.Find ("Ge").GetComponent<ShisyaSyoukaijyoSelect> ().OnClick ();
					
				} else {
					CyouteiSelectScrollView.gameObject.SetActive (true);
					CyouteiSelectScrollView.transform.Find ("CyouteiContent").transform.Find ("Ge").GetComponent<ShisyaSyoukaijyoSelect> ().Start ();
					CyouteiSelectScrollView.transform.Find ("CyouteiContent").transform.Find ("Ge").GetComponent<ShisyaSyoukaijyoSelect> ().OnClick ();
				}
				if (GameObject.Find ("SyouninSelectScrollView")) {
					GameObject.Find ("SyouninSelectScrollView").SetActive (false);
				} else {
					SyouninSelectScrollView.gameObject.SetActive (false);
				}

			} else if (shisyaId == 18) {
				//Syounin
				if (GameObject.Find ("CyouteiSelectScrollView")) {
					GameObject.Find ("CyouteiSelectScrollView").SetActive (false);
				} else {
					CyouteiSelectScrollView.gameObject.SetActive (false);
				}
				if (GameObject.Find ("SyouninSelectScrollView")) {
					GameObject viewObj = GameObject.Find ("SyouninSelectScrollView").gameObject;
					viewObj.SetActive (true);
					viewObj.transform.Find ("SyouninContent").transform.Find ("Ge").GetComponent<ShisyaSyoukaijyoSelect> ().Start ();
					viewObj.transform.Find ("SyouninContent").transform.Find ("Ge").GetComponent<ShisyaSyoukaijyoSelect> ().OnClick ();

				} else {
					SyouninSelectScrollView.gameObject.SetActive (true);
					SyouninSelectScrollView.transform.Find ("SyouninContent").transform.Find ("Ge").GetComponent<ShisyaSyoukaijyoSelect> ().Start ();
					SyouninSelectScrollView.transform.Find ("SyouninContent").transform.Find ("Ge").GetComponent<ShisyaSyoukaijyoSelect> ().OnClick ();

				}

			} else if (shisyaId == 19) {
				//Nanban
			}
		}


	}
    
}
