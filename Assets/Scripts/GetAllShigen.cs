using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class GetAllShigen : MonoBehaviour {

	public bool doneCyosyuFlg = false;
	public int totalMoney = 0;
	public int totalKozanMoney = 0;
	public int totalHyourou = 0;
	public int totalYRL = 0;
	public int totalKBL = 0;
	public int totalYML = 0;
	public int totalTPL = 0;
	public int totalYRM = 0;
	public int totalKBM = 0;
	public int totalYMM = 0;
	public int totalTPM = 0;
	public int totalYRH = 0;
	public int totalKBH = 0;
	public int totalYMH = 0;
	public int totalTPH = 0;
	public int totalSNBL = 0;
	public int totalSNBM = 0;
	public int totalSNBH = 0;

	public string cyosyuTarget = "";

	public void OnClick () {

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (!doneCyosyuFlg) {
			audioSources [3].Play ();
            doneCyosyuFlg = true;
            MainStageController main = new MainStageController();

            /*** season change Start ***/
            string yearSeason = PlayerPrefs.GetString("yearSeason");
            char[] delimiterChars = { ',' };
            string[] yearSeasonList = yearSeason.Split(delimiterChars);
            int nowYear = int.Parse(yearSeasonList[0]);
            int nowSeason = int.Parse(yearSeasonList[1]);

            if (nowSeason == 4) {
                nowYear = nowYear + 1;
                nowSeason = 1;
            }else {
                nowSeason = nowSeason + 1;
            }

            string newYearSeason = nowYear.ToString() + "," + nowSeason.ToString();
            PlayerPrefs.DeleteKey("bakuhuTobatsuDaimyoId");
            PlayerPrefs.SetString("yearSeason", newYearSeason);

            string lastSeasonChangeTime = System.DateTime.Now.ToString();
            PlayerPrefs.SetString("lastSeasonChangeTime", lastSeasonChangeTime);
            PlayerPrefs.DeleteKey("usedBusyo");
            DoNextSeason DoNextSeason = new DoNextSeason();
            DoNextSeason.deleteLinkCut();
            DoNextSeason.deleteWinOver();
            PlayerPrefs.Flush();

            //Change Label
            GameObject.Find("YearValue").GetComponent<Text>().text = nowYear.ToString();
            main.SetSeason(nowSeason);
            /*** season change End ***/
            

			PlayerPrefs.SetBool("doneCyosyuFlg", doneCyosyuFlg);
			string targetName = "";

			//Cyosyu Handling
			if(cyosyuTarget == "money"){
				int nowMoney = PlayerPrefs.GetInt ("money");
				int resultMoney = nowMoney + totalMoney;
				if (totalKozanMoney != 0) {
					resultMoney = resultMoney + totalKozanMoney;

				}
                if (resultMoney < 0) {
                    resultMoney = int.MaxValue;
                }
                PlayerPrefs.SetInt("money",resultMoney);
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    targetName = "Money";
                }else {
                    targetName = "金";
                }
				GameObject.Find ("MoneyValue").GetComponent<Text> ().text = resultMoney.ToString ();

				int TrackGetMoneyNo = PlayerPrefs.GetInt ("TrackGetMoneyNo",0);
				TrackGetMoneyNo = TrackGetMoneyNo + totalMoney + totalKozanMoney;
				PlayerPrefs.SetInt ("TrackGetMoneyNo",TrackGetMoneyNo);


			}else if(cyosyuTarget == "hyourou"){
				int nowHyourou = PlayerPrefs.GetInt ("hyourou");
				int maxHyourou = PlayerPrefs.GetInt ("hyourouMax");
				int resultHyourou = nowHyourou + totalHyourou;
				if(resultHyourou > maxHyourou) resultHyourou = maxHyourou;
				PlayerPrefs.SetInt("hyourou",resultHyourou);
				GameObject.Find ("HyourouCurrentValue").GetComponent<Text> ().text = resultHyourou.ToString();
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    targetName = "Stamina";
                }else {
                    targetName = "兵糧";
                }
				int TrackGetHyourouNo = PlayerPrefs.GetInt ("TrackGetHyourouNo",0);
				TrackGetHyourouNo = TrackGetHyourouNo + totalHyourou;
				PlayerPrefs.SetInt ("TrackGetHyourouNo",TrackGetHyourouNo);

				if (totalKozanMoney != 0) {
					int nowMoney = PlayerPrefs.GetInt ("money");
					int resultMoney = nowMoney + totalKozanMoney;
                    if (resultMoney < 0) {
                        resultMoney = int.MaxValue;
                    }
                    PlayerPrefs.SetInt("money",resultMoney);
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        targetName = targetName + " and Gold";
                    }else {
                        targetName = targetName + "と鉱山収入";
                    }

					GameObject.Find ("MoneyValue").GetComponent<Text> ().text = resultMoney.ToString ();

					int TrackGetMoneyNo = PlayerPrefs.GetInt ("TrackGetMoneyNo",0);
					TrackGetMoneyNo = TrackGetMoneyNo + totalKozanMoney;
					PlayerPrefs.SetInt ("TrackGetMoneyNo",TrackGetMoneyNo);
				}

			}else if(cyosyuTarget == "gunjyu"){
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    targetName = "Weapon";
                }else {
                    targetName = "軍需物資";
                }

				//YR
				if(totalYRL !=0 || totalYRM !=0 || totalYRH !=0 ){
					string cyoheiYRString = PlayerPrefs.GetString ("cyouheiYR");
					List<string> cyoheiYRList = new List<string>();
					cyoheiYRList = new List<string> (cyoheiYRString.Split (delimiterChars));

					int newYRL = totalYRL;
					int newYRM = totalYRM;
					int newYRH = totalYRH;

					newYRL = newYRL + int.Parse(cyoheiYRList[0]);
					newYRM = newYRM + int.Parse(cyoheiYRList[1]);
					newYRH = newYRH + int.Parse(cyoheiYRList[2]);

					string newCyoheiYR = newYRL + "," + newYRM + "," + newYRH;
					PlayerPrefs.SetString("cyouheiYR",newCyoheiYR);
				}
				//KB
				if(totalKBL !=0 || totalKBM !=0 || totalKBH !=0 ){
					string cyoheiKBString = PlayerPrefs.GetString ("cyouheiKB");
					List<string> cyoheiKBList = new List<string>();
					cyoheiKBList = new List<string> (cyoheiKBString.Split (delimiterChars));
					int newKBL = totalKBL;
					int newKBM = totalKBM;
					int newKBH = totalKBH;
					
					newKBL = newKBL + int.Parse(cyoheiKBList[0]);
					newKBM = newKBM + int.Parse(cyoheiKBList[1]);
					newKBH = newKBH + int.Parse(cyoheiKBList[2]);

					string newCyoheiKB = newKBL + "," + newKBM + "," + newKBH;
					PlayerPrefs.SetString("cyouheiKB",newCyoheiKB);
				}
				//YM
				if(totalYML !=0 || totalYMM !=0 || totalYMH !=0 ){
					string cyoheiYMString = PlayerPrefs.GetString ("cyouheiYM");
					List<string> cyoheiYMList = new List<string>();
					cyoheiYMList = new List<string> (cyoheiYMString.Split (delimiterChars));
					int newYML = totalYML;
					int newYMM = totalYMM;
					int newYMH = totalYMH;

					newYML = newYML + int.Parse(cyoheiYMList[0]);
					newYMM = newYMM + int.Parse(cyoheiYMList[1]);
					newYMH = newYMH + int.Parse(cyoheiYMList[2]);

					string newCyoheiYM = newYML + "," + newYMM + "," + newYMH;
					PlayerPrefs.SetString("cyouheiYM",newCyoheiYM);
				}
				//TP
				if(totalTPL !=0 || totalTPM !=0 || totalTPH !=0 ){
					string cyoheiTPString = PlayerPrefs.GetString ("cyouheiTP");
					List<string> cyoheiTPList = new List<string>();
					cyoheiTPList = new List<string> (cyoheiTPString.Split (delimiterChars));
					int newTPL = totalTPL;
					int newTPM = totalTPM;
					int newTPH = totalTPH;
					
					newTPL = newTPL + int.Parse(cyoheiTPList[0]);
					newTPM = newTPM + int.Parse(cyoheiTPList[1]);
					newTPH = newTPH + int.Parse(cyoheiTPList[2]);

					string newCyoheiTP = newTPL + "," + newTPM + "," + newTPH;
					PlayerPrefs.SetString("cyouheiTP",newCyoheiTP);
				}
				//SNB
				if(totalSNBL !=0 || totalSNBM !=0 || totalSNBH !=0 ){
					if(totalSNBL !=0){
						int SNBQty = PlayerPrefs.GetInt("shinobiGe");
						int newQty = SNBQty + totalSNBL;

						PlayerPrefs.SetInt("shinobiGe",newQty);
					}
					if(totalSNBM !=0){
						int SNBQty = PlayerPrefs.GetInt("shinobiCyu");
						int newQty = SNBQty + totalSNBM;
						
						PlayerPrefs.SetInt("shinobiCyu",newQty);
					}
					if(totalSNBH !=0){
						int SNBQty = PlayerPrefs.GetInt("shinobiJyo");
						int newQty = SNBQty + totalSNBH;
						
						PlayerPrefs.SetInt("shinobiJyo",newQty);
					}

				}
				int TrackGetSozaiNo= PlayerPrefs.GetInt ("TrackGetSozaiNo",0);
				TrackGetSozaiNo= TrackGetSozaiNo + totalYRL + totalKBL + totalYML + totalTPL + totalYRM + totalKBM + totalYMM + totalTPM + totalYRH + totalKBH + totalYMH + totalTPH + totalSNBL + totalSNBM + totalSNBH;
				PlayerPrefs.SetInt ("TrackGetSozaiNo",TrackGetSozaiNo);
	

				if (totalKozanMoney != 0) {
					int nowMoney = PlayerPrefs.GetInt ("money");
					int resultMoney = nowMoney + totalKozanMoney;
                    if (resultMoney < 0) {
                        resultMoney = int.MaxValue;
                    }
                    PlayerPrefs.SetInt("money",resultMoney);
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        targetName = targetName + " and Gold";
                    }else {
                        targetName = targetName + "と鉱山収入";
                    }

					GameObject.Find ("MoneyValue").GetComponent<Text> ().text = resultMoney.ToString ();

					int TrackGetMoneyNo = PlayerPrefs.GetInt ("TrackGetMoneyNo",0);
					TrackGetMoneyNo = TrackGetMoneyNo + totalKozanMoney;
					PlayerPrefs.SetInt ("TrackGetMoneyNo",TrackGetMoneyNo);
				}

			}

			PlayerPrefs.SetBool ("questDailyFlg38",true);
			PlayerPrefs.Flush();

			MainStageController mainStage = new MainStageController();
			mainStage.questExtension();

			//Message
			Message msg = new Message();
            string text = "";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                text = "My lord, you earned " + targetName + ".\nPlease enrich the country more by development.";
            }else {
                text = targetName + "を徴収しましたぞ。\n内政でより国を富ませましょう。";
            }
            msg.makeMessageOnBoard(text);

			//Restart
			GameObject.Find ("GameController").GetComponent<MainStageController>().doneCyosyuFlg = true;
			GameObject.Find("GetTimer").GetComponent<GetNaiseiTimer>().Start();
			GameObject.Find("GetTimer").GetComponent<GetNaiseiTimer>().timer = GameObject.Find ("GameController").GetComponent<MainStageController>().yearTimer;

            //tutorial
            if (Application.loadedLevelName == "tutorialMain") {
                Destroy(GameObject.Find("board").gameObject);
                Destroy(GameObject.Find("Back(Clone)").gameObject);
                PlayerPrefs.SetInt("tutorialId", 5);
                PlayerPrefs.Flush();

                TextController txtScript = GameObject.Find("TextBoard").transform.FindChild("Text").GetComponent<TextController>();
                txtScript.SetText(5);
                txtScript.SetNextLine();
                txtScript.tutorialId = 5;
                txtScript.actOnFlg = false;

                GameObject SubButtonViewLeft = GameObject.Find("SubButtonViewLeft").gameObject;
                GameObject.Find("SeiryokuInfo").transform.SetParent(SubButtonViewLeft.transform);
                
            }

            } else {
			audioSources [4].Play ();

			Message msg = new Message();
            string text = "";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                text = "Season hasn't changed.\nPlease wait a moment for collecting taxes.";
            }else {
                text = "まだ季節は変わっておりませぬぞ。\n徴収は今しばらくお待ち下さいませ。";
            }
            msg.makeMessageOnBoard(text);
		}
	}	
}
