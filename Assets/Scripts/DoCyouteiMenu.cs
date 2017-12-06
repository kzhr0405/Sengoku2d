using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class DoCyouteiMenu : MonoBehaviour {
    
	public AudioSource[] audioSources;

	public void OnClick(){
        CyouteiMenu serihuScript = new CyouteiMenu();
        audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		CloseLayer closeLayerScript = GameObject.Find ("CloseSyoukaijyo").GetComponent<CloseLayer> ();
		string rank = closeLayerScript.syoukaijyoRank;
		bool occupiedFlg = closeLayerScript.occupiedFlg; 

		int nowPoint = PlayerPrefs.GetInt ("cyouteiPoint");
		Message msg = new Message ();

		if (name == "DoKenjyoButton") {
			audioSources [3].Play ();

			//Reduce Action Qty 
			reduceActionQty ();

			//Get Money Value & reduce money
			int giveMoney = int.Parse (GameObject.Find ("GiveMoneyValue").GetComponent<Text> ().text);
			int money = PlayerPrefs.GetInt ("money");
			int newMoney = money - giveMoney;
			PlayerPrefs.SetInt ("money", newMoney);
			GameObject.Find ("MoneyValue").GetComponent<Text> ().text = newMoney.ToString ();

			//Add Kouken Value
			float addPoint = 0;
			if (rank == "Jyo") {
				addPoint = giveMoney / 200;
			} else if (rank == "Cyu") {
				addPoint = giveMoney / 300;
			} else if (rank == "Ge") {
				addPoint = giveMoney / 500;
			}

			if (occupiedFlg) {
				addPoint = addPoint * 1.5f;
			}

			int rdmId = UnityEngine.Random.Range (0, 6);
			List<float> rdmValueList = new List<float> (){ 1.0f, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f };
			float rdmValue = rdmValueList [rdmId];
			
			addPoint = addPoint * rdmValue;
			int newPoint = nowPoint + Mathf.CeilToInt (addPoint);
			if (newPoint > 100) {
				newPoint = 100;
			}
			PlayerPrefs.SetInt ("cyouteiPoint", newPoint);
			
			GameObject.Find ("CyouteiValue").GetComponent<Text> ().text = newPoint.ToString () + "%";

			PlayerPrefs.Flush ();

			string pathOfAnim = "Prefabs/EffectAnime/point_up";
			GameObject anim = Instantiate (Resources.Load (pathOfAnim)) as GameObject;
			anim.transform.SetParent (GameObject.Find ("CyouteiPoint").transform);
			anim.transform.localScale = new Vector2 (80, 80);
			anim.transform.localPosition = new Vector2 (40, 30);


			GameObject menu = GameObject.Find ("MenuKenjyo").gameObject;
			menu.transform.Find ("Close").GetComponent<CloseMenu> ().OnClick ();

			serihuScript.mikadoSerihuChanger (msg.getMessage(32));

		} else if (name == "DoCyouteiButton") {

			int addYukoudo = closeLayerScript.yukouAddValue;
			int reducePoint = closeLayerScript.yukouReducePoint;

			if (reducePoint <= nowPoint) {
				audioSources [3].Play ();

				//Reduce Action Qty 
				reduceActionQty ();

				int newPoint = nowPoint - reducePoint;
				PlayerPrefs.SetInt ("cyouteiPoint", newPoint);
				GameObject.Find ("CyouteiValue").GetComponent<Text> ().text = newPoint.ToString () + "%";

				upYukoudo (addYukoudo);
                
				serihuScript.mikadoSerihuChanger (msg.getMessage(33));

			} else {
				audioSources [4].Play ();
				msg.makeMessage (msg.getMessage(34));
			}

			GameObject menu = GameObject.Find ("MenuCyoutei").gameObject;
			menu.transform.Find ("Close").GetComponent<CloseMenu> ().OnClick ();

		} else if (name == "DoTeisenButton") {
			
			int stropBattleRatio = closeLayerScript.stopBattleRatio;
			int reducePoint = closeLayerScript.stopBattleReducePoint;

			if (reducePoint <= nowPoint) {
				
				//Reduce Action Qty 
				reduceActionQty ();

				bool successFlg = teisen (stropBattleRatio);

				if (successFlg) {
					int newPoint = nowPoint - reducePoint;
					PlayerPrefs.SetInt ("cyouteiPoint", newPoint);
					GameObject.Find ("CyouteiValue").GetComponent<Text> ().text = newPoint.ToString () + "%";

				}
				
			} else {
				audioSources [4].Play ();
				msg.makeMessage (msg.getMessage(34));
			}

			GameObject menu = GameObject.Find ("MenuTeisen").gameObject;
			menu.transform.Find ("Close").GetComponent<CloseMenu> ().OnClick ();
			
			
		} else if (name == "DoKanniButton") {

			int reducePoint = closeLayerScript.kanniReducePoint;
			if (reducePoint <= nowPoint) {
				
				//Reduce Action Qty 
				reduceActionQty ();

				int ratio = closeLayerScript.kanniRatio;
				int kanniId = closeLayerScript.kanniId;
				string kanniName = closeLayerScript.kanniName;

				bool successFlg = registerKanni (ratio, kanniId, kanniName);

				if (successFlg) {
					int newPoint = nowPoint - reducePoint;
					PlayerPrefs.SetInt ("cyouteiPoint", newPoint);
					GameObject.Find ("CyouteiValue").GetComponent<Text> ().text = newPoint.ToString () + "%";

					Color enableImageColor = new Color (35f / 255f, 35f / 255f, 35f / 255f, 155f / 255f);
					Color enableTextColor = new Color (125f / 255f, 125f / 255f, 125f / 255f, 255f / 255f);
					GameObject btn = GameObject.Find ("Kanni").gameObject;
					btn.GetComponent<Button> ().enabled = false;
					btn.GetComponent<Image> ().color = enableImageColor;
					btn.transform.Find ("Text").GetComponent<Text> ().color = enableTextColor;					
				}
			} else {
				audioSources [4].Play ();
				msg.makeMessage (msg.getMessage(34));
			}
			
			GameObject menu = GameObject.Find ("MenuKanni").gameObject;
			menu.transform.Find ("Close").GetComponent<CloseMenu> ().OnClick ();


		} else if (name == "DoCyoutekiButton") {
			int reducePoint = closeLayerScript.cyoutekiReducePoint;
			
			if (reducePoint <= nowPoint) {
				audioSources [3].Play ();
				//Reduce Action Qty 
				reduceActionQty ();

				int newPoint = nowPoint - reducePoint;
				PlayerPrefs.SetInt ("cyouteiPoint", newPoint);
				GameObject.Find ("CyouteiValue").GetComponent<Text> ().text = newPoint.ToString () + "%";

				int targetDaimyoId = closeLayerScript.cyoutekiDaimyo;
				string targetDaimyoName = closeLayerScript.cyoutekiDaimyoName;


				//reduce yukoudo
				string seiryoku = PlayerPrefs.GetString ("seiryoku");
				List<string> seiryokuList = new List<string> ();
				char[] delimiterChars = { ',' };
				seiryokuList = new List<string> (seiryoku.Split (delimiterChars));

				//src daimyo kuni list
				List<string> srcDaimyoKuniList = new List<string> ();
				GameObject KuniIconView = GameObject.Find ("KuniIconView").gameObject;

				for (int i = 0; i < seiryokuList.Count; i++) {
					string tempDaimyoId = seiryokuList [i];

					if (tempDaimyoId == targetDaimyoId.ToString ()) {
						int temp = i + 1;
						srcDaimyoKuniList.Add (temp.ToString ());

						//Change Map Valye
						KuniIconView.transform.Find (temp.ToString ()).GetComponent<SendParam> ().myYukouValue = 0;

					}
				}

				//src daimyo open kuni list
				KuniInfo kuni = new KuniInfo ();
				List<int> openKuniList = new List<int> ();
				for (int j = 0; j < srcDaimyoKuniList.Count; j++) {
					openKuniList.AddRange (kuni.getMappingKuni (int.Parse (srcDaimyoKuniList [j])));
				}

				//Target Daimyo (exculde this src daimyo & mydaimyo)
				int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
				List<int> dstDaimyoList = new List<int> ();

				for (int k = 0; k < openKuniList.Count; k++) {
					int temp = openKuniList [k] - 1;
					int tempDaimyoId = int.Parse (seiryokuList [temp]);
					if (tempDaimyoId != targetDaimyoId) {
						if (!dstDaimyoList.Contains (tempDaimyoId)) {
							dstDaimyoList.Add (tempDaimyoId);
						}
					}
				}

				//Reduce Yukoudo
				for (int l = 0; l < dstDaimyoList.Count; l++) {
					int dstDaimyoId = dstDaimyoList [l];
					DownYukouToZeroWithOther (targetDaimyoId, dstDaimyoId, myDaimyo);
				}


                //Hist
                PlayerPrefs.SetBool("questSpecialFlg11", true);
                MainStageController main = new MainStageController();
                main.questExtension();
                PlayerPrefs.SetInt ("cyoutekiDaimyo", targetDaimyoId);
				PlayerPrefs.Flush ();


                string OKtext = "";
                int langId = PlayerPrefs.GetInt("langId");
                if (langId == 2) {
                    OKtext = "Royal court declared " + targetDaimyoName + " is the enemy.\n friendship with surrounded families decreased.";
                } else {
                    OKtext = targetDaimyoName + "討伐の勅令が出されました。\n周辺大名との関係が著しく悪化しますぞ。";
                }
                msg.makeMessage (OKtext);


			} else {
				audioSources [4].Play ();
				msg.makeMessage (msg.getMessage(34));
			}

			GameObject menu = GameObject.Find ("MenuCyouteki").gameObject;
			menu.transform.Find ("Close").GetComponent<CloseMenu> ().OnClick ();
		
		} else if (name == "DoBakuhuButton") {
			int reducePoint = 100;

			if (reducePoint <= nowPoint) {
				audioSources [3].Play ();
				//Reduce Action Qty 
				reduceActionQty ();

				int newPoint = nowPoint - reducePoint;
				PlayerPrefs.SetInt ("cyouteiPoint", newPoint);
				GameObject.Find ("CyouteiValue").GetComponent<Text> ().text = newPoint.ToString () + "%";

				//Syogun
				string year = GameObject.Find("YearValue").GetComponent<Text>().text;
				string season = GameObject.Find("SeasonValue").GetComponent<Text>().text;
				string daimyoBusyoName = GameObject.Find("DaimyoValue").GetComponent<Text>().text;
                string text = "";
                int langId = PlayerPrefs.GetInt("langId");
                if (langId == 2) {
                    text = year + " " + season + "," + daimyoBusyoName + " was assigned syogun.\n You opened shogunate.";
                }else {
                    text = year + "年" + season + "," + daimyoBusyoName + "は征夷大将軍に任じられました。\n幕府を開き、天下に号令をかけます。";
                }
                msg.makeMessageWithImage(text);

				//Change Value
				int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
				PlayerPrefs.SetInt("syogunDaimyoId",myDaimyo);

				//Up yukoudo
				int rdmAddYukoudo = UnityEngine.Random.Range(20,50);
				upYukoudoWithEveryDaimyo(rdmAddYukoudo);
				PlayerPrefs.Flush ();

				//Enable Syogun Button
				GameObject oya = GameObject.Find("SubButtonViewRight").gameObject;
				GameObject ko = oya.transform.Find ("Bakuhu").gameObject;
				ko.SetActive (true);

				string pathOfAnim = "Prefabs/EffectAnime/point_up";
				GameObject anim = Instantiate (Resources.Load (pathOfAnim)) as GameObject;
				anim.transform.SetParent (ko.transform);
				anim.transform.localScale = new Vector2 (200, 200);
				anim.transform.localPosition = new Vector2 (0, 100);

                PlayerPrefs.SetBool("questSpecialFlg10", true);
                MainStageController main = new MainStageController();
                main.questExtension();
                PlayerPrefs.Flush();
                
            } else {
				audioSources [4].Play ();
				msg.makeMessage (msg.getMessage(34));
			}

			GameObject menu = GameObject.Find ("MenuBakuhu").gameObject;
			menu.transform.Find ("Close").GetComponent<CloseMenu> ().OnClick ();
		}
	}

	public void reduceActionQty(){
		bool actionOKFlg = false;
		GameObject acrionValue = GameObject.Find ("ActionValue").gameObject;
		int actionRemainQty = int.Parse(acrionValue.GetComponent<Text> ().text);

		actionRemainQty = actionRemainQty - 1;
		acrionValue.GetComponent<Text>().text = actionRemainQty.ToString();

		//Track
		int TrackCyouteiNo = PlayerPrefs.GetInt("TrackCyouteiNo",0);
		TrackCyouteiNo = TrackCyouteiNo + 1;
		PlayerPrefs.SetInt ("TrackCyouteiNo", TrackCyouteiNo);
	}

	public void upYukoudo(int addYukoudo){
		//Daimyo Id
		int myDaimyoId = PlayerPrefs.GetInt("myDaimyo");
		
		//Seiryoku List
		string seiryoku = PlayerPrefs.GetString ("seiryoku");
		List<string> seiryokuList = new List<string> ();
		char[] delimiterChars = {','};
		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
		
		//src daimyo kuni list
		List<string> srcDaimyoKuniList = new List<string> ();
		for(int i=0; i<seiryokuList.Count;i++){
			string tempDaimyoId = seiryokuList[i];
			
			if(tempDaimyoId == myDaimyoId.ToString()){
				int temp = i + 1;
				srcDaimyoKuniList.Add(temp.ToString());
			}
		}
		
		//src daimyo open kuni list
		KuniInfo kuni = new KuniInfo();
		List<int> openKuniList = new List<int>();
		for(int j=0; j<srcDaimyoKuniList.Count; j++){
			openKuniList.AddRange(kuni.getMappingKuni(int.Parse(srcDaimyoKuniList[j])));
		}
		
		//Target Daimyo (exculde this src daimyo & mydaimyo)
		List<int> dstDaimyoList = new List<int>();
		
		for(int k=0; k<openKuniList.Count;k++){
			int temp = openKuniList[k] - 1;
			int tempDaimyoId = int.Parse(seiryokuList[temp]);
			if(tempDaimyoId != myDaimyoId){
				if(!dstDaimyoList.Contains(tempDaimyoId)){
					dstDaimyoList.Add(tempDaimyoId);
				}
			}
		}
		
		
		//Add Yukoudo
		MainEventHandler main = new MainEventHandler();
		Daimyo daimyo = new Daimyo();
		string cyouteiText = "";
        int langId = PlayerPrefs.GetInt("langId");
        int senarioId = PlayerPrefs.GetInt("senarioId");

        for (int l=0; l<dstDaimyoList.Count;l++){
			int dstDaimyoId = dstDaimyoList[l];
			string dstDaimyoName = daimyo.getName(dstDaimyoId,langId,senarioId);

			string tempGaikou = "gaikou" + dstDaimyoId;
			int nowYukoudo = 0;
			if (PlayerPrefs.HasKey (tempGaikou)) {
				nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
			} else {
				nowYukoudo = 50;
			}
			int newYukoudo = nowYukoudo + addYukoudo;
			if (newYukoudo > 100) {
				newYukoudo = 100;
			}
			PlayerPrefs.SetInt (tempGaikou, newYukoudo);
            if (langId == 2) {
                cyouteiText = cyouteiText + "Friendship with " + dstDaimyoName + " increased " + addYukoudo + " point\n";
            }else {
                cyouteiText = cyouteiText + dstDaimyoName + "との友好度が" + addYukoudo + "上がりました。\n";
            }
        }

		PlayerPrefs.Flush();

		//Message
		Message msg = new Message ();
        string OKtext = "";
        if (langId == 2) {
            OKtext = "Friendship with surrounded families increased.\n " + cyouteiText;
        }else {
            OKtext = "周辺大名との友好度が上がります。\n " + cyouteiText;
        }
		msg.makeMessage (OKtext);


	}

	public bool teisen(int stropBattleRatio){
        CyouteiMenu serihuScript = new CyouteiMenu();
        float ratio = (float)stropBattleRatio;
		float percent = Random.value;
		percent = percent * 100;
		bool successFlg = false;

		//Gunzei
		GameObject gunzeiObj = GameObject.Find ("Teisen").GetComponent<CyouteiMenu> ().targetGunzei;
		Message msg = new Message ();

		if(percent <= ratio){
			//OK
			audioSources [3].Play ();

			successFlg = true;

			//Delete Key
			string gunzeiKey = gunzeiObj.name;
			PlayerPrefs.DeleteKey(gunzeiKey);
			
			//Delete Key History
			char[] delimiterChars = {','};
			string keyHistory = PlayerPrefs.GetString ("keyHistory");
			List<string> keyHistoryList = new List<string>();
			if (keyHistory != null && keyHistory != "") {
				if(keyHistory.Contains(",")){
					keyHistoryList = new List<string> (keyHistory.Split (delimiterChars));
				}else{
					keyHistoryList.Add(keyHistory);
				}
			}
			keyHistoryList.Remove(gunzeiKey);
			string newKeyHistory = "";
			for(int i=0; i<keyHistoryList.Count; i++){
				if(i==0){
					newKeyHistory = keyHistoryList[i];
				}else{
					newKeyHistory = newKeyHistory + "," + keyHistoryList[i];
				}
			}
			PlayerPrefs.SetString("keyHistory",newKeyHistory);
			
			//Message
			string daimyoName = gunzeiObj.GetComponent<Gunzei>().srcDaimyoName;
            string OKtext = "";
            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                OKtext = "Royal Court successfully requested a ceasefire.\n " + daimyoName + "'s army was withdrawn.";
            }else {
                OKtext = "朝廷が停戦要請に成功しました。\n " + daimyoName + "の軍勢が退却します。";
            }
			msg.makeMessage (OKtext);

			//Delete Gunzei
			Destroy(gunzeiObj);

			PlayerPrefs.Flush();

			serihuScript.mikadoSerihuChanger(msg.getMessage(35));
		}else{
			//NG
			audioSources [4].Play ();

			//Message
			msg.makeMessage (msg.getMessage(36));
            serihuScript.mikadoSerihuChanger(msg.getMessage(37));
		}

		return successFlg;
	}

	public void upYukoudoWithEveryDaimyo(int addYukoudo){
		//Daimyo Id
		int myDaimyoId = PlayerPrefs.GetInt("myDaimyo");

		//Seiryoku List
		string seiryoku = PlayerPrefs.GetString ("seiryoku");
		List<string> seiryokuList = new List<string> ();
		char[] delimiterChars = {','};
		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));

		//src daimyo kuni list
		List<int> targetDaimyoList = new List<int>();

		for(int i=0; i<seiryokuList.Count;i++){
			int tempDaimyoId = int.Parse(seiryokuList[i]);

			if (!targetDaimyoList.Contains (tempDaimyoId)) {
				if (tempDaimyoId != myDaimyoId) {
					targetDaimyoList.Add (tempDaimyoId);

				}
			}
		}


		//Add Yukoudo
		Daimyo daimyo = new Daimyo();
		string cyouteiText = "";
		for(int l=0; l<targetDaimyoList.Count;l++){
			int dstDaimyoId = targetDaimyoList[l];

			string tempGaikou = "gaikou" + dstDaimyoId;
			int nowYukoudo = 0;
			if (PlayerPrefs.HasKey (tempGaikou)) {
				nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
			} else {
				nowYukoudo = 50;
			}
			int newYukoudo = nowYukoudo + addYukoudo;
			if (newYukoudo > 100) {
				newYukoudo = 100;
			}
			PlayerPrefs.SetInt (tempGaikou, newYukoudo);
		}
		PlayerPrefs.Flush();

	}



	public bool registerKanni(int kanniRatio, int kanniId, string kanniName){
        CyouteiMenu serihuScript = new CyouteiMenu();
        float ratio = (float)kanniRatio;
		float percent = Random.value;
		percent = percent * 100;
		bool successFlg = false;
		
		//Kanni
		Message msg = new Message ();
		
		if(percent <= ratio){
			//OK
			successFlg = true;
			audioSources [3].Play ();
			string myKanni = PlayerPrefs.GetString ("myKanni");
			if(myKanni != null && myKanni !=""){
				myKanni = myKanni + "," + kanniId.ToString();
			}else{
				myKanni = kanniId.ToString();
			}
			PlayerPrefs.SetString ("myKanni",myKanni);

            //Message
            string OKtext = "";
            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                OKtext = "Congratulations.\n" + "My lord was assigned " + kanniName + ".";
            }else {
                OKtext = "祝着至極に存じます。\n" + kanniName + "が叙位されましたぞ。";
            }
			msg.makeMessage (OKtext);
			PlayerPrefs.Flush();

			serihuScript.mikadoSerihuChanger(msg.getMessage(38));
			
		}else{
			//NG
			audioSources [4].Play ();
			//Message
			msg.makeMessage (msg.getMessage(39));

			serihuScript.mikadoSerihuChanger(msg.getMessage(40));
		}

		return successFlg;
	}


	public void DownYukouToZeroWithOther(int srcDaimyoId, int dstDaimyoId, int myDaimyoId){

		string temp = "";

		if (dstDaimyoId == myDaimyoId) {
			temp = "gaikou" + srcDaimyoId.ToString ();
		} else {
			if (srcDaimyoId < dstDaimyoId) {
				temp = srcDaimyoId.ToString () + "gaikou" + dstDaimyoId.ToString ();
			} else {
				temp = dstDaimyoId.ToString () + "gaikou" + srcDaimyoId.ToString ();
			}
		}

		int value = 0;
		PlayerPrefs.SetInt (temp, value);
		PlayerPrefs.Flush ();
	}


}
