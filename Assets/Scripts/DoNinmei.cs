using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class DoNinmei : MonoBehaviour {

	public string busyoId = "";
	public int kuniId = 0;
	public int jyosyuHei = 0;

	public void OnClick () {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		Message msgLine = new Message ();
		int nowMoney = PlayerPrefs.GetInt ("money");
		if (nowMoney < 1000) {
			audioSources [4].Play ();
			msgLine.makeMessage(msgLine.getMessage(6));

		} else {

			int nowHyourou = PlayerPrefs.GetInt("hyourou");
			if(nowHyourou<10){
				audioSources [4].Play ();
				//msgLine.makeMessage(msgLine.getMessage(7));
                msgLine.hyourouMovieMessage();
                GameObject.Find("close").GetComponent<CloseBoard>().onClick();
            }
            else{
				audioSources [3].Play ();
				//Reduce Money & Hyourou
				int newMoney = nowMoney - 1000;
				int newHyourou = nowHyourou - 10;
				PlayerPrefs.SetInt("money",newMoney);
				PlayerPrefs.SetInt("hyourou",newHyourou);

				//Jyosyu Ninmei
				if (Application.loadedLevelName == "naisei") {
					kuniId = GameObject.Find ("NaiseiController").GetComponent<NaiseiController> ().activeKuniId;
				}
				string temp = "jyosyu" + kuniId;
				PlayerPrefs.SetInt (temp, int.Parse (busyoId));

				//Jyosyu Heiryoku
				string temp2 = "jyosyuHei" + busyoId;
				if (Application.loadedLevelName == "naisei") {
					jyosyuHei = int.Parse (GameObject.Find ("AshigaruValue").GetComponent<Text> ().text);
				}
				PlayerPrefs.SetInt (temp2, jyosyuHei);

				//Track
				int TrackJyosyuNinmeiNo = PlayerPrefs.GetInt("TrackJyosyuNinmeiNo",0);
				TrackJyosyuNinmeiNo = TrackJyosyuNinmeiNo + 1;
				PlayerPrefs.SetInt("TrackJyosyuNinmeiNo",TrackJyosyuNinmeiNo);


				//Jyosyu Busyo for KuniId
				string temp3 = "jyosyuBusyo" + busyoId;
				PlayerPrefs.SetInt (temp3, kuniId);
				PlayerPrefs.SetBool ("questSpecialFlg9",true);
				PlayerPrefs.Flush ();


                //Delete Box
                Destroy(GameObject.Find ("board(Clone)"));
				Destroy (GameObject.Find ("Back(Clone)"));

				//Message
				MessageBusyo msg = new MessageBusyo ();
                string text = msgLine.getMessage(88);
                string type = "ninmei";
				msg.makeMessage (text, int.Parse (busyoId), type);

				//Initialization
				if (Application.loadedLevelName == "naisei") {
					NaiseiController naisei = new NaiseiController ();
					naisei.Start ();
				}else if(Application.loadedLevelName == "busyo"){
					SyoguScene syogu = new SyoguScene();
					syogu.createSyoguView(busyoId.ToString());
				}
			}
		}
	}	
}
