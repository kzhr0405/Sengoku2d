using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System.Collections.Generic;

public class DoShisya : MonoBehaviour {

	public GameObject slot;

	//Select
	public int busyoDamaQty;
	public bool cyouteiFlg;
	public bool syouninFlg;


	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		Message msgScript = new Message ();
        int langId = PlayerPrefs.GetInt("langId");
        ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();

		if (name == "YesButton") {
			if (script.shisyaId == 1) {
				//doumei
				int money = PlayerPrefs.GetInt("money");
				if (money >= 3000) {
					int hyourou = PlayerPrefs.GetInt ("hyourou");
					if (hyourou >= 10) {
						audioSources [3].Play ();
						string msg = registerDoumei (slot);
						msgScript.makeMessage (msg);
						AfterShisyaProcess (slot, true);
						Destroy (slot);
					}else {
						audioSources [4].Play ();
						msgScript.makeMessage (msgScript.getMessage(68, langId));	
					}
				} else {
					audioSources [4].Play ();
                    msgScript.makeMessage(msgScript.getMessage(67, langId));
                }
			} else if (script.shisyaId == 3) {
				//engun
				int money = PlayerPrefs.GetInt("money");
				if (money >= 3000) {
					int hyourou = PlayerPrefs.GetInt ("hyourou");
					if (hyourou >= 10) {
						audioSources [3].Play ();
						string msg = registerEngun (slot);
						msgScript.makeMessage (msg);
						AfterShisyaProcess (slot, true);
						Destroy (slot);
					} else {
						audioSources [4].Play ();
						msgScript.makeMessage (msgScript.getMessage(69, langId));	
					}
				} else {
					audioSources [4].Play ();
					msgScript.makeMessage (msgScript.getMessage(70, langId));	
				}
			} else if (script.shisyaId == 4) {
				//doukatsu
				int money = PlayerPrefs.GetInt("money");
				if (script.moneyNo != 0) {
					//money
					if (money >= script.moneyNo) {
						audioSources [3].Play ();
						string msg = registerDoukatsu (slot,true);
						msgScript.makeMessage (msg);
						AfterShisyaProcess (slot, true);
						Destroy (slot);
					} else {
						audioSources [4].Play ();
						msgScript.makeMessage (msgScript.getMessage(71, langId));	
					}
				} else {
					//item
					audioSources [3].Play ();
					string msg = registerDoukatsu (slot,false);
					msgScript.makeMessage (msg);
					AfterShisyaProcess (slot, true);
					Destroy (slot);
				}



			} else if (script.shisyaId == 5) {
				//koueki
				int money = PlayerPrefs.GetInt("money");
				if (money >= slot.GetComponent<ShisyaSelect>().moneyNo) {
					audioSources [3].Play ();
					string msg = registerKoueki (slot);
					msgScript.makeMessage (msg);
					AfterShisyaProcess (slot, true);
					Destroy (slot);
					
				} else {
					audioSources [4].Play ();
					msgScript.makeMessage (msgScript.getMessage(72, langId));	
				}


			} else if (script.shisyaId == 6) {
				//doumei haki
				audioSources [3].Play ();
				string msg = registerDoumeiHaki(slot);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, true);
				Destroy (slot);

			} else if (script.shisyaId == 7) {
				//mitsugimono
				audioSources [3].Play ();
				string msg = registerMitsugimono(slot);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, true);
				Destroy (slot);

			} else if (script.shisyaId == 8) {
				//kyouhaku
				int busyoDama = PlayerPrefs.GetInt("busyoDama");
				if (busyoDama < 100) {
					audioSources [4].Play ();
					msgScript.makeMessage (msgScript.getMessage(73, langId));	
				} else {
					audioSources [3].Play ();
					string msg = registerKyouhaku(busyoDama,slot);
					msgScript.makeMessage (msg);
					AfterShisyaProcess (slot, true);
					Destroy (slot);
				}
			} else if (script.shisyaId == 9) {
				//cyakai
				int money = PlayerPrefs.GetInt("money");
				if (money >= 3000) {
					int hyourou = PlayerPrefs.GetInt ("hyourou");
					if (hyourou >= 10) {
						audioSources [3].Play ();
						string msg = registerCyakai (slot);
						msgScript.makeMessage (msg);
						AfterShisyaProcess (slot, true);
						Destroy (slot);
					} else {
						audioSources [4].Play ();
						msgScript.makeMessage (msgScript.getMessage(74, langId));	
					}
				} else {
					audioSources [4].Play ();
					msgScript.makeMessage (msgScript.getMessage(75, langId));	
				}

			} else if (script.shisyaId == 10) {
				//cyusai
				int hyourou = PlayerPrefs.GetInt ("hyourou");
				if (hyourou >= 10) {
					audioSources [3].Play ();
					string msg = registerCyouteiCyusai (slot,false);
					msgScript.makeMessage (msg);
					AfterShisyaProcess (slot, true);
					Destroy (slot);
				}else {
					audioSources [4].Play ();
					msgScript.makeMessage (msgScript.getMessage(76, langId));
				}
			} else if (script.shisyaId == 11) {
				//Toubatsurei
				int money = PlayerPrefs.GetInt("money");
				if (money >= 3000) {
					int hyourou = PlayerPrefs.GetInt ("hyourou");
					if (hyourou >= 10) {
						audioSources [3].Play ();
						string msg = registerToubatsurei (slot);
						msgScript.makeMessage (msg);
						AfterShisyaProcess (slot, true);
						Destroy (slot);
					} else {
						audioSources [4].Play ();
						msgScript.makeMessage (msgScript.getMessage(77, langId));	
					}
				} else {
					audioSources [4].Play ();
					msgScript.makeMessage (msgScript.getMessage(78, langId));	
				}


			} else if (script.shisyaId == 12) {
				
				int money = PlayerPrefs.GetInt("money");
				if (money >= 3000) {
					int hyourou = PlayerPrefs.GetInt ("hyourou");
					if (hyourou >= 10) {
						audioSources [3].Play ();
						string msg = registerBoueirei (slot);
						msgScript.makeMessage (msg);
						AfterShisyaProcess (slot, true);
						Destroy (slot);
					} else {
						audioSources [4].Play ();
						msgScript.makeMessage (msgScript.getMessage(79, langId));	
					}
				} else {
					audioSources [4].Play ();
					msgScript.makeMessage (msgScript.getMessage(80, langId));	
				}


			
			} else if (script.shisyaId == 13) {
				//Syougun Syounin
				int hyourou = PlayerPrefs.GetInt ("hyourou");
				if (hyourou >= 50) {
					audioSources [3].Play ();
					string msg = registerSyogunApproval (slot,langId);
					msgScript.makeMessage (msg);
					AfterShisyaProcess (slot, true);
					Destroy (slot);
				}else {
					audioSources [4].Play ();
					msgScript.makeMessage (msgScript.getMessage(81, langId));
				}


			} else if (script.shisyaId == 14) {
				//Kizoku musin
				int money = PlayerPrefs.GetInt("money");
				if (money >= 3000) {
					int hyourou = PlayerPrefs.GetInt ("hyourou");
					if (hyourou >= 10) {
						audioSources [3].Play ();
						string msg = registerMusin (slot,false);
						msgScript.makeMessage (msg);
						AfterShisyaProcess (slot, true);
						Destroy (slot);
					} else {
						audioSources [4].Play ();
						msgScript.makeMessage (msgScript.getMessage(82, langId));	
					}
				} else {
					audioSources [4].Play ();
					msgScript.makeMessage (msgScript.getMessage(83,langId));	
				}
			} else if (script.shisyaId == 15) {
				//Kizoku musin
				int money = PlayerPrefs.GetInt("money");
				if (money >= 3000) {
					int hyourou = PlayerPrefs.GetInt ("hyourou");
					if (hyourou >= 10) {
						audioSources [3].Play ();
						string msg = registerMusin (slot,true);
						msgScript.makeMessage (msg);
						AfterShisyaProcess (slot, true);
						Destroy (slot);
					} else {
						audioSources [4].Play ();
						msgScript.makeMessage (msgScript.getMessage(82, langId));	
					}
				} else {
					audioSources [4].Play ();
					msgScript.makeMessage (msgScript.getMessage(83,langId));	
				}

			} else if (script.shisyaId == 16) {
				//Kyucyu Gyouji
				int myBusyoDama = PlayerPrefs.GetInt("busyoDama");
				if (myBusyoDama >= busyoDamaQty) {

					audioSources [3].Play ();
					string msg = registerSelection (slot,true,myBusyoDama);
					msgScript.makeMessage (msg);
					AfterShisyaProcess (slot, true);
					Destroy (slot);

				} else {
					audioSources [4].Play ();
					msgScript.makeMessage (msgScript.getMessage(2, langId));	
				}

			} else if (script.shisyaId == 17) {
				//Cyoutei
				int hyourou = PlayerPrefs.GetInt ("hyourou");
				if (hyourou >= 10) {
					audioSources [3].Play ();
					string msg = registerCyouteiCyusai (slot,true);
					msgScript.makeMessage (msg);
					AfterShisyaProcess (slot, true);
					Destroy (slot);
				}else {
					audioSources [4].Play ();
					msgScript.makeMessage (msgScript.getMessage(84, langId));	
				}

			} else if (script.shisyaId == 18) {
				//Kyucyu
				int myBusyoDama = PlayerPrefs.GetInt("busyoDama");
				if (myBusyoDama >= busyoDamaQty) {

					audioSources [3].Play ();
					string msg = registerSelection (slot,false,myBusyoDama);
					msgScript.makeMessage (msg);
					AfterShisyaProcess (slot, true);
					Destroy (slot);

				} else {
					audioSources [4].Play ();
					msgScript.makeMessage (msgScript.getMessage(2, langId));	
				}
			} else if (script.shisyaId == 19) {
				//Nanban
				int money = PlayerPrefs.GetInt("money");
				if (money >= slot.GetComponent<ShisyaSelect>().moneyNo) {
					audioSources [3].Play ();
					string msg = registerKoueki (slot);
					msgScript.makeMessage (msg);
					AfterShisyaProcess (slot, true);
					Destroy (slot);

				} else {
					audioSources [4].Play ();
					msgScript.makeMessage (msgScript.getMessage(72, langId));	
				}
			} else if (script.shisyaId == 20) {
				//Tsuji Seppou
				int hyourou = PlayerPrefs.GetInt ("hyourou");
				if (hyourou >= 30) {
					audioSources [3].Play ();
					string msg = registerTsujiSeppou (slot);
					msgScript.makeMessage (msg);
					AfterShisyaProcess (slot, true);
					Destroy (slot);
				}else {
					audioSources [4].Play ();
					msgScript.makeMessage (msgScript.getMessage(7, langId));	
				}


			} else if (script.shisyaId == 21) {
				//Jikiso
				int money = PlayerPrefs.GetInt("money");
				if (money >= 3000) {
					int hyourou = PlayerPrefs.GetInt ("hyourou");
					if (hyourou >= 30) {
						audioSources [3].Play ();
						string msg = registerJikiso (slot);
						msgScript.makeMessage (msg);
						AfterShisyaProcess (slot, true);
						Destroy (slot);

                    } else {
						audioSources [4].Play ();
						msgScript.makeMessage (msgScript.getMessage(7, langId));	
					}
				} else {
					audioSources [4].Play ();
					msgScript.makeMessage (msgScript.getMessage(6, langId));	
				}


			} else if (script.shisyaId == 22) {
                audioSources[3].Play();
                string msg = surrenderText(slot);
                msgScript.makeMessage(msg);
                AfterShisyaProcess(slot, true);
                Destroy(slot);
            }

        } else {
			//No button
			if (script.shisyaId == 1) {
				//reject doumei
				audioSources [4].Play ();
				string msg = rejectDoumei (slot);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, false);
				Destroy (slot);

			} else if (script.shisyaId == 3) {
				//reject engun
				audioSources [4].Play ();
				string msg = rejectEngun (slot);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, false);
				Destroy (slot);
			
			} else if (script.shisyaId == 4) {
				//reject doukatsu
				audioSources [4].Play ();
				string msg = rejectDoukatsu (slot);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, false);
				Destroy (slot);

			} else if (script.shisyaId == 5) {
				//reject koueki
				audioSources [4].Play ();
				string msg = rejectKoueki (slot);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, false);
				Destroy (slot);

			} else if (script.shisyaId == 6) {
				//Doumei Haki
				//Nothing to do
			} else if (script.shisyaId == 7) {
				//reject mitsugimono
				audioSources [4].Play ();
				string msg = rejectMitsugimono (slot);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, false);
				Destroy (slot);

			} else if (script.shisyaId == 8) {
				//kyouhaku
				audioSources [4].Play ();
				string msg = rejectKyouhaku(slot);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, false);
				Destroy (slot);

			} else if (script.shisyaId == 9) {
				//Reject cyakai
				audioSources [4].Play ();
				string msg = rejectCyakai (slot);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, false);
				Destroy (slot);

			} else if (script.shisyaId == 10) {
				//Reject cyusai
				audioSources [4].Play ();
				string msg = rejectCyouteiCyusai (slot,false);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, false);
				Destroy (slot);

			} else if (script.shisyaId == 11) {
				
				audioSources [4].Play ();
				string msg = rejecToubatsurei (slot);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, false);
				Destroy (slot);
			
			} else if (script.shisyaId == 12) {

				audioSources [4].Play ();
				string msg = rejectBoueirei (slot);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, false);
				Destroy (slot);
			
			} else if (script.shisyaId == 13) {
				//Reject Syougun Syounin
				audioSources [4].Play ();
				string msg = rejectSyogunApproval (slot,langId);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, false);
				Destroy (slot);

			} else if (script.shisyaId == 14) {
				//Reject Syogun musin
				audioSources [4].Play ();
				string msg = rejectMusin (slot, false);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, false);
				Destroy (slot);


			} else if (script.shisyaId == 15) {
				//Reject Kizoku musin
				audioSources [4].Play ();
				string msg = rejectMusin (slot, true);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, false);
				Destroy (slot);

			} else if (script.shisyaId == 16) {
				//Reject Kyucyu Gyouji
				audioSources [4].Play ();
				string msg = rejectKyucyuGyouji (slot);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, false);
				Destroy (slot);

			} else if (script.shisyaId == 17) {
				//Reject Cyoutei
				audioSources [4].Play ();
				string msg = rejectCyouteiCyusai (slot,true);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, false);
				Destroy (slot);

			} else if (script.shisyaId == 18) {
				//Reject Koueki
				audioSources [4].Play ();
				string msg = rejectKoueki (slot);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, false);
				Destroy (slot);

			} else if (script.shisyaId == 19) {
				//Reject Koueki
				audioSources [4].Play ();
				string msg = rejectKoueki (slot);
				msgScript.makeMessage (msg);
				AfterShisyaProcess (slot, false);
				Destroy (slot);

			} else if (script.shisyaId == 20) {
				//Reject Tsuji Seppou
				audioSources [4].Play ();
                msgScript.makeMessage(msgScript.getMessage(85,langId));
                PlayerPrefs.DeleteKey ("shisya20");

				AfterShisyaProcess (slot, false);
				Destroy (slot);


			} else if (script.shisyaId == 21) {
				//Reject Jikiso
				audioSources [4].Play ();
                int kokuninReject = PlayerPrefs.GetInt("kokuninReject");
                kokuninReject = kokuninReject + 1;
                if (kokuninReject > 10) kokuninReject = 10;
                PlayerPrefs.SetInt("kokuninReject", kokuninReject);

                string text = "";
                
                if (kokuninReject < 2) {
                    text = msgScript.getMessage(220,langId);
                }else if (2 <= kokuninReject && kokuninReject < 6) {
                    text = msgScript.getMessage(221, langId);
                }else if (6 <= kokuninReject) {
                    text = msgScript.getMessage(222, langId);
                }
                
                
                msgScript.makeMessage (msgScript.getMessage(86, langId) + text);
				PlayerPrefs.DeleteKey ("shisya21");

				AfterShisyaProcess (slot, false);
				Destroy (slot);
			}
		}
		PlayerPrefs.Flush();

		ShisyaScene shisya = new ShisyaScene ();
		shisya.viewCurrentValue ();
	}

	public void AfterShisyaProcess(GameObject slot, bool yesFlg){
		//disable button
		Color NGColor = new Color (118f / 255f, 118f / 255f, 45f / 255f, 255f / 255f);

		GameObject ysBtn = GameObject.Find ("YesButton").gameObject;
		ysBtn.GetComponent<Button>().enabled = false;
		ysBtn.GetComponent<Image> ().color = NGColor;
		ysBtn.transform.Find("Text").GetComponent<Text> ().color = NGColor;

		GameObject noBtn = GameObject.Find ("NoButton").gameObject;
		noBtn.GetComponent<Button>().enabled = false;
		noBtn.GetComponent<Image> ().color = NGColor;
		noBtn.transform.Find("Text").GetComponent<Text> ().color = NGColor;

		//comment
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		Shisya shisya = new Shisya ();
		string serihu = "";
		if (yesFlg) {
			serihu = shisya.getOKSerihu (script.shisyaId);
		} else {
			serihu = shisya.getNGSerihu (script.shisyaId);
		}
		GameObject.Find ("Comment").transform.Find ("Text").GetComponent<Text> ().text = serihu;

	}

	public void reduceMoneyHyoruou(){
		int money = PlayerPrefs.GetInt ("money");
		money = money - 3000;
		int hyourou = PlayerPrefs.GetInt ("hyourou");
		hyourou = hyourou - 10;
		PlayerPrefs.SetInt ("money",money);
		PlayerPrefs.SetInt ("hyourou",hyourou);

	}


	public string registerDoumei(GameObject slot){
		reduceMoneyHyoruou ();

		string msg = "";
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();

		//Delete Key
		string tmp = "shisya1";	
		string shisyaString = PlayerPrefs.GetString (tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}
		shisyaList.Remove (script.srcDaimyoId.ToString());
		string newValue = "";
		for (int i = 0; i < shisyaList.Count; i++) {
			if (i == 0) {
				newValue = shisyaList [i];
			} else {
				newValue = newValue + "," + shisyaList [i];
			}
		}
		PlayerPrefs.SetString (tmp,newValue);

		//Track
		int TrackDoumeiNo = PlayerPrefs.GetInt("TrackDoumeiNo",0);
		TrackDoumeiNo = TrackDoumeiNo + 1;
		PlayerPrefs.SetInt("TrackDoumeiNo",TrackDoumeiNo);

		//Main Process
		int daimyoId = script.srcDaimyoId;

		string doumei = PlayerPrefs.GetString ("doumei");
		if (doumei == null || doumei == "") {
			doumei = daimyoId.ToString ();
		} else {
			doumei = doumei + "," + daimyoId.ToString ();
		}

		int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
		string cpuDoumeiTemp = "doumei" + daimyoId.ToString ();
		string cpuDoumei = PlayerPrefs.GetString (cpuDoumeiTemp);
		if (cpuDoumei != null & cpuDoumei != "") {
			cpuDoumei = cpuDoumei + "," + myDaimyo.ToString ();
		} else {
			cpuDoumei = myDaimyo.ToString ();
		}
		PlayerPrefs.SetString (cpuDoumeiTemp, cpuDoumei);
		PlayerPrefs.SetString ("doumei", doumei);

		//Quest
		PlayerPrefs.SetBool ("questSpecialFlg2", true);


		//Msg
		string daimyoName = script.srcDaimyoName;

        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msg = "You formed alliance with "+daimyoName + ".\n We can choose some strategic options.";
        }else if(langId==3) {
            msg = "和" + daimyoName + "大人缔结同盟了，战略视野更开阔了。";
        }
        else {
            msg = daimyoName + "殿と同盟締結致しました。\n戦略の幅が拡がりますな。";
        }
		return msg;
	}


	public string registerKyouhaku(int busyoDama, GameObject slot){

		string msg = "";
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
        Message Message = new Message();
        int langId = PlayerPrefs.GetInt("langId");
        string key  = script.key;

		//Delete Key
		string tmp = "shisya8";	
		string shisyaString = PlayerPrefs.GetString (tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}

		List<string> shisyaNewList = new List<string> ();
		for (int j = 0; j < shisyaList.Count; j++) {
			string shisyaParam = shisyaList [j];

			List<string> shisyaParamList = new List<string> ();
			char[] delimiterChars2 = { ':' };
			if (shisyaParam.Contains (":")) {
				shisyaParamList = new List<string> (shisyaParam.Split (delimiterChars2));
			} else {
				shisyaParamList.Add (shisyaParam);
			}
			if (shisyaParamList [2] != key) {
				shisyaNewList.Add (shisyaParam);
			}
		}

		string newValue = "";
		for (int i = 0; i < shisyaNewList.Count; i++) {
			if (i == 0) {
				newValue = shisyaNewList [i];
			} else {
				newValue = newValue + "," + shisyaNewList [i];
			}
		}
		PlayerPrefs.SetString (tmp,newValue);


		if (PlayerPrefs.HasKey (key)) {
            
            msg = Message.getMessage(223, langId);
			busyoDama = busyoDama - 100;
			PlayerPrefs.SetInt ("busyoDama", busyoDama);

			PlayerPrefs.DeleteKey (key);

			//Delete Key History
			string keyHistory = PlayerPrefs.GetString ("keyHistory");
			List<string> keyHistoryList = new List<string> ();
			if (keyHistory != null && keyHistory != "") {
				if (keyHistory.Contains (",")) {
					keyHistoryList = new List<string> (keyHistory.Split (delimiterChars));
				} else {
					keyHistoryList.Add (keyHistory);
				}
			}
			keyHistoryList.Remove (key);
			string newKeyHistory = "";
			for (int i = 0; i < keyHistoryList.Count; i++) {
				if (i == 0) {
					newKeyHistory = keyHistoryList [i];
				} else {
					newKeyHistory = newKeyHistory + "," + keyHistoryList [i];
				}
			}
			PlayerPrefs.SetString ("keyHistory", newKeyHistory);
		} else {
            msg = Message.getMessage(224, langId);            
		}
		return msg;
	}


	public string registerEngun(GameObject slot){
        string msg = "";
        Message Message = new Message();
        int langId = PlayerPrefs.GetInt("langId");
        reduceMoneyHyoruou ();
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		string key  = script.key;

		//Delete Key
		string tmp = "shisya3";	
		string shisyaString = PlayerPrefs.GetString (tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}

		List<string> shisyaNewList = new List<string> ();
		for (int j = 0; j < shisyaList.Count; j++) {
			string shisyaParam = shisyaList [j];

			List<string> shisyaParamList = new List<string> ();
			char[] delimiterChars2 = { ':' };
			if (shisyaParam.Contains (":")) {
				shisyaParamList = new List<string> (shisyaParam.Split (delimiterChars2));
			} else {
				shisyaParamList.Add (shisyaParam);
			}
			if (shisyaParamList [3] != key) {
				shisyaNewList.Add (shisyaParam);
			}
		}

		string newValue = "";
		for (int i = 0; i < shisyaNewList.Count; i++) {
			if (i == 0) {
				newValue = shisyaNewList [i];
			} else {
				newValue = newValue + "," + shisyaNewList [i];
			}
		}
		PlayerPrefs.SetString (tmp,newValue);


		string keyValue = PlayerPrefs.GetString (key);
        if (PlayerPrefs.HasKey(key)) {
            List<string> keyValueList = new List<string> ();
		    keyValueList = new List<string> (keyValue.Split (delimiterChars));

		    int myJinkeiBusyo = PlayerPrefs.GetInt("jinkeiBusyoQty");
		    int myTotalHei = PlayerPrefs.GetInt ("jinkeiHeiryoku");
		    int myUnitHei = myTotalHei / myJinkeiBusyo;
		    int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");

		    if (keyValueList [9] == "true") {
			    //engun exist
			    string tmp1 = keyValueList[10] + ":"+myDaimyo;
			    string tmp2 = keyValueList [11] + ":" + myUnitHei;

			    keyValue = keyValueList[0] + "," + keyValueList[1] + "," + keyValueList[2] + "," + keyValueList[3] + "," + keyValueList[4] + "," + keyValueList[5] + "," + keyValueList[6] + "," + keyValueList[7] + "," + keyValueList[8] + "," + keyValueList[9] + "," + tmp1 + "," + tmp2 + "," + "";
		    } else {
			    keyValue = keyValueList[0] + "," + keyValueList[1] + "," + keyValueList[2] + "," + keyValueList[3] + "," + keyValueList[4] + "," + keyValueList[5] + "," + keyValueList[6] + "," + keyValueList[7] + "," + keyValueList[8] + "," + true + "," + myDaimyo + "," + myUnitHei + "," + "";
		    }
		    PlayerPrefs.SetString (key, keyValue);

		    string engunDaimyoName = script.srcDaimyoName;

		    //yukoudo up
		    string tempGaikou = "gaikou" + script.srcDaimyoId;
		    int nowYukoudo = 0;
		    if (PlayerPrefs.HasKey (tempGaikou)) {
			    nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
		    } else {
			    nowYukoudo = 50;
		    }
		    int addPoint = UnityEngine.Random.Range (10, 30);
		    int newyukoudo = nowYukoudo + addPoint;
		    if (newyukoudo > 100) {
			    newyukoudo = 100;
		    }
		    PlayerPrefs.SetInt (tempGaikou, newyukoudo);
            if (langId == 2) {
                msg = "We sent reinforcement " + myUnitHei + " soldiers to support "+ engunDaimyoName + ". \n Frienship increased " + addPoint + " point.";
            }else if (langId == 3) {
                msg = engunDaimyoName + "派出了援军" + myUnitHei + "人，友好度上升了" + addPoint + "。";
            } else {
                msg = engunDaimyoName + "救援の兵" + myUnitHei + "を送りました。\n友好度が" + addPoint + "上昇します。";
            }
        }else {           
            msg = Message.getMessage(225, langId);           
        }

		
		return msg;
	}



	public string registerMusin (GameObject slot, bool cyouteiFlg){
		reduceMoneyHyoruou ();
		string msg = "";
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();

		string tmp = "";

		if (cyouteiFlg) {
			tmp = "shisya15";
			int nowPoint = PlayerPrefs.GetInt ("cyouteiPoint");
			int addPoint = UnityEngine.Random.Range (5, 20);
			int newPoint = nowPoint + addPoint;
			if (newPoint > 100) {
				newPoint = 100;
			}
			PlayerPrefs.SetInt ("cyouteiPoint", newPoint);
            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                msg = "We gave money and stamina to them. Court point increased " + addPoint + " point.";
            }
            else if (langId == 3) {
                msg = "响应贵族的无心，进献了金钱和兵粮，朝廷贡献度上升了" + addPoint + "。";
            }
            else {
                msg = "貴族の無心に応え、金と兵糧を提供しました。朝廷貢献度が" + addPoint + "上昇します。";
            }
		} else {
			tmp = "shisya14";
			int syogunDaimyoId = PlayerPrefs.GetInt("syogunDaimyoId");

			string tempGaikou = "gaikou" + syogunDaimyoId;
			int nowYukoudo = 0;
			if (PlayerPrefs.HasKey (tempGaikou)) {
				nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
			} else {
				nowYukoudo = 50;
			}
			int addPoint = UnityEngine.Random.Range (5, 20);
			int newyukoudo = nowYukoudo + addPoint;
			if (newyukoudo > 100) {
				newyukoudo = 100;
			}
			PlayerPrefs.SetInt (tempGaikou, newyukoudo);
            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                msg = "We gave money and stamina to syogun. Friendship increased " + addPoint + " point.";
            }
            else if (langId == 3) {
                msg = "响应将军的无心，进献了金钱和兵粮，友好度上升了" + addPoint + "。";
            }
            else {
                msg = "将軍の無心に応え、金と兵糧を提供しました。友好度が" + addPoint + "上昇します。";
            }
		}

		//Main Process
		PlayerPrefs.DeleteKey (tmp);

		return msg;

	}


	public string registerCyouteiCyusai (GameObject slot, bool cyouteiFlg){
		int hyourou = PlayerPrefs.GetInt ("hyourou");
		hyourou = hyourou - 10;
		PlayerPrefs.SetInt ("hyourou",hyourou);
		string msg = "";
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		int srcDaimyoId = script.srcDaimyoId;
		string srcDaimyoName = script.srcDaimyoName;
        int langId = PlayerPrefs.GetInt("langId");

        //Delete Key
        string tmp = "";

		if (cyouteiFlg) {
			tmp = "shisya17";
            if (langId == 2) {
                msg = "Became friendly with " + srcDaimyoName + " by royal court request.";
            }
            else if (langId == 3) {
                msg = "响应贵族的调停，和" + srcDaimyoName + "关系加深了。";
            }
            else {
                msg = "貴族の調停に応え、" + srcDaimyoName + "と誼を深めました。";
            }
		} else {
			tmp = "shisya10";
            if (langId == 2) {
                msg = "Became friendly with " + srcDaimyoName + " by syogun request.";
            }
            else if (langId == 3) {
                msg = "响应将军的仲裁，和" + srcDaimyoName + "关系加深了。";
            }
            else {
                msg = "将軍の仲裁に応え、" + srcDaimyoName + "と誼を深めました。";
            }
		}

		string tempGaikou = "gaikou" + srcDaimyoId;
		int nowYukoudo = 0;
		if (PlayerPrefs.HasKey (tempGaikou)) {
			nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
		} else {
			nowYukoudo = 50;
		}
		int addPoint = UnityEngine.Random.Range (5, 15);
		int newyukoudo = nowYukoudo + addPoint;
		if (newyukoudo > 100) {
			newyukoudo = 100;
		}
		PlayerPrefs.SetInt (tempGaikou, newyukoudo);


		string shisyaString = PlayerPrefs.GetString (tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}
		shisyaList.Remove (srcDaimyoId.ToString());
		string newValue = "";
		for (int i = 0; i < shisyaList.Count; i++) {
			if (i == 0) {
				newValue = shisyaList [i];
			} else {
				newValue = newValue + "," + shisyaList [i];
			}
		}
		PlayerPrefs.SetString (tmp,newValue);

        if (langId == 2) {
            msg = msg + "Friendship increased " + addPoint  + " point.";
        }
        else if (langId == 3) {
            msg = msg + "友好度上升了" + addPoint + "。";
        }
        else {
            msg = msg + "友好度が" + addPoint + "上昇します。";
        }
		return msg;
	}


	public string registerSelection (GameObject slot, bool cyouteiFlg, int myBusyoDama){

		string msg = "";
		myBusyoDama = myBusyoDama - busyoDamaQty;
		PlayerPrefs.SetInt ("busyoDama" , myBusyoDama);

		if (cyouteiFlg) {
			string nowQty = PlayerPrefs.GetString ("cyoutei");
			List<string> nowQtyList = new List<string> ();
			char[] delimiterChars = { ',' };
			nowQtyList = new List<string> (nowQty.Split (delimiterChars));
			string newQty = "";
			if (busyoDamaQty == 100) {
				int newUnitQty = int.Parse (nowQtyList [0]);
				newUnitQty = newUnitQty + 1;
				newQty = newUnitQty + "," + nowQtyList [1] + "," + nowQtyList [2];
			} else if (busyoDamaQty == 200) {
				int newUnitQty = int.Parse (nowQtyList [1]);
				newUnitQty = newUnitQty + 1;
				newQty = nowQtyList [0] + "," + newUnitQty + "," + nowQtyList [2];
			} else if (busyoDamaQty == 500) {
				int newUnitQty = int.Parse (nowQtyList [2]);
				newUnitQty = newUnitQty + 1;
				newQty = nowQtyList [0] + "," + nowQtyList [1] + "," + newUnitQty;
			}
			PlayerPrefs.SetString ("cyoutei",newQty);

			int nowPoint = PlayerPrefs.GetInt ("cyouteiPoint");
			int addPoint = UnityEngine.Random.Range (5, 20);
			int newPoint = nowPoint + addPoint;
			if (newPoint > 100) {
				newPoint = 100;
			}
			PlayerPrefs.SetInt ("cyouteiPoint", newPoint);

			PlayerPrefs.DeleteKey ("shisya16");

            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                msg = "Paied " + busyoDamaQty + " stone. \n Royal court point increased "+addPoint+" point and got letter of introduction.";
            }
            else if (langId == 3) {
                msg = "为进行宫中形势，进献武将珠" + busyoDamaQty + "个，朝廷贡献度上升了" + addPoint + "，获得宫中介绍状。";
            } else {
                msg = "宮中行事のため武将珠" + busyoDamaQty + "を差し出しました。\n朝廷貢献度が" + addPoint + "上昇し、宮中への紹介状を入手。";
            }
		} else {
			string nowQty = PlayerPrefs.GetString ("koueki");
			List<string> nowQtyList = new List<string> ();
			char[] delimiterChars = { ',' };
			nowQtyList = new List<string> (nowQty.Split (delimiterChars));

			string newQty = "";
			if (busyoDamaQty == 100) {
				int newUnitQty = int.Parse (nowQtyList [0]);
				newUnitQty = newUnitQty + 1;
				newQty = newUnitQty + "," + nowQtyList [1] + "," + nowQtyList [2];
			} else if (busyoDamaQty == 200) {
				int newUnitQty = int.Parse (nowQtyList [1]);
				newUnitQty = newUnitQty + 1;
				newQty = nowQtyList [0] + "," + newUnitQty + "," + nowQtyList [2];
			} else if (busyoDamaQty == 500) {
				int newUnitQty = int.Parse (nowQtyList [2]);
				newUnitQty = newUnitQty + 1;
				newQty = nowQtyList [0] + "," + nowQtyList [1] + "," + newUnitQty;
			}
			PlayerPrefs.SetString ("koueki",newQty);
			PlayerPrefs.DeleteKey ("shisya18");

            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                msg = "Paied " + busyoDamaQty + " stone. \n and you got letter of introduction.";
            }
            else if (langId == 3) {
                msg = "将" + busyoDamaQty + "个武将珠作为介绍费交给商人了，获得豪商介绍状。";
            }
            else {
                msg = "武将珠" + busyoDamaQty + "個を口利き料として商人に差し出しました。豪商への紹介状を入手。";
            }

        }


		return msg;
	}


	public string registerMitsugimono(GameObject slot){

		string msg = "";
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		int srcDaimyoId = script.srcDaimyoId;
		string srcDaimyoName = script.srcDaimyoName;
		int gotMoney = script.moneyNo;

		int nowMoney = PlayerPrefs.GetInt("money");
		nowMoney = nowMoney + gotMoney;
        if (nowMoney < 0) {
            nowMoney = int.MaxValue;
        }
        PlayerPrefs.SetInt("money",nowMoney);

		//Delete Key
		string tmp = "shisya7";
		string tempGaikou = "gaikou" + srcDaimyoId;
		int nowYukoudo = 0;
		if (PlayerPrefs.HasKey (tempGaikou)) {
			nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
		} else {
			nowYukoudo = 50;
		}
		int addPoint = UnityEngine.Random.Range (5, 15);
		int newyukoudo = nowYukoudo + addPoint;
		if (newyukoudo > 100) {
			newyukoudo = 100;
		}
		PlayerPrefs.SetInt (tempGaikou, newyukoudo);


		string shisyaString = PlayerPrefs.GetString (tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}
		shisyaList.Remove (srcDaimyoId.ToString());
		string newValue = "";
		for (int i = 0; i < shisyaList.Count; i++) {
			if (i == 0) {
				newValue = shisyaList [i];
			} else {
				newValue = newValue + "," + shisyaList [i];
			}
		}
		PlayerPrefs.SetString (tmp,newValue);

        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msg =  srcDaimyoName + " gave you money " + gotMoney +". Friendship increased " + addPoint  + "point.";
        }
        else if (langId == 3) {
            msg = srcDaimyoName + "进献贡物，收取金钱" + gotMoney + "个，友好度上升了" + addPoint + "。";
        }
        else {
            msg = srcDaimyoName + "より貢物、金" + gotMoney + "を受け取りました。友好度が" + addPoint + "上昇します。";
        }
		return msg;

	}


	public string registerDoukatsu (GameObject slot, bool moneyFlg){
		string msg = "";
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		int srcDaimyoId = script.srcDaimyoId;
		string srcDaimyoName = script.srcDaimyoName;

		if (moneyFlg) {
			//money
			int money = PlayerPrefs.GetInt ("money");
			int newMoney = money - script.moneyNo;
			PlayerPrefs.SetInt ("money",newMoney);

		} else {
			//item
			string kahouCd = script.itemCd;
			string kahouId = script.itemId;
			string itemDataCd = script.itemDataCd;

			DoSell kahouSell = new DoSell ();
			kahouSell.reduceKahou (itemDataCd,int.Parse(kahouId),1);

		}

		//yukoudo
		string tempGaikou = "gaikou" + srcDaimyoId;
		int nowYukoudo = 0;
		if (PlayerPrefs.HasKey (tempGaikou)) {
			nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
		} else {
			nowYukoudo = 50;
		}
		int addPoint = UnityEngine.Random.Range (5, 15);
		int newyukoudo = nowYukoudo + addPoint;
		if (newyukoudo > 100) {
			newyukoudo = 100;
		}
		PlayerPrefs.SetInt (tempGaikou, newyukoudo);
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msg = "You agreed with "+srcDaimyoName + "'s threat. Friendship increased " + addPoint + " point.";
        }
        else if (langId == 3) {
            msg = "回应了" + srcDaimyoName + "的恫吓，友好度上升了" + addPoint + "。";
        }
        else {
            msg = srcDaimyoName + "の恫喝に応じました。友好度が" + addPoint + "上昇します。";
        }

		//Delete Key
		string tmp = "shisya4";
		string shisyaString = PlayerPrefs.GetString (tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}
		shisyaList.Remove (srcDaimyoId.ToString());
		string newValue = "";
		for (int i = 0; i < shisyaList.Count; i++) {
			if (i == 0) {
				newValue = shisyaList [i];
			} else {
				newValue = newValue + "," + shisyaList [i];
			}
		}
		PlayerPrefs.SetString (tmp,newValue);
		
		return msg;
	}


	public string registerTsujiSeppou(GameObject slot){
		string msg = "";

		int hyourou = PlayerPrefs.GetInt ("hyourou");
		hyourou = hyourou - 30;
		PlayerPrefs.SetInt ("hyourou",hyourou);

		Item item = new Item ();
		int rdmType = UnityEngine.Random.Range (1, 5); //1-4
		int rdmRank = UnityEngine.Random.Range (1, 4); //1-3
		int addQty = UnityEngine.Random.Range (1, 4); //1-3

		string itemname = item.getRandomShigen(rdmType, rdmRank, addQty);
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msg = "Citizens delivered " + addQty + " " + itemname + " by street preaching.";
        }
        else if (langId == 3) {
            msg = "因为行道演法，民众进献" + itemname + addQty + "个。";
        }
        else {
            msg = "辻説法によって、民から" + itemname + "が" + addQty + "個、寄贈されました。";
        }
		PlayerPrefs.DeleteKey ("shisya20");
		return msg;
	}

	public string registerJikiso (GameObject slot){
		string msg = "";
        Message Message = new Message();
        int hyourou = PlayerPrefs.GetInt ("hyourou");
		hyourou = hyourou - 30;
		PlayerPrefs.SetInt ("hyourou",hyourou);
		int money = PlayerPrefs.GetInt ("money");
		money = money - 3000;
		PlayerPrefs.SetInt ("money",money);

        int kokuninReject = PlayerPrefs.GetInt("kokuninReject");
        kokuninReject = kokuninReject - 1;
        if (kokuninReject < 0) kokuninReject = 0;
        PlayerPrefs.SetInt("kokuninReject", kokuninReject);

        string text = "";
        int langId = PlayerPrefs.GetInt("langId");
        
        if(kokuninReject < 2) {
            text = Message.getMessage(226, langId);
        }else if (2 <= kokuninReject && kokuninReject < 6) {
            text = Message.getMessage(227, langId);
        }else if (6 <= kokuninReject) {
            text = Message.getMessage(228, langId);
        }
        
        Item item = new Item ();
		int rdmType = UnityEngine.Random.Range (1, 5); //1-4
		int rdmRank = UnityEngine.Random.Range (1, 4); //1-3
		int addQty = UnityEngine.Random.Range (1, 6); //1-5
		string itemname = item.getRandomShigen(rdmType, rdmRank, addQty);
        if (langId == 2) {
            msg = "Citizens delivered " + addQty + " " + itemname + " by your support." + text;
        }else if(langId==3) {
            msg = "因受理直诉，民众进献" + itemname + addQty + "个。" + text;
        }
        else {
            msg = "直訴受諾によって、民から" + itemname +"が" + addQty + "個、寄贈されました。" + text;
        }
		PlayerPrefs.DeleteKey ("shisya21");
		return msg;
	}

	public string registerSyogunApproval(GameObject slot, int langId){
		string msg = "";

		int hyourou = PlayerPrefs.GetInt ("hyourou");
		hyourou = hyourou - 50;
		PlayerPrefs.SetInt ("hyourou",hyourou);

		//Syogun
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
        int myDaimyoId = PlayerPrefs.GetInt("myDaimyo");
		PlayerPrefs.SetInt ("syogunDaimyoId", myDaimyoId);

        Message Message = new Message();
        msg = Message.getMessage(229, langId);
		PlayerPrefs.DeleteKey ("shisya13");
		return msg;
	}


	public string registerToubatsurei(GameObject slot){
		reduceMoneyHyoruou ();
		string msg = "";

		int targetDaimyoId = slot.GetComponent<ShisyaSelect> ().srcDaimyoId;
		string targetDaimyoName = slot.GetComponent<ShisyaSelect> ().srcDaimyoName;

		//Reduce Yukoudo
		string tempGaikou = "gaikou" + targetDaimyoId;
		int newYukoudo = 0;
		PlayerPrefs.SetInt (tempGaikou, newYukoudo);


		//Delete Key
		string tmp = "shisya11";
		string shisyaString = PlayerPrefs.GetString (tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}
		shisyaList.Remove (targetDaimyoId.ToString());
		string newValue = "";
		for (int i = 0; i < shisyaList.Count; i++) {
			if (i == 0) {
				newValue = shisyaList [i];
			} else {
				newValue = newValue + "," + shisyaList [i];
			}
		}
		PlayerPrefs.SetString (tmp,newValue);

        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msg = "You received syogun's attack order and declared of war against " + targetDaimyoName + ". Now frienship with "+targetDaimyoName+" is 0.";
        }else if(langId==3) {
            msg = "已接下将军的讨伐令，向" + targetDaimyoName + "宣战，与" + targetDaimyoName + "的友好度归零。";
        } else {
            msg = "将軍の討伐令を受諾し、" + targetDaimyoName + "に宣戦を布告しました。" + targetDaimyoName + "との友好度が0となります。";
        }
		return msg;
	}

	public string registerBoueirei(GameObject slot){

		reduceMoneyHyoruou ();

		//srcDaimyoId + ":" + dstDaimyoId + ":" + targetKuniId + ":" + key
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		string key  = script.key;

		//Delete Key
		string tmp = "shisya12";	
		string shisyaString = PlayerPrefs.GetString (tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}

		List<string> shisyaNewList = new List<string> ();
		for (int j = 0; j < shisyaList.Count; j++) {
			string shisyaParam = shisyaList [j];

			List<string> shisyaParamList = new List<string> ();
			char[] delimiterChars2 = { ':' };
			if (shisyaParam.Contains (":")) {
				shisyaParamList = new List<string> (shisyaParam.Split (delimiterChars2));
			} else {
				shisyaParamList.Add (shisyaParam);
			}
			if (shisyaParamList [3] != key) {
				shisyaNewList.Add (shisyaParam);
			}
		}

		string newValue = "";
		for (int i = 0; i < shisyaNewList.Count; i++) {
			if (i == 0) {
				newValue = shisyaNewList [i];
			} else {
				newValue = newValue + "," + shisyaNewList [i];
			}
		}
		PlayerPrefs.SetString (tmp,newValue);


		string keyValue = PlayerPrefs.GetString (key);
		List<string> keyValueList = new List<string> ();
		bool alreadyFlg = false;
		if (keyValue != "" && keyValue != null) {
			keyValueList = new List<string> (keyValue.Split (delimiterChars));
		} else {
			alreadyFlg = true;
		}

		string msg = "";
		if (!alreadyFlg) {
			int myJinkeiBusyo = PlayerPrefs.GetInt ("jinkeiBusyoQty");
			int myTotalHei = PlayerPrefs.GetInt ("jinkeiHeiryoku");
			int myUnitHei = myTotalHei / myJinkeiBusyo;
			int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");

			if (keyValueList [9] == "true") {
				//engun exist
				string tmp1 = keyValueList [10] + ":" + myDaimyo;
				string tmp2 = keyValueList [11] + ":" + myUnitHei;

				keyValue = keyValueList [0] + "," + keyValueList [1] + "," + keyValueList [2] + "," + keyValueList [3] + "," + keyValueList [4] + "," + keyValueList [5] + "," + keyValueList [6] + "," + keyValueList [7] + "," + keyValueList [8] + "," + keyValueList [9] + "," + tmp1 + "," + tmp2 + "," + "";
			} else {
				keyValue = keyValueList [0] + "," + keyValueList [1] + "," + keyValueList [2] + "," + keyValueList [3] + "," + keyValueList [4] + "," + keyValueList [5] + "," + keyValueList [6] + "," + keyValueList [7] + "," + keyValueList [8] + "," + true + "," + myDaimyo + "," + myUnitHei + "," + "";
			}
			PlayerPrefs.SetString (key, keyValue);

			//yukoudo up
			string tempGaikou = "gaikou" + script.srcDaimyoId;
			int nowYukoudo = 0;
			if (PlayerPrefs.HasKey (tempGaikou)) {
				nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
			} else {
				nowYukoudo = 50;
			}
			int addPoint = UnityEngine.Random.Range (10, 30);
			int newyukoudo = nowYukoudo + addPoint;
			if (newyukoudo > 100) {
				newyukoudo = 100;
			}
			PlayerPrefs.SetInt (tempGaikou, newyukoudo);
			string dstDaimyoName = script.dstDaimyoName;

            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                msg = "We sent reinforcement " + myUnitHei + " soldiers to support " + dstDaimyoName + " by syogun's request. \n Frienship increased " + addPoint + " point.";
            }
            else if (langId == 3) {
                msg = "响应将军的要求，派出" + dstDaimyoName + "人救援" + myUnitHei + "，和" + dstDaimyoName + "的友好度提升了" + addPoint + "。";
            } else {
                msg = "将軍の要請に応じ、" + dstDaimyoName +  "救援の兵" + myUnitHei + "を送りました。" + dstDaimyoName + "との友好度が" + addPoint + "上昇します。";
            }
		} else {

			string tempGaikou = "gaikou" + script.srcDaimyoId;
			int nowYukoudo = 0;
			if (PlayerPrefs.HasKey (tempGaikou)) {
				nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
			} else {
				nowYukoudo = 50;
			}
			int reducePoint = UnityEngine.Random.Range (10, 30);
			int newyukoudo = nowYukoudo - reducePoint;
			if (newyukoudo < 0) {
				newyukoudo = 0;
			}
			PlayerPrefs.SetInt (tempGaikou, newyukoudo);
			string dstDaimyoName = script.dstDaimyoName;
            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                msg = "My lord. Battle was already finished. Friendship with " + dstDaimyoName + " reduced " + reducePoint + " point.";
            }
            else if (langId == 3) {
                msg = "主公，胜负已分，和" + dstDaimyoName + "的友好度下降了" + reducePoint + "。";
            } else {
                msg = "殿、勝敗は既に決着したようですぞ。" + dstDaimyoName + "殿との友好度が" + reducePoint + "減少します。";
            }
		}

		return msg;
	}

	public string registerDoumeiHaki(GameObject slot){
		
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		string daimyoName = script.srcDaimyoName;
        string msg = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msg = daimyoName + " renounced alliance with our clan.";
        }
        else if (langId == 3) {
            msg = daimyoName + "已经撕毁了和本家的同盟。";
        }
        else {
            msg = daimyoName + "が当家との同盟を破棄しました。";
        }
		PlayerPrefs.DeleteKey ("shisya6");
		return msg;

	}

	public string registerCyakai(GameObject slot){

		reduceMoneyHyoruou ();

		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		int srcDaimyoId = script.srcDaimyoId;
		string srcDaimyoName = script.srcDaimyoName;

		//Cyakai Meisei
		int meiseiQty = PlayerPrefs.GetInt ("meisei");
		meiseiQty = meiseiQty + 1;
		PlayerPrefs.SetInt ("meisei",meiseiQty);

		//Up Yukoudo
		string tempGaikou = "gaikou" + srcDaimyoId;
		int nowYukoudo = 0;
		if (PlayerPrefs.HasKey (tempGaikou)) {
			nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
		} else {
			nowYukoudo = 50;
		}
		int addPoint = UnityEngine.Random.Range (10, 30);
		int newyukoudo = nowYukoudo + addPoint;
		if (newyukoudo > 100) {
			newyukoudo = 100;
		}
		PlayerPrefs.SetInt (tempGaikou, newyukoudo);

		//Delete Key
		string tmp = "shisya9";
		string shisyaString = PlayerPrefs.GetString (tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}
		shisyaList.Remove (srcDaimyoId.ToString());
		string newValue = "";
		for (int i = 0; i < shisyaList.Count; i++) {
			if (i == 0) {
				newValue = shisyaList [i];
			} else {
				newValue = newValue + "," + shisyaList [i];
			}
		}
		PlayerPrefs.SetString (tmp,newValue);

        string msg = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msg = "You joined "+srcDaimyoName + "'s tea party. Friendship increased " + addPoint + " point. And you got repuration. Traveller will visit your country to see your tea things.";
        }
        else if (langId == 3) {
            msg = "参加了" + srcDaimyoName + "大人的茶会，友好度上升了" + addPoint + "，另外入手了名声，为拜见主公的茶道具，旅人们云集城下。";
        } else {
            msg = srcDaimyoName + "殿の茶会に参加しました。友好度が" + addPoint + "上昇します。また名声を入手致しました。御屋形様の茶道具を見に旅人が集まりますぞ。";
        }
        return msg;

	}

	public string registerKoueki(GameObject slot){

		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		string kahouCd = script.itemCd;
		string kahouName = script.itemName;
		int kahouId = int.Parse(script.itemId);
		int payMoney = script.moneyNo;

		//Minus Money
		int money = PlayerPrefs.GetInt ("money");
		money = money - payMoney;
		PlayerPrefs.SetInt ("money",money);

		//Add kahou
		Kahou kahou = new Kahou ();
		if (kahouCd == "bugu") {
			kahou.registerBugu (kahouId);
		} else if (kahouCd == "gusoku") {
			kahou.registerGusoku (kahouId);
		} else if (kahouCd == "kabuto") {
			kahou.registerKabuto (kahouId);
		} else if (kahouCd == "meiba") {
			kahou.registerMeiba (kahouId);
		} else if (kahouCd == "cyadougu") {
			kahou.registerCyadougu (kahouId);
		} else if (kahouCd == "chishikisyo") {
			kahou.registerChishikisyo (kahouId);
		} else if (kahouCd == "heihousyo") {
			kahou.registerHeihousyo (kahouId);					
		}

		int id = script.shisyaId;
		string temp = "shisya" + id.ToString ();
		PlayerPrefs.DeleteKey (temp);
        string msg = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msg = "You purchased "+kahouName + ". It's a good item.";
        }
        else if (langId == 3) {
            msg = "已购买" + kahouName + ",真是入手了一个好东西啊。";
        }
        else {
            msg = kahouName + "を購入しましたぞ。良い物を手に入れられましたな。";
        }
		return msg;
	}

	public string rejectDoumei (GameObject slot){
		
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		int daimyoId = script.srcDaimyoId;
		string daimyoName = script.srcDaimyoName;

		//Process
		string tempGaikou = "gaikou" + daimyoId;
		int nowYukoudo = 0;
		if (PlayerPrefs.HasKey (tempGaikou)) {
			nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
		} else {
			nowYukoudo = 50;
		}
		int reducePoint = UnityEngine.Random.Range (5, 20);
		int newyukoudo = nowYukoudo -reducePoint;
		if (newyukoudo < 0) {
			newyukoudo = 0;
		}
		PlayerPrefs.SetInt (tempGaikou, newyukoudo);

		//Delete Key
		string tmp = "shisya1";	
		string shisyaString = PlayerPrefs.GetString (tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}
		shisyaList.Remove (script.srcDaimyoId.ToString());
		string newValue = "";
		for (int i = 0; i < shisyaList.Count; i++) {
			if (i == 0) {
				newValue = shisyaList [i];
			} else {
				newValue = newValue + "," + shisyaList [i];
			}
		}
		PlayerPrefs.SetString (tmp,newValue);

        //Msg
        string msg = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msg = "Declined alliance request by "+daimyoName + ". Friendship decreased " + reducePoint + " point.";
        }
        else if (langId == 3) {
            msg = "已拒绝了" + daimyoName + "的同盟请求，友好度下降了" + reducePoint + "。";
        }
        else {
            msg = daimyoName + "からの同盟依頼を断りました。友好度が" + reducePoint + "減少します。";
        }
            
		return msg;
	}

	public string rejectEngun(GameObject slot){
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		int daimyoId = script.srcDaimyoId;
		string daimyoName = script.srcDaimyoName;
		string key = script.key;

		//Delete Key
		string tmp = "shisya3";	
		string shisyaString = PlayerPrefs.GetString (tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}
		List<string> shisyaNewList = new List<string> ();
		for (int j = 0; j < shisyaList.Count; j++) {
			string shisyaParam = shisyaList [j];

			List<string> shisyaParamList = new List<string> ();
			char[] delimiterChars2 = { ':' };
			if (shisyaParam.Contains (":")) {
				shisyaParamList = new List<string> (shisyaParam.Split (delimiterChars2));
			} else {
				shisyaParamList.Add (shisyaParam);
			}
			if (shisyaParamList [3] != key) {
				shisyaNewList.Add (shisyaParam);
			}
		}
		string newValue = "";
		for (int i = 0; i < shisyaNewList.Count; i++) {
			if (i == 0) {
				newValue = shisyaNewList [i];
			} else {
				newValue = newValue + "," + shisyaNewList [i];
			}
		}
		PlayerPrefs.SetString (tmp,newValue);

		//Process
		string tempGaikou = "gaikou" + daimyoId;
		int nowYukoudo = 0;
		if (PlayerPrefs.HasKey (tempGaikou)) {
			nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
		} else {
			nowYukoudo = 50;
		}
		int reducePoint = UnityEngine.Random.Range (5, 25);
		int newyukoudo = nowYukoudo - reducePoint;
		if (newyukoudo < 0) {
			newyukoudo = 0;
		}
		PlayerPrefs.SetInt (tempGaikou, newyukoudo);
        string msg = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msg = "Declined reinforcement request by "+daimyoName + ". Friendship decreased " + reducePoint + " point.";
        }
        else if (langId == 3) {
            msg = "已拒绝了" + daimyoName + "的援军邀请，友好度下降了" + reducePoint + "。";
        }
        else {
            msg = daimyoName + "からの援軍依頼を断りました。\n友好度が" + reducePoint + "減少します。";
        }
        return msg;
	}


	public string rejectDoukatsu(GameObject slot){
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		int daimyoId = script.srcDaimyoId;
		string daimyoName = script.srcDaimyoName;

		//yukoudo
		string tempGaikou = "gaikou" + daimyoId;
		int nowYukoudo = 0;
		if (PlayerPrefs.HasKey (tempGaikou)) {
			nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
		} else {
			nowYukoudo = 50;
		}
		int reducePoint = UnityEngine.Random.Range (5, 10);
		int newyukoudo = nowYukoudo - reducePoint;
		if (newyukoudo < 0) {
			newyukoudo = 0;
		}
		PlayerPrefs.SetInt (tempGaikou, newyukoudo);


		//Delete Key
		string tmp = "shisya4";
		string shisyaString = PlayerPrefs.GetString (tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}
		shisyaList.Remove (daimyoId.ToString());
		string newValue = "";
		for (int i = 0; i < shisyaList.Count; i++) {
			if (i == 0) {
				newValue = shisyaList [i];
			} else {
				newValue = newValue + "," + shisyaList [i];
			}
		}
		PlayerPrefs.SetString (tmp,newValue);

		string msg = daimyoName + "の恫喝を拒絶しました。友好度が" + reducePoint + "減少します。";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msg = "Rejected threat by " + daimyoName + ". Friendship decreased " + reducePoint + " point.";
        }
        else if (langId == 3) {
            msg = "已拒绝了" + daimyoName + "的恫吓，友好度下降了" + reducePoint + "。";
        }
        else {
            msg = daimyoName + "の恫喝を拒絶しました。友好度が" + reducePoint + "減少します。";
        }
        return msg;
	}

	public string rejectKoueki(GameObject slot){
	
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		string shisyaName = "";
		int id = script.shisyaId;
        int langId = PlayerPrefs.GetInt("langId");
        if (id == 5) {
			shisyaName = script.srcDaimyoName;
		} else if (id == 19) {
            if (langId == 2) {
                shisyaName = "Westerner";
            }else {
                shisyaName = "南蛮人";
            }
		} else if (id == 18) {
            if (langId == 2) {
                shisyaName = "Merchant";
            }else {
                shisyaName = "行商人";
            }
		}
		string temp = "shisya" + id.ToString ();
		PlayerPrefs.DeleteKey (temp);
        string msg = "";
        if (langId == 2) {
            msg = "Declined trade request by " + shisyaName + ".";
        }
        else if (langId == 3) {
            msg = "已拒绝" + shisyaName + "的交易请求。";
        }
        else {
            msg = shisyaName + "からの交易依頼を断りました。";
        }
        
		return msg;
	}

	public string rejectMitsugimono(GameObject slot){
		
		string msg = "";
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		int srcDaimyoId = script.srcDaimyoId;
		string srcDaimyoName = script.srcDaimyoName;

		//Process
		string tempGaikou = "gaikou" + srcDaimyoId;
		int nowYukoudo = 0;
		if (PlayerPrefs.HasKey (tempGaikou)) {
			nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
		} else {
			nowYukoudo = 50;
		}
		int reducePoint = UnityEngine.Random.Range (5, 15);
		int newyukoudo = nowYukoudo - reducePoint;
		if (newyukoudo < 0) {
			newyukoudo = 0;
		}
		PlayerPrefs.SetInt (tempGaikou, newyukoudo);

		//Delete Key
		string tmp = "shisya7";
		string shisyaString = PlayerPrefs.GetString (tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}
		shisyaList.Remove (srcDaimyoId.ToString());
		string newValue = "";
		for (int i = 0; i < shisyaList.Count; i++) {
			if (i == 0) {
				newValue = shisyaList [i];
			} else {
				newValue = newValue + "," + shisyaList [i];
			}
		}
		PlayerPrefs.SetString (tmp,newValue);
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msg =  "Declined gift by " + srcDaimyoName + ". Friendshio decreased " + reducePoint +" point.";
        }
        else if (langId == 3) {
            msg = "已拒绝" + srcDaimyoName + "的进贡请求，友好度下降了" + reducePoint + "。";
        }
        else {
            msg = srcDaimyoName + "からの貢物を断りました。友好度が、" + reducePoint + "減少します。";
        }
		return msg;
	}

	public string rejectKyouhaku(GameObject slot){

		string msg = "";
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		string key  = script.key;
		string srcDaimyoName  = script.srcDaimyoName;

		//Delete Key
		string tmp = "shisya8";	
		string shisyaString = PlayerPrefs.GetString (tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}
		List<string> shisyaNewList = new List<string> ();
		for (int j = 0; j < shisyaList.Count; j++) {
			string shisyaParam = shisyaList [j];

			List<string> shisyaParamList = new List<string> ();
			char[] delimiterChars2 = { ':' };
			if (shisyaParam.Contains (":")) {
				shisyaParamList = new List<string> (shisyaParam.Split (delimiterChars2));
			} else {
				shisyaParamList.Add (shisyaParam);
			}
			if (shisyaParamList [2] != key) {
				shisyaNewList.Add (shisyaParam);
			}
		}
		string newValue = "";
		for (int i = 0; i < shisyaNewList.Count; i++) {
			if (i == 0) {
				newValue = shisyaNewList [i];
			} else {
				newValue = newValue + "," + shisyaNewList [i];
			}
		}
		PlayerPrefs.SetString (tmp,newValue);

        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msg = "Rejected blackmail from " + srcDaimyoName + ". Let's destroy them in battle.";
        }
        else if (langId == 3) {
            msg = "已拒绝" + srcDaimyoName + "的胁迫，家臣们士气高涨，让我们在战场上击败他们吧。";
        }
        else {
            msg = srcDaimyoName + "からの脅迫を断りました。家来の士気も高まっています。戦にて撃退しましょうぞ。";
        }

		return msg;

	}
	public string rejectCyakai(GameObject slot){
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		int srcDaimyoId = script.srcDaimyoId;
		string srcDaimyoName = script.srcDaimyoName;

		//Process
		string tempGaikou = "gaikou" + srcDaimyoId;
		int nowYukoudo = 0;
		if (PlayerPrefs.HasKey (tempGaikou)) {
			nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
		} else {
			nowYukoudo = 50;
		}
		int reducePoint = UnityEngine.Random.Range (5, 15);
		int newyukoudo = nowYukoudo - reducePoint;
		if (newyukoudo < 0) {
			newyukoudo = 0;
		}
		PlayerPrefs.SetInt (tempGaikou, newyukoudo);

		//Delete Key
		string tmp = "shisya9";
		string shisyaString = PlayerPrefs.GetString (tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}
		shisyaList.Remove (srcDaimyoId.ToString());
		string newValue = "";
		for (int i = 0; i < shisyaList.Count; i++) {
			if (i == 0) {
				newValue = shisyaList [i];
			} else {
				newValue = newValue + "," + shisyaList [i];
			}
		}
		PlayerPrefs.SetString (tmp,newValue);
        string msg = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msg = "Declined tea party invitation from " + srcDaimyoName + ". Friendship reduced " + reducePoint + " point.";
        }
        else if (langId == 3) {
            msg = "已拒绝" + srcDaimyoName + "的茶会邀请，友好度下降了" + reducePoint + "。";
        }
        else {
            msg = srcDaimyoName + "殿の茶会の誘いを断りました。友好度が" + reducePoint + "減少します。";
        }
		return msg;
	}

	public string rejectCyouteiCyusai(GameObject slot ,bool cyouteiFlg){

		string msg = "";
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		int srcDaimyoId = script.srcDaimyoId;

		//Delete Key
		string tmp = "";
		if (cyouteiFlg) {
			tmp = "shisya17";
			int nowPoint = PlayerPrefs.GetInt ("cyouteiPoint");
			int reducePoint = UnityEngine.Random.Range (5, 10);
			int newPoint = nowPoint - reducePoint;
			if (newPoint < 0) {
				newPoint = 0;
			}
			PlayerPrefs.SetInt ("cyouteiPoint", newPoint);
            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                msg = "Declined mediation of royal court. Royal court point decreased " + reducePoint + " point.";
            }
            else if (langId == 3) {
                msg = "已拒绝贵族的调停，朝廷贡献度下降了" + reducePoint + "。";
            }
            else {
                msg = "貴族の調停を断りました。朝廷貢献度が" + reducePoint + "減少します。";
            }
		} else {
			tmp = "shisya10";
			int syogunDaimyoId = PlayerPrefs.GetInt ("syogunDaimyoId");
			string tempGaikou = "gaikou" + syogunDaimyoId;
			int nowYukoudo = 0;
			if (PlayerPrefs.HasKey (tempGaikou)) {
				nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
			} else {
				nowYukoudo = 50;
			}
			int reducePoint = UnityEngine.Random.Range (5, 10);
			int newyukoudo = nowYukoudo - reducePoint;
			if (newyukoudo < 0) {
				newyukoudo = 0;
			}
			PlayerPrefs.SetInt (tempGaikou, newyukoudo);

            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                msg = "Declined mediation of syogun. Friendship decreased " + reducePoint + " point.";
            }
            else if (langId == 3) {
                msg = "已拒绝将军的仲裁，与将军的友好度下降了" + reducePoint + "。";
            }
            else {
                msg = "将軍の仲裁を断りました。将軍との友好度が" + reducePoint + "減少します。";
            }
        }

		string shisyaString = PlayerPrefs.GetString (tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}
		shisyaList.Remove (srcDaimyoId.ToString());
		string newValue = "";
		for (int i = 0; i < shisyaList.Count; i++) {
			if (i == 0) {
				newValue = shisyaList [i];
			} else {
				newValue = newValue + "," + shisyaList [i];
			}
		}
		PlayerPrefs.SetString (tmp,newValue);

		return msg;
	}

	public string rejecToubatsurei(GameObject slot){
		string msg = "";
		int targetDaimyoId = slot.GetComponent<ShisyaSelect> ().srcDaimyoId;

		int syogunDaimyoId = PlayerPrefs.GetInt ("syogunDaimyoId");
		string tempGaikou = "gaikou" + syogunDaimyoId;
		int nowYukoudo = 0;
		if (PlayerPrefs.HasKey (tempGaikou)) {
			nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
		} else {
			nowYukoudo = 50;
		}
		int reducePoint = UnityEngine.Random.Range (5, 15);
		int newyukoudo = nowYukoudo - reducePoint;
		if (newyukoudo < 0) {
			newyukoudo = 0;
		}
		PlayerPrefs.SetInt (tempGaikou, newyukoudo);
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msg = "Ignored Syogun's order. Friendship decreased " + reducePoint + " point.";
        }
        else if (langId == 3) {
            msg = "已无视将军的讨伐令，与将军的友好度下降了" + reducePoint + "。";
        }
        else {
            msg = "将軍の討伐令を無視しました。将軍との友好度が" + reducePoint + "減少します。";
        }

		//Delete Key
		string tmp = "shisya11";
		string shisyaString = PlayerPrefs.GetString (tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}
		shisyaList.Remove (targetDaimyoId.ToString());
		string newValue = "";
		for (int i = 0; i < shisyaList.Count; i++) {
			if (i == 0) {
				newValue = shisyaList [i];
			} else {
				newValue = newValue + "," + shisyaList [i];
			}
		}
		PlayerPrefs.SetString (tmp,newValue);

		return msg;
	}

	public string rejectBoueirei(GameObject slot){

		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		string key  = script.key;

		int syogunDaimyoId = PlayerPrefs.GetInt ("syogunDaimyoId");
		string tempGaikou = "gaikou" + syogunDaimyoId;
		int nowYukoudo = 0;
		if (PlayerPrefs.HasKey (tempGaikou)) {
			nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
		} else {
			nowYukoudo = 50;
		}
		int reducePoint = UnityEngine.Random.Range (5, 15);
		int newyukoudo = nowYukoudo - reducePoint;
		if (newyukoudo < 0) {
			newyukoudo = 0;
		}
		PlayerPrefs.SetInt (tempGaikou, newyukoudo);
        string msg = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msg = "Ignored Syogun's order. Friendship decreased " + reducePoint + " point.";
        }
        else if (langId == 3) {
            msg = "已无视将军的防卫令，与将军的友好度下降了" + reducePoint + "。";
        }
        else {
            msg = "将軍の防衛令を無視しました。将軍との友好度が" + reducePoint + "減少します。";
        }

		//Delete Key
		string tmp = "shisya12";	
		string shisyaString = PlayerPrefs.GetString (tmp);
		List<string> shisyaList = new List<string> ();
		char[] delimiterChars = {','};
		if (shisyaString != null && shisyaString != "") {
			if (shisyaString.Contains (",")) {
				shisyaList = new List<string> (shisyaString.Split (delimiterChars));
			} else {
				shisyaList.Add (shisyaString);
			}
		}
		List<string> shisyaNewList = new List<string> ();
		for (int j = 0; j < shisyaList.Count; j++) {
			string shisyaParam = shisyaList [j];

			List<string> shisyaParamList = new List<string> ();
			char[] delimiterChars2 = { ':' };
			if (shisyaParam.Contains (":")) {
				shisyaParamList = new List<string> (shisyaParam.Split (delimiterChars2));
			} else {
				shisyaParamList.Add (shisyaParam);
			}
			if (shisyaParamList [3] != key) {
				shisyaNewList.Add (shisyaParam);
			}
		}
		string newValue = "";
		for (int i = 0; i < shisyaNewList.Count; i++) {
			if (i == 0) {
				newValue = shisyaNewList [i];
			} else {
				newValue = newValue + "," + shisyaNewList [i];
			}
		}
		PlayerPrefs.SetString (tmp,newValue);

		return msg;
	}

	public string rejectSyogunApproval(GameObject slot, int langId){
		string msg = "";

        //Shogun        
        Message Message = new Message();
        msg = Message.getMessage(230, langId);
		PlayerPrefs.DeleteKey ("shisya13");
		return msg;
	}

	public string rejectMusin(GameObject slot, bool cyouteiFlg){
		
		string msg = "";
		ShisyaSelect script = slot.GetComponent<ShisyaSelect> ();
		int srcDaimyoId = script.srcDaimyoId;

		//Delete Key
		string tmp = "";
		if (cyouteiFlg) {
			tmp = "shisya15";
			int nowPoint = PlayerPrefs.GetInt ("cyouteiPoint");
			int reducePoint = UnityEngine.Random.Range (5, 10);
			int newPoint = nowPoint - reducePoint;
			if (newPoint < 0) {
				newPoint = 0;
			}
			PlayerPrefs.SetInt ("cyouteiPoint", newPoint);
            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                msg = "Declined asking for money. Royal Court point decreased " + reducePoint + " point.";
            }else if(langId==3) {
                msg = "已拒绝贵族的无心，朝廷贡献度下降了" + reducePoint + "。";
            }
            else {
                msg = "貴族の無心を断りました。朝廷貢献度が" + reducePoint + "減少します。";
            }
		} else {
			tmp = "shisya14";
			int syogunDaimyoId = PlayerPrefs.GetInt ("syogunDaimyoId");
			string tempGaikou = "gaikou" + syogunDaimyoId;
			int nowYukoudo = 0;
			if (PlayerPrefs.HasKey (tempGaikou)) {
				nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
			} else {
				nowYukoudo = 50;
			}
			int reducePoint = UnityEngine.Random.Range (5, 10);
			int newyukoudo = nowYukoudo - reducePoint;
			if (newyukoudo < 0) {
				newyukoudo = 0;
			}
			PlayerPrefs.SetInt (tempGaikou, newyukoudo);
            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                msg = "Declined asking for money. Friendship decreased " + reducePoint + " point.";
            }
            else if (langId == 3) {
                msg = "已拒绝将军的无心，与将军的友好度下降了" + reducePoint + "。";
            }
            else {
                msg = "将軍の無心を断りました。将軍との友好度が" + reducePoint + "減少します。";
            }
		}

		PlayerPrefs.DeleteKey (tmp);
		return msg;
	}

	public string rejectKyucyuGyouji(GameObject slot){
		
		int nowPoint = PlayerPrefs.GetInt ("cyouteiPoint");
		int reducePoint = UnityEngine.Random.Range (5, 10);
		int newPoint = nowPoint - reducePoint;
		if (newPoint < 0) {
			newPoint = 0;
		}
		PlayerPrefs.SetInt ("cyouteiPoint", newPoint);
		PlayerPrefs.DeleteKey ("shisya16");
        string msg = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msg = "Declined providing stone to royal court. Royal court point decreased " + reducePoint + " point.";
        }
        else if (langId == 3) {
            msg = "已拒绝为朝廷的宫中行事提供金钱，朝廷贡献度下降了" + reducePoint + "。";
        }
        else {
            msg = "朝廷への宮中行事費用の提供を断りました。朝廷貢献度が" + reducePoint + "減少します。";
        }
		return msg;
	}
    
    public string surrenderText(GameObject slot) {
        string msg = "";
        ShisyaSelect script = slot.GetComponent<ShisyaSelect>();
        int srcDaimyoId = script.srcDaimyoId;
        Daimyo Daimyo = new Daimyo();
        int langId = PlayerPrefs.GetInt("langId");
        string clanName = Daimyo.getClanName(srcDaimyoId, langId);
        
        if (langId == 2) {
            msg = clanName + " clan surrendered to us.";
        } else {
            msg = clanName + "家が当家に従属し、滅亡しました。天下の趨勢は決したも同然ですな。";
        }


        return msg;
    }



}
