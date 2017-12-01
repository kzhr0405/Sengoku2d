using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class UpdateNaisei : MonoBehaviour {

	public int activeKuniId = 0;
	public int requiredMoney = 0;
	public int requiredHyourou = 0;
	public string areaId = ""; //1-22 or shiro_x
	public int naiseiId = 0;
	public int targetLv = 0;
	public string naiseiName = "";

	public void OnClick () {

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		//Money Check
		Message msg = new Message(); 
		int nowMoney = PlayerPrefs.GetInt ("money");
		int nowHyourou = PlayerPrefs.GetInt ("hyourou");

		if (nowMoney < requiredMoney) {
			//Error
			//Message
			audioSources [4].Play ();
			msg.makeMessage (msg.getMessage(6));
			
		} else {
			if (nowHyourou < requiredHyourou) {
				//Error
				audioSources [4].Play ();
				//msg.makeMessage(msg.getMessage(7));
                msg.hyourouMovieMessage();
                GameObject.Find("close").GetComponent<CloseBoard>().onClick();
            } else {
				audioSources [3].Play ();

				//Update
				//Track
				int TrackBuildMoneyNo = PlayerPrefs.GetInt("TrackBuildMoneyNo",0);
				TrackBuildMoneyNo = TrackBuildMoneyNo + requiredMoney;
				PlayerPrefs.SetInt("TrackBuildMoneyNo",TrackBuildMoneyNo);


				string temp = "naisei" + activeKuniId.ToString ();
				if (PlayerPrefs.HasKey (temp)) {
					//Money Reduce
					nowMoney = nowMoney - requiredMoney;
					PlayerPrefs.SetInt ("money", nowMoney);

					//Hyourou Reduce
					nowHyourou = nowHyourou - requiredHyourou;
					PlayerPrefs.SetInt ("hyourou", nowHyourou);

					/*Update Lv*/
					//Get Data
					string naiseiString = PlayerPrefs.GetString (temp);
					List<string> naiseiList = new List<string>();
					char[] delimiterChars = {','};
					naiseiList = new List<string>(naiseiString.Split (delimiterChars));

					//replace vtarget column's value by area id
					if(areaId.Contains("shiro")){
						//shiro
						naiseiList[0] = targetLv.ToString();

					}else{
						//the other
						string targetValue = naiseiId.ToString() + ":" + targetLv.ToString();
						naiseiList[int.Parse(areaId)] = targetValue;
					
					}

					//Remake string
					string newNaiseiString = "";
					for(int i=0; i<naiseiList.Count; i++){
						if(i+1 != naiseiList.Count){
							newNaiseiString = newNaiseiString + naiseiList[i] + ",";
						}else{
							newNaiseiString = newNaiseiString + naiseiList[i];
						}
					}
					PlayerPrefs.SetString (temp,newNaiseiString);
					PlayerPrefs.SetBool ("questDailyFlg16",true);
					PlayerPrefs.Flush();


				}else{
					Debug.Log ("no possible error");
				}

                //Message
                string OKtext = "";
                int langId = PlayerPrefs.GetInt("langId");
                if (langId == 2) {
                    OKtext = "You upgraded " + naiseiName + ".\n The country is thriving.";
                }else {
                    OKtext = naiseiName + "を開発しましたぞ。\nますます国が栄えますな。";
                }
				GameObject msgObj = msg.makeMessage (OKtext);

                //Close Tab
                GameObject.Find ("close").GetComponent<CloseBoard> ().onClick ();

                //Initialization
                NaiseiController naisei = new NaiseiController();
				naisei.Start ();
                NaiseiController naiseiObjScript = GameObject.Find("NaiseiController").GetComponent<NaiseiController>();
                naiseiObjScript.total = int.Parse(GameObject.Find("Tabibito").transform.Find("TabibitoMaxValue").GetComponent<Text>().text);
                naiseiObjScript.remain = int.Parse(GameObject.Find("Tabibito").transform.Find("TabibitoCountDownValue").GetComponent<Text>().text);


            }
        }
	}	
}
