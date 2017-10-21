using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class DoDoumeiHaki : MonoBehaviour {

	public int daimyoId = 0;
	public string daimyoName = "";

	// Use this for initialization
	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (name == "YesButton") {
			Message msg = new Message();
			audioSources [0].Play ();

			int nowHyourou = PlayerPrefs.GetInt ("hyourou");
			if(nowHyourou >= 5){

				//Hyourou
				int newHyourou = nowHyourou - 5;
				PlayerPrefs.SetInt ("hyourou", newHyourou);
				GameObject.Find ("HyourouCurrentValue").GetComponent<Text> ().text = newHyourou.ToString ();

				//Haki Delete Doumei
				string myDoumei = PlayerPrefs.GetString ("doumei");
				char[] delimiterChars = {','};
				List<string> myDoumeiList = new List<string> ();
				if(myDoumei.Contains(",")){
					myDoumeiList = new List<string> (myDoumei.Split (delimiterChars));
				}else{
					myDoumeiList.Add(myDoumei);
				}
				myDoumeiList.Remove(daimyoId.ToString());

				string newMyDoumei = "";
				for(int i=0; i<myDoumeiList.Count; i++){
					if(i==0){
						newMyDoumei = myDoumeiList[i];
					}else{
						newMyDoumei = newMyDoumei + "," + myDoumeiList[i];
					}
				}
				PlayerPrefs.SetString ("doumei",newMyDoumei);

				string temp = "doumei" + daimyoId;
				string otherDoumei = PlayerPrefs.GetString (temp);
				List<string> otherDoumeiList = new List<string> ();
				if(otherDoumei.Contains(",")){
					otherDoumeiList = new List<string> (otherDoumei.Split (delimiterChars));
				}else{
					otherDoumeiList.Add(otherDoumei);
				}
				int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
				otherDoumeiList.Remove(myDaimyo.ToString());

				string newOtherDoumei = "";
				for(int i=0; i<otherDoumeiList.Count; i++){
					if(i==0){
						newOtherDoumei = otherDoumeiList[i];
					}else{
						newOtherDoumei = newOtherDoumei + "," + otherDoumeiList[i];
					}
				}
				PlayerPrefs.SetString (temp,newOtherDoumei);


				//Reduce Yukoudo
				int reduceYukoudo = UnityEngine.Random.Range(5,20);
				string tempGaikou = "gaikou" + daimyoId;
				int nowYukoudo = 0;
				if (PlayerPrefs.HasKey (tempGaikou)) {
					nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
				} else {
					nowYukoudo = 50;
				}
				int newYukoudo = nowYukoudo - reduceYukoudo;
				PlayerPrefs.SetInt (tempGaikou, newYukoudo);
				GameObject.Find ("YukouValue").GetComponent<Text> ().text = newYukoudo.ToString (); 
				PlayerPrefs.Flush ();

				//Icon & Parametor
				string seiryoku = PlayerPrefs.GetString ("seiryoku");
				List<string> seiryokuList = new List<string> ();
				seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
				GameObject KuniIconView = GameObject.Find ("KuniIconView").gameObject;

				Color openKuniColor = new Color (255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f); //Yellow
				string openKuni = PlayerPrefs.GetString ("openKuni");
				List<string> openKuniList = new List<string> ();
				if (openKuni != null && openKuni != "") {
					if (openKuni.Contains (",")) {
						openKuniList = new List<string> (openKuni.Split (delimiterChars));
					} else {
						openKuniList.Add (openKuni);
					}
				}

				Color normalColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f); //White
				for(int i=0;i<seiryokuList.Count;i++){
					int tempDaimyoId = int.Parse (seiryokuList [i]);

					if(tempDaimyoId == daimyoId){
						int kuniId = i + 1;
						GameObject kuniIcon = KuniIconView.transform.FindChild(kuniId.ToString()).gameObject;
						if (openKuniList.Contains (kuniId.ToString ())) {
							kuniIcon.GetComponent<Image> ().color = openKuniColor;
						} else {
							kuniIcon.GetComponent<Image> ().color = normalColor;
						}
						kuniIcon.GetComponent<SendParam>().doumeiFlg = false;
						kuniIcon.GetComponent<SendParam> ().myYukouValue = newYukoudo;
					}
				}

                //Message
                string OKtext = "";
                int langId = PlayerPrefs.GetInt("langId");
                if (langId == 2) {
                    OKtext = "Renounced alliance with " + daimyoName + ".\n Friendship decreased " + reduceYukoudo + " point";
                }else {
                    OKtext = daimyoName + "殿との同盟を破棄致しました。\n友好度が" + reduceYukoudo + "下がりまする。";
                }
				msg.makeMessage (OKtext);


				//Close
				Destroy(GameObject.Find("DoumeiHakiConfirm"));
				Destroy(GameObject.Find("Back(Clone)"));
				Destroy(GameObject.Find("smallBoard(Clone)"));
				Destroy(GameObject.Find("TouchBack(Clone)"));
				
			}else{
				msg.makeMessage (msg.getMessage(7));

				Destroy(GameObject.Find("DoumeiHakiConfirm"));
				Destroy(GameObject.Find("Back(Clone)"));
			}


		}else if(name == "NoButton"){
			//Back
			audioSources [1].Play ();

			Destroy(GameObject.Find("DoumeiHakiConfirm"));
			Destroy(GameObject.Find("Back(Clone)"));
		}
	}
}
