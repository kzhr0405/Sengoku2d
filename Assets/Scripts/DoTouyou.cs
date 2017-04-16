using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class DoTouyou : MonoBehaviour {

	public int busyoId;
	public string busyoName;
	public string heisyu;
	public int sequence;
	public string rank;
	public int senpouId = 0;
	public bool daimyoFlg;

	public void OnClick () {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();


		//Limit Check
		int stockLimit = PlayerPrefs.GetInt("stockLimit");
		int myBusyoQty = PlayerPrefs.GetInt("myBusyoQty");
		char[] delimiterChars = {','};

		if (myBusyoQty + 1 > stockLimit) {
			//Error
			audioSources [4].Play ();
			Message msg = new Message();
            string Text = "";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                Text = "You can hire samurai by " + stockLimit.ToString()+ " members.";
            }else {
                Text = "現在の国力では登用出来る武将数は" + stockLimit.ToString()+ "人までですぞ。";
            }
			msg.makeMessage (Text);

		} else {
			audioSources [3].Play ();
			audioSources [7].Play ();

			//Track
			int TrackNewBusyoHireNo = PlayerPrefs.GetInt("TrackNewBusyoHireNo",0);
			TrackNewBusyoHireNo = TrackNewBusyoHireNo + 1;
			PlayerPrefs.SetInt("TrackNewBusyoHireNo",TrackNewBusyoHireNo);


			/*Add zukan & gacya History Start*/
			string zukanBusyoHst = PlayerPrefs.GetString ("zukanBusyoHst");
			if(zukanBusyoHst!=null && zukanBusyoHst != ""){
				zukanBusyoHst = zukanBusyoHst + "," + busyoId.ToString();
			}else{
				zukanBusyoHst = busyoId.ToString();
			}
			PlayerPrefs.SetString ("zukanBusyoHst",zukanBusyoHst);

			//Daimyo Busyo History
			Daimyo daimyo = new Daimyo();
			if (daimyo.daimyoBusyoCheck (busyoId)) {
				string gacyaDaimyoHst = PlayerPrefs.GetString ("gacyaDaimyoHst");
				if (gacyaDaimyoHst != null && gacyaDaimyoHst != "") {
					gacyaDaimyoHst = gacyaDaimyoHst + "," + busyoId.ToString ();
				} else {
					gacyaDaimyoHst = busyoId.ToString ();
				}
				PlayerPrefs.SetString ("gacyaDaimyoHst", gacyaDaimyoHst);
			}

			//sequence
			string sequenceString = "";
			if (sequence == 1) {
				sequenceString = "1,0,0";
			} else if (sequence == 2) {
				sequenceString = "0,1,0";
			} else if (sequence == 3) {
				sequenceString = "0,0,1";
			}
			PlayerPrefs.SetString ("touyouHst", sequenceString);

			if(rank=="S"){
				PlayerPrefs.SetBool ("questSpecialFlg0",true);
			}else if(rank == "A"){
				PlayerPrefs.SetBool ("questSpecialFlg1",true);
			}
			PlayerPrefs.Flush ();
            /*Add zukan & gacya History End*/


            //My Busyo Exist Check
            string myBusyoString = PlayerPrefs.GetString("myBusyo");
            List<string> myBusyoList = new List<string>();
            char[] delimiterChars2 = { ',' };
            if (myBusyoString.Contains(",")) {
                myBusyoList = new List<string>(myBusyoString.Split(delimiterChars));
            }else {
                myBusyoList.Add(myBusyoString);
            }

            if (myBusyoList.Contains(busyoId.ToString())) {

                //add lv
                string addLvTmp = "addlv" + busyoId.ToString();
                int addLvValue = 0;
                if (PlayerPrefs.HasKey(addLvTmp)) {
                    addLvValue = PlayerPrefs.GetInt(addLvTmp);
                    addLvValue = addLvValue + 1;
                    if(addLvValue >= 100) {
                        addLvValue = 100;
                    }
                }else {
                    addLvValue = 1;
                }

                if(addLvValue<100) {
                    PlayerPrefs.SetInt(addLvTmp,addLvValue);
                    PlayerPrefs.Flush();

                    //View Message Box
                    Destroy(GameObject.Find("board(Clone)"));
                    Destroy(GameObject.Find("Back(Clone)"));

                    MessageBusyo msg = new MessageBusyo();
                    string type = "touyou";
                    string msgText = "";
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        msgText = "Max Lv of " + busyoName + " increased.";
                    }else {
                        msgText = busyoName + "の最大レベルが1上がりました。";
                    }
                    msg.makeMessage(msgText, busyoId, type);
                } else {
                    
                    //Lv up
                    int currentLv = PlayerPrefs.GetInt (busyoId.ToString ());
                    int maxLv = 100 + addLvValue;

                    int newLv = 0;
                    string lvUpText = "";

                    //Already Lv Max
                    if (currentLv == maxLv) {
                        newLv = currentLv;
                        int busyoDama = 0;
                        if (rank == "S") {
                            busyoDama = 200;
                        } else if (rank == "A") {
                            busyoDama = 50;
                        } else if (rank == "B") {
                            busyoDama = 20;
                        } else if (rank == "C") {
                            busyoDama = 10;
                        }

                        int myBusyoDama = PlayerPrefs.GetInt ("busyoDama");
                        myBusyoDama = myBusyoDama + busyoDama;
                        PlayerPrefs.SetInt ("busyoDama", myBusyoDama);
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            lvUpText = "You got " + busyoDama + " stone.";
                        }else {
                            lvUpText = "武将珠" + busyoDama + "個を贈呈します。";
                        }
                        GameObject.Find ("BusyoDamaValue").GetComponent<Text> ().text = myBusyoDama.ToString ();

                    } else {
                        newLv = currentLv + 1;
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            lvUpText = busyoName + " Lv was increased.";
                        }else {
                            lvUpText = busyoName + "をレベルアップしました。";
                        }
                    }
                    PlayerPrefs.SetInt (busyoId.ToString (), newLv);

                    if (currentLv != maxLv) {
                        string exp = "exp" + busyoId.ToString ();
                        Exp expCalc = new Exp ();
                        int totalExp = expCalc.getExpforNextLv (currentLv);
                        PlayerPrefs.SetInt (exp, totalExp);
                    }

                    //View Message Box
                    Destroy (GameObject.Find ("board(Clone)"));
                    Destroy (GameObject.Find ("Back(Clone)"));

                    MessageBusyo msg = new MessageBusyo ();

                    string type = "touyou";
                    msg.makeMessage (lvUpText, busyoId, type);
                    
                }
            } else {

                int existCheck = PlayerPrefs.GetInt(busyoId.ToString());
                if (existCheck != 0 && existCheck != null) {
                    //my Busyo not contain but player used him before daimyo was changed
                    if (myBusyoString == null || myBusyoString == "") {
                        myBusyoString = busyoId.ToString();
                    }else {
                        myBusyoString = myBusyoString + "," + busyoId.ToString();
                    }
                    PlayerPrefs.SetString("myBusyo", myBusyoString);

                    //Add Qty
                    myBusyoQty = myBusyoQty + 1;
                    PlayerPrefs.SetInt("myBusyoQty", myBusyoQty);

                    //View Message Box
                    Destroy(GameObject.Find("board(Clone)"));
                    Destroy(GameObject.Find("Back(Clone)"));

                    MessageBusyo msg = new MessageBusyo();
                    string touyouuText = "";
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        touyouuText = "We hired " + busyoName + ".";
                    }else {
                        touyouuText = busyoName + "を登用しました。";
                    }
                    string type = "touyou";
                    msg.makeMessage(touyouuText, busyoId, type);

                }else {                    
                    //Add Completely New Data
                    if (myBusyoString == null || myBusyoString == "") {
					    myBusyoString = busyoId.ToString ();
				    } else {
					    myBusyoString = myBusyoString + "," + busyoId.ToString ();
				    }
				    PlayerPrefs.SetString ("myBusyo", myBusyoString);
				    PlayerPrefs.SetInt (busyoId.ToString (), 1);
				
				    string hei = "hei" + busyoId.ToString ();
				    string heiValue = heisyu + ":1:1:1";
				    PlayerPrefs.SetString (hei, heiValue);
				
				    string senpou = "senpou" + busyoId.ToString ();
				    PlayerPrefs.SetInt (senpou, 1); //Lv

				    string saku = "saku" + busyoId.ToString ();
				    PlayerPrefs.SetInt (saku, 1); //Lv

				    string kahou = "kahou" + busyoId.ToString ();
				    PlayerPrefs.SetString (kahou, "0,0,0,0,0,0,0,0");
				
				    string exp = "exp" + busyoId.ToString ();
				    PlayerPrefs.SetInt (exp, 0);

				    //Add Qty
				    myBusyoQty = myBusyoQty + 1;
				    PlayerPrefs.SetInt("myBusyoQty",myBusyoQty);

				    //View Message Box
				    Destroy (GameObject.Find ("board(Clone)"));
				    Destroy (GameObject.Find ("Back(Clone)"));
				
				    MessageBusyo msg = new MessageBusyo ();
                    string touyouuText = "";
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        touyouuText = "We hired " + busyoName + ".";
                    }else {
                        touyouuText = busyoName + "を登用しました。";
                    }
                    string type = "touyou"; 
				    msg.makeMessage (touyouuText, busyoId, type);
                }
			}
		}
		PlayerPrefs.Flush ();

        //Close Process
        string gacyaHst = "";
        if (Application.loadedLevelName == "tutorialTouyou") {
            gacyaHst = "4,16,52";

            //Set Parametor
            PlayerPrefs.SetInt("tutorialId", 9);
            PlayerPrefs.Flush();

            TextController txtScript = GameObject.Find("TextBoard").transform.FindChild("Text").GetComponent<TextController>();
            txtScript.SetText(8);
            txtScript.SetNextLine();
            txtScript.tutorialId = 8;
            txtScript.actOnFlg = false;
            
            //Center View
            GameObject centerView = GameObject.Find("CenterView").gameObject;
            centerView.transform.SetParent(GameObject.Find("tFinished").transform);

        } else { 
            gacyaHst = PlayerPrefs.GetString("gacyaHst");
        }
        string[] tokens = gacyaHst.Split(delimiterChars);
        int[] hitBusyo = Array.ConvertAll<string, int>(tokens, int.Parse);
		
		Gacya viewBusyo = new Gacya();
		viewBusyo.viewBusyo(hitBusyo, false);
	
	}




}
