using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class CyouhouSelect : MonoBehaviour {

	public int kuniId = 0;
	public string kuniName = "";
	public int daimyoId = 0;
	public string daimyoName = "";
	public int snbRank = 0;
	public GameObject board;
	public GameObject status;
	public GameObject close;
	public List<string> seiryokuList;
	public bool sameDaimyoFlg = false;

	public void OnClick(){

		if (close.GetComponent<CloseBoard> ().kuniId != kuniId) {
			close.GetComponent<CloseBoard> ().kuniId = kuniId;
            Message Message = new Message();
            AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
			audioSources [2].Play ();

			/*Status*/
			//Common
			GameObject kuniIconView = GameObject.Find ("KuniIconView").gameObject;
			SendParam script = kuniIconView.transform.Find (kuniId.ToString ()).GetComponent<SendParam> ();
			KuniInfo kuni = new KuniInfo ();
			Daimyo daimyo = new Daimyo ();
			Gaikou gaikou = new Gaikou ();
			List<int> targetKuniList = new List<int> ();
			targetKuniList = kuni.getMappingKuni (kuniId);
			char[] delimiterChars = { ',' };
            int langId = PlayerPrefs.GetInt("langId");

            //Kamon
            GameObject daimyoNameObj = status.transform.Find ("DaimyoName").gameObject;
			string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString ();
			daimyoNameObj.transform.Find ("Kamon").GetComponent<Image> ().sprite = 
			Resources.Load (imagePath, typeof(Sprite)) as Sprite;

			//Daimyo Name
			daimyoNameObj.transform.Find ("Value").GetComponent<Text> ().text = daimyoName;

			//Kuni Name
			GameObject kuniNameObj = status.transform.Find ("KuniName").gameObject;
			kuniNameObj.transform.Find ("Value").GetComponent<Text> ().text = kuniName;

			//Heiryoku
			status.transform.Find ("Heiryoku").transform.Find ("Value").GetComponent<Text> ().text = script.heiryoku.ToString ();

			//Yukou
			status.transform.Find ("Yukoudo").transform.Find ("Value").GetComponent<Text> ().text = script.myYukouValue.ToString ();

			//Attack Target
			bool aggressiveFlg = script.aggressiveFlg;
			int myDaimyoId = PlayerPrefs.GetInt ("myDaimyo");

			int targetKuniId = getKassenTargetKuni (kuniId, daimyoId, targetKuniList, kuniIconView, aggressiveFlg, seiryokuList, myDaimyoId);
			int targetDaimyoId =0;
			if (targetKuniId != 0) {
				string targetKuniName = kuni.getKuniName (targetKuniId,langId);
				targetDaimyoId = int.Parse (seiryokuList [targetKuniId - 1]);
				string targetDaimyoName = daimyo.getClanName (targetDaimyoId, langId);
				status.transform.Find ("Atk").transform.Find ("Value").GetComponent<Text> ().text = targetKuniName + "(" + targetDaimyoName + ")";
			} else {               
                status.transform.Find("Atk").transform.Find("Value").GetComponent<Text>().text = Message.getMessage(219,langId);
            }


            int targetGaikouKuniId = 0;
			if (snbRank > 1) { //Jyo or Cyu

				//Gaikou
				targetGaikouKuniId = getGaikouTargetKuni (kuniId, daimyoId, targetKuniList, kuniIconView, aggressiveFlg, seiryokuList, myDaimyoId);
				if (targetGaikouKuniId != 0) {
					string targetGaikouKuniName = kuni.getKuniName (targetGaikouKuniId,langId);
					int targetGaikouDaimyoId = int.Parse (seiryokuList [targetGaikouKuniId - 1]);
					string targetGaikouDaimyoName = daimyo.getClanName (targetGaikouDaimyoId,langId);

					if (targetDaimyoId != targetGaikouDaimyoId) {
						status.transform.Find ("Gaiko").transform.Find ("Value").GetComponent<Text> ().text = targetGaikouKuniName + "(" + targetGaikouDaimyoName + ")";
					} else {
						sameDaimyoFlg = true;
                        status.transform.Find ("Gaiko").transform.Find ("Value").GetComponent<Text> ().text = Message.getMessage(219,langId);
                        
                    }
				} else {                                        
                    status.transform.Find("Gaiko").transform.Find("Value").GetComponent<Text>().text = Message.getMessage(219,langId);
                    
                }

				//Doumei
				string doumeiTmp = "doumei" + daimyoId.ToString ();
				string doumeiString = PlayerPrefs.GetString (doumeiTmp);
				List<string> doumeiList = new List<string> ();
				if (doumeiString != null && doumeiString != "") {
					if (doumeiString.Contains (",")) {
						doumeiList = new List<string> (doumeiString.Split (delimiterChars));
					} else {
						doumeiList.Add (doumeiString);
					}
				}


				//Exist Check
				if (doumeiList.Count != 0) {
					List<string> doumeiListTmp = new List<string> (doumeiList);
					for (int j = 0; j < doumeiListTmp.Count; j++) {
						string doumeiDaimyoId = doumeiListTmp [j];
						if (!seiryokuList.Contains (doumeiDaimyoId)) {
							doumeiList.Remove (doumeiDaimyoId);
						}
					}
				}
                string doumeiNameList = "";                                
                doumeiNameList = Message.getMessage(219,langId);
                

                for (int j = 0; j < doumeiList.Count; j++) {
					if (j == 0) {
						doumeiNameList = daimyo.getClanName (int.Parse (doumeiList [j]),langId);
					} else {
						doumeiNameList = doumeiNameList + "," + daimyo.getClanName(int.Parse (doumeiList [j]),langId);
					}
				}

				status.transform.Find ("Doumei").transform.Find ("Value").GetComponent<Text> ().text = doumeiNameList;
			
			} else {
				//Ge
				status.transform.Find ("Gaiko").transform.Find ("Value").GetComponent<Text> ().text = "?";
				status.transform.Find ("Doumei").transform.Find ("Value").GetComponent<Text> ().text = "?";

			}


			if (snbRank > 2) { //Jyo
				BusyoInfoGet busyo = new BusyoInfoGet();

				string qtyAndHeisyu = busyo.getDaimyoBusyoQtyHeisyu(daimyoId,langId);
				List<string> qtyAndHeisyuiList = new List<string> ();
				qtyAndHeisyuiList = new List<string> (qtyAndHeisyu.Split (delimiterChars));

				//BusyoQty
				//Heisyu
				status.transform.Find ("BusyoQty").transform.Find ("Value").GetComponent<Text> ().text = qtyAndHeisyuiList[0];
				status.transform.Find ("Heisyu").transform.Find ("Value").GetComponent<Text> ().text = qtyAndHeisyuiList[1];
			
			} else {
				//Cyu or Ge
				status.transform.Find ("BusyoQty").transform.Find ("Value").GetComponent<Text> ().text = "?";
				status.transform.Find ("Heisyu").transform.Find ("Value").GetComponent<Text> ().text = "?";

			}


			//Main Map
			foreach(Transform obj in board.transform){
				if (obj.name != "Explanation") {
					Destroy (obj.gameObject);
				}
			}


            //Create Map
            GameObject originalKuniMap = GameObject.Find("KuniMap");
            GameObject copiedKuniMap = Object.Instantiate(originalKuniMap) as GameObject;
            copiedKuniMap.transform.SetParent(board.transform);
            copiedKuniMap.transform.localScale = new Vector2(1, 0.8f);
            Vector3 vect = copiedKuniMap.transform.Find(kuniId.ToString()).transform.localPosition;
            float adjstX = vect.x * -1;
            float adjustY = vect.y * -1;
            RectTransform mapRect = copiedKuniMap.GetComponent<RectTransform>();
            mapRect.anchoredPosition3D = new Vector3(adjstX, adjustY, 0);

            //Create Kamon
            GameObject originalKamon = GameObject.Find("KuniIconView");
            GameObject copiedKamon = Object.Instantiate(originalKamon) as GameObject;
            copiedKamon.transform.SetParent(board.transform);
            copiedKamon.transform.localScale = new Vector2(1, 0.8f);
            RectTransform kamonRect = copiedKamon.GetComponent<RectTransform>();
            kamonRect.anchoredPosition3D = new Vector3(adjstX, adjustY, 0);

            Entity_kuni_mst kuniMst = Resources.Load("Data/kuni_mst") as Entity_kuni_mst;
            Color whiteColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            for (int i = 0; i < kuniMst.param.Count; i++) {
                int subKuniId = i + 1;

                GameObject tmpKuniObj = copiedKuniMap.transform.Find(subKuniId.ToString()).gameObject;
                GameObject tmpKamonObj = copiedKamon.transform.Find(subKuniId.ToString()).gameObject;
                tmpKamonObj.GetComponent<Image>().color = whiteColor;

                if (subKuniId == kuniId) {
                    tmpKamonObj.GetComponent<Button>().enabled = false;
                    tmpKamonObj.GetComponent<Image>().sprite =
                    Resources.Load(imagePath, typeof(Sprite)) as Sprite;
                    
                    //EFFECT
                    string effectPath = "Prefabs/EffectAnime/point_up";
                    GameObject pointUp = Instantiate(Resources.Load(effectPath)) as GameObject;
                    pointUp.transform.SetParent(tmpKamonObj.transform);
                    pointUp.transform.localScale = new Vector2(70, 70);
                    pointUp.transform.localPosition = new Vector2(0, 25);
                    pointUp.GetComponent<Fadeout>().enabled = false;
                } else { 
                    if (targetKuniList.Contains(subKuniId)) {
                        tmpKamonObj.GetComponent<Button>().enabled = false;
                        int subDaimyoId = int.Parse(seiryokuList[subKuniId - 1]);

                        if (daimyoId != subDaimyoId) {

                            //yukoudo
                            int yukoudoValue = gaikou.getExistGaikouValue(daimyoId, subDaimyoId);
                            string syukoudoPath = "Prefabs/Map/cyouhou/YukoudoLabel";
                            GameObject yukoudoObj = Instantiate(Resources.Load(syukoudoPath)) as GameObject;
                            yukoudoObj.transform.SetParent(tmpKamonObj.transform);
                            yukoudoObj.GetComponent<Text>().text = yukoudoValue.ToString();
                            yukoudoObj.transform.localScale = new Vector2(0.08f, 0.1f);
                            yukoudoObj.transform.localPosition = new Vector2(0, 26);

                        }
                        else {
                            tmpKamonObj.GetComponent<Image>().sprite =
                                Resources.Load(imagePath, typeof(Sprite)) as Sprite;
                        }

                        if (targetKuniId != 0) {
                            if (targetKuniId == subKuniId) {
                                //kassen target
                                Color atkColor = new Color(180f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
                                tmpKamonObj.GetComponent<Image>().color = atkColor;
                            }
                        }
                        if (snbRank > 1) {
                            if (targetGaikouKuniId != 0) {
                                if (targetGaikouKuniId == subKuniId) {
                                    if (!sameDaimyoFlg) {
                                        //gaikou target
                                        Color gaikouColor = new Color(80f / 255f, 100f / 255f, 185f / 255f, 255f / 255f);
                                        tmpKamonObj.GetComponent<Image>().color = gaikouColor;
                                    }
                                }
                            }
                        }
                    }
                    else {
                        Color noSubKuniColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 40f / 255f);
                        tmpKuniObj.GetComponent<Image>().color = noSubKuniColor;

                        tmpKamonObj.SetActive(false);
                    }
                }
            }
                /*
                string kuniMapPath = "Prefabs/Map/cyouhou/kuniImage";
                GameObject mainMap = Instantiate (Resources.Load (kuniMapPath)) as GameObject;
                mainMap.transform.SetParent(board.transform);
                mainMap.transform.localScale = new Vector2 (13, 9);
                mainMap.name = "kuniMap" + kuniId;
                string kuniImagePath = "Prefabs/Map/kuniMap/" + kuniId.ToString ();
                mainMap.GetComponent<Image> ().sprite = 
                    Resources.Load (kuniImagePath, typeof(Sprite)) as Sprite;

                int baseX = kuni.getKuniLocationX (kuniId);
                int baseY = kuni.getKuniLocationY (kuniId);
                int adjstX = baseX * -1;
                int adjustY = baseY * -1;

                float colorR = daimyo.getColorR (daimyoId);
                float colorG = daimyo.getColorG (daimyoId);
                float colorB = daimyo.getColorB (daimyoId);
                Color kuniColor = new Color (colorR / 255f, colorG / 255f, colorB / 255f, 255f / 255f);
                mainMap.GetComponent<Image> ().color = kuniColor;

                RectTransform mapRect = mainMap.GetComponent<RectTransform> ();
                mapRect.anchoredPosition3D = new Vector3 (adjstX, adjustY, 0);

                //My Kuni Kamon Icon
                string kamonBackPath ="Prefabs/Map/cyouhou/KamonBack";
                GameObject kamonBack = Instantiate (Resources.Load (kamonBackPath)) as GameObject;
                kamonBack.transform.SetParent (board.transform);
                kamonBack.transform.localScale = new Vector2 (1, 1);
                kamonBack.transform.localPosition = new Vector2(0,0);

                string kamonKuniPath = "Prefabs/Map/Kuni/" + kuniId.ToString();
                GameObject kamonObj = Instantiate (Resources.Load (kamonKuniPath)) as GameObject;
                kamonObj.transform.SetParent (kamonBack.transform);
                kamonObj.transform.localScale = new Vector2 (1, 0.8f);
                kamonObj.transform.localPosition = new Vector2(0,0);
                kamonObj.GetComponent<Image> ().sprite = 
                    Resources.Load (imagePath, typeof(Sprite)) as Sprite;
                kamonObj.GetComponent<Button> ().enabled = false;


                //Mapping Kuni
                Entity_kuni_mst kuniMst = Resources.Load ("Data/kuni_mst") as Entity_kuni_mst;
                for (int i=0; i < kuniMst.param.Count; i++) {
                //for (int i=0; i < targetKuniList.Count; i++) {
                    int subKuniId = i + 1;

                    GameObject subMap = Instantiate (Resources.Load (kuniMapPath)) as GameObject;
                    subMap.transform.SetParent(mainMap.transform);
                    subMap.transform.localScale = new Vector2 (1, 1);
                    subMap.transform.localPosition = new Vector2 (0, 0);

                    subMap.name = "kuniMap" + subKuniId;
                    string subKuniImagePath = "Prefabs/Map/kuniMap/" + subKuniId;
                    subMap.GetComponent<Image> ().sprite = 
                        Resources.Load (subKuniImagePath, typeof(Sprite)) as Sprite;


                    if (targetKuniList.Contains (subKuniId)) {
                        //color
                        int subDaimyoId = int.Parse (seiryokuList [subKuniId - 1]);
                        float subColorR = daimyo.getColorR (subDaimyoId);
                        float subColorG = daimyo.getColorG (subDaimyoId);
                        float subColorB = daimyo.getColorB (subDaimyoId);
                        Color subKuniColor = new Color (subColorR / 255f, subColorG / 255f, subColorB / 255f, 255f / 255f);
                        subMap.GetComponent<Image> ().color = subKuniColor;

                        //Kamon
                        string subKamonKuniPath = "Prefabs/Map/Kuni/" + subKuniId.ToString ();
                        GameObject subKamonObj = Instantiate (Resources.Load (subKamonKuniPath)) as GameObject;
                        subKamonObj.transform.SetParent (board.transform);
                        subKamonObj.transform.localScale = new Vector2 (1, 0.8f);
                        if (daimyoId != subDaimyoId) {
                            string subImagePath = "Prefabs/Kamon/" + subDaimyoId.ToString ();
                            subKamonObj.GetComponent<Image> ().sprite = 
                                Resources.Load (subImagePath, typeof(Sprite)) as Sprite;

                            //yukoudo
                            int yukoudoValue = gaikou.getExistGaikouValue (daimyoId, subDaimyoId);
                            string syukoudoPath = "Prefabs/Map/cyouhou/YukoudoLabel";
                            GameObject yukoudoObj = Instantiate (Resources.Load (syukoudoPath)) as GameObject;
                            yukoudoObj.transform.SetParent (subKamonObj.transform);
                            yukoudoObj.GetComponent<Text> ().text = yukoudoValue.ToString ();
                            yukoudoObj.transform.localScale = new Vector2 (0.08f, 0.1f);
                            yukoudoObj.transform.localPosition = new Vector2 (0, 26);

                        } else {
                            subKamonObj.GetComponent<Image> ().sprite = 
                                Resources.Load (imagePath, typeof(Sprite)) as Sprite;
                        }
                        subKamonObj.GetComponent<Button> ().enabled = false;

                        //Kamon adjustment
                        int subBaseX = kuni.getKuniLocationX (subKuniId);
                        int subBaseY = kuni.getKuniLocationY (subKuniId);
                        int subAdjstX = subBaseX - baseX;
                        int subAdjstY = subBaseY - baseY;

                        RectTransform subMapRect = subKamonObj.GetComponent<RectTransform> ();
                        subMapRect.anchoredPosition3D = new Vector3 (subAdjstX, subAdjstY, 0);

                        if (targetKuniId != 0) {
                            if (targetKuniId == subKuniId) {
                                //kassen target
                                Color atkColor = new Color (180f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
                                subKamonObj.GetComponent<Image> ().color = atkColor;
                            }
                        }

                        if (snbRank > 1) {
                            if (targetGaikouKuniId != 0) {
                                if (targetGaikouKuniId == subKuniId) {
                                    if (!sameDaimyoFlg) {
                                        //gaikou target
                                        Color gaikouColor = new Color (80f / 255f, 100f / 255f, 185f / 255f, 255f / 255f);
                                        subKamonObj.GetComponent<Image> ().color = gaikouColor;
                                    }
                                }
                            }
                        }
                    } else {
                        Color noSubKuniColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 40f / 255f);
                        subMap.GetComponent<Image> ().color = noSubKuniColor;
                    }

                }

        */




            }


	}


	public int getKassenTargetKuni(int srcKuniId, int srcDaimyoId, List<int> targetKuniList, GameObject kuniIconView, bool aggressiveFlg, List<string> seiryokuList, int myDaimyoId){

		int dstDaimyoId = 0;
		bool doumeiFlg = false;
		Gaikou gaikou = new Gaikou ();


		//Yukoudo Check
		int worstGaikouDaimyo = 0;
		int worstGaikouValue = 100;
		int worstGaikouKuni = 0;
		int worstHeiryokuValue = 100000000;


		for (int k=0; k<targetKuniList.Count; k++) {	
			
			SendParam sendParam = kuniIconView.transform.Find (targetKuniList [k].ToString ()).GetComponent<SendParam> ();

			if (aggressiveFlg) {
				//Find worst gaikou daimyo

				dstDaimyoId = int.Parse(seiryokuList [targetKuniList [k] - 1]);
				int gaikouValue = 0;

				if (srcDaimyoId != dstDaimyoId) {
					if (dstDaimyoId == myDaimyoId) {
						//Get Gaikou Value

						string eGaikouM = "gaikou" + srcDaimyoId;
						gaikouValue = PlayerPrefs.GetInt (eGaikouM);

					} else {
						//Gaikou Data Check
						string gaikouTemp = "";
						if (srcDaimyoId < dstDaimyoId) {
							gaikouTemp = srcDaimyoId + "gaikou" + dstDaimyoId;
						} else {
							gaikouTemp = dstDaimyoId + "gaikou" + srcDaimyoId;
						}

						if (PlayerPrefs.HasKey (gaikouTemp)) {
							//exsit
							gaikouValue = PlayerPrefs.GetInt (gaikouTemp);

						} else {
                            //non exist
                            //gaikou check
                            int senarioId = PlayerPrefs.GetInt("senarioId");
							gaikouValue = gaikou.getGaikouValue (srcDaimyoId, dstDaimyoId,senarioId);

						}
					}

					//Compare with Previous one
					if (worstGaikouValue > gaikouValue) {
						worstGaikouValue = gaikouValue;
						worstGaikouDaimyo = dstDaimyoId;
						worstGaikouKuni = targetKuniList [k];
					}
				}


			} else {
				//Find worst heiryoku daimyo

				dstDaimyoId = int.Parse(seiryokuList [targetKuniList [k] - 1]);

				if (srcDaimyoId != dstDaimyoId) {								
					//Heiryoku Check
					int heiryoku = sendParam.heiryoku;

					//Compare with Previous one
					if (worstHeiryokuValue > heiryoku) {
						worstHeiryokuValue = heiryoku;

						int gaikouValue = 0;
						if (dstDaimyoId == myDaimyoId) {
							//Get Gaikou Value
							string eGaikouM = "gaikou" + srcDaimyoId;
							gaikouValue = PlayerPrefs.GetInt (eGaikouM);

						} else {
							//Gaikou Data Check
							string gaikouTemp = "";
							if (srcDaimyoId < dstDaimyoId) {
								gaikouTemp = srcDaimyoId + "gaikou" + dstDaimyoId;
							} else {
								gaikouTemp = dstDaimyoId + "gaikou" + srcDaimyoId;
							}
							if (PlayerPrefs.HasKey (gaikouTemp)) {
								//exsit
								gaikouValue = PlayerPrefs.GetInt (gaikouTemp);
							} else {
                                //non exist
                                int senarioId = PlayerPrefs.GetInt("senarioId");
                                gaikouValue = gaikou.getGaikouValue (srcDaimyoId, dstDaimyoId,senarioId);
							}
						}
						worstGaikouValue = gaikouValue;
						worstGaikouDaimyo = dstDaimyoId;
						worstGaikouKuni = targetKuniList [k];

					}
				}

			}

		}//Loop End

		return worstGaikouKuni;
	}


	public int getGaikouTargetKuni(int srcKuniId, int srcDaimyoId, List<int> targetKuniList, GameObject kuniIconView, bool aggressiveFlg, List<string> seiryokuList, int myDaimyoId){
			

		//Yukoudo Check
		int bestGaikouDaimyo = 0;
		int bestGaikouValue = 0;
		int bestGaikouKuni = 0;
		int bestHeiryokuValue = 0;
		Gaikou gaikou = new Gaikou ();

		for (int k=0; k<targetKuniList.Count; k++) {

			SendParam sendParam = kuniIconView.transform.Find (targetKuniList [k].ToString ()).GetComponent<SendParam> ();

			if (aggressiveFlg) {
				//Find best gaikou daimyo

				int dstDaimyoId = int.Parse (seiryokuList [targetKuniList [k] - 1]);
				int gaikouValue = 0;

				if (srcDaimyoId != dstDaimyoId) {
					if (dstDaimyoId == myDaimyoId) {
						//Get Gaikou Value

						string eGaikouM = "gaikou" + srcDaimyoId;
						gaikouValue = PlayerPrefs.GetInt (eGaikouM);

					} else {
						//Gaikou Data Check
						string gaikouTemp = "";
						if (srcDaimyoId < dstDaimyoId) {
							gaikouTemp = srcDaimyoId + "gaikou" + dstDaimyoId;
						} else {
							gaikouTemp = dstDaimyoId + "gaikou" + srcDaimyoId;
						}

						if (PlayerPrefs.HasKey (gaikouTemp)) {
							//exsit
							gaikouValue = PlayerPrefs.GetInt (gaikouTemp);

						} else {
                            //non exist
                            //gaikou check
                            int senarioId = PlayerPrefs.GetInt("senarioId");
                            gaikouValue = gaikou.getGaikouValue (srcDaimyoId, dstDaimyoId,senarioId);

						}
					}

					//Compare with Previous one
					//Best one
					if (gaikouValue >= bestGaikouValue) {
						bestGaikouValue = gaikouValue;
						bestGaikouDaimyo = dstDaimyoId;
						bestGaikouKuni = targetKuniList [k];

					}
				}


			} else {
				//Find best heiryoku daimyo
				int dstDaimyoId = int.Parse (seiryokuList [targetKuniList [k] - 1]);

				if (srcDaimyoId != dstDaimyoId) {								
					//Heiryoku Check
					int heiryoku = sendParam.heiryoku;

					//Compare with Previous one
					if (heiryoku >= bestHeiryokuValue) {
						bestHeiryokuValue = heiryoku;

						int gaikouValue = 0;
						if (dstDaimyoId == myDaimyoId) {
							//Get Gaikou Value
							string eGaikouM = "gaikou" + srcDaimyoId;
							gaikouValue = PlayerPrefs.GetInt (eGaikouM);

						} else {
							//Gaikou Data Check
							string gaikouTemp = "";
							if (srcDaimyoId < dstDaimyoId) {
								gaikouTemp = srcDaimyoId + "gaikou" + dstDaimyoId;
							} else {
								gaikouTemp = dstDaimyoId + "gaikou" + srcDaimyoId;
							}
							if (PlayerPrefs.HasKey (gaikouTemp)) {
								//exsit
								gaikouValue = PlayerPrefs.GetInt (gaikouTemp);
							} else {
                                //non exist
                                int senarioId = PlayerPrefs.GetInt("senarioId");
                                gaikouValue = gaikou.getGaikouValue (srcDaimyoId, dstDaimyoId,senarioId);
							}
						}
						bestGaikouValue = gaikouValue;
						bestGaikouDaimyo = dstDaimyoId;
						bestGaikouKuni = targetKuniList [k];
					}


				}

			}

		}//Loop End

		return bestGaikouKuni;
	}
}
