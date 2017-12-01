using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class ShiroAttack : MonoBehaviour {

    public List<string> keyList;
    public GameObject clearedObj;
    public int kuniId = 0;
    public int toStageId = 0;
    public int fromStageId = 0;
    public int enemyDaimyoId = 0;

    public int myHei = 0;
    public int enemyHei = 0;
    public int hp = 0;
    public int atk = 0; //per sec
    public bool rakujyoFlg = false;
    public float time = 0;
    public TextMesh timeText;

    public bool oldGunzeiFlg = false;
    public string dataName = "";
    public AudioSource[] audioSources;

    void Start(){
        timeText = transform.Find("time").GetComponent<TextMesh>();
        dataName = "remain" + kuniId + "-" + fromStageId;
        if(PlayerPrefs.HasKey(dataName)){
            time = PlayerPrefs.GetFloat(dataName);
        }
        transform.Find("Button").GetComponent<EnemyAttackPop>().enemyDaimyoId = enemyDaimyoId;
    }

	void Update () {
        //Count down

        if(!rakujyoFlg) {
            time -= Time.deltaTime;            
            if (time > 0.0f){
                //On Play
                PlayerPrefs.SetFloat(dataName, time);
                timeText.text = ((int)time).ToString();
            }else{
                //Enemy Win
                rakujyoFlg = true;
                changeShiroOwner();
            }
        }
    }

    public void changeShiroOwner(){

        audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[7].Play();
        audioSources[6].Play();
        int senarioId = PlayerPrefs.GetInt("senarioId");

        //Change Cleared Data
        string clearedStage = "kuni" + kuniId;
        string clearedStageString = PlayerPrefs.GetString(clearedStage);
        List<string> clearedStageList = new List<string>();
        char[] delimiterChars = { ',' };
        if (clearedStageString != null && clearedStageString != ""){
            clearedStageList = new List<string>(clearedStageString.Split(delimiterChars));
        }
        clearedStageList.Remove(toStageId.ToString());

        string newClearedStage = "";
        for (int i=0; i<clearedStageList.Count;i++){
            if(i==0){
                newClearedStage = clearedStageList[i];
            }else{
                newClearedStage = newClearedStage + "," + clearedStageList[i];
            }
        }
        PlayerPrefs.SetString(clearedStage, newClearedStage);
        
        //Delete Enemy Attack Data
        foreach(string key in keyList){
            string deleteLabel1 = "attack" + key;
            string deleteLabel2 = "remain" + key;
            PlayerPrefs.DeleteKey(deleteLabel1);
            PlayerPrefs.DeleteKey(deleteLabel2);
        }
        PlayerPrefs.Flush();

        //Update
        GameObject kuniMap = GameObject.Find("kuniMap" + kuniId).gameObject;
        UpdateShiroStatus(kuniMap);

        GameObject toStageObj = kuniMap.transform.Find("stage" + toStageId).gameObject;
        Destroy(toStageObj.transform.Find("cleared").gameObject);
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("StageGunzei")) {
            if(obs.GetComponent<TabStageGunzei>().toStageId == toStageId) {
                Destroy(obs);
            }
        }

        //Animation
        string animPath = "Prefabs/Naisei/DestroyAnim";
        GameObject destroyObj = Instantiate(Resources.Load(animPath)) as GameObject;
        destroyObj.transform.SetParent(kuniMap.transform);
        destroyObj.transform.localScale = new Vector2(20, 20);
        destroyObj.transform.localPosition = new Vector2(toStageObj.transform.localPosition.x, toStageObj.transform.localPosition.y + 14);

        KassenEvent kassenEventScript = new KassenEvent();
        GameObject commentObj = kassenEventScript.MakeCommentObj(enemyDaimyoId, kuniId,senarioId);
        Stage stageScript = new Stage();
        int langId = PlayerPrefs.GetInt("langId");
        string stageName = stageScript.getStageName(kuniId, toStageId, langId);
        if (langId == 2) {
            commentObj.transform.Find("SerihuText").GetComponent<Text>().text = "Hahaha, I got " + stageName + " castle！";
        }else {
            commentObj.transform.Find("SerihuText").GetComponent<Text>().text = "ははは、" + stageName + "を盗り返したぞ！";
        }

    }



    public void UpdateShiroStatus(GameObject kuniMap) {

        char[] delimiterChars = { ',' };
        char[] delimiterChars2 = { '-' };
        int myDaimyoId = PlayerPrefs.GetInt("myDaimyo");

        //open shiro
        List<int> closeStageIdList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        string seiryoku = PlayerPrefs.GetString("seiryoku");
        List<string> seiryokuList = new List<string>();
        seiryokuList = new List<string>(seiryoku.Split(delimiterChars));

        //compare linkKuni with mySeiryoku
        List<int> linkKuniList = new List<int>();
        List<int> linkMyKuniList = new List<int>();

        KuniInfo kuniScript = new KuniInfo();
        linkKuniList = kuniScript.getMappingKuni(kuniId);
        for (int i = 0; i < linkKuniList.Count; i++) {
            int linkKuniId = linkKuniList[i];
            if (seiryokuList[linkKuniId - 1] == myDaimyoId.ToString()) {
                linkMyKuniList.Add(linkKuniId);
            }
        }

        //open initial stage
        for (int j = 0; j < linkMyKuniList.Count; j++) {
            int srcKuniId = linkMyKuniList[j];
            string linkStage = kuniScript.getLinkStage(srcKuniId, kuniId);
            linkStage = linkStage.Replace("stage", "");
            List<int> linkStageList = new List<int>(Array.ConvertAll(linkStage.Split(','),
                new Converter<string, int>((s) => { return Convert.ToInt32(s); })));
            closeStageIdList.RemoveAll(linkStageList.Contains);

        }

        //Clear Stage Setting
        string clearedStage = "kuni" + kuniId;
        string clearedStageString = PlayerPrefs.GetString(clearedStage);
        List<string> clearedStageList = new List<string>();
        if (clearedStageString != null && clearedStageString != "") {
            clearedStageList = new List<string>(clearedStageString.Split(delimiterChars));
        }

        //Line Setting
        Entity_stageLink_mst stageLinkMst = Resources.Load("Data/stageLink_mst") as Entity_stageLink_mst;
        List<string> myStageLink = new List<string>();
        for (int i = 0; i < stageLinkMst.param.Count; i++) {
            int tempKuniId = stageLinkMst.param[i].kuniId;
            if (tempKuniId == kuniId) {
                myStageLink.Add(stageLinkMst.param[i].Link);
            }
        }
        List<string> myOriginalStageLink = new List<string>(myStageLink);

        //open cleared kuni & linked stage
        List<int> clearedStageIntList = clearedStageList.ConvertAll(x => int.Parse(x));
        closeStageIdList.RemoveAll(clearedStageIntList.Contains);
        for (int l = 0; l < clearedStageIntList.Count; l++) {
            int srcStageId = clearedStageIntList[l];

            for (int m = 0; m < myOriginalStageLink.Count; m++) {
                List<string> linkList = new List<string>(myOriginalStageLink[m].Split(delimiterChars2));
                int stage1Id = int.Parse(linkList[0]);
                int stage2Id = int.Parse(linkList[1]);
                if (srcStageId == stage1Id) {
                    closeStageIdList.Remove(stage2Id);
                }
                else if (srcStageId == stage2Id) {
                    closeStageIdList.Remove(stage1Id);
                }
            }
        }


        //Active or Disabled
        Color openColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0f / 255f); //White
        Color closeColor = new Color(60f / 255f, 60f / 255f, 60f / 255f, 255f / 255f); //Black
        bool fireflg = false;

        for (int k=1; k<11; k++) {
            string stageName = "stage" + k.ToString();
            GameObject shiroObj = kuniMap.transform.Find(stageName).gameObject;

            if (closeStageIdList.Contains(k)) {
                //Close
                shiroObj.GetComponent<Button>().enabled = false;
                shiroObj.transform.Find("shiroImage").GetComponent<SpriteRenderer>().color = closeColor;
            }else {
                //Open
                //shiroObj.GetComponent<Button>().enabled = true;
                //shiroObj.transform.FindChild("shiroImage").GetComponent<SpriteRenderer>().color = openColor;
                shiroObj.GetComponent<ShowStageDtl>().clearedFlg = false;
                if(!fireflg) {
                    shiroObj.GetComponent<ShowStageDtl>().OnClick();
                    fireflg = true;
                }
            }
        }



    }
    
}
