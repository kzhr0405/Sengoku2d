using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class RonkouKousyoMenu : MonoBehaviour {

	public string busyoId = "";
	public int kanniId = 0;
	public int jyosyuKuniId = 0;
	public string jyosyuName = "";

	public void OnClick(){

		BusyoStatusButton bsb = new BusyoStatusButton ();
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (name == "kanni") {
			if (kanniId == 0) {
				string myKanni = PlayerPrefs.GetString ("myKanni");

				if (myKanni != null && myKanni != "") {
					audioSources [0].Play ();

					bsb.commonPopup (24);
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        GameObject.Find ("popText").GetComponent<Text> ().text = "Royal Court Rank";
                    }else {
                        GameObject.Find("popText").GetComponent<Text>().text = "官位授与";
                    }
					string scrollPath = "Prefabs/Busyo/KanniScrollView";
					GameObject scroll = Instantiate (Resources.Load (scrollPath)) as GameObject;
					scroll.transform.SetParent (GameObject.Find ("board(Clone)").transform);
					scroll.transform.localScale = new Vector2 (1, 1);
					scroll.name = "KanniScrollView";
					RectTransform scrollTransform = scroll.GetComponent<RectTransform> ();
					scrollTransform.anchoredPosition3D = new Vector3 (0, 0, 0);

					List<string> myKanniList = new List<string> ();

					if (myKanni.Contains (",")) {
						char[] delimiterChars = { ',' };
						myKanniList = new List<string> (myKanni.Split (delimiterChars));
					} else {
						myKanniList.Add (myKanni);
					}

					myKanniList.Sort ();

					string pathSlot = "Prefabs/Busyo/KanniSlot";
					Kanni kanni = new Kanni ();
					GameObject content = scroll.transform.FindChild ("KanniContent").gameObject;
					for (int i = 0; i < myKanniList.Count; i++) {
						GameObject slot = Instantiate (Resources.Load (pathSlot)) as GameObject;
						slot.transform.SetParent (content.transform);
						slot.transform.localScale = new Vector2 (1, 1);

						int kanniIdTmp = int.Parse (myKanniList [i]);
						string kanniName = kanni.getKanni (kanniIdTmp);
						string kanniIkai = kanni.getIkai (kanniIdTmp);
						string EffectLabel = kanni.getEffectLabel (kanniIdTmp);
						int effect = kanni.getEffect (kanniIdTmp);

						slot.transform.FindChild ("Name").GetComponent<Text> ().text = kanniIkai + "\n" + kanniName;
						slot.transform.FindChild ("EffectLabel").GetComponent<Text> ().text = EffectLabel;
						slot.transform.FindChild ("EffectValue").GetComponent<Text> ().text = "+" + effect.ToString () + "%";

						GameObject btn = slot.transform.FindChild ("GiveButton").gameObject;
						btn.GetComponent<GiveKanni> ().busyoId = busyoId;
						btn.GetComponent<GiveKanni> ().kanniId = kanniIdTmp;

					}



				} else {
					Message msg = new Message ();
					audioSources [4].Play ();
					msg.makeMessage (msg.getMessage(60));
				}
			} else {
				audioSources [0].Play ();

				string backPath = "Prefabs/Busyo/back";
				GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
				back.transform.SetParent (GameObject.Find ("Panel").transform);
				back.transform.localScale = new Vector2 (1, 1);
				RectTransform backTransform = back.GetComponent<RectTransform> ();
				backTransform.anchoredPosition3D = new Vector3 (0, 0, 0);

				string removePath = "Prefabs/Busyo/KanniRemoveConfirm";
				GameObject remove = Instantiate (Resources.Load (removePath)) as GameObject;
				remove.transform.SetParent (GameObject.Find ("Panel").transform);
				remove.transform.localScale = new Vector2 (1, 1);
				remove.transform.localPosition = new Vector3 (0, 0, 0);
				remove.name = "KanniRemoveConfirm";
				BusyoInfoGet busyo = new BusyoInfoGet ();
				string busyoName = busyo.getName (int.Parse (busyoId));
				remove.transform.FindChild ("YesButton").GetComponent<DoRemoveKanni> ().busyoId = busyoId;
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    remove.transform.FindChild ("RemoveText").GetComponent<Text> ().text = "Are you sure you want to remove the rank of " + busyoName + "?";
                }else {
                    remove.transform.FindChild("RemoveText").GetComponent<Text>().text = busyoName + "殿の官位を\n罷免なさるのですか？";
                }
			}



		} else if (name == "jyosyu") {
			if (jyosyuKuniId == 0) {
				//Available Kuni

				string clearedKuniString = PlayerPrefs.GetString ("clearedKuni");
				char[] delimiterChars = { ',' };
				List<string> clearedKuniList = new List<string> ();
				if (clearedKuniString.Contains (",")) {
					clearedKuniList = new List<string> (clearedKuniString.Split (delimiterChars));
				} else {
					clearedKuniList.Add (clearedKuniString);
				}

				List<string> okKuniList = new List<string> ();
				for (int i = 0; i < clearedKuniList.Count; i++) {
					int kuniId = int.Parse (clearedKuniList [i]);
					string temp = "kuni" + kuniId.ToString ();
					string clearedKuni = PlayerPrefs.GetString (temp);
					//Shiro Qty
					if (clearedKuni != null && clearedKuni != "") {
						//Jyosyu Exist Check
						string jyosyuTmp = "jyosyu" + kuniId.ToString ();
						if (!PlayerPrefs.HasKey (jyosyuTmp)) {
							okKuniList.Add (kuniId.ToString ());
						}
					}
				}

				if (okKuniList.Count != 0) {
					audioSources [0].Play ();
					bsb.commonPopup (19);
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        GameObject.Find ("popText").GetComponent<Text> ().text = "Assign Lord";
                    }else {
                        GameObject.Find("popText").GetComponent<Text>().text = "城主任命";
                    }
					string scrollPath = "Prefabs/Busyo/KanniScrollView";
					GameObject scroll = Instantiate (Resources.Load (scrollPath)) as GameObject;
					scroll.transform.SetParent (GameObject.Find ("board(Clone)").transform);
					scroll.transform.localScale = new Vector2 (1, 1);
					scroll.name = "KanniScrollView";
					RectTransform scrollTransform = scroll.GetComponent<RectTransform> ();
					scrollTransform.anchoredPosition3D = new Vector3 (0, 0, 0);

					string pathSlot = "Prefabs/Busyo/ShiroSlot";
					GameObject content = scroll.transform.FindChild ("KanniContent").gameObject;
					KuniInfo kuni = new KuniInfo ();
					for (int i = 0; i < okKuniList.Count; i++) {

						GameObject slot = Instantiate (Resources.Load (pathSlot)) as GameObject;
						slot.transform.SetParent (content.transform);
						slot.transform.localScale = new Vector2 (1, 1);
						
						int kuniId = int.Parse (okKuniList [i]);
						string kuniName = kuni.getKuniName (kuniId);

						slot.transform.FindChild ("Name").GetComponent<Text> ().text = kuniName;


						//Status
						int jyosyuHei = 0;
						string naiseiTemp = "naisei" + kuniId.ToString ();
						string naiseiString = PlayerPrefs.GetString (naiseiTemp);

						List<string> naiseiList = new List<string> ();
						naiseiList = new List<string> (naiseiString.Split (delimiterChars));
						char[] delimiterChars2 = { ':' };
						Entity_naisei_mst naiseiMst = Resources.Load ("Data/naisei_mst") as Entity_naisei_mst;
						NaiseiController naisei = new NaiseiController ();


						string shiroLv = naiseiList [0];
						List<int> shiroEffectList = new List<int> ();
						shiroEffectList = naisei.getNaiseiList ("shiro", int.Parse (shiroLv));
						jyosyuHei = shiroEffectList [0];

						for (int j = 1; j < naiseiList.Count; j++) {
							List<string> naiseiContentList = new List<string> ();
							naiseiContentList = new List<string> (naiseiList [j].Split (delimiterChars2));

							if (naiseiContentList [0] != "0") {
								string type = naiseiMst.param [int.Parse (naiseiContentList [0])].code;
								if (type == "hsy") {
									List<int> naiseiEffectList = new List<int> ();
									naiseiEffectList = naisei.getNaiseiList (type, int.Parse (naiseiContentList [1]));

									jyosyuHei = jyosyuHei + naiseiEffectList [0];
								}

							}
						}


						slot.transform.FindChild ("EffectValue").GetComponent<Text> ().text = "+" + jyosyuHei.ToString ();
						
						GameObject btn = slot.transform.FindChild ("GiveButton").gameObject;
						btn.GetComponent<DoNinmei> ().busyoId = busyoId;
						btn.GetComponent<DoNinmei> ().kuniId = kuniId;
						btn.GetComponent<DoNinmei> ().jyosyuHei = jyosyuHei;
					}



				} else {
					audioSources [4].Play ();
					Message msg = new Message ();
					msg.makeMessage (msg.getMessage(61));
				}




			} else {
				audioSources [0].Play ();

				string backPath = "Prefabs/Common/TouchBack";
				GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
				back.transform.SetParent (GameObject.Find ("Panel").transform);
				back.transform.localScale = new Vector2 (1, 1);
				RectTransform backTransform = back.GetComponent<RectTransform> ();
				backTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
				back.name = "TouchBack";

				//Message Box
				string msgPath = "Prefabs/Naisei/KaininConfirm";
				GameObject msg = Instantiate (Resources.Load (msgPath)) as GameObject;
				msg.transform.SetParent (back.transform);
				msg.transform.localScale = new Vector2 (1, 1);
				RectTransform msgTransform = msg.GetComponent<RectTransform> ();
				msgTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
				msgTransform.name = "kaininConfirm";
				msg.transform.FindChild ("YesButton").GetComponent<DoKainin> ().kuniId = jyosyuKuniId;


				//Message Text Mod
				GameObject msgObj = msg.transform.FindChild ("KaininText").gameObject;
				int myDaimyoBusyo = PlayerPrefs.GetInt ("myDaimyoBusyo");
                string msgText = "";
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    if (myDaimyoBusyo == int.Parse (busyoId)) {
					    msgText = "My lord, do you want to resign the lord of this country?";
				    } else {
                        msgText = "My lord, do you want to remove " + jyosyuName + " from the lord of this country?";
                    }
                }else {
                    if (myDaimyoBusyo == int.Parse(busyoId)) {
                        msgText = "御館様、自らを城主から解任なさいますか？";
                    }
                    else {
                        msgText = "御館様、" + jyosyuName + "殿を城主から解任なさいますか？";
                    }
                }
				msgObj.GetComponent<Text> ().text = msgText;
			}

		} else if (name == "syugyo") {

			//Lv Check
			string sakuTmp = "saku" + busyoId;
			int sakuLv = PlayerPrefs.GetInt (sakuTmp,0);
            if (sakuLv == 0) {
                sakuLv = 1;
                PlayerPrefs.SetInt(sakuTmp, 1);
                PlayerPrefs.Flush();
            }

            if (sakuLv < 20) {
				audioSources [0].Play ();
				bsb.commonPopup (23);
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    GameObject.Find ("popText").GetComponent<Text> ().text = "Samurai Training";
                }else {
                    GameObject.Find("popText").GetComponent<Text>().text = "武者修行";
                }
				GameObject board = GameObject.Find ("board(Clone)").gameObject;

				//icon
				Saku saku = new Saku ();
				List<string> sakuList = new List<string>();
				string tmp = "gokui" + busyoId;
				if (PlayerPrefs.HasKey (tmp)) {
					int gokuiId = PlayerPrefs.GetInt (tmp);
                    if(gokuiId != 0) {
                        sakuList = saku.getGokuiInfoForNextLv(int.Parse(busyoId), gokuiId);
                    }else {
                        sakuList = saku.getSakuInfoForNextLv(int.Parse(busyoId));
                    }
                } else {
					sakuList = saku.getSakuInfoForNextLv (int.Parse (busyoId));
				}


				//Icon
				string sakuPath = "Prefabs/Saku/saku" + sakuList[0];
				GameObject sakuIcon = Instantiate (Resources.Load (sakuPath)) as GameObject;
				sakuIcon.transform.SetParent (board.transform);
				sakuIcon.transform.localScale = new Vector2 (1, 1);
				sakuIcon.transform.localPosition = new Vector3 (-300, 0, 0);
				sakuIcon.GetComponent<Button>().enabled = false;

				//board
				string syugyoPath = "Prefabs/Busyo/Syugyo";
				GameObject syugyo = Instantiate (Resources.Load (syugyoPath)) as GameObject;
				syugyo.transform.SetParent (board.transform);
				syugyo.transform.localScale = new Vector2 (1, 1);
				syugyo.transform.localPosition = new Vector3 (0, 0, 0);

				//busyo Icon
				GameObject busyoImageObj = syugyo.transform.FindChild("BusyoImage").gameObject;
				string busyoPath = "Prefabs/Player/Sprite/unit" + busyoId;
				busyoImageObj.GetComponent<Image> ().sprite = 
					Resources.Load (busyoPath, typeof(Sprite)) as Sprite;

				//Info
				GameObject popBack = syugyo.transform.FindChild("PopBack").gameObject;
				popBack.transform.FindChild("LvFrom").GetComponent<Text>().text = sakuLv.ToString();
				int nextLv = sakuLv + 1;
				popBack.transform.FindChild("LvTo").GetComponent<Text>().text = nextLv.ToString();
				popBack.transform.FindChild("SakuNameValue").GetComponent<Text>().text = sakuList[1];
				popBack.transform.FindChild("SakuExp").transform.FindChild("PopSakuExpValue").GetComponent<Text>().text = sakuList[2];

				GameObject btn = syugyo.transform.FindChild ("GiveSyugyo").gameObject;
				btn.GetComponent<DoSyugyo> ().busyoId = int.Parse(busyoId);
				btn.GetComponent<DoSyugyo> ().nextLv = nextLv;
				Color shortageColor = new Color (203f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);

				//Required Item

				//Kengou Start
				if (nextLv == 2) {
					//kengou Low
					string itemPath = "Prefabs/Item/kengou";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.7f, 0.7f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					string rankPath = "Prefabs/Busyo/RankForKengou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.12f,0.12f);
					rank.transform.localPosition = new Vector2 (0,0);
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        rank.GetComponent<Text> ().text = "Ono";
                    }else {
                        rank.GetComponent<Text>().text = "小野善鬼";
                    }
					item.transform.FindChild ("Rank").GetComponent<Text> ().enabled = false;

					int kengouId = 3;
					btn.GetComponent<DoSyugyo> ().kengouFlg = true;
					btn.GetComponent<DoSyugyo> ().kengouId = kengouId;
					bool kengouOKFlg = getKengouOKFlg(kengouId);
					btn.GetComponent<DoSyugyo> ().itemOKFlg = kengouOKFlg;
					if (kengouOKFlg == false) {
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}

				} else if (nextLv == 3) {
					string itemPath = "Prefabs/Item/kengou";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.7f, 0.7f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					string rankPath = "Prefabs/Busyo/RankForKengou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.12f,0.12f);
					rank.transform.localPosition = new Vector2 (0,0);
					if (Application.systemLanguage != SystemLanguage.Japanese) {
                        rank.GetComponent<Text>().text = "Kawasaki";
                    }else {
                        rank.GetComponent<Text>().text = "川崎時盛";
                    }
                    item.transform.FindChild ("Rank").GetComponent<Text> ().enabled = false;

					int kengouId = 5;
					btn.GetComponent<DoSyugyo> ().kengouFlg = true;
					btn.GetComponent<DoSyugyo> ().kengouId = kengouId;
					bool kengouOKFlg = getKengouOKFlg(kengouId);
					btn.GetComponent<DoSyugyo> ().itemOKFlg = kengouOKFlg;
					if (kengouOKFlg == false) {
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}

				} else if (nextLv == 4) {
					string itemPath = "Prefabs/Item/kengou";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.7f, 0.7f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					string rankPath = "Prefabs/Busyo/RankForKengou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.12f,0.12f);
					rank.transform.localPosition = new Vector2 (0,0);
					if (Application.systemLanguage != SystemLanguage.Japanese) {
                        rank.GetComponent<Text>().text = "Hayashizaki";
                    }else {
                        rank.GetComponent<Text>().text = "林崎重信";
                    }
                    item.transform.FindChild ("Rank").GetComponent<Text> ().enabled = false;

					int kengouId = 6;
					btn.GetComponent<DoSyugyo> ().kengouFlg = true;
					btn.GetComponent<DoSyugyo> ().kengouId = kengouId;
					bool kengouOKFlg = getKengouOKFlg(kengouId);
					btn.GetComponent<DoSyugyo> ().itemOKFlg = kengouOKFlg;
					if (kengouOKFlg == false) {
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}


				} else if (nextLv == 5) {
					//kengou Mid
					string itemPath = "Prefabs/Item/kengou";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.7f, 0.7f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					string rankPath = "Prefabs/Busyo/RankForKengou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.12f,0.12f);
					rank.transform.localPosition = new Vector2 (0,0);					
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        rank.GetComponent<Text>().text = "Morooka";
                    }else {
                        rank.GetComponent<Text>().text = "師岡一羽";
                    }
                    item.transform.FindChild ("Rank").GetComponent<Text> ().enabled = false;

					int kengouId = 9;
					btn.GetComponent<DoSyugyo> ().kengouFlg = true;
					btn.GetComponent<DoSyugyo> ().kengouId = kengouId;
					bool kengouOKFlg = getKengouOKFlg(kengouId);
					btn.GetComponent<DoSyugyo> ().itemOKFlg = kengouOKFlg;
					if (kengouOKFlg == false) {
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}
				
				} else if (nextLv == 6) {
					//kengou Mid
					string itemPath = "Prefabs/Item/kengou";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.7f, 0.7f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					string rankPath = "Prefabs/Busyo/RankForKengou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.12f,0.12f);
					rank.transform.localPosition = new Vector2 (0,0);
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        rank.GetComponent<Text>().text = "Okuyama";
                    }else {
                        rank.GetComponent<Text>().text = "奥山公重";
                    }
                    item.transform.FindChild ("Rank").GetComponent<Text> ().enabled = false;

					int kengouId = 2;
					btn.GetComponent<DoSyugyo> ().kengouFlg = true;
					btn.GetComponent<DoSyugyo> ().kengouId = kengouId;
					bool kengouOKFlg = getKengouOKFlg(kengouId);
					btn.GetComponent<DoSyugyo> ().itemOKFlg = kengouOKFlg;
					if (kengouOKFlg == false) {
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}

				} else if (nextLv == 7) {
					//kengou Mid
					string itemPath = "Prefabs/Item/kengou";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.7f, 0.7f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					string rankPath = "Prefabs/Busyo/RankForKengou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.12f,0.12f);
					rank.transform.localPosition = new Vector2 (0,0);					
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        rank.GetComponent<Text>().text = "Jingo";
                    }else {
                        rank.GetComponent<Text>().text = "神後宗治";
                    }
                    item.transform.FindChild ("Rank").GetComponent<Text> ().enabled = false;

					int kengouId = 10;
					btn.GetComponent<DoSyugyo> ().kengouFlg = true;
					btn.GetComponent<DoSyugyo> ().kengouId = kengouId;
					bool kengouOKFlg = getKengouOKFlg(kengouId);
					btn.GetComponent<DoSyugyo> ().itemOKFlg = kengouOKFlg;
					if (kengouOKFlg == false) {
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}


				} else if (nextLv == 8) {
					//kengou High
					string itemPath = "Prefabs/Item/kengou";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.7f, 0.7f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					string rankPath = "Prefabs/Busyo/RankForKengou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.12f,0.12f);
					rank.transform.localPosition = new Vector2 (0,0);
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        rank.GetComponent<Text>().text = "Kanemaki";
                    }else {
                        rank.GetComponent<Text>().text = "鐘捲自斎";
                    }
                    item.transform.FindChild ("Rank").GetComponent<Text> ().enabled = false;

					int kengouId = 4;
					btn.GetComponent<DoSyugyo> ().kengouFlg = true;
					btn.GetComponent<DoSyugyo> ().kengouId = kengouId;
					bool kengouOKFlg = getKengouOKFlg(kengouId);
					btn.GetComponent<DoSyugyo> ().itemOKFlg = kengouOKFlg;
					if (kengouOKFlg == false) {
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}

				} else if (nextLv == 9) {
					//kengou High
					string itemPath = "Prefabs/Item/kengou";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.7f, 0.7f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					string rankPath = "Prefabs/Busyo/RankForKengou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.12f,0.12f);
					rank.transform.localPosition = new Vector2 (0,0);
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        rank.GetComponent<Text>().text = "Hozoin";
                    }else {
                        rank.GetComponent<Text>().text = "宝蔵院胤栄";
                    }
                    item.transform.FindChild ("Rank").GetComponent<Text> ().enabled = false;

					int kengouId = 8;
					btn.GetComponent<DoSyugyo> ().kengouFlg = true;
					btn.GetComponent<DoSyugyo> ().kengouId = kengouId;
					bool kengouOKFlg = getKengouOKFlg(kengouId);
					btn.GetComponent<DoSyugyo> ().itemOKFlg = kengouOKFlg;
					if (kengouOKFlg == false) {
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}

				} else if (nextLv == 10) {
					//kengou High
					string itemPath = "Prefabs/Item/kengou";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.7f, 0.7f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					string rankPath = "Prefabs/Busyo/RankForKengou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.12f,0.12f);
					rank.transform.localPosition = new Vector2 (0,0);
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        rank.GetComponent<Text>().text = "Aisu";
                    }else {
                        rank.GetComponent<Text>().text = "愛州元香斎";
                    }
                    item.transform.FindChild ("Rank").GetComponent<Text> ().enabled = false;

					int kengouId = 1;
					btn.GetComponent<DoSyugyo> ().kengouFlg = true;
					btn.GetComponent<DoSyugyo> ().kengouId = kengouId;
					bool kengouOKFlg = getKengouOKFlg(kengouId);
					btn.GetComponent<DoSyugyo> ().itemOKFlg = kengouOKFlg;
					if (kengouOKFlg == false) {
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}

				} else if (nextLv == 11) {
					//kengou High
					string itemPath = "Prefabs/Item/kengou";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.7f, 0.7f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					string rankPath = "Prefabs/Busyo/RankForKengou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.12f,0.12f);
					rank.transform.localPosition = new Vector2 (0,0);
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        rank.GetComponent<Text>().text = "Hikita";
                    }else {
                        rank.GetComponent<Text>().text = "疋田景兼";
                    }
                    item.transform.FindChild ("Rank").GetComponent<Text> ().enabled = false;

					int kengouId = 7;
					btn.GetComponent<DoSyugyo> ().kengouFlg = true;
					btn.GetComponent<DoSyugyo> ().kengouId = kengouId;
					bool kengouOKFlg = getKengouOKFlg(kengouId);
					btn.GetComponent<DoSyugyo> ().itemOKFlg = kengouOKFlg;
					if (kengouOKFlg == false) {
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}
				
					//Kengou End


					//Kahou Start
				} else if (nextLv == 12) {
					//heihousyo B
					string itemPath = "Prefabs/Item/Kahou/NoHeihousyo";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.5f, 0.5f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					string rankPath = "Prefabs/Busyo/RankForKahou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.4f,0.4f);
					rank.transform.localPosition = new Vector2 (30,-30);

					string kahouType = "heihousyo";
					string kahouRank = "B";
					rank.GetComponent<Text> ().text = kahouRank;
					int kahouId = getKahouOKFlg(kahouType, kahouRank);
					btn.GetComponent<DoSyugyo> ().kengouFlg = false;
					btn.GetComponent<DoSyugyo> ().kahouId = kahouId;
					btn.GetComponent<DoSyugyo> ().kahouType = kahouType;
					if (kahouId != 0) {
						btn.GetComponent<DoSyugyo> ().itemOKFlg = true;
					}else{
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}

				} else if (nextLv == 13) {
					//chishikisyo B
					string itemPath = "Prefabs/Item/Kahou/NoChishikisyo";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.5f, 0.5f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					int itemQty = 1;
					popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().text = itemQty.ToString ();

					string rankPath = "Prefabs/Busyo/RankForKahou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.4f,0.4f);
					rank.transform.localPosition = new Vector2 (30,-30);

					string kahouType = "chishikisyo";
					string kahouRank = "B";
					rank.GetComponent<Text> ().text = kahouRank;
					int kahouId = getKahouOKFlg(kahouType, kahouRank);
					btn.GetComponent<DoSyugyo> ().kengouFlg = false;
					btn.GetComponent<DoSyugyo> ().kahouId = kahouId;
					btn.GetComponent<DoSyugyo> ().kahouType = kahouType;
					if (kahouId != 0) {
						btn.GetComponent<DoSyugyo> ().itemOKFlg = true;
					}else{
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}

				} else if (nextLv == 14) {
					//Heihousyo A
					string itemPath = "Prefabs/Item/Kahou/NoHeihousyo";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.5f, 0.5f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					int itemQty = 1;
					popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().text = itemQty.ToString ();

					string rankPath = "Prefabs/Busyo/RankForKahou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.4f,0.4f);
					rank.transform.localPosition = new Vector2 (30,-30);

					string kahouType = "heihousyo";
					string kahouRank = "A";
					rank.GetComponent<Text> ().text = kahouRank;
					int kahouId = getKahouOKFlg(kahouType, kahouRank);
					btn.GetComponent<DoSyugyo> ().kengouFlg = false;
					btn.GetComponent<DoSyugyo> ().kahouId = kahouId;
					btn.GetComponent<DoSyugyo> ().kahouType = kahouType;
					if (kahouId != 0) {
						btn.GetComponent<DoSyugyo> ().itemOKFlg = true;
					}else{
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}

				} else if (nextLv == 15) {
					//Chishikisyo A
					string itemPath = "Prefabs/Item/Kahou/NoChishikisyo";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.5f, 0.5f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					int itemQty = 1;
					popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().text = itemQty.ToString ();

					string rankPath = "Prefabs/Busyo/RankForKahou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.4f,0.4f);
					rank.transform.localPosition = new Vector2 (30,-30);

					string kahouType = "chishikisyo";
					string kahouRank = "A";
					rank.GetComponent<Text> ().text = kahouRank;
					int kahouId = getKahouOKFlg(kahouType, kahouRank);
					btn.GetComponent<DoSyugyo> ().kengouFlg = false;
					btn.GetComponent<DoSyugyo> ().kahouId = kahouId;
					btn.GetComponent<DoSyugyo> ().kahouType = kahouType;
					if (kahouId != 0) {
						btn.GetComponent<DoSyugyo> ().itemOKFlg = true;
					}else{
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}

				} else if (nextLv == 16) {
					//Heihousyo S
					string itemPath = "Prefabs/Item/Kahou/NoHeihousyo";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.5f, 0.5f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					int itemQty = 1;
					popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().text = itemQty.ToString ();

					string rankPath = "Prefabs/Busyo/RankForKahou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.4f,0.4f);
					rank.transform.localPosition = new Vector2 (30,-30);

					string kahouType = "heihousyo";
					string kahouRank = "S";
					rank.GetComponent<Text> ().text = kahouRank;
					int kahouId = getKahouOKFlg(kahouType, kahouRank);
					btn.GetComponent<DoSyugyo> ().kengouFlg = false;
					btn.GetComponent<DoSyugyo> ().kahouId = kahouId;
					btn.GetComponent<DoSyugyo> ().kahouType = kahouType;
					if (kahouId != 0) {
						btn.GetComponent<DoSyugyo> ().itemOKFlg = true;
					}else{
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}

				} else if (nextLv == 17) {
					//Chishikisyo S
					string itemPath = "Prefabs/Item/Kahou/NoChishikisyo";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.5f, 0.5f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					int itemQty = 1;
					popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().text = itemQty.ToString ();

					string rankPath = "Prefabs/Busyo/RankForKahou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.4f,0.4f);
					rank.transform.localPosition = new Vector2 (30,-30);

					string kahouType = "chishikisyo";
					string kahouRank = "S";
					rank.GetComponent<Text> ().text = kahouRank;
					int kahouId = getKahouOKFlg(kahouType, kahouRank);
					btn.GetComponent<DoSyugyo> ().kengouFlg = false;
					btn.GetComponent<DoSyugyo> ().kahouId = kahouId;
					btn.GetComponent<DoSyugyo> ().kahouType = kahouType;
					if (kahouId != 0) {
						btn.GetComponent<DoSyugyo> ().itemOKFlg = true;
					}else{
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}


				} else if (nextLv == 18) {
					//Heihousyo S
					string itemPath = "Prefabs/Item/Kahou/NoHeihousyo";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.5f, 0.5f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					int itemQty = 1;
					popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().text = itemQty.ToString ();

					string rankPath = "Prefabs/Busyo/RankForKahou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.4f,0.4f);
					rank.transform.localPosition = new Vector2 (30,-30);

					string kahouType = "heihousyo";
					string kahouRank = "S";
					rank.GetComponent<Text> ().text = kahouRank;
					int kahouId = getKahouOKFlg(kahouType, kahouRank);
					btn.GetComponent<DoSyugyo> ().kengouFlg = false;
					btn.GetComponent<DoSyugyo> ().kahouId = kahouId;
					btn.GetComponent<DoSyugyo> ().kahouType = kahouType;
					if (kahouId != 0) {
						btn.GetComponent<DoSyugyo> ().itemOKFlg = true;
					}else{
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}


				} else if (nextLv == 19) {
					//Chishikisyo S
					string itemPath = "Prefabs/Item/Kahou/NoChishikisyo";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.5f, 0.5f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					int itemQty = 1;
					popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().text = itemQty.ToString ();

					string rankPath = "Prefabs/Busyo/RankForKahou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.4f,0.4f);
					rank.transform.localPosition = new Vector2 (30,-30);

					string kahouType = "chishikisyo";
					string kahouRank = "S";
					rank.GetComponent<Text> ().text = kahouRank;
					int kahouId = getKahouOKFlg(kahouType, kahouRank);
					btn.GetComponent<DoSyugyo> ().kengouFlg = false;
					btn.GetComponent<DoSyugyo> ().kahouId = kahouId;
					btn.GetComponent<DoSyugyo> ().kahouType = kahouType;
					if (kahouId != 0) {
						btn.GetComponent<DoSyugyo> ().itemOKFlg = true;
					}else{
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}


				} else if (nextLv == 20) {
					//Heihousyo S
					string itemPath = "Prefabs/Item/Kahou/NoHeihousyo";
					GameObject item = Instantiate (Resources.Load (itemPath)) as GameObject;
					item.transform.SetParent (board.transform);
					item.transform.localScale = new Vector2 (0.5f, 0.5f);
					item.transform.localPosition = new Vector3 (180, -100, 0);
					item.GetComponent<Button> ().enabled = false;

					int itemQty = 1;
					popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().text = itemQty.ToString ();

					string rankPath = "Prefabs/Busyo/RankForKahou";
					GameObject rank = Instantiate (Resources.Load (rankPath)) as GameObject;
					rank.transform.SetParent (item.transform);
					rank.transform.localScale = new Vector2 (0.4f,0.4f);
					rank.transform.localPosition = new Vector2 (30,-30);

					string kahouType = "heihousyo";
					string kahouRank = "S";
					rank.GetComponent<Text> ().text = kahouRank;
					int kahouId = getKahouOKFlg(kahouType, kahouRank);
					btn.GetComponent<DoSyugyo> ().kengouFlg = false;
					btn.GetComponent<DoSyugyo> ().kahouId = kahouId;
					btn.GetComponent<DoSyugyo> ().kahouType = kahouType;
					if (kahouId != 0) {
						btn.GetComponent<DoSyugyo> ().itemOKFlg = true;
					}else{
						popBack.transform.FindChild ("RequiredItem").transform.FindChild ("RequiredItemValue").GetComponent<Text> ().color = shortageColor;
					}

				}

				//Requried Money
				int money = getRequiredMoney(nextLv);
				popBack.transform.FindChild ("RequiredMoney").transform.FindChild ("RequiredMoneyValue").GetComponent<Text> ().text = money.ToString ();

				int nowMoney = PlayerPrefs.GetInt ("money");
				if (nowMoney < money) {
					btn.GetComponent<DoSyugyo> ().moneyOKFlg = false;
					popBack.transform.FindChild ("RequiredMoney").transform.FindChild ("RequiredMoneyValue").GetComponent<Text> ().color = shortageColor;
				} else {
					btn.GetComponent<DoSyugyo> ().moneyOKFlg = true;
				}


			


			} else {
				audioSources [4].Play ();
				Message msg = new Message ();
				msg.makeMessage (msg.getMessage(62));
			}




		} else if (name == "gokui") {

			string gokuiItem = PlayerPrefs.GetString ("gokuiItem");
			List<string> gokuiItemList = new List<string> ();
			char[] delimiterChars = {','};
			gokuiItemList = new List<string> (gokuiItem.Split (delimiterChars));
			bool existFlg = false;
			foreach(string num in gokuiItemList){
				if (num != "0") {
					existFlg = true;
				}
			}
			if(existFlg){
				audioSources [0].Play ();

				//Show
				bsb.commonPopup (25);
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    GameObject.Find ("popText").GetComponent<Text> ().text = "Master Secrets";
                }else {
                    GameObject.Find("popText").GetComponent<Text>().text = "極意一覧";
                }
				string scrollPath = "Prefabs/Busyo/KanniScrollView";
				GameObject scroll = Instantiate (Resources.Load (scrollPath)) as GameObject;
				scroll.transform.SetParent (GameObject.Find ("board(Clone)").transform);
				scroll.transform.localScale = new Vector2 (1, 1);
				scroll.name = "KanniScrollView";
				RectTransform scrollTransform = scroll.GetComponent<RectTransform> ();
				scrollTransform.anchoredPosition3D = new Vector3 (0, 0, 0);

				string pathSlot = "Prefabs/Busyo/GokuiSlot";
				GameObject content = scroll.transform.FindChild ("KanniContent").gameObject;
				Saku sakuScript = new Saku ();
				for(int i=0; i<gokuiItemList.Count; i++){
					string num = gokuiItemList [i];

					if (num != "0") {
						GameObject slot = Instantiate (Resources.Load (pathSlot)) as GameObject;
						slot.transform.SetParent (content.transform);
						slot.transform.localScale = new Vector2 (1, 1);
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            slot.transform.FindChild ("Qty").GetComponent<Text> ().text = num + " remain";
                        }else {
                            slot.transform.FindChild("Qty").GetComponent<Text>().text = "残" +num + "個";
                        }
						int sakuId = i + 11;
						List<string> gokuiInfoList = new List<string> ();
						gokuiInfoList = sakuScript.getGokuiInfoByLv(sakuId, 20);

						slot.transform.FindChild ("Name").GetComponent<Text> ().text = gokuiInfoList [1];
						slot.transform.FindChild ("Effect").GetComponent<Text> ().text = gokuiInfoList [2];

						slot.GetComponent<GiveGokuiConfirm> ().sakuId = sakuId;
						slot.GetComponent<GiveGokuiConfirm> ().sakuName = gokuiInfoList [1];
						slot.GetComponent<GiveGokuiConfirm> ().gokuiQty = int.Parse(num);
						slot.GetComponent<GiveGokuiConfirm> ().boardObj = GameObject.Find ("board(Clone)").gameObject;
						slot.GetComponent<GiveGokuiConfirm> ().boardBackObj = GameObject.Find ("Back(Clone)").gameObject;
					}
				}




			}else{
				audioSources [4].Play ();
				Message msg = new Message ();
				msg.makeMessage (msg.getMessage(63));
			}
		}


	}


	public int getRequiredMoney(int nextLv){

		int requiredMoney = 0;
		Entity_senpouItem_mst senpouItemMst  = Resources.Load ("Data/senpouItem_mst") as Entity_senpouItem_mst;
		requiredMoney = senpouItemMst.param [nextLv-1].requiredMoney;

		return requiredMoney;
	}

	public bool getKengouOKFlg(int kengouId){

		bool kengouOKFlg = false;

		string kengouString = PlayerPrefs.GetString("kengouItem");
		char[] delimiterChars = {','};
		List<string> kengouList = new List<string> ();
		kengouList = new List<string> (kengouString.Split (delimiterChars));

		int qty = int.Parse(kengouList[kengouId-1]);
		if (qty > 0) {
			kengouOKFlg = true;
		} 

		return kengouOKFlg;
	}


	public int getKahouOKFlg(string kahouType, string kahouRank){
		int targetKahouId = 0;

		Kahou kahou = new Kahou ();
		char[] delimiterChars = {','};
        if (kahouType=="heihousyo"){
			//heihousyo
			string availableHeihousyo = PlayerPrefs.GetString("availableHeihousyo");
            List<string> kahouList = new List<string> ();
			if (availableHeihousyo != null && availableHeihousyo != "") {
				if (availableHeihousyo.Contains (",")) {
					kahouList = new List<string> (availableHeihousyo.Split (delimiterChars));
				} else {
					kahouList.Add (availableHeihousyo);
				}
			}

			List<string> targetKahouList = new List<string> ();
			for(int i=0; i<kahouList.Count; i++){
				int kahouId = int.Parse(kahouList [i]);
                if (kahou.getKahouRank (kahouType, kahouId) == kahouRank){
					targetKahouList.Add (kahouId.ToString());	
				}
			}
			if (targetKahouList.Count != 0) {
				int rdmId =  UnityEngine.Random.Range(0,targetKahouList.Count);
				targetKahouId = int.Parse(targetKahouList[rdmId]);
			}

		}else{
			//chishikisyo
			string availableChishikisyo = PlayerPrefs.GetString("availableChishikisyo");
			List<string> kahouList = new List<string> ();
			if (availableChishikisyo != null && availableChishikisyo != "") {
				if (availableChishikisyo.Contains (",")) {
					kahouList = new List<string> (availableChishikisyo.Split (delimiterChars));
				} else {
					kahouList.Add (availableChishikisyo);
				}
			}

			List<string> targetKahouList = new List<string> ();
			for(int i=0; i<kahouList.Count; i++){
				int kahouId = int.Parse(kahouList [i]);
				if(kahou.getKahouRank (kahouType, kahouId) == kahouRank){
					targetKahouList.Add (kahouId.ToString());	
				}
			}
			if (targetKahouList.Count != 0) {
				int rdmId =  UnityEngine.Random.Range(0,targetKahouList.Count);
				targetKahouId = int.Parse(targetKahouList[rdmId]);
			}
		}

		return targetKahouId;
	}
}
