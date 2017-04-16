using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyAttackPop : MonoBehaviour {

    public int enemyDaimyoId = 0;

	public void OnClick(){
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[0].Play();

        //Common
        string simpaleBattlePath = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            simpaleBattlePath = "Prefabs/SimpleBattle/BattleBoardEng";
        } else {
            simpaleBattlePath = "Prefabs/SimpleBattle/BattleBoard";
        }
        GameObject boardObj = Instantiate(Resources.Load(simpaleBattlePath)) as GameObject;
        boardObj.transform.SetParent(GameObject.Find("Map").transform);
        boardObj.transform.localScale = new Vector2(45,55);
        boardObj.transform.FindChild("No").GetComponent<CloseSimpleBattle>().boardObj = boardObj;
        GameObject battleArea = boardObj.transform.FindChild("Base").transform.FindChild("BattleArea").gameObject;
        GameObject YesBtn = boardObj.transform.FindChild("Yes").gameObject;
        GameObject NoBtn = boardObj.transform.FindChild("No").gameObject;
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
        makeSimplePlayer(soudaisyoId, true, battleArea, 0, YesBtn);

        for (int i=1; i<26; i++) {
            string mapId =  jinkei.ToString() + "map" + i.ToString();
            int busyoId = PlayerPrefs.GetInt(mapId);
            if(busyoId != 0) {
                if(soudaisyoId != busyoId) {
                    makeSimplePlayer(busyoId, false, battleArea, i, YesBtn);
                }
            }
        }
        //View Enemy
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
        string myDaimyoName = daimyScript.getName(myDaimyoId);
        string enemyDaimyoName = daimyScript.getName(enemyDaimyoId);
        GameObject baseObj = boardObj.transform.FindChild("Base").gameObject;
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            baseObj.transform.FindChild("Player").transform.FindChild("Name").GetComponent<TextMesh>().text = myDaimyoName;
            baseObj.transform.FindChild("Enemy").transform.FindChild("Name").GetComponent<TextMesh>().text = enemyDaimyoName;
        }else {
            baseObj.transform.FindChild("Player").transform.FindChild("Name").GetComponent<TextMesh>().text = myDaimyoName + "軍";
            baseObj.transform.FindChild("Enemy").transform.FindChild("Name").GetComponent<TextMesh>().text = enemyDaimyoName + "軍";
        }
        simpleHPCounter playerHPScript = baseObj.transform.FindChild("Player").transform.FindChild("Hei").GetComponent<simpleHPCounter>();
        simpleHPCounter enemyHPScript = baseObj.transform.FindChild("Enemy").transform.FindChild("Hei").GetComponent<simpleHPCounter>();

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

        prefab.GetComponent<Homing>().speed = spd/20;
        prefab.GetComponent<SimpleAttack>().atk = atk;
        prefab.GetComponent<SimpleHP>().dfc = dfc;
        prefab.GetComponent<SimpleHP>().life = hp;
    }

    public void makeSimpleEnemy(int busyoId, GameObject battleArea, int xAdjust, GameObject YesBtn) {
        string path = "Prefabs/Enemy/" + busyoId;
        GameObject prefab = Instantiate(Resources.Load(path)) as GameObject;
        prefab.name = busyoId.ToString();
        prefab.transform.SetParent(battleArea.transform);
        prefab.transform.localScale = new Vector2(-0.4f, 0.6f);
        prefab.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
        prefab.GetComponent<SpriteRenderer>().sortingOrder = 350;

        float xAdjust2 = (float)xAdjust / 2;
        prefab.transform.localPosition = new Vector2(9 - xAdjust2, 1.8f);
        
        prefab.GetComponent<Rigidbody2D>().gravityScale = 1;

        //Set Scirpt
        Destroy(prefab.GetComponent<EnemyHP>());
        Destroy(prefab.GetComponent<SenpouController>());
        Destroy(prefab.GetComponent<LineLocation>());
        if (prefab.GetComponent<HomingLong>()) {
            Destroy(prefab.GetComponent<HomingLong>());
            prefab.AddComponent<Homing>();
        }
        if (prefab.GetComponent<AttackLong>()) {
            Destroy(prefab.GetComponent<AttackLong>());
        }else {
            Destroy(prefab.GetComponent<EnemyAttack>());
        }
        Destroy(prefab.GetComponent<EnemyHP>());
        prefab.AddComponent<SimpleAttack>();
        prefab.AddComponent<SimpleHP>();
        prefab.GetComponent<Homing>().speed = 50;
        prefab.GetComponent<Homing>().enabled = false;

        YesBtn.GetComponent<StartSimpleKassen>().busyoObjList.Add(prefab);

        //Parametor
        int lv = GameObject.Find("BattleButton").GetComponent<StartKassen>().activeBusyoLv;
        string busyoString = busyoId.ToString();
        StatusGet sts = new StatusGet();
        int hp = 100 * sts.getHp(busyoId, lv);
        int atk = 10 * sts.getAtk(busyoId, lv);
        int dfc = 10 * sts.getDfc(busyoId, lv);
        float spd = sts.getSpd(busyoId, lv);

        prefab.GetComponent<Homing>().speed = spd/20;
        prefab.GetComponent<SimpleAttack>().atk = atk;
        prefab.GetComponent<SimpleHP>().dfc = dfc;
        prefab.GetComponent<SimpleHP>().life = hp;



    }

}
