using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System.Collections.Generic;

public class CyouteiMenu : MonoBehaviour {

	public GameObject targetGunzei;
	public bool onceBakuhuNg = false;

	public void OnClick () {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		GameObject board = GameObject.Find ("CyouteiBoard").gameObject;
		Message msg = new Message (); 

		GameObject actionValue = GameObject.Find ("ActionValue").gameObject;
		int actionRemainQty = int.Parse(actionValue.GetComponent<Text> ().text);

		if (actionRemainQty <= 0) {
			audioSources [4].Play ();

			msg.makeMessage (msg.getMessage(17));

            string serihu = msg.getMessage(18);
            mikadoSerihuChanger(serihu);

		} else {

			CloseLayer CloseLayerScript = GameObject.Find ("CloseSyoukaijyo").GetComponent<CloseLayer>();

			if (name == "Kenjyo") {
				int money = PlayerPrefs.GetInt ("money");
				if (money < 1000) {
					audioSources [4].Play ();
					msg.makeMessage (msg.getMessage(19));
					mikadoSerihuChanger(msg.getMessage(20));
				} else {
					audioSources [0].Play ();

					string path = "Prefabs/Cyoutei/MenuKenjyo";
					GameObject menu = Instantiate (Resources.Load (path)) as GameObject;
					menu.transform.SetParent (board.transform);
					menu.transform.localScale = new Vector2 (1, 1);
					menu.transform.localPosition = new Vector2 (0, -180);
					menu.transform.FindChild ("Close").GetComponent<CloseMenu> ().obj = menu;
					menu.name = "MenuKenjyo";
					GameObject giveSlider = menu.transform.FindChild ("GiveSlider").gameObject;
					giveSlider.GetComponent<GiveSlider> ().valueObj = menu.transform.FindChild ("GiveMoneyValue").gameObject;
					giveSlider.GetComponent<Slider> ().value = 1.0f;

					int tmp = money / 1000;
					if (tmp < 10) {
						giveSlider.GetComponent<Slider> ().maxValue = (float)tmp;
					}
					mikadoSerihuChanger(msg.getMessage(21));
				}

			} else if (name == "Cyoutei") {
				audioSources [0].Play ();

				string path = "Prefabs/Cyoutei/MenuCyoutei";
				GameObject menu = Instantiate (Resources.Load (path)) as GameObject;
				menu.transform.SetParent (board.transform);
				menu.transform.localScale = new Vector2 (1, 1);
				menu.transform.localPosition = new Vector2 (0, -180);
				menu.transform.FindChild ("Close").GetComponent<CloseMenu> ().obj = menu;
				menu.name = "MenuCyoutei";

				int yukoudoUp = CloseLayerScript.yukouAddValue;
				int reducePoint = CloseLayerScript.yukouReducePoint;

				menu.transform.FindChild("CyouteiUpValue").GetComponent<Text>().text = yukoudoUp.ToString();
				menu.transform.FindChild("ReduceValue").GetComponent<Text>().text = reducePoint.ToString();
				mikadoSerihuChanger(msg.getMessage(22));

			} else if (name == "Teisen") {

				//Check
				bool gunzeiFlg = false;
				int myDaimyo = PlayerPrefs.GetInt("myDaimyo");

				foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Gunzei")){
					int checkDaimyoId = obs.GetComponent<Gunzei>().dstDaimyoId;
					if(checkDaimyoId == myDaimyo){
						gunzeiFlg = true;
						targetGunzei = obs;
						break;
					}
				}
				if(gunzeiFlg){
					audioSources [0].Play ();

					string path = "Prefabs/Cyoutei/MenuTeisen";
					GameObject menu = Instantiate (Resources.Load (path)) as GameObject;
					menu.transform.SetParent (board.transform);
					menu.transform.localScale = new Vector2 (1, 1);
					menu.transform.localPosition = new Vector2 (0, -180);
					menu.transform.FindChild ("Close").GetComponent<CloseMenu> ().obj = menu;
					menu.name = "MenuTeisen";

					int ratio = CloseLayerScript.stopBattleRatio;
					int reducePoint = CloseLayerScript.stopBattleReducePoint;

					menu.transform.FindChild("RatioValue").GetComponent<Text>().text = ratio.ToString();
					menu.transform.FindChild("ReduceValue").GetComponent<Text>().text = reducePoint.ToString();

					int srcDaimyoId = targetGunzei.GetComponent<Gunzei>().srcDaimyoId;
					string srcDaimyoName = targetGunzei.GetComponent<Gunzei>().srcDaimyoName;

					string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + srcDaimyoId.ToString ();
					GameObject gunzei = menu.transform.FindChild("Gunzei").gameObject;
					gunzei.GetComponent<Image> ().sprite = 
						Resources.Load (imagePath, typeof(Sprite)) as Sprite;
                    string serihu = "";
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        gunzei.transform.FindChild("Text").GetComponent<Text>().text = srcDaimyoName;
                        serihu = "Have you not been doing well with " + srcDaimyoName + "? We can stop the battle.";
                    }else { 
                        gunzei.transform.FindChild("Text").GetComponent<Text>().text = srcDaimyoName + "隊";
                        serihu = "ほう、" + srcDaimyoName + "と上手くいっていないのか。停戦要請をしても良いぞ。";
                    }
                    mikadoSerihuChanger(serihu);

				}else{
					audioSources [4].Play ();
					msg.makeMessage (msg.getMessage(23));
				}


			} else if (name == "Kanni") {

				int kanniId = CloseLayerScript.kanniId;

				if(kanniId !=0){
					audioSources [0].Play ();

					string path = "Prefabs/Cyoutei/MenuKanni";
					GameObject menu = Instantiate (Resources.Load (path)) as GameObject;
					menu.transform.SetParent (board.transform);
					menu.transform.localScale = new Vector2 (1, 1);
					menu.transform.localPosition = new Vector2 (0, -180);
					menu.transform.FindChild ("Close").GetComponent<CloseMenu> ().obj = menu;
					menu.name = "MenuKanni";
					

					int ratio = CloseLayerScript.kanniRatio;
					int reducePoint = CloseLayerScript.kanniReducePoint;
					string kanniName = CloseLayerScript.kanniName;
					GameObject.Find("NextKanni").GetComponent<Text>().text = kanniName;

					GameObject.Find("ratioValue").GetComponent<Text>().text = ratio.ToString();
					GameObject.Find("ReduceValue").GetComponent<Text>().text = reducePoint.ToString();
                    string serihu = "";
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        serihu = "I'm glad of your loyalty.\n I can give you " + kanniName + ".";
                    } else {
                        serihu = "そなたの忠勤うれしく思う。\n今は" + kanniName + "の任が空いておるぞ。";
                    }
					mikadoSerihuChanger(serihu);

				}else{
					audioSources [4].Play ();
                    
					msg.makeMessage (msg.getMessage(24));
					mikadoSerihuChanger(msg.getMessage(25));
					
				}
			} else if (name == "Bakuhu") {
				//Check I'm not Syougun 
				int syogunDaimyoId = PlayerPrefs.GetInt("syogunDaimyoId");
				int myDaimyoId = PlayerPrefs.GetInt ("myDaimyo");
				Daimyo daimyo = new Daimyo ();
				string myDaimyoBusyoName = daimyo.getName (myDaimyoId);

				if (syogunDaimyoId == myDaimyoId) {
					audioSources [4].Play ();
					msg.makeMessage (msg.getMessage(26));

                    string serihu = "";
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        serihu = "Lord " + myDaimyoBusyoName + ". You are syogun. What do you want anymore?";
                    }else {
                        serihu = myDaimyoBusyoName + "殿、そなた既に幕府を開いておろう。これ以上何を望むというのじゃ。";
                    }
                        
					mikadoSerihuChanger(serihu);

				} else {
					
					string seiryoku = PlayerPrefs.GetString ("seiryoku");
					List<string> seiryokuList = new List<string> ();
					char[] delimiterChars = {','};
					seiryokuList = new List<string> (seiryoku.Split (delimiterChars));

					if (seiryokuList.Contains (syogunDaimyoId.ToString())) {
						audioSources [4].Play ();

						//Check the other Syougun Not Exist
						string syogunBusyoName = daimyo.getName (syogunDaimyoId);
                        string Text = "";
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            Text = "Other family " + syogunBusyoName + " has been assigned as syogun.";
                        } else {
                            Text = "既に" + syogunBusyoName + "殿が征夷大将軍に任命されておりますぞ。";
                        }
						msg.makeMessage (Text);
                        string serihu = "";
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            serihu = "Other family, loard " + syogunBusyoName + " has been assigned as syogun.";
                        } else {
                            serihu = "既に" + syogunBusyoName + "殿が幕府を開いておる。そなた世情にあまりにも疎いのう。";
                        }
						mikadoSerihuChanger(serihu);

					} else {
						//Checku Kinai Cleared
						List<int> needKuni = new List<int>{6,11,12,13,16,17,38,39};
						bool kuniCheckOKFlg = true;
						string NGKuniName = "";
						KuniInfo kuni = new KuniInfo ();

						for (int i = 0; i < needKuni.Count; i++) {
							int kuniId = needKuni [i] - 1;
							if(seiryokuList[kuniId] != myDaimyoId.ToString()){
								kuniCheckOKFlg = false;
								NGKuniName = kuni.getKuniName (kuniId+1);
							}
						}

						if (!kuniCheckOKFlg) {
							//NG
							audioSources [4].Play ();

							msg.makeMessage (msg.getMessage(27));
                            string serihu = "";
                            if (Application.systemLanguage != SystemLanguage.Japanese) {
                                serihu = "You don't have country " + NGKuniName + ". It's too early for you to assign syogun.";
                            }else {
                                serihu = NGKuniName + "をまだ治めていないようじゃのう。残念だが征夷大将軍の任命は時期尚早じゃ。";
                            }
							mikadoSerihuChanger(serihu);

						} else {
							//Check whether is there any other daimyo who has 1/2 kuni
							string clearedKuni = PlayerPrefs.GetString ("clearedKuni");
							List<string> clearedKuniList = new List<string> ();

							if (clearedKuni.Contains (",")) {
								clearedKuniList = new List<string> (clearedKuni.Split (delimiterChars));
							} else {
								clearedKuniList.Add (clearedKuni);
							}

							int maxKuniQty = 0;
							int myYukouValue = 0;
							string maxDaimyoName = "";
							GameObject kuniIconView = GameObject.Find ("KuniIconView");
							foreach(Transform obj in kuniIconView.transform){
								int daimyoId = obj.gameObject.GetComponent<SendParam> ().daimyoId;
								if (daimyoId != myDaimyoId) {
									int tmpQty = obj.gameObject.GetComponent<SendParam> ().kuniQty;
									if (tmpQty > maxKuniQty) {
										maxKuniQty = tmpQty;
										myYukouValue = obj.gameObject.GetComponent<SendParam> ().myYukouValue;
										maxDaimyoName = obj.gameObject.GetComponent<SendParam> ().daimyoName;
									}
								}
							}

							if (clearedKuniList.Count <= maxKuniQty * 2) {
								float bakuhuPercent = UnityEngine.Random.value;
								bakuhuPercent = bakuhuPercent * 100;

								if (bakuhuPercent < myYukouValue && !onceBakuhuNg) {
									//ok
									audioSources [0].Play ();

									string path = "Prefabs/Cyoutei/MenuBakuhu";
									GameObject menu = Instantiate (Resources.Load (path)) as GameObject;
									menu.transform.SetParent (board.transform);
									menu.transform.localScale = new Vector2 (1, 1);
									menu.transform.localPosition = new Vector2 (0, -180);
									menu.transform.FindChild ("Close").GetComponent<CloseMenu> ().obj = menu;
                                    menu.name = "MenuBakuhu";
									mikadoSerihuChanger(msg.getMessage(28));

								} else {
									//NG
									audioSources [4].Play ();

									onceBakuhuNg = true;
									msg.makeMessage (msg.getMessage(29));

                                    string serihu = "";

                                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                                        serihu = "I want to assign you but " + maxDaimyoName + " disagreed.\n I can't ignore due to his influence.";
                                    } else {
                                        serihu = "そちを任命したいのだが、\n" + maxDaimyoName + "が五月蝿うてのう。\n宮中にも影響力があるゆえ無視は出来ぬ。";
                                    }                                        
									mikadoSerihuChanger (serihu);
								}
							} else {
								audioSources [0].Play ();

								string path = "Prefabs/Cyoutei/MenuBakuhu";
								GameObject menu = Instantiate (Resources.Load (path)) as GameObject;
								menu.transform.SetParent (board.transform);
								menu.transform.localScale = new Vector2 (1, 1);
								menu.transform.localPosition = new Vector2 (0, -180);
								menu.transform.FindChild ("Close").GetComponent<CloseMenu> ().obj = menu;
								menu.name = "MenuBakuhu";
								mikadoSerihuChanger(msg.getMessage(28));

							}



						}
					}
				}


			} else if (name == "Cyouteki") {
				//Cyouteki Check
				int cyoutekiDaimyo = PlayerPrefs.GetInt("cyoutekiDaimyo",0);
				string seiryoku = PlayerPrefs.GetString ("seiryoku");
				List<string> seiryokuList = new List<string> ();
				char[] delimiterChars = {','};
				seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
				Daimyo daimyo = new Daimyo();

				if (seiryokuList.Contains (cyoutekiDaimyo.ToString ())) {
					//Aleady Exsit
					audioSources [4].Play ();

					string daimyoName = daimyo.getName (cyoutekiDaimyo);

                    string Text = "";
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        Text = daimyoName + " was declared as enemy of imperial court.";
                    }else {
                        Text = "既に" + daimyoName + "が朝敵に指定されているようですぞ。";
                    }
                        msg.makeMessage (Text);

				} else {
					//Not Exist
					int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
					bool remain1DaimyoFlg = daimyo.checkRemain1DaimyoOnMain(myDaimyo);
					if (remain1DaimyoFlg) {
						audioSources [4].Play ();
						msg.makeMessage (msg.getMessage(14));

					} else {
						audioSources [0].Play ();

						string path = "Prefabs/Cyoutei/MenuCyouteki";
						GameObject menu = Instantiate (Resources.Load (path)) as GameObject;
						menu.transform.SetParent (board.transform);
						menu.transform.localScale = new Vector2 (1, 1);
						menu.transform.localPosition = new Vector2 (0, -180);
						menu.transform.FindChild ("Close").GetComponent<CloseMenu> ().obj = menu;
						menu.name = "MenuCyouteki";

						cyoutekiDaimyo = CloseLayerScript.cyoutekiDaimyo;
						string daimyoName = daimyo.getName (cyoutekiDaimyo);

						menu.transform.FindChild ("CyoutekiDaimyo").GetComponent<Text> ().text = daimyoName;

						int cyoutekiReducePoint = CloseLayerScript.cyoutekiReducePoint;
						menu.transform.FindChild ("ReduceValue").GetComponent<Text> ().text = cyoutekiReducePoint.ToString ();

						CloseLayerScript.cyoutekiDaimyoName = daimyoName;
                        string serihu = "";
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            serihu = "Please attack " + daimyoName + ", enemy of imperial court.";
                        }else {
                            serihu = "朝廷に弓引く、逆賊" + daimyoName + "を討ってくれ。";
                        }
						mikadoSerihuChanger (serihu);
					}
				}




			}
		}
	}

	public void mikadoSerihuChanger(string serihu){
		GameObject.Find("Serihu").transform.FindChild("Text").GetComponent<Text>().text = serihu;
	}
}
