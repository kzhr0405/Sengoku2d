using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class preKaisen : MonoBehaviour {

    public int jinkei = 0;
    public int enemyHei = 0;
    EnemyInstance enemyIns = new EnemyInstance();
    public int tempEnemySoudaisyo = 0;
    public bool isAttackedFlg;
    public bool isKessenFlg;
    public int kessenHyourou = 0;
    public List<string> jinkeiBusyo_list = new List<string>();
    public int jinkeiLimit = 0;
    public int busyoCurrentQty = 0;
    public int weatherId = 0;

    void Start() {

        //Sound
        BGMSESwitch bgm = new BGMSESwitch();
        bgm.StopSEVolume();
        bgm.StopBGMVolume();

        //Flag
        isAttackedFlg = PlayerPrefs.GetBool("isAttackedFlg");
        if (!isAttackedFlg) {
            isKessenFlg = PlayerPrefs.GetBool("isKessenFlg");
        }

        //message
        string kassenMsg = "Prefabs/Common/KassenMessage";
        if (isAttackedFlg || isKessenFlg) {
            GameObject msgObj = Instantiate(Resources.Load(kassenMsg)) as GameObject;
            msgObj.transform.SetParent(GameObject.Find("Panel").transform);
            msgObj.transform.localScale = new Vector2(1, 1);
            msgObj.transform.localPosition = new Vector2(0, 0);
            RectTransform msgObjTransform = msgObj.GetComponent<RectTransform>();
            msgObjTransform.sizeDelta = new Vector2(1000, 150);

            string msgTxt = "";
            int daimyoId = PlayerPrefs.GetInt("activeDaimyoId");
            Daimyo daimyo = new Daimyo();
            string daimyoName = daimyo.getName(daimyoId);

            if (isAttackedFlg) {
                int activeKuniId = PlayerPrefs.GetInt("activeKuniId");
                KuniInfo kuni = new KuniInfo();
                string kuniName = kuni.getKuniName(activeKuniId);
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    msgTxt = daimyoName + " is attacking " + kuniName + ". \n Let's defend this country.";
                }else {
                    msgTxt = daimyoName + "が" + kuniName + "に侵攻していますぞ。\n守り抜きましょうぞ。";
                }
            }
            else if (isKessenFlg) {
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    msgTxt = "It's a time to have a showdown with " + daimyoName + ".";
                }else {
                    msgTxt = daimyoName + "と雌雄を決する時です。\n腕が鳴りますな。";
                }
            }
            msgObj.transform.FindChild("MessageText").GetComponent<Text>().text = msgTxt;
        }

        //Stage Name
        string stageName = PlayerPrefs.GetString("activeStageName");
        if (!isKessenFlg) {
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                GameObject.Find("KassenNameValue").GetComponent<Text>().text = "Battle of " + stageName;
            }else {
                GameObject.Find("KassenNameValue").GetComponent<Text>().text = stageName + "の戦い";
            }
        }
        else {
            GameObject.Find("KassenNameValue").GetComponent<Text>().text = stageName;
        }

        //Wether Handling
        int weatherId = getWeatherId();
        Color rainSnowColor = new Color(140f / 255f, 140f / 255f, 140f / 255f, 255f / 255f);

        //if passive kassen
        Stage stage = new Stage();
        GameObject panel = GameObject.Find("Panel").gameObject;
        if (isAttackedFlg) {
            //Passive
            GameObject tettaiBtn = GameObject.Find("TettaiBtn").gameObject;
            GameObject hyourouIcon = GameObject.Find("StartBtn").transform.FindChild("hyourouIcon").gameObject;
            Destroy(tettaiBtn.gameObject);
            Destroy(hyourouIcon.gameObject);
            
        }

        /*Status Down by Weather & Map*/
        //Delete Previous minus status
        PlayerPrefs.DeleteKey("mntMinusStatus");
        PlayerPrefs.DeleteKey("seaMinusStatus");
        PlayerPrefs.DeleteKey("rainMinusStatus");
        PlayerPrefs.DeleteKey("snowMinusStatus");

        string txtPath = "Prefabs/PreKassen/TextSlot";
        GameObject content = GameObject.Find("EffectContent").gameObject;
       
        if (weatherId == 2 || weatherId == 3) {
            //snow or rain
            GameObject slot4 = Instantiate(Resources.Load(txtPath)) as GameObject;
            slot4.transform.SetParent(content.transform);
            GameObject text4 = slot4.transform.FindChild("Text1").gameObject;
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                text4.GetComponent<Text>().text = "All Unit Speed";
            }else {
                text4.GetComponent<Text>().text = "気象効果 全隊 迅速";
            }
            text4.transform.FindChild("Text2").GetComponent<Text>().text = "-50%";
            slot4.transform.localScale = new Vector2(1, 1);
            slot4.transform.localPosition = new Vector3(0, 0, 0);

            float minuValue = 50/ 100;
            PlayerPrefs.SetFloat("snowMinusStatus", minuValue);
        }

        //boubi addition
        if (isAttackedFlg) {
            int boubi = PlayerPrefs.GetInt("activeBoubi", 0);

            GameObject slot = Instantiate(Resources.Load(txtPath)) as GameObject;
            slot.transform.SetParent(content.transform);
            GameObject text = slot.transform.FindChild("Text1").gameObject;
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                text.GetComponent<Text>().text = "Player Unit Defence";
            }else {
                text.GetComponent<Text>().text = "防備効果 味方 守備";
            }
            text.transform.FindChild("Text2").GetComponent<Text>().text = "+" + boubi;
            slot.transform.localScale = new Vector2(1, 1);
            slot.transform.localPosition = new Vector3(0, 0, 0);

        }


        PlayerPrefs.Flush();

        jinkei = PlayerPrefs.GetInt("jinkei");
        //changeFormButtonColor(jinkei);
        prekassenPlayerJinkei(jinkei, weatherId, isAttackedFlg, false);

    }


    public int getTaisyoMapId(int enemyJinkei) {
        int taisyoMapId = 0;
        if (enemyJinkei == 1) {
            taisyoMapId = 15;
        }
        else if (enemyJinkei == 2) {
            taisyoMapId = 15;
        }
        else if (enemyJinkei == 3) {
            taisyoMapId = 14;
        }
        else if (enemyJinkei == 4) {
            taisyoMapId = 14;
        }
        return taisyoMapId;
    }


    public List<int> enemyJinkeiMaker(int enemyJinkei) {

        /*Jinkei*/
        List<int> mapList = new List<int>();
        if (enemyJinkei == 1) {

            mapList = new List<int>() { 4, 5, 8, 9, 12, 13, 14, 18, 19, 24, 25 };

            //Disable 1,2,3,6,7,10,11,16,17,20,21,22,23
            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("EnemySlot")) {
                if (obs.name == "Slot1" || obs.name == "Slot2" || obs.name == "Slot3" || obs.name == "Slot6" ||
                   obs.name == "Slot7" || obs.name == "Slot10" || obs.name == "Slot11" || obs.name == "Slot16" || obs.name == "Slot17" ||
                   obs.name == "Slot20" || obs.name == "Slot21" || obs.name == "Slot22" || obs.name == "Slot23") {

                    obs.GetComponent<Image>().enabled = false;
                    if (obs.transform.IsChildOf(obs.transform)) {

                        foreach (Transform n in obs.transform) {
                            GameObject.Destroy(n.gameObject);
                        }
                    }
                }
            }

        }
        else if (enemyJinkei == 2) {

            mapList = new List<int>() { 1, 2, 3, 8, 9, 14, 18, 19, 21, 22, 23 };

            //Disable 4,5,6,7,10,11,12,13,16,17,20,24,25
            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("EnemySlot")) {
                if (obs.name == "Slot4" || obs.name == "Slot5" || obs.name == "Slot6" || obs.name == "Slot7" ||
                   obs.name == "Slot10" || obs.name == "Slot11" || obs.name == "Slot12" || obs.name == "Slot13" ||
                   obs.name == "Slot16" || obs.name == "Slot17" || obs.name == "Slot20" || obs.name == "Slot24" || obs.name == "Slot25") {

                    obs.GetComponent<Image>().enabled = false;
                    if (obs.transform.IsChildOf(obs.transform)) {

                        foreach (Transform n in obs.transform) {
                            GameObject.Destroy(n.gameObject);
                        }
                    }
                }
            }

        }
        else if (enemyJinkei == 3) {

            mapList = new List<int>() { 1, 5, 6, 10, 11, 12, 15, 17, 18, 19, 23 };

            //Disable 2,3,4,7,8,9,13,16,20,21,22,24,25
            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("EnemySlot")) {
                if (obs.name == "Slot2" || obs.name == "Slot3" || obs.name == "Slot4" || obs.name == "Slot7" ||
                   obs.name == "Slot8" || obs.name == "Slot9" || obs.name == "Slot13" || obs.name == "Slot16" ||
                   obs.name == "Slot20" || obs.name == "Slot21" || obs.name == "Slot22" || obs.name == "Slot24" || obs.name == "Slot25") {

                    obs.GetComponent<Image>().enabled = false;
                    if (obs.transform.IsChildOf(obs.transform)) {

                        foreach (Transform n in obs.transform) {
                            GameObject.Destroy(n.gameObject);
                        }
                    }
                }
            }

        }
        else if (enemyJinkei == 4) {

            mapList = new List<int>() { 4, 5, 8, 9, 12, 13, 16, 17, 18, 21, 22 };

            //Disable 1,2,3,6,7,10,11,15,19,20,23,24,25
            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("EnemySlot")) {
                if (obs.name == "Slot1" || obs.name == "Slot2" || obs.name == "Slot3" || obs.name == "Slot6" ||
                   obs.name == "Slot7" || obs.name == "Slot10" || obs.name == "Slot11" || obs.name == "Slot15" ||
                   obs.name == "Slot19" || obs.name == "Slot20" || obs.name == "Slot23" || obs.name == "Slot24" || obs.name == "Slot25") {

                    obs.GetComponent<Image>().enabled = false;
                    if (obs.transform.IsChildOf(obs.transform)) {

                        foreach (Transform n in obs.transform) {
                            GameObject.Destroy(n.gameObject);
                        }
                    }
                }
            }
        }
        return mapList;
    }


    //PowerType1
    //Busyo + Mob
    public int powerType1(List<int> mapList, int taisyoMapId, int linkNo, int activeDaimyoId) {
        int totalHei = 0;

        int activeBusyoQty = PlayerPrefs.GetInt("activeBusyoQty");
        int activeBusyoLv = PlayerPrefs.GetInt("activeBusyoLv");
        int activeButaiQty = PlayerPrefs.GetInt("activeButaiQty");
        int activeButaiLv = PlayerPrefs.GetInt("activeButaiLv");
        Entity_daimyo_mst daimyoMst = Resources.Load("Data/daimyo_mst") as Entity_daimyo_mst;
        int daimyoBusyoId = daimyoMst.param[activeDaimyoId - 1].busyoId;

        /*Busyo Master Setting Start*/
        //Active Busyo List
        List<string> metsubouDaimyoList = new List<string>();
        string metsubouTemp = "metsubou" + activeDaimyoId;
        string metsubouDaimyoString = PlayerPrefs.GetString(metsubouTemp);
        char[] delimiterChars2 = { ',' };
        if (metsubouDaimyoString != null && metsubouDaimyoString != "") {
            if (metsubouDaimyoString.Contains(",")) {
                metsubouDaimyoList = new List<string>(metsubouDaimyoString.Split(delimiterChars2));
            }
            else {
                metsubouDaimyoList = new List<string>(metsubouDaimyoString.Split(delimiterChars2));
            }
        }
        metsubouDaimyoList.Add(activeDaimyoId.ToString());


        Entity_busyo_mst busyoMst = Resources.Load("Data/busyo_mst") as Entity_busyo_mst;
        List<int> busyoList = new List<int>();

        for (int i = 0; i < busyoMst.param.Count; i++) {
            int busyoId = busyoMst.param[i].id;
            int daimyoId = busyoMst.param[i].daimyoId;

            if (metsubouDaimyoList.Contains(daimyoId.ToString())) {
                if (busyoId != daimyoBusyoId) {

                    busyoList.Add(busyoId);
                }
            }
        }
        /*Busyo Master Setting End*/

        /*Random Shuffle*/
        for (int i = 0; i < busyoList.Count; i++) {
            int temp = busyoList[i];
            int randomIndex = UnityEngine.Random.Range(0, busyoList.Count);
            busyoList[i] = busyoList[randomIndex];
            busyoList[randomIndex] = temp;
        }

        for (int i = 0; i < mapList.Count; i++) {
            int temp = mapList[i];
            int randomIndex = UnityEngine.Random.Range(0, mapList.Count);
            mapList[i] = mapList[randomIndex];
            mapList[randomIndex] = temp;
        }


        /*Taisyo Setting Start*/
        GameObject EnemyJinkeiView = GameObject.Find("EnemyJinkeiView").gameObject;
        int taisyoBusyoId = busyoList[0];

        StatusGet sts = new StatusGet();
        BusyoInfoGet info = new BusyoInfoGet();

        int hp = sts.getHp(taisyoBusyoId, activeBusyoLv);
        hp = hp * 100;

        //Roujyo Check
        if (!isAttackedFlg && !isKessenFlg) {
            int atk = sts.getBaseAtk(taisyoBusyoId);
            int dfc = sts.getBaseDfc(taisyoBusyoId);
        }


        //Link Status Adjustment
        if (linkNo != 0) {
            float linkAdjst = (float)linkNo / 10;
            float adjstHp = hp * linkAdjst;
            hp = hp + (int)adjstHp;
        }

        //ship hp adjust
        BusyoInfoGet busyoScript = new BusyoInfoGet();
        int shipId = busyoScript.getShipId(taisyoBusyoId);
        if (shipId == 1) {
            hp = hp * 2;
        }else if (shipId == 2) {
            hp = Mathf.FloorToInt((float)hp * 1.5f);
        }



        string TaisyoBusyoName = info.getName(taisyoBusyoId);
        string TaisyoType = info.getHeisyu(taisyoBusyoId);

        int chldHp = activeButaiQty * enemyIns.getChildStatus(activeButaiLv, TaisyoType, linkNo);
        totalHei = hp + chldHp;

        string path = "Prefabs/Player/Unit/Ship";
        GameObject tsyBusyo = Instantiate(Resources.Load(path)) as GameObject;
        tsyBusyo.name = taisyoBusyoId.ToString();
        getShipSprite(tsyBusyo, taisyoBusyoId);

        string slotName = "Slot" + taisyoMapId;
        tsyBusyo.transform.SetParent(EnemyJinkeiView.transform.FindChild(slotName).transform);
        tsyBusyo.name = taisyoBusyoId.ToString();
        tsyBusyo.transform.localScale = new Vector2(-3, 3);
        tsyBusyo.transform.localPosition = new Vector3(0, 0, 0);
        tempEnemySoudaisyo = int.Parse(tsyBusyo.name);


        //Button
        string soudaisyoPath = "Prefabs/Jinkei/soudaisyo";
        GameObject soudaisyo = Instantiate(Resources.Load(soudaisyoPath)) as GameObject;
        soudaisyo.transform.SetParent(tsyBusyo.transform);
        soudaisyo.transform.localScale = new Vector2(27, 12);
        soudaisyo.name = "enemySoudaisyo";
        soudaisyo.transform.localPosition = new Vector3(0, 11, 0);
        tsyBusyo.GetComponent<DragHandler>().enabled = false;

        //Label & Text
        GameObject txtObj = tsyBusyo.transform.FindChild("Text").gameObject;

        Vector2 txtScale = txtObj.transform.localScale;
        txtScale.x *= -1;
        txtObj.transform.localScale = txtScale;
        Vector2 txtPos = txtObj.transform.localPosition;
        txtPos.x *= -1;
        txtObj.transform.localPosition = txtPos;

        GameObject rblObj = tsyBusyo.transform.FindChild("Rank").gameObject;
        Vector2 rblScale = rblObj.transform.localScale;
        rblScale.x *= -1;
        rblObj.transform.localScale = rblScale;
        Vector2 rblPos = rblObj.transform.localPosition;
        rblPos.x *= -1;
        rblObj.transform.localPosition = rblPos;

        GameObject tsyTxtObj = tsyBusyo.transform.FindChild("enemySoudaisyo").transform.FindChild("Text").gameObject;
        Vector2 tsyScale = tsyTxtObj.transform.localScale;
        tsyScale.x *= -1;
        tsyTxtObj.transform.localScale = tsyScale;
        Vector2 tsyPos = tsyTxtObj.transform.localPosition;
        tsyPos.x *= -1;
        tsyTxtObj.transform.localPosition = tsyPos;

        /*Taisyo Setting End*/


        //Make Instance
        int busyoListCount = busyoList.Count - 1;
        int torideQty = linkNo;
        for (int j = 0; j < activeBusyoQty - 1; j++) {

            if (busyoListCount > 0) {
                busyoListCount = busyoListCount - 1;
                int mapId = mapList[j];

                //samurai daisyo make
                int busyoHp = sts.getHp(35, activeBusyoLv);
                busyoHp = busyoHp * 100;

                //Link Status Adjustment
                if (linkNo != 0) {
                    float linkAdjst = (float)linkNo / 10;
                    float adjstHp = busyoHp * linkAdjst;
                    busyoHp = busyoHp + (int)adjstHp;
                }

                //ship hp adjust
                int chldShipId = busyoScript.getShipId(35);
                if (chldShipId == 1) {
                    busyoHp = busyoHp * 2;
                }else if (chldShipId == 2) {
                    busyoHp = Mathf.FloorToInt((float)busyoHp * 1.5f);
                }

                string busyoName = info.getName(35);
                string[] texts = new string[] { "YR", "KB" };
                string busyoType = texts[UnityEngine.Random.Range(0, texts.Length - 1)];

                int chldHp2 = activeButaiQty * enemyIns.getChildStatus(activeButaiLv, busyoType, linkNo);
                totalHei = totalHei + busyoHp + chldHp2;

                string busyoPath = "Prefabs/Player/Unit/Ship";
                GameObject chdBusyo = Instantiate(Resources.Load(busyoPath)) as GameObject;
                chdBusyo.name = "35";
                getShipSprite(chdBusyo, 35);

                string chdSlotName = "Slot" + mapId;
                chdBusyo.transform.SetParent(EnemyJinkeiView.transform.FindChild(chdSlotName).transform);
                chdBusyo.transform.localScale = new Vector2(-3, 3);
                chdBusyo.transform.localPosition = new Vector3(0, 0, 0);

                //Button
                chdBusyo.GetComponent<DragHandler>().enabled = false;

                //Rabel & Text
                GameObject chTxtObj = chdBusyo.transform.FindChild("Text").gameObject;
                Vector2 chTxtScale = chTxtObj.transform.localScale;
                chTxtScale.x *= -1;
                chTxtObj.transform.localScale = chTxtScale;
                Vector2 chTxtPos = chTxtObj.transform.localPosition;
                chTxtPos.x *= -1;
                chTxtObj.transform.localPosition = chTxtPos;

                GameObject chLblObj = chdBusyo.transform.FindChild("Rank").gameObject;
                Vector2 chLblScale = chLblObj.transform.localScale;
                chLblScale.x *= -1;
                chLblObj.transform.localScale = chLblScale;
                Vector2 chLblPos = chLblObj.transform.localPosition;
                chLblPos.x *= -1;
                chLblObj.transform.localPosition = chLblPos;

                

            }
        }




        return totalHei;
    }




    //PowerType2
    //Busyo + Busyo
    public int powerType2(List<int> mapList, int taisyoMapId, int linkNo, int activeDaimyoId) {
        int totalHei = 0;

        int activeBusyoQty = PlayerPrefs.GetInt("activeBusyoQty");
        int activeBusyoLv = PlayerPrefs.GetInt("activeBusyoLv");
        int activeButaiQty = PlayerPrefs.GetInt("activeButaiQty");
        int activeButaiLv = PlayerPrefs.GetInt("activeButaiLv");
        Entity_daimyo_mst daimyoMst = Resources.Load("Data/daimyo_mst") as Entity_daimyo_mst;
        int daimyoBusyoId = daimyoMst.param[activeDaimyoId - 1].busyoId;


        /*Busyo Master Setting Start*/
        //Active Busyo List
        List<string> metsubouDaimyoList = new List<string>();
        string metsubouTemp = "metsubou" + activeDaimyoId;
        string metsubouDaimyoString = PlayerPrefs.GetString(metsubouTemp);
        char[] delimiterChars2 = { ',' };
        if (metsubouDaimyoString != null && metsubouDaimyoString != "") {
            if (metsubouDaimyoString.Contains(",")) {
                metsubouDaimyoList = new List<string>(metsubouDaimyoString.Split(delimiterChars2));
            }
            else {
                metsubouDaimyoList = new List<string>(metsubouDaimyoString.Split(delimiterChars2));
            }
        }
        metsubouDaimyoList.Add(activeDaimyoId.ToString());

        Entity_busyo_mst busyoMst = Resources.Load("Data/busyo_mst") as Entity_busyo_mst;
        List<int> busyoList = new List<int>();

        for (int i = 0; i < busyoMst.param.Count; i++) {
            int busyoId = busyoMst.param[i].id;
            int daimyoId = busyoMst.param[i].daimyoId;

            if (metsubouDaimyoList.Contains(daimyoId.ToString())) {
                if (busyoId != daimyoBusyoId) {

                    busyoList.Add(busyoId);
                }
            }
        }
        /*Busyo Master Setting End*/

        /*Random Shuffle*/
        for (int i = 0; i < busyoList.Count; i++) {
            int temp = busyoList[i];
            int randomIndex = UnityEngine.Random.Range(0, busyoList.Count);
            busyoList[i] = busyoList[randomIndex];
            busyoList[randomIndex] = temp;
        }

        for (int i = 0; i < mapList.Count; i++) {
            int temp = mapList[i];
            int randomIndex = UnityEngine.Random.Range(0, mapList.Count);
            mapList[i] = mapList[randomIndex];
            mapList[randomIndex] = temp;
        }


        /*Taisyo Setting Start*/
        GameObject EnemyJinkeiView = GameObject.Find("EnemyJinkeiView").gameObject;
        int taisyoBusyoId = busyoList[0];

        StatusGet sts = new StatusGet();
        BusyoInfoGet info = new BusyoInfoGet();

        int hp = sts.getHp(taisyoBusyoId, activeBusyoLv);
        hp = hp * 100;

        //Roujyo Check
        if (!isAttackedFlg) {
            int atk = sts.getBaseAtk(taisyoBusyoId);
            int dfc = sts.getBaseDfc(taisyoBusyoId);
        }

        //Link Status Adjustment
        if (linkNo != 0) {
            float linkAdjst = (float)linkNo / 10;
            float adjstHp = hp * linkAdjst;
            hp = hp + (int)adjstHp;
        }
        //ship hp adjust
        BusyoInfoGet busyoScript = new BusyoInfoGet();
        int shipId = busyoScript.getShipId(taisyoBusyoId);
        if (shipId == 1) {
            hp = hp * 2;
        }else if (shipId == 2) {
            hp = Mathf.FloorToInt((float)hp * 1.5f);
        }


        string TaisyoBusyoName = info.getName(taisyoBusyoId);
        string TaisyoType = info.getHeisyu(taisyoBusyoId);

        int chldHp = activeButaiQty * enemyIns.getChildStatus(activeButaiLv, TaisyoType, linkNo);
        totalHei = hp + chldHp;

        string path = "Prefabs/Player/Unit/Ship";
        GameObject tsyBusyo = Instantiate(Resources.Load(path)) as GameObject;
        tsyBusyo.name = taisyoBusyoId.ToString();
        getShipSprite(tsyBusyo, taisyoBusyoId);

        string slotName = "Slot" + taisyoMapId;
        tsyBusyo.transform.SetParent(EnemyJinkeiView.transform.FindChild(slotName).transform);
        tsyBusyo.name = taisyoBusyoId.ToString();
        tsyBusyo.transform.localScale = new Vector2(-3, 3);
        tsyBusyo.transform.localPosition = new Vector3(0, 0, 0);
        tempEnemySoudaisyo = int.Parse(tsyBusyo.name);


        //Button
        string soudaisyoPath = "Prefabs/Jinkei/soudaisyo";
        GameObject soudaisyo = Instantiate(Resources.Load(soudaisyoPath)) as GameObject;
        soudaisyo.transform.SetParent(tsyBusyo.transform);
        soudaisyo.transform.localScale = new Vector2(27, 12);
        soudaisyo.name = "enemySoudaisyo";
        soudaisyo.transform.localPosition = new Vector3(0, 11, 0);
        tsyBusyo.GetComponent<DragHandler>().enabled = false;

        //Text
        GameObject txtObj = tsyBusyo.transform.FindChild("Text").gameObject;
        Vector2 txtScale = txtObj.transform.localScale;
        txtScale.x *= -1;
        txtObj.transform.localScale = txtScale;
        Vector2 txtPos = txtObj.transform.localPosition;
        txtPos.x *= -1;
        txtObj.transform.localPosition = txtPos;

        GameObject rblObj = tsyBusyo.transform.FindChild("Rank").gameObject;
        Vector2 rblScale = rblObj.transform.localScale;
        rblScale.x *= -1;
        rblObj.transform.localScale = rblScale;
        Vector2 rblPos = rblObj.transform.localPosition;
        rblPos.x *= -1;
        rblObj.transform.localPosition = rblPos;

        GameObject tsyTxtObj = tsyBusyo.transform.FindChild("enemySoudaisyo").transform.FindChild("Text").gameObject;
        Vector2 tsyScale = tsyTxtObj.transform.localScale;
        tsyScale.x *= -1;
        tsyTxtObj.transform.localScale = tsyScale;
        Vector2 tsyPos = tsyTxtObj.transform.localPosition;
        tsyPos.x *= -1;
        tsyTxtObj.transform.localPosition = tsyPos;

        /*Taisyo Setting End*/


        //Make Instance
        busyoList.Remove(taisyoBusyoId);
        int busyoListCount = busyoList.Count;
        int torideQty = linkNo;
        for (int j = 0; j < activeBusyoQty - 1; j++) {

            if (busyoListCount > 0) {
                int randomBusyoId = busyoList[j];
                busyoListCount = busyoListCount - 1;
                int mapId = mapList[j];

                //Status
                if (randomBusyoId != 0) {
                    int busyoHp = sts.getHp(randomBusyoId, activeBusyoLv);
                    busyoHp = busyoHp * 100;

                    //Link Status Adjustment
                    if (linkNo != 0) {
                        float linkAdjst = (float)linkNo / 10;
                        float adjstHp = busyoHp * linkAdjst;
                        busyoHp = busyoHp + (int)adjstHp;
                    }
                    //ship hp adjust
                    int chldShipId = busyoScript.getShipId(randomBusyoId);
                    
                    if (chldShipId == 1) {
                        busyoHp = busyoHp * 2;
                    }else if (chldShipId == 2) {
                        busyoHp = Mathf.FloorToInt((float)busyoHp * 1.5f);
                    }
                    
                    string busyoName = info.getName(randomBusyoId);
                    string busyoType = info.getHeisyu(randomBusyoId);

                    int chldHp2 = activeButaiQty * enemyIns.getChildStatus(activeButaiLv, busyoType, linkNo);
                    totalHei = totalHei + busyoHp + chldHp2;

                    string busyoPath = "Prefabs/Player/Unit/Ship";
                    GameObject chdBusyo = Instantiate(Resources.Load(busyoPath)) as GameObject;
                    chdBusyo.name = randomBusyoId.ToString();
                    getShipSprite(chdBusyo, randomBusyoId);

                    string chdSlotName = "Slot" + mapId;
                    chdBusyo.transform.SetParent(EnemyJinkeiView.transform.FindChild(chdSlotName).transform);
                    chdBusyo.name = randomBusyoId.ToString();
                    chdBusyo.transform.localScale = new Vector2(-3, 3);
                    chdBusyo.transform.localPosition = new Vector3(0, 0, 0);

                    //Button
                    chdBusyo.GetComponent<DragHandler>().enabled = false;

                    //Text
                    GameObject chTxtObj = chdBusyo.transform.FindChild("Text").gameObject;
                    Vector2 chTxtScale = chTxtObj.transform.localScale;
                    chTxtScale.x *= -1;
                    chTxtObj.transform.localScale = chTxtScale;
                    Vector2 chTxtPos = chTxtObj.transform.localPosition;
                    chTxtPos.x *= -1;
                    chTxtObj.transform.localPosition = chTxtPos;

                    GameObject chRblObj = chdBusyo.transform.FindChild("Rank").gameObject;
                    Vector2 chRblScale = chRblObj.transform.localScale;
                    chRblScale.x *= -1;
                    chRblObj.transform.localScale = chRblScale;
                    Vector2 chRblPos = chRblObj.transform.localPosition;
                    chRblPos.x *= -1;
                    chRblObj.transform.localPosition = chRblPos;


                }
            }
            else {
                //samurai daisyo make
                busyoListCount = busyoListCount - 1;
                int mapId = mapList[j];

                int busyoHp = sts.getHp(35, activeBusyoLv);
                busyoHp = busyoHp * 100;

                //Link Status Adjustment
                if (linkNo != 0) {
                    float linkAdjst = (float)linkNo / 10;
                    float adjstHp = busyoHp * linkAdjst;
                    busyoHp = busyoHp + (int)adjstHp;
                }
                //ship hp adjust
                int chldShipId = busyoScript.getShipId(35);
                if (chldShipId == 1) {
                    busyoHp = busyoHp * 2;
                }
                else if (chldShipId == 2) {
                    busyoHp = Mathf.FloorToInt((float)busyoHp * 1.5f);
                }


                string busyoName = info.getName(35);
                string[] texts = new string[] { "YR", "KB" };
                string busyoType = texts[UnityEngine.Random.Range(0, texts.Length - 1)];

                int chldHp2 = activeButaiQty * enemyIns.getChildStatus(activeButaiLv, busyoType, linkNo);
                totalHei = totalHei + busyoHp + chldHp2;

                string busyoPath = "Prefabs/Player/Unit/Ship";
                GameObject chdBusyo = Instantiate(Resources.Load(busyoPath)) as GameObject;
                chdBusyo.name = "35";
                getShipSprite(chdBusyo, 35);

                string chdSlotName = "Slot" + mapId;
                chdBusyo.transform.SetParent(EnemyJinkeiView.transform.FindChild(chdSlotName).transform);
                chdBusyo.transform.localScale = new Vector2(-3, 3);
                chdBusyo.transform.localPosition = new Vector3(0, 0, 0);

                //Button
                chdBusyo.GetComponent<DragHandler>().enabled = false;

                //Rabel & Text
                GameObject chTxtObj = chdBusyo.transform.FindChild("Text").gameObject;
                Vector2 chTxtScale = chTxtObj.transform.localScale;
                chTxtScale.x *= -1;
                chTxtObj.transform.localScale = chTxtScale;
                Vector2 chTxtPos = chTxtObj.transform.localPosition;
                chTxtPos.x *= -1;
                chTxtObj.transform.localPosition = chTxtPos;

                GameObject chRblObj = chdBusyo.transform.FindChild("Rank").gameObject;
                Vector2 chRblScale = chRblObj.transform.localScale;
                chRblScale.x *= -1;
                chRblObj.transform.localScale = chRblScale;
                Vector2 chRblPos = chRblObj.transform.localPosition;
                chRblPos.x *= -1;
                chRblObj.transform.localPosition = chRblPos;

            }

        }

        return totalHei;
    }



    //PowerType3
    //Daimyo + Busyo
    public int powerType3(List<int> mapList, int taisyoMapId, int linkNo, int activeDaimyoId) {
        int totalHei = 0;

        int activeBusyoQty = PlayerPrefs.GetInt("activeBusyoQty");
        int activeBusyoLv = PlayerPrefs.GetInt("activeBusyoLv");
        int activeButaiQty = PlayerPrefs.GetInt("activeButaiQty");
        int activeButaiLv = PlayerPrefs.GetInt("activeButaiLv");

        Entity_daimyo_mst daimyoMst = Resources.Load("Data/daimyo_mst") as Entity_daimyo_mst;
        int daimyoBusyoId = daimyoMst.param[activeDaimyoId - 1].busyoId;

        /*Taisyo Setting Start*/
        GameObject EnemyJinkeiView = GameObject.Find("EnemyJinkeiView").gameObject;
        StatusGet sts = new StatusGet();
        BusyoInfoGet info = new BusyoInfoGet();

        int hp = sts.getHp(daimyoBusyoId, activeBusyoLv);
        hp = hp * 100;

        //Roujyo Check
        if (!isAttackedFlg) {
            int atk = sts.getBaseAtk(daimyoBusyoId);
            int dfc = sts.getBaseDfc(daimyoBusyoId);
        }

        //Link Status Adjustment
        if (linkNo != 0) {
            float linkAdjst = (float)linkNo / 10;
            float adjstHp = hp * linkAdjst;
            hp = hp + (int)adjstHp;
        }
        //ship hp adjust
        BusyoInfoGet busyoScript = new BusyoInfoGet();
        int shipId = busyoScript.getShipId(daimyoBusyoId);
        if (shipId == 1) {
            hp = hp * 2;
        }else if (shipId == 2) {
            hp = Mathf.FloorToInt((float)hp * 1.5f);
        }

        string daimyoBusyoName = info.getName(daimyoBusyoId);
        string daimyoType = info.getHeisyu(daimyoBusyoId);

        int chldHp = activeButaiQty * enemyIns.getChildStatus(activeButaiLv, daimyoType, linkNo);
        totalHei = hp + chldHp;

        string path = "Prefabs/Player/Unit/Ship";
        GameObject tsyBusyo = Instantiate(Resources.Load(path)) as GameObject;
        tsyBusyo.name = daimyoBusyoId.ToString();
        getShipSprite(tsyBusyo, daimyoBusyoId);

        string slotName = "Slot" + taisyoMapId;
        tsyBusyo.transform.SetParent(EnemyJinkeiView.transform.FindChild(slotName).transform);
        tsyBusyo.name = daimyoBusyoId.ToString();
        tsyBusyo.transform.localScale = new Vector2(-3, 3);
        tsyBusyo.transform.localPosition = new Vector3(0, 0, 0);
        tempEnemySoudaisyo = int.Parse(tsyBusyo.name);

        //Button
        string soudaisyoPath = "Prefabs/Jinkei/soudaisyo";
        GameObject soudaisyo = Instantiate(Resources.Load(soudaisyoPath)) as GameObject;
        soudaisyo.transform.SetParent(tsyBusyo.transform);
        soudaisyo.transform.localScale = new Vector2(27, 12);
        soudaisyo.name = "enemySoudaisyo";
        soudaisyo.transform.localPosition = new Vector3(0, 11, 0);
        tsyBusyo.GetComponent<DragHandler>().enabled = false;

        //Text
        GameObject txtObj = tsyBusyo.transform.FindChild("Text").gameObject;
        Vector2 txtScale = txtObj.transform.localScale;
        txtScale.x *= -1;
        txtObj.transform.localScale = txtScale;
        Vector2 txtPos = txtObj.transform.localPosition;
        txtPos.x *= -1;
        txtObj.transform.localPosition = txtPos;

        GameObject rblObj = tsyBusyo.transform.FindChild("Rank").gameObject;
        Vector2 rblScale = rblObj.transform.localScale;
        rblScale.x *= -1;
        rblObj.transform.localScale = rblScale;
        Vector2 rblPos = rblObj.transform.localPosition;
        rblPos.x *= -1;
        rblObj.transform.localPosition = rblPos;

        GameObject tsyTxtObj = tsyBusyo.transform.FindChild("enemySoudaisyo").transform.FindChild("Text").gameObject;
        Vector2 tsyScale = tsyTxtObj.transform.localScale;
        tsyScale.x *= -1;
        tsyTxtObj.transform.localScale = tsyScale;
        Vector2 tsyPos = tsyTxtObj.transform.localPosition;
        tsyPos.x *= -1;
        tsyTxtObj.transform.localPosition = tsyPos;
        /*Taisyo Setting End*/


        /*Busyo Setting Start*/
        if (activeBusyoQty > 1) {
            //Active Busyo List
            List<string> metsubouDaimyoList = new List<string>();
            string metsubouTemp = "metsubou" + activeDaimyoId;
            string metsubouDaimyoString = PlayerPrefs.GetString(metsubouTemp);
            char[] delimiterChars2 = { ',' };
            if (metsubouDaimyoString != null && metsubouDaimyoString != "") {
                if (metsubouDaimyoString.Contains(",")) {
                    metsubouDaimyoList = new List<string>(metsubouDaimyoString.Split(delimiterChars2));
                }
                else {
                    metsubouDaimyoList = new List<string>(metsubouDaimyoString.Split(delimiterChars2));
                }
            }
            metsubouDaimyoList.Add(activeDaimyoId.ToString());

            Entity_busyo_mst busyoMst = Resources.Load("Data/busyo_mst") as Entity_busyo_mst;
            List<int> busyoList = new List<int>();

            for (int i = 0; i < busyoMst.param.Count; i++) {
                int busyoId = busyoMst.param[i].id;
                int daimyoId = busyoMst.param[i].daimyoId;

                if (metsubouDaimyoList.Contains(daimyoId.ToString())) {
                    if (busyoId != daimyoBusyoId) {
                        busyoList.Add(busyoId);
                    }
                }
            }

            //Random Shuffle
            for (int i = 0; i < busyoList.Count; i++) {
                int temp = busyoList[i];
                int randomIndex = UnityEngine.Random.Range(0, busyoList.Count);
                busyoList[i] = busyoList[randomIndex];
                busyoList[randomIndex] = temp;
            }

            for (int i = 0; i < mapList.Count; i++) {
                int temp = mapList[i];
                int randomIndex = UnityEngine.Random.Range(0, mapList.Count);
                mapList[i] = mapList[randomIndex];
                mapList[randomIndex] = temp;
            }


            //Make Instance
            int busyoListCount = busyoList.Count;
            int torideQty = linkNo;
            for (int j = 0; j < activeBusyoQty - 1; j++) {

                if (busyoListCount > 0) {
                    int randomBusyoId = busyoList[j];
                    busyoListCount = busyoListCount - 1;
                    int mapId = mapList[j];


                    //Status
                    if (randomBusyoId != 0) {
                        int busyoHp = sts.getHp(randomBusyoId, activeBusyoLv);
                        busyoHp = busyoHp * 100;

                        if (linkNo != 0) {
                            float linkAdjst = (float)linkNo / 10;
                            float adjstHp = busyoHp * linkAdjst;
                            busyoHp = busyoHp + (int)adjstHp;
                        }
                        //ship hp adjust
                        int chldShipId = busyoScript.getShipId(randomBusyoId);
                        if (chldShipId == 1) {
                            busyoHp = busyoHp * 2;
                        }
                        else if (chldShipId == 2) {
                            busyoHp = Mathf.FloorToInt((float)busyoHp * 1.5f);
                        }


                        string busyoName = info.getName(randomBusyoId);
                        string busyoType = info.getHeisyu(randomBusyoId);

                        int chldHp2 = activeButaiQty * enemyIns.getChildStatus(activeButaiLv, busyoType, linkNo);
                        totalHei = totalHei + busyoHp + chldHp2;

                        string busyoPath = "Prefabs/Player/Unit/Ship";
                        GameObject chdBusyo = Instantiate(Resources.Load(busyoPath)) as GameObject;
                        chdBusyo.name = randomBusyoId.ToString();
                        getShipSprite(chdBusyo, randomBusyoId);

                        string chdSlotName = "Slot" + mapId;
                        chdBusyo.transform.SetParent(EnemyJinkeiView.transform.FindChild(chdSlotName).transform);
                        chdBusyo.name = randomBusyoId.ToString();
                        chdBusyo.transform.localScale = new Vector2(-3, 3);
                        chdBusyo.transform.localPosition = new Vector3(0, 0, 0);

                        //Button
                        chdBusyo.GetComponent<DragHandler>().enabled = false;

                        //Text
                        GameObject chTxtObj = chdBusyo.transform.FindChild("Text").gameObject;
                        Vector2 chTxtScale = chTxtObj.transform.localScale;
                        chTxtScale.x *= -1;
                        chTxtObj.transform.localScale = chTxtScale;
                        Vector2 chTxtPos = chTxtObj.transform.localPosition;
                        chTxtPos.x *= -1;
                        chTxtObj.transform.localPosition = chTxtPos;

                        GameObject chRblObj = chdBusyo.transform.FindChild("Rank").gameObject;
                        Vector2 chRblScale = chRblObj.transform.localScale;
                        chRblScale.x *= -1;
                        chRblObj.transform.localScale = chRblScale;
                        Vector2 chRblPos = chRblObj.transform.localPosition;
                        chRblPos.x *= -1;
                        chRblObj.transform.localPosition = chRblPos;

                    }
                }
                else {
                    //samurai daisyo make
                    busyoListCount = busyoListCount - 1;
                    int mapId = mapList[j];

                    int busyoHp = sts.getHp(35, activeBusyoLv);
                    busyoHp = busyoHp * 100;

                    if (linkNo != 0) {
                        float linkAdjst = (float)linkNo / 10;
                        float adjstHp = busyoHp * linkAdjst;
                        busyoHp = busyoHp + (int)adjstHp;
                    }
                    //ship hp adjust
                    int chldShipId = busyoScript.getShipId(35);
                    if (chldShipId == 1) {
                        busyoHp = busyoHp * 2;
                    }else if (chldShipId == 2) {
                        busyoHp = Mathf.FloorToInt((float)busyoHp * 1.5f);
                    }


                    string busyoName = info.getName(35);
                    string[] texts = new string[] { "YR", "KB" };
                    string busyoType = texts[UnityEngine.Random.Range(0, texts.Length - 1)];

                    int chldHp2 = activeButaiQty * enemyIns.getChildStatus(activeButaiLv, busyoType, linkNo);
                    totalHei = totalHei + busyoHp + chldHp2;

                    string busyoPath = "Prefabs/Player/Unit/Ship";
                    GameObject chdBusyo = Instantiate(Resources.Load(busyoPath)) as GameObject;
                    chdBusyo.name = "35";
                    getShipSprite(chdBusyo, 35);

                    string chdSlotName = "Slot" + mapId;
                    chdBusyo.transform.SetParent(EnemyJinkeiView.transform.FindChild(chdSlotName).transform);
                    chdBusyo.transform.localScale = new Vector2(-3, 3);
                    chdBusyo.transform.localPosition = new Vector3(0, 0, 0);

                    //Button
                    chdBusyo.GetComponent<DragHandler>().enabled = false;

                    //Rabel & Text
                    GameObject chTxtObj = chdBusyo.transform.FindChild("Text").gameObject;
                    Vector2 chTxtScale = chTxtObj.transform.localScale;
                    chTxtScale.x *= -1;
                    chTxtObj.transform.localScale = chTxtScale;
                    Vector2 chTxtPos = chTxtObj.transform.localPosition;
                    chTxtPos.x *= -1;
                    chTxtObj.transform.localPosition = chTxtPos;

                    GameObject chRblObj = chdBusyo.transform.FindChild("Rank").gameObject;
                    Vector2 chRblScale = chRblObj.transform.localScale;
                    chRblScale.x *= -1;
                    chRblObj.transform.localScale = chRblScale;
                    Vector2 chRblPos = chRblObj.transform.localPosition;
                    chRblPos.x *= -1;
                    chRblObj.transform.localPosition = chRblPos;

                }
            }
        }
        return totalHei;
    }



    public int getWeatherId() {

        //get season
        string yearSeason = PlayerPrefs.GetString("yearSeason");
        char[] delimiterChars = { ',' };
        string[] yearSeasonList = yearSeason.Split(delimiterChars);
        int nowSeason = int.Parse(yearSeasonList[1]);

        int weatherId = 0; //1:normal, 2:rain, 3:snow

        if (nowSeason == 1 || nowSeason == 3) {
            //Spring & Fall
            float percent = UnityEngine.Random.value;
            percent = percent * 100;

            if (percent <= 70) {
                weatherId = 1;
            }
            else if (70 < percent && percent <= 90) {
                weatherId = 2;
            }
            else if (90 < percent && percent <= 100) {
                weatherId = 3;
            }
        }
        else if (nowSeason == 2) {
            //Summer
            float percent = UnityEngine.Random.value;
            percent = percent * 100;

            if (percent <= 60) {
                weatherId = 1;
            }
            else if (60 < percent && percent <= 100) {
                weatherId = 2;
            }

        }
        else if (nowSeason == 4) {
            //Winter

            //Check snow area or not
            int activeKuniId = PlayerPrefs.GetInt("activeKuniId");
            KuniInfo kuni = new KuniInfo();
            bool isSnowFlg = kuni.getKuniIsSnowFlg(activeKuniId);

            if (isSnowFlg) {
                weatherId = 3;
            }
            else {
                float percent = UnityEngine.Random.value;
                percent = percent * 100;

                if (percent <= 50) {
                    weatherId = 1;
                }
                else if (50 < percent && percent <= 70) {
                    weatherId = 2;
                }
                else if (70 < percent && percent <= 100) {
                    weatherId = 3;
                }
            }
        }


        //Set Object by wether Id
        if (weatherId == 2) {
            //rain
            string path = "Prefabs/PreKassen/particle/PreRain";
            GameObject particle = Instantiate(Resources.Load(path)) as GameObject;
            particle.transform.SetParent(GameObject.Find("Panel").transform);
            particle.transform.localScale = new Vector3(1, 1, 1);
            particle.transform.localPosition = new Vector3(0, 360, 0);
        }
        else if (weatherId == 3) {
            //snow
            string path = "Prefabs/PreKassen/particle/PreSnow";
            GameObject particle = Instantiate(Resources.Load(path)) as GameObject;
            particle.transform.SetParent(GameObject.Find("Panel").transform);
            particle.transform.localScale = new Vector3(1, 1, 1);
            particle.transform.localPosition = new Vector3(0, 360, 0);
        }

        return weatherId;
    }

    public int getShipSprite(GameObject shipObj, int busyoId) {
        BusyoInfoGet busyoScript = new BusyoInfoGet();
        int shipId = busyoScript.getShipId(busyoId);

        string imagePath = "Prefabs/Kaisen/Ship/" + shipId.ToString();
        shipObj.GetComponent<Image>().sprite =
            Resources.Load(imagePath, typeof(Sprite)) as Sprite;

        if(shipObj.GetComponent<Senryoku>()) {
            shipObj.GetComponent<Senryoku>().shipId = shipId;
        }
        return shipId;
    }

    public void prekassenPlayerJinkei(int jinkeiId, int weatherId, bool isAttackedFlg, bool onlyPlayerFlg) {
        /*Plyaer Jinkei*/

        //reset disabled slot
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
            obs.GetComponent<Image>().enabled = true;
            if (obs.transform.IsChildOf(obs.transform)) {

                foreach (Transform n in obs.transform) {
                    GameObject.Destroy(n.gameObject);
                }
            }
        }
        jinkei = jinkeiId;
        jinkeiBusyo_list = new List<string>();

        startKassen2 startScript = GameObject.Find("StartBtn").GetComponent<startKassen2>();
        startScript.myJinkei = jinkei;
        startScript.weatherId = weatherId;
        startScript.isAttackedFlg = isAttackedFlg;


        //*************My Roujyou Preparation Start*************
        if (isAttackedFlg) {
            GameObject PlayerShiroTorideView = GameObject.Find("PlayerShiroTorideView").gameObject;

            //Random Slot of Toride
            List<string> targetTorideList = new List<string>();
            if (jinkei == 1) {
                List<string> tmp = new List<string>() { "Slot1", "Slot2", "Slot7", "Slot8", "Slot11", "Slot13", "Slot14", "Slot17", "Slot18", "Slot21", "Slot22" };
                targetTorideList.AddRange(tmp);
            }
            else if (jinkei == 2) {
                List<string> tmp = new List<string>() { "Slot3", "Slot4", "Slot5", "Slot7", "Slot8", "Slot11", "Slot17", "Slot18", "Slot23", "Slot24", "Slot25" };
                targetTorideList.AddRange(tmp);
            }
            else if (jinkei == 3) {
                List<string> tmp = new List<string>() { "Slot3", "Slot7", "Slot8", "Slot9", "Slot11", "Slot14", "Slot15", "Slot16", "Slot20", "Slot21", "Slot25" };
                targetTorideList.AddRange(tmp);
            }
            else if (jinkei == 4) {
                List<string> tmp = new List<string>() { "Slot1", "Slot2", "Slot7", "Slot8", "Slot13", "Slot14", "Slot18", "Slot19", "Slot20", "Slot24", "Slot25" };
                targetTorideList.AddRange(tmp);
            }
        }

        BusyoInfoGet busyoScript = new BusyoInfoGet();
        if (jinkei == 1) {
            int soudaisyo = PlayerPrefs.GetInt("soudaisyo1");

            //Clear Previous Unit
            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
                //Enable 1,2,7,8,11,12,13,14,17,18,21,22
                if (obs.name == "Slot1" || obs.name == "Slot2" || obs.name == "Slot7" || obs.name == "Slot8" ||
                    obs.name == "Slot11" || obs.name == "Slot12" || obs.name == "Slot13" || obs.name == "Slot14" ||
                    obs.name == "Slot17" || obs.name == "Slot18" || obs.name == "Slot21" || obs.name == "Slot22") {

                    obs.GetComponent<Image>().enabled = true;
                    string mapId = "1map" + obs.name.Substring(4);
                    if (PlayerPrefs.HasKey(mapId)) {
                        int busyoId = PlayerPrefs.GetInt(mapId);
                        jinkeiBusyo_list.Add(busyoId.ToString());

                        //Instantiate
                        string path = "Prefabs/Player/Unit/Ship";
                        GameObject chldBusyo = Instantiate(Resources.Load(path)) as GameObject;
                        chldBusyo.name = busyoId.ToString();
                        chldBusyo.transform.SetParent(obs.transform);
                        chldBusyo.name = busyoId.ToString();
                        chldBusyo.transform.localScale = new Vector2(3, 3);
                        chldBusyo.AddComponent<Senryoku>();
                        chldBusyo.transform.localPosition = new Vector3(0, 0, 0);
                        getShipSprite(chldBusyo, busyoId);
                        chldBusyo.GetComponent<Senryoku>().GetPlayerSenryoku(chldBusyo.name);

                        //Button
                        chldBusyo.AddComponent<Button>();
                        chldBusyo.AddComponent<Soudaisyo>();
                        chldBusyo.GetComponent<Button>().onClick.AddListener(chldBusyo.GetComponent<Soudaisyo>().OnClick);

                        //soudaisyo
                        if (soudaisyo == int.Parse(chldBusyo.name)) {
                            chldBusyo.GetComponent<Soudaisyo>().OnClick();
                        }

                        //Add Kamon
                        string KamonPath = "Prefabs/Jinkei/Kamon";
                        GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
                        kamon.transform.SetParent(chldBusyo.transform);
                        kamon.transform.localScale = new Vector2(0.1f, 0.1f);
                        kamon.transform.localPosition = new Vector2(-15, -12);
                        int daimyoId = busyoScript.getDaimyoId(int.Parse(chldBusyo.name));
                        if (daimyoId == 0)
                        {
                            daimyoId = busyoScript.getDaimyoHst(int.Parse(chldBusyo.name));
                        }
                        string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
                        kamon.GetComponent<Image>().sprite =
                            Resources.Load(imagePath, typeof(Sprite)) as Sprite;
                    }

                    //Disable 3,4,5,6,9,15,16,19,20,23,24,25
                }
                else {
                    obs.GetComponent<Image>().enabled = false;

                    if (obs.transform.IsChildOf(obs.transform)) {

                        foreach (Transform n in obs.transform) {
                            GameObject.Destroy(n.gameObject);
                        }

                    }
                }
            }


            //鶴翼
        }
        else if (jinkei == 2) {
            int soudaisyo = PlayerPrefs.GetInt("soudaisyo2");

            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
                //Enable 3,4,5,7,8,11,12,17,18,23,24,25
                if (obs.name == "Slot3" || obs.name == "Slot4" || obs.name == "Slot5" || obs.name == "Slot7" ||
                   obs.name == "Slot8" || obs.name == "Slot11" || obs.name == "Slot12" || obs.name == "Slot17" ||
                   obs.name == "Slot18" || obs.name == "Slot23" || obs.name == "Slot24" || obs.name == "Slot25") {


                    obs.GetComponent<Image>().enabled = true;
                    string mapId = "2map" + obs.name.Substring(4);
                    if (PlayerPrefs.HasKey(mapId)) {
                        int busyoId = PlayerPrefs.GetInt(mapId);
                        jinkeiBusyo_list.Add(busyoId.ToString());

                        //Instantiate
                        string path = "Prefabs/Player/Unit/Ship";
                        GameObject chldBusyo = Instantiate(Resources.Load(path)) as GameObject;
                        chldBusyo.name = busyoId.ToString();
                        chldBusyo.transform.SetParent(obs.transform);
                        chldBusyo.name = busyoId.ToString();
                        chldBusyo.transform.localScale = new Vector2(3, 3);
                        chldBusyo.AddComponent<Senryoku>();
                        chldBusyo.transform.localPosition = new Vector3(0, 0, 0);
                        getShipSprite(chldBusyo, busyoId);
                        chldBusyo.GetComponent<Senryoku>().GetPlayerSenryoku(chldBusyo.name);

                        //Button
                        chldBusyo.AddComponent<Button>();
                        chldBusyo.AddComponent<Soudaisyo>();
                        chldBusyo.GetComponent<Button>().onClick.AddListener(chldBusyo.GetComponent<Soudaisyo>().OnClick);

                        //soudaisyo
                        if (soudaisyo == int.Parse(chldBusyo.name)) {
                            chldBusyo.GetComponent<Soudaisyo>().OnClick();
                        }

                        //Add Kamon
                        string KamonPath = "Prefabs/Jinkei/Kamon";
                        GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
                        kamon.transform.SetParent(chldBusyo.transform);
                        kamon.transform.localScale = new Vector2(0.1f, 0.1f);
                        kamon.transform.localPosition = new Vector2(-15, -12);
                        int daimyoId = busyoScript.getDaimyoId(int.Parse(chldBusyo.name));
                        if (daimyoId == 0)
                        {
                            daimyoId = busyoScript.getDaimyoHst(int.Parse(chldBusyo.name));
                        }
                        string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
                        kamon.GetComponent<Image>().sprite =
                            Resources.Load(imagePath, typeof(Sprite)) as Sprite;
                    }

                    //Disable 1,2,6,9,10,13,14,15,16,19,20,21
                }
                else {
                    obs.GetComponent<Image>().enabled = false;

                    if (obs.transform.IsChildOf(obs.transform)) {

                        foreach (Transform n in obs.transform) {
                            GameObject.Destroy(n.gameObject);
                        }
                    }
                }
            }

        }
        else if (jinkei == 3) {
            int soudaisyo = PlayerPrefs.GetInt("soudaisyo3");

            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
                //Enable 3,7,8,9,11,12,14,15,16,20,21,25
                if (obs.name == "Slot3" || obs.name == "Slot7" || obs.name == "Slot8" || obs.name == "Slot9" ||
                   obs.name == "Slot11" || obs.name == "Slot12" || obs.name == "Slot14" || obs.name == "Slot15" ||
                   obs.name == "Slot16" || obs.name == "Slot20" || obs.name == "Slot21" || obs.name == "Slot25") {

                    obs.GetComponent<Image>().enabled = true;
                    string mapId = "3map" + obs.name.Substring(4);
                    if (PlayerPrefs.HasKey(mapId)) {
                        int busyoId = PlayerPrefs.GetInt(mapId);
                        jinkeiBusyo_list.Add(busyoId.ToString());

                        //Instantiate
                        string path = "Prefabs/Player/Unit/Ship";
                        GameObject chldBusyo = Instantiate(Resources.Load(path)) as GameObject;
                        chldBusyo.name = busyoId.ToString();
                        chldBusyo.transform.SetParent(obs.transform);
                        chldBusyo.name = busyoId.ToString();
                        chldBusyo.transform.localScale = new Vector2(3, 3);
                        chldBusyo.AddComponent<Senryoku>();
                        chldBusyo.transform.localPosition = new Vector3(0, 0, 0);
                        getShipSprite(chldBusyo, busyoId);
                        chldBusyo.GetComponent<Senryoku>().GetPlayerSenryoku(chldBusyo.name);

                        //Button
                        chldBusyo.AddComponent<Button>();
                        chldBusyo.AddComponent<Soudaisyo>();
                        chldBusyo.GetComponent<Button>().onClick.AddListener(chldBusyo.GetComponent<Soudaisyo>().OnClick);

                        //soudaisyo
                        if (soudaisyo == int.Parse(chldBusyo.name)) {
                            chldBusyo.GetComponent<Soudaisyo>().OnClick();
                        }

                        //Add Kamon
                        string KamonPath = "Prefabs/Jinkei/Kamon";
                        GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
                        kamon.transform.SetParent(chldBusyo.transform);
                        kamon.transform.localScale = new Vector2(0.1f, 0.1f);
                        kamon.transform.localPosition = new Vector2(-15, -12);
                        int daimyoId = busyoScript.getDaimyoId(int.Parse(chldBusyo.name));
                        if (daimyoId == 0)
                        {
                            daimyoId = busyoScript.getDaimyoHst(int.Parse(chldBusyo.name));
                        }
                        string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
                        kamon.GetComponent<Image>().sprite =
                            Resources.Load(imagePath, typeof(Sprite)) as Sprite;
                    }

                    //Disable 1,2,4,5,6,10,13,17,18,19,22,23,24
                }
                else {
                    obs.GetComponent<Image>().enabled = false;
                    if (obs.transform.IsChildOf(obs.transform)) {

                        foreach (Transform n in obs.transform) {
                            GameObject.Destroy(n.gameObject);
                        }
                    }
                }
            }

        }
        else if (jinkei == 4) {
            int soudaisyo = PlayerPrefs.GetInt("soudaisyo4");

            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
                //Enable 1,2,7,8,12,13,14,18,19,20,24,25
                if (obs.name == "Slot1" || obs.name == "Slot2" || obs.name == "Slot7" || obs.name == "Slot8" ||
                   obs.name == "Slot12" || obs.name == "Slot13" || obs.name == "Slot14" || obs.name == "Slot18" ||
                   obs.name == "Slot19" || obs.name == "Slot20" || obs.name == "Slot24" || obs.name == "Slot25") {

                    obs.GetComponent<Image>().enabled = true;
                    string mapId = "4map" + obs.name.Substring(4);
                    if (PlayerPrefs.HasKey(mapId)) {
                        int busyoId = PlayerPrefs.GetInt(mapId);
                        jinkeiBusyo_list.Add(busyoId.ToString());

                        //Instantiate
                        string path = "Prefabs/Player/Unit/Ship";
                        GameObject chldBusyo = Instantiate(Resources.Load(path)) as GameObject;
                        chldBusyo.name = busyoId.ToString();
                        chldBusyo.transform.SetParent(obs.transform);
                        chldBusyo.name = busyoId.ToString();
                        chldBusyo.transform.localScale = new Vector2(3, 3);
                        chldBusyo.AddComponent<Senryoku>();
                        chldBusyo.transform.localPosition = new Vector3(0, 0, 0);
                        getShipSprite(chldBusyo, busyoId);
                        chldBusyo.GetComponent<Senryoku>().GetPlayerSenryoku(chldBusyo.name);

                        //Button
                        chldBusyo.AddComponent<Button>();
                        chldBusyo.AddComponent<Soudaisyo>();
                        chldBusyo.GetComponent<Button>().onClick.AddListener(chldBusyo.GetComponent<Soudaisyo>().OnClick);

                        //soudaisyo
                        if (soudaisyo == int.Parse(chldBusyo.name)) {
                            chldBusyo.GetComponent<Soudaisyo>().OnClick();
                        }

                        //Add Kamon
                        string KamonPath = "Prefabs/Jinkei/Kamon";
                        GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
                        kamon.transform.SetParent(chldBusyo.transform);
                        kamon.transform.localScale = new Vector2(0.1f, 0.1f);
                        kamon.transform.localPosition = new Vector2(-15, -12);
                        int daimyoId = busyoScript.getDaimyoId(int.Parse(chldBusyo.name));
                        if (daimyoId == 0)
                        {
                            daimyoId = busyoScript.getDaimyoHst(int.Parse(chldBusyo.name));
                        }
                        string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
                        kamon.GetComponent<Image>().sprite =
                            Resources.Load(imagePath, typeof(Sprite)) as Sprite;
                    }

                    //Disable 3,4,5,6,9,10,11,15,16,17,21,22,23
                }
                else {
                    obs.GetComponent<Image>().enabled = false;
                    if (obs.transform.IsChildOf(obs.transform)) {

                        foreach (Transform n in obs.transform) {
                            GameObject.Destroy(n.gameObject);
                        }
                    }
                }
            }
        }

        //Player Senryoku
        int totalHp = 0;
        GameObject playerImage = GameObject.Find("PlayerJinkeiView").gameObject;
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
            if (obs.transform.childCount > 0) {
                foreach (Transform busyo in obs.transform) {
                    totalHp = totalHp + busyo.GetComponent<Senryoku>().totalHp;
                }
            }
        }

        PlayerPrefs.SetInt("jinkeiHeiryoku", totalHp);
        Text playerHeiText = GameObject.Find("PlayerHei").transform.FindChild("HeiValue").GetComponent<Text>();
        playerHeiText.text = totalHp.ToString();

        JinkeiPowerEffection powerEffection = new JinkeiPowerEffection();
        powerEffection.UpdateSenryoku();

        GameObject.Find("BusyoScrollMenu").transform.FindChild("ScrollView").transform.FindChild("Content").GetComponent<PrepBusyoScrollMenu>().jinkeiBusyo_list = jinkeiBusyo_list;
        jinkeiLimit = PlayerPrefs.GetInt("jinkeiLimit");
        int addLimit = 0;
        if (PlayerPrefs.GetBool("addJinkei1"))
        {
            addLimit = 1;
        }
        if (PlayerPrefs.GetBool("addJinkei2"))
        {
            addLimit = addLimit + 1;
        }
        if (PlayerPrefs.GetBool("addJinkei3"))
        {
            addLimit = addLimit + 1;
        }
        if (PlayerPrefs.GetBool("addJinkei4"))
        {
            addLimit = addLimit + 1;
        }
        jinkeiLimit = jinkeiLimit + addLimit;


        busyoCurrentQty = jinkeiBusyo_list.Count;

        if (!onlyPlayerFlg) {
            /*------------*/
            /*Enemy Jinkei*/
            /*------------*/
            int powerType = PlayerPrefs.GetInt("activePowerType", 0);
            int linkNo = PlayerPrefs.GetInt("activeLink", 0);

            List<int> jinkeiList = new List<int>() { 1, 2, 3, 4 };
            int enemyJinkei = UnityEngine.Random.Range(1, jinkeiList.Count + 1);
            startScript.enemyJinkei = enemyJinkei;

            //Back Kamon
            int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
            string playerImagePath = "Prefabs/Kamon/" + myDaimyo.ToString();
            playerImage.GetComponent<Image>().sprite =
                Resources.Load(playerImagePath, typeof(Sprite)) as Sprite;

            int activeDaimyoId = PlayerPrefs.GetInt("activeDaimyoId");
            string enemyImagePath = "Prefabs/Kamon/" + activeDaimyoId.ToString();
            GameObject enemyImage = GameObject.Find("EnemyJinkeiView").gameObject;
            enemyImage.GetComponent<Image>().sprite =
                Resources.Load(enemyImagePath, typeof(Sprite)) as Sprite;
            startScript.activeDaimyoId = activeDaimyoId;

            //Set Busyo Jinkei
            List<int> mapList = new List<int>();
            if (powerType == 1) {
                //busyo + mob
                mapList = enemyJinkeiMaker(enemyJinkei);
                enemyHei = powerType1(mapList, getTaisyoMapId(enemyJinkei), linkNo, activeDaimyoId);
            }
            else if (powerType == 2) {
                //busyo only
                mapList = enemyJinkeiMaker(enemyJinkei);
                enemyHei = powerType2(mapList, getTaisyoMapId(enemyJinkei), linkNo, activeDaimyoId);
            }
            else if (powerType == 3 || powerType == 0) {
                //daimyo + busyo
                mapList = enemyJinkeiMaker(enemyJinkei);
                enemyHei = powerType3(mapList, getTaisyoMapId(enemyJinkei), linkNo, activeDaimyoId);
            }

            Text enemyHeiText = GameObject.Find("EnemyHei").transform.FindChild("HeiValue").GetComponent<Text>();
            enemyHeiText.text = enemyHei.ToString();

            startScript.enemySoudaisyo = tempEnemySoudaisyo;


            startScript.myHei = totalHp;
            startScript.enemyHei = enemyHei;
        }
    }

    public void changeFormButtonColor(int jinkeiId) {
        Color pushedTabColor = new Color(118f / 255f, 103f / 255f, 16f / 255f, 255f / 255f);
        Color pushedTextColor = new Color(190f / 255f, 190f / 255f, 80f / 255f, 255f / 255f);
        GameObject JinkeiButton = GameObject.Find("JinkeiButton").gameObject;

        if (jinkeiId == 1) {
            JinkeiButton.transform.FindChild("Gyorin").GetComponent<Image>().color = pushedTabColor;
            JinkeiButton.transform.FindChild("Gyorin").transform.FindChild("Text").GetComponent<Text>().color = pushedTextColor;
        }
        else if (jinkeiId == 2) {
            JinkeiButton.transform.FindChild("Kakuyoku").GetComponent<Image>().color = pushedTabColor;
            JinkeiButton.transform.FindChild("Kakuyoku").transform.FindChild("Text").GetComponent<Text>().color = pushedTextColor;
        }
        else if (jinkeiId == 3) {
            JinkeiButton.transform.FindChild("Engetsu").GetComponent<Image>().color = pushedTabColor;
            JinkeiButton.transform.FindChild("Engetsu").transform.FindChild("Text").GetComponent<Text>().color = pushedTextColor;
        }
        else if (jinkeiId == 4) {
            JinkeiButton.transform.FindChild("Gankou").GetComponent<Image>().color = pushedTabColor;
            JinkeiButton.transform.FindChild("Gankou").transform.FindChild("Text").GetComponent<Text>().color = pushedTextColor;
        }



    }
}
