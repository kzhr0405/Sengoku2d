using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class EnemyEventHandler : MonoBehaviour {

    public List<int> usedBusyoList;
    public AudioSource[] audioSources;

    public void doEnemyEvent(GameObject kuniMap, GameObject closeObj, int kuniId, int daimyoId, int activeBusyoQty, int activeBusyoLv, int activeButaiQty, int activeButaiLv){

        List<string> clearedStageList = new List<string>();
        int senarioId = PlayerPrefs.GetInt("senarioId");
        string temp = "kuni" + kuniId.ToString();
        string clearedStageString = PlayerPrefs.GetString(temp);
        if (clearedStageString != null && clearedStageString != ""){
            char[] delimiterChars = { ',' };
            clearedStageList = new List<string>(clearedStageString.Split(delimiterChars));
        }
        if (clearedStageList.Count != 0) {

            //Show Enemy Attack
            showAttack(kuniMap, kuniId, activeBusyoLv);
            

            //Create Enemy Attack
            Entity_stageLink_mst stageLinkMst = Resources.Load("Data/stageLink_mst") as Entity_stageLink_mst;
            List<string> stageLinkList = new List<string>();
            for (int i = 0; i < stageLinkMst.param.Count; i++){
                int tempKuniId = stageLinkMst.param[i].kuniId;
                if (tempKuniId == kuniId){
                    stageLinkList.Add(stageLinkMst.param[i].Link);
                }
            }

            char[] delimiterChars2 = { '-' };
            int toStageIdFirst = 0;

            foreach (string stageLink in stageLinkList){
                int fromStageId = 0;
                int toStageId = 0;
                bool attackFlg = false;

                List<string> linkList = new List<string>(stageLink.Split(delimiterChars2));
                string stage1Name = linkList[0];
                string stage2Name = linkList[1];

                if (clearedStageList.Contains(stage1Name)) {
                    if (!clearedStageList.Contains(stage2Name)) {
                        //attack from stage2 >> stage1
                        attackFlg = checkAttack();
                        fromStageId = int.Parse(stage2Name);
                        toStageId = int.Parse(stage1Name);
                    }
                } else if (clearedStageList.Contains(stage2Name)) {
                    if (!clearedStageList.Contains(stage1Name)){
                        //attack from stage1 >> stage2
                        attackFlg = checkAttack();
                        fromStageId = int.Parse(stage1Name);
                        toStageId = int.Parse(stage2Name);
                    }
                }
                
                if (attackFlg){
                    if(toStageIdFirst ==0 || toStageIdFirst == toStageId) {
                        toStageIdFirst = toStageId;

                        //from duplication check
                        bool fromDupFlg = false;
                        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("StageGunzei")) {
                            if(fromStageId == obs.GetComponent<TabStageGunzei>().fromStageId) {
                                fromDupFlg = true;
                            }
                        }
                        if(!fromDupFlg) {
                            createAttack(kuniMap, kuniId,fromStageId, toStageId, daimyoId, activeBusyoQty, activeBusyoLv, activeButaiQty, activeButaiLv, senarioId);
                        }
                    }
                }
            }
        }
    }


    public bool checkAttack(){
        bool attackFlg = false;
        float percent = UnityEngine.Random.value;
        percent = percent * 100;

        if (percent <= 20){
            attackFlg = true;
        }
        
        return attackFlg;
    }

    
    public void showAttack(GameObject kuniMap, int kuniId, int activeBusyoLv) {


        char[] delimiterChars = { ',' };
         char[] delimiterChars2 = { ':' };
        for (int i=1;i<11;i++){

            string dataLabel = "attack" + kuniId + "-" + i;
            if (PlayerPrefs.HasKey(dataLabel)){
                string gunzeiString = PlayerPrefs.GetString(dataLabel);
                List<string> gunzeiValueList = new List<string>();
                gunzeiValueList = new List<string>(gunzeiString.Split(delimiterChars));
                createGunzei(gunzeiValueList[2] + "," + gunzeiValueList[3], kuniMap, kuniId, int.Parse(gunzeiValueList[0]), int.Parse(gunzeiValueList[1]), int.Parse(gunzeiValueList[4]), activeBusyoLv,false);

                List<string> tmpUsedBusyoList = new List<string>(gunzeiValueList[2].Split(delimiterChars2));
                List<int> tmpUsedBusyoList2 = tmpUsedBusyoList.ConvertAll(x => int.Parse(x));
                usedBusyoList.AddRange(tmpUsedBusyoList2);
            }
        }
    }



    public void createAttack(GameObject kuniMap, int kuniId, int fromStageId, int toStageId, int daimyoId, int activeBusyoQty, int activeBusyoLv, int activeButaiQty, int activeButaiLv, int senarioId){

        audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[7].Play();

        //Check Src Shiro Type
        string stageName = "stage" + fromStageId.ToString();
        int powerType = kuniMap.transform.Find(stageName).GetComponent<ShowStageDtl>().powerType;

        //Choose Busyo
        string busyoListString = getBusyoList(powerType, daimyoId, activeBusyoQty, activeBusyoLv, activeButaiQty, activeButaiLv, senarioId);
        
        if(busyoListString!="") {
            createGunzei(busyoListString, kuniMap, kuniId, fromStageId, toStageId, daimyoId, activeBusyoLv,true);

            //Data Set
            string dataName = "attack" + kuniId + "-" + fromStageId;
            string dataValue = fromStageId.ToString() + "," + toStageId.ToString() + "," + busyoListString + "," + daimyoId;
            PlayerPrefs.SetString(dataName, dataValue);
            PlayerPrefs.Flush();
        }

    }

    public void createGunzei(string busyoListString, GameObject kuniMap, int kuniId, int fromStageId, int toStageId, int daimyoId, int activeBusyoLv, bool newFlg){

        
        char[] delimiterChars = { ',' };
        char[] delimiterChars2 = { ':' };
        List<string> itemList = new List<string>(busyoListString.Split(delimiterChars));
        string busyoListString2 = itemList[0];
        List<string> busyoList = new List<string>(busyoListString2.Split(delimiterChars2));
        Message Message = new Message();
        int totalHei = int.Parse(itemList[1]);
        int taisyoBusyoId = int.Parse(busyoList[0]);

        //Visualize Gunzei
        string gunzeiPath = "Prefabs/Map/stage/stageGunzei";
        GameObject gunzeiObj = Instantiate(Resources.Load(gunzeiPath)) as GameObject;
        gunzeiObj.transform.SetParent(kuniMap.transform);

        string fromStageName = "stage" + fromStageId.ToString();
        string toStageName = "stage" + toStageId.ToString();
        float fromLocationX = kuniMap.transform.Find(fromStageName).transform.localPosition.x;
        float fromLocationY = kuniMap.transform.Find(fromStageName).transform.localPosition.y;
        float toLocationX = kuniMap.transform.Find(toStageName).transform.localPosition.x;
        float toLocationY = kuniMap.transform.Find(toStageName).transform.localPosition.y;

        string commentPath = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            commentPath = "Prefabs/Map/stage/spriteEventCommentEng";
        }else if(langId==3) {
            commentPath = "Prefabs/Map/stage/spriteEventCommentSChn";
        }else {
            commentPath = "Prefabs/Map/stage/spriteEventComment";
        }
        GameObject commentObj = Instantiate(Resources.Load(commentPath)) as GameObject;
        commentObj.transform.SetParent(gunzeiObj.transform);
        commentObj.transform.localPosition = new Vector2(0,10);
        if(!newFlg) {
            int rdmId = UnityEngine.Random.Range(1, 4);
            string rdmTxt = "";            
            if (rdmId == 1) {
                rdmTxt = Message.getMessage(243,langId);
            }else if (rdmId == 2) {
                rdmTxt = Message.getMessage(244, langId);
            }else if (rdmId == 3) {
                rdmTxt = Message.getMessage(245,langId);
            }
            
            commentObj.transform.Find("txt").GetComponent<TextMesh>().text = rdmTxt;
        }

        if (fromLocationX > toLocationX){
            gunzeiObj.transform.localScale = new Vector2(-1.0f, 1.0f);
            commentObj.transform.localScale = new Vector2(-3, 5);
        }else{
            gunzeiObj.transform.localScale = new Vector2(1.0f, 1.0f);
            commentObj.transform.localScale = new Vector2(3, 5);
        }

        float middleLocationX = (fromLocationX + toLocationX) / 2;
        float middleLocationY = (fromLocationY + toLocationY) / 2;
        gunzeiObj.transform.localPosition = new Vector2(middleLocationX, middleLocationY);


        string taisyoBusyoPath = "Prefabs/Player/Sprite/unit" + taisyoBusyoId.ToString();
        gunzeiObj.transform.Find("taisyo").GetComponent<SpriteRenderer>().sprite =
            Resources.Load(taisyoBusyoPath, typeof(Sprite)) as Sprite;

        //Script
        gunzeiObj.GetComponent<TabStageGunzei>().fromStageId = fromStageId;
        gunzeiObj.GetComponent<TabStageGunzei>().toStageId = toStageId;
        gunzeiObj.GetComponent<TabStageGunzei>().taisyoBusyoId = taisyoBusyoId;
        if (busyoList.Count > 1){
            string busyoString = "";
            for (int i=1; i<busyoList.Count; i++) {
                if(i==1) {
                    busyoString = busyoList[i];
                }else {
                    busyoString = busyoString + ":" + busyoList[i];
                }
            }
            gunzeiObj.GetComponent<TabStageGunzei>().busyoString = busyoString;
        }
        string key = kuniId + "-" + fromStageId;
        gunzeiObj.GetComponent<TabStageGunzei>().totalHei = totalHei;
        gunzeiObj.GetComponent<TabStageGunzei>().key = key;


        //Create Katana Animation
        //Duplication Check
        string stageToName = "stage" + toStageId.ToString();
        GameObject stageToObj = kuniMap.transform.Find(stageToName).gameObject;
        GameObject clearedObj = stageToObj.transform.Find("cleared").gameObject;

        int atk = 0;
        int hp = 0;
        if (clearedObj.transform.childCount == 0){
            //1st time

            string katanaPath = "Prefabs/Map/EnemyBattle/enemyKatana";
            GameObject katanaObj = Instantiate(Resources.Load(katanaPath)) as GameObject;
            katanaObj.transform.SetParent(clearedObj.transform);
            katanaObj.transform.localScale = new Vector2(1.8f, 1.8f);
            katanaObj.transform.localPosition = new Vector2(0, 0);
            katanaObj.name = "enemyKatana";

            int toPowerType = stageToObj.GetComponent<ShowStageDtl>().powerType;
            if (toPowerType == 1){
                hp = 60;
            }else if (toPowerType == 2){
                hp = 100;
            }else if (toPowerType == 3){
                hp = 200;
            }
            int myHei = PlayerPrefs.GetInt("jinkeiHeiryoku");
            if (totalHei > myHei){
                int tmpCalc = (totalHei - myHei) / 1000;
                if (tmpCalc > 1){
                    atk = tmpCalc;
                }else{
                    atk = 1;
                }
            }else{
                atk = 1;
            }
            
            katanaObj.GetComponent<ShiroAttack>().keyList.Add(key);
            katanaObj.GetComponent<ShiroAttack>().clearedObj = clearedObj;
            katanaObj.GetComponent<ShiroAttack>().kuniId = kuniId;
            katanaObj.GetComponent<ShiroAttack>().fromStageId = fromStageId;
            katanaObj.GetComponent<ShiroAttack>().toStageId = toStageId;
            katanaObj.GetComponent<ShiroAttack>().myHei = myHei;
            katanaObj.GetComponent<ShiroAttack>().enemyHei = totalHei;
            katanaObj.GetComponent<ShiroAttack>().time = (float)hp / atk;
            katanaObj.GetComponent<ShiroAttack>().enemyDaimyoId = daimyoId;
        }
        else{
            //Duplication
            ShiroAttack shiroScript = clearedObj.transform.Find("enemyKatana").GetComponent<ShiroAttack>();
            int myHei = shiroScript.myHei;
            int preTotalHei = shiroScript.enemyHei;
            totalHei = totalHei + preTotalHei;
            if (totalHei > myHei){
                int tmpCalc = (totalHei - myHei) / 1000;
                if (tmpCalc > 1){
                    atk = tmpCalc;
                }else{
                    atk = 1;
                }
            }else{
                atk = 1;
            }
            shiroScript.keyList.Add(key);
            shiroScript.enemyHei = totalHei;
            shiroScript.atk = atk;


        }




    }



    public string getBusyoList(int powerType, int activeDaimyoId, int activeBusyoQty, int activeBusyoLv, int activeButaiQty, int activeButaiLv, int senarioId){
        string returnString = ""; //busyoA-busyoB-busyoC...,totalhei

        int totalHei = 0;
        Daimyo Daimyo = new Daimyo();
        int daimyoBusyoId = Daimyo.getDaimyoBusyoId(activeDaimyoId, senarioId);

        /*Busyo Master Setting Start*/
        //Active Busyo List
        List<string> metsubouDaimyoList = new List<string>();
        string metsubouTemp = "metsubou" + activeDaimyoId;
        string metsubouDaimyoString = PlayerPrefs.GetString(metsubouTemp);
        char[] delimiterChars2 = { ',' };
        if (metsubouDaimyoString != null && metsubouDaimyoString != ""){
            if (metsubouDaimyoString.Contains(",")){
                metsubouDaimyoList = new List<string>(metsubouDaimyoString.Split(delimiterChars2));
            }else{
                metsubouDaimyoList = new List<string>(metsubouDaimyoString.Split(delimiterChars2));
            }
        }
        metsubouDaimyoList.Add(activeDaimyoId.ToString());
        
        Entity_busyo_mst busyoMst = Resources.Load("Data/busyo_mst") as Entity_busyo_mst;
        List<int> busyoList = new List<int>();

        for (int i = 0; i < busyoMst.param.Count; i++){
            int busyoId = busyoMst.param[i].id;
            int daimyoId = 0;
            if (senarioId == 1) {
                daimyoId = busyoMst.param[i].daimyoId1;
            }else if (senarioId == 2) {
                daimyoId = busyoMst.param[i].daimyoId2;
            }else if (senarioId == 3) {
                daimyoId = busyoMst.param[i].daimyoId3;
            }else {
                daimyoId = busyoMst.param[i].daimyoId;
            }

            if (metsubouDaimyoList.Contains(daimyoId.ToString())){
                if (busyoId != daimyoBusyoId){
                    busyoList.Add(busyoId);
                }
            }
        }
        //minus
        if (usedBusyoList.Count > 0) { 
            busyoList.RemoveAll(usedBusyoList.Contains);
        }
        
        /*Busyo Master Setting End*/
        /*Random Shuffle*/
        for (int i = 0; i < busyoList.Count; i++){
            int temp = busyoList[i];
            int randomIndex = UnityEngine.Random.Range(0, busyoList.Count);
            busyoList[i] = busyoList[randomIndex];
            busyoList[randomIndex] = temp;
        }


        int min = (activeBusyoQty-1) / 2;
        if(min < 1) {
            min = 1;
        }
        int max = activeBusyoQty-1;
        activeBusyoQty = UnityEngine.Random.Range(min, max);

        for (int i = 0; i < activeBusyoQty; i++) {
            int busyoId = 0;
            if(busyoList.Count>i) {
                if(!usedBusyoList.Contains(busyoList[i])) {
                    busyoId = busyoList[i];
                    usedBusyoList.Add(busyoId);
                }else {
                    busyoId = 35;
                }
            }else {
                busyoId = 35;
            }
            if(i==0) {
                returnString = busyoId.ToString();
            }else { 
                returnString = returnString + ":" + busyoId.ToString();
            }
            totalHei = totalHei + getHei(busyoId, activeBusyoLv, activeButaiQty, activeButaiLv);
        }
        returnString = returnString + "," + totalHei.ToString();



        return returnString;
    }

    public int getHei(int busyoId, int activeBusyoLv, int activeButaiQty, int activeButaiLv){
        int hei = 0;

        StatusGet sts = new StatusGet();
        BusyoInfoGet info = new BusyoInfoGet();
        string TaisyoType = info.getHeisyu(busyoId);
        int hp = sts.getHp(busyoId, activeBusyoLv);
        hp = hp * 100;

        EnemyInstance enemyIns = new EnemyInstance();
        int chldHp = activeButaiQty * enemyIns.getChildStatus(activeButaiLv, TaisyoType, 0);
        hei = hp + chldHp;

        return hei;
    }



}
