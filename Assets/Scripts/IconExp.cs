using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class IconExp : MonoBehaviour {

    public int IconId = 0;
    public bool serihuFlg = false;

	public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[0].Play();

        GameObject canvas = null;

        if (Application.loadedLevelName == "busyo" || Application.loadedLevelName == "mainStage" || Application.loadedLevelName == "touyou" || Application.loadedLevelName == "zukan" || Application.loadedLevelName == "tutorialTouyou" || Application.loadedLevelName == "hyojyo" || Application.loadedLevelName == "tutorialHyojyo" || Application.loadedLevelName == "pvp" || Application.loadedLevelName == "touyouEvent") {
            canvas = GameObject.Find("Panel").gameObject;
        }

        string text = "";
        if(!serihuFlg) {
            Entity_icon_exp_mst iconExpMst = Resources.Load("Data/icon_exp_mst") as Entity_icon_exp_mst;
            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                text = iconExpMst.param[IconId - 1].expEng;
            } else if(langId ==3){
                text = iconExpMst.param[IconId - 1].expSChn;
            } else {
                text = iconExpMst.param[IconId - 1].exp;
            }
                
        }else {
            Entity_serihu_mst serihuMst = Resources.Load("Data/serihu_mst") as Entity_serihu_mst;
            string busyoId = GameObject.Find("GameScene").GetComponent<NowOnBusyo>().OnBusyo;
            int rdmId = UnityEngine.Random.Range(0, 2);
            if(rdmId ==0) {
                int langId = PlayerPrefs.GetInt("langId");
                if (langId == 2) {
                    text = serihuMst.param[int.Parse(busyoId) - 1].touyouMsgEng;
                }else {
                    text = serihuMst.param[int.Parse(busyoId) - 1].touyouMsg;
                }
            }else {
                int langId = PlayerPrefs.GetInt("langId");
                if (langId == 2) {
                    text = serihuMst.param[int.Parse(busyoId) - 1].senpouMsgEng;
                }else {
                    text = serihuMst.param[int.Parse(busyoId) - 1].senpouMsg;
                }
            }

        }

        Message msg = new Message();
        msg.makeIconExpMessage(text, canvas);

    }
}
