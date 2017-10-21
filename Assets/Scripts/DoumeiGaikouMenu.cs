using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System.Collections.Generic;

public class DoumeiGaikouMenu : MonoBehaviour {

	public int kuniDiff = 1;
	public int daimyoId = 0;

	public void OnClick(){

		//SE
		AudioSource sound = GameObject.Find ("SEController").GetComponent<AudioSource> ();
		sound.Play ();

		CloseBoard close = GameObject.Find ("close").GetComponent<CloseBoard> ();
		close.layer = close.layer + 1;
		
		int nowHyourou = PlayerPrefs.GetInt ("hyourou");
		bool hyourouOKflg = false;
		
		//Kuni Qty Difference
		int myKuniQty = GameObject.Find ("GameController").GetComponent<MainStageController> ().myKuniQty;
		int tKuniQty = close.kuniQty;
		if (myKuniQty > tKuniQty) {
			kuniDiff = kuniDiff + myKuniQty - tKuniQty;
		}

		GaikouMenu actBusyoScript = new GaikouMenu ();
		bool isExistFlg = actBusyoScript.isExistActiveBusyo ();
		Message msg = new Message();
        int langId = PlayerPrefs.GetInt("langId");
        if (name == "Mitsugi") {

			if (isExistFlg) {

                //Menu Handling                
                if (langId == 2) {
                    GameObject.Find("kuniName").GetComponent<Text>().text = "Gift";
                }else {
                    GameObject.Find("kuniName").GetComponent<Text>().text = "貢物";
                }
                OffGaikouMenu ();
				
				//Mitsugi Object
				string mitugiPath = "Prefabs/Map/gaikou/MitsugiObj";
				GameObject mitsugiObj = Instantiate (Resources.Load (mitugiPath)) as GameObject;
				mitsugiObj.transform.SetParent (GameObject.Find ("smallBoard(Clone)").transform);
				mitsugiObj.transform.localScale = new Vector3 (1, 1, 1);
				
				//Do Button
				string buttonPath = "Prefabs/Map/gaikou/DoGaikouBtn";
				GameObject btn = Instantiate (Resources.Load (buttonPath)) as GameObject;
				btn.transform.SetParent (mitsugiObj.transform);
				btn.transform.localScale = new Vector3 (1, 1, 1);
				btn.name = "DoMitsugiBtn";
				RectTransform btn_transform = btn.GetComponent<RectTransform> ();
				btn_transform.anchoredPosition = new Vector3 (0, -200, 0);
                if (langId == 2) {
                    btn.transform.FindChild("Text").GetComponent<Text>().text = "Gift";
                }else {
                    btn.transform.FindChild("Text").GetComponent<Text>().text = "貢物";
                }
                //Slider
                string sliderPath = "Prefabs/Map/common/MoneySlider";
				GameObject slider = Instantiate (Resources.Load (sliderPath)) as GameObject;
				slider.transform.SetParent (mitsugiObj.transform);
				slider.transform.localScale = new Vector3 (1, 1.2f, 1);
				RectTransform slider_transform = slider.GetComponent<RectTransform> ();
				slider_transform.anchoredPosition = new Vector3 (-70, -90, 0);
				
				Slider sliderScript = slider.GetComponent<Slider> ();
				int nowMoney = PlayerPrefs.GetInt ("money");
				nowMoney = nowMoney / 1000;
				if (nowMoney < 1) {
					sliderScript.enabled = false;
					
				} else {
					sliderScript.minValue = 1;
					btn.GetComponent<DoGaikou> ().moneyOKflg = true;
					btn.GetComponent<DoGaikou> ().paiedMoney = 1000;
					
					if (nowMoney < 10) {
						sliderScript.maxValue = nowMoney;
					} else {
						sliderScript.maxValue = 10;
					}
				}
				slider.GetComponent<MoneySlider> ().doBtn = btn;
				
				//Hyourou Check
				GaikouMenu gaikou = new GaikouMenu ();
				hyourouOKflg = gaikou.HyourouCheck (nowHyourou);
				btn.GetComponent<DoGaikou> ().hyourouOKflg = hyourouOKflg;
				
				//Busyo Scroll View
				gaikou.ScrollView (mitsugiObj, btn);
				
				//Set Obj
				GameObject.Find ("return").GetComponent<MenuReturn> ().layer2 = mitsugiObj;
			
			} else {
				msg.makeMessage (msg.getMessage(8));
			}			


		} else if (name == "Kyoutou") {

			if (isExistFlg) {
                //Menu Handling
                if (langId == 2) {
                    GameObject.Find("kuniName").GetComponent<Text>().text = "Joint Battle";
                }else {
                    GameObject.Find("kuniName").GetComponent<Text>().text = "共闘";
                }
                OffGaikouMenu ();

				string path = "Prefabs/Map/gaikou/KyoutouObj";
				GameObject obj = Instantiate (Resources.Load (path)) as GameObject;
				obj.transform.SetParent (GameObject.Find ("smallBoard(Clone)").transform);
				obj.transform.localScale = new Vector3 (1, 1, 1);

				//Do Button
				string buttonPath = "Prefabs/Map/gaikou/DoGaikouBtn";
				GameObject btn = Instantiate (Resources.Load (buttonPath)) as GameObject;
				btn.transform.SetParent (obj.transform);
				btn.transform.localScale = new Vector3 (1, 1, 1);
				btn.name = "DoKyoutouBtn";
				RectTransform btn_transform = btn.GetComponent<RectTransform> ();
				btn_transform.anchoredPosition = new Vector3 (0, -200, 0);

                if (langId == 2) {
                    btn.transform.FindChild("Text").GetComponent<Text>().text = "Do";
                }else {
                    btn.transform.FindChild("Text").GetComponent<Text>().text = "共闘";
                }
                //Money
                int nowMoney = PlayerPrefs.GetInt ("money");
				if (nowMoney >= 3000) {
					btn.GetComponent<DoGaikou> ().moneyOKflg = true;
				}
				btn.GetComponent<DoGaikou> ().paiedMoney = 3000;

				//Hyourou Check
				GaikouMenu gaikou = new GaikouMenu ();
				hyourouOKflg = gaikou.HyourouCheck (nowHyourou);
				btn.GetComponent<DoGaikou> ().hyourouOKflg = hyourouOKflg;

				int daimyoId = GameObject.Find ("close").GetComponent<CloseBoard> ().daimyoId;
				kuniScrollView (obj, daimyoId.ToString (), btn);

				//Set Obj
				GameObject.Find ("return").GetComponent<MenuReturn> ().layer2 = obj;
			} else {
				msg.makeMessage (msg.getMessage(8));
			}
		} else if (name == "Haki") {
			//Back Cover
			string backPath = "Prefabs/Busyo/back";
			GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
			back.transform.SetParent (GameObject.Find ("Panel").transform);
			back.transform.localScale = new Vector2 (1, 1);
			RectTransform backTransform = back.GetComponent<RectTransform> ();
			backTransform.anchoredPosition = new Vector3 (0, 0, 0);
			
			//Message Box
			string msgPath = "Prefabs/Map/gaikou/DoumeiHakiConfirm";
			GameObject msgObj = Instantiate (Resources.Load (msgPath)) as GameObject;
			msgObj.transform.SetParent (GameObject.Find ("Panel").transform);
			msgObj.transform.localScale = new Vector2 (1, 1);
			RectTransform msgTransform = msgObj.GetComponent<RectTransform> ();
			msgTransform.anchoredPosition = new Vector3 (0, 0, 0);
			msgTransform.name = "DoumeiHakiConfirm";

			int daimyoId = GameObject.Find ("close").GetComponent<CloseBoard> ().daimyoId;
			msgObj.transform.FindChild ("YesButton").GetComponent<DoDoumeiHaki> ().daimyoId = daimyoId;
			Daimyo daimyo = new Daimyo ();
			string daimyoName = daimyo.getName (daimyoId,langId);
			msgObj.transform.FindChild ("YesButton").GetComponent<DoDoumeiHaki> ().daimyoName = daimyoName;

			close.layer = close.layer - 1;
		
		} else if (name == "Koueki") {

			int yukoudo = close.yukoudo;
			string myDaimyoName = GameObject.Find ("DaimyoValue").GetComponent<Text> ().text;

			if (yukoudo >= 20) {


                if (langId == 2) {
                    GameObject.Find("kuniName").GetComponent<Text>().text = "Trade";
                }else {
                    GameObject.Find("kuniName").GetComponent<Text>().text = "交易";
                }
                OffGaikouMenu ();
				
				string kouekiPath = "Prefabs/Map/gaikou/kouekiObj";
				GameObject kouekiObj = Instantiate (Resources.Load (kouekiPath)) as GameObject;
				kouekiObj.transform.SetParent (GameObject.Find ("smallBoard(Clone)").transform);
				kouekiObj.transform.localScale = new Vector3 (1, 1, 1);
				kouekiObj.transform.FindChild ("Buy").GetComponent<KouekiMenu> ().kouekiObj = kouekiObj;
				kouekiObj.transform.FindChild ("Change").GetComponent<KouekiMenu> ().kouekiObj = kouekiObj;
				
				kouekiObj.transform.FindChild ("Buy").GetComponent<KouekiMenu> ().clickBuy ();
				GameObject.Find ("return").GetComponent<MenuReturn> ().layer2 = kouekiObj;
				
				//Daimyo Change
				GameObject daimyo = kouekiObj.transform.FindChild ("Daimyo").gameObject;
				foreach (Transform obj in daimyo.transform) {
					Destroy (obj);
				}

                //SerihuChange
                if (langId == 2) {
                    if (20 <= yukoudo && yukoudo < 30)
                        GameObject.Find("SerihuText").GetComponent<Text>().text = myDaimyoName + "... Why are coming here?";
                    if (30 <= yukoudo && yukoudo < 40)
                        GameObject.Find("SerihuText").GetComponent<Text>().text = "Long time no see. Recently the price of trade goods has raised";
                    if (40 <= yukoudo && yukoudo < 50)
                        GameObject.Find("SerihuText").GetComponent<Text>().text = "Long time no see. Please see our trade goods.";
                    if (50 <= yukoudo && yukoudo < 60)
                        GameObject.Find("SerihuText").GetComponent<Text>().text = "We can trade depends on the condition. Please see our trade goods.";
                    if (60 <= yukoudo && yukoudo < 70)
                        GameObject.Find("SerihuText").GetComponent<Text>().text = "Lord " + myDaimyoName + ", How about this item? It's a rare isn't it?";
                    if (70 <= yukoudo && yukoudo < 80)
                        GameObject.Find("SerihuText").GetComponent<Text>().text = "Lord " + myDaimyoName + ", I've heared your good rumor. Please check our goods.";
                    if (80 <= yukoudo && yukoudo < 90)
                        GameObject.Find("SerihuText").GetComponent<Text>().text = "Lord " + myDaimyoName + ",  I just wanted to talk to you. Please enjoy our country.";
                    if (90 <= yukoudo)
                        GameObject.Find("SerihuText").GetComponent<Text>().text = "Oh, Lord " + myDaimyoName + ", Let's make good trade! I got a special item.";

                }
                else {
                    if (20 <= yukoudo && yukoudo < 30)
					    GameObject.Find ("SerihuText").GetComponent<Text> ().text = myDaimyoName + "か・・・何用で参ったのかな。";
				    if (30 <= yukoudo && yukoudo < 40)
					    GameObject.Find ("SerihuText").GetComponent<Text> ().text = "お久しゅうござるな。近頃は交易品が値上げしておりましてのう。";
				    if (40 <= yukoudo && yukoudo < 50)
					    GameObject.Find ("SerihuText").GetComponent<Text> ().text = "お久しゅうござるな。交易品を見ていって下され。";
				    if (50 <= yukoudo && yukoudo < 60)
					    GameObject.Find ("SerihuText").GetComponent<Text> ().text = "条件次第で交易致しますぞ。さ、交易品を見ていって下され。";
				    if (60 <= yukoudo && yukoudo < 70)
					    GameObject.Find ("SerihuText").GetComponent<Text> ().text = myDaimyoName + "殿、この品は如何かな。珍しいものでござろう。";
				    if (70 <= yukoudo && yukoudo < 80)
					    GameObject.Find ("SerihuText").GetComponent<Text> ().text = myDaimyoName + "殿、ご活躍は噂で聞いておりますぞ。是非品を行ってくだされ。";
				    if (80 <= yukoudo && yukoudo < 90)
					    GameObject.Find ("SerihuText").GetComponent<Text> ().text = myDaimyoName + "殿、丁度話がしたいと思うておったところよ。ゆるりとして行ってくだされ。";
				    if (90 <= yukoudo )
					    GameObject.Find ("SerihuText").GetComponent<Text> ().text = "おお、" + myDaimyoName + "殿では御座らんか！是非見て参られよ。\n素晴らしい品が入ったのじゃ。";

                }

                int daimyoBusyoId = close.daimyoBusyoId;
				string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
				GameObject busyo = Instantiate (Resources.Load (busyoPath)) as GameObject;
				busyo.name = daimyoBusyoId.ToString ();
				busyo.transform.SetParent (daimyo.transform);
				busyo.transform.localScale = new Vector2 (4, 4);
				busyo.GetComponent<DragHandler> ().enabled = false;
				
				RectTransform busyoTransform = busyo.GetComponent<RectTransform> ();
				busyoTransform.anchoredPosition = new Vector3 (70, 80, 0);
				busyoTransform.sizeDelta = new Vector2 (35, 40);

				
			} else {
				msg.makeMessage (msg.getMessage(9));
			}


		} else if (name == "Syuppei") {
			
			if (isExistFlg) {
				//Menu Handling

				//Gunzei Exist Check
				bool gunzeiExistFlg = false;
				int daimyoId = GameObject.Find ("close").GetComponent<CloseBoard> ().daimyoId;
				foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Gunzei")){
					int gunzeiSrcDaimyoId = obs.GetComponent<Gunzei>().srcDaimyoId;

					if(daimyoId == gunzeiSrcDaimyoId){
						gunzeiExistFlg = true;
					}
				}
				if (gunzeiExistFlg) {
					msg.makeMessage (msg.getMessage(10));
				} else {


                    if (langId == 2) {
                        GameObject.Find("kuniName").GetComponent<Text>().text = "Battle Request";
                    }
                    else {
                        GameObject.Find("kuniName").GetComponent<Text>().text = "出兵願";
                    }
                    OffGaikouMenu ();

					string path = "Prefabs/Map/gaikou/SyuppeiObj";
					GameObject obj = Instantiate (Resources.Load (path)) as GameObject;
					obj.transform.SetParent (GameObject.Find ("smallBoard(Clone)").transform);
					obj.transform.localScale = new Vector3 (1, 1, 1);

					//Do Button
					string buttonPath = "Prefabs/Map/gaikou/DoGaikouBtn";
					GameObject btn = Instantiate (Resources.Load (buttonPath)) as GameObject;
					btn.transform.SetParent (obj.transform);
					btn.transform.localScale = new Vector3 (1, 1, 1);
					btn.name = "DoSyuppeiBtn";
					RectTransform btn_transform = btn.GetComponent<RectTransform> ();
					btn_transform.anchoredPosition = new Vector3 (0, -200, 0);

                    if (langId == 2) {
                        btn.transform.FindChild("Text").GetComponent<Text>().text = "Request";
                    }
                    else {
                        btn.transform.FindChild("Text").GetComponent<Text>().text = "依頼";
                    }
                    //Money
                    int nowMoney = PlayerPrefs.GetInt ("money");
					if (nowMoney >= 3000) {
						btn.GetComponent<DoGaikou> ().moneyOKflg = true;
					}
					btn.GetComponent<DoGaikou> ().paiedMoney = 3000;

					//Hyourou Check
					GaikouMenu gaikou = new GaikouMenu ();
					hyourouOKflg = gaikou.HyourouCheck (nowHyourou);
					btn.GetComponent<DoGaikou> ().hyourouOKflg = hyourouOKflg;

					SyuppeiKuniScrollView (obj, daimyoId.ToString (), btn);

					//Set Obj
					GameObject.Find ("return").GetComponent<MenuReturn> ().layer2 = obj;
				}
			} else {
				msg.makeMessage (msg.getMessage(8));
			}
			
		}




	}

	public void kuniScrollView(GameObject baseObj,  string targetDaimyo, GameObject btn){

		GameObject content = baseObj.transform.FindChild("scroll").transform.FindChild("Content").gameObject;
        int langId = PlayerPrefs.GetInt("langId");

        string seiryoku = PlayerPrefs.GetString ("seiryoku");
		char[] delimiterChars = {','};
		List<string> seiryokuList = new List<string>();
		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));

		//my kuni
		int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
		string myKuni = PlayerPrefs.GetString ("clearedKuni"); 
		List<int> myKuniList = new List<int>();
        int myDaimyoId = PlayerPrefs.GetInt("myDaimyo");
        for (int i = 0; i < seiryokuList.Count; i++) {
            int seiryokuId = int.Parse(seiryokuList[i]);
            if (seiryokuId == myDaimyoId) {
                int kuniId = i + 1;
                myKuniList.Add(kuniId);
            }
        }
        
        if (myKuni.Contains (",")) {
			List<string> tempMyKuniList = new List<string> (myKuni.Split (delimiterChars));
			myKuniList = tempMyKuniList.ConvertAll(x => int.Parse(x));
		} else {
			myKuniList.Add(int.Parse(myKuni));
		}

		//doumei daimyo's opne kuni
		KuniInfo kuni = new KuniInfo ();
		List<int> doumeiOpenKuniList = new List<int>();
		List<int> doumeiKuniList = new List<int>();
		for(int i=0; i<seiryokuList.Count; i++){
			string tempDaimyo = seiryokuList[i];
			if(tempDaimyo == targetDaimyo){
				int doumeiKuniId = i + 1;
				doumeiKuniList.Add(doumeiKuniId);
				doumeiOpenKuniList.AddRange(kuni.getMappingKuni(doumeiKuniId));
			}
		}

		//"doumei daimyo's opne kuni" minus "my kuni"  
		List<int> doumeiOpenKuniMinusMyKuniList = new List<int>();
		foreach(int n in doumeiOpenKuniList){

			if(!myKuniList.Contains(n)){
				doumeiOpenKuniMinusMyKuniList.Add(n);
			}
		}

		//"doumei daimyo's doumei check
		string tempDoumei = "doumei" + targetDaimyo;
		string doumei = PlayerPrefs.GetString (tempDoumei);
		List<string> doumeiList = new List<string>();
        Debug.Log(doumei);
        if(doumei != "") {
		    if (doumei.Contains (",")) {
			    doumeiList = new List<string> (doumei.Split (delimiterChars));
		    } else {
			    doumeiList.Add(doumei);
		    }
        }

        if (doumei != null && doumei != "") {
			for(int t=0; t<doumeiOpenKuniMinusMyKuniList.Count; t++){
				//foreach (int n in doumeiOpenKuniMinusMyKuniList) {
				string checkDaimyoId = seiryokuList [doumeiOpenKuniMinusMyKuniList[t] - 1];
				if (doumeiList.Contains (checkDaimyoId)) {
					doumeiOpenKuniMinusMyKuniList.Remove (doumeiOpenKuniMinusMyKuniList[t]);
				}
			}
		}



		//Compare "doumei open" with "my open"
		string myOpenKuni = PlayerPrefs.GetString ("openKuni"); 
		List<int> myOpenKuniList = new List<int>();
		if (myOpenKuni.Contains (",")) {
			List<string> tempMyOpenKuniList = new List<string> (myOpenKuni.Split (delimiterChars));
			myOpenKuniList = tempMyOpenKuniList.ConvertAll(x => int.Parse(x));
		} else {
			myOpenKuniList.Add(int.Parse(myOpenKuni));
		}

		List<int> sameTargetKuniList = new List<int>();
		foreach(int n in myOpenKuniList){
			if(doumeiOpenKuniMinusMyKuniList.Contains(n)){
				if(!doumeiKuniList.Contains(n)){
					if(!sameTargetKuniList.Contains(n)){
						sameTargetKuniList.Add(n);
					}
				}
			}
		}


		/*Slot making*/
		string temp = "gaikou" + targetDaimyo;
		int myYukoudo = PlayerPrefs.GetInt(temp);

        //Get Chiryaku
        Daimyo daimyoScript = new Daimyo();
		Entity_daimyo_mst daimyoMst = Resources.Load ("Data/daimyo_mst") as Entity_daimyo_mst;
		int myDaimyoBusyoId = daimyoMst.param [myDaimyo - 1].busyoId;
		BusyoInfoGet busyo = new BusyoInfoGet ();
		int chiryaku = busyo.getMaxDfc (myDaimyoBusyoId);
		//StatusGet sts = new StatusGet();
		//int lv = PlayerPrefs.GetInt (myDaimyoBusyoId.ToString());
		//float chiryaku = (float)sts.getDfc(myDaimyoBusyoId,lv);
		//chiryaku = chiryaku *10;


		string slotPath = "Prefabs/Map/common/kuniSlot";
		for (int i=0; i<sameTargetKuniList.Count; i++) {
			GameObject prefab = Instantiate (Resources.Load (slotPath)) as GameObject;
			prefab.transform.SetParent(content.transform);
			prefab.transform.localScale = new Vector3 (1, 1, 1);

			int kuniId = sameTargetKuniList[i];
			int daimyoId = int.Parse (seiryokuList [kuniId - 1]);
			// daimyoName = daimyoMst.param [daimyoId - 1].daimyoName;
            string daimyoName = daimyoScript.getName(daimyoId,langId);

            string imagePath = "Prefabs/Kamon/" + daimyoId.ToString();
            prefab.transform.FindChild("Image").GetComponent<Image>().sprite =
                Resources.Load(imagePath, typeof(Sprite)) as Sprite;

            string theirYukouTemp = "";
			if(int.Parse(targetDaimyo) < daimyoId){
				theirYukouTemp = targetDaimyo + "gaikou" + daimyoId.ToString();
			}else{
				theirYukouTemp = daimyoId.ToString() + "gaikou" + targetDaimyo;
			}

			int theirYukoudo = PlayerPrefs.GetInt(theirYukouTemp);

			string kuniName = kuni.getKuniName(kuniId,langId);
			prefab.transform.FindChild("KuniValue").GetComponent<Text>().text = kuniName;
			prefab.transform.FindChild("DaimyoValue").GetComponent<Text>().text = daimyoName;
			prefab.GetComponent<GaikouKuniSelect>().Content = content;
			prefab.GetComponent<GaikouKuniSelect>().Btn = btn;

			prefab.GetComponent<GaikouKuniSelect>().myYukoudo = myYukoudo;
			prefab.GetComponent<GaikouKuniSelect>().chiryaku = chiryaku;
			prefab.GetComponent<GaikouKuniSelect>().kuniDiff = kuniDiff;
			prefab.GetComponent<GaikouKuniSelect>().theirYukoudo = theirYukoudo;
			prefab.GetComponent<GaikouKuniSelect>().kuniName = kuniName;
			prefab.GetComponent<GaikouKuniSelect>().targetKuniId = kuniId;

			if(i == 0){
				prefab.GetComponent<GaikouKuniSelect>().OnClick();
			}
		}

		if (sameTargetKuniList.Count == 0) {
			string msgPath = "Prefabs/Map/gaikou/NoKyoutouMsg";
			GameObject msg = Instantiate (Resources.Load (msgPath)) as GameObject;
			msg.transform.SetParent(GameObject.Find("scroll").transform);
			msg.transform.localScale = new Vector3 (0.17f, 0.2f, 1);
			RectTransform msgTransform = msg.GetComponent<RectTransform> ();
			msgTransform.anchoredPosition = new Vector3 (-260, -115, 0);

			GameObject a = baseObj.transform.FindChild ("RequiredMoney").gameObject;
			GameObject b = baseObj.transform.FindChild ("KyoutouRatio").gameObject;
			Destroy(a);
			Destroy(b);

			btn.GetComponent<Button>().enabled = false;
            Color NGClorBtn = new Color(133 / 255f, 133 / 255f, 80 / 255f, 255f / 255f);
            Color NGClorTxt = new Color(90 / 255f, 90 / 255f, 40 / 255f, 255f / 255f);
            btn.GetComponent<Image>().color = NGClorBtn;
            btn.transform.FindChild("Text").GetComponent<Text>().color = NGClorTxt;

            btn.transform.FindChild("hyourouIcon").GetComponent<Image>().color = NGClorBtn;
            btn.transform.FindChild("hyourouIcon").transform.FindChild("hyourouNoValue").GetComponent<Text>().color = NGClorTxt;
            btn.transform.FindChild("hyourouIcon").transform.FindChild("hyourouNoValue").transform.FindChild("syouhiText").GetComponent<Text>().color = NGClorTxt;
        }


	}

	public void SyuppeiKuniScrollView(GameObject baseObj,  string targetDaimyo, GameObject btn){
        //View kuni which have openkuni
        int langId = PlayerPrefs.GetInt("langId");
        GameObject content = baseObj.transform.FindChild("scroll").transform.FindChild("Content").gameObject;

		string seiryoku = PlayerPrefs.GetString ("seiryoku");
		char[] delimiterChars = {','};
		List<string> seiryokuList = new List<string>();
		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));

		//my kuni
		int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
		string myKuni = PlayerPrefs.GetString ("clearedKuni"); 
		List<int> myKuniList = new List<int>();
		if (myKuni.Contains (",")) {
			List<string> tempMyKuniList = new List<string> (myKuni.Split (delimiterChars));
			myKuniList = tempMyKuniList.ConvertAll(x => int.Parse(x));
		} else {
			myKuniList.Add(int.Parse(myKuni));
		}

		//doumei daimyo's opne kuni
		KuniInfo kuni = new KuniInfo ();
		List<int> doumeiOpenKuniList = new List<int>();
		List<int> doumeiKuniList = new List<int>();
		for(int i=0; i<seiryokuList.Count; i++){
			string tempDaimyo = seiryokuList[i];
			if(tempDaimyo == targetDaimyo){
				int doumeiKuniId = i + 1;
				doumeiKuniList.Add(doumeiKuniId);
				doumeiOpenKuniList.AddRange(kuni.getMappingKuni(doumeiKuniId));
			}
		}

		//"doumei daimyo's doumei check
		string tempDoumei = "doumei" + targetDaimyo;
		string doumei = PlayerPrefs.GetString (tempDoumei);
		List<string> doumeiList = new List<string>();
		if (doumei.Contains (",")) {
			doumeiList = new List<string> (doumei.Split (delimiterChars));
		} else {
			doumeiList.Add(doumei);
		}


		//"doumei daimyo's opne kuni" minus "my kuni"  /minus Doumei's doumei
		List<int> doumeiOpenKuniMinusMyKuniList = new List<int>();
		foreach(int n in doumeiOpenKuniList){
			if(!myKuniList.Contains(n)){
				if (!doumeiKuniList.Contains (n)) {
					
					if (doumeiList.Count != 0) {
						string checkDaimyoId = seiryokuList [n - 1];				
						if (!doumeiList.Contains (checkDaimyoId)) {
							doumeiOpenKuniMinusMyKuniList.Add (n);
						}
					} else {
						doumeiOpenKuniMinusMyKuniList.Add (n);
					}
				}
			}
		}


		//delete duplication
		List<int> finalKuniList = new List<int>();
		for(int i=0; i<doumeiOpenKuniMinusMyKuniList.Count;i++){
			int tmpKuniId = doumeiOpenKuniMinusMyKuniList [i];

			if(!finalKuniList.Contains(tmpKuniId)){
				finalKuniList.Add (tmpKuniId);
			}
		}

		//Src Kuni List
		List<int> srcKuniList = new List<int>();
		for (int j = 0; j < finalKuniList.Count; j++) {
			int dstKuniId = finalKuniList [j];

			List<int> targetKuniList = new List<int>();
			targetKuniList.AddRange(kuni.getMappingKuni (dstKuniId));
			for (int k = 0; k < targetKuniList.Count; k++) {
				int srcKuniId = targetKuniList [k];
				if (doumeiKuniList.Contains (srcKuniId)) {
					srcKuniList.Add (srcKuniId);
					break;
				}
			}
		}



		/*Slot making*/
		string temp = "gaikou" + targetDaimyo;
		int myYukoudo = PlayerPrefs.GetInt(temp);

        //Get Chiryaku
        //Entity_daimyo_mst daimyoMst = Resources.Load ("Data/daimyo_mst") as Entity_daimyo_mst;
        Daimyo daimyoScript = new Daimyo();
        int myDaimyoBusyoId = daimyoScript.getDaimyoBusyoId(myDaimyo);
		BusyoInfoGet busyo = new BusyoInfoGet ();
		int chiryaku = busyo.getMaxDfc (myDaimyoBusyoId);

		string slotPath = "Prefabs/Map/common/kuniSlot";
		for(int i=0; i<finalKuniList.Count; i++){

			GameObject prefab = Instantiate (Resources.Load (slotPath)) as GameObject;
			prefab.transform.SetParent(content.transform);
			prefab.transform.localScale = new Vector3 (1, 1, 1);

			int kuniId = finalKuniList[i];
			int daimyoId = int.Parse (seiryokuList [kuniId - 1]);
            string daimyoName = daimyoScript.getName(daimyoId,langId);
			int srcKuniId = srcKuniList [i];
			int srcDaimyoId = int.Parse(seiryokuList [srcKuniId - 1]);
            string srcDaimyoName = daimyoScript.getName(srcDaimyoId,langId);

            string imagePath = "Prefabs/Kamon/" + daimyoId.ToString();
            prefab.transform.FindChild("Image").GetComponent<Image>().sprite =
                Resources.Load(imagePath, typeof(Sprite)) as Sprite;

            string theirYukouTemp = "";
			if(int.Parse(targetDaimyo) < daimyoId){
				theirYukouTemp = targetDaimyo + "gaikou" + daimyoId.ToString();
			}else{
				theirYukouTemp = daimyoId.ToString() + "gaikou" + targetDaimyo;
			}

			int theirYukoudo = PlayerPrefs.GetInt(theirYukouTemp);

			string kuniName = kuni.getKuniName(kuniId,langId);
			prefab.transform.FindChild("KuniValue").GetComponent<Text>().text = kuniName;
			prefab.transform.FindChild("DaimyoValue").GetComponent<Text>().text = daimyoName;
			prefab.GetComponent<GaikouKuniSelect>().Content = content;
			prefab.GetComponent<GaikouKuniSelect>().Btn = btn;

			prefab.GetComponent<GaikouKuniSelect>().myYukoudo = myYukoudo;
			prefab.GetComponent<GaikouKuniSelect>().chiryaku = chiryaku;
			prefab.GetComponent<GaikouKuniSelect>().kuniDiff = kuniDiff;
			prefab.GetComponent<GaikouKuniSelect>().theirYukoudo = theirYukoudo;
			prefab.GetComponent<GaikouKuniSelect>().kuniName = kuniName;
			prefab.GetComponent<GaikouKuniSelect>().targetKuniId = kuniId;
			prefab.GetComponent<GaikouKuniSelect>().targetDaimyoId = daimyoId;
			prefab.GetComponent<GaikouKuniSelect>().targetDaimyoName = daimyoName;
			prefab.GetComponent<GaikouKuniSelect>().srcKuniId = srcKuniId;
			prefab.GetComponent<GaikouKuniSelect>().srcDaimyoId = srcDaimyoId;
			prefab.GetComponent<GaikouKuniSelect>().srcDaimyoName = srcDaimyoName;

			if(i == 0){
				prefab.GetComponent<GaikouKuniSelect>().OnClick();
			}

		}

        if(finalKuniList.Count == 0) {
            string msgPath = "Prefabs/Map/gaikou/NoKyoutouMsg";
            GameObject msg = Instantiate(Resources.Load(msgPath)) as GameObject;
            msg.transform.SetParent(GameObject.Find("scroll").transform);
            msg.transform.localScale = new Vector3(0.17f, 0.2f, 1);
            RectTransform msgTransform = msg.GetComponent<RectTransform>();
            msgTransform.anchoredPosition = new Vector3(-260, -115, 0);
            if (langId == 2) {
                msg.GetComponent<Text>().text = "There is no available country.\nWe are surranded by allianced party.";
            }else {
                msg.GetComponent<Text>().text = "出兵可能な国はありませぬ\n周辺国は同盟大名しかおりませぬぞ";
            }

            GameObject a = baseObj.transform.FindChild("RequiredMoney").gameObject;
            GameObject b = baseObj.transform.FindChild("KyoutouRatio").gameObject;
            Destroy(a);
            Destroy(b);

            btn.GetComponent<Button>().enabled = false;
            Color NGClorBtn = new Color(133 / 255f, 133 / 255f, 80 / 255f, 255f / 255f);
            Color NGClorTxt = new Color(90 / 255f, 90 / 255f, 40 / 255f, 255f / 255f);
            btn.GetComponent<Image>().color = NGClorBtn;
            btn.transform.FindChild("Text").GetComponent<Text>().color = NGClorTxt;

            btn.transform.FindChild("hyourouIcon").GetComponent<Image>().color = NGClorBtn;
            btn.transform.FindChild("hyourouIcon").transform.FindChild("hyourouNoValue").GetComponent<Text>().color = NGClorTxt;
            btn.transform.FindChild("hyourouIcon").transform.FindChild("hyourouNoValue").transform.FindChild("syouhiText").GetComponent<Text>().color = NGClorTxt;

        }


    }






	public void OffGaikouMenu(){
		
		GameObject kamon = GameObject.Find ("KamonBack").gameObject;
		GameObject daimyoName = GameObject.Find ("DaimyoName").gameObject;
		GameObject Heiryoku = GameObject.Find ("Heiryoku").gameObject;
		GameObject Yukoudo = GameObject.Find ("Yukoudo").gameObject;
		
		kamon.GetComponent<Image> ().enabled = false;
		daimyoName.GetComponent<Image> ().enabled = false;
		Heiryoku.GetComponent<Image> ().enabled = false;
		Yukoudo.GetComponent<Image> ().enabled = false;
		
		foreach (Transform n in kamon.transform) {
			n.gameObject.GetComponent<Image>().enabled = false;
			if(n.name == "Doumei"){
				n.FindChild("Text").GetComponent<Text>().enabled = false;
			}
		}
		foreach (Transform n in daimyoName.transform) {
			n.gameObject.GetComponent<Text>().enabled = false;
		}
		foreach (Transform n in Heiryoku.transform) {
			n.gameObject.GetComponent<Text>().enabled = false;
		}
		foreach (Transform n in Yukoudo.transform) {
			n.gameObject.GetComponent<Text>().enabled = false;
		}
		
		GameObject newMenu = GameObject.Find ("return").GetComponent<MenuReturn>().NewMenu;
		GameObject MitsugiBtn = newMenu.transform.FindChild ("Mitsugi").gameObject;
		GameObject KyoutouBtn = newMenu.transform.FindChild ("Kyoutou").gameObject;
		GameObject HakiBtn = newMenu.transform.FindChild ("Haki").gameObject;
		GameObject KouekiBtn = newMenu.transform.FindChild ("Koueki").gameObject;
		GameObject SyuppeiBtn = newMenu.transform.FindChild ("Syuppei").gameObject;

		MitsugiBtn.GetComponent<Image> ().enabled = false;
		MitsugiBtn.GetComponent<Button> ().enabled = false;
		MitsugiBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
		
		KyoutouBtn.GetComponent<Image> ().enabled = false;
		KyoutouBtn.GetComponent<Button> ().enabled = false;
		KyoutouBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
		
		HakiBtn.GetComponent<Image> ().enabled = false;
		HakiBtn.GetComponent<Button> ().enabled = false;
		HakiBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
		
		KouekiBtn.GetComponent<Image> ().enabled = false;
		KouekiBtn.GetComponent<Button> ().enabled = false;
		KouekiBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;

		SyuppeiBtn.GetComponent<Image> ().enabled = false;
		SyuppeiBtn.GetComponent<Button> ().enabled = false;
		SyuppeiBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;


		
	}


	public void OnGaikouMenu(){
		
		GameObject kamon = GameObject.Find ("KamonBack").gameObject;
		GameObject daimyoName = GameObject.Find ("DaimyoName").gameObject;
		GameObject Heiryoku = GameObject.Find ("Heiryoku").gameObject;
		GameObject Yukoudo = GameObject.Find ("Yukoudo").gameObject;
		
		kamon.GetComponent<Image> ().enabled = true;
		daimyoName.GetComponent<Image> ().enabled = true;
		Heiryoku.GetComponent<Image> ().enabled = true;
		Yukoudo.GetComponent<Image> ().enabled = true;
		
		foreach (Transform n in kamon.transform) {
			n.gameObject.GetComponent<Image>().enabled = true;
			if(n.name == "Doumei"){
				n.FindChild("Text").GetComponent<Text>().enabled = true;
			}
		}
		foreach (Transform n in daimyoName.transform) {
			n.gameObject.GetComponent<Text>().enabled = true;
		}
		foreach (Transform n in Heiryoku.transform) {
			n.gameObject.GetComponent<Text>().enabled = true;
		}
		foreach (Transform n in Yukoudo.transform) {
			n.gameObject.GetComponent<Text>().enabled = true;
		}
		
		GameObject newMenu = GameObject.Find ("return").GetComponent<MenuReturn>().NewMenu;
		GameObject MitsugiBtn = newMenu.transform.FindChild ("Mitsugi").gameObject;
		GameObject KyoutouBtn = newMenu.transform.FindChild ("Kyoutou").gameObject;
		GameObject HakiBtn = newMenu.transform.FindChild ("Haki").gameObject;
		GameObject KouekiBtn = newMenu.transform.FindChild ("Koueki").gameObject;
		GameObject SyuppeiBtn = newMenu.transform.FindChild ("Syuppei").gameObject;
		
		MitsugiBtn.GetComponent<Image> ().enabled = true;
		MitsugiBtn.GetComponent<Button> ().enabled = true;
		MitsugiBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
		
		KyoutouBtn.GetComponent<Image> ().enabled = true;
		KyoutouBtn.GetComponent<Button> ().enabled = true;
		KyoutouBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
		
		HakiBtn.GetComponent<Image> ().enabled = true;
		HakiBtn.GetComponent<Button> ().enabled = true;
		HakiBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
		
		KouekiBtn.GetComponent<Image> ().enabled = true;
		KouekiBtn.GetComponent<Button> ().enabled = true;
		KouekiBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
		
		SyuppeiBtn.GetComponent<Image> ().enabled = true;
		SyuppeiBtn.GetComponent<Button> ().enabled = true;
		SyuppeiBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
		
		
		
	}



}
