using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System.Collections.Generic;

public class DoBuy : MonoBehaviour {

	public string item;
	public int qty;
	public int amt;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		int hyourou = PlayerPrefs.GetInt ("hyourou");
		Message msg = new Message ();

		if (hyourou >= 5) {
			int money = PlayerPrefs.GetInt("money");
			int payMoney = int.Parse(GameObject.Find("MoneyAmt").GetComponent<Text>().text);
			if(money >= payMoney){
				/***OK***/
				audioSources [3].Play ();

				//Reduce Hyourou
				int newHyourou = hyourou - 5;
				PlayerPrefs.SetInt ("hyourou", newHyourou);
				GameObject.Find ("HyourouCurrentValue").GetComponent<Text> ().text = newHyourou.ToString ();
				
				//Reduce Money
				int newMoney = money - payMoney;
				PlayerPrefs.SetInt ("money", newMoney);
				GameObject.Find ("MoneyValue").GetComponent<Text> ().text = newMoney.ToString ();


				//Add Item
				char[] delimiterChars = {','};
				int buyQty = int.Parse(GameObject.Find("BuyMenu").transform.FindChild("Qty").GetComponent<Text>().text);

				if(item.Contains("Cyouhei")==true){
					if(item.Contains("YR")==true){
						string cyouheiYRString = PlayerPrefs.GetString("cyouheiYR");
						string[] cyouheiYR_list = cyouheiYRString.Split (delimiterChars);
						string newCyouheiYRString = "";

						if(item == "CyouheiYR1"){
							int newQty = 0;
							newQty = int.Parse(cyouheiYR_list[0]) + buyQty;
							newCyouheiYRString = newQty.ToString() + "," + cyouheiYR_list[1] + "," + cyouheiYR_list[2];

						}else if(item == "CyouheiYR2"){
							int newQty = 0;
							newQty = int.Parse(cyouheiYR_list[1]) + buyQty;
							newCyouheiYRString = cyouheiYR_list[0] + "," + newQty.ToString() + "," + cyouheiYR_list[2];
							
						}else if(item == "CyouheiYR3"){
							int newQty = 0;
							newQty = int.Parse(cyouheiYR_list[2]) + buyQty;
							newCyouheiYRString = cyouheiYR_list[0] + "," + cyouheiYR_list[1] + "," + newQty.ToString();
							
						}
                        if (newCyouheiYRString != "") {
                            PlayerPrefs.SetString ("cyouheiYR",newCyouheiYRString);
                        }
					}else if(item.Contains("KB")==true){
						string cyouheiKBString = PlayerPrefs.GetString("cyouheiKB");
						string[] cyouheiKB_list = cyouheiKBString.Split (delimiterChars);
						string newCyouheiKBString = "";
						
						if(item == "CyouheiKB1"){
							int newQty = 0;
							newQty = int.Parse(cyouheiKB_list[0]) + buyQty;
							newCyouheiKBString = newQty.ToString() + "," + cyouheiKB_list[1] + "," + cyouheiKB_list[2];
							
							
						}else if(item == "CyouheiKB2"){
							int newQty = 0;
							newQty = int.Parse(cyouheiKB_list[1]) + buyQty;
							newCyouheiKBString = cyouheiKB_list[0] + "," + newQty.ToString() + "," + cyouheiKB_list[2];
							
						}else if(item == "CyouheiKB3"){
							int newQty = 0;
							newQty = int.Parse(cyouheiKB_list[2]) + buyQty;
							newCyouheiKBString = cyouheiKB_list[0] + "," + cyouheiKB_list[1] + "," + newQty.ToString();
							
						}
                        if (newCyouheiKBString != "") {
                            PlayerPrefs.SetString ("cyouheiKB",newCyouheiKBString);
                        }
					}else if(item.Contains("TP")==true){
						string cyouheiTPString = PlayerPrefs.GetString("cyouheiTP");
						string[] cyouheiTP_list = cyouheiTPString.Split (delimiterChars);
						string newCyouheiTPString = "";
						
						if(item == "CyouheiTP1"){
							int newQty = 0;
							newQty = int.Parse(cyouheiTP_list[0]) + buyQty;
							newCyouheiTPString = newQty.ToString() + "," + cyouheiTP_list[1] + "," + cyouheiTP_list[2];
							
							
						}else if(item == "CyouheiTP2"){
							int newQty = 0;
							newQty = int.Parse(cyouheiTP_list[1]) + buyQty;
							newCyouheiTPString = cyouheiTP_list[0] + "," + newQty.ToString() + "," + cyouheiTP_list[2];
							
						}else if(item == "CyouheiTP3"){
							int newQty = 0;
							newQty = int.Parse(cyouheiTP_list[2]) + buyQty;
							newCyouheiTPString = cyouheiTP_list[0] + "," + cyouheiTP_list[1] + "," + newQty.ToString();
							
						}
                        if (newCyouheiTPString != "") {
                            PlayerPrefs.SetString ("cyouheiTP",newCyouheiTPString);
                        }
					}else if(item.Contains("YM")==true){
						string cyouheiYMString = PlayerPrefs.GetString("cyouheiYM");
						string[] cyouheiYM_list = cyouheiYMString.Split (delimiterChars);
						string newCyouheiYMString = "";
						
						if(item == "CyouheiYM1"){
							int newQty = 0;
							newQty = int.Parse(cyouheiYM_list[0]) + buyQty;
							newCyouheiYMString = newQty.ToString() + "," + cyouheiYM_list[1] + "," + cyouheiYM_list[2];
							
							
						}else if(item == "CyouheiYM2"){
							int newQty = 0;
							newQty = int.Parse(cyouheiYM_list[1]) + buyQty;
							newCyouheiYMString = cyouheiYM_list[0] + "," + newQty.ToString() + "," + cyouheiYM_list[2];
							
						}else if(item == "CyouheiYM3"){
							int newQty = 0;
							newQty = int.Parse(cyouheiYM_list[2]) + buyQty;
							newCyouheiYMString = cyouheiYM_list[0] + "," + cyouheiYM_list[1] + "," + newQty.ToString();
							
						}
                        if (newCyouheiYMString != "") {
                            PlayerPrefs.SetString ("cyouheiYM",newCyouheiYMString);
                        }
					}
					
				
				} else if(item.Contains("Hidensyo")==true){
					if(item == "Hidensyo1"){
						int newQty = 0;
						int HidensyoQty = PlayerPrefs.GetInt("hidensyoGe");
						newQty = HidensyoQty + buyQty;
						PlayerPrefs.SetInt ("hidensyoGe",newQty);
						
					}else if(item == "Hidensyo2"){
						int newQty = 0;
						int HidensyoQty = PlayerPrefs.GetInt("hidensyoCyu");
						newQty = HidensyoQty + buyQty;
						PlayerPrefs.SetInt ("hidensyoCyu",newQty);
						
					}else if(item == "Hidensyo3"){
						int newQty = 0;
						int HidensyoQty = PlayerPrefs.GetInt("hidensyoJyo");
						newQty = HidensyoQty + buyQty;
						PlayerPrefs.SetInt ("hidensyoJyo",newQty);
						
					}
				}else if(item.Contains("Shinobi")==true){
					if(item == "Shinobi1"){
						int newQty = 0;
						int shinobiQty = PlayerPrefs.GetInt("shinobiGe");
						newQty = shinobiQty + buyQty;
						PlayerPrefs.SetInt ("shinobiGe",newQty);
						
					}else if(item == "Shinobi2"){
						int newQty = 0;
						int shinobiQty = PlayerPrefs.GetInt("shinobiCyu");
						newQty = shinobiQty + buyQty;
						PlayerPrefs.SetInt ("shinobiCyu",newQty);
						
					}else if(item == "Shinobi3"){
						int newQty = 0;
						int shinobiQty = PlayerPrefs.GetInt("shinobiJyo");
						newQty = shinobiQty + buyQty;
						PlayerPrefs.SetInt ("shinobiJyo",newQty);
						
					}

				} else if(item.Contains("Kanjyo")==true){
					string kanjyoString = PlayerPrefs.GetString("kanjyo");
					string[] kanjyo_list = kanjyoString.Split (delimiterChars);
					string newKanjyoString = "";
					
					if(item == "Kanjyo1"){
						int newQty = 0;
						newQty = int.Parse(kanjyo_list[0]) + buyQty;
						newKanjyoString = newQty.ToString() + "," + kanjyo_list[1] + "," + kanjyo_list[2];
						
					}else if(item == "Kanjyo2"){
						int newQty = 0;
						newQty = int.Parse(kanjyo_list[1]) + buyQty;
						newKanjyoString = kanjyo_list[0] + "," + newQty.ToString() + "," + kanjyo_list[2];
						
					}else if(item == "Kanjyo3"){
						int newQty = 0;
						newQty = int.Parse(kanjyo_list[2]) + buyQty;
						newKanjyoString = kanjyo_list[0] + "," + kanjyo_list[1] + "," + newQty.ToString();
						
					}
					PlayerPrefs.SetString ("kanjyo",newKanjyoString);
				}
				PlayerPrefs.SetBool ("questDailyFlg27",true);
				PlayerPrefs.Flush();

				MainStageController mainStage = new MainStageController();
				mainStage.questExtension();

				//Closing
				GameObject.Find("SerihuText").GetComponent<Text>().text = msg.getMessage(11);

				//Message
                msg.makeMessage (msg.getMessage(12));


			}else{
				//Message
				audioSources [4].Play ();
				msg.makeMessage (msg.getMessage(6));
			}
		} else {
			//Message
			audioSources [4].Play ();
            //msg.makeMessage (msg.getMessage(7));
            msg.hyourouMovieMessage();
        }
	}
}
