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
    public bool sceneChangedFlg = false;

    public GameObject loading;

    //Data Register
    DataUserId DataUserId = new DataUserId();
    DataJinkei DataJinkei = new DataJinkei();


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
        DataUserId = GameObject.Find("DataStore").GetComponent<DataUserId>();
        DataJinkei = GameObject.Find("DataStore").GetComponent<DataJinkei>();

        //User Id
        if (!PlayerPrefs.HasKey("userId")) {
            string randomA = StringUtils.GeneratePassword(10);
            System.DateTime now = System.DateTime.Now;
            string randomB = now.ToString("yyyyMMddHHmmss");
            string userId = randomA + randomB;
            PlayerPrefs.SetString("userId", userId);
            PlayerPrefs.Flush();
            DataUserId.InsertUserId(userId);
        }else {
            string userId = PlayerPrefs.GetString("userId");
            DataUserId.UpdateUserId(userId);
            DataJinkei.UpdateJinkei(userId);
        }
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
                SceneManager.LoadScene("mainStage");
            });
        }
    }
    

    void Update() {

        if (!tutorialDoneFlg) {
            if(clickedFlg && DataUserId.RegisteredFlg) {
                if (!sceneChangedFlg) {
                    sceneChangedFlg = true;
                    fade.FadeIn(2, () => {
                        SceneManager.LoadScene("tutorialMain");
                    });
                }
            }
        }else {            
            if (clickedFlg) {
                if (DataUserId.RegisteredFlg && DataJinkei.RegisteredFlg) {
                    if (!sceneChangedFlg) {
                        sceneChangedFlg = true;
                        fade.FadeIn(2, () => {                        
                            SceneManager.LoadScene("mainStage");                        
                        });
                    }
                }             
            }
        }
    }
    



}
