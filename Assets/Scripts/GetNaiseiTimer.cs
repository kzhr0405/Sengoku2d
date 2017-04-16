using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;	
using UnityEngine.UI;
using System;

public class GetNaiseiTimer : MonoBehaviour {


	public bool doneCyosyuFlg = false;
	public double timer = 0;
	public GameObject naiseiTimerObj;
	public GameObject seasonObj;
	public GameObject btn;
	public string targetSeason = "";
	public bool tempFlg = false;

	public Color NGColor = new Color (118f / 255f, 118f / 255f, 45f / 255f, 255f / 255f);
	public Color OKbtnColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
	public Color OKtxtColor = new Color (190f / 255f, 190f / 255f, 0f / 255f, 255f / 255f);

	public void Start () {

		timer = GameObject.Find ("GameController").GetComponent<MainStageController>().yearTimer;

		naiseiTimerObj = transform.FindChild("TimerText").gameObject;
		seasonObj = transform.FindChild("SeasonText").gameObject;
		btn = GameObject.Find ("GetShigenBtn").gameObject;

		//Season Handling & Target Shigen Handling
		string yearSeason = PlayerPrefs.GetString ("yearSeason");
		char[] delimiterChars = {','};
		string[] yearSeasonList = yearSeason.Split (delimiterChars);
		int seasonId = int.Parse(yearSeasonList[1]);
		GameObject GetTargetBack = GameObject.Find ("Syukaku").gameObject;


		doneCyosyuFlg = PlayerPrefs.GetBool ("doneCyosyuFlg");
		btn.GetComponent<GetAllShigen> ().doneCyosyuFlg = doneCyosyuFlg;

		//Reset
		GameObject moneyObj = GameObject.Find ("TargetMoney").gameObject;
		GameObject gunjyuObj = GameObject.Find ("TargetGunjyu").gameObject;
		GameObject hyourouObj = GameObject.Find ("TargetHyourou").gameObject;
		moneyObj.transform.FindChild ("TargetMoneyValue").GetComponent<Text> ().text = "0";
		gunjyuObj.transform.FindChild ("YR").transform.FindChild ("CyouheiYRValueL").GetComponent<Text> ().text = "0";
		gunjyuObj.transform.FindChild ("KB").transform.FindChild ("CyouheiKBValueL").GetComponent<Text> ().text = "0";
		gunjyuObj.transform.FindChild ("YM").transform.FindChild ("CyouheiYMValueL").GetComponent<Text> ().text = "0";
		gunjyuObj.transform.FindChild ("TP").transform.FindChild ("CyouheiTPValueL").GetComponent<Text> ().text = "0";
		gunjyuObj.transform.FindChild ("YR").transform.FindChild ("CyouheiYRValueM").GetComponent<Text> ().text = "0";
		gunjyuObj.transform.FindChild ("KB").transform.FindChild ("CyouheiKBValueM").GetComponent<Text> ().text = "0";
		gunjyuObj.transform.FindChild ("YM").transform.FindChild ("CyouheiYMValueM").GetComponent<Text> ().text = "0";
		gunjyuObj.transform.FindChild ("TP").transform.FindChild ("CyouheiTPValueM").GetComponent<Text> ().text = "0";
		gunjyuObj.transform.FindChild ("YR").transform.FindChild ("CyouheiYRValueH").GetComponent<Text> ().text = "0";
		gunjyuObj.transform.FindChild ("KB").transform.FindChild ("CyouheiKBValueH").GetComponent<Text> ().text = "0";
		gunjyuObj.transform.FindChild ("YM").transform.FindChild ("CyouheiYMValueH").GetComponent<Text> ().text = "0";
		gunjyuObj.transform.FindChild ("TP").transform.FindChild ("CyouheiTPValueH").GetComponent<Text> ().text = "0";
		gunjyuObj.transform.FindChild ("SNB").transform.FindChild ("SNBValueL").GetComponent<Text> ().text = "0";
		gunjyuObj.transform.FindChild ("SNB").transform.FindChild ("SNBValueM").GetComponent<Text> ().text = "0";
		gunjyuObj.transform.FindChild ("SNB").transform.FindChild ("SNBValueH").GetComponent<Text> ().text = "0";


		hyourouObj.transform.FindChild ("TargetHyourouValue").GetComponent<Text> ().text = "0";

		if (doneCyosyuFlg) {
			//Already Done Cyosyu
			btn.GetComponent<Image>().color = NGColor;
			btn.transform.FindChild("GetAllShigenText").GetComponent<Text>().color = NGColor;
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                btn.transform.FindChild("GetAllShigenText").GetComponent<Text>().text = "Done";
            } else {
                btn.transform.FindChild("GetAllShigenText").GetComponent<Text>().text = "徴収済";
            }

            if (seasonId == 4) {
                //Winter to Spring
                GameObject target = null;
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    transform.FindChild("SeasonText").GetComponent<Text>().text = "To Spring";
                    target = GameObject.Find("TargetMoney").gameObject;
                    targetSeason = "To Summer";
                    GetTargetBack.transform.FindChild("NowSeasonText").GetComponent<Text>().text = "Spring Gain";
                }else { 
                    transform.FindChild ("SeasonText").GetComponent<Text> ().text = "春まで";
				    target = GameObject.Find ("TargetMoney").gameObject;
				    targetSeason = "夏まで";               
                    GetTargetBack.transform.FindChild ("NowSeasonText").GetComponent<Text> ().text = "春収穫";
                }
                //Set Value
                int totalAmount = btn.GetComponent<GetAllShigen> ().totalMoney + btn.GetComponent<GetAllShigen>().totalKozanMoney;
				target.transform.FindChild ("TargetMoneyValue").GetComponent<Text> ().text = totalAmount.ToString();
				btn.GetComponent<GetAllShigen> ().cyosyuTarget = "money";

			} else if (seasonId == 1) {
                GameObject target = null;
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    transform.FindChild("SeasonText").GetComponent<Text>().text = "To Summer";
                    target = GameObject.Find("TargetGunjyu").gameObject;
                    targetSeason = "To Autumn";
                    GetTargetBack.transform.FindChild("NowSeasonText").GetComponent<Text>().text = "Summer Gain";
                }
                else {
                    transform.FindChild("SeasonText").GetComponent<Text>().text = "夏まで";
                    target = GameObject.Find("TargetGunjyu").gameObject;
                    targetSeason = "秋まで";
                    GetTargetBack.transform.FindChild("NowSeasonText").GetComponent<Text>().text = "夏収穫";
                }
                //Spring to Summer


				//Set Value
				target.transform.FindChild ("YR").transform.FindChild ("CyouheiYRValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYRL.ToString ();
				target.transform.FindChild ("KB").transform.FindChild ("CyouheiKBValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalKBL.ToString ();
				target.transform.FindChild ("YM").transform.FindChild ("CyouheiYMValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYML.ToString ();
				target.transform.FindChild ("TP").transform.FindChild ("CyouheiTPValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalTPL.ToString ();
				target.transform.FindChild ("YR").transform.FindChild ("CyouheiYRValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYRM.ToString ();
				target.transform.FindChild ("KB").transform.FindChild ("CyouheiKBValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalKBM.ToString ();
				target.transform.FindChild ("YM").transform.FindChild ("CyouheiYMValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYMM.ToString ();
				target.transform.FindChild ("TP").transform.FindChild ("CyouheiTPValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalTPM.ToString ();
				target.transform.FindChild ("YR").transform.FindChild ("CyouheiYRValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYRH.ToString ();
				target.transform.FindChild ("KB").transform.FindChild ("CyouheiKBValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalKBH.ToString ();
				target.transform.FindChild ("YM").transform.FindChild ("CyouheiYMValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYMH.ToString ();
				target.transform.FindChild ("TP").transform.FindChild ("CyouheiTPValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalTPH.ToString ();
				target.transform.FindChild ("SNB").transform.FindChild ("SNBValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalSNBL.ToString ();
				target.transform.FindChild ("SNB").transform.FindChild ("SNBValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalSNBM.ToString ();
				target.transform.FindChild ("SNB").transform.FindChild ("SNBValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalSNBH.ToString ();

				btn.GetComponent<GetAllShigen> ().cyosyuTarget = "gunjyu";

				//Kozan
				GetTargetBack.transform.FindChild ("TargetMoney").transform.FindChild ("TargetMoneyValue").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalKozanMoney.ToString ();

			} else if (seasonId == 2) {
                GameObject target = null;
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    transform.FindChild("SeasonText").GetComponent<Text>().text = "To Autumn";
                    target = GameObject.Find("TargetHyourou").gameObject;
                    btn.GetComponent<GetAllShigen>().cyosyuTarget = "hyourou";
                    targetSeason = "To Winter";
                    GetTargetBack.transform.FindChild("NowSeasonText").GetComponent<Text>().text = "Autumn Gain";
                }else { 
                    //Summer to Fall
                    transform.FindChild ("SeasonText").GetComponent<Text> ().text = "秋まで";
				    target = GameObject.Find ("TargetHyourou").gameObject;
				    btn.GetComponent<GetAllShigen> ().cyosyuTarget = "hyourou";
				    targetSeason = "冬まで";
				    GetTargetBack.transform.FindChild ("NowSeasonText").GetComponent<Text> ().text = "秋収穫";
                }

                //Set Value
                target.transform.FindChild ("TargetHyourouValue").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalHyourou.ToString ();
				btn.GetComponent<GetAllShigen> ().cyosyuTarget = "hyourou";

				//Kozan
				GetTargetBack.transform.FindChild ("TargetMoney").transform.FindChild ("TargetMoneyValue").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalKozanMoney.ToString ();

			} else if (seasonId == 3) {
                GameObject target = null;
                //Fall to Winter
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    transform.FindChild("SeasonText").GetComponent<Text>().text = "To Winter";
                    target = GameObject.Find("TargetGunjyu").gameObject;
                    targetSeason = "To Spring";
                    GetTargetBack.transform.FindChild("NowSeasonText").GetComponent<Text>().text = "Winter Gain";
                }else { 
                    transform.FindChild ("SeasonText").GetComponent<Text> ().text = "冬まで";
				    target = GameObject.Find ("TargetGunjyu").gameObject;
				    targetSeason = "春まで";
                    GetTargetBack.transform.FindChild ("NowSeasonText").GetComponent<Text> ().text = "冬収穫";
                }

                //Set Value
                btn.GetComponent<GetAllShigen> ().cyosyuTarget = "gunjyu";
				target.transform.FindChild ("YR").transform.FindChild ("CyouheiYRValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYRL.ToString ();
				target.transform.FindChild ("KB").transform.FindChild ("CyouheiKBValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalKBL.ToString ();
				target.transform.FindChild ("YM").transform.FindChild ("CyouheiYMValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYML.ToString ();
				target.transform.FindChild ("TP").transform.FindChild ("CyouheiTPValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalTPL.ToString ();
				target.transform.FindChild ("YR").transform.FindChild ("CyouheiYRValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYRM.ToString ();
				target.transform.FindChild ("KB").transform.FindChild ("CyouheiKBValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalKBM.ToString ();
				target.transform.FindChild ("YM").transform.FindChild ("CyouheiYMValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYMM.ToString ();
				target.transform.FindChild ("TP").transform.FindChild ("CyouheiTPValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalTPM.ToString ();
				target.transform.FindChild ("YR").transform.FindChild ("CyouheiYRValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYRH.ToString ();
				target.transform.FindChild ("KB").transform.FindChild ("CyouheiKBValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalKBH.ToString ();
				target.transform.FindChild ("YM").transform.FindChild ("CyouheiYMValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYMH.ToString ();
				target.transform.FindChild ("TP").transform.FindChild ("CyouheiTPValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalTPH.ToString ();
				target.transform.FindChild ("SNB").transform.FindChild ("SNBValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalSNBL.ToString ();
				target.transform.FindChild ("SNB").transform.FindChild ("SNBValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalSNBM.ToString ();
				target.transform.FindChild ("SNB").transform.FindChild ("SNBValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalSNBH.ToString ();


				//Kozan
				GetTargetBack.transform.FindChild ("TargetMoney").transform.FindChild ("TargetMoneyValue").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalKozanMoney.ToString ();

			}

			//Covered
			string coverPath = "Prefabs/Map/seiryoku/Covered";
			GameObject cover = Instantiate (Resources.Load (coverPath)) as GameObject;
			cover.transform.SetParent(GetTargetBack.transform);
			cover.transform.localScale = new Vector2 (1, 1);
			cover.transform.localPosition = new Vector2 (-130, 0);
			cover.name = "Covered";


		} else {
			//Not Cyosyu Yet
			btn.GetComponent<Image>().color = OKbtnColor;
			btn.transform.FindChild("GetAllShigenText").GetComponent<Text>().color = OKtxtColor;
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                btn.transform.FindChild("GetAllShigenText").GetComponent<Text>().text = "Gain";
            }else {
                btn.transform.FindChild("GetAllShigenText").GetComponent<Text>().text = "徴収";
            }
			if (seasonId == 1) {
                //Winter to Spring
                GameObject target = null;
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    transform.FindChild("SeasonText").GetComponent<Text>().text = "To Summer";
                    target = GameObject.Find("TargetMoney").gameObject;
                    //naiseiTimerObj.GetComponent<Text> ().text = "徴収可能";
                    targetSeason = "To Autumn";
                }else { 
                    transform.FindChild ("SeasonText").GetComponent<Text> ().text = "夏まで";
				    target = GameObject.Find ("TargetMoney").gameObject;
				    //naiseiTimerObj.GetComponent<Text> ().text = "徴収可能";
				    targetSeason = "秋まで";
                }
                //Set Value
                int totalAmount = btn.GetComponent<GetAllShigen> ().totalMoney + btn.GetComponent<GetAllShigen>().totalKozanMoney;
				target.transform.FindChild ("TargetMoneyValue").GetComponent<Text> ().text = totalAmount.ToString();
				btn.GetComponent<GetAllShigen> ().cyosyuTarget = "money";

			} else if (seasonId == 2) {
                //Spring to Summer
                GameObject target = null;
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    transform.FindChild("SeasonText").GetComponent<Text>().text = "To Autumn";
                    target = GameObject.Find("TargetGunjyu").gameObject;
                    targetSeason = "To Winter";
                } else {
                    transform.FindChild ("SeasonText").GetComponent<Text> ().text = "秋まで";
				    target = GameObject.Find ("TargetGunjyu").gameObject;
				    targetSeason = "冬まで";
                }

                //Set Value
                target.transform.FindChild ("YR").transform.FindChild ("CyouheiYRValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYRL.ToString ();
				target.transform.FindChild ("KB").transform.FindChild ("CyouheiKBValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalKBL.ToString ();
				target.transform.FindChild ("YM").transform.FindChild ("CyouheiYMValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYML.ToString ();
				target.transform.FindChild ("TP").transform.FindChild ("CyouheiTPValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalTPL.ToString ();
				target.transform.FindChild ("YR").transform.FindChild ("CyouheiYRValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYRM.ToString ();
				target.transform.FindChild ("KB").transform.FindChild ("CyouheiKBValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalKBM.ToString ();
				target.transform.FindChild ("YM").transform.FindChild ("CyouheiYMValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYMM.ToString ();
				target.transform.FindChild ("TP").transform.FindChild ("CyouheiTPValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalTPM.ToString ();
				target.transform.FindChild ("YR").transform.FindChild ("CyouheiYRValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYRH.ToString ();
				target.transform.FindChild ("KB").transform.FindChild ("CyouheiKBValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalKBH.ToString ();
				target.transform.FindChild ("YM").transform.FindChild ("CyouheiYMValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYMH.ToString ();
				target.transform.FindChild ("TP").transform.FindChild ("CyouheiTPValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalTPH.ToString ();
				target.transform.FindChild ("SNB").transform.FindChild ("SNBValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalSNBL.ToString ();
				target.transform.FindChild ("SNB").transform.FindChild ("SNBValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalSNBM.ToString ();
				target.transform.FindChild ("SNB").transform.FindChild ("SNBValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalSNBH.ToString ();

				btn.GetComponent<GetAllShigen> ().cyosyuTarget = "gunjyu";
				
				//Kozan
				GetTargetBack.transform.FindChild ("TargetMoney").transform.FindChild ("TargetMoneyValue").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalKozanMoney.ToString ();
				
			} else if (seasonId == 3) {
                GameObject target = null;
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    transform.FindChild("SeasonText").GetComponent<Text>().text = "To Winter";
                    target = GameObject.Find("TargetHyourou").gameObject;
                    btn.GetComponent<GetAllShigen>().cyosyuTarget = "hyourou";
                    targetSeason = "To Spring";
                }else { 
                    transform.FindChild ("SeasonText").GetComponent<Text> ().text = "冬まで";
				    target = GameObject.Find ("TargetHyourou").gameObject;
				    btn.GetComponent<GetAllShigen> ().cyosyuTarget = "hyourou";
				    targetSeason = "春まで";
                }
                //Set Value
                target.transform.FindChild ("TargetHyourouValue").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalHyourou.ToString ();
				btn.GetComponent<GetAllShigen> ().cyosyuTarget = "hyourou";
				
				//Kozan
				GetTargetBack.transform.FindChild ("TargetMoney").transform.FindChild ("TargetMoneyValue").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalKozanMoney.ToString ();
				
			} else if (seasonId == 4) {
                GameObject target = null;
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    //Fall to Winter
                    transform.FindChild("SeasonText").GetComponent<Text>().text = "To Spring";
                    target = GameObject.Find("TargetGunjyu").gameObject;
                    targetSeason = "To Summer";
                }else {
                    //Fall to Winter
                    transform.FindChild ("SeasonText").GetComponent<Text> ().text = "春まで";
			        target = GameObject.Find ("TargetGunjyu").gameObject;
			        targetSeason = "夏まで";
                }
 
                //Set Value
                btn.GetComponent<GetAllShigen> ().cyosyuTarget = "gunjyu";
				target.transform.FindChild ("YR").transform.FindChild ("CyouheiYRValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYRL.ToString ();
				target.transform.FindChild ("KB").transform.FindChild ("CyouheiKBValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalKBL.ToString ();
				target.transform.FindChild ("YM").transform.FindChild ("CyouheiYMValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYML.ToString ();
				target.transform.FindChild ("TP").transform.FindChild ("CyouheiTPValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalTPL.ToString ();
				target.transform.FindChild ("YR").transform.FindChild ("CyouheiYRValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYRM.ToString ();
				target.transform.FindChild ("KB").transform.FindChild ("CyouheiKBValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalKBM.ToString ();
				target.transform.FindChild ("YM").transform.FindChild ("CyouheiYMValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYMM.ToString ();
				target.transform.FindChild ("TP").transform.FindChild ("CyouheiTPValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalTPM.ToString ();
				target.transform.FindChild ("YR").transform.FindChild ("CyouheiYRValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYRH.ToString ();
				target.transform.FindChild ("KB").transform.FindChild ("CyouheiKBValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalKBH.ToString ();
				target.transform.FindChild ("YM").transform.FindChild ("CyouheiYMValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalYMH.ToString ();
				target.transform.FindChild ("TP").transform.FindChild ("CyouheiTPValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalTPH.ToString ();
				target.transform.FindChild ("SNB").transform.FindChild ("SNBValueL").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalSNBL.ToString ();
				target.transform.FindChild ("SNB").transform.FindChild ("SNBValueM").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalSNBM.ToString ();
				target.transform.FindChild ("SNB").transform.FindChild ("SNBValueH").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalSNBH.ToString ();

				
				//Kozan
				GetTargetBack.transform.FindChild ("TargetMoney").transform.FindChild ("TargetMoneyValue").GetComponent<Text> ().text = btn.GetComponent<GetAllShigen> ().totalKozanMoney.ToString ();
				
			}
		}
	}

	void Update(){


		//Countdown
		timer -= Time.deltaTime;
			
		if (timer > 0.0f) {
			//On Play
			TimeSpan ts = new TimeSpan (0, 0, (int)timer);
			string hms = ts.ToString ();
			naiseiTimerObj.GetComponent<Text> ().text = hms;
			
		} else {

			if(!tempFlg){
				//Now on waiting for Cyosyu
				seasonObj.GetComponent<Text> ().text = targetSeason;
				//naiseiTimerObj.GetComponent<Text> ().text = "徴収可能";

				//Btn
				btn.GetComponent<Image>().color = OKbtnColor;
				btn.transform.FindChild("GetAllShigenText").GetComponent<Text>().color = OKtxtColor;
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    btn.transform.FindChild("GetAllShigenText").GetComponent<Text>().text = "Gain";
                }else {
                    btn.transform.FindChild("GetAllShigenText").GetComponent<Text>().text = "徴収";
                }
				btn.GetComponent<GetAllShigen> ().doneCyosyuFlg = false;

				//Remove Cover
				if (GameObject.Find ("Covered") != null) {
					GameObject cover = GameObject.Find ("Covered").gameObject;
					Destroy (cover);
				}

				//Timer reset
				timer = GameObject.Find ("GameController").GetComponent<MainStageController>().yearTimer;

				//Season reset


				PlayerPrefs.SetBool ("doneCyosyuFlg", false);
				PlayerPrefs.Flush ();
				tempFlg = true;
			}
		}

	}
}