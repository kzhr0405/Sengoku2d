using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System.Collections.Generic;

public class GaikouMenu : MonoBehaviour {

	public int kuniDiff = 1;
	public int daimyoBusyoAtk = 0;
	public int daimyoBusyoDfc = 0;
	//Sound
	public AudioSource sound;

	public void OnClick () {

		//SE
		sound = GameObject.Find ("SEController").GetComponent<AudioSource> ();
		sound.PlayOneShot (sound.clip); 


		CloseBoard close = GameObject.Find ("close").GetComponent<CloseBoard> ();
		daimyoBusyoAtk = close.daimyoBusyoAtk;
		daimyoBusyoDfc = close.daimyoBusyoDfc;

		int nowHyourou = PlayerPrefs.GetInt ("hyourou");
		bool hyourouOKflg = false;

		//Kuni Qty Difference
		int myKuniQty = GameObject.Find ("GameController").GetComponent<MainStageController> ().myKuniQty;
		int tKuniQty = close.kuniQty;
		if (myKuniQty > tKuniQty) {
			kuniDiff = myKuniQty - tKuniQty;
		}

		Message msg = new Message ();


		if (name == "Mitsugi") {

			bool isExistFlg = isExistActiveBusyo ();
			if (isExistFlg) {
				close.layer = close.layer + 1;

                //Menu Handling
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    GameObject.Find ("kuniName").GetComponent<Text> ().text = "Gift";
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
                if (Application.systemLanguage != SystemLanguage.Japanese) {
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
				hyourouOKflg = HyourouCheck (nowHyourou);
				btn.GetComponent<DoGaikou> ().hyourouOKflg = hyourouOKflg;

				//Busyo Scroll View
				ScrollView (mitsugiObj, btn);

				//Set Obj
				GameObject.Find ("return").GetComponent<MenuReturn> ().layer2 = mitsugiObj;
			
			} else {
				msg.makeMessage (msg.getMessage(8));
			}

		} else if (name == "Doumei") {

			bool isExistFlg = isExistActiveBusyo ();
			if (isExistFlg) {
				close.layer = close.layer + 1;

				//Menu Handling
				if (Application.systemLanguage != SystemLanguage.Japanese) {
                    GameObject.Find("kuniName").GetComponent<Text>().text = "Alliance";
                }else {
                    GameObject.Find("kuniName").GetComponent<Text>().text = "同盟";
                }
                OffGaikouMenu ();

				//Mitsugi Object
				string doumeiPath = "Prefabs/Map/gaikou/DoumeiObj";
				GameObject doumeiObj = Instantiate (Resources.Load (doumeiPath)) as GameObject;
				doumeiObj.transform.SetParent (GameObject.Find ("smallBoard(Clone)").transform);
				doumeiObj.transform.localScale = new Vector3 (1, 1, 1);
				
				//Do Button
				string buttonPath = "Prefabs/Map/gaikou/DoGaikouBtn";
				GameObject btn = Instantiate (Resources.Load (buttonPath)) as GameObject;
				btn.transform.SetParent (doumeiObj.transform);
				btn.transform.localScale = new Vector3 (1, 1, 1);
				btn.name = "DoDoumeiBtn";
				RectTransform btn_transform = btn.GetComponent<RectTransform> ();
				btn_transform.anchoredPosition = new Vector3 (0, -200, 0);
				
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    btn.transform.FindChild("Text").GetComponent<Text>().text = "Alliance";
                }else {
                    btn.transform.FindChild("Text").GetComponent<Text>().text = "同盟";
                }

                //Money
                string moneyPath = "Prefabs/Map/common/RequiredMoney";
				GameObject money = Instantiate (Resources.Load (moneyPath)) as GameObject;
				money.transform.SetParent (doumeiObj.transform);
				money.transform.localScale = new Vector3 (0.8f, 1, 1);
				RectTransform money_transform = money.GetComponent<RectTransform> ();
				money_transform.anchoredPosition = new Vector3 (-180, -90, 0);
				int nowMoney = PlayerPrefs.GetInt ("money");
				if (nowMoney >= 3000) {
					btn.GetComponent<DoGaikou> ().moneyOKflg = true;
				}
				btn.GetComponent<DoGaikou> ().paiedMoney = 3000;

				//Hyourou Check
				hyourouOKflg = HyourouCheck (nowHyourou);
				btn.GetComponent<DoGaikou> ().hyourouOKflg = hyourouOKflg;

				//Ratio
				string ratioPath = "Prefabs/Map/common/DoumeiRatio";
				GameObject ratio = Instantiate (Resources.Load (ratioPath)) as GameObject;
				ratio.transform.SetParent (doumeiObj.transform);
				ratio.transform.localScale = new Vector3 (0.8f, 1, 1);
				ratio.name = "DoumeiRatio";
				RectTransform ratio_transform = ratio.GetComponent<RectTransform> ();
				ratio_transform.anchoredPosition = new Vector3 (-20, -90, 0);

				//Busyo Scroll View
				ScrollView (doumeiObj, btn);

				//Set Obj
				GameObject.Find ("return").GetComponent<MenuReturn> ().layer2 = doumeiObj;

			} else {
				msg.makeMessage (msg.getMessage(8));
			}

			
		} else if (name == "Doukatsu") {

			bool isExistFlg = isExistActiveBusyo ();
			if (isExistFlg) {
				close.layer = close.layer + 1;
                
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    GameObject.Find("kuniName").GetComponent<Text>().text = "Threat";
                }else {
                    GameObject.Find("kuniName").GetComponent<Text>().text = "恫喝";
                }
                OffGaikouMenuList ();
				
				string doukatsuPath = "Prefabs/Map/gaikou/DoumeiObj";
				GameObject gaikouObj = Instantiate (Resources.Load (doukatsuPath)) as GameObject;
				gaikouObj.transform.SetParent (GameObject.Find ("smallBoard(Clone)").transform);
				gaikouObj.transform.localScale = new Vector3 (1, 1, 1);

				//Do Button
				string buttonPath = "Prefabs/Map/gaikou/DoGaikouBtn";
				GameObject btn = Instantiate (Resources.Load (buttonPath)) as GameObject;
				btn.transform.SetParent (gaikouObj.transform);
				btn.transform.localScale = new Vector3 (1, 1, 1);
				btn.name = "DoDoukatsuBtn";
				RectTransform btn_transform = btn.GetComponent<RectTransform> ();
				btn_transform.anchoredPosition = new Vector3 (0, -200, 0);
				
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    btn.transform.FindChild("Text").GetComponent<Text>().text = "Threat";
                }
                else {
                    btn.transform.FindChild("Text").GetComponent<Text>().text = "恫喝";
                }
                //Hyourou Check
                hyourouOKflg = HyourouCheck (nowHyourou);
				btn.GetComponent<DoGaikou> ().hyourouOKflg = hyourouOKflg;


				ScrollView (gaikouObj, btn);

				//Scroll Adjustment
				GameObject scroll = gaikouObj.transform.FindChild ("scroll").gameObject;
				scroll.transform.localScale = new Vector3 (0.65f, 0.65f, 1);
				RectTransform scrollTransform = scroll.GetComponent<RectTransform> ();
				scrollTransform.anchoredPosition = new Vector3 (0, -15, 0);
				scrollTransform.sizeDelta = new Vector2 (820, 230);

				GameObject.Find ("return").GetComponent<MenuReturn> ().layer2 = gaikouObj;
			
			} else {
				msg.makeMessage (msg.getMessage(8));
			}

		} else if (name == "Koueki") {
			
			int yukoudo = close.yukoudo;
			string myDaimyoName = GameObject.Find("DaimyoValue").GetComponent<Text>().text;

			if(yukoudo>=20){
                close.layer = close.layer + 1;

                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    GameObject.Find("kuniName").GetComponent<Text>().text = "Trade";
                }
                else {
                    GameObject.Find("kuniName").GetComponent<Text>().text = "交易";
                }
                OffGaikouMenu ();
				
				string kouekiPath = "Prefabs/Map/gaikou/kouekiObj";
				GameObject kouekiObj = Instantiate (Resources.Load (kouekiPath)) as GameObject;
				kouekiObj.transform.SetParent (GameObject.Find ("smallBoard(Clone)").transform);
				kouekiObj.transform.localScale = new Vector3 (1, 1, 1);
				kouekiObj.transform.FindChild("Buy").GetComponent<KouekiMenu>().kouekiObj = kouekiObj;
				kouekiObj.transform.FindChild("Change").GetComponent<KouekiMenu>().kouekiObj = kouekiObj;

				kouekiObj.transform.FindChild("Buy").GetComponent<KouekiMenu>().clickBuy();
				GameObject.Find ("return").GetComponent<MenuReturn> ().layer2 = kouekiObj;

				//Daimyo Change
				GameObject daimyo = kouekiObj.transform.FindChild("Daimyo").gameObject;
				foreach(Transform obj in daimyo.transform){
					Destroy(obj);
				}

                //SerihuChange
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    if (20<=yukoudo && yukoudo<30)GameObject.Find("SerihuText").GetComponent<Text>().text = "...What are you doing here?";
				    if(30<=yukoudo && yukoudo<40)GameObject.Find("SerihuText").GetComponent<Text>().text = "Hi. What do you want?";
				    if(40<=yukoudo && yukoudo<50)GameObject.Find("SerihuText").GetComponent<Text>().text = "Long time no see. We can trade depends on the condition.";
				    if(50<=yukoudo && yukoudo<60)GameObject.Find("SerihuText").GetComponent<Text>().text = "Long time no see. Please see our traiding goods.";
				    if(60<=yukoudo && yukoudo<70)GameObject.Find("SerihuText").GetComponent<Text>().text = "Lord " + myDaimyoName + ", How about this good? It's a rare isn't it?";
				    if(70<=yukoudo && yukoudo<80)GameObject.Find("SerihuText").GetComponent<Text>().text = "Lord " + myDaimyoName + ", I heard your success with rumors. Well, let's trade.";
				    if(80<=yukoudo && yukoudo<90)GameObject.Find("SerihuText").GetComponent<Text>().text = "Lord " + myDaimyoName + ", I just wanted to talk to you. Please enjoy our country.";
				    if(90<=yukoudo)GameObject.Find("SerihuText").GetComponent<Text>().text = "Oh, Lord " + myDaimyoName + "\nLet's make good trade!";
                }else {
                    if (20 <= yukoudo && yukoudo < 30) GameObject.Find("SerihuText").GetComponent<Text>().text = "・・・何用で参ったのかな。";
                    if (30 <= yukoudo && yukoudo < 40) GameObject.Find("SerihuText").GetComponent<Text>().text = "さて、今日は何用ですかな。";
                    if (40 <= yukoudo && yukoudo < 50) GameObject.Find("SerihuText").GetComponent<Text>().text = "お久しゅうござるな。条件次第で交易致しますぞ。";
                    if (50 <= yukoudo && yukoudo < 60) GameObject.Find("SerihuText").GetComponent<Text>().text = "ご無沙汰しておりましたな。さ、交易品を見て行って下され。";
                    if (60 <= yukoudo && yukoudo < 70) GameObject.Find("SerihuText").GetComponent<Text>().text = myDaimyoName + "殿、この品は如何かな。珍しいものでござろう。";
                    if (70 <= yukoudo && yukoudo < 80) GameObject.Find("SerihuText").GetComponent<Text>().text = myDaimyoName + "殿、ご活躍は噂で聞いておりますぞ。さ、交易致しましょう。";
                    if (80 <= yukoudo && yukoudo < 90) GameObject.Find("SerihuText").GetComponent<Text>().text = myDaimyoName + "殿、丁度話がしたいと思うておったところよ。ゆるりとして行って下され。";
                    if (90 <= yukoudo) GameObject.Find("SerihuText").GetComponent<Text>().text = "おお、" + myDaimyoName + "殿。\n良き交易にしましょうぞ。";
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




			}else{
				msg.makeMessage(msg.getMessage(9));
			}
		}


	}





	public bool HyourouCheck(int nowHyourou){
		bool hyourouOKFlg = false;

		if (nowHyourou >= 5) {
			hyourouOKFlg = true;
		}
		return hyourouOKFlg;
	}


	public void ScrollView(GameObject obj, GameObject btnObj){

		string scrollPath = "Prefabs/Map/common/ScrollView";
		GameObject scroll = Instantiate (Resources.Load (scrollPath)) as GameObject;
		scroll.transform.SetParent(obj.transform);
		scroll.transform.localScale = new Vector3 (1, 1, 1);
		RectTransform scroll_transform = scroll.GetComponent<RectTransform>();
		scroll_transform.anchoredPosition = new Vector3(0,130,0);
		scroll.name = "scroll";

		List<string> myBusyoList = new List<string>();
		string myBusyoString = PlayerPrefs.GetString ("myBusyo");
		char[] delimiterChars = {','};
		myBusyoList = new List<string>(myBusyoString.Split (delimiterChars));

		//reduce used busyo
		List<string> usedBusyoList = new List<string>();
		string usedBusyo = PlayerPrefs.GetString ("usedBusyo");
		if (usedBusyo != null && usedBusyo != "") {
			usedBusyoList = new List<string> (usedBusyo.Split (delimiterChars));
			myBusyoList.RemoveAll (usedBusyo.Contains);
		}

		string slotPath = "Prefabs/Map/common/Slot";
		string chiryakuPath = "Prefabs/Map/common/Chiryaku";
		GameObject contents = scroll.transform.FindChild("Content").gameObject;

		for (int i=0; i<myBusyoList.Count; i++) {

			string myBusyoId = myBusyoList[i];

			//Slot
			GameObject prefab = Instantiate (Resources.Load (slotPath)) as GameObject;
			prefab.transform.SetParent(contents.transform);
			prefab.transform.localScale = new Vector3 (1, 1, 1);
			prefab.GetComponent<GaikouBusyoSelect>().DoBtn = btnObj;
			prefab.GetComponent<GaikouBusyoSelect>().Content = contents;
			prefab.GetComponent<GaikouBusyoSelect>().kuniDiff = kuniDiff;
			prefab.GetComponent<GaikouBusyoSelect>().daimyoBusyoAtk = daimyoBusyoAtk;
			prefab.GetComponent<GaikouBusyoSelect>().daimyoBusyoDfc = daimyoBusyoDfc;
			prefab.GetComponent<GaikouBusyoSelect>().busyoId = int.Parse(myBusyoId);

			//Busyo
			string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
			GameObject busyo = Instantiate (Resources.Load (busyoPath)) as GameObject;
			busyo.name = myBusyoId;
			busyo.transform.SetParent(prefab.transform);
			busyo.transform.localScale = new Vector3 (4, 5, 4);
			busyo.name = myBusyoList [i].ToString ();

			RectTransform text_transform = busyo.transform.FindChild("Text").GetComponent<RectTransform>();
			text_transform.anchoredPosition = new Vector3(-32,12,0);
			prefab.name = "Slot" + busyo.name;

			//Get Chiryaku
			StatusGet sts = new StatusGet();
			int lv = PlayerPrefs.GetInt (myBusyoId);
			float chiryakuSts = (float)sts.getDfc(int.Parse(myBusyoId),lv);
			chiryakuSts = chiryakuSts *10;

			GameObject chiryaku = Instantiate (Resources.Load (chiryakuPath)) as GameObject;
			chiryaku.transform.SetParent(busyo.transform);
			chiryaku.transform.FindChild("value").GetComponent<Text>().text = chiryakuSts.ToString();
			chiryaku.transform.localScale = new Vector3 (1, 1, 1);
            chiryaku.transform.localPosition = new Vector3(0, 0, 0);

            busyo.GetComponent<DragHandler> ().enabled = false;

			//Deffault Busyo Click
			if(i == 0){
				prefab.GetComponent<GaikouBusyoSelect>().OnClick();
			}

		}

	}

	public void OffGaikouMenuList(){
		
		GameObject newMenu = GameObject.Find ("return").GetComponent<MenuReturn>().NewMenu;
		GameObject MitsugiBtn = newMenu.transform.FindChild ("Mitsugi").gameObject;
		GameObject DoumeiBtn = newMenu.transform.FindChild ("Doumei").gameObject;
		GameObject KouekiBtn = newMenu.transform.FindChild ("Koueki").gameObject;
		GameObject DoukatsuBtn = newMenu.transform.FindChild ("Doukatsu").gameObject;
		
		MitsugiBtn.GetComponent<Image> ().enabled = false;
		MitsugiBtn.GetComponent<Button> ().enabled = false;
		MitsugiBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
		
		DoumeiBtn.GetComponent<Image> ().enabled = false;
		DoumeiBtn.GetComponent<Button> ().enabled = false;
		DoumeiBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
		
		KouekiBtn.GetComponent<Image> ().enabled = false;
		KouekiBtn.GetComponent<Button> ().enabled = false;
		KouekiBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
		
		DoukatsuBtn.GetComponent<Image> ().enabled = false;
		DoukatsuBtn.GetComponent<Button> ().enabled = false;
		DoukatsuBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
		
		
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
		GameObject DoumeiBtn = newMenu.transform.FindChild ("Doumei").gameObject;
		GameObject KouekiBtn = newMenu.transform.FindChild ("Koueki").gameObject;
		GameObject DoukatsuBtn = newMenu.transform.FindChild ("Doukatsu").gameObject;
		
		MitsugiBtn.GetComponent<Image> ().enabled = false;
		MitsugiBtn.GetComponent<Button> ().enabled = false;
		MitsugiBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;

		DoumeiBtn.GetComponent<Image> ().enabled = false;
		DoumeiBtn.GetComponent<Button> ().enabled = false;
		DoumeiBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
		
		KouekiBtn.GetComponent<Image> ().enabled = false;
		KouekiBtn.GetComponent<Button> ().enabled = false;
		KouekiBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
		
		DoukatsuBtn.GetComponent<Image> ().enabled = false;
		DoukatsuBtn.GetComponent<Button> ().enabled = false;
		DoukatsuBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
	

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
		GameObject DoumeiBtn = newMenu.transform.FindChild ("Doumei").gameObject;
		GameObject KouekiBtn = newMenu.transform.FindChild ("Koueki").gameObject;
		GameObject DoukatsuBtn = newMenu.transform.FindChild ("Doukatsu").gameObject;
		
		MitsugiBtn.GetComponent<Image> ().enabled = true;
		MitsugiBtn.GetComponent<Button> ().enabled = true;
		MitsugiBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
		
		DoumeiBtn.GetComponent<Image> ().enabled = true;
		DoumeiBtn.GetComponent<Button> ().enabled = true;
		DoumeiBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
		
		KouekiBtn.GetComponent<Image> ().enabled = true;
		KouekiBtn.GetComponent<Button> ().enabled = true;
		KouekiBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
		
		DoukatsuBtn.GetComponent<Image> ().enabled = true;
		DoukatsuBtn.GetComponent<Button> ().enabled = true;
		DoukatsuBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
		
	}

	public bool isExistActiveBusyo(){

		bool isExistFlg = false;

		char[] delimiterChars = { ',' };
		List<string> myBusyoList = new List<string> ();
		string myBusyoString = PlayerPrefs.GetString ("myBusyo");
		if (myBusyoString.Contains (",")) {
			myBusyoList = new List<string> (myBusyoString.Split (delimiterChars));
		} else {
			myBusyoList.Add (myBusyoString);
		}

		//reduce used busyo
		List<string> usedBusyoList = new List<string>();
		string usedBusyo = PlayerPrefs.GetString ("usedBusyo");
		if (usedBusyo != null && usedBusyo != "") {
			usedBusyoList = new List<string> (usedBusyo.Split (delimiterChars));
			myBusyoList.RemoveAll (usedBusyo.Contains);
		}

		if (myBusyoList.Count > 0) {
			isExistFlg = true;
		}

		return isExistFlg;
	}

}
