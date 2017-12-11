using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using System.Linq;

public class NewDaimyoDataMaker : MonoBehaviour {

	public void dataMake(bool busyoExitFlg, int newDaimyo, int newDaimyoBusyo, string heisyu, bool sameDaimyoFlg, int senarioId) {

        /*******************************/
        /*****      Base Value     *****/
        /*******************************/
        int oldSenarioId = PlayerPrefs.GetInt("senarioId");
        System.DateTime now = System.DateTime.Now;
		PlayerPrefs.SetString ("lasttime", now.ToString ());
        if(senarioId==1) {
            PlayerPrefs.SetString("yearSeason", "1572,1");
            PlayerPrefs.SetInt("syogunDaimyoId", 14);
        }else if(senarioId==2) {
            PlayerPrefs.SetString("yearSeason", "1582,2");
            PlayerPrefs.DeleteKey("syogunDaimyoId");
        }else if(senarioId==3) {
            PlayerPrefs.SetString("yearSeason", "1600,2");
            PlayerPrefs.DeleteKey("syogunDaimyoId");
        }else {
            PlayerPrefs.SetString("yearSeason", "1560,1");
            PlayerPrefs.SetInt("syogunDaimyoId", 14);            
        }		
		PlayerPrefs.DeleteKey("gameClearFlg");
		PlayerPrefs.DeleteKey("gameClearItemGetFlg");
		PlayerPrefs.DeleteKey("gameOverFlg");
		PlayerPrefs.DeleteKey("kuniClearedFlg");
        PlayerPrefs.DeleteKey("rengouFlg");
        PlayerPrefs.DeleteKey("rengouDaimyo");
        PlayerPrefs.DeleteKey("kokuninReject");

        /*******************************/
        /*****   Delete History    *****/
        /*******************************/
        PlayerPrefs.DeleteKey("TrackTotalKassenNo");
		PlayerPrefs.DeleteKey("TrackWinNo");
		PlayerPrefs.DeleteKey("TrackTettaiNo");
		PlayerPrefs.DeleteKey("TrackBiggestDaimyoId");
		PlayerPrefs.DeleteKey("TrackBiggestDaimyoHei");
		PlayerPrefs.DeleteKey("TrackMyBiggestHei");
		PlayerPrefs.DeleteKey("TrackNewBusyoHireNo");
		PlayerPrefs.DeleteKey("TrackEarnMoney");
		PlayerPrefs.DeleteKey("TrackGetMoneyNo");
		PlayerPrefs.DeleteKey("TrackGetHyourouNo");
		PlayerPrefs.DeleteKey("TrackGetSozaiNo");
		PlayerPrefs.DeleteKey("TrackBuildMoneyNo");
		PlayerPrefs.DeleteKey("TrackJyosyuNinmeiNo");
		PlayerPrefs.DeleteKey("TrackTabibitoNo");
		PlayerPrefs.DeleteKey("TrackIjinNo");
		PlayerPrefs.DeleteKey("HstNanbansen");
		PlayerPrefs.DeleteKey("TrackGaikouNo");
		PlayerPrefs.DeleteKey("TrackGaikouMoneyNo");
		PlayerPrefs.DeleteKey("TrackDoumeiNo");
		PlayerPrefs.DeleteKey("TrackCyouteiNo");
		PlayerPrefs.DeleteKey("TrackSyouninNo");
		PlayerPrefs.DeleteKey("TrackToubatsuNo");
		PlayerPrefs.DeleteKey("TrackBouryakuNo");
		PlayerPrefs.DeleteKey("TrackBouryakuSuccessNo");
		PlayerPrefs.DeleteKey("TrackCyouhouNo");
		PlayerPrefs.DeleteKey("TrackRyugenNo");
		PlayerPrefs.DeleteKey("TrackGihouHei");
		PlayerPrefs.DeleteKey("TrackCyouryakuNo");
		PlayerPrefs.DeleteKey("TrackLinkCutNo");
		PlayerPrefs.DeleteKey("TrackSyuppeiNo");


		/*******************************/
		/*****   Delete Temp Value *****/
		/*******************************/
		PlayerPrefs.DeleteKey("playerEngunList");
		PlayerPrefs.DeleteKey("enemyEngunList");
		PlayerPrefs.DeleteKey("playerKyoutouList");
		PlayerPrefs.DeleteKey("tempKyoutouList");
		PlayerPrefs.DeleteKey("keyHistory");
		PlayerPrefs.DeleteKey("metsubou");
        
        HPCounter deleteGunzeiScript = new HPCounter();
		for(int i=1; i<66; i++){
			string kuniTemp = "kuni" + i.ToString();
			PlayerPrefs.DeleteKey(kuniTemp);

			string jyosyuTemp = "jyosyu" + i.ToString();
			PlayerPrefs.DeleteKey(jyosyuTemp);
           
			string naiseiLoginDateTemp = "naiseiLoginDate" + i.ToString();
			PlayerPrefs.DeleteKey(naiseiLoginDateTemp);
			
			string naiseiTabibitoCounterTemp = "naiseiTabibitoCounter" + i.ToString();
			PlayerPrefs.DeleteKey(naiseiTabibitoCounterTemp);

			string cyouhouTemp = "cyouhou" + i.ToString();
			PlayerPrefs.DeleteKey(cyouhouTemp);

            string linkuctTmp = "linkcut" + i.ToString();
            PlayerPrefs.DeleteKey(linkuctTmp);

            string kokuninTmp = "kokunin" + i.ToString();
            PlayerPrefs.DeleteKey(kokuninTmp);
            
            //Delete Enemy Gunzei
            deleteGunzeiScript.deleteEnemyGunzeiData(i);
        }

        PlayerPrefs.DeleteKey("cyouhou");
		PlayerPrefs.DeleteKey("lastSeasonChangeTime");
		PlayerPrefs.DeleteKey("doneCyosyuFlg");
		PlayerPrefs.DeleteKey ("twiceHeiFlg");
		PlayerPrefs.DeleteKey ("rdmEventTimer");
		PlayerPrefs.DeleteKey ("fromNaiseiFlg");
		PlayerPrefs.DeleteKey ("fromKassenFlg");
		PlayerPrefs.DeleteKey ("kassenWinLoseFlee");
		PlayerPrefs.DeleteKey ("kessenFlg");
		PlayerPrefs.DeleteKey ("kessenHyourou");
		PlayerPrefs.DeleteKey ("winChecker");
        PlayerPrefs.DeleteKey("isAttackedFlg");
        PlayerPrefs.DeleteKey("isKessenFlg");


        /*******************************/
        /*****      Cyoutei & Bakuhu Value    *****/
        /*******************************/
        PlayerPrefs.DeleteKey ("cyoutekiDaimyo");
		PlayerPrefs.DeleteKey ("cyouteiPoint");		
		PlayerPrefs.DeleteKey ("bakuhuTobatsuDaimyoId");
		PlayerPrefs.DeleteKey ("soubujireiFlg");


        /*******************************/
        /*****      Busyo Value    *****/
        /*******************************/
        //Jyosyu Delete
        Entity_busyo_mst busyoMst = Resources.Load("Data/busyo_mst") as Entity_busyo_mst;
        for (int i=0; i< busyoMst.param.Count; i++) {
            int busyoId = i + 1;
            string daimyoTemp = "jyosyuHei" + busyoId;
            string daimyoTemp2 = "jyosyuBusyo" + busyoId;
            PlayerPrefs.DeleteKey(daimyoTemp);
            PlayerPrefs.DeleteKey(daimyoTemp2);
            
        }
        PlayerPrefs.Flush();

        char[] delimiterChars = { ',' };
		if (!sameDaimyoFlg) {
            //Old Daimyo
            //Delete Previous Daimyo Busyo in the case of it has never been gotten by Gacya
            Daimyo Daimyo = new Daimyo();
            int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
            int preDaimyoBusyoId = Daimyo.getDaimyoBusyoId(myDaimyo, senarioId);
           
            string gacyaDaimyoHst = PlayerPrefs.GetString ("gacyaDaimyoHst");
            Debug.Log(gacyaDaimyoHst);
			List<string> gacyaDaimyoHstList = new List<string> ();

			if (gacyaDaimyoHst != null && gacyaDaimyoHst != "") {
				if (gacyaDaimyoHst.Contains (",")) {
					gacyaDaimyoHstList = new List<string> (gacyaDaimyoHst.Split (delimiterChars));
				} else {
					gacyaDaimyoHstList.Add (gacyaDaimyoHst);
				}
			}
			if (!gacyaDaimyoHstList.Contains (preDaimyoBusyoId.ToString ())) {
				//delete daimyo
				//delete my busyo
				string myBusyo = PlayerPrefs.GetString ("myBusyo");                
				List<string> myBusyoList = new List<string> ();
				if (myBusyo.Contains (",")) {
					myBusyoList = new List<string> (myBusyo.Split (delimiterChars));
				} else {
					myBusyoList.Add (myBusyo);
				}

               
                myBusyoList.Remove (preDaimyoBusyoId.ToString ());
				string myNewBusyo = "";

				for (int i = 0; i < myBusyoList.Count; i++) {
					if (i == 0) {
						myNewBusyo = myBusyoList [i];
					} else {
						myNewBusyo = myNewBusyo + "," + myBusyoList [i];				
					}
				}
                Debug.Log(myBusyo + " > " + myNewBusyo);
				PlayerPrefs.SetString ("myBusyo", myNewBusyo);
				int myBusyoQty = PlayerPrefs.GetInt ("myBusyoQty");
				myBusyoQty = myBusyoQty - 1;
				PlayerPrefs.SetInt ("myBusyoQty", myBusyoQty);
				PlayerPrefs.Flush ();

				//delete jinkei
				int jinkeiMaxId = 4;
				int slotMaxId = 25;
                bool soudaisyoSetFlg = false;
				for (int i = 1; i <= jinkeiMaxId; i++) {
					for (int j = 1; j <= slotMaxId; j++) {
						string tmp = i.ToString () + "map" + j.ToString ();
						if (PlayerPrefs.HasKey (tmp)) {
							int busyoId = PlayerPrefs.GetInt (tmp);
							if (busyoId == preDaimyoBusyoId) {
								PlayerPrefs.DeleteKey (tmp);
							}else {
                                if(!soudaisyoSetFlg) {
                                    string sJinkeiId = "soudaisyo" + i.ToString();
                                    PlayerPrefs.SetInt(sJinkeiId, busyoId);
                                }
                            }
						}
					}
				}

                //kahou check
                string tempBusyo = "kahou" + preDaimyoBusyoId;
                string busyoKahou = PlayerPrefs.GetString(tempBusyo);
                if(busyoKahou != null && busyoKahou != "") {
                    string[] busyoKahouList = busyoKahou.Split(delimiterChars);
                    for (int k = 0; k < busyoKahouList.Length; k++) {
                        int kahouId = int.Parse(busyoKahouList[k]);
                        if(kahouId != 0) {
                            //back kahou data
                            string tmp = "";
                            if(k==0) {
                                tmp = "Bugu";
                            }else if(k==1) {
                                tmp = "Kabuto";
                            }else if (k == 2) {
                                tmp = "Gusoku";
                            }else if (k == 3) {
                                tmp = "Meiba";
                            }else if (k == 4) {
                                tmp = "Cyadougu";
                            }else if (k == 5) {
                                tmp = "Cyadougu";
                            }else if (k == 6) {
                                tmp = "Heihousyo";
                            }else if (k == 7) {
                                tmp = "Chishikisyo";
                            }

                            string temp = "available" + tmp;
                            string availableKahou = PlayerPrefs.GetString(temp);
                            if (availableKahou == null || availableKahou == "") {
                                availableKahou = kahouId.ToString();
                            }else {
                                availableKahou = availableKahou + "," + kahouId.ToString();
                            }
                            PlayerPrefs.SetString(temp, availableKahou);
                            PlayerPrefs.SetString(tempBusyo,"0,0,0,0,0,0,0,0");
                        }
                    }
                }                
			}

            //New Daimyo
			if (!busyoExitFlg) {
				//Player dosen't have this daimyo busyo

				//busyo
				int myBusyoQty = PlayerPrefs.GetInt ("myBusyoQty");
				myBusyoQty = myBusyoQty + 1;
				PlayerPrefs.SetInt ("myBusyoQty", myBusyoQty);

				string myBusyo = PlayerPrefs.GetString ("myBusyo");
				if (myBusyo != null && myBusyo != "") {
					myBusyo = myBusyo + "," + newDaimyoBusyo.ToString ();
				} else {
					myBusyo = newDaimyoBusyo.ToString ();
				}

				PlayerPrefs.SetString ("myBusyo", myBusyo);

				if (!PlayerPrefs.HasKey (newDaimyoBusyo.ToString ())) {
					PlayerPrefs.SetInt (newDaimyoBusyo.ToString (), 1);
					string tempHei = "hei" + newDaimyoBusyo.ToString ();
					string tempSenpou = "senpou" + newDaimyoBusyo.ToString ();
					string tempSaku = "saku" + newDaimyoBusyo.ToString ();
					string tempKahou = "kahou" + newDaimyoBusyo.ToString ();
					string tempExp = "exp" + newDaimyoBusyo.ToString ();

					string valueHei = heisyu + ":1:1:1";
					PlayerPrefs.SetString (tempHei, valueHei);
					PlayerPrefs.SetInt (tempSenpou, 1);
					PlayerPrefs.SetInt (tempSaku, 1);
					PlayerPrefs.SetString (tempKahou, "0,0,0,0,0,0,0,0");
					PlayerPrefs.SetInt (tempExp, 0);
				}

				//jinkei
				//if there is no busyo in active jinkei
				int jinkei = PlayerPrefs.GetInt ("jinkei");
				bool jinkeiBusyoExistFlg = false;
				for (int i = 1; i <= 25; i++) {
					string tmp = jinkei.ToString () + "map" + i.ToString ();
					if (PlayerPrefs.HasKey (tmp)) {
						jinkeiBusyoExistFlg = true;
					}
				}
				if (!jinkeiBusyoExistFlg) {
					string tmpMap = "";
					if (jinkei == 1) {
						tmpMap = jinkei.ToString () + "map11";
					} else if (jinkei == 2) {
						tmpMap = jinkei.ToString () + "map11";
					} else if (jinkei == 3) {
						tmpMap = jinkei.ToString () + "map14";
					} else if (jinkei == 4) {
						tmpMap = jinkei.ToString () + "map12";
					}
					PlayerPrefs.SetInt (tmpMap, newDaimyoBusyo);
				}
			}
		}

		//My Daimyo Busyo
		PlayerPrefs.SetInt("myDaimyo",newDaimyo);
		PlayerPrefs.SetInt("myDaimyoBusyo",newDaimyoBusyo);
		PlayerPrefs.DeleteKey ("usedBusyo");


        /*******************************/
        /*****    	  Kuni Value     *****/
        /*******************************/
        KuniInfo KuniInfo = new KuniInfo();        
        string newSeiryoku = KuniInfo.getDefaultSeiryoku(senarioId);        
        //string newSeiryoku = "1,2,3,4,5,6,7,8,3,4,9,10,12,11,13,14,15,16,3,17,18,17,19,8,19,19,20,21,22,23,24,25,26,27,28,29,30,31,31,32,33,34,35,35,36,37,38,38,38,38,31,31,31,39,40,41,41,41,41,42,43,44,45,45,46";
        PlayerPrefs.SetString ("seiryoku",newSeiryoku);
		List<string> seiryokuList = new List<string> ();
		seiryokuList = new List<string> (newSeiryoku.Split (delimiterChars));
		string newClearedKuni = "";

		for (int i=0; i<seiryokuList.Count; i++) {
			string tempDaimyoId = seiryokuList[i];
			if(tempDaimyoId == newDaimyo.ToString()){
				int kuniId = i + 1;
				if(newClearedKuni==null || newClearedKuni == ""){
					newClearedKuni = kuniId.ToString();
				}else{
					newClearedKuni = newClearedKuni + "," + kuniId.ToString();
				}
			}
		}
		PlayerPrefs.SetString ("clearedKuni",newClearedKuni);

		List<string> clearedKuniList = new List<string> ();
		if (newClearedKuni.Contains (",")) {
			clearedKuniList = new List<string> (newClearedKuni.Split (delimiterChars));
		} else {
			clearedKuniList.Add (newClearedKuni);
		}


		//New Open Kuni
		KuniInfo kuni = new KuniInfo ();
		PlayerPrefs.DeleteKey("openKuni");
		PlayerPrefs.Flush ();
		foreach(string kuniId in clearedKuniList){
			kuni.registerOpenKuni (int.Parse(kuniId));

			string temp = "kuni" + kuniId;
			PlayerPrefs.SetString (temp,"1,2,3,4,5,6,7,8,9,10");

			string tempNaisei = "naisei" + kuniId;
			if(!PlayerPrefs.HasKey(tempNaisei)){
				PlayerPrefs.SetString(tempNaisei,"1,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0");
			}
			PlayerPrefs.Flush ();
		}

		//Add Cleared Kuni to OpenKuni
		string tempOpenKuni = PlayerPrefs.GetString ("openKuni");
		tempOpenKuni = tempOpenKuni + "," + newClearedKuni;
		PlayerPrefs.SetString ("openKuni",tempOpenKuni);
		PlayerPrefs.Flush ();


		/*******************************/
		/*****     Cyouryaku Value    *****/
		/*******************************/
		string cyouryaku = PlayerPrefs.GetString ("cyouryaku");

		List<string> cyouryakuList = new List<string> ();
		if (cyouryaku != null && cyouryaku != "") {
			if (cyouryaku.Contains (",")) {	
				cyouryakuList = new List<string> (cyouryaku.Split (delimiterChars));
			} else {
				cyouryakuList.Add (cyouryaku);
			}
		}
		for (int i = 0; i < cyouryakuList.Count; i++) {
			PlayerPrefs.DeleteKey (cyouryakuList[i]);
		}
		PlayerPrefs.DeleteKey ("cyouryaku");


		/*******************************/
		/*****     Gaikou Value    *****/
		/*******************************/

		//My Gaikou
		Gaikou gaikou = new Gaikou ();
        List<string> daimyoList = new List<string>(seiryokuList);
        daimyoList.Distinct();

        for (int l=0; l< daimyoList.Count; l++){
            int otherDaimyo = int.Parse(daimyoList[l]);
			int value = gaikou.getGaikouValue(newDaimyo, otherDaimyo,senarioId);
			string temp = "gaikou" + otherDaimyo.ToString();
			PlayerPrefs.SetInt (temp, value);

			string metsubouTemp = "metsubou" + otherDaimyo.ToString();
			PlayerPrefs.DeleteKey (metsubouTemp);
		}

		//Other Daimyo Gaikou
		for(int x=0; x< daimyoList.Count; x++ ){
            int daimyo1 = int.Parse(daimyoList[x]);
            for (int y=0; y< daimyoList.Count; y++){
                int daimyo2 = int.Parse(daimyoList[y]);
                if (daimyo1 != daimyo2) {
					string temp = daimyo1.ToString() + "gaikou" + daimyo2.ToString();
					string temp2 = daimyo1.ToString() + "key" + daimyo2.ToString();
					PlayerPrefs.DeleteKey(temp);
					PlayerPrefs.DeleteKey(temp2);
				}
			}
		}

		/*******************************/
		/*****       Shisya        *****/
		/*******************************/
		PlayerPrefs.DeleteKey("shisyaFlg");
		PlayerPrefs.DeleteKey("shisya1");
		PlayerPrefs.DeleteKey("shisya2");
		PlayerPrefs.DeleteKey("shisya3");
		PlayerPrefs.DeleteKey("shisya4");
		PlayerPrefs.DeleteKey("shisya5");
		PlayerPrefs.DeleteKey("shisya6");
		PlayerPrefs.DeleteKey("shisya7");
		PlayerPrefs.DeleteKey("shisya8");
		PlayerPrefs.DeleteKey("shisya9");
		PlayerPrefs.DeleteKey("shisya10");
		PlayerPrefs.DeleteKey("shisya11");
		PlayerPrefs.DeleteKey("shisya12");
		PlayerPrefs.DeleteKey("shisya13");
		PlayerPrefs.DeleteKey("shisya14");
		PlayerPrefs.DeleteKey("shisya15");
		PlayerPrefs.DeleteKey("shisya16");
		PlayerPrefs.DeleteKey("shisya17");
		PlayerPrefs.DeleteKey("shisya18");
		PlayerPrefs.DeleteKey("shisya19");
		PlayerPrefs.DeleteKey("shisya20");
		PlayerPrefs.DeleteKey("shisya21");
        PlayerPrefs.DeleteKey("shisya22");



        /*******************************/
        /*****       Doumei        *****/
        /*******************************/
        //Delete doumei history
        Entity_daimyo_mst daimyoMst = Resources.Load ("Data/daimyo_mst") as Entity_daimyo_mst;
		for(int i=0; i<daimyoMst.param.Count; i++){
			int daimyoId = daimyoMst.param[i].daimyoId;
			string temp = "doumei" + daimyoId;
			PlayerPrefs.DeleteKey (temp); 
		}
		PlayerPrefs.Flush ();


		PlayerPrefs.DeleteKey("doumei");
		string newMyDoumei = "";
		Entity_doumei_mst doumeiMst = Resources.Load ("Data/doumei_mst") as Entity_doumei_mst;        
		for(int i=0; i < doumeiMst.param.Count; i++){            
            if (senarioId == doumeiMst.param[i].senarioId) {
			    int daimyoId1 = doumeiMst.param[i].doumeiSrc;
			    int daimyoId2 = doumeiMst.param[i].doumeiDst;
              
			    if(daimyoId1 == newDaimyo){
				    if(newMyDoumei != null && newMyDoumei !=""){
					    newMyDoumei = newMyDoumei + "," + daimyoId2;
				    }else{
					    newMyDoumei = daimyoId2.ToString();
				    }
			    }else{
				    string temp = "doumei" + daimyoId1;
				    string previous = PlayerPrefs.GetString(temp);

				    if(previous != null && previous !=""){
					    previous = previous + "," + daimyoId2;
					    PlayerPrefs.SetString (temp,previous);
				    }else{
					    PlayerPrefs.SetString (temp, daimyoId2.ToString());
				    }
			    }

                if (daimyoId2 == newDaimyo) {
                    if (newMyDoumei != null && newMyDoumei != "") {
                        newMyDoumei = newMyDoumei + "," + daimyoId1;
                    }
                    else {
                        newMyDoumei = daimyoId1.ToString();
                    }
                }else {
                    string temp = "doumei" + daimyoId2;
                    string previous = PlayerPrefs.GetString(temp);

                    if (previous != null && previous != "") {
                        previous = previous + "," + daimyoId1;
                        PlayerPrefs.SetString(temp, previous);
                    }else {
                        PlayerPrefs.SetString(temp, daimyoId1.ToString());
                    }
                }


            }
		}
		PlayerPrefs.SetString ("doumei",newMyDoumei);
        PlayerPrefs.SetInt("senarioId", senarioId);
        if (senarioId == 1 && newDaimyo == 1) {
            PlayerPrefs.SetBool("rengouFlg", true);
            PlayerPrefs.SetString("rengouDaimyo", "5,6,8,10,13,14,31");
        }

        PlayerPrefs.Flush ();


		Application.LoadLevel ("mainStage");
	}
}
