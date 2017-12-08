using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyAttackPop : MonoBehaviour {

    public int enemyDaimyoId = 0;
    public bool myDaimyoBusyoFlg = false;
    int addRatioForMyDaimyoBusyo = 10;

    public void OnClick(){
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[0].Play();
        int langId = PlayerPrefs.GetInt("langId");
        int senarioId = PlayerPrefs.GetInt("senarioId");

        //Common
        string simpaleBattlePath = "";
        if (langId == 2) {
            simpaleBattlePath = "Prefabs/SimpleBattle/BattleBoardEng";
        } else {
            simpaleBattlePath = "Prefabs/SimpleBattle/BattleBoard";
        }
        GameObject boardObj = Instantiate(Resources.Load(simpaleBattlePath)) as GameObject;
        boardObj.transform.SetParent(GameObject.Find("Map").transform);
        boardObj.transform.localScale = new Vector2(45,55);
        boardObj.transform.Find("No").GetComponent<CloseSimpleBattle>().boardObj = boardObj;
        GameObject battleArea = boardObj.transform.Find("Base").transform.Find("BattleArea").gameObject;
        GameObject YesBtn = boardObj.transform.Find("Yes").gameObject;
        GameObject NoBtn = boardObj.transform.Find("No").gameObject;
        GetComponent<Button>().enabled = false;
        NoBtn.GetComponent<CloseSimpleBattle>().katanaBtnObj = gameObject;
        boardObj.name = "BattleBoard";

        //Time Stop
        GameObject timer = transform.parent.gameObject;
        timer.GetComponent<ShiroAttack>().rakujyoFlg = true;
        NoBtn.GetComponent<CloseSimpleBattle>().timer = timer;

        //View Player 
        int jinkei = PlayerPrefs.GetInt("jinkei");
        string soudaisyoLabel = "soudaisyo" + jinkei;
        int soudaisyoId = PlayerPrefs.GetInt(soudaisyoLabel);
        if(soudaisyoId != 0) {
            makeSimplePlayer(soudaisyoId, true, battleArea, 0, YesBtn);
        }
        for (int i=1; i<26; i++) {
            string mapId =  jinkei.ToString() + "map" + i.ToString();
            int busyoId = PlayerPrefs.GetInt(mapId);
            if(busyoId != 0) {
                if(soudaisyoId != busyoId) {
                    makeSimplePlayer(busyoId, false, battleArea, i, YesBtn);
                }
            }
        }
        
        //Same Daimyo Check
        bool sameDaimyoOnFlg = false;
        foreach (GameObject playerObj in GameObject.FindGameObjectsWithTag("Player")) {
            int belongDaimyoId = playerObj.GetComponent<SimpleHP>().belongDaimyoId;
            foreach (GameObject playerObj2 in GameObject.FindGameObjectsWithTag("Player")) {
                if (playerObj2.GetComponent<SimpleHP>().belongDaimyoId == belongDaimyoId) {
                    playerObj.GetComponent<SimpleHP>().numSameDaimyo = playerObj.GetComponent<SimpleHP>().numSameDaimyo + 1;
                    if (playerObj.GetComponent<SimpleHP>().numSameDaimyo == 2) {
                        sameDaimyoOnFlg = true;
                    }
                }
            }
        }

        //Power Up Effection
        if(myDaimyoBusyoFlg) {
            foreach (GameObject busyoObj in GameObject.FindGameObjectsWithTag("Player")) {
                int atk = busyoObj.GetComponent<SimpleAttack>().baseAtk;                
                int dfc = busyoObj.GetComponent<SimpleHP>().baseDfc;
                int addAtk = Mathf.CeilToInt((float)atk * (float)addRatioForMyDaimyoBusyo) / 100;
                int addDfc = Mathf.CeilToInt((float)dfc * (float)addRatioForMyDaimyoBusyo) / 100;
                atk = atk + addAtk;
                dfc = dfc + addDfc;
                busyoObj.GetComponent<SimpleAttack>().atk = Mathf.FloorToInt(atk);
                busyoObj.GetComponent<SimpleHP>().dfc = Mathf.FloorToInt(dfc);
            }
        }
        if (sameDaimyoOnFlg) {
            foreach (GameObject busyoObj in GameObject.FindGameObjectsWithTag("Player")) {
                int totalAtk = busyoObj.GetComponent<SimpleAttack>().baseAtk;
                int totalDfc = busyoObj.GetComponent<SimpleHP>().baseDfc;
                int atk = busyoObj.GetComponent<SimpleAttack>().atk;
                int dfc = busyoObj.GetComponent<SimpleHP>().dfc;
                int numSameDaimyo = busyoObj.GetComponent<SimpleHP>().numSameDaimyo;

                int addRatio = (numSameDaimyo - 1) * 5;
                busyoObj.GetComponent<SimpleAttack>().atk = atk + Mathf.FloorToInt(((float)totalAtk * (float)addRatio) / 100);
                busyoObj.GetComponent<SimpleHP>().dfc = dfc + Mathf.FloorToInt(((float)totalDfc * (float)addRatio) / 100);
            }
        }




        /**View Enemy**/
        //makeSimpleEnemy(enemyBusyoId, battleArea, 0);
        int stageId = timer.GetComponent<ShiroAttack>().toStageId;
        char[] delimiterChars = { ':' };
        List<string> chldBusyoList = new List<string>();
        foreach (GameObject gunzeiObj in GameObject.FindGameObjectsWithTag("StageGunzei")) {
            int gunzeiToStageId = gunzeiObj.GetComponent<TabStageGunzei>().toStageId;

            if (stageId == gunzeiToStageId) {
                int prnt_busyoId = gunzeiObj.GetComponent<TabStageGunzei>().taisyoBusyoId;
                makeSimpleEnemy(prnt_busyoId, battleArea, 0, YesBtn);

                string chld_busyoId = gunzeiObj.GetComponent<TabStageGunzei>().busyoString;
                if(chld_busyoId != "") {                
                    if (chld_busyoId.Contains(":")) {
                        chldBusyoList.AddRange(new List<string>(chld_busyoId.Split(delimiterChars)));
                    }else {
                        chldBusyoList.Add(chld_busyoId);
                    }
                }
            }
        }
        for(int j = 0; j < chldBusyoList.Count; j++) {
            int chldBusyoId = int.Parse(chldBusyoList[j]);
            makeSimpleEnemy(chldBusyoId, battleArea, j, YesBtn);
        }

        //View
        Daimyo daimyScript = new Daimyo();
        int myDaimyoId = PlayerPrefs.GetInt("myDaimyo");
        string myDaimyoName = daimyScript.getName(myDaimyoId,langId, senarioId);
        string enemyDaimyoName = daimyScript.getName(enemyDaimyoId,langId,senarioId);
        GameObject baseObj = boardObj.transform.Find("Base").gameObject;
        if (langId == 2) {
            baseObj.transform.Find("Player").transform.Find("Name").GetComponent<TextMesh>().text = myDaimyoName;
            baseObj.transform.Find("Enemy").transform.Find("Name").GetComponent<TextMesh>().text = enemyDaimyoName;
        }else {
            baseObj.transform.Find("Player").transform.Find("Name").GetComponent<TextMesh>().text = myDaimyoName + "軍";
            baseObj.transform.Find("Enemy").transform.Find("Name").GetComponent<TextMesh>().text = enemyDaimyoName + "軍";
        }
        simpleHPCounter playerHPScript = baseObj.transform.Find("Player").transform.Find("Hei").GetComponent<simpleHPCounter>();
        simpleHPCounter enemyHPScript = baseObj.transform.Find("Enemy").transform.Find("Hei").GetComponent<simpleHPCounter>();

        playerHPScript.board = boardObj ;
        enemyHPScript.board = boardObj;
        enemyHPScript.stageId = stageId;
        playerHPScript.katanaBtnObj = gameObject;
        playerHPScript.timer = timer;
        playerHPScript.stageId = stageId;
    }

    public void makeSimplePlayer(int busyoId, bool soudaisyoFlg, GameObject battleArea, int xAdjust, GameObject YesBtn) {
        string path = "Prefabs/Player/" + busyoId;
        GameObject prefab = Instantiate(Resources.Load(path)) as GameObject;
        prefab.name = busyoId.ToString();
        prefab.transform.SetParent(battleArea.transform);
        prefab.transform.localScale = new Vector2(0.4f,0.6f);
        prefab.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
        prefab.GetComponent<SpriteRenderer>().sortingOrder = 350;
        if(soudaisyoFlg) {
            prefab.transform.localPosition = new Vector2(1.0f, 1.8f);
        }else {
            float xAdjust2 = (float)xAdjust / 20;
            prefab.transform.localPosition = new Vector2(3 - xAdjust2, 1.8f);
        }
        prefab.GetComponent<Rigidbody2D>().gravityScale = 1;
        prefab.layer = LayerMask.NameToLayer("PlayerNoColl");


        //Set Scirpt
        Destroy(prefab.GetComponent<PlayerHP>());
        Destroy(prefab.GetComponent<SenpouController>());
        Destroy(prefab.GetComponent<UnitMover>());
        Destroy(prefab.GetComponent<Kunkou>());
        Destroy(prefab.GetComponent<LineLocation>());
        if(prefab.GetComponent<AttackLong>()) {
            Destroy(prefab.GetComponent<AttackLong>());
        }else {
            Destroy(prefab.GetComponent<PlayerAttack>());
        }
        Destroy(prefab.GetComponent<PlayerHP>());
        prefab.AddComponent<SimpleAttack>();
        prefab.AddComponent<SimpleHP>();
        prefab.AddComponent<Homing>();
        
        prefab.GetComponent<Homing>().enabled = false;

        YesBtn.GetComponent<StartSimpleKassen>().busyoObjList.Add(prefab);

        //Parametor
        string busyoString = busyoId.ToString();
        int lv = PlayerPrefs.GetInt(busyoString);
        StatusGet sts = new StatusGet();
        int hp = 100 * sts.getHp(busyoId, lv);
        int atk = 10 * sts.getAtk(busyoId, lv);
        int dfc = 10 * sts.getDfc(busyoId, lv);
        float spd = sts.getSpd(busyoId, lv);

        JyosyuHeiryoku jyosyuHei = new JyosyuHeiryoku();
        int addJyosyuHei = jyosyuHei.GetJyosyuHeiryoku(busyoId.ToString());

        KahouStatusGet kahouSts = new KahouStatusGet();
        string[] KahouStatusArray = kahouSts.getKahouForStatus(busyoId.ToString(), hp, atk, dfc, (int)spd);
        string kanniTmp = "kanni" + busyoId;
        float addAtkByKanni = 0;
        float addHpByKanni = 0;
        float addDfcByKanni = 0;

        if (PlayerPrefs.HasKey(kanniTmp)) {
            int kanniId = PlayerPrefs.GetInt(kanniTmp);
            if(kanniId != 0) {
                Kanni kanni = new Kanni();

                //Status
                string kanniTarget = kanni.getEffectTarget(kanniId);
                int effect = kanni.getEffect(kanniId);
                if (kanniTarget == "atk") {
                    addAtkByKanni = ((float)atk * (float)effect) / 100;
                }
                else if (kanniTarget == "hp") {
                    addHpByKanni = ((float)hp * (float)effect) / 100;
                }
                else if (kanniTarget == "dfc") {
                    addDfcByKanni = ((float)dfc * (float)effect) / 100;
                }
            }
        }

        atk = atk + int.Parse(KahouStatusArray[0]) + Mathf.FloorToInt(addAtkByKanni);
        hp = hp + int.Parse(KahouStatusArray[1]) + Mathf.FloorToInt(addHpByKanni) + addJyosyuHei;
        dfc = dfc + int.Parse(KahouStatusArray[2]) + Mathf.FloorToInt(addDfcByKanni);


        //Child Parametor
        string heiId = "hei" + busyoId.ToString();
        string chParam = PlayerPrefs.GetString(heiId, "0");
        if (chParam == "0" || chParam == "") {
            StatusGet statusScript = new StatusGet();
            string heisyu = statusScript.getHeisyu(busyoId);
            chParam = heisyu + ":1:1:1";
            PlayerPrefs.SetString(heiId, chParam);
            PlayerPrefs.Flush();
        }

        char[] delimiterChars = { ':' };
        if (chParam.Contains(":")) {
            string[] ch_list = chParam.Split(delimiterChars);

            int chQty = int.Parse(ch_list[1]);
            int chlv = int.Parse(ch_list[2]);
            int ch_status = int.Parse(ch_list[3]);
            int totalChldHp = 0;
            int totalChldAtk = 0;
            int totalChldDfc = 0;

            ch_status = ch_status * 10;
            int atkDfc = (int)sts.getChAtkDfc(ch_status, hp);

            totalChldHp = ch_status * chQty;
            totalChldAtk = atkDfc * chQty;
            totalChldDfc = atkDfc * chQty;

            //Set value
            hp = hp + totalChldHp;
            atk = atk + totalChldAtk;
            dfc = dfc + totalChldDfc;
        }


        prefab.GetComponent<Homing>().speed = spd/20;
        prefab.GetComponent<SimpleAttack>().atk = atk;
        prefab.GetComponent<SimpleHP>().dfc = dfc;
        prefab.GetComponent<SimpleAttack>().baseAtk = atk;
        prefab.GetComponent<SimpleHP>().baseDfc = dfc;
        prefab.GetComponent<SimpleHP>().life = hp;

        //check
        Daimyo Daimyo = new Daimyo();
        int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
        int senarioId = PlayerPrefs.GetInt("senarioId");
        int myDaimyoBusyo = Daimyo.getDaimyoBusyoId(myDaimyo, senarioId);

        if (busyoId == myDaimyoBusyo) {
            myDaimyoBusyoFlg = true;
        }
        

    }

    public void makeSimpleEnemy(int busyoId, GameObject battleArea, int xAdjust, GameObject YesBtn) {
        string path = "Prefabs/Player/" + busyoId;
        GameObject prefab = Instantiate(Resources.Load(path)) as GameObject;
        prefab.name = busyoId.ToString();
        prefab.transform.SetParent(battleArea.transform);
        prefab.transform.localScale = new Vector2(-0.4f, 0.6f);
        prefab.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
        prefab.GetComponent<SpriteRenderer>().sortingOrder = 350;

        /**Player to Enemy**/ 
        prefab.tag = "Enemy";
        prefab.layer = LayerMask.NameToLayer("EnemyNoColl");
        /**Player to Enemy End**/

        float xAdjust2 = (float)xAdjust / 5;
        prefab.transform.localPosition = new Vector2(9 - xAdjust2, 1.8f);        
        prefab.GetComponent<Rigidbody2D>().gravityScale = 1;

        //Set Scirpt
        Destroy(prefab.GetComponent<PlayerHP>());
        Destroy(prefab.GetComponent<SenpouController>());
        Destroy(prefab.GetComponent<LineLocation>());
        if (prefab.GetComponent<HomingLong>()) {
            Destroy(prefab.GetComponent<HomingLong>());
            prefab.AddComponent<Homing>();
        }
        if (prefab.GetComponent<AttackLong>()) {
            Destroy(prefab.GetComponent<AttackLong>());
        }else {
            Destroy(prefab.GetComponent<PlayerAttack>());
        }
        Destroy(prefab.GetComponent<PlayerHP>());
        prefab.AddComponent<SimpleAttack>();
        prefab.AddComponent<SimpleHP>();
        prefab.AddComponent<Homing>();
        prefab.GetComponent<Homing>().speed = 50;
        prefab.GetComponent<Homing>().enabled = false;

        YesBtn.GetComponent<StartSimpleKassen>().busyoObjList.Add(prefab);

        //Parametor
        StartKassen stksn = GameObject.Find("BattleButton").GetComponent<StartKassen>();
        int lv = stksn.activeBusyoLv;
        string busyoString = busyoId.ToString();
        StatusGet sts = new StatusGet();
        int hp = 100 * sts.getHp(busyoId, lv);
        int atk = 10 * sts.getAtk(busyoId, lv);
        int dfc = 10 * sts.getDfc(busyoId, lv);
        float spd = sts.getSpd(busyoId, lv);


        //Child Parametor
        int chlv = stksn.activeButaiLv;
        int chQty = stksn.activeButaiQty;
        EnemyInstance enemyInstance = new EnemyInstance();
        string ch_type = sts.getHeisyu(busyoId);
        int ch_status = enemyInstance.getChildStatus(lv, ch_type, 0);
        int totalChldHp = 0;
        int totalChldAtk = 0;
        int totalChldDfc = 0;
        int atkDfc = (int)sts.getChAtkDfc(ch_status, hp);

        totalChldHp = ch_status * chQty;
        totalChldAtk = atkDfc * chQty;
        totalChldDfc = atkDfc * chQty;

        //Set value
        hp = hp + totalChldHp;
        atk = atk + totalChldAtk;
        dfc = dfc + totalChldDfc;       

        prefab.GetComponent<Homing>().speed = spd/20;
        prefab.GetComponent<SimpleAttack>().atk = atk;
        prefab.GetComponent<SimpleHP>().dfc = dfc;
        prefab.GetComponent<SimpleAttack>().baseAtk = atk;
        prefab.GetComponent<SimpleHP>().baseDfc = dfc;
        prefab.GetComponent<SimpleHP>().life = hp;

    }

}
