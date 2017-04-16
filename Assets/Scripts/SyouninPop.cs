using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class SyouninPop : MonoBehaviour {

	public GameObject SelectSyoukaijyoBoard;
	public string syoukaijyoRank = "";
	public int yukoudo = 0;
	public bool myDaimyoFlg;
	public string occupiedDaimyoName = "";
	public bool sakaiFlg;

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
			string pathOfPop = "Prefabs/Syounin/SelectSyoukaijyoBoard";
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
			string nowQty = PlayerPrefs.GetString ("koueki");
			//string nowQty = "0,0,0";
			List<string> nowQtyList = new List<string> ();
			char[] delimiterChars = { ',' };
			nowQtyList = new List<string> (nowQty.Split (delimiterChars));
			
			GameObject scrollView = pop.transform.FindChild ("ScrollView").gameObject;
			GameObject content = scrollView.transform.FindChild ("Content").gameObject;
			bool notZeroflg = false;
			//Jyo
			if (nowQtyList [2] == "0") {
				content.transform.FindChild ("Jyo").gameObject.SetActive (false);
			} else {
				notZeroflg = true;
				content.transform.FindChild ("Jyo").transform.FindChild ("syounin").transform.FindChild ("Qty").GetComponent<Text> ().text = nowQtyList [2];
				content.transform.FindChild ("Jyo").GetComponent<SyoukaijyoSelect> ().OnClick ();
			}
			
			//Cyu
			if (nowQtyList [1] == "0") {
				content.transform.FindChild ("Cyu").gameObject.SetActive (false);
			} else {
				notZeroflg = true;
				content.transform.FindChild ("Cyu").transform.FindChild ("syounin").transform.FindChild ("Qty").GetComponent<Text> ().text = nowQtyList [1];
				content.transform.FindChild ("Cyu").GetComponent<SyoukaijyoSelect> ().OnClick ();
			}
			
			//Ge
			if (nowQtyList [0] == "0") {
				content.transform.FindChild ("Ge").gameObject.SetActive (false);
			} else {
				notZeroflg = true;
				content.transform.FindChild ("Ge").transform.FindChild ("syounin").transform.FindChild ("Qty").GetComponent<Text> ().text = nowQtyList [0];
				content.transform.FindChild ("Ge").GetComponent<SyoukaijyoSelect> ().OnClick ();
			}
			
			
			if (!notZeroflg) {
				scrollView.transform.FindChild ("NoSyoukaijyo").GetComponent<Text> ().enabled = true;
				pop.transform.FindChild ("Serihu").transform.FindChild ("Text").GetComponent<Text> ().text = msg.getMessage(41);
				pop.transform.FindChild ("PassButton").gameObject.SetActive (false);
			}
			
			pop.transform.FindChild ("PassButton").GetComponent<SyouninPop> ().SelectSyoukaijyoBoard = pop;
			pop.transform.FindChild ("PassButton").GetComponent<SyouninPop> ().myDaimyoFlg = myDaimyoFlg;
			pop.transform.FindChild ("PassButton").GetComponent<SyouninPop> ().occupiedDaimyoName = occupiedDaimyoName;
			pop.transform.FindChild ("PassButton").GetComponent<SyouninPop> ().yukoudo = yukoudo;

			//Icon Change
			if (sakaiFlg) {
				string imagePath = "Prefabs/Syounin/Sprite/syounin2";
				pop.transform.FindChild ("Syounin").GetComponent<Image> ().sprite = 
					Resources.Load (imagePath, typeof(Sprite)) as Sprite;
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    pop.transform.FindChild("SyouninName").GetComponent<Text>().text = "Rikyu Sen";
                }else { 
                    pop.transform.FindChild ("SyouninName").GetComponent<Text> ().text = "千利休";
                }
            }else {
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    pop.transform.FindChild("SyouninName").GetComponent<Text>().text = "Sotan Kamiya";
                }
            }


		} else {
			//Cyoutei Main Pop
			

			int hyourou = PlayerPrefs.GetInt ("hyourou");
			if (hyourou >= 5) {

				int newHyourou = hyourou - 5;
				PlayerPrefs.SetInt ("hyourou", newHyourou);
				GameObject.Find ("HyourouCurrentValue").GetComponent<Text> ().text = newHyourou.ToString ();
                
                //Check Yukoudo
                int ratio = 100 - yukoudo;
				if (myDaimyoFlg) {
					ratio = 0;
				}
				float percent = Random.value;
				percent = percent * 100;
				
				if (percent > ratio) {
                    //Stop Timer
                    GameObject.Find("GameController").GetComponent<MainStageController>().eventStopFlg = true;
                    
                    audioSources [3].Play ();
					SelectSyoukaijyoBoard.transform.FindChild ("CloseBtn").GetComponent<CloseLayer> ().OnClick ();

                    string pathOfBack = "Prefabs/Cyoutei/CyouteiBack";
					GameObject back = Instantiate (Resources.Load (pathOfBack)) as GameObject;
					back.transform.parent = GameObject.Find ("Panel").transform;
					back.transform.localScale = new Vector2 (1, 1);
					back.transform.localPosition = new Vector2 (0, 0);
					
					string pathOfPop = "Prefabs/Syounin/SyouninBoard";
					GameObject pop = Instantiate (Resources.Load (pathOfPop)) as GameObject;
					pop.transform.parent = GameObject.Find ("Panel").transform;
					pop.transform.localScale = new Vector2 (1, 1);
					pop.transform.localPosition = new Vector2 (0, 0);
					pop.name = "SyouninBoard";
					
					CloseLayer CloseLayerScript = pop.transform.FindChild ("CloseSyoukaijyo").GetComponent<CloseLayer> ();
					CloseLayerScript.closeTargetBack = back;
					CloseLayerScript.closeTargetObj = pop;
					CloseLayerScript.syoukaijyoRank = syoukaijyoRank;
					CloseLayerScript.occupiedFlg = myDaimyoFlg;
                    CloseLayerScript.syouninCyouteiFlg = true;

                    //RandomValue
                    int yukouAddValue = 0;
					int yukouReducePoint = Random.Range (2, 10);
					
					int stopBattleRatio = 0;
					int stopBattleReducePoint = Random.Range (2, 10);
					
					int kanniRatio = 0;
					int kanniReducePoint = Random.Range (20, 100);
					int syoukaijyoRankId = 0;
					
					//Change Menu by syoukaijyo rank
					GameObject action = pop.transform.FindChild ("Action").gameObject;

					if (syoukaijyoRank == "Ge") {
						if (!myDaimyoFlg) {
							List<string> btnNameList = new List<string> (){ "Yasen", "Youjinbou", "Cyakai", "Gijyutsu" };
							enableButton (pop, btnNameList);
						} else {
							List<string> btnNameList = new List<string> (){ "Youjinbou", "Cyakai", "Gijyutsu" };
							enableButton (pop, btnNameList);
						}
						yukouAddValue = Random.Range (1, 3);
						stopBattleRatio = Random.Range (10, 30);
						kanniRatio = Random.Range (20, 60);
						syoukaijyoRankId = 1;
						action.transform.FindChild ("ActionValue").GetComponent<Text> ().text = "1";
						action.transform.FindChild ("ActionMaxValue").GetComponent<Text> ().text = "1";


					} else if (syoukaijyoRank == "Cyu") {
						if (!myDaimyoFlg) {
							List<string> btnNameList = new List<string> (){ "Yasen", "Gijyutsu" };
							enableButton (pop, btnNameList);
						} else {
							List<string> btnNameList = new List<string> (){ "Gijyutsu" };
							enableButton (pop, btnNameList);
						}
						yukouAddValue = Random.Range (3, 8);
						stopBattleRatio = Random.Range (30, 80);
						kanniRatio = Random.Range (40, 80);
						syoukaijyoRankId = 2;
						action.transform.FindChild ("ActionValue").GetComponent<Text> ().text = "2";
						action.transform.FindChild ("ActionMaxValue").GetComponent<Text> ().text = "2";

						
					} else if (syoukaijyoRank == "Jyo") {
						if (!myDaimyoFlg) {
							List<string> btnNameList = new List<string> (){ "Yasen", "Gijyutsu" };
							enableButton (pop, btnNameList);
						}
						yukouAddValue = Random.Range (8, 15);
						stopBattleRatio = 100;
						kanniRatio = Random.Range (60, 100);
						syoukaijyoRankId = 3;
						action.transform.FindChild ("ActionValue").GetComponent<Text> ().text = "3";
						action.transform.FindChild ("ActionMaxValue").GetComponent<Text> ().text = "3";


					}

					//reduce cyoutei syoukaijyo
					DoSell script = new DoSell ();
					script.deleteKouekiOrCyoutei(syoukaijyoRankId,"koueki",1);

					//TargetKahou Preparation
					Kahou kahou = new Kahou ();
					string kahouCdString = "";
					string kahouIdString = "";
					for (int i = 1; i < 4; i++) {
						List<string> kahouRandom = new List<string> () {
							"bugu",
							"kabuto",
							"gusoku",
							"meiba",
							"cyadougu",
							"chishikisyo",
							"heihousyo"
						};
						int rdm = UnityEngine.Random.Range (0, 7);
						string kahouType = kahouRandom [rdm];

						string kahouRank = getItemRank (syoukaijyoRankId);
						int kahouId = kahou.getRamdomKahouId (kahouType, kahouRank);
						//string targetKahou = kahouType + kahouId.ToString();

						if (kahouCdString != null && kahouCdString != "") {
							kahouCdString = kahouCdString + "," + kahouType;
							kahouIdString = kahouIdString + "," + kahouId.ToString ();
						} else {
							kahouCdString = kahouType;
							kahouIdString = kahouId.ToString ();
						}
					}
					CloseLayerScript.kahouCdString = kahouCdString;
					CloseLayerScript.kahouIdString = kahouIdString;


					//TargetBusshi Preparation
					string busshiQtyString = ""; //Qty of busshi
					string busshiRankString = ""; //Rank of busshi
					for (int l = 1; l < 6; l++) {
						int rdmQty = UnityEngine.Random.Range (1, 10);
						int rdmRnk = UnityEngine.Random.Range (1, 4);

						if (busshiQtyString != null && busshiQtyString != "") {
							busshiQtyString = busshiQtyString + "," + rdmQty.ToString ();
							busshiRankString = busshiRankString + "," + rdmRnk.ToString ();
						} else {
							busshiQtyString = rdmQty.ToString ();
							busshiRankString = rdmRnk.ToString ();
						}

					}
					CloseLayerScript.busshiQtyString = busshiQtyString;
					CloseLayerScript.busshiRankString = busshiRankString;


					//TargetYoujinbou
					int rdmKengouId = UnityEngine.Random.Range (1, 10);
					CloseLayerScript.rdmKengouId = rdmKengouId;


					//Yasengaku
					int yasenAmt = 0;
					if (syoukaijyoRankId == 1) {
						yasenAmt = UnityEngine.Random.Range (1000, 3000);
					} else if (syoukaijyoRankId == 2) {
						yasenAmt = UnityEngine.Random.Range (2000, 5000);
					} else if (syoukaijyoRankId == 3) {
						yasenAmt = UnityEngine.Random.Range (3000, 10000);
					}
					CloseLayerScript.yasenAmt = yasenAmt;


					//Gijyutsuiten
					int techId = UnityEngine.Random.Range (1, 4);
					CloseLayerScript.techId = techId;
					

					//Discount Percent
					float discount = UnityEngine.Random.Range (0.5f, 0.9f);
					CloseLayerScript.discount = discount;
					

					Daimyo daimyo = new Daimyo ();
					int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
						
					//Serihu
					string daimyoName = daimyo.getName (myDaimyo);
                    string serihu = "";
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        serihu = "Oh, lord " + daimyoName + ".\nCan I help you?";
                    }else {
                        serihu = "これは" + daimyoName + "様。\n私共めに何用で御座いましょうか。";
                    }
                        
					pop.transform.FindChild ("Serihu").transform.FindChild ("Text").GetComponent<Text> ().text = serihu;


					PlayerPrefs.SetBool ("questSpecialFlg4",true);
					PlayerPrefs.Flush ();

					MainStageController mainStage = new MainStageController();
					mainStage.questExtension();

				} else {
					audioSources [4].Play ();
					
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        msg.makeMessage("My lord " + occupiedDaimyoName + " disturbed our business.");
                    }
                    else {
                        msg.makeMessage("御屋形様、" + occupiedDaimyoName + "めに\n取引を邪魔されました。");
                    }
                }
			} else {
				audioSources [4].Play ();	
				msg.makeMessage (msg.getMessage(7));
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


	public string getItemRank(int syoukaijyoRankId){

		string itemRank = "";
		float kahouPercent = UnityEngine.Random.value;
		kahouPercent = kahouPercent * 100;

		if (syoukaijyoRankId == 3) {
			//Jyo
			if (kahouPercent <= 50) {
				//S
				itemRank = "S";
			} else if(50 < kahouPercent && kahouPercent <= 85){
				//A
				itemRank = "A";
			}else if(85 < kahouPercent  && kahouPercent <= 95){
				//B
				itemRank = "B";
			}else if(95 < kahouPercent){
				//C
				itemRank = "C";
			}

		} else if (syoukaijyoRankId == 2) {
			//Cyu
			if (kahouPercent <= 10) {
				//S
				itemRank = "S";
			} else if(10 < kahouPercent && kahouPercent <= 60){
				//A
				itemRank = "A";
			}else if(60 < kahouPercent  && kahouPercent <= 90){
				//B
				itemRank = "B";
			}else if(90 < kahouPercent){
				//C
				itemRank = "C";
			}

		} else if (syoukaijyoRankId == 1) {
			//Ge
			if (kahouPercent <= 5) {
				//S
				itemRank = "S";
			} else if(5 < kahouPercent && kahouPercent <= 25){
				//A
				itemRank = "A";
			}else if(25 < kahouPercent  && kahouPercent <= 55){
				//B
				itemRank = "B";
			}else if(55 < kahouPercent){
				//C
				itemRank = "C";
			}
		}

		return itemRank;
	}



}
