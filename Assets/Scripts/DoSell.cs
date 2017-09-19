using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System.Collections.Generic;

public class DoSell : MonoBehaviour {

	public int kahouId;
	public string kahouName;
	public string kahouType;
	public int kahouSell;
	public int itemId;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [3].Play ();

		/*Get Scene*/
		string currentTab = GameObject.Find ("GameScene").GetComponent<SoukoScene> ().currentTab;
		int money = 0;

		/*kahou*/
		if (currentTab == "KahouScene") {
			string target = "";
			if (kahouType == "bugu") {
				target = "availableBugu";
			} else if (kahouType == "kabuto") {
				 target = "availableKabuto";
			} else if (kahouType == "gusoku") {
				 target = "availableGusoku";
			} else if (kahouType == "meiba") {
				 target = "availableMeiba";
			} else if (kahouType == "cyadougu") {
				 target = "availableCyadougu";
			} else if (kahouType == "heihousyo") {
				 target = "availableHeihousyo";
			} else if (kahouType == "chishikisyo") {
				 target = "availableChishikisyo";
			}
			reduceKahou (target,kahouId);

			//Add money
			money = PlayerPrefs.GetInt ("money");
			money = money + kahouSell;
            if (money < 0) {
                money = int.MaxValue;
            }
            PlayerPrefs.SetInt ("money", money);
			PlayerPrefs.SetBool ("questDailyFlg20",true);

			PlayerPrefs.Flush();
			
			//Reload
			GameObject.Find ("MoneyValue").GetComponent<Text> ().text = money.ToString();		
			GameObject.Find ("Kahou").GetComponent<KahouSoukoScene> ().OnClick ();	

			//Msg
			Message msg = new Message();
            string OKtext = "";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                OKtext= "Sold " + kahouName + ".";
            }else {
                OKtext = kahouName + "を売却致しました。";
            }
			msg.makeMessage(OKtext);


		} else if (currentTab == "DouguScene") {
			/*dougu*/

			//Common
			char[] delimiterChars = {','};
			int sellQty = 0;

			//Kanjyo
			if (kahouType.Contains ("Kanjyo") == true) {
				string kanjyoString = PlayerPrefs.GetString ("kanjyo");
				string[] kanjyo_list = kanjyoString.Split (delimiterChars);
				string newKanjyoString = "";

				if (kahouType == "Kanjyo1") {
					int remainQty = 0;
					sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
					remainQty = int.Parse (kanjyo_list [0]) - sellQty;
					newKanjyoString = remainQty.ToString () + "," + kanjyo_list [1] + "," + kanjyo_list [2];
					
				} else if (kahouType == "Kanjyo2") {
					int remainQty = 0;
					sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
					remainQty = int.Parse (kanjyo_list [1]) - sellQty;
					newKanjyoString = kanjyo_list [0] + "," + remainQty.ToString () + "," + kanjyo_list [2];

				} else if (kahouType == "Kanjyo3") {
					int remainQty = 0;
					sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
					remainQty = int.Parse (kanjyo_list [2]) - sellQty;
					newKanjyoString = kanjyo_list [0] + "," + kanjyo_list [1] + "," + remainQty.ToString ();
				
				}
				PlayerPrefs.SetString ("kanjyo", newKanjyoString);
			

			} else if (kahouType.Contains ("Cyouhei") == true) {
				if (kahouType.Contains ("YR") == true) {
					string cyouheiYRString = PlayerPrefs.GetString ("cyouheiYR");
					string[] cyouheiYR_list = cyouheiYRString.Split (delimiterChars);
					string newCyouheiYRString = "";

					if (kahouType == "CyouheiYR1") {
						int remainQty = 0;
						sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
						remainQty = int.Parse (cyouheiYR_list [0]) - sellQty;
						newCyouheiYRString = remainQty.ToString () + "," + cyouheiYR_list [1] + "," + cyouheiYR_list [2];


					} else if (kahouType == "CyouheiYR2") {
						int remainQty = 0;
						sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
						remainQty = int.Parse (cyouheiYR_list [1]) - sellQty;
						newCyouheiYRString = cyouheiYR_list [0] + "," + remainQty.ToString () + "," + cyouheiYR_list [2];

					} else if (kahouType == "CyouheiYR3") {
						int remainQty = 0;
						sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
						remainQty = int.Parse (cyouheiYR_list [2]) - sellQty;
						newCyouheiYRString = cyouheiYR_list [0] + "," + cyouheiYR_list [1] + "," + remainQty.ToString ();

					}
                    if (newCyouheiYRString != "") {
                        PlayerPrefs.SetString ("cyouheiYR", newCyouheiYRString);
                    }
                } else if (kahouType.Contains ("KB") == true) {
					string cyouheiKBString = PlayerPrefs.GetString ("cyouheiKB");
					string[] cyouheiKB_list = cyouheiKBString.Split (delimiterChars);
					string newCyouheiKBString = "";
					
					if (kahouType == "CyouheiKB1") {
						int remainQty = 0;
						sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
						remainQty = int.Parse (cyouheiKB_list [0]) - sellQty;
						newCyouheiKBString = remainQty.ToString () + "," + cyouheiKB_list [1] + "," + cyouheiKB_list [2];
						
						
					} else if (kahouType == "CyouheiKB2") {
						int remainQty = 0;
						sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
						remainQty = int.Parse (cyouheiKB_list [1]) - sellQty;
						newCyouheiKBString = cyouheiKB_list [0] + "," + remainQty.ToString () + "," + cyouheiKB_list [2];
						
					} else if (kahouType == "CyouheiKB3") {
						int remainQty = 0;
						sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
						remainQty = int.Parse (cyouheiKB_list [2]) - sellQty;
						newCyouheiKBString = cyouheiKB_list [0] + "," + cyouheiKB_list [1] + "," + remainQty.ToString ();
						
					}
                    if (newCyouheiKBString != "") {
                        PlayerPrefs.SetString ("cyouheiKB", newCyouheiKBString);
                    }
				} else if (kahouType.Contains ("TP") == true) {
					string cyouheiTPString = PlayerPrefs.GetString ("cyouheiTP");
					string[] cyouheiTP_list = cyouheiTPString.Split (delimiterChars);
					string newCyouheiTPString = "";
					
					if (kahouType == "CyouheiTP1") {
						int remainQty = 0;
						sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
						remainQty = int.Parse (cyouheiTP_list [0]) - sellQty;
						newCyouheiTPString = remainQty.ToString () + "," + cyouheiTP_list [1] + "," + cyouheiTP_list [2];
						
						
					} else if (kahouType == "CyouheiTP2") {
						int remainQty = 0;
						sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
						remainQty = int.Parse (cyouheiTP_list [1]) - sellQty;
						newCyouheiTPString = cyouheiTP_list [0] + "," + remainQty.ToString () + "," + cyouheiTP_list [2];
						
					} else if (kahouType == "CyouheiTP3") {
						int remainQty = 0;
						sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
						remainQty = int.Parse (cyouheiTP_list [2]) - sellQty;
						newCyouheiTPString = cyouheiTP_list [0] + "," + cyouheiTP_list [1] + "," + remainQty.ToString ();
						
					}
                    if (newCyouheiTPString != "") {
                        PlayerPrefs.SetString ("cyouheiTP", newCyouheiTPString);
                    }
				} else if (kahouType.Contains ("YM") == true) {
					string cyouheiYMString = PlayerPrefs.GetString ("cyouheiYM");
					string[] cyouheiYM_list = cyouheiYMString.Split (delimiterChars);
					string newCyouheiYMString = "";
					
					if (kahouType == "CyouheiYM1") {
						int remainQty = 0;
						sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
						remainQty = int.Parse (cyouheiYM_list [0]) - sellQty;
						newCyouheiYMString = remainQty.ToString () + "," + cyouheiYM_list [1] + "," + cyouheiYM_list [2];
						
						
					} else if (kahouType == "CyouheiYM2") {
						int remainQty = 0;
						sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
						remainQty = int.Parse (cyouheiYM_list [1]) - sellQty;
						newCyouheiYMString = cyouheiYM_list [0] + "," + remainQty.ToString () + "," + cyouheiYM_list [2];
						
					} else if (kahouType == "CyouheiYM3") {
						int remainQty = 0;
						sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
						remainQty = int.Parse (cyouheiYM_list [2]) - sellQty;
						newCyouheiYMString = cyouheiYM_list [0] + "," + cyouheiYM_list [1] + "," + remainQty.ToString ();
						
					}
                    if (newCyouheiYMString != "") {
                        PlayerPrefs.SetString ("cyouheiYM", newCyouheiYMString);
                    }
				}



			} else if (kahouType.Contains ("Hidensyo") == true) {
				if (kahouType == "Hidensyo1") {
					int Hidensyo1Qty = PlayerPrefs.GetInt ("hidensyoGe");
					sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
					int remainQty = Hidensyo1Qty - sellQty;
					PlayerPrefs.SetInt ("hidensyoGe", remainQty);

				} else if (kahouType == "Hidensyo2") {
					int Hidensyo1Qty = PlayerPrefs.GetInt ("hidensyoCyu");
					sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
					int remainQty = Hidensyo1Qty - sellQty;
					PlayerPrefs.SetInt ("hidensyoCyu", remainQty);

				} else if (kahouType == "Hidensyo3") {
					int Hidensyo1Qty = PlayerPrefs.GetInt ("hidensyoJyo");
					sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
					int remainQty = Hidensyo1Qty - sellQty;
					PlayerPrefs.SetInt ("hidensyoJyo", remainQty);

				}
			} else if (kahouType.Contains ("Shinobi") == true) {
				if (kahouType == "Shinobi1") {
					int shinobi1Qty = PlayerPrefs.GetInt ("shinobiGe");
					sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
					int remainQty = shinobi1Qty - sellQty;
					PlayerPrefs.SetInt ("shinobiGe", remainQty);
					
				} else if (kahouType == "Shinobi2") {
					int shinobi2Qty = PlayerPrefs.GetInt ("shinobiCyu");
					sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
					int remainQty = shinobi2Qty - sellQty;
					PlayerPrefs.SetInt ("shinobiCyu", remainQty);
					
				} else if (kahouType == "Shinobi3") {
					int shinobi3Qty = PlayerPrefs.GetInt ("shinobiJyo");
					sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
					int remainQty = shinobi3Qty - sellQty;
					PlayerPrefs.SetInt ("shinobiJyo", remainQty);
					
				}



			} else if (kahouType.Contains ("kengou") == true) {


				sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);

				string kengouString = PlayerPrefs.GetString ("kengouItem");

				List<string> kengouList = new List<string> ();
				kengouList = new List<string> (kengouString.Split (delimiterChars));

				itemId = int.Parse (kahouType.Remove (0, 6));
				int qty = int.Parse (kengouList [itemId - 1]);

				int remainQty = qty - sellQty;
				kengouList [itemId - 1] = remainQty.ToString ();

				string newKengouString = "";
				for (int i = 0; i < kengouList.Count; i++) {
					
					if (i == 0) {
						newKengouString = kengouList [i];
					} else {
						newKengouString = newKengouString + "," + kengouList [i];
					}
				}
				
				PlayerPrefs.SetString ("kengouItem", newKengouString);



			} else if (kahouType.Contains ("gokui") == true) {
				
				
				sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
				
				string gokuiString = PlayerPrefs.GetString ("gokuiItem");
				
				List<string> gokuiList = new List<string> ();
				gokuiList = new List<string> (gokuiString.Split (delimiterChars));
				
				itemId = int.Parse (kahouType.Remove (0, 5));
				int qty = int.Parse (gokuiList [itemId - 1]);
				
				int remainQty = qty - sellQty;
				gokuiList [itemId - 1] = remainQty.ToString ();
				
				string newGokuiString = "";
				for (int i = 0; i < gokuiList.Count; i++) {
					
					if (i == 0) {
						newGokuiString = gokuiList [i];
					} else {
						newGokuiString = newGokuiString + "," + gokuiList [i];
					}
				}
				
				PlayerPrefs.SetString ("gokuiItem", newGokuiString);



			} else if (kahouType.Contains ("nanban") == true) {
				
				
				sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
				
				string nanbanString = PlayerPrefs.GetString ("nanbanItem");
				
				List<string> nanbanList = new List<string> ();
				nanbanList = new List<string> (nanbanString.Split (delimiterChars));
				
				itemId = int.Parse (kahouType.Remove (0, 6));
				int qty = int.Parse (nanbanList [itemId - 1]);
				
				int remainQty = qty - sellQty;
				nanbanList [itemId - 1] = remainQty.ToString ();
				
				string newNanbanString = "";
				for (int i = 0; i < nanbanList.Count; i++) {
					
					if (i == 0) {
						newNanbanString = nanbanList [i];
					} else {
						newNanbanString = newNanbanString + "," + nanbanList [i];
					}
				}
				
				PlayerPrefs.SetString ("nanbanItem", newNanbanString);
			

			} else if (kahouType == "koueki") {
				sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
				deleteKouekiOrCyoutei (itemId, "koueki", sellQty);
			
			} else if (kahouType == "cyoutei") {
				sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
				deleteKouekiOrCyoutei (itemId, "cyoutei", sellQty);
			
			} else if (kahouType.Contains ("tech") == true) {

				sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);

				if (kahouType == "tech1") {
					//TP
					int qty = PlayerPrefs.GetInt ("transferTP", 0);
					int newQty = qty - sellQty;
					PlayerPrefs.SetInt ("transferTP", newQty);

				} else if (kahouType == "tech2") {
					//KB
					int qty = PlayerPrefs.GetInt ("transferKB", 0);
					int newQty = qty - sellQty;
					PlayerPrefs.SetInt ("transferKB", newQty);

				} else if (kahouType == "tech3") {
					//SNB
					int qty = PlayerPrefs.GetInt ("transferSNB", 0);
					int newQty = qty - sellQty;
					PlayerPrefs.SetInt ("transferSNB", newQty);

				}


			} else if (kahouType.Contains ("meisei") == true) {
				sellQty = int.Parse (GameObject.Find ("SellQtyValue").GetComponent<Text> ().text);
				int qty = PlayerPrefs.GetInt ("meisei");
				int newQty = qty - sellQty;
				PlayerPrefs.SetInt ("meisei", newQty);

            } else if(kahouType.Contains("shiro")) {
                sellQty = int.Parse(GameObject.Find("SellQtyValue").GetComponent<Text>().text);
                Shiro shiro = new Shiro();
                shiro.deleteShiro(itemId, sellQty);
            }


			//Add money
			money = PlayerPrefs.GetInt ("money");
			int addMoney = int.Parse(GameObject.Find ("GetMoneyValue").GetComponent<Text>().text); 
			money = money + addMoney;
            if (money < 0) {
                money = int.MaxValue;
            }
            PlayerPrefs.SetInt ("money", money);
			PlayerPrefs.SetBool ("questDailyFlg20",true);

			PlayerPrefs.Flush();
			
			//Reload
			GameObject.Find ("MoneyValue").GetComponent<Text> ().text = money.ToString();		
			GameObject.Find ("Dougu").GetComponent<DouguSoukoScene> ().OnClick ();	
			
			//Msg
			Message msg = new Message();
            string OKtext = "";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                OKtext = "Sold " + sellQty.ToString() + " " + kahouName  + ".";
            } else {
                OKtext = kahouName + "を" + sellQty.ToString() + "個売却致しました。";
            }
			msg.makeMessage(OKtext);

		}








	}

	public void reduceKahou(string target, int reduceKahouId){
		//Common Prametor
		char[] delimiterChars = {','};

		string availableBuguString = PlayerPrefs.GetString(target);
		string newList = "";
		if(availableBuguString != null && availableBuguString !=""){
			string[] available_list = availableBuguString.Split (delimiterChars);
			
			ArrayList newAvailableList = new ArrayList ();
			bool flag = false;
			
			for(int i=0;i<available_list.Length;i++){
				int tempKahouId = int.Parse(available_list[i]);
				
				if(reduceKahouId==tempKahouId){
					if(flag==false){
						flag = true;
					}else{
						newAvailableList.Add(tempKahouId);
					}
				}else{
					newAvailableList.Add(tempKahouId);
				}
			}
			//Set String
			string kahouForData="";
			for(int j=0;j<newAvailableList.Count;j++){
				if(j != newAvailableList.Count-1){
					kahouForData = kahouForData + newAvailableList[j] + ",";
				}else{
					kahouForData = kahouForData + newAvailableList[j];
				}
			}

			PlayerPrefs.SetString (target,kahouForData);
		}
	}

	public void deleteKouekiOrCyoutei(int itemId, string itemCd, int sellQty){

		string nowQty = PlayerPrefs.GetString (itemCd);
		List<string> nowQtyList = new List<string> ();
		char[] delimiterChars = {','};
		nowQtyList = new List<string> (nowQty.Split (delimiterChars));
		
		string newQty = "";
		if (itemId == 1) {
			//Ge
			int nowNo = int.Parse(nowQtyList[0]);
			int newNo = nowNo - sellQty;
			newQty = newNo.ToString() + "," + nowQtyList[1] + "," + nowQtyList[2];
			
		} else if (itemId == 2) {
			//Cyu
			int nowNo = int.Parse(nowQtyList[1]);
			int newNo = nowNo - sellQty;
			newQty = nowQtyList[0] + "," + newNo.ToString()  + "," + nowQtyList[2];
			
		} else if (itemId == 3) {
			//Jyo
			int nowNo = int.Parse(nowQtyList[2]);
			int newNo = nowNo - sellQty;
			newQty = nowQtyList[0] + "," + nowQtyList[1]  + "," + newNo.ToString();
		}
		if (newQty != null && newQty != "") {
			PlayerPrefs.SetString (itemCd, newQty);
			PlayerPrefs.Flush ();
		}
	}
}
