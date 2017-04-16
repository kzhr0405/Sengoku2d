using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class DoKanjyo : MonoBehaviour {

	public string[] kanjyoList;

	// Use this for initialization
	public void OnClick () {

        string kanjyoQtyString = PlayerPrefs.GetString("kanjyo");
        char[] delimiterChars = { ',' };
        kanjyoList = kanjyoQtyString.Split(delimiterChars);


        GameObject ExpSliderObj = GameObject.Find ("ExpSlider");
		string busyoId = GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyo;
		string busyoName = GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyoName;

		/*Common Process*/
		//Disable buttons
		GameObject.Find ("DoKakyuKanjyo").GetComponent<Button>().enabled = false;
		GameObject.Find ("DoCyukyuKanjyo").GetComponent<Button>().enabled = false;
		GameObject.Find ("DoJyokyuKanjyo").GetComponent<Button>().enabled = false;
		GameObject.Find ("close").GetComponent<Button>().enabled = false;

		//Do Kanjyo by Rank
		if(name == "DoKakyuKanjyo"){
			DoKanjyoOperation(name);
		}else if(name == "DoCyukyuKanjyo"){
			DoKanjyoOperation(name);
		}else if(name == "DoJyokyuKanjyo"){
			DoKanjyoOperation(name);
		}
	}


	public void DoKanjyoOperation(string kanjyoTyp){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		GameObject ExpSliderObj = GameObject.Find ("ExpSlider");
		string busyoId = GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyo;
		string busyoName = GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyoName;

		//Classification
		string QtyValue = "";
		string ExValue = "";
		if (kanjyoTyp == "DoKakyuKanjyo") {
			QtyValue = "KakyuKanjyoQtyValue";
			ExValue = "KakyuKanjyoExpValue";

		}else if(kanjyoTyp == "DoCyukyuKanjyo"){
			QtyValue = "CyukyuKanjyoQtyValue";
			ExValue = "CyukyuKanjyoExpValue";

		}else if(kanjyoTyp == "DoJyokyuKanjyo"){
			QtyValue = "JyokyuKanjyoQtyValue";
			ExValue = "JyokyuKanjyoExpValue";
		}

		//Check exist or not
		GameObject qty = GameObject.Find (QtyValue);
		int kanjyoQty = int.Parse(qty.GetComponent<Text>().text);
		
		if (kanjyoQty == 0) {
			/*Error*/
			audioSources [4].Play ();
			Message msg = new Message(); 
			msg.makeMessage (msg.getMessage(64));
			
			GameObject.Find ("DoKakyuKanjyo").GetComponent<Button> ().enabled = true;
			GameObject.Find ("DoCyukyuKanjyo").GetComponent<Button> ().enabled = true;
			GameObject.Find ("DoJyokyuKanjyo").GetComponent<Button> ().enabled = true;
			GameObject.Find ("close").GetComponent<Button> ().enabled = true;
			
		} else {
			audioSources [3].Play ();
			/*Correct Case*/
			//reduce qty
			qty.GetComponent<Text> ().text = (kanjyoQty - 1).ToString ();
			
			//increase exp
			int nowLv = PlayerPrefs.GetInt (busyoId);
			string tempExp = "exp" + busyoId;
			int nowExp = PlayerPrefs.GetInt (tempExp);
			int newExp = nowExp + int.Parse (GameObject.Find (ExValue).GetComponent<Text> ().text);
			
			Exp exp = new Exp ();
            string addLvTmp = "addlv" + busyoId.ToString();
            int maxLv = 100 + PlayerPrefs.GetInt(addLvTmp);
            if (maxLv > 200) {
                maxLv = 200;
            }

            int targetLv = exp.getLvbyTotalExp (nowLv, newExp, maxLv);
			int targetMaxExp = 0;
			List<float> maxExpList = new List<float> ();
			if(targetLv != maxLv) {
				targetMaxExp = exp.getExpforNextLv (targetLv);
				for (int k=nowLv; k<=targetLv; k++) {
					maxExpList.Add (exp.getDifExpforNextLv (k));
				}
			}else{
				int LvMaxExp = exp.getExpLvMax (maxLv);
				maxExpList.Add((float)LvMaxExp);
				newExp = LvMaxExp;
			}

			//Lv up
			if (nowLv != targetLv) {
				GameObject.Find ("kanjyo").GetComponent<BusyoStatusButton> ().pa_lv = targetLv;
			}

			if (targetLv != maxLv) {
				ExpSliderObj.GetComponent<ExpSlider> ().maxExpArray = maxExpList;  //Max Experience by Level
				ExpSliderObj.GetComponent<ExpSlider> ().i = 0; //
				ExpSliderObj.GetComponent<ExpSlider> ().startLv = nowLv; //
				ExpSliderObj.GetComponent<ExpSlider> ().nowExp = float.Parse (GameObject.Find ("CurrentExpValue").GetComponent<Text> ().text); //
				ExpSliderObj.GetComponent<ExpSlider> ().kanjyoExp = float.Parse (GameObject.Find (ExValue).GetComponent<Text> ().text);
				ExpSliderObj.GetComponent<ExpSlider> ().newExp = newExp; //
				ExpSliderObj.GetComponent<ExpSlider> ().targetMaxExp = targetMaxExp; //
			
				ExpSliderObj.GetComponent<Slider> ().value = float.Parse (GameObject.Find ("CurrentExpValue").GetComponent<Text> ().text);
				ExpSliderObj.GetComponent<Slider> ().maxValue = maxExpList [0];
			

				//Set Qty of Kanjyo
				string kanjyoQtyString = "";
				if (kanjyoTyp == "DoKakyuKanjyo") {
					kanjyoQtyString = (kanjyoQty - 1).ToString () + "," + kanjyoList [1] + "," + kanjyoList [2];

				} else if (kanjyoTyp == "DoCyukyuKanjyo") {
					kanjyoQtyString = kanjyoList [0] + "," + (kanjyoQty - 1).ToString () + "," + kanjyoList [2];

				} else if (kanjyoTyp == "DoJyokyuKanjyo") {
					kanjyoQtyString = kanjyoList [0] + "," + kanjyoList [1] + "," + (kanjyoQty - 1).ToString ();
				}
				//Run
				ExpSliderObj.GetComponent<ExpSlider> ().enabled = true;
				PlayerPrefs.SetString ("kanjyo", kanjyoQtyString);

			} else {
				//Lv100
				GameObject.Find ("close").GetComponent<CloseBoard> ().onClick ();

				Color ngImageColor = new Color (40f / 255f, 40f / 255f, 40f / 255f, 180f / 255f);
				Color ngTextColor = new Color (90f / 255f, 90f / 255f, 90f / 255f, 90f / 255f);
				GameObject kanjyo = GameObject.Find("kanjyo").gameObject;
				kanjyo.GetComponent<Image> ().color = ngImageColor; 
				kanjyo.transform.FindChild ("Text").GetComponent<Text> ().color = ngTextColor; 
				kanjyo.GetComponent<Button> ().enabled = false;
			}

			PlayerPrefs.SetInt (tempExp, newExp);
			PlayerPrefs.SetInt (busyoId, targetLv);
			PlayerPrefs.SetBool ("questDailyFlg25",true);

			PlayerPrefs.Flush();

            string kanjyoText = "";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                kanjyoText = "You gave certificate to " + busyoName + ".";
            }else {
                kanjyoText = busyoName + "に感状を与えました。";
            }
			Message msg = new Message(); 
			msg.makeMessage (kanjyoText);


		}
	}
}
