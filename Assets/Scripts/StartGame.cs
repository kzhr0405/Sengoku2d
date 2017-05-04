using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class StartGame : MonoBehaviour {

    public Fade fade = null;
    public bool tutorialDoneFlg = false;
    public bool clickedFlg = false;
    public GameObject loading;

    public void Start(){
		Resources.UnloadUnusedAssets ();

        //Data Patch
        DataPatch DataPatch = new DataPatch();
        DataPatch.DataPatch1(); //20170315


        fade = GameObject.Find("FadeCanvas").GetComponent<Fade>();

        /*Sound Controller Start*/
        if (GameObject.Find ("BGMController") == null) {			
			string bgmPath = "Prefabs/Common/SoundController/BGMController";
			GameObject bgmObj = Instantiate (Resources.Load (bgmPath)) as GameObject;
			bgmObj.name = "BGMController";
		}

		if (GameObject.Find ("SEController") == null) {		
			string sePath = "Prefabs/Common/SoundController/SEController";
			GameObject seObj = Instantiate (Resources.Load (sePath)) as GameObject;
			seObj.name = "SEController";

		}
		BGMSESwitch bgm = new BGMSESwitch ();
		bgm.StopSEVolume ();
		bgm.StopBGMVolume ();
        /*Sound Controller End*/

        //Data Loard Start        
        tutorialDoneFlg = PlayerPrefs.GetBool("tutorialDoneFlg");

    }

	public void OnClick(){

        loading.SetActive(true);
        GetComponent<Button>().enabled = false;

		//SE
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [5].Play ();
        clickedFlg = true;

        if (Application.internetReachability == NetworkReachability.NotReachable) {
            Debug.Log("No Network");
            //接続されていないときの処理                     
            fade.FadeIn(2, () => {
                if (!tutorialDoneFlg) {
                    SceneManager.LoadScene("tutorialMain");
                }else {
                    SceneManager.LoadScene("mainStage");
                }
            });           
        }
    }
    

    void Update() {
        if(clickedFlg) {
            clickedFlg = false;
            fade.FadeIn(2, () => {
                if (!tutorialDoneFlg) {
                    SceneManager.LoadScene("tutorialMain");
                }else {
                    SceneManager.LoadScene("mainStage");
                }                
            });
            
        }
    }
}
