using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class AdfuriMoviepop : MonoBehaviour {

    private int test_num;
    private SCENE_STATE sceneState = SCENE_STATE.MAIN;

    bool initialized = false;
    enum SCENE_STATE {
        MAIN,
        QUIT_WAIT,
        QUIT,
        END
    }

    void Update() {
        if (!initialized) {
            initialized = true;
            GameObject ob = GameObject.Find("AdfurikunMovieRewardUtility");
            AdfurikunMovieRewardUtility au = ob.GetComponent<AdfurikunMovieRewardUtility>();
            au.setMovieRewardSrcObject(this.gameObject);
            au.initializeMovieReward();
        }
        switch (this.sceneState) {
        case SCENE_STATE.MAIN:
            break;
        case SCENE_STATE.QUIT_WAIT:
            this.sceneState = SCENE_STATE.QUIT;
            break;
        case SCENE_STATE.QUIT:
            this.sceneState = SCENE_STATE.END;
            break;
        case SCENE_STATE.END:
            break;
        }
    }


    public void PushAdsense() {
        // topに戻るのを伏せぐためのフラグ変更
        GameObject ob = GameObject.Find("GameController");
        MainStageController mc = ob.GetComponent<MainStageController>();
        if (!mc.adRunFlg)
        {
            StartCoroutine(PlayAdsense());
            mc.adRunFlg = true;
        }
    }

    // 準備ができたら広告を再生する
    IEnumerator PlayAdsense()
    {
        GameObject ob = GameObject.Find("AdfurikunMovieRewardUtility");
        AdfurikunMovieRewardUtility au = ob.GetComponent<AdfurikunMovieRewardUtility>();
        while (!au.isPreparedMovieReward())
        {
            yield return null;
        }
        au.playMovieReward();
    }

    void MovieRewardCallback(ArrayList vars) {
        int stateName = (int)vars[0];
        string appID = (string)vars[1];
        string adnetworkKey = (string)vars[2];

        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        Message msg = new Message();
        string text = "";

        GameObject ob = GameObject.Find("GameController");
        MainStageController mc = ob.GetComponent<MainStageController>();

        AdfurikunMovieRewardUtility.ADF_MovieStatus state = (AdfurikunMovieRewardUtility.ADF_MovieStatus)stateName; switch (state) {
        case AdfurikunMovieRewardUtility.ADF_MovieStatus.PrepareSuccess:
                Debug.Log("The ad was preapred.");
                break;
        case AdfurikunMovieRewardUtility.ADF_MovieStatus.StartPlaying:
                Debug.Log("The ad was started.");
                break;
        case AdfurikunMovieRewardUtility.ADF_MovieStatus.AdClose:
                // topに戻るのを伏せぐためのフラグ変更                
                mc.adRunFlg = false;

                int busyoDamaQty = 0;
                string atariMsg = "";
                float rankPercent = UnityEngine.Random.value;
                rankPercent = rankPercent * 100;
                if (rankPercent <= 10) {
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        atariMsg = "My lord, Big Hit! \n";
                    }else {
                        atariMsg = "大当たりですぞ。\n";
                    }
                    busyoDamaQty = UnityEngine.Random.Range(20, 51); //20-50
                }else if (10 < rankPercent && rankPercent <= 40) {
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        atariMsg = "My lord, Mid Hit. \n";
                    }else {
                        atariMsg = "中当たりですぞ。\n";
                    }
                    busyoDamaQty = UnityEngine.Random.Range(10, 21); //10-20
                }else if (40 < rankPercent) {
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        atariMsg = "My lord, Low Hit. \n";
                    }else {
                        atariMsg = "小当たりですぞ。\n";
                    }
                    busyoDamaQty = UnityEngine.Random.Range(5, 11); //5-10
                }
                
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    text = atariMsg + "You got " + busyoDamaQty + " stone.";
                }else {
                    text = atariMsg + "武将珠を" + busyoDamaQty + "個手に入れましたぞ。";
                }
                msg.makeMessageOnBoard(text);

                int busyoDama = PlayerPrefs.GetInt("busyoDama");
                int newBusyoDama = busyoDama + busyoDamaQty;
                PlayerPrefs.SetInt("busyoDama", newBusyoDama);
                PlayerPrefs.Flush();

                GameObject.Find("BusyoDamaValue").GetComponent<Text>().text = newBusyoDama.ToString();
                audioSources[3].Play();
                GameObject.Find("GameController").GetComponent<MainStageController>().adRunFlg = false;

                //縦動画後に横固定に変更用
                //Screen.orientation = ScreenOrientation.Portrait;

                break;
                default:
                return;

            case AdfurikunMovieRewardUtility.ADF_MovieStatus.FailedPlaying:
                mc.adRunFlg = false;

                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    text = "There is no available video now. Please try it later.";
                }else {
                    text = "再生可能な動画広告がありません。時間を置いて試してくだされ。";
                }
                msg.makeMessageOnBoard(text);

                break;
            //case AdfurikunMovieRewardUtility.ADF_MovieStatus.AdClose:
            //        Debug.Log("The ad was closed.");
            //break;
            //default:
            //return;
        }
    }
}