using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class StartKassenPvP : MonoBehaviour {

    public int pvpStageId;
    public string userId;
    public string enemyUserName;
    public bool clickedFlg = false;
    public bool sceneChangeFlg = false;
    public PvPDataStore PvPDataStore;
    public bool isJinkeiMapFetched;
    public bool isBusyoStatusFetched;
    public bool updatePvPAtkFlg;

    public int nowHyourou = 0;

    private void Awake() {
        //Enemy Jinkei Load
        PvPDataStore = GameObject.Find("PvPDataStore").GetComponent<PvPDataStore>();
    }


    public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        Message msg = new Message();

        nowHyourou = PlayerPrefs.GetInt("hyourou");

        if (nowHyourou >= 5) {
            audioSources[5].Play();
            PlayerPrefs.SetInt("pvpStageId", pvpStageId);
            PlayerPrefs.Flush();        
            clickedFlg = true;
        }else {
            audioSources[4].Play();
            msg.makeMessage(msg.getMessage(7));
        }

    }

    void Update() {
        
        //get jinkei
        if(userId != "" && !isJinkeiMapFetched) {
            PvPDataStore.GetEnemyJinkei(userId, pvpStageId);
            isJinkeiMapFetched = true;
        }

        //get busyo data
        if (pvpStageId == 1) {            
            if(PvPDataStore.PvP1BusyoList != null && PvPDataStore.PvP1BusyoList.Count != 0 && !isBusyoStatusFetched) {
                isBusyoStatusFetched = true;
            }

        }else if (pvpStageId == 2 && userId != "") {
            if (PvPDataStore.PvP2BusyoList != null && PvPDataStore.PvP2BusyoList.Count != 0 && !isBusyoStatusFetched) {
                isBusyoStatusFetched = true;
            }
        }else if (pvpStageId == 3 && userId != "") {
            if (PvPDataStore.PvP3BusyoList != null && PvPDataStore.PvP3BusyoList.Count != 0 && !isBusyoStatusFetched) {
                isBusyoStatusFetched = true;
            }
        }

        //register temp lose tran
        if(userId != "" && isJinkeiMapFetched && isBusyoStatusFetched && clickedFlg && !PvPDataStore.PvPAtkNoFlg && !updatePvPAtkFlg) {
            updatePvPAtkFlg = true;
            PvPController PvPController = GameObject.Find("GameScene").GetComponent<PvPController>();
            PvPDataStore.UpdatePvPAtkNo(PvPController.myUserId);
            GameObject.Find("PvPDataStore").GetComponent<PvPDataStore>().enemyUserId = userId;
            GameObject.Find("PvPDataStore").GetComponent<PvPDataStore>().enemyUserName = enemyUserName;
            GameObject.Find("PvPDataStore").GetComponent<PvPDataStore>().myUserName = PvPController.myUserName;
        }

        //scene change
        if (userId != "" && isJinkeiMapFetched && isBusyoStatusFetched && clickedFlg && PvPDataStore.PvPAtkNoFlg && updatePvPAtkFlg && !sceneChangeFlg) {

            //hyourou
            int newHyourou = nowHyourou - 5;
            PlayerPrefs.SetInt("hyourou", newHyourou);
            PlayerPrefs.SetBool("pvpFlg", true);
            PlayerPrefs.Flush();

            sceneChangeFlg = true;
            Application.LoadLevel("kassen");
        }

    }


}
