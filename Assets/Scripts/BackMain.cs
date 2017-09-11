using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BackMain : MonoBehaviour {

	public GameObject Required1;
	public GameObject Required2;
	public GameObject Circle;
	public GameObject CyouteiSelectScrollView;

    public bool spdUpFlg = false;
    public string fromSceneName = "";

    public void Awake(){

        fromSceneName = PlayerPrefs.GetString("fromSceneName");

        if (SceneManager.GetActiveScene ().name == "shisya") {
			Required1 = GameObject.Find ("Required1").gameObject;
			Required2 = GameObject.Find ("Required2").gameObject;
			Circle = GameObject.Find ("Circle").gameObject;
			CyouteiSelectScrollView = GameObject.Find ("CyouteiSelectScrollView").gameObject;
		}
	}

	public void OnClick () {

        
        //Back to Main
        AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
        
		//back from naisei to main 
		if (SceneManager.GetActiveScene ().name == "naisei") {
            if(!spdUpFlg) {
			    PlayerPrefs.SetBool ("fromNaiseiFlg", true);
                PlayerPrefs.Flush();
                audioSources [1].Play ();
			    Application.LoadLevel("mainStage");
            }else {
                string backPath = "Prefabs/Common/TouchBack";
                GameObject back = Instantiate(Resources.Load(backPath)) as GameObject;
                back.transform.SetParent(GameObject.Find("Panel").transform);
                back.transform.localScale = new Vector2(1, 1);
                RectTransform backTransform = back.GetComponent<RectTransform>();
                backTransform.anchoredPosition3D = new Vector3(0, 0, 0);
                back.name = "TouchBack";

                //Message Box
                string msgPath = "Prefabs/Naisei/LeaveSpdUpConfirm";
                GameObject msg = Instantiate(Resources.Load(msgPath)) as GameObject;
                msg.transform.SetParent(back.transform);
                msg.transform.localScale = new Vector2(1, 1);
                RectTransform msgTransform = msg.GetComponent<RectTransform>();
                msgTransform.anchoredPosition3D = new Vector3(0, 0, 0);
                msgTransform.name = "LeaveSpdUpConfirm";

            }
        } else if (SceneManager.GetActiveScene ().name == "preKassen"|| SceneManager.GetActiveScene().name == "preKaisen") {
			//Tettai
			audioSources [4].Play ();

			int TrackTettaiNo = PlayerPrefs.GetInt ("TrackTettaiNo", 0);
			TrackTettaiNo = TrackTettaiNo + 1;
			PlayerPrefs.SetInt ("TrackTettaiNo", TrackTettaiNo);

			int kuniId = PlayerPrefs.GetInt ("activeKuniId");
			int stageId = PlayerPrefs.GetInt ("activeStageId");

			string clearedStage = "kuni" + kuniId;
			string clearedStageString = PlayerPrefs.GetString (clearedStage);
			List<string> clearedStageList = new List<string> ();
			char[] delimiterChars = { ',' };
			if (clearedStageString != null && clearedStageString != "") {
				clearedStageList = new List<string> (clearedStageString.Split (delimiterChars));
			}
			if (!clearedStageList.Contains (stageId.ToString ())) {
				PlayerPrefs.SetString ("kassenWinLoseFlee", stageId.ToString () + ",1");
			}
			bool isAttackedFlg = PlayerPrefs.GetBool ("isAttackedFlg");
			bool isKessenFlg = PlayerPrefs.GetBool ("isKessenFlg");
			if (!isAttackedFlg && !isKessenFlg) {
				PlayerPrefs.SetBool ("fromKassenFlg", true);
			}
			//Reduce Hyourou
			int nowHyourou = PlayerPrefs.GetInt ("hyourou");
			int newHyourou = 0;
			if (isKessenFlg) {
				int kessenHyourou = PlayerPrefs.GetInt ("kessenHyourou");
				int half = kessenHyourou / 2;
				newHyourou = nowHyourou - half;
			} else {
				newHyourou = nowHyourou - 2;
			}
			PlayerPrefs.SetInt ("hyourou", newHyourou);
			Application.LoadLevel("mainStage");
		
		} else if(SceneManager.GetActiveScene ().name == "shisya"){

            audioSources[1].Play();

            //check
            if (GameObject.Find("Shisya").transform.FindChild("Panel").transform.FindChild("ScrollView").transform.FindChild("Content").transform.childCount > 0) {
                string backPath = "Prefabs/Common/TouchBack";
                GameObject back = Instantiate(Resources.Load(backPath)) as GameObject;
                back.transform.SetParent(GameObject.Find("Panel").transform);
                back.transform.localScale = new Vector2(1, 1);
                RectTransform backTransform = back.GetComponent<RectTransform>();
                backTransform.anchoredPosition3D = new Vector3(0, 0, 0);
                back.name = "TouchBack";

                //Message Box
                string msgPath = "Prefabs/Shisya/ShisyaBackConfirm";
                GameObject msg = Instantiate(Resources.Load(msgPath)) as GameObject;
                msg.transform.SetParent(back.transform);
                msg.transform.localScale = new Vector2(1, 1);
                RectTransform msgTransform = msg.GetComponent<RectTransform>();
                msgTransform.anchoredPosition3D = new Vector3(0, 0, 0);
                msg.name = "ShisyaBackConfirm";
                msg.transform.FindChild("NoButton").GetComponent<DoShisyaBack>().back = back;
            }else {
                PlayerPrefs.SetBool("fromShisyaFlg", true);
                PlayerPrefs.Flush();
                Application.LoadLevel("mainStage");
            }
            
		}else if (Application.loadedLevelName == "tutorialNaisei") {
            audioSources[1].Play();
            Application.LoadLevel("tutorialMain");

        }else if (Application.loadedLevelName == "tutorialTouyou") {
            audioSources[1].Play();
            Application.LoadLevel("tutorialMain");

        }else if (Application.loadedLevelName == "tutorialBusyo") {
            audioSources[1].Play();
            Application.LoadLevel("tutorialMain");

        }else if (Application.loadedLevelName == "pvp") {
            audioSources[1].Play();
            Destroy(GameObject.Find("PvPDataStore"));
            Application.LoadLevel("mainStage");
        }else if (Application.loadedLevelName == "dataRecovery") {
            audioSources[1].Play();
            Destroy(GameObject.Find("PvPDataStore"));
            Destroy(GameObject.Find("DataStore"));            
            Application.LoadLevel("top");
        }else {
            if(fromSceneName == "" || fromSceneName == null) {
                audioSources[1].Play();
			    Application.LoadLevel("mainStage");
            }else {
                PlayerPrefs.DeleteKey("fromSceneName");
                PlayerPrefs.Flush();
                audioSources[1].Play();
                Application.LoadLevel(fromSceneName);
            }
        }
		PlayerPrefs.Flush ();
	}



}
