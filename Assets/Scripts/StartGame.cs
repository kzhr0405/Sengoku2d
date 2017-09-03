using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;
using NCMB;

public class StartGame : MonoBehaviour {

    public Fade fade = null;
    public bool tutorialDoneFlg = false;
    public bool clickedFlg = false;
    public GameObject loading;
    public int criteria = 0;

    public void Start(){

		Resources.UnloadUnusedAssets ();
        GetLock();
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
		BGMSESwitch bgm = GetComponent<BGMSESwitch> ();
		bgm.StopSEVolume ();
		bgm.StopBGMVolume ();
        /*Sound Controller End*/

        //Data Loard Start        
        tutorialDoneFlg = PlayerPrefs.GetBool("tutorialDoneFlg");

        string versionNo = Application.version;
        GameObject.Find("Ver").GetComponent<Text>().text = versionNo;

    }

	public void OnClick(){

        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        int busyoDama = PlayerPrefs.GetInt("busyoDama");
        if(busyoDama>criteria) {
            Message msg = new Message();
            msg.makeMessage(msg.getMessage(153));
            audioSources[4].Play();
        }else { 
            loading.SetActive(true);
            GetComponent<Button>().enabled = false;

		    //SE	   
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

    }

    void Update() {
        if (clickedFlg) {
            clickedFlg = false;
            fade.FadeIn(2, () => {
                if (!tutorialDoneFlg) {
                    SceneManager.LoadScene("tutorialMain");
                }
                else {
                    SceneManager.LoadScene("mainStage");
                }
            });

        }
    }

    public void GetLock() {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("lock");
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                foreach (NCMBObject obj in objList) {
                    criteria = System.Convert.ToInt32(obj["criteria"]);
                }
            }
        });
    }

}
