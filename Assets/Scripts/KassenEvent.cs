using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class KassenEvent : MonoBehaviour {

	char[] delimiterChars = {','};
	Stage stage = new Stage ();
	BusyoInfoGet busyo = new BusyoInfoGet ();


	public void MakeKassenComment(string kassenWinLoseFlee, int enemyDaimyoId, int kuniId){

		//make comment object
		GameObject commentObj = MakeCommentObj(enemyDaimyoId,kuniId);

        //Comment
        int langId = PlayerPrefs.GetInt("langId");
        List<string> kassenWinLoseFleeList = new List<string> ();
		kassenWinLoseFleeList = new List<string> (kassenWinLoseFlee.Split (delimiterChars));
		int myDaimyoBusyo = PlayerPrefs.GetInt ("myDaimyoBusyo");
		string myDiamyoName = busyo.getName (myDaimyoBusyo, langId);

        //Comment Select
        string finalComment = "";        
        if (langId == 2) {
            if (kassenWinLoseFleeList [1] == "2") {
                //Player Win
                string stageNmae = stage.getStageName(kuniId, int.Parse(kassenWinLoseFleeList[0]), langId);
                int powerTyp = stage.getPowerTyp(kuniId, int.Parse(kassenWinLoseFleeList[0]));
                if (powerTyp == 1) {
				    finalComment = "Aw, hell! We lost " + stageNmae + " castle and face.";
			    } else if (powerTyp == 2) {
				    finalComment = "What? " + stageNmae + " is strategic site for managing the territory. Let's get it back!";	
			    } else if (powerTyp == 3) {
				    finalComment = "Aw, hell! " + myDiamyoName + "！ We must get " + stageNmae + " castle back!";			
			    }
		    } else if (kassenWinLoseFleeList [1] == "1") {
                //Flee
                string stageNmae = stage.getStageName(kuniId, int.Parse(kassenWinLoseFleeList[0]), langId);
                int powerTyp = stage.getPowerTyp(kuniId, int.Parse(kassenWinLoseFleeList[0]));
                finalComment = "Hahaha, " + myDiamyoName + ". He ran away due to well-fortified " + stageNmae + " castle.";
		    } else if (kassenWinLoseFleeList [1] == "0") {
                //Player Lose
                string stageNmae = stage.getStageName(kuniId, int.Parse(kassenWinLoseFleeList[0]), langId);
                int powerTyp = stage.getPowerTyp(kuniId, int.Parse(kassenWinLoseFleeList[0]));
                if (powerTyp == 1) {
				    finalComment = "Haha, We have a good soldiers! We defeated " + myDiamyoName + ".";
			    } else if (powerTyp == 2) {
				    finalComment = "Hahaha, " + myDiamyoName + " ran away! He will give up to aim " + stageNmae + " castle.";	
			    } else if (powerTyp == 3) {
				    finalComment = "Our samurai, Raise a battle cry! We won the battle and defended " + stageNmae + " castle!";
			    }

		    } else if (kassenWinLoseFleeList [1] == "3") {
			    //Kuni
			    KuniInfo kuni = new KuniInfo ();
			    string kuniName = kuni.getKuniName (kuniId,langId);
			    finalComment = "We lost " + kuniName + " castle... Damn it " + myDiamyoName + ". Let's bury our resentment deep in our hearts！";

		    } else if (kassenWinLoseFleeList [1] == "4") {
			    //Metsubou
			    finalComment = "It's a fall of our clan...It was short-lived. How earn my place in history?";

		    }
        }else {
            if (kassenWinLoseFleeList[1] == "2") {
                //Player Win
                string stageNmae = stage.getStageName(kuniId, int.Parse(kassenWinLoseFleeList[0]), langId);
                int powerTyp = stage.getPowerTyp(kuniId, int.Parse(kassenWinLoseFleeList[0]));
                if (powerTyp == 1) {
                    finalComment = "うぬ、" + stageNmae + "を落とされたか。このままでは主家の面目が立たぬわ。";
                }
                else if (powerTyp == 2) {
                    finalComment = "なんと！" + stageNmae + "は重要拠点…。孤立する前に取り返すぞ。";
                }
                else if (powerTyp == 3) {
                    finalComment = "おのれ、" + myDiamyoName + "め！" + stageNmae + "の借りは必ず返してくれる。";
                }
            }
            else if (kassenWinLoseFleeList[1] == "1") {
                //Flee
                string stageNmae = stage.getStageName(kuniId, int.Parse(kassenWinLoseFleeList[0]), langId);
                int powerTyp = stage.getPowerTyp(kuniId, int.Parse(kassenWinLoseFleeList[0]));
                finalComment = "ははは、" + myDiamyoName + "め、あまりの" + stageNmae + "の堅固さに、戦わずに逃げ帰りおったぞ！";
            }
            else if (kassenWinLoseFleeList[1] == "0") {
                //Player Lose
                string stageNmae = stage.getStageName(kuniId, int.Parse(kassenWinLoseFleeList[0]), langId);
                int powerTyp = stage.getPowerTyp(kuniId, int.Parse(kassenWinLoseFleeList[0]));
                if (powerTyp == 1) {
                    finalComment = "ははは、者共、見事な槍働きじゃ！" + myDiamyoName + "めを叩き潰したぞ。";
                }
                else if (powerTyp == 2) {
                    finalComment = "ははは、" + myDiamyoName + "め、ほうほうの体で逃げ帰りおったわ。" + stageNmae + "の堅固さに舌をまいたろうて。";
                }
                else if (powerTyp == 3) {
                    finalComment = "勝ち鬨を上げよ、我等が勝利じゃ！天下の険、" + stageNmae + "を落とせるわけがなかろう。";
                }

            }
            else if (kassenWinLoseFleeList[1] == "3") {
                //Kuni
                KuniInfo kuni = new KuniInfo();
                string kuniName = kuni.getKuniName(kuniId,langId);
                finalComment = "我が" + kuniName + "を盗られてしまった…。おのれ、" + myDiamyoName + "め。この雪辱、必ず晴らしてくれようぞ！";

            }
            else if (kassenWinLoseFleeList[1] == "4") {
                //Metsubou
                finalComment = "当家もお終いか…。儚い世であったわ。我が名は後世にどう残るのかのう。";

            }


        }

		commentObj.transform.FindChild ("SerihuText").GetComponent<Text> ().text = finalComment;

	}

	public GameObject MakeCommentObj(int enemyDaimyoId, int kuniId){

		//Popup
		string cmntPath = "Prefabs/Map/stage/EventComment";
		GameObject commentObj = Instantiate (Resources.Load (cmntPath)) as GameObject;
		GameObject attackStagePopup = GameObject.Find ("AttackStagePopup").gameObject;
		commentObj.transform.SetParent (attackStagePopup.transform);
		commentObj.transform.localScale = new Vector2 (2.0f, 2.5f);
		commentObj.transform.localPosition = new Vector2 (-145, 307);
		commentObj.GetComponent<FadeoutSenpou> ().enabled = true;
		commentObj.name = "EventComment";

		//Busyo Image
		GameObject busyoImage = commentObj.transform.FindChild ("Mask").transform.FindChild ("BusyoImage").gameObject;
		Daimyo daimyo = new Daimyo ();
		int daimyoBusyoId = daimyo.getDaimyoBusyoId (enemyDaimyoId);
		string imagePath = "Prefabs/Player/Sprite/unit" + daimyoBusyoId.ToString ();
		busyoImage.GetComponent<Image> ().sprite = 
			Resources.Load (imagePath, typeof(Sprite)) as Sprite;
		
		return commentObj;
	}



	public void MakeEvent(bool clearFlg, int kuniId, GameObject kuniMap, int enemyDaimyoId){

        //Check No Commnet
        int langId = PlayerPrefs.GetInt("langId");
        bool fromKassenFlg = PlayerPrefs.GetBool ("fromKassenFlg");
		if (!fromKassenFlg) {
			if (clearFlg) {
				//cleared
				//1.Kokunin Ikki
				//2.Ikkou Ikki
				Debug.Log(clearFlg +",ikki");

			} else {
				//never cleared 

				/*1. Betlay in the case of isolation >> 5-20%*/
				//Count Link No
				List<int> noLinkStageList = new List<int>();
				foreach (Transform stage in kuniMap.transform) {
					if (stage.gameObject.GetComponent<ShowStageDtl> ()) {
						if(!stage.gameObject.GetComponent<ShowStageDtl> ().clearedFlg){
							if (stage.gameObject.GetComponent<ShowStageDtl> ().linkNo <= 0) {
								noLinkStageList.Add (stage.gameObject.GetComponent<ShowStageDtl> ().stageId);
							}
						}
					}
				}

				//Count Remain Stage and check there are over 2 stages
				string clearedStage = "kuni" + kuniId;
				string clearedStageString = PlayerPrefs.GetString (clearedStage);
				List<string> clearedStageList = new List<string> ();
				if (clearedStageString != null && clearedStageString != "") {
					clearedStageList = new List<string> (clearedStageString.Split (delimiterChars));
				}


				if (0 < clearedStageList.Count && clearedStageList.Count < 9) {
					if (noLinkStageList.Count != 0) {
						int rdmId = UnityEngine.Random.Range (0, noLinkStageList.Count);
						int betlayStageId = noLinkStageList [rdmId];
						string stageTmp = "stage" + betlayStageId.ToString ();
						GameObject stageObj = kuniMap.transform.FindChild (stageTmp).gameObject;
						int powerType = stageObj.GetComponent<ShowStageDtl> ().powerType;

						if (powerType == 1 || powerType == 2) {
							float eventRatio = 0;
							if (powerType == 1) {
								eventRatio = 30;
							} else if (powerType == 2) {
								eventRatio = 15;
							}

							//test
							//eventRatio = 100;

							float percent = UnityEngine.Random.value;
							percent = percent * 100;

							if (percent < eventRatio) {
								//Hit
								AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
								audioSources [7].Play ();

								GameObject commentObj = MakeCommentObj (enemyDaimyoId, kuniId);
								int myDaimyoBusyo = PlayerPrefs.GetInt ("myDaimyoBusyo");
								int myDaimyoId = PlayerPrefs.GetInt ("myDaimyo");
								BusyoInfoGet busyo = new BusyoInfoGet ();
								string myDiamyoName = busyo.getName (myDaimyoBusyo, langId);
								string stageName = stage.getStageName (kuniId, betlayStageId, langId);
								string finalComment = "";

                                if (langId == 2) {
                                    finalComment = "What? Isolated " + stageName + " castle betrayed " + myDiamyoName + ".";
                                } else {
                                    finalComment = "うぬう、何とした事だ。孤立した" + stageName + "が、" + myDiamyoName + "に寝返りおったわ。";
                                }
								commentObj.transform.FindChild ("SerihuText").GetComponent<Text> ().text = finalComment;

								//Data Change
								clearedStageString = clearedStageString + "," + betlayStageId;
								PlayerPrefs.SetString (clearedStage,clearedStageString);
								PlayerPrefs.Flush ();

								//Visualize & change value
								string clearedPath = "Prefabs/Map/cleared";
								GameObject cleared = Instantiate (Resources.Load (clearedPath)) as GameObject;
								cleared.transform.SetParent (stageObj.transform);
								stageObj.GetComponent<ShowStageDtl> ().clearedFlg = true;
								cleared.transform.localScale = new Vector2 (3, 5);
								cleared.transform.localPosition = new Vector2 (0, 0);
								stageObj.GetComponent<ShowStageDtl> ().clearedFlg = true;

								string animPath = "Prefabs/Map/stage/betrayAnimation";
								GameObject anim = Instantiate (Resources.Load (animPath)) as GameObject;
								anim.transform.SetParent (stageObj.transform);
								anim.transform.localScale = new Vector2 (8,4);
								anim.transform.localPosition = new Vector2 (0, 0);

							}
						}
					}
				}




				//1.Enemy Daimyo Attack(if there is enemy shiro) 
				//Not Yet



			}
		}
	}




}
