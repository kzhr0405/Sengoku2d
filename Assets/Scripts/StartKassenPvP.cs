using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class StartKassenPvP : MonoBehaviour {

    public int pvpStageId;
    public string userId;
    public bool clickedFlg = false;
    public bool sceneChangeFlg = false;
    public PvPDataStore PvPDataStore;
    public bool isJinkeiMapFetched;
    public bool isBusyoStatusFetched;

    private void Awake() {
        //Enemy Jinkei Load
        PvPDataStore = GameObject.Find("DataStore").GetComponent<PvPDataStore>();
    }


    public void OnClick() {
        PlayerPrefs.SetInt("pvpStageId", pvpStageId);
        PlayerPrefs.Flush();        
        clickedFlg = true;

    }

    void Update() {
        
        //get jinkei
        if(userId != "" && !isJinkeiMapFetched) {
            PvPDataStore.GetEnemyJinkei(userId, pvpStageId);
            isJinkeiMapFetched = true;
        }

        //get busyo data
        if (pvpStageId == 1) {            
            if(PvPDataStore.PvP1BusyoList != null && !isBusyoStatusFetched) {
                isBusyoStatusFetched = true;
            }

        }else if (pvpStageId == 2 && userId != "") {
            if (PvPDataStore.PvP2BusyoList != null && !isBusyoStatusFetched) {
                isBusyoStatusFetched = true;
            }
        }else if (pvpStageId == 3 && userId != "") {
            if (PvPDataStore.PvP3BusyoList != null && !isBusyoStatusFetched) {
                isBusyoStatusFetched = true;
            }
        }

        //register temp lose tran
        if(userId != "" && isJinkeiMapFetched && isBusyoStatusFetched && clickedFlg && !PvPDataStore.PvPAtkNoFlg) {
            PvPDataStore.UpdatePvPAtkNo(GameObject.Find("GameScene").GetComponent<PvPController>().myUserId);
            GameObject.Find("DataStore").GetComponent<PvPDataStore>().enemyUserId = userId;
        }

        //scene change
        if (userId != "" && isJinkeiMapFetched && isBusyoStatusFetched && clickedFlg && PvPDataStore.PvPAtkNoFlg && !sceneChangeFlg) {
            sceneChangeFlg = true;
            Application.LoadLevel("pvpKassen");
        }

    }


}
