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
    public bool updateLosePtFlg;
    public bool updateWinPtFlg;
    public PvPController PvPController;
    public int nowHyourou = 0;
    public int getPt = 0;

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
            //msg.makeMessage(msg.getMessage(7));
            msg.hyourouMovieMessage();
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
            PvPController = GameObject.Find("GameScene").GetComponent<PvPController>();
            PvPDataStore.UpdatePvPAtkNo(PvPController.myUserId, int.Parse(PvPController.todayNCMB));
            PvPDataStore.enemyUserId = userId;
            PvPDataStore.enemyUserName = enemyUserName;
            PvPDataStore.myUserName = PvPController.myUserName;
            PvPDataStore.getPt = getPt;
            PvPDataStore.todayNCMB = int.Parse(PvPDataStore.PvPTimer.todayNCMB);
        }

        //register temp lose tran
        if (userId != "" && isJinkeiMapFetched && isBusyoStatusFetched && clickedFlg && PvPDataStore.PvPAtkNoFlg && updatePvPAtkFlg && !PvPDataStore.donePlusUpdatePtFlg  && !updateLosePtFlg) {
            updateLosePtFlg = true;
            PvPDataStore.UpdatePvPPt(userId, true, getPt);
        }

        if (userId != "" && isJinkeiMapFetched && isBusyoStatusFetched && clickedFlg && PvPDataStore.PvPAtkNoFlg && updatePvPAtkFlg && PvPDataStore.donePlusUpdatePtFlg && updateLosePtFlg && !PvPDataStore.doneMinusUpdatePtFlg && !updateWinPtFlg) {
            updateWinPtFlg = true;
            PvPDataStore.UpdatePvPPt(PvPController.myUserId, false, getPt);
        }



        //scene change
        if (userId != "" && isJinkeiMapFetched && isBusyoStatusFetched && clickedFlg && PvPDataStore.PvPAtkNoFlg && updatePvPAtkFlg && PvPDataStore.donePlusUpdatePtFlg && PvPDataStore.doneMinusUpdatePtFlg && updateLosePtFlg && updateWinPtFlg && !sceneChangeFlg) {

            //hyourou
            int newHyourou = nowHyourou - 5;
            PlayerPrefs.SetInt("hyourou", newHyourou);
            PlayerPrefs.SetBool("pvpFlg", true);

            //money and exp calculation
            int minExp = getPvPMinExp(PvPController.per);
            int maxExp = getPvPMaxExp(PvPController.per);
            int exp = UnityEngine.Random.Range(minExp, maxExp + 1);

            int minMoney = minExp * 2;
            int maxMoney = maxExp * 3;
            int money = UnityEngine.Random.Range(minMoney, maxMoney + 1);

            string itemGrp = getPvPRandomItemGrp();
            string itemTyp = "";
            int itemId = 0;
            int itemQty = 1;

            AttackNaiseiView AttackNaiseiView = new AttackNaiseiView();
            if (itemGrp == "item") {
                itemTyp = AttackNaiseiView.getRandomItemTyp(itemGrp);
                if (itemTyp == "tech") {
                    itemId = AttackNaiseiView.getItemRank(66, 33);
                }else if (itemTyp == "Tama") {
                    itemId = AttackNaiseiView.getItemRank(20, 5);
                    if (itemId == 3) {
                        itemQty = 100;
                    }else if (itemId == 2) {
                        itemQty = 50;
                    }else if (itemId == 1) {
                        itemQty = 10;
                    }
                }else {
                    itemId = AttackNaiseiView.getItemRank(20, 5);
                }

            }else if (itemGrp == "kahou") {
                itemTyp = AttackNaiseiView.getRandomItemTyp(itemGrp);
                Kahou kahou = new Kahou();
                string kahouRank = AttackNaiseiView.getKahouRank();
                itemId = kahou.getRamdomKahouId(itemTyp, kahouRank);
            }
            PlayerPrefs.SetInt("activeStageMoney", money);
            PlayerPrefs.SetInt("activeStageExp", exp);
            PlayerPrefs.SetString("activeItemGrp", itemGrp);
            PlayerPrefs.SetString("activeItemType", itemTyp);
            PlayerPrefs.SetInt("activeItemId", itemId);
            PlayerPrefs.SetInt("activeItemQty", itemQty);
            
            PlayerPrefs.Flush();

            sceneChangeFlg = true;
            Application.LoadLevel("kassen");
        }

    }

    public int getPvPMinExp(float per) {
        int minExp = 0;
        List<int> baseList = new List<int>() { 200, 300, 400, 500};
        int rdmId = UnityEngine.Random.Range(0, baseList.Count);
        int baseExp = baseList[rdmId];

        //senryoku ratio
        int calcExp = Mathf.CeilToInt((float)baseExp * per);

        //-50%
        minExp = calcExp / 2;

        return minExp;
    }

    public int getPvPMaxExp(float per) {
        int maxExp = 0;
        List<int> baseList = new List<int>() { 200, 300, 400, 500};
        int rdmId = UnityEngine.Random.Range(0, baseList.Count);
        int baseExp = baseList[rdmId];

        //senryoku ratio
        int calcExp = Mathf.CeilToInt((float)baseExp * per);

        //+50%
        maxExp = calcExp + Mathf.CeilToInt((float)calcExp / 2);

        return maxExp;
    }

    public string getPvPRandomItemGrp() {
        string itemGrp = "no"; //no or item or kahou

        float percent = UnityEngine.Random.value;
        percent = percent * 100;

        if (percent <= 10) {
            //kahou
            itemGrp = "kahou";
        }
        else if (10 < percent && percent <= 75) {
            //item
            itemGrp = "item";
        }

        return itemGrp;
    }
}
