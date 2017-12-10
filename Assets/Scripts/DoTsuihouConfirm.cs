using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class DoTsuihouConfirm : MonoBehaviour {

	// Use this for initialization
	public void OnClick () {
		
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		string busyoName = GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyoName;
		string busyoId = GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyo;
        Daimyo Daimyo = new Daimyo();
        int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
        int senarioId = PlayerPrefs.GetInt("senarioId");
        int daimyoBusyoId = Daimyo.getDaimyoBusyoId(myDaimyo, senarioId);

        int langId = PlayerPrefs.GetInt("langId");

        if (busyoId == daimyoBusyoId.ToString ()) {
			audioSources [4].Play ();
			Message msgNoBtn = new Message();
            string text = "";                        
            text = msgNoBtn.getMessage(231,langId);
            
			msgNoBtn.makeMessage(text);

		} else {
            RonkouScene ronko = new RonkouScene();
            bool jinkeiBusyoFlg = ronko.jinkeiBusyoCheck(int.Parse(busyoId));
            if (jinkeiBusyoFlg) {
                //Error
                audioSources[4].Play();
                Message errorMsg = new Message();
                errorMsg.makeMessage(errorMsg.getMessage(138,langId));

            }else {
                
                audioSources [0].Play ();

			    //Common Process
			    //Back Cover
			    string backPath = "Prefabs/Busyo/back";
			    GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
			    back.transform.SetParent(GameObject.Find ("Panel").transform);
			    back.transform.localScale = new Vector2 (1, 1);
			    RectTransform backTransform = back.GetComponent<RectTransform> ();
			    backTransform.anchoredPosition3D = new Vector3 (0, 0, 0);

			    //Message Box
			    string msgPath = "Prefabs/Busyo/TsuihouConfirm";
			    GameObject msg = Instantiate (Resources.Load (msgPath)) as GameObject;
			    msg.transform.SetParent(GameObject.Find ("Panel").transform);
			    msg.transform.localScale = new Vector2 (1, 1);
			    RectTransform msgTransform = msg.GetComponent<RectTransform> ();
			    msgTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
			    msgTransform.name = "TsuihouConfirm";

			    GameObject msgObj = GameObject.Find ("TsuihouText");
			    string msgText = msgObj.GetComponent<Text> ().text;


                //Message Text Mod
                if (langId == 2) {
                    msgText = "My lord, do you want to banish "+busyoName+"?";
                }else if(langId==3) {
                    msgText = "主公，真的要流放" + busyoName + "吗？";
                }
                else {
                    msgText = "御屋形様、誠に"+busyoName+"を追放なさるのですか？";
                }
                msgObj.GetComponent<Text> ().text = msgText;

			    //Add busyoId
			    GameObject.Find ("YesButton").GetComponent<DoTsuihou> ().busyoId = int.Parse (busyoId);
			    GameObject.Find ("YesButton").GetComponent<DoTsuihou> ().busyoName = busyoName;
			    GameObject.Find ("NoButton").GetComponent<DoTsuihou> ().busyoId = int.Parse (busyoId);
            }
		}
	}

    
}
