using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class clearOrGameOver : MonoBehaviour {

	public string myDaimyoName = "";
	public int myDaimyo = 0;

	void Start () {

		Resources.UnloadUnusedAssets ();

		bool gameOverFlg = PlayerPrefs.GetBool("gameOverFlg");
		if (gameOverFlg) {
            AudioSource[] seSources = GameObject.Find("SEController").GetComponents<AudioSource>();
            seSources[12].Stop();

            /*--------------------*/
            /*Game Over*/
            /*--------------------*/

            showSeiryoku();

			GameObject panel = GameObject.Find("Panel").gameObject;

			string backPath = "Prefabs/clearOrGameOver/Back";
			GameObject backObj = Instantiate(Resources.Load (backPath)) as GameObject;
			backObj.transform.SetParent(panel.transform);
			backObj.transform.localScale = new Vector2(1,1);	
			
			string popPath = "Prefabs/clearOrGameOver/KakejikuMetsubouPop";
			GameObject popObj = Instantiate(Resources.Load (popPath)) as GameObject;
			popObj.transform.SetParent(panel.transform);
			popObj.transform.localScale = new Vector2(1,1);

            //Change Name
            string exp = "";// popObj.transform.FindChild("ExpValue").GetComponent<Text>().text;
            Daimyo daimyo = new Daimyo();
            string clanName = daimyo.getClanName(myDaimyo);
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                exp = clanName + " clan was downfallen. " + myDaimyoName + " fell in battle and there is no one remember his name now.";
            }else {
                exp = clanName + "の一族は滅亡した。武運尽きた"+ myDaimyoName + "は、戦場の露と消え、今や彼の者を覚えるものは無い。";
            }   
			popObj.transform.FindChild("ExpValue").GetComponent<Text>().text = exp;

			//Change Yaer & Season
			string yearSeason = PlayerPrefs.GetString ("yearSeason");
			char[] delimiterChars = {','};
			string[] yearSeasonList = yearSeason.Split (delimiterChars);
			int nowYear = int.Parse (yearSeasonList [0]);
			int nowSeason = int.Parse (yearSeasonList [1]);
			GameObject yearSeasonObj = popObj.transform.FindChild ("YearValue").gameObject;
			yearSeasonObj.GetComponent<Text>().text = nowYear.ToString();
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                if (nowSeason == 1){
				    yearSeasonObj.transform.FindChild ("SeasonValue").GetComponent<Text>().text  = "Spring";

			    }else if(nowSeason == 2){
				    yearSeasonObj.transform.FindChild ("SeasonValue").GetComponent<Text>().text  = "Summer";

			    }else if(nowSeason == 3){
				    yearSeasonObj.transform.FindChild ("SeasonValue").GetComponent<Text>().text  = "Autumn";

			    }else if(nowSeason == 4){
				    yearSeasonObj.transform.FindChild ("SeasonValue").GetComponent<Text>().text  = "Winter";
			    }
            }else {
                if (nowSeason == 1) {
                    yearSeasonObj.transform.FindChild("SeasonValue").GetComponent<Text>().text = "春";

                }else if (nowSeason == 2) {
                    yearSeasonObj.transform.FindChild("SeasonValue").GetComponent<Text>().text = "夏";

                }else if (nowSeason == 3) {
                    yearSeasonObj.transform.FindChild("SeasonValue").GetComponent<Text>().text = "秋";

                }else if (nowSeason == 4) {
                    yearSeasonObj.transform.FindChild("SeasonValue").GetComponent<Text>().text = "冬";
                }
            }
			GameObject button = GameObject.Find("Button").gameObject;
			button.GetComponent<ShowDaimyoSelect>().fin = popObj;
			button.GetComponent<ShowDaimyoSelect>().panel = panel;
			button.GetComponent<ShowDaimyoSelect>().gameOverFlg = true;


		} else {
			//Audio Change
			AudioSource[] bgmSources = GameObject.Find ("BGMController").GetComponents<AudioSource> ();
			bgmSources [0].Stop ();
			bgmSources [1].Play();

			bool gameClearItemGetFlg = PlayerPrefs.GetBool ("gameClearItemGetFlg");		
			GameObject panel = GameObject.Find("Panel").gameObject;

			if(!gameClearItemGetFlg){
				//Never Got Item

				showSeiryoku();

				bool gameClearFlg = true;//PlayerPrefs.GetBool ("gameClearFlg");		
				if (gameClearFlg) {
					/*--------------------*/
					/*Game Clear*/
					/*--------------------*/
					string backPath = "Prefabs/clearOrGameOver/Back";
					GameObject backObj = Instantiate(Resources.Load (backPath)) as GameObject;
					backObj.transform.SetParent(panel.transform);
					backObj.transform.localScale = new Vector2(1,1);	

					string popPath = "Prefabs/clearOrGameOver/KakejikuPop";
					GameObject popObj = Instantiate(Resources.Load (popPath)) as GameObject;
					popObj.transform.SetParent(panel.transform);
					popObj.transform.localScale = new Vector2(1,1);	

					string particlePath = "Prefabs/clearOrGameOver/particle";
					GameObject particleObj = Instantiate(Resources.Load (particlePath)) as GameObject;
					particleObj.transform.SetParent(panel.transform);
					particleObj.transform.localPosition = new Vector2(0,300);	
					
					GameObject button = GameObject.Find("Button").gameObject;
					button.GetComponent<StaffRoll>().backObj = backObj;
					button.GetComponent<StaffRoll>().popObj = popObj;
					button.GetComponent<StaffRoll>().particleObj = particleObj;
					button.GetComponent<StaffRoll>().panel = panel;


                    //Change Name
                    string exp = "";
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        exp = "Finally " + myDaimyoName + " achieved the unification of the whole country. He will be able to create the time of peace as ruler.";
                    }else {
                        exp = "遂に" + myDaimyoName + "は、天下統一を果たした。これより天下人として、泰平の世を創っていくこととなる。";
                    }
                    popObj.transform.FindChild("ExpValue").GetComponent<Text>().text = exp;

					//Change Yaer & Season
					string yearSeason = PlayerPrefs.GetString ("yearSeason");
					char[] delimiterChars = {','};
					string[] yearSeasonList = yearSeason.Split (delimiterChars);
					int nowYear = int.Parse (yearSeasonList [0]);
					int nowSeason = int.Parse (yearSeasonList [1]);
					GameObject yearSeasonObj = popObj.transform.FindChild ("YearValue").gameObject;
					yearSeasonObj.GetComponent<Text>().text = nowYear.ToString();
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        if (nowSeason == 1) {
                            yearSeasonObj.transform.FindChild("SeasonValue").GetComponent<Text>().text = "Spring";
                        }else if (nowSeason == 2) {
                            yearSeasonObj.transform.FindChild("SeasonValue").GetComponent<Text>().text = "Summer";
                        }else if (nowSeason == 3) {
                            yearSeasonObj.transform.FindChild("SeasonValue").GetComponent<Text>().text = "Autumn";
                        }else if (nowSeason == 4) {
                            yearSeasonObj.transform.FindChild("SeasonValue").GetComponent<Text>().text = "Winter";
                        }
                    }else {
                        if (nowSeason == 1) {
                            yearSeasonObj.transform.FindChild("SeasonValue").GetComponent<Text>().text = "春";
                        }else if (nowSeason == 2) {
                            yearSeasonObj.transform.FindChild("SeasonValue").GetComponent<Text>().text = "夏";
                        }else if (nowSeason == 3) {
                            yearSeasonObj.transform.FindChild("SeasonValue").GetComponent<Text>().text = "秋";
                        }else if (nowSeason == 4) {
                            yearSeasonObj.transform.FindChild("SeasonValue").GetComponent<Text>().text = "冬";
                        }
                    }

                    //Change Kamon
                    string imagePath = "Prefabs/Kamon/" + myDaimyo.ToString ();
					popObj.transform.FindChild("Kamon").GetComponent<Image>().sprite = 
						Resources.Load (imagePath, typeof(Sprite)) as Sprite;
					


					/*Show Item*/
					//1st Item - Tenkahubu
					string itm1Path = "Prefabs/Item/Tenkahubu/tenkahubu";
					GameObject item1Obj = Instantiate(Resources.Load (itm1Path)) as GameObject;
					GameObject item1 = GameObject.Find("item1").gameObject;
					item1Obj.transform.SetParent(item1.transform);
					item1Obj.transform.localScale = new Vector2(1.0f,1.1f);	

					GameObject myKamon = item1Obj.transform.FindChild("kamon").gameObject;
					string kamonPath = "Prefabs/Kamon/MyDaimyoKamon/" + myDaimyo.ToString ();
					myKamon.GetComponent<Image> ().sprite = 
						Resources.Load (kamonPath, typeof(Sprite)) as Sprite;

					//2nd Item - S Rank Item
					string kahouCdId = "";
					string kahouCd = "";
					string kahouId = "";

					if(PlayerPrefs.HasKey("gameClearKahouCd")){
						kahouCd = PlayerPrefs.GetString("gameClearKahouCd");
						kahouId = PlayerPrefs.GetString("gameClearKahouId");
						kahouCdId = kahouCd + kahouId;
					}else{
						Kahou kahou = new Kahou();
						List<string> kahouRandom = new List<string> (){"bugu","kabuto","gusoku","meiba","cyadougu","chishikisyo","heihousyo"};
						int rdm = UnityEngine.Random.Range(0,7);
						kahouCd = kahouRandom[rdm];
						kahouId = kahou.getRamdomKahouId(kahouCd, "S").ToString();
						kahouCdId = kahouCd + kahouId.ToString();
						PlayerPrefs.SetString("gameClearKahouCd",kahouCd);
						PlayerPrefs.SetString("gameClearKahouId",kahouId);
						PlayerPrefs.Flush();
					}

					string itm2Path = "Prefabs/Item/Kahou/" + kahouCdId;
					GameObject item2Obj = Instantiate(Resources.Load (itm2Path)) as GameObject;
					GameObject item2 = GameObject.Find("item2").gameObject;
					item2Obj.transform.SetParent(item2.transform);
					item2Obj.transform.localScale = new Vector2(1.0f,1.1f);	
					RectTransform rectRank = item2Obj.transform.FindChild("Rank").GetComponent<RectTransform>();
					rectRank.anchoredPosition3D = new Vector3(30,-30,0);
					item2Obj.GetComponent<Button>().enabled = false;

					StaffRoll btn = GameObject.Find("Button").GetComponent<StaffRoll>();
					btn.kahouCd = kahouCd;
					btn.kahouId = kahouId;

					//3rd Item - BusyoDama
					string itm3Path = "Prefabs/Item/busyoDama";
					GameObject item3Obj = Instantiate(Resources.Load (itm3Path)) as GameObject;
					GameObject item3 = GameObject.Find("item3").gameObject;
					item3Obj.transform.SetParent(item3.transform);
					item3Obj.transform.localScale = new Vector2(1.0f,1.1f);	



				}
			}else{
				//Already got item
				GameObject kunimap = GameObject.Find("KuniMap").gameObject;
				GameObject kuniIconView = GameObject.Find("KuniIconView").gameObject;
				Destroy (kunimap.gameObject);
				Destroy (kuniIconView.gameObject);

				StaffRoll roll = new StaffRoll();
				roll.FinMaker(panel);
			}
		}
	}

	public void showSeiryoku(){

		/*--------------------*/
		/*Show Daimyo Seiryoku*/
		/*--------------------*/
		Entity_kuni_mst kuniMst = Resources.Load ("Data/kuni_mst") as Entity_kuni_mst;
		Entity_daimyo_mst daimyoMst = Resources.Load ("Data/daimyo_mst") as Entity_daimyo_mst;
		
		string seiryoku = PlayerPrefs.GetString ("seiryoku");
		List<string> seiryokuList = new List<string> ();
		char[] delimiterChars = {','};
		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
		
		string kuniPath = "Prefabs/Map/kuni/";
		GameObject kuniIconView = GameObject.Find ("KuniIconView");
		GameObject KuniMap = GameObject.Find ("KuniMap");
		myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
		Daimyo daimyo = new Daimyo ();
		myDaimyoName = daimyo.getName (myDaimyo);

		string gameClearDaimyo = PlayerPrefs.GetString ("gameClearDaimyo");
		List<string> gameClearDaimyoList = new List<string> ();
		if (gameClearDaimyo != null && gameClearDaimyo != "") {
			if (gameClearDaimyo.Contains (",")) {
				gameClearDaimyoList = new List<string> (gameClearDaimyo.Split (delimiterChars));
			}else{
				gameClearDaimyoList.Add(gameClearDaimyo);
			}
		}
		for (int i=0; i<kuniMst.param.Count; i++) {
			int kuniId = kuniMst.param [i].kunId;
			
			string newKuniPath = kuniPath + kuniId.ToString ();
			int locationX = kuniMst.param [i].locationX;
			int locationY = kuniMst.param [i].locationY;
			
			GameObject kuni = Instantiate (Resources.Load (newKuniPath)) as GameObject;
			
			kuni.transform.SetParent (kuniIconView.transform);
			kuni.name = kuniId.ToString ();
			kuni.transform.localScale = new Vector2 (1, 1);
			kuni.GetComponent<Button>().enabled = false;
			
			//Seiryoku Handling
			int daimyoId = int.Parse (seiryokuList [kuniId - 1]);			
			string daimyoName = daimyoMst.param [daimyoId - 1].daimyoName;
			int daimyoBusyoIdTemp = daimyoMst.param [daimyoId - 1].busyoId;
			
			//Color Handling
			float colorR = (float)daimyoMst.param [daimyoId - 1].colorR;
			float colorG = (float)daimyoMst.param [daimyoId - 1].colorG;
			float colorB = (float)daimyoMst.param [daimyoId - 1].colorB;
			Color kuniColor = new Color (colorR / 255f, colorG / 255f, colorB / 255f, 255f / 255f);
			
			KuniMap.transform.FindChild (kuni.name).GetComponent<Image> ().color = kuniColor;
			
			//Daimyo Kamon Image
			string imagePath = "Prefabs/Kamon/" + daimyoId.ToString ();
			kuni.GetComponent<Image> ().sprite = 
				Resources.Load (imagePath, typeof(Sprite)) as Sprite;
			RectTransform kuniTransform = kuni.GetComponent<RectTransform> ();
			kuniTransform.anchoredPosition = new Vector3 (locationX, locationY, 0);
			
			
			//My daimyo Check
			if (daimyoId == myDaimyo) {
				string myDaimyoPath = "Prefabs/Kamon/MyDaimyoKamon/" + myDaimyo.ToString ();
				kuni.GetComponent<Image> ().sprite = 
					Resources.Load (myDaimyoPath, typeof(Sprite)) as Sprite;

			}

			//Clear Flg
			if(gameClearDaimyoList.Contains(daimyoId.ToString())){
				kuni.GetComponent<SendParam> ().clearFlg = true;
			}

		}


	}
}
