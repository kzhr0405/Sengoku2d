using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class StartNaisei : MonoBehaviour {

	public int activeKuniId;
	public string activeKuniName;
	public bool clearedFlg = false;


	public void OnClick () {

		if (clearedFlg == true) {
			AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
			audioSources [0].Play ();

			//Stop BGM
			GameObject.Find ("BGMController").GetComponent<DontDestroySoundOnLoad> ().DestoryFlg = true;


			PlayerPrefs.SetInt ("activeKuniId", activeKuniId);
			PlayerPrefs.SetString ("activeKuniName", activeKuniName);

			PlayerPrefs.Flush ();
            if (Application.loadedLevelName == "tutorialMain") {
                Application.LoadLevel("tutorialNaisei");
            }else { 
                Application.LoadLevel ("naisei");
            }
        } else {
			Message msg = new Message(); 
			string OKtext = "";
            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                OKtext = "You can develop after conquered this country.";
            }else {
                OKtext = "城を一つも陥とさない内は\n内政は出来ませぬぞ。";
            }
            msg.makeUpperMessageOnBoard(OKtext);

		}
	}	
}
