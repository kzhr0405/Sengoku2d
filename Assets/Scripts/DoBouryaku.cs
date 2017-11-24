using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class DoBouryaku : MonoBehaviour {

	public int itemQty = 0;
	public string itemRank = "";
	public GameObject Gunzei;
	public int maxReduceValue = 8;

	public void OnClick(){
		
		Message msg = new Message();
		Gaikou gaikou = new Gaikou ();
		DoGaikou yukouChange = new DoGaikou ();
		int nowHyourou = PlayerPrefs.GetInt ("hyourou");
		CloseBoard closeScript = GameObject.Find ("close").GetComponent<CloseBoard> ();
		int daimyoBusyoAtk = closeScript.daimyoBusyoAtk;
		int daimyoBusyoDfc = closeScript.daimyoBusyoDfc;
		int daimyoId = closeScript.daimyoId;
		int kuniId = closeScript.kuniId;
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
        int langId = PlayerPrefs.GetInt("langId");
        int senarioId = PlayerPrefs.GetInt("senarioId");

        if (nowHyourou >= 5){

			//Track
			int TrackBouryakuNo = PlayerPrefs.GetInt("TrackBouryakuNo",0);
			TrackBouryakuNo = TrackBouryakuNo + 1;
			PlayerPrefs.SetInt ("TrackBouryakuNo", TrackBouryakuNo);

			if (name == "DoGihouBtn") {

				//Reduce Hyourou
				reduceHyourou ();

				//Reduce Shinobi
				//Ratio
				//Ge 5-15%, Cyu 15-30%, Jyo 30-50%
				int randomPercent = 0;
				int newQty = itemQty - 1;

				if(itemRank == "Ge"){
					randomPercent = UnityEngine.Random.Range(5,15);
					PlayerPrefs.SetInt ("shinobiGe",newQty);
				}else if(itemRank == "Cyu"){
					randomPercent = UnityEngine.Random.Range(15,30);
					PlayerPrefs.SetInt ("shinobiCyu",newQty);

				}else if(itemRank == "Jyo"){
					randomPercent = UnityEngine.Random.Range(30,50);
					PlayerPrefs.SetInt ("shinobiJyo",newQty);
				}

				float ratio = (float)randomPercent;
				float percent = Random.value;
				percent = percent * 100;

				if(percent <= ratio){
					//OK
					audioSources [3].Play ();

					//Track
					int TrackBouryakuSuccessNo = PlayerPrefs.GetInt("TrackBouryakuSuccessNo",0);
					TrackBouryakuSuccessNo = TrackBouryakuSuccessNo + 1;
					PlayerPrefs.SetInt ("TrackBouryakuSuccessNo", TrackBouryakuSuccessNo);

					//Delete Gunzei
					Destroy(Gunzei);

					//Delete Key
					string gunzeiKey = Gunzei.name;
					PlayerPrefs.DeleteKey(gunzeiKey);

					//Delete Key History
					char[] delimiterChars = {','};
					string keyHistory = PlayerPrefs.GetString ("keyHistory");
					List<string> keyHistoryList = new List<string>();
					if (keyHistory != null && keyHistory != "") {
						if(keyHistory.Contains(",")){
							keyHistoryList = new List<string> (keyHistory.Split (delimiterChars));
						}else{
							keyHistoryList.Add(keyHistory);
						}
					}
					keyHistoryList.Remove(gunzeiKey);
					string newKeyHistory = "";
					for(int i=0; i<keyHistoryList.Count; i++){
						if(i==0){
							newKeyHistory = keyHistoryList[i];
						}else{
							newKeyHistory = newKeyHistory + "," + keyHistoryList[i];
						}
					}
					PlayerPrefs.SetString("keyHistory",newKeyHistory);

					//Message
					PlayerPrefs.SetBool ("questDailyFlg33",true);

					//Extension Mark Handling
					MainStageController main = new MainStageController();
					main.questExtension();


					int TrackGihouHei = PlayerPrefs.GetInt("TrackGihouHei",0);
					int hei = Gunzei.GetComponent<Gunzei> ().myHei;
					TrackGihouHei = TrackGihouHei + hei;
					PlayerPrefs.SetInt ("TrackGihouHei", TrackGihouHei);

					string daimyoName = Gunzei.GetComponent<Gunzei>().srcDaimyoName;
                    string OKtext = "";
                    if (langId == 2) {
                        OKtext = "My lord, misreport was successful. \n" + daimyoName + " army withdrawn.";
                    }else {
                        OKtext = "御屋形様、偽報に成功しましたぞ。\n" + daimyoName + "の軍勢が退却します。";
                    }   
					msg.makeMessage (OKtext);

				}else{
					//NG
					audioSources [4].Play ();

					int nowYukoudo = gaikou.getMyGaikou(daimyoId);
					int newYukoudo = gaikou.downMyGaikou(daimyoId,nowYukoudo,maxReduceValue);
					int reduceYukoudo = nowYukoudo - newYukoudo;
					GameObject.Find ("YukouValue").GetComponent<Text> ().text = newYukoudo.ToString ();

                    //Message
                    string NGtext = "";
                    if (langId == 2) {
                        NGtext = "My lord, I'm sorry. failed misreport. \n Friendship decreased " + reduceYukoudo + " point";
                    }else {
                        NGtext = "申し訳御座りませぬ。偽報に失敗しましたぞ。\n友好度が" + reduceYukoudo + "下がりますぞ。";
                    }
                        
					msg.makeMessage (NGtext);

					yukouChange.downYukouOnIcon (daimyoId,newYukoudo);

				}
				PlayerPrefs.Flush();

				//Back
				GameObject.Find ("return").GetComponent<MenuReturn> ().OnClick ();



			}else if(name == "DoRyugenBtn"){
				//Reduce Hyourou
				reduceHyourou ();

				//Ratio
				//Ge 10-20%, Cyu 20-40%, Jyo 40-70%
				float randomPercent = 0;
				int newQty = itemQty - 1;
				
				if(itemRank == "Ge"){
					float tempRandomPercent = (200 - daimyoBusyoDfc)/4;
					float tempValue = UnityEngine.Random.Range(0.5f,1.5f);
					randomPercent = tempRandomPercent * tempValue;
					PlayerPrefs.SetInt ("shinobiGe",newQty);

				}else if(itemRank == "Cyu"){
					float tempRandomPercent = (200 - daimyoBusyoDfc)/2;
					float tempValue = UnityEngine.Random.Range(0.8f,1.2f);
					randomPercent = tempRandomPercent * tempValue;
					PlayerPrefs.SetInt ("shinobiCyu",newQty);

					
				}else if(itemRank == "Jyo"){
					float tempRandomPercent = (200 - daimyoBusyoDfc);
					float tempValue = UnityEngine.Random.Range(0.9f,1.1f);
					randomPercent = tempRandomPercent * tempValue;
					PlayerPrefs.SetInt ("shinobiJyo",newQty);

				}

				float percent = Random.value;
				percent = percent * 100;

				if(percent <= randomPercent){
					//Success
					audioSources [3].Play ();

					//Track
					int TrackBouryakuSuccessNo = PlayerPrefs.GetInt("TrackBouryakuSuccessNo",0);
					TrackBouryakuSuccessNo = TrackBouryakuSuccessNo + 1;
					PlayerPrefs.SetInt ("TrackBouryakuSuccessNo", TrackBouryakuSuccessNo);

					int TrackRyugenNo = PlayerPrefs.GetInt("TrackRyugenNo",0);
					TrackRyugenNo = TrackRyugenNo + 1;
					PlayerPrefs.SetInt ("TrackRyugenNo", TrackRyugenNo);

					//Daimyo Id
					int srcDaimyoId = GameObject.Find ("close").GetComponent<CloseBoard> ().daimyoId;

					//Seiryoku List
					string seiryoku = PlayerPrefs.GetString ("seiryoku");
					List<string> seiryokuList = new List<string> ();
					char[] delimiterChars = {','};
					seiryokuList = new List<string> (seiryoku.Split (delimiterChars));

					//src daimyo kuni list
					List<string> srcDaimyoKuniList = new List<string> ();
					for(int i=0; i<seiryokuList.Count;i++){
						string tempDaimyoId = seiryokuList[i];

						if(tempDaimyoId == srcDaimyoId.ToString()){
							int temp = i + 1;
							srcDaimyoKuniList.Add(temp.ToString());
						}
					}

					//src daimyo open kuni list
					KuniInfo kuni = new KuniInfo();
					List<int> openKuniList = new List<int>();
					for(int j=0; j<srcDaimyoKuniList.Count; j++){
						openKuniList.AddRange(kuni.getMappingKuni(int.Parse(srcDaimyoKuniList[j])));
					}

					//Target Daimyo (exculde this src daimyo & mydaimyo)
					int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
					List<int> dstDaimyoList = new List<int>();

					for(int k=0; k<openKuniList.Count;k++){
						int temp = openKuniList[k] - 1;
						int tempDaimyoId = int.Parse(seiryokuList[temp]);
						if(tempDaimyoId != myDaimyo && tempDaimyoId != srcDaimyoId){
							if(!dstDaimyoList.Contains(tempDaimyoId)){
								dstDaimyoList.Add(tempDaimyoId);
							}
						}
					}


					//Reduce Yukoudo
					MainEventHandler main = new MainEventHandler();
					Daimyo daimyo = new Daimyo();
					string ryugenText = "";
					for(int l=0; l<dstDaimyoList.Count;l++){
						int dstDaimyoId = dstDaimyoList[l];
						string dstDaimyoName = daimyo.getName(dstDaimyoId,langId,senarioId);
						int reduceYukoudo = main.DownYukouValueWithOther(srcDaimyoId,dstDaimyoId);
						reduceYukoudo = reduceYukoudo/2;
						if(reduceYukoudo==0){
							reduceYukoudo=1;
						}

                        if (langId == 2) {
                            ryugenText = ryugenText + dstDaimyoName + " friendship decreased " + reduceYukoudo + " point \n";
                        }else {
                            ryugenText = ryugenText + dstDaimyoName + "との友好度が" + reduceYukoudo + "下がりました。\n";
                        }
                    }

					//Message
					PlayerPrefs.SetBool ("questDailyFlg31",true);

					//Extension Mark Handling
					MainStageController mainStage = new MainStageController();
					mainStage.questExtension();

                    string OKtext = "";
                    if (langId == 2) {
                        OKtext = "My lord, bad rumor was successful. \n" + ryugenText;
                    }else {
                        OKtext = "御屋形様、流言に成功しましたぞ。\n " + ryugenText;
                    }
                    msg.makeMessage (OKtext);

				}else{
					//Failed
					audioSources [4].Play ();

					//Message
					int nowYukoudo = gaikou.getMyGaikou(daimyoId);
					int newYukoudo = gaikou.downMyGaikou(daimyoId,nowYukoudo,maxReduceValue);
					int reduceYukoudo = nowYukoudo - newYukoudo;
					GameObject.Find ("YukouValue").GetComponent<Text> ().text = newYukoudo.ToString ();


                    //Message
                    string NGtext = "";
                    if (langId == 2) {
                        NGtext = "My lord, I'm sorry. failed misreport. \n Friendship decreased " + reduceYukoudo + " point";
                    }else {
                        NGtext = "申し訳御座りませぬ。流言に失敗しましたぞ。\n友好度が" + reduceYukoudo + "下がりますぞ。";
                    }
                       
					yukouChange.downYukouOnIcon (daimyoId,newYukoudo);

					msg.makeMessage (NGtext);

				}
				PlayerPrefs.Flush();

				//Back
				GameObject.Find ("return").GetComponent<MenuReturn> ().OnClick ();


			}else if(name == "DoGoudatsuBtn"){
				reduceHyourou ();
				
				//Ratio
				//Ge 10-20%, Cyu 20-40%, Jyo 40-70%
				float randomPercent = 0;
				int newQty = itemQty - 1;
				
				if(itemRank == "Ge"){
					float tempRandomPercent = (200 - daimyoBusyoDfc)/4;
					float tempValue = UnityEngine.Random.Range(0.5f,1.5f);
					randomPercent = tempRandomPercent * tempValue;
					PlayerPrefs.SetInt ("shinobiGe",newQty);
					
				}else if(itemRank == "Cyu"){
					float tempRandomPercent = (200 - daimyoBusyoDfc)/2;
					float tempValue = UnityEngine.Random.Range(0.8f,1.2f);
					randomPercent = tempRandomPercent * tempValue;
					PlayerPrefs.SetInt ("shinobiCyu",newQty);
					
					
				}else if(itemRank == "Jyo"){
					float tempRandomPercent = (200 - daimyoBusyoDfc);
					float tempValue = UnityEngine.Random.Range(0.9f,1.1f);
					randomPercent = tempRandomPercent * tempValue;
					PlayerPrefs.SetInt ("shinobiJyo",newQty);
					
				}
				
				float percent = Random.value;
				percent = percent * 100;

				if(percent <= randomPercent){
					audioSources [3].Play ();

					//Track
					int TrackBouryakuSuccessNo = PlayerPrefs.GetInt("TrackBouryakuSuccessNo",0);
					TrackBouryakuSuccessNo = TrackBouryakuSuccessNo + 1;
					PlayerPrefs.SetInt ("TrackBouryakuSuccessNo", TrackBouryakuSuccessNo);

					//Success
					int kuniQty = GameObject.Find("close").GetComponent<CloseBoard>().kuniQty; 
					int getMoney =0;
					//Money or Item 0:money, 1:item
					int moneyOrItem = UnityEngine.Random.Range(0,2);
					//Kahou or Shizai 0:kahou, 1:shizai 
					int kahouOrShizai = UnityEngine.Random.Range(0,2);
					string kahouName = "";
					string shigenName = "";
					int addQty =0;
					int kahouRank = 0; //kahouRank S,A,B,C=1,2,3,4
					//shigen Type
					int shigenType = 0; //KB,YR,TP,YM=1,2,3,4

					if(moneyOrItem==0){
						//money
						int temGetMoney = UnityEngine.Random.Range(1000,1501);
						getMoney = temGetMoney * kuniQty;
						int nowMoney = PlayerPrefs.GetInt("money");
						nowMoney = nowMoney + getMoney;
                        if (nowMoney < 0) {
                            nowMoney = int.MaxValue;
                        }
                        PlayerPrefs.SetInt("money",nowMoney);
						PlayerPrefs.Flush();

					}else{
						//item
						//Kahou or Shizai 0:kahou, 1:shizai 
						kahouOrShizai = UnityEngine.Random.Range(0,2);
						if(kahouOrShizai==0){
							//kahou
							Kahou kahou = new Kahou();
							////Bugu, Gusoku, Kabuto, Meiba, Heihousyo, Cyadougu, Chishikisyo(1,2,3,4,5,6)
							int kahouType = UnityEngine.Random.Range(1,7);

							float khPercent = Random.value;
							khPercent = khPercent * 100;
							if(5<=kuniQty){
								//(S,A,B  5,35,60%)
								if(khPercent<=5){
									//S
									kahouRank = 1;
									kahouName = kahou.getRamdomKahou(kahouType, kahouRank);
								}else if(5<khPercent && khPercent <=41){
									//A
									kahouRank = 2;
									kahouName = kahou.getRamdomKahou(kahouType, kahouRank);
								}else if(41<khPercent){
									//B
									kahouRank = 3;
									kahouName = kahou.getRamdomKahou(kahouType, kahouRank);
								}

							}else if(3<=kuniQty && kuniQty<5){
								//(S,A,B,C : 1,15,25,59%)
								if(khPercent<=1){
									//S
									kahouRank = 1;
									kahouName = kahou.getRamdomKahou(kahouType, kahouRank);
								}else if(1<khPercent && khPercent <=16){
									//A
									kahouRank = 2;
									kahouName = kahou.getRamdomKahou(kahouType, kahouRank);
								}else if(16<khPercent && khPercent <= 41){
									//B
									kahouRank = 3;
									kahouName = kahou.getRamdomKahou(kahouType, kahouRank);
								}else if(41<khPercent){
									//C
									kahouRank = 4;
									kahouName = kahou.getRamdomKahou(kahouType, kahouRank);
								}

							}else if(kuniQty<3){
								//(A,B,C : 5, 35, 60%)
								if(khPercent<=5){
									//A
									kahouRank = 2;
									kahouName = kahou.getRamdomKahou(kahouType, kahouRank);
								}else if(5<khPercent && khPercent <=41){
									//B
									kahouRank = 3;
									kahouName = kahou.getRamdomKahou(kahouType, kahouRank);
								}else if(41<khPercent){
									//C
									kahouRank = 3;
									kahouName = kahou.getRamdomKahou(kahouType, kahouRank);
								}

							}


						}else{

							//shizai
							shigenType = UnityEngine.Random.Range(1,5);
							float sgPercent = Random.value;
							sgPercent = sgPercent * 100;
							addQty = UnityEngine.Random.Range(1,6);
							Item item = new Item();
							int shigenRank = 0;//下、中、上=1,2,3

							if(5<=kuniQty){
								//(上,中,下  40,40, 20%)
								if(sgPercent<=40){
									shigenRank = 3;
								}else if(40<sgPercent && sgPercent <=81){
									shigenRank = 2;
								}else if(81<sgPercent){
									shigenRank = 1;
								}
								shigenName = item.getRandomShigen(shigenType,shigenRank,addQty);

							}else if(3<=kuniQty && kuniQty<5){
								//(上,中,下  20,50,30%)
								if(sgPercent<=20){
									shigenRank = 3;
								}else if(20<sgPercent && sgPercent <=51){
									shigenRank = 2;
								}else if(51<sgPercent){
									shigenRank = 1;
								}
								shigenName = item.getRandomShigen(shigenType,shigenRank,addQty);

							}else if(kuniQty<3){
								//(上,中,下  5,25,70%)
								if(sgPercent<=5){
									shigenRank = 3;
								}else if(5<sgPercent && sgPercent <=26){
									shigenRank = 2;
								}else if(26<sgPercent){
									shigenRank = 1;
								}
								shigenName = item.getRandomShigen(shigenType,shigenRank,addQty);
							}
						}
					}


					//Message
					PlayerPrefs.SetBool ("questDailyFlg32",true);

					MainStageController mainStage = new MainStageController();
					mainStage.questExtension();

                    string OKtext = "";
                    if (langId == 2) {
                        OKtext = "My lord, successed to rob.\n";
                    }
                    else {
                        OKtext = "御屋形様、強奪に成功しましたぞ。\n";
                    }


                    string addText = "";
					if(moneyOrItem==0){

                        if (langId == 2) {
                            addText = "got " + getMoney + " money.";
                        }
                        else {
                            addText = "金を" + getMoney + "奪って参りました。";
                        }
                    }
                    else{
						if(kahouOrShizai==0){
                            //kahou

                            if (langId == 2) {
                                addText = " got treasure " + kahouName + ".";
                            }
                            else {
                                addText = "家宝、" + kahouName + "を奪って参りました。";
                            }
                        }
                        else{
                            //shizai＋

                            if (langId == 2) {
                                addText = " got " + addQty + " " + shigenName + ".";
                            }
                            else {
                                addText = shigenName + "を" + addQty + "個奪って参りました。";
                            }
                        }
					}

					OKtext = OKtext + addText;
					msg.makeMessage (OKtext);

				}else{
					//Failed
					audioSources [4].Play ();

					int nowYukoudo = gaikou.getMyGaikou(daimyoId);
					int newYukoudo = gaikou.downMyGaikou(daimyoId,nowYukoudo,maxReduceValue);
					int reduceYukoudo = nowYukoudo - newYukoudo;
					GameObject.Find ("YukouValue").GetComponent<Text> ().text = newYukoudo.ToString ();

                    //Message
                    string NGtext = "";
                    if (langId == 2) {
                        NGtext = "My lord, I'm sorry. failed to rob. \n Friendship decreased " + reduceYukoudo + " point";
                    }
                    else {
                        NGtext = "申し訳御座りませぬ。強奪に失敗しましたぞ。\n友好度が" + reduceYukoudo + "下がりますぞ。";
                    }
                    msg.makeMessage (NGtext);

					yukouChange.downYukouOnIcon (daimyoId,newYukoudo);

				}

				PlayerPrefs.Flush();
				
				//Back
				GameObject.Find ("return").GetComponent<MenuReturn> ().OnClick ();
			
			}else if(name == "DoCyouhouBtn"){

				reduceHyourou ();

				//Set Value & REduce Qty
				float missPercent = 0;
				int newQty = itemQty - 1;
				int snbValue = 0;

				if(itemRank == "Ge"){
					missPercent = 30;
					PlayerPrefs.SetInt ("shinobiGe",newQty);
					snbValue = 1;

				}else if(itemRank == "Cyu"){
					missPercent = 15;
					PlayerPrefs.SetInt ("shinobiCyu",newQty);
					snbValue = 2;

				}else if(itemRank == "Jyo"){
					missPercent = 5;
					PlayerPrefs.SetInt ("shinobiJyo",newQty);
					snbValue = 3;

				}


				//Random Check
				float percent = Random.value;
				percent = percent * 100;
					
				if (percent >= missPercent) {
					//Success
					audioSources [3].Play ();

					//Track
					int TrackBouryakuSuccessNo = PlayerPrefs.GetInt("TrackBouryakuSuccessNo",0);
					TrackBouryakuSuccessNo = TrackBouryakuSuccessNo + 1;
					PlayerPrefs.SetInt ("TrackBouryakuSuccessNo", TrackBouryakuSuccessNo);

					int TrackCyouhouNo = PlayerPrefs.GetInt("TrackCyouhouNo",0);
					TrackCyouhouNo = TrackCyouhouNo + 1;
					PlayerPrefs.SetInt ("TrackCyouhouNo", TrackCyouhouNo);

					string cyouhouHist = PlayerPrefs.GetString("cyouhou");
					char[] delimiterChars = {','};
					List<string> cyouhouHistList = new List<string> ();
					if (cyouhouHist != null && cyouhouHist != "") {
						if (cyouhouHist.Contains (",")) {
							cyouhouHistList = new List<string> (cyouhouHist.Split (delimiterChars));
						} else {
							cyouhouHistList.Add (cyouhouHist);			
						}
					} 

					//Add new kuni
					if (!cyouhouHistList.Contains (kuniId.ToString())) {
						cyouhouHistList.Add (kuniId.ToString());
					} 

					string newCyouhouHist = "";
					for(int i=0;i<cyouhouHistList.Count;i++){
						string tmpCyouhouKuniId = cyouhouHistList [i];

						if (i == 0) {
							newCyouhouHist = tmpCyouhouKuniId;
						} else {
							newCyouhouHist = newCyouhouHist + "," + tmpCyouhouKuniId;
						}
					}

					PlayerPrefs.SetString("cyouhou",newCyouhouHist);

					string cyouhouKuni = "cyouhou" + kuniId.ToString ();
					PlayerPrefs.SetInt(cyouhouKuni,snbValue);

					bool cyouhouFlg = closeScript.cyouhouFlg;

					if (cyouhouFlg) {
						//Change Icon
						GameObject shinobi = GameObject.Find ("shinobi").gameObject;

						if (snbValue == 1) {
							Color lowColor = new Color (0f / 255f, 0f / 255f, 219f / 255f, 255f / 255f);
							shinobi.GetComponent<Image> ().color = lowColor;
							shinobi.transform.FindChild ("ShinobiRank").GetComponent<Text> ().text = "下";
						} else if (snbValue == 2) {
							Color midColor = new Color (94f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
							shinobi.GetComponent<Image> ().color = midColor;
							shinobi.transform.FindChild ("ShinobiRank").GetComponent<Text> ().text = "中";
						} else if (snbValue == 3) {
							Color highColor = new Color (84f / 255f, 103f / 255f, 0f / 255f, 255f / 255f);
							shinobi.GetComponent<Image> ().color = highColor;
							shinobi.transform.FindChild ("ShinobiRank").GetComponent<Text> ().text = "上";
						}



					} else {

                        GameObject smallBoardObj = GameObject.Find("smallBoard(Clone)").gameObject;

                        //new Icon
                        string shinobiItemPath = "Prefabs/Item/Shinobi/Shinobi";
						GameObject shinobi = Instantiate (Resources.Load (shinobiItemPath)) as GameObject;	
						shinobi.transform.SetParent (smallBoardObj.transform);
						shinobi.transform.localScale = new Vector2 (0.25f, 0.31f);
						shinobi.name = "shinobi";
						RectTransform snbTransform = shinobi.GetComponent<RectTransform> ();
						snbTransform.anchoredPosition = new Vector3 (-251, 250, 0);
						shinobi.GetComponent<Button> ().enabled = false;

						if (snbValue == 1) {
							Color lowColor = new Color (0f / 255f, 0f / 255f, 219f / 255f, 255f / 255f);
							shinobi.GetComponent<Image>().color = lowColor;
							shinobi.transform.FindChild("ShinobiRank").GetComponent<Text>().text = "下";
						} else if (snbValue == 2) {
							Color midColor = new Color (94f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
							shinobi.GetComponent<Image>().color = midColor;
							shinobi.transform.FindChild("ShinobiRank").GetComponent<Text>().text = "中";
						} else if (snbValue == 3) {
							Color highColor = new Color (84f / 255f, 103f / 255f, 0f / 255f, 255f / 255f);
							shinobi.GetComponent<Image>().color = highColor;
							shinobi.transform.FindChild("ShinobiRank").GetComponent<Text>().text = "上";
						}

                        //Set Flg
                        closeScript.cyouhouFlg = true;
                        closeScript.cyouhouSnbRankId = snbValue;
                    }

					GameObject kuniIconView = GameObject.Find ("KuniIconView").gameObject;
					kuniIconView.transform.FindChild (kuniId.ToString ()).GetComponent<SendParam> ().cyouhouSnbRankId = snbValue;


					KuniInfo kuni = new KuniInfo();
					string kuniName = kuni.getKuniName (kuniId,langId);
					PlayerPrefs.SetBool ("questDailyFlg30",true);

					MainStageController mainStage = new MainStageController();
					mainStage.questExtension();


                    string OKtext = "";
                    if (langId == 2) {
                        OKtext = "Ninja hided in " + kuniName + " well. \n Please check spy report.";
                    }else {
                        OKtext = "忍が上手く" + kuniName + "に潜伏しましたぞ。\n諜報内容をご確認下され。";
                    }
                    msg.makeMessage (OKtext);

				} else {
					//Fail
					audioSources [4].Play ();

					int nowYukoudo = gaikou.getMyGaikou(daimyoId);
					int newYukoudo = gaikou.downMyGaikou(daimyoId,nowYukoudo,maxReduceValue);
					int reduceYukoudo = nowYukoudo - newYukoudo;
					GameObject.Find ("YukouValue").GetComponent<Text> ().text = newYukoudo.ToString ();
                    string NGtext = "";
                    if (langId == 2) {
                        NGtext = "Ninja was caught. \n Friendship decreased " + reduceYukoudo + " point";
                    } else {
                        NGtext = "忍が捕まってしまったようです。\n友好度が" + reduceYukoudo + "下がりますぞ。";
                    }
                    
					msg.makeMessage (NGtext);

					yukouChange.downYukouOnIcon (daimyoId,newYukoudo);

				}


				PlayerPrefs.Flush();

				//Back
				GameObject.Find ("return").GetComponent<MenuReturn> ().OnClick ();
			}





		
		} else {
			audioSources [4].Play ();
            msg.hyourouMovieMessage();
            //msg.makeMessage (msg.getMessage(7));

		}

	}

	public void reduceHyourou (){
		
		//Reduce Hyourou
		int nowHyourou = PlayerPrefs.GetInt ("hyourou");
		int newHyourou = nowHyourou - 5;
		PlayerPrefs.SetInt ("hyourou", newHyourou);
		GameObject.Find ("HyourouCurrentValue").GetComponent<Text> ().text = newHyourou.ToString ();
		PlayerPrefs.Flush();
	}
	
}
