using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DoKainin : MonoBehaviour {

	public int kuniId = 0;

	public void OnClick () {
		
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (name == "YesButton") {
			audioSources [4].Play ();

			//Kainin
			//Delete Key
			if (Application.loadedLevelName == "naisei") {
				kuniId = GameObject.Find ("NaiseiController").GetComponent<NaiseiController> ().activeKuniId;
			}

			string temp = "jyosyu" + kuniId;
			int busyoId = PlayerPrefs.GetInt (temp);
			PlayerPrefs.DeleteKey(temp);

			//JyosyuHei Kainin
			string temp2 = "jyosyuHei" + busyoId;
            int jyosyuHei = PlayerPrefs.GetInt(temp2);
			PlayerPrefs.DeleteKey(temp2);

			string temp3 = "jyosyuBusyo" + busyoId;
			PlayerPrefs.DeleteKey(temp3);

			PlayerPrefs.Flush ();

			//Close
			GameObject.Find ("TouchBack").GetComponent<CloseBoard>().onClick();

			//Initialization
			//Message
			Message msg = new Message(); 
			msg.makeMessage(msg.getMessage(87));

            Jinkei Jinkei = new Jinkei();
            Jinkei.jinkeiHpUpda(false, jyosyuHei);

            //Initialization
            if (Application.loadedLevelName == "naisei") {
				NaiseiController naisei = new NaiseiController ();
				naisei.Start ();
			}else if(Application.loadedLevelName == "busyo"){
				SyoguScene syogu = new SyoguScene();
				syogu.createSyoguView(busyoId.ToString());
			}

		}else if(name == "NoButton"){
			//Close
			audioSources [1].Play ();
			GameObject.Find ("TouchBack").GetComponent<CloseBoard>().onClick();
		}
	}	
}
