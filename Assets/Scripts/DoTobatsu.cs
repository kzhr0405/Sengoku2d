using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DoTobatsu : MonoBehaviour {

	public bool myKuniQtyIsBiggestFlg = false;
	public int targetDaimyoId = 0;
	public string targetDaimyoName = "";
	public int kuniQty = 0;
	public int myKuniQty = 0;
	public int myDaimyo = 0;
	public float baseRatio = 0;
	public bool toubatsuFlg = false;
	public AudioSource[] audioSources;

	public void OnClick(){
		audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [5].Play ();

		//Hyourou
		int hyourou = PlayerPrefs.GetInt ("hyourou");
		int newHyourou = hyourou - 10;
		PlayerPrefs.SetInt ("hyourou", newHyourou);
		GameObject.Find ("HyourouCurrentValue").GetComponent<Text> ().text = newHyourou.ToString ();

		//Track
		int TrackToubatsuNo = PlayerPrefs.GetInt("TrackToubatsuNo",0);
		TrackToubatsuNo = TrackToubatsuNo + 1;
		PlayerPrefs.SetInt ("TrackToubatsuNo", TrackToubatsuNo);

		MainStageController script = GameObject.Find ("GameController").GetComponent<MainStageController> ();
		myKuniQty = script.myKuniQty;
		myDaimyo = script.myDaimyo;
		myKuniQtyIsBiggestFlg = GameObject.Find ("bakuhuReturn").GetComponent<BakuhuMenuReturn> ().myKuniQtyIsBiggestFlg;
		List<string> messageList = new List<string> ();

		//reduce yukoudo
		Gaikou gaikou = new Gaikou();
		DoGaikou doGaikou = new DoGaikou ();
		int myGaikouValueWithTarget = gaikou.getMyGaikou (targetDaimyoId);
		int newYukoudoWithTarget = gaikou.downMyGaikou(targetDaimyoId, myGaikouValueWithTarget, 50);
		int reducedValueWithTarget = myGaikouValueWithTarget - newYukoudoWithTarget;
        string firstKassenText = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            firstKassenText = "Declared " + targetDaimyoName + " attack order. \n Friendship reduced " +reducedValueWithTarget+ " point.";
        }else {
            firstKassenText = targetDaimyoName + "討伐を宣言しました。\n当家との友好度が" + reducedValueWithTarget + "下がります。";
        }
		doGaikou.downYukouOnIcon(targetDaimyoId, newYukoudoWithTarget);
		messageList.Add (firstKassenText);

		//doumei check
		Doumei doumei = new Doumei();
		KuniInfo kuni = new KuniInfo ();
		bool doumeiExistFlg = doumei.myDoumeiExistCheck(targetDaimyoId);
		if (doumeiExistFlg) {
			doumei.deleteDoumei (myDaimyo.ToString(),targetDaimyoId.ToString());

			//Change Map & Yukoudo
			kuni.deleteDoumeiKuniIcon(targetDaimyoId);
		}
		PlayerPrefs.Flush ();


		//Listup Target kuni Id
		string seiryoku = PlayerPrefs.GetString ("seiryoku");
		List<string> seiryokuList = new List<string> ();
		char[] delimiterChars = {','};
		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
		List<int> targetKuniList = new List<int> ();
		for (int i = 0; i < seiryokuList.Count; i++) {
			if (int.Parse (seiryokuList [i]) == targetDaimyoId) {
				int kuniId = i + 1;
				targetKuniList.Add (kuniId);
			}
		}

		//Listup Available Open kuni
		List<string> srcDstKuniList = new List<string> ();
		Entity_kuni_mapping_mst kuniMapMst  = Resources.Load ("Data/kuni_mapping_mst") as Entity_kuni_mapping_mst;
		for (int j = 0; j < targetKuniList.Count; j++) {
			int tmpTargetKuniId = targetKuniList [j];

			for(int i=0; i<kuniMapMst.param.Count; i++){
				int temClearedKuniId = kuniMapMst.param[i].Souce;
				if(temClearedKuniId == tmpTargetKuniId){
					int openKuniId = kuniMapMst.param [i].Open;

					if (int.Parse (seiryokuList [openKuniId - 1]) != targetDaimyoId && 
						int.Parse (seiryokuList [openKuniId - 1]) != myDaimyo){
						srcDstKuniList.Add (openKuniId.ToString() + "-" + tmpTargetKuniId.ToString());
					}
				}
			}
		}

		//Shuffule
		for (int i = 0; i < srcDstKuniList.Count; i++) {
			string temp = srcDstKuniList[i];
			int randomIndex = Random.Range(i, srcDstKuniList.Count);
			srcDstKuniList[i] = srcDstKuniList[randomIndex];
			srcDstKuniList[randomIndex] = temp;
		}

		/*Condition of succesful*/
		//1. number of own kuni is more thab twice as number as target ... 20%
		//2. number of own kuni is the largest ... 30%
		//3. relathionship btwn my party and src kuni is 100% ... 20%
		//4. relathionship btwn the other countiries each other is 0% ... 20%
		//5. attacker keep kuni number more than country receiving attack ... 10%

		//1.
		if (myKuniQty > 2 * kuniQty) {
			baseRatio = 20;
		}

		//2.
		if (myKuniQtyIsBiggestFlg) {
			baseRatio = baseRatio + 30;
		}

		//Make Gunzei
		char[] delimiterChars2 = {'-'};
		GameObject BakuhuKuniIconView = GameObject.Find ("BakuhuKuniIconView").gameObject;
		string path = "Prefabs/Map/Gunzei";
		string visualizePath = "Prefabs/Bakuhu/ToubatsuSrcBusyo";
		GameObject panel = GameObject.Find ("Panel").gameObject;
		Gunzei gunzei = new Gunzei ();
		Daimyo daimyo = new Daimyo ();
		MainEventHandler mEvent = new MainEventHandler ();
		List<int> doneSrcDaimyoList = new List<int> ();

		for (int k = 0; k < srcDstKuniList.Count; k++) {
			float indvRatio = 0;

			string key = srcDstKuniList[k];
			List<string> srcDstList = new List<string> ();
			srcDstList = new List<string> (key.Split (delimiterChars2));

			//3
			int srcKuniId = int.Parse(srcDstList[0]);
			int srcDaimyo = int.Parse(seiryokuList[srcKuniId-1]);

			if(!doneSrcDaimyoList.Contains(srcDaimyo)){
				doneSrcDaimyoList.Add (srcDaimyo);
				bool ExistFlg = false;
				foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Gunzei")){
					int gunzeiSrcDaimyoId = obs.GetComponent<Gunzei>().srcDaimyoId;

					if(srcDaimyo == gunzeiSrcDaimyoId){
						ExistFlg = true;
					}
				}

				if (!ExistFlg) {
					int myGaikouValue = gaikou.getMyGaikou (srcDaimyo);
					indvRatio = baseRatio + (float)myGaikouValue / 5;

					//4.
					int dstKuniId = int.Parse (srcDstList [1]);
					int dstDaimyo = int.Parse (seiryokuList [dstKuniId - 1]);
					int otherGaikouValue = gaikou.getOtherGaikouValue (srcDaimyo, dstDaimyo);
					indvRatio = indvRatio + (float)((100 - otherGaikouValue) / 5);

					//5.
					int srcKuniQty = BakuhuKuniIconView.transform.FindChild (srcDstList [0]).GetComponent<SendParam> ().kuniQty;
					float addRatio = 0;
					if (srcKuniQty >= kuniQty) {
						addRatio = 10;
						indvRatio = indvRatio + addRatio;
					}

					//Success Check
					float percent = UnityEngine.Random.value;
					percent = percent * 100;
					if (percent <= indvRatio) {
						//OK
						toubatsuFlg = true;
						GameObject Gunzei = Instantiate (Resources.Load (path)) as GameObject;			
						Gunzei.transform.SetParent (panel.transform);
						Gunzei.transform.localScale = new Vector2 (1, 1);

						GameObject minGunzei = Instantiate (Resources.Load (visualizePath)) as GameObject;		
						minGunzei.transform.SetParent (BakuhuKuniIconView.transform);
						int daimyoBusyoId = daimyo.getDaimyoBusyoId (srcDaimyo);
						string daimyoPath = "Prefabs/Player/Sprite/unit" + daimyoBusyoId.ToString ();
						minGunzei.GetComponent<SpriteRenderer> ().sprite = 
							Resources.Load (daimyoPath, typeof(Sprite)) as Sprite;

						//Location
						int srcX = kuni.getKuniLocationX(srcKuniId);
						int srcY = kuni.getKuniLocationY(srcKuniId);
						int dstX = kuni.getKuniLocationX(dstKuniId);
						int dstY = kuni.getKuniLocationY(dstKuniId);
						string direction = "";

						if(srcX < dstX){
							Gunzei.transform.localScale = new Vector2 (1, 1);
							minGunzei.transform.localScale = new Vector2 (60, 72);
							direction = "right";
						}else{
							Gunzei.transform.localScale = new Vector2 (-1, 1);
							minGunzei.transform.localScale = new Vector2 (-60, 72);
							direction = "left";
							Gunzei.GetComponent<Gunzei> ().leftFlg = true;
						}

						int aveX = (srcX + dstX)/2;
						int aveY = (srcY + dstY)/2;
						RectTransform GunzeiTransform = Gunzei.GetComponent<RectTransform> ();
						GunzeiTransform.anchoredPosition = new Vector3 (aveX, aveY, 0);

						RectTransform minGunzeiTransform = minGunzei.GetComponent<RectTransform> ();
						float minAveX = (float)aveX * 0.8f;
						float minAveY = (float)aveY * 0.65f;
						minGunzeiTransform.anchoredPosition = new Vector3 ((int)minAveX, (int)minAveY, 0);


						Gunzei.GetComponent<Gunzei> ().key = key;
						Gunzei.GetComponent<Gunzei> ().srcKuni = srcKuniId;
						Gunzei.GetComponent<Gunzei> ().srcDaimyoId = srcDaimyo;
						string srcDaimyoName = daimyo.getName (srcDaimyo,langId);
						Gunzei.GetComponent<Gunzei> ().srcDaimyoName = srcDaimyoName;
						Gunzei.GetComponent<Gunzei> ().dstKuni = dstKuniId;
						Gunzei.GetComponent<Gunzei> ().dstDaimyoId = dstDaimyo;
						string dstDaimyoName = daimyo.getName (dstDaimyo,langId);
						Gunzei.GetComponent<Gunzei> ().dstDaimyoName = dstDaimyoName;
						int myHei = gunzei.heiryokuCalc (srcKuniId);

						//random myHei from -50%-myHei
						List<float> randomPercent = new List<float>{0.8f,0.9f,1.0f};
						int rmd = UnityEngine.Random.Range(0,randomPercent.Count);
						float per = randomPercent [rmd];
						myHei = Mathf.CeilToInt (myHei * per);

						Gunzei.GetComponent<Gunzei> ().myHei = myHei;
						Gunzei.name = key;

						//Engun from Doumei
						List<string> doumeiDaimyoList = new List<string> ();
						bool dstEngunFlg = false;
						string dstEngunDaimyoId = ""; //2:3:5 
						string dstEngunHei = "";
						string dstEngunSts = ""; //BusyoId-BusyoLv-ButaiQty-ButaiLv:
						int totalEngunHei = 0;

						//Doumei Check
						doumeiDaimyoList = doumei.doumeiExistCheck(dstDaimyo,srcDaimyo.ToString());
						string doumeiCheck = "doumei" + srcDaimyo;
						if(PlayerPrefs.HasKey(doumeiCheck)){
							string cDoumei = PlayerPrefs.GetString(doumeiCheck);
							List<string> cDoumeiList = new List<string>();
							if(cDoumei.Contains(",")){
								cDoumeiList = new List<string> (cDoumei.Split (delimiterChars));

							}else{
								cDoumeiList.Add(cDoumei);
							}

							//If Doumei Daimyo -> Delete
							if(cDoumeiList.Contains(dstDaimyo.ToString())){
								doumei.deleteDoumei (srcDaimyo.ToString(),dstDaimyo.ToString());	
							}
						}

						if(doumeiDaimyoList.Count != 0){
							//Doumei Exist

							//Trace Check
							List<string> okDaimyoList = new List<string> ();
							List<string> checkList = new List<string> ();
							okDaimyoList = doumei.traceNeighborDaimyo(dstKuniId, dstDaimyo, doumeiDaimyoList, seiryokuList, checkList,okDaimyoList);

							if(okDaimyoList.Count !=0){
								//Doumei & Neghbor Daimyo Exist

								for(int h=0; h<okDaimyoList.Count; h++){
									string engunDaimyo = okDaimyoList[h];
									int yukoudo = gaikou.getExistGaikouValue(int.Parse(engunDaimyo), dstDaimyo);

									//engun check

									dstEngunFlg = mEvent.CheckByProbability (yukoudo);
									if(dstEngunFlg){
										//Engun OK
										dstEngunFlg = true;
										if(dstEngunDaimyoId !=null && dstEngunDaimyoId !=""){
											dstEngunDaimyoId = dstEngunDaimyoId + ":" + engunDaimyo;
											string tempEngunSts = engunDaimyo + "-" + mEvent.getEngunSts(engunDaimyo);
											int tempEngunHei = mEvent.getEngunHei(tempEngunSts);
											dstEngunHei = dstEngunHei + ":" + tempEngunHei.ToString();
											totalEngunHei = totalEngunHei + tempEngunHei;
											dstEngunSts = dstEngunSts + ":" + tempEngunSts;

										}else{
											dstEngunDaimyoId = engunDaimyo;
											string tempEngunSts = engunDaimyo + "-" + mEvent.getEngunSts(engunDaimyo);
											int tempEngunHei = mEvent.getEngunHei(tempEngunSts);
											dstEngunHei = tempEngunHei.ToString();
											totalEngunHei = tempEngunHei;
											dstEngunSts = tempEngunSts;

										}
									}
								}
								Gunzei.GetComponent<Gunzei> ().dstEngunFlg = dstEngunFlg;
								Gunzei.GetComponent<Gunzei> ().dstEngunDaimyoId = dstEngunDaimyoId;
								Gunzei.GetComponent<Gunzei> ().dstEngunHei = dstEngunHei;
								Gunzei.GetComponent<Gunzei> ().dstEngunSts = dstEngunSts;
							}
						}

						//Set Value
						//CreateTime,srcDaimyoId,dstDaimyoId,srcDaimyoName,dstDaimyoName, srcHei,locationX,locationY,left or right, engunFlg, engunDaimyoId(A:B:C), dstEngunHei(1000:2000:3000), dstEngunSts
						string keyValue = "";
						string createTime = System.DateTime.Now.ToString ();
						keyValue = createTime + "," + srcDaimyo + "," + dstDaimyo + "," + srcDaimyoName + "," + dstDaimyoName + "," + myHei + "," + aveX + "," + aveY + "," + direction + "," + dstEngunFlg + "," + dstEngunDaimyoId + "," + dstEngunHei + ","+ dstEngunSts;
						PlayerPrefs.SetString (key, keyValue);
						string keyHistory = PlayerPrefs.GetString ("keyHistory");
						if(keyHistory == null || keyHistory == ""){
							keyHistory = key;
						}else{
							keyHistory = keyHistory + "," + key;
						}
						PlayerPrefs.SetString ("keyHistory", keyHistory);
						PlayerPrefs.SetInt ("bakuhuTobatsuDaimyoId",dstDaimyo);
						PlayerPrefs.Flush ();

						string kassenText = "";                       
                        if (langId == 2) {
                            if (!dstEngunFlg){
							    kassenText = srcDaimyoName + " is attacking " + dstDaimyoName + " with " + myHei + " soldiers.";
						    }else{
							    kassenText = srcDaimyoName + " is attacking " + dstDaimyoName + " with " + myHei + " soldiers.\n Defender's allianced country sent " + totalEngunHei + " soldiers.";
						    }
                        }else {
                            if (!dstEngunFlg) {
                                kassenText = srcDaimyoName + "が" + dstDaimyoName + "討伐の兵" + myHei + "人を起こしました。";
                            }
                            else {
                                kassenText = srcDaimyoName + "が" + dstDaimyoName + "討伐の兵" + myHei + "人を起こしました。\n防衛側の同盟国が援軍" + totalEngunHei + "人を派兵しました。";
                            }
                        }
						messageList.Add (kassenText);

					} else {
						//NG
						string kassenText = "";
						string srcDaimyoName = daimyo.getName (srcDaimyo,langId);
						int newYukoudo = gaikou.downMyGaikou(srcDaimyo, myGaikouValue, 15);
						int reducedValue = myGaikouValue - newYukoudo;
                        if (langId == 2) {
                            kassenText = srcDaimyoName + " rejected our attack order. Friendship reduced " +reducedValue+ " point.";
                        }else {
                            kassenText = srcDaimyoName + "が討伐令を黙殺しました。当家との友好度が" + reducedValue + "下がります。";
                        }
						doGaikou.downYukouOnIcon(srcDaimyo, newYukoudo);
						messageList.Add (kassenText);
					}
				}
			}
		}


		//Disabled
		foreach (Transform obj in BakuhuKuniIconView.transform) {
			if (obj.GetComponent<Button> ()) {
				obj.GetComponent<Button> ().enabled = false;
			}
		}

		GameObject toubatsuSelect = GameObject.Find ("ToubatsuSelect").gameObject;
		toubatsuSelect.transform.FindChild ("ToubatsuBtn").gameObject.SetActive (false);
		if (toubatsuFlg) {
			audioSources [3].Play ();
            if (langId == 2) {
                toubatsuSelect.transform.FindChild ("Exp").GetComponent<Text> ().text = "Declared " + targetDaimyoName + " attack order. Other clan responded to it.";
            }else {
                toubatsuSelect.transform.FindChild("Exp").GetComponent<Text>().text = targetDaimyoName + "の討伐令を出しました。諸大名が呼応したようです。";
            }
		} else {
			audioSources [4].Play ();
			if (srcDstKuniList.Count == 0) {
                if (langId == 2) {
                    toubatsuSelect.transform.FindChild ("Exp").GetComponent<Text> ().text = "There is no clan who can respond to " + targetDaimyoName + " attack order.";
                }else {
                    toubatsuSelect.transform.FindChild("Exp").GetComponent<Text>().text = "現在" + targetDaimyoName + "の討伐に呼応可能な大名はおりません。";
                }
            } else{
                if (langId == 2) {
                    toubatsuSelect.transform.FindChild ("Exp").GetComponent<Text> ().text = "No clan responded to our attack order because of fear for " + targetDaimyoName + ".";
			    }else {
                    toubatsuSelect.transform.FindChild("Exp").GetComponent<Text>().text = targetDaimyoName + "を恐れてか、討伐令にどの大名も呼応しませんでした。";
                }
            }
		}

		Message msg = new Message ();
		msg.makeSlotMessage (messageList);








	}



}
