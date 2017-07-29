using UnityEngine;
using UnityEngine.Advertisements;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;   

public class UnityAdsExample : MonoBehaviour {

    //void Awake() {
    //    if(Advertisement.isSupported) {
    //Advertisement.Initialize("1212428");
    //}
    //}

    public void ShowAd() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();

        if (Application.internetReachability == NetworkReachability.NotReachable) {
            //接続されていないときの処理
            audioSources[4].Play();
            Message msg = new Message();
            msg.makeMessage(msg.getMessage(5));
        }else {
            audioSources[0].Play();

            GameObject.Find("GameController").GetComponent<MainStageController>().adRunFlg = true;

            if (Advertisement.IsReady("rewardedVideo")) {
                Advertisement.Show("rewardedVideo", new ShowOptions {
                    resultCallback = result => {
                        if (result == ShowResult.Finished) {
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
                        
                            Message msg = new Message();
                            string text = "";
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

                        }else if(result == ShowResult.Skipped){
                            Debug.Log("The ad was skipped before reaching the end.");
                        }else if (result == ShowResult.Failed) {
                            Debug.LogError("The ad failed to be shown.");
                        }
                    }
                });
            }else {
                string text = "";
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    text = "There is no available video now. Please try it later.";
                }
                else {
                    text = "再生可能な動画広告がありません。時間を置いて試してくだされ。";
                }
                Message msg = new Message();
                msg.makeMessageOnBoard(text);
            }
        }
    }
}
