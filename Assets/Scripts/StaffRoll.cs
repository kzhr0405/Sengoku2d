using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class StaffRoll : MonoBehaviour {

	public GameObject backObj;
	public GameObject popObj;
	public GameObject particleObj;
	public GameObject panel;
	public string kahouCd = "";
	public string kahouId = "";

	public void OnClick(){
		/*Receive Item*/
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [3].Play ();

		//Item1
		string gameClearDaimyo = PlayerPrefs.GetString ("gameClearDaimyo");
        bool hardFlg = PlayerPrefs.GetBool("hardFlg");
        List<string> gameClearDaimyoList = new List<string> ();
		char[] delimiterChars = {','};
		if (gameClearDaimyo != null && gameClearDaimyo != "") {
			if (gameClearDaimyo.Contains (",")) {
				gameClearDaimyoList = new List<string> (gameClearDaimyo.Split (delimiterChars));
			}else{
				gameClearDaimyoList.Add(gameClearDaimyo);
			}
		}
		int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
		if (!gameClearDaimyoList.Contains (myDaimyo.ToString ())) {
			if (gameClearDaimyo != null && gameClearDaimyo != "") {
				gameClearDaimyo = gameClearDaimyo + "," + myDaimyo.ToString ();
			} else {
				gameClearDaimyo = myDaimyo.ToString ();
			}
			PlayerPrefs.SetString ("gameClearDaimyo", gameClearDaimyo);           
        }

        //Hard mode
        if (hardFlg) {
            string gameClearDaimyoHard = PlayerPrefs.GetString("gameClearDaimyoHard");
            List<string> gameClearDaimyoHardList = new List<string>();
            if (gameClearDaimyoHard != null && gameClearDaimyoHard != "") {
                if (gameClearDaimyoHard.Contains(",")) {
                    gameClearDaimyoHardList = new List<string>(gameClearDaimyoHard.Split(delimiterChars));
                }
                else {
                    gameClearDaimyoHardList.Add(gameClearDaimyoHard);
                }
            }
            if (!gameClearDaimyoHardList.Contains(myDaimyo.ToString())) {
                if (gameClearDaimyoHard != null && gameClearDaimyoHard != "") {
                    gameClearDaimyoHard = gameClearDaimyoHard + "," + myDaimyo.ToString();
                }
                else {
                    gameClearDaimyoHard = myDaimyo.ToString();
                }
                PlayerPrefs.SetString("gameClearDaimyoHard", gameClearDaimyoHard);
            }
        }
        

        PlayerPrefs.SetBool ("gameClearItemGetFlg",true);


		//Item2
		PlayerPrefs.DeleteKey ("gameClearKahouCd");
		PlayerPrefs.DeleteKey ("gameClearKahouId");
		Kahou kahou = new Kahou ();
		if (kahouCd == "bugu") {
			kahou.registerBugu (int.Parse(kahouId));
		} else if (kahouCd == "gusoku") {
			kahou.registerGusoku (int.Parse(kahouId));
		} else if (kahouCd == "kabuto") {
			kahou.registerKabuto (int.Parse(kahouId));
		} else if (kahouCd == "meiba") {
			kahou.registerMeiba (int.Parse(kahouId));
		} else if (kahouCd == "cyadougu") {
			kahou.registerCyadougu (int.Parse(kahouId));
		} else if (kahouCd == "chishikisyo") {
			kahou.registerChishikisyo (int.Parse(kahouId));
		}else if (kahouCd == "heihousyo") {
            kahou.registerHeihousyo(int.Parse(kahouId));
        }

        //Item3
        int busyoDama = PlayerPrefs.GetInt ("busyoDama");
		int newBusyoDama = busyoDama + 1000;
		PlayerPrefs.SetInt ("busyoDama", newBusyoDama);
		


        //Get Daimyo Busyo
        Daimyo daimyoScript = new Daimyo();
        int daimyoId = PlayerPrefs.GetInt("myDaimyo");
        int daimyoBusyoId = daimyoScript.getDaimyoBusyoId(daimyoId);

        string zukanBusyoHst = PlayerPrefs.GetString("zukanBusyoHst");
        List<string> zukanBusyoHstList = new List<string>();
        if (zukanBusyoHst != null && zukanBusyoHst != "") {
            if (zukanBusyoHst.Contains(",")) {
                zukanBusyoHstList = new List<string>(zukanBusyoHst.Split(delimiterChars));
            }else {
                zukanBusyoHstList.Add(zukanBusyoHst);
            }
        }
        if(!zukanBusyoHstList.Contains(daimyoBusyoId.ToString())) {
            if (zukanBusyoHst != null && zukanBusyoHst != "") {
                zukanBusyoHst = zukanBusyoHst + "," + daimyoBusyoId.ToString();
            }else {
                zukanBusyoHst = daimyoBusyoId.ToString();
            }
            PlayerPrefs.SetString("zukanBusyoHst", zukanBusyoHst);
        }

        if (daimyoScript.daimyoBusyoCheck(daimyoBusyoId)) {
            string gacyaDaimyoHst = PlayerPrefs.GetString("gacyaDaimyoHst");
            List<string> gacyaDaimyoHstList = new List<string>();
            if (gacyaDaimyoHst != null && gacyaDaimyoHst != "") {
                if (gacyaDaimyoHst.Contains(",")) {
                    gacyaDaimyoHstList = new List<string>(gacyaDaimyoHst.Split(delimiterChars));
                }else {
                    gacyaDaimyoHstList.Add(gacyaDaimyoHst);
                }
            }
            if(!gacyaDaimyoHstList.Contains(daimyoBusyoId.ToString())) {
                if (gacyaDaimyoHst != null && gacyaDaimyoHst != "") {
                    gacyaDaimyoHst = gacyaDaimyoHst + "," + daimyoBusyoId.ToString();
                }
                else {
                    gacyaDaimyoHst = daimyoBusyoId.ToString();
                }
                PlayerPrefs.SetString("gacyaDaimyoHst", gacyaDaimyoHst);
            }
        }

        PlayerPrefs.Flush();



        //Hide Back
        Destroy (backObj.gameObject);
		Destroy (popObj.gameObject);
		Destroy (particleObj.gameObject);
		GameObject kunimap = GameObject.Find("KuniMap").gameObject;
		GameObject kuniIconView = GameObject.Find("KuniIconView").gameObject;
		Destroy (kunimap.gameObject);
		Destroy (kuniIconView.gameObject);

		FinMaker (panel);

	}

	public void FinMaker(GameObject panel){
        int langId = PlayerPrefs.GetInt("langId");
        string finPath = "Prefabs/clearOrGameOver/Fin";
		GameObject finObj = Instantiate(Resources.Load (finPath)) as GameObject;
		finObj.transform.SetParent(panel.transform);
		finObj.transform.localScale = new Vector2(1,1);
		finObj.transform.FindChild ("ReStartBtn").GetComponent<ShowDaimyoSelect> ().fin = finObj;
		finObj.transform.FindChild ("ReStartBtn").GetComponent<ShowDaimyoSelect> ().panel = panel;
		finObj.transform.FindChild ("ReStartBtn").transform.FindChild("Question").GetComponent<QA> ().qaId = 12;

		int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
		Daimyo daimyo = new Daimyo ();
		int busyoId = daimyo.getDaimyoBusyoId (myDaimyo);
		string path = "Prefabs/Player/" + busyoId;
		GameObject prefab = Instantiate(Resources.Load (path)) as GameObject;
		prefab.transform.SetParent (finObj.transform);
		prefab.transform.localScale = new Vector2 (50, 60);
		prefab.transform.localPosition = new Vector2 (-170, 340);
		

		prefab.GetComponent<UnitMover> ().enabled = false;
		Destroy (prefab.GetComponent<PlayerHP>());

		//Value Set
		GameObject eval = finObj.transform.FindChild("Eval").gameObject;
		GameObject Gunji = eval.transform.FindChild("Gunji").gameObject;
		GameObject Naisei = eval.transform.FindChild("Naisei").gameObject;
		GameObject Gaikou = eval.transform.FindChild("Gaikou").gameObject;
		GameObject Bouryaku = eval.transform.FindChild("Bouryaku").gameObject;

		//Gunji
		int TrackTotalKassenNo = PlayerPrefs.GetInt("TrackTotalKassenNo");
		int TrackWinNo = PlayerPrefs.GetInt("TrackWinNo");
		int TrackTettaiNo = PlayerPrefs.GetInt("TrackTettaiNo");
		int TrackBiggestDaimyoId = PlayerPrefs.GetInt("TrackBiggestDaimyoId");
		int TrackBiggestDaimyoHei = PlayerPrefs.GetInt("TrackBiggestDaimyoHei");
		int TrackMyBiggestHei = PlayerPrefs.GetInt("TrackMyBiggestHei");
		int TrackNewBusyoHireNo = PlayerPrefs.GetInt("TrackNewBusyoHireNo");
		int TrackEarnMoney = PlayerPrefs.GetInt("TrackEarnMoney");

		Gunji.transform.FindChild ("KassenNo").transform.FindChild ("Text").GetComponent<Text> ().text = TrackTotalKassenNo.ToString();
		int winRatio = Mathf.CeilToInt((float)TrackWinNo / (float)TrackTotalKassenNo * 100);
		if (TrackWinNo == 0) {
			winRatio = 0;
		}
		Gunji.transform.FindChild ("WinRatio").transform.FindChild ("Text").GetComponent<Text> ().text = winRatio.ToString() + "%";
		Gunji.transform.FindChild ("Tettai").transform.FindChild ("Text").GetComponent<Text> ().text = TrackTettaiNo.ToString();
		string daimyoName = daimyo.getName(TrackBiggestDaimyoId,langId);
		Gunji.transform.FindChild ("BiggestEnemy").transform.FindChild ("Text").GetComponent<Text> ().text = daimyoName;
		Gunji.transform.FindChild ("BiggestEnemyHei").transform.FindChild ("Text").GetComponent<Text> ().text = TrackBiggestDaimyoHei.ToString();
		Gunji.transform.FindChild ("BiggestPlayerHei").transform.FindChild ("Text").GetComponent<Text> ().text = TrackMyBiggestHei.ToString();
		Gunji.transform.FindChild ("NewBusyoHire").transform.FindChild ("Text").GetComponent<Text> ().text = TrackNewBusyoHireNo.ToString();
		Gunji.transform.FindChild ("EarnMoney").transform.FindChild ("Text").GetComponent<Text> ().text = TrackEarnMoney.ToString();



		//Naisei
		int TrackGetMoneyNo = PlayerPrefs.GetInt("TrackGetMoneyNo");
		int TrackGetHyourouNo = PlayerPrefs.GetInt("TrackGetHyourouNo");
		int TrackGetSozaiNo = PlayerPrefs.GetInt("TrackGetSozaiNo");
		int TrackBuildMoneyNo = PlayerPrefs.GetInt("TrackBuildMoneyNo");
		int TrackJyosyuNinmeiNo = PlayerPrefs.GetInt("TrackJyosyuNinmeiNo");
		int TrackTabibitoNo = PlayerPrefs.GetInt("TrackTabibitoNo");
		int TrackIjinNo = PlayerPrefs.GetInt("TrackIjinNo");
		int HstNanbansen = PlayerPrefs.GetInt("HstNanbansen");

		Naisei.transform.FindChild ("MoneyProfitQty").transform.FindChild ("Text").GetComponent<Text> ().text = TrackGetMoneyNo.ToString();
		Naisei.transform.FindChild ("HyourouProfitQty").transform.FindChild ("Text").GetComponent<Text> ().text = TrackGetHyourouNo.ToString();
		Naisei.transform.FindChild ("SozaiProfitQty").transform.FindChild ("Text").GetComponent<Text> ().text = TrackGetSozaiNo.ToString();
		Naisei.transform.FindChild ("NaiseiInvestment").transform.FindChild ("Text").GetComponent<Text> ().text = TrackBuildMoneyNo.ToString();
		Naisei.transform.FindChild ("JyosyuNinmeiNo").transform.FindChild ("Text").GetComponent<Text> ().text = TrackJyosyuNinmeiNo.ToString();
		Naisei.transform.FindChild ("TabibitoNo").transform.FindChild ("Text").GetComponent<Text> ().text = TrackTabibitoNo.ToString();
		Naisei.transform.FindChild ("IjinNo").transform.FindChild ("Text").GetComponent<Text> ().text = TrackIjinNo.ToString();
		Naisei.transform.FindChild ("Nanbansen").transform.FindChild ("Text").GetComponent<Text> ().text = HstNanbansen.ToString();



		//Gaikou
		int TrackGaikouNo = PlayerPrefs.GetInt("TrackGaikouNo");
		int TrackGaikouMoneyNo = PlayerPrefs.GetInt("TrackGaikouMoneyNo");
		int TrackDoumeiNo = PlayerPrefs.GetInt("TrackDoumeiNo");
		int TrackCyouteiNo = PlayerPrefs.GetInt("TrackCyouteiNo");
		int TrackSyouninNo = PlayerPrefs.GetInt("TrackSyouninNo");
		int syogunDaimyoId = PlayerPrefs.GetInt ("syogunDaimyoId");
        string Bakuhu = "";        
        if (langId == 2) {
            Bakuhu = "Not Yet";
        }else {
            Bakuhu = "未開闢";
        }
		if(syogunDaimyoId == myDaimyo){
            if (langId == 2) {
                Bakuhu = "Done";
            }else {
                Bakuhu = "開闢済";
            }
		}
		bool soubujireiFlg = PlayerPrefs.GetBool ("soubujireiFlg");
        string Soubujirei = "";
        if (langId == 2) {
            Soubujirei = "Not Yet";
        }else {
            Soubujirei = "未施行";
        }
        if (soubujireiFlg){
            if (langId == 2) {
                Soubujirei = "Done";
		    }else {
                Soubujirei = "施行済";
            }
        }
		int TrackToubatsuNo = PlayerPrefs.GetInt("TrackToubatsuNo");

		Gaikou.transform.FindChild ("GaikouNo").transform.FindChild ("Text").GetComponent<Text> ().text = TrackGaikouNo.ToString();
		Gaikou.transform.FindChild ("GaikouMoney").transform.FindChild ("Text").GetComponent<Text> ().text = TrackGaikouMoneyNo.ToString();
		Gaikou.transform.FindChild ("DoumeiNo").transform.FindChild ("Text").GetComponent<Text> ().text = TrackDoumeiNo.ToString();
		Gaikou.transform.FindChild ("CyouteiNo").transform.FindChild ("Text").GetComponent<Text> ().text = TrackCyouteiNo.ToString();
		Gaikou.transform.FindChild ("SyouninNo").transform.FindChild ("Text").GetComponent<Text> ().text = TrackSyouninNo.ToString();
		Gaikou.transform.FindChild ("Bakuhu").transform.FindChild ("Text").GetComponent<Text> ().text = Bakuhu;
		Gaikou.transform.FindChild ("Soubujirei").transform.FindChild ("Text").GetComponent<Text> ().text = Soubujirei;
		Gaikou.transform.FindChild ("ToubatsuNo").transform.FindChild ("Text").GetComponent<Text> ().text = TrackToubatsuNo.ToString();



		//Bouryaku
		int TrackBouryakuNo = PlayerPrefs.GetInt("TrackBouryakuNo");
		int TrackBouryakuSuccessNo = PlayerPrefs.GetInt("TrackBouryakuSuccessNo");
		int TrackCyouhouNo = PlayerPrefs.GetInt("TrackCyouhouNo");
		int TrackRyugenNo = PlayerPrefs.GetInt("TrackRyugenNo");
		int TrackGihouHei = PlayerPrefs.GetInt("TrackGihouHei");
		int TrackCyouryakuNo = PlayerPrefs.GetInt("TrackCyouryakuNo");
		int TrackLinkCutNo = PlayerPrefs.GetInt("TrackLinkCutNo");
		int TrackSyuppeiNo = PlayerPrefs.GetInt("TrackSyuppeiNo");

		Bouryaku.transform.FindChild ("BouryakuQty").transform.FindChild ("Text").GetComponent<Text> ().text = TrackBouryakuNo.ToString();
		int successRatio = Mathf.CeilToInt((float)TrackBouryakuSuccessNo / (float)TrackBouryakuNo * 100);
		if (TrackBouryakuSuccessNo == 0) {
			successRatio = 0;
		}
		Bouryaku.transform.FindChild ("BouryakuRatio").transform.FindChild ("Text").GetComponent<Text> ().text = successRatio.ToString() + "%";
		Bouryaku.transform.FindChild ("Cyouhou").transform.FindChild ("Text").GetComponent<Text> ().text = TrackCyouhouNo.ToString();
		Bouryaku.transform.FindChild ("Ryugen").transform.FindChild ("Text").GetComponent<Text> ().text = TrackRyugenNo.ToString();
		Bouryaku.transform.FindChild ("GihouHei").transform.FindChild ("Text").GetComponent<Text> ().text = TrackGihouHei.ToString();
		Bouryaku.transform.FindChild ("Cyouryaku").transform.FindChild ("Text").GetComponent<Text> ().text = TrackCyouryakuNo.ToString();
		Bouryaku.transform.FindChild ("LinkCut").transform.FindChild ("Text").GetComponent<Text> ().text = TrackLinkCutNo.ToString();
		Bouryaku.transform.FindChild ("Syuppei").transform.FindChild ("Text").GetComponent<Text> ().text = TrackSyuppeiNo.ToString();


	}
}
