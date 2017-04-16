using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class CyouteiPop : MonoBehaviour {

	public GameObject SelectSyoukaijyoBoard;
	public string syoukaijyoRank = "";
	public int yukoudo = 0;
	public bool myDaimyoFlg = false;
	public int occupiedDaimyoId = 0;
	public string occupiedDaimyoName = "";

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
        Message msg = new Message();

        if (name != "PassButton") {
			//Syoukaijyo Confirm Pop
			audioSources [0].Play ();

			//Back
			string pathOfBack = "Prefabs/Cyoutei/TouchBackLayer";
			GameObject back = Instantiate (Resources.Load (pathOfBack)) as GameObject;
			back.transform.parent = GameObject.Find ("Panel").transform;
			back.transform.localScale = new Vector2 (1, 1);
			back.transform.localPosition = new Vector2 (0, 0);

			//Cyoutei Pop
			string pathOfPop = "Prefabs/Cyoutei/SelectSyoukaijyoBoard";
			GameObject pop = Instantiate (Resources.Load (pathOfPop)) as GameObject;
			pop.transform.parent = GameObject.Find ("Panel").transform;
			pop.transform.localScale = new Vector2 (1, 1);
			pop.transform.localPosition = new Vector2 (0, 0);
			pop.name = "SelectSyoukaijyoBoard";
			back.GetComponent<CloseLayer> ().closeTargetObj = pop;
			back.GetComponent<CloseLayer> ().closeTargetBack = back;
			pop.transform.FindChild ("CloseBtn").GetComponent<CloseLayer> ().closeTargetObj = pop;
			pop.transform.FindChild ("CloseBtn").GetComponent<CloseLayer> ().closeTargetBack = back;

			//Check Syoukaijyo
			string nowQty = PlayerPrefs.GetString ("cyoutei");
			//string nowQty = "0,0,0";
			List<string> nowQtyList = new List<string> ();
			char[] delimiterChars = {','};
			nowQtyList = new List<string> (nowQty.Split (delimiterChars));

			GameObject scrollView = pop.transform.FindChild ("ScrollView").gameObject;
			GameObject content = scrollView.transform.FindChild ("Content").gameObject;
			bool notZeroflg = false;
			//Jyo
			if (nowQtyList [2] == "0") {
				content.transform.FindChild ("Jyo").gameObject.SetActive (false);
			} else {
				notZeroflg = true;
				content.transform.FindChild ("Jyo").transform.FindChild ("cyoutei").transform.FindChild ("Qty").GetComponent<Text> ().text = nowQtyList [2];
				content.transform.FindChild ("Jyo").GetComponent<SyoukaijyoSelect>().OnClick();
			}

			//Cyu
			if (nowQtyList [1] == "0") {
				content.transform.FindChild ("Cyu").gameObject.SetActive (false);
			} else {
				notZeroflg = true;
				content.transform.FindChild ("Cyu").transform.FindChild ("cyoutei").transform.FindChild ("Qty").GetComponent<Text> ().text = nowQtyList [1];
				content.transform.FindChild ("Cyu").GetComponent<SyoukaijyoSelect>().OnClick();
			}

			//Ge
			if (nowQtyList [0] == "0") {
				content.transform.FindChild ("Ge").gameObject.SetActive (false);
			} else {
				notZeroflg = true;
				content.transform.FindChild ("Ge").transform.FindChild ("cyoutei").transform.FindChild ("Qty").GetComponent<Text> ().text = nowQtyList [0];
				content.transform.FindChild ("Ge").GetComponent<SyoukaijyoSelect>().OnClick();
			}


			if (!notZeroflg) {
				scrollView.transform.FindChild ("NoSyoukaijyo").GetComponent<Text> ().enabled = true;
				pop.transform.FindChild ("Serihu").transform.FindChild ("Text").GetComponent<Text> ().text = msg.getMessage(16);
				pop.transform.FindChild ("PassButton").gameObject.SetActive (false);
			}

			pop.transform.FindChild("PassButton").GetComponent<CyouteiPop>().SelectSyoukaijyoBoard = pop;
			pop.transform.FindChild("PassButton").GetComponent<CyouteiPop>().myDaimyoFlg = myDaimyoFlg;
			pop.transform.FindChild("PassButton").GetComponent<CyouteiPop>().occupiedDaimyoId = occupiedDaimyoId;
			pop.transform.FindChild("PassButton").GetComponent<CyouteiPop>().occupiedDaimyoName = occupiedDaimyoName;
			pop.transform.FindChild("PassButton").GetComponent<CyouteiPop>().yukoudo = yukoudo;

		} else {
			//Cyoutei Main Pop

			int hyourou = PlayerPrefs.GetInt ("hyourou");
			if (hyourou >= 5) {

				int newHyourou = hyourou - 5;
				PlayerPrefs.SetInt("hyourou",newHyourou);
				GameObject.Find("HyourouCurrentValue").GetComponent<Text>().text = newHyourou.ToString();

                //Check Yukoudo
                int ratio = 100 - yukoudo;
				if(myDaimyoFlg){
					ratio = 0;
				}
				float percent = Random.value;
				percent = percent * 100;

				if(percent > ratio){

                    //Stop Timer
                    GameObject.Find("GameController").GetComponent<MainStageController>().eventStopFlg = true;

                    audioSources [3].Play ();
					SelectSyoukaijyoBoard.transform.FindChild("CloseBtn").GetComponent<CloseLayer>().OnClick();

					string pathOfBack = "Prefabs/Cyoutei/CyouteiBack";
					GameObject back = Instantiate (Resources.Load (pathOfBack)) as GameObject;
					back.transform.parent = GameObject.Find ("Panel").transform;
					back.transform.localScale = new Vector2 (1, 1);
					back.transform.localPosition = new Vector2 (0, 0);

					string pathOfPop = "Prefabs/Cyoutei/CyouteiBoard";
					GameObject pop = Instantiate (Resources.Load (pathOfPop)) as GameObject;
					pop.transform.parent = GameObject.Find ("Panel").transform;
					pop.transform.localScale = new Vector2 (1, 1);
					pop.transform.localPosition = new Vector2 (0, 0);
					pop.name = "CyouteiBoard";

					CloseLayer CloseLayerScript =  pop.transform.FindChild("CloseSyoukaijyo").GetComponent<CloseLayer>();
					CloseLayerScript.closeTargetBack = back;
					CloseLayerScript.closeTargetObj = pop;
					CloseLayerScript.syoukaijyoRank = syoukaijyoRank;
					CloseLayerScript.occupiedFlg = myDaimyoFlg;
                    CloseLayerScript.syouninCyouteiFlg = true;

                    //RandomValue
                    int yukouAddValue = 0;
					int yukouReducePoint = Random.Range(2, 10);

					int stopBattleRatio = 0;
					int stopBattleReducePoint = Random.Range(2, 10);

					int kanniRatio = 0;
					int kanniReducePoint = Random.Range(20, 100);
					int syoukaijyoRankId = 0;

					int cyoutekiReducePoint = Random.Range(80, 100);

					//Change Menu by syoukaijyo rank
					GameObject action = pop.transform.FindChild("Action").gameObject;
					if(syoukaijyoRank == "Ge"){
						List<string> btnNameList = new List<string> (){"Bakuhu","Cyouteki"};
						enableButton(pop,btnNameList);
						yukouAddValue = Random.Range(1, 3);
						stopBattleRatio = Random.Range(10, 30);
						kanniRatio = Random.Range(20, 60);
						syoukaijyoRankId = 1;
						action.transform.FindChild("ActionValue").GetComponent<Text>().text = "1";
						action.transform.FindChild("ActionMaxValue").GetComponent<Text>().text = "1";

					}else if(syoukaijyoRank == "Cyu"){
						List<string> btnNameList = new List<string> (){"Bakuhu","Cyouteki"};
						enableButton(pop,btnNameList);
						yukouAddValue = Random.Range(3, 8);
						stopBattleRatio = Random.Range(30, 80);
						kanniRatio = Random.Range(40, 80);
						syoukaijyoRankId = 2;
						action.transform.FindChild("ActionValue").GetComponent<Text>().text = "2";
						action.transform.FindChild("ActionMaxValue").GetComponent<Text>().text = "2";


					}else if(syoukaijyoRank == "Jyo"){
						yukouAddValue = Random.Range(8, 15);
						stopBattleRatio = 100;
						kanniRatio = Random.Range(60, 100);
						syoukaijyoRankId = 3;
						action.transform.FindChild("ActionValue").GetComponent<Text>().text = "3";
						action.transform.FindChild("ActionMaxValue").GetComponent<Text>().text = "3";

					}

					//TargetKanni
					Kanni kanni = new Kanni();
					int kuniQty = GameObject.Find("GameController").GetComponent<MainStageController>().myKuniQty;
					int kanniId = kanni.getRandomKanni(syoukaijyoRankId,kuniQty);

					//TargetCyouteki
					string seiryoku = PlayerPrefs.GetString ("seiryoku");
					List<string> seiryokuList = new List<string> ();
					char[] delimiterChars = {','};
					seiryokuList = new List<string> (seiryoku.Split (delimiterChars));

					string openKuni = PlayerPrefs.GetString ("openKuni");
					List<string> openKuniList = new List<string> ();
					openKuniList = new List<string> (openKuni.Split (delimiterChars));

					int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
					int cyoutekiDaimyoId = CloseLayerScript.cyoutekiDaimyo;
					//openKuniList.RemoveAll (x => x == occupiedDaimyoId.ToString());

					//my kuni
					List<string> myKuniList = new List<string>();
					List<string> occupiedDaimyoKuniList = new List<string>();
					for (int i = 0; i < seiryokuList.Count; i++) {
						int tmpDaimyoId = int.Parse(seiryokuList [i]);
						if (tmpDaimyoId == myDaimyo) {
							int kuniId = i + 1;
							myKuniList.Add (kuniId.ToString());
						}else if(tmpDaimyoId == occupiedDaimyoId){
							int kuniId = i + 1;
							occupiedDaimyoKuniList.Add (kuniId.ToString());
						}
					}
					openKuniList.RemoveAll (myKuniList.Contains);
					openKuniList.RemoveAll (occupiedDaimyoKuniList.Contains);


					int rdmId = UnityEngine.Random.Range(0,openKuniList.Count);
					int targetKuniId = int.Parse(openKuniList[rdmId]);
					int cyoutekiDaimyo = int.Parse(seiryokuList[targetKuniId-1]);


					//reduce cyoutei syoukaijyo
					DoSell script = new DoSell ();
					script.deleteKouekiOrCyoutei(syoukaijyoRankId,"cyoutei",1);


					//Set Parametor
					CloseLayerScript.yukouAddValue = yukouAddValue;
					CloseLayerScript.yukouReducePoint = yukouReducePoint;
					CloseLayerScript.stopBattleRatio = stopBattleRatio;
					CloseLayerScript.stopBattleReducePoint = stopBattleReducePoint;
					CloseLayerScript.kanniId = kanniId;
					if(kanniId != 0){
						CloseLayerScript.kanniName = kanni.getKanniName(kanniId);
						CloseLayerScript.kanniRatio = kanniRatio;
						CloseLayerScript.kanniReducePoint = kanniReducePoint;
					}
					CloseLayerScript.cyoutekiDaimyo = cyoutekiDaimyo;
					CloseLayerScript.cyoutekiReducePoint = cyoutekiReducePoint;


					//Cyoutei Point
					int cyouteiPoint = PlayerPrefs.GetInt("cyouteiPoint");
					pop.transform.FindChild("CyouteiPoint").transform.FindChild("CyouteiValue").GetComponent<Text>().text = cyouteiPoint.ToString() + "%";


					bool doneFirstCyouteiFlg = PlayerPrefs.GetBool("doneFirstCyouteiFlg");
					Daimyo daimyo = new Daimyo();

					if(!doneFirstCyouteiFlg){
						//1st time
						PlayerPrefs.SetBool ("questSpecialFlg5",true);

						PlayerPrefs.SetBool("doneFirstCyouteiFlg",true);

						KuniInfo kuni = new KuniInfo();
						int kuniId = kuni.getOneKuniId(myDaimyo);
						int firstKanniId = kanni.getKuniKanni(kuniId);
						string firstKanniName = kanni.getKanniName(firstKanniId);

						string myKanni = PlayerPrefs.GetString ("myKanni");
						if(myKanni != null && myKanni !=""){
							myKanni = myKanni + "," + firstKanniId.ToString();
						}else{
							myKanni = firstKanniId.ToString();
						}
						PlayerPrefs.SetString ("myKanni",myKanni);
						PlayerPrefs.Flush();

						MainStageController mainStage = new MainStageController();
						mainStage.questExtension();
                        string serihu = "";
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            serihu = "Your good rumor has arrived to the Imperial court. Please lend me your power for the world.\n I assigned you as " + firstKanniName + ".";
                        }else {
                            serihu = "天下静謐のため力を貸してくれ。\n" + firstKanniName + "に任ずるぞ。";
                        }
                        pop.transform.FindChild("Serihu").transform.FindChild("Text").GetComponent<Text>().text = serihu;

					}else{
						//2nd time

						//Serihu
						string daimyoName = daimyo.getName(myDaimyo);
                        string serihu = "";
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            serihu = "Lord " + daimyoName + ".\n What do you want?";
                        }else {
                            serihu = "おお、" + daimyoName + "殿。\n此度は何用か。" ;
                        }
                        pop.transform.FindChild("Serihu").transform.FindChild("Text").GetComponent<Text>().text = serihu;

					}


				}else{
					audioSources [4].Play ();
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        msg.makeMessage("My lord " + occupiedDaimyoName + " disturbed us to visit coart.");
                    } else {
                        msg.makeMessage("御屋形様、" + occupiedDaimyoName + "めに参内を邪魔されました。");
                    }
                        
				
				}
			}else{
				audioSources [4	].Play ();

				string NGtext = msg.getMessage(7);
				msg.makeMessage (NGtext);
			}
		}
	}

	public void enableButton(GameObject pop, List<string> btnNameList){
		Color enableImageColor = new Color (35f / 255f, 35f / 255f, 35f / 255f, 155f / 255f);
		Color enableTextColor = new Color (125f / 255f, 125f / 255f, 125f / 255f, 255f / 255f);

		foreach(string n in btnNameList){
			GameObject btn = pop.transform.FindChild (n).gameObject;
			btn.GetComponent<Button>().enabled = false;
			btn.GetComponent<Image> ().color = enableImageColor;
			btn.transform.FindChild("Text").GetComponent<Text>().color = enableTextColor;
		}
	}


}
