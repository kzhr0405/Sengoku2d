using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class StartScene : MonoBehaviour {

	//Sound
	public AudioSource sound;

	public void OnClick(){

		//SE
		sound = GameObject.Find ("SEController").GetComponent<AudioSource> ();
		sound.PlayOneShot(sound.clip);

        bool tutorialDoneFlg = PlayerPrefs.GetBool("tutorialDoneFlg");
        //Under Button
        if (name == "Jinkei") {
            if (tutorialDoneFlg&& Application.loadedLevelName != "tutorialMain") {
                Application.LoadLevel("hyojyo");
            }else {
                Application.LoadLevel("tutorialHyojyo");
            }
        } else if (name == "PvP") {
            
            /*
            Message msg = new Message();
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                msg.makeMessage("PvP is Under Preparation");
            }else {
                msg.makeMessage("PvP準備中");
            }
            */
            
            
            if (Application.internetReachability == NetworkReachability.NotReachable) {
                //接続されていないときの処理
                Message msg = new Message();
                msg.makeMessage(msg.getMessage(5));
            }else {
                string PvPName = PlayerPrefs.GetString("PvPName");
                
                if (PvPName == "" || PvPName == null) {
                    //first time
                    GameObject panel = GameObject.Find("Panel").gameObject;
                    string pathOfBack = "Prefabs/Common/TouchBackForOne";
                    GameObject back = Instantiate(Resources.Load(pathOfBack)) as GameObject;
                    back.transform.SetParent(panel.transform);
                    back.transform.localScale = new Vector2(1, 1);
                    back.transform.localPosition = new Vector2(0, 0);
                    string pathOfBoard = "Prefabs/PvP/FirstPvPBoard";
                    GameObject board = Instantiate(Resources.Load(pathOfBoard)) as GameObject;
                    board.transform.SetParent(panel.transform);
                    board.transform.localScale = new Vector2(1, 1);
                    board.transform.localPosition = new Vector2(0, 0);

                    back.GetComponent<CloseOneBoard>().deleteObj = board;
                    board.transform.FindChild("NoButton").GetComponent<AddHyourou>().touchBackObj = back;
                    
                }else {
                    if(GameObject.Find("DataStore")) {
                        DataPvP pvpScript = GameObject.Find("DataStore").GetComponent<DataPvP>();
                        pvpScript.UpdatePvP();
                    }
                    Application.LoadLevel("pvp");
                }

            }
            
            
        } else if (name == "Busyo") {			
            if (tutorialDoneFlg && Application.loadedLevelName != "tutorialMain") {
                Application.LoadLevel("busyo");
            }else {
                Application.LoadLevel("tutorialBusyo");
            }
        } else if (name == "Touyou") {
            if(tutorialDoneFlg && Application.loadedLevelName != "tutorialMain") {
			    Application.LoadLevel ("touyou");
            }else {
                Application.LoadLevel("tutorialTouyou");
            }
        } else if (name == "Houmotsuko") {
			Application.LoadLevel ("souko");
		} else if (name == "Zukan") {
			Application.LoadLevel ("zukan");
		} else if (name == "Syounin") {

			if (Application.internetReachability == NetworkReachability.NotReachable) {
				//接続されていないときの処理
				Message msg = new Message ();
				msg.makeMessage (msg.getMessage(136));
			} else {
                //接続されているときの処理
                Application.LoadLevel("purchase");

                /*
                //Make Purchase Pop
                GameObject.Find("GameController").GetComponent<MainStageController>().iapRunFlg = false;

                //Popup
                GameObject map = GameObject.Find ("Map").gameObject;

				string backPath = "Prefabs/Busyo/back";
				GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
				back.transform.SetParent (map.transform);
				back.transform.localScale = new Vector2 (1, 1);
				RectTransform backTransform = back.GetComponent<RectTransform> ();
				backTransform.anchoredPosition = new Vector3 (0, 0, 0);

				//Popup Screen
				string popupPath = "Prefabs/Purchase/PurchaseManager";
				GameObject popup = Instantiate (Resources.Load (popupPath)) as GameObject;
				popup.transform.SetParent (map.transform);
				popup.transform.localScale = new Vector2 (1, 1);
				RectTransform popupTransform = popup.GetComponent<RectTransform> ();
				popupTransform.anchoredPosition = new Vector3 (0, 0, 0);
				popup.name = "PurchaseManager";
                */
			}
		} else if (name == "Config") {
			GameObject panel = GameObject.Find ("Panel").gameObject;

			string pathOfBack = "Prefabs/Common/TouchBackForOne";
			GameObject back = Instantiate (Resources.Load (pathOfBack)) as GameObject;
			back.transform.SetParent (panel.transform);
			back.transform.localScale = new Vector2 (1, 1);
			back.transform.localPosition = new Vector2 (0, 0);

			string pathOfBoard = "Prefabs/Common/ConfigBoard";
			GameObject board = Instantiate (Resources.Load (pathOfBoard)) as GameObject;
			board.transform.SetParent (panel.transform);
			board.transform.localScale = new Vector2 (1, 1);
			board.transform.localPosition = new Vector2 (0, 0);

			//qa
			board.transform.FindChild("qa").GetComponent<QA> ().qaId = 15;
			back.GetComponent<CloseOneBoard> ().deleteObj = board;


			//On or Off
			Color onBtnColor = new Color (85f / 255f, 85f / 255f, 85f / 255f, 160f / 255f);
			Color onTxtColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 40f / 255f);

			bool SEOffFlg = PlayerPrefs.GetBool ("SEOffFlg");
			if(SEOffFlg){
				GameObject offObj = board.transform.FindChild ("SE").transform.FindChild ("OffButton").gameObject;
				offObj.GetComponent<Image> ().color = onBtnColor;
				offObj.transform.FindChild("Text").GetComponent<Text> ().color = onTxtColor;
				offObj.GetComponent<Button> ().enabled = false;

			}else{
				GameObject onObj = board.transform.FindChild ("SE").transform.FindChild ("OnButton").gameObject;
				onObj.GetComponent<Image> ().color = onBtnColor;
				onObj.transform.FindChild("Text").GetComponent<Text> ().color = onTxtColor;
				onObj.GetComponent<Button> ().enabled = false;

			}

			bool BGMOffFlg = PlayerPrefs.GetBool ("BGMOffFlg");
			if(BGMOffFlg){
				GameObject offObj = board.transform.FindChild ("BGM").transform.FindChild ("OffButton").gameObject;
				offObj.GetComponent<Image> ().color = onBtnColor;
				offObj.transform.FindChild("Text").GetComponent<Text> ().color = onTxtColor;
				offObj.GetComponent<Button> ().enabled = false;

			}else{
				GameObject onObj = board.transform.FindChild ("BGM").transform.FindChild ("OnButton").gameObject;
				onObj.GetComponent<Image> ().color = onBtnColor;
				onObj.transform.FindChild("Text").GetComponent<Text> ().color = onTxtColor;
				onObj.GetComponent<Button> ().enabled = false;

			}

            //Auto
            bool Auto2Flg = PlayerPrefs.GetBool("Auto2Flg");
            if(Auto2Flg) {
                GameObject onObj = board.transform.FindChild("Auto").transform.FindChild("2").gameObject;
                onObj.GetComponent<Image>().color = onBtnColor;
                onObj.transform.FindChild("Text").GetComponent<Text>().color = onTxtColor;
                onObj.GetComponent<Button>().enabled = false;
            }else {
                GameObject onObj = board.transform.FindChild("Auto").transform.FindChild("4").gameObject;
                onObj.GetComponent<Image>().color = onBtnColor;
                onObj.transform.FindChild("Text").GetComponent<Text>().color = onTxtColor;
                onObj.GetComponent<Button>().enabled = false;
            }            
        }
        else if (name == "Reward") {
            if (Application.internetReachability == NetworkReachability.NotReachable) {
                //接続されていないときの処理
                Message msg = new Message();
                msg.makeMessage(msg.getMessage(5));
            }else {
                Application.LoadLevel("reward");
            }
        }else if(name == "Tutorial") {
            PlayerPrefs.SetInt("tutorialId",1);
            PlayerPrefs.Flush();
            Application.LoadLevel("tutorialMain");

        }else if (name == "User") {
            GameObject panel = GameObject.Find("Panel").gameObject;

            string pathOfBack = "Prefabs/Common/TouchBackForOne";
            GameObject back = Instantiate(Resources.Load(pathOfBack)) as GameObject;
            back.transform.SetParent(panel.transform);
            back.transform.localScale = new Vector2(1, 1);
            back.transform.localPosition = new Vector2(0, 0);

            string pathOfBoard = "Prefabs/Common/UserId";
            GameObject board = Instantiate(Resources.Load(pathOfBoard)) as GameObject;
            board.transform.SetParent(panel.transform);
            board.transform.localScale = new Vector2(1, 1);
            board.transform.localPosition = new Vector2(0, 0);

            string userId = PlayerPrefs.GetString("userId");
            board.transform.FindChild("userId").GetComponent<Text>().text = userId;

            string versionNo = Application.version;
            string verTxt = "( Ver." + versionNo + ")";
            string final = SystemInfo.operatingSystem + verTxt;
            board.transform.FindChild("ver").GetComponent<Text>().text = final;
            back.GetComponent<CloseOneBoard>().deleteObj = board;

        }
        else if (name == "DataRecovery") {
            Application.LoadLevel("dataRecovery");
        }
    }
}
